/// AB ZhaoYJ@2017-03-11
/// for: default enable speech
/// TODO: make sure it's stable
/// start
// #define SPEECH_EN
/// end
/// 
using PrimaryFlightDisplay;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Collections;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Linq;
using System.Threading;
using MissionPlanner.Utilities;
using IronPython.Hosting;
using log4net;
using MissionPlanner.Controls;
using MissionPlanner.Comms;
using MissionPlanner.Log;
using Transitions;
using MissionPlanner.Warnings;
using System.Collections.Concurrent;
using MissionPlanner.LogList;
using FileExplorer;
using ZedGraph;
using System.Reflection;

namespace MissionPlanner
{
    public partial class MainV2 : Form
    {
        private static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        int tickStart;
        RollingPointPairList list1 = new RollingPointPairList(1200);
        RollingPointPairList list2 = new RollingPointPairList(1200);
        RollingPointPairList list3 = new RollingPointPairList(1200);
        RollingPointPairList list4 = new RollingPointPairList(1200);
        RollingPointPairList list5 = new RollingPointPairList(1200);
        RollingPointPairList list6 = new RollingPointPairList(1200);
        RollingPointPairList list7 = new RollingPointPairList(1200);
        RollingPointPairList list8 = new RollingPointPairList(1200);
        RollingPointPairList list9 = new RollingPointPairList(1200);
        RollingPointPairList list10 = new RollingPointPairList(1200);

        PropertyInfo list1item;
        PropertyInfo list2item;
        PropertyInfo list3item;
        PropertyInfo list4item;
        PropertyInfo list5item;
        PropertyInfo list6item;
        PropertyInfo list7item;
        PropertyInfo list8item;
        PropertyInfo list9item;
        PropertyInfo list10item;

        CurveItem list1curve;
        CurveItem list2curve;
        CurveItem list3curve;
        CurveItem list4curve;
        CurveItem list5curve;
        CurveItem list6curve;
        CurveItem list7curve;
        CurveItem list8curve;
        CurveItem list9curve;
        CurveItem list10curve;

        public static int di_status = 0;

        private static class NativeMethods
        {
            // used to hide/show console window
            [DllImport("user32.dll")]
            public static extern int FindWindow(string szClass, string szTitle);

            [DllImport("user32.dll")]
            public static extern int ShowWindow(int Handle, int showState);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            internal static extern IntPtr RegisterDeviceNotification
                (IntPtr hRecipient,
                    IntPtr NotificationFilter,
                    Int32 Flags);

            // Import SetThreadExecutionState Win32 API and necessary flags

            [DllImport("kernel32.dll")]
            public static extern uint SetThreadExecutionState(uint esFlags);

            public const uint ES_CONTINUOUS = 0x80000000;
            public const uint ES_SYSTEM_REQUIRED = 0x00000001;

            static public int SW_SHOWNORMAL = 1;
            static public int SW_HIDE = 0;
            // [DllImport("MyColoredLogListBoxDll.dll")]
            // public MyColoredLogListBox 
        }

        public static menuicons displayicons = new burntkermitmenuicons();

        public enum Motor_frame_class
        {
            MOTOR_FRAME_UNDEFINED = 0,
            MOTOR_FRAME_QUAD = 1,
            MOTOR_FRAME_HEXA = 2,
            MOTOR_FRAME_OCTA = 3,
            MOTOR_FRAME_OCTAQUAD = 4,
            MOTOR_FRAME_Y6 = 5,
            MOTOR_FRAME_HELI = 6,
            MOTOR_FRAME_TRI = 7,
            MOTOR_FRAME_SINGLE = 8,
            MOTOR_FRAME_COAX = 9,
            MOTOR_FRAME_TAILSITTER = 10,
            MOTOR_FRAME_HELI_DUAL = 11,
            MOTOR_FRAME_DODECAHEXA = 12,
            MOTOR_FRAME_HELI_QUAD = 13,
        };
        public enum Motor_frame_type
        {
            MOTOR_FRAME_TYPE_PLUS = 0,
            MOTOR_FRAME_TYPE_X = 1,
            MOTOR_FRAME_TYPE_V = 2,
            MOTOR_FRAME_TYPE_H = 3,
            MOTOR_FRAME_TYPE_VTAIL = 4,
            MOTOR_FRAME_TYPE_ATAIL = 5,
            MOTOR_FRAME_TYPE_Y6B = 10,
            MOTOR_FRAME_TYPE_Y6F = 11 // for FireFlyY6
        };


        public abstract class menuicons
        {
            public abstract Image fd { get; }
            public abstract Image fp { get; }
            public abstract Image initsetup { get; }
            public abstract Image config_tuning { get; }
            public abstract Image sim { get; }
            public abstract Image terminal { get; }
            public abstract Image help { get; }
            public abstract Image donate { get; }
            public abstract Image connect { get; }
            public abstract Image disconnect { get; }
            public abstract Image bg { get; }
            public abstract Image wizard { get; }
            public abstract Image motor { get; }
        }

        public class burntkermitmenuicons : menuicons
        {
            public override Image fd
            {
                get { return global::MissionPlanner.Properties.Resources.light_flightdata_icon; }
            }

            public override Image fp
            {
                get { return global::MissionPlanner.Properties.Resources.light_flightplan_icon; }
                // get { return global::MissionPlanner.Properties.Resources.small_user; }
            }

            public override Image initsetup
            {
                get { return global::MissionPlanner.Properties.Resources.light_initialsetup_icon; }
            }

            public override Image config_tuning
            {
                get { return global::MissionPlanner.Properties.Resources.light_tuningconfig_icon; }
            }

            public override Image sim
            {
                get { return global::MissionPlanner.Properties.Resources.light_simulation_icon; }
            }

            public override Image terminal
            {
                get { return global::MissionPlanner.Properties.Resources.light_terminal_icon; }
            }

            public override Image help
            {
                get { return global::MissionPlanner.Properties.Resources.light_help_icon; }
            }

            public override Image donate
            {
                get { return global::MissionPlanner.Properties.Resources.donate; }
            }

            public override Image connect
            {
                //get { return global::MissionPlanner.Properties.Resources.light_connect_icon; }
                get { return global::MissionPlanner.Properties.Resources.connect_start; }
            }

            public override Image disconnect
            {
                get { return global::MissionPlanner.Properties.Resources.connect_done; }
            }

            public override Image bg
            {
                get { return global::MissionPlanner.Properties.Resources.bgdark; }
            }
            public override Image wizard
            {
                get { return global::MissionPlanner.Properties.Resources.wizardicon; }
            }
            public override Image motor
            {
                get { return global::MissionPlanner.Properties.Resources.motor_zoom; }
            }
        }

        public class highcontrastmenuicons : menuicons
        {
            public override Image fd
            {
                get { return global::MissionPlanner.Properties.Resources.dark_flightdata_icon; }
            }

            public override Image fp
            {
                get { return global::MissionPlanner.Properties.Resources.dark_flightplan_icon; }
            }

            public override Image initsetup
            {
                get { return global::MissionPlanner.Properties.Resources.dark_initialsetup_icon; }
            }

            public override Image config_tuning
            {
                get { return global::MissionPlanner.Properties.Resources.dark_tuningconfig_icon; }
            }

            public override Image sim
            {
                get { return global::MissionPlanner.Properties.Resources.dark_simulation_icon; }
            }

            public override Image terminal
            {
                get { return global::MissionPlanner.Properties.Resources.dark_terminal_icon; }
            }

            public override Image help
            {
                get { return global::MissionPlanner.Properties.Resources.dark_help_icon; }
            }

            public override Image donate
            {
                get { return global::MissionPlanner.Properties.Resources.donate; }
            }

            public override Image connect
            {
                get { return global::MissionPlanner.Properties.Resources.connect_start; }
            }

            public override Image disconnect
            {
                get { return global::MissionPlanner.Properties.Resources.connect_done; }
            }

            public override Image bg
            {
                get { return null; }
            }
            public override Image wizard
            {
                get { return global::MissionPlanner.Properties.Resources.wizardicon; }
            }
            public override Image motor
            {
                get { return global::MissionPlanner.Properties.Resources.motor_zoom; }
            }
        }

        Controls.MainSwitcher MyView;

        private static DisplayView _displayConfiguration = new DisplayView().Basic();

        public static event EventHandler LayoutChanged;

        public static DisplayView DisplayConfiguration
        {
            get { return _displayConfiguration; }
            set
            {
                _displayConfiguration = value;
                if (LayoutChanged != null)
                    LayoutChanged(null, EventArgs.Empty);
            }
        }

        
        public static bool ShowAirports { get; set; }
        public static bool ShowTFR { get; set; }

        private Utilities.adsb _adsb;

        public bool EnableADSB
        {
            get { return _adsb != null; }
            set
            {
                if (value == true)
                {
                    _adsb = new Utilities.adsb();

                    if (Settings.Instance["adsbserver"] != null)
                        Utilities.adsb.server = Settings.Instance["adsbserver"];
                    if (Settings.Instance["adsbport"] != null)
                        Utilities.adsb.serverport = int.Parse(Settings.Instance["adsbport"].ToString());
                }
                else
                {
                    Utilities.adsb.Stop();
                    _adsb = null;
                }
            }
        }

        //public static event EventHandler LayoutChanged;

        /// <summary>
        /// Active Comport interface
        /// </summary>
        public static MAVLinkInterface comPort
        {
            get
            {
                return _comPort;
            }
            set {
                if (_comPort == value)
                    return;
                _comPort = value;
                _comPort.MavChanged -= instance.comPort_MavChanged;
                _comPort.MavChanged += instance.comPort_MavChanged;
                instance.comPort_MavChanged(null, null);
            }
        }

        static MAVLinkInterface _comPort = new MAVLinkInterface();

        /// <summary>
        /// passive comports
        /// </summary>
        public static List<MAVLinkInterface> Comports = new List<MAVLinkInterface>();

        public delegate void WMDeviceChangeEventHandler(WM_DEVICECHANGE_enum cause);

        public event WMDeviceChangeEventHandler DeviceChanged;

        /// <summary>
        /// other planes in the area from adsb
        /// </summary>
        internal object adsblock = new object();

        public ConcurrentDictionary<string,adsb.PointLatLngAltHdg> adsbPlanes = new ConcurrentDictionary<string, adsb.PointLatLngAltHdg>();

        string titlebar;

 
        /// <summary>
        /// Comport name
        /// </summary>
        public static string comPortName = "";

        /// <summary>
        /// mono detection
        /// </summary>
        public static bool MONO = false;

        /// <summary>
        /// speech engine enable
        /// </summary>
        public static bool speechEnable = false;

        /// <summary>
        /// spech engine static class
        /// </summary>
        public static Speech speechEngine = null;

        public static ListBoxLog lblog = null; //  = new ListBoxLog(listBox_log);

        /// <summary>
        /// joystick static class
        /// </summary>
        public static Joystick.Joystick joystick = null;

        /// <summary>
        /// track last joystick packet sent. used to control rate
        /// </summary>
        DateTime lastjoystick = DateTime.Now;

        /// <summary>
        /// determine if we are running sitl
        /// </summary>
        public static bool sitl
        {
            get
            {
                if (MissionPlanner.Controls.SITL.SITLSEND == null) return false;
                if (MissionPlanner.Controls.SITL.SITLSEND.Client.Connected) return true;
                return false;
            }
        }

        /// <summary>
        /// hud background image grabber from a video stream - not realy that efficent. ie no hardware overlays etc.
        /// </summary>
        public static WebCamService.Capture cam = null;

        /// <summary>
        /// controls the main serial reader thread
        /// </summary>
        bool serialThread = false;

        bool pluginthreadrun = false;
        bool tuningthreadrun = false;
        bool joystickthreadrun = false;

        Thread httpthread;
        // Thread tuningthread;
        Thread joystickthread;
        Thread serialreaderthread;
        Thread pluginthread;

        /// <summary>
        /// track the last heartbeat sent
        /// </summary>
        private DateTime heatbeatSend = DateTime.Now;

        /// <summary>
        /// used to call anything as needed.
        /// </summary>
        public static MainV2 instance = null;


        public static MainSwitcher View;

        /// <summary>
        /// store the time we first connect
        /// </summary>
        DateTime connecttime = DateTime.Now;

        DateTime nodatawarning = DateTime.Now;
        DateTime OpenTime = DateTime.Now;

        /// <summary>
        /// enum of firmwares
        /// </summary>
        public enum Firmwares
        {
            ArduPlane,
            ArduCopter2,
            ArduRover,
            Ateryx,
            ArduTracker,
            Gymbal,
            PX4
        }

        DateTime connectButtonUpdate = DateTime.Now;

        /// <summary>
        /// declared here if i want a "single" instance of the form
        /// ie configuration gets reloaded on every click
        /// </summary>
        public GCSViews.FlightData FlightData;

        public GCSViews.FlightPlanner FlightPlanner;
        Controls.SITL Simulation;

        private Form connectionStatsForm;
        private ConnectionStats _connectionStats;

        /// <summary>
        /// This 'Control' is the toolstrip control that holds the comport combo, baudrate combo etc
        /// Otiginally seperate controls, each hosted in a toolstip sqaure, combined into this custom
        /// control for layout reasons.
        /// </summary>
        static internal ConnectionControl _connectionControl;

        // AB ZhaoYJ@2017-02-23 for auto-conn FC: default TXT is AUTO
        // ========= start =========
        enum connectState : ushort {KO = 0xF, WIP, OK};
        connectState connState;
        public static bool connectOK { get; set; }

        private static Int32 timer1_tick_cnt;
        private static bool timer1_tick_first = true;
        public static string comm_type = "";
        // ========= end ========= 

        private PrimaryFlightDisplay.PFDControl pfdControl1;

        public static bool TerminalTheming = true;

        public void updateLayout(object sender, EventArgs e)
        {
            MenuSimulation.Visible = DisplayConfiguration.displaySimulation;
            MenuTerminal.Visible = DisplayConfiguration.displayTerminal;
            //MenuDonate.Visible = DisplayConfiguration.displayDonate;
            MenuHelp.Visible = false;
            MissionPlanner.Controls.BackstageView.BackstageView.Advanced = DisplayConfiguration.isAdvancedMode;

            if (_connectionControl.CMB_baudrate != null && _connectionControl.CMB_serialport != null)
            {
                _connectionControl.CMB_baudrate.Visible = DisplayConfiguration.displayBaudCMB;
                if (!_connectionControl.CMB_baudrate.Visible)
                {
                    int index = _connectionControl.CMB_baudrate.FindStringExact ("115200");      //115200 Baud
                    _connectionControl.CMB_baudrate.SelectedIndex = index;
                }
                _connectionControl.CMB_serialport.Visible = DisplayConfiguration.displaySerialPortCMB;
                if (!_connectionControl.CMB_serialport.Visible)
                {
                    // AB ZhaoYJ@2017-02-23 for auto-conn FC: default TXT is AUTO
                    // ========= start =========
                    int index = _connectionControl.CMB_serialport.FindStringExact("AUTO");
                    // int index = _connectionControl.CMB_serialport.FindStringExact("UDP");
                    // ========= end =========
                    _connectionControl.CMB_serialport.SelectedIndex = index;

                }
            }

            if (MainV2.instance.FlightData != null)
            {
                
                TabControl t = MainV2.instance.FlightData.tabControlactions;

                if (DisplayConfiguration.displayAdvActionsTab && !t.TabPages.Contains(FlightData.tabActions))
                {
                    t.TabPages.Add(FlightData.tabActions);
                }
                else if (!DisplayConfiguration.displayAdvActionsTab && t.TabPages.Contains(FlightData.tabActions))
                {
                    t.TabPages.Remove(FlightData.tabActions);
                }
                if (DisplayConfiguration.displaySimpleActionsTab && !t.TabPages.Contains(FlightData.tabActionsSimple))
                {
                    /// AB ZhaoYJ@2017-03-18
                    /// for: try to disable action simple
                    /// start
                    // t.TabPages.Add(FlightData.tabActionsSimple);
                    /// end
                }
                else if (!DisplayConfiguration.displaySimpleActionsTab && t.TabPages.Contains(FlightData.tabActionsSimple))
                {
                    t.TabPages.Remove(FlightData.tabActionsSimple);
                }
                if (DisplayConfiguration.displayStatusTab && !t.TabPages.Contains(FlightData.tabStatus))
                {
                    t.TabPages.Add(FlightData.tabStatus);
                }
                else if (!DisplayConfiguration.displayStatusTab && t.TabPages.Contains(FlightData.tabStatus))
                {
                    t.TabPages.Remove(FlightData.tabStatus);
                }
                if (DisplayConfiguration.displayServoTab && !t.TabPages.Contains(FlightData.tabServo))
                {
                    t.TabPages.Add(FlightData.tabServo);
                }
                else if (!DisplayConfiguration.displayServoTab && t.TabPages.Contains(FlightData.tabServo))
                {
                    t.TabPages.Remove(FlightData.tabServo);
                }
                if (DisplayConfiguration.displayScriptsTab && !t.TabPages.Contains(FlightData.tabScripts))
                {
                    t.TabPages.Add(FlightData.tabScripts);
                }
                else if (!DisplayConfiguration.displayScriptsTab && t.TabPages.Contains(FlightData.tabScripts))
                {
                    t.TabPages.Remove(FlightData.tabScripts);
                }
            }
        }
        

