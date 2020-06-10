using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using DotSpatial.Data;
using DotSpatial.Projections;
using GeoUtility.GeoSystem;
using GeoUtility.GeoSystem.Base;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using Ionic.Zip;
using log4net;
using MissionPlanner.Controls;
using MissionPlanner.Controls.Waypoints;
using MissionPlanner.Maps;
using MissionPlanner.Properties;
using MissionPlanner.Utilities;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using SharpKml.Base;
using SharpKml.Dom;
using Feature = SharpKml.Dom.Feature;
using ILog = log4net.ILog;
using Placemark = SharpKml.Dom.Placemark;
using Point = System.Drawing.Point;
using System.Text.RegularExpressions;
using MissionPlanner.Poly_split;

namespace MissionPlanner.GCSViews
{
    public partial class FlightPlanner : MyUserControl, IDeactivate, IActivate
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        int selectedrow;
        int avoidPntsselectedrow;

        public bool quickadd;
        bool isonline = true;
        bool sethome;

        public bool polygongridmode;
        public bool polygonsplitmode;
        public bool wpinsertmode;

        Hashtable param = new Hashtable();
        bool splinemode;
        altmode currentaltmode = altmode.Relative;

        // AB ZhaoYJ@2017-08-02 for import abs-alt KML/KMZ
        bool abs_alt_kml = false;

        string wp_alt = "5";

        public string work_mode = "farmming";

        bool grid;

        public static FlightPlanner instance;

        public bool autopan { get; set; }

        public List<PointLatLngAlt> pointlist = new List<PointLatLngAlt>(); // used to calc distance
        public List<PointLatLngAlt> fullpointlist = new List<PointLatLngAlt>();
        public GMapRoute route = new GMapRoute("wp route");
        public GMapRoute homeroute = new GMapRoute("home route");
        static public Object thisLock = new Object();
        private ComponentResourceManager rm = new ComponentResourceManager(typeof (FlightPlanner));

        private Dictionary<string, string[]> cmdParamNames = new Dictionary<string, string[]>();

        List<List<Locationwp>> history = new List<List<Locationwp>>();

        List<int> groupmarkers = new List<int>();

        // AB ZhaoYJ@2017-07-29 for autoTORTL
        bool autoTORTL_added = false;
        List<Locationwp> commands_list_bk;
        bool read_wps_done = false;        
        public enum altmode
        {
            Relative = MAVLink.MAV_FRAME.GLOBAL_RELATIVE_ALT,
            Absolute = MAVLink.MAV_FRAME.GLOBAL,
            Terrain = MAVLink.MAV_FRAME.GLOBAL_TERRAIN_ALT
        }
        GMapMarker marker_routes;
        PointLatLng currentMousePosition;
        Point currMousePoint;
        Color orig_color;

        bool first_wp_add = true;
        ToolTip ttpSettings = new ToolTip();
        ToolTip ttpMeasure = new ToolTip();
        
        private GeocodingProvider gp; //地址编码服务
        List<PointLatLng> searchResult = new List<PointLatLng>(); //搜索结果

        public System.Windows.Forms.Form form_modWPhgt;
        public int fly_vel_max = 5;
        private static string mod_type = "selectWP";

        // AB ZhaoYJ@2017-11-21 for polygon split
        List<List<PointLatLng>> polygons_splited = new List<List<PointLatLng>>();
        List<GMapRoute> lines_splited = new List<GMapRoute>();
        int select_poly_id = 0;

        enum mission_item_info
        {
            LABEL_AUTOTO_WP = 0x20,
            LABEL_RESUME_WP = 0x30,
        };

        private Int16 mission_type;
        private Int16 resume_wp_idx;
        private Int32 resume_wp_loc_x;
        private Int32 resume_wp_loc_y;
        private Int32 resume_wp_loc_z;

        public static GMapOverlay avoidPntsoverlay; // 
        List<squareBound> avoidBoundsList = new List<squareBound>();
        public class squareBound
        {
            
            // 
            //    1———2
            //    |   C  |
            //    |      |
            //    0———3     
            //  C_bearing: 3->0 
            //  p0_bearing: C_bearing - 45
            //  p1_bearing: p0_bearing + 90
            //  p2_bearing: p1_bearing + 90
            //  p3_bearing: p2_bearing + 90 (should be = -C_bearing)
            // 
            public squareBound(PointLatLng centerIn, double bearingIn, double distIn)
            {
                center = centerIn;
                bearing = bearingIn;
                dist = distIn;
                double bearingNow = bearingIn - 45;
                PointLatLng p0 = PointLatLongOnBearingAndDist(center.Lat, center.Lng, bearingIn + bearingNow, distIn);
                bounds.Add(p0);
                bearingNow += 90;
                PointLatLng p1 = PointLatLongOnBearingAndDist(center.Lat, center.Lng, bearingIn + bearingNow, distIn);
                bounds.Add(p1);
                bearingNow += 90;
                PointLatLng p2 = PointLatLongOnBearingAndDist(center.Lat, center.Lng, bearingIn + bearingNow, distIn);
                bounds.Add(p2);
                bearingNow += 90;
                PointLatLng p3 = PointLatLongOnBearingAndDist(center.Lat, center.Lng, bearingIn + bearingNow, distIn);
                bounds.Add(p3);

            }

            public List<PointLatLng> bounds = new List<PointLatLng>();
            double bearing;
            double dist;
            public PointLatLng center;

        }

        public static PointLatLng PointLatLongOnBearingAndDist(double centerLat, double centerLong, double bearing, double distance) // distance: m
        {

            var lonRads = radians(centerLong);
            var latRads = radians(centerLat);
            var bearingRads = radians(bearing);
            double dist_factor = distance / 6371000;
            var maxLatRads = Math.Asin(Math.Sin(latRads) * Math.Cos(dist_factor) + Math.Cos(latRads) * Math.Sin(dist_factor) * Math.Cos(bearingRads));
            var maxLonRads = lonRads + Math.Atan2((Math.Sin(bearingRads) * Math.Sin(dist_factor) * Math.Cos(latRads)), (Math.Cos(dist_factor) - Math.Sin(latRads) * Math.Sin(maxLatRads)));

            var maxLat = degrees(maxLatRads);
            var maxLong = degrees(maxLonRads);

            return new PointLatLng(maxLat, maxLong);
        }

        public static double radians(double val)
        {
            return val * deg2rad;
        }

        public static double degrees(double val)
        {
            return val * rad2deg;
        }

        private void poieditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentGMapMarker == null || !(CurrentGMapMarker is GMapMarkerPOI))
                return;

            POI.POIEdit(CurrentPOIMarker);
        }

        private void poideleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentPOIMarker  == null)
                return;

            POI.POIDelete(CurrentPOIMarker);
        }

        private void poiaddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            POI.POIAdd(MouseDownStart);
        }

        /// <summary>
        /// used to adjust existing point in the datagrid including "H"
        /// </summary>
        /// <param name="pointno"></param>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="alt"></param>
        public void callMeDrag(string pointno, double lat, double lng, int alt)
        {

            // first, update currentWP for GPS correction
            currentWP = currentMarker.Position;
            log.Info("更新当前航点: lat: " + currentWP.Lat + ", lng: " + currentWP.Lng + "\n");

            if (pointno == "")
            {
                return;
            }

            // dragging a WP
            if (pointno == "起航点")
            {
                // auto update home alt
                TXT_homealt.Text = (srtm.getAltitude(lat, lng).alt * CurrentState.multiplierdist).ToString();

                TXT_homelat.Text = lat.ToString();
                TXT_homelng.Text = lng.ToString();
                return;
            }

            if (pointno == "Tracker Home")
            {
                MainV2.comPort.MAV.cs.TrackerLocation = new PointLatLngAlt(lat, lng, alt, "");
                return;
            }

            try
            {
                selectedrow = int.Parse(pointno) - 1;
                Commands.CurrentCell = Commands[1, selectedrow];
                // depending on the dragged item, selectedrow can be reset 
                selectedrow = int.Parse(pointno) - 1;
            }
            catch
            {
                return;
            }

            setfromMap(lat, lng, alt);
        }

        /// <summary>
        /// Actualy Sets the values into the datagrid and verifys height if turned on
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="alt"></param>
        public void setfromMap(double lat, double lng, int alt, double p1 = 0)
        {
            if (selectedrow > Commands.RowCount)
            {
                MessageBox.Show("Invalid coord, How did you do this?");
                return;
            }

            try
            {
                // get current command list
                var currentlist = GetCommandList();
                // add history
                history.Add(currentlist);
            }
            catch (Exception ex)
            {
                MessageBox.Show("A invalid entry has been detected\n" + ex.Message, Strings.ERROR);
            }

            // remove more than 20 revisions
            if (history.Count > 20)
            {
                history.RemoveRange(0, history.Count - 20);
            }

            DataGridViewTextBoxCell cell;
            if (alt == -2 && Commands.Columns[Alt.Index].HeaderText.Equals(cmdParamNames["WAYPOINT"][6] /*"Alt"*/))
            {
                if (CHK_verifyheight.Checked && (altmode)CMB_altmode.SelectedValue != altmode.Terrain) //Drag with verifyheight // use srtm data
                {
                    cell = Commands.Rows[selectedrow].Cells[Alt.Index] as DataGridViewTextBoxCell;
                    float ans;
                    if (float.TryParse(cell.Value.ToString(), out ans))
                    {
                        ans = (int) ans;

                        DataGridViewTextBoxCell celllat =
                            Commands.Rows[selectedrow].Cells[Lat.Index] as DataGridViewTextBoxCell;
                        DataGridViewTextBoxCell celllon =
                            Commands.Rows[selectedrow].Cells[Lon.Index] as DataGridViewTextBoxCell;
                        int oldsrtm =
                            (int)
                                ((srtm.getAltitude(double.Parse(celllat.Value.ToString()),
                                    double.Parse(celllon.Value.ToString())).alt)*CurrentState.multiplierdist);
                        int newsrtm = (int) ((srtm.getAltitude(lat, lng).alt)*CurrentState.multiplierdist);
                        int newh = (int) (ans + newsrtm - oldsrtm);

                        cell.Value = newh;

                        cell.DataGridView.EndEdit();
                    }
                }
            }
            if (Commands.Columns[Lat.Index].HeaderText.Equals(cmdParamNames["WAYPOINT"][4] /*"Lat"*/))
            {
                cell = Commands.Rows[selectedrow].Cells[Lat.Index] as DataGridViewTextBoxCell;
                cell.Value = lat.ToString("0.0000000");
                cell.DataGridView.EndEdit();
            }
            if (Commands.Columns[Lon.Index].HeaderText.Equals(cmdParamNames["WAYPOINT"][5] /*"Long"*/))
            {
                cell = Commands.Rows[selectedrow].Cells[Lon.Index] as DataGridViewTextBoxCell;
                cell.Value = lng.ToString("0.0000000");
                cell.DataGridView.EndEdit();
            }
            if (alt != -1 && alt != -2 &&
                Commands.Columns[Alt.Index].HeaderText.Equals(cmdParamNames["WAYPOINT"][6] /*"Alt"*/))
            {
                cell = Commands.Rows[selectedrow].Cells[Alt.Index] as DataGridViewTextBoxCell;

                {
                    double result;
                    bool pass = double.TryParse(TXT_homealt.Text, out result);

                    if (pass == false)
                    {
                        MessageBox.Show("You must have a home altitude");
                        string homealt = "100";
                        if (DialogResult.Cancel == InputBox.Show("Home Alt", "Home Altitude", ref homealt))
                            return;
                        TXT_homealt.Text = homealt;
                    }
                    int results1;
                    if (!int.TryParse(TXT_DefaultAlt.Text, out results1))
                    {
                        MessageBox.Show("Your default alt is not valid");
                        return;
                    }

                    if (results1 == 0)
                    {
                        string defalt = "100";
                        if (DialogResult.Cancel == InputBox.Show("Default Alt", "Default Altitude", ref defalt))
                            return;
                        TXT_DefaultAlt.Text = defalt;
                    }
                }

                cell.Value = TXT_DefaultAlt.Text;

                float ans;
                if (float.TryParse(cell.Value.ToString(), out ans))
                {
                    ans = (int) ans;
                    if (alt != 0) // use passed in value;
                        cell.Value = alt.ToString();
                    if (ans == 0) // default
                        cell.Value = 50;
                    if (ans == 0 && (MainV2.comPort.MAV.cs.firmware == MainV2.Firmwares.ArduCopter2))
                        cell.Value = 15;

                    // not online and verify alt via srtm
                    if (CHK_verifyheight.Checked) // use srtm data
                    {
                        // is absolute but no verify
                        if ((altmode) CMB_altmode.SelectedValue == altmode.Absolute)
                        {
                            //abs
                            cell.Value =
                                ((srtm.getAltitude(lat, lng).alt)*CurrentState.multiplierdist +
                                 int.Parse(TXT_DefaultAlt.Text)).ToString();
                        }
                        else if ((altmode) CMB_altmode.SelectedValue == altmode.Terrain)
                        {
                            cell.Value = int.Parse(TXT_DefaultAlt.Text);
                        }
                        else
                        {
                            //relative and verify
                            cell.Value =
                                ((int) (srtm.getAltitude(lat, lng).alt)*CurrentState.multiplierdist +
                                 int.Parse(TXT_DefaultAlt.Text) -
                                 (int)
                                     srtm.getAltitude(MainV2.comPort.MAV.cs.HomeLocation.Lat,
                                         MainV2.comPort.MAV.cs.HomeLocation.Lng).alt*CurrentState.multiplierdist)
                                    .ToString();
                        }
                    }

                    cell.DataGridView.EndEdit();
                }
                else
                {
                    MessageBox.Show("Invalid Home or wp Alt");
                    cell.Style.BackColor = Color.Red;
                }
            }

            // convert to utm
            convertFromGeographic(lat, lng);

            // Add more for other params
            if (Commands.Columns[Param1.Index].HeaderText.Equals(cmdParamNames["WAYPOINT"][1] /*"Delay"*/))
            {
                cell = Commands.Rows[selectedrow].Cells[Param1.Index] as DataGridViewTextBoxCell;
                cell.Value = p1;
                cell.DataGridView.EndEdit();
            }

            writeKML();
            Commands.EndEdit();
        }

        private void convertFromGeographic(double lat, double lng)
        {
            if (lat == 0 && lng == 0)
            {
                return;
            }

            // always update other systems, incase user switchs while planning
            try
            {
                //UTM
                var temp = new PointLatLngAlt(lat, lng);
                int zone = temp.GetUTMZone();
                var temp2 = temp.ToUTM();
                Commands[coordZone.Index, selectedrow].Value = zone;
                Commands[coordEasting.Index, selectedrow].Value = temp2[0].ToString("0.000");
                Commands[coordNorthing.Index, selectedrow].Value = temp2[1].ToString("0.000");
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            try
            {
                //MGRS
                Commands[MGRS.Index, selectedrow].Value = ((MGRS) new Geographic(lng, lat)).ToString();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        void convertFromUTM(int rowindex)
        {
            try
            {
                var zone = int.Parse(Commands[coordZone.Index, rowindex].Value.ToString());

                var east = double.Parse(Commands[coordEasting.Index, rowindex].Value.ToString());

                var north = double.Parse(Commands[coordNorthing.Index, rowindex].Value.ToString());

                if (east == 0 && north == 0)
                {
                    return;
                }

                var utm = new utmpos(east, north, zone);

                Commands[Lat.Index, rowindex].Value = utm.ToLLA().Lat;
                Commands[Lon.Index, rowindex].Value = utm.ToLLA().Lng;

            }
            catch
            {
                return;
            }
        }

        void convertFromMGRS(int rowindex)
        {
            try
            {
                var mgrs = Commands[MGRS.Index, rowindex].Value.ToString();

                MGRS temp = new MGRS(mgrs);

                var convert = temp.ConvertTo<Geographic>();

                if (convert.Latitude == 0 || convert.Longitude == 0)
                    return;

                Commands[Lat.Index, rowindex].Value = convert.Latitude.ToString();
                Commands[Lon.Index, rowindex].Value = convert.Longitude.ToString();

            }
            catch
            {
                return;
            }
        }

        PointLatLngAlt mouseposdisplay = new PointLatLngAlt(0, 0);

        /// <summary>
        /// Used for current mouse position
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="alt"></param>
        public void SetMouseDisplay(double lat, double lng, int alt)
        {
            mouseposdisplay.Lat = lat;
            mouseposdisplay.Lng = lng;
            mouseposdisplay.Alt = alt;

            coords1.Lat = mouseposdisplay.Lat;
            coords1.Lng = mouseposdisplay.Lng;
            var altdata = srtm.getAltitude(mouseposdisplay.Lat, mouseposdisplay.Lng, MainMap.Zoom);
            coords1.Alt = altdata.alt;
            coords1.AltSource = altdata.altsource;

            try
            {
                PointLatLng last;

                if (pointlist.Count == 0 || pointlist[pointlist.Count - 1] == null)
                    return;

                last = pointlist[pointlist.Count - 1];

                double lastdist = MainMap.MapProvider.Projection.GetDistance(last, currentMarker.Position);

                double lastbearing = 0;

                if (pointlist.Count > 0)
                {
                    lastbearing = MainMap.MapProvider.Projection.GetBearing(last, currentMarker.Position);
                }

                lbl_prevdist.Text = rm.GetString("lbl_prevdist.Text") + ": " + FormatDistance(lastdist, true) + " " + rm.GetString("AZ.HeaderText") + ": " +
                                    lastbearing.ToString("0");

                // 0 is home
                if (pointlist[0] != null)
                {
                    double homedist = MainMap.MapProvider.Projection.GetDistance(currentMarker.Position, pointlist[0]);

                    lbl_homedist.Text = rm.GetString("lbl_homedist.Text") + ": " + FormatDistance(homedist, true);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Used to create a new WP
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="alt"></param>
        public void AddWPToMap(double lat, double lng, int alt)
        {
            if (polygongridmode)
            {
                addPolygonPointToolStripMenuItem_Click(null, null);
                return;
            }
            else if (polygonsplitmode)
            {
                // add split point
                if(startsplit.IsEmpty)
                {
                    startsplit = MouseDownStart;
                    GMarkerGoogle gmm = new GMarkerGoogle(startsplit, GMarkerGoogleType.red_big_stop);
                    gmm.Tag = "0";
                    splitoverlay.Markers.Add(gmm);
                    MainMap.Invalidate();
                }
                else if(endsplit.IsEmpty)
                {
                    endsplit = MouseDownStart;

                    List<PointLatLng> polygonPoints = new List<PointLatLng>();
                    polygonPoints.Add(startsplit);
                    polygonPoints.Add(endsplit);

                    GMapPolygon line = new GMapPolygon(polygonPoints, "split poly");
                    line.Stroke.Color = Color.Red;

                    splitoverlay.Polygons.Add(line);
                    GMarkerGoogle gmm = new GMarkerGoogle(endsplit, GMarkerGoogleType.red_big_stop);
                    gmm.Tag = "1";
                    splitoverlay.Markers.Add(gmm);
                    MainMap.Invalidate();

                }

                return;
            }
            else if(!wpinsertmode)
            {
                return;
            }

            if (sethome)
            {
                sethome = false;
                callMeDrag("起航点", lat, lng, alt);
                return;
            }
            // creating a WP

            selectedrow = Commands.Rows.Add();

            if (splinemode)
            {
                Commands.Rows[selectedrow].Cells[Command.Index].Value = MAVLink.MAV_CMD.SPLINE_WAYPOINT.ToString();
                ChangeColumnHeader(MAVLink.MAV_CMD.SPLINE_WAYPOINT.ToString());
            }
            else
            {
                string tmp_str = MAVLink.MAV_CMD.WAYPOINT.ToString();
                Commands.Rows[selectedrow].Cells[Command.Index].Value = MAVLink.MAV_CMD.WAYPOINT.ToString();
                ChangeColumnHeader(MAVLink.MAV_CMD.WAYPOINT.ToString());
            }

            setfromMap(lat, lng, alt);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // undo
            if (keyData == (Keys.Control | Keys.Z))
            {
                if (history.Count > 0)
                {
                    int no = history.Count - 1;
                    var pop = history[no];
                    history.RemoveAt(no);
                    WPtoScreen(pop);
                }
                return true;
            }

            // open wp file
            if (keyData == (Keys.Control | Keys.O))
            {
                loadWPFileToolStripMenuItem_Click(null, null);
                return true;
            }

            // save wp file
            if (keyData == (Keys.Control | Keys.S))
            {
                saveWPFileToolStripMenuItem_Click(null, null);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        public FlightPlanner()
        {
            instance = this;

            InitializeComponent();

            // config map             
            MainMap.CacheLocation = Settings.GetDataDirectory() +
                                    "gmapcache" + Path.DirectorySeparatorChar;

            // map events
            MainMap.OnPositionChanged += MainMap_OnCurrentPositionChanged;
            MainMap.OnTileLoadStart += MainMap_OnTileLoadStart;
            MainMap.OnTileLoadComplete += MainMap_OnTileLoadComplete;
            MainMap.OnMarkerClick += MainMap_OnMarkerClick;
            MainMap.OnMapZoomChanged += MainMap_OnMapZoomChanged;
            MainMap.OnMapTypeChanged += MainMap_OnMapTypeChanged;
            MainMap.MouseMove += MainMap_MouseMove;
            MainMap.MouseDown += MainMap_MouseDown;
            MainMap.MouseUp += MainMap_MouseUp;
            MainMap.OnMarkerEnter += MainMap_OnMarkerEnter;
            MainMap.OnMarkerLeave += MainMap_OnMarkerLeave;
            MainMap.OnRouteEnter += new RouteEnter(map_OnRouteEnter);
            MainMap.OnRouteLeave += new RouteLeave(map_OnRouteLeave);
            MainMap.OnPolygonLeave += new PolygonLeave(this.map_OnPolygonLeave);
            MainMap.OnPolygonEnter += new PolygonEnter(this.map_OnPolygonEnter);

            MainMap.MapScaleInfoEnabled = false;
            MainMap.ScalePen = new Pen(Color.Red);

            MainMap.DisableFocusOnMouseEnter = true;

            MainMap.ForceDoubleBuffer = false;

            //WebRequest.DefaultWebProxy.Credentials = System.Net.CredentialCache.DefaultCredentials;

            // get map type
            MainMap.MapProvider = GMap.NET.MapProviders.AMapSateliteProvider.Instance;
            comboBoxMapType.ValueMember = "Name";
            comboBoxMapType.DataSource = GMapProviders.List.ToArray();
            comboBoxMapType.SelectedItem = MainMap.MapProvider;
            // comboBoxMapType.Text = "高德卫星地图";

            comboBoxMapType.SelectedValueChanged += comboBoxMapType_SelectedValueChanged;

            MainMap.RoutesEnabled = true;
            MainMap.PolygonsEnabled = true;

            //MainMap.MaxZoom = 18;

            // get zoom  
            MainMap.MinZoom = 0;
            MainMap.MaxZoom = 24;

            // draw this layer first
            kmlpolygonsoverlay = new GMapOverlay("kmlpolygons");
            MainMap.Overlays.Add(kmlpolygonsoverlay);

            geofenceoverlay = new GMapOverlay("geofence");
            MainMap.Overlays.Add(geofenceoverlay);

            rallypointoverlay = new GMapOverlay("rallypoints");
            MainMap.Overlays.Add(rallypointoverlay);

            routesoverlay = new GMapOverlay("routes");
            MainMap.Overlays.Add(routesoverlay);

            measureoverlay = new GMapOverlay("measure");
            MainMap.Overlays.Add(measureoverlay);

            polygonsoverlay = new GMapOverlay("polygons");
            MainMap.Overlays.Add(polygonsoverlay);

            splitoverlay = new GMapOverlay("split_poly");
            MainMap.Overlays.Add(splitoverlay);

            avoidPntsoverlay = new GMapOverlay("avoid_poly");
            MainMap.Overlays.Add(avoidPntsoverlay);

            airportsoverlay = new GMapOverlay("airports");
            MainMap.Overlays.Add(airportsoverlay);

            objectsoverlay = new GMapOverlay("objects");
            MainMap.Overlays.Add(objectsoverlay);

            drawnpolygonsoverlay = new GMapOverlay("drawnpolygons");
            MainMap.Overlays.Add(drawnpolygonsoverlay);

            searchoverlay = new GMapOverlay("searchs");
            MainMap.Overlays.Add(searchoverlay);

            // private GMapOverlay locations = new GMapOverlay("locations"); //放置搜索结果的图层
            // 
            // MainMap.Overlays.Add(poioverlay);

            top = new GMapOverlay("top");
            //MainMap.Overlays.Add(top);

            objectsoverlay.Markers.Clear();

            // set current marker
            currentMarker = new GMarkerGoogle(MainMap.Position, GMarkerGoogleType.red);
            //top.Markers.Add(currentMarker);

            // map center
            center = new GMarkerGoogle(MainMap.Position, GMarkerGoogleType.none);
            top.Markers.Add(center);

            MainMap.Zoom = 3;

            CMB_altmode.DisplayMember = "Value";
            CMB_altmode.ValueMember = "Key";
            CMB_altmode.DataSource = EnumTranslator.EnumToList<altmode>();

            //set default
            CMB_altmode.SelectedItem = altmode.Relative;

            RegeneratePolygon();

            updateCMDParams();

            Up.Image = Resources.up;
            Down.Image = Resources.down;

            updateMapType(null, null);

            // hide the map to prevent redraws when its loaded
            panelMap.Visible = false;

            CMB_altmode.Visible = false;
            CMB_altmode.Hide();
            this.pannel_wpcommands.Hide();
            this.Commands.Hide();
            this.panel_avoidPnts.Hide();
            this.dataGridView_avoidPnts.Hide();


            // modify wp hgt form
            initModWPhgtForm();

            // this.gb_modWPhgt.Location = new Point(this.panelWaypoints.Width, 10);
            // this.gb_modWPhgt.Hide();

            // this.panel2.Visible = false; 

            /*
            var timer = new System.Timers.Timer();

            // 2 second
            timer.Interval = 2000;
            timer.Elapsed += updateMapType;

            timer.Start();
            */
        }

        private void initModWPhgtForm()
        {
            form_modWPhgt = new Form();
            form_modWPhgt.ControlBox = false;
            form_modWPhgt.Text = "修改航点高度";
            // form_modWPhgt.Location = new Point(800, 800);
            form_modWPhgt.AutoSize = false;
            form_modWPhgt.Size = new Size(this.gb_modWPhgt.Size.Width + 40, this.gb_modWPhgt.Size.Height + 60);
            // form_modWPhgt.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            form_modWPhgt.MaximizeBox = false;
            form_modWPhgt.TopMost = true;
            this.form_modWPhgt.StartPosition = FormStartPosition.Manual;
            this.form_modWPhgt.Location = new Point(1000, 560);

            this.form_modWPhgt.Controls.Add(gb_modWPhgt);
            this.gb_modWPhgt.Location = new Point(8, 8);

        }

        void updateMapType(object sender, System.Timers.ElapsedEventArgs e)
        {
            log.Info("updateMapType invoke req? " + comboBoxMapType.InvokeRequired);

            if (sender is System.Timers.Timer)
                ((System.Timers.Timer)sender).Stop();

            string mapType = Settings.Instance["MapType"];
            // AB ZhaoYJ@2017-06-11 for def map type to AMap satellite
            // mapType = "AMapSatellite";
            if (!string.IsNullOrEmpty(mapType))
            {
                try
                {
                    var index = GMapProviders.List.FindIndex(x => (x.Name == mapType));

                    if (index != -1)
                        comboBoxMapType.SelectedIndex = index;
                }
                catch
                {
                }
            }
            else
            {
                if (L10N.ConfigLang.IsChildOf(CultureInfo.GetCultureInfo("zh-Hans")))
                {
                    //--MessageBox.Show(
                    //--    "亲爱的中国用户，为保证地图使用正常，已为您将默认地图自动切换到具有中国特色的【谷歌中国卫星地图】！\r\n与默认【谷歌卫星地图】的区别：使用.cn服务器，加入火星坐标修正\r\n如果您所在的地区仍然无法使用，天书同时推荐必应或高德地图，其它地图由于没有加入坐标修正功能，为确保飞行安全，请谨慎选择",
                    //--    "默认地图已被切换");

                    try
                    {
                        var index = GMapProviders.List.FindIndex(x => (x.Name == "谷歌中国卫星地图"));

                        if (index != -1)
                            comboBoxMapType.SelectedIndex = index;
                    }
                    catch
                    {
                    }
                }
                else
                {
                    mapType = "GoogleSatelliteMap";
                    // set default
                    try
                    {
                        var index = GMapProviders.List.FindIndex(x => (x.Name == mapType));

                        if (index != -1)
                            comboBoxMapType.SelectedIndex = index;
                    }
                    catch
                    {
                    }
                }
            }
        }

        void updateCMDParams()
        {
            cmdParamNames = readCMDXML();

            List<string> cmds = new List<string>();

            foreach (string item in cmdParamNames.Keys)
            {
                cmds.Add(item);
            }

            cmds.Add("UNKNOWN");

            Command.DataSource = cmds;
        }

        Dictionary<string, string[]> readCMDXML()
        {
            Dictionary<string, string[]> cmd = new Dictionary<string, string[]>();

            // do lang stuff here

            string file = Settings.GetRunningDirectory() + "mavcmd.xml";

            if (!File.Exists(file))
            {
                MessageBox.Show("Missing mavcmd.xml file");
                return cmd;
            }

            log.Info("Reading MAV_CMD for " + MainV2.comPort.MAV.cs.firmware);

            using (XmlReader reader = XmlReader.Create(file))
            {
                reader.Read();
                reader.ReadStartElement("CMD");
                if (MainV2.comPort.MAV.cs.firmware == MainV2.Firmwares.ArduPlane ||
                    MainV2.comPort.MAV.cs.firmware == MainV2.Firmwares.Ateryx)
                {
                    reader.ReadToFollowing("APM");
                }
                else if (MainV2.comPort.MAV.cs.firmware == MainV2.Firmwares.ArduRover)
                {
                    reader.ReadToFollowing("APRover");
                }
                else
                {
                    reader.ReadToFollowing("AC2");
                }

                XmlReader inner = reader.ReadSubtree();

                inner.Read();

                inner.MoveToElement();

                inner.Read();

                while (inner.Read())
                {
                    inner.MoveToElement();
                    if (inner.IsStartElement())
                    {
                        string cmdname = inner.Name;
                        string[] cmdarray = new string[7];
                        int b = 0;

                        XmlReader inner2 = inner.ReadSubtree();

                        inner2.Read();

                        while (inner2.Read())
                        {
                            inner2.MoveToElement();
                            if (inner2.IsStartElement())
                            {
                                cmdarray[b] = inner2.ReadString();
                                b++;
                            }
                        }

                        cmd[cmdname] = cmdarray;
                    }
                }
            }

            return cmd;
        }

        void Commands_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.Info(e.Exception + " " + e.Context + " col " + e.ColumnIndex);
            e.Cancel = false;
            e.ThrowException = false;
            //throw new NotImplementedException();
        }
        void AvoidCommands_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.Info(e.Exception + " " + e.Context + " col " + e.ColumnIndex);
            e.Cancel = false;
            e.ThrowException = false;
            //throw new NotImplementedException();
        }
        private bool verify_code()
        {
            string vcode = "F";
            string correct_code = "Gcs123";
            if (InputBox.Show("注意：这是高级调试功能，不熟悉此功能的人员慎点！", "请输入检验码", ref vcode, true) ==
        System.Windows.Forms.DialogResult.OK)
            {

                if (!vcode.Equals(correct_code))
                {
                    MessageBox.Show("校验失败，无法编辑航点列表！", "校验失败");
                    return false;

                }
                else
                {
                    MessageBox.Show("校验成功，可以编辑航点列表！", "校验成功");
                    return true;
                }

            }
            return false;

        }

        /// <summary>
        /// Adds a new row to the datagrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BUT_Add_Click(object sender, EventArgs e)
        {
            if (Commands.CurrentRow == null)
            {
                selectedrow = 0;
            }
            else
            {
                selectedrow = Commands.CurrentRow.Index;
            }

            if (Commands.RowCount <= 1)
            {
                selectedrow = Commands.Rows.Add();
            }
            else
            {
                if (Commands.RowCount == selectedrow + 1)
                {
                    DataGridViewRow temp = Commands.Rows[selectedrow];
                    selectedrow = Commands.Rows.Add();
                }
                else
                {
                    Commands.Rows.Insert(selectedrow + 1, 1);
                }
            }
            writeKML();
        }
        bool display_pannel_wpcommands = true;
        private void BUT_Edit_Click(object sender, EventArgs e)
        {
            if (display_pannel_wpcommands)
            {
                if (!verify_code()) return;
                this.pannel_wpcommands.Show();
                this.Commands.Show();
            }
            else
            {
                this.pannel_wpcommands.Hide();
                this.Commands.Hide();
            }
            display_pannel_wpcommands = !display_pannel_wpcommands;

        }

        private void FlightPlanner_Load(object sender, EventArgs e)
        {
            quickadd = true;

            Visible = false;

            config(false);

            quickadd = false;

            POI.POIModified += POI_POIModified;

            if (Settings.Instance["WMSserver"] != null)
                WMSProvider.CustomWMSURL = Settings.Instance["WMSserver"];

            trackBar1.Value = (int) MainMap.Zoom;

            // check for net and set offline if needed
            try
            {
                IPAddress[] addresslist = Dns.GetHostAddresses("www.google.com");
            }
            catch (Exception)
            {
                // here if dns failed
                isonline = false;
            }

            // setup geofence
            List<PointLatLng> polygonPoints = new List<PointLatLng>();
            geofencepolygon = new GMapPolygon(polygonPoints, "geofence");
            geofencepolygon.Stroke = new Pen(Color.Pink, 5);
            geofencepolygon.Fill = Brushes.Transparent;

            //setup drawnpolgon
            List<PointLatLng> polygonPoints2 = new List<PointLatLng>();
            drawnpolygon = new GMapPolygon(polygonPoints2, "drawnpoly");
            drawnpolygon.Stroke = new Pen(Color.LightGoldenrodYellow, 2);
            drawnpolygon.Fill = Brushes.Transparent;

            updateCMDParams();

            panelMap.Visible = false;

            // mono
            panelMap.Dock = DockStyle.None;
            panelMap.Dock = DockStyle.Fill;
            panelMap_Resize(null, null);

            //set home
            try
            {
                if (TXT_homelat.Text != "")
                {
                    MainMap.Position = new PointLatLng(double.Parse(TXT_homelat.Text), double.Parse(TXT_homelng.Text));
                    MainMap.Zoom = 16;
                }
            }
            catch (Exception)
            {
            }

            panelMap.Refresh();

            panelMap.Visible = true;

            writeKML();

            // switch the action and wp table
            if (Settings.Instance["FP_docking"] == "Bottom")
            {
                switchDockingToolStripMenuItem_Click(null, null);
            }

            Visible = true;

            timer1.Start();
        }

        void POI_POIModified(object sender, EventArgs e)
        {
            POI.UpdateOverlay(poioverlay);
        }

        void parser_ElementAdded(object sender, ElementEventArgs e)
        {
            processKML(e.Element);
        }

        private void processKML(Element Element)
        {
            try
            {
                //  log.Info(Element.ToString() + " " + Element.Parent);
            }
            catch
            {
            }

            Document doc = Element as Document;
            Placemark pm = Element as Placemark;
            Folder folder = Element as Folder;
            Polygon polygon = Element as Polygon;
            LineString ls = Element as LineString;

            if (doc != null)
            {
                foreach (var feat in doc.Features)
                {
                    //Console.WriteLine("feat " + feat.GetType());
                    //processKML((Element)feat);
                }
            }
            else if (folder != null)
            {
                foreach (Feature feat in folder.Features)
                {
                    //Console.WriteLine("feat "+feat.GetType());
                    //processKML(feat);
                }
            }
            else if (pm != null)
            {
            }
            else if (polygon != null)
            {
                GMapPolygon kmlpolygon = new GMapPolygon(new List<PointLatLng>(), "kmlpolygon");

                kmlpolygon.Stroke.Color = Color.Purple;
                kmlpolygon.Fill = Brushes.Transparent;

                foreach (var loc in polygon.OuterBoundary.LinearRing.Coordinates)
                {
                    kmlpolygon.Points.Add(new PointLatLng(loc.Latitude, loc.Longitude));
                }

                kmlpolygonsoverlay.Polygons.Add(kmlpolygon);
            }
            else if (ls != null)
            {
                GMapRoute kmlroute = new GMapRoute(new List<PointLatLng>(), "kmlroute");

                kmlroute.Stroke.Color = Color.Purple;

                foreach (var loc in ls.Coordinates)
                {
                    kmlroute.Points.Add(new PointLatLng(loc.Latitude, loc.Longitude));
                }

                kmlpolygonsoverlay.Routes.Add(kmlroute);
            }
        }

        private void ChangeColumnHeader(string command)
        {
#if HANS
            // for Hans
            switch(command)
            {
                case "WAYPOINT":
                    command = "WAYPOINT";
                    break;
                case "SPLINE_WAYPOINT":
                    command = "弧形航点";
                    break;
                case "RETURN_TO_LAUNCH":
                    command = "自动返航";
                    break;
                case "LAND":
                    command = "自动降落";
                    break;
                case "TAKEOFF":
                    command = "自动起飞";
                    break;
                case "DELAY":
                    command = "停留时间";
                    break;
                case "DO_JUMP":
                    command = "跳转航点";
                    break;
                case "DO_CHANGE_SPEED":
                    command = "改变速度";
                    break;
                case "DO_SET_SERVO":
                    command = "执行输出";
                    break;
                case "DO_REPEAT_SERVO":
                    command = "执行周期性输出";
                    break;
                case "CONDITION_DELAY":
                case "DO_SET_ROI":
                case "DO_GUIDED_LIMITS":
                case "PAYLOAD_PLACE":
                case "GUIDED_ENABLE":
                case "LOITER_UNLIM":
                case "LOITER_TIME":
                case "LOITER_TURNS":
                    MessageBox.Show(command + " 不支持");
                    return;
                    // break;

            }
#endif

            try
            {
                if (cmdParamNames.ContainsKey(command))
                    for (int i = 1; i <= 7; i++)
                        Commands.Columns[i].HeaderText = cmdParamNames[command][i - 1];
                else
                    for (int i = 1; i <= 7; i++)
                        Commands.Columns[i].HeaderText = "setme";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Used to update column headers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Commands_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (quickadd)
                return;
            try
            {
                selectedrow = e.RowIndex;
                string option = Commands[Command.Index, selectedrow].EditedFormattedValue.ToString();
                string cmd;
                try
                {
                    cmd = Commands[Command.Index, selectedrow].Value.ToString();
                }
                catch
                {
                    cmd = option;
                }
                //Console.WriteLine("editformat " + option + " value " + cmd);
                ChangeColumnHeader(cmd);

                if (cmd == "WAYPOINT")
                {
                }

                //  writeKML();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Commands_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for (int i = 0; i < Commands.ColumnCount; i++)
            {
                DataGridViewCell tcell = Commands.Rows[e.RowIndex].Cells[i];
                if (tcell.GetType() == typeof (DataGridViewTextBoxCell))
                {
                    if (tcell.Value == null)
                        tcell.Value = "0";
                }
            }

            DataGridViewComboBoxCell cell = Commands.Rows[e.RowIndex].Cells[Command.Index] as DataGridViewComboBoxCell;
            if (cell.Value == null)
            {
                cell.Value = "WAYPOINT";
                cell.DropDownWidth = 200;
                Commands.Rows[e.RowIndex].Cells[Delete.Index].Value = "X";
                if (!quickadd)
                {
                    Commands_RowEnter(sender, new DataGridViewCellEventArgs(0, e.RowIndex - 0)); // do header labels
                    Commands_RowValidating(sender, new DataGridViewCellCancelEventArgs(0, e.RowIndex));
                        // do default values
                }
            }

            if (quickadd)
                return;

            try
            {
                Commands.CurrentCell = Commands.Rows[e.RowIndex].Cells[0];

                if (Commands.Rows.Count > 1)
                {
                    if (Commands.Rows[e.RowIndex - 1].Cells[Command.Index].Value.ToString() == "WAYPOINT")
                    {
                        Commands.Rows[e.RowIndex].Selected = true; // highlight row
                    }
                    else
                    {
                        Commands.CurrentCell = Commands[1, e.RowIndex - 1];
                        //Commands_RowEnter(sender, new DataGridViewCellEventArgs(0, e.RowIndex-1));
                    }
                }
            }
            catch (Exception)
            {
            }
            // Commands.EndEdit();
        }

        private void Commands_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            selectedrow = e.RowIndex;
            Commands_RowEnter(sender, new DataGridViewCellEventArgs(0, e.RowIndex - 0));
                // do header labels - encure we dont 0 out valid colums
            int cols = Commands.Columns.Count;
            for (int a = 1; a < cols; a++)
            {
                DataGridViewTextBoxCell cell;
                cell = Commands.Rows[selectedrow].Cells[a] as DataGridViewTextBoxCell;

                if (Commands.Columns[a].HeaderText.Equals("") && cell != null && cell.Value == null)
                {
                    cell.Value = "0";
                }
                else
                {
                    if (cell != null && (cell.Value == null || cell.Value.ToString() == ""))
                    {
                        cell.Value = "?";
                    }
                }
            }
        }

        private void AvoidCommands_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (quickadd)
                return;
            try
            {
                selectedrow = e.RowIndex;
                string option = Commands[Command.Index, selectedrow].EditedFormattedValue.ToString();
                string cmd;
                try
                {
                    cmd = Commands[Command.Index, selectedrow].Value.ToString();
                }
                catch
                {
                    cmd = option;
                }
                //Console.WriteLine("editformat " + option + " value " + cmd);
                ChangeColumnHeader(cmd);

                if (cmd == "WAYPOINT")
                {
                }

                //  writeKML();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void AvoidCommands_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for (int i = 0; i < dataGridView_avoidPnts.ColumnCount; i++)
            {
                DataGridViewCell tcell = dataGridView_avoidPnts.Rows[e.RowIndex].Cells[i];
                if (tcell.GetType() == typeof(DataGridViewTextBoxCell))
                {
                    if (tcell.Value == null)
                        tcell.Value = "0";
                }
            }

            DataGridViewComboBoxCell cell = dataGridView_avoidPnts.Rows[e.RowIndex].Cells[Command.Index] as DataGridViewComboBoxCell;
            if (cell.Value == null)
            {
                cell.Value = "WAYPOINT";
                cell.DropDownWidth = 200;
                dataGridView_avoidPnts.Rows[e.RowIndex].Cells[Delete.Index].Value = "X";
                if (!quickadd)
                {
                    AvoidCommands_RowEnter(sender, new DataGridViewCellEventArgs(0, e.RowIndex - 0)); // do header labels
                    AvoidCommands_RowValidating(sender, new DataGridViewCellCancelEventArgs(0, e.RowIndex));
                    // do default values
                }
            }

            if (quickadd)
                return;

            try
            {
                dataGridView_avoidPnts.CurrentCell = dataGridView_avoidPnts.Rows[e.RowIndex].Cells[0];

                if (dataGridView_avoidPnts.Rows.Count > 1)
                {
                    if (dataGridView_avoidPnts.Rows[e.RowIndex - 1].Cells[Command.Index].Value.ToString() == "WAYPOINT")
                    {
                        dataGridView_avoidPnts.Rows[e.RowIndex].Selected = true; // highlight row
                    }
                    else
                    {
                        dataGridView_avoidPnts.CurrentCell = dataGridView_avoidPnts[1, e.RowIndex - 1];
                        //Commands_RowEnter(sender, new DataGridViewCellEventArgs(0, e.RowIndex-1));
                    }
                }
            }
            catch (Exception)
            {
            }
            // Commands.EndEdit();
        }

        private void AvoidCommands_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            selectedrow = e.RowIndex;
            Commands_RowEnter(sender, new DataGridViewCellEventArgs(0, e.RowIndex - 0));
            // do header labels - encure we dont 0 out valid colums
            int cols = dataGridView_avoidPnts.Columns.Count;
            for (int a = 1; a < cols; a++)
            {
                DataGridViewTextBoxCell cell;
                cell = dataGridView_avoidPnts.Rows[selectedrow].Cells[a] as DataGridViewTextBoxCell;

                if (dataGridView_avoidPnts.Columns[a].HeaderText.Equals("") && cell != null && cell.Value == null)
                {
                    cell.Value = "0";
                }
                else
                {
                    if (cell != null && (cell.Value == null || cell.Value.ToString() == ""))
                    {
                        cell.Value = "?";
                    }
                }
            }
        }

        /// <summary>
        /// used to add a marker to the map display
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="lng"></param>
        /// <param name="lat"></param>
        /// <param name="alt"></param>
        private void addpolygonmarker(string tag, double lng, double lat, double alt, Color? color)
        {
            try
            {
                PointLatLng point = new PointLatLng(lat, lng);
                GMapMarkerWP m;

                // AB ZhaoYJ for distinguish home
                if (tag.Equals("起航点"))
                {
                    m = new GMapMarkerWP(point, tag, GMarkerGoogleType.blue);
                }
                else
                {
                    m = new GMapMarkerWP(point, tag);
                }
                

                m.ToolTipMode = MarkerTooltipMode.OnMouseOver;
                // m.ToolTipText = "高度: " + alt.ToString("0");
                m.Tag = tag;

                int wpno = -1;
                if (int.TryParse(tag, out wpno))
                {
                    // preselect groupmarker
                    if (groupmarkers.Contains(wpno))
                        m.selected = true;
                }

                //MissionPlanner.GMapMarkerRectWPRad mBorders = new MissionPlanner.GMapMarkerRectWPRad(point, (int)float.Parse(TXT_WPRad.Text), MainMap);
                GMapMarkerRect mBorders = new GMapMarkerRect(point);
                {
                    mBorders.InnerMarker = m;
                    mBorders.Tag = tag;
                    mBorders.wprad = (int) (float.Parse(TXT_WPRad.Text) / MainMap.Zoom);
                    if (color.HasValue)
                    {
                        mBorders.Color = color.Value;
                    }
                }

                objectsoverlay.Markers.Add(m);
                objectsoverlay.Markers.Add(mBorders);
            }
            catch (Exception)
            {
            }
        }

        private void addpolygonmarkergrid(string tag, double lng, double lat, int alt)
        {
            try
            {
                PointLatLng point = new PointLatLng(lat, lng);
                GMarkerGoogle m = new GMarkerGoogle(point, GMarkerGoogleType.orange);
                m.ToolTipMode = MarkerTooltipMode.Never;
                m.ToolTipText = "grid" + tag;
                m.Tag = "grid" + tag;
                
                //MissionPlanner.GMapMarkerRectWPRad mBorders = new MissionPlanner.GMapMarkerRectWPRad(point, (int)float.Parse(TXT_WPRad.Text), MainMap);
                GMapMarkerRect mBorders = new GMapMarkerRect(point);
                {
                    mBorders.InnerMarker = m;
                }

                mBorders.wprad = (int)(float.Parse(TXT_WPRad.Text) / MainMap.Zoom);
                mBorders.Color = Color.LightBlue ;

                drawnpolygonsoverlay.Markers.Add(m);
                drawnpolygonsoverlay.Markers.Add(mBorders);
            }
            catch (Exception ex)
            {
                log.Info(ex.ToString());
            }
        }
        private void insertpolygonmarkergrid(int insert_id, string tag, double lng, double lat, int alt)
        {
            try
            {
                PointLatLng point = new PointLatLng(lat, lng);
                GMarkerGoogle m = new GMarkerGoogle(point, GMarkerGoogleType.orange);
                m.ToolTipMode = MarkerTooltipMode.Never;
                m.ToolTipText = "grid" + tag;
                m.Tag = "grid" + tag;

                //MissionPlanner.GMapMarkerRectWPRad mBorders = new MissionPlanner.GMapMarkerRectWPRad(point, (int)float.Parse(TXT_WPRad.Text), MainMap);
                GMapMarkerRect mBorders = new GMapMarkerRect(point);
                {
                    mBorders.InnerMarker = m;
                }

                mBorders.wprad = (int)(float.Parse(TXT_WPRad.Text) / MainMap.Zoom);
                mBorders.Color = Color.LightBlue;

                drawnpolygonsoverlay.Markers.Insert(insert_id, m);
                drawnpolygonsoverlay.Markers.Insert(insert_id, mBorders);               

            }
            catch (Exception ex)
            {
                log.Info(ex.ToString());
            }
        }

        void updateRowNumbers()
        {
            // number rows 
            this.BeginInvoke((MethodInvoker) delegate
            {
                // thread for updateing row numbers
                for (int a = 0; a < Commands.Rows.Count - 0; a++)
                {
                    try
                    {
                        if (Commands.Rows[a].HeaderCell.Value == null)
                        {
                            //Commands.Rows[a].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                            Commands.Rows[a].HeaderCell.Value = (a + 1).ToString();
                        }
                        // skip rows with the correct number
                        string rowno = Commands.Rows[a].HeaderCell.Value.ToString();
                        if (!rowno.Equals((a + 1).ToString()))
                        {
                            // this code is where the delay is when deleting.
                            Commands.Rows[a].HeaderCell.Value = (a + 1).ToString();
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            });
        }


        void updateAvoidPntsRowNumbers()
        {
            // number rows 
            this.BeginInvoke((MethodInvoker)delegate
            {
                // thread for updateing row numbers
                for (int a = 0; a < dataGridView_avoidPnts.Rows.Count - 0; a++)
                {
                    try
                    {
                        if (dataGridView_avoidPnts.Rows[a].HeaderCell.Value == null)
                        {
                            //Commands.Rows[a].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                            dataGridView_avoidPnts.Rows[a].HeaderCell.Value = (a + 1).ToString();
                        }
                        // skip rows with the correct number
                        string rowno = dataGridView_avoidPnts.Rows[a].HeaderCell.Value.ToString();
                        if (!rowno.Equals((a + 1).ToString()))
                        {
                            // this code is where the delay is when deleting.
                            dataGridView_avoidPnts.Rows[a].HeaderCell.Value = (a + 1).ToString();
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            });
        }

        /// <summary>
        /// used to write a KML, update the Map view polygon, and update the row headers
        /// </summary>
        public void writeKML()
        {
            // quickadd is for when loading wps from eeprom or file, to prevent slow, loading times
            if (quickadd)
                return;

            // this is to share the current mission with the data tab
            pointlist = new List<PointLatLngAlt>();

            fullpointlist.Clear();
            routesoverlay.Clear();

            Debug.WriteLine(DateTime.Now);
            try
            {
                if (objectsoverlay != null) // hasnt been created yet
                {
                    objectsoverlay.Markers.Clear();
                }

                // process and add home to the list
                string home;
                if (TXT_homealt.Text != "" && TXT_homelat.Text != "" && TXT_homelng.Text != "")
                {
                    home = string.Format("{0},{1},{2}\r\n", TXT_homelng.Text, TXT_homelat.Text, TXT_DefaultAlt.Text);
                    if (objectsoverlay != null) // during startup
                    {
                        pointlist.Add(new PointLatLngAlt(double.Parse(TXT_homelat.Text), double.Parse(TXT_homelng.Text),
                            double.Parse(TXT_homealt.Text), "起航点"));
                        fullpointlist.Add(pointlist[pointlist.Count - 1]);
                        addpolygonmarker("起航点", double.Parse(TXT_homelng.Text), double.Parse(TXT_homelat.Text), 0, System.Drawing.Color.Blue);
                    }
                }
                else
                {
                    home = "";
                    pointlist.Add(null);
                    fullpointlist.Add(pointlist[pointlist.Count - 1]);
                }

                // setup for centerpoint calc etc.
                double avglat = 0;
                double avglong = 0;
                double maxlat = -180;
                double maxlong = -180;
                double minlat = 180;
                double minlong = 180;
                double homealt = 0;
                try
                {
                    if (!String.IsNullOrEmpty(TXT_homealt.Text))
                        homealt = (int) double.Parse(TXT_homealt.Text);
                }
                catch
                {
                }
                if ((altmode) CMB_altmode.SelectedValue == altmode.Absolute)
                {
                    homealt = 0; // for absolute we dont need to add homealt
                }

                int usable = 0;

                updateRowNumbers();
                // updateAvoidPntsRowNumbers();


                long temp = Stopwatch.GetTimestamp();

                string lookat = "";
                PointLatLng last_pnt = new PointLatLng(0.0, 0.0);
                bool first_pnt_add = true;
                for (int a = 0; a < Commands.Rows.Count - 0; a++)
                {
                    try
                    {
                        if (Commands.Rows[a].Cells[Command.Index].Value.ToString().Contains("UNKNOWN"))
                            continue;

                        int command =
                            (byte)
                                (int)
                                    Enum.Parse(typeof (MAVLink.MAV_CMD),
                                        Commands.Rows[a].Cells[Command.Index].Value.ToString(), false);
                        if (command < (byte) MAVLink.MAV_CMD.LAST &&
                            command != (byte) MAVLink.MAV_CMD.TAKEOFF && // doesnt have a position
                            command != (byte)MAVLink.MAV_CMD.VTOL_TAKEOFF && // doesnt have a position
                            command != (byte) MAVLink.MAV_CMD.RETURN_TO_LAUNCH &&
                            command != (byte) MAVLink.MAV_CMD.CONTINUE_AND_CHANGE_ALT &&
                            command != (byte) MAVLink.MAV_CMD.GUIDED_ENABLE
                            || command == (byte) MAVLink.MAV_CMD.DO_SET_ROI)
                        {
                            string cell2 = Commands.Rows[a].Cells[Alt.Index].Value.ToString(); // alt
                            string cell3 = Commands.Rows[a].Cells[Lat.Index].Value.ToString(); // lat
                            string cell4 = Commands.Rows[a].Cells[Lon.Index].Value.ToString(); // lng

                            // land can be 0,0 or a lat,lng
                            if (command == (byte) MAVLink.MAV_CMD.LAND && cell3 == "0" && cell4 == "0")
                                continue;

                            if (cell4 == "?" || cell3 == "?")
                                continue;

                            // update routesoverlay first
                            PointLatLng curr_pnt = new PointLatLngAlt(double.Parse(cell3), double.Parse(cell4));
                            if (!first_pnt_add)
                            {
                                updateRoutes(curr_pnt, last_pnt);
                            }

                            first_pnt_add = false;
                            last_pnt = curr_pnt;

                            if (command == (byte) MAVLink.MAV_CMD.DO_SET_ROI)
                            {
                                pointlist.Add(new PointLatLngAlt(double.Parse(cell3), double.Parse(cell4),
                                    double.Parse(cell2) + homealt, "ROI" + (a + 1)) {color = Color.Red});
                                // do set roi is not a nav command. so we dont route through it
                                //fullpointlist.Add(pointlist[pointlist.Count - 1]);
                                GMarkerGoogle m =
                                    new GMarkerGoogle(new PointLatLng(double.Parse(cell3), double.Parse(cell4)),
                                        GMarkerGoogleType.red);
                                m.ToolTipMode = MarkerTooltipMode.Always;
                                m.ToolTipText = (a + 1).ToString();
                                m.Tag = (a + 1).ToString();

                                GMapMarkerRect mBorders = new GMapMarkerRect(m.Position);
                                {
                                    mBorders.InnerMarker = m;
                                    mBorders.Tag = "Dont draw line";
                                }

                                // check for clear roi, and hide it
                                if (m.Position.Lat != 0 && m.Position.Lng != 0)
                                {
                                    // order matters
                                    objectsoverlay.Markers.Add(m);
                                    objectsoverlay.Markers.Add(mBorders);
                                }
                            }
                            else if (command == (byte) MAVLink.MAV_CMD.LOITER_TIME ||
                                     command == (byte) MAVLink.MAV_CMD.LOITER_TURNS ||
                                     command == (byte) MAVLink.MAV_CMD.LOITER_UNLIM)
                            {
                                pointlist.Add(new PointLatLngAlt(double.Parse(cell3), double.Parse(cell4),
                                    double.Parse(cell2) + homealt, (a + 1).ToString())
                                {
                                    color = Color.LightBlue
                                });
                                fullpointlist.Add(pointlist[pointlist.Count - 1]);
                                addpolygonmarker((a + 1).ToString(), double.Parse(cell4), double.Parse(cell3),
                                    double.Parse(cell2), Color.LightBlue);
                            }
                            else if (command == (byte) MAVLink.MAV_CMD.SPLINE_WAYPOINT)
                            {
                                pointlist.Add(new PointLatLngAlt(double.Parse(cell3), double.Parse(cell4),
                                    double.Parse(cell2) + homealt, (a + 1).ToString()) {Tag2 = "spline"});
                                fullpointlist.Add(pointlist[pointlist.Count - 1]);
                                addpolygonmarker((a + 1).ToString(), double.Parse(cell4), double.Parse(cell3),
                                    double.Parse(cell2), Color.White);
                            }
                            else
                            {
                                pointlist.Add(new PointLatLngAlt(double.Parse(cell3), double.Parse(cell4),
                                    double.Parse(cell2) + homealt, (a + 1).ToString()));
                                fullpointlist.Add(pointlist[pointlist.Count - 1]);
                                addpolygonmarker((a + 1).ToString(), double.Parse(cell4), double.Parse(cell3),
                                    double.Parse(cell2), null);
                            }

                            avglong += double.Parse(Commands.Rows[a].Cells[Lon.Index].Value.ToString());
                            avglat += double.Parse(Commands.Rows[a].Cells[Lat.Index].Value.ToString());
                            usable++;

                            maxlong = Math.Max(double.Parse(Commands.Rows[a].Cells[Lon.Index].Value.ToString()), maxlong);
                            maxlat = Math.Max(double.Parse(Commands.Rows[a].Cells[Lat.Index].Value.ToString()), maxlat);
                            minlong = Math.Min(double.Parse(Commands.Rows[a].Cells[Lon.Index].Value.ToString()), minlong);
                            minlat = Math.Min(double.Parse(Commands.Rows[a].Cells[Lat.Index].Value.ToString()), minlat);

                            Debug.WriteLine(temp - Stopwatch.GetTimestamp());
                        }
                        else if (command == (byte) MAVLink.MAV_CMD.DO_JUMP) // fix do jumps into the future
                        {
                            pointlist.Add(null);

                            int wpno = int.Parse(Commands.Rows[a].Cells[Param1.Index].Value.ToString());
                            int repeat = int.Parse(Commands.Rows[a].Cells[Param2.Index].Value.ToString());

                            List<PointLatLngAlt> list = new List<PointLatLngAlt>();

                            // cycle through reps
                            for (int repno = repeat; repno > 0; repno--)
                            {
                                // cycle through wps
                                for (int no = wpno; no <= a; no++)
                                {
                                    if (pointlist[no] != null)
                                        list.Add(pointlist[no]);
                                }
                            }

                            fullpointlist.AddRange(list);
                        }
                        else
                        {
                            pointlist.Add(null);
                        }
                    }
                    catch (Exception e)
                    {
                        log.Info("writekml - bad wp data " + e);
                    }
                }

                if (usable > 0)
                {
                    avglat = avglat/usable;
                    avglong = avglong/usable;
                    double latdiff = maxlat - minlat;
                    double longdiff = maxlong - minlong;
                    float range = 4000;

                    Locationwp loc1 = new Locationwp();
                    loc1.lat = (minlat);
                    loc1.lng = (minlong);
                    Locationwp loc2 = new Locationwp();
                    loc2.lat = (maxlat);
                    loc2.lng = (maxlong);

                    //double distance = getDistance(loc1, loc2);  // same code as ardupilot
                    double distance = 2000;

                    if (usable > 1)
                    {
                        range = (float) (distance*2);
                    }
                    else
                    {
                        range = 4000;
                    }

                    if (avglong != 0 && usable < 3)
                    {
                        // no autozoom
                        lookat = "<LookAt>     <longitude>" + (minlong + longdiff/2).ToString(new CultureInfo("en-US")) +
                                 "</longitude>     <latitude>" + (minlat + latdiff/2).ToString(new CultureInfo("en-US")) +
                                 "</latitude> <range>" + range + "</range> </LookAt>";
                        //MainMap.ZoomAndCenterMarkers("objects");
                        //MainMap.Zoom -= 1;
                        //MainMap_OnMapZoomChanged();
                    }
                }
                else if (home.Length > 5 && usable == 0)
                {
                    lookat = "<LookAt>     <longitude>" + TXT_homelng.Text.ToString(new CultureInfo("en-US")) +
                             "</longitude>     <latitude>" + TXT_homelat.Text.ToString(new CultureInfo("en-US")) +
                             "</latitude> <range>4000</range> </LookAt>";

                    RectLatLng? rect = MainMap.GetRectOfAllMarkers("objects");
                    if (rect.HasValue)
                    {
                        MainMap.Position = rect.Value.LocationMiddle;
                    }

                    //MainMap.Zoom = 17;

                    MainMap_OnMapZoomChanged();
                }


                RegenerateWPRoute(fullpointlist);

                if (fullpointlist.Count > 0)
                {
                    double homedist = 0;

                    if (home.Length > 5)
                    {
                        homedist = MainMap.MapProvider.Projection.GetDistance(fullpointlist[fullpointlist.Count - 1],
                            fullpointlist[0]);
                    }

                    double dist = 0;

                    for (int a = 1; a < fullpointlist.Count - 1; a++)
                    // for (int a = 1; a < fullpointlist.Count - 1; a++) for last pnt is always home, so not count it into total dist
                    {
                        if (fullpointlist[a - 1] == null)
                            continue;

                        if (fullpointlist[a] == null)
                            continue;

                        dist += MainMap.MapProvider.Projection.GetDistance(fullpointlist[a - 1], fullpointlist[a]);
                    }

                    lbl_distance.Text = rm.GetString("lbl_distance.Text") + ": " +
                                        FormatDistance(dist + homedist, false);
                    System.TimeSpan time = System.TimeSpan.FromSeconds((dist + homedist) * 1000.0f / fly_vel_max);
                    lbl_ftime.Text = "预计航时: " + time.ToString(@"hh\:mm\:ss") ;
                }

                setgradanddistandaz();
            }
            catch (Exception ex)
            {
                log.Info(ex.ToString());
            }

            Debug.WriteLine(DateTime.Now);
        }

        private bool AvoidPointsAdd(ref List<Locationwp> wproute)
        {
            bool avoid_valid = false;
            // each of line in fullpoint will be tried to cross all avoid points, then update full points list, then do this again with next of those avoid points
            var cmds = GetCommandList();

            // for each avoid point, generate bound and display it in mainmap
            for (int wpIdx = 0; wpIdx < (cmds.Count); wpIdx++)
            {
                // record commands
                wproute.Add(cmds[wpIdx]);

                PointLatLng startPnt = PointLatLng.Empty;
                PointLatLng endPnt = PointLatLng.Empty;
                double avoid_pnt_alt = 0; 

                if(cmds[wpIdx].id == (ushort)MAVLink.MAV_CMD.WAYPOINT) // this a wp
                {
                    startPnt = new PointLatLng(cmds[wpIdx].lat, cmds[wpIdx].lng);
                    avoid_pnt_alt = cmds[wpIdx].alt; // for add avoid points using
                    int endwpIdx = wpIdx + 1;
                    for (; endwpIdx < (cmds.Count); endwpIdx++)
                    {
                        if (cmds[endwpIdx].id == (ushort)MAVLink.MAV_CMD.WAYPOINT) // this a wp
                        {
                            endPnt = new PointLatLng(cmds[endwpIdx].lat, cmds[endwpIdx].lng); // get end point
                            break;
                        }
                    }
                    if(endwpIdx == (cmds.Count)) // the last point
                    {
                        break; // end
                    }
                }
                else
                {
                    continue; // find out next wp
                }

                if((startPnt == PointLatLng.Empty) || (endPnt == PointLatLng.Empty)) // make sure
                {
                    continue;
                }

                // reorder avoidBoundList via nearest avoidBound from start Pnt
                avoidBoundsList.Sort((x, y) => (distance(x.center, startPnt).CompareTo(distance(y.center, startPnt))));

                foreach (var avoidBound in avoidBoundsList)
                {
                    int cross_pnt_cnt = 0;
                    int first_cross_pnt_idx = 0;
                    int second_cross_pnt_idx = 0;

                    double dist = 0;

                    List<PointLatLng> boundListTmp = new List<PointLatLng>();
                    for (int idx = 0; idx < (avoidBound.bounds.Count); idx++)
                    {
                        boundListTmp.Add(avoidBound.bounds[idx]);
                        PointLatLng interPnt = FindLineIntersection(avoidBound.bounds[idx], avoidBound.bounds[(idx+1)% avoidBound.bounds.Count], startPnt, endPnt);
                        if(interPnt != PointLatLng.Empty) // cross now
                        {
                            if (first_cross_pnt_idx == 0)
                            {
                                // first_cross_pnt = interPnt;
                                dist = distance(startPnt, interPnt);
                                first_cross_pnt_idx = idx + 1; // in boundListTmp, should + 1

                            }
                            else
                            {
                                second_cross_pnt_idx = idx + 2; // in boundListTmp, should + 2

                            }
                            boundListTmp.Add(interPnt);
                            cross_pnt_cnt++;
                            if(cross_pnt_cnt > 2)
                            {
                                MessageBox.Show("提取穿越点错误", "错误", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                            }


                        }
                    }

                    // add interPnt to wproute
                    // first, according to first cross point, get direction(CCW or CW): near with first cross point
                    if(first_cross_pnt_idx != 0)
                    {
                        // reorder first and second according to dist with start pnt
                        if (distance(boundListTmp[first_cross_pnt_idx], startPnt) >
                            distance(boundListTmp[second_cross_pnt_idx], startPnt))
                        {
                            int tmp = second_cross_pnt_idx;
                            second_cross_pnt_idx = first_cross_pnt_idx;
                            first_cross_pnt_idx = tmp;
                        }

                        avoid_valid = true;

                        int add_pnt_cnt = 0;
                        if (distance(boundListTmp[first_cross_pnt_idx], boundListTmp[(first_cross_pnt_idx - 1 + boundListTmp.Count) % boundListTmp.Count]) >
                            distance(boundListTmp[first_cross_pnt_idx], boundListTmp[(first_cross_pnt_idx + 1) % boundListTmp.Count]))
                        {


                            for (int pnt_idx = first_cross_pnt_idx; ;)
                            {
                                if (add_pnt_cnt++ >= 4)
                                {
                                    break;
                                }
                                Locationwp temp = new Locationwp();
                                wproute.Add(temp.Set(boundListTmp[pnt_idx].Lat, boundListTmp[pnt_idx].Lng, avoid_pnt_alt, (ushort)MAVLink.MAV_CMD.WAYPOINT));
                                pnt_idx++;
                                pnt_idx = (pnt_idx) % boundListTmp.Count;
                                // if (first_cross_pnt_idx < second_cross_pnt_idx) // normal order
                                // {
;                               // 
                                // 
                                // }
                                // else
                                // {
                                //     pnt_idx--;
                                //     pnt_idx = (pnt_idx + boundListTmp.Count) % boundListTmp.Count;
                                // }
                            }

                        }
                        else
                        {

                            for (int pnt_idx = first_cross_pnt_idx; ;)
                            {
                                if (add_pnt_cnt++ >= 4)
                                {
                                    break;
                                }
                                Locationwp temp = new Locationwp();
                                wproute.Add(temp.Set(boundListTmp[pnt_idx].Lat, boundListTmp[pnt_idx].Lng, avoid_pnt_alt, (ushort)MAVLink.MAV_CMD.WAYPOINT));
                                pnt_idx--;
                                pnt_idx = (pnt_idx + boundListTmp.Count) % boundListTmp.Count;
                                // if (first_cross_pnt_idx > second_cross_pnt_idx) // normal order, diff from above
                                // {
                                //     pnt_idx++;
                                //     pnt_idx = (pnt_idx) % boundListTmp.Count;
                                // 
                                // }
                                // else
                                // {
                                //     pnt_idx--;
                                //     pnt_idx = (pnt_idx + boundListTmp.Count) % boundListTmp.Count;
                                // }
                            }
                        }
                    }

                }

            }

            return avoid_valid;


        }

        private double distance(PointLatLng p1, PointLatLng p2)
        {
            return (new PointLatLngAlt(p1.Lat, p1.Lng, 0)).GetDistance(new PointLatLngAlt(p2.Lat, p2.Lng, 0));
        }

        private void RegenerateWPRoute(List<PointLatLngAlt> fullpointlist)
        {
            route.Clear();
            homeroute.Clear();

            polygonsoverlay.Routes.Clear();

            PointLatLngAlt lastpnt = fullpointlist[0];
            PointLatLngAlt lastpnt2 = fullpointlist[0];
            PointLatLngAlt lastnonspline = fullpointlist[0];
            List<PointLatLngAlt> splinepnts = new List<PointLatLngAlt>();
            List<PointLatLngAlt> wproute = new List<PointLatLngAlt>();

            // add home - this causeszx the spline to always have a straight finish
            fullpointlist.Add(fullpointlist[0]); // already add in AvoidPointsAdd()

            for (int a = 0; a < fullpointlist.Count; a++)
            {
                if (fullpointlist[a] == null)
                    continue;

                if (fullpointlist[a].Tag2 == "spline")
                {
                    if (splinepnts.Count == 0)
                        splinepnts.Add(lastpnt);

                    splinepnts.Add(fullpointlist[a]);
                }
                else
                {
                    if (splinepnts.Count > 0)
                    {
                        List<PointLatLng> list = new List<PointLatLng>();

                        splinepnts.Add(fullpointlist[a]);

                        Spline2 sp = new Spline2();

                        //sp._flags.segment_type = MissionPlanner.Controls.Waypoints.Spline2.SegmentType.SEGMENT_STRAIGHT;
                        //sp._flags.reached_destination = true;
                        //sp._origin = sp.pv_location_to_vector(lastpnt);
                        //sp._destination = sp.pv_location_to_vector(fullpointlist[0]);

                        // sp._spline_origin_vel = sp.pv_location_to_vector(lastpnt) - sp.pv_location_to_vector(lastnonspline);

                        sp.set_wp_origin_and_destination(sp.pv_location_to_vector(lastpnt2),
                            sp.pv_location_to_vector(lastpnt));

                        sp._flags.reached_destination = true;

                        for (int no = 1; no < (splinepnts.Count - 1); no++)
                        {
                            Spline2.spline_segment_end_type segtype =
                                Spline2.spline_segment_end_type.SEGMENT_END_STRAIGHT;

                            if (no < (splinepnts.Count - 2))
                            {
                                segtype = Spline2.spline_segment_end_type.SEGMENT_END_SPLINE;
                            }

                            sp.set_spline_destination(sp.pv_location_to_vector(splinepnts[no]), false, segtype,
                                sp.pv_location_to_vector(splinepnts[no + 1]));

                            //sp.update_spline();

                            while (sp._flags.reached_destination == false)
                            {
                                float t = 1f;
                                //sp.update_spline();
                                sp.advance_spline_target_along_track(t);
                                // Console.WriteLine(sp.pv_vector_to_location(sp.target_pos).ToString());
                                list.Add(sp.pv_vector_to_location(sp.target_pos));
                            }

                            list.Add(splinepnts[no]);
                        }

                        list.ForEach(x => { wproute.Add(x); });


                        splinepnts.Clear();

                        /*
                        MissionPlanner.Controls.Waypoints.Spline sp = new Controls.Waypoints.Spline();
                        
                        var spline = sp.doit(splinepnts, 20, lastlastpnt.GetBearing(splinepnts[0]),false);

                  
                         */

                        lastnonspline = fullpointlist[a];
                    }

                    wproute.Add(fullpointlist[a]);

                    lastpnt2 = lastpnt;
                    lastpnt = fullpointlist[a];
                }
            }
            /*

           List<PointLatLng> list = new List<PointLatLng>();
           fullpointlist.ForEach(x => { list.Add(x); });
           route.Points.AddRange(list);
           */
            // route is full need to get 1, 2 and last point as "HOME" route

            int count = wproute.Count;
            int counter = 0;
            PointLatLngAlt homepoint = new PointLatLngAlt();
            PointLatLngAlt firstpoint = new PointLatLngAlt();
            PointLatLngAlt lastpoint = new PointLatLngAlt();

            if (count > 2)
            {
                // homeroute = last, home, first
                wproute.ForEach(x =>
                {
                    counter++;
                    if (counter == 1)
                    {
                        homepoint = x;
                        return;
                    }
                    if (counter == 2)
                    {
                        firstpoint = x;
                    }
                    if (counter == count - 1)
                    {
                        lastpoint = x;
                    }
                    if (counter == count)
                    {
                        homeroute.Points.Add(lastpoint);
                        homeroute.Points.Add(homepoint);
                        homeroute.Points.Add(firstpoint);
                        return;
                    }
                    route.Points.Add(x);
                });

                homeroute.Stroke = new Pen(ThemeManager.wpRouteColor, 2);
                // if we have a large distance between home and the first/last point, it hangs on the draw of a the dashed line.
                if (homepoint.GetDistance(lastpoint) < 5000 && homepoint.GetDistance(firstpoint) < 5000)
                    homeroute.Stroke.DashStyle = DashStyle.Dash;

                polygonsoverlay.Routes.Add(homeroute);

                route.Stroke = new Pen(ThemeManager.wpRouteColor, 4);
                route.Stroke.DashStyle = DashStyle.Custom;
                polygonsoverlay.Routes.Add(route);
            }
        }


        /// <summary>
        /// used to redraw the polygon
        /// </summary>
        void RegeneratePolygon()
        {
            List<PointLatLng> polygonPoints = new List<PointLatLng>();

            if (objectsoverlay == null)
                return;

            foreach (GMapMarker m in objectsoverlay.Markers)
            {
                if (m is GMapMarkerRect)
                {
                    if (m.Tag == null)
                    {
                        m.Tag = polygonPoints.Count;
                        polygonPoints.Add(m.Position);
                    }
                }
            }

            if (wppolygon == null)
            {
                wppolygon = new GMapPolygon(polygonPoints, "polygon test");
                polygonsoverlay.Polygons.Add(wppolygon);
            }
            else
            {
                wppolygon.Points.Clear();
                wppolygon.Points.AddRange(polygonPoints);

                wppolygon.Stroke = new Pen(ThemeManager.wpRouteColor, 4);
                wppolygon.Stroke.DashStyle = DashStyle.Custom;
                wppolygon.Fill = Brushes.Transparent;

                if (polygonsoverlay.Polygons.Count == 0)
                {
                    polygonsoverlay.Polygons.Add(wppolygon);
                }
                else
                {
                    lock (thisLock)
                    {
                        MainMap.UpdatePolygonLocalPosition(wppolygon);
                    }
                }
            }
        }

        void setgradanddistandaz()
        {

            // no display for these
            return;

            int a = 0;
            PointLatLngAlt last = MainV2.comPort.MAV.cs.HomeLocation;
            foreach (var lla in pointlist)
            {
                if (lla == null)
                    continue;
                try
                {
                    if (lla.Tag != null && lla.Tag != "起航点" && !lla.Tag.Contains("ROI"))
                    {
                        double height = lla.Alt - last.Alt;
                        double distance = lla.GetDistance(last) * CurrentState.multiplierdist;
                        double grad = height / distance;

                        Commands.Rows[int.Parse(lla.Tag) - 1].Cells[Grad.Index].Value =
                            (grad * 100).ToString("0.0");

                        Commands.Rows[int.Parse(lla.Tag) - 1].Cells[Angle.Index].Value =
                            ((180.0 / Math.PI) * Math.Atan(grad)).ToString("0.0");

                        Commands.Rows[int.Parse(lla.Tag) - 1].Cells[Dist.Index].Value =
                            (lla.GetDistance(last)*CurrentState.multiplierdist).ToString("0.0");

                        Commands.Rows[int.Parse(lla.Tag) - 1].Cells[AZ.Index].Value =
                            ((lla.GetBearing(last) + 180)%360).ToString("0");

                    }
                }
                catch
                {
                }
                a++;
                last = lla;
            }
            
        }

        /// <summary>
        /// Saves a waypoint writer file
        /// </summary>
        private void savewaypoints()
        {
            using (SaveFileDialog fd = new SaveFileDialog())
            {
                fd.Filter = "Mission|*.waypoints;*.txt|Mission JSON|*.mission";
                fd.DefaultExt = ".waypoints";
                fd.FileName = wpfilename;
                DialogResult result = fd.ShowDialog();
                string file = fd.FileName;
                if (file != "")
                {
                    try
                    {
                        if (file.EndsWith(".mission"))
                        {
                            var list = GetCommandList();
                            Locationwp home = new Locationwp();
                            try
                            {
                                home.id = (ushort)MAVLink.MAV_CMD.WAYPOINT;
                                home.lat = (double.Parse(TXT_homelat.Text));
                                home.lng = (double.Parse(TXT_homelng.Text));
                                home.alt = (float.Parse(TXT_homealt.Text) / CurrentState.multiplierdist); // use saved home
                            }
                            catch { }

                            list.Insert(0, home);

                            var format = MissionFile.ConvertFromLocationwps(list, (byte)(altmode)CMB_altmode.SelectedValue);

                            MissionFile.WriteFile(file, format);
                            return;
                        }

                        StreamWriter sw = new StreamWriter(file);
                        sw.WriteLine("QGC WPL 110" + ((abs_alt_kml)?" ABS":" REL"));
                        try
                        {
                            sw.WriteLine("0\t1\t0\t16\t0\t0\t0\t0\t" +
                                         double.Parse(TXT_homelat.Text).ToString("0.000000", new CultureInfo("en-US")) +
                                         "\t" +
                                         double.Parse(TXT_homelng.Text).ToString("0.000000", new CultureInfo("en-US")) +
                                         "\t" +
                                         double.Parse(TXT_homealt.Text).ToString("0.000000", new CultureInfo("en-US")) +
                                         "\t1");
                        }
                        catch
                        {
                            sw.WriteLine("0\t1\t0\t0\t0\t0\t0\t0\t0\t0\t0\t1");
                        }
                        for (int a = 0; a < Commands.Rows.Count - 0; a++)
                        {
                            ushort mode = 0;

                            if (Commands.Rows[a].Cells[0].Value.ToString() == "UNKNOWN")
                            {
                                mode = (ushort)Commands.Rows[a].Cells[Command.Index].Tag;
                            }
                            else
                            {
                                mode =
                                (ushort)
                                    (MAVLink.MAV_CMD)
                                        Enum.Parse(typeof(MAVLink.MAV_CMD), Commands.Rows[a].Cells[Command.Index].Value.ToString());
                            }

                            sw.Write((a + 1)); // seq
                            sw.Write("\t" + 0); // current
                            sw.Write("\t" + CMB_altmode.SelectedValue); //frame 
                            sw.Write("\t" + mode);
                            sw.Write("\t" +
                                     double.Parse(Commands.Rows[a].Cells[Param1.Index].Value.ToString())
                                         .ToString("0.000000", new CultureInfo("en-US")));
                            sw.Write("\t" +
                                     double.Parse(Commands.Rows[a].Cells[Param2.Index].Value.ToString())
                                         .ToString("0.000000", new CultureInfo("en-US")));
                            sw.Write("\t" +
                                     double.Parse(Commands.Rows[a].Cells[Param3.Index].Value.ToString())
                                         .ToString("0.000000", new CultureInfo("en-US")));
                            sw.Write("\t" +
                                     double.Parse(Commands.Rows[a].Cells[Param4.Index].Value.ToString())
                                         .ToString("0.000000", new CultureInfo("en-US")));
                            sw.Write("\t" +
                                     double.Parse(Commands.Rows[a].Cells[Lat.Index].Value.ToString())
                                         .ToString("0.000000", new CultureInfo("en-US")));
                            sw.Write("\t" +
                                     double.Parse(Commands.Rows[a].Cells[Lon.Index].Value.ToString())
                                         .ToString("0.000000", new CultureInfo("en-US")));
                            sw.Write("\t" +
                                     (double.Parse(Commands.Rows[a].Cells[Alt.Index].Value.ToString())/
                                      CurrentState.multiplierdist).ToString("0.000000", new CultureInfo("en-US")));
                            sw.Write("\t" + 1);
                            sw.WriteLine("");
                        }
                        sw.Close();
                        sw = null;

                        // save split waypoints
                        int split_cnt = 1;
                        int seq_num = 0;
                        for (int a = 0; a < Commands.Rows.Count - 0; a++)
                        {
                            int command = (byte) (int) Enum.Parse(typeof (MAVLink.MAV_CMD), Commands.Rows[a].Cells[Command.Index].Value.ToString(), false);

                            if(a == 0)
                            {
                                if (command != (byte)MAVLink.MAV_CMD.TAKEOFF)
                                {
                                    if (MessageBox.Show("第一个航点不是起飞航点，航点列表可能错误，是否继续保存", "继续保存",
                                        MessageBoxButtons.OKCancel) != DialogResult.OK)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        string path_file = Path.GetDirectoryName(file);
                                        string file_name = Path.GetFileNameWithoutExtension(file);
                                        string file_split = path_file + "\\" + file_name + "_" + split_cnt + ".waypoints";
                                        split_cnt++;
                                        sw = new StreamWriter(file_split);
                                        sw.WriteLine("QGC WPL 110" + ((abs_alt_kml) ? " ABS" : " REL"));
                                        try
                                        {
                                            sw.WriteLine("0\t1\t0\t16\t0\t0\t0\t0\t" +
                                                         double.Parse(TXT_homelat.Text).ToString("0.000000", new CultureInfo("en-US")) +
                                                         "\t" +
                                                         double.Parse(TXT_homelng.Text).ToString("0.000000", new CultureInfo("en-US")) +
                                                         "\t" +
                                                         double.Parse(TXT_homealt.Text).ToString("0.000000", new CultureInfo("en-US")) +
                                                         "\t1");
                                        }
                                        catch
                                        {
                                            sw.WriteLine("0\t1\t0\t0\t0\t0\t0\t0\t0\t0\t0\t1");
                                        }
                                    }
                                }
                            }

                            // divide by takeoff
                            if (command == (byte)MAVLink.MAV_CMD.TAKEOFF)
                            {
                                if (sw != null) // full waypoint list file & RTL command sw != null
                                {
                                    MessageBox.Show("错误", "请检查航点列表，航线段" + split_cnt + "可能没有添加返航航点");
                                    return;
                                }

                                string path_file = Path.GetDirectoryName(file);
                                string file_name = Path.GetFileNameWithoutExtension(file);
                                string file_split = path_file + "\\" +file_name + "_" + split_cnt + ".waypoints";
                                split_cnt++;
                                sw = new StreamWriter(file_split);
                                sw.WriteLine("QGC WPL 110" + ((abs_alt_kml) ? " ABS" : " REL"));
                                try
                                {
                                    sw.WriteLine("0\t1\t0\t16\t0\t0\t0\t0\t" +
                                                 double.Parse(TXT_homelat.Text).ToString("0.000000", new CultureInfo("en-US")) +
                                                 "\t" +
                                                 double.Parse(TXT_homelng.Text).ToString("0.000000", new CultureInfo("en-US")) +
                                                 "\t" +
                                                 double.Parse(TXT_homealt.Text).ToString("0.000000", new CultureInfo("en-US")) +
                                                 "\t1");
                                }
                                catch
                                {
                                    sw.WriteLine("0\t1\t0\t0\t0\t0\t0\t0\t0\t0\t0\t1");
                                }
                            }


                            ushort mode = 0;

                            if (Commands.Rows[a].Cells[0].Value.ToString() == "UNKNOWN")
                            {
                                mode = (ushort)Commands.Rows[a].Cells[Command.Index].Tag;
                            }
                            else
                            {
                                mode =
                                (ushort)
                                    (MAVLink.MAV_CMD)
                                        Enum.Parse(typeof(MAVLink.MAV_CMD), Commands.Rows[a].Cells[Command.Index].Value.ToString());
                            }

                            sw.Write((seq_num + 1)); // seq
                            seq_num++;
                            sw.Write("\t" + 0); // current
                            sw.Write("\t" + CMB_altmode.SelectedValue); //frame 
                            sw.Write("\t" + mode);
                            sw.Write("\t" +
                                     double.Parse(Commands.Rows[a].Cells[Param1.Index].Value.ToString())
                                         .ToString("0.000000", new CultureInfo("en-US")));
                            sw.Write("\t" +
                                     double.Parse(Commands.Rows[a].Cells[Param2.Index].Value.ToString())
                                         .ToString("0.000000", new CultureInfo("en-US")));
                            sw.Write("\t" +
                                     double.Parse(Commands.Rows[a].Cells[Param3.Index].Value.ToString())
                                         .ToString("0.000000", new CultureInfo("en-US")));
                            sw.Write("\t" +
                                     double.Parse(Commands.Rows[a].Cells[Param4.Index].Value.ToString())
                                         .ToString("0.000000", new CultureInfo("en-US")));
                            sw.Write("\t" +
                                     double.Parse(Commands.Rows[a].Cells[Lat.Index].Value.ToString())
                                         .ToString("0.000000", new CultureInfo("en-US")));
                            sw.Write("\t" +
                                     double.Parse(Commands.Rows[a].Cells[Lon.Index].Value.ToString())
                                         .ToString("0.000000", new CultureInfo("en-US")));
                            sw.Write("\t" +
                                     (double.Parse(Commands.Rows[a].Cells[Alt.Index].Value.ToString()) /
                                      CurrentState.multiplierdist).ToString("0.000000", new CultureInfo("en-US")));
                            sw.Write("\t" + 1);
                            sw.WriteLine("");


                            // finish by RTL
                            if (command == (byte)MAVLink.MAV_CMD.RETURN_TO_LAUNCH)
                            {
                                // close
                                if (sw != null)
                                {
                                    sw.Close();
                                    sw = null;
                                }
                                seq_num = 0;
                            }
                        }

                        if(sw != null)
                            sw.Close();

                        // lbl_wpfile.Text = "Saved " + Path.GetFileName(file);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, Strings.ERROR);
                    }
                }
            }
        }

        private void SaveFile_Click(object sender, EventArgs e)
        {
            savewaypoints();
            writeKML();
        }

        private void refresh_autoTORTL_hgt()
        {
            // AB ZhaoYJ@2017-07-30 for refresh autoTORTL hgt & checked
            var cl_tmp1 = GetCommandList();

            if (read_wps_done && (cl_tmp1.Count > 0))
            {
                var takeoff_wp = cl_tmp1[0];
                var rtl_wp = cl_tmp1[cl_tmp1.Count - 1];

                if (takeoff_wp.id == (ushort)(MAVLink.MAV_CMD.TAKEOFF))
                {
                    // take off & rtl hgt
                    this.tb_autoTO_hgt.Value = (Int32)takeoff_wp.alt;
                    this.tb_RTL_hgt.Value = (Int32)(MainV2.comPort.GetParam("RTL_ALT")/100.0f);

                    // checked?
                    this.cb_en_autoTORTL.Checked = true;
                }
                else
                {
                    this.cb_en_autoTORTL.Checked = false;
                }

                // clear
                read_wps_done = false;
            }
            else
            {
                this.cb_en_autoTORTL.Checked = false;
            }
        }

        /// <summary>
        /// Reads the EEPROM from a com port
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BUT_read_Click(object sender, EventArgs e)
        {
            // waypoint first
            if (Commands.Rows.Count > 0)
            {
                if (sender is FlightData)
                {
                }
                else
                {
                    if (
                        MessageBox.Show("从飞控读取航点会清空当前界面的航线，是否继续", "警告",
                            MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
                    {
                        return;
                    }
                }
            }

            ProgressReporterDialogue frmProgressReporter = new ProgressReporterDialogue
            {
                StartPosition = FormStartPosition.CenterScreen,
                Text = "从飞控加载航点"
            };

            frmProgressReporter.DoWork += getWPs;
            frmProgressReporter.UpdateProgressAndStatus(-1, "从飞控加载航点");

            ThemeManager.ApplyThemeTo(frmProgressReporter);

            frmProgressReporter.RunBackgroundOperationAsync();

            frmProgressReporter.Dispose();

            // rally point then
            getRallyPointsToolStripMenuItem_Click(sender, e);

            refresh_autoTORTL_hgt();
        }

        void getWPs(object sender, ProgressWorkerEventArgs e, object passdata = null)
        {
            List<Locationwp> cmds = new List<Locationwp>();

            try
            {
                MAVLinkInterface port = MainV2.comPort;

                if (!port.BaseStream.IsOpen)
                {
                    throw new Exception("请首先连接飞控!");
                }

                MainV2.comPort.giveComport = true;

                param = port.MAV.param;

                log.Info("Getting Home");

                ((ProgressReporterDialogue) sender).UpdateProgressAndStatus(0, "Getting WP count");

                if (port.MAV.apname == MAVLink.MAV_AUTOPILOT.PX4)
                {
                    try
                    {
                        cmds.Add(port.getHomePosition());
                    }
                    catch (TimeoutException)
                    {
                        // blank home
                        cmds.Add(new Locationwp() { id = (ushort)MAVLink.MAV_CMD.WAYPOINT });
                    }
                }

                log.Info("Getting WP #");

                int cmdcount = port.getWPCount();

                for (ushort a = 0; a < cmdcount; a++)
                {
                    if (((ProgressReporterDialogue) sender).doWorkArgs.CancelRequested)
                    {
                        ((ProgressReporterDialogue) sender).doWorkArgs.CancelAcknowledged = true;
                        throw new Exception("Cancel Requested");
                    }

                    log.Info("Getting WP" + a);
                    ((ProgressReporterDialogue) sender).UpdateProgressAndStatus(a*100/cmdcount, "获取航点 " + a);
                    Locationwp tmp_loc = port.getWP(a);
                    if((tmp_loc.options & 0x1) == 0)
                    {
                        // abs_alt_kml = true;
                    }

                    cmds.Add(tmp_loc);
                }

                port.setWPACK();

                ((ProgressReporterDialogue) sender).UpdateProgressAndStatus(100, "完成");
                read_wps_done = true;
                log.Info("Done");
            }
            catch
            {
                throw;
            }

            WPtoScreen(cmds);
        }

        public void WPtoScreen(List<Locationwp> cmds, bool withrally = true)
        {
            try
            {
                Invoke((MethodInvoker) delegate
                {
                    try
                    {
                        log.Info("Process " + cmds.Count);
                        processToScreen(cmds);
                    }
                    catch (Exception exx)
                    {
                        log.Info(exx.ToString());
                    }

                    try
                    {
                        if (withrally && MainV2.comPort.MAV.param.ContainsKey("RALLY_TOTAL") &&
                            int.Parse(MainV2.comPort.MAV.param["RALLY_TOTAL"].ToString()) >= 1)
                            getRallyPointsToolStripMenuItem_Click(null, null);
                    }
                    catch
                    {
                    }

                    MainV2.comPort.giveComport = false;

                    BUT_read.Enabled = true;

                    writeKML();
                });
            }
            catch (Exception exx)
            {
                log.Info(exx.ToString());
            }
        }

        /// <summary>
        /// Writes the mission from the datagrid and values to the EEPROM
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BUT_write_Click(object sender, EventArgs e)
        {
#if EN_ALT_MODE
            if ((altmode) CMB_altmode.SelectedValue == altmode.Absolute)
            {
                if (DialogResult.No ==
                    MessageBox.Show("Absolute Alt is selected are you sure?", "Alt Mode", MessageBoxButtons.YesNo))
                {
                    CMB_altmode.SelectedValue = (int) altmode.Relative;
                }
            }
#endif
            // check home
            Locationwp home = new Locationwp();
            try
            {
                home.id = (ushort)MAVLink.MAV_CMD.WAYPOINT;
                home.lat = (double.Parse(TXT_homelat.Text));
                home.lng = (double.Parse(TXT_homelng.Text));
                home.alt = (float.Parse(TXT_homealt.Text) / CurrentState.multiplierdist); // use saved home
            }
            catch
            {
                MessageBox.Show("飞机坐标未定位，无法下发航点", "下发错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // check for invalid grid data
            for (int a = 0; a < Commands.Rows.Count - 0; a++)
            {
                for (int b = 0; b < Commands.ColumnCount - 0; b++)
                {
                    double answer;
                    if (b >= 1 && b <= 7)
                    {
                        if (!double.TryParse(Commands[b, a].Value.ToString(), out answer))
                        {
                            MessageBox.Show("航线中有错误，请通过<航点编辑>按钮检查航点列表", "航线错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    if (TXT_altwarn.Text == "")
                        TXT_altwarn.Text = (0).ToString();

                    if (Commands.Rows[a].Cells[Command.Index].Value.ToString().Contains("UNKNOWN"))
                        continue;

                    byte cmd =
                        (byte)
                            (int)
                                Enum.Parse(typeof (MAVLink.MAV_CMD),
                                    Commands.Rows[a].Cells[Command.Index].Value.ToString(), false);

                    if (cmd < (byte) MAVLink.MAV_CMD.LAST &&
                        double.Parse(Commands[Alt.Index, a].Value.ToString()) < double.Parse(TXT_altwarn.Text))
                    {
                        if (cmd != (byte) MAVLink.MAV_CMD.TAKEOFF &&
                            cmd != (byte) MAVLink.MAV_CMD.LAND &&
                            cmd != (byte) MAVLink.MAV_CMD.RETURN_TO_LAUNCH)
                        {
                            // MessageBox.Show("Low alt on WP#" + (a + 1) + "(" + double.Parse(Commands[Alt.Index, a].Value.ToString()) + "<" + double.Parse(TXT_altwarn.Text) + 
                                                  // "\nPlease reduce the alt warning, or increase the altitude");
                            // return;
                        }
                    }
                }
            }

            ProgressReporterDialogue frmProgressReporter = new ProgressReporterDialogue
            {
                StartPosition = FormStartPosition.CenterScreen,
                Text = "航点发送"
            };

            frmProgressReporter.DoWork += saveWPs;
            frmProgressReporter.UpdateProgressAndStatus(-1, "发送航点 ");

            ThemeManager.ApplyThemeTo(frmProgressReporter);

            frmProgressReporter.RunBackgroundOperationAsync();

            frmProgressReporter.Dispose();

            MainMap.Focus();

            // rally point then
            saveRallyPointsToolStripMenuItem_Click(sender, e);
        }

        Locationwp DataViewtoLocationwp(int a)
        {
            try
            {
                Locationwp temp = new Locationwp();
                if (Commands.Rows[a].Cells[Command.Index].Value.ToString().Contains("UNKNOWN"))
                {
                    temp.id = (ushort)Commands.Rows[a].Cells[Command.Index].Tag;
                }
                else
                {
                    temp.id =
                        (ushort) 
                            (int)
                                Enum.Parse(typeof (MAVLink.MAV_CMD),
                                    Commands.Rows[a].Cells[Command.Index].Value.ToString(),
                                    false);
                }
                temp.p1 = float.Parse(Commands.Rows[a].Cells[Param1.Index].Value.ToString());

                temp.alt =
                    (float)
                        (double.Parse(Commands.Rows[a].Cells[Alt.Index].Value.ToString())/CurrentState.multiplierdist);
                temp.lat = (double.Parse(Commands.Rows[a].Cells[Lat.Index].Value.ToString()));
                temp.lng = (double.Parse(Commands.Rows[a].Cells[Lon.Index].Value.ToString()));

                temp.p2 = (float) (double.Parse(Commands.Rows[a].Cells[Param2.Index].Value.ToString()));
                temp.p3 = (float) (double.Parse(Commands.Rows[a].Cells[Param3.Index].Value.ToString()));
                temp.p4 = (float) (double.Parse(Commands.Rows[a].Cells[Param4.Index].Value.ToString()));

                temp.Tag = Commands.Rows[a].Cells[TagData.Index].Value;

                // for autoTO WP label
                temp.options |= (Commands.Rows[a].Cells[Lat.Index].Tag != null)?((byte)Commands.Rows[a].Cells[Lat.Index].Tag):(byte)0x0;

                return temp;
            }
            catch (Exception ex)
            {
                throw new FormatException("Invalid number on row " + (a + 1).ToString(), ex);
            }
        }

        List<Locationwp> GetCommandList()
        {
            List<Locationwp> commands = new List<Locationwp>();

            for (int a = 0; a < Commands.Rows.Count - 0; a++)
            {
                var temp = DataViewtoLocationwp(a);

                commands.Add(temp);
            }

            return commands;
        }

        Locationwp AvoidDataViewtoLocationwp(int a)
        {
            try
            {
                Locationwp temp = new Locationwp();

                if ((dataGridView_avoidPnts.Rows[a].Cells[avoidPntLat.Index].Value != null ) && !dataGridView_avoidPnts.Rows[a].Cells[avoidPntLat.Index].Value.ToString().Equals(""))
                {
                    temp.alt =
                        (float)
                            (double.Parse(dataGridView_avoidPnts.Rows[a].Cells[avoidPntRadius.Index].Value.ToString()));
                    temp.lat = (double.Parse(dataGridView_avoidPnts.Rows[a].Cells[avoidPntLat.Index].Value.ToString()));
                    temp.lng = (double.Parse(dataGridView_avoidPnts.Rows[a].Cells[avoidPntLng.Index].Value.ToString()));

                }
                return temp;
            }
            catch (Exception ex)
            {
                throw new FormatException("无效的障碍点经纬度数据： " + (a + 1).ToString());
            }
        }


        List<Locationwp> GetAvoidCommandList()
        {
            List<Locationwp> avoidcommands = new List<Locationwp>();

            for (int a = 0; a < dataGridView_avoidPnts.Rows.Count - 0; a++)
            {
                var temp = AvoidDataViewtoLocationwp(a);

                avoidcommands.Add(temp);
            }

            return avoidcommands;
        }

        void saveWPs(object sender, ProgressWorkerEventArgs e, object passdata = null)
        {
            try
            {
                MAVLinkInterface port = MainV2.comPort;

                if (!port.BaseStream.IsOpen)
                {
                    throw new Exception("请先连接飞控，再重试发送航点!");
                }

                MainV2.comPort.giveComport = true;
                int a = 0;

                // define the home point
                Locationwp home = new Locationwp();
                try
                {
                    home.id = (ushort)MAVLink.MAV_CMD.WAYPOINT;
                    home.lat = (double.Parse(TXT_homelat.Text));
                    home.lng = (double.Parse(TXT_homelng.Text));
                    home.alt = (float.Parse(TXT_homealt.Text)/CurrentState.multiplierdist); // use saved home
                }
                catch
                {
                    throw new Exception("起航点位置可能无效！");
                }

                // log
                log.Info("wps values " + MainV2.comPort.MAV.wps.Values.Count);
                log.Info("cmd rows " + (Commands.Rows.Count + 1)); // + home

                // check for changes / future mod to send just changed wp's
                if (MainV2.comPort.MAV.wps.Values.Count == (Commands.Rows.Count + 1))
                {
                    Hashtable wpstoupload = new Hashtable();

                    a = -1;

                    foreach (var item in MainV2.comPort.MAV.wps.Values)
                    {
                        // skip home
                        if (a == -1)
                        {
                            a++;
                            continue;
                        }

                        MAVLink.mavlink_mission_item_t temp = DataViewtoLocationwp(a);

                        if (temp.command == item.command &&
                            temp.x == item.x &&
                            temp.y == item.y &&
                            temp.z == item.z &&
                            temp.param1 == item.param1 &&
                            temp.param2 == item.param2 &&
                            temp.param3 == item.param3 &&
                            temp.param4 == item.param4
                            )
                        {
                            log.Info("wp match " + (a + 1));
                        }
                        else
                        {
                            log.Info("wp no match" + (a + 1));
                            wpstoupload[a] = "";
                        }

                        a++;
                    }
                }

                bool use_int = (port.MAV.cs.capabilities & MAVLink.MAV_PROTOCOL_CAPABILITY.MISSION_INT) > 0;

                // set wp total
                ((ProgressReporterDialogue)sender).UpdateProgressAndStatus(0, "Set total wps ");

                ushort totalwpcountforupload = (ushort)(Commands.Rows.Count + 1);

                if (port.MAV.apname == MAVLink.MAV_AUTOPILOT.PX4)
                {
                    totalwpcountforupload--;
                }

                port.setWPTotal(totalwpcountforupload); // + home

                // set home location - overwritten/ignored depending on firmware.
                ((ProgressReporterDialogue)sender).UpdateProgressAndStatus(0, "Set home");

                // upload from wp0
                a = 0;

                if (port.MAV.apname != MAVLink.MAV_AUTOPILOT.PX4)
                {
                    try
                    {
                        var homeans = port.setWP(home, (ushort)a, MAVLink.MAV_FRAME.GLOBAL, 0, 1, use_int);
                        if (homeans != MAVLink.MAV_MISSION_RESULT.MAV_MISSION_ACCEPTED)
                        {
                            if (homeans != MAVLink.MAV_MISSION_RESULT.MAV_MISSION_INVALID_SEQUENCE)
                            {
                                MessageBox.Show(Strings.ErrorRejectedByMAV, Strings.ERROR);
                                return;
                            }
                        }
                        a++;
                    }
                    catch (TimeoutException)
                    {
                        use_int = false;
                        // added here to prevent timeout errors
                        port.setWPTotal(totalwpcountforupload);
                        var homeans = port.setWP(home, (ushort)a, MAVLink.MAV_FRAME.GLOBAL, 0, 1, use_int);
                        if (homeans != MAVLink.MAV_MISSION_RESULT.MAV_MISSION_ACCEPTED)
                        {
                            if (homeans != MAVLink.MAV_MISSION_RESULT.MAV_MISSION_INVALID_SEQUENCE)
                            {
                                MessageBox.Show(Strings.ErrorRejectedByMAV, Strings.ERROR);
                                return;
                            }
                        }
                        a++;
                    }
                }
                else
                {
                    use_int = false;
                }

                // define the default frame.
                MAVLink.MAV_FRAME frame = MAVLink.MAV_FRAME.GLOBAL_RELATIVE_ALT;

                // get the command list from the datagrid
                var commandlist = GetCommandList();

#if AUTOTAKEOFFRTL_WHEN_SENDWP
                // AB ZhaoYJ@2017-07-29 for autoTORTL adding
                // MainV2.instance.FlightPlanner.AddCommand(cmd, p1, p2, p3, p4, x, y, z, tag);
                if (cb_en_autoTORTL.Checked)
                {
                    // add takeoff wp
                    
                    MainV2.instance.FlightPlanner.InsertCommand(1, MAVLink.MAV_CMD.TAKEOFF, 20, 0, 0, 0, 0, 0,
                        double.Parse(tb_autotakeoff_hgt.Text), null);
                    // add pre-work wp
                    // take 1st waypoint xyz, then change z to takeoff_z, then add
                    var first_wp = commandlist[0];
                    MainV2.instance.FlightPlanner.InsertCommand(2, MAVLink.MAV_CMD.WAYPOINT, 20, 0, 0, 0, first_wp.lat, first_wp.lng,
                        double.Parse(tb_autotakeoff_hgt.Text), null);
                    // add pre-RTL wp
                    // insert the last wp
                    // temply to set RTL_ALT
                    // TODO: 1. using mavlink msg in FC src code
                    //       2. auto-choose nearest old wp-line as a virtual wp-line, then keep working-hgt fly from last user wp on this virtual line, then RTL 
                    port.setParam("RTL_ALT", double.Parse(tb_autotakeoff_hgt.Text));
                    MainV2.instance.FlightPlanner.InsertCommand(commandlist.Count+1, MAVLink.MAV_CMD.RETURN_TO_LAUNCH, 0, 0, 0, 0, 0, 0,
                        double.Parse(tb_autotakeoff_hgt.Text), null);
                }
#endif
                // notify FC this is a abs-alt mission
                if(abs_alt_kml)
                {
                    currentaltmode = (altmode)MAVLink.MAV_FRAME.GLOBAL;
                }
                else
                {
                    // AB ZhaoYJ@2017-08-02
                    // hard-code altmode as relative mode
                    // TODO: get clear with altmode for extendig in future
                    currentaltmode = (altmode)MAVLink.MAV_FRAME.GLOBAL_RELATIVE_ALT;
                }

                // process commandlist to the mav
                for (a = 1; a <= commandlist.Count; a++)
                {
                    var temp = commandlist[a-1];

                    ((ProgressReporterDialogue) sender).UpdateProgressAndStatus(a*100/Commands.Rows.Count,
                        "发送航点 " + a);

                    // make sure we are using the correct frame for these commands
                    if (temp.id < (ushort)MAVLink.MAV_CMD.LAST || temp.id == (ushort)MAVLink.MAV_CMD.DO_SET_HOME)
                    {


                        var mode = currentaltmode;

                        if (mode == altmode.Terrain)
                        {
                            frame = MAVLink.MAV_FRAME.GLOBAL_TERRAIN_ALT;
                        }
                        else if (mode == altmode.Absolute)
                        {
                            frame = MAVLink.MAV_FRAME.GLOBAL;
                        }
                        else
                        {
                            frame = MAVLink.MAV_FRAME.GLOBAL_RELATIVE_ALT;
                        }
                    }

                    // handle current wp upload number
                    int uploadwpno = a;
                    if (port.MAV.apname == MAVLink.MAV_AUTOPILOT.PX4)
                        uploadwpno--;



                    // AB ZhaoYJ@2017-06-12 for correct map offset
                    log.Info("before correct WP" + uploadwpno + ": <" + temp.lat + ", " + temp.lng + ">");
                    temp.lat -= currentOffset.Lat;
                    temp.lng -= currentOffset.Lng;
                    log.Info("after correct WP" + uploadwpno + ": <" + temp.lat + ", " + temp.lng + ">, offset: <" + currentOffset.Lat + ", " + currentOffset.Lng + ">");

                    // AB ZhaoYJ@2017-06-12 for auto correct map offset
#if ENABLE_MAP_AUTO_CORR
                    if(map_corr_auto.Checked)
                    {
                        log.Info("before auto correct WP" + uploadwpno + ": <" + temp.lat + ", " + temp.lng + ">");
                        double lat_in = temp.lat;
                        double lng_in = temp.lng;
                        double[] pos = { 0.0, 0.0 };
                        map_corr_auto_action(lat_in, lng_in, pos);
                        temp.lat = pos[0];
                        temp.lng = pos[1];
                        log.Info("after auto correct WP" + uploadwpno + ": <" + temp.lat + ", " + temp.lng + ">");
                    }
#endif
                    // try send the wp
                    MAVLink.MAV_MISSION_RESULT ans = port.setWP(temp, (ushort)(uploadwpno), frame, 0, 1, use_int);

                    // we timed out while uploading wps/ command wasnt replaced/ command wasnt added
                    if (ans == MAVLink.MAV_MISSION_RESULT.MAV_MISSION_ERROR)
                    {
                        // resend for partial upload
                        port.setWPPartialUpdate((ushort) (uploadwpno), totalwpcountforupload);
                        // reupload this point.
                        ans = port.setWP(temp, (ushort) (uploadwpno), frame, 0, 1, use_int);
                    }

                    if (ans == MAVLink.MAV_MISSION_RESULT.MAV_MISSION_NO_SPACE)
                    {
                        e.ErrorMessage = "Upload failed, please reduce the number of wp's";
                        return;
                    }
                    if (ans == MAVLink.MAV_MISSION_RESULT.MAV_MISSION_INVALID)
                    {
                        e.ErrorMessage =
                            "Upload failed, mission was rejected byt the Mav,\n item had a bad option wp# " + a + " " +
                            ans;
                        return;
                    }
                    if (ans == MAVLink.MAV_MISSION_RESULT.MAV_MISSION_INVALID_SEQUENCE)
                    {
                        // invalid sequence can only occur if we failed to see a response from the apm when we sent the request.
                        // or there is io lag and we send 2 mission_items and get 2 responces, one valid, one a ack of the second send
                        
                        // the ans is received via mission_ack, so we dont know for certain what our current request is for. as we may have lost the mission_request

                        // get requested wp no - 1;
                        a = port.getRequestedWPNo() - 1;

                        continue;
                    }
                    if (ans != MAVLink.MAV_MISSION_RESULT.MAV_MISSION_ACCEPTED)
                    {
                        e.ErrorMessage = "Upload wps failed " + Enum.Parse(typeof (MAVLink.MAV_CMD), temp.id.ToString()) +
                                         " " + Enum.Parse(typeof (MAVLink.MAV_MISSION_RESULT), ans.ToString());
                        return;
                    }
                }

                port.setWPACK();

                ((ProgressReporterDialogue) sender).UpdateProgressAndStatus(95, "Setting params");

                // m
                // port.setParam("WP_RADIUS", float.Parse(TXT_WPRad.Text)/CurrentState.multiplierdist);

                // cm's
                port.setParam("WPNAV_RADIUS", float.Parse(TXT_WPRad.Text)/CurrentState.multiplierdist*100.0);

                try
                {
                    port.setParam(new[] {"LOITER_RAD", "WP_LOITER_RAD"},
                        float.Parse(TXT_loiterrad.Text)/CurrentState.multiplierdist);
                }
                catch
                {
                }

                ((ProgressReporterDialogue) sender).UpdateProgressAndStatus(100, "Done.");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MainV2.comPort.giveComport = false;
                throw;
            }

            MainV2.comPort.giveComport = false;
        }

        /// <summary>
        /// Processes a loaded EEPROM to the map and datagrid
        /// </summary>
        void processToScreen(List<Locationwp> cmds, bool append = false)
        {
            quickadd = true;


            // mono fix
            Commands.CurrentCell = null;

            while (Commands.Rows.Count > 0 && !append)
                Commands.Rows.Clear();

            if (cmds.Count == 0)
            {
                quickadd = false;
                return;
            }

            Commands.SuspendLayout();
            Commands.Enabled = false;

            int i = Commands.Rows.Count - 1;
            foreach (Locationwp temp in cmds)
            {
                i++;
                //Console.WriteLine("FP processToScreen " + i);
                if (temp.id == 0 && i != 0) // 0 and not home
                    break;
                if (temp.id == 255 && i != 0) // bad record - never loaded any WP's - but have started the board up.
                    break;
                if (i == 0 && append) // we dont want to add home again.
                    continue;
                if (i + 1 >= Commands.Rows.Count)
                {
                    selectedrow = Commands.Rows.Add();
                }
                //if (i == 0 && temp.alt == 0) // skip 0 home
                //  continue;
                DataGridViewTextBoxCell cell;
                DataGridViewComboBoxCell cellcmd;
                cellcmd = Commands.Rows[i].Cells[Command.Index] as DataGridViewComboBoxCell;
                cellcmd.Value = "UNKNOWN";
                cellcmd.Tag = temp.id;

                foreach (object value in Enum.GetValues(typeof (MAVLink.MAV_CMD)))
                {
                    if ((int) value == temp.id)
                    {
                        cellcmd.Value = value.ToString();
                        break;
                    }
                }

                // from ap_common.h
                if (temp.id < (ushort)MAVLink.MAV_CMD.LAST || temp.id == (ushort)MAVLink.MAV_CMD.DO_SET_HOME)
                {
                    // check ralatice and terrain flags
                    if ((temp.options & 0x9) == 0 && i != 0)
                    {
                        CMB_altmode.SelectedValue = (int) altmode.Absolute;
                    } // check terrain flag
                    else if ((temp.options & 0x8) != 0 && i != 0)
                    {
                        CMB_altmode.SelectedValue = (int) altmode.Terrain;
                    } // check relative flag
                    else if ((temp.options & 0x1) != 0 && i != 0)
                    {
                        CMB_altmode.SelectedValue = (int) altmode.Relative;
                    }
                }

                cell = Commands.Rows[i].Cells[Alt.Index] as DataGridViewTextBoxCell;
                cell.Value = temp.alt*CurrentState.multiplierdist;
                cell = Commands.Rows[i].Cells[Lat.Index] as DataGridViewTextBoxCell;
                cell.Value = temp.lat;
                cell.Tag = temp.options; // add for tag autoTO WP

                cell = Commands.Rows[i].Cells[Lon.Index] as DataGridViewTextBoxCell;
                cell.Value = temp.lng;

                cell = Commands.Rows[i].Cells[Param1.Index] as DataGridViewTextBoxCell;
                cell.Value = temp.p1;
                cell = Commands.Rows[i].Cells[Param2.Index] as DataGridViewTextBoxCell;
                cell.Value = temp.p2;
                cell = Commands.Rows[i].Cells[Param3.Index] as DataGridViewTextBoxCell;
                cell.Value = temp.p3;
                cell = Commands.Rows[i].Cells[Param4.Index] as DataGridViewTextBoxCell;
                cell.Value = temp.p4;

                // convert to utm
                convertFromGeographic(temp.lat, temp.lng);
            }

            Commands.Enabled = true;
            Commands.ResumeLayout();

            setWPParams();

            try
            {
                DataGridViewTextBoxCell cellhome;
                cellhome = Commands.Rows[0].Cells[Lat.Index] as DataGridViewTextBoxCell;
                if (cellhome.Value != null)
                {
                    if (cellhome.Value.ToString() != TXT_homelat.Text && cellhome.Value.ToString() != "0")
                    {
                        // DialogResult dr = MessageBox.Show("Reset Home to loaded coords", "Reset Home Coords",
                        //     MessageBoxButtons.YesNo);
                        // 
                        // if (dr == DialogResult.Yes)
                        {
                            TXT_homelat.Text = (double.Parse(cellhome.Value.ToString())).ToString();
                            cellhome = Commands.Rows[0].Cells[Lon.Index] as DataGridViewTextBoxCell;
                            TXT_homelng.Text = (double.Parse(cellhome.Value.ToString())).ToString();
                            cellhome = Commands.Rows[0].Cells[Alt.Index] as DataGridViewTextBoxCell;
                            TXT_homealt.Text =
                                (double.Parse(cellhome.Value.ToString())*CurrentState.multiplierdist).ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            } // if there is no valid home

            if (Commands.RowCount > 0)
            {
                log.Info("remove home from list");
                Commands.Rows.Remove(Commands.Rows[0]); // remove home row
            }

            quickadd = false;

            writeKML();

            MainMap.ZoomAndCenterMarkers("objects");

            MainMap_OnMapZoomChanged();


        }

        void setWPParams()
        {
            try
            {
                log.Info("Loading wp params");

                Hashtable param = new Hashtable((Hashtable) MainV2.comPort.MAV.param);

                if (param["WP_RADIUS"] != null)
                {
                    TXT_WPRad.Text = (((double) param["WP_RADIUS"]*CurrentState.multiplierdist)).ToString();
                }
                if (param["WPNAV_RADIUS"] != null)
                {
                    TXT_WPRad.Text = (((double) param["WPNAV_RADIUS"]*CurrentState.multiplierdist/100.0)).ToString();
                }

                log.Info("param WP_RADIUS " + TXT_WPRad.Text);

                try
                {
                    TXT_loiterrad.Enabled = false;
                    if (param["LOITER_RADIUS"] != null)
                    {
                        TXT_loiterrad.Text = (((double) param["LOITER_RADIUS"]*CurrentState.multiplierdist)).ToString();
                        TXT_loiterrad.Enabled = true;
                    }
                    else if (param["WP_LOITER_RAD"] != null)
                    {
                        TXT_loiterrad.Text = (((double) param["WP_LOITER_RAD"]*CurrentState.multiplierdist)).ToString();
                        TXT_loiterrad.Enabled = true;
                    }

                    log.Info("param LOITER_RADIUS " + TXT_loiterrad.Text);
                }
                catch
                {
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        /// <summary>
        /// Saves this forms config to MAIN, where it is written in a global config
        /// </summary>
        /// <param name="write">true/false</param>
        private void config(bool write)
        {
            if (write)
            {
                Settings.Instance["TXT_homelat"] = TXT_homelat.Text;
                Settings.Instance["TXT_homelng"] = TXT_homelng.Text;
                Settings.Instance["TXT_homealt"] = TXT_homealt.Text;


                Settings.Instance["TXT_WPRad"] = TXT_WPRad.Text;

                Settings.Instance["TXT_loiterrad"] = TXT_loiterrad.Text;

                Settings.Instance["TXT_DefaultAlt"] = TXT_DefaultAlt.Text;

                // DB ZhaoYJ@2017-07-29 
                // Settings.Instance["CMB_altmode"] = CMB_altmode.Text;

                Settings.Instance["fpminaltwarning"] = TXT_altwarn.Text;

                Settings.Instance["fpcoordmouse"] = coords1.System;
            }
            else
            {
                foreach (string key in Settings.Instance.Keys)
                {
                    switch (key)
                    {
                        case "TXT_WPRad":
                            TXT_WPRad.Text = "" + Settings.Instance[key];
                            break;
                        case "TXT_loiterrad":
                            TXT_loiterrad.Text = "" + Settings.Instance[key];
                            break;
                        case "TXT_DefaultAlt":
                            TXT_DefaultAlt.Text = "" + Settings.Instance[key];
                            break;
                        case "CMB_altmode":
                            CMB_altmode.Text = "" + Settings.Instance[key];
                            break;
                        case "fpminaltwarning":
                            TXT_altwarn.Text = "" + Settings.Instance["fpminaltwarning"];
                            break;
                        case "fpcoordmouse":
                            coords1.System = "" + Settings.Instance[key];
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void TXT_WPRad_KeyPress(object sender, KeyPressEventArgs e)
        {
            float isNumber = 0;
            if (e.KeyChar.ToString() == "\b")
                return;
            e.Handled = !float.TryParse(e.KeyChar.ToString(), out isNumber);
        }

        private void TXT_WPRad_Leave(object sender, EventArgs e)
        {
            float isNumber = 0;
            if (!float.TryParse(TXT_WPRad.Text, out isNumber))
            {
                TXT_WPRad.Text = "30";
            }
            if (isNumber > (127*CurrentState.multiplierdist))
            {
                //MessageBox.Show("The value can only be between 0 and 127 m");
                //TXT_WPRad.Text = (127 * CurrentState.multiplierdist).ToString();
            }
            writeKML();
        }

        private void TXT_loiterrad_KeyPress(object sender, KeyPressEventArgs e)
        {
            float isNumber = 0;
            if (e.KeyChar.ToString() == "\b")
                return;

            if (e.KeyChar == '-')
                return;

            e.Handled = !float.TryParse(e.KeyChar.ToString(), out isNumber);
        }

        private void TXT_loiterrad_Leave(object sender, EventArgs e)
        {
            float isNumber = 0;
            if (!float.TryParse(TXT_loiterrad.Text, out isNumber))
            {
                TXT_loiterrad.Text = "45";
            }
        }

        private void TXT_DefaultAlt_KeyPress(object sender, KeyPressEventArgs e)
        {
            float isNumber = 0;
            if (e.KeyChar.ToString() == "\b")
                return;
            e.Handled = !float.TryParse(e.KeyChar.ToString(), out isNumber);
        }

        private void TXT_DefaultAlt_Leave(object sender, EventArgs e)
        {
            float isNumber = 0;
            if (!float.TryParse(TXT_DefaultAlt.Text, out isNumber))
            {
                TXT_DefaultAlt.Text = "100";
            }
        }


        /// <summary>
        /// used to control buttons in the datagrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Commands_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                if (e.ColumnIndex == Delete.Index && (e.RowIndex + 0) < Commands.RowCount) // delete
                {
                    quickadd = true;
                    // mono fix
                    Commands.CurrentCell = null;
                    Commands.Rows.RemoveAt(e.RowIndex);
                    quickadd = false;
                    writeKML();
                }
                if (e.ColumnIndex == Up.Index && e.RowIndex != 0) // up
                {
                    DataGridViewRow myrow = Commands.CurrentRow;
                    Commands.Rows.Remove(myrow);
                    Commands.Rows.Insert(e.RowIndex - 1, myrow);
                    writeKML();
                }
                if (e.ColumnIndex == Down.Index && e.RowIndex < Commands.RowCount - 1) // down
                {
                    DataGridViewRow myrow = Commands.CurrentRow;
                    Commands.Rows.Remove(myrow);
                    Commands.Rows.Insert(e.RowIndex + 1, myrow);
                    writeKML();
                }
                setgradanddistandaz();
            }
            catch (Exception)
            {
                MessageBox.Show("航点信息错误！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Commands_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells[Delete.Index].Value = "X";
            e.Row.Cells[Up.Index].Value = Resources.up;
            e.Row.Cells[Down.Index].Value = Resources.down;
        }

        private void AvoidCommands_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                if (e.ColumnIndex == Manage.Index && (e.RowIndex + 0) < dataGridView_avoidPnts.RowCount) // delete
                {
                    // mono fix
                    dataGridView_avoidPnts.CurrentCell = null;
                    dataGridView_avoidPnts.Rows.RemoveAt(e.RowIndex);
                    updateAvoidPntsRowNumbers();
                }

            }
            catch (Exception)
            {
                MessageBox.Show("障碍点错误！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AvoidCommands_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells[Delete.Index].Value = "X";
            e.Row.Cells[Up.Index].Value = Resources.up;
            e.Row.Cells[Down.Index].Value = Resources.down;
        }

        private void TXT_homelat_TextChanged(object sender, EventArgs e)
        {
            sethome = false;
            try
            {
                MainV2.comPort.MAV.cs.HomeLocation.Lat = double.Parse(TXT_homelat.Text);
            }
            catch
            {
            }
            writeKML();
        }

        private void TXT_homelng_TextChanged(object sender, EventArgs e)
        {
            sethome = false;
            try
            {
                MainV2.comPort.MAV.cs.HomeLocation.Lng = double.Parse(TXT_homelng.Text);
            }
            catch
            {
            }
            writeKML();
        }

        private void TXT_homealt_TextChanged(object sender, EventArgs e)
        {
            sethome = false;
            try
            {
                MainV2.comPort.MAV.cs.HomeLocation.Alt = double.Parse(TXT_homealt.Text);
            }
            catch
            {
            }
            writeKML();
        }

        private void BUT_loadwpfile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog fd = new OpenFileDialog())
            {
                fd.Filter = "All Supported Types|*.txt;*.waypoints;*.shp;*.mission";
                DialogResult result = fd.ShowDialog();
                string file = fd.FileName;

                if (File.Exists(file))
                {
                    if (file.ToLower().EndsWith(".shp"))
                    {
                        LoadSHPFile(file);
                    }
                    else
                    {
                        string line = "";
                        using (var fs = File.OpenText(file))
                        {
                            line = fs.ReadLine();
                        }

                        if (line.StartsWith("{"))
                        {
                            var format = MissionFile.ReadFile(file);

                            var cmds = MissionFile.ConvertToLocationwps(format);

                            processToScreen(cmds);

                            writeKML();

                            MainMap.ZoomAndCenterMarkers("objects");
                        }
                        else
                        {
                            wpfilename = file;
                            readQGC110wpfile(file);
                        }
                    }

                    // lbl_wpfile.Text = "Loaded " + Path.GetFileName(file);
                }
            }
        }

        private void BUT_planning_Click(object sender, EventArgs e)
        {
            drawnpolygonsoverlay.Markers.Clear();
            drawnpolygonsoverlay.Polygons.Clear();
            drawnpolygon.Points.Clear();
            polygons_splited.Clear();
            addPolygonPointToolStripMenuItem_Click(sender, e);
        }

        // send MainV2.instance.FlightPlanner.drawnpolygon.Points to GridUI
        private void BUT_generate_Click(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem item in this.autoWPToolStripMenuItem.DropDownItems)
            {
                if (item.Name == "GridGenerate")
                {
                    item.PerformClick();
                    break;
                }
            }
            
        }

        private void genWPsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem item in this.autoWPToolStripMenuItem.DropDownItems)
            {
                if (item.Name == "GridGenerate")
                {
                    item.PerformClick();
                    break;
                }
            }
            
        }

        public void readQGC110wpfile(string file, bool append = false)
        {
            int wp_count = 0;
            bool error = false;
            List<Locationwp> cmds = new List<Locationwp>();

            try
            {
                StreamReader sr = new StreamReader(file); //"defines.h"
                string header = sr.ReadLine();
                if (header == null || !header.Contains("QGC WPL"))
                {
                    MessageBox.Show("航线文件中存在错误");
                    return;
                }

                if(header.Contains("ABS"))
                {
                     // abs_alt_kml = true;
                }

                while (!error && !sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    // waypoints

                    if (line.StartsWith("#"))
                        continue;

                    string[] items = line.Split(new[] {'\t', ' ', ','}, StringSplitOptions.RemoveEmptyEntries);

                    if (items.Length <= 9)
                        continue;

                    try
                    {
                        Locationwp temp = new Locationwp();
                        if (items[2] == "3")
                        {
                            // abs MAV_FRAME_GLOBAL_RELATIVE_ALT=3
                            temp.options = 1;
                        }
                        else
                        {
                            temp.options = 0;
                        }
                        temp.id = (ushort)(int)Enum.Parse(typeof (MAVLink.MAV_CMD), items[3], false);
                        temp.p1 = float.Parse(items[4], new CultureInfo("en-US"));

                        if (temp.id == 99)
                            temp.id = 0;

                        temp.alt = (float) (double.Parse(items[10], new CultureInfo("en-US")));
                        temp.lat = (double.Parse(items[8], new CultureInfo("en-US")));
                        temp.lng = (double.Parse(items[9], new CultureInfo("en-US")));

                        temp.p2 = (float) (double.Parse(items[5], new CultureInfo("en-US")));
                        temp.p3 = (float) (double.Parse(items[6], new CultureInfo("en-US")));
                        temp.p4 = (float) (double.Parse(items[7], new CultureInfo("en-US")));

                        cmds.Add(temp);

                        wp_count++;
                    }
                    catch
                    {
                        MessageBox.Show("Line invalid\n" + line);
                    }
                }

                sr.Close();

                processToScreen(cmds, append);

                writeKML();

                MainMap.ZoomAndCenterMarkers("objects");
            }
            catch (Exception ex)
            {
                MessageBox.Show("文件打开错误! " + ex);
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                lock (thisLock)
                {
                    MainMap.Zoom = trackBar1.Value;
                }
            }
            catch
            {
            }
        }

        double calcpolygonarea(List<PointLatLng> polygon)
        {
            // should be a closed polygon
            // coords are in lat long
            // need utm to calc area

            if (polygon.Count == 0)
            {
                // MessageBox.Show("Please define a polygon!");
                return 0;
            }

            // close the polygon
            if (polygon[0] != polygon[polygon.Count - 1])
                polygon.Add(polygon[0]); // make a full loop

            CoordinateTransformationFactory ctfac = new CoordinateTransformationFactory();

            GeographicCoordinateSystem wgs84 = GeographicCoordinateSystem.WGS84;

            int utmzone = (int) ((polygon[0].Lng - -186.0)/6.0);

            IProjectedCoordinateSystem utm = ProjectedCoordinateSystem.WGS84_UTM(utmzone,
                polygon[0].Lat < 0 ? false : true);

            ICoordinateTransformation trans = ctfac.CreateFromCoordinateSystems(wgs84, utm);

            double prod1 = 0;
            double prod2 = 0;

            for (int a = 0; a < (polygon.Count - 1); a++)
            {
                double[] pll1 = {polygon[a].Lng, polygon[a].Lat};
                double[] pll2 = {polygon[a + 1].Lng, polygon[a + 1].Lat};

                double[] p1 = trans.MathTransform.Transform(pll1);
                double[] p2 = trans.MathTransform.Transform(pll2);

                prod1 += p1[0]*p2[1];
                prod2 += p1[1]*p2[0];
            }

            double answer = (prod1 - prod2)/2;

            if (polygon[0] == polygon[polygon.Count - 1])
                polygon.RemoveAt(polygon.Count - 1); // unmake a full loop

            return answer;
        }

        // marker
        GMapMarker currentMarker;
        PointLatLng currentWP = new PointLatLng(0.0, 0.0);
        PointLatLng currentOffset = new PointLatLng(0.0, 0.0);
        GMapMarker center = new GMarkerGoogle(new PointLatLng(0.0, 0.0), GMarkerGoogleType.none);

        // polygons
        GMapPolygon wppolygon;
        internal GMapPolygon drawnpolygon;
        int selected_poly = -1;
        GMapPolygon geofencepolygon;


        // layers
        GMapOverlay top; // not currently used
        public static GMapOverlay objectsoverlay; // where the markers a drawn
        public static GMapOverlay routesoverlay; // static so can update from gcs
        public static GMapOverlay measureoverlay; // static so can update from gcs
        public static GMapOverlay polygonsoverlay; // where the track is drawn
        public static GMapOverlay splitoverlay; // where the track is drawn
        public static GMapOverlay airportsoverlay;
        public static GMapOverlay poioverlay = new GMapOverlay("POI"); // poi layer
        GMapOverlay drawnpolygonsoverlay;
        GMapOverlay kmlpolygonsoverlay;
        GMapOverlay geofenceoverlay;
        GMapOverlay searchoverlay;
        static GMapOverlay rallypointoverlay;
        int clear_cnt = 0;

        // etc
        readonly Random rnd = new Random();
        string mobileGpsLog = string.Empty;
        GMapMarkerRect CurentRectMarker;
        GMapMarkerRallyPt CurrentRallyPt;
        GMapMarkerPOI CurrentPOIMarker;
        GMapMarker CurrentGMapMarker;
        GMarkerGoogle CurrentSplitMarker;
        bool isMouseDown;
        bool isMouseDraging;
        bool isMouseClickOffMenu;
        PointLatLng MouseDownStart;
        internal PointLatLng MouseDownEnd;

        bool is_delete_poly_point = false;

        //public long ElapsedMilliseconds;

#region -- map events --

        void MainMap_OnMarkerLeave(GMapMarker item)
        {
            if (!isMouseDown)
            {
                if (item is GMapMarkerRect)
                {
                    CurentRectMarker = null;
                    GMapMarkerRect rc = item as GMapMarkerRect;
                    rc.ResetColor();
                    MainMap.Invalidate(false);
                }
                if (item is GMapMarkerRallyPt)
                {
                    CurrentRallyPt = null;
                }
                if (item is GMapMarkerPOI)
                {
                    CurrentPOIMarker = null;
                }
                if (item is GMapMarker)
                {
                    // when you click the context menu this triggers and causes problems
                    CurrentGMapMarker = null;
                }
            }
        }

        int getWPid(int cmd_id)
        {
            int wp_id = 0;
            for (int a = 0; a < cmd_id; a++)
            {
                if(Commands.Rows[a].Cells[Command.Index].Value.ToString().Equals(MAVLink.MAV_CMD.WAYPOINT.ToString()))
                {
                    wp_id++;
                }

            }

            return wp_id;
        }

        void MainMap_OnMarkerEnter(GMapMarker item)
        {
            if (!isMouseDown)
            {
                if (item is GMapMarkerRect)
                {
                    GMapMarkerRect rc = item as GMapMarkerRect;
                    rc.Pen.Color = Color.Red;
                    MainMap.Invalidate(false);

                    int answer;
                    if (item.Tag != null && rc.InnerMarker != null &&
                        int.TryParse(rc.InnerMarker.Tag.ToString(), out answer))
                    {
                        try
                        {
                            Commands.CurrentCell = Commands[0, answer - 1];

                            // display dist until current pnt
                            // get commands lla waypoint id
                            int wp_id = getWPid(answer);
                            double dist = 0;
                            for (int a = 1; a <= wp_id; a++)
                            {
                                if (fullpointlist[a - 1] == null)
                                    continue;

                                if (fullpointlist[a] == null)
                                    continue;

                                dist += MainMap.MapProvider.Projection.GetDistance(fullpointlist[a - 1], fullpointlist[a]);
                            }

                            // double homedist = MainMap.MapProvider.Projection.GetDistance(pointlist[1], pointlist[0]);
                            // dist += homedist;

                            System.TimeSpan time = System.TimeSpan.FromSeconds((dist) * 1000.0f / fly_vel_max);
                            string str_curr_ft = "预计航时: " + time.ToString(@"hh\:mm\:ss");
                            item.ToolTipText = "高度: " + Commands[Alt.Index, answer - 1].Value + "米， 当前总航程：" + dist.ToString("0.####") + "千米" + "\n" + str_curr_ft;
                            item.ToolTipMode = MarkerTooltipMode.OnMouseOver;
                        }
                        catch
                        {
                        }
                    }

                    CurentRectMarker = rc;
                }
                if (item is GMapMarkerRallyPt)
                {
                    CurrentRallyPt = item as GMapMarkerRallyPt;
                }
                if (item is GMapMarkerAirport)
                {
                    // do nothing - readonly
                    return;
                }
                if (item is GMapMarkerPOI)
                {
                    CurrentPOIMarker = item as GMapMarkerPOI;
                }
                if (item is GMapMarkerWP)
                {
                    //CurrentGMapMarker = item;
                }
                if (item is GMapMarker)
                {
                    //CurrentGMapMarker = item;
                }
                if(item is GMarkerGoogle)
                {
                    if (polygonsplitmode)
                    {
                        GMarkerGoogle rc = item as GMarkerGoogle;
                        MainMap.Invalidate(false);
                        CurrentSplitMarker = rc;
                    }

                }
            }
        }

        // click on some marker
        void MainMap_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            int answer;
            try // when dragging item can sometimes be null
            {
                if (item.Tag == null)
                {
                    // home.. etc
                    return;
                }

                if (ModifierKeys == Keys.Control)
                {
                    try
                    {
                        groupmarkeradd(item);

                        log.Info("add marker to group");
                    }
                    catch
                    {
                    }
                }
                if (int.TryParse(item.Tag.ToString(), out answer))
                {
                    Commands.CurrentCell = Commands[0, answer - 1];
                }
            }
            catch
            {
            }
        }

        void MainMap_OnMapTypeChanged(GMapProvider type)
        {
            comboBoxMapType.SelectedItem = MainMap.MapProvider;

            MainMap.ZoomAndCenterMarkers("objects");

            if (type == WMSProvider.Instance)
            {
                string url = "";
                if (Settings.Instance["WMSserver"] != null)
                    url = Settings.Instance["WMSserver"];
                if (DialogResult.Cancel == InputBox.Show("WMS Server", "Enter the WMS server URL", ref url))
                    return;

                // Build get capability request.
                string szCapabilityRequest = BuildGetCapabilitityRequest(url);

                XmlDocument xCapabilityResponse = MakeRequest(szCapabilityRequest);
                ProcessWmsCapabilitesRequest(xCapabilityResponse);

                Settings.Instance["WMSserver"] = url;
                WMSProvider.CustomWMSURL = url;
            }
        }

        /// <summary>
        /// Builds the get Capability request.
        /// </summary>
        /// <param name="serverUrl">The server URL.</param>
        /// <returns></returns>
        private string BuildGetCapabilitityRequest(string serverUrl)
        {
            // What happens if the URL already has  '?'. 
            // For example: http://foo.com?Token=yyyy
            // In this example, the get capability request should be 
            // http://foo.com?Token=yyyy&version=1.1.0&Request=GetCapabilities&service=WMS but not
            // http://foo.com?Token=yyyy?version=1.1.0&Request=GetCapabilities&service=WMS

            // If the URL doesn't contain '?', append it.
            if (!serverUrl.Contains("?"))
            {
                serverUrl += "?";
            }
            else
            {
                // Check if the URL already has query strings.
                // If the URL doesn't have query strings, '?' comes at the end.
                if (!serverUrl.EndsWith("?"))
                {
                    // Already have query string, so add '&' before adding other query strings.
                    serverUrl += "&";
                }
            }
            return serverUrl + "version=1.1.0&Request=GetCapabilities&service=WMS";
        }

        /**
        * This function requests an XML document from a webserver.
        * @param requestUrl The request url as a string including. Example: http://129.206.228.72/cached/hillshade?Request=GetCapabilities
        * @return An XML document containing the response.
        */

        private XmlDocument MakeRequest(string requestUrl)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;


                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(response.GetResponseStream());
                return (xmlDoc);
            }
            catch (Exception e)
            {
                MessageBox.Show("WMS Server 获取信息失败: " + e.Message);
                return null;
            }
        }


        /**
         * This function parses a WMS server capabilites response.
         */

        private void ProcessWmsCapabilitesRequest(XmlDocument xCapabilitesResponse)
        {
            //Create namespace manager
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xCapabilitesResponse.NameTable);

            //check if the response is a valid xml document - if not, the server might still be able to serve us but all the checks below would fail. example: http://tiles.kartat.kapsi.fi/peruskartta
            //best sign is that there is no node WMT_MS_Capabilities
            if (xCapabilitesResponse.SelectNodes("//WMT_MS_Capabilities", nsmgr).Count == 0)
                return;


            //first, we have to make sure that the server is able to send us png imagery
            bool bPngCapable = false;
            XmlNodeList getMapElements = xCapabilitesResponse.SelectNodes("//GetMap", nsmgr);
            if (getMapElements.Count != 1)
                MessageBox.Show("无效 WMS Server 返回值");
            else
            {
                XmlNode getMapNode = getMapElements.Item(0);
                //search through all format nodes for image/png
                foreach (XmlNode formatNode in getMapNode.SelectNodes("//Format", nsmgr))
                {
                    if (formatNode.InnerText.Contains("image/png"))
                    {
                        bPngCapable = true;
                        break;
                    }
                }
            }


            if (!bPngCapable)
            {
                MessageBox.Show("无效 WMS Server 返回值: Server unable to return PNG images.");
                return;
            }


            //now search through all layer -> srs nodes for EPSG:4326 compatibility
            bool bEpsgCapable = false;
            XmlNodeList srsELements = xCapabilitesResponse.SelectNodes("//SRS", nsmgr);
            foreach (XmlNode srsNode in srsELements)
            {
                if (srsNode.InnerText.Contains("EPSG:4326"))
                {
                    bEpsgCapable = true;
                    break;
                }
            }


            if (!bEpsgCapable)
            {
                MessageBox.Show(
                    "无效 WMS Server 返回值: Server unable to return EPSG:4326 / WGS84 compatible images.");
                return;
            }


            // the server is capable of serving our requests - now check if there is a layer to be selected
            // Display layer title in the input box instead of layer name.
            // format: layer -> layer -> name
            //         layer -> layer -> title
            string szLayerSelection = "";
            int iSelect = 0;
            List<string> szListLayerName = new List<string>();
            // Loop through all layers.
            XmlNodeList layerElements = xCapabilitesResponse.SelectNodes("//Layer/Layer", nsmgr);
            foreach (XmlNode layerElement in layerElements)
            {
                // Get Name element.
                var nameNode = layerElement.SelectSingleNode("Name", nsmgr);

                // Skip if no name element is found.
                if (nameNode != null)
                {
                    var name = nameNode.InnerText;
                    // Set the default title as the layer name. 
                    var title = name;
                    // Get Title element.
                    var titleNode = layerElement.SelectSingleNode("Title", nsmgr);
                    if (titleNode != null)
                    {
                        var titleText = titleNode.InnerText;
                        if (!string.IsNullOrWhiteSpace(titleText))
                        {
                            title = titleText;
                        }
                    }
                    szListLayerName.Add(name);

                    szLayerSelection += string.Format("{0}: {1}\n ", iSelect, title);
                    //mixing control and formatting is not optimal...
                    iSelect++;
                }
            }
           
            //only select layer if there is one
            if (szListLayerName.Count != 0)
            {
                //now let the user select a layer
                string szUserSelection = "";
                if (DialogResult.Cancel ==
                    InputBox.Show("WMS Server",
                        "The following layers were detected:\n " + szLayerSelection +
                        "Please choose one by typing the associated number.", ref szUserSelection))
                    return;
                int iUserSelection = 0;
                try
                {
                    iUserSelection = Convert.ToInt32(szUserSelection);
                }
                catch
                {
                    iUserSelection = 0; //ignore all errors and default to first layer
                }

                WMSProvider.szWmsLayer = szListLayerName[iUserSelection];
            }
        }

        void groupmarkeradd(GMapMarker marker)
        {
            System.Diagnostics.Debug.WriteLine("add marker " + marker.Tag.ToString());
            groupmarkers.Add(int.Parse(marker.Tag.ToString()));
            if (marker is GMapMarkerWP)
            {
                ((GMapMarkerWP) marker).selected = true;
            }
            if (marker is GMapMarkerRect)
            {
                ((GMapMarkerWP) ((GMapMarkerRect) marker).InnerMarker).selected = true;
            }
        }

        void updateRoutes(PointLatLng new_pnt, PointLatLng old_pnt)
        {
            List<PointLatLng> segment = new List<PointLatLng>();

            segment.Add(old_pnt);
            segment.Add(new_pnt);
        
            GMapRoute seg = new GMapRoute(segment, "segment" + a.ToString());
            seg.Stroke = new Pen(Color.White, 4);
            seg.Stroke.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            seg.IsHitTestVisible = true;
            // routetotal = routetotal + (float)seg.Distance;

            routesoverlay.Routes.Add(seg);

            segment.Clear();
        }

        void MainMap_MouseUp(object sender, MouseEventArgs e)
        {
            if (isMouseClickOffMenu)
            {
                isMouseClickOffMenu = false;
                return;
            }


            // if in measure, then clear and return
            if (!startmeasure.IsEmpty && e.Button == MouseButtons.Left)
            {
                startmeasure = new PointLatLng();
                measureoverlay.Clear();
                ttpMeasure.Hide(this);
                polygonsoverlay.Markers.Clear();
                measureoverlay.Clear();
                return;
            }

            // check if the mouse up happend over our button
            if (polyicon.Rectangle.Contains(e.Location))
            {
                polyicon.IsSelected = !polyicon.IsSelected;

                if (e.Button == MouseButtons.Right)
                {
                    polyicon.IsSelected = false;
                    clearPolygonToolStripMenuItem_Click(this, null);

                    contextMenuStrip1.Visible = false;

                    return;
                }

                if (polyicon.IsSelected)
                {
                    polygongridmode = true;
                }
                else
                {
                    polygongridmode = false;
                }

                return;
            }

            MouseDownEnd = MainMap.FromLocalToLatLng(e.X, e.Y);

            // Console.WriteLine("MainMap MU");

            if (e.Button == MouseButtons.Right) // ignore right clicks
            {
                return;
            }

            if (isMouseDown) // mouse down on some other object and dragged to here.
            {
                // drag finished, update poi db
                if (CurrentPOIMarker != null)
                {
                    POI.POIMove(CurrentPOIMarker);
                    CurrentPOIMarker = null;
                }

                if (e.Button == MouseButtons.Left)
                {
                    isMouseDown = false;
                }
                if (ModifierKeys == Keys.Control)
                {
                    // group select wps
                    GMapPolygon poly = new GMapPolygon(new List<PointLatLng>(), "temp");

                    poly.Points.Add(MouseDownStart);
                    poly.Points.Add(new PointLatLng(MouseDownStart.Lat, MouseDownEnd.Lng));
                    poly.Points.Add(MouseDownEnd);
                    poly.Points.Add(new PointLatLng(MouseDownEnd.Lat, MouseDownStart.Lng));

                    foreach (var marker in objectsoverlay.Markers)
                    {
                        if (poly.IsInside(marker.Position))
                        {
                            try
                            {
                                if (marker.Tag != null)
                                {
                                    groupmarkeradd(marker);
                                }
                            }
                            catch
                            {
                            }
                        }
                    }

                    isMouseDraging = false;
                    return;
                }
                if (!isMouseDraging)
                {
                    if (CurentRectMarker != null)
                    {
                        // cant add WP in existing rect
                    }
                    else
                    {
                        // record this point
                        if(!first_wp_add && !polygongridmode && !polygonsplitmode && wpinsertmode)
                            updateRoutes(currentMarker.Position, currentWP);

                        first_wp_add = false;

                        currentWP = currentMarker.Position;
                        log.Info("更新当前航点: lat: " + currentWP.Lat + ", lng: " + currentWP.Lng + "\n");

                        AddWPToMap(currentMarker.Position.Lat, currentMarker.Position.Lng, 0);


                        // update WP info
                        this.rbtn_modall.Text = "全部航点(1-" + Commands.RowCount + ")";
                        this.num_endWP.Maximum = Commands.RowCount;
                        this.num_startWP.Maximum = Commands.RowCount;
                    }
                }
                else
                {
                    if(polygonsplitmode) // handle with split poly marker
                    {
                        if ((CurrentSplitMarker != null) && !CurrentSplitMarker.Tag.ToString().Contains("grid")) // avoid grid
                        {
                            int idx = (int.Parse(CurrentSplitMarker.Tag.ToString()));
                            PointLatLng curr_pos = new PointLatLng(MouseDownEnd.Lat, MouseDownEnd.Lng);
                            GMarkerGoogle gmm = new GMarkerGoogle(curr_pos, GMarkerGoogleType.red_big_stop);
                            gmm.Tag = CurrentSplitMarker.Tag;
                            splitoverlay.Markers[idx] = gmm;

                            if(gmm.Tag.ToString() == "0") // start
                            {
                                startsplit = curr_pos;
                            }
                            else if(gmm.Tag.ToString() == "1") // end
                            {
                                endsplit = curr_pos;
                            }

                            List<PointLatLng> polygonPoints = new List<PointLatLng>();
                            polygonPoints.Add(startsplit);
                            polygonPoints.Add(endsplit);

                            GMapPolygon line = new GMapPolygon(polygonPoints, "split poly");
                            line.Stroke.Color = Color.Red;
                            splitoverlay.Polygons.Clear();
                            splitoverlay.Polygons.Add(line);
                            MainMap.Invalidate();

                            CurrentSplitMarker = null;
                        }
            
                    }
                    else
                    {
                        if (groupmarkers.Count > 0)
                        {
                            Dictionary<string, PointLatLng> dest = new Dictionary<string, PointLatLng>();

                            foreach (var markerid in groupmarkers)
                            {
                                for (int a = 0; a < objectsoverlay.Markers.Count; a++)
                                {
                                    var marker = objectsoverlay.Markers[a];

                                    if (marker.Tag != null && marker.Tag.ToString() == markerid.ToString())
                                    {
                                        dest[marker.Tag.ToString()] = marker.Position;
                                        break;
                                    }
                                }
                            }

                            foreach (KeyValuePair<string, PointLatLng> item in dest)
                            {
                                var value = item.Value;
                                quickadd = true;
                                callMeDrag(item.Key, value.Lat, value.Lng, -1);
                                quickadd = false;
                            }

                            MainMap.SelectedArea = RectLatLng.Empty;
                            groupmarkers.Clear();
                            // redraw to remove selection
                            writeKML();

                            CurentRectMarker = null;
                        }
                        else if (CurentRectMarker != null && CurentRectMarker.InnerMarker != null)
                        {
                            if (CurentRectMarker.InnerMarker.Tag.ToString().Contains("grid"))
                            {
                                try
                                {
                                    if (drawnpolygonsoverlay.Polygons.Count != 0)
                                    {
                                        string marker_tag = "";
                                        marker_tag = CurentRectMarker.InnerMarker.Tag.ToString().Replace("grid", "");
                                        int seperator = marker_tag.IndexOf('-');
                                        // int leng = marker_tag.Length;
                                        int poly_idx = int.Parse(marker_tag.Substring(0, seperator));
                                        int pnt_idx = int.Parse(marker_tag.Substring(seperator + 1));

                                        drawnpolygonsoverlay.Polygons[poly_idx].Points[pnt_idx - 1] = new PointLatLng(MouseDownEnd.Lat, MouseDownEnd.Lng);
                                        MainMap.UpdatePolygonLocalPosition(drawnpolygonsoverlay.Polygons[poly_idx]);
                                        MainMap.Invalidate();

                                    }

                                    // drawnpolygon.Points[
                                    //     int.Parse(CurentRectMarker.InnerMarker.Tag.ToString().Replace("grid", "")) - 1] =
                                    //     new PointLatLng(MouseDownEnd.Lat, MouseDownEnd.Lng);
                                    // MainMap.UpdatePolygonLocalPosition(drawnpolygon);
                                    // MainMap.Invalidate();
                                }
                                catch
                                {
                                }
                            }
                            else
                            {
                                callMeDrag(CurentRectMarker.InnerMarker.Tag.ToString(), currentMarker.Position.Lat,
                                    currentMarker.Position.Lng, -2);
                            }
                            CurentRectMarker = null;
                        }
                        else
                        {
                            double latdif = MouseDownStart.Lat - MouseDownEnd.Lat;
                            double lngdif = MouseDownStart.Lng - MouseDownEnd.Lng;

                            try
                            {
                                lock (thisLock)
                                {
                                    if (!isMouseClickOffMenu)
                                        MainMap.Position = new PointLatLng(center.Position.Lat + latdif,
                                            center.Position.Lng + lngdif);
                                }
                            }
                            catch
                            {
                            }
                        }

                    }

                }
            }

            isMouseDraging = false;
        }

        void MainMap_MouseDown(object sender, MouseEventArgs e)
        {

            if (isMouseClickOffMenu)
                return;

            MouseDownStart = MainMap.FromLocalToLatLng(e.X, e.Y);

            //   Console.WriteLine("MainMap MD");

            if (e.Button == MouseButtons.Left && (groupmarkers.Count > 0 || ModifierKeys == Keys.Control))
            {
                // group move
                isMouseDown = true;
                isMouseDraging = false;

                return;

            }

            // if in measure, then clear and return
            if (!startmeasure.IsEmpty && e.Button == MouseButtons.Left)
            {
                startmeasure = new PointLatLng();
                measureoverlay.Clear();
                ttpMeasure.Hide(this);
                polygonsoverlay.Markers.Clear();
                measureoverlay.Clear();
                return;
            }


            if (e.Button == MouseButtons.Left && ModifierKeys != Keys.Alt && ModifierKeys != Keys.Control)
            {
                isMouseDown = true;
                isMouseDraging = false;

                if (currentMarker.IsVisible)
                {
                    currentMarker.Position = MainMap.FromLocalToLatLng(e.X, e.Y);
                }
            }
        }

        public bool PointInPolygon(PointLatLng p, List<PointLatLng> poly)
        {
            PointLatLng p1, p2;
            bool inside = false;

            if (poly.Count < 3)
            {
                return inside;
            }
            PointLatLng oldPoint = new PointLatLng(poly[poly.Count - 1].Lat, poly[poly.Count - 1].Lng);

            for (int i = 0; i < poly.Count; i++)
            {

                PointLatLng newPoint = new PointLatLng(poly[i].Lat, poly[i].Lng);

                if (newPoint.Lng > oldPoint.Lng)
                {
                    p1 = oldPoint;
                    p2 = newPoint;
                }
                else
                {
                    p1 = newPoint;
                    p2 = oldPoint;
                }

                if ((newPoint.Lng < p.Lng) == (p.Lng <= oldPoint.Lng)
                    && ((double)p.Lat - (double)p1.Lat) * (double)(p2.Lng - p1.Lng)
                    < ((double)p2.Lat - (double)p1.Lat) * (double)(p.Lng - p1.Lng))
                {
                    inside = !inside;
                }
                oldPoint = newPoint;
            }
            return inside;
        }
        // move current marker with left holding
        void MainMap_MouseMove(object sender, MouseEventArgs e)
        {

            PointLatLng point = MainMap.FromLocalToLatLng(e.X, e.Y);
            currentMousePosition = point;
            currMousePoint = new System.Drawing.Point(e.X, e.Y);

            // update selected polygon

            if (drawnpolygonsoverlay.Polygons.Count != 0)
            {
                bool selected = false;
                foreach(var gpoly in drawnpolygonsoverlay.Polygons)
                {
                    // List<PointLatLng> poly = gpoly.Points;
                    if (PointInPolygon(currentMousePosition, gpoly.Points))
                    {
                        // gpoly.Fill = Brushes.LightBlue;
                        // gpoly.Stroke = new Pen(Color.Yellow, 3);
                        drawnpolygon = gpoly;
                        selected_poly = int.Parse(drawnpolygon.Tag.ToString());
                        // var labelMarker = new GmapMarkerWithLabel(new PointLatLng(53.3, 9), "caption text", GMarkerGoogleType.blue);
                        // markerOverlay.Markers.Add(labelMarker)
                        // currentMarker.ToolTip.
                        selected = true;
                        // break;
                    }
                    else
                    {
                        gpoly.Fill = Brushes.Transparent;
                        gpoly.Stroke = new Pen(Color.Orange, 3);
                    }
                }

                if(!selected)
                {
                    selected_poly = -1;
                    // drawnpolygon.Clear();
                }
                else // highlighten
                {
                    drawnpolygon.Fill = Brushes.Transparent;
                    drawnpolygon.Stroke = new Pen(Color.LightGreen, 6);
                }
            }
            else
            {

            }

            // for measure
            // if in measure mode
            if (!startmeasure.IsEmpty)
            {
                if (e.Button == MouseButtons.Left)
                {
                    startmeasure = new PointLatLng();
                    measureoverlay.Clear();
                    ttpMeasure.Hide(this);
                    polygonsoverlay.Markers.Clear();
                    measureoverlay.Clear();
                    return;
                }

                // display dist

                ttpMeasure.IsBalloon = true;

                double meas_dist = MainMap.MapProvider.Projection.GetDistance(startmeasure, currentMousePosition);

                marker_routes = new GMapMarkerRect(currentMousePosition);
                marker_routes.ToolTip = new GMapToolTip(marker_routes);
                marker_routes.ToolTipMode = MarkerTooltipMode.Always;
                marker_routes.ToolTipText = "距离: " + ((float)meas_dist * 1000f).ToString("0.##") + " 米";
                Point adjust_pnt = new System.Drawing.Point(currMousePoint.X, currMousePoint.Y);

                ttpMeasure.Show(marker_routes.ToolTipText, this, adjust_pnt);
                List<PointLatLng> points = new List<PointLatLng>();
                points.Add(startmeasure);
                points.Add(currentMousePosition);

                GMapRoute r = new GMapRoute(points, "");
                r.Stroke = new Pen(Color.BlueViolet, 3);
                r.Stroke.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
                r.IsHitTestVisible = true;

                // not display old measure route
                if(measureoverlay.Routes.Count != 0)
                {
                    measureoverlay.Routes.Clear();
                }
                measureoverlay.Routes.Add(r);
                // MainMap.Invalidate();
                // 
            }

            if (MouseDownStart == point)
                return;

            //  Console.WriteLine("MainMap MM " + point);

            currentMarker.Position = point;

            if (!isMouseDown)
            {
                // update mouse pos display
                SetMouseDisplay(point.Lat, point.Lng, 0);
            }

            //draging
            if (e.Button == MouseButtons.Left && isMouseDown)
            {
                isMouseDraging = true;
                if (CurrentRallyPt != null)
                {
                    PointLatLng pnew = MainMap.FromLocalToLatLng(e.X, e.Y);

                    CurrentRallyPt.Position = pnew;
                }
                else if (groupmarkers.Count > 0)
                {
                    // group drag

                    double latdif = MouseDownStart.Lat - point.Lat;
                    double lngdif = MouseDownStart.Lng - point.Lng;

                    MouseDownStart = point;

                    Hashtable seen = new Hashtable();

                    foreach (var markerid in groupmarkers)
                    {
                        if (seen.ContainsKey(markerid))
                            continue;

                        seen[markerid] = 1;
                        for (int a = 0; a < objectsoverlay.Markers.Count; a++)
                        {
                            var marker = objectsoverlay.Markers[a];

                            if (marker.Tag != null && marker.Tag.ToString() == markerid.ToString())
                            {
                                var temp = new PointLatLng(marker.Position.Lat, marker.Position.Lng);
                                temp.Offset(latdif, -lngdif);
                                marker.Position = temp;
                            }
                        }
                    }
                }
                else if (CurentRectMarker != null) // left click pan
                {
                    try
                    {
                        // check if this is a grid point
                        if (CurentRectMarker.InnerMarker.Tag.ToString().Contains("grid"))
                        {

                            // first, find out this point's poly
                            if (drawnpolygonsoverlay.Polygons.Count != 0)
                            {
                                string marker_tag = "";
                                marker_tag = CurentRectMarker.InnerMarker.Tag.ToString().Replace("grid", "");
                                int seperator = marker_tag.IndexOf('-');
                                // int leng = marker_tag.Length;
                                int poly_idx = int.Parse(marker_tag.Substring(0, seperator));
                                int pnt_idx = int.Parse(marker_tag.Substring(seperator + 1));

                                drawnpolygonsoverlay.Polygons[poly_idx].Points[pnt_idx - 1] = new PointLatLng(point.Lat, point.Lng);
                                MainMap.UpdatePolygonLocalPosition(drawnpolygonsoverlay.Polygons[poly_idx]);
                                MainMap.Invalidate();
                                
                            }

                            // drawnpolygon.Points[
                            //     int.Parse(CurentRectMarker.InnerMarker.Tag.ToString().Replace("grid", "")) - 1] =
                            //     new PointLatLng(point.Lat, point.Lng);
                            // MainMap.UpdatePolygonLocalPosition(drawnpolygon);
                            // MainMap.Invalidate();
                        }
                    }
                    catch
                    {
                    }

                    PointLatLng pnew = MainMap.FromLocalToLatLng(e.X, e.Y);

                    // adjust polyline point while we drag
                    try
                    {
                        if (CurrentGMapMarker != null && CurrentGMapMarker.Tag is int)
                        {
                            int? pIndex = (int?) CurentRectMarker.Tag;
                            if (pIndex.HasValue)
                            {
                                if (pIndex < wppolygon.Points.Count)
                                {
                                    wppolygon.Points[pIndex.Value] = pnew;
                                    lock (thisLock)
                                    {
                                        MainMap.UpdatePolygonLocalPosition(wppolygon);
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                    }

                    // update rect and marker pos.
                    if (currentMarker.IsVisible)
                    {
                        currentMarker.Position = pnew;
                    }
                    CurentRectMarker.Position = pnew;

                    if (CurentRectMarker.InnerMarker != null)
                    {
                        CurentRectMarker.InnerMarker.Position = pnew;
                    }
                }
                else if (CurrentPOIMarker != null)
                {
                    PointLatLng pnew = MainMap.FromLocalToLatLng(e.X, e.Y);

                    CurrentPOIMarker.Position = pnew;
                }
                else if (CurrentGMapMarker != null)
                {
                    PointLatLng pnew = MainMap.FromLocalToLatLng(e.X, e.Y);

                    CurrentGMapMarker.Position = pnew;
                }
                else if (CurrentSplitMarker != null)
                {
                    // PointLatLng pnew = MainMap.FromLocalToLatLng(e.X, e.Y);                    
                    // CurrentSplitMarker.Position = pnew;

                    if (polygonsplitmode) // handle with split poly marker
                    {
                        // if (CurrentSplitMarker != null)
                        {
                            int idx = (int.Parse(CurrentSplitMarker.Tag.ToString()));
                            PointLatLng curr_pos = new PointLatLng(point.Lat, point.Lng);
                            GMarkerGoogle gmm = new GMarkerGoogle(curr_pos, GMarkerGoogleType.red_big_stop);
                            gmm.Tag = CurrentSplitMarker.Tag;
                            splitoverlay.Markers[idx] = gmm;

                            if (gmm.Tag.ToString() == "0") // start
                            {
                                startsplit = curr_pos;
                            }
                            else if (gmm.Tag.ToString() == "1") // end
                            {
                                endsplit = curr_pos;
                            }

                            List<PointLatLng> polygonPoints = new List<PointLatLng>();
                            polygonPoints.Add(startsplit);
                            polygonPoints.Add(endsplit);

                            GMapPolygon line = new GMapPolygon(polygonPoints, "split poly");
                            line.Stroke.Color = Color.Red;
                            splitoverlay.Polygons.Clear();
                            splitoverlay.Polygons.Add(line);
                            MainMap.Invalidate();
                            // CurrentSplitMarker = null;

                        }

                    }
                }
                else if (ModifierKeys == Keys.Control)
                {
                    // draw selection box
                    double latdif = MouseDownStart.Lat - point.Lat;
                    double lngdif = MouseDownStart.Lng - point.Lng;

                    MainMap.SelectedArea = new RectLatLng(Math.Max(MouseDownStart.Lat, point.Lat),
                        Math.Min(MouseDownStart.Lng, point.Lng), Math.Abs(lngdif), Math.Abs(latdif));
                }
                else // left click pan
                {
                    double latdif = MouseDownStart.Lat - point.Lat;
                    double lngdif = MouseDownStart.Lng - point.Lng;

                    try
                    {
                        lock (thisLock)
                        {
                            if (!isMouseClickOffMenu)
                                MainMap.Position = new PointLatLng(center.Position.Lat + latdif,
                                    center.Position.Lng + lngdif);
                        }
                    }
                    catch
                    {
                    }
                }
            }
            else if (e.Button == MouseButtons.None)
            {
                isMouseDown = false;
            }
        }

        // MapZoomChanged
        void MainMap_OnMapZoomChanged()
        {
            if (MainMap.Zoom > 0)
            {
                try
                {
                    trackBar1.Value = (int) (MainMap.Zoom);
                }
                catch
                {
                }
                //textBoxZoomCurrent.Text = MainMap.Zoom.ToString();
                center.Position = MainMap.Position;
            }
        }


        // loader start loading tiles
        void MainMap_OnTileLoadStart()
        {
            MethodInvoker m = delegate { lbl_status.Text = "Status: loading tiles..."; };
            try
            {
                if (IsHandleCreated)
                    BeginInvoke(m);
            }
            catch
            {
            }
        }

        // loader end loading tiles
        void MainMap_OnTileLoadComplete(long ElapsedMilliseconds)
        {
            //MainMap.ElapsedMilliseconds = ElapsedMilliseconds;

            MethodInvoker m = delegate
            {
                lbl_status.Text = "Status: loaded tiles";

                //panelMenu.Text = "Menu, last load in " + MainMap.ElapsedMilliseconds + "ms";

                //textBoxMemory.Text = string.Format(CultureInfo.InvariantCulture, "{0:0.00}MB of {1:0.00}MB", MainMap.Manager.MemoryCacheSize, MainMap.Manager.MemoryCacheCapacity);
            };
            try
            {
                if (!IsDisposed && IsHandleCreated)
                    BeginInvoke(m);
            }
            catch
            {
            }
        }

        // current point changed
        void MainMap_OnCurrentPositionChanged(PointLatLng point)
        {
            if (point.Lat > 90)
            {
                point.Lat = 90;
            }
            if (point.Lat < -90)
            {
                point.Lat = -90;
            }
            if (point.Lng > 180)
            {
                point.Lng = 180;
            }
            if (point.Lng < -180)
            {
                point.Lng = -180;
            }
            center.Position = point;

            coords1.Lat = point.Lat;
            coords1.Lng = point.Lng;

            // always show on planner view
            //if (MainV2.ShowAirports)
            {
                airportsoverlay.Clear();
                foreach (var item in Airports.getAirports(MainMap.Position))
                {
                    airportsoverlay.Markers.Add(new GMapMarkerAirport(item)
                    {
                        ToolTipText = item.Tag,
                        ToolTipMode = MarkerTooltipMode.OnMouseOver
                    });
                }
            }
        }

        // center markers on start
        private void MainForm_Load(object sender, EventArgs e)
        {
            if (objectsoverlay.Markers.Count > 0)
            {
                MainMap.ZoomAndCenterMarkers(null);
            }
            trackBar1.Value = (int) MainMap.Zoom;
        }

        // ensure focus on map, trackbar can have it too
        private void MainMap_MouseEnter(object sender, EventArgs e)
        {
            // MainMap.Focus();
        }

#endregion

        private void comboBoxMapType_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                MainMap.MapProvider = (GMapProvider) comboBoxMapType.SelectedItem;
                FlightData.mymap.MapProvider = (GMapProvider) comboBoxMapType.SelectedItem;
                Settings.Instance["MapType"] = comboBoxMapType.Text;
            }
            catch
            {
                MessageBox.Show("地图切换失败，请缩放后再尝试");
            }
        }

        private void Commands_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control.GetType() == typeof (DataGridViewComboBoxEditingControl))
            {
                var temp = ((ComboBox) e.Control);
                ((ComboBox) e.Control).SelectionChangeCommitted -= Commands_SelectionChangeCommitted;
                ((ComboBox) e.Control).SelectionChangeCommitted += Commands_SelectionChangeCommitted;
                ((ComboBox) e.Control).ForeColor = Color.White;
                ((ComboBox) e.Control).BackColor = Color.FromArgb(0x43, 0x44, 0x45);
                Debug.WriteLine("Setting event handle");
            }
        }
        private void AvoidCommands_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control.GetType() == typeof(DataGridViewComboBoxEditingControl))
            {
                var temp = ((ComboBox)e.Control);
                ((ComboBox)e.Control).SelectionChangeCommitted -= Commands_SelectionChangeCommitted;
                ((ComboBox)e.Control).SelectionChangeCommitted += Commands_SelectionChangeCommitted;
                ((ComboBox)e.Control).ForeColor = Color.White;
                ((ComboBox)e.Control).BackColor = Color.FromArgb(0x43, 0x44, 0x45);
                Debug.WriteLine("Setting event handle");
            }
        }

        void Commands_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // update row headers
            ((ComboBox) sender).ForeColor = Color.White;
            ChangeColumnHeader(((ComboBox) sender).Text);
            try
            {
                // default takeoff to non 0 alt
                if (((ComboBox) sender).Text == "TAKEOFF")
                {
                    if (Commands.Rows[selectedrow].Cells[Alt.Index].Value != null &&
                        Commands.Rows[selectedrow].Cells[Alt.Index].Value.ToString() == "0")
                        Commands.Rows[selectedrow].Cells[Alt.Index].Value = TXT_DefaultAlt.Text;
                }

                // default to take shot
                if (((ComboBox) sender).Text == "DO_DIGICAM_CONTROL")
                {
                    if (Commands.Rows[selectedrow].Cells[Lat.Index].Value != null &&
                        Commands.Rows[selectedrow].Cells[Lat.Index].Value.ToString() == "0")
                        Commands.Rows[selectedrow].Cells[Lat.Index].Value = (1).ToString();
                }

                if (((ComboBox)sender).Text == "UNKNOWN")
                {
                    string cmdid = "-1";
                    if (InputBox.Show("Mavlink ID", "Please enter the command ID", ref cmdid) == DialogResult.OK)
                    {
                        if (cmdid != "-1")
                        {
                            Commands.Rows[selectedrow].Cells[Command.Index].Tag = ushort.Parse(cmdid);
                        }
                    }
                }

                for (int i = 0; i < Commands.ColumnCount; i++)
                {
                    DataGridViewCell tcell = Commands.Rows[selectedrow].Cells[i];
                    if (tcell.GetType() == typeof (DataGridViewTextBoxCell))
                    {
                        if (tcell.Value.ToString() == "?")
                            tcell.Value = "0";
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Get the Google earth ALT for a given coord
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <returns>Altitude</returns>
        double getGEAlt(double lat, double lng)
        {
            double alt = 0;
            //http://maps.google.com/maps/api/elevation/xml

            try
            {
                using (
                    XmlTextReader xmlreader =
                        new XmlTextReader("http://maps.google.com/maps/api/elevation/xml?locations=" +
                                          lat.ToString(new CultureInfo("en-US")) + "," +
                                          lng.ToString(new CultureInfo("en-US")) + "&sensor=true"))
                {
                    while (xmlreader.Read())
                    {
                        xmlreader.MoveToElement();
                        switch (xmlreader.Name)
                        {
                            case "elevation":
                                alt = double.Parse(xmlreader.ReadString(), new CultureInfo("en-US"));
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch
            {
            }

            return alt*CurrentState.multiplierdist;
        }

        private void TXT_homelat_Enter(object sender, EventArgs e)
        {
            if (!sethome)
                MessageBox.Show("请在地图上设置home点");
            sethome = true;
            
        }

        private void Planner_Resize(object sender, EventArgs e)
        {
            MainMap.Zoom = trackBar1.Value;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            try
            {
                base.OnPaint(pe);
            }
            catch (Exception)
            {
            }
        }

        private void Commands_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // we have modified a utm coords
            if (e.ColumnIndex == coordZone.Index ||
                e.ColumnIndex == coordNorthing.Index ||
                e.ColumnIndex == coordEasting.Index)
            {
                convertFromUTM(e.RowIndex);
            }

            if(e.ColumnIndex == MGRS.Index)
            {
                convertFromMGRS(e.RowIndex);
            }

            // we have modified a ll coord
            if (e.ColumnIndex == Lat.Index ||
                e.ColumnIndex == Lon.Index)
            {
                try
                {
                    var lat = double.Parse(Commands.Rows[e.RowIndex].Cells[Lat.Index].Value.ToString());
                    var lng = double.Parse(Commands.Rows[e.RowIndex].Cells[Lon.Index].Value.ToString());
                    convertFromGeographic(lat, lng);
                } catch (Exception ex)
                {
                    log.Error(ex);
                    MessageBox.Show("经纬度坐标数据有误：",Strings.ERROR);
                }
            }

            Commands_RowEnter(null,
                new DataGridViewCellEventArgs(Commands.CurrentCell.ColumnIndex, Commands.CurrentCell.RowIndex));

            writeKML();
        }
        private void AvoidCommands_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // we have modified a utm coords
            if (e.ColumnIndex == coordZone.Index ||
                e.ColumnIndex == coordNorthing.Index ||
                e.ColumnIndex == coordEasting.Index)
            {
                convertFromUTM(e.RowIndex);
            }

            if (e.ColumnIndex == MGRS.Index)
            {
                convertFromMGRS(e.RowIndex);
            }

            // we have modified a ll coord
            if (e.ColumnIndex == Lat.Index ||
                e.ColumnIndex == Lon.Index)
            {
                try
                {
                    var lat = double.Parse(Commands.Rows[e.RowIndex].Cells[Lat.Index].Value.ToString());
                    var lng = double.Parse(Commands.Rows[e.RowIndex].Cells[Lon.Index].Value.ToString());
                    convertFromGeographic(lat, lng);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    MessageBox.Show("经纬度坐标数据有误：", Strings.ERROR);
                }
            }

            Commands_RowEnter(null,
                new DataGridViewCellEventArgs(Commands.CurrentCell.ColumnIndex, Commands.CurrentCell.RowIndex));

            writeKML();
        }

        private void MainMap_Resize(object sender, EventArgs e)
        {
            MainMap.Zoom = MainMap.Zoom + 0.01;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            try
            {
                lock (thisLock)
                {
                    MainMap.Zoom = trackBar1.Value;
                }
            }
            catch
            {
            }
        }


        private void BUT_Prefetch_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// from http://stackoverflow.com/questions/1119451/how-to-tell-if-a-line-intersects-a-polygon-in-c
        /// </summary>
        /// <param name="start1"></param>
        /// <param name="end1"></param>
        /// <param name="start2"></param>
        /// <param name="end2"></param>
        /// <returns></returns>
        public PointLatLng FindLineIntersection(PointLatLng start1, PointLatLng end1, PointLatLng start2,
            PointLatLng end2)
        {
            double denom = ((end1.Lng - start1.Lng)*(end2.Lat - start2.Lat)) -
                           ((end1.Lat - start1.Lat)*(end2.Lng - start2.Lng));
            //  AB & CD are parallel         
            if (denom == 0)
                return PointLatLng.Empty;
            double numer = ((start1.Lat - start2.Lat)*(end2.Lng - start2.Lng)) -
                           ((start1.Lng - start2.Lng)*(end2.Lat - start2.Lat));
            double r = numer/denom;
            double numer2 = ((start1.Lat - start2.Lat)*(end1.Lng - start1.Lng)) -
                            ((start1.Lng - start2.Lng)*(end1.Lat - start1.Lat));
            double s = numer2/denom;
            if ((r < 0 || r > 1) || (s < 0 || s > 1))
                return PointLatLng.Empty;
            // Find intersection point      
            PointLatLng result = new PointLatLng();
            result.Lng = start1.Lng + (r*(end1.Lng - start1.Lng));
            result.Lat = start1.Lat + (r*(end1.Lat - start1.Lat));
            return result;
        }

        RectLatLng getPolyMinMax(GMapPolygon poly)
        {
            if (poly.Points.Count == 0)
                return new RectLatLng();

            double minx, miny, maxx, maxy;

            minx = maxx = poly.Points[0].Lng;
            miny = maxy = poly.Points[0].Lat;

            foreach (PointLatLng pnt in poly.Points)
            {
                //Console.WriteLine(pnt.ToString());
                minx = Math.Min(minx, pnt.Lng);
                maxx = Math.Max(maxx, pnt.Lng);

                miny = Math.Min(miny, pnt.Lat);
                maxy = Math.Max(maxy, pnt.Lat);
            }

            return new RectLatLng(maxy, minx, Math.Abs(maxx - minx), Math.Abs(miny - maxy));
        }

        const float rad2deg = (float) (180/Math.PI);
        const float deg2rad = (float) (1.0/rad2deg);

        private void BUT_grid_Click(object sender, EventArgs e)
        {
        }

        private void label4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (MainV2.comPort.MAV.cs.lat != 0)
            {
                TXT_homealt.Text = (MainV2.comPort.MAV.cs.altasl).ToString("0");
                TXT_homelat.Text = MainV2.comPort.MAV.cs.lat.ToString();
                TXT_homelng.Text = MainV2.comPort.MAV.cs.lng.ToString();
                MainMap.Position = new PointLatLng(double.Parse(TXT_homelat.Text), double.Parse(TXT_homelng.Text));
            }
            else
            {
                MessageBox.Show(
                    "飞行器没有GPS坐标，请查看GPS的状态", "定位错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        /// <summary>
        /// Format distance according to prefer distance unit
        /// </summary>
        /// <param name="distInKM">distance in kilometers</param>
        /// <param name="toMeterOrFeet">convert distance to meter or feet if true, covert to km or miles if false</param>
        /// <returns>formatted distance with unit</returns>
        private string FormatDistance(double distInKM, bool toMeterOrFeet)
        {
            string sunits = Settings.Instance["distunits"];
            Common.distances units = Common.distances.Meters;

            if (sunits != null)
                try
                {
                    units = (Common.distances) Enum.Parse(typeof (Common.distances), sunits);
                }
                catch (Exception)
                {
                }

            switch (units)
            {
                case Common.distances.Feet:
                    return toMeterOrFeet
                        ? string.Format((distInKM*3280.8399).ToString("0.00 ft"))
                        : string.Format((distInKM*0.621371).ToString("0.0000 miles"));
                case Common.distances.Meters:
                default:
                    return toMeterOrFeet
                        ? string.Format((distInKM*1000).ToString("0.00 米"))
                        : string.Format(distInKM.ToString("0.0000 千米"));
            }
        }

        PointLatLng startmeasure;

        private void ContextMeasure_Click(object sender, EventArgs e)
        {
            if (startmeasure.IsEmpty)
            {
                startmeasure = MouseDownStart;
                polygonsoverlay.Markers.Add(new GMarkerGoogle(MouseDownStart, GMarkerGoogleType.arrow));
                measureoverlay.Clear();
                MainMap.Invalidate();
                Common.MessageShowAgain("Measure Dist",
                    "You can now pan/zoom around.\nClick this option again to get the distance.");
                
            }
            else
            {
                /*
                List<PointLatLng> polygonPoints = new List<PointLatLng>();
                polygonPoints.Add(startmeasure);
                polygonPoints.Add(MouseDownStart);

                GMapPolygon line = new GMapPolygon(polygonPoints, "measure dist");
                line.Stroke.Color = Color.Green;

                polygonsoverlay.Polygons.Add(line);

                polygonsoverlay.Markers.Add(new GMarkerGoogle(MouseDownStart, GMarkerGoogleType.black_small));
                MainMap.Invalidate();
                MessageBox.Show("Distance: " +
                                      FormatDistance(
                                          MainMap.MapProvider.Projection.GetDistance(startmeasure, MouseDownStart), true) +
                                      " AZ: " +
                                      (MainMap.MapProvider.Projection.GetBearing(startmeasure, MouseDownStart)
                                          .ToString("0")));
                polygonsoverlay.Polygons.Remove(line);
                polygonsoverlay.Markers.Clear();
                startmeasure = new PointLatLng();*/
            }
        }

        //若点a大于点b,即点a在点b顺时针方向,返回true,否则返回false
        public bool PointCmp(PointLatLng a, PointLatLng b, PointLatLng center)
        {
            if (a.Lat >= 0 && b.Lat < 0)
                return true;
            if (a.Lat == 0 && b.Lat == 0)
                return a.Lng > b.Lng;
            //向量OA和向量OB的叉积
            double det = (a.Lat - center.Lat) * (b.Lng - center.Lng) - (b.Lat - center.Lat) * (a.Lng - center.Lng);
            if (Math.Sign(det) < 0)
                return true;
            if (Math.Sign(det) > 0)
                return false;
            //向量OA和向量OB共线，以距离判断大小
            double d1 = (a.Lat - center.Lat) * (a.Lat - center.Lat) + (a.Lng - center.Lng) * (a.Lng - center.Lng);
            double d2 = (b.Lat - center.Lat) * (b.Lat - center.Lng) + (b.Lng - center.Lng) * (b.Lng - center.Lng);
            return d1 > d2;
        }

        public void ClockwiseSortPoints(List<PointLatLng> vPoints, bool CCW)
        {
            //计算重心
            PointLatLng center = new PointLatLng();
            double x = 0, y = 0;
            for (int i = 0; i < vPoints.Count; i++)
            {
                x += vPoints[i].Lat;
                y += vPoints[i].Lng;
            }
            center.Lat = (double)x / vPoints.Count;
            center.Lng = (double)y / vPoints.Count;

            //冒泡排序
            for (int i = 0; i < vPoints.Count - 1; i++)
            {
                for (int j = 0; j < vPoints.Count - i - 1; j++)
                {
                    if (PointCmp(vPoints[j], vPoints[j + 1], center))
                    {
                        PointLatLng tmp = vPoints[j];
                        vPoints[j] = vPoints[j + 1];
                        vPoints[j + 1] = tmp;
                    }
                }
            }

            if (CCW)
                vPoints.Reverse();
        }


        private void addPolygonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            polygongridmode = true;
            polygonsplitmode = false;
            wpinsertmode = false;
            // drawnpolygonsoverlay.Markers.Clear();
            // drawnpolygonsoverlay.Polygons.Clear();
            // drawnpolygon.Points.Clear();
            // polygons_splited.Clear();
            // wait for adding split line
        }

        private void donePolygonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            polygongridmode = false;

            // add 
            // drawnpolygonsoverlay.Polygons.Add(drawnpolygon);
        }

        PointLatLng startsplit;
        PointLatLng endsplit;
        private void polySplitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            polygonsplitmode = true;
            polygongridmode = false;
            wpinsertmode = false;
            // wait for adding split line
        }
        private void DonePolySplitToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (startsplit.IsEmpty || endsplit.IsEmpty)
            {

                startsplit = new PointLatLng();
                endsplit = new PointLatLng();
                polygonsplitmode = false;
                splitoverlay.Markers.Clear();
                splitoverlay.Polygons.Clear();
                splitoverlay.Routes.Clear();
                splitoverlay.Clear();

                MessageBox.Show("请首先在地图上插入两点作为分割线.", "作业区域分割错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                // just for test
                // TODO:
                // startsplit = MouseDownStart;

                // MessageBox.Show("Distance: " +
                //                       FormatDistance(
                //                           MainMap.MapProvider.Projection.GetDistance(startmeasure, MouseDownStart), true) +
                //                       " AZ: " +
                //                       (MainMap.MapProvider.Projection.GetBearing(startmeasure, MouseDownStart)
                //                           .ToString("0")));
                // polygonsoverlay.Polygons.Remove(line);
                // polygonsoverlay.Markers.Clear();
                // startmeasure = new PointLatLng();

                // split
                PolySplitter ps_inst = new PolySplitter();
                List<QPointF> poly_in = new List<QPointF>();
                List<List<QPointF>> poly_out = new List<List<QPointF>>();
                PointLatLngAlt pnt1 = startsplit;
                PointLatLngAlt pnt2 = endsplit;
                QPointF qp1 = new QPointF(pnt1);
                QPointF qp2 = new QPointF(pnt2);

                PolySplitter.QLineF line_in = new PolySplitter.QLineF(qp1, qp2);

                List<PointLatLng> list_poly = new List<PointLatLng>();
                List<PointLatLng> list_poly_all = new List<PointLatLng>();
                List<List<PointLatLng>> polygons_splited_tmp = new List<List<PointLatLng>>();
                polygons_splited_tmp.Clear();

                // reload drawnpoly first
                polygons_splited.Clear();
                if (drawnpolygonsoverlay.Polygons.Count != 0)
                {
                    foreach (var gpoly in drawnpolygonsoverlay.Polygons)
                    {
                        polygons_splited.Add(gpoly.Points);
                    }
                }
                else // exit this mode
                {
                    startsplit = new PointLatLng();
                    endsplit = new PointLatLng();
                    polygonsplitmode = false;
                    splitoverlay.Markers.Clear();
                    splitoverlay.Polygons.Clear();
                    splitoverlay.Routes.Clear();
                    splitoverlay.Clear();
                    MessageBox.Show("还没有绘制作业区域，请首先在地图上绘制.", "作业区域分割", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                foreach (var poly in polygons_splited)
                {
                    Polygoncalc polycalc = new Polygoncalc();
                    if(ClockDirection.Clockwise == polycalc.CalculateClockDirection(poly))
                    {
                        poly.Reverse();
                    }

                    // ClockwiseSortPoints(poly, false);
                    // poly.Sort((x, y) => (Math.Atan2(x.Lat, x.Lng)).CompareTo(Math.Atan2(x.Lat, x.Lng)));
                    // poly.Sort(SortCornersClockwise);

                    poly_in.Clear();
                    foreach (var pll in poly)
                    {
                        PointLatLngAlt pnt = pll;
                        QPointF qp = new QPointF(pnt);
                        poly_in.Add(qp);
                    }

                    // poly_out = ps_inst.Split(poly_in, line_in);
                    poly_out = ps_inst.Split2(poly_in, line_in);

                    if (poly_out.Count == 0)
                    {
                        // just add old poly
                        polygons_splited_tmp.Add(new List<PointLatLng>(poly));

                        continue;
                    }
                    // drawnpolygon.Points
                    // for makesure, redraw it

                    // 
                    foreach (var poly_item in poly_out)
                    {
                        list_poly.Clear();
                        foreach (var qp_item in poly_item)
                        {
                            PointLatLngAlt pnt = qp_item.point;
                            PointLatLng pnt_tmp = new PointLatLng(pnt.Lat, pnt.Lng);
                            list_poly.Add(pnt_tmp);
                            list_poly_all.Add(pnt_tmp);
                        }

                        polygons_splited_tmp.Add(new List<PointLatLng>(list_poly));
                    }
                }

                polygons_splited.Clear();
                polygons_splited = new List<List<PointLatLng>>(polygons_splited_tmp);
                // list_poly.Sort(SortCornersClockwise);
                // list_poly.Reverse();
                redrawPolygonList(polygons_splited);
                // MainMap.UpdatePolygonLocalPosition(drawnpolygon);
                splitoverlay.Markers.Clear();
                splitoverlay.Polygons.Clear();
                splitoverlay.Routes.Clear();
                splitoverlay.Clear();

                MainMap.Invalidate();
                startsplit = new PointLatLng();
                endsplit = new PointLatLng();

                // polygonsoverlay.Clear();
            }

            polygonsplitmode = false;
            splitoverlay.Markers.Clear();
            splitoverlay.Polygons.Clear();
            splitoverlay.Routes.Clear();
            splitoverlay.Clear();
            polygons_splited.Clear();

        }

        private void DeletePolySplitToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // if selected 
            // if((drawnpolygon != null) && (selected_poly != -1))
            if ((drawnpolygon.Points.Count > 2) && (selected_poly != -1))
            {
                // int idx = int.Parse(drawnpolygon.Tag.ToString());
                drawnpolygonsoverlay.Polygons.RemoveAt(selected_poly);
                // drawnpolygonsoverlay.Polygons
                polygons_splited.Clear();
                if (drawnpolygonsoverlay.Polygons.Count != 0)
                {
                    foreach (var gpoly in drawnpolygonsoverlay.Polygons)
                    {
                        polygons_splited.Add(gpoly.Points);
                    }
                }
                redrawPolygonList(polygons_splited);
            }

        }


        private void rotateMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string heading = "0";
            if (DialogResult.Cancel == InputBox.Show("Rotate map to heading", "Enter new UP heading", ref heading))
                return;
            float ans = 0;
            if (float.TryParse(heading, out ans))
            {
                MainMap.Bearing = ans;
            }
        }

        private int find_nearest_point_id_in_poly2D(PointLatLng pnt_in, List<PointLatLng> poly)
        {
            int id = 0;
            PointLatLngAlt pnt_lla = new PointLatLngAlt(pnt_in);
            if (poly.Count == 0)
            {
                id = 0;
            }
            else
            {
                double dist_pp = pnt_lla.GetDistance(poly[0]); //  
                for(int ii = 0; ii < poly.Count; ii++)
                {
                    PointLatLngAlt plla = new PointLatLngAlt(poly[ii]);
                    double dist_pp_tmp = pnt_lla.GetDistance(plla);
                    // near more
                    if (dist_pp.CompareTo(dist_pp_tmp) == 1)
                    {
                        dist_pp = dist_pp_tmp;
                        id = ii;
                    }

                }

            }

            return id;
        }

        public static int SortCornersClockwise(PointLatLng A, PointLatLng B)
        {
            //  Variables to Store the atans
            double aTanA, aTanB;

            //  Reference Point
            PointLatLngAlt reference = MainV2.comPort.MAV.cs.HomeLocation;

            //  Fetch the atans
            aTanA = Math.Atan2(A.Lat - reference.Lat, A.Lng - reference.Lng);
            aTanB = Math.Atan2(B.Lat - reference.Lat, B.Lng - reference.Lng);

            //  Determine next point in Clockwise rotation
            if (aTanA < aTanB) return -1;
            else if (aTanB < aTanA) return 1;
            return 0;
        }

        private void addPolygonPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (polygongridmode == false)
            {
                MessageBox.Show(
                    // "You will remain in polygon mode until you clear the polygon or create a grid/upload a fence");
                "开始绘制作业区域，您可以用鼠标左键在地图上点击画出作业区域,\n\n然后点击航线生成按钮，以进入自动航线生成模式！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // polygongridmode = true;
            polygonsplitmode = false;
            wpinsertmode = false;

            List<PointLatLng> polygonPoints = new List<PointLatLng>();
            if (drawnpolygonsoverlay.Polygons.Count == 0)
            {
                drawnpolygon.Points.Clear();
                drawnpolygonsoverlay.Polygons.Add(drawnpolygon);
            }


            drawnpolygon.Fill = Brushes.Transparent;

            // remove full loop is exists
            if (drawnpolygon.Points.Count > 1 &&
                drawnpolygon.Points[0] == drawnpolygon.Points[drawnpolygon.Points.Count - 1])
                drawnpolygon.Points.RemoveAt(drawnpolygon.Points.Count - 1); // unmake a full loop

            // AB ZhaoYJ@2017-07-31 for making sure non (0,0) point
            if((MouseDownStart.Lat == 0) && (MouseDownStart.Lng == 0))
            {
                MouseDownStart = MainV2.comPort.MAV.cs.HomeLocation;
            }
#if MP_METHOD
            drawnpolygon.Points.Add(new PointLatLng(MouseDownStart.Lat, MouseDownStart.Lng));

            addpolygonmarkergrid(drawnpolygon.Points.Count.ToString(), MouseDownStart.Lng, MouseDownStart.Lat, 0);
#else
            // try to insert this point to nearest point in drawnpolygen.points 
            // 1. find out the nearest point
            // 2. insert to drawnpolygen.points
            PointLatLng insert_pnt = new PointLatLng(MouseDownStart.Lat, MouseDownStart.Lng);
            // insert_id = 1;
            int insert_id = 1;

            if (drawnpolygon.Points.Count >= 2)
            {
                insert_id = find_nearest_point_id_in_poly2D(insert_pnt, drawnpolygon.Points) + 1;
                drawnpolygon.Points.Insert(insert_id, insert_pnt);
                // insert_pnt = new PointLatLng(35, 112);
                // drawnpolygon.Points.Add(insert_pnt);

                // drawnpolygon.Tag = 0; // initial

                // TOOD: dont know how to re-order this
                string poly_tag = "";
                if (drawnpolygon.Tag == null)
                {
                    poly_tag = "0";
                }
                else
                {
                    poly_tag = drawnpolygon.Tag.ToString();
                }

                // insertpolygonmarkergrid(insert_id, poly_tag + "-" + drawnpolygon.Points.Count.ToString(), insert_pnt.Lng, insert_pnt.Lat, 0);
                List<PointLatLng> list_tmp = new List<PointLatLng>();
                foreach(var pll in drawnpolygon.Points)
                {
                    PointLatLng pnt_tmp = new PointLatLng(pll.Lat, pll.Lng);
                    list_tmp.Add(pnt_tmp);
                }
                // drawnpolygonsoverlay.Markers.Clear();
                // drawnpolygonsoverlay.Polygons.Clear();
                clearDrawpolygonsoverlayByPolyId(poly_tag);

                redrawPolygonSurvey(list_tmp);
            }
            else
            {
                // drawnpolygon.Points.Add(insert_pnt);
                drawnpolygon.Points.Add(insert_pnt);
                string poly_tag = "";
                if (drawnpolygon.Tag == null)
                {
                    poly_tag = "0";
                }
                else
                {
                    poly_tag = drawnpolygon.Tag.ToString();
                }

                addpolygonmarkergrid(poly_tag + "-" + drawnpolygon.Points.Count.ToString(), MouseDownStart.Lng, MouseDownStart.Lat, 0);
                // drawnpolygon.Tag = poly_tag;
            }
            
            // list_poly.Sort(SortCornersClockwise);
            // list_poly.Reverse();
            // redrawPolygonSurvey(list_poly);

#endif


            MainMap.UpdatePolygonLocalPosition(drawnpolygon);

            MainMap.Invalidate();
        }

        public void redrawPolygonSurvey(List<PointLatLng> list)
        {
            string tag_polygon = "";
            if(drawnpolygon.Tag == null)
            {
                tag_polygon = "0";
            }
            else
            {
                tag_polygon = drawnpolygon.Tag.ToString();
            }

            drawnpolygon.Points.Clear();
            // drawnpolygonsoverlay.Clear();

            int tag = 0;
            list.ForEach(x =>
            {
                tag++;
                drawnpolygon.Points.Add(new PointLatLng(x.Lat, x.Lng));
                addpolygonmarkergrid(tag_polygon + "-" + tag.ToString(), x.Lng, x.Lat, 0);
            });
            // int tag_poly = 0;
            drawnpolygon.Tag = tag_polygon;
            if (drawnpolygonsoverlay.Polygons.Count == 0)
            {
                drawnpolygonsoverlay.Polygons.Add(drawnpolygon);
            }
            else
            {
                drawnpolygonsoverlay.Polygons[int.Parse(tag_polygon)] = new GMapPolygon(new List<PointLatLng>(drawnpolygon.Points), "");
                drawnpolygonsoverlay.Polygons[int.Parse(tag_polygon)].Tag = tag_polygon;
            }
            // 
            // MainMap.UpdatePolygonLocalPosition(drawnpolygon);
            // MainMap.Invalidate();
        }
        public void clearDrawpolygonsoverlayByPolyId(string poly_id)
        {
            // clear polygon
            drawnpolygonsoverlay.Polygons[int.Parse(poly_id)] = new GMapPolygon(new List<PointLatLng>(), "");
            // clear markers

            List<GMapMarker> markers_tmp = new List<GMapMarker>();
            for (int ii = 0; ii < drawnpolygonsoverlay.Markers.Count; ii += 2)
            {
                var mker = drawnpolygonsoverlay.Markers[ii];
                // marker not in poly_id
                if ((mker.Tag != null) && !mker.Tag.ToString().Contains(poly_id + "-"))
                {
                    markers_tmp.Add(mker); // m
                    markers_tmp.Add(drawnpolygonsoverlay.Markers[ii + 1]); // mbound
                }
            }
            drawnpolygonsoverlay.Markers.Clear();

            foreach (var mker in markers_tmp)
            {
                drawnpolygonsoverlay.Markers.Add(mker);
            }

        }

        public void redrawPolygonList(List<List<PointLatLng>> list_all)
        {
            drawnpolygon.Points.Clear();
            drawnpolygonsoverlay.Clear();

            
            int tag_poly = 0;
            // List<GMapPolygon> list_gmappoly = new List<GMapPolygon>();
            foreach (var list in list_all)
            {
                int tag = 0;
                GMapPolygon gmp_tmp = new GMapPolygon(list, "tmp");// drawnpolygon.Points.Clear();
                list.ForEach(x =>
                {
                    tag++;
                    // drawnpolygon.Points.Add(x);
                    addpolygonmarkergrid(tag_poly.ToString() + "-" + tag.ToString(), x.Lng, x.Lat, 0);
                });

                gmp_tmp.Tag = tag_poly.ToString();
                gmp_tmp.Fill = Brushes.Transparent;
                gmp_tmp.Stroke = new Pen(Color.OrangeRed, 3); 
                drawnpolygonsoverlay.Polygons.Add(gmp_tmp);

                MainMap.UpdatePolygonLocalPosition(gmp_tmp);
                drawnpolygon = gmp_tmp;

                tag_poly++;
            }            

            MainMap.Invalidate();
        }

        public void redrawPolygonSurvey(List<PointLatLngAlt> list)
        {
            string tag_polygon = "";
            if (drawnpolygon.Tag == null)
            {
                tag_polygon = "0";
            }
            else
            {
                tag_polygon = drawnpolygon.Tag.ToString();
            }

            drawnpolygon.Points.Clear();
            // drawnpolygonsoverlay.Clear();

            int tag = 0;
            list.ForEach(x =>
            {
                tag++;
                drawnpolygon.Points.Add(new PointLatLng(x.Lat, x.Lng));
                addpolygonmarkergrid(tag_polygon + "-" + tag.ToString(), x.Lng, x.Lat, 0);
            });
            // int tag_poly = 0;
            drawnpolygon.Tag = tag_polygon;
            if (drawnpolygonsoverlay.Polygons.Count == 0)
            {
                drawnpolygonsoverlay.Polygons.Add(drawnpolygon);
            }
            else
            {
                drawnpolygonsoverlay.Polygons[int.Parse(tag_polygon)] = new GMapPolygon(new List<PointLatLng>(drawnpolygon.Points), "");
                drawnpolygonsoverlay.Polygons[int.Parse(tag_polygon)].Tag = tag_polygon;
            }
            // 
            // MainMap.UpdatePolygonLocalPosition(drawnpolygon);
            // MainMap.Invalidate();
        }

        private void clearPolygonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            polygongridmode = false;
            if (drawnpolygon == null)
                return;


            // delelte all

            // if selected 
            // if((drawnpolygon != null) && (selected_poly != -1))
            // if ((drawnpolygon.Points.Count > 2) && (selected_poly != -1))
            // {
            //     // int idx = int.Parse(drawnpolygon.Tag.ToString());
            //     drawnpolygonsoverlay.Polygons.RemoveAt(selected_poly);
            //     // drawnpolygonsoverlay.Polygons
            //     polygons_splited.Clear();
            //     if (drawnpolygonsoverlay.Polygons.Count != 0)
            //     {
            //         foreach (var gpoly in drawnpolygonsoverlay.Polygons)
            //         {
            //             polygons_splited.Add(gpoly.Points);
            //         }
            //     }
            //     redrawPolygonList(polygons_splited);
            // }

            polygons_splited.Clear();
            drawnpolygonsoverlay.Polygons.Clear();
            drawnpolygonsoverlay.Markers.Clear();
            drawnpolygon.Clear();
            drawnpolygonsoverlay = null;
            drawnpolygon = null;

            drawnpolygonsoverlay = new GMapOverlay("drawnpolygons"); // + clear_cnt.ToString());
            MainMap.Overlays.Add(drawnpolygonsoverlay);

            List<PointLatLng> polygonPoints2 = new List<PointLatLng>();
            drawnpolygon = new GMapPolygon(polygonPoints2, "drawnpoly"); //  + clear_cnt.ToString());
            clear_cnt++;

            // drawnpolygon.Points = new List<PointLatLng>();
            // drawnpolygonsoverlay.Polygons.Clear();

            // drawnpolygonsoverlay.Markers.Clear();
            MainMap.Invalidate();

            writeKML();
        }

        private void clearPolygon_Click(object sender, EventArgs e)
        {
            // clear polygon
            clearPolygonToolStripMenuItem_Click(sender, e);
            // clear mission
            // clearMissionToolStripMenuItem_Click(sender, e);

        }
        private void clearMissionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            quickadd = true;

            // mono fix
            Commands.CurrentCell = null;

            Commands.Rows.Clear();

            routesoverlay.Clear();

            selectedrow = 0;
            quickadd = false;
            CurentRectMarker = null;

            writeKML();

            // for autoTORTL
            autoTORTL_added = false;
            read_wps_done = false;
            this.cb_en_autoTORTL.Checked = false;
            abs_alt_kml = false;

        }

        private void loiterForeverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectedrow = Commands.Rows.Add();

            Commands.Rows[selectedrow].Cells[Command.Index].Value = MAVLink.MAV_CMD.LOITER_UNLIM.ToString();

            ChangeColumnHeader(MAVLink.MAV_CMD.LOITER_UNLIM.ToString());

            setfromMap(MouseDownEnd.Lat, MouseDownEnd.Lng, (int) float.Parse(TXT_DefaultAlt.Text));

            writeKML();
        }

        private void jumpstartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string repeat = "5";
            if (DialogResult.Cancel == InputBox.Show("Jump repeat", "Number of times to Repeat", ref repeat))
                return;

            selectedrow = Commands.Rows.Add();

            Commands.Rows[selectedrow].Cells[Command.Index].Value = MAVLink.MAV_CMD.DO_JUMP.ToString();

            Commands.Rows[selectedrow].Cells[Param1.Index].Value = 1;

            Commands.Rows[selectedrow].Cells[Param2.Index].Value = repeat;

            writeKML();
        }

        private void jumpwPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string wp = "1";
            if (DialogResult.Cancel == InputBox.Show("WP No", "Jump to WP no?", ref wp))
                return;
            string repeat = "5";
            if (DialogResult.Cancel == InputBox.Show("Jump repeat", "Number of times to Repeat", ref repeat))
                return;

            selectedrow = Commands.Rows.Add();

            Commands.Rows[selectedrow].Cells[Command.Index].Value = MAVLink.MAV_CMD.DO_JUMP.ToString();

            Commands.Rows[selectedrow].Cells[Param1.Index].Value = wp;

            Commands.Rows[selectedrow].Cells[Param2.Index].Value = repeat;

            writeKML();
        }

        private void deleteWPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int no = 0;
            if (CurentRectMarker != null)
            {
                if(CurentRectMarker.InnerMarker == null) // avoid marker(routesoverlay)
                {
                    return;
                }

                if (int.TryParse(CurentRectMarker.InnerMarker.Tag.ToString(), out no))
                {
                    try
                    {
                        Commands.Rows.RemoveAt(no - 1); // home is 0
                    }
                    catch
                    {
                        MessageBox.Show("未选中航点，请重新选择");
                    }
                }
                else if (CurentRectMarker.InnerMarker.Tag.ToString().Contains("grid"))
                {
                    try
                    {

                        if (drawnpolygonsoverlay.Polygons.Count != 0)
                        {
                            string marker_tag = "";
                            marker_tag = CurentRectMarker.InnerMarker.Tag.ToString().Replace("grid", "");
                            int seperator = marker_tag.IndexOf('-');
                            // int leng = marker_tag.Length;
                            int poly_idx = int.Parse(marker_tag.Substring(0, seperator));
                            int pnt_idx = int.Parse(marker_tag.Substring(seperator + 1));


                            drawnpolygonsoverlay.Polygons[poly_idx].Points.RemoveAt(pnt_idx - 1); //  = new PointLatLng(MouseDownEnd.Lat, MouseDownEnd.Lng);
                                                                                                  // drawnpolygon.Points.RemoveAt(no - 1);

                            List<PointLatLng> list_tmp = new List<PointLatLng>();
                            foreach (var pll in drawnpolygonsoverlay.Polygons[poly_idx].Points)
                            {
                                PointLatLng pnt_tmp = new PointLatLng(pll.Lat, pll.Lng);
                                list_tmp.Add(pnt_tmp);
                            }

                            clearDrawpolygonsoverlayByPolyId(poly_idx.ToString());

                            drawnpolygonsoverlay.Polygons[poly_idx] = new GMapPolygon(list_tmp, "");
                            drawnpolygonsoverlay.Polygons[poly_idx].Tag = poly_idx.ToString();

                            int tag = 0;
                            list_tmp.ForEach(x =>
                            {
                                tag++;
                                addpolygonmarkergrid(poly_idx.ToString() + "-" + tag.ToString(), x.Lng, x.Lat, 0);
                            });

                            MainMap.UpdatePolygonLocalPosition(drawnpolygonsoverlay.Polygons[poly_idx]);
                            
                            MainMap.Invalidate();
                        }

                        // drawnpolygon.Points.RemoveAt(no - 1);
                        // drawnpolygonsoverlay.Markers.Clear();
                        // 
                        // int a = 1;
                        // foreach (PointLatLng pnt in drawnpolygon.Points)
                        // {
                        //     addpolygonmarkergrid(a.ToString(), pnt.Lng, pnt.Lat, 0);
                        //     a++;
                        // }
                        // 
                        // MainMap.UpdatePolygonLocalPosition(drawnpolygon);
                        // 
                        // MainMap.Invalidate();
                    }
                    catch
                    {
                        MessageBox.Show("删除航点失败，请重试！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else if (CurrentRallyPt != null)
            {
                rallypointoverlay.Markers.Remove(CurrentRallyPt);
                MainMap.Invalidate(true);

                CurrentRallyPt = null;
            }
            else if (groupmarkers.Count > 0)
            {
                for (int a = Commands.Rows.Count; a > 0; a--)
                {
                    try
                    {
                        if (groupmarkers.Contains(a))
                            Commands.Rows.RemoveAt(a - 1); // home is 0
                    }
                    catch
                    {
                        MessageBox.Show("选择航点失败，请重试", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                groupmarkers.Clear();
            }


            if (currentMarker != null)
                CurentRectMarker = null;

            writeKML();
        }

        private void loitertimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string time = "5";
            if (DialogResult.Cancel == InputBox.Show("Loiter Time", "Loiter Time", ref time))
                return;

            selectedrow = Commands.Rows.Add();

            Commands.Rows[selectedrow].Cells[Command.Index].Value = MAVLink.MAV_CMD.LOITER_TIME.ToString();

            Commands.Rows[selectedrow].Cells[Param1.Index].Value = time;

            ChangeColumnHeader(MAVLink.MAV_CMD.LOITER_TIME.ToString());

            setfromMap(MouseDownEnd.Lat, MouseDownEnd.Lng, (int) float.Parse(TXT_DefaultAlt.Text));

            writeKML();
        }

        private void loitercirclesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string turns = "3";
            if (DialogResult.Cancel == InputBox.Show("Loiter Turns", "Loiter Turns", ref turns))
                return;

            selectedrow = Commands.Rows.Add();

            Commands.Rows[selectedrow].Cells[Command.Index].Value = MAVLink.MAV_CMD.LOITER_TURNS.ToString();

            Commands.Rows[selectedrow].Cells[Param1.Index].Value = turns;

            ChangeColumnHeader(MAVLink.MAV_CMD.LOITER_TURNS.ToString());

            setfromMap(MouseDownEnd.Lat, MouseDownEnd.Lng, (int) float.Parse(TXT_DefaultAlt.Text));

            writeKML();
        }

        private void panelMap_Resize(object sender, EventArgs e)
        {
            // this is a mono fix for the zoom bar
            //Console.WriteLine("panelmap "+panelMap.Size.ToString());
            MainMap.Size = new Size(panelMap.Size.Width - 50, panelMap.Size.Height);
            trackBar1.Location = new Point(panelMap.Size.Width - 50, trackBar1.Location.Y);
            trackBar1.Size = new Size(trackBar1.Size.Width, panelMap.Size.Height - trackBar1.Location.Y);
            label11.Location = new Point(panelMap.Size.Width - 50, label11.Location.Y);
        }

        DateTime mapupdate = DateTime.MinValue;

        /// <summary>
        /// Draw an mav icon, and update tracker location icon and guided mode wp on FP screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (isMouseDown || CurentRectMarker != null)
                    return;

                routesoverlay.Markers.Clear();

                if (MainV2.comPort.MAV.cs.TrackerLocation != MainV2.comPort.MAV.cs.HomeLocation &&
                    MainV2.comPort.MAV.cs.TrackerLocation.Lng != 0)
                {
                    addpolygonmarker("Tracker Home", MainV2.comPort.MAV.cs.TrackerLocation.Lng,
                        MainV2.comPort.MAV.cs.TrackerLocation.Lat, (int) MainV2.comPort.MAV.cs.TrackerLocation.Alt,
                        Color.Blue, routesoverlay);
                }

                if (MainV2.comPort.MAV.cs.lat == 0 || MainV2.comPort.MAV.cs.lng == 0)
                {
                    return;
                }
                     
                var marker = Common.getMAVMarker(MainV2.comPort.MAV);

                routesoverlay.Markers.Add(marker);

                if (MainV2.comPort.MAV.cs.mode.ToLower() == "guided" && MainV2.comPort.MAV.GuidedMode.x != 0)
                {
                    addpolygonmarker("Guided Mode", MainV2.comPort.MAV.GuidedMode.y, MainV2.comPort.MAV.GuidedMode.x,
                        (int) MainV2.comPort.MAV.GuidedMode.z, Color.Blue, routesoverlay);
                }

                //autopan
                if (autopan)
                {
                    if (route.Points[route.Points.Count - 1].Lat != 0 && (mapupdate.AddSeconds(3) < DateTime.Now))
                    {
                        PointLatLng currentloc = new PointLatLng(MainV2.comPort.MAV.cs.lat, MainV2.comPort.MAV.cs.lng);
                        updateMapPosition(currentloc);
                        mapupdate = DateTime.Now;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Warn(ex);
            }

        }

        /// <summary>
        /// Try to reduce the number of map position changes generated by the code
        /// </summary>
        DateTime lastmapposchange = DateTime.MinValue;

        private void updateMapPosition(PointLatLng currentloc)
        {
            BeginInvoke((MethodInvoker) delegate
            {
                try
                {
                    if (lastmapposchange.Second != DateTime.Now.Second)
                    {
                        MainMap.Position = currentloc;
                        lastmapposchange = DateTime.Now;
                    }
                }
                catch
                {
                }
            });
        }

        private void addpolygonmarker(string tag, double lng, double lat, int alt, Color? color, GMapOverlay overlay)
        {
            try
            {
                PointLatLng point = new PointLatLng(lat, lng);
                GMarkerGoogle m = new GMarkerGoogle(point, GMarkerGoogleType.orange);
                m.ToolTipMode = MarkerTooltipMode.Always;
                m.ToolTipText = tag;
                m.Tag = tag;

                GMapMarkerRect mBorders = new GMapMarkerRect(point);
                {
                    mBorders.InnerMarker = m;
                    try
                    {
                        mBorders.wprad =
                            (int) (Settings.Instance.GetFloat("TXT_WPRad")/CurrentState.multiplierdist);
                    }
                    catch
                    {
                    }
                    if (color.HasValue)
                    {
                        mBorders.Color = color.Value;
                    }
                }

                overlay.Markers.Add(m);
                overlay.Markers.Add(mBorders);
            }
            catch (Exception)
            {
            }
        }

        private void GeoFenceuploadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            polygongridmode = false;
            //FENCE_ENABLE ON COPTER
            //FENCE_ACTION ON PLANE
            if (!MainV2.comPort.MAV.param.ContainsKey("FENCE_ENABLE") && !MainV2.comPort.MAV.param.ContainsKey("FENCE_ACTION"))
            {
                MessageBox.Show("Not Supported");
                return;
            }

            if (drawnpolygon == null)
            {
                MessageBox.Show("No polygon to upload");
                return;
            }

            if (geofenceoverlay.Markers.Count == 0)
            {
                MessageBox.Show("No return location set");
                return;
            }

            if (drawnpolygon.Points.Count == 0)
            {
                MessageBox.Show("No polygon drawn");
                return;
            }

            // check if return is inside polygon
            List<PointLatLng> plll = new List<PointLatLng>(drawnpolygon.Points.ToArray());
            // close it
            plll.Add(plll[0]);
            // check it
            if (
                !pnpoly(plll.ToArray(), geofenceoverlay.Markers[0].Position.Lat, geofenceoverlay.Markers[0].Position.Lng))
            {
                MessageBox.Show("Your return location is outside the polygon");
                return;
            }

            int minalt = 0;
            int maxalt = 0;

            if (MainV2.comPort.MAV.param.ContainsKey("FENCE_MINALT"))
            {
                string minalts =
                    (int.Parse(MainV2.comPort.MAV.param["FENCE_MINALT"].ToString())*CurrentState.multiplierdist)
                        .ToString(
                            "0");
                if (DialogResult.Cancel == InputBox.Show("Min Alt", "Box Minimum Altitude?", ref minalts))
                    return;

                if (!int.TryParse(minalts, out minalt))
                {
                    MessageBox.Show("Bad Min Alt");
                    return;
                }
            }

            if (MainV2.comPort.MAV.param.ContainsKey("FENCE_MAXALT"))
            {
                string maxalts =
                    (int.Parse(MainV2.comPort.MAV.param["FENCE_MAXALT"].ToString())*CurrentState.multiplierdist)
                        .ToString(
                            "0");
                if (DialogResult.Cancel == InputBox.Show("Max Alt", "Box Maximum Altitude?", ref maxalts))
                    return;

                if (!int.TryParse(maxalts, out maxalt))
                {
                    MessageBox.Show("Bad Max Alt");
                    return;
                }
            }

            try
            {
                if (MainV2.comPort.MAV.param.ContainsKey("FENCE_MINALT"))
                    MainV2.comPort.setParam("FENCE_MINALT", minalt);
                if (MainV2.comPort.MAV.param.ContainsKey("FENCE_MAXALT"))
                    MainV2.comPort.setParam("FENCE_MAXALT", maxalt);
            }
            catch
            {
                MessageBox.Show("Failed to set min/max fence alt");
                return;
            }

            float oldaction = (float) MainV2.comPort.MAV.param["FENCE_ACTION"];

            try
            {
                MainV2.comPort.setParam("FENCE_ACTION", 0);
            }
            catch
            {
                MessageBox.Show("Failed to set FENCE_ACTION");
                return;
            }

            // points + return + close
            byte pointcount = (byte) (drawnpolygon.Points.Count + 2);


            try
            {
                MainV2.comPort.setParam("FENCE_TOTAL", pointcount);
            }
            catch
            {
                MessageBox.Show("Failed to set FENCE_TOTAL");
                return;
            }

            try
            {
                byte a = 0;
                // add return loc
                MainV2.comPort.setFencePoint(a, new PointLatLngAlt(geofenceoverlay.Markers[0].Position), pointcount);
                a++;
                // add points
                foreach (var pll in drawnpolygon.Points)
                {
                    MainV2.comPort.setFencePoint(a, new PointLatLngAlt(pll), pointcount);
                    a++;
                }

                // add polygon close
                MainV2.comPort.setFencePoint(a, new PointLatLngAlt(drawnpolygon.Points[0]), pointcount);

                try
                {
                    MainV2.comPort.setParam("FENCE_ACTION", oldaction);
                }
                catch
                {
                    MessageBox.Show("Failed to restore FENCE_ACTION");
                    return;
                }

                // clear everything
                drawnpolygonsoverlay.Polygons.Clear();
                drawnpolygonsoverlay.Markers.Clear();
                geofenceoverlay.Polygons.Clear();
                geofencepolygon.Points.Clear();

                // add polygon
                geofencepolygon.Points.AddRange(drawnpolygon.Points.ToArray());

                drawnpolygon.Points.Clear();

                geofenceoverlay.Polygons.Add(geofencepolygon);

                // update flightdata
                FlightData.geofence.Markers.Clear();
                FlightData.geofence.Polygons.Clear();
                FlightData.geofence.Polygons.Add(new GMapPolygon(geofencepolygon.Points, "gf fd")
                {
                    Stroke = geofencepolygon.Stroke,
                    Fill = Brushes.Transparent
                });
                FlightData.geofence.Markers.Add(new GMarkerGoogle(geofenceoverlay.Markers[0].Position,
                    GMarkerGoogleType.red)
                {
                    ToolTipText = geofenceoverlay.Markers[0].ToolTipText,
                    ToolTipMode = geofenceoverlay.Markers[0].ToolTipMode
                });

                MainMap.UpdatePolygonLocalPosition(geofencepolygon);
                MainMap.UpdateMarkerLocalPosition(geofenceoverlay.Markers[0]);

                MainMap.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to send new fence points " + ex, Strings.ERROR);
            }
        }

        private void GeoFencedownloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            polygongridmode = false;
            int count = 1;

            if (MainV2.comPort.MAV.param["FENCE_ACTION"] == null || MainV2.comPort.MAV.param["FENCE_TOTAL"] == null)
            {
                MessageBox.Show("Not Supported");
                return;
            }

            if (int.Parse(MainV2.comPort.MAV.param["FENCE_TOTAL"].ToString()) <= 1)
            {
                MessageBox.Show("Nothing to download");
                return;
            }

            geofenceoverlay.Polygons.Clear();
            geofenceoverlay.Markers.Clear();
            geofencepolygon.Points.Clear();


            for (int a = 0; a < count; a++)
            {
                try
                {
                    PointLatLngAlt plla = MainV2.comPort.getFencePoint(a, ref count);
                    geofencepolygon.Points.Add(new PointLatLng(plla.Lat, plla.Lng));
                }
                catch
                {
                    MessageBox.Show("Failed to get fence point", Strings.ERROR);
                    return;
                }
            }

            // do return location
            geofenceoverlay.Markers.Add(
                new GMarkerGoogle(new PointLatLng(geofencepolygon.Points[0].Lat, geofencepolygon.Points[0].Lng),
                    GMarkerGoogleType.red)
                {
                    ToolTipMode = MarkerTooltipMode.OnMouseOver,
                    ToolTipText = "GeoFence Return"
                });
            geofencepolygon.Points.RemoveAt(0);

            // add now - so local points are calced
            geofenceoverlay.Polygons.Add(geofencepolygon);

            // update flight data
            FlightData.geofence.Markers.Clear();
            FlightData.geofence.Polygons.Clear();
            FlightData.geofence.Polygons.Add(new GMapPolygon(geofencepolygon.Points, "gf fd")
            {
                Stroke = geofencepolygon.Stroke,
                Fill = Brushes.Transparent
            });
            FlightData.geofence.Markers.Add(new GMarkerGoogle(geofenceoverlay.Markers[0].Position, GMarkerGoogleType.red)
            {
                ToolTipText = geofenceoverlay.Markers[0].ToolTipText,
                ToolTipMode = geofenceoverlay.Markers[0].ToolTipMode
            });

            MainMap.UpdatePolygonLocalPosition(geofencepolygon);
            MainMap.UpdateMarkerLocalPosition(geofenceoverlay.Markers[0]);

            MainMap.Invalidate();
        }

        private void setReturnLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            geofenceoverlay.Markers.Clear();
            geofenceoverlay.Markers.Add(new GMarkerGoogle(new PointLatLng(MouseDownStart.Lat, MouseDownStart.Lng),
                GMarkerGoogleType.red) {ToolTipMode = MarkerTooltipMode.OnMouseOver, ToolTipText = "GeoFence Return"});

            MainMap.Invalidate();
        }

        /// <summary>
        /// from http://www.ecse.rpi.edu/Homepages/wrf/Research/Short_Notes/pnpoly.html
        /// </summary>
        /// <param name="array"> a closed polygon</param>
        /// <param name="testx"></param>
        /// <param name="testy"></param>
        /// <returns> true = outside</returns>
        bool pnpoly(PointLatLng[] array, double testx, double testy)
        {
            int nvert = array.Length;
            int i, j = 0;
            bool c = false;
            for (i = 0, j = nvert - 1; i < nvert; j = i++)
            {
                if (((array[i].Lng > testy) != (array[j].Lng > testy)) &&
                    (testx <
                     (array[j].Lat - array[i].Lat)*(testy - array[i].Lng)/(array[j].Lng - array[i].Lng) + array[i].Lat))
                    c = !c;
            }
            return c;
        }

        private void loadFromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog fd = new OpenFileDialog())
            {
                fd.Filter = "Fence (*.fen)|*.fen";
                fd.ShowDialog();
                if (File.Exists(fd.FileName))
                {
                    StreamReader sr = new StreamReader(fd.OpenFile());

                    drawnpolygonsoverlay.Markers.Clear();
                    drawnpolygonsoverlay.Polygons.Clear();
                    drawnpolygon.Points.Clear();

                    int a = 0;

                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        if (line.StartsWith("#"))
                        {
                        }
                        else
                        {
                            string[] items = line.Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);

                            if (a == 0)
                            {
                                geofenceoverlay.Markers.Clear();
                                geofenceoverlay.Markers.Add(
                                    new GMarkerGoogle(new PointLatLng(double.Parse(items[0]), double.Parse(items[1])),
                                        GMarkerGoogleType.red)
                                    {
                                        ToolTipMode = MarkerTooltipMode.OnMouseOver,
                                        ToolTipText = "GeoFence Return"
                                    });
                                MainMap.UpdateMarkerLocalPosition(geofenceoverlay.Markers[0]);
                            }
                            else
                            {
                                drawnpolygon.Points.Add(new PointLatLng(double.Parse(items[0]), double.Parse(items[1])));
                                addpolygonmarkergrid(drawnpolygon.Points.Count.ToString(), double.Parse(items[1]),
                                    double.Parse(items[0]), 0);
                            }
                            a++;
                        }
                    }

                    // remove loop close
                    if (drawnpolygon.Points.Count > 1 &&
                        drawnpolygon.Points[0] == drawnpolygon.Points[drawnpolygon.Points.Count - 1])
                    {
                        drawnpolygon.Points.RemoveAt(drawnpolygon.Points.Count - 1);
                    }

                    drawnpolygonsoverlay.Polygons.Add(drawnpolygon);

                    MainMap.UpdatePolygonLocalPosition(drawnpolygon);

                    MainMap.Invalidate();
                }
            }
        }

        private void saveToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (geofenceoverlay.Markers.Count == 0)
            {
                MessageBox.Show("Please set a return location");
                return;
            }


            using (SaveFileDialog sf = new SaveFileDialog())
            {
                sf.Filter = "Fence (*.fen)|*.fen";
                sf.ShowDialog();
                if (sf.FileName != "")
                {
                    try
                    {
                        StreamWriter sw = new StreamWriter(sf.OpenFile());

                        sw.WriteLine("#saved by APM Planner " + Application.ProductVersion);

                        sw.WriteLine(geofenceoverlay.Markers[0].Position.Lat + " " +
                                     geofenceoverlay.Markers[0].Position.Lng);
                        if (drawnpolygon.Points.Count > 0)
                        {
                            foreach (var pll in drawnpolygon.Points)
                            {
                                sw.WriteLine(pll.Lat + " " + pll.Lng);
                            }

                            PointLatLng pll2 = drawnpolygon.Points[0];

                            sw.WriteLine(pll2.Lat + " " + pll2.Lng);
                        }
                        else
                        {
                            foreach (var pll in geofencepolygon.Points)
                            {
                                sw.WriteLine(pll.Lat + " " + pll.Lng);
                            }

                            PointLatLng pll2 = geofencepolygon.Points[0];

                            sw.WriteLine(pll2.Lat + " " + pll2.Lng);
                        }

                        sw.Close();
                    }
                    catch
                    {
                        MessageBox.Show("Failed to write fence file");
                    }
                }
            }
        }

        public T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();

                formatter.Serialize(ms, obj);

                ms.Position = 0;

                return (T) formatter.Deserialize(ms);
            }
        }

        private void createWpCircleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string RadiusIn = "50";
            if (DialogResult.Cancel == InputBox.Show("Radius", "Radius", ref RadiusIn))
                return;

            string Pointsin = "20";
            if (DialogResult.Cancel == InputBox.Show("Points", "Number of points to generate Circle", ref Pointsin))
                return;

            string Directionin = "1";
            if (DialogResult.Cancel == InputBox.Show("Points", "Direction of circle (-1 or 1)", ref Directionin))
                return;

            string startanglein = "0";
            if (DialogResult.Cancel == InputBox.Show("angle", "Angle of first point (whole degrees)", ref startanglein))
                return;

            int Points = 0;
            int Radius = 0;
            int Direction = 1;
            int startangle = 0;

            if (!int.TryParse(RadiusIn, out Radius))
            {
                MessageBox.Show("Bad Radius");
                return;
            }

            Radius = (int)(Radius / CurrentState.multiplierdist);

            if (!int.TryParse(Pointsin, out Points))
            {
                MessageBox.Show("Bad Point value");
                return;
            }

            if (!int.TryParse(Directionin, out Direction))
            {
                MessageBox.Show("Bad Direction value");
                return;
            }

            if (!int.TryParse(startanglein, out startangle))
            {
                MessageBox.Show("Bad start angle value");
                return;
            }

            double a = startangle;
            double step = 360.0f/Points;
            if (Direction == -1)
            {
                a += 360;
                step *= -1;
            }

            quickadd = true;

            for (; a <= (startangle + 360) && a >= 0; a += step)
            {
                selectedrow = Commands.Rows.Add();

                Commands.Rows[selectedrow].Cells[Command.Index].Value = MAVLink.MAV_CMD.WAYPOINT.ToString();

                ChangeColumnHeader(MAVLink.MAV_CMD.WAYPOINT.ToString());

                float d = Radius;
                float R = 6371000;

                var lat2 = Math.Asin(Math.Sin(MouseDownEnd.Lat*deg2rad)*Math.Cos(d/R) +
                                     Math.Cos(MouseDownEnd.Lat*deg2rad)*Math.Sin(d/R)*Math.Cos(a*deg2rad));
                var lon2 = MouseDownEnd.Lng*deg2rad +
                           Math.Atan2(Math.Sin(a*deg2rad)*Math.Sin(d/R)*Math.Cos(MouseDownEnd.Lat*deg2rad),
                               Math.Cos(d/R) - Math.Sin(MouseDownEnd.Lat*deg2rad)*Math.Sin(lat2));

                PointLatLng pll = new PointLatLng(lat2*rad2deg, lon2*rad2deg);

                setfromMap(pll.Lat, pll.Lng, (int) float.Parse(TXT_DefaultAlt.Text));
            }

            quickadd = false;
            writeKML();
        }

        public void Activate()
        {
            timer1.Start();

            if (MainV2.comPort.BaseStream.IsOpen && MainV2.comPort.MAV.cs.firmware == MainV2.Firmwares.ArduCopter2 &&
                MainV2.comPort.MAV.cs.version < new Version(3, 3))
            {
                CMB_altmode.Visible = false;
            }
            else
            {
                // DB ZhaoYJ@2017-07-29
                // CMB_altmode.Visible = true;
            }

            //switchDockingToolStripMenuItem_Click(null, null);

            updateHome();

            setWPParams();

            updateCMDParams();

            try
            {
                int.Parse(TXT_DefaultAlt.Text);
            }
            catch
            {
                MessageBox.Show("Please fix your default alt value");
                TXT_DefaultAlt.Text = (50*CurrentState.multiplierdist).ToString("0");
            }


        }

        public void updateHome()
        {
            quickadd = true;
            if (InvokeRequired)
            {
                Invoke((MethodInvoker) delegate { updateHomeText(); });
            }
            else
            {
                updateHomeText();
            }
            quickadd = false;
        }

        private void updateHomeText()
        {
            // set home location
            if (MainV2.comPort.MAV.cs.HomeLocation.Lat != 0 && MainV2.comPort.MAV.cs.HomeLocation.Lng != 0)
            {
                TXT_homelat.Text = MainV2.comPort.MAV.cs.HomeLocation.Lat.ToString();

                TXT_homelng.Text = MainV2.comPort.MAV.cs.HomeLocation.Lng.ToString();

                TXT_homealt.Text = MainV2.comPort.MAV.cs.HomeLocation.Alt.ToString();

                writeKML();
            }
        }

        public void Deactivate()
        {
            config(true);
            timer1.Stop();
        }

        private void FlightPlanner_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Stop();
        }

        private void setROIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!cmdParamNames.ContainsKey("DO_SET_ROI"))
            {
                MessageBox.Show(Strings.ErrorFeatureNotEnabled, Strings.ERROR);
                return;
            }

            selectedrow = Commands.Rows.Add();

            Commands.Rows[selectedrow].Cells[Command.Index].Value = MAVLink.MAV_CMD.DO_SET_ROI.ToString();

            //Commands.Rows[selectedrow].Cells[Param1.Index].Value = time;

            ChangeColumnHeader(MAVLink.MAV_CMD.DO_SET_ROI.ToString());

            setfromMap(MouseDownEnd.Lat, MouseDownEnd.Lng, (int) float.Parse(TXT_DefaultAlt.Text));

            writeKML();
        }

        private void zoomToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string place = "天安门";
            if (DialogResult.OK == InputBox.Show("地名", "请输入需要搜索的地名", ref place))
            {
                GeoCoderStatusCode status = MainMap.SetPositionByKeywords(place);
                if (status != GeoCoderStatusCode.G_GEO_SUCCESS)
                {
                    MessageBox.Show("抱歉，搜索不到 '" + place + "', 请重试或者换个地名尝试");
                }
                else
                {
                    MainMap.Zoom = 15;
                }
            }
        }

        bool fetchpathrip;

        private void FetchPath()
        {
            PointLatLngAlt lastpnt = null;

            string maxzoomstring = "20";
            if(InputBox.Show("最大缩放尺度", "请输入最大缩放尺度", ref maxzoomstring) != DialogResult.OK)
                return;

            int maxzoom = 20;
            if (!int.TryParse(maxzoomstring, out maxzoom))
            {
                MessageBox.Show(Strings.InvalidNumberEntered, Strings.ERROR);
                return;
            }

            fetchpathrip = true;

            maxzoom = Math.Min(maxzoom, MainMap.MaxZoom);

            // zoom
            for (int i = 1; i <= maxzoom; i++)
            {
                // exit if reqested
                if (!fetchpathrip)
                    break;

                lastpnt = null;
                // location
                foreach (var pnt in pointlist)
                {
                    if (pnt == null)
                        continue;

                    // exit if reqested
                    if (!fetchpathrip)
                        break;

                    // setup initial enviroment
                    if (lastpnt == null)
                    {
                        lastpnt = pnt;
                        continue;
                    }

                    RectLatLng area = new RectLatLng();
                    double top = Math.Max(lastpnt.Lat, pnt.Lat);
                    double left = Math.Min(lastpnt.Lng, pnt.Lng);
                    double bottom = Math.Min(lastpnt.Lat, pnt.Lat);
                    double right = Math.Max(lastpnt.Lng, pnt.Lng);

                    area.LocationTopLeft = new PointLatLng(top, left);
                    area.HeightLat = top - bottom;
                    area.WidthLng = right - left;

                    TilePrefetcher obj = new TilePrefetcher();
                    ThemeManager.ApplyThemeTo(obj);
                    obj.KeyDown += obj_KeyDown;
                    obj.ShowCompleteMessage = false;
                    obj.Start(area, i, MainMap.MapProvider, 0, 0);

                    if (obj.UserAborted)
                    {
                        fetchpathrip = false;
                        break;
                    }

                    lastpnt = pnt;
                }
            }
        }

        void obj_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                fetchpathrip = false;
            }
        }

        private void prefetchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RectLatLng area = MainMap.SelectedArea;
            if (area.IsEmpty)
            {
                DialogResult res = MessageBox.Show("未框选地图区域，是否直接下载当前页面地图？", "离线地图区域",
                    MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                {
                    area = MainMap.ViewArea;
                }
            }

            if (!area.IsEmpty)
            {
                string maxzoomstring = "20";
                if (InputBox.Show("最大缩放尺度", "请输入最大缩放尺度", ref maxzoomstring) != DialogResult.OK)
                        return;

                int maxzoom = 20;
                if (!int.TryParse(maxzoomstring, out maxzoom))
                {
                    MessageBox.Show(Strings.InvalidNumberEntered, Strings.ERROR);
                    return;
                }

                maxzoom = Math.Min(maxzoom, MainMap.MaxZoom);

                for (int i = 1; i <= maxzoom; i++)
                {
                    TilePrefetcher obj = new TilePrefetcher();
                    ThemeManager.ApplyThemeTo(obj);
                    obj.ShowCompleteMessage = false;
                    obj.Start(area, i, MainMap.MapProvider, 0, 0);

                    if (obj.UserAborted)
                        break;
                }
            }
            else
            {
                MessageBox.Show("Select map area holding ALT", "GMap.NET", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }
        }

        private void kMLOverlayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog fd = new OpenFileDialog())
            {
                fd.Filter = "Google Earth KML |*.kml;*.kmz|AutoCad DXF|*.dxf";
                DialogResult result = fd.ShowDialog();
                string file = fd.FileName;
                if (file != "")
                {
                    kmlpolygonsoverlay.Polygons.Clear();
                    kmlpolygonsoverlay.Routes.Clear();

                    FlightData.kmlpolygons.Routes.Clear();
                    FlightData.kmlpolygons.Polygons.Clear();
                    if (file.ToLower().EndsWith("dxf"))
                    {
                        string zone = "-99";
                        InputBox.Show("Zone", "Please enter the UTM zone, or cancel to not change", ref zone);

                        dxf dxf = new dxf();
                        if (zone != "-99")
                            dxf.Tag = zone;

                        dxf.newLine += Dxf_newLine;
                        dxf.newPolyLine += Dxf_newPolyLine;
                        dxf.newLwPolyline += Dxf_newLwPolyline;
                        dxf.newMLine += Dxf_newMLine;
                        dxf.Read(file);
                    }
                    else
                    {
                        try
                        {
                            string kml = "";
                            string tempdir = "";
                            if (file.ToLower().EndsWith("kmz"))
                            {
                                ZipFile input = new ZipFile(file);

                                tempdir = Path.GetTempPath() + Path.DirectorySeparatorChar + Path.GetRandomFileName();
                                input.ExtractAll(tempdir, ExtractExistingFileAction.OverwriteSilently);

                                string[] kmls = Directory.GetFiles(tempdir, "*.kml");

                                if (kmls.Length > 0)
                                {
                                    file = kmls[0];

                                    input.Dispose();
                                }
                                else
                                {
                                    input.Dispose();
                                    return;
                                }
                            }

                            var sr = new StreamReader(File.OpenRead(file));
                            kml = sr.ReadToEnd();
                            sr.Close();

                            // cleanup after out
                            if (tempdir != "")
                                Directory.Delete(tempdir, true);

                            kml = kml.Replace("<Snippet/>", "");

                            var parser = new Parser();

                            parser.ElementAdded += parser_ElementAdded;
                            parser.ParseString(kml, false);

                            if (DialogResult.Yes ==
                                MessageBox.Show(Strings.Do_you_want_to_load_this_into_the_flight_data_screen, Strings.Load_data,
                                    MessageBoxButtons.YesNo))
                            {
                                foreach (var temp in kmlpolygonsoverlay.Polygons)
                                {
                                    FlightData.kmlpolygons.Polygons.Add(temp);
                                }
                                foreach (var temp in kmlpolygonsoverlay.Routes)
                                {
                                    FlightData.kmlpolygons.Routes.Add(temp);
                                }
                            }

                            if (
                                MessageBox.Show(Strings.Zoom_To, Strings.Zoom_to_the_center_or_the_loaded_file, MessageBoxButtons.YesNo) ==
                                DialogResult.Yes)
                            {
                                MainMap.SetZoomToFitRect(GetBoundingLayer(kmlpolygonsoverlay));
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Strings.Bad_KML_File + ex);
                        }
                    }
                }
            }
        }

        private void Dxf_newMLine(dxf sender, netDxf.Entities.MLine pline)
        {
            var route = new GMapRoute(pline.Handle);
            foreach (var item in pline.Vertexes)
            {
                route.Points.Add(new PointLatLng(item.Location.Y, item.Location.X));
            }

            route.Stroke = new Pen(Color.FromArgb(pline.Color.R, pline.Color.G, pline.Color.B));

            if (sender.Tag != null)
                ConvertUTMCoords(route, int.Parse(sender.Tag.ToString()));

            kmlpolygonsoverlay.Routes.Add(route);
        }

        private void Dxf_newLwPolyline(dxf sender, netDxf.Entities.LwPolyline pline)
        {
            var route = new GMapRoute(pline.Handle);
            foreach (var item in pline.Vertexes)
            {
                route.Points.Add(new PointLatLng(item.Location.Y, item.Location.X));
            }

            route.Stroke = new Pen(Color.FromArgb(pline.Color.R, pline.Color.G, pline.Color.B));

            if (sender.Tag != null)
                ConvertUTMCoords(route, int.Parse(sender.Tag.ToString()));

            kmlpolygonsoverlay.Routes.Add(route);
        }

        private void Dxf_newPolyLine(dxf sender, netDxf.Entities.Polyline pline)
        {
            var route = new GMapRoute(pline.Handle);
            foreach (var item in pline.Vertexes)
            {
                route.Points.Add(new PointLatLng(item.Location.Y, item.Location.X));
            }

            route.Stroke = new Pen(Color.FromArgb(pline.Color.R, pline.Color.G, pline.Color.B));

            if (sender.Tag != null)
                ConvertUTMCoords(route, int.Parse(sender.Tag.ToString()));

            kmlpolygonsoverlay.Routes.Add(route);
        }

        private void Dxf_newLine(dxf sender, netDxf.Entities.Line line)
        {
            var route = new GMapRoute(line.Handle);
            route.Points.Add(new PointLatLng(line.StartPoint.Y,line.StartPoint.X));
            route.Points.Add(new PointLatLng(line.EndPoint.Y, line.EndPoint.X));

            route.Stroke = new Pen(Color.FromArgb(line.Color.R, line.Color.G, line.Color.B));

            if (sender.Tag != null)
                ConvertUTMCoords(route, int.Parse(sender.Tag.ToString()));

            kmlpolygonsoverlay.Routes.Add(route);
        }

        void ConvertUTMCoords(GMapRoute route, int zone = -9999)
        {
            for (int i =0;i < route.Points.Count;i++)
            {
                var pnt = route.Points[i];
                // load input
                utmpos pos = new utmpos(pnt.Lng, pnt.Lat, zone);
                // convert to geo
                var llh = pos.ToLLA();
                // save it back
                route.Points[i] = llh;
                //route.Points[i].Lng = llh.Lng;
            }
        }


        public static RectLatLng GetBoundingLayer(GMapOverlay o)
        {
            RectLatLng ret = RectLatLng.Empty;

            double left = double.MaxValue;
            double top = double.MinValue;
            double right = double.MinValue;
            double bottom = double.MaxValue;

            if (o.IsVisibile)
            {
                foreach (var m in o.Markers)
                {
                    if (m.IsVisible)
                    {
                        // left
                        if (m.Position.Lng < left)
                        {
                            left = m.Position.Lng;
                        }

                        // top
                        if (m.Position.Lat > top)
                        {
                            top = m.Position.Lat;
                        }

                        // right
                        if (m.Position.Lng > right)
                        {
                            right = m.Position.Lng;
                        }

                        // bottom
                        if (m.Position.Lat < bottom)
                        {
                            bottom = m.Position.Lat;
                        }
                    }
                }
                foreach (GMapRoute route in o.Routes)
                {
                    if (route.IsVisible && route.From.HasValue && route.To.HasValue)
                    {
                        foreach (PointLatLng p in route.Points)
                        {
                            // left
                            if (p.Lng < left)
                            {
                                left = p.Lng;
                            }

                            // top
                            if (p.Lat > top)
                            {
                                top = p.Lat;
                            }

                            // right
                            if (p.Lng > right)
                            {
                                right = p.Lng;
                            }

                            // bottom
                            if (p.Lat < bottom)
                            {
                                bottom = p.Lat;
                            }
                        }
                    }
                }
                foreach (GMapPolygon polygon in o.Polygons)
                {
                    if (polygon.IsVisible)
                    {
                        foreach (PointLatLng p in polygon.Points)
                        {
                            // left
                            if (p.Lng < left)
                            {
                                left = p.Lng;
                            }

                            // top
                            if (p.Lat > top)
                            {
                                top = p.Lat;
                            }

                            // right
                            if (p.Lng > right)
                            {
                                right = p.Lng;
                            }

                            // bottom
                            if (p.Lat < bottom)
                            {
                                bottom = p.Lat;
                            }
                        }
                    }
                }
            }

            if (left != double.MaxValue && right != double.MinValue && top != double.MinValue && bottom != double.MaxValue)
            {
                ret = RectLatLng.FromLTRB(left, top, right, bottom);
            }

            return ret;
        }

        private void elevationGraphToolStripMenuItem_Click(object sender, EventArgs e)
        {
            writeKML();
            double homealt = MainV2.comPort.MAV.cs.HomeAlt;
            Form temp = new ElevationProfile(pointlist, homealt, (altmode) CMB_altmode.SelectedValue);
            ThemeManager.ApplyThemeTo(temp);
            temp.ShowDialog();
        }

        private void rTLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectedrow = Commands.Rows.Add();

            Commands.Rows[selectedrow].Cells[Command.Index].Value = MAVLink.MAV_CMD.RETURN_TO_LAUNCH.ToString();

            //Commands.Rows[selectedrow].Cells[Param1.Index].Value = time;

            ChangeColumnHeader(MAVLink.MAV_CMD.RETURN_TO_LAUNCH.ToString());

            writeKML();
        }

        private void landToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectedrow = Commands.Rows.Add();

            Commands.Rows[selectedrow].Cells[Command.Index].Value = MAVLink.MAV_CMD.LAND.ToString();

            //Commands.Rows[selectedrow].Cells[Param1.Index].Value = time;

            ChangeColumnHeader(MAVLink.MAV_CMD.LAND.ToString());

            setfromMap(MouseDownEnd.Lat, MouseDownEnd.Lng, 1);

            writeKML();
        }

        private void AddDigicamControlPhoto()
        {
            selectedrow = Commands.Rows.Add();

            Commands.Rows[selectedrow].Cells[Command.Index].Value = MAVLink.MAV_CMD.DO_DIGICAM_CONTROL.ToString();

            ChangeColumnHeader(MAVLink.MAV_CMD.DO_DIGICAM_CONTROL.ToString());

            writeKML();
        }

        public int AddCommand(MAVLink.MAV_CMD cmd, double p1, double p2, double p3, double p4, double x, double y,
            double z, object tag = null, byte loc_option = 0xFF)
        {
            selectedrow = Commands.Rows.Add();

            FillCommand(this.selectedrow, cmd, p1, p2, p3, p4, x, y, z, tag, loc_option);

            writeKML();

            return selectedrow;
        }

        public void InsertCommand(int rowIndex, MAVLink.MAV_CMD cmd, double p1, double p2, double p3, double p4, double x, double y,
            double z, object tag = null, byte loc_option = 0xFF)
        {
            if (Commands.Rows.Count <= rowIndex)
            {
                AddCommand(cmd, p1, p2, p3, p4, x, y, z, tag, loc_option);
                return;
            }

            Commands.Rows.Insert(rowIndex);

            this.selectedrow = rowIndex;

            FillCommand(this.selectedrow, cmd, p1, p2, p3, p4, x, y, z, tag, loc_option);

            writeKML();
        }

        private void FillCommand(int rowIndex, MAVLink.MAV_CMD cmd, double p1, double p2, double p3, double p4, double x,
            double y, double z, object tag = null, byte loc_option = 0xFF)
        {
            Commands.Rows[rowIndex].Cells[Command.Index].Value = cmd.ToString();
            Commands.Rows[rowIndex].Cells[TagData.Index].Tag = tag;
            Commands.Rows[rowIndex].Cells[TagData.Index].Value = tag;

            // for wp labelled
            if(loc_option != 0xFF)
            {
                Commands.Rows[rowIndex].Cells[Lat.Index].Tag = loc_option;
            }

            ChangeColumnHeader(cmd.ToString());

            // switch wp to spline if spline checked
            if (splinemode && cmd == MAVLink.MAV_CMD.WAYPOINT)
            {
                Commands.Rows[rowIndex].Cells[Command.Index].Value = MAVLink.MAV_CMD.SPLINE_WAYPOINT.ToString();
                ChangeColumnHeader(MAVLink.MAV_CMD.SPLINE_WAYPOINT.ToString());
            }

            if (cmd == MAVLink.MAV_CMD.WAYPOINT)
            {
                // add delay if supplied
                Commands.Rows[rowIndex].Cells[Param1.Index].Value = p1;

                setfromMap(y, x, (int)z, Math.Round(p1, 1));
            }
            else if (cmd == MAVLink.MAV_CMD.LOITER_UNLIM)
            {
                setfromMap(y, x, (int)z);
            }
            else
            {
                Commands.Rows[rowIndex].Cells[Param1.Index].Value = p1;
                Commands.Rows[rowIndex].Cells[Param2.Index].Value = p2;
                Commands.Rows[rowIndex].Cells[Param3.Index].Value = p3;
                Commands.Rows[rowIndex].Cells[Param4.Index].Value = p4;
                Commands.Rows[rowIndex].Cells[Lat.Index].Value = y;
                Commands.Rows[rowIndex].Cells[Lon.Index].Value = x;
                Commands.Rows[rowIndex].Cells[Alt.Index].Value = z;
            }
        }

        private void takeoffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // altitude
            string alt = "10";

            if (DialogResult.Cancel == InputBox.Show("Altitude", "Please enter your takeoff altitude", ref alt))
                return;

            int alti = -1;

            if (!int.TryParse(alt, out alti))
            {
                MessageBox.Show("Bad Alt");
                return;
            }

            // take off pitch
            int topi = 0;

            if (MainV2.comPort.MAV.cs.firmware == MainV2.Firmwares.ArduPlane ||
                MainV2.comPort.MAV.cs.firmware == MainV2.Firmwares.Ateryx)
            {
                string top = "15";

                if (DialogResult.Cancel == InputBox.Show("Takeoff Pitch", "Please enter your takeoff pitch", ref top))
                    return;

                if (!int.TryParse(top, out topi))
                {
                    MessageBox.Show("Bad Takeoff pitch");
                    return;
                }
            }

            selectedrow = Commands.Rows.Add();

            Commands.Rows[selectedrow].Cells[Command.Index].Value = MAVLink.MAV_CMD.TAKEOFF.ToString();

            Commands.Rows[selectedrow].Cells[Param1.Index].Value = topi;

            Commands.Rows[selectedrow].Cells[Alt.Index].Value = alti;

            ChangeColumnHeader(MAVLink.MAV_CMD.TAKEOFF.ToString());

            writeKML();
        }

        internal string wpfilename;

        private void loadWPFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BUT_loadwpfile_Click(null, null);
        }

        private void saveWPFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile_Click(null, null);
        }

        private void trackerHomeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainV2.comPort.MAV.cs.TrackerLocation = new PointLatLngAlt(MouseDownEnd)
            {
                Alt = MainV2.comPort.MAV.cs.HomeAlt
            };
        }

        private void reverseWPsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewRowCollection rows = Commands.Rows;
            //Commands.Rows.Clear();

            int count = rows.Count;

            quickadd = true;

            for (int a = count; a > 0; a--)
            {
                DataGridViewRow row = Commands.Rows[a - 1];
                Commands.Rows.Remove(row);
                Commands.Rows.Add(row);
            }

            quickadd = false;

            writeKML();
        }

        private void loadAndAppendToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog fd = new OpenFileDialog())
            {
                fd.Filter = "Ardupilot Mission|*.waypoints;*.txt";
                fd.DefaultExt = ".waypoints";
                DialogResult result = fd.ShowDialog();
                string file = fd.FileName;
                if (file != "")
                {
                    readQGC110wpfile(file, true);
                }
            }
        }

        private void savePolygonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (drawnpolygon.Points.Count == 0)
            {
                return;
            }


            using (SaveFileDialog sf = new SaveFileDialog())
            {
                sf.Filter = "边界 (*.bj)|*.bj";
                sf.ShowDialog();
                if (sf.FileName != "")
                {
                    try
                    {

                        if (drawnpolygonsoverlay.Polygons.Count != 0)
                        {

                            // all in
                            StreamWriter sw = new StreamWriter(sf.OpenFile());
                            sw.WriteLine("#saved by Farring-GCS " + DateTime.Now.ToString());
                            foreach (var gpoly in drawnpolygonsoverlay.Polygons)
                            {

                                if (gpoly.Points.Count > 0)
                                {
                                    foreach (var pll in gpoly.Points)
                                    {
                                        sw.WriteLine(pll.Lat + " " + pll.Lng);
                                    }
                                
                                    PointLatLng pll2 = gpoly.Points[0];
                                
                                    sw.WriteLine(pll2.Lat + " " + pll2.Lng);
                                }

                                if(drawnpolygonsoverlay.Polygons.Count > 1) // multi-poly, then add marker
                                {
                                    sw.WriteLine("#end of polygon ");
                                }

                            }
                            sw.Close();

                            // splits
                            int split_cnt = 1;
                            foreach (var gpoly in drawnpolygonsoverlay.Polygons)
                            {
                                string path_file = Path.GetDirectoryName(sf.FileName);
                                string file_name = Path.GetFileNameWithoutExtension(sf.FileName);
                                string file_split = path_file + "\\" + file_name + "_" + split_cnt + ".bj";
                                split_cnt++;
                                sw = new StreamWriter(file_split);

                                sw.WriteLine("#saved by Farring-GCS " + DateTime.Now.ToString());
                                if (gpoly.Points.Count > 0)
                                {
                                    foreach (var pll in gpoly.Points)
                                    {
                                        sw.WriteLine(pll.Lat + " " + pll.Lng);
                                    }

                                    PointLatLng pll2 = gpoly.Points[0];

                                    sw.WriteLine(pll2.Lat + " " + pll2.Lng);
                                }

                                // sw.WriteLine("#end of polygon " + DateTime.Now.ToString());
                                sw.Close();
                            }
                            

                        }
                        else // exit this mode
                        {
                            startsplit = new PointLatLng();
                            endsplit = new PointLatLng();
                            polygonsplitmode = false;
                            splitoverlay.Markers.Clear();
                            splitoverlay.Polygons.Clear();
                            splitoverlay.Routes.Clear();
                            splitoverlay.Clear();
                            MessageBox.Show("还没有绘制作业区域，请首先在地图上绘制.", "作业区域分割");
                            return;
                        }

                        // StreamWriter sw = new StreamWriter(sf.OpenFile());
                        // 
                        // sw.WriteLine("#saved by Mission Planner " + Application.ProductVersion);
                        // 
                        // if (drawnpolygon.Points.Count > 0)
                        // {
                        //     foreach (var pll in drawnpolygon.Points)
                        //     {
                        //         sw.WriteLine(pll.Lat + " " + pll.Lng);
                        //     }
                        // 
                        //     PointLatLng pll2 = drawnpolygon.Points[0];
                        // 
                        //     sw.WriteLine(pll2.Lat + " " + pll2.Lng);
                        // }
                        // 
                        // sw.Close();
                    }
                    catch
                    {
                        MessageBox.Show("Failed to write fence file");
                    }
                }
            }
        }

        private void loadPolygonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog fd = new OpenFileDialog())
            {
                fd.Filter = "边界 (*.bj)|*.bj";
                fd.ShowDialog();
                if (File.Exists(fd.FileName))
                {
                    StreamReader sr = new StreamReader(fd.OpenFile());

                    drawnpolygonsoverlay.Markers.Clear();
                    drawnpolygonsoverlay.Polygons.Clear();
                    drawnpolygon.Points.Clear();
                    polygons_splited.Clear();

                    int a = 0;

                    GMapPolygon list_tmp = new GMapPolygon(new List<PointLatLng>(), "");
                    int split_cnt = 0;
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();

                        if (line.Contains("#end of polygon")) //  new polygon or first time
                        {

                            // if (a != 0) //  new polygon
                            {
                                // remove loop close
                                if (list_tmp.Points.Count > 1 &&
                                    list_tmp.Points[0] == list_tmp.Points[list_tmp.Points.Count - 1])
                                {
                                    list_tmp.Points.RemoveAt(list_tmp.Points.Count - 1);
                                }
                                list_tmp.Tag = split_cnt.ToString();
                                drawnpolygonsoverlay.Polygons.Add(list_tmp);
                                drawnpolygon = list_tmp;
                                MainMap.UpdatePolygonLocalPosition(list_tmp);

                                for (int ii = 0; ii < list_tmp.Points.Count; ii++)
                                {
                                    PointLatLng pll = list_tmp.Points[ii];
                                    addpolygonmarkergrid(split_cnt.ToString() + "-" + (ii + 1).ToString(), pll.Lng,
                pll.Lat, 0);
                                }

                                list_tmp = new GMapPolygon(new List<PointLatLng>(), "");
                                split_cnt++;
                            }

                        }
                        else if(line.Contains("#"))
                        {

                        }
                        else
                        {
                            string[] items = line.Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);

                            if (items.Length < 2)
                                continue;

                            list_tmp.Points.Add(new PointLatLng(double.Parse(items[0]), double.Parse(items[1])));


                            a++;
                        }
                    }

                    if(split_cnt == 0) // just one polygon, then add this poly to overlays
                    {
                        // remove loop close
                        if (list_tmp.Points.Count > 1 &&
                            list_tmp.Points[0] == list_tmp.Points[list_tmp.Points.Count - 1])
                        {
                            list_tmp.Points.RemoveAt(list_tmp.Points.Count - 1);
                        }
                        for (int ii = 0; ii < list_tmp.Points.Count; ii++)
                        {
                            PointLatLng pll = list_tmp.Points[ii];
                            addpolygonmarkergrid(split_cnt.ToString() + "-" + (ii + 1).ToString(), pll.Lng,
        pll.Lat, 0);
                        }

                        list_tmp.Tag = split_cnt.ToString();

                        drawnpolygonsoverlay.Polygons.Add(list_tmp);
                        drawnpolygon = list_tmp;
                        MainMap.UpdatePolygonLocalPosition(list_tmp);
                        // st_tmp.Points.Clear();
                        // st_tmp.Clear();
                    }


                    polygongridmode = true;
                    polygonsplitmode = false;
                    wpinsertmode = false;



                    MainMap.Invalidate();

                    MainMap.ZoomAndCenterMarkers(drawnpolygonsoverlay.Id);
                }
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (CurentRectMarker == null && CurrentRallyPt == null && groupmarkers.Count == 0)
            {
                deleteWPToolStripMenuItem.Enabled = false;
            }
            else
            {
                deleteWPToolStripMenuItem.Enabled = true;
            }

            isMouseClickOffMenu = false; // Just incase
        }

        private void contextMenuStrip1_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            if (e.CloseReason.ToString() == "AppClicked" || e.CloseReason.ToString() == "AppFocusChange")
                isMouseClickOffMenu = true;
        }

        private void areaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double aream2 = Math.Abs(calcpolygonarea(drawnpolygon.Points));

            double areaa = aream2*0.000247105;

            double areaha = aream2*1e-4;

            double areasqf = aream2*10.7639;

            MessageBox.Show(
                "Area: " + aream2.ToString("0") + " m2\n\t" + areaa.ToString("0.00") + " Acre\n\t" +
                areaha.ToString("0.00") + " Hectare\n\t" + areasqf.ToString("0") + " sqf", "Area");
        }

        private void MainMap_Paint(object sender, PaintEventArgs e)
        {
            // draw utm grid
            if (grid)
            {
                if (MainMap.Zoom < 10)
                    return;

                var rect = MainMap.ViewArea;

                var plla1 = new PointLatLngAlt(rect.LocationTopLeft);
                var plla2 = new PointLatLngAlt(rect.LocationRightBottom);

                var center = new PointLatLngAlt(rect.LocationMiddle);

                var zone = center.GetUTMZone();

                var utm1 = plla1.ToUTM(zone);
                var utm2 = plla2.ToUTM(zone);

                var deltax = utm1[0] - utm2[0];
                var deltay = utm1[1] - utm2[1];

                //if (deltax)

                var gridsize = 1000.0;


                if (Math.Abs(deltax)/100000 < 40)
                    gridsize = 100000;

                if (Math.Abs(deltax)/10000 < 40)
                    gridsize = 10000;

                if (Math.Abs(deltax)/1000 < 40)
                    gridsize = 1000;

                if (Math.Abs(deltax)/100 < 40)
                    gridsize = 100;


                // round it - x
                utm1[0] = utm1[0] - (utm1[0]%gridsize);
                // y
                utm2[1] = utm2[1] - (utm2[1]%gridsize);

                // x's
                for (double x = utm1[0]; x < utm2[0]; x += gridsize)
                {
                    var p1 = MainMap.FromLatLngToLocal(PointLatLngAlt.FromUTM(zone, x, utm1[1]));
                    var p2 = MainMap.FromLatLngToLocal(PointLatLngAlt.FromUTM(zone, x, utm2[1]));

                    int x1 = (int) p1.X;
                    int y1 = (int) p1.Y;
                    int x2 = (int) p2.X;
                    int y2 = (int) p2.Y;

                    e.Graphics.DrawLine(new Pen(MainMap.SelectionPen.Color, 1), x1, y1, x2, y2);
                }

                // y's
                for (double y = utm2[1]; y < utm1[1]; y += gridsize)
                {
                    var p1 = MainMap.FromLatLngToLocal(PointLatLngAlt.FromUTM(zone, utm1[0], y));
                    var p2 = MainMap.FromLatLngToLocal(PointLatLngAlt.FromUTM(zone, utm2[0], y));

                    int x1 = (int) p1.X;
                    int y1 = (int) p1.Y;
                    int x2 = (int) p2.X;
                    int y2 = (int) p2.Y;

                    e.Graphics.DrawLine(new Pen(MainMap.SelectionPen.Color, 1), x1, y1, x2, y2);
                }
            }

            e.Graphics.ResetTransform();

            polyicon.Location = new Point(10,100);
            polyicon.Paint(e.Graphics);

            // display polygon infos
            display_polygons_info(e.Graphics);
        }

        public void display_polygons_info(Graphics g)
        {
            // first, find out this point's poly
            if (drawnpolygonsoverlay.Polygons.Count != 0)
            {
                foreach (var gpoly in drawnpolygonsoverlay.Polygons)
                {
                    PointLatLng center = get_center(gpoly.Points);
                    GPoint p = MainMap.FromLatLngToLocal(center);
                    int offset = 10;

                    Point[] pnts = {new Point((int)(p.X + offset), (int)p.Y + offset), new Point((int)p.X + offset, (int)p.Y - offset),
                                     new Point((int)(p.X - offset), (int)p.Y + offset), new Point((int)p.X - offset, (int)p.Y - offset)};

                    SolidBrush polygonBrush = new SolidBrush(Color.Transparent);
                    SolidBrush textBrush = new SolidBrush(Color.Yellow);
                    Font font = new Font("Courier", 12);
                    g.Transform = new Matrix();
                    // g.FillPolygon(textBrush, pnts);
                    string poly_info = "作业区域：" + (int.Parse(gpoly.Tag.ToString())+1).ToString() + "\n面积：" + (Math.Abs(calcpolygonarea(gpoly.Points))).ToString("0.0") + "平方米\n(亩数:" + (Math.Abs(calcpolygonarea(gpoly.Points))/666.67).ToString("0.0") + ")";
                    g.DrawString(poly_info, font, textBrush, new PointF(p.X, p.Y));
                }

            }
            if (avoidPntsoverlay.Polygons.Count != 0)
            {
                int idx = 1;
                foreach (var gpoly in avoidPntsoverlay.Polygons)
                {
                    PointLatLng center = get_center(gpoly.Points);
                    GPoint p = MainMap.FromLatLngToLocal(center);
                    int offset = 10;

                    Point[] pnts = {new Point((int)(p.X + offset), (int)p.Y + offset), new Point((int)p.X + offset, (int)p.Y - offset),
                                     new Point((int)(p.X - offset), (int)p.Y + offset), new Point((int)p.X - offset, (int)p.Y - offset)};

                    SolidBrush textBrush = new SolidBrush(Color.White);
                    Font font = new Font("Courier", 12);
                    g.Transform = new Matrix();
                    // g.FillPolygon(textBrush, pnts);
                    string poly_info = "障碍区域：" + (idx++) + "\n";
                    g.DrawString(poly_info, font, textBrush, new PointF(p.X, p.Y));
                }

            }
        }


        double calcpolygonarea(List<PointLatLngAlt> polygon)
        {
            // should be a closed polygon
            // coords are in lat long
            // need utm to calc area

            if (polygon.Count == 0)
            {
                // MessageBox.Show("Please define a polygon!");
                return 0;
            }

            // close the polygon
            if (polygon[0] != polygon[polygon.Count - 1])
                polygon.Add(polygon[0]); // make a full loop

            CoordinateTransformationFactory ctfac = new CoordinateTransformationFactory();

            GeographicCoordinateSystem wgs84 = GeographicCoordinateSystem.WGS84;

            int utmzone = (int)((polygon[0].Lng - -186.0) / 6.0);

            IProjectedCoordinateSystem utm = ProjectedCoordinateSystem.WGS84_UTM(utmzone, polygon[0].Lat < 0 ? false : true);

            ICoordinateTransformation trans = ctfac.CreateFromCoordinateSystems(wgs84, utm);

            double prod1 = 0;
            double prod2 = 0;

            for (int a = 0; a < (polygon.Count - 1); a++)
            {
                double[] pll1 = { polygon[a].Lng, polygon[a].Lat };
                double[] pll2 = { polygon[a + 1].Lng, polygon[a + 1].Lat };

                double[] p1 = trans.MathTransform.Transform(pll1);
                double[] p2 = trans.MathTransform.Transform(pll2);

                prod1 += p1[0] * p2[1];
                prod2 += p1[1] * p2[0];
            }

            double answer = (prod1 - prod2) / 2;

            if (polygon[0] == polygon[polygon.Count - 1])
                polygon.RemoveAt(polygon.Count - 1); // unmake a full loop

            return Math.Abs(answer);
        }


        public PointLatLng get_center(List<PointLatLng> l_poly)
        {
            PointLatLng center = new PointLatLng();
            double x = 0, y = 0;
            for (int i = 0; i < l_poly.Count; i++)
            {
                x += l_poly[i].Lat;
                y += l_poly[i].Lng;
            }
            center.Lat = (double)x / l_poly.Count;
            center.Lng = (double)y / l_poly.Count;
            return center;
        }

        MissionPlanner.Controls.Icon.Polygon polyicon = new MissionPlanner.Controls.Icon.Polygon();

        private void chk_grid_CheckedChanged(object sender, EventArgs e)
        {
            grid = chk_grid.Checked;
        }

        private void insertWpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string wpno = (selectedrow + 1).ToString("0");
            if (InputBox.Show("在航线中插入航点", "请输入插入的新航点位置（即位于哪个航点后面）", ref wpno) == DialogResult.OK)
            {
                if (InputBox.Show("新航点的高度", "请输入插入的新航点高度（米）", ref wp_alt) == DialogResult.OK)
                {
                    try
                    {
                        Commands.Rows.Insert(int.Parse(wpno), 1);
                    }
                    catch
                    {
                        MessageBox.Show("插入的航点位置有误", Strings.ERROR);
                        return;
                    }

                    selectedrow = int.Parse(wpno);

                    ChangeColumnHeader(MAVLink.MAV_CMD.WAYPOINT.ToString());

                    setfromMap(MouseDownStart.Lat, MouseDownStart.Lng, int.Parse(wp_alt));
                }

            }
            
        }

        private void editWpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wpinsertmode = true;
            polygongridmode = false;
            polygonsplitmode = false; 
        }

        private void doneWpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wpinsertmode = false;
        }

        public void getRallyPointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MainV2.comPort.MAV.param["RALLY_TOTAL"] == null)
            {
                // MessageBox.Show("Not Supported");
                return;
            }

            if (int.Parse(MainV2.comPort.MAV.param["RALLY_TOTAL"].ToString()) < 1)
            {
                // MessageBox.Show("Rally points - Nothing to download");
                return;
            }

            rallypointoverlay.Markers.Clear();

            int count = int.Parse(MainV2.comPort.MAV.param["RALLY_TOTAL"].ToString());

            for (int a = 0; a < (count); a++)
            {
                try
                {
                    PointLatLngAlt plla = MainV2.comPort.getRallyPoint(a, ref count);
                    rallypointoverlay.Markers.Add(new GMapMarkerRallyPt(new PointLatLng(plla.Lat, plla.Lng))
                    {
                        Alt = (int) plla.Alt,
                        ToolTipMode = MarkerTooltipMode.OnMouseOver,
                        ToolTipText = "备降点" + "\n高度: " + (plla.Alt*CurrentState.multiplierdist)
                    });
                }
                catch
                {
                    MessageBox.Show("抱歉，加载备降点失败", Strings.ERROR);
                    return;
                }
            }

            MainMap.UpdateMarkerLocalPosition(rallypointoverlay.Markers[0]);

            MainMap.Invalidate();
        }

        private void saveRallyPointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            byte count = 0;

            MainV2.comPort.setParam("RALLY_TOTAL", rallypointoverlay.Markers.Count);

            foreach (GMapMarkerRallyPt pnt in rallypointoverlay.Markers)
            {
                try
                {
                    MainV2.comPort.setRallyPoint(count, new PointLatLngAlt(pnt.Position) {Alt = pnt.Alt}, 0, 0, 0,
                        (byte) (float) MainV2.comPort.MAV.param["RALLY_TOTAL"]);
                    count++;
                }
                catch
                {
                    MessageBox.Show("抱歉，保存备降点失败", Strings.ERROR);
                    return;
                }
            }
        }

        private void setRallyPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string altstring = TXT_DefaultAlt.Text;

            if (InputBox.Show("请设置备降点高度", "高度值(米)", ref altstring) == DialogResult.Cancel)
                return;

            int alt = 0;

            if (int.TryParse(altstring, out alt))
            {
                PointLatLngAlt rallypt = new PointLatLngAlt(MouseDownStart.Lat, MouseDownStart.Lng,
                    alt/CurrentState.multiplierdist, "Rally Point");
                rallypointoverlay.Markers.Add(
                    new GMapMarkerRallyPt(rallypt)
                    {
                        ToolTipMode = MarkerTooltipMode.OnMouseOver,
                        ToolTipText = "备降点" + "\n高度: " + alt,
                        Tag = rallypointoverlay.Markers.Count,
                        Alt = (int) rallypt.Alt
                    }
                    );
            }
            else
            {
                MessageBox.Show(Strings.InvalidAlt, Strings.ERROR);
            }
        }

        private void clearRallyPointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                MainV2.comPort.setParam("RALLY_TOTAL", 0);
            }
            catch
            {
            }
            rallypointoverlay.Markers.Clear();
            MainV2.comPort.MAV.rallypoints.Clear();
        }

        
        private void loadKMLbjStripMenuItem_Click(object sender, EventArgs e)
        {
            rb_bj.Checked = true; // just use it
            loadKMLFileToolStripMenuItem_Click(sender, e);
        }

        private void loadKMLWPStripMenuItem_Click(object sender, EventArgs e)
        {
            rb_bj.Checked = false; // just use it
            loadKMLFileToolStripMenuItem_Click(sender, e);
        }

        private void loadKMLFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog fd = new OpenFileDialog())
            {
                fd.Filter = "谷歌地图 KML |*.kml;*.kmz";
                DialogResult result = fd.ShowDialog();
                string file = fd.FileName;
                if (file != "")
                {
                    try
                    {
                        string kml = "";
                        string tempdir = "";
                        if (file.ToLower().EndsWith("kmz"))
                        {
                            ZipFile input = new ZipFile(file);

                            tempdir = Path.GetTempPath() + Path.DirectorySeparatorChar + Path.GetRandomFileName();
                            input.ExtractAll(tempdir, ExtractExistingFileAction.OverwriteSilently);

                            string[] kmls = Directory.GetFiles(tempdir, "*.kml");

                            if (kmls.Length > 0)
                            {
                                file = kmls[0];

                                input.Dispose();
                            }
                            else
                            {
                                input.Dispose();
                                return;
                            }
                        }

                        var sr = new StreamReader(File.OpenRead(file));
                        kml = sr.ReadToEnd();
                        sr.Close();

                        if (rb_bj.Checked)
                        {
                            drawnpolygonsoverlay.Markers.Clear();
                            drawnpolygonsoverlay.Polygons.Clear();
                            drawnpolygon.Points.Clear();
                            polygons_splited.Clear();

                            polygongridmode = true;
                            polygonsplitmode = false;
                            wpinsertmode = false;
                        }

                        // cleanup after out
                        if (tempdir != "")
                        Directory.Delete(tempdir, true);

                        kml = kml.Replace("<Snippet/>", "");

                        var parser = new Parser();

                        parser.ElementAdded += processKMLMission;
                        parser.ParseString(kml, false);

                        // polygons_splited.Clear();
                        // if (drawnpolygonsoverlay.Polygons.Count != 0)
                        // {
                        //     foreach (var gpoly in drawnpolygonsoverlay.Polygons)
                        //     {
                        //         polygons_splited.Add(gpoly.Points);
                        //     }
                        // }
                        // redrawPolygonList(polygons_splited);
                        // 
                        // // MainMap.UpdatePolygonLocalPosition(drawnpolygon);
                        // 
                        // MainMap.Invalidate();
                        // 
                        // MainMap.ZoomAndCenterMarkers(drawnpolygonsoverlay.Id);

                        // abs_alt_kml = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(Strings.Bad_KML_File + ex);
                    }
                }
            }
        }

        private void processKMLMission(object sender, ElementEventArgs e)
        {
            Element element = e.Element;
            try
            {
                //  log.Info(Element.ToString() + " " + Element.Parent);
            }
            catch
            {
            }

            Document doc = element as Document;
            Placemark pm = element as Placemark;
            Folder folder = element as Folder;
            Polygon polygon = element as Polygon;
            LineString ls = element as LineString;

            if (doc != null)
            {
                foreach (var feat in doc.Features)
                {
                    //Console.WriteLine("feat " + feat.GetType());
                    //processKML((Element)feat);
                }
            }
            else if (folder != null)
            {
                foreach (Feature feat in folder.Features)
                {
                    //Console.WriteLine("feat "+feat.GetType());
                    //processKML(feat);
                }
            }
            else if (pm != null)
            {
                if (pm.Geometry is SharpKml.Dom.Point)
                {
                    var point = ((SharpKml.Dom.Point)pm.Geometry).Coordinate;
                    POI.POIAdd(new PointLatLngAlt(point.Latitude, point.Longitude), pm.Name);
                }
            }
            else if (polygon != null)
            {
            }
            else if (ls != null)
            {
                List<PointLatLng> l_tmp = new List<PointLatLng>();
                foreach (var loc in ls.Coordinates)
                {
                    if (rb_bj.Checked)
                    {

                        // drawnpolygon.Points.Add(new PointLatLng(loc.Latitude, loc.Longitude));
                        // addpolygonmarkergrid(drawnpolygon.Points.Count.ToString(), loc.Longitude,
                        //     loc.Latitude, 0);
                        l_tmp.Add(new PointLatLng(loc.Latitude, loc.Longitude));
                        addpolygonmarkergrid(drawnpolygonsoverlay.Polygons.Count.ToString() + "-" + l_tmp.Count.ToString(), loc.Longitude,
                            loc.Latitude, 0);
                    }
                    else
                    {
                        selectedrow = Commands.Rows.Add();
                        setfromMap(loc.Latitude, loc.Longitude, (int)loc.Altitude);
                    }

                }

                if (rb_bj.Checked)
                {

                    // remove loop close
                    if (l_tmp.Count > 1 &&
                        l_tmp[0] == l_tmp[l_tmp.Count - 1])
                    {
                        l_tmp.RemoveAt(l_tmp.Count - 1);
                    }
                    GMapPolygon gmp_tmp = new GMapPolygon(l_tmp, "");
                    gmp_tmp.Tag = drawnpolygonsoverlay.Polygons.Count; // poly seq num
                    MainMap.UpdatePolygonLocalPosition(gmp_tmp);
                    drawnpolygonsoverlay.Polygons.Add(gmp_tmp);
                    // drawnpolygon.Clear();
                    drawnpolygon = gmp_tmp;
                    MainMap.ZoomAndCenterMarkers(drawnpolygonsoverlay.Id);                  

                }

            }
        }

        private void lnk_kml_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start("http://127.0.0.1:56781/network.kml");
            }
            catch
            {
                MessageBox.Show("Failed to open url http://127.0.0.1:56781/network.kml");
            }
        }

        private void modifyAltToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string altdif = "0";
            InputBox.Show("Alt Change", "Please enter the alitude change you require.\n(20 = up 20, *2 = up by alt * 2)",
                ref altdif);

            int altchange = 0;
            float multiplyer = 1;

            try
            {
                if (altdif.Contains("*"))
                {
                    multiplyer = float.Parse(altdif.Replace('*', ' '));
                }
                else
                {
                    altchange = int.Parse(altdif);
                }
            }
            catch
            {
                MessageBox.Show(Strings.InvalidNumberEntered, Strings.ERROR);
                return;
            }


            foreach (DataGridViewRow line in Commands.Rows)
            {
                line.Cells[Alt.Index].Value =
                    (int) (float.Parse(line.Cells[Alt.Index].Value.ToString())*multiplyer + altchange);
            }
        }

        private void saveToFileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (rallypointoverlay.Markers.Count == 0)
            {
                MessageBox.Show("Please set some rally points");
                return;
            }
            /*
Column 1: Field type (RALLY is the only one at the moment -- may have RALLY_LAND in the future)
 Column 2,3: Lat, lon
 Column 4: Loiter altitude
 Column 5: Break altitude (when landing from rally is implemented, this is the altitude to break out of loiter from)
 Column 6: Landing heading (also for future when landing from rally is implemented)
 Column 7: Flags (just 0 for now, also future use).
             */

            using (SaveFileDialog sf = new SaveFileDialog())
            {
                sf.Filter = "Rally (*.ral)|*.ral";
                sf.ShowDialog();
                if (sf.FileName != "")
                {
                    try
                    {
                        using (StreamWriter sw = new StreamWriter(sf.OpenFile()))
                        {
                            sw.WriteLine("#saved by Mission Planner " + Application.ProductVersion);


                            foreach (GMapMarkerRallyPt mark in rallypointoverlay.Markers)
                            {
                                sw.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}", "RALLY", mark.Position.Lat,
                                    mark.Position.Lng, mark.Alt, 0, 0, 0);
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Failed to write rally file");
                    }
                }
            }
        }

        private void loadFromFileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog fd = new OpenFileDialog())
            {
                fd.Filter = "Rally (*.ral)|*.ral";
                fd.ShowDialog();
                if (File.Exists(fd.FileName))
                {
                    StreamReader sr = new StreamReader(fd.OpenFile());

                    int a = 0;

                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        if (line.StartsWith("#"))
                        {
                        }
                        else
                        {
                            string[] items = line.Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);

                            MAVLink.mavlink_rally_point_t rally = new MAVLink.mavlink_rally_point_t();

                            rally.lat = (int) (float.Parse(items[1])*1e7);
                            rally.lng = (int) (float.Parse(items[2])*1e7);
                            rally.alt = (short) float.Parse(items[3]);
                            rally.break_alt = (short) float.Parse(items[4]);
                            rally.land_dir = (ushort) float.Parse(items[5]);
                            rally.flags = byte.Parse(items[6]);

                            if (a == 0)
                            {
                                rallypointoverlay.Markers.Clear();

                                rallypointoverlay.Markers.Add(new GMapMarkerRallyPt(rally));
                            }
                            else
                            {
                                rallypointoverlay.Markers.Add(new GMapMarkerRallyPt(rally));
                            }
                            a++;
                        }
                    }
                    MainMap.Invalidate();
                }
            }
        }

        private void prefetchWPPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FetchPath();
        }

        static string zone = "50s";

        private void enterUTMCoordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string easting = "578994";
            string northing = "6126244";

            if (InputBox.Show("Zone", "Enter Zone. (eg 50S, 11N)", ref zone) != DialogResult.OK)
                return;
            if (InputBox.Show("Easting", "Easting", ref easting) != DialogResult.OK)
                return;
            if (InputBox.Show("Northing", "Northing", ref northing) != DialogResult.OK)
                return;

            string newzone = zone.ToLower().Replace('s', ' ');
            newzone = newzone.ToLower().Replace('n', ' ');

            int zoneint = int.Parse(newzone);

            UTM utm = new UTM(zoneint, double.Parse(easting), double.Parse(northing),
                zone.ToLower().Contains("N") ? Geocentric.Hemisphere.North : Geocentric.Hemisphere.South);

            PointLatLngAlt ans = ((Geographic) utm);

            selectedrow = Commands.Rows.Add();

            ChangeColumnHeader(MAVLink.MAV_CMD.WAYPOINT.ToString());

            setfromMap(ans.Lat, ans.Lng, (int) ans.Alt);
        }

        private void loadSHPFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog fd = new OpenFileDialog())
            {
                fd.Filter = "Shape file|*.shp";
                DialogResult result = fd.ShowDialog();
                string file = fd.FileName;

                LoadSHPFile(file);
            }
        }

        private void LoadSHPFile(string file)
        {
            ProjectionInfo pStart = new ProjectionInfo();
            ProjectionInfo pESRIEnd = KnownCoordinateSystems.Geographic.World.WGS1984;
            bool reproject = false;

            if (File.Exists(file))
            {
                string prjfile = Path.GetDirectoryName(file) + Path.DirectorySeparatorChar +
                                 Path.GetFileNameWithoutExtension(file) + ".prj";
                if (File.Exists(prjfile))
                {
                    using (
                        StreamReader re =
                            File.OpenText(Path.GetDirectoryName(file) + Path.DirectorySeparatorChar +
                                          Path.GetFileNameWithoutExtension(file) + ".prj"))
                    {
                        pStart.ParseEsriString(re.ReadLine());

                        reproject = true;
                    }
                }

                IFeatureSet fs = FeatureSet.Open(file);

                fs.FillAttributes();

                int rows = fs.NumRows();

                DataTable dtOriginal = fs.DataTable;
                for (int row = 0; row < dtOriginal.Rows.Count; row++)
                {
                    object[] original = dtOriginal.Rows[row].ItemArray;
                }

                foreach (DataColumn col in dtOriginal.Columns)
                {
                    Console.WriteLine(col.ColumnName + " " + col.DataType);
                }

                quickadd = true;

                bool dosort = false;

                List<PointLatLngAlt> wplist = new List<PointLatLngAlt>();

                for (int row = 0; row < dtOriginal.Rows.Count; row++)
                {
                    double x = fs.Vertex[row*2];
                    double y = fs.Vertex[row*2 + 1];

                    double z = -1;
                    float wp = 0;

                    try
                    {
                        if (dtOriginal.Columns.Contains("ELEVATION"))
                            z = (float) Convert.ChangeType(dtOriginal.Rows[row]["ELEVATION"], TypeCode.Single);
                    }
                    catch
                    {
                    }

                    try
                    {
                        if (z == -1 && dtOriginal.Columns.Contains("alt"))
                            z = (float) Convert.ChangeType(dtOriginal.Rows[row]["alt"], TypeCode.Single);
                    }
                    catch
                    {
                    }

                    try
                    {
                        if (z == -1)
                            z = fs.Z[row];
                    }
                    catch
                    {
                    }


                    try
                    {
                        if (dtOriginal.Columns.Contains("wp"))
                        {
                            wp = (float) Convert.ChangeType(dtOriginal.Rows[row]["wp"], TypeCode.Single);
                            dosort = true;
                        }
                    }
                    catch
                    {
                    }

                    if (reproject)
                    {
                        double[] xyarray = {x, y};
                        double[] zarray = {z};

                        Reproject.ReprojectPoints(xyarray, zarray, pStart, pESRIEnd, 0, 1);


                        x = xyarray[0];
                        y = xyarray[1];
                        z = zarray[0];
                    }

                    PointLatLngAlt pnt = new PointLatLngAlt(x, y, z, wp.ToString());

                    wplist.Add(pnt);
                }

                if (dosort)
                    wplist.Sort();

                foreach (var item in wplist)
                {
                    AddCommand(MAVLink.MAV_CMD.WAYPOINT, 0, 0, 0, 0, item.Lat, item.Lng, item.Alt);
                }

                quickadd = false;

                writeKML();

                MainMap.ZoomAndCenterMarkers("objects");
            }
        }

        private void BUT_saveWPFile_Click(object sender, EventArgs e)
        {
            SaveFile_Click(null, null);
        }

        private void switchDockingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (panelAction.Dock == DockStyle.Bottom)
            {
                panelAction.Dock = DockStyle.Right;
                panelWaypoints.Dock = DockStyle.Bottom;
            }
            else
            {
                panelAction.Dock = DockStyle.Bottom;
                panelAction.Height = 120;
                panelWaypoints.Dock = DockStyle.Right;
                panelWaypoints.Width = Width/2;
            }

            Settings.Instance["FP_docking"] = panelAction.Dock.ToString();
        }

        private void insertSplineWPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string wpno = (selectedrow + 1).ToString("0");
            if (InputBox.Show("Insert WP", "Insert WP after wp#", ref wpno) == DialogResult.OK)
            {
                try
                {
                    Commands.Rows.Insert(int.Parse(wpno), 1);
                }
                catch
                {
                    MessageBox.Show(Strings.InvalidNumberEntered, Strings.ERROR);
                    return;
                }

                selectedrow = int.Parse(wpno);

                try
                {
                    Commands.Rows[selectedrow].Cells[Command.Index].Value = MAVLink.MAV_CMD.SPLINE_WAYPOINT.ToString();
                }
                catch
                {
                    MessageBox.Show("SPLINE_WAYPOINT command not supported.");
                    Commands.Rows.RemoveAt(selectedrow);
                    return;
                }

                ChangeColumnHeader(MAVLink.MAV_CMD.SPLINE_WAYPOINT.ToString());

                setfromMap(MouseDownStart.Lat, MouseDownStart.Lng, (int) float.Parse(TXT_DefaultAlt.Text));
            }
        }

        private void CHK_splinedefault_CheckedChanged(object sender, EventArgs e)
        {
            splinemode = CHK_splinedefault.Checked;
        }

        private void createSplineCircleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string RadiusIn = "50";
            if (DialogResult.Cancel == InputBox.Show("Radius", "Radius", ref RadiusIn))
                return;

            string minaltin = "5";
            if (DialogResult.Cancel == InputBox.Show("min alt", "Min Alt", ref minaltin))
                return;

            string maxaltin = "20";
            if (DialogResult.Cancel == InputBox.Show("max alt", "Max Alt", ref maxaltin))
                return;

            string altstepin = "5";
            if (DialogResult.Cancel == InputBox.Show("alt step", "alt step", ref altstepin))
                return;


            string startanglein = "0";
            if (DialogResult.Cancel == InputBox.Show("angle", "Angle of first point (whole degrees)", ref startanglein))
                return;

            int Points = 4;
            int Radius = 0;
            int startangle = 0;
            int minalt = 5;
            int maxalt = 20;
            int altstep = 5;
            if (!int.TryParse(RadiusIn, out Radius))
            {
                MessageBox.Show("Bad Radius");
                return;
            }

            if (!int.TryParse(minaltin, out minalt))
            {
                MessageBox.Show("Bad min alt");
                return;
            }
            if (!int.TryParse(maxaltin, out maxalt))
            {
                MessageBox.Show("Bad maxalt");
                return;
            }
            if (!int.TryParse(altstepin, out altstep))
            {
                MessageBox.Show("Bad alt step");
                return;
            }

            double a = startangle;
            double step = 360.0f/Points;

            quickadd = true;

            AddCommand(MAVLink.MAV_CMD.DO_SET_ROI, 0, 0, 0, 0, MouseDownStart.Lng, MouseDownStart.Lat, 0);

            bool startup = true;

            for (int stepalt = minalt; stepalt <= maxalt;)
            {
                for (a = 0; a <= (startangle + 360) && a >= 0; a += step)
                {
                    selectedrow = Commands.Rows.Add();

                    Commands.Rows[selectedrow].Cells[Command.Index].Value = MAVLink.MAV_CMD.SPLINE_WAYPOINT.ToString();

                    ChangeColumnHeader(MAVLink.MAV_CMD.SPLINE_WAYPOINT.ToString());

                    float d = Radius;
                    float R = 6371000;

                    var lat2 = Math.Asin(Math.Sin(MouseDownEnd.Lat*deg2rad)*Math.Cos(d/R) +
                                         Math.Cos(MouseDownEnd.Lat*deg2rad)*Math.Sin(d/R)*Math.Cos(a*deg2rad));
                    var lon2 = MouseDownEnd.Lng*deg2rad +
                               Math.Atan2(Math.Sin(a*deg2rad)*Math.Sin(d/R)*Math.Cos(MouseDownEnd.Lat*deg2rad),
                                   Math.Cos(d/R) - Math.Sin(MouseDownEnd.Lat*deg2rad)*Math.Sin(lat2));

                    PointLatLng pll = new PointLatLng(lat2*rad2deg, lon2*rad2deg);

                    setfromMap(pll.Lat, pll.Lng, stepalt);

                    if (!startup)
                        stepalt += altstep/Points;
                }

                // reset back to the start
                if (startup)
                    stepalt = minalt;

                // we have finsihed the first run
                startup = false;
            }

            quickadd = false;
            writeKML();
        }

        private void CMB_altmode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CMB_altmode.SelectedValue == null)
            {
                CMB_altmode.SelectedIndex = 0;
            }
            else
            {
                currentaltmode = (altmode) CMB_altmode.SelectedValue;
            }
        }

        private void fromSHPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog fd = new OpenFileDialog())
            {
                fd.Filter = "Shape file|*.shp";
                DialogResult result = fd.ShowDialog();
                string file = fd.FileName;
                ProjectionInfo pStart = new ProjectionInfo();
                ProjectionInfo pESRIEnd = KnownCoordinateSystems.Geographic.World.WGS1984;
                bool reproject = false;
                // Poly Clear
                drawnpolygonsoverlay.Markers.Clear();
                drawnpolygonsoverlay.Polygons.Clear();
                drawnpolygon.Points.Clear();
                if (File.Exists(file))
                {
                    string prjfile = Path.GetDirectoryName(file) + Path.DirectorySeparatorChar +
                                     Path.GetFileNameWithoutExtension(file) + ".prj";
                    if (File.Exists(prjfile))
                    {
                        using (
                            StreamReader re =
                                File.OpenText(Path.GetDirectoryName(file) + Path.DirectorySeparatorChar +
                                              Path.GetFileNameWithoutExtension(file) + ".prj"))
                        {
                            pStart.ParseEsriString(re.ReadLine());
                            reproject = true;
                        }
                    }
                    try
                    {
                        IFeatureSet fs = FeatureSet.Open(file);
                        fs.FillAttributes();
                        int rows = fs.NumRows();
                        DataTable dtOriginal = fs.DataTable;
                        for (int row = 0; row < dtOriginal.Rows.Count; row++)
                        {
                            object[] original = dtOriginal.Rows[row].ItemArray;
                        }
                        string path = Path.GetDirectoryName(file);
                        foreach (var feature in fs.Features)
                        {
                            foreach (var point in feature.Coordinates)
                            {
                                if (reproject)
                                {
                                    double[] xyarray = {point.X, point.Y};
                                    double[] zarray = {point.Z};
                                    Reproject.ReprojectPoints(xyarray, zarray, pStart, pESRIEnd, 0, 1);
                                    point.X = xyarray[0];
                                    point.Y = xyarray[1];
                                    point.Z = zarray[0];
                                }
                                drawnpolygon.Points.Add(new PointLatLng(point.Y, point.X));
                                addpolygonmarkergrid(drawnpolygon.Points.Count.ToString(), point.X, point.Y, 0);
                            }
                            // remove loop close
                            if (drawnpolygon.Points.Count > 1 &&
                                drawnpolygon.Points[0] == drawnpolygon.Points[drawnpolygon.Points.Count - 1])
                            {
                                drawnpolygon.Points.RemoveAt(drawnpolygon.Points.Count - 1);
                            }
                            drawnpolygonsoverlay.Polygons.Add(drawnpolygon);
                            MainMap.UpdatePolygonLocalPosition(drawnpolygon);
                            MainMap.Invalidate();
                            MainMap.ZoomAndCenterMarkers(drawnpolygonsoverlay.Id);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(Strings.ERROR + "\n" + ex, Strings.ERROR);
                    }
                }
            }
        }

        private void panelWaypoints_ExpandClick(object sender, EventArgs e)
        {
            Commands.AutoResizeColumns();
        }

        private void textToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string text = "";
            InputBox.Show("Enter String", "Enter String (requires 1CamBam_Stick_3 font)", ref text);
            string size = "5";
            InputBox.Show("Enter size", "Enter size", ref size);

            using (Font font = new System.Drawing.Font("1CamBam_Stick_3", float.Parse(size) * 1.35f, FontStyle.Regular))
            using (GraphicsPath gp = new GraphicsPath())
            using (StringFormat sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Near;
                sf.LineAlignment = StringAlignment.Near;
                gp.AddString(text, font.FontFamily, (int) font.Style, font.Size, new PointF(0, 0), sf);

                utmpos basepos = new utmpos(MouseDownStart);

                try
                {

                    foreach (var pathPoint in gp.PathPoints)
                    {
                        utmpos newpos = new utmpos(basepos);

                        newpos.x += pathPoint.X;
                        newpos.y += -pathPoint.Y;

                        var newlla = newpos.ToLLA();
                        quickadd = true;
                        AddWPToMap(newlla.Lat, newlla.Lng, int.Parse(TXT_DefaultAlt.Text));

                    }
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show("Bad input options, please try again\n" + ex.ToString(), Strings.ERROR);
                }

                quickadd = false;
                writeKML();
            }
        }

        private void setHomeHereToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TXT_homealt.Text = srtm.getAltitude(MouseDownStart.Lat, MouseDownStart.Lng).alt.ToString("0");
            TXT_homelat.Text = MouseDownStart.Lat.ToString();
            TXT_homelng.Text = MouseDownStart.Lng.ToString();
        }

        private void coords1_SystemChanged(object sender, EventArgs e)
        {
            if (coords1.System == Coords.CoordsSystems.GEO.ToString())
            {
                Lat.Visible = true;
                Lon.Visible = true;

                coordZone.Visible = false;
                coordEasting.Visible = false;
                coordNorthing.Visible = false;
                MGRS.Visible = false;
            }
            else if (coords1.System == Coords.CoordsSystems.MGRS.ToString())
            {
                Lat.Visible = false;
                Lon.Visible = false;

                coordZone.Visible = false;
                coordEasting.Visible = false;
                coordNorthing.Visible = false;
                MGRS.Visible = true;
            }
            else if (coords1.System == Coords.CoordsSystems.UTM.ToString())
            {
                Lat.Visible = false;
                Lon.Visible = false;

                coordZone.Visible = true;
                coordEasting.Visible = true;
                coordNorthing.Visible = true;
                MGRS.Visible = false;
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //FENCE_ENABLE ON COPTER
            //FENCE_ACTION ON PLANE

            try
            {
                MainV2.comPort.setParam("FENCE_ENABLE", 0);
            }
            catch
            {
                MessageBox.Show("Failed to set FENCE_ENABLE");
                return;
            }

            try
            {
                MainV2.comPort.setParam("FENCE_ACTION", 0);
            }
            catch
            {
                MessageBox.Show("Failed to set FENCE_ACTION");
                return;
            }

            try
            {
                MainV2.comPort.setParam("FENCE_TOTAL", 0);
            }
            catch
            {
                MessageBox.Show("Failed to set FENCE_TOTAL");
                return;
            }
        }

        private void map_corr_manual_Click(object sender, EventArgs e)
        {
            // MessageBox.Show("功能待开发...");
            // current fc location
            log.Info("map_corr_manual start ");

            MAVLink.MAVLinkMessage mavLinkMessage = MainV2.comPort.MAV.getPacket((uint)MAVLink.MAVLINK_MSG_ID.GLOBAL_POSITION_INT);
            if (mavLinkMessage != null)
            // if (true)
            {
                var loc_fc = mavLinkMessage.ToStructure<MAVLink.mavlink_global_position_int_t>();
                double lat = loc_fc.lat / 10000000.0;
                double lng = loc_fc.lon / 10000000.0;

                PointLatLng loc_point =  new PointLatLng(lat, lng); //  PointLatLng

                double marker_lat = currentWP.Lat;
                double marker_lng = currentWP.Lng;
                // double marker_lat = currentMarker.Position.Lat;
                // double marker_lng = currentMarker.Position.Lng;

                double dist_diff = MainMap.MapProvider.Projection.GetDistance(loc_point, currentWP);

                // greater than 100 m
                if(dist_diff.CompareTo(0.1) > 0)
                {
                    if (MessageBox.Show("危险：地图纠偏值大于100米！是否采用本次校准结果?", "使用此纠偏值",
        MessageBoxButtons.OKCancel) != DialogResult.OK)
                    {
                        return;
                    }
                }


                // double dist_diff = MainMap.MapProvider.Projection.GetDistance(loc_point, currentMarker.Position);

                // currentOffset.Position = new PointLatLng(marker_lat - lat, marker_lng - lng); //  (currentWP.Position.Lat - lat, ;

                double offset_lat = marker_lat - lat;
                double offset_lng = marker_lng - lng;
                currentOffset = new PointLatLng( offset_lat, offset_lng);

                
                // MessageBox.Show("loc_fc: lat: " + lat + " lng: " + lng + " <--> marker: lat: " + marker_lat + " lng: " + marker_lng + ", dist_diff: " + dist_diff);
                log.Info("loc_fc: lat: " + lat + " lng: " + lng + " <--> marker: lat: " + marker_lat + " lng: " + marker_lng);
                log.Info("地图手动纠偏完成:\n纬度偏差：" + offset_lat + " \n经度偏差: " + offset_lng + "\n距离偏移： " + dist_diff * 1000.0 + " 米\n");
                MessageBox.Show("地图手动纠偏完成:\n\n纬度偏差：" + offset_lat + " 度 \n经度偏差: " + offset_lng + " 度 \n距离偏移： " + dist_diff*1000.0 + " 米\n", "地图纠偏完成");
                // store map offset 
                if(MessageBox.Show("是否保存本次校准结果，以便下次使用?\n【注意：下次使用时距离超过30千米，请重新校准】", "保存",
    MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    //创建一个数据集，将其写入xml文件
                    System.Data.DataSet ds = new System.Data.DataSet("FARRING_metadata");
                    System.Data.DataTable table = new System.Data.DataTable("lla_offset");
                    ds.Tables.Add(table);
                    table.Columns.Add("Date", typeof(string));
                    table.Columns.Add("lat_offset", typeof(string));
                    table.Columns.Add("lng_offset", typeof(string));

                    System.Data.DataRow row = table.NewRow();
                    row[0] = DateTime.Now.ToString("yyyyMMddHHmmss");
                    row[1] = offset_lat;
                    row[2] = offset_lng;
                    ds.Tables["lla_offset"].Rows.Add(row);

                    ds.WriteXml("./lla_offs.xml");
                }

            }
            else
            {
                MessageBox.Show("未连接飞控，地图手动纠偏失败！", "地图纠偏失败");

            }

            return;
        }


        private static double pi = 3.14159265358979324;

        private static double a = 6378245.0;

        private static double ee = 0.00669342162296594323;

        private static bool outOfChina(double lat, double lon)

        {

            if (lon < 72.004 || lon > 137.8347)

                return true;

            if (lat < 0.8293 || lat > 55.8271)

                return true;

            return false;

        }

        private static double transformLat(double x, double y)

        {

            double ret = -100.0 + 2.0 * x + 3.0 * y + 0.2 * y * y + 0.1 * x * y + 0.2 * Math.Sqrt(Math.Abs(x));

            ret += (20.0 * Math.Sin(6.0 * x * pi) + 20.0 * Math.Sin(2.0 * x * pi)) * 2.0 / 3.0;

            ret += (20.0 * Math.Sin(y * pi) + 40.0 * Math.Sin(y / 3.0 * pi)) * 2.0 / 3.0;

            ret += (160.0 * Math.Sin(y / 12.0 * pi) + 320 * Math.Sin(y * pi / 30.0)) * 2.0 / 3.0;

            return ret;

        }

        private static double transformLon(double x, double y)

        {

            double ret = 300.0 + x + 2.0 * y + 0.1 * x * x + 0.1 * x * y + 0.1 * Math.Sqrt(Math.Abs(x));

            ret += (20.0 * Math.Sin(6.0 * x * pi) + 20.0 * Math.Sin(2.0 * x * pi)) * 2.0 / 3.0;

            ret += (20.0 * Math.Sin(x * pi) + 40.0 * Math.Sin(x / 3.0 * pi)) * 2.0 / 3.0;

            ret += (150.0 * Math.Sin(x / 12.0 * pi) + 300.0 * Math.Sin(x / 30.0 * pi)) * 2.0 / 3.0;

            return ret;

        }

        public static void map_corr_auto_action(double wgLat, double wgLon, double[] latlng)
        {
            // MessageBox.Show("功能待开发...");
            // current fc location
            log.Info("map_corr_auto start ");
            if (outOfChina(wgLat, wgLon))
            { //判断有没有在中国范围内
                latlng[0] = wgLat;
                latlng[1] = wgLon;
                return;
            }
            double dLat = transformLat(wgLon - 105.0, wgLat - 35.0);
            double dLon = transformLon(wgLon - 105.0, wgLat - 35.0);
            double radLat = wgLat / 180.0 * pi;
            double magic = Math.Sin(radLat);
            magic = 1 - ee * magic * magic;
            double sqrtMagic = Math.Sqrt(magic);
            dLat = (dLat * 180.0) / ((a * (1 - ee)) / (magic * sqrtMagic) * pi);
            dLon = (dLon * 180.0) / (a / sqrtMagic * Math.Cos(radLat) * pi);
            latlng[0] = wgLat + dLat;
            latlng[1] = wgLon + dLon;
            return;
        }

        private void map_corr_auto_CheckedChanged(object sender, EventArgs e)
        {
            if(map_corr_auto.Checked)
                MessageBox.Show("功能测试中，请谨慎试用！！\n\n规划完航线后请从飞控重新读取航线，看是否存在较大偏移，是则人工确认航点坐标的正确性，或者取消地图自动纠偏功能.");
            return;
        }

        private void load_last_offs_Click(object sender, EventArgs e)
        {
            System.Xml.XmlTextReader reader;

            try
            {
                reader = new System.Xml.XmlTextReader("./lla_offs.xml");
                double offset_lat = 0; // ds.Tables.Contains;
                double offset_lng = 0; // marker_lng - lng;
                bool offset_readed = false;
                while (reader.Read())
                {

                    if (reader.IsStartElement("lat_offset"))
                    {
                        reader.Read();
                        offset_lat = double.Parse(reader.Value);
                        offset_readed = true;
                    }
                    else if (reader.IsStartElement("lng_offset"))
                    {
                        reader.Read();
                        offset_lng = double.Parse(reader.Value);
                        offset_readed = true;
                    }
                }

                // close xml
                reader.Close();

                // invalid offset
                if (!offset_readed)
                {
                    return;
                }

                PointLatLng currentOffset_tmp = new PointLatLng(offset_lat, offset_lng);

                double dist_diff = MainMap.MapProvider.Projection.GetDistance(currentOffset_tmp, new PointLatLng(0.0, 0.0));
                // greater than 100 m
                if (dist_diff.CompareTo(100.0) > 0)
                {
                    if (MessageBox.Show("危险：地图纠偏值大于100米！是否采用本次校准结果?", "使用此纠偏值",
        MessageBoxButtons.OKCancel) != DialogResult.OK)
                    {
                        return;
                    }
                }

                currentOffset = new PointLatLng(offset_lat, offset_lng);

                log.Info("加载地图纠偏值完成:\n\n纬度偏差：" + offset_lat + " \n经度偏差: " + offset_lng + "\n距离偏移： " + dist_diff * 1000.0 + " 米\n");

                MessageBox.Show("加载地图纠偏值完成:\n\n纬度偏差：" + offset_lat + " 度 \n经度偏差: " + offset_lng + "度 \n距离偏移： " + dist_diff * 1000.0 + " 米\n", "地图纠偏完成");

            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("未发现上次保存的地图纠偏值，请重新执行 <地图手动纠偏>.", "错误：地图纠偏文件未找到");
            }

        }

        private void lb_save_poly_Click(object sender, EventArgs e)
        {
            savePolygonToolStripMenuItem_Click(sender, e);
        }

        public void lb_load_poly_Click(object sender, EventArgs e)
        {
            loadPolygonToolStripMenuItem_Click(sender, e);
        }

        private void cb_en_autoTORTL_CheckedChanged(object sender, EventArgs e)
        {
            var cl_tmp = GetCommandList();

            // if ((cl_tmp.Count == 0) && this.cb_en_autoTORTL.Checked)
            if (cl_tmp.Count == 0)
            {
                if(this.cb_en_autoTORTL.Checked)
                {
                    MessageBox.Show("请先设计自动航线，设计完后再选择本功能");
                    this.cb_en_autoTORTL.Checked = false; //  !this.cb_en_autoTORTL.Checked;
                }

                return;
            }

            if (this.cb_en_autoTORTL.Checked)
            {

                var first_wp = cl_tmp[0];
                if (first_wp.id == (ushort)(MAVLink.MAV_CMD.TAKEOFF))
                {
                    
                    {
                        MessageBox.Show("自动航线中已经添加了自动起飞功能，请勿重复添加!");
                        // this.cb_en_autoTORTL.Checked = false;
                        autoTORTL_added = true;
                        return;
                    }
                }

                if (!autoTORTL_added)
                {
                    // makesure get first wp
                    for(UInt16 ii = 0; ii < cl_tmp.Count; ii++)
                    {
                        first_wp = cl_tmp[ii];
                        if (first_wp.id == (ushort)(MAVLink.MAV_CMD.WAYPOINT))
                        {
                            break;
                        }

                    }

                    // var first_wp = cl_tmp[0];

                    // alt valid
                    if (((float)tb_autoTO_hgt.Value).CompareTo(first_wp.alt) == -1)
                    {
                        if (MessageBox.Show("请务必注意：起飞点高度比第一个作业航点高度低，可能存在风险,是否使用该高度？", "强制使用",
            MessageBoxButtons.OKCancel) != DialogResult.OK)
                        {
                            return;
                        }
                    }

                    commands_list_bk = GetCommandList();
                    // add takeoff wp
                    MainV2.instance.FlightPlanner.InsertCommand(0, MAVLink.MAV_CMD.TAKEOFF, 20, 0, 0, 0, 0, 0,
                        (float)tb_autoTO_hgt.Value, null, (byte)mission_item_info.LABEL_AUTOTO_WP);
                    // add pre-work wp
                    // take 1st waypoint xyz, then change z to takeoff_z, then add

                    MainV2.instance.FlightPlanner.InsertCommand(1, MAVLink.MAV_CMD.WAYPOINT, 0, 0, 0, 0, first_wp.lng, first_wp.lat,
                        (float)tb_autoTO_hgt.Value, null, (byte)mission_item_info.LABEL_AUTOTO_WP); // 0x20 is magic num for autoTO wp tag
                    // add pre-RTL wp
                    // insert the last wp
                    // temply to set RTL_ALT
                    // TODO: 1. using mavlink msg in FC src code
                    //       2. auto-choose nearest old wp-line as a virtual wp-line, then keep working-hgt fly from last user wp on this virtual line, then RTL 
                    MainV2.comPort.setParam("RTL_ALT", (float)tb_RTL_hgt.Value * 100.0f);
                    MainV2.instance.FlightPlanner.InsertCommand(commands_list_bk.Count + 2, MAVLink.MAV_CMD.RETURN_TO_LAUNCH, 0, 0, 0, 0, 0, 0,
                        0, null);
                    autoTORTL_added = true;
                }
            }
            else
            {

                var first_wp = cl_tmp[0];
                if (first_wp.id != (ushort)(MAVLink.MAV_CMD.TAKEOFF))
                {

                    {
                        MessageBox.Show("自动航线中尚未添加自动起飞功能，无法删除!");
                        // this.cb_en_autoTORTL.Checked = false;
                        autoTORTL_added = false;
                        return;
                    }
                }

                if (autoTORTL_added) //  delete wps
                {
                    try
                    {
                        quickadd = true;
                        // mono fix
                        Commands.CurrentCell = null;
                        Commands.Rows.RemoveAt(0); // takeoff
                        Commands.Rows.RemoveAt(0); // virtual wp
                        Commands.Rows.RemoveAt(cl_tmp.Count - 1 - 2); // RTL

                        quickadd = false;
                        writeKML();
                        autoTORTL_added = false;
                        // Commands.Rows.RemoveAt(2); // home is 0
                        //Commands.Rows.RemoveAt(commands_list_bk.Count + 2); // home is 0
                    }
                    catch
                    {
                        MessageBox.Show("取消一键起飞、一键返航失败！");
                    }
                }
            }
            
        }

        private void btn_wphgt_mod_Click(object sender, EventArgs e)
        {
            btn_wphgt_mod.Enabled = false;
            this.rbtn_modall.Text = "全部航点(1-" + Commands.RowCount + ")";
            this.num_endWP.Maximum = Commands.RowCount;
            this.num_startWP.Maximum = Commands.RowCount;
            form_modWPhgt.Show();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            form_modWPhgt.Hide();
            btn_wphgt_mod.Enabled = true;
        }

        private void btn_modconfirm_Click(object sender, EventArgs e)
        {
            // modify
            // get WP range
            Int32 startWPno, endWPno;

            List<Locationwp> cmd_list = GetCommandList();
            Int32 total_cmd = cmd_list.Count;


            // if ((cl_tmp.Count == 0) && this.cb_en_autoTORTL.Checked)
            if (total_cmd == 0)
            {
                MessageBox.Show("请先设计航线，设计完后再选择本功能");
                goto exit_modconfirm;
            }


            if (mod_type == "allWP")
            {
                startWPno = 1;
                endWPno = total_cmd;
            }
            else if(mod_type == "selectWP")
            {
                if((num_startWP.Value < cmd_list.Count) 
                    && (num_endWP.Value <= total_cmd) 
                    && (num_startWP.Value <= num_endWP.Value))
                {
                    startWPno = (Int32)num_startWP.Value;
                    endWPno = (Int32)num_endWP.Value;
                }
                else
                {
                    MessageBox.Show("航点范围无效，请重新检查航点范围:\n\n 1. 起始航点不能大于结束航点;\n\n 2. 结束航点不能大于总航点个数" + total_cmd);
                    return;
                    //goto exit_modconfirm;
                }
            }
            else
            {
                return;
            }

            // modify hgt
            for(Int32 idx = (startWPno - 1); idx < endWPno; idx++)
            {
                // autoTO WP ignore
                if(Commands.Rows[idx].Cells[Lat.Index].Tag != null)
                {
                    byte options_tmp = (byte)Commands.Rows[idx].Cells[Lat.Index].Tag;
                    if((options_tmp & (byte)mission_item_info.LABEL_AUTOTO_WP) != 0x0)
                    {
                        continue;
                    }
                }

                // make sure is waypoint
                if(Commands.Rows[idx].Cells[Command.Index].Value.ToString() == "WAYPOINT")
                {
                    Commands.Rows[idx].Cells[Alt.Index].Value = (int)num_newhgt.Value;
                }
                
            }


exit_modconfirm:
            form_modWPhgt.Hide();
            btn_wphgt_mod.Enabled = true;
        }

        private void rbtn_modall_CheckedChanged(object sender, EventArgs e)
        {
            if(rbtn_modall.Checked)
            {
                mod_type = "allWP";
            }
        }

        private void rbtn_sel_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtn_sel.Checked)
            {
                mod_type = "selectWP";
            }
        }

        private void wp_migrate_Click(object sender, EventArgs e)
        {

            if(this.wp_migrate.Text.Equals("航点实时迁移"))
            {
                if (MessageBox.Show("首先，点击后地面站会将当前连接的飞行器的航点读取上来，\n然后不要关闭地面站，将另一个飞行器连接，再点击“下发迁移”按钮即完成航点迁移", "航点实时迁移：这是高级调试功能，不熟悉此功能的人员慎点！") ==
            System.Windows.Forms.DialogResult.OK)
                {

                    // download wps list
                    BUT_read_Click(sender, e);
                    var cl_tmp = GetCommandList();
                    if (cl_tmp.Count == 0)
                    {
                        MessageBox.Show("请先设计自动航线，设计完后再选择本功能");

                        return;
                    }

                    // get mission type & mission interrupt idx
                    this.mission_type = (Int16)MainV2.comPort.GetParam("MIS_TYPE");
                    this.resume_wp_idx = (Int16)MainV2.comPort.GetParam("MIS_LAST_IDX");
                    this.resume_wp_loc_x = (Int32)MainV2.comPort.GetParam("MIS_RES_PNT_X");
                    this.resume_wp_loc_y = (Int32)MainV2.comPort.GetParam("MIS_RES_PNT_Y");
                    this.resume_wp_loc_z = (Int32)MainV2.comPort.GetParam("MIS_RES_PNT_Z");
                }
                this.wp_migrate.Text = "下发迁移";
            }
            else if (this.wp_migrate.Text.Equals("下发迁移"))
            {
                try
                {

                    int try_times = 0;
                    for (try_times = 0; try_times < 5; try_times++)
                    {

                        // set mission type & mission interrupt idx
                        MainV2.comPort.setParam("MIS_TYPE", this.mission_type);
                        MainV2.comPort.setParam("MIS_LAST_IDX", this.resume_wp_idx);
                        MainV2.comPort.setParam("MIS_RES_PNT_X", this.resume_wp_loc_x);
                        MainV2.comPort.setParam("MIS_RES_PNT_Y", this.resume_wp_loc_y);
                        MainV2.comPort.setParam("MIS_RES_PNT_Z", this.resume_wp_loc_z);
                        Thread.Sleep(1000);

                        if (Convert.ToInt32(MainV2.comPort.GetParam("MIS_TYPE")).Equals(this.mission_type))
                        {
                            if (Convert.ToInt32(MainV2.comPort.GetParam("MIS_LAST_IDX")).Equals(this.resume_wp_idx))
                            {
                                if (Convert.ToInt32(MainV2.comPort.GetParam("MIS_RES_PNT_X")).Equals(this.resume_wp_loc_x))
                                {
                                    if (Convert.ToInt32(MainV2.comPort.GetParam("MIS_RES_PNT_Y")).Equals(this.resume_wp_loc_y))
                                    {
                                        if (Convert.ToInt32(MainV2.comPort.GetParam("MIS_RES_PNT_Z")).Equals(this.resume_wp_loc_z))
                                        {
                                            break;
                                        }
                                    }
                                }
                            }

                        }

                    }

                    if (5 == try_times)
                    {
                        MessageBox.Show("抱歉，航点实时迁移失败，请尝试重新连接飞控后，再次迁移!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                catch
                {
                    MessageBox.Show("抱歉，航点实时迁移失败，请尝试重新连接飞控后，再次迁移!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                var cl_tmp = GetCommandList();
                if (cl_tmp.Count == 0)
                {
                    MessageBox.Show("请先设计自动航线，设计完后再选择本功能", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

                // send wps list 
                BUT_write_Click(sender, e);

                this.wp_migrate.Text = "航点实时迁移";

                MessageBox.Show("恭喜，航点实时迁移成功!", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }

        }

        private void lb_download_map_Click(object sender, EventArgs e)
        {
            prefetchToolStripMenuItem_Click(sender, e);
        }

        // Map Operators
        private void map_OnRouteEnter(GMapRoute item)
        {
            // no need in measure mode
            if (!startmeasure.IsEmpty)
            {
                return;
            }

            string dist;
            string DistUnits = "";
            if (DistUnits == "Feet")
            {
                dist = ((float)item.Distance * 3280.84f).ToString("0.##") + " ft";
            }
            else
            {
                dist = ((float)item.Distance * 1000f).ToString("0.##") + " 米";
            }

            if (marker_routes != null)
            {
                if (routesoverlay.Markers.Contains(marker_routes))
                    routesoverlay.Markers.Remove(marker_routes);
            }

            orig_color = item.Stroke.Color;
            // item.Stroke.Color = Color.Blue;

            ttpSettings.IsBalloon = true;

            marker_routes = new GMapMarkerRect(currentMousePosition);
            marker_routes.ToolTip = new GMapToolTip(marker_routes);
            marker_routes.ToolTipMode = MarkerTooltipMode.Always;
            marker_routes.ToolTipText = "此段航程: " + dist;
            Point adjust_pnt = new System.Drawing.Point(currMousePoint.X, currMousePoint.Y - 40);

            ttpSettings.Show(marker_routes.ToolTipText, this, adjust_pnt);

            // routesoverlay.Markers.Add(marker_routes);
        }

        private void map_OnRouteLeave(GMapRoute item)
        {
            if (marker_routes != null)
            {
                try
                {
                    if (routesoverlay.Markers.Contains(marker_routes))
                        routesoverlay.Markers.Remove(marker_routes);

                    orig_color = item.Stroke.Color;
                    // item.Stroke.Color = orig_color;
                    ttpSettings.Hide(this);
                }
                catch { }
            }
            
        }

        private void map_OnPolygonLeave(GMapPolygon item)
        {
            if (item != null)
            {
                item.Fill = new SolidBrush(Color.FromArgb(50, Color.Red));
                item.Stroke = new Pen(Color.Red, 1);
            }
        }

        private void map_OnPolygonEnter(GMapPolygon item)
        {

            if (item != null)
            {
                item.Fill = new SolidBrush(Color.FromArgb(50, Color.Green));
                item.Stroke = new Pen(Color.Green, 1);
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            // searchResult.Clear();
            // searchoverlay.Markers.Clear();

            gp = GMapProviders.AMapStatelite as GeocodingProvider;
            GMapProvider.Language = LanguageType.ChineseSimplified; //使用的语言，默认是英文

            string searchStr = this.tb_place.Text;
            GeoCoderStatusCode status = MainMap.SetPositionByKeywords(searchStr);
            if (status != GeoCoderStatusCode.G_GEO_SUCCESS)
            {
                MessageBox.Show("抱歉，搜索不到 '" + searchStr + "', 请重试或者换个地名尝试");
            }
            else
            {
                MainMap.Zoom = 15;
                // foreach (PointLatLng point in searchResult)
                // {
                //     GMarkerGoogle marker = new GMarkerGoogle(point, GMarkerGoogleType.arrow);
                //     GMapMarkerRect rect = new GMapMarkerRect(point);
                //     PointLatLng tmp_pnt = point;
                //     // searchoverlay.Markers.Add(marker);
                // }
                // MainMap.ZoomAndCenterMarkers(searchoverlay.Id);
                // MainMap.ZoomAndCenterMarkers(drawnpolygonsoverlay.Id);
                
            }


        }

        private void lb_search_map_Click(object sender, EventArgs e)
        {
            buttonSearch_Click(sender, e);
        }
        
        private void tb_place_enter(object sender, EventArgs e)
        {
            // buttonSearch_Click(sender, e);
        }

        private void rb_center_Click(object sender, EventArgs e)
        {
            label4_LinkClicked(sender, null);
        }
        
        private void rb_en_lidar_Click(object sender, EventArgs e)
        {

            if (MainV2.connectOK)
            {
                bool en_lidar = ((int)MainV2.comPort.GetParam("SERIAL2_PROTOCOL")) == (9);
                float param_val = 0;
                if(en_lidar) // enable already, now disable
                {
                    rb_en_lidar.BackColor = Color.IndianRed;
                    param_val = 0;
                }
                else
                {
                    rb_en_lidar.BackColor = Color.Lime;
                    param_val = 9;
                }
                int try_times = 0;
                for (try_times = 0; try_times < 5; try_times++)
                {

                    if (MainV2.comPort.setParam("SERIAL2_PROTOCOL", param_val))
                    {
                        if(((int)param_val == 9))
                        {
                            currentaltmode = altmode.Terrain;
                            MessageBox.Show("使能地形跟随功能成功!");

                        }
                        else
                        {
                            currentaltmode = altmode.Relative;
                            MessageBox.Show("地形跟随已关闭!");

                        }

                        break;

                    }

                    if (5 == try_times)
                    {
                        MessageBox.Show("抱歉，使能地形跟随功能失败，请尝试重新配置，或者重新连接飞控后再次尝试!");
                    }
                }


            }

        }
        private void rb_clear_mission_Click(object sender, EventArgs e)
        {
            clearMissionToolStripMenuItem_Click(sender, e);
        }
        private void rb_clear_polygon_Click(object sender, EventArgs e)
        {
            // clear polygon
            clearPolygonToolStripMenuItem_Click(sender, e);

        }
        private void rb_autoWP_Click(object sender, EventArgs e)
        {
            BUT_generate_Click(sender, e);

        }
        private void rb_inv_wps_Click(object sender, EventArgs e)
        {
            reverseWPsToolStripMenuItem_Click(sender, e);

        }

#if false
        PointLatLng[] testPnts = { new PointLatLng(34.1209534, 108.8777304), new PointLatLng(34.120865,  108.879619), new PointLatLng(34.119515, 108.879726), new PointLatLng(34.119355, 108.877666) };
#endif

        bool display_pannel_avoidPnts = true;
        private void rb_avoid_pnts_Click(object sender, EventArgs e)
        {
            avoidBoundsList.Clear();

            if (display_pannel_avoidPnts)
            {
                this.panel_avoidPnts.Show();
                this.dataGridView_avoidPnts.Show();
            }
            else
            {
                this.panel_avoidPnts.Hide();
                this.dataGridView_avoidPnts.Hide();
            }
            display_pannel_avoidPnts = !display_pannel_avoidPnts;

        }

        private int avoidPntsIdx = 0;

        private void rb_avoidPntsAdd_Click(object sender, EventArgs e)
        {

            avoidPntsIdx = dataGridView_avoidPnts.Rows.Add();

            updateAvoidPntsRowNumbers();
            // BUT_Add_Click(sender, e);
            // int idx = dataGridView_avoidPnts.Rows.Add();
            // just for testing
            // dataGridView_avoidPnts.Rows[avoidPntsIdx].Cells[avoidPntLng.Index].Value = testPnts[avoidPntsIdx%4].Lng;
            // dataGridView_avoidPnts.Rows[avoidPntsIdx].Cells[avoidPntLat.Index].Value = testPnts[avoidPntsIdx%4].Lat;

            // default as 10 m
            dataGridView_avoidPnts.Rows[avoidPntsIdx].Cells[avoidPntRadius.Index].Value = 10;
            dataGridView_avoidPnts.Rows[avoidPntsIdx].Cells[Manage.Index].Value = "删除本点";
            // avoidPntsIdx++;
        }

        private void rb_avoidPntsOk_Click(object sender, EventArgs e)
        {
            avoidPntsoverlay.Polygons.Clear();
            avoidBoundsList.Clear();

            var avoidcommandlist = GetAvoidCommandList();

            // for each avoid point, generate bound and display it in mainmap
            foreach (var avoidP in avoidcommandlist)
            {

                PointLatLng pos = new PointLatLng(avoidP.lat, avoidP.lng);
                if (pos == PointLatLng.Empty)
                {
                    continue;
                }

                squareBound sqBnd = new squareBound(pos, 180, avoidP.alt); // borrow alt as radius
                avoidBoundsList.Add(sqBnd);

                // display
                GMarkerGoogle gmm = new GMarkerGoogle(pos, GMarkerGoogleType.red_dot);
                // gmm.Tag = CurrentSplitMarker.Tag;
                // splitoverlay.Markers[idx] = gmm;

                List<PointLatLng> polygonPoints = sqBnd.bounds;

                GMapPolygon line = new GMapPolygon(polygonPoints, "avoid poly");
                line.Stroke.Color = Color.Transparent;
                line.Fill = Brushes.Red;
                // avoidPntsoverlay.Polygons.Clear();
                avoidPntsoverlay.Polygons.Add(line);

                // MainMap.Invalidate();
            }

            //RegeneratePolygon();
            // AB ZhaoYJ@2018-04-19 for avoid points
            // Idea: foreach all of the wp lines, then cross to avoidPolys, then get the last fullpoints 
            List<Locationwp> wproute = new List<Locationwp>();
            // add home first
            Locationwp home = new Locationwp();
            try
            {
                home.id = (ushort)MAVLink.MAV_CMD.WAYPOINT;
                home.lat = (double.Parse(TXT_homelat.Text));
                home.lng = (double.Parse(TXT_homelng.Text));
                home.alt = (float.Parse(TXT_homealt.Text) / CurrentState.multiplierdist); // use saved home
            }
            catch { }

            wproute.Add(home);

            if (AvoidPointsAdd(ref wproute))
            {
                processToScreen(wproute, false);
            }


            this.panel_avoidPnts.Hide();
            this.dataGridView_avoidPnts.Hide();
            display_pannel_avoidPnts = true; // for rb_avoid_pnts_Click

        }



        // 
    }
}