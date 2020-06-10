namespace MissionPlanner.GCSViews
{
    partial class FlightPlanner
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            if (currentMarker != null)
                currentMarker.Dispose();
            if (drawnpolygon != null)
                drawnpolygon.Dispose();
            if (kmlpolygonsoverlay != null)
                kmlpolygonsoverlay.Dispose();
            if (wppolygon != null)
                wppolygon.Dispose();
            if (top != null)
                top.Dispose();
            if (geofencepolygon != null)
                geofencepolygon.Dispose();
            if (geofenceoverlay != null)
                geofenceoverlay.Dispose();
            if (drawnpolygonsoverlay != null)
                drawnpolygonsoverlay.Dispose();
            if (center != null)
                center.Dispose(); 

            base.Dispose(disposing);
        }


        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FlightPlanner));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            this.Commands = new System.Windows.Forms.DataGridView();
            this.Command = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Param1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Param2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Param3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Param4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Lat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Lon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Alt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.coordZone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.coordEasting = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.coordNorthing = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MGRS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Delete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Up = new System.Windows.Forms.DataGridViewImageColumn();
            this.Down = new System.Windows.Forms.DataGridViewImageColumn();
            this.Grad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Angle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Dist = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AZ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TagData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CHK_verifyheight = new System.Windows.Forms.CheckBox();
            this.TXT_WPRad = new System.Windows.Forms.TextBox();
            this.TXT_DefaultAlt = new System.Windows.Forms.TextBox();
            this.LBL_WPRad = new System.Windows.Forms.Label();
            this.LBL_defalutalt = new System.Windows.Forms.Label();
            this.TXT_loiterrad = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.BUT_write = new MissionPlanner.Controls.MyButton();
            this.BUT_read = new MissionPlanner.Controls.MyButton();
            this.cb_en_autoTORTL = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.lb_search_map = new MissionPlanner.Controls.MyButton();
            this.tb_place = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.TXT_homealt = new System.Windows.Forms.TextBox();
            this.TXT_homelng = new System.Windows.Forms.TextBox();
            this.TXT_homelat = new System.Windows.Forms.TextBox();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn2 = new System.Windows.Forms.DataGridViewImageColumn();
            this.label6 = new System.Windows.Forms.Label();
            this.coords1 = new MissionPlanner.Controls.Coords();
            this.lbl_status = new System.Windows.Forms.Label();
            this.panelWaypoints = new BSE.Windows.Forms.Panel();
            this.splitter1 = new BSE.Windows.Forms.Splitter();
            this.panel6 = new System.Windows.Forms.Panel();
            this.wp_migrate = new MissionPlanner.Controls.MyButton();
            this.tb_RTL_hgt = new System.Windows.Forms.NumericUpDown();
            this.comboBoxMapType = new System.Windows.Forms.ComboBox();
            this.tb_autoTO_hgt = new System.Windows.Forms.NumericUpDown();
            this.BUT_planning = new MissionPlanner.Controls.MyButton();
            this.btn_wphgt_mod = new MissionPlanner.Controls.MyButton();
            this.lb_wp_revert = new MissionPlanner.Controls.MyButton();
            this.bt_edit_wps = new MissionPlanner.Controls.MyButton();
            this.BUT_generate = new MissionPlanner.Controls.MyButton();
            this.lb_clear_polygon = new MissionPlanner.Controls.MyButton();
            this.lb_autotakeoff_hgt = new System.Windows.Forms.Label();
            this.lb_rtl_hgt = new System.Windows.Forms.Label();
            this.CMB_altmode = new System.Windows.Forms.ComboBox();
            this.CHK_splinedefault = new System.Windows.Forms.CheckBox();
            this.label17 = new System.Windows.Forms.Label();
            this.TXT_altwarn = new System.Windows.Forms.TextBox();
            this.BUT_Add = new MissionPlanner.Controls.MyButton();
            this.panelAction = new BSE.Windows.Forms.Panel();
            this.splitter2 = new BSE.Windows.Forms.Splitter();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.map_corr_auto = new System.Windows.Forms.CheckBox();
            this.chk_grid = new System.Windows.Forms.CheckBox();
            this.lnk_kml = new System.Windows.Forms.LinkLabel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.BUT_loadwpfile = new MissionPlanner.Controls.MyButton();
            this.BUT_saveWPFile = new MissionPlanner.Controls.MyButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.myButton2 = new MissionPlanner.Controls.MyButton();
            this.myButton3 = new MissionPlanner.Controls.MyButton();
            this.panel10 = new System.Windows.Forms.Panel();
            this.rb_bj = new System.Windows.Forms.RadioButton();
            this.rb_wp = new System.Windows.Forms.RadioButton();
            this.lb_load_kml = new MissionPlanner.Controls.MyButton();
            this.panel9 = new System.Windows.Forms.Panel();
            this.lb_download_map = new MissionPlanner.Controls.MyButton();
            this.myButton1 = new MissionPlanner.Controls.MyButton();
            this.panel4 = new System.Windows.Forms.Panel();
            this.load_last_offs = new MissionPlanner.Controls.MyButton();
            this.map_corr_manual = new MissionPlanner.Controls.MyButton();
            this.panelMap = new System.Windows.Forms.Panel();
            this.panel_avoidPnts = new BSE.Windows.Forms.Panel();
            this.rb_avoidPntsOk = new GaryPerkin.UserControls.Buttons.RoundButton();
            this.rb_avoidPntsAdd = new GaryPerkin.UserControls.Buttons.RoundButton();
            this.dataGridView_avoidPnts = new System.Windows.Forms.DataGridView();
            this.pannel_wpcommands = new BSE.Windows.Forms.Panel();
            this.rb_avoid_pnts = new GaryPerkin.UserControls.Buttons.RoundButton();
            this.rb_en_lidar = new GaryPerkin.UserControls.Buttons.RoundButton();
            this.rb_clear_polygon = new GaryPerkin.UserControls.Buttons.RoundButton();
            this.rb_clear_mission = new GaryPerkin.UserControls.Buttons.RoundButton();
            this.rb_wp_hgt = new GaryPerkin.UserControls.Buttons.RoundButton();
            this.rb_editwpcommand = new GaryPerkin.UserControls.Buttons.RoundButton();
            this.rb_wpmigrate = new GaryPerkin.UserControls.Buttons.RoundButton();
            this.rb_corr = new GaryPerkin.UserControls.Buttons.RoundButton();
            this.rb_inv_wps = new GaryPerkin.UserControls.Buttons.RoundButton();
            this.rb_autoWP = new GaryPerkin.UserControls.Buttons.RoundButton();
            this.rb_center = new GaryPerkin.UserControls.Buttons.RoundButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbl_prevdist = new System.Windows.Forms.Label();
            this.lbl_ftime = new System.Windows.Forms.Label();
            this.lbl_homedist = new System.Windows.Forms.Label();
            this.lbl_distance = new System.Windows.Forms.Label();
            this.gb_modWPhgt = new System.Windows.Forms.GroupBox();
            this.btn_cancel = new MissionPlanner.Controls.MyButton();
            this.btn_modconfirm = new MissionPlanner.Controls.MyButton();
            this.panel11 = new System.Windows.Forms.Panel();
            this.num_endWP = new System.Windows.Forms.NumericUpDown();
            this.num_startWP = new System.Windows.Forms.NumericUpDown();
            this.lineSeparator1 = new MissionPlanner.Controls.LineSeparator();
            this.rbtn_sel = new System.Windows.Forms.RadioButton();
            this.rbtn_modall = new System.Windows.Forms.RadioButton();
            this.num_newhgt = new System.Windows.Forms.NumericUpDown();
            this.lb_newWPhgt = new System.Windows.Forms.Label();
            this.MainMap = new MissionPlanner.Controls.myGMAP();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ContextMeasure = new System.Windows.Forms.ToolStripMenuItem();
            this.insertWpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.genWPsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteWPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wpEditToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editWpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.doneWpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wpManageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wpUploadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wpDownloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wpSaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wpOpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.polyEditToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addPolygonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.donePolygonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.polySplitToolStripMenuItemAll = new System.Windows.Forms.ToolStripMenuItem();
            this.polySplitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DonePolySplitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DeletePolySplitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bjToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadPolygonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.savePolygonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kmlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadKMLbjStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadKMLWPStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapToolStripMenuItem_simp = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.prefetchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearMissionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setHomeHereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rallyPointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setRallyPointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearRallyPointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getRallyPointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveRallyPointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToFileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.loadFromFileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.trackBar1 = new MissionPlanner.Controls.MyTrackBar();
            this.label11 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.insertSplineWPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loiterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loiterForeverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loitertimeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loitercirclesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jumpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jumpstartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jumpwPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rTLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.landToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.takeoffToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setROIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.polygonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addPolygonPointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearPolygonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fromSHPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.areaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.geoFenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.GeoFenceuploadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GeoFencedownloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setReturnLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadFromFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoWPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createWpCircleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createSplineCircleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.areaToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.textToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapToolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rotateMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.prefetchWPPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kMLOverlayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.elevationGraphToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reverseWPsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileLoadSaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadWPFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadAndAppendToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveWPFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadKMLFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadSHPFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pOIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.poiaddToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.poideleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.poieditToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trackerHomeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modifyAltToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enterUTMCoordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.switchDockingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelBASE = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.directorySearcher1 = new System.DirectoryServices.DirectorySearcher();
            this.avoidPntLat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.avoidPntLng = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.avoidPntRadius = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.avoidPntsInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Manage = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.Commands)).BeginInit();
            this.panel5.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panelWaypoints.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tb_RTL_hgt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_autoTO_hgt)).BeginInit();
            this.panelAction.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel10.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panelMap.SuspendLayout();
            this.panel_avoidPnts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_avoidPnts)).BeginInit();
            this.pannel_wpcommands.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gb_modWPhgt.SuspendLayout();
            this.panel11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_endWP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_startWP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_newhgt)).BeginInit();
            this.contextMenuStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.panelBASE.SuspendLayout();
            this.SuspendLayout();
            // 
            // Commands
            // 
            this.Commands.AllowUserToAddRows = false;
            this.Commands.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCellsExceptHeader;
            this.Commands.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Commands.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.Commands, "Commands");
            this.Commands.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Command,
            this.Param1,
            this.Param2,
            this.Param3,
            this.Param4,
            this.Lat,
            this.Lon,
            this.Alt,
            this.coordZone,
            this.coordEasting,
            this.coordNorthing,
            this.MGRS,
            this.Delete,
            this.Up,
            this.Down,
            this.Grad,
            this.Angle,
            this.Dist,
            this.AZ,
            this.TagData});
            this.Commands.Name = "Commands";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.Format = "N0";
            dataGridViewCellStyle5.NullValue = "0";
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.Commands.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black;
            this.Commands.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.Commands.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Commands_CellContentClick);
            this.Commands.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.Commands_CellEndEdit);
            this.Commands.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.Commands_DataError);
            this.Commands.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.Commands_DefaultValuesNeeded);
            this.Commands.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Commands_EditingControlShowing);
            this.Commands.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.Commands_RowEnter);
            this.Commands.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.Commands_RowsAdded);
            this.Commands.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.Commands_RowValidating);
            // 
            // Command
            // 
            this.Command.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(68)))), ((int)(((byte)(69)))));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            this.Command.DefaultCellStyle = dataGridViewCellStyle2;
            this.Command.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            resources.ApplyResources(this.Command, "Command");
            this.Command.Name = "Command";
            // 
            // Param1
            // 
            this.Param1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            resources.ApplyResources(this.Param1, "Param1");
            this.Param1.Name = "Param1";
            this.Param1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Param2
            // 
            this.Param2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            resources.ApplyResources(this.Param2, "Param2");
            this.Param2.Name = "Param2";
            this.Param2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Param3
            // 
            this.Param3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            resources.ApplyResources(this.Param3, "Param3");
            this.Param3.Name = "Param3";
            this.Param3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Param4
            // 
            this.Param4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            resources.ApplyResources(this.Param4, "Param4");
            this.Param4.Name = "Param4";
            this.Param4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Lat
            // 
            this.Lat.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            resources.ApplyResources(this.Lat, "Lat");
            this.Lat.Name = "Lat";
            this.Lat.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Lon
            // 
            this.Lon.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            resources.ApplyResources(this.Lon, "Lon");
            this.Lon.Name = "Lon";
            this.Lon.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Alt
            // 
            this.Alt.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            resources.ApplyResources(this.Alt, "Alt");
            this.Alt.Name = "Alt";
            this.Alt.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // coordZone
            // 
            resources.ApplyResources(this.coordZone, "coordZone");
            this.coordZone.Name = "coordZone";
            // 
            // coordEasting
            // 
            resources.ApplyResources(this.coordEasting, "coordEasting");
            this.coordEasting.Name = "coordEasting";
            // 
            // coordNorthing
            // 
            resources.ApplyResources(this.coordNorthing, "coordNorthing");
            this.coordNorthing.Name = "coordNorthing";
            // 
            // MGRS
            // 
            resources.ApplyResources(this.MGRS, "MGRS");
            this.MGRS.Name = "MGRS";
            // 
            // Delete
            // 
            this.Delete.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            resources.ApplyResources(this.Delete, "Delete");
            this.Delete.Name = "Delete";
            this.Delete.Text = "X";
            // 
            // Up
            // 
            this.Up.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            this.Up.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.Up, "Up");
            this.Up.Image = ((System.Drawing.Image)(resources.GetObject("Up.Image")));
            this.Up.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Stretch;
            this.Up.Name = "Up";
            // 
            // Down
            // 
            this.Down.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Down.DefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this.Down, "Down");
            this.Down.Image = ((System.Drawing.Image)(resources.GetObject("Down.Image")));
            this.Down.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Stretch;
            this.Down.Name = "Down";
            // 
            // Grad
            // 
            this.Grad.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            resources.ApplyResources(this.Grad, "Grad");
            this.Grad.Name = "Grad";
            this.Grad.ReadOnly = true;
            this.Grad.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Angle
            // 
            this.Angle.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            resources.ApplyResources(this.Angle, "Angle");
            this.Angle.Name = "Angle";
            this.Angle.ReadOnly = true;
            this.Angle.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Dist
            // 
            this.Dist.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            resources.ApplyResources(this.Dist, "Dist");
            this.Dist.Name = "Dist";
            this.Dist.ReadOnly = true;
            this.Dist.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // AZ
            // 
            this.AZ.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            resources.ApplyResources(this.AZ, "AZ");
            this.AZ.Name = "AZ";
            this.AZ.ReadOnly = true;
            this.AZ.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // TagData
            // 
            resources.ApplyResources(this.TagData, "TagData");
            this.TagData.Name = "TagData";
            this.TagData.ReadOnly = true;
            // 
            // CHK_verifyheight
            // 
            resources.ApplyResources(this.CHK_verifyheight, "CHK_verifyheight");
            this.CHK_verifyheight.Name = "CHK_verifyheight";
            this.CHK_verifyheight.UseVisualStyleBackColor = true;
            // 
            // TXT_WPRad
            // 
            resources.ApplyResources(this.TXT_WPRad, "TXT_WPRad");
            this.TXT_WPRad.Name = "TXT_WPRad";
            this.TXT_WPRad.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_WPRad_KeyPress);
            this.TXT_WPRad.Leave += new System.EventHandler(this.TXT_WPRad_Leave);
            // 
            // TXT_DefaultAlt
            // 
            resources.ApplyResources(this.TXT_DefaultAlt, "TXT_DefaultAlt");
            this.TXT_DefaultAlt.Name = "TXT_DefaultAlt";
            this.TXT_DefaultAlt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_DefaultAlt_KeyPress);
            this.TXT_DefaultAlt.Leave += new System.EventHandler(this.TXT_DefaultAlt_Leave);
            // 
            // LBL_WPRad
            // 
            resources.ApplyResources(this.LBL_WPRad, "LBL_WPRad");
            this.LBL_WPRad.Name = "LBL_WPRad";
            // 
            // LBL_defalutalt
            // 
            resources.ApplyResources(this.LBL_defalutalt, "LBL_defalutalt");
            this.LBL_defalutalt.Name = "LBL_defalutalt";
            // 
            // TXT_loiterrad
            // 
            resources.ApplyResources(this.TXT_loiterrad, "TXT_loiterrad");
            this.TXT_loiterrad.Name = "TXT_loiterrad";
            this.TXT_loiterrad.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_loiterrad_KeyPress);
            this.TXT_loiterrad.Leave += new System.EventHandler(this.TXT_loiterrad_Leave);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel5.Controls.Add(this.BUT_write);
            this.panel5.Controls.Add(this.BUT_read);
            resources.ApplyResources(this.panel5, "panel5");
            this.panel5.Name = "panel5";
            // 
            // BUT_write
            // 
            resources.ApplyResources(this.BUT_write, "BUT_write");
            this.BUT_write.Name = "BUT_write";
            this.BUT_write.UseVisualStyleBackColor = true;
            this.BUT_write.Click += new System.EventHandler(this.BUT_write_Click);
            // 
            // BUT_read
            // 
            resources.ApplyResources(this.BUT_read, "BUT_read");
            this.BUT_read.Name = "BUT_read";
            this.BUT_read.UseVisualStyleBackColor = true;
            this.BUT_read.Click += new System.EventHandler(this.BUT_read_Click);
            // 
            // cb_en_autoTORTL
            // 
            resources.ApplyResources(this.cb_en_autoTORTL, "cb_en_autoTORTL");
            this.cb_en_autoTORTL.ForeColor = System.Drawing.Color.Black;
            this.cb_en_autoTORTL.Name = "cb_en_autoTORTL";
            this.cb_en_autoTORTL.UseVisualStyleBackColor = true;
            this.cb_en_autoTORTL.CheckedChanged += new System.EventHandler(this.cb_en_autoTORTL_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightGray;
            this.panel1.Controls.Add(this.panel8);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.Label1);
            this.panel1.Controls.Add(this.TXT_homealt);
            this.panel1.Controls.Add(this.TXT_homelng);
            this.panel1.Controls.Add(this.TXT_homelat);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.panel8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel8.Controls.Add(this.lb_search_map);
            this.panel8.Controls.Add(this.tb_place);
            this.panel8.Controls.Add(this.label8);
            resources.ApplyResources(this.panel8, "panel8");
            this.panel8.Name = "panel8";
            // 
            // lb_search_map
            // 
            resources.ApplyResources(this.lb_search_map, "lb_search_map");
            this.lb_search_map.Name = "lb_search_map";
            this.lb_search_map.UseVisualStyleBackColor = true;
            this.lb_search_map.Click += new System.EventHandler(this.lb_search_map_Click);
            // 
            // tb_place
            // 
            resources.ApplyResources(this.tb_place, "tb_place");
            this.tb_place.Name = "tb_place";
            this.tb_place.TextChanged += new System.EventHandler(this.TXT_homelng_TextChanged);
            this.tb_place.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb_place_enter);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            this.label4.TabStop = true;
            this.label4.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.label4_LinkClicked);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // Label1
            // 
            resources.ApplyResources(this.Label1, "Label1");
            this.Label1.Name = "Label1";
            // 
            // TXT_homealt
            // 
            resources.ApplyResources(this.TXT_homealt, "TXT_homealt");
            this.TXT_homealt.Name = "TXT_homealt";
            this.TXT_homealt.TextChanged += new System.EventHandler(this.TXT_homealt_TextChanged);
            // 
            // TXT_homelng
            // 
            resources.ApplyResources(this.TXT_homelng, "TXT_homelng");
            this.TXT_homelng.Name = "TXT_homelng";
            this.TXT_homelng.TextChanged += new System.EventHandler(this.TXT_homelng_TextChanged);
            // 
            // TXT_homelat
            // 
            resources.ApplyResources(this.TXT_homelat, "TXT_homelat");
            this.TXT_homelat.Name = "TXT_homelat";
            this.TXT_homelat.TextChanged += new System.EventHandler(this.TXT_homelat_TextChanged);
            this.TXT_homelat.Enter += new System.EventHandler(this.TXT_homelat_Enter);
            // 
            // dataGridViewImageColumn1
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewImageColumn1.DefaultCellStyle = dataGridViewCellStyle7;
            resources.ApplyResources(this.dataGridViewImageColumn1, "dataGridViewImageColumn1");
            this.dataGridViewImageColumn1.Image = ((System.Drawing.Image)(resources.GetObject("dataGridViewImageColumn1.Image")));
            this.dataGridViewImageColumn1.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Stretch;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            // 
            // dataGridViewImageColumn2
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewImageColumn2.DefaultCellStyle = dataGridViewCellStyle8;
            resources.ApplyResources(this.dataGridViewImageColumn2, "dataGridViewImageColumn2");
            this.dataGridViewImageColumn2.Image = ((System.Drawing.Image)(resources.GetObject("dataGridViewImageColumn2.Image")));
            this.dataGridViewImageColumn2.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Stretch;
            this.dataGridViewImageColumn2.Name = "dataGridViewImageColumn2";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // coords1
            // 
            this.coords1.Alt = 0D;
            this.coords1.AltSource = "";
            this.coords1.AltUnit = "米";
            this.coords1.BackColor = System.Drawing.Color.Transparent;
            this.coords1.Lat = 0D;
            this.coords1.Lng = 0D;
            resources.ApplyResources(this.coords1, "coords1");
            this.coords1.Name = "coords1";
            this.coords1.Vertical = true;
            this.coords1.SystemChanged += new System.EventHandler(this.coords1_SystemChanged);
            // 
            // lbl_status
            // 
            resources.ApplyResources(this.lbl_status, "lbl_status");
            this.lbl_status.Name = "lbl_status";
            // 
            // panelWaypoints
            // 
            this.panelWaypoints.AssociatedSplitter = this.splitter1;
            this.panelWaypoints.BackColor = System.Drawing.Color.Transparent;
            this.panelWaypoints.CaptionFont = new System.Drawing.Font("Segoe UI", 11.75F, System.Drawing.FontStyle.Bold);
            this.panelWaypoints.CaptionHeight = 21;
            this.panelWaypoints.ColorScheme = BSE.Windows.Forms.ColorScheme.Custom;
            this.panelWaypoints.Controls.Add(this.panel6);
            this.panelWaypoints.Controls.Add(this.CMB_altmode);
            this.panelWaypoints.Controls.Add(this.CHK_splinedefault);
            this.panelWaypoints.Controls.Add(this.label17);
            this.panelWaypoints.Controls.Add(this.TXT_altwarn);
            this.panelWaypoints.Controls.Add(this.LBL_WPRad);
            this.panelWaypoints.Controls.Add(this.label5);
            this.panelWaypoints.Controls.Add(this.TXT_loiterrad);
            this.panelWaypoints.Controls.Add(this.LBL_defalutalt);
            this.panelWaypoints.Controls.Add(this.TXT_DefaultAlt);
            this.panelWaypoints.Controls.Add(this.CHK_verifyheight);
            this.panelWaypoints.Controls.Add(this.TXT_WPRad);
            this.panelWaypoints.Controls.Add(this.BUT_Add);
            this.panelWaypoints.CustomColors.BorderColor = System.Drawing.Color.Black;
            this.panelWaypoints.CustomColors.CaptionCloseIcon = System.Drawing.Color.White;
            this.panelWaypoints.CustomColors.CaptionExpandIcon = System.Drawing.Color.White;
            this.panelWaypoints.CustomColors.CaptionGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(193)))), ((int)(((byte)(31)))));
            this.panelWaypoints.CustomColors.CaptionGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(226)))), ((int)(((byte)(150)))));
            this.panelWaypoints.CustomColors.CaptionGradientMiddle = System.Drawing.Color.Transparent;
            this.panelWaypoints.CustomColors.CaptionSelectedGradientBegin = System.Drawing.Color.Transparent;
            this.panelWaypoints.CustomColors.CaptionSelectedGradientEnd = System.Drawing.Color.Transparent;
            this.panelWaypoints.CustomColors.CaptionText = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.panelWaypoints.CustomColors.CollapsedCaptionText = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.panelWaypoints.CustomColors.ContentGradientBegin = System.Drawing.SystemColors.ButtonFace;
            this.panelWaypoints.CustomColors.ContentGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.panelWaypoints.CustomColors.InnerBorderColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.panelWaypoints, "panelWaypoints");
            this.panelWaypoints.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panelWaypoints.Image = null;
            this.panelWaypoints.Name = "panelWaypoints";
            this.panelWaypoints.ShowExpandIcon = true;
            this.panelWaypoints.ToolTipTextCloseIcon = null;
            this.panelWaypoints.ToolTipTextExpandIconPanelCollapsed = null;
            this.panelWaypoints.ToolTipTextExpandIconPanelExpanded = null;
            this.panelWaypoints.ExpandClick += new System.EventHandler<System.EventArgs>(this.panelWaypoints_ExpandClick);
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.splitter1, "splitter1");
            this.splitter1.Name = "splitter1";
            this.splitter1.TabStop = false;
            // 
            // panel6
            // 
            resources.ApplyResources(this.panel6, "panel6");
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.wp_migrate);
            this.panel6.Controls.Add(this.tb_RTL_hgt);
            this.panel6.Controls.Add(this.comboBoxMapType);
            this.panel6.Controls.Add(this.tb_autoTO_hgt);
            this.panel6.Controls.Add(this.BUT_planning);
            this.panel6.Controls.Add(this.btn_wphgt_mod);
            this.panel6.Controls.Add(this.lb_wp_revert);
            this.panel6.Controls.Add(this.cb_en_autoTORTL);
            this.panel6.Controls.Add(this.bt_edit_wps);
            this.panel6.Controls.Add(this.BUT_generate);
            this.panel6.Controls.Add(this.lb_clear_polygon);
            this.panel6.Controls.Add(this.lb_autotakeoff_hgt);
            this.panel6.Controls.Add(this.lb_rtl_hgt);
            this.panel6.Name = "panel6";
            // 
            // wp_migrate
            // 
            resources.ApplyResources(this.wp_migrate, "wp_migrate");
            this.wp_migrate.Name = "wp_migrate";
            this.wp_migrate.UseVisualStyleBackColor = true;
            this.wp_migrate.Click += new System.EventHandler(this.wp_migrate_Click);
            // 
            // tb_RTL_hgt
            // 
            resources.ApplyResources(this.tb_RTL_hgt, "tb_RTL_hgt");
            this.tb_RTL_hgt.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.tb_RTL_hgt.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.tb_RTL_hgt.Name = "tb_RTL_hgt";
            this.tb_RTL_hgt.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // comboBoxMapType
            // 
            this.comboBoxMapType.BackColor = System.Drawing.SystemColors.Highlight;
            this.comboBoxMapType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMapType.FormattingEnabled = true;
            resources.ApplyResources(this.comboBoxMapType, "comboBoxMapType");
            this.comboBoxMapType.Name = "comboBoxMapType";
            this.toolTip1.SetToolTip(this.comboBoxMapType, resources.GetString("comboBoxMapType.ToolTip"));
            // 
            // tb_autoTO_hgt
            // 
            resources.ApplyResources(this.tb_autoTO_hgt, "tb_autoTO_hgt");
            this.tb_autoTO_hgt.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.tb_autoTO_hgt.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.tb_autoTO_hgt.Name = "tb_autoTO_hgt";
            this.tb_autoTO_hgt.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // BUT_planning
            // 
            resources.ApplyResources(this.BUT_planning, "BUT_planning");
            this.BUT_planning.Name = "BUT_planning";
            this.BUT_planning.UseVisualStyleBackColor = true;
            this.BUT_planning.Click += new System.EventHandler(this.BUT_planning_Click);
            // 
            // btn_wphgt_mod
            // 
            resources.ApplyResources(this.btn_wphgt_mod, "btn_wphgt_mod");
            this.btn_wphgt_mod.Name = "btn_wphgt_mod";
            this.btn_wphgt_mod.UseVisualStyleBackColor = true;
            this.btn_wphgt_mod.Click += new System.EventHandler(this.btn_wphgt_mod_Click);
            // 
            // lb_wp_revert
            // 
            resources.ApplyResources(this.lb_wp_revert, "lb_wp_revert");
            this.lb_wp_revert.Name = "lb_wp_revert";
            this.lb_wp_revert.UseVisualStyleBackColor = true;
            this.lb_wp_revert.Click += new System.EventHandler(this.reverseWPsToolStripMenuItem_Click);
            // 
            // bt_edit_wps
            // 
            resources.ApplyResources(this.bt_edit_wps, "bt_edit_wps");
            this.bt_edit_wps.Name = "bt_edit_wps";
            this.toolTip1.SetToolTip(this.bt_edit_wps, resources.GetString("bt_edit_wps.ToolTip"));
            this.bt_edit_wps.UseVisualStyleBackColor = true;
            this.bt_edit_wps.Click += new System.EventHandler(this.BUT_Edit_Click);
            // 
            // BUT_generate
            // 
            resources.ApplyResources(this.BUT_generate, "BUT_generate");
            this.BUT_generate.Name = "BUT_generate";
            this.BUT_generate.UseVisualStyleBackColor = true;
            this.BUT_generate.Click += new System.EventHandler(this.BUT_generate_Click);
            // 
            // lb_clear_polygon
            // 
            resources.ApplyResources(this.lb_clear_polygon, "lb_clear_polygon");
            this.lb_clear_polygon.Name = "lb_clear_polygon";
            this.lb_clear_polygon.UseVisualStyleBackColor = true;
            this.lb_clear_polygon.Click += new System.EventHandler(this.clearPolygon_Click);
            // 
            // lb_autotakeoff_hgt
            // 
            resources.ApplyResources(this.lb_autotakeoff_hgt, "lb_autotakeoff_hgt");
            this.lb_autotakeoff_hgt.Name = "lb_autotakeoff_hgt";
            // 
            // lb_rtl_hgt
            // 
            resources.ApplyResources(this.lb_rtl_hgt, "lb_rtl_hgt");
            this.lb_rtl_hgt.Name = "lb_rtl_hgt";
            // 
            // CMB_altmode
            // 
            this.CMB_altmode.FormattingEnabled = true;
            resources.ApplyResources(this.CMB_altmode, "CMB_altmode");
            this.CMB_altmode.Name = "CMB_altmode";
            this.CMB_altmode.SelectedIndexChanged += new System.EventHandler(this.CMB_altmode_SelectedIndexChanged);
            // 
            // CHK_splinedefault
            // 
            resources.ApplyResources(this.CHK_splinedefault, "CHK_splinedefault");
            this.CHK_splinedefault.Name = "CHK_splinedefault";
            this.CHK_splinedefault.UseVisualStyleBackColor = true;
            this.CHK_splinedefault.CheckedChanged += new System.EventHandler(this.CHK_splinedefault_CheckedChanged);
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.label17.Name = "label17";
            // 
            // TXT_altwarn
            // 
            resources.ApplyResources(this.TXT_altwarn, "TXT_altwarn");
            this.TXT_altwarn.Name = "TXT_altwarn";
            // 
            // BUT_Add
            // 
            resources.ApplyResources(this.BUT_Add, "BUT_Add");
            this.BUT_Add.Name = "BUT_Add";
            this.toolTip1.SetToolTip(this.BUT_Add, resources.GetString("BUT_Add.ToolTip"));
            this.BUT_Add.UseVisualStyleBackColor = true;
            this.BUT_Add.Click += new System.EventHandler(this.BUT_Add_Click);
            // 
            // panelAction
            // 
            this.panelAction.AssociatedSplitter = this.splitter2;
            resources.ApplyResources(this.panelAction, "panelAction");
            this.panelAction.BackColor = System.Drawing.Color.Transparent;
            this.panelAction.CaptionFont = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelAction.CaptionHeight = 21;
            this.panelAction.ColorScheme = BSE.Windows.Forms.ColorScheme.Custom;
            this.panelAction.Controls.Add(this.flowLayoutPanel1);
            this.panelAction.CustomColors.BorderColor = System.Drawing.Color.Black;
            this.panelAction.CustomColors.CaptionCloseIcon = System.Drawing.Color.White;
            this.panelAction.CustomColors.CaptionExpandIcon = System.Drawing.Color.White;
            this.panelAction.CustomColors.CaptionGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(193)))), ((int)(((byte)(31)))));
            this.panelAction.CustomColors.CaptionGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(226)))), ((int)(((byte)(150)))));
            this.panelAction.CustomColors.CaptionGradientMiddle = System.Drawing.Color.Transparent;
            this.panelAction.CustomColors.CaptionSelectedGradientBegin = System.Drawing.Color.Transparent;
            this.panelAction.CustomColors.CaptionSelectedGradientEnd = System.Drawing.Color.Transparent;
            this.panelAction.CustomColors.CaptionText = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.panelAction.CustomColors.CollapsedCaptionText = System.Drawing.Color.White;
            this.panelAction.CustomColors.ContentGradientBegin = System.Drawing.SystemColors.ButtonFace;
            this.panelAction.CustomColors.ContentGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.panelAction.CustomColors.InnerBorderColor = System.Drawing.SystemColors.Window;
            this.panelAction.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panelAction.Image = null;
            this.panelAction.Name = "panelAction";
            this.panelAction.ShowExpandIcon = true;
            this.panelAction.ToolTipTextCloseIcon = null;
            this.panelAction.ToolTipTextExpandIconPanelCollapsed = null;
            this.panelAction.ToolTipTextExpandIconPanelExpanded = null;
            // 
            // splitter2
            // 
            this.splitter2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.splitter2, "splitter2");
            this.splitter2.Name = "splitter2";
            this.splitter2.TabStop = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.panel3);
            this.flowLayoutPanel1.Controls.Add(this.panel1);
            this.flowLayoutPanel1.Controls.Add(this.panel7);
            this.flowLayoutPanel1.Controls.Add(this.panel5);
            this.flowLayoutPanel1.Controls.Add(this.panel2);
            this.flowLayoutPanel1.Controls.Add(this.panel10);
            this.flowLayoutPanel1.Controls.Add(this.panel9);
            this.flowLayoutPanel1.Controls.Add(this.panel4);
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.map_corr_auto);
            this.panel3.Controls.Add(this.chk_grid);
            this.panel3.Controls.Add(this.lbl_status);
            this.panel3.Controls.Add(this.lnk_kml);
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // map_corr_auto
            // 
            resources.ApplyResources(this.map_corr_auto, "map_corr_auto");
            this.map_corr_auto.Name = "map_corr_auto";
            this.map_corr_auto.UseVisualStyleBackColor = true;
            this.map_corr_auto.CheckedChanged += new System.EventHandler(this.map_corr_auto_CheckedChanged);
            // 
            // chk_grid
            // 
            resources.ApplyResources(this.chk_grid, "chk_grid");
            this.chk_grid.Name = "chk_grid";
            this.chk_grid.UseVisualStyleBackColor = true;
            this.chk_grid.CheckedChanged += new System.EventHandler(this.chk_grid_CheckedChanged);
            // 
            // lnk_kml
            // 
            resources.ApplyResources(this.lnk_kml, "lnk_kml");
            this.lnk_kml.Name = "lnk_kml";
            this.lnk_kml.TabStop = true;
            this.lnk_kml.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnk_kml_LinkClicked);
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel7.Controls.Add(this.BUT_loadwpfile);
            this.panel7.Controls.Add(this.BUT_saveWPFile);
            resources.ApplyResources(this.panel7, "panel7");
            this.panel7.Name = "panel7";
            // 
            // BUT_loadwpfile
            // 
            resources.ApplyResources(this.BUT_loadwpfile, "BUT_loadwpfile");
            this.BUT_loadwpfile.Name = "BUT_loadwpfile";
            this.BUT_loadwpfile.UseVisualStyleBackColor = true;
            this.BUT_loadwpfile.Click += new System.EventHandler(this.BUT_loadwpfile_Click);
            // 
            // BUT_saveWPFile
            // 
            resources.ApplyResources(this.BUT_saveWPFile, "BUT_saveWPFile");
            this.BUT_saveWPFile.Name = "BUT_saveWPFile";
            this.BUT_saveWPFile.UseVisualStyleBackColor = true;
            this.BUT_saveWPFile.Click += new System.EventHandler(this.BUT_saveWPFile_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.myButton2);
            this.panel2.Controls.Add(this.myButton3);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // myButton2
            // 
            resources.ApplyResources(this.myButton2, "myButton2");
            this.myButton2.Name = "myButton2";
            this.myButton2.UseVisualStyleBackColor = true;
            this.myButton2.Click += new System.EventHandler(this.lb_load_poly_Click);
            // 
            // myButton3
            // 
            resources.ApplyResources(this.myButton3, "myButton3");
            this.myButton3.Name = "myButton3";
            this.myButton3.UseVisualStyleBackColor = true;
            this.myButton3.Click += new System.EventHandler(this.lb_save_poly_Click);
            // 
            // panel10
            // 
            this.panel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.panel10.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel10.Controls.Add(this.rb_bj);
            this.panel10.Controls.Add(this.rb_wp);
            this.panel10.Controls.Add(this.lb_load_kml);
            resources.ApplyResources(this.panel10, "panel10");
            this.panel10.Name = "panel10";
            // 
            // rb_bj
            // 
            resources.ApplyResources(this.rb_bj, "rb_bj");
            this.rb_bj.Checked = true;
            this.rb_bj.Name = "rb_bj";
            this.rb_bj.TabStop = true;
            this.rb_bj.UseVisualStyleBackColor = true;
            // 
            // rb_wp
            // 
            resources.ApplyResources(this.rb_wp, "rb_wp");
            this.rb_wp.Name = "rb_wp";
            this.rb_wp.UseVisualStyleBackColor = true;
            // 
            // lb_load_kml
            // 
            resources.ApplyResources(this.lb_load_kml, "lb_load_kml");
            this.lb_load_kml.Name = "lb_load_kml";
            this.lb_load_kml.UseVisualStyleBackColor = true;
            this.lb_load_kml.Click += new System.EventHandler(this.loadKMLFileToolStripMenuItem_Click);
            // 
            // panel9
            // 
            this.panel9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.panel9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel9.Controls.Add(this.lb_download_map);
            this.panel9.Controls.Add(this.myButton1);
            resources.ApplyResources(this.panel9, "panel9");
            this.panel9.Name = "panel9";
            // 
            // lb_download_map
            // 
            resources.ApplyResources(this.lb_download_map, "lb_download_map");
            this.lb_download_map.Name = "lb_download_map";
            this.lb_download_map.UseVisualStyleBackColor = true;
            this.lb_download_map.Click += new System.EventHandler(this.lb_download_map_Click);
            // 
            // myButton1
            // 
            resources.ApplyResources(this.myButton1, "myButton1");
            this.myButton1.Name = "myButton1";
            this.myButton1.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel4.Controls.Add(this.load_last_offs);
            this.panel4.Controls.Add(this.map_corr_manual);
            resources.ApplyResources(this.panel4, "panel4");
            this.panel4.Name = "panel4";
            // 
            // load_last_offs
            // 
            resources.ApplyResources(this.load_last_offs, "load_last_offs");
            this.load_last_offs.Name = "load_last_offs";
            this.load_last_offs.UseVisualStyleBackColor = true;
            // 
            // map_corr_manual
            // 
            resources.ApplyResources(this.map_corr_manual, "map_corr_manual");
            this.map_corr_manual.Name = "map_corr_manual";
            this.map_corr_manual.UseVisualStyleBackColor = true;
            this.map_corr_manual.Click += new System.EventHandler(this.map_corr_manual_Click);
            // 
            // panelMap
            // 
            this.panelMap.Controls.Add(this.panel_avoidPnts);
            this.panelMap.Controls.Add(this.pannel_wpcommands);
            this.panelMap.Controls.Add(this.rb_avoid_pnts);
            this.panelMap.Controls.Add(this.rb_en_lidar);
            this.panelMap.Controls.Add(this.rb_clear_polygon);
            this.panelMap.Controls.Add(this.rb_clear_mission);
            this.panelMap.Controls.Add(this.rb_wp_hgt);
            this.panelMap.Controls.Add(this.rb_editwpcommand);
            this.panelMap.Controls.Add(this.rb_wpmigrate);
            this.panelMap.Controls.Add(this.rb_corr);
            this.panelMap.Controls.Add(this.rb_inv_wps);
            this.panelMap.Controls.Add(this.rb_autoWP);
            this.panelMap.Controls.Add(this.rb_center);
            this.panelMap.Controls.Add(this.groupBox1);
            this.panelMap.Controls.Add(this.gb_modWPhgt);
            this.panelMap.Controls.Add(this.MainMap);
            this.panelMap.Controls.Add(this.trackBar1);
            this.panelMap.Controls.Add(this.label11);
            resources.ApplyResources(this.panelMap, "panelMap");
            this.panelMap.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panelMap.Name = "panelMap";
            this.panelMap.Resize += new System.EventHandler(this.panelMap_Resize);
            // 
            // panel_avoidPnts
            // 
            this.panel_avoidPnts.AllowDrop = true;
            this.panel_avoidPnts.AssociatedSplitter = null;
            resources.ApplyResources(this.panel_avoidPnts, "panel_avoidPnts");
            this.panel_avoidPnts.BackColor = System.Drawing.Color.Transparent;
            this.panel_avoidPnts.CaptionFont = new System.Drawing.Font("Microsoft YaHei UI", 11.75F, System.Drawing.FontStyle.Bold);
            this.panel_avoidPnts.CaptionHeight = 27;
            this.panel_avoidPnts.Controls.Add(this.rb_avoidPntsOk);
            this.panel_avoidPnts.Controls.Add(this.rb_avoidPntsAdd);
            this.panel_avoidPnts.Controls.Add(this.dataGridView_avoidPnts);
            this.panel_avoidPnts.CustomColors.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(184)))), ((int)(((byte)(184)))));
            this.panel_avoidPnts.CustomColors.CaptionCloseIcon = System.Drawing.SystemColors.ControlText;
            this.panel_avoidPnts.CustomColors.CaptionExpandIcon = System.Drawing.SystemColors.ControlText;
            this.panel_avoidPnts.CustomColors.CaptionGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.panel_avoidPnts.CustomColors.CaptionGradientEnd = System.Drawing.SystemColors.ButtonFace;
            this.panel_avoidPnts.CustomColors.CaptionGradientMiddle = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.panel_avoidPnts.CustomColors.CaptionSelectedGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.panel_avoidPnts.CustomColors.CaptionSelectedGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.panel_avoidPnts.CustomColors.CaptionText = System.Drawing.SystemColors.ControlText;
            this.panel_avoidPnts.CustomColors.CollapsedCaptionText = System.Drawing.SystemColors.ControlText;
            this.panel_avoidPnts.CustomColors.ContentGradientBegin = System.Drawing.SystemColors.ButtonFace;
            this.panel_avoidPnts.CustomColors.ContentGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.panel_avoidPnts.CustomColors.InnerBorderColor = System.Drawing.SystemColors.Window;
            this.panel_avoidPnts.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panel_avoidPnts.Image = null;
            this.panel_avoidPnts.Name = "panel_avoidPnts";
            this.panel_avoidPnts.PanelStyle = BSE.Windows.Forms.PanelStyle.Office2007;
            this.panel_avoidPnts.ToolTipTextCloseIcon = null;
            this.panel_avoidPnts.ToolTipTextExpandIconPanelCollapsed = null;
            this.panel_avoidPnts.ToolTipTextExpandIconPanelExpanded = null;
            // 
            // rb_avoidPntsOk
            // 
            this.rb_avoidPntsOk.BackColor = System.Drawing.Color.Lime;
            resources.ApplyResources(this.rb_avoidPntsOk, "rb_avoidPntsOk");
            this.rb_avoidPntsOk.Name = "rb_avoidPntsOk";
            this.rb_avoidPntsOk.RecessDepth = 0;
            this.rb_avoidPntsOk.UseVisualStyleBackColor = false;
            this.rb_avoidPntsOk.Click += new System.EventHandler(this.rb_avoidPntsOk_Click);
            // 
            // rb_avoidPntsAdd
            // 
            this.rb_avoidPntsAdd.BackColor = System.Drawing.Color.Gold;
            resources.ApplyResources(this.rb_avoidPntsAdd, "rb_avoidPntsAdd");
            this.rb_avoidPntsAdd.Name = "rb_avoidPntsAdd";
            this.rb_avoidPntsAdd.RecessDepth = 0;
            this.rb_avoidPntsAdd.UseVisualStyleBackColor = false;
            this.rb_avoidPntsAdd.Click += new System.EventHandler(this.rb_avoidPntsAdd_Click);
            // 
            // dataGridView_avoidPnts
            // 
            this.dataGridView_avoidPnts.AllowUserToAddRows = false;
            this.dataGridView_avoidPnts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCellsExceptHeader;
            this.dataGridView_avoidPnts.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_avoidPnts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
            resources.ApplyResources(this.dataGridView_avoidPnts, "dataGridView_avoidPnts");
            this.dataGridView_avoidPnts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.avoidPntLat,
            this.avoidPntLng,
            this.avoidPntRadius,
            this.avoidPntsInfo,
            this.Manage});
            this.dataGridView_avoidPnts.Name = "dataGridView_avoidPnts";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.Format = "N0";
            dataGridViewCellStyle10.NullValue = "0";
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dataGridView_avoidPnts.RowHeadersDefaultCellStyle = dataGridViewCellStyle10;
            dataGridViewCellStyle11.ForeColor = System.Drawing.Color.Black;
            this.dataGridView_avoidPnts.RowsDefaultCellStyle = dataGridViewCellStyle11;
            this.dataGridView_avoidPnts.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.AvoidCommands_CellContentClick);
            // 
            // pannel_wpcommands
            // 
            this.pannel_wpcommands.AllowDrop = true;
            this.pannel_wpcommands.AssociatedSplitter = null;
            resources.ApplyResources(this.pannel_wpcommands, "pannel_wpcommands");
            this.pannel_wpcommands.BackColor = System.Drawing.Color.Transparent;
            this.pannel_wpcommands.CaptionFont = new System.Drawing.Font("Microsoft YaHei UI", 11.75F, System.Drawing.FontStyle.Bold);
            this.pannel_wpcommands.CaptionHeight = 27;
            this.pannel_wpcommands.Controls.Add(this.Commands);
            this.pannel_wpcommands.CustomColors.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(184)))), ((int)(((byte)(184)))));
            this.pannel_wpcommands.CustomColors.CaptionCloseIcon = System.Drawing.SystemColors.ControlText;
            this.pannel_wpcommands.CustomColors.CaptionExpandIcon = System.Drawing.SystemColors.ControlText;
            this.pannel_wpcommands.CustomColors.CaptionGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.pannel_wpcommands.CustomColors.CaptionGradientEnd = System.Drawing.SystemColors.ButtonFace;
            this.pannel_wpcommands.CustomColors.CaptionGradientMiddle = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.pannel_wpcommands.CustomColors.CaptionSelectedGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.pannel_wpcommands.CustomColors.CaptionSelectedGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.pannel_wpcommands.CustomColors.CaptionText = System.Drawing.SystemColors.ControlText;
            this.pannel_wpcommands.CustomColors.CollapsedCaptionText = System.Drawing.SystemColors.ControlText;
            this.pannel_wpcommands.CustomColors.ContentGradientBegin = System.Drawing.SystemColors.ButtonFace;
            this.pannel_wpcommands.CustomColors.ContentGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.pannel_wpcommands.CustomColors.InnerBorderColor = System.Drawing.SystemColors.Window;
            this.pannel_wpcommands.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pannel_wpcommands.Image = null;
            this.pannel_wpcommands.Name = "pannel_wpcommands";
            this.pannel_wpcommands.PanelStyle = BSE.Windows.Forms.PanelStyle.Office2007;
            this.pannel_wpcommands.ToolTipTextCloseIcon = null;
            this.pannel_wpcommands.ToolTipTextExpandIconPanelCollapsed = null;
            this.pannel_wpcommands.ToolTipTextExpandIconPanelExpanded = null;
            // 
            // rb_avoid_pnts
            // 
            this.rb_avoid_pnts.BackColor = System.Drawing.Color.Lime;
            resources.ApplyResources(this.rb_avoid_pnts, "rb_avoid_pnts");
            this.rb_avoid_pnts.Name = "rb_avoid_pnts";
            this.rb_avoid_pnts.RecessDepth = 0;
            this.toolTip1.SetToolTip(this.rb_avoid_pnts, resources.GetString("rb_avoid_pnts.ToolTip"));
            this.rb_avoid_pnts.UseVisualStyleBackColor = false;
            this.rb_avoid_pnts.Click += new System.EventHandler(this.rb_avoid_pnts_Click);
            // 
            // rb_en_lidar
            // 
            this.rb_en_lidar.BackColor = System.Drawing.Color.IndianRed;
            resources.ApplyResources(this.rb_en_lidar, "rb_en_lidar");
            this.rb_en_lidar.Name = "rb_en_lidar";
            this.rb_en_lidar.RecessDepth = 0;
            this.toolTip1.SetToolTip(this.rb_en_lidar, resources.GetString("rb_en_lidar.ToolTip"));
            this.rb_en_lidar.UseVisualStyleBackColor = false;
            this.rb_en_lidar.Click += new System.EventHandler(this.rb_en_lidar_Click);
            // 
            // rb_clear_polygon
            // 
            this.rb_clear_polygon.BackColor = System.Drawing.Color.DarkOrange;
            resources.ApplyResources(this.rb_clear_polygon, "rb_clear_polygon");
            this.rb_clear_polygon.Name = "rb_clear_polygon";
            this.rb_clear_polygon.RecessDepth = 0;
            this.toolTip1.SetToolTip(this.rb_clear_polygon, resources.GetString("rb_clear_polygon.ToolTip"));
            this.rb_clear_polygon.UseVisualStyleBackColor = false;
            this.rb_clear_polygon.Click += new System.EventHandler(this.rb_clear_polygon_Click);
            // 
            // rb_clear_mission
            // 
            this.rb_clear_mission.BackColor = System.Drawing.Color.DarkOrange;
            resources.ApplyResources(this.rb_clear_mission, "rb_clear_mission");
            this.rb_clear_mission.Name = "rb_clear_mission";
            this.rb_clear_mission.RecessDepth = 0;
            this.toolTip1.SetToolTip(this.rb_clear_mission, resources.GetString("rb_clear_mission.ToolTip"));
            this.rb_clear_mission.UseVisualStyleBackColor = false;
            this.rb_clear_mission.Click += new System.EventHandler(this.rb_clear_mission_Click);
            // 
            // rb_wp_hgt
            // 
            this.rb_wp_hgt.BackColor = System.Drawing.Color.DarkOrange;
            resources.ApplyResources(this.rb_wp_hgt, "rb_wp_hgt");
            this.rb_wp_hgt.Name = "rb_wp_hgt";
            this.rb_wp_hgt.RecessDepth = 0;
            this.toolTip1.SetToolTip(this.rb_wp_hgt, resources.GetString("rb_wp_hgt.ToolTip"));
            this.rb_wp_hgt.UseVisualStyleBackColor = false;
            this.rb_wp_hgt.Click += new System.EventHandler(this.btn_wphgt_mod_Click);
            // 
            // rb_editwpcommand
            // 
            this.rb_editwpcommand.BackColor = System.Drawing.Color.DarkOrange;
            resources.ApplyResources(this.rb_editwpcommand, "rb_editwpcommand");
            this.rb_editwpcommand.Name = "rb_editwpcommand";
            this.rb_editwpcommand.RecessDepth = 0;
            this.toolTip1.SetToolTip(this.rb_editwpcommand, resources.GetString("rb_editwpcommand.ToolTip"));
            this.rb_editwpcommand.UseVisualStyleBackColor = false;
            this.rb_editwpcommand.Click += new System.EventHandler(this.BUT_Edit_Click);
            // 
            // rb_wpmigrate
            // 
            this.rb_wpmigrate.BackColor = System.Drawing.Color.DarkOrange;
            resources.ApplyResources(this.rb_wpmigrate, "rb_wpmigrate");
            this.rb_wpmigrate.Name = "rb_wpmigrate";
            this.rb_wpmigrate.RecessDepth = 0;
            this.toolTip1.SetToolTip(this.rb_wpmigrate, resources.GetString("rb_wpmigrate.ToolTip"));
            this.rb_wpmigrate.UseVisualStyleBackColor = false;
            this.rb_wpmigrate.Click += new System.EventHandler(this.wp_migrate_Click);
            // 
            // rb_corr
            // 
            this.rb_corr.BackColor = System.Drawing.Color.DarkOrange;
            resources.ApplyResources(this.rb_corr, "rb_corr");
            this.rb_corr.Name = "rb_corr";
            this.rb_corr.RecessDepth = 0;
            this.toolTip1.SetToolTip(this.rb_corr, resources.GetString("rb_corr.ToolTip"));
            this.rb_corr.UseVisualStyleBackColor = false;
            this.rb_corr.Click += new System.EventHandler(this.map_corr_manual_Click);
            // 
            // rb_inv_wps
            // 
            this.rb_inv_wps.BackColor = System.Drawing.Color.DarkOrange;
            resources.ApplyResources(this.rb_inv_wps, "rb_inv_wps");
            this.rb_inv_wps.Name = "rb_inv_wps";
            this.rb_inv_wps.RecessDepth = 0;
            this.toolTip1.SetToolTip(this.rb_inv_wps, resources.GetString("rb_inv_wps.ToolTip"));
            this.rb_inv_wps.UseVisualStyleBackColor = false;
            this.rb_inv_wps.Click += new System.EventHandler(this.rb_inv_wps_Click);
            // 
            // rb_autoWP
            // 
            this.rb_autoWP.BackColor = System.Drawing.Color.DarkOrange;
            resources.ApplyResources(this.rb_autoWP, "rb_autoWP");
            this.rb_autoWP.Name = "rb_autoWP";
            this.rb_autoWP.RecessDepth = 0;
            this.toolTip1.SetToolTip(this.rb_autoWP, resources.GetString("rb_autoWP.ToolTip"));
            this.rb_autoWP.UseVisualStyleBackColor = false;
            this.rb_autoWP.Click += new System.EventHandler(this.rb_autoWP_Click);
            // 
            // rb_center
            // 
            this.rb_center.BackColor = System.Drawing.Color.Lime;
            resources.ApplyResources(this.rb_center, "rb_center");
            this.rb_center.Name = "rb_center";
            this.rb_center.RecessDepth = 0;
            this.toolTip1.SetToolTip(this.rb_center, resources.GetString("rb_center.ToolTip"));
            this.rb_center.UseVisualStyleBackColor = false;
            this.rb_center.Click += new System.EventHandler(this.rb_center_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.groupBox1.Controls.Add(this.coords1);
            this.groupBox1.Controls.Add(this.lbl_prevdist);
            this.groupBox1.Controls.Add(this.lbl_ftime);
            this.groupBox1.Controls.Add(this.lbl_homedist);
            this.groupBox1.Controls.Add(this.lbl_distance);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // lbl_prevdist
            // 
            resources.ApplyResources(this.lbl_prevdist, "lbl_prevdist");
            this.lbl_prevdist.Name = "lbl_prevdist";
            // 
            // lbl_ftime
            // 
            resources.ApplyResources(this.lbl_ftime, "lbl_ftime");
            this.lbl_ftime.Name = "lbl_ftime";
            // 
            // lbl_homedist
            // 
            resources.ApplyResources(this.lbl_homedist, "lbl_homedist");
            this.lbl_homedist.Name = "lbl_homedist";
            // 
            // lbl_distance
            // 
            resources.ApplyResources(this.lbl_distance, "lbl_distance");
            this.lbl_distance.Name = "lbl_distance";
            // 
            // gb_modWPhgt
            // 
            this.gb_modWPhgt.Controls.Add(this.btn_cancel);
            this.gb_modWPhgt.Controls.Add(this.btn_modconfirm);
            this.gb_modWPhgt.Controls.Add(this.panel11);
            this.gb_modWPhgt.Controls.Add(this.rbtn_sel);
            this.gb_modWPhgt.Controls.Add(this.rbtn_modall);
            this.gb_modWPhgt.Controls.Add(this.num_newhgt);
            this.gb_modWPhgt.Controls.Add(this.lb_newWPhgt);
            resources.ApplyResources(this.gb_modWPhgt, "gb_modWPhgt");
            this.gb_modWPhgt.Name = "gb_modWPhgt";
            this.gb_modWPhgt.TabStop = false;
            // 
            // btn_cancel
            // 
            resources.ApplyResources(this.btn_cancel, "btn_cancel");
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btn_modconfirm
            // 
            resources.ApplyResources(this.btn_modconfirm, "btn_modconfirm");
            this.btn_modconfirm.Name = "btn_modconfirm";
            this.btn_modconfirm.UseVisualStyleBackColor = true;
            this.btn_modconfirm.Click += new System.EventHandler(this.btn_modconfirm_Click);
            // 
            // panel11
            // 
            this.panel11.Controls.Add(this.num_endWP);
            this.panel11.Controls.Add(this.num_startWP);
            this.panel11.Controls.Add(this.lineSeparator1);
            resources.ApplyResources(this.panel11, "panel11");
            this.panel11.Name = "panel11";
            // 
            // num_endWP
            // 
            resources.ApplyResources(this.num_endWP, "num_endWP");
            this.num_endWP.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.num_endWP.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_endWP.Name = "num_endWP";
            this.num_endWP.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // num_startWP
            // 
            resources.ApplyResources(this.num_startWP, "num_startWP");
            this.num_startWP.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.num_startWP.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_startWP.Name = "num_startWP";
            this.num_startWP.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lineSeparator1
            // 
            this.lineSeparator1.BackColor = System.Drawing.Color.Black;
            this.lineSeparator1.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.lineSeparator1, "lineSeparator1");
            this.lineSeparator1.Name = "lineSeparator1";
            this.lineSeparator1.PrimaryColor = System.Drawing.Color.Black;
            this.lineSeparator1.SecondaryColor = System.Drawing.Color.Black;
            // 
            // rbtn_sel
            // 
            resources.ApplyResources(this.rbtn_sel, "rbtn_sel");
            this.rbtn_sel.Checked = true;
            this.rbtn_sel.Name = "rbtn_sel";
            this.rbtn_sel.TabStop = true;
            this.rbtn_sel.UseVisualStyleBackColor = true;
            this.rbtn_sel.CheckedChanged += new System.EventHandler(this.rbtn_sel_CheckedChanged);
            // 
            // rbtn_modall
            // 
            resources.ApplyResources(this.rbtn_modall, "rbtn_modall");
            this.rbtn_modall.Name = "rbtn_modall";
            this.rbtn_modall.UseVisualStyleBackColor = true;
            this.rbtn_modall.CheckedChanged += new System.EventHandler(this.rbtn_modall_CheckedChanged);
            // 
            // num_newhgt
            // 
            resources.ApplyResources(this.num_newhgt, "num_newhgt");
            this.num_newhgt.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.num_newhgt.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_newhgt.Name = "num_newhgt";
            this.num_newhgt.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // lb_newWPhgt
            // 
            resources.ApplyResources(this.lb_newWPhgt, "lb_newWPhgt");
            this.lb_newWPhgt.Name = "lb_newWPhgt";
            // 
            // MainMap
            // 
            this.MainMap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(39)))), ((int)(((byte)(40)))));
            this.MainMap.Bearing = 0F;
            this.MainMap.CanDragMap = true;
            this.MainMap.ContextMenuStrip = this.contextMenuStrip2;
            this.MainMap.Cursor = System.Windows.Forms.Cursors.Default;
            this.MainMap.EmptyTileColor = System.Drawing.Color.Gray;
            this.MainMap.GrayScaleMode = false;
            this.MainMap.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.MainMap.LevelsKeepInMemmory = 5;
            resources.ApplyResources(this.MainMap, "MainMap");
            this.MainMap.MarkersEnabled = true;
            this.MainMap.MaxZoom = 19;
            this.MainMap.MinZoom = 0;
            this.MainMap.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.MainMap.Name = "MainMap";
            this.MainMap.NegativeMode = false;
            this.MainMap.PolygonsEnabled = true;
            this.MainMap.RetryLoadTile = 0;
            this.MainMap.RoutesEnabled = false;
            this.MainMap.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Fractional;
            this.MainMap.SelectedArea = ((GMap.NET.RectLatLng)(resources.GetObject("MainMap.SelectedArea")));
            this.MainMap.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.MainMap.ShowTileGridLines = false;
            this.MainMap.Zoom = 0D;
            this.MainMap.Paint += new System.Windows.Forms.PaintEventHandler(this.MainMap_Paint);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ContextMeasure,
            this.insertWpToolStripMenuItem,
            this.genWPsToolStripMenuItem,
            this.deleteWPToolStripMenuItem,
            this.wpEditToolStripMenuItem,
            this.wpManageToolStripMenuItem,
            this.polyEditToolStripMenuItem,
            this.polySplitToolStripMenuItemAll,
            this.bjToolStripMenuItem,
            this.kmlToolStripMenuItem,
            this.mapToolStripMenuItem_simp,
            this.clearMissionToolStripMenuItem,
            this.setHomeHereToolStripMenuItem,
            this.rallyPointsToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            resources.ApplyResources(this.contextMenuStrip2, "contextMenuStrip2");
            // 
            // ContextMeasure
            // 
            this.ContextMeasure.Name = "ContextMeasure";
            resources.ApplyResources(this.ContextMeasure, "ContextMeasure");
            this.ContextMeasure.Click += new System.EventHandler(this.ContextMeasure_Click);
            // 
            // insertWpToolStripMenuItem
            // 
            this.insertWpToolStripMenuItem.Name = "insertWpToolStripMenuItem";
            resources.ApplyResources(this.insertWpToolStripMenuItem, "insertWpToolStripMenuItem");
            this.insertWpToolStripMenuItem.Click += new System.EventHandler(this.insertWpToolStripMenuItem_Click);
            // 
            // genWPsToolStripMenuItem
            // 
            this.genWPsToolStripMenuItem.Name = "genWPsToolStripMenuItem";
            resources.ApplyResources(this.genWPsToolStripMenuItem, "genWPsToolStripMenuItem");
            this.genWPsToolStripMenuItem.Click += new System.EventHandler(this.genWPsToolStripMenuItem_Click);
            // 
            // deleteWPToolStripMenuItem
            // 
            this.deleteWPToolStripMenuItem.Name = "deleteWPToolStripMenuItem";
            resources.ApplyResources(this.deleteWPToolStripMenuItem, "deleteWPToolStripMenuItem");
            this.deleteWPToolStripMenuItem.Click += new System.EventHandler(this.deleteWPToolStripMenuItem_Click);
            // 
            // wpEditToolStripMenuItem
            // 
            this.wpEditToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editWpToolStripMenuItem,
            this.doneWpToolStripMenuItem});
            this.wpEditToolStripMenuItem.Name = "wpEditToolStripMenuItem";
            resources.ApplyResources(this.wpEditToolStripMenuItem, "wpEditToolStripMenuItem");
            // 
            // editWpToolStripMenuItem
            // 
            this.editWpToolStripMenuItem.Name = "editWpToolStripMenuItem";
            resources.ApplyResources(this.editWpToolStripMenuItem, "editWpToolStripMenuItem");
            this.editWpToolStripMenuItem.Click += new System.EventHandler(this.editWpToolStripMenuItem_Click);
            // 
            // doneWpToolStripMenuItem
            // 
            this.doneWpToolStripMenuItem.Name = "doneWpToolStripMenuItem";
            resources.ApplyResources(this.doneWpToolStripMenuItem, "doneWpToolStripMenuItem");
            this.doneWpToolStripMenuItem.Click += new System.EventHandler(this.doneWpToolStripMenuItem_Click);
            // 
            // wpManageToolStripMenuItem
            // 
            this.wpManageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.wpUploadToolStripMenuItem,
            this.wpDownloadToolStripMenuItem,
            this.wpSaveToolStripMenuItem,
            this.wpOpenToolStripMenuItem});
            this.wpManageToolStripMenuItem.Name = "wpManageToolStripMenuItem";
            resources.ApplyResources(this.wpManageToolStripMenuItem, "wpManageToolStripMenuItem");
            // 
            // wpUploadToolStripMenuItem
            // 
            this.wpUploadToolStripMenuItem.Name = "wpUploadToolStripMenuItem";
            resources.ApplyResources(this.wpUploadToolStripMenuItem, "wpUploadToolStripMenuItem");
            this.wpUploadToolStripMenuItem.Click += new System.EventHandler(this.BUT_write_Click);
            // 
            // wpDownloadToolStripMenuItem
            // 
            this.wpDownloadToolStripMenuItem.Name = "wpDownloadToolStripMenuItem";
            resources.ApplyResources(this.wpDownloadToolStripMenuItem, "wpDownloadToolStripMenuItem");
            this.wpDownloadToolStripMenuItem.Click += new System.EventHandler(this.BUT_read_Click);
            // 
            // wpSaveToolStripMenuItem
            // 
            this.wpSaveToolStripMenuItem.Name = "wpSaveToolStripMenuItem";
            resources.ApplyResources(this.wpSaveToolStripMenuItem, "wpSaveToolStripMenuItem");
            this.wpSaveToolStripMenuItem.Click += new System.EventHandler(this.BUT_saveWPFile_Click);
            // 
            // wpOpenToolStripMenuItem
            // 
            this.wpOpenToolStripMenuItem.Name = "wpOpenToolStripMenuItem";
            resources.ApplyResources(this.wpOpenToolStripMenuItem, "wpOpenToolStripMenuItem");
            this.wpOpenToolStripMenuItem.Click += new System.EventHandler(this.BUT_loadwpfile_Click);
            // 
            // polyEditToolStripMenuItem
            // 
            this.polyEditToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addPolygonToolStripMenuItem,
            this.donePolygonToolStripMenuItem});
            this.polyEditToolStripMenuItem.Name = "polyEditToolStripMenuItem";
            resources.ApplyResources(this.polyEditToolStripMenuItem, "polyEditToolStripMenuItem");
            // 
            // addPolygonToolStripMenuItem
            // 
            this.addPolygonToolStripMenuItem.Name = "addPolygonToolStripMenuItem";
            resources.ApplyResources(this.addPolygonToolStripMenuItem, "addPolygonToolStripMenuItem");
            this.addPolygonToolStripMenuItem.Click += new System.EventHandler(this.addPolygonToolStripMenuItem_Click);
            // 
            // donePolygonToolStripMenuItem
            // 
            this.donePolygonToolStripMenuItem.Name = "donePolygonToolStripMenuItem";
            resources.ApplyResources(this.donePolygonToolStripMenuItem, "donePolygonToolStripMenuItem");
            this.donePolygonToolStripMenuItem.Click += new System.EventHandler(this.donePolygonToolStripMenuItem_Click);
            // 
            // polySplitToolStripMenuItemAll
            // 
            this.polySplitToolStripMenuItemAll.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.polySplitToolStripMenuItem,
            this.DonePolySplitToolStripMenuItem,
            this.DeletePolySplitToolStripMenuItem});
            this.polySplitToolStripMenuItemAll.Name = "polySplitToolStripMenuItemAll";
            resources.ApplyResources(this.polySplitToolStripMenuItemAll, "polySplitToolStripMenuItemAll");
            // 
            // polySplitToolStripMenuItem
            // 
            this.polySplitToolStripMenuItem.Name = "polySplitToolStripMenuItem";
            resources.ApplyResources(this.polySplitToolStripMenuItem, "polySplitToolStripMenuItem");
            this.polySplitToolStripMenuItem.Click += new System.EventHandler(this.polySplitToolStripMenuItem_Click);
            // 
            // DonePolySplitToolStripMenuItem
            // 
            this.DonePolySplitToolStripMenuItem.Name = "DonePolySplitToolStripMenuItem";
            resources.ApplyResources(this.DonePolySplitToolStripMenuItem, "DonePolySplitToolStripMenuItem");
            this.DonePolySplitToolStripMenuItem.Click += new System.EventHandler(this.DonePolySplitToolStripMenuItem_Click);
            // 
            // DeletePolySplitToolStripMenuItem
            // 
            this.DeletePolySplitToolStripMenuItem.Name = "DeletePolySplitToolStripMenuItem";
            resources.ApplyResources(this.DeletePolySplitToolStripMenuItem, "DeletePolySplitToolStripMenuItem");
            this.DeletePolySplitToolStripMenuItem.Click += new System.EventHandler(this.DeletePolySplitToolStripMenuItem_Click);
            // 
            // bjToolStripMenuItem
            // 
            this.bjToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadPolygonToolStripMenuItem,
            this.savePolygonToolStripMenuItem});
            this.bjToolStripMenuItem.Name = "bjToolStripMenuItem";
            resources.ApplyResources(this.bjToolStripMenuItem, "bjToolStripMenuItem");
            // 
            // loadPolygonToolStripMenuItem
            // 
            this.loadPolygonToolStripMenuItem.Name = "loadPolygonToolStripMenuItem";
            resources.ApplyResources(this.loadPolygonToolStripMenuItem, "loadPolygonToolStripMenuItem");
            this.loadPolygonToolStripMenuItem.Click += new System.EventHandler(this.loadPolygonToolStripMenuItem_Click);
            // 
            // savePolygonToolStripMenuItem
            // 
            this.savePolygonToolStripMenuItem.Name = "savePolygonToolStripMenuItem";
            resources.ApplyResources(this.savePolygonToolStripMenuItem, "savePolygonToolStripMenuItem");
            this.savePolygonToolStripMenuItem.Click += new System.EventHandler(this.savePolygonToolStripMenuItem_Click);
            // 
            // kmlToolStripMenuItem
            // 
            this.kmlToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadKMLbjStripMenuItem,
            this.loadKMLWPStripMenuItem});
            this.kmlToolStripMenuItem.Name = "kmlToolStripMenuItem";
            resources.ApplyResources(this.kmlToolStripMenuItem, "kmlToolStripMenuItem");
            // 
            // loadKMLbjStripMenuItem
            // 
            this.loadKMLbjStripMenuItem.Name = "loadKMLbjStripMenuItem";
            resources.ApplyResources(this.loadKMLbjStripMenuItem, "loadKMLbjStripMenuItem");
            this.loadKMLbjStripMenuItem.Click += new System.EventHandler(this.loadKMLbjStripMenuItem_Click);
            // 
            // loadKMLWPStripMenuItem
            // 
            this.loadKMLWPStripMenuItem.Name = "loadKMLWPStripMenuItem";
            resources.ApplyResources(this.loadKMLWPStripMenuItem, "loadKMLWPStripMenuItem");
            this.loadKMLWPStripMenuItem.Click += new System.EventHandler(this.loadKMLWPStripMenuItem_Click);
            // 
            // mapToolStripMenuItem_simp
            // 
            this.mapToolStripMenuItem_simp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoomToToolStripMenuItem,
            this.prefetchToolStripMenuItem});
            this.mapToolStripMenuItem_simp.Name = "mapToolStripMenuItem_simp";
            resources.ApplyResources(this.mapToolStripMenuItem_simp, "mapToolStripMenuItem_simp");
            // 
            // zoomToToolStripMenuItem
            // 
            this.zoomToToolStripMenuItem.Name = "zoomToToolStripMenuItem";
            resources.ApplyResources(this.zoomToToolStripMenuItem, "zoomToToolStripMenuItem");
            this.zoomToToolStripMenuItem.Click += new System.EventHandler(this.zoomToToolStripMenuItem_Click);
            // 
            // prefetchToolStripMenuItem
            // 
            this.prefetchToolStripMenuItem.Name = "prefetchToolStripMenuItem";
            resources.ApplyResources(this.prefetchToolStripMenuItem, "prefetchToolStripMenuItem");
            this.prefetchToolStripMenuItem.Click += new System.EventHandler(this.prefetchToolStripMenuItem_Click);
            // 
            // clearMissionToolStripMenuItem
            // 
            this.clearMissionToolStripMenuItem.Name = "clearMissionToolStripMenuItem";
            resources.ApplyResources(this.clearMissionToolStripMenuItem, "clearMissionToolStripMenuItem");
            this.clearMissionToolStripMenuItem.Click += new System.EventHandler(this.clearMissionToolStripMenuItem_Click);
            // 
            // setHomeHereToolStripMenuItem
            // 
            this.setHomeHereToolStripMenuItem.Name = "setHomeHereToolStripMenuItem";
            resources.ApplyResources(this.setHomeHereToolStripMenuItem, "setHomeHereToolStripMenuItem");
            this.setHomeHereToolStripMenuItem.Click += new System.EventHandler(this.setHomeHereToolStripMenuItem_Click);
            // 
            // rallyPointsToolStripMenuItem
            // 
            this.rallyPointsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setRallyPointToolStripMenuItem,
            this.clearRallyPointsToolStripMenuItem,
            this.getRallyPointsToolStripMenuItem,
            this.saveRallyPointsToolStripMenuItem,
            this.saveToFileToolStripMenuItem1,
            this.loadFromFileToolStripMenuItem1});
            this.rallyPointsToolStripMenuItem.Name = "rallyPointsToolStripMenuItem";
            resources.ApplyResources(this.rallyPointsToolStripMenuItem, "rallyPointsToolStripMenuItem");
            // 
            // setRallyPointToolStripMenuItem
            // 
            this.setRallyPointToolStripMenuItem.Name = "setRallyPointToolStripMenuItem";
            resources.ApplyResources(this.setRallyPointToolStripMenuItem, "setRallyPointToolStripMenuItem");
            this.setRallyPointToolStripMenuItem.Click += new System.EventHandler(this.setRallyPointToolStripMenuItem_Click);
            // 
            // clearRallyPointsToolStripMenuItem
            // 
            this.clearRallyPointsToolStripMenuItem.Name = "clearRallyPointsToolStripMenuItem";
            resources.ApplyResources(this.clearRallyPointsToolStripMenuItem, "clearRallyPointsToolStripMenuItem");
            this.clearRallyPointsToolStripMenuItem.Click += new System.EventHandler(this.clearRallyPointsToolStripMenuItem_Click);
            // 
            // getRallyPointsToolStripMenuItem
            // 
            this.getRallyPointsToolStripMenuItem.Name = "getRallyPointsToolStripMenuItem";
            resources.ApplyResources(this.getRallyPointsToolStripMenuItem, "getRallyPointsToolStripMenuItem");
            this.getRallyPointsToolStripMenuItem.Click += new System.EventHandler(this.getRallyPointsToolStripMenuItem_Click);
            // 
            // saveRallyPointsToolStripMenuItem
            // 
            this.saveRallyPointsToolStripMenuItem.Name = "saveRallyPointsToolStripMenuItem";
            resources.ApplyResources(this.saveRallyPointsToolStripMenuItem, "saveRallyPointsToolStripMenuItem");
            this.saveRallyPointsToolStripMenuItem.Click += new System.EventHandler(this.saveRallyPointsToolStripMenuItem_Click);
            // 
            // saveToFileToolStripMenuItem1
            // 
            this.saveToFileToolStripMenuItem1.Name = "saveToFileToolStripMenuItem1";
            resources.ApplyResources(this.saveToFileToolStripMenuItem1, "saveToFileToolStripMenuItem1");
            this.saveToFileToolStripMenuItem1.Click += new System.EventHandler(this.saveToFileToolStripMenuItem1_Click);
            // 
            // loadFromFileToolStripMenuItem1
            // 
            this.loadFromFileToolStripMenuItem1.Name = "loadFromFileToolStripMenuItem1";
            resources.ApplyResources(this.loadFromFileToolStripMenuItem1, "loadFromFileToolStripMenuItem1");
            this.loadFromFileToolStripMenuItem1.Click += new System.EventHandler(this.loadFromFileToolStripMenuItem1_Click);
            // 
            // trackBar1
            // 
            resources.ApplyResources(this.trackBar1, "trackBar1");
            this.trackBar1.LargeChange = 0.005F;
            this.trackBar1.Maximum = 24F;
            this.trackBar1.Minimum = 1F;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.SmallChange = 0.001F;
            this.trackBar1.TickFrequency = 1F;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trackBar1.Value = 2F;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.insertSplineWPToolStripMenuItem,
            this.loiterToolStripMenuItem,
            this.jumpToolStripMenuItem,
            this.rTLToolStripMenuItem,
            this.landToolStripMenuItem,
            this.takeoffToolStripMenuItem,
            this.setROIToolStripMenuItem,
            this.toolStripSeparator1,
            this.polygonToolStripMenuItem,
            this.geoFenceToolStripMenuItem,
            this.autoWPToolStripMenuItem,
            this.mapToolToolStripMenuItem,
            this.fileLoadSaveToolStripMenuItem,
            this.pOIToolStripMenuItem,
            this.trackerHomeToolStripMenuItem,
            this.modifyAltToolStripMenuItem,
            this.enterUTMCoordToolStripMenuItem,
            this.switchDockingToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            this.contextMenuStrip1.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(this.contextMenuStrip1_Closed);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // insertSplineWPToolStripMenuItem
            // 
            this.insertSplineWPToolStripMenuItem.Name = "insertSplineWPToolStripMenuItem";
            resources.ApplyResources(this.insertSplineWPToolStripMenuItem, "insertSplineWPToolStripMenuItem");
            this.insertSplineWPToolStripMenuItem.Click += new System.EventHandler(this.insertSplineWPToolStripMenuItem_Click);
            // 
            // loiterToolStripMenuItem
            // 
            this.loiterToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loiterForeverToolStripMenuItem,
            this.loitertimeToolStripMenuItem,
            this.loitercirclesToolStripMenuItem});
            this.loiterToolStripMenuItem.Name = "loiterToolStripMenuItem";
            resources.ApplyResources(this.loiterToolStripMenuItem, "loiterToolStripMenuItem");
            // 
            // loiterForeverToolStripMenuItem
            // 
            this.loiterForeverToolStripMenuItem.Name = "loiterForeverToolStripMenuItem";
            resources.ApplyResources(this.loiterForeverToolStripMenuItem, "loiterForeverToolStripMenuItem");
            this.loiterForeverToolStripMenuItem.Click += new System.EventHandler(this.loiterForeverToolStripMenuItem_Click);
            // 
            // loitertimeToolStripMenuItem
            // 
            this.loitertimeToolStripMenuItem.Name = "loitertimeToolStripMenuItem";
            resources.ApplyResources(this.loitertimeToolStripMenuItem, "loitertimeToolStripMenuItem");
            this.loitertimeToolStripMenuItem.Click += new System.EventHandler(this.loitertimeToolStripMenuItem_Click);
            // 
            // loitercirclesToolStripMenuItem
            // 
            this.loitercirclesToolStripMenuItem.Name = "loitercirclesToolStripMenuItem";
            resources.ApplyResources(this.loitercirclesToolStripMenuItem, "loitercirclesToolStripMenuItem");
            this.loitercirclesToolStripMenuItem.Click += new System.EventHandler(this.loitercirclesToolStripMenuItem_Click);
            // 
            // jumpToolStripMenuItem
            // 
            this.jumpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.jumpstartToolStripMenuItem,
            this.jumpwPToolStripMenuItem});
            this.jumpToolStripMenuItem.Name = "jumpToolStripMenuItem";
            resources.ApplyResources(this.jumpToolStripMenuItem, "jumpToolStripMenuItem");
            // 
            // jumpstartToolStripMenuItem
            // 
            this.jumpstartToolStripMenuItem.Name = "jumpstartToolStripMenuItem";
            resources.ApplyResources(this.jumpstartToolStripMenuItem, "jumpstartToolStripMenuItem");
            this.jumpstartToolStripMenuItem.Click += new System.EventHandler(this.jumpstartToolStripMenuItem_Click);
            // 
            // jumpwPToolStripMenuItem
            // 
            this.jumpwPToolStripMenuItem.Name = "jumpwPToolStripMenuItem";
            resources.ApplyResources(this.jumpwPToolStripMenuItem, "jumpwPToolStripMenuItem");
            this.jumpwPToolStripMenuItem.Click += new System.EventHandler(this.jumpwPToolStripMenuItem_Click);
            // 
            // rTLToolStripMenuItem
            // 
            this.rTLToolStripMenuItem.Name = "rTLToolStripMenuItem";
            resources.ApplyResources(this.rTLToolStripMenuItem, "rTLToolStripMenuItem");
            this.rTLToolStripMenuItem.Click += new System.EventHandler(this.rTLToolStripMenuItem_Click);
            // 
            // landToolStripMenuItem
            // 
            this.landToolStripMenuItem.Name = "landToolStripMenuItem";
            resources.ApplyResources(this.landToolStripMenuItem, "landToolStripMenuItem");
            this.landToolStripMenuItem.Click += new System.EventHandler(this.landToolStripMenuItem_Click);
            // 
            // takeoffToolStripMenuItem
            // 
            this.takeoffToolStripMenuItem.Name = "takeoffToolStripMenuItem";
            resources.ApplyResources(this.takeoffToolStripMenuItem, "takeoffToolStripMenuItem");
            this.takeoffToolStripMenuItem.Click += new System.EventHandler(this.takeoffToolStripMenuItem_Click);
            // 
            // setROIToolStripMenuItem
            // 
            this.setROIToolStripMenuItem.Name = "setROIToolStripMenuItem";
            resources.ApplyResources(this.setROIToolStripMenuItem, "setROIToolStripMenuItem");
            this.setROIToolStripMenuItem.Click += new System.EventHandler(this.setROIToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // polygonToolStripMenuItem
            // 
            this.polygonToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addPolygonPointToolStripMenuItem,
            this.clearPolygonToolStripMenuItem,
            this.fromSHPToolStripMenuItem,
            this.areaToolStripMenuItem});
            this.polygonToolStripMenuItem.Name = "polygonToolStripMenuItem";
            resources.ApplyResources(this.polygonToolStripMenuItem, "polygonToolStripMenuItem");
            // 
            // addPolygonPointToolStripMenuItem
            // 
            this.addPolygonPointToolStripMenuItem.Name = "addPolygonPointToolStripMenuItem";
            resources.ApplyResources(this.addPolygonPointToolStripMenuItem, "addPolygonPointToolStripMenuItem");
            this.addPolygonPointToolStripMenuItem.Click += new System.EventHandler(this.addPolygonPointToolStripMenuItem_Click);
            // 
            // clearPolygonToolStripMenuItem
            // 
            this.clearPolygonToolStripMenuItem.Name = "clearPolygonToolStripMenuItem";
            resources.ApplyResources(this.clearPolygonToolStripMenuItem, "clearPolygonToolStripMenuItem");
            this.clearPolygonToolStripMenuItem.Click += new System.EventHandler(this.clearPolygonToolStripMenuItem_Click);
            // 
            // fromSHPToolStripMenuItem
            // 
            this.fromSHPToolStripMenuItem.Name = "fromSHPToolStripMenuItem";
            resources.ApplyResources(this.fromSHPToolStripMenuItem, "fromSHPToolStripMenuItem");
            this.fromSHPToolStripMenuItem.Click += new System.EventHandler(this.fromSHPToolStripMenuItem_Click);
            // 
            // areaToolStripMenuItem
            // 
            this.areaToolStripMenuItem.Name = "areaToolStripMenuItem";
            resources.ApplyResources(this.areaToolStripMenuItem, "areaToolStripMenuItem");
            this.areaToolStripMenuItem.Click += new System.EventHandler(this.areaToolStripMenuItem_Click);
            // 
            // geoFenceToolStripMenuItem
            // 
            this.geoFenceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripSeparator4,
            this.GeoFenceuploadToolStripMenuItem,
            this.GeoFencedownloadToolStripMenuItem,
            this.setReturnLocationToolStripMenuItem,
            this.loadFromFileToolStripMenuItem,
            this.saveToFileToolStripMenuItem,
            this.clearToolStripMenuItem});
            this.geoFenceToolStripMenuItem.Name = "geoFenceToolStripMenuItem";
            resources.ApplyResources(this.geoFenceToolStripMenuItem, "geoFenceToolStripMenuItem");
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // GeoFenceuploadToolStripMenuItem
            // 
            this.GeoFenceuploadToolStripMenuItem.Name = "GeoFenceuploadToolStripMenuItem";
            resources.ApplyResources(this.GeoFenceuploadToolStripMenuItem, "GeoFenceuploadToolStripMenuItem");
            this.GeoFenceuploadToolStripMenuItem.Click += new System.EventHandler(this.GeoFenceuploadToolStripMenuItem_Click);
            // 
            // GeoFencedownloadToolStripMenuItem
            // 
            this.GeoFencedownloadToolStripMenuItem.Name = "GeoFencedownloadToolStripMenuItem";
            resources.ApplyResources(this.GeoFencedownloadToolStripMenuItem, "GeoFencedownloadToolStripMenuItem");
            this.GeoFencedownloadToolStripMenuItem.Click += new System.EventHandler(this.GeoFencedownloadToolStripMenuItem_Click);
            // 
            // setReturnLocationToolStripMenuItem
            // 
            this.setReturnLocationToolStripMenuItem.Name = "setReturnLocationToolStripMenuItem";
            resources.ApplyResources(this.setReturnLocationToolStripMenuItem, "setReturnLocationToolStripMenuItem");
            this.setReturnLocationToolStripMenuItem.Click += new System.EventHandler(this.setReturnLocationToolStripMenuItem_Click);
            // 
            // loadFromFileToolStripMenuItem
            // 
            this.loadFromFileToolStripMenuItem.Name = "loadFromFileToolStripMenuItem";
            resources.ApplyResources(this.loadFromFileToolStripMenuItem, "loadFromFileToolStripMenuItem");
            this.loadFromFileToolStripMenuItem.Click += new System.EventHandler(this.loadFromFileToolStripMenuItem_Click);
            // 
            // saveToFileToolStripMenuItem
            // 
            this.saveToFileToolStripMenuItem.Name = "saveToFileToolStripMenuItem";
            resources.ApplyResources(this.saveToFileToolStripMenuItem, "saveToFileToolStripMenuItem");
            this.saveToFileToolStripMenuItem.Click += new System.EventHandler(this.saveToFileToolStripMenuItem_Click);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            resources.ApplyResources(this.clearToolStripMenuItem, "clearToolStripMenuItem");
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // autoWPToolStripMenuItem
            // 
            this.autoWPToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createWpCircleToolStripMenuItem,
            this.createSplineCircleToolStripMenuItem,
            this.areaToolStripMenuItem1,
            this.textToolStripMenuItem});
            this.autoWPToolStripMenuItem.Name = "autoWPToolStripMenuItem";
            resources.ApplyResources(this.autoWPToolStripMenuItem, "autoWPToolStripMenuItem");
            // 
            // createWpCircleToolStripMenuItem
            // 
            this.createWpCircleToolStripMenuItem.Name = "createWpCircleToolStripMenuItem";
            resources.ApplyResources(this.createWpCircleToolStripMenuItem, "createWpCircleToolStripMenuItem");
            this.createWpCircleToolStripMenuItem.Click += new System.EventHandler(this.createWpCircleToolStripMenuItem_Click);
            // 
            // createSplineCircleToolStripMenuItem
            // 
            this.createSplineCircleToolStripMenuItem.Name = "createSplineCircleToolStripMenuItem";
            resources.ApplyResources(this.createSplineCircleToolStripMenuItem, "createSplineCircleToolStripMenuItem");
            this.createSplineCircleToolStripMenuItem.Click += new System.EventHandler(this.createSplineCircleToolStripMenuItem_Click);
            // 
            // areaToolStripMenuItem1
            // 
            this.areaToolStripMenuItem1.Name = "areaToolStripMenuItem1";
            resources.ApplyResources(this.areaToolStripMenuItem1, "areaToolStripMenuItem1");
            this.areaToolStripMenuItem1.Click += new System.EventHandler(this.areaToolStripMenuItem_Click);
            // 
            // textToolStripMenuItem
            // 
            this.textToolStripMenuItem.Name = "textToolStripMenuItem";
            resources.ApplyResources(this.textToolStripMenuItem, "textToolStripMenuItem");
            this.textToolStripMenuItem.Click += new System.EventHandler(this.textToolStripMenuItem_Click);
            // 
            // mapToolToolStripMenuItem
            // 
            this.mapToolToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rotateMapToolStripMenuItem,
            this.prefetchWPPathToolStripMenuItem,
            this.kMLOverlayToolStripMenuItem,
            this.elevationGraphToolStripMenuItem,
            this.reverseWPsToolStripMenuItem});
            this.mapToolToolStripMenuItem.Name = "mapToolToolStripMenuItem";
            resources.ApplyResources(this.mapToolToolStripMenuItem, "mapToolToolStripMenuItem");
            // 
            // rotateMapToolStripMenuItem
            // 
            this.rotateMapToolStripMenuItem.Name = "rotateMapToolStripMenuItem";
            resources.ApplyResources(this.rotateMapToolStripMenuItem, "rotateMapToolStripMenuItem");
            this.rotateMapToolStripMenuItem.Click += new System.EventHandler(this.rotateMapToolStripMenuItem_Click);
            // 
            // prefetchWPPathToolStripMenuItem
            // 
            this.prefetchWPPathToolStripMenuItem.Name = "prefetchWPPathToolStripMenuItem";
            resources.ApplyResources(this.prefetchWPPathToolStripMenuItem, "prefetchWPPathToolStripMenuItem");
            this.prefetchWPPathToolStripMenuItem.Click += new System.EventHandler(this.prefetchWPPathToolStripMenuItem_Click);
            // 
            // kMLOverlayToolStripMenuItem
            // 
            this.kMLOverlayToolStripMenuItem.Name = "kMLOverlayToolStripMenuItem";
            resources.ApplyResources(this.kMLOverlayToolStripMenuItem, "kMLOverlayToolStripMenuItem");
            this.kMLOverlayToolStripMenuItem.Click += new System.EventHandler(this.kMLOverlayToolStripMenuItem_Click);
            // 
            // elevationGraphToolStripMenuItem
            // 
            this.elevationGraphToolStripMenuItem.Name = "elevationGraphToolStripMenuItem";
            resources.ApplyResources(this.elevationGraphToolStripMenuItem, "elevationGraphToolStripMenuItem");
            this.elevationGraphToolStripMenuItem.Click += new System.EventHandler(this.elevationGraphToolStripMenuItem_Click);
            // 
            // reverseWPsToolStripMenuItem
            // 
            this.reverseWPsToolStripMenuItem.Name = "reverseWPsToolStripMenuItem";
            resources.ApplyResources(this.reverseWPsToolStripMenuItem, "reverseWPsToolStripMenuItem");
            this.reverseWPsToolStripMenuItem.Click += new System.EventHandler(this.reverseWPsToolStripMenuItem_Click);
            // 
            // fileLoadSaveToolStripMenuItem
            // 
            this.fileLoadSaveToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadWPFileToolStripMenuItem,
            this.loadAndAppendToolStripMenuItem,
            this.saveWPFileToolStripMenuItem,
            this.loadKMLFileToolStripMenuItem,
            this.loadSHPFileToolStripMenuItem});
            this.fileLoadSaveToolStripMenuItem.Name = "fileLoadSaveToolStripMenuItem";
            resources.ApplyResources(this.fileLoadSaveToolStripMenuItem, "fileLoadSaveToolStripMenuItem");
            // 
            // loadWPFileToolStripMenuItem
            // 
            this.loadWPFileToolStripMenuItem.Name = "loadWPFileToolStripMenuItem";
            resources.ApplyResources(this.loadWPFileToolStripMenuItem, "loadWPFileToolStripMenuItem");
            this.loadWPFileToolStripMenuItem.Click += new System.EventHandler(this.loadWPFileToolStripMenuItem_Click);
            // 
            // loadAndAppendToolStripMenuItem
            // 
            this.loadAndAppendToolStripMenuItem.Name = "loadAndAppendToolStripMenuItem";
            resources.ApplyResources(this.loadAndAppendToolStripMenuItem, "loadAndAppendToolStripMenuItem");
            this.loadAndAppendToolStripMenuItem.Click += new System.EventHandler(this.loadAndAppendToolStripMenuItem_Click);
            // 
            // saveWPFileToolStripMenuItem
            // 
            this.saveWPFileToolStripMenuItem.Name = "saveWPFileToolStripMenuItem";
            resources.ApplyResources(this.saveWPFileToolStripMenuItem, "saveWPFileToolStripMenuItem");
            this.saveWPFileToolStripMenuItem.Click += new System.EventHandler(this.saveWPFileToolStripMenuItem_Click);
            // 
            // loadKMLFileToolStripMenuItem
            // 
            this.loadKMLFileToolStripMenuItem.Name = "loadKMLFileToolStripMenuItem";
            resources.ApplyResources(this.loadKMLFileToolStripMenuItem, "loadKMLFileToolStripMenuItem");
            this.loadKMLFileToolStripMenuItem.Click += new System.EventHandler(this.loadKMLFileToolStripMenuItem_Click);
            // 
            // loadSHPFileToolStripMenuItem
            // 
            this.loadSHPFileToolStripMenuItem.Name = "loadSHPFileToolStripMenuItem";
            resources.ApplyResources(this.loadSHPFileToolStripMenuItem, "loadSHPFileToolStripMenuItem");
            this.loadSHPFileToolStripMenuItem.Click += new System.EventHandler(this.loadSHPFileToolStripMenuItem_Click);
            // 
            // pOIToolStripMenuItem
            // 
            this.pOIToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.poiaddToolStripMenuItem,
            this.poideleteToolStripMenuItem,
            this.poieditToolStripMenuItem});
            this.pOIToolStripMenuItem.Name = "pOIToolStripMenuItem";
            resources.ApplyResources(this.pOIToolStripMenuItem, "pOIToolStripMenuItem");
            // 
            // poiaddToolStripMenuItem
            // 
            this.poiaddToolStripMenuItem.Name = "poiaddToolStripMenuItem";
            resources.ApplyResources(this.poiaddToolStripMenuItem, "poiaddToolStripMenuItem");
            this.poiaddToolStripMenuItem.Click += new System.EventHandler(this.poiaddToolStripMenuItem_Click);
            // 
            // poideleteToolStripMenuItem
            // 
            this.poideleteToolStripMenuItem.Name = "poideleteToolStripMenuItem";
            resources.ApplyResources(this.poideleteToolStripMenuItem, "poideleteToolStripMenuItem");
            this.poideleteToolStripMenuItem.Click += new System.EventHandler(this.poideleteToolStripMenuItem_Click);
            // 
            // poieditToolStripMenuItem
            // 
            this.poieditToolStripMenuItem.Name = "poieditToolStripMenuItem";
            resources.ApplyResources(this.poieditToolStripMenuItem, "poieditToolStripMenuItem");
            this.poieditToolStripMenuItem.Click += new System.EventHandler(this.poieditToolStripMenuItem_Click);
            // 
            // trackerHomeToolStripMenuItem
            // 
            this.trackerHomeToolStripMenuItem.Name = "trackerHomeToolStripMenuItem";
            resources.ApplyResources(this.trackerHomeToolStripMenuItem, "trackerHomeToolStripMenuItem");
            this.trackerHomeToolStripMenuItem.Click += new System.EventHandler(this.trackerHomeToolStripMenuItem_Click);
            // 
            // modifyAltToolStripMenuItem
            // 
            this.modifyAltToolStripMenuItem.Name = "modifyAltToolStripMenuItem";
            resources.ApplyResources(this.modifyAltToolStripMenuItem, "modifyAltToolStripMenuItem");
            this.modifyAltToolStripMenuItem.Click += new System.EventHandler(this.modifyAltToolStripMenuItem_Click);
            // 
            // enterUTMCoordToolStripMenuItem
            // 
            this.enterUTMCoordToolStripMenuItem.Name = "enterUTMCoordToolStripMenuItem";
            resources.ApplyResources(this.enterUTMCoordToolStripMenuItem, "enterUTMCoordToolStripMenuItem");
            this.enterUTMCoordToolStripMenuItem.Click += new System.EventHandler(this.enterUTMCoordToolStripMenuItem_Click);
            // 
            // switchDockingToolStripMenuItem
            // 
            this.switchDockingToolStripMenuItem.Name = "switchDockingToolStripMenuItem";
            resources.ApplyResources(this.switchDockingToolStripMenuItem, "switchDockingToolStripMenuItem");
            this.switchDockingToolStripMenuItem.Click += new System.EventHandler(this.switchDockingToolStripMenuItem_Click);
            // 
            // panelBASE
            // 
            this.panelBASE.Controls.Add(this.splitter2);
            this.panelBASE.Controls.Add(this.splitter1);
            this.panelBASE.Controls.Add(this.panelMap);
            this.panelBASE.Controls.Add(this.panelWaypoints);
            this.panelBASE.Controls.Add(this.panelAction);
            this.panelBASE.Controls.Add(this.label6);
            resources.ApplyResources(this.panelBASE, "panelBASE");
            this.panelBASE.Name = "panelBASE";
            // 
            // timer1
            // 
            this.timer1.Interval = 1200;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // directorySearcher1
            // 
            this.directorySearcher1.ClientTimeout = System.TimeSpan.Parse("-00:00:01");
            this.directorySearcher1.ServerPageTimeLimit = System.TimeSpan.Parse("-00:00:01");
            this.directorySearcher1.ServerTimeLimit = System.TimeSpan.Parse("-00:00:01");
            // 
            // avoidPntLat
            // 
            this.avoidPntLat.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            resources.ApplyResources(this.avoidPntLat, "avoidPntLat");
            this.avoidPntLat.Name = "avoidPntLat";
            this.avoidPntLat.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // avoidPntLng
            // 
            this.avoidPntLng.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            resources.ApplyResources(this.avoidPntLng, "avoidPntLng");
            this.avoidPntLng.Name = "avoidPntLng";
            this.avoidPntLng.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // avoidPntRadius
            // 
            this.avoidPntRadius.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            resources.ApplyResources(this.avoidPntRadius, "avoidPntRadius");
            this.avoidPntRadius.Name = "avoidPntRadius";
            this.avoidPntRadius.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // avoidPntsInfo
            // 
            resources.ApplyResources(this.avoidPntsInfo, "avoidPntsInfo");
            this.avoidPntsInfo.Name = "avoidPntsInfo";
            // 
            // Manage
            // 
            this.Manage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            resources.ApplyResources(this.Manage, "Manage");
            this.Manage.Name = "Manage";
            this.Manage.Text = "删除";
            // 
            // FlightPlanner
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.panelBASE);
            resources.ApplyResources(this, "$this");
            this.Name = "FlightPlanner";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FlightPlanner_FormClosing);
            this.Load += new System.EventHandler(this.FlightPlanner_Load);
            this.Resize += new System.EventHandler(this.Planner_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.Commands)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panelWaypoints.ResumeLayout(false);
            this.panelWaypoints.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tb_RTL_hgt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_autoTO_hgt)).EndInit();
            this.panelAction.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panelMap.ResumeLayout(false);
            this.panelMap.PerformLayout();
            this.panel_avoidPnts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_avoidPnts)).EndInit();
            this.pannel_wpcommands.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gb_modWPhgt.ResumeLayout(false);
            this.gb_modWPhgt.PerformLayout();
            this.panel11.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.num_endWP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_startWP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_newhgt)).EndInit();
            this.contextMenuStrip2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.panelBASE.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion

        private Controls.MyButton BUT_read;
        private Controls.MyButton BUT_write;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.LinkLabel label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label Label1;
        private System.Windows.Forms.TextBox TXT_homealt;
        private System.Windows.Forms.TextBox TXT_homelng;
        private System.Windows.Forms.TextBox TXT_homelat;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lbl_status;
        private System.Windows.Forms.DataGridView Commands;
        private Controls.MyButton BUT_Add;
        private System.Windows.Forms.Label LBL_WPRad;
        private System.Windows.Forms.Label LBL_defalutalt;
        private System.Windows.Forms.Label label5;
        public BSE.Windows.Forms.Panel panelWaypoints;
        public BSE.Windows.Forms.Panel panelAction;
        private System.Windows.Forms.Panel panelMap;
        public Controls.myGMAP MainMap;
        private Controls.MyTrackBar trackBar1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lbl_distance;
        private System.Windows.Forms.Label lbl_prevdist;
        private BSE.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panelBASE;
        private System.Windows.Forms.Label lbl_homedist;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripMenuItem clearMissionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem polygonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addPolygonPointToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearPolygonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loiterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loiterForeverToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loitertimeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loitercirclesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jumpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jumpstartToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jumpwPToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem deleteWPToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripMenuItem geoFenceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem GeoFencedownloadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setReturnLocationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadFromFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem GeoFenceuploadToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem setROIToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoWPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createWpCircleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mapToolToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ContextMeasure;
        private System.Windows.Forms.ToolStripMenuItem polySplitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DonePolySplitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addPolygonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem donePolygonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DeletePolySplitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem genWPsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rotateMapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoomToToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem prefetchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem kMLOverlayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem elevationGraphToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rTLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem landToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem takeoffToolStripMenuItem;
        private System.Windows.Forms.ComboBox comboBoxMapType;
        private System.Windows.Forms.ToolStripMenuItem fileLoadSaveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadWPFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveWPFileToolStripMenuItem;

        private System.Windows.Forms.ToolStripMenuItem wpManageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wpUploadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wpDownloadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wpSaveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wpOpenToolStripMenuItem;

        private System.Windows.Forms.ToolStripMenuItem trackerHomeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reverseWPsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadAndAppendToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem savePolygonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadPolygonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadKMLbjStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadKMLWPStripMenuItem;
        public System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.CheckBox chk_grid;
        private System.Windows.Forms.ToolStripMenuItem insertWpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editWpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem doneWpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rallyPointsToolStripMenuItem;

        private System.Windows.Forms.ToolStripMenuItem wpEditToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem polyEditToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem polySplitToolStripMenuItemAll;
        private System.Windows.Forms.ToolStripMenuItem mapToolStripMenuItem_simp;
        private System.Windows.Forms.ToolStripMenuItem bjToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem kmlToolStripMenuItem;

        private System.Windows.Forms.ToolStripMenuItem getRallyPointsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveRallyPointsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setRallyPointToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearRallyPointsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadKMLFileToolStripMenuItem;
        private System.Windows.Forms.LinkLabel lnk_kml;
        private System.Windows.Forms.ToolStripMenuItem modifyAltToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToFileToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem loadFromFileToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem prefetchWPPathToolStripMenuItem;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox TXT_altwarn;
        private System.Windows.Forms.ToolStripMenuItem pOIToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem poiaddToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem poideleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem poieditToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enterUTMCoordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadSHPFileToolStripMenuItem;
        private Controls.Coords coords1;
        private Controls.MyButton BUT_loadwpfile;
        private Controls.MyButton BUT_saveWPFile;
        private Controls.MyButton BUT_planning;
        private Controls.MyButton BUT_generate;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.ToolStripMenuItem switchDockingToolStripMenuItem;
        private BSE.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.ToolStripMenuItem insertSplineWPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createSplineCircleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fromSHPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem textToolStripMenuItem;
        public System.Windows.Forms.CheckBox CHK_verifyheight;
        public System.Windows.Forms.TextBox TXT_WPRad;
        public System.Windows.Forms.TextBox TXT_DefaultAlt;
        public System.Windows.Forms.TextBox TXT_loiterrad;
        public System.Windows.Forms.CheckBox CHK_splinedefault;
        public System.Windows.Forms.ComboBox CMB_altmode;
        private System.Windows.Forms.ToolStripMenuItem areaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setHomeHereToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem areaToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.CheckBox map_corr_auto;
        private Controls.MyButton lb_clear_polygon;
        private Controls.MyButton lb_wp_revert;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.Label lb_autotakeoff_hgt;
        private System.Windows.Forms.Label lb_rtl_hgt;
        private System.DirectoryServices.DirectorySearcher directorySearcher1;
        public System.Windows.Forms.CheckBox cb_en_autoTORTL;
        private Controls.MyButton bt_edit_wps;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel2;
        private Controls.MyButton myButton2;
        private Controls.MyButton myButton3;
        private Controls.MyButton btn_wphgt_mod;
        private System.Windows.Forms.NumericUpDown tb_RTL_hgt;
        private System.Windows.Forms.NumericUpDown tb_autoTO_hgt;
        private System.Windows.Forms.GroupBox gb_modWPhgt;
        private Controls.MyButton btn_cancel;
        private Controls.MyButton btn_modconfirm;
        private System.Windows.Forms.Panel panel11;
        private Controls.LineSeparator lineSeparator1;
        private System.Windows.Forms.NumericUpDown num_endWP;
        private System.Windows.Forms.NumericUpDown num_startWP;
        private System.Windows.Forms.RadioButton rbtn_sel;
        private System.Windows.Forms.RadioButton rbtn_modall;
        private System.Windows.Forms.NumericUpDown num_newhgt;
        private System.Windows.Forms.Label lb_newWPhgt;
        private System.Windows.Forms.Panel panel10;
        private Controls.MyButton lb_load_kml;
        private Controls.MyButton wp_migrate;
        private System.Windows.Forms.Panel panel9;
        private Controls.MyButton lb_download_map;
        private Controls.MyButton myButton1;
        private System.Windows.Forms.Panel panel8;
        private Controls.MyButton lb_search_map;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbl_ftime;
        private System.Windows.Forms.RadioButton rb_bj;
        private System.Windows.Forms.RadioButton rb_wp;
        private System.Windows.Forms.TextBox tb_place;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel4;
        private Controls.MyButton load_last_offs;
        private Controls.MyButton map_corr_manual;
        private GaryPerkin.UserControls.Buttons.RoundButton rb_center;
        private GaryPerkin.UserControls.Buttons.RoundButton rb_avoid_pnts;
        public  GaryPerkin.UserControls.Buttons.RoundButton rb_en_lidar;
        private GaryPerkin.UserControls.Buttons.RoundButton rb_clear_polygon;
        private GaryPerkin.UserControls.Buttons.RoundButton rb_clear_mission;
        private GaryPerkin.UserControls.Buttons.RoundButton rb_autoWP;
        private GaryPerkin.UserControls.Buttons.RoundButton rb_inv_wps;
        private BSE.Windows.Forms.Panel pannel_wpcommands;
        private GaryPerkin.UserControls.Buttons.RoundButton rb_wp_hgt;
        private GaryPerkin.UserControls.Buttons.RoundButton rb_editwpcommand;
        private GaryPerkin.UserControls.Buttons.RoundButton rb_wpmigrate;
        private GaryPerkin.UserControls.Buttons.RoundButton rb_corr;
        private BSE.Windows.Forms.Panel panel_avoidPnts;
        private System.Windows.Forms.DataGridView dataGridView_avoidPnts;
        private GaryPerkin.UserControls.Buttons.RoundButton rb_avoidPntsAdd;
        private GaryPerkin.UserControls.Buttons.RoundButton rb_avoidPntsOk;
        private System.Windows.Forms.DataGridViewComboBoxColumn Command;
        private System.Windows.Forms.DataGridViewTextBoxColumn Param1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Param2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Param3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Param4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Lat;
        private System.Windows.Forms.DataGridViewTextBoxColumn Lon;
        private System.Windows.Forms.DataGridViewTextBoxColumn Alt;
        private System.Windows.Forms.DataGridViewTextBoxColumn coordZone;
        private System.Windows.Forms.DataGridViewTextBoxColumn coordEasting;
        private System.Windows.Forms.DataGridViewTextBoxColumn coordNorthing;
        private System.Windows.Forms.DataGridViewTextBoxColumn MGRS;
        private System.Windows.Forms.DataGridViewButtonColumn Delete;
        private System.Windows.Forms.DataGridViewImageColumn Up;
        private System.Windows.Forms.DataGridViewImageColumn Down;
        private System.Windows.Forms.DataGridViewTextBoxColumn Grad;
        private System.Windows.Forms.DataGridViewTextBoxColumn Angle;
        private System.Windows.Forms.DataGridViewTextBoxColumn Dist;
        private System.Windows.Forms.DataGridViewTextBoxColumn AZ;
        private System.Windows.Forms.DataGridViewTextBoxColumn TagData;
        private System.Windows.Forms.DataGridViewTextBoxColumn avoidPntLat;
        private System.Windows.Forms.DataGridViewTextBoxColumn avoidPntLng;
        private System.Windows.Forms.DataGridViewTextBoxColumn avoidPntRadius;
        private System.Windows.Forms.DataGridViewTextBoxColumn avoidPntsInfo;
        private System.Windows.Forms.DataGridViewButtonColumn Manage;
    }
}