        public MainV2()
        {
            // log.Info("Mainv2 ctor");

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            // set this before we reset it
            Settings.Instance["NUM_tracklength"] = "200";

            // create one here - but override on load
            Settings.Instance["guid"] = Guid.NewGuid().ToString();

            // load config
            LoadConfig();

            // force language to be loaded
            L10N.GetConfigLang();

            ShowAirports = true;

            // setup adsb
            Utilities.adsb.UpdatePlanePosition += adsb_UpdatePlanePosition;

            Form splash = Program.Splash;

            splash.Refresh();

            Application.DoEvents();

            instance = this;

            //disable dpi scaling
            if (Font.Name != "宋体")
            {
                //Chinese displayed normally when scaling. But would be too small or large using this line of code.
                using (var g = CreateGraphics())
                {
                    Font = new Font(Font.Name, 8.25f*96f/g.DpiX, Font.Style, Font.Unit, Font.GdiCharSet,
                        Font.GdiVerticalFont);
                }
            }

            InitializeComponent();
            try
            {
                // ThemeManager.SetTheme((ThemeManager.Themes)Enum.Parse(typeof(ThemeManager.Themes), Settings.Instance["theme"]));
                ThemeManager.SetTheme(ThemeManager.Themes.BurntKermit);
            }
            catch
            {
                ThemeManager.SetTheme(ThemeManager.Themes.BurntKermit);
            }
            Utilities.ThemeManager.ApplyThemeTo(this);
            MyView = new MainSwitcher(this);

            View = MyView;

            lblog = new ListBoxLog(listBox_log);
            // this.listBox_log.Location


            //startup console
            // TCPConsole.Write((byte) 'S');

            // start listener
            // UDPVideoShim.Start();

            _connectionControl = toolStripConnectionControl.ConnectionControl;
            _connectionControl.CMB_baudrate.TextChanged += this.CMB_baudrate_TextChanged;
            _connectionControl.CMB_serialport.SelectedIndexChanged += this.CMB_serialport_SelectedIndexChanged;
            _connectionControl.CMB_serialport.Click += this.CMB_serialport_Click;
            _connectionControl.cmb_sysid.Click += cmb_sysid_Click;

            _connectionControl.ShowLinkStats += (sender, e) => ShowConnectionStatsForm();
            // AB ZhaoYJ@2017-02-23 for auto-conn FC: default TXT is AUTO
            // ========= start =========
            _connectionControl.Hide();

            connState = connectState.KO;
            connectOK = false;

            // ========= end ========= 
            srtm.datadirectory = Settings.GetDataDirectory() +
                                 "srtm";

            var t = Type.GetType("Mono.Runtime");
            MONO = (t != null);

            speechEngine = new Speech();

            Warnings.CustomWarning.defaultsrc = comPort.MAV.cs;
            Warnings.WarningEngine.Start();

            // proxy loader - dll load now instead of on config form load
            new Transition(new TransitionType_EaseInEaseOut(2000));

            foreach (object obj in Enum.GetValues(typeof (Firmwares)))
            {
                _connectionControl.TOOL_APMFirmware.Items.Add(obj);
            }

            if (_connectionControl.TOOL_APMFirmware.Items.Count > 0)
                _connectionControl.TOOL_APMFirmware.SelectedIndex = 0;

            comPort.BaseStream.BaudRate = 115200;

            // PopulateSerialportList();
            if (_connectionControl.CMB_serialport.Items.Count > 0)
            {
                _connectionControl.CMB_baudrate.SelectedIndex = 8;
                _connectionControl.CMB_serialport.SelectedIndex = 0;
            }
            // ** Done

            splash.Refresh();
            Application.DoEvents();

            string temp = Settings.Instance.ComPort;
            if (!string.IsNullOrEmpty(temp))
            {
                _connectionControl.CMB_serialport.SelectedIndex = _connectionControl.CMB_serialport.FindString(temp);
                if (_connectionControl.CMB_serialport.SelectedIndex == -1)
                {
                    _connectionControl.CMB_serialport.Text = temp; // allows ports that dont exist - yet
                }
                comPort.BaseStream.PortName = temp;
                comPortName = temp;
            }
            string temp2 = Settings.Instance.BaudRate;
            if (!string.IsNullOrEmpty(temp2))
            {
                _connectionControl.CMB_baudrate.SelectedIndex = _connectionControl.CMB_baudrate.FindString(temp2);
                if (_connectionControl.CMB_baudrate.SelectedIndex == -1)
                {
                    _connectionControl.CMB_baudrate.Text = temp2;
                }
            }
            string temp3 = Settings.Instance.APMFirmware;
            if (!string.IsNullOrEmpty(temp3))
            {
                _connectionControl.TOOL_APMFirmware.SelectedIndex =
                    _connectionControl.TOOL_APMFirmware.FindStringExact(temp3);
                if (_connectionControl.TOOL_APMFirmware.SelectedIndex == -1)
                    _connectionControl.TOOL_APMFirmware.SelectedIndex = 0;
                MainV2.comPort.MAV.cs.firmware =
                    (MainV2.Firmwares) Enum.Parse(typeof (MainV2.Firmwares), _connectionControl.TOOL_APMFirmware.Text);
            }

            MissionPlanner.Utilities.Tracking.cid = new Guid(Settings.Instance["guid"].ToString());

            // setup guids for droneshare
            if (!Settings.Instance.ContainsKey("plane_guid"))
                Settings.Instance["plane_guid"] = Guid.NewGuid().ToString();

            if (!Settings.Instance.ContainsKey("copter_guid"))
                Settings.Instance["copter_guid"] = Guid.NewGuid().ToString();

            if (!Settings.Instance.ContainsKey("rover_guid"))
                Settings.Instance["rover_guid"] = Guid.NewGuid().ToString();

            if (Settings.Instance.ContainsKey("language") && !string.IsNullOrEmpty(Settings.Instance["language"]))
            {
                changelanguage(CultureInfoEx.GetCultureInfo(Settings.Instance["language"]));
            }

            //Modified by HL below
            //--this.Text = splash.Text;
            //--titlebar = splash.Text;

            string GCS_main_name = "远智地面站 V2.23 (Build ";
            string GCS_name = GCS_main_name + "for debug)"; //   = "丰禾伟业地面站 V1.00 (Build " + DateTime.Now.ToString("yyyy.MM.dd") + ")";
            // Read the file and display it line by line.
            try
            {
                System.IO.StreamReader file =
                   new System.IO.StreamReader("./build.date");
                string build_date;

                if ((file != null) && ((build_date = file.ReadLine()) != null))
                {
                    GCS_name = GCS_main_name + build_date + ")";
                    file.Close();
                }
                else
                {
                    GCS_name = GCS_main_name + DateTime.Now.ToString("yyyy.MM.dd") + ")";
                }
            }
            catch (Exception ex)
            {

            }
          

            this.Text = GCS_name;
            titlebar = GCS_name;
            //Modified by HL above

            if (!MONO) // windows only
            {
                if (Settings.Instance["showconsole"] != null && Settings.Instance["showconsole"].ToString() == "True")
                {
#if !CAN_SHOW_CONSOLE
                    int win = NativeMethods.FindWindow("ConsoleWindowClass", null);
                    NativeMethods.ShowWindow(win, NativeMethods.SW_HIDE); // hide window
#endif
                }
                else
                {

                    int win = NativeMethods.FindWindow("ConsoleWindowClass", null);
                    NativeMethods.ShowWindow(win, NativeMethods.SW_HIDE); // hide window
                }

                // prevent system from sleeping while mp open
                var previousExecutionState =
                    NativeMethods.SetThreadExecutionState(NativeMethods.ES_CONTINUOUS | NativeMethods.ES_SYSTEM_REQUIRED);
            }

            ChangeUnits();

            if (Settings.Instance["theme"] != null)
            {
                try
                {
                    ThemeManager.SetTheme(
                        (ThemeManager.Themes)
                            Enum.Parse(typeof (ThemeManager.Themes), Settings.Instance["theme"].ToString()));
                }
                catch (Exception exception)
                {
                    log.Error(exception);
                }

                if (ThemeManager.CurrentTheme == ThemeManager.Themes.Custom)
                {
                    try
                    {
                        ThemeManager.BGColor = Color.FromArgb(int.Parse(Settings.Instance["theme_bg"].ToString()));
                        ThemeManager.ControlBGColor = Color.FromArgb(int.Parse(Settings.Instance["theme_ctlbg"].ToString()));
                        ThemeManager.TextColor = Color.FromArgb(int.Parse(Settings.Instance["theme_text"].ToString()));
                        ThemeManager.ButBG = Color.FromArgb(int.Parse(Settings.Instance["theme_butbg"].ToString()));
                        ThemeManager.ButBorder = Color.FromArgb(int.Parse(Settings.Instance["theme_butbord"].ToString()));
                    }
                    catch
                    {
                        log.Error("Bad Custom theme - reset to standard");
                        ThemeManager.SetTheme(ThemeManager.Themes.BurntKermit);
                    }
                }

                if (ThemeManager.CurrentTheme == ThemeManager.Themes.HighContrast)
                {
                    switchicons(new highcontrastmenuicons());
                }
            }

            //Added by HL below
            //ModifyToolbarColor(new menuicons2());
            this.MenuFlightData.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.MenuFlightData.AutoSize = false;
            this.MenuFlightData.Size = new Size(80, 40);
            this.MenuFlightData.TextAlign = ContentAlignment.MiddleCenter;
            this.MenuFlightData.Font = new System.Drawing.Font("微软雅黑", 12, FontStyle.Bold);
            this.MenuFlightData.Visible = false;

            this.MenuFlightPlanner.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.MenuFlightPlanner.AutoSize = true;
            // this.MenuFlightPlanner.Size = new Size(76, 30);
            // this.MenuFlightPlanner.Size = new Size(80, 40);
            // this.MenuFlightPlanner.TextAlign = ContentAlignment.MiddleCenter;
            // this.MenuFlightPlanner.Font = new System.Drawing.Font("微软雅黑", 12, FontStyle.Bold);

            this.MenuInitConfig.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.MenuInitConfig.AutoSize = true;
            // this.MenuInitConfig.Size = new Size(76, 30);
            // this.MenuInitConfig.Size = new Size(80, 40);
            // this.MenuInitConfig.TextAlign = ContentAlignment.MiddleCenter;
            // this.MenuInitConfig.Font = new System.Drawing.Font("微软雅黑", 12, FontStyle.Bold);



            this.MenuSimulation.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.MenuSimulation.AutoSize = true;

            this.MenuConfigTune.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.MenuConfigTune.AutoSize = true;

            this.MenuTerminal.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.MenuTerminal.AutoSize = true;

            // this.listBox_log.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.listBox_log.AutoSize = false;
            this.listBox_log.Size = new Size(400, 80);
            this.listBox_log.Location = new System.Drawing.Point(this.curr_loc.Location.X, 80);
            // this.curr_loc.Size = new System.Drawing.Size(100, 30);
            this.listBox_log.BackColor = Color.DarkBlue;
            this.listBox_log.Visible = false;

            this.bt_log.AutoSize = true;
            this.bt_log.Font = new System.Drawing.Font("微软雅黑", 10, FontStyle.Bold);
            this.bt_log.ForeColor = Color.Blue;
            // this.bt_log.Location = new System.Drawing.Point(this.gb_comm_type.Location.X - this.bt_log.Size.Width - 10, this.label2.Location.Y - (this.label2.Size.Height/2));


            this.MenuConnect.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.MenuConnect.AutoSize = true;
            // this.MenuConnect.Size = new Size(100, 60);
            // this.MenuConnect.TextAlign = ContentAlignment.MiddleCenter;
            // this.MenuConnect.Font = new System.Drawing.Font("微软雅黑", 14, FontStyle.Bold);


            this.MenuUserAppConfig.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.MenuUserAppConfig.AutoSize = true;

            this.MenuUpdateSoftware.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.MenuUpdateSoftware.AutoSize = true;

            // this.MenuHelp.DisplayStyle = ToolStripItemDisplayStyle.Image;
            // this.MenuHelp.AutoSize = true;

            // label3: armed status
            Int32 offset_W = 700;
            Int32 offset_H = 16;
            this.label3.Location = new System.Drawing.Point(offset_W, offset_H);
            this.label3.TextAlign = ContentAlignment.MiddleCenter;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 14, FontStyle.Bold);
            this.label3.BackColor = this.MainMenu.BackColor;

            // label2: mode value
            this.label2.Location = new System.Drawing.Point(offset_W + 160 + 100, offset_H);
            this.label2.Size = new System.Drawing.Size(100, 30);
            this.label2.TextAlign = ContentAlignment.MiddleCenter;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 14, FontStyle.Bold);
            this.label2.AutoSize = false;
            this.label2.BackColor = this.MainMenu.BackColor;

            // bt_log
            this.bt_log.Location = new Point(this.gb_comm_type.Location.X - this.bt_log.Size.Width - 5, this.gb_comm_type.Location.Y + 3);

            this.btn_work_mode.Location = new Point(this.gb_comm_type.Location.X - this.bt_log.Size.Width - this.btn_work_mode.Size.Width - 5, this.gb_comm_type.Location.Y + 8);

            log.Info("Graph Setup");
            CreateChart(zg1);
            this.pictureBox1.Hide();
            this.textBox1.Hide();
            this.panel_tuningPID.Hide();

            //this.label1.Hide();
            //this.label2.Hide();
            //Added by HL above


            this.panel1.Size = new Size(953, 500);

            this.curr_loc.Text = "飞行器坐标: \n[" + ("000.000000") + "," + ("000.000000") + "]";
            this.curr_loc.Location = new System.Drawing.Point(offset_W + this.label2.Size.Width + this.label2.Size.Width, 75);
            // this.curr_loc.Size = new System.Drawing.Size(100, 30);
            this.curr_loc.TextAlign = ContentAlignment.MiddleCenter;
            this.curr_loc.Font = new System.Drawing.Font("宋体", 11, FontStyle.Bold);
            this.curr_loc.AutoSize = true;

            // fp_mode
            this.lb_fp_mode.Text = "常规查看模式";
            // this.curr_loc.Size = new System.Drawing.Size(100, 30);
            this.lb_fp_mode.TextAlign = ContentAlignment.MiddleCenter;
            this.lb_fp_mode.Font = new System.Drawing.Font("宋体", 12, FontStyle.Bold);
            this.lb_fp_mode.AutoSize = true;
            this.lb_fp_mode.Location = new System.Drawing.Point(this.curr_loc.Location.X + this.curr_loc.Size.Width + 10, 80);

            // listBox_log
            this.listBox_log.Location = new System.Drawing.Point(this.curr_loc.Location.X + 10, 80);


            this.lb_power.Text = "动力电压：00.0伏";
            this.lb_gps.Text = "卫星：" + "00" + "/" + ("0.00");
            this.lb_hgt.Text = "高度：" + ("000.00") + "米";
            this.lb_fc_volt.Text = "飞控电压：00.0伏";
            this.lb_flight_time.Text = "飞行时间：" + ("00.00") + "分";
            this.lb_vel.Text = "速度：" + ("00.00") + "米/秒";
            this.lb_dist_home.Text = "返航距离：" + ("000.00") + "米";
            this.lb_water_value.Text = "药量：" + ("00.00") + "升";

            this.lb_power.Font = new System.Drawing.Font("宋体", 10, FontStyle.Bold);
            this.lb_gps.Font = new System.Drawing.Font("宋体", 10, FontStyle.Bold);
            this.lb_hgt.Font = new System.Drawing.Font("宋体", 10, FontStyle.Bold);
            this.lb_fc_volt.Font = new System.Drawing.Font("宋体", 10, FontStyle.Bold);
            this.lb_flight_time.Font = new System.Drawing.Font("宋体", 10, FontStyle.Bold);
            this.lb_vel.Font = new System.Drawing.Font("宋体", 10, FontStyle.Bold);
            this.lb_dist_home.Font = new System.Drawing.Font("宋体", 10, FontStyle.Bold);
            this.lb_water_value.Font = new System.Drawing.Font("宋体", 10, FontStyle.Bold);

            // this.lb_flight_time.Location = new System.Drawing.Point(this.lb_gps.Size.Width);
            // this.lb_vel.Location = new System.Drawing.Point(this.lb_flight_time.Size.Width);
            // this.lb_gps.Location = new System.Drawing.Point(this.lb_power.Location.X + this.lb_power.Size.Width);

            this.lb_power.AutoSize = true;
            this.lb_gps.AutoSize = true;
            this.lb_hgt.AutoSize = true;
            this.lb_fc_volt.AutoSize = true;
            this.lb_flight_time.AutoSize = true;
            this.lb_vel.AutoSize = true;
            this.lb_dist_home.AutoSize = true;
            this.lb_water_value.AutoSize = true;

            if (Settings.Instance["showairports"] != null)
            {
                MainV2.ShowAirports = bool.Parse(Settings.Instance["showairports"]);
            }

            this.pfdControl1 = new PrimaryFlightDisplay.PFDControl();
            this.pfdControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pfdControl1.Location = new System.Drawing.Point(0, 0);
            this.pfdControl1.Name = "pfdControl1";
            // this.pfdControl1.Size = new System.Drawing.Size(520, 549);
            this.pfdControl1.TabIndex = 0;
            this.pfdControl1.AutoSize = true;
            this.pn_pfd.AutoSize = true;
            this.pn_pfd.Controls.Add(pfdControl1);

            // this.pn_pfd.Location = new Point(this.MenuConnect.Location.X, 61);

            // this.splitContainer1.Panel2.Controls.Add(this.pfdControl1);
            // this.splitContainer1.Size = new System.Drawing.Size(792, 549);
            // this.splitContainer1.SplitterDistance = 267;
            // this.splitContainer1.SplitterWidth = 5;
            // this.splitContainer1.TabIndex = 0;
            // IPrimaryFlightDisplay pfd = pfdControl1 as IPrimaryFlightDisplay;
            // pfdControl1.Redraw();

            // pfdControl1.Show();

            // this.yaw_gauge.Location = new Point(this.pn_pfd.Location.X + this.pn_pfd.Size.Width + 436, 61);

            // set default
            ShowTFR = true;
            // load saved
            if (Settings.Instance["showtfr"] != null)
            {
                MainV2.ShowTFR = Settings.Instance.GetBoolean("showtfr");
            }

            if (Settings.Instance["enableadsb"] != null)
            {
                MainV2.instance.EnableADSB = Settings.Instance.GetBoolean("enableadsb");
            }

            try
            {
                // log.Info("Create FD");
                FlightData = new GCSViews.FlightData();
                // log.Info("Create FP");
                FlightPlanner = new GCSViews.FlightPlanner();
                //Configuration = new GCSViews.ConfigurationView.Setup();
                // log.Info("Create SIM");
                // Simulation = new SITL();
                //Firmware = new GCSViews.Firmware();
                //Terminal = new GCSViews.Terminal();

                FlightData.Width = MyView.Width;
                FlightPlanner.Width = MyView.Width;
                // Simulation.Width = MyView.Width;
            }
            catch (ArgumentException e)
            {
                //http://www.microsoft.com/en-us/download/details.aspx?id=16083
                //System.ArgumentException: Font 'Arial' does not support style 'Regular'.

                log.Fatal(e);
                CustomMessageBox.Show(e.ToString() +
                                      "\n\n Font Issues? Please install this http://www.microsoft.com/en-us/download/details.aspx?id=16083");
                //splash.Close();
                //this.Close();
                Application.Exit();
            }
            catch (Exception e)
            {
                log.Fatal(e);
                CustomMessageBox.Show("A Major error has occured : " + e.ToString());
                Application.Exit();
            }

            //set first instance display configuration
            if (DisplayConfiguration == null)
            {
                DisplayConfiguration = DisplayConfiguration.Basic();
            }

            // load old config
            if (Settings.Instance["advancedview"] != null)
            {
                if (Settings.Instance.GetBoolean("advancedview") == true)
                {
                    DisplayConfiguration = new DisplayView().Advanced();
                }
                // remove old config
                Settings.Instance.Remove("advancedview");
            }            //// load this before the other screens get loaded
            if (Settings.Instance["displayview"] != null)
            {
                try
                {
                DisplayConfiguration = Settings.Instance.GetDisplayView("displayview");
            }
                catch
                {
                    DisplayConfiguration = DisplayConfiguration.Basic();
                    Settings.Instance["displayview"] = MainV2.DisplayConfiguration.ConvertToString();
                }
            }

            LayoutChanged += updateLayout;
            LayoutChanged(null, EventArgs.Empty);

            if (Settings.Instance["CHK_GDIPlus"] != null)
                GCSViews.FlightData.myhud.UseOpenGL = !bool.Parse(Settings.Instance["CHK_GDIPlus"].ToString());

            if (Settings.Instance["CHK_hudshow"] != null)
                GCSViews.FlightData.myhud.hudon = bool.Parse(Settings.Instance["CHK_hudshow"].ToString());

            try
            {
                if (Settings.Instance["MainLocX"] != null && Settings.Instance["MainLocY"] != null)
                {
                    this.StartPosition = FormStartPosition.Manual;
                    Point startpos = new Point(Settings.Instance.GetInt32("MainLocX"),
                        Settings.Instance.GetInt32("MainLocY"));

                    // fix common bug which happens when user removes a monitor, the app shows up
                    // offscreen and it is very hard to move it onscreen.  Also happens with 
                    // remote desktop a lot.  So this only restores position if the position
                    // is visible.
                    foreach (Screen s in Screen.AllScreens)
                    {
                        if (s.WorkingArea.Contains(startpos))
                        {
                            this.Location = startpos;
                            break;
                        }
                    }

                }

                if (Settings.Instance["MainMaximised"] != null)
                {
                    this.WindowState =
                        (FormWindowState) Enum.Parse(typeof (FormWindowState), Settings.Instance["MainMaximised"]);
                    // dont allow minimised start state
                    if (this.WindowState == FormWindowState.Minimized)
                    {
                        this.WindowState = FormWindowState.Normal;
                        this.Location = new Point(100, 100);
                    }
                }

                if (Settings.Instance["MainHeight"] != null)
                    this.Height = Settings.Instance.GetInt32("MainHeight");
                if (Settings.Instance["MainWidth"] != null)
                    this.Width = Settings.Instance.GetInt32("MainWidth");

                if (Settings.Instance["CMB_rateattitude"] != null)
                    CurrentState.rateattitudebackup = Settings.Instance.GetByte("CMB_rateattitude");
                if (Settings.Instance["CMB_rateposition"] != null)
                    CurrentState.ratepositionbackup = Settings.Instance.GetByte("CMB_rateposition");
                if (Settings.Instance["CMB_ratestatus"] != null)
                    CurrentState.ratestatusbackup = Settings.Instance.GetByte("CMB_ratestatus");
                if (Settings.Instance["CMB_raterc"] != null)
                    CurrentState.ratercbackup = Settings.Instance.GetByte("CMB_raterc");
                if (Settings.Instance["CMB_ratesensors"] != null)
                    CurrentState.ratesensorsbackup = Settings.Instance.GetByte("CMB_ratesensors");

                // make sure rates propogate
                MainV2.comPort.MAV.cs.ResetInternals();

                if (Settings.Instance["speechenable"] != null)
                    MainV2.speechEnable = Settings.Instance.GetBoolean("speechenable");
                /// AB ZhaoYJ@2017-03-11
                /// for: default enable speech
                /// TODO: make sure it's stable
                /// start
#if SPEECH_EN
                MainV2.speechEnable = true;
#endif
                /// end
                if (Settings.Instance["analyticsoptout"] != null)
                    MissionPlanner.Utilities.Tracking.OptOut = Settings.Instance.GetBoolean("analyticsoptout");

                try
                {
                    if (Settings.Instance["TXT_homelat"] != null)
                        MainV2.comPort.MAV.cs.HomeLocation.Lat = Settings.Instance.GetDouble("TXT_homelat");

                    if (Settings.Instance["TXT_homelng"] != null)
                        MainV2.comPort.MAV.cs.HomeLocation.Lng = Settings.Instance.GetDouble("TXT_homelng");

                    if (Settings.Instance["TXT_homealt"] != null)
                        MainV2.comPort.MAV.cs.HomeLocation.Alt = Settings.Instance.GetDouble("TXT_homealt");

                    // remove invalid entrys
                    if (Math.Abs(MainV2.comPort.MAV.cs.HomeLocation.Lat) > 90 ||
                        Math.Abs(MainV2.comPort.MAV.cs.HomeLocation.Lng) > 180)
                        MainV2.comPort.MAV.cs.HomeLocation = new PointLatLngAlt();
                }
                catch
                {
                }
            }
            catch
            {
            }

