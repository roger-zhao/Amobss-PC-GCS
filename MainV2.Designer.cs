using System;
using System.Drawing;
namespace MissionPlanner
{
    partial class MainV2
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
            Console.WriteLine("mainv2_Dispose");
            if (PluginThreadrunner != null)
                PluginThreadrunner.Dispose();
            if (disposing && (components != null))
            {
                components.Dispose();
            }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainV2));
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.CTX_mainmenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.autoHideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fullScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.readonlyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectionOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuFlightData = new System.Windows.Forms.ToolStripButton();
            this.MenuFlightPlanner = new System.Windows.Forms.ToolStripButton();
            this.MenuInitConfig = new System.Windows.Forms.ToolStripButton();
            this.MenuUserAppConfig = new System.Windows.Forms.ToolStripButton();
            this.MenuConfigTune = new System.Windows.Forms.ToolStripButton();
            this.MenuUpdateSoftware = new System.Windows.Forms.ToolStripButton();
            this.MenuHelp = new System.Windows.Forms.ToolStripButton();
            this.MenuConnect = new System.Windows.Forms.ToolStripButton();
            this.toolStripConnectionControl = new MissionPlanner.Controls.ToolStripConnectionControl();
            this.MenuSimulation = new System.Windows.Forms.ToolStripButton();
            this.MenuTerminal = new System.Windows.Forms.ToolStripButton();
            this.MenuDonate = new System.Windows.Forms.ToolStripMenuItem();
            this.menu = new MissionPlanner.Controls.MyButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_work_mode = new System.Windows.Forms.Button();
            this.bt_log = new System.Windows.Forms.Button();
            this.ls_pannel1 = new MissionPlanner.Controls.LineSeparator();
            this.ss_remrssi = new WindowWidgets.SignalStrength();
            this.rb_heartbeat = new GaryPerkin.UserControls.Buttons.RoundButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cb_cmd = new System.Windows.Forms.CheckBox();
            this.cb_tuningPID = new System.Windows.Forms.CheckBox();
            this.cb_log = new System.Windows.Forms.CheckBox();
            this.lb_fp_mode = new System.Windows.Forms.Label();
            this.lb_water_value = new System.Windows.Forms.Label();
            this.lb_dist_home = new System.Windows.Forms.Label();
            this.lb_vel = new System.Windows.Forms.Label();
            this.lb_pitch = new System.Windows.Forms.Label();
            this.lb_roll = new System.Windows.Forms.Label();
            this.lb_yaw = new System.Windows.Forms.Label();
            this.lb_flight_time = new System.Windows.Forms.Label();
            this.lb_hgt = new System.Windows.Forms.Label();
            this.lb_gps = new System.Windows.Forms.Label();
            this.lb_fc_volt = new System.Windows.Forms.Label();
            this.lb_power = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.curr_loc = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pn_pfd = new System.Windows.Forms.Panel();
            this.yaw_gauge = new AGaugeApp.AGauge();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.rb_wifi = new System.Windows.Forms.RadioButton();
            this.gb_comm_type = new System.Windows.Forms.GroupBox();
            this.rb_remradio = new System.Windows.Forms.RadioButton();
            this.listBox_log = new System.Windows.Forms.ListBox();
            this.panel_tuningPID = new BSE.Windows.Forms.Panel();
            this.zg1 = new ZedGraph.ZedGraphControl();
            this.ZedGraphTimer = new System.Windows.Forms.Timer(this.components);
            this.MainMenu.SuspendLayout();
            this.CTX_mainmenu.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.gb_comm_type.SuspendLayout();
            this.panel_tuningPID.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            this.MainMenu.BackgroundImage = global::MissionPlanner.Properties.Resources.bg;
            this.MainMenu.ContextMenuStrip = this.CTX_mainmenu;
            this.MainMenu.GripMargin = new System.Windows.Forms.Padding(0);
            this.MainMenu.ImageScalingSize = new System.Drawing.Size(0, 0);
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuFlightData,
            this.MenuFlightPlanner,
            this.MenuInitConfig,
            this.MenuUserAppConfig,
            this.MenuConfigTune,
            this.MenuUpdateSoftware,
            this.MenuHelp,
            this.MenuConnect,
            this.toolStripConnectionControl});
            resources.ApplyResources(this.MainMenu, "MainMenu");
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Stretch = false;
            this.MainMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.MainMenu_ItemClicked);
            this.MainMenu.MouseLeave += new System.EventHandler(this.MainMenu_MouseLeave);
            // 
            // CTX_mainmenu
            // 
            this.CTX_mainmenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autoHideToolStripMenuItem,
            this.fullScreenToolStripMenuItem,
            this.readonlyToolStripMenuItem,
            this.connectionOptionsToolStripMenuItem});
            this.CTX_mainmenu.Name = "CTX_mainmenu";
            resources.ApplyResources(this.CTX_mainmenu, "CTX_mainmenu");
            // 
            // autoHideToolStripMenuItem
            // 
            this.autoHideToolStripMenuItem.CheckOnClick = true;
            this.autoHideToolStripMenuItem.Name = "autoHideToolStripMenuItem";
            resources.ApplyResources(this.autoHideToolStripMenuItem, "autoHideToolStripMenuItem");
            this.autoHideToolStripMenuItem.Click += new System.EventHandler(this.autoHideToolStripMenuItem_Click);
            // 
            // fullScreenToolStripMenuItem
            // 
            this.fullScreenToolStripMenuItem.CheckOnClick = true;
            this.fullScreenToolStripMenuItem.Name = "fullScreenToolStripMenuItem";
            resources.ApplyResources(this.fullScreenToolStripMenuItem, "fullScreenToolStripMenuItem");
            this.fullScreenToolStripMenuItem.Click += new System.EventHandler(this.fullScreenToolStripMenuItem_Click);
            // 
            // readonlyToolStripMenuItem
            // 
            this.readonlyToolStripMenuItem.CheckOnClick = true;
            this.readonlyToolStripMenuItem.Name = "readonlyToolStripMenuItem";
            resources.ApplyResources(this.readonlyToolStripMenuItem, "readonlyToolStripMenuItem");
            this.readonlyToolStripMenuItem.Click += new System.EventHandler(this.readonlyToolStripMenuItem_Click);
            // 
            // connectionOptionsToolStripMenuItem
            // 
            this.connectionOptionsToolStripMenuItem.Name = "connectionOptionsToolStripMenuItem";
            resources.ApplyResources(this.connectionOptionsToolStripMenuItem, "connectionOptionsToolStripMenuItem");
            this.connectionOptionsToolStripMenuItem.Click += new System.EventHandler(this.connectionOptionsToolStripMenuItem_Click);
            // 
            // MenuFlightData
            // 
            resources.ApplyResources(this.MenuFlightData, "MenuFlightData");
            this.MenuFlightData.ForeColor = System.Drawing.Color.Transparent;
            this.MenuFlightData.Margin = new System.Windows.Forms.Padding(0);
            this.MenuFlightData.Name = "MenuFlightData";
            this.MenuFlightData.Click += new System.EventHandler(this.MenuFlightData_Click);
            // 
            // MenuFlightPlanner
            // 
            this.MenuFlightPlanner.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.MenuFlightPlanner, "MenuFlightPlanner");
            this.MenuFlightPlanner.ForeColor = System.Drawing.Color.Transparent;
            this.MenuFlightPlanner.Margin = new System.Windows.Forms.Padding(0);
            this.MenuFlightPlanner.Name = "MenuFlightPlanner";
            this.MenuFlightPlanner.Click += new System.EventHandler(this.MenuFlightPlanner_Click);
            // 
            // MenuInitConfig
            // 
            resources.ApplyResources(this.MenuInitConfig, "MenuInitConfig");
            this.MenuInitConfig.ForeColor = System.Drawing.Color.Transparent;
            this.MenuInitConfig.Margin = new System.Windows.Forms.Padding(0);
            this.MenuInitConfig.Name = "MenuInitConfig";
            this.MenuInitConfig.Click += new System.EventHandler(this.MenuSetup_Click);
            // 
            // MenuUserAppConfig
            // 
            resources.ApplyResources(this.MenuUserAppConfig, "MenuUserAppConfig");
            this.MenuUserAppConfig.ForeColor = System.Drawing.Color.Transparent;
            this.MenuUserAppConfig.Margin = new System.Windows.Forms.Padding(0);
            this.MenuUserAppConfig.Name = "MenuUserAppConfig";
            this.MenuUserAppConfig.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.MenuUserAppConfig.Click += new System.EventHandler(this.MenuUserAppConfig_Click);
            // 
            // MenuConfigTune
            // 
            resources.ApplyResources(this.MenuConfigTune, "MenuConfigTune");
            this.MenuConfigTune.ForeColor = System.Drawing.Color.Transparent;
            this.MenuConfigTune.Margin = new System.Windows.Forms.Padding(0);
            this.MenuConfigTune.Name = "MenuConfigTune";
            this.MenuConfigTune.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.MenuConfigTune.Click += new System.EventHandler(this.MenuTuning_Click);
            // 
            // MenuUpdateSoftware
            // 
            resources.ApplyResources(this.MenuUpdateSoftware, "MenuUpdateSoftware");
            this.MenuUpdateSoftware.ForeColor = System.Drawing.Color.Transparent;
            this.MenuUpdateSoftware.Margin = new System.Windows.Forms.Padding(0);
            this.MenuUpdateSoftware.Name = "MenuUpdateSoftware";
            this.MenuUpdateSoftware.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.MenuUpdateSoftware.Click += new System.EventHandler(this.MenuUpdateSoftware_Click);
            // 
            // MenuHelp
            // 
            resources.ApplyResources(this.MenuHelp, "MenuHelp");
            this.MenuHelp.ForeColor = System.Drawing.Color.Transparent;
            this.MenuHelp.Margin = new System.Windows.Forms.Padding(0);
            this.MenuHelp.Name = "MenuHelp";
            this.MenuHelp.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            // 
            // MenuConnect
            // 
            this.MenuConnect.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            resources.ApplyResources(this.MenuConnect, "MenuConnect");
            this.MenuConnect.ForeColor = System.Drawing.Color.Transparent;
            this.MenuConnect.Margin = new System.Windows.Forms.Padding(0);
            this.MenuConnect.Name = "MenuConnect";
            this.MenuConnect.Click += new System.EventHandler(this.MenuConnect_Click);
            // 
            // toolStripConnectionControl
            // 
            this.toolStripConnectionControl.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripConnectionControl.BackgroundImage = global::MissionPlanner.Properties.Resources.bgdark;
            this.toolStripConnectionControl.ForeColor = System.Drawing.Color.Black;
            this.toolStripConnectionControl.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripConnectionControl.Name = "toolStripConnectionControl";
            resources.ApplyResources(this.toolStripConnectionControl, "toolStripConnectionControl");
            this.toolStripConnectionControl.MouseLeave += new System.EventHandler(this.MainMenu_MouseLeave);
            // 
            // MenuSimulation
            // 
            resources.ApplyResources(this.MenuSimulation, "MenuSimulation");
            this.MenuSimulation.ForeColor = System.Drawing.Color.White;
            this.MenuSimulation.Margin = new System.Windows.Forms.Padding(0);
            this.MenuSimulation.Name = "MenuSimulation";
            this.MenuSimulation.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.MenuSimulation.Click += new System.EventHandler(this.MenuSimulation_Click);
            // 
            // MenuTerminal
            // 
            resources.ApplyResources(this.MenuTerminal, "MenuTerminal");
            this.MenuTerminal.ForeColor = System.Drawing.Color.White;
            this.MenuTerminal.Margin = new System.Windows.Forms.Padding(0);
            this.MenuTerminal.Name = "MenuTerminal";
            this.MenuTerminal.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.MenuTerminal.Click += new System.EventHandler(this.MenuTerminal_Click);
            // 
            // MenuDonate
            // 
            resources.ApplyResources(this.MenuDonate, "MenuDonate");
            this.MenuDonate.ForeColor = System.Drawing.Color.White;
            this.MenuDonate.Image = global::MissionPlanner.Properties.Resources.donate;
            this.MenuDonate.Name = "MenuDonate";
            this.MenuDonate.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.MenuDonate.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // menu
            // 
            resources.ApplyResources(this.menu, "menu");
            this.menu.Name = "menu";
            this.menu.UseVisualStyleBackColor = true;
            this.menu.MouseEnter += new System.EventHandler(this.menu_MouseEnter);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.btn_work_mode);
            this.panel1.Controls.Add(this.bt_log);
            this.panel1.Controls.Add(this.ls_pannel1);
            this.panel1.Controls.Add(this.ss_remrssi);
            this.panel1.Controls.Add(this.rb_heartbeat);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.lb_fp_mode);
            this.panel1.Controls.Add(this.lb_water_value);
            this.panel1.Controls.Add(this.lb_dist_home);
            this.panel1.Controls.Add(this.lb_vel);
            this.panel1.Controls.Add(this.lb_pitch);
            this.panel1.Controls.Add(this.lb_roll);
            this.panel1.Controls.Add(this.lb_yaw);
            this.panel1.Controls.Add(this.lb_flight_time);
            this.panel1.Controls.Add(this.lb_hgt);
            this.panel1.Controls.Add(this.lb_gps);
            this.panel1.Controls.Add(this.lb_fc_volt);
            this.panel1.Controls.Add(this.lb_power);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.curr_loc);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.MainMenu);
            this.panel1.Controls.Add(this.pn_pfd);
            this.panel1.Controls.Add(this.yaw_gauge);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            this.panel1.MouseLeave += new System.EventHandler(this.MainMenu_MouseLeave);
            // 
            // btn_work_mode
            // 
            resources.ApplyResources(this.btn_work_mode, "btn_work_mode");
            this.btn_work_mode.BackColor = System.Drawing.SystemColors.Window;
            this.btn_work_mode.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btn_work_mode.Name = "btn_work_mode";
            this.btn_work_mode.UseVisualStyleBackColor = false;
            this.btn_work_mode.Click += new System.EventHandler(this.btn_work_mode_Click);
            // 
            // bt_log
            // 
            resources.ApplyResources(this.bt_log, "bt_log");
            this.bt_log.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.bt_log.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.bt_log.Name = "bt_log";
            this.bt_log.UseVisualStyleBackColor = false;
            this.bt_log.Click += new System.EventHandler(this.bt_log_Click);
            // 
            // ls_pannel1
            // 
            this.ls_pannel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.ls_pannel1, "ls_pannel1");
            this.ls_pannel1.Name = "ls_pannel1";
            // 
            // ss_remrssi
            // 
            this.ss_remrssi.BackColor = System.Drawing.Color.Transparent;
            this.ss_remrssi.BackgroundStyle = WindowWidgets.SignalStrengthBackgroundStyle.Normal;
            this.ss_remrssi.BarLayout = WindowWidgets.SignalStrengthLayout.LeftToRight;
            this.ss_remrssi.BarSpacing = 3;
            this.ss_remrssi.BarStepSize = 20;
            this.ss_remrssi.CenterGradientColor = System.Drawing.Color.Gray;
            this.ss_remrssi.EmptyBarColor = System.Drawing.Color.Gray;
            this.ss_remrssi.GoodSignalColor = System.Drawing.Color.Lime;
            this.ss_remrssi.GoodSignalThreshold = 0.6F;
            resources.ApplyResources(this.ss_remrssi, "ss_remrssi");
            this.ss_remrssi.MaximumValue = 1F;
            this.ss_remrssi.MinimumValue = 0F;
            this.ss_remrssi.Name = "ss_remrssi";
            this.ss_remrssi.NoSignalColor = System.Drawing.Color.White;
            this.ss_remrssi.NoSignalThreshold = 0F;
            this.ss_remrssi.NumberOfBars = 5;
            this.ss_remrssi.PoorSignalColor = System.Drawing.Color.Yellow;
            this.ss_remrssi.PoorSignalThreshold = 0.3F;
            this.ss_remrssi.SmallBarHeight = 10;
            this.ss_remrssi.UseSolidBars = false;
            this.ss_remrssi.Value = 0F;
            this.ss_remrssi.WeakSignalColor = System.Drawing.Color.Red;
            this.ss_remrssi.WeakSignalThreshold = 0.1F;
            this.ss_remrssi.XColor = System.Drawing.Color.Red;
            this.ss_remrssi.XIfNoSignal = true;
            this.ss_remrssi.XWidth = 1.5F;
            // 
            // rb_heartbeat
            // 
            this.rb_heartbeat.BackColor = System.Drawing.Color.Brown;
            resources.ApplyResources(this.rb_heartbeat, "rb_heartbeat");
            this.rb_heartbeat.Name = "rb_heartbeat";
            this.rb_heartbeat.RecessDepth = 0;
            this.rb_heartbeat.UseVisualStyleBackColor = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cb_cmd);
            this.groupBox1.Controls.Add(this.cb_tuningPID);
            this.groupBox1.Controls.Add(this.cb_log);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // cb_cmd
            // 
            resources.ApplyResources(this.cb_cmd, "cb_cmd");
            this.cb_cmd.Name = "cb_cmd";
            this.cb_cmd.UseVisualStyleBackColor = true;
            this.cb_cmd.CheckedChanged += new System.EventHandler(this.cb_cmd_CheckedChanged);
            // 
            // cb_tuningPID
            // 
            resources.ApplyResources(this.cb_tuningPID, "cb_tuningPID");
            this.cb_tuningPID.Name = "cb_tuningPID";
            this.cb_tuningPID.UseVisualStyleBackColor = true;
            this.cb_tuningPID.CheckedChanged += new System.EventHandler(this.cb_tuningPID_CheckedChanged);
            // 
            // cb_log
            // 
            resources.ApplyResources(this.cb_log, "cb_log");
            this.cb_log.Name = "cb_log";
            this.cb_log.UseVisualStyleBackColor = true;
            this.cb_log.CheckedChanged += new System.EventHandler(this.cb_log_CheckedChanged);
            // 
            // lb_fp_mode
            // 
            this.lb_fp_mode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.lb_fp_mode, "lb_fp_mode");
            this.lb_fp_mode.Name = "lb_fp_mode";
            // 
            // lb_water_value
            // 
            resources.ApplyResources(this.lb_water_value, "lb_water_value");
            this.lb_water_value.Name = "lb_water_value";
            // 
            // lb_dist_home
            // 
            resources.ApplyResources(this.lb_dist_home, "lb_dist_home");
            this.lb_dist_home.Name = "lb_dist_home";
            // 
            // lb_vel
            // 
            resources.ApplyResources(this.lb_vel, "lb_vel");
            this.lb_vel.Name = "lb_vel";
            // 
            // lb_pitch
            // 
            resources.ApplyResources(this.lb_pitch, "lb_pitch");
            this.lb_pitch.Name = "lb_pitch";
            // 
            // lb_roll
            // 
            resources.ApplyResources(this.lb_roll, "lb_roll");
            this.lb_roll.Name = "lb_roll";
            // 
            // lb_yaw
            // 
            resources.ApplyResources(this.lb_yaw, "lb_yaw");
            this.lb_yaw.Name = "lb_yaw";
            // 
            // lb_flight_time
            // 
            resources.ApplyResources(this.lb_flight_time, "lb_flight_time");
            this.lb_flight_time.Name = "lb_flight_time";
            // 
            // lb_hgt
            // 
            resources.ApplyResources(this.lb_hgt, "lb_hgt");
            this.lb_hgt.Name = "lb_hgt";
            // 
            // lb_gps
            // 
            resources.ApplyResources(this.lb_gps, "lb_gps");
            this.lb_gps.Name = "lb_gps";
            // 
            // lb_fc_volt
            // 
            resources.ApplyResources(this.lb_fc_volt, "lb_fc_volt");
            this.lb_fc_volt.Name = "lb_fc_volt";
            // 
            // lb_power
            // 
            resources.ApplyResources(this.lb_power, "lb_power");
            this.lb_power.Name = "lb_power";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.Color.Transparent;
            this.label3.Name = "label3";
            // 
            // curr_loc
            // 
            resources.ApplyResources(this.curr_loc, "curr_loc");
            this.curr_loc.Name = "curr_loc";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.Color.Transparent;
            this.label2.Name = "label2";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // pn_pfd
            // 
            resources.ApplyResources(this.pn_pfd, "pn_pfd");
            this.pn_pfd.Name = "pn_pfd";
            // 
            // yaw_gauge
            // 
            this.yaw_gauge.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            resources.ApplyResources(this.yaw_gauge, "yaw_gauge");
            this.yaw_gauge.BaseArcColor = System.Drawing.Color.DimGray;
            this.yaw_gauge.BaseArcRadius = 10;
            this.yaw_gauge.BaseArcStart = 270;
            this.yaw_gauge.BaseArcSweep = 360;
            this.yaw_gauge.BaseArcWidth = 20;
            this.yaw_gauge.Cap_Idx = ((byte)(2));
            this.yaw_gauge.CapColor = System.Drawing.Color.Transparent;
            this.yaw_gauge.CapColors = new System.Drawing.Color[] {
        System.Drawing.Color.White,
        System.Drawing.Color.White,
        System.Drawing.Color.Transparent,
        System.Drawing.Color.Black,
        System.Drawing.Color.Black};
            this.yaw_gauge.CapPosition = new System.Drawing.Point(10, 10);
            this.yaw_gauge.CapsPosition = new System.Drawing.Point[] {
        new System.Drawing.Point(10, 10),
        new System.Drawing.Point(10, 10),
        new System.Drawing.Point(10, 10),
        new System.Drawing.Point(10, 10),
        new System.Drawing.Point(10, 10)};
            this.yaw_gauge.CapsText = new string[] {
        "Yaw",
        "",
        "",
        "",
        ""};
            this.yaw_gauge.CapText = "";
            this.yaw_gauge.Center = new System.Drawing.Point(75, 75);
            this.yaw_gauge.MaxValue = 359F;
            this.yaw_gauge.MinValue = 0F;
            this.yaw_gauge.Name = "yaw_gauge";
            this.yaw_gauge.Need_Idx = ((byte)(3));
            this.yaw_gauge.NeedleColor1 = AGaugeApp.AGauge.NeedleColorEnum.Red;
            this.yaw_gauge.NeedleColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.yaw_gauge.NeedleEnabled = true;
            this.yaw_gauge.NeedleRadius = 60;
            this.yaw_gauge.NeedlesColor1 = new AGaugeApp.AGauge.NeedleColorEnum[] {
        AGaugeApp.AGauge.NeedleColorEnum.Gray,
        AGaugeApp.AGauge.NeedleColorEnum.Gray,
        AGaugeApp.AGauge.NeedleColorEnum.Gray,
        AGaugeApp.AGauge.NeedleColorEnum.Red};
            this.yaw_gauge.NeedlesColor2 = new System.Drawing.Color[] {
        System.Drawing.Color.DimGray,
        System.Drawing.Color.DimGray,
        System.Drawing.Color.DimGray,
        System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))))};
            this.yaw_gauge.NeedlesEnabled = new bool[] {
        true,
        false,
        false,
        true};
            this.yaw_gauge.NeedlesRadius = new int[] {
        50,
        80,
        80,
        60};
            this.yaw_gauge.NeedlesType = new int[] {
        0,
        0,
        0,
        0};
            this.yaw_gauge.NeedlesWidth = new int[] {
        2,
        2,
        2,
        6};
            this.yaw_gauge.NeedleType = 0;
            this.yaw_gauge.NeedleWidth = 6;
            this.yaw_gauge.Range_Idx = ((byte)(0));
            this.yaw_gauge.RangeColor = System.Drawing.Color.Aqua;
            this.yaw_gauge.RangeEnabled = true;
            this.yaw_gauge.RangeEndValue = 360F;
            this.yaw_gauge.RangeInnerRadius = 46;
            this.yaw_gauge.RangeOuterRadius = 56;
            this.yaw_gauge.RangesColor = new System.Drawing.Color[] {
        System.Drawing.Color.Aqua,
        System.Drawing.Color.DimGray,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control};
            this.yaw_gauge.RangesEnabled = new bool[] {
        true,
        false,
        false,
        false,
        false};
            this.yaw_gauge.RangesEndValue = new float[] {
        360F,
        180F,
        0F,
        0F,
        0F};
            this.yaw_gauge.RangesInnerRadius = new int[] {
        46,
        45,
        50,
        70,
        70};
            this.yaw_gauge.RangesOuterRadius = new int[] {
        56,
        50,
        60,
        80,
        80};
            this.yaw_gauge.RangesStartValue = new float[] {
        0F,
        -180F,
        0F,
        0F,
        0F};
            this.yaw_gauge.RangeStartValue = 0F;
            this.yaw_gauge.ScaleLinesInterColor = System.Drawing.Color.Black;
            this.yaw_gauge.ScaleLinesInterInnerRadius = 60;
            this.yaw_gauge.ScaleLinesInterOuterRadius = 50;
            this.yaw_gauge.ScaleLinesInterWidth = 1;
            this.yaw_gauge.ScaleLinesMajorColor = System.Drawing.Color.Black;
            this.yaw_gauge.ScaleLinesMajorInnerRadius = 50;
            this.yaw_gauge.ScaleLinesMajorOuterRadius = 60;
            this.yaw_gauge.ScaleLinesMajorStepValue = 60F;
            this.yaw_gauge.ScaleLinesMajorWidth = 2;
            this.yaw_gauge.ScaleLinesMinorColor = System.Drawing.Color.Gray;
            this.yaw_gauge.ScaleLinesMinorInnerRadius = 50;
            this.yaw_gauge.ScaleLinesMinorNumOf = 2;
            this.yaw_gauge.ScaleLinesMinorOuterRadius = 55;
            this.yaw_gauge.ScaleLinesMinorWidth = 1;
            this.yaw_gauge.ScaleNumbersColor = System.Drawing.Color.Transparent;
            this.yaw_gauge.ScaleNumbersFormat = null;
            this.yaw_gauge.ScaleNumbersRadius = 38;
            this.yaw_gauge.ScaleNumbersRotation = 0;
            this.yaw_gauge.ScaleNumbersStartScaleLine = 1;
            this.yaw_gauge.ScaleNumbersStepScaleLines = 1;
            this.yaw_gauge.Value = 0F;
            this.yaw_gauge.Value0 = 0F;
            this.yaw_gauge.Value1 = 0F;
            this.yaw_gauge.Value2 = 0F;
            this.yaw_gauge.Value3 = 0F;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.MenuText;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.ForeColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.Name = "textBox1";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::MissionPlanner.Properties.Resources.alarmGreen;
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 400;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // rb_wifi
            // 
            resources.ApplyResources(this.rb_wifi, "rb_wifi");
            this.rb_wifi.Name = "rb_wifi";
            this.rb_wifi.UseVisualStyleBackColor = true;
            this.rb_wifi.CheckedChanged += new System.EventHandler(this.rb_wifi_CheckedChanged);
            // 
            // gb_comm_type
            // 
            this.gb_comm_type.BackgroundImage = global::MissionPlanner.Properties.Resources.bg;
            this.gb_comm_type.Controls.Add(this.rb_remradio);
            this.gb_comm_type.Controls.Add(this.rb_wifi);
            resources.ApplyResources(this.gb_comm_type, "gb_comm_type");
            this.gb_comm_type.Name = "gb_comm_type";
            this.gb_comm_type.TabStop = false;
            // 
            // rb_remradio
            // 
            resources.ApplyResources(this.rb_remradio, "rb_remradio");
            this.rb_remradio.Checked = true;
            this.rb_remradio.Name = "rb_remradio";
            this.rb_remradio.TabStop = true;
            this.rb_remradio.UseVisualStyleBackColor = true;
            this.rb_remradio.CheckedChanged += new System.EventHandler(this.rb_remradio_CheckedChanged);
            // 
            // listBox_log
            // 
            this.listBox_log.BackColor = System.Drawing.Color.White;
            this.listBox_log.ForeColor = System.Drawing.Color.Black;
            this.listBox_log.FormattingEnabled = true;
            resources.ApplyResources(this.listBox_log, "listBox_log");
            this.listBox_log.Name = "listBox_log";
            // 
            // panel_tuningPID
            // 
            this.panel_tuningPID.AssociatedSplitter = null;
            this.panel_tuningPID.BackColor = System.Drawing.Color.Transparent;
            this.panel_tuningPID.CaptionFont = new System.Drawing.Font("Microsoft YaHei UI", 11.75F, System.Drawing.FontStyle.Bold);
            this.panel_tuningPID.CaptionHeight = 27;
            this.panel_tuningPID.Controls.Add(this.zg1);
            this.panel_tuningPID.CustomColors.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(184)))), ((int)(((byte)(184)))));
            this.panel_tuningPID.CustomColors.CaptionCloseIcon = System.Drawing.SystemColors.ControlText;
            this.panel_tuningPID.CustomColors.CaptionExpandIcon = System.Drawing.SystemColors.ControlText;
            this.panel_tuningPID.CustomColors.CaptionGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.panel_tuningPID.CustomColors.CaptionGradientEnd = System.Drawing.SystemColors.ButtonFace;
            this.panel_tuningPID.CustomColors.CaptionGradientMiddle = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.panel_tuningPID.CustomColors.CaptionSelectedGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(215)))), ((int)(((byte)(243)))));
            this.panel_tuningPID.CustomColors.CaptionSelectedGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(215)))), ((int)(((byte)(243)))));
            this.panel_tuningPID.CustomColors.CaptionText = System.Drawing.SystemColors.ControlText;
            this.panel_tuningPID.CustomColors.CollapsedCaptionText = System.Drawing.SystemColors.ControlText;
            this.panel_tuningPID.CustomColors.ContentGradientBegin = System.Drawing.SystemColors.ButtonFace;
            this.panel_tuningPID.CustomColors.ContentGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.panel_tuningPID.CustomColors.InnerBorderColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.panel_tuningPID, "panel_tuningPID");
            this.panel_tuningPID.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panel_tuningPID.Image = null;
            this.panel_tuningPID.Name = "panel_tuningPID";
            this.panel_tuningPID.ToolTipTextCloseIcon = null;
            this.panel_tuningPID.ToolTipTextExpandIconPanelCollapsed = null;
            this.panel_tuningPID.ToolTipTextExpandIconPanelExpanded = null;
            // 
            // zg1
            // 
            this.zg1.AllowDrop = true;
            resources.ApplyResources(this.zg1, "zg1");
            this.zg1.IsShowPointValues = true;
            this.zg1.Name = "zg1";
            this.zg1.ScrollGrace = 0D;
            this.zg1.ScrollMaxX = 0D;
            this.zg1.ScrollMaxY = 0D;
            this.zg1.ScrollMaxY2 = 0D;
            this.zg1.ScrollMinX = 0D;
            this.zg1.ScrollMinY = 0D;
            this.zg1.ScrollMinY2 = 0D;
            this.zg1.DoubleClick += new System.EventHandler(this.zg1_DoubleClick);
            // 
            // ZedGraphTimer
            // 
            this.ZedGraphTimer.Tick += new System.EventHandler(this.timerZed_Tick);
            // 
            // MainV2
            // 
            this.AllowDrop = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panel_tuningPID);
            this.Controls.Add(this.listBox_log);
            this.Controls.Add(this.gb_comm_type);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menu);
            this.KeyPreview = true;
            this.MainMenuStrip = this.MainMenu;
            this.Name = "MainV2";
            this.Load += new System.EventHandler(this.MainV2_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainV2_KeyDown);
            this.Resize += new System.EventHandler(this.MainV2_Resize);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.CTX_mainmenu.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.gb_comm_type.ResumeLayout(false);
            this.gb_comm_type.PerformLayout();
            this.panel_tuningPID.ResumeLayout(false);
            this.panel_tuningPID.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ToolStripButton MenuFlightData;
        public System.Windows.Forms.ToolStripButton MenuFlightPlanner;
        public System.Windows.Forms.ToolStripButton MenuInitConfig;
        public System.Windows.Forms.ToolStripButton MenuSimulation;
        public System.Windows.Forms.ToolStripButton MenuConfigTune;
        public System.Windows.Forms.ToolStripButton MenuTerminal;
        public System.Windows.Forms.ToolStripButton MenuConnect;
        private System.Windows.Forms.ToolStripButton MenuUserAppConfig;
        private System.Windows.Forms.ToolStripButton MenuUpdateSoftware;

        private System.Windows.Forms.ToolStripButton MenuHelp;
        private Controls.MyButton menu;
        public System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ContextMenuStrip CTX_mainmenu;
        private System.Windows.Forms.ToolStripMenuItem autoHideToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MenuDonate;
        public System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem fullScreenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem readonlyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectionOptionsToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label curr_loc;
        private System.Windows.Forms.Label lb_water_value;
        private System.Windows.Forms.Label lb_dist_home;
        private System.Windows.Forms.Label lb_vel;
        private System.Windows.Forms.Label lb_flight_time;
        private System.Windows.Forms.Label lb_hgt;
        private System.Windows.Forms.Label lb_gps;
        private System.Windows.Forms.Label lb_fc_volt;
        private System.Windows.Forms.Label lb_power;
        private System.Windows.Forms.CheckBox cb_log;
        private System.Windows.Forms.CheckBox cb_cmd;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel pn_pfd;
        private AGaugeApp.AGauge yaw_gauge;
        private GaryPerkin.UserControls.Buttons.RoundButton rb_heartbeat;
        private WindowWidgets.SignalStrength ss_remrssi;
        private System.Windows.Forms.RadioButton rb_wifi;
        private System.Windows.Forms.GroupBox gb_comm_type;
        private System.Windows.Forms.RadioButton rb_remradio;
        private Controls.ToolStripConnectionControl toolStripConnectionControl;
        private Controls.LineSeparator ls_pannel1;
        private System.Windows.Forms.Label lb_fp_mode;
        private System.Windows.Forms.Button bt_log;
        private System.Windows.Forms.ListBox listBox_log;
        private System.Windows.Forms.Button btn_work_mode;
        private System.Windows.Forms.Label lb_pitch;
        private System.Windows.Forms.Label lb_roll;
        private System.Windows.Forms.Label lb_yaw;
        private System.Windows.Forms.CheckBox cb_tuningPID;
        private BSE.Windows.Forms.Panel panel_tuningPID;
        private System.Windows.Forms.Timer ZedGraphTimer;
        private ZedGraph.ZedGraphControl zg1;
        //private System.Windows.Forms.Label lb_power;
        //private System.Windows.Forms.Label lb_gps;
        //private System.Windows.Forms.Label lb_hgt;
        //private System.Windows.Forms.Label lb_fc_volt;
        //private System.Windows.Forms.Label lb_flight_time;
        //private System.Windows.Forms.Label lb_vel;
        //private System.Windows.Forms.Label lb_dist_home;
        //private System.Windows.Forms.Label lb_water_quan;
    }
}