            if (CurrentState.rateattitudebackup == 0) // initilised to 10, configured above from save
            {
                CustomMessageBox.Show(
                    "NOTE: your attitude rate is 0, the hud will not work\nChange in Configuration > Planner > Telemetry Rates");
            }
            
            // create log dir if it doesnt exist
            try
            {
                if (!Directory.Exists(Settings.Instance.LogDir))
                    Directory.CreateDirectory(Settings.Instance.LogDir);
            }
            catch (Exception ex) { log.Error(ex); }

            Microsoft.Win32.SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;

            // make sure new enough .net framework is installed
            if (!MONO)
            {
                Microsoft.Win32.RegistryKey installed_versions =
                    Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP");
                string[] version_names = installed_versions.GetSubKeyNames();
                //version names start with 'v', eg, 'v3.5' which needs to be trimmed off before conversion
                double Framework = Convert.ToDouble(version_names[version_names.Length - 1].Remove(0, 1),
                    CultureInfo.InvariantCulture);
                int SP =
                    Convert.ToInt32(installed_versions.OpenSubKey(version_names[version_names.Length - 1])
                        .GetValue("SP", 0));

                if (Framework < 4.0)
                {
                    CustomMessageBox.Show("This program requires .NET Framework 4.0. You currently have " + Framework);
                }
            }

            if (Program.IconFile != null)
            {
                //--this.Icon = Icon.FromHandle(((Bitmap)Program.IconFile).GetHicon());
            }

            if (Program.Logo != null && Program.name == "VVVVZ")
            {
                MenuDonate.Click -= this.toolStripMenuItem1_Click;
                MenuDonate.Text = "";
                MenuDonate.Image = Program.Logo;

                MenuDonate.Click += MenuCustom_Click;

                MenuFlightData.Visible = false;
                MenuFlightPlanner.Visible = true;
                MenuConfigTune.Visible = false;
                MenuHelp.Visible = false;
                MenuInitConfig.Visible = false;
                MenuSimulation.Visible = false;
                MenuTerminal.Visible = false;
            }
            else if (Program.Logo != null && Program.names.Contains(Program.name))
            {
                MenuDonate.Click -= this.toolStripMenuItem1_Click;
                MenuDonate.Text = "";
                MenuDonate.Image = Program.Logo;
            }

            Application.DoEvents();

            Comports.Add(comPort);

            MainV2.comPort.MavChanged += comPort_MavChanged;

            // save config to test we have write access
            SaveConfig();
        }

        void cmb_sysid_Click(object sender, EventArgs e)
        {
            MainV2._connectionControl.UpdateSysIDS();
        }

        void comPort_MavChanged(object sender, EventArgs e)
        {
            log.Info("Mav Changed " + MainV2.comPort.MAV.sysid);

            HUD.Custom.src = MainV2.comPort.MAV.cs;

            CustomWarning.defaultsrc = MainV2.comPort.MAV.cs;

            MissionPlanner.Controls.PreFlight.CheckListItem.defaultsrc = MainV2.comPort.MAV.cs;

            // when uploading a firmware we dont want to reload this screen.
            if (instance.MyView.current.Control != null && instance.MyView.current.Control.GetType() == typeof(GCSViews.InitialSetup))
            {
                var page = ((GCSViews.InitialSetup)instance.MyView.current.Control).backstageView.SelectedPage;
                if (page != null && page.Text == "Install Firmware")
                {
                    return;
                }
            }

            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker) delegate
                {
                    instance.MyView.Reload();
                });
            }
            else
            {
                instance.MyView.Reload();
            }
        }

        void SystemEvents_PowerModeChanged(object sender, Microsoft.Win32.PowerModeChangedEventArgs e)
        {
            // try prevent crash on resume
            if (e.Mode == Microsoft.Win32.PowerModes.Suspend)
            {
                doDisconnect(MainV2.comPort);
            }
        }

        private void BGLoadAirports(object nothing)
        {
            // read airport list
            try
            {
                Utilities.Airports.ReadOurairports(Settings.GetRunningDirectory() +
                                                   "airports.csv");

                Utilities.Airports.checkdups = true;

                //Utilities.Airports.ReadOpenflights(Application.StartupPath + Path.DirectorySeparatorChar + "airports.dat");

                log.Info("Loaded " + Utilities.Airports.GetAirportCount + " airports");
            }
            catch
            {
            }
        }

        //Added by HL below
        void ModifyToolbarColor(menuicons icons)
        {
            displayicons = icons;

            // MainMenu.BackColor = SystemColors.MenuBar;
            // MainMenu.BackColor = Color.Yellow;
            //ThemeManager.ApplyHighContrast(MainMenu, 0);

            MainMenu.BackgroundImage = displayicons.bg;

            MenuFlightData.Image = displayicons.fd;
            MenuFlightPlanner.Image = displayicons.fp;
            MenuInitConfig.Image = displayicons.config_tuning;
            MenuSimulation.Image = displayicons.sim;
            MenuConfigTune.Image = displayicons.config_tuning;
            MenuTerminal.Image = displayicons.terminal;
            MenuConnect.Image = displayicons.connect;
            MenuHelp.Image = displayicons.help;
            MenuDonate.Image = displayicons.donate;
            MenuUserAppConfig.Image = displayicons.config_tuning;
            MenuUpdateSoftware.Image = displayicons.config_tuning;


            // MenuFlightData.ForeColor = Color.Transparent;
            // MenuFlightPlanner.ForeColor = Color.Transparent;
            // MenuInitConfig.ForeColor = Color.Transparent;
            // MenuSimulation.ForeColor = Color.Transparent;
            // MenuConfigTune.ForeColor = Color.Transparent;
            // MenuTerminal.ForeColor = Color.Transparent;
            // MenuConnect.ForeColor = Color.Transparent;
            // MenuHelp.ForeColor = Color.Transparent;
            // MenuDonate.ForeColor = Color.Transparent;
            // MenuUserAppConfig.ForeColor = Color.Transparent;
            // MenuUpdateSoftware.ForeColor = Color.Transparent;
        }
        //Added by HL above

        public void switchicons(menuicons icons)
        {
            // dont update if no change
            if (displayicons.GetType() == icons.GetType())
                return;

            displayicons = icons;

            // MainMenu.BackColor = SystemColors.MenuBar;
            MainMenu.BackColor = Color.Red;

            MainMenu.BackgroundImage = displayicons.bg;

            MenuFlightData.Image = displayicons.fd;
            MenuFlightPlanner.Image = displayicons.fp;
            MenuInitConfig.Image = displayicons.initsetup;
            MenuSimulation.Image = displayicons.sim;
            MenuConfigTune.Image = displayicons.config_tuning;
            MenuTerminal.Image = displayicons.terminal;
            MenuConnect.Image = displayicons.connect;
            MenuHelp.Image = displayicons.help;
            MenuDonate.Image = displayicons.donate;

#if false
            MenuFlightData.ForeColor = ThemeManager.TextColor;
            MenuFlightPlanner.ForeColor = ThemeManager.TextColor;
            MenuInitConfig.ForeColor = ThemeManager.TextColor;
            MenuSimulation.ForeColor = ThemeManager.TextColor;
            MenuConfigTune.ForeColor = ThemeManager.TextColor;
            MenuTerminal.ForeColor = ThemeManager.TextColor;
            MenuConnect.ForeColor = ThemeManager.TextColor;
            MenuHelp.ForeColor = ThemeManager.TextColor;
            MenuDonate.ForeColor = ThemeManager.TextColor;
#endif
            // MenuFlightData.ForeColor = Color.Transparent;
            // MenuFlightPlanner.ForeColor = Color.Transparent;
            // MenuInitConfig.ForeColor = Color.Transparent;
            // MenuSimulation.ForeColor = Color.Transparent;
            // MenuConfigTune.ForeColor = Color.Transparent;
            // MenuTerminal.ForeColor = Color.Transparent;
            // MenuConnect.ForeColor = Color.Transparent;
            // MenuHelp.ForeColor = Color.Transparent;
            // MenuDonate.ForeColor = Color.Transparent;
            // MenuUserAppConfig.ForeColor = Color.Transparent;
            // MenuUpdateSoftware.ForeColor = Color.Transparent;
        }

        void MenuCustom_Click(object sender, EventArgs e)
        {
            if (Settings.Instance.GetBoolean("password_protect") == false)
            {
                MenuFlightData.Visible = true;
                MenuFlightPlanner.Visible = true;
                MenuConfigTune.Visible = true;
                MenuHelp.Visible = false;
                MenuInitConfig.Visible = true;
                MenuSimulation.Visible = true;
                MenuTerminal.Visible = true;
            }
            else
            {
                if (Password.VerifyPassword())
                {
                    MenuFlightData.Visible = true;
                    MenuFlightPlanner.Visible = true;
                    MenuConfigTune.Visible = true;
                    MenuHelp.Visible = false;
                    MenuInitConfig.Visible = true;
                    MenuSimulation.Visible = true;
                    MenuTerminal.Visible = true;
                }
            }
        }

        void adsb_UpdatePlanePosition(object sender, EventArgs e)
        {
            lock (adsblock)
            {
                var adsb = ((MissionPlanner.Utilities.adsb.PointLatLngAltHdg)sender);

                var id = adsb.Tag;

                if (MainV2.instance.adsbPlanes.ContainsKey(id))
                {
                    // update existing
                    ((adsb.PointLatLngAltHdg) instance.adsbPlanes[id]).Lat = adsb.Lat;
                    ((adsb.PointLatLngAltHdg) instance.adsbPlanes[id]).Lng = adsb.Lng;
                    ((adsb.PointLatLngAltHdg) instance.adsbPlanes[id]).Alt = adsb.Alt;
                    ((adsb.PointLatLngAltHdg) instance.adsbPlanes[id]).Heading = adsb.Heading;
                    ((adsb.PointLatLngAltHdg) instance.adsbPlanes[id]).Time = DateTime.Now;
                    ((adsb.PointLatLngAltHdg) instance.adsbPlanes[id]).CallSign = adsb.CallSign;
                }
                else
                {
                    // create new plane
                    MainV2.instance.adsbPlanes[id] =
                        new adsb.PointLatLngAltHdg(adsb.Lat, adsb.Lng,
                            adsb.Alt, adsb.Heading, id,
                            DateTime.Now) {CallSign = adsb.CallSign};
                }
            }
        }


        private void ResetConnectionStats()
        {
            log.Info("Reset connection stats");
            // If the form has been closed, or never shown before, we need do nothing, as 
            // connection stats will be reset when shown
            if (this.connectionStatsForm != null && connectionStatsForm.Visible)
            {
                // else the form is already showing.  reset the stats
                this.connectionStatsForm.Controls.Clear();
                _connectionStats = new ConnectionStats(comPort);
                this.connectionStatsForm.Controls.Add(_connectionStats);
                ThemeManager.ApplyThemeTo(this.connectionStatsForm);
            }
        }

        private void ShowConnectionStatsForm()
        {
            if (this.connectionStatsForm == null || this.connectionStatsForm.IsDisposed)
            {
                // If the form has been closed, or never shown before, we need all new stuff
                this.connectionStatsForm = new Form
                {
                    Width = 430,
                    Height = 180,
                    MaximizeBox = false,
                    MinimizeBox = false,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Text = Strings.LinkStats
                };
                // Change the connection stats control, so that when/if the connection stats form is showing,
                // there will be something to see
                this.connectionStatsForm.Controls.Clear();
                _connectionStats = new ConnectionStats(comPort);
                this.connectionStatsForm.Controls.Add(_connectionStats);
                this.connectionStatsForm.Width = _connectionStats.Width;
            }

            this.connectionStatsForm.Show();
            ThemeManager.ApplyThemeTo(this.connectionStatsForm);
        }

        private void CMB_serialport_Click(object sender, EventArgs e)
        {
            string oldport = _connectionControl.CMB_serialport.Text;
            PopulateSerialportList();
            if (_connectionControl.CMB_serialport.Items.Contains(oldport))
                _connectionControl.CMB_serialport.Text = oldport;
        }

        private void PopulateSerialportList()
        {
            _connectionControl.CMB_serialport.Items.Clear();
            _connectionControl.CMB_serialport.Items.Add("AUTO");
            _connectionControl.CMB_serialport.Items.AddRange(SerialPort.GetPortNames());
            _connectionControl.CMB_serialport.Items.Add("TCP");
            _connectionControl.CMB_serialport.Items.Add("UDP");
            _connectionControl.CMB_serialport.Items.Add("UDPCl");
        }

        private void MenuFlightData_Click(object sender, EventArgs e)
        {
            MyView.ShowScreen("FlightData");
        }

        private void MenuFlightPlanner_Click(object sender, EventArgs e)
        {
            MyView.ShowScreen("FlightPlanner");
        }

        public void MenuSetup_Click(object sender, EventArgs e)
        {
            if (Settings.Instance.GetBoolean("password_protect") == false)
            {
                MyView.ShowScreen("HWConfig");
            }
            else
            {
                if (Password.VerifyPassword())
                {
                    MyView.ShowScreen("HWConfig");
                }
            }
        }

        //Added by HL below
        public void MenuUpdateSoftware_Click(object sender, EventArgs e)
        {
            // AB ZhaoYJ@2017-03-18: no need to update firmware
            // TODO: update firmware should be enable when firmware newer to user
            // ========= start =========
            MyView.ShowScreen("UpdateFirmware");
            //  ========= start =========
        }
        //Added by HL above

        private void MenuSimulation_Click(object sender, EventArgs e)
        {
            MyView.ShowScreen("Simulation");
        }

        private void MenuTuning_Click(object sender, EventArgs e)
        {
            //deleted by HL below
            //if (Settings.Instance.GetBoolean("password_protect") == false)
            //{
            //    MyView.ShowScreen("SWConfig");
            //}
            //else
            //{
            if (Password.VerifyPassword())
            {
                MyView.ShowScreen("SWConfig");
            }
            //}
            //deleted by HL above
        }

        private void MenuTerminal_Click(object sender, EventArgs e)
        {
            MyView.ShowScreen("Terminal");
        }

        public void doDisconnect(MAVLinkInterface comPort)
        {

            // AB ZhaoYJ@2017-02-23 for auto-conn FC: default TXT is AUTO
            // ========= start =========
            connState = connectState.KO;
            connectOK = false;
            // ========= end ========= 

            log.Info("We are disconnecting");
            try
            {
                if (speechEngine != null) // cancel all pending speech
                    speechEngine.SpeakAsyncCancelAll();

                comPort.BaseStream.DtrEnable = false;
                comPort.Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            // now that we have closed the connection, cancel the connection stats
            // so that the 'time connected' etc does not grow, but the user can still
            // look at the now frozen stats on the still open form
            try
            {
                // if terminal is used, then closed using this button.... exception
                if (this.connectionStatsForm != null)
                    ((ConnectionStats) this.connectionStatsForm.Controls[0]).StopUpdates();
            }
            catch
            {
            }

            // refresh config window if needed
            if (MyView.current != null)
            {
                if (MyView.current.Name == "HWConfig")
                    MyView.ShowScreen("HWConfig");
                if (MyView.current.Name == "SWConfig")
                    MyView.ShowScreen("SWConfig");
            }

            try
            {
                System.Threading.ThreadPool.QueueUserWorkItem((WaitCallback) delegate
                {
                    try
                    {
                        MissionPlanner.Log.LogSort.SortLogs(Directory.GetFiles(Settings.Instance.LogDir, "*.tlog"));
                    }
                    catch
                    {
                    }
                }
                    );
            }
            catch
            {
            }

            this.MenuConnect.Image = global::MissionPlanner.Properties.Resources.light_connect_icon;
        }

        public void doConnect(MAVLinkInterface comPort, string portname, string baud)
        {

            bool skipconnectcheck = false;
            log.Info("We are connecting");
            switch (portname)
            {
                case "preset":
                    skipconnectcheck = true;
                    if (comPort.BaseStream is TcpSerial)
                        _connectionControl.CMB_serialport.Text = "TCP";
                    if (comPort.BaseStream is UdpSerial)
                        _connectionControl.CMB_serialport.Text = "UDP";
                    if (comPort.BaseStream is UdpSerialConnect)
                        _connectionControl.CMB_serialport.Text = "UDPCl";
                    if (comPort.BaseStream is SerialPort)
                    {
                        _connectionControl.CMB_serialport.Text = comPort.BaseStream.PortName;
                        _connectionControl.CMB_baudrate.Text = comPort.BaseStream.BaudRate.ToString();
                    }
                    break;
                case "TCP":
                    comPort.BaseStream = new TcpSerial();
                    _connectionControl.CMB_serialport.Text = "TCP";
                    break;
                case "UDP":
                    comPort.BaseStream = new UdpSerial();
                    _connectionControl.CMB_serialport.Text = "UDP";
                    break;
                case "UDPCl":
                    comPort.BaseStream = new UdpSerialConnect();
                    _connectionControl.CMB_serialport.Text = "UDPCl";
                    break;
                case "AUTO":
                default:
                    comPort.BaseStream = new SerialPort();
                    break;
            }

            // Tell the connection UI that we are now connected.
            _connectionControl.IsConnected(true);

            // Here we want to reset the connection stats counter etc.
            this.ResetConnectionStats();

            comPort.MAV.cs.ResetInternals();

            //cleanup any log being played
            comPort.logreadmode = false;
            if (comPort.logplaybackfile != null)
                comPort.logplaybackfile.Close();
            comPort.logplaybackfile = null;

            try
            {
                // do autoscan
                if ((portname == "AUTO") || (portname == "Serial"))
                {
                    if(portname == "AUTO")
                    {
                        Comms.CommsSerialScan.Scan(false, true); // scan wifi

                    }
                    else if (portname == "Serial")
                    {
                        Comms.CommsSerialScan.Scan(false); // dont scan wifi

                    }

                    DateTime deadline = DateTime.Now.AddSeconds(50);

                    while (Comms.CommsSerialScan.foundport == false)
                    {
                        System.Threading.Thread.Sleep(100);

                        if (DateTime.Now > deadline)
                        {
                            MessageBox.Show(Strings.Timeout, "连接失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            _connectionControl.IsConnected(false);
                            return;
                        }
                    }

                    // AB ZhaoYJ@2017-02-23 for auto-conn FC
                    // ========= start =========
                    if(!Comms.CommsSerialScan.portinterface.PortName.Contains("TCP"))
                    {
                        _connectionControl.CMB_serialport.Text = portname = Comms.CommsSerialScan.portinterface.PortName;
                        _connectionControl.CMB_baudrate.Text =
                            baud = Comms.CommsSerialScan.portinterface.BaudRate.ToString();
                    }
                    else
                    {
                        _connectionControl.CMB_serialport.Text = portname = Comms.CommsSerialScan.portinterface.PortName;
                        _connectionControl.CMB_baudrate.Text =
                            baud = Comms.CommsSerialScan.portinterface.BaudRate.ToString();
                        comPort.BaseStream = Comms.CommsSerialScan.portinterface;
                    }
                    // ========= end =========

                }

                log.Info("Set Portname");
                // set port, then options
                comPort.BaseStream.PortName = portname;

                log.Info("Set Baudrate");
                try
                {
                    comPort.BaseStream.BaudRate = int.Parse(baud);
                }
                catch (Exception exp)
                {
                    log.Error(exp);
                }

                // prevent serialreader from doing anything
                comPort.giveComport = true;

                log.Info("About to do dtr if needed");
                // reset on connect logic.
                if (Settings.Instance.GetBoolean("CHK_resetapmonconnect") == true)
                {
                    log.Info("set dtr rts to false");
                    comPort.BaseStream.DtrEnable = false;
                    comPort.BaseStream.RtsEnable = false;

                    comPort.BaseStream.toggleDTR();
                }

                comPort.giveComport = false;

                // setup to record new logs
                try
                {
                    Directory.CreateDirectory(Settings.Instance.LogDir);
                    comPort.logfile =
                        new BufferedStream(
                            File.Open(
                                Settings.Instance.LogDir + Path.DirectorySeparatorChar +
                                DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".tlog", FileMode.CreateNew,
                                FileAccess.ReadWrite, FileShare.None));

                    comPort.rawlogfile =
                        new BufferedStream(
                            File.Open(
                                Settings.Instance.LogDir + Path.DirectorySeparatorChar +
                                DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".rlog", FileMode.CreateNew,
                                FileAccess.ReadWrite, FileShare.None));

                    log.Info("creating logfile " + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".tlog");
                }
                catch (Exception exp2)
                {
                    log.Error(exp2);
                    CustomMessageBox.Show(Strings.Failclog);
                } // soft fail

                // reset connect time - for timeout functions
                connecttime = DateTime.Now;

                // do the connect
                comPort.Open(false, skipconnectcheck);
                // comPort.Open(true, skipconnectcheck);

                if (!comPort.BaseStream.IsOpen)
                {
                    log.Info("comport is closed. existing connect");
                    try
                    {
                        _connectionControl.IsConnected(false);
                        UpdateConnectIcon();
                        comPort.Close();
                    }
                    catch
                    {
                    }
                    return;
                }

                // get all the params
                foreach (var mavstate in comPort.MAVlist)
                {
                    comPort.sysidcurrent = mavstate.sysid;
                    comPort.compidcurrent = mavstate.compid;
                    // AB ZhaoYJ@2017-03-18 for auto-conn FC: no need to get all params when start up
                    // ========= start =========
                    comPort.getParamList();
                    // ========= end ========= 
                }

                // AB ZhaoYJ@2017-02-23 for auto-conn FC: default TXT is AUTO
                // ========= start =========
                connState = connectState.OK;
                connectOK = true;
                // ========= end ========= 

                if (MainV2.connectOK)
                {
                    bool en_lidar = ((int)MainV2.comPort.GetParam("SERIAL2_PROTOCOL")) == (9);

                    if (en_lidar)
                    {
                        FlightPlanner.rb_en_lidar.BackColor = Color.Lime;
                    }
                }

                // set to first seen
                comPort.sysidcurrent = comPort.MAVlist.First().sysid;
                comPort.compidcurrent = comPort.MAVlist.First().compid;

                _connectionControl.UpdateSysIDS();

                // detect firmware we are conected to.
                if (comPort.MAV.cs.firmware == Firmwares.ArduCopter2)
                {
                    _connectionControl.TOOL_APMFirmware.SelectedIndex =
                        _connectionControl.TOOL_APMFirmware.Items.IndexOf(Firmwares.ArduCopter2);
                }
                else if (comPort.MAV.cs.firmware == Firmwares.Ateryx)
                {
                    _connectionControl.TOOL_APMFirmware.SelectedIndex =
                        _connectionControl.TOOL_APMFirmware.Items.IndexOf(Firmwares.Ateryx);
                }
                else if (comPort.MAV.cs.firmware == Firmwares.ArduRover)
                {
                    _connectionControl.TOOL_APMFirmware.SelectedIndex =
                        _connectionControl.TOOL_APMFirmware.Items.IndexOf(Firmwares.ArduRover);
                }
                else if (comPort.MAV.cs.firmware == Firmwares.ArduPlane)
                {
                    _connectionControl.TOOL_APMFirmware.SelectedIndex =
                        _connectionControl.TOOL_APMFirmware.Items.IndexOf(Firmwares.ArduPlane);
                }

                // check for newer firmware
                var softwares = Firmware.LoadSoftwares();

                if (softwares.Count > 0)
                {
                    try
                    {
                        string[] fields1 = comPort.MAV.VersionString.Split(' ');

                        foreach (Firmware.software item in softwares)
                        {
                            string[] fields2 = item.name.Split(' ');

                            // check primare firmware type. ie arudplane, arducopter
                            if (fields1[0] == fields2[0])
                            {
                                Version ver1 = VersionDetection.GetVersion(comPort.MAV.VersionString);
                                Version ver2 = VersionDetection.GetVersion(item.name);

                                if (ver2 > ver1)
                                {
                                    Common.MessageShowAgain(Strings.NewFirmware + "-" + item.name,
                                        Strings.NewFirmwareA + item.name + Strings.Pleaseup);
                                    break;
                                }

                                // check the first hit only
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                }

                FlightData.CheckBatteryShow();

                MissionPlanner.Utilities.Tracking.AddEvent("Connect", "Connect", comPort.MAV.cs.firmware.ToString(),
                    comPort.MAV.param.Count.ToString());
                MissionPlanner.Utilities.Tracking.AddTiming("Connect", "Connect Time",
                    (DateTime.Now - connecttime).TotalMilliseconds, "");

                MissionPlanner.Utilities.Tracking.AddEvent("Connect", "Baud", comPort.BaseStream.BaudRate.ToString(), "");

                // save the baudrate for this port
                Settings.Instance[_connectionControl.CMB_serialport.Text + "_BAUD"] = _connectionControl.CMB_baudrate.Text;

                // this.Text = titlebar + " " + comPort.MAV.VersionString;

                // refresh config window if needed
                if (MyView.current != null)
                {
                    if (MyView.current.Name == "HWConfig")
                        MyView.ShowScreen("HWConfig");
                    if (MyView.current.Name == "SWConfig")
                        MyView.ShowScreen("SWConfig");
                }

                // load wps on connect option.
                if (Settings.Instance.GetBoolean("loadwpsonconnect") == true)
                {
                    // only do it if we are connected.
                    if (comPort.BaseStream.IsOpen)
                    {
                        MenuFlightPlanner_Click(null, null);
                        FlightPlanner.BUT_read_Click(null, null);
                    }
                }

                // get any rallypoints
                if (MainV2.comPort.MAV.param.ContainsKey("RALLY_TOTAL") &&
                    int.Parse(MainV2.comPort.MAV.param["RALLY_TOTAL"].ToString()) > 0)
                {
                    FlightPlanner.getRallyPointsToolStripMenuItem_Click(null, null);

                    double maxdist = 0;

                    foreach (var rally in comPort.MAV.rallypoints)
                    {
                        foreach (var rally1 in comPort.MAV.rallypoints)
                        {
                            var pnt1 = new PointLatLngAlt(rally.Value.lat/10000000.0f, rally.Value.lng/10000000.0f);
                            var pnt2 = new PointLatLngAlt(rally1.Value.lat/10000000.0f, rally1.Value.lng/10000000.0f);

                            var dist = pnt1.GetDistance(pnt2);

                            maxdist = Math.Max(maxdist, dist);
                        }
                    }

                    if (comPort.MAV.param.ContainsKey("RALLY_LIMIT_KM") &&
                        (maxdist/1000.0) > (float) comPort.MAV.param["RALLY_LIMIT_KM"])
                    {
                        CustomMessageBox.Show(Strings.Warningrallypointdistance + " " +
                                              (maxdist/1000.0).ToString("0.00") + " > " +
                                              (float) comPort.MAV.param["RALLY_LIMIT_KM"]);
                    }
                }

                // set connected icon
                this.MenuConnect.Image = displayicons.disconnect;
            }
            catch (Exception ex)
            {
                log.Warn(ex);
                try
                {
                    _connectionControl.IsConnected(false);
                    UpdateConnectIcon();
                    comPort.Close();
                }
                catch (Exception ex2)
                {
                    log.Warn(ex2);
                }
                CustomMessageBox.Show("Can not establish a connection\n\n" + ex.Message);
                return;
            }
        }

        // AB ZhaoYJ@2017-02-23 for auto-conn FC
        // ========= start =========
        public void connectFC(string comm_type)
        {
            // AB ZhaoYJ@2017-02-23 for auto-conn FC: default TXT is AUTO
            // ========= start =========
            connState = connectState.WIP;
            connectOK = false;
            // ========= end ========= 

            comPort.giveComport = false;

            log.Info("MenuConnect Start");

            // sanity check
            if (comPort.BaseStream.IsOpen && MainV2.comPort.MAV.cs.groundspeed > 4)
            {
                if (DialogResult.No ==
                    MessageBox.Show(Strings.Stillmoving, Strings.Disconnect, MessageBoxButtons.YesNo))
                {
                    return;
                }
            }

            try
            {
                log.Info("Cleanup last logfiles");
                // cleanup from any previous sessions
                if (comPort.logfile != null)
                    comPort.logfile.Close();

                if (comPort.rawlogfile != null)
                    comPort.rawlogfile.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Strings.ErrorClosingLogFile + ex.Message, Strings.ERROR);
            }

            comPort.logfile = null;
            comPort.rawlogfile = null;

            // decide if this is a connect or disconnect
            if (comPort.BaseStream.IsOpen)
            {
                doDisconnect(comPort);
            }
            else
            {
                // AB ZhaoYJ@2017-02-23 for auto-conn FC: default TXT is AUTO
                // ========= start =========
                _connectionControl.CMB_serialport.Text = comm_type;
                doConnect(comPort, _connectionControl.CMB_serialport.Text, _connectionControl.CMB_baudrate.Text);
                // doConnect(comPort, _connectionControl.CMB_serialport.Text, _connectionControl.CMB_baudrate.Text);
                // ========= end =========
            }

            MainV2._connectionControl.UpdateSysIDS();
        }
        // ========= end =========

        private void MenuConnect_Click(object sender, EventArgs e)
        {

            if (comm_type.Equals("Serial") || comm_type.Equals("TCP"))
            {
                connectFC(comm_type);

            }
            else    
            {
                connectFC("AUTO");
            }
        }

        private void CMB_serialport_SelectedIndexChanged(object sender, EventArgs e)
        {
            comPortName = _connectionControl.CMB_serialport.Text;
            if (comPortName == "UDP" || comPortName == "UDPCl" || comPortName == "TCP" || comPortName == "AUTO")
            {
                _connectionControl.CMB_baudrate.Enabled = false;
                if (comPortName == "TCP")
                    MainV2.comPort.BaseStream = new TcpSerial();
                if (comPortName == "UDP")
                    MainV2.comPort.BaseStream = new UdpSerial();
                if (comPortName == "UDPCl")
                    MainV2.comPort.BaseStream = new UdpSerialConnect();
                if (comPortName == "AUTO")
                {
                    MainV2.comPort.BaseStream = new SerialPort();
                    return;
                }
            }
            else
            {
                _connectionControl.CMB_baudrate.Enabled = true;
                MainV2.comPort.BaseStream = new SerialPort();
            }

            try
            {
                if (!String.IsNullOrEmpty(_connectionControl.CMB_serialport.Text))
                    comPort.BaseStream.PortName = _connectionControl.CMB_serialport.Text;

                MainV2.comPort.BaseStream.BaudRate = int.Parse(_connectionControl.CMB_baudrate.Text);

                // check for saved baud rate and restore
                if (Settings.Instance[_connectionControl.CMB_serialport.Text + "_BAUD"] != null)
                {
                    _connectionControl.CMB_baudrate.Text =
                        Settings.Instance[_connectionControl.CMB_serialport.Text + "_BAUD"];
                }
            }
            catch
            {
            }
        }


        /// <summary>
        /// overriding the OnCLosing is a bit cleaner than handling the event, since it 
        /// is this object.
        /// 
        /// This happens before FormClosed
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            // speed up tile saving on exit
            GMap.NET.GMaps.Instance.CacheOnIdleRead = false;
            GMap.NET.GMaps.Instance.BoostCacheEngine = true;

            log.Info("MainV2_FormClosing");

            Settings.Instance["MainHeight"] = this.Height.ToString();
            Settings.Instance["MainWidth"] = this.Width.ToString();
            Settings.Instance["MainMaximised"] = this.WindowState.ToString();

            Settings.Instance["MainLocX"] = this.Location.X.ToString();
            Settings.Instance["MainLocY"] = this.Location.Y.ToString();

            // close bases connection
            try
            {
                comPort.logreadmode = false;
                if (comPort.logfile != null)
                    comPort.logfile.Close();

                if (comPort.rawlogfile != null)
                    comPort.rawlogfile.Close();

                comPort.logfile = null;
                comPort.rawlogfile = null;
            }
            catch
            {
            }

            // close all connections
            foreach (var port in Comports)
            {
                try
                {
                    port.logreadmode = false;
                    if (port.logfile != null)
                        port.logfile.Close();

                    if (port.rawlogfile != null)
                        port.rawlogfile.Close();

                    port.logfile = null;
                    port.rawlogfile = null;
                }
                catch
                {
                }
            }

            Utilities.adsb.Stop();

            Warnings.WarningEngine.Stop();

            log.Info("closing vlcrender");
            try
            {
                while (vlcrender.store.Count > 0)
                    vlcrender.store[0].Stop();
            }
            catch
            {
            }

            log.Info("closing pluginthread");

            pluginthreadrun = false;

            if (pluginthread != null)
                pluginthread.Join();

            log.Info("closing serialthread");

            serialThread = false;

            if (serialreaderthread != null)
                serialreaderthread.Join();

            log.Info("closing joystickthread");

            joystickthreadrun = false;

            if (joystickthread != null)
                joystickthread.Join();

            // tuningthreadrun = false;
            // 
            // if (tuningthread != null)
            //     tuningthread.Join();

            log.Info("closing httpthread");

            // if we are waiting on a socket we need to force an abort
            httpserver.Stop();

            log.Info("sorting tlogs");
            try
            {
                System.Threading.ThreadPool.QueueUserWorkItem((WaitCallback) delegate
                {
                    try
                    {
                        MissionPlanner.Log.LogSort.SortLogs(Directory.GetFiles(Settings.Instance.LogDir, "*.tlog"));
                    }
                    catch
                    {
                    }
                }
                    );
            }
            catch
            {
            }

            log.Info("closing MyView");

            // close all tabs
            MyView.Dispose();

            log.Info("closing fd");
            try
            {
                FlightData.Dispose();
            }
            catch
            {
            }
            log.Info("closing fp");
            try
            {
                FlightPlanner.Dispose();
            }
            catch
            {
            }
            log.Info("closing sim");
            try
            {
                Simulation.Dispose();
            }
            catch
            {
            }

            try
            {
                if (comPort.BaseStream.IsOpen)
                    comPort.Close();
            }
            catch
            {
            } // i get alot of these errors, the port is still open, but not valid - user has unpluged usb

            // save config
            SaveConfig();

            Console.WriteLine(httpthread.IsAlive);
            Console.WriteLine(joystickthread.IsAlive);
            Console.WriteLine(serialreaderthread.IsAlive);
            Console.WriteLine(pluginthread.IsAlive);
            // Console.WriteLine(tuningthread.IsAlive);

            log.Info("MainV2_FormClosing done");

            if (MONO)
                this.Dispose();
        }


        /// <summary>
        /// this happens after FormClosing...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);

            Console.WriteLine("MainV2_FormClosed");

            if (joystick != null)
            {
                while (!joysendThreadExited)
                    Thread.Sleep(10);

                joystick.Dispose(); //proper clean up of joystick.
            }
        }

        private void LoadConfig()
        {
            try
            {
                // log.Info("Loading config");

                Settings.Instance.Load();

                comPortName = Settings.Instance.ComPort;
            }
            catch (Exception ex)
            {
                // log.Error("Bad Config File", ex);
            }
        }

        private void SaveConfig()
        {
            try
            {
                // log.Info("Saving config");
                Settings.Instance.ComPort = comPortName;

                if (_connectionControl != null)
                    Settings.Instance.BaudRate = _connectionControl.CMB_baudrate.Text;

                Settings.Instance.APMFirmware = MainV2.comPort.MAV.cs.firmware.ToString();

                Settings.Instance.Save();
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// needs to be true by default so that exits properly if no joystick used.
        /// </summary>
        volatile private bool joysendThreadExited = true;

        /// <summary>
        /// thread used to send joystick packets to the MAV
        /// </summary>
        private void joysticksend()
        {
            float rate = 50; // 1000 / 50 = 20 hz
            int count = 0;

            DateTime lastratechange = DateTime.Now;

            joystickthreadrun = true;

            while (joystickthreadrun)
            {
                joysendThreadExited = false;
                //so we know this thread is stil alive.           
                try
                {
                    if (MONO)
                    {
                        log.Error("Mono: closing joystick thread");
                        break;
                    }

                    if (!MONO)
                    {
                        //joystick stuff

                        if (joystick != null && joystick.enabled)
                        {
                            MAVLink.mavlink_rc_channels_override_t rc = new MAVLink.mavlink_rc_channels_override_t();

                            rc.target_component = comPort.MAV.compid;
                            rc.target_system = comPort.MAV.sysid;

                            if (joystick.getJoystickAxis(1) != Joystick.Joystick.joystickaxis.None)
                                rc.chan1_raw = MainV2.comPort.MAV.cs.rcoverridech1;
                            if (joystick.getJoystickAxis(2) != Joystick.Joystick.joystickaxis.None)
                                rc.chan2_raw = MainV2.comPort.MAV.cs.rcoverridech2;
                            if (joystick.getJoystickAxis(3) != Joystick.Joystick.joystickaxis.None)
                                rc.chan3_raw = MainV2.comPort.MAV.cs.rcoverridech3;
                            if (joystick.getJoystickAxis(4) != Joystick.Joystick.joystickaxis.None)
                                rc.chan4_raw = MainV2.comPort.MAV.cs.rcoverridech4;
                            if (joystick.getJoystickAxis(5) != Joystick.Joystick.joystickaxis.None)
                                rc.chan5_raw = MainV2.comPort.MAV.cs.rcoverridech5;
                            if (joystick.getJoystickAxis(6) != Joystick.Joystick.joystickaxis.None)
                                rc.chan6_raw = MainV2.comPort.MAV.cs.rcoverridech6;
                            if (joystick.getJoystickAxis(7) != Joystick.Joystick.joystickaxis.None)
                                rc.chan7_raw = MainV2.comPort.MAV.cs.rcoverridech7;
                            if (joystick.getJoystickAxis(8) != Joystick.Joystick.joystickaxis.None)
                                rc.chan8_raw = MainV2.comPort.MAV.cs.rcoverridech8;

                            if (lastjoystick.AddMilliseconds(rate) < DateTime.Now)
                            {
                                /*
                                if (MainV2.comPort.MAV.cs.rssi > 0 && MainV2.comPort.MAV.cs.remrssi > 0)
                                {
                                    if (lastratechange.Second != DateTime.Now.Second)
                                    {
                                        if (MainV2.comPort.MAV.cs.txbuffer > 90)
                                        {
                                            if (rate < 20)
                                                rate = 21;
                                            rate--;

                                            if (MainV2.comPort.MAV.cs.linkqualitygcs < 70)
                                                rate = 50;
                                        }
                                        else
                                        {
                                            if (rate > 100)
                                                rate = 100;
                                            rate++;
                                        }

                                        lastratechange = DateTime.Now;
                                    }
                                 
                                }
                                */
                                //                                Console.WriteLine(DateTime.Now.Millisecond + " {0} {1} {2} {3} {4}", rc.chan1_raw, rc.chan2_raw, rc.chan3_raw, rc.chan4_raw,rate);

                                //Console.WriteLine("Joystick btw " + comPort.BaseStream.BytesToWrite);

                                if (!comPort.BaseStream.IsOpen)
                                    continue;

                                if (comPort.BaseStream.BytesToWrite < 50)
                                {
                                    if (sitl)
                                    {
                                        MissionPlanner.Controls.SITL.rcinput();
                                    }
                                    else
                                    {
                                        comPort.sendPacket(rc, rc.target_system, rc.target_component);
                                    }
                                    count++;
                                    lastjoystick = DateTime.Now;
                                }
                            }
                        }
                    }
                    Thread.Sleep(20);
                }
                catch
                {
                } // cant fall out
            }
            joysendThreadExited = true; //so we know this thread exited.    
        }

        /// <summary>
        /// Used to fix the icon status for unexpected unplugs etc...
        /// </summary>
        private void UpdateConnectIcon()
        {
            if ((DateTime.Now - connectButtonUpdate).Milliseconds > 500)
            {
                //                        Console.WriteLine(DateTime.Now.Millisecond);
                if (comPort.BaseStream.IsOpen)
                {
                    if ((string) this.MenuConnect.Image.Tag != "Disconnect")
                    {
                        this.BeginInvoke((MethodInvoker) delegate
                        {
                            this.MenuConnect.Image = displayicons.disconnect;
                            this.MenuConnect.Image.Tag = "Disconnect";
                            this.MenuConnect.Text = Strings.DISCONNECTc;
                            _connectionControl.IsConnected(true);
                        });
                    }
                }
                else
                {
                    if (this.MenuConnect.Image != null && (string) this.MenuConnect.Image.Tag != "Connect")
                    {
                        this.BeginInvoke((MethodInvoker) delegate
                        {
                            this.MenuConnect.Image = displayicons.connect;
                            this.MenuConnect.Image.Tag = "Connect";
                            this.MenuConnect.Text = Strings.CONNECTc;
                            _connectionControl.IsConnected(false);
                            if (_connectionStats != null)
                            {
                                _connectionStats.StopUpdates();
                            }
                        });
                    }

                    if (comPort.logreadmode)
                    {
                        this.BeginInvoke((MethodInvoker) delegate { _connectionControl.IsConnected(true); });
                    }
                }
                connectButtonUpdate = DateTime.Now;
            }
        }

        ManualResetEvent PluginThreadrunner = new ManualResetEvent(false);

        private void PluginThread()
        {
            Hashtable nextrun = new Hashtable();

            pluginthreadrun = true;

            PluginThreadrunner.Reset();

            while (pluginthreadrun)
            {
                try
                {
                    lock (Plugin.PluginLoader.Plugins)
                    {
                        foreach (var plugin in Plugin.PluginLoader.Plugins)
                        {
                            if (!nextrun.ContainsKey(plugin))
                                nextrun[plugin] = DateTime.MinValue;

                            if (DateTime.Now > plugin.NextRun)
                            {
                                // get ms till next run
                                int msnext = (int) (1000/plugin.loopratehz);
                                // allow the plug to modify this, if needed
                                plugin.NextRun = DateTime.Now.AddMilliseconds(msnext);

                                try
                                {
                                    bool ans = plugin.Loop();
                                }
                                catch (Exception ex)
                                {
                                    log.Error(ex);
                                }
                            }
                        }
                    }
                }
                catch
                {
                }

                // max rate is 100 hz - prevent massive cpu usage
                System.Threading.Thread.Sleep(10);
            }

            while (Plugin.PluginLoader.Plugins.Count > 0)
            {
                var plugin = Plugin.PluginLoader.Plugins[0];
                try
                {
                    plugin.Exit();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
                Plugin.PluginLoader.Plugins.Remove(plugin);
            }

            PluginThreadrunner.Set();

            return;
        }

        ManualResetEvent SerialThreadrunner = new ManualResetEvent(false);

        /// <summary>
        /// main serial reader thread
        /// controls
        /// serial reading
        /// link quality stats
        /// speech voltage - custom - alt warning - data lost
        /// heartbeat packet sending
        /// 
        /// and can't fall out
        /// </summary>
        private void SerialReader()
        {
            if (serialThread == true)
                return;
            serialThread = true;

            SerialThreadrunner.Reset();

            int minbytes = 0;

            int altwarningmax = 0;

            bool armedstatus = false;

            string lastmessagehigh = "";

            DateTime speechcustomtime = DateTime.Now;

            DateTime speechlowspeedtime = DateTime.Now;

            DateTime linkqualitytime = DateTime.Now;

            while (serialThread)
            {
                try
                {
                    Thread.Sleep(1); // was 5

                    try
                    {
                        if (GCSViews.Terminal.comPort is MAVLinkSerialPort)
                        {
                        }
                        else
                        {
                            if (GCSViews.Terminal.comPort != null && GCSViews.Terminal.comPort.IsOpen)
                                continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }

                    // update connect/disconnect button and info stats
                    try
                    {
                        UpdateConnectIcon();
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }

                    // 30 seconds interval speech options
                    if (speechEnable && speechEngine != null && (DateTime.Now - speechcustomtime).TotalSeconds > 30 &&
                        (MainV2.comPort.logreadmode || comPort.BaseStream.IsOpen))
                    {
                        if (MainV2.speechEngine.IsReady)
                        {
                            if (Settings.Instance.GetBoolean("speechcustomenabled"))
                            {
                                MainV2.speechEngine.SpeakAsync(Common.speechConversion(""+ Settings.Instance["speechcustom"]));
                            }

                            speechcustomtime = DateTime.Now;
                        }

                        // speech for battery alerts
                        //speechbatteryvolt
                        float warnvolt = Settings.Instance.GetFloat("speechbatteryvolt");
                        float warnpercent = Settings.Instance.GetFloat("speechbatterypercent");

                        if (Settings.Instance.GetBoolean("speechbatteryenabled") == true &&
                            MainV2.comPort.MAV.cs.battery_voltage <= warnvolt &&
                            MainV2.comPort.MAV.cs.battery_voltage >= 5.0)
                        {
                            if (MainV2.speechEngine.IsReady)
                            {
                                MainV2.speechEngine.SpeakAsync(Common.speechConversion(""+ Settings.Instance["speechbattery"]));
                            }
                        }
                        else if (Settings.Instance.GetBoolean("speechbatteryenabled") == true &&
                                 (MainV2.comPort.MAV.cs.battery_remaining) < warnpercent &&
                                 MainV2.comPort.MAV.cs.battery_voltage >= 5.0 &&
                                 MainV2.comPort.MAV.cs.battery_remaining != 0.0)
                        {
                            if (MainV2.speechEngine.IsReady)
                            {
                                MainV2.speechEngine.SpeakAsync(
                                    Common.speechConversion("" + Settings.Instance["speechbattery"]));
                            }
                        }
                    }

                    // speech for airspeed alerts
                    if (speechEnable && speechEngine != null && (DateTime.Now - speechlowspeedtime).TotalSeconds > 10 &&
                        (MainV2.comPort.logreadmode || comPort.BaseStream.IsOpen))
                    {
                        if (Settings.Instance.GetBoolean("speechlowspeedenabled") == true && MainV2.comPort.MAV.cs.armed)
                        {
                            float warngroundspeed = Settings.Instance.GetFloat("speechlowgroundspeedtrigger");
                            float warnairspeed = Settings.Instance.GetFloat("speechlowairspeedtrigger");

                            if (MainV2.comPort.MAV.cs.airspeed < warnairspeed)
                            {
                                if (MainV2.speechEngine.IsReady)
                                {
                                    MainV2.speechEngine.SpeakAsync(
                                        Common.speechConversion(""+ Settings.Instance["speechlowairspeed"]));
                                    speechlowspeedtime = DateTime.Now;
                                }
                            }
                            else if (MainV2.comPort.MAV.cs.groundspeed < warngroundspeed)
                            {
                                if (MainV2.speechEngine.IsReady)
                                {
                                    MainV2.speechEngine.SpeakAsync(
                                        Common.speechConversion(""+ Settings.Instance["speechlowgroundspeed"]));
                                    speechlowspeedtime = DateTime.Now;
                                }
                            }
                            else
                            {
                                speechlowspeedtime = DateTime.Now;
                            }
                        }
                    }

                    // speech altitude warning - message high warning
                    if (speechEnable && speechEngine != null &&
                        (MainV2.comPort.logreadmode || comPort.BaseStream.IsOpen))
                    {
                        float warnalt = float.MaxValue;
                        if (Settings.Instance.ContainsKey("speechaltheight"))
                        {
                            warnalt = Settings.Instance.GetFloat("speechaltheight");
                        }
                        try
                        {
                            altwarningmax = (int) Math.Max(MainV2.comPort.MAV.cs.alt, altwarningmax);

                            if (Settings.Instance.GetBoolean("speechaltenabled") == true && MainV2.comPort.MAV.cs.alt != 0.00 &&
                                (MainV2.comPort.MAV.cs.alt <= warnalt) && MainV2.comPort.MAV.cs.armed)
                            {
                                if (altwarningmax > warnalt)
                                {
                                    if (MainV2.speechEngine.IsReady)
                                        MainV2.speechEngine.SpeakAsync(
                                            Common.speechConversion(""+Settings.Instance["speechalt"]));
                                }
                            }
                        }
                        catch
                        {
                        } // silent fail


                        try
                        {
                            // say the latest high priority message
                            if (MainV2.speechEngine.IsReady &&
                                lastmessagehigh != MainV2.comPort.MAV.cs.messageHigh && MainV2.comPort.MAV.cs.messageHigh != null)
                            {
                                if (!MainV2.comPort.MAV.cs.messageHigh.StartsWith("PX4v2 "))
                                {
                                    MainV2.speechEngine.SpeakAsync(MainV2.comPort.MAV.cs.messageHigh);
                                    lastmessagehigh = MainV2.comPort.MAV.cs.messageHigh;
                                }
                            }
                        }
                        catch
                        {
                        }
                    }

                    // not doing anything
                    if (!MainV2.comPort.logreadmode && !comPort.BaseStream.IsOpen)
                    {
                        altwarningmax = 0;
                    }

                    // attenuate the link qualty over time
                    if ((DateTime.Now - MainV2.comPort.MAV.lastvalidpacket).TotalSeconds >= 1)
                    {
                        if (linkqualitytime.Second != DateTime.Now.Second)
                        {
                            MainV2.comPort.MAV.cs.linkqualitygcs = (ushort) (MainV2.comPort.MAV.cs.linkqualitygcs*0.8f);
                            linkqualitytime = DateTime.Now;

                            // force redraw is no other packets are being read
                            GCSViews.FlightData.myhud.Invalidate();
                        }
                    }

                    // heartbeat led
                    if ((DateTime.Now - MainV2.comPort.MAV.lastvalidpacket).TotalSeconds >= 2)
                    {
                        MainV2.comPort.fc_alive = false;
                    }
                    else
                    {
                        MainV2.comPort.fc_alive = true;
                    }

                    // data loss warning - wait min of 10 seconds, ignore first 30 seconds of connect, repeat at 5 seconds interval
                    if ((DateTime.Now - MainV2.comPort.MAV.lastvalidpacket).TotalSeconds > 10
                        && (DateTime.Now - connecttime).TotalSeconds > 30
                        && (DateTime.Now - nodatawarning).TotalSeconds > 5
                        && (MainV2.comPort.logreadmode || comPort.BaseStream.IsOpen)
                        && MainV2.comPort.MAV.cs.armed)
                    {
                        if (speechEnable && speechEngine != null)
                        {
                            if (MainV2.speechEngine.IsReady)
                            {
                                MainV2.speechEngine.SpeakAsync("警告：丢失 " +
                                                               (int)
                                                                   (DateTime.Now - MainV2.comPort.MAV.lastvalidpacket)
                                                                       .TotalSeconds + " 秒");
                                nodatawarning = DateTime.Now;
                            }
                        }
                    }

                    // get home point on armed status change.
                    if (armedstatus != MainV2.comPort.MAV.cs.armed && comPort.BaseStream.IsOpen)
                    {
                        armedstatus = MainV2.comPort.MAV.cs.armed;
                        // status just changed to armed
                        if (MainV2.comPort.MAV.cs.armed == true && MainV2.comPort.MAV.aptype != MAVLink.MAV_TYPE.GIMBAL)
                        {
                            try
                            {
                                //MainV2.comPort.getHomePosition();
                                MainV2.comPort.MAV.cs.HomeLocation = new PointLatLngAlt(MainV2.comPort.getWP(0));
                                if (MyView.current != null && MyView.current.Name == "FlightPlanner")
                                {
                                    // update home if we are on flight data tab
                                    FlightPlanner.updateHome();
                                }
                            }
                            catch
                            {
                                // dont hang this loop
                                this.BeginInvoke(
                                    (MethodInvoker)
                                        delegate
                                        {
                                            MessageBox.Show("GPS未定位，无法获取返航点");
                                        });
                            }
                        }

                        if (speechEnable && speechEngine != null)
                        {
                            if (Settings.Instance.GetBoolean("speecharmenabled"))
                            {
                                string speech = armedstatus ? Settings.Instance["speecharm"] : Settings.Instance["speechdisarm"];
                                if (!string.IsNullOrEmpty(speech))
                                {
                                    MainV2.speechEngine.SpeakAsync(Common.speechConversion(speech));
                                }
                            }
                        }
                    }

                    // send a hb every seconds from gcs to ap
                    if (heatbeatSend.Second != DateTime.Now.Second)
                    {
                        MAVLink.mavlink_heartbeat_t htb = new MAVLink.mavlink_heartbeat_t()
                        {
                            type = (byte) MAVLink.MAV_TYPE.GCS,
                            autopilot = (byte) MAVLink.MAV_AUTOPILOT.INVALID,
                            mavlink_version = 3 // MAVLink.MAVLINK_VERSION
                        };

                        // enumerate each link
                        foreach (var port in Comports)
                        {
                            // poll for params at heartbeat interval
                            if (!port.giveComport)
                            {
                                try
                                {
                                    port.getParamPoll();
                                    port.getParamPoll();
                                }
                                catch
                                {
                                }
                            }

                            // there are 3 hb types we can send, mavlink1, mavlink2 signed and unsigned
                            bool sentsigned = false;
                            bool sentmavlink1 = false;
                            bool sentmavlink2 = false;

                            // enumerate each mav
                            foreach (var MAV in port.MAVlist)
                            {
                                try
                                {
                                    // are we talking to a mavlink2 device
                                    if (MAV.mavlinkv2)
                                    {
                                        // is signing enabled
                                        if (MAV.signing)
                                        {
                                            // check if we have already sent
                                            if (sentsigned)
                                                continue;
                                            sentsigned = true;
                                        }
                                        else
                                        {
                                            // check if we have already sent
                                            if (sentmavlink2)
                                                continue;
                                            sentmavlink2 = true;
                                        }
                                    }
                                    else
                                    {
                                        // check if we have already sent
                                        if (sentmavlink1)
                                            continue;
                                        sentmavlink1 = true;
                                    }

                                    port.sendPacket(htb, MAV.sysid, MAV.compid);
                                }
                                catch (Exception ex)
                                {
                                    log.Error(ex);
                                    // close the bad port
                                    try
                                    {
                                        port.Close();
                                    }
                                    catch
                                    {
                                    }
                                    // refresh the screen if needed
                                    if (port == MainV2.comPort)
                                    {
                                        // refresh config window if needed
                                        if (MyView.current != null)
                                        {
                                            this.Invoke((MethodInvoker) delegate()
                                            {
                                                if (MyView.current.Name == "HWConfig")
                                                    MyView.ShowScreen("HWConfig");
                                                if (MyView.current.Name == "SWConfig")
                                                    MyView.ShowScreen("SWConfig");
                                            });
                                        }
                                    }
                                }
                            }
                        }

                        heatbeatSend = DateTime.Now;
                    }

                    // if not connected or busy, sleep and loop
                    if (!comPort.BaseStream.IsOpen || comPort.giveComport == true)
                    {
                        if (!comPort.BaseStream.IsOpen)
                        {
                            // check if other ports are still open
                            foreach (var port in Comports)
                            {
                                if (port.BaseStream.IsOpen)
                                {
                                    Console.WriteLine("Main comport shut, swapping to other mav");
                                    comPort = port;
                                    break;
                                }
                            }
                        }

                        System.Threading.Thread.Sleep(100);
                    }

                    // read the interfaces
                    foreach (var port in Comports.ToArray())
                    {
                        if (!port.BaseStream.IsOpen)
                        {
                            // skip primary interface
                            if (port == comPort)
                                continue;

                            // modify array and drop out
                            Comports.Remove(port);
                            port.Dispose();
                            break;
                        }

                        while (port.BaseStream.IsOpen && port.BaseStream.BytesToRead > minbytes &&
                               port.giveComport == false && serialThread)
                        {
                            try
                            {
                                port.readPacket();
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                            }
                        }
                        // update currentstate of sysids on the port
                        foreach (var MAV in port.MAVlist)
                        {
                            try
                            {
                                MAV.cs.UpdateCurrentSettings(null, false, port, MAV);
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Tracking.AddException(e);
                    log.Error("Serial Reader fail :" + e.ToString());
                    try
                    {
                        comPort.Close();
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                }
            }

            Console.WriteLine("SerialReader Done");
            SerialThreadrunner.Set();
        }



        protected override void OnLoad(EventArgs e)
        {
            // check if its defined, and force to show it if not known about
            if (Settings.Instance["menu_autohide"] == null)
            {
                Settings.Instance["menu_autohide"] = "false";
            }
            Settings.Instance["menu_autohide"] = "false";
            try
            {
                AutoHideMenu(Settings.Instance.GetBoolean("menu_autohide"));
            }
            catch
            {
            }

            MyView.AddScreen(new MainSwitcher.Screen("FlightPlanner", FlightPlanner, true));
            MyView.AddScreen(new MainSwitcher.Screen("FlightData", FlightData, true));
            MyView.AddScreen(new MainSwitcher.Screen("HWConfig", typeof(GCSViews.InitialSetup), false));
            MyView.AddScreen(new MainSwitcher.Screen("SWConfig", typeof(GCSViews.SoftwareConfig), false));
            MyView.AddScreen(new MainSwitcher.Screen("Simulation", Simulation, true));
            MyView.AddScreen(new MainSwitcher.Screen("Terminal", typeof(GCSViews.Terminal), false));
            MyView.AddScreen(new MainSwitcher.Screen("Help", typeof(GCSViews.Help), false));
            //AB ZhaoYJ@2017-04-08
            // for userapp config
            // start
            MyView.AddScreen(new MainSwitcher.Screen("UserAppConfig", typeof(GCSViews.UserConfig), false));
            // end
            //Added by HL below
            MyView.AddScreen(new MainSwitcher.Screen("UpdateFirmware", typeof(GCSViews.ConfigurationView.ConfigFirmware), true));
            //Added by HL above

            try
            {
                if (Control.ModifierKeys == Keys.Shift)
                {
                }
                else
                {
                    log.Info("Load Pluggins");
                    Plugin.PluginLoader.LoadAll();
                    log.Info("Load Pluggins Done");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            // if (1)
            {
                this.PerformLayout();
                MenuFlightPlanner_Click(this, e);
                MainMenu_ItemClicked(this, new ToolStripItemClickedEventArgs(MenuFlightPlanner));
            }
            // else
            // {
            //     this.PerformLayout();
            //     MenuFlightData_Click(this, e);
            //     MainMenu_ItemClicked(this, new ToolStripItemClickedEventArgs(MenuFlightData));
            // }

            // for long running tasks using own threads.
            // for short use threadpool

            this.SuspendLayout();

            // setup http server
            try
            {
                httpthread = new Thread(new httpserver().listernforclients)
                {
                    Name = "motion jpg stream-network kml",
                    IsBackground = true
                };
                httpthread.Start();
            }
            catch (Exception ex)
            {
                log.Error("Error starting TCP listener thread: ", ex);
                CustomMessageBox.Show(ex.ToString());
            }

            // setup joystick packet sender
            joystickthread = new Thread(new ThreadStart(joysticksend))
            {
                IsBackground = true,
                Priority = ThreadPriority.AboveNormal,
                Name = "Main joystick sender"
            };
            joystickthread.Start();

            // setup main serial reader
            serialreaderthread = new Thread(SerialReader)
            {
                IsBackground = true,
                Name = "Main Serial reader",
                Priority = ThreadPriority.AboveNormal
            };
            serialreaderthread.Start();


            // setup main plugin thread
            pluginthread = new Thread(PluginThread)
            {
                IsBackground = true,
                Name = "plugin runner thread",
                Priority = ThreadPriority.BelowNormal
            };
            pluginthread.Start();

#if false
            try
            {
                tuningthread = new Thread(tuningLoop)
                {
                    Name = "tuning PID graph",
                    IsBackground = true
                };
                tuningthread.Start();
            }
            catch (Exception ex)
            {
                log.Error("Error starting TCP listener thread: ", ex);
                CustomMessageBox.Show(ex.ToString());
            }
#endif

            ThreadPool.QueueUserWorkItem(BGLoadAirports);

            ThreadPool.QueueUserWorkItem(BGCreateMaps);

            //ThreadPool.QueueUserWorkItem(BGGetAlmanac);

            ThreadPool.QueueUserWorkItem(BGgetTFR);

            ThreadPool.QueueUserWorkItem(BGNoFly);

            ThreadPool.QueueUserWorkItem(BGGetKIndex);

            // update firmware version list - only once per day
            ThreadPool.QueueUserWorkItem(BGFirmwareCheck);

            try
            {
                new Utilities.AltitudeAngel.AltitudeAngel();

                /*
                // setup as a prompt once dialog
                if (!Settings.Instance.GetBoolean("AACheck"))
                {
                    if (CustomMessageBox.Show(
                            "Do you wish to enable Altitude Angel airspace management data?\nFor more information visit [link;http://www.altitudeangel.com;www.altitudeangel.com]",
                            "Altitude Angel - Enable", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Utilities.AltitudeAngel.AltitudeAngel.service.SignInAsync();
                    }

                    Settings.Instance["AACheck"] = true.ToString();
                }
                */
            }
            catch (TypeInitializationException) // windows xp lacking patch level
            {
                CustomMessageBox.Show("Please update your .net version. kb2468871");
            }
            catch (Exception ex)
            {
                Tracking.AddException(ex);
            }

            this.ResumeLayout();

            Program.Splash.Close();

            MissionPlanner.Utilities.Tracking.AddTiming("AppLoad", "Load Time",
                (DateTime.Now - Program.starttime).TotalMilliseconds, "");

#if ONLINE_UPDATE
            try
            {
                // single update check per day - in a seperate thread
                if (Settings.Instance["update_check"] != DateTime.Now.ToShortDateString())
                {
                    System.Threading.ThreadPool.QueueUserWorkItem(checkupdate);
                    Settings.Instance["update_check"] = DateTime.Now.ToShortDateString();
                }
                else if (Settings.Instance.GetBoolean("beta_updates") == true)
                {
                    MissionPlanner.Utilities.Update.dobeta = true;
                    System.Threading.ThreadPool.QueueUserWorkItem(checkupdate);
                }
            }
            catch (Exception ex)
            {
                log.Error("Update check failed", ex);
            }
#endif

            // play a tlog that was passed to the program/ load a bin log passed
            if (Program.args.Length > 0)
            {
                if (File.Exists(Program.args[0]) && Program.args[0].ToLower().EndsWith(".tlog"))
                {
                    FlightData.LoadLogFile(Program.args[0]);
                    FlightData.BUT_playlog_Click(null, null);
                }
                else if (File.Exists(Program.args[0]) && Program.args[0].ToLower().EndsWith(".bin"))
                {
                    LogBrowse logbrowse = new LogBrowse();
                    ThemeManager.ApplyThemeTo(logbrowse);
                    logbrowse.logfilename = Program.args[0];
                    logbrowse.Show(this);
                    logbrowse.TopMost = true;
                }
            }

            /// AB ZhaoYJ@2017-03-17
            /// for: disable wizard when first use
            /// start
#if WIZARD_1ST_USE
            // show wizard on first use
            if (Settings.Instance["newuser"] == null)
            {
                if (CustomMessageBox.Show("This is your first run, Do you wish to use the setup wizard?\nRecomended for new users.", "Wizard", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    Wizard.Wizard wiz = new Wizard.Wizard();

                    wiz.ShowDialog(this);
                }

                CustomMessageBox.Show("To use the wizard please goto the initial setup screen, and click the wizard icon.", "Wizard");

                Settings.Instance["newuser"] = DateTime.Now.ToShortDateString();
            }
#endif
            /// end
        }

        private double ConvertToDouble(object input)
        {
            if (input.GetType() == typeof(float))
            {
                return (float)input;
            }
            if (input.GetType() == typeof(double))
            {
                return (double)input;
            }
            if (input.GetType() == typeof(ulong))
            {
                return (ulong)input;
            }
            if (input.GetType() == typeof(long))
            {
                return (long)input;
            }
            if (input.GetType() == typeof(int))
            {
                return (int)input;
            }
            if (input.GetType() == typeof(uint))
            {
                return (uint)input;
            }
            if (input.GetType() == typeof(short))
            {
                return (short)input;
            }
            if (input.GetType() == typeof(ushort))
            {
                return (ushort)input;
            }
            if (input.GetType() == typeof(bool))
            {
                return (bool)input ? 1 : 0;
            }
            if (input.GetType() == typeof(string))
            {
                double ans = 0;
                if (double.TryParse((string)input, out ans))
                {
                    return ans;
                }
            }
            if (input is Enum)
            {
                return Convert.ToInt32(input);
            }

            if (input == null)
                throw new Exception("Bad Type Null");
            else
                throw new Exception("Bad Type " + input.GetType().ToString());
        }

        DateTime tunning = DateTime.Now.AddSeconds(0);
        private void tuningLoop()
        {
            tuningthreadrun = true;

            // while (tuningthreadrun)
            {
                // udpate tunning tab
                if (tunning.AddMilliseconds(50) < DateTime.Now && cb_tuningPID.Checked)
                {
                    double time = (Environment.TickCount - tickStart) / 1000.0;
                    if (list1item != null)
                        list1.Add(time, ConvertToDouble(list1item.GetValue(MainV2.comPort.MAV.cs, null)));
                    if (list2item != null)
                        list2.Add(time, ConvertToDouble(list2item.GetValue(MainV2.comPort.MAV.cs, null)));
                    if (list3item != null)
                        list3.Add(time, ConvertToDouble(list3item.GetValue(MainV2.comPort.MAV.cs, null)));
                    if (list4item != null)
                        list4.Add(time, ConvertToDouble(list4item.GetValue(MainV2.comPort.MAV.cs, null)));
                    if (list5item != null)
                        list5.Add(time, ConvertToDouble(list5item.GetValue(MainV2.comPort.MAV.cs, null)));
                    if (list6item != null)
                        list6.Add(time, ConvertToDouble(list6item.GetValue(MainV2.comPort.MAV.cs, null)));
                    if (list7item != null)
                        list7.Add(time, ConvertToDouble(list7item.GetValue(MainV2.comPort.MAV.cs, null)));
                    if (list8item != null)
                        list8.Add(time, ConvertToDouble(list8item.GetValue(MainV2.comPort.MAV.cs, null)));
                    if (list9item != null)
                        list9.Add(time, ConvertToDouble(list9item.GetValue(MainV2.comPort.MAV.cs, null)));
                    if (list10item != null)
                        list10.Add(time, ConvertToDouble(list10item.GetValue(MainV2.comPort.MAV.cs, null)));
                }
                tunning = DateTime.Now;

            }
            Console.WriteLine("Tuning loop exit");
        }

        private void BGFirmwareCheck(object state)
        {
            try
            {
                if (Settings.Instance["fw_check"] != DateTime.Now.ToShortDateString())
                {
                    var fw = new Firmware();
                    var list = fw.getFWList();
                    if (list.Count > 1)
                        Firmware.SaveSoftwares(list);

                    Settings.Instance["fw_check"] = DateTime.Now.ToShortDateString();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void BGGetKIndex(object state)
        {
            try
            {
                // check the last kindex date
                if (Settings.Instance["kindexdate"] == DateTime.Now.ToShortDateString())
                {
                    KIndex_KIndex(Settings.Instance.GetInt32("kindex"), null);
                }
                else
                {
                    // get a new kindex
                    KIndex.KIndexEvent += KIndex_KIndex;
                    KIndex.GetKIndex();

                    Settings.Instance["kindexdate"] = DateTime.Now.ToShortDateString();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void BGgetTFR(object state)
        {
            try
            {
                tfr.tfrcache = Settings.GetUserDataDirectory() + "tfr.xml";
                tfr.GetTFRs();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void BGNoFly(object state)
        {
            try
            {
                NoFly.NoFly.Scan();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }


        void KIndex_KIndex(object sender, EventArgs e)
        {
            CurrentState.KIndexstatic = (int) sender;
            Settings.Instance["kindex"] = CurrentState.KIndexstatic.ToString();
        }

        private void BGCreateMaps(object state)
        {
            // sort logs
            try
            {
                MissionPlanner.Log.LogSort.SortLogs(Directory.GetFiles(Settings.Instance.LogDir, "*.tlog"));
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            try
            {
                // create maps
                Log.LogMap.MapLogs(Directory.GetFiles(Settings.Instance.LogDir, "*.tlog", SearchOption.AllDirectories));
                Log.LogMap.MapLogs(Directory.GetFiles(Settings.Instance.LogDir, "*.bin", SearchOption.AllDirectories));
                Log.LogMap.MapLogs(Directory.GetFiles(Settings.Instance.LogDir, "*.log", SearchOption.AllDirectories));

                if (File.Exists(tlogThumbnailHandler.tlogThumbnailHandler.queuefile))
                {
                    Log.LogMap.MapLogs(File.ReadAllLines(tlogThumbnailHandler.tlogThumbnailHandler.queuefile));

                    File.Delete(tlogThumbnailHandler.tlogThumbnailHandler.queuefile);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            try
            {
                if (File.Exists(tlogThumbnailHandler.tlogThumbnailHandler.queuefile))
                {
                    Log.LogMap.MapLogs(File.ReadAllLines(tlogThumbnailHandler.tlogThumbnailHandler.queuefile));

                    File.Delete(tlogThumbnailHandler.tlogThumbnailHandler.queuefile);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void checkupdate(object stuff)
        {
            try
            {
                MissionPlanner.Utilities.Update.CheckForUpdate();
            }
            catch (Exception ex)
            {
                log.Error("Update check failed", ex);
            }
        }

        private void MainV2_Resize(object sender, EventArgs e)
        {
            // mono - resize is called before the control is created
            if (MyView != null)
                log.Info("myview width " + MyView.Width + " height " + MyView.Height);

            log.Info("this   width " + this.Width + " height " + this.Height);
        }

        private void MenuHelp_Click(object sender, EventArgs e)
        {
            // MyView.ShowScreen("Help");
        }


        /// <summary>
        /// keyboard shortcuts override
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F12)
            {
                MenuConnect_Click(null, null);
                return true;
            }

            if (keyData == Keys.F2)
            {
                MenuFlightData_Click(null, null);
                return true;
            }
            if (keyData == Keys.F3)
            {
                MenuFlightPlanner_Click(null, null);
                return true;
            }
            if (keyData == Keys.F4)
            {
                MenuTuning_Click(null, null);
                return true;
            }

            if (keyData == Keys.F5)
            {
                comPort.getParamList();
                MyView.ShowScreen(MyView.current.Name);
                return true;
            }

            if (keyData == (Keys.Control | Keys.F)) // temp
            {
                Form frm = new temp();
                ThemeManager.ApplyThemeTo(frm);
                frm.Show();
                return true;
            }
            /*if (keyData == (Keys.Control | Keys.S)) // screenshot
            {
                ScreenShot();
                return true;
            }*/
            if (keyData == (Keys.Control | Keys.G)) // nmea out
            {
                Form frm = new SerialOutputNMEA();
                ThemeManager.ApplyThemeTo(frm);
                frm.Show();
                return true;
            }
            if (keyData == (Keys.Control | Keys.X)) 
            {

            }
            if (keyData == (Keys.Control | Keys.L)) // limits
            {
                return true;
            }
            if (keyData == (Keys.Control | Keys.W)) // test ac config
            {
                return true;
            }
            if (keyData == (Keys.Control | Keys.Z))
            {
                MissionPlanner.GenOTP otp = new MissionPlanner.GenOTP();

                otp.ShowDialog(this);

                return true;
            }
            if (keyData == (Keys.Control | Keys.T)) // for override connect
            {
                try
                {
                    MainV2.comPort.Open(false);
                }
                catch (Exception ex)
                {
                    CustomMessageBox.Show(ex.ToString());
                }
                return true;
            }
            if (keyData == (Keys.Control | Keys.Y)) // for ryan beall and ollyw42
            {
                // write
                try
                {
                    MainV2.comPort.doCommand(MAVLink.MAV_CMD.PREFLIGHT_STORAGE, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f);
                }
                catch
                {
                    CustomMessageBox.Show("Invalid command");
                    return true;
                }
                //read
                ///////MainV2.comPort.doCommand(MAVLink09.MAV_CMD.PREFLIGHT_STORAGE, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f);
                CustomMessageBox.Show("Done MAV_ACTION_STORAGE_WRITE");
                return true;
            }
            if (keyData == (Keys.Control | Keys.J))
            {
                /*
                var test = MainV2.comPort.GetLogList();

                foreach (var item in test)
                {
                    var ms = comPort.GetLog(item.id);

                    using (BinaryWriter bw = new BinaryWriter(File.OpenWrite("test" + item.id + ".bin")))
                    {
                        bw.Write(ms.ToArray());
                    }

                    var temp1 = Log.BinaryLog.ReadLog("test" + item.id + ".bin");

                    File.WriteAllLines("test" + item.id + ".log", temp1);
                }*/
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public void changelanguage(CultureInfo ci)
        {
            log.Info("change lang to " + ci.ToString() + " current " + Thread.CurrentThread.CurrentUICulture.ToString());

            if (ci != null && !Thread.CurrentThread.CurrentUICulture.Equals(ci))
            {
                Thread.CurrentThread.CurrentUICulture = ci;
                Settings.Instance["language"] = ci.Name;
                //System.Threading.Thread.CurrentThread.CurrentCulture = ci;

                HashSet<Control> views = new HashSet<Control> {this, FlightData, FlightPlanner, Simulation};

                foreach (Control view in MyView.Controls)
                    views.Add(view);

                foreach (Control view in views)
                {
                    if (view != null)
                    {
                        ComponentResourceManager rm = new ComponentResourceManager(view.GetType());
                        foreach (Control ctrl in view.Controls)
                        {
                            rm.ApplyResource(ctrl);
                        }
                        rm.ApplyResources(view, "$this");
                    }
                }
            }
        }


        public void ChangeUnits()
        {
            try
            {
                // dist
                if (Settings.Instance["distunits"] != null)
                {
                    switch (
                        (Common.distances) Enum.Parse(typeof (Common.distances), Settings.Instance["distunits"].ToString()))
                    {
                        case Common.distances.Meters:
                            CurrentState.multiplierdist = 1;
                            CurrentState.DistanceUnit = "m";
                            break;
                        case Common.distances.Feet:
                            CurrentState.multiplierdist = 3.2808399f;
                            CurrentState.DistanceUnit = "ft";
                            break;
                    }
                }
                else
                {
                    CurrentState.multiplierdist = 1;
                    CurrentState.DistanceUnit = "m";
                }

                // speed
                if (Settings.Instance["speedunits"] != null)
                {
                    switch ((Common.speeds) Enum.Parse(typeof (Common.speeds), Settings.Instance["speedunits"].ToString()))
                    {
                        case Common.speeds.meters_per_second:
                            CurrentState.multiplierspeed = 1;
                            CurrentState.SpeedUnit = "m/s";
                            break;
                        case Common.speeds.fps:
                            CurrentState.multiplierdist = 3.2808399f;
                            CurrentState.SpeedUnit = "fps";
                            break;
                        case Common.speeds.kph:
                            CurrentState.multiplierspeed = 3.6f;
                            CurrentState.SpeedUnit = "kph";
                            break;
                        case Common.speeds.mph:
                            CurrentState.multiplierspeed = 2.23693629f;
                            CurrentState.SpeedUnit = "mph";
                            break;
                        case Common.speeds.knots:
                            CurrentState.multiplierspeed = 1.94384449f;
                            CurrentState.SpeedUnit = "knots";
                            break;
                    }
                }
                else
                {
                    CurrentState.multiplierspeed = 1;
                    CurrentState.SpeedUnit = "m/s";
                }
            }
            catch
            {
            }
        }

        private void CMB_baudrate_TextChanged(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            int baud = 0;
            for (int i = 0; i < _connectionControl.CMB_baudrate.Text.Length; i++)
                if (char.IsDigit(_connectionControl.CMB_baudrate.Text[i]))
                {
                    sb.Append(_connectionControl.CMB_baudrate.Text[i]);
                    baud = baud*10 + _connectionControl.CMB_baudrate.Text[i] - '0';
                }
            if (_connectionControl.CMB_baudrate.Text != sb.ToString())
            {
                _connectionControl.CMB_baudrate.Text = sb.ToString();
            }
            try
            {
                if (baud > 0 && comPort.BaseStream.BaudRate != baud)
                    comPort.BaseStream.BaudRate = baud;
            }
            catch (Exception)
            {
            }
        }

        private void MainMenu_MouseLeave(object sender, EventArgs e)
        {
            if (_connectionControl.PointToClient(Control.MousePosition).Y < MainMenu.Height)
                return;

            this.SuspendLayout();

            panel1.Visible = false;

            this.ResumeLayout();
        }

        void menu_MouseEnter(object sender, EventArgs e)
        {
            this.SuspendLayout();
            panel1.Location = new Point(0, 0);
            panel1.Width = menu.Width;
            panel1.BringToFront();
            panel1.Visible = true;
            this.ResumeLayout();
        }

        private void autoHideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AutoHideMenu(autoHideToolStripMenuItem.Checked);

            Settings.Instance["menu_autohide"] = autoHideToolStripMenuItem.Checked.ToString();
        }

        void AutoHideMenu(bool hide)
        {
            autoHideToolStripMenuItem.Checked = hide;

            if (!hide)
            {
                this.SuspendLayout();
                panel1.Dock = DockStyle.Top;
                panel1.SendToBack();
                panel1.Visible = true;
                menu.Visible = false;
                MainMenu.MouseLeave -= MainMenu_MouseLeave;
                panel1.MouseLeave -= MainMenu_MouseLeave;
                toolStripConnectionControl.MouseLeave -= MainMenu_MouseLeave;
                this.ResumeLayout(false);
            }
            else
            {
                this.SuspendLayout();
                panel1.Dock = DockStyle.None;
                panel1.Visible = false;
                MainMenu.MouseLeave += MainMenu_MouseLeave;
                panel1.MouseLeave += MainMenu_MouseLeave;
                toolStripConnectionControl.MouseLeave += MainMenu_MouseLeave;
                menu.Visible = true;
                menu.SendToBack();
                this.ResumeLayout(false);
            }
        }

        private void MainV2_KeyDown(object sender, KeyEventArgs e)
        {
            Message temp = new Message();
            ProcessCmdKey(ref temp, e.KeyData);
            Console.WriteLine("MainV2_KeyDown " + e.ToString());
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(
                    "https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=mich146%40hotmail%2ecom&lc=AU&item_name=Michael%20Oborne&no_note=0&bn=PP%2dDonationsBF%3abtn_donate_SM%2egif%3aNonHostedGuest");
            }
            catch
            {
                CustomMessageBox.Show("Link open failed. check your default webpage association");
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal class DEV_BROADCAST_HDR
        {
            internal Int32 dbch_size;
            internal Int32 dbch_devicetype;
            internal Int32 dbch_reserved;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal class DEV_BROADCAST_PORT
        {
            public int dbcp_size;
            public int dbcp_devicetype;
            public int dbcp_reserved; // MSDN say "do not use"
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 255)] public byte[] dbcp_name;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal class DEV_BROADCAST_DEVICEINTERFACE
        {
            public Int32 dbcc_size;
            public Int32 dbcc_devicetype;
            public Int32 dbcc_reserved;

            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 16)] internal Byte[]
                dbcc_classguid;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 255)] internal Byte[] dbcc_name;
        }


        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_CREATE:
                    try
                    {
                        DEV_BROADCAST_DEVICEINTERFACE devBroadcastDeviceInterface = new DEV_BROADCAST_DEVICEINTERFACE();
                        IntPtr devBroadcastDeviceInterfaceBuffer;
                        IntPtr deviceNotificationHandle = IntPtr.Zero;
                        Int32 size = 0;

                        // frmMy is the form that will receive device-change messages.


                        size = Marshal.SizeOf(devBroadcastDeviceInterface);
                        devBroadcastDeviceInterface.dbcc_size = size;
                        devBroadcastDeviceInterface.dbcc_devicetype = DBT_DEVTYP_DEVICEINTERFACE;
                        devBroadcastDeviceInterface.dbcc_reserved = 0;
                        devBroadcastDeviceInterface.dbcc_classguid = GUID_DEVINTERFACE_USB_DEVICE.ToByteArray();
                        devBroadcastDeviceInterfaceBuffer = Marshal.AllocHGlobal(size);
                        Marshal.StructureToPtr(devBroadcastDeviceInterface, devBroadcastDeviceInterfaceBuffer, true);


                        deviceNotificationHandle = NativeMethods.RegisterDeviceNotification(this.Handle,
                            devBroadcastDeviceInterfaceBuffer, DEVICE_NOTIFY_WINDOW_HANDLE);
                    }
                    catch
                    {
                    }

                    break;

                case WM_DEVICECHANGE:
                    // The WParam value identifies what is occurring.
                    WM_DEVICECHANGE_enum n = (WM_DEVICECHANGE_enum) m.WParam;
                    var l = m.LParam;
                    if (n == WM_DEVICECHANGE_enum.DBT_DEVICEREMOVEPENDING)
                    {
                        Console.WriteLine("DBT_DEVICEREMOVEPENDING");
                    }
                    if (n == WM_DEVICECHANGE_enum.DBT_DEVNODES_CHANGED)
                    {
                        Console.WriteLine("DBT_DEVNODES_CHANGED");
                    }
                    if (n == WM_DEVICECHANGE_enum.DBT_DEVICEARRIVAL ||
                        n == WM_DEVICECHANGE_enum.DBT_DEVICEREMOVECOMPLETE)
                    {
                        Console.WriteLine(((WM_DEVICECHANGE_enum) n).ToString());

                        DEV_BROADCAST_HDR hdr = new DEV_BROADCAST_HDR();
                        Marshal.PtrToStructure(m.LParam, hdr);

                        try
                        {
                            switch (hdr.dbch_devicetype)
                            {
                                case DBT_DEVTYP_DEVICEINTERFACE:
                                    DEV_BROADCAST_DEVICEINTERFACE inter = new DEV_BROADCAST_DEVICEINTERFACE();
                                    Marshal.PtrToStructure(m.LParam, inter);
                                    log.InfoFormat("Interface {0}",
                                        ASCIIEncoding.Unicode.GetString(inter.dbcc_name, 0, inter.dbcc_size - (4*3)));
                                    break;
                                case DBT_DEVTYP_PORT:
                                    DEV_BROADCAST_PORT prt = new DEV_BROADCAST_PORT();
                                    Marshal.PtrToStructure(m.LParam, prt);
                                    log.InfoFormat("port {0}",
                                        ASCIIEncoding.Unicode.GetString(prt.dbcp_name, 0, prt.dbcp_size - (4*3)));
                                    break;
                            }
                        }
                        catch
                        {
                        }

                        //string port = Marshal.PtrToStringAuto((IntPtr)((long)m.LParam + 12));
                        //Console.WriteLine("Added port {0}",port);
                    }
                    log.InfoFormat("Device Change {0} {1} {2}", m.Msg, (WM_DEVICECHANGE_enum) m.WParam, m.LParam);

                    if (DeviceChanged != null)
                    {
                        try
                        {
                            DeviceChanged((WM_DEVICECHANGE_enum) m.WParam);
                        }
                        catch
                        {
                        }
                    }

                    foreach (Plugin.Plugin item in MissionPlanner.Plugin.PluginLoader.Plugins)
                    {
                        item.Host.ProcessDeviceChanged((WM_DEVICECHANGE_enum) m.WParam);
                    }

                    break;
                case 0x86: // WM_NCACTIVATE
                    //var thing = Control.FromHandle(m.HWnd);

                    var child = Control.FromHandle(m.LParam);

                    if (child is Form)
                    {
                        log.Debug("ApplyThemeTo " + child.Name);
                        ThemeManager.ApplyThemeTo(child);
                    }
                    break;
                default:
                    //Console.WriteLine(m.ToString());
                    break;
            }
            base.WndProc(ref m);
        }

        const int DBT_DEVTYP_PORT = 0x00000003;
        const int WM_CREATE = 0x0001;
        const Int32 DBT_DEVTYP_HANDLE = 6;
        const Int32 DBT_DEVTYP_DEVICEINTERFACE = 5;
        const Int32 DEVICE_NOTIFY_WINDOW_HANDLE = 0;
        const Int32 DIGCF_PRESENT = 2;
        const Int32 DIGCF_DEVICEINTERFACE = 0X10;
        const Int32 WM_DEVICECHANGE = 0X219;
        public static Guid GUID_DEVINTERFACE_USB_DEVICE = new Guid("A5DCBF10-6530-11D2-901F-00C04FB951ED");


        public enum WM_DEVICECHANGE_enum
        {
            DBT_CONFIGCHANGECANCELED = 0x19,
            DBT_CONFIGCHANGED = 0x18,
            DBT_CUSTOMEVENT = 0x8006,
            DBT_DEVICEARRIVAL = 0x8000,
            DBT_DEVICEQUERYREMOVE = 0x8001,
            DBT_DEVICEQUERYREMOVEFAILED = 0x8002,
            DBT_DEVICEREMOVECOMPLETE = 0x8004,
            DBT_DEVICEREMOVEPENDING = 0x8003,
            DBT_DEVICETYPESPECIFIC = 0x8005,
            DBT_DEVNODES_CHANGED = 0x7,
            DBT_QUERYCHANGECONFIG = 0x17,
            DBT_USERDEFINED = 0xFFFF,
        }

        private void MainMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            foreach (ToolStripItem item in MainMenu.Items)
            {
                if (e.ClickedItem == item)
                {
                    item.BackColor = Color.DeepSkyBlue;
                }
                else
                {
                    item.BackColor = Color.Transparent;
                    item.BackgroundImage = this.MainMenu.BackgroundImage; //.BackColor = Color.Black;
                }
            }
            //MainMenu.BackColor = Color.Black;
            //MainMenu.BackgroundImage = MissionPlanner.Properties.Resources.bgdark;
        }

        private void fullScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // full screen
            if (fullScreenToolStripMenuItem.Checked)
            {
                this.TopMost = true;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                this.WindowState = FormWindowState.Normal;
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.TopMost = false;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void readonlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainV2.comPort.ReadOnly = readonlyToolStripMenuItem.Checked;
        }

        private void connectionOptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // new ConnectionOptions().Show(this);
        }

        private void relayout_userdefine()
        {
            // yaw gauge
            this.yaw_gauge.Location = new Point(this.pn_pfd.Location.X - this.yaw_gauge.Size.Width, 66);
            this.lb_yaw.Location = new Point(this.pn_pfd.Location.X - this.lb_yaw.Size.Width - 4, this.lb_yaw.Location.Y);
            this.lb_roll.Location = new Point(this.pn_pfd.Location.X - this.lb_yaw.Size.Width - 4, this.lb_roll.Location.Y);
            this.lb_pitch.Location = new Point(this.pn_pfd.Location.X - this.lb_yaw.Size.Width - 4, this.lb_pitch.Location.Y);
            this.lb_yaw.AutoSize = true;
            this.lb_roll.AutoSize = true;
            this.lb_pitch.AutoSize = true;

            // link led & remrssi
            this.ss_remrssi.Location = new Point(this.rb_heartbeat.Location.X + this.rb_heartbeat.Size.Width + 5, this.rb_heartbeat.Size.Height + 2 );
            this.ss_remrssi.BackColor = Color.Transparent;

            // arm status
            this.label3.Location = new Point(this.ss_remrssi.Location.X + this.ss_remrssi.Size.Width + 10, this.rb_heartbeat.Size.Height + 4);
            
            // mode
            this.label2.Location = new Point(this.label3.Location.X + this.label3.Size.Width + 10, this.rb_heartbeat.Size.Height + 2);

            // bt_log
            this.bt_log.Location = new Point(this.gb_comm_type.Location.X - this.bt_log.Size.Width - 5, this.gb_comm_type.Location.Y + 3);

            this.btn_work_mode.Location = new Point(this.gb_comm_type.Location.X - this.bt_log.Size.Width - 5 - this.btn_work_mode.Size.Width, this.gb_comm_type.Location.Y + 8);
            this.btn_work_mode.Size = new Size(100, 40);
            this.btn_work_mode.BackColor = Color.Black;
            this.btn_work_mode.ForeColor = Color.White;
            this.btn_work_mode.Font = new System.Drawing.Font("微软雅黑", 12, FontStyle.Bold);


            // fp_mode
            this.lb_fp_mode.Location = new Point(this.curr_loc.Location.X + this.curr_loc.Size.Width + 10, this.curr_loc.Location.Y + 8);

            // listBox_log
            this.listBox_log.Location = new System.Drawing.Point(this.curr_loc.Location.X + 10, this.panel1.Height + 2);

            // comm type
            this.gb_comm_type.Location = new Point(this.panel1.Size.Width - this.MenuConnect.Size.Width - 130, 10);

            // line seperator
            this.ls_pannel1.Width = this.panel1.Size.Width + 20;

        }

        int FW_VER = 223; // 2.23

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (timer1_tick_first)
            {
                relayout_userdefine();
                // BespokeFusion.MaterialMessageBox.Show("Your cool message here", "The awesome message title");
                // timer1_tick_first = false; // no need
                // MyColoredLogListBox.App app = new MyColoredLogListBox.App();
                // app.InitializeComponent();
                // app.Run();

            }


            // AB ZhaoYJ@2017-08-28 for disp current location 
            // gps 3d fix
            if (MainV2.comPort.MAV.cs.gpsstatus >= 3)
            {
                curr_loc.Text = "飞行器坐标: \n[" + MainV2.comPort.MAV.cs.lat.ToString("0.000000") + "," + MainV2.comPort.MAV.cs.lng.ToString("0.000000") + "]"; //  + "," +
            }

            string fp_mode = "常规查看模式";
            if(FlightPlanner != null)
            {
                if (FlightPlanner.wpinsertmode)
                {
                    fp_mode = "航点编辑模式";
                }
                else if (FlightPlanner.polygongridmode)
                {
                    fp_mode = "作业区域编辑";

                }
                else if (FlightPlanner.polygonsplitmode)
                {
                    fp_mode = "作业区域分割";

                }
            }

            lb_fp_mode.Text = fp_mode;

            // this.yaw_gauge.Size = this.yaw_gauge.Size;

            // AB ZhaoYJ@2017-08-28 for disp flight data in pannel1 
            //
            lb_power.Text = "电源1：" + MainV2.comPort.MAV.cs.battery_voltage.ToString("0.0") + "伏";
            lb_gps.Text = "卫星：" + MainV2.comPort.MAV.cs.satcount + "/" + MainV2.comPort.MAV.cs.gpshdop.ToString("0.00");
            lb_fc_volt.Text = "电源2：" + MainV2.comPort.MAV.cs.battery_voltage2.ToString("0.0") + "伏";
            lb_hgt.Text = "高度：" + MainV2.comPort.MAV.cs.alt.ToString("0.00") + "/" +  MainV2.comPort.MAV.cs.sonarrange.ToString("0.00") + "米";
            lb_yaw.Text = "航向角：" + (MainV2.comPort.MAV.cs.yaw).ToString("0.0") + "°";
            lb_roll.Text = "横滚角：" + (MainV2.comPort.MAV.cs.roll).ToString("0.0") + "°";
            lb_pitch.Text = "俯仰角：" + (MainV2.comPort.MAV.cs.pitch).ToString("0.0") + "°";

            int tia_sec = 0;
            if(MainV2.comPort.MAV.cs.armed)
            {
                if (MainV2.comPort.fc_alive)
                {
                    try
                    {
                        tia_sec = (int)MainV2.comPort.MAV.cs.vibey;
                        //TODO: need config this param to adapter with real pump, not hard-code
                        // pump_rpm_sec = (int)MainV2.comPort.GetParam("PUMP_MAX");
                    }
                    catch (Exception ex)
                    {
                        // MessageBox.Show("获取飞行时间错误：" + ex.ToString());
                        FW_VER = 200; // 2.00
                    }
                }
                else
                {
                    tia_sec = (int)MainV2.comPort.MAV.cs.timeInAir;
                }
            }


            lb_flight_time.Text = "飞行时间：" + (tia_sec / 60).ToString("00") + "分" + ((float)(((int)tia_sec) % 60)).ToString("00") + "秒";
            lb_vel.Text = "速度：" + MainV2.comPort.MAV.cs.groundspeed.ToString("0.00") + "米/秒";
            lb_dist_home.Text = "返航距离：" + MainV2.comPort.MAV.cs.DistToHome.ToString("0.00") + "米";
            // speed_water is vibex， unit: RPM/sec
            lb_water_value.Text = "喷速：" + (MainV2.comPort.MAV.cs.vibex).ToString("0.00") + "(升/分)" + ((1 == (int)MainV2.comPort.MAV.cs.vibez) ?"/有":"/无");

            yaw_gauge.Value = MainV2.comPort.MAV.cs.yaw;

            // wait for init
            if(pfdControl1 != null)
            {
                pfdControl1.AttitudeIndicator.RollAngle = MainV2.comPort.MAV.cs.roll;
                pfdControl1.AttitudeIndicator.PitchAngle = MainV2.comPort.MAV.cs.pitch;
                pfdControl1.Redraw();
            }

            // heartbeat
            if (this.MenuConnect.Text == Strings.CONNECTc)
            {
                rb_heartbeat.BackColor = System.Drawing.Color.Red;
                // MainV2.comPort.MAV.cs.timeInAir = 0.0f;
                // connectFC();
            }
            
            {
                if (MainV2.comPort.fc_alive)
                {

                    if (timer1_tick_cnt++ % 2 == 0)
                    {
                        rb_heartbeat.BackColor = System.Drawing.Color.Green;

                    }
                    else
                    {
                        rb_heartbeat.BackColor = this.BackColor;
                    }
                }
                else
                {
                    rb_heartbeat.BackColor = System.Drawing.Color.Red;
                    this.MenuConnect.Text = Strings.CONNECTc;
                    // MainV2.comPort.MAV.cs.timeInAir = 0.0f;
                }
            }

            // rssi
            // ss_remrssi.Value = MainV2.comPort.MAV.cs.remrssi/255;
            ss_remrssi.Value = MainV2.comPort.MAV.cs.linkqualitygcs/100.0f;

            // AB ZhaoYJ@2017-03-27 for disarm & arm status 
            // ========= start =========
            this.label3.Font = new System.Drawing.Font("微软雅黑", 12, FontStyle.Bold);
            if (MainV2.comPort.MAV.cs.armed)
            {
                if (!label3.Text.Contains("飞机解锁"))
                {
                    lblog.Log(Level.Warning, "飞机解锁");
                }
                label3.Text = " 飞机解锁 ";

                label3.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                if (!label3.Text.Contains("飞机锁定"))
                {

                    lblog.Log(Level.Info, "飞机锁定");

                }
                label3.Text = " 飞机锁定 ";

                label3.ForeColor = System.Drawing.Color.Red;
            }
            // ========= end =========


            if (MissionPlanner.GCSViews.FlightData.myhud != null)
            {
                string mode = MainV2.comPort.MAV.cs.mode;
                //this.label2.Font = new Font(pfc.Families[0], label1.Font.Size);
                string last_mode = this.label2.Text;
                if ("Stabilize" == mode)
                {
                    this.label2.Text = "手动自稳";
                }
                else if ("Acro" == mode)
                {
                    this.label2.Text = "特技";
                }
                else if ("AltHold" == mode)
                {
                    this.label2.Text = "手动增稳";
                }
                else if ("Auto" == mode)
                {
                    this.label2.Text = "自动航线";
                }
                else if ("follow" == mode)
                {
                    this.label2.Text = "目标跟踪";
                }
                else if ("Guided" == mode)
                {
                    this.label2.Text = "定宽";
                }
                else if (mode.Equals("semi-forward"))
                {
                    this.label2.Text = "前向定宽";
                }
                else if (mode.Equals("ab-point"))
                {
                    this.label2.Text = "AB作业";
                }
                else if (mode.Equals("semi-backward"))
                {
                    this.label2.Text = "后向定宽";
                }
                else if ("Loiter" == mode)
                {
                    this.label2.Text = "自动悬停";
                }
                else if ("RTL" == mode)
                {
                    this.label2.Text = "返航降落";
                }
                else if ("Circle" == mode)
                {
                    this.label2.Text = "绕圈";
                }
                else if ("Land" == mode)
                {
                    this.label2.Text = "降落";
                }
                else if ("Drift" == mode)
                {
                    this.label2.Text = "飘移";
                }
                else if ("Sport" == mode)
                {
                    this.label2.Text = "运动";
                }
                else if ("Flip" == mode)
                {
                    this.label2.Text = "翻转";
                }
                else if ("AutoTune" == mode)
                {
                    this.label2.Text = "自动调参";
                }
                else if ("PosHold" == mode)
                {
                    this.label2.Text = "定点";
                }
                else if ("Brake" == mode)
                {
                    this.label2.Text = "暂停";
                }
                else if ("Unknown" == mode)
                {
                    this.label2.Text = "未知";
                }

                if (last_mode != this.label2.Text)
                {
                    this.label2.Refresh();
                }
            }
        }

        private void MainV2_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void MenuUserAppConfig_Click(object sender, EventArgs e)
        {
            MyView.ShowScreen("UserAppConfig");
        }

        private void cb_cmd_CheckedChanged(object sender, EventArgs e)
        {
            
            if (true == cb_cmd.Checked)
            {
                FlightData.form_actions.Show();
            }
            else
            {
                FlightData.form_actions.Hide();
            }
        }

        private void cb_log_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_log.Checked)
            {
                if (!MainV2.comPort.MAV.cs.armed) // make sure not armed, WiFi mode will stop app
                {
                    if (MainV2.comPort.setParam("WF_EN", 1))
                    {
                        MessageBox.Show("下载日志说明：\n\n  请使用电脑无线网卡连接WiFi热点“FA-xxxx”", "提示：下载日志方法说明", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                LogForm form_downloadLog = new LogForm();
                // form_downloadLog.ControlBox = false;
                form_downloadLog.StartPosition = FormStartPosition.Manual;
                form_downloadLog.Location = new Point(175, 210);
                form_downloadLog.Show();
            }
            else
            {
                // form_downloadLog.Hide();
            }
        }

        private void cb_tuningPID_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_tuningPID.Checked)
            {
                MainV2.comPort.MAV.cs.rateattitude = 10;
                MainV2.comPort.requestDatastream(MAVLink.MAV_DATA_STREAM.EXTRA1, MainV2.comPort.MAV.cs.rateattitude);
                panel_tuningPID.Show();
                ZedGraphTimer.Enabled = true;
                ZedGraphTimer.Start();
                zg1.Visible = true;
                zg1.Refresh();
            }
            else
            {
                panel_tuningPID.Hide();
                ZedGraphTimer.Enabled = false;
                ZedGraphTimer.Stop();
                zg1.Visible = false;
            }
        }

        private void rb_wifi_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_wifi.Checked)
            {
                comm_type = "TCP";
                if(!MainV2.comPort.MAV.cs.armed) // make sure not armed, WiFi mode will stop app
                {
                    if (MainV2.comPort.setParam("WF_EN", 1))
                    {
                        MessageBox.Show("WiFi连接方式下：\n\n  请在使用电脑无线网卡连接WiFi热点“FA-xxxx”，以下载飞行日志和升级飞控版本；\n\n  注意：WiFi下无法连接飞控查看数据!", "提示：WiFi连接方式说明", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

            }
        }

        private void rb_remradio_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_remradio.Checked)
            {
                comm_type = "Serial";
            }
        }
        // listBox_log

        int click_cnt = 0;
        private void bt_log_Click(object sender, EventArgs e)
        {
            if(click_cnt == 0)
            {
  
                listBox_log.Visible = true;
                bt_log.Text = "点击关闭\n运行详情";
            }
            else if(1 == (click_cnt & 0x1)) // second click
            {
                listBox_log.Visible = false;
                bt_log.Text = "点击查看\n运行详情";

            }
            else if(0 == (click_cnt & 0x1))
            {
                listBox_log.Visible = true;
                bt_log.Text = "点击关闭\n运行详情";


            }
            click_cnt++;

        }

        int click_cnt_wm = 0;
        private void btn_work_mode_Click(object sender, EventArgs e)
        {

            // volt then mean connected
            if(MainV2.comPort.MAV.cs._battery_voltage.Equals(0.0f))
            {
                CustomMessageBox.Show("注意：只有在连接飞控下，才可以切换作业模式");
                return;
            }

            if (0 == (click_cnt_wm & 0x1))  // 植保
            {

                // set work_mode, yaw_behavior
                if (MainV2.comPort.setParam("WP_YAW_BEHAVIOR", 0)) // 机头保持不变
                {
                    this.FlightPlanner.work_mode = "farmming";
                    btn_work_mode.Text = "植保模式";
                }
            }
            else if (1 == (click_cnt_wm & 0x1)) // second click, 测绘
            {
                // set work_mode, yaw_behavior
                if (MainV2.comPort.setParam("WP_YAW_BEHAVIOR", 1)) // 机头跟随航线
                {
                    this.FlightPlanner.work_mode = "survey";
                    btn_work_mode.Text = "测绘模式";
                }


            }

            click_cnt_wm++;
        }

        public void CreateChart(ZedGraphControl zgc)
        {
            GraphPane myPane = zgc.GraphPane;

            // Set the titles and axis labels
            myPane.Title.Text = "Tuning";
            myPane.XAxis.Title.Text = "Time (s)";
            myPane.YAxis.Title.Text = "Unit";

            // Show the x axis grid
            myPane.XAxis.MajorGrid.IsVisible = true;

            myPane.XAxis.Scale.Min = 0;
            myPane.XAxis.Scale.Max = 5;

            // Make the Y axis scale red
            myPane.YAxis.Scale.FontSpec.FontColor = Color.White;
            myPane.YAxis.Title.FontSpec.FontColor = Color.White;
            // turn off the opposite tics so the Y tics don't show up on the Y2 axis
            myPane.YAxis.MajorTic.IsOpposite = false;
            myPane.YAxis.MinorTic.IsOpposite = false;
            // Don't display the Y zero line
            myPane.YAxis.MajorGrid.IsZeroLine = true;
            // Align the Y axis labels so they are flush to the axis
            myPane.YAxis.Scale.Align = AlignP.Inside;
            // Manually set the axis range
            //myPane.YAxis.Scale.Min = -1;
            //myPane.YAxis.Scale.Max = 1;

            // Fill the axis background with a gradient
            //myPane.Chart.Fill = new Fill(Color.White, Color.LightGray, 45.0f);

            // Sample at 50ms intervals
            ZedGraphTimer.Interval = 200;
            //timer1.Enabled = true;
            //timer1.Start();


            // Calculate the Axis Scale Ranges
            //zgc.AxisChange();

            tickStart = Environment.TickCount;
        }

        private void timerZed_Tick(object sender, EventArgs e)
        {
            try
            {
                tuningLoop();

                // Make sure that the curvelist has at least one curve
                if (zg1.GraphPane.CurveList.Count <= 0)
                    return;

                // Get the first CurveItem in the graph
                LineItem curve = zg1.GraphPane.CurveList[0] as LineItem;
                if (curve == null)
                    return;

                // Get the PointPairList
                IPointListEdit list = curve.Points as IPointListEdit;
                // If this is null, it means the reference at curve.Points does not
                // support IPointListEdit, so we won't be able to modify it
                if (list == null)
                    return;

                // Time is measured in seconds
                double time = (Environment.TickCount - tickStart) / 1000.0;

                // Keep the X scale at a rolling 30 second interval, with one
                // major step between the max X value and the end of the axis
                Scale xScale = zg1.GraphPane.XAxis.Scale;
                if (time > xScale.Max - xScale.MajorStep)
                {
                    xScale.Max = time + xScale.MajorStep;
                    xScale.Min = xScale.Max - 10.0;
                }

                // Make sure the Y axis is rescaled to accommodate actual data
                zg1.AxisChange();

                // Force a redraw

                zg1.Invalidate();
            }
            catch
            {
            }
        }

        private void zg1_DoubleClick(object sender, EventArgs e)
        {
            string formname = "实时调试数据选项";
            Form selectform = Application.OpenForms[formname];
            if (selectform != null)
            {
                selectform.WindowState = FormWindowState.Minimized;
                selectform.Show();
                selectform.WindowState = FormWindowState.Normal;
                return;
            }

            selectform = new Form
            {
                Name = formname,
                Width = 800,
                Height = 300,
                Text = "实时调试数据选项"
            };

            int x = 10;
            int y = 10;

            {
                CheckBox chk_box = new CheckBox();
                chk_box.Text = "曲线平滑";
                chk_box.Name = "Logarithmic";
                chk_box.Location = new Point(x, y);
                chk_box.Size = new Size(100, 20);
                chk_box.CheckedChanged += chk_box_CheckedChanged;

                selectform.Controls.Add(chk_box);
            }

            ThemeManager.ApplyThemeTo(selectform);

            y += 20;

            object thisBoxed = MainV2.comPort.MAV.cs;
            Type test = thisBoxed.GetType();

            foreach (var field in test.GetProperties())
            {
                if (!(field.Name.Contains("AngRoll") || (field.Name.Contains("AngPitch"))
                    || field.Name.Contains("AngYaw")
                    || field.Name.Contains("RatRoll")
                    || field.Name.Contains("RatPitch")
                    || field.Name.Contains("RatYaw")
                    || field.Name.Contains("PosX")
                    || field.Name.Contains("PosZ")
                    || field.Name.Contains("VelX")
                    || field.Name.Contains("VelY")
                    || field.Name.Contains("VelZ")
                    || field.Name.Contains("AccZ")
                    || field.Name.Contains("M1")
                    || field.Name.Contains("M2")
                    || field.Name.Contains("M3")
                    || field.Name.Contains("M4")
                    || field.Name.Contains("M5")
                    || field.Name.Contains("M6")
                    || field.Name.Contains("M7")
                    || field.Name.Contains("M8")
                    ))
                {
                    continue;
                }

                    // field.Name has the field's name.
                 object fieldValue;
                TypeCode typeCode;
                try
                {
                    fieldValue = field.GetValue(thisBoxed, null); // Get value

                    if (fieldValue == null)
                        continue;

                    // Get the TypeCode enumeration. Multiple types get mapped to a common typecode.
                    typeCode = Type.GetTypeCode(fieldValue.GetType());
                }
                catch
                {
                    continue;
                }

                if (
                    !(typeCode == TypeCode.Single || typeCode == TypeCode.Double || typeCode == TypeCode.Int32 ||
                      typeCode == TypeCode.UInt16))
                    continue;

                CheckBox chk_box = new CheckBox();

                ThemeManager.ApplyThemeTo(chk_box);

                if (list1item != null && list1item.Name == field.Name)
                {
                    chk_box.Checked = true;
                    chk_box.BackColor = Color.Green;
                }
                if (list2item != null && list2item.Name == field.Name)
                {
                    chk_box.Checked = true;
                    chk_box.BackColor = Color.Green;
                }
                if (list3item != null && list3item.Name == field.Name)
                {
                    chk_box.Checked = true;
                    chk_box.BackColor = Color.Green;
                }
                if (list4item != null && list4item.Name == field.Name)
                {
                    chk_box.Checked = true;
                    chk_box.BackColor = Color.Green;
                }
                if (list5item != null && list5item.Name == field.Name)
                {
                    chk_box.Checked = true;
                    chk_box.BackColor = Color.Green;
                }
                if (list6item != null && list6item.Name == field.Name)
                {
                    chk_box.Checked = true;
                    chk_box.BackColor = Color.Green;
                }
                if (list7item != null && list7item.Name == field.Name)
                {
                    chk_box.Checked = true;
                    chk_box.BackColor = Color.Green;
                }
                if (list8item != null && list8item.Name == field.Name)
                {
                    chk_box.Checked = true;
                    chk_box.BackColor = Color.Green;
                }
                if (list9item != null && list9item.Name == field.Name)
                {
                    chk_box.Checked = true;
                    chk_box.BackColor = Color.Green;
                }
                if (list10item != null && list10item.Name == field.Name)
                {
                    chk_box.Checked = true;
                    chk_box.BackColor = Color.Green;
                }

                chk_box.Text = field.Name;
#if true
                // translate
                if(field.Name.Contains("M"))
                {
                    chk_box.Text = chk_box.Text.Replace("M", "电机");
                }
                else if (field.Name.Contains("Ang"))
                {
                    chk_box.Text = chk_box.Text.Replace("Ang", "角度");
                }
                else if (field.Name.Contains("Rat"))
                {
                    chk_box.Text = chk_box.Text.Replace("Rat", "角速度");
                }
                else if (field.Name.Contains("Pos"))
                {
                    chk_box.Text = chk_box.Text.Replace("Pos", "位置");
                }
                else if (field.Name.Contains("Vel"))
                {
                    chk_box.Text = chk_box.Text.Replace("Vel", "速度");
                }
                else if (field.Name.Contains("AccZ"))
                {
                    chk_box.Text = chk_box.Text.Replace("AccZ", "油门");
                }

                // just replace
                chk_box.Text = chk_box.Text.Replace("Act", "实时值");
                chk_box.Text = chk_box.Text.Replace("Tar", "目标值");
                chk_box.Text = chk_box.Text.Replace("pid", "");
#endif

                chk_box.Name = field.Name;
                chk_box.Location = new Point(x, y);
                chk_box.Size = new Size(130, 20);
                chk_box.CheckedChanged += chk_box_CheckedChanged;

                selectform.Controls.Add(chk_box);

                Application.DoEvents();

                x += 0;
                y += 20;

                if (y > selectform.Height - 60)
                {
                    x += 130;
                    y = 10;

                    selectform.Width = x + 130;
                }
            }

            selectform.Show();
        }

        bool setupPropertyInfo(ref PropertyInfo input, string name, object source)
        {
            Type test = source.GetType();

            foreach (var field in test.GetProperties())
            {
                if (field.Name == name)
                {
                    input = field;
                    return true;
                }
            }

            return false;
        }

        void chk_box_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                ((CheckBox)sender).BackColor = Color.Green;

                if (list1item == null)
                {
                    if (setupPropertyInfo(ref list1item, ((CheckBox)sender).Name, MainV2.comPort.MAV.cs))
                    {
                        list1.Clear();
                        list1curve = zg1.GraphPane.AddCurve(((CheckBox)sender).Name, list1, Color.Red, SymbolType.None);
                    }
                }
                else if (list2item == null)
                {
                    if (setupPropertyInfo(ref list2item, ((CheckBox)sender).Name, MainV2.comPort.MAV.cs))
                    {
                        list2.Clear();
                        list2curve = zg1.GraphPane.AddCurve(((CheckBox)sender).Name, list2, Color.Blue, SymbolType.None);
                    }
                }
                else if (list3item == null)
                {
                    if (setupPropertyInfo(ref list3item, ((CheckBox)sender).Name, MainV2.comPort.MAV.cs))
                    {
                        list3.Clear();
                        list3curve = zg1.GraphPane.AddCurve(((CheckBox)sender).Name, list3, Color.Green,
                            SymbolType.None);
                    }
                }
                else if (list4item == null)
                {
                    if (setupPropertyInfo(ref list4item, ((CheckBox)sender).Name, MainV2.comPort.MAV.cs))
                    {
                        list4.Clear();
                        list4curve = zg1.GraphPane.AddCurve(((CheckBox)sender).Name, list4, Color.Orange,
                            SymbolType.None);
                    }
                }
                else if (list5item == null)
                {
                    if (setupPropertyInfo(ref list5item, ((CheckBox)sender).Name, MainV2.comPort.MAV.cs))
                    {
                        list5.Clear();
                        list5curve = zg1.GraphPane.AddCurve(((CheckBox)sender).Name, list5, Color.Yellow,
                            SymbolType.None);
                    }
                }
                else if (list6item == null)
                {
                    if (setupPropertyInfo(ref list6item, ((CheckBox)sender).Name, MainV2.comPort.MAV.cs))
                    {
                        list6.Clear();
                        list6curve = zg1.GraphPane.AddCurve(((CheckBox)sender).Name, list6, Color.Magenta,
                            SymbolType.None);
                    }
                }
                else if (list7item == null)
                {
                    if (setupPropertyInfo(ref list7item, ((CheckBox)sender).Name, MainV2.comPort.MAV.cs))
                    {
                        list7.Clear();
                        list7curve = zg1.GraphPane.AddCurve(((CheckBox)sender).Name, list7, Color.Purple,
                            SymbolType.None);
                    }
                }
                else if (list8item == null)
                {
                    if (setupPropertyInfo(ref list8item, ((CheckBox)sender).Name, MainV2.comPort.MAV.cs))
                    {
                        list8.Clear();
                        list8curve = zg1.GraphPane.AddCurve(((CheckBox)sender).Name, list8, Color.LimeGreen,
                            SymbolType.None);
                    }
                }
                else if (list9item == null)
                {
                    if (setupPropertyInfo(ref list9item, ((CheckBox)sender).Name, MainV2.comPort.MAV.cs))
                    {
                        list9.Clear();
                        list9curve = zg1.GraphPane.AddCurve(((CheckBox)sender).Name, list9, Color.Cyan, SymbolType.None);
                    }
                }
                else if (list10item == null)
                {
                    if (setupPropertyInfo(ref list10item, ((CheckBox)sender).Name, MainV2.comPort.MAV.cs))
                    {
                        list10.Clear();
                        list10curve = zg1.GraphPane.AddCurve(((CheckBox)sender).Name, list10, Color.Violet,
                            SymbolType.None);
                    }
                }
                else
                {
                    CustomMessageBox.Show("Max 10 at a time.");
                    ((CheckBox)sender).Checked = false;
                }
                ThemeManager.ApplyThemeTo((Control)sender);

                string selected = "";
                try
                {
                    foreach (var curve in zg1.GraphPane.CurveList)
                    {
                        selected = selected + curve.Label.Text + "|";
                    }

                    // enable gcs_mask
                    int enable = 1;
                    if(((CheckBox)sender).Name.Contains("Ang"))
                    {
                        MainV2.comPort.setParam("GCS_PID_MASK", enable);
                    }
                    // enable gcs_mask
                    else if (((CheckBox)sender).Name.Contains("RatRoll"))
                    {
                        MainV2.comPort.setParam("GCS_PID_MASK", enable << 3);
                    }
                    // enable gcs_mask
                    else if (((CheckBox)sender).Name.Contains("RatPitch")) //  || ((CheckBox)sender).Name.Contains("RatRoll"))
                    {
                        MainV2.comPort.setParam("GCS_PID_MASK", enable << 4);
                    }
                    // enable gcs_mask
                    else if (((CheckBox)sender).Name.Contains("RatYaw")) //  || ((CheckBox)sender).Name.Contains("RatRoll"))
                    {
                        MainV2.comPort.setParam("GCS_PID_MASK", enable << 5);
                    }
                    else if (((CheckBox)sender).Name.Contains("AccZ")) //  || ((CheckBox)sender).Name.Contains("RatRoll"))
                    {
                        MainV2.comPort.setParam("GCS_PID_MASK", enable << 6);
                    }
                    // enable gcs_mask
                    else if (((CheckBox)sender).Name.Contains("VelX")) //  || ((CheckBox)sender).Name.Contains("RatRoll"))
                    {
                        MainV2.comPort.setParam("GCS_PID_MASK", enable << 9);
                    }
                    // enable gcs_mask
                    else if (((CheckBox)sender).Name.Contains("VelY")) //  || ((CheckBox)sender).Name.Contains("RatRoll"))
                    {
                        MainV2.comPort.setParam("GCS_PID_MASK", enable << 10);
                    }
                    else if (((CheckBox)sender).Name.Contains("PosX") || ((CheckBox)sender).Name.Contains("PosZ") || ((CheckBox)sender).Name.Contains("VelZ")) //  || ((CheckBox)sender).Name.Contains("RatRoll"))
                    {
                        MainV2.comPort.setParam("GCS_PID_MASK", enable << 11);
                    }
                }
                catch
                {
                }
                Settings.Instance["Tuning_Graph_Selected"] = selected;
            }
            else
            {
                ((CheckBox)sender).BackColor = Color.Transparent;

                // reset old stuff
                if (list1item != null && list1item.Name == ((CheckBox)sender).Name)
                {
                    list1item = null;
                    zg1.GraphPane.CurveList.Remove(list1curve);
                }
                if (list2item != null && list2item.Name == ((CheckBox)sender).Name)
                {
                    list2item = null;
                    zg1.GraphPane.CurveList.Remove(list2curve);
                }
                if (list3item != null && list3item.Name == ((CheckBox)sender).Name)
                {
                    list3item = null;
                    zg1.GraphPane.CurveList.Remove(list3curve);
                }
                if (list4item != null && list4item.Name == ((CheckBox)sender).Name)
                {
                    list4item = null;
                    zg1.GraphPane.CurveList.Remove(list4curve);
                }
                if (list5item != null && list5item.Name == ((CheckBox)sender).Name)
                {
                    list5item = null;
                    zg1.GraphPane.CurveList.Remove(list5curve);
                }
                if (list6item != null && list6item.Name == ((CheckBox)sender).Name)
                {
                    list6item = null;
                    zg1.GraphPane.CurveList.Remove(list6curve);
                }
                if (list7item != null && list7item.Name == ((CheckBox)sender).Name)
                {
                    list7item = null;
                    zg1.GraphPane.CurveList.Remove(list7curve);
                }
                if (list8item != null && list8item.Name == ((CheckBox)sender).Name)
                {
                    list8item = null;
                    zg1.GraphPane.CurveList.Remove(list8curve);
                }
                if (list9item != null && list9item.Name == ((CheckBox)sender).Name)
                {
                    list9item = null;
                    zg1.GraphPane.CurveList.Remove(list9curve);
                }
                if (list10item != null && list10item.Name == ((CheckBox)sender).Name)
                {
                    list10item = null;
                    zg1.GraphPane.CurveList.Remove(list10curve);
                }

                // enable gcs_mask
                int enable = (int)MainV2.comPort.GetParam("GCS_PID_MASK");
                if (((CheckBox)sender).Name.Contains("Ang"))
                {
                    MainV2.comPort.setParam("GCS_PID_MASK", enable & (~(1 << 0)));
                }
                // enable gcs_mask
                else if (((CheckBox)sender).Name.Contains("RatRoll"))
                {
                    MainV2.comPort.setParam("GCS_PID_MASK", enable & (~(1 << 3)));
                }
                // enable gcs_mask
                else if (((CheckBox)sender).Name.Contains("RatPitch")) //  || ((CheckBox)sender).Name.Contains("RatRoll"))
                {
                    MainV2.comPort.setParam("GCS_PID_MASK", enable & (~(1 << 4)));
                }
                // enable gcs_mask
                else if (((CheckBox)sender).Name.Contains("RatYaw")) //  || ((CheckBox)sender).Name.Contains("RatRoll"))
                {
                    MainV2.comPort.setParam("GCS_PID_MASK", enable & (~(1 << 5)));
                }
                // enable gcs_mask
                else if (((CheckBox)sender).Name.Contains("AccZ")) //  || ((CheckBox)sender).Name.Contains("RatRoll"))
                {
                    MainV2.comPort.setParam("GCS_PID_MASK", enable & (~(1 << 6)));
                }
                // enable gcs_mask
                else if (((CheckBox)sender).Name.Contains("VelX")) //  || ((CheckBox)sender).Name.Contains("RatRoll"))
                {
                    MainV2.comPort.setParam("GCS_PID_MASK", enable & (~(1 << 9)));
                }
                // enable gcs_mask
                else if (((CheckBox)sender).Name.Contains("VelY")) //  || ((CheckBox)sender).Name.Contains("RatRoll"))
                {
                    MainV2.comPort.setParam("GCS_PID_MASK", enable & (~(1 << 10)));
                }
                else if (((CheckBox)sender).Name.Contains("PosX") ||　((CheckBox)sender).Name.Contains("PosZ") || ((CheckBox)sender).Name.Contains("VelZ")) //  || ((CheckBox)sender).Name.Contains("RatRoll"))
                {
                    MainV2.comPort.setParam("GCS_PID_MASK", enable & (~(1 << 11)));
                }
            }
        }

    }
}