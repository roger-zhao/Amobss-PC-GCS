using System.Windows.Forms;

namespace MissionPlanner.GCSViews.ConfigurationView
{
    partial class ConfigArducopter
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
            base.Dispose(disposing);
        }
        private bool first_run = true;
        public void OnFocus(object sender, System.EventArgs e)
        {
            if (!first_run)
            {
                // return;
            }
            TabControl tc = (TabControl)sender;
            // if (tc.SelectedTab == this.Parent)
            {
                //Parent-Tab is selected, do stuff...
                // Activate();
                first_run = false;
            }

        }
        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigArducopter));
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.mavlinkNumericUpDown3 = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.THR_RATE_P = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.label21 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.CHK_lockrollpitch = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.WPNAV_SPEED_UP = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.label27 = new System.Windows.Forms.Label();
            this.WPNAV_LOIT_SPEED = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.WPNAV_SPEED_DN = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.WPNAV_RADIUS = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.WPNAV_SPEED = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.THR_ALT_P = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.label22 = new System.Windows.Forms.Label();
            this.groupBox19 = new System.Windows.Forms.GroupBox();
            this.PosXY_D = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.label40 = new System.Windows.Forms.Label();
            this.PosXY_I = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.PosXY_IMAX = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.lbdd = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.HLD_LAT_P = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.label31 = new System.Windows.Forms.Label();
            this.groupBox20 = new System.Windows.Forms.GroupBox();
            this.mavlinkNumericUpDown13 = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.STB_YAW_D = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.label34 = new System.Windows.Forms.Label();
            this.STB_YAW_IMAX = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.STB_YAW_I = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.label44 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.STB_YAW_P = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.label35 = new System.Windows.Forms.Label();
            this.groupBox21 = new System.Windows.Forms.GroupBox();
            this.mavlinkNumericUpDown10 = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.STB_PIT_D = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.label30 = new System.Windows.Forms.Label();
            this.STB_PIT_IMAX = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.STB_PIT_I = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.label43 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.STB_PIT_P = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.label42 = new System.Windows.Forms.Label();
            this.groupBox22 = new System.Windows.Forms.GroupBox();
            this.mavlinkNumericUpDown2 = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.label41 = new System.Windows.Forms.Label();
            this.STB_RLL_D = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.STB_RLL_IMAX = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.STB_RLL_I = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.label26 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.STB_RLL_P = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.label46 = new System.Windows.Forms.Label();
            this.groupBox23 = new System.Windows.Forms.GroupBox();
            this.RATE_YAW_D = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.RATE_YAW_IMAX = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.label47 = new System.Windows.Forms.Label();
            this.RATE_YAW_I = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.label77 = new System.Windows.Forms.Label();
            this.RATE_YAW_P = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.label82 = new System.Windows.Forms.Label();
            this.lb_acc = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.groupBox24 = new System.Windows.Forms.GroupBox();
            this.mavlinkNumericUpDown15 = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.RATE_PIT_D = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.RATE_PIT_IMAX = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.label84 = new System.Windows.Forms.Label();
            this.RATE_PIT_I = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.label86 = new System.Windows.Forms.Label();
            this.RATE_PIT_P = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.label87 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.groupBox25 = new System.Windows.Forms.GroupBox();
            this.mavlinkNumericUpDown14 = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.RATE_RLL_D = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.label17 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.RATE_RLL_IMAX = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.label88 = new System.Windows.Forms.Label();
            this.RATE_RLL_I = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.label90 = new System.Windows.Forms.Label();
            this.RATE_RLL_P = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.label91 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.mavlinkNumericUpDown1 = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.LOITER_LAT_D = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.LOITER_LAT_IMAX = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.label19 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.LOITER_LAT_I = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.LOITER_LAT_P = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.BUT_rerequestparams = new MissionPlanner.Controls.MyButton();
            this.BUT_writePIDS = new MissionPlanner.Controls.MyButton();
            this.myLabel3 = new MissionPlanner.Controls.MyLabel();
            this.myLabel2 = new MissionPlanner.Controls.MyLabel();
            this.myLabel1 = new MissionPlanner.Controls.MyLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.mavlinkNumericUpDown4 = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.THR_ACCEL_D = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.THR_ACCEL_IMAX = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.THR_ACCEL_I = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.THR_ACCEL_P = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.BUT_refreshpart = new MissionPlanner.Controls.MyButton();
            this.myLabel4 = new MissionPlanner.Controls.MyLabel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.CH7_OPT = new MissionPlanner.Controls.MavlinkComboBox();
            this.TUNE = new MissionPlanner.Controls.MavlinkComboBox();
            this.CH8_OPT = new MissionPlanner.Controls.MavlinkComboBox();
            this.TUNE_HIGH = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.TUNE_LOW = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.myLabel5 = new MissionPlanner.Controls.MyLabel();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.RATE_YAW_FILT = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.RATE_PIT_FILT = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.RATE_RLL_FILT = new MissionPlanner.Controls.MavlinkNumericUpDown();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.cb_pwm_log_rate = new System.Windows.Forms.ComboBox();
            this.cb_log_accZ_rate = new System.Windows.Forms.ComboBox();
            this.cb_log_pos_rate = new System.Windows.Forms.ComboBox();
            this.cb_log_angrate_rate = new System.Windows.Forms.ComboBox();
            this.cb_log_rcout = new System.Windows.Forms.CheckBox();
            this.cb_log_accz = new System.Windows.Forms.CheckBox();
            this.cb_log_pos = new System.Windows.Forms.CheckBox();
            this.cb_log_angrate = new System.Windows.Forms.CheckBox();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.num_gyr_Hz = new System.Windows.Forms.NumericUpDown();
            this.num_acc_Hz = new System.Windows.Forms.NumericUpDown();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.rb_8KHz = new System.Windows.Forms.RadioButton();
            this.rb_1KHz = new System.Windows.Forms.RadioButton();
            this.label39 = new System.Windows.Forms.Label();
            this.cb_8KHz_ft = new System.Windows.Forms.ComboBox();
            this.cb_1KHz_ft = new System.Windows.Forms.ComboBox();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.btn_write_angle_max = new System.Windows.Forms.Button();
            this.num_max_angle = new System.Windows.Forms.NumericUpDown();
            this.label48 = new System.Windows.Forms.Label();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mavlinkNumericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.THR_RATE_P)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WPNAV_SPEED_UP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WPNAV_LOIT_SPEED)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WPNAV_SPEED_DN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WPNAV_RADIUS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WPNAV_SPEED)).BeginInit();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.THR_ALT_P)).BeginInit();
            this.groupBox19.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PosXY_D)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PosXY_I)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PosXY_IMAX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HLD_LAT_P)).BeginInit();
            this.groupBox20.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mavlinkNumericUpDown13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.STB_YAW_D)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.STB_YAW_IMAX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.STB_YAW_I)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.STB_YAW_P)).BeginInit();
            this.groupBox21.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mavlinkNumericUpDown10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.STB_PIT_D)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.STB_PIT_IMAX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.STB_PIT_I)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.STB_PIT_P)).BeginInit();
            this.groupBox22.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mavlinkNumericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.STB_RLL_D)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.STB_RLL_IMAX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.STB_RLL_I)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.STB_RLL_P)).BeginInit();
            this.groupBox23.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RATE_YAW_D)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RATE_YAW_IMAX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RATE_YAW_I)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RATE_YAW_P)).BeginInit();
            this.groupBox24.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mavlinkNumericUpDown15)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RATE_PIT_D)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RATE_PIT_IMAX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RATE_PIT_I)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RATE_PIT_P)).BeginInit();
            this.groupBox25.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mavlinkNumericUpDown14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RATE_RLL_D)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RATE_RLL_IMAX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RATE_RLL_I)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RATE_RLL_P)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mavlinkNumericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LOITER_LAT_D)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LOITER_LAT_IMAX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LOITER_LAT_I)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LOITER_LAT_P)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mavlinkNumericUpDown4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.THR_ACCEL_D)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.THR_ACCEL_IMAX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.THR_ACCEL_I)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.THR_ACCEL_P)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TUNE_HIGH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TUNE_LOW)).BeginInit();
            this.groupBox9.SuspendLayout();
            this.groupBox10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RATE_YAW_FILT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RATE_PIT_FILT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RATE_RLL_FILT)).BeginInit();
            this.groupBox11.SuspendLayout();
            this.groupBox12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_gyr_Hz)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_acc_Hz)).BeginInit();
            this.groupBox13.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_max_angle)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.mavlinkNumericUpDown3);
            this.groupBox5.Controls.Add(this.THR_RATE_P);
            this.groupBox5.Controls.Add(this.label21);
            this.groupBox5.Controls.Add(this.label25);
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // mavlinkNumericUpDown3
            // 
            resources.ApplyResources(this.mavlinkNumericUpDown3, "mavlinkNumericUpDown3");
            this.mavlinkNumericUpDown3.Max = 1F;
            this.mavlinkNumericUpDown3.Min = 0F;
            this.mavlinkNumericUpDown3.Name = "mavlinkNumericUpDown3";
            this.mavlinkNumericUpDown3.ParamName = null;
            this.mavlinkNumericUpDown3.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // THR_RATE_P
            // 
            resources.ApplyResources(this.THR_RATE_P, "THR_RATE_P");
            this.THR_RATE_P.Max = 1F;
            this.THR_RATE_P.Min = 0F;
            this.THR_RATE_P.Name = "THR_RATE_P";
            this.THR_RATE_P.ParamName = null;
            this.THR_RATE_P.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label21
            // 
            resources.ApplyResources(this.label21, "label21");
            this.label21.Name = "label21";
            // 
            // label25
            // 
            resources.ApplyResources(this.label25, "label25");
            this.label25.Name = "label25";
            // 
            // CHK_lockrollpitch
            // 
            resources.ApplyResources(this.CHK_lockrollpitch, "CHK_lockrollpitch");
            this.CHK_lockrollpitch.Name = "CHK_lockrollpitch";
            this.CHK_lockrollpitch.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.WPNAV_SPEED_UP);
            this.groupBox4.Controls.Add(this.label27);
            this.groupBox4.Controls.Add(this.WPNAV_LOIT_SPEED);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.WPNAV_SPEED_DN);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Controls.Add(this.WPNAV_RADIUS);
            this.groupBox4.Controls.Add(this.label15);
            this.groupBox4.Controls.Add(this.WPNAV_SPEED);
            this.groupBox4.Controls.Add(this.label16);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // WPNAV_SPEED_UP
            // 
            resources.ApplyResources(this.WPNAV_SPEED_UP, "WPNAV_SPEED_UP");
            this.WPNAV_SPEED_UP.Max = 1F;
            this.WPNAV_SPEED_UP.Min = 0F;
            this.WPNAV_SPEED_UP.Name = "WPNAV_SPEED_UP";
            this.WPNAV_SPEED_UP.ParamName = null;
            this.WPNAV_SPEED_UP.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label27
            // 
            resources.ApplyResources(this.label27, "label27");
            this.label27.Name = "label27";
            // 
            // WPNAV_LOIT_SPEED
            // 
            resources.ApplyResources(this.WPNAV_LOIT_SPEED, "WPNAV_LOIT_SPEED");
            this.WPNAV_LOIT_SPEED.Max = 1F;
            this.WPNAV_LOIT_SPEED.Min = 0F;
            this.WPNAV_LOIT_SPEED.Name = "WPNAV_LOIT_SPEED";
            this.WPNAV_LOIT_SPEED.ParamName = null;
            this.WPNAV_LOIT_SPEED.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // WPNAV_SPEED_DN
            // 
            resources.ApplyResources(this.WPNAV_SPEED_DN, "WPNAV_SPEED_DN");
            this.WPNAV_SPEED_DN.Max = 1F;
            this.WPNAV_SPEED_DN.Min = 0F;
            this.WPNAV_SPEED_DN.Name = "WPNAV_SPEED_DN";
            this.WPNAV_SPEED_DN.ParamName = null;
            this.WPNAV_SPEED_DN.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // WPNAV_RADIUS
            // 
            resources.ApplyResources(this.WPNAV_RADIUS, "WPNAV_RADIUS");
            this.WPNAV_RADIUS.Max = 1F;
            this.WPNAV_RADIUS.Min = 0F;
            this.WPNAV_RADIUS.Name = "WPNAV_RADIUS";
            this.WPNAV_RADIUS.ParamName = null;
            this.WPNAV_RADIUS.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // WPNAV_SPEED
            // 
            resources.ApplyResources(this.WPNAV_SPEED, "WPNAV_SPEED");
            this.WPNAV_SPEED.Max = 1F;
            this.WPNAV_SPEED.Min = 0F;
            this.WPNAV_SPEED.Name = "WPNAV_SPEED";
            this.WPNAV_SPEED.ParamName = null;
            this.WPNAV_SPEED.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.THR_ALT_P);
            this.groupBox7.Controls.Add(this.label22);
            resources.ApplyResources(this.groupBox7, "groupBox7");
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.TabStop = false;
            // 
            // THR_ALT_P
            // 
            resources.ApplyResources(this.THR_ALT_P, "THR_ALT_P");
            this.THR_ALT_P.Max = 1F;
            this.THR_ALT_P.Min = 0F;
            this.THR_ALT_P.Name = "THR_ALT_P";
            this.THR_ALT_P.ParamName = null;
            this.THR_ALT_P.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label22
            // 
            resources.ApplyResources(this.label22, "label22");
            this.label22.Name = "label22";
            // 
            // groupBox19
            // 
            this.groupBox19.Controls.Add(this.PosXY_D);
            this.groupBox19.Controls.Add(this.label40);
            this.groupBox19.Controls.Add(this.PosXY_I);
            this.groupBox19.Controls.Add(this.PosXY_IMAX);
            this.groupBox19.Controls.Add(this.lbdd);
            this.groupBox19.Controls.Add(this.label20);
            this.groupBox19.Controls.Add(this.HLD_LAT_P);
            this.groupBox19.Controls.Add(this.label31);
            resources.ApplyResources(this.groupBox19, "groupBox19");
            this.groupBox19.Name = "groupBox19";
            this.groupBox19.TabStop = false;
            // 
            // PosXY_D
            // 
            resources.ApplyResources(this.PosXY_D, "PosXY_D");
            this.PosXY_D.Max = 1F;
            this.PosXY_D.Min = 0F;
            this.PosXY_D.Name = "PosXY_D";
            this.PosXY_D.ParamName = null;
            // 
            // label40
            // 
            resources.ApplyResources(this.label40, "label40");
            this.label40.Name = "label40";
            // 
            // PosXY_I
            // 
            resources.ApplyResources(this.PosXY_I, "PosXY_I");
            this.PosXY_I.Max = 1F;
            this.PosXY_I.Min = 0F;
            this.PosXY_I.Name = "PosXY_I";
            this.PosXY_I.ParamName = null;
            this.PosXY_I.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // PosXY_IMAX
            // 
            resources.ApplyResources(this.PosXY_IMAX, "PosXY_IMAX");
            this.PosXY_IMAX.Max = 1F;
            this.PosXY_IMAX.Min = 0F;
            this.PosXY_IMAX.Name = "PosXY_IMAX";
            this.PosXY_IMAX.ParamName = null;
            this.PosXY_IMAX.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // lbdd
            // 
            resources.ApplyResources(this.lbdd, "lbdd");
            this.lbdd.Name = "lbdd";
            // 
            // label20
            // 
            resources.ApplyResources(this.label20, "label20");
            this.label20.Name = "label20";
            // 
            // HLD_LAT_P
            // 
            resources.ApplyResources(this.HLD_LAT_P, "HLD_LAT_P");
            this.HLD_LAT_P.Max = 1F;
            this.HLD_LAT_P.Min = 0F;
            this.HLD_LAT_P.Name = "HLD_LAT_P";
            this.HLD_LAT_P.ParamName = null;
            this.HLD_LAT_P.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label31
            // 
            resources.ApplyResources(this.label31, "label31");
            this.label31.Name = "label31";
            // 
            // groupBox20
            // 
            this.groupBox20.Controls.Add(this.mavlinkNumericUpDown13);
            this.groupBox20.Controls.Add(this.STB_YAW_D);
            this.groupBox20.Controls.Add(this.label34);
            this.groupBox20.Controls.Add(this.STB_YAW_IMAX);
            this.groupBox20.Controls.Add(this.STB_YAW_I);
            this.groupBox20.Controls.Add(this.label44);
            this.groupBox20.Controls.Add(this.label36);
            this.groupBox20.Controls.Add(this.label33);
            this.groupBox20.Controls.Add(this.STB_YAW_P);
            this.groupBox20.Controls.Add(this.label35);
            resources.ApplyResources(this.groupBox20, "groupBox20");
            this.groupBox20.Name = "groupBox20";
            this.groupBox20.TabStop = false;
            // 
            // mavlinkNumericUpDown13
            // 
            resources.ApplyResources(this.mavlinkNumericUpDown13, "mavlinkNumericUpDown13");
            this.mavlinkNumericUpDown13.Max = 1F;
            this.mavlinkNumericUpDown13.Min = 0F;
            this.mavlinkNumericUpDown13.Name = "mavlinkNumericUpDown13";
            this.mavlinkNumericUpDown13.ParamName = null;
            this.mavlinkNumericUpDown13.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // STB_YAW_D
            // 
            resources.ApplyResources(this.STB_YAW_D, "STB_YAW_D");
            this.STB_YAW_D.Max = 1F;
            this.STB_YAW_D.Min = 0F;
            this.STB_YAW_D.Name = "STB_YAW_D";
            this.STB_YAW_D.ParamName = null;
            this.STB_YAW_D.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label34
            // 
            resources.ApplyResources(this.label34, "label34");
            this.label34.Name = "label34";
            // 
            // STB_YAW_IMAX
            // 
            resources.ApplyResources(this.STB_YAW_IMAX, "STB_YAW_IMAX");
            this.STB_YAW_IMAX.Max = 1F;
            this.STB_YAW_IMAX.Min = 0F;
            this.STB_YAW_IMAX.Name = "STB_YAW_IMAX";
            this.STB_YAW_IMAX.ParamName = null;
            this.STB_YAW_IMAX.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // STB_YAW_I
            // 
            resources.ApplyResources(this.STB_YAW_I, "STB_YAW_I");
            this.STB_YAW_I.Max = 1F;
            this.STB_YAW_I.Min = 0F;
            this.STB_YAW_I.Name = "STB_YAW_I";
            this.STB_YAW_I.ParamName = null;
            this.STB_YAW_I.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label44
            // 
            resources.ApplyResources(this.label44, "label44");
            this.label44.Name = "label44";
            // 
            // label36
            // 
            resources.ApplyResources(this.label36, "label36");
            this.label36.Name = "label36";
            // 
            // label33
            // 
            resources.ApplyResources(this.label33, "label33");
            this.label33.Name = "label33";
            // 
            // STB_YAW_P
            // 
            resources.ApplyResources(this.STB_YAW_P, "STB_YAW_P");
            this.STB_YAW_P.Max = 1F;
            this.STB_YAW_P.Min = 0F;
            this.STB_YAW_P.Name = "STB_YAW_P";
            this.STB_YAW_P.ParamName = null;
            this.STB_YAW_P.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label35
            // 
            resources.ApplyResources(this.label35, "label35");
            this.label35.Name = "label35";
            // 
            // groupBox21
            // 
            this.groupBox21.Controls.Add(this.mavlinkNumericUpDown10);
            this.groupBox21.Controls.Add(this.STB_PIT_D);
            this.groupBox21.Controls.Add(this.label30);
            this.groupBox21.Controls.Add(this.STB_PIT_IMAX);
            this.groupBox21.Controls.Add(this.STB_PIT_I);
            this.groupBox21.Controls.Add(this.label43);
            this.groupBox21.Controls.Add(this.label32);
            this.groupBox21.Controls.Add(this.label29);
            this.groupBox21.Controls.Add(this.STB_PIT_P);
            this.groupBox21.Controls.Add(this.label42);
            resources.ApplyResources(this.groupBox21, "groupBox21");
            this.groupBox21.Name = "groupBox21";
            this.groupBox21.TabStop = false;
            // 
            // mavlinkNumericUpDown10
            // 
            resources.ApplyResources(this.mavlinkNumericUpDown10, "mavlinkNumericUpDown10");
            this.mavlinkNumericUpDown10.Max = 1F;
            this.mavlinkNumericUpDown10.Min = 0F;
            this.mavlinkNumericUpDown10.Name = "mavlinkNumericUpDown10";
            this.mavlinkNumericUpDown10.ParamName = null;
            this.mavlinkNumericUpDown10.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // STB_PIT_D
            // 
            resources.ApplyResources(this.STB_PIT_D, "STB_PIT_D");
            this.STB_PIT_D.Max = 1F;
            this.STB_PIT_D.Min = 0F;
            this.STB_PIT_D.Name = "STB_PIT_D";
            this.STB_PIT_D.ParamName = null;
            this.STB_PIT_D.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label30
            // 
            resources.ApplyResources(this.label30, "label30");
            this.label30.Name = "label30";
            // 
            // STB_PIT_IMAX
            // 
            resources.ApplyResources(this.STB_PIT_IMAX, "STB_PIT_IMAX");
            this.STB_PIT_IMAX.Max = 1F;
            this.STB_PIT_IMAX.Min = 0F;
            this.STB_PIT_IMAX.Name = "STB_PIT_IMAX";
            this.STB_PIT_IMAX.ParamName = null;
            this.STB_PIT_IMAX.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // STB_PIT_I
            // 
            resources.ApplyResources(this.STB_PIT_I, "STB_PIT_I");
            this.STB_PIT_I.Max = 1F;
            this.STB_PIT_I.Min = 0F;
            this.STB_PIT_I.Name = "STB_PIT_I";
            this.STB_PIT_I.ParamName = null;
            this.STB_PIT_I.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label43
            // 
            resources.ApplyResources(this.label43, "label43");
            this.label43.Name = "label43";
            // 
            // label32
            // 
            resources.ApplyResources(this.label32, "label32");
            this.label32.Name = "label32";
            // 
            // label29
            // 
            resources.ApplyResources(this.label29, "label29");
            this.label29.Name = "label29";
            // 
            // STB_PIT_P
            // 
            resources.ApplyResources(this.STB_PIT_P, "STB_PIT_P");
            this.STB_PIT_P.Max = 1F;
            this.STB_PIT_P.Min = 0F;
            this.STB_PIT_P.Name = "STB_PIT_P";
            this.STB_PIT_P.ParamName = null;
            this.STB_PIT_P.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label42
            // 
            resources.ApplyResources(this.label42, "label42");
            this.label42.Name = "label42";
            // 
            // groupBox22
            // 
            this.groupBox22.Controls.Add(this.mavlinkNumericUpDown2);
            this.groupBox22.Controls.Add(this.label41);
            this.groupBox22.Controls.Add(this.STB_RLL_D);
            this.groupBox22.Controls.Add(this.STB_RLL_IMAX);
            this.groupBox22.Controls.Add(this.STB_RLL_I);
            this.groupBox22.Controls.Add(this.label26);
            this.groupBox22.Controls.Add(this.label28);
            this.groupBox22.Controls.Add(this.label24);
            this.groupBox22.Controls.Add(this.STB_RLL_P);
            this.groupBox22.Controls.Add(this.label46);
            resources.ApplyResources(this.groupBox22, "groupBox22");
            this.groupBox22.Name = "groupBox22";
            this.groupBox22.TabStop = false;
            // 
            // mavlinkNumericUpDown2
            // 
            resources.ApplyResources(this.mavlinkNumericUpDown2, "mavlinkNumericUpDown2");
            this.mavlinkNumericUpDown2.Max = 1F;
            this.mavlinkNumericUpDown2.Min = 0F;
            this.mavlinkNumericUpDown2.Name = "mavlinkNumericUpDown2";
            this.mavlinkNumericUpDown2.ParamName = null;
            // 
            // label41
            // 
            resources.ApplyResources(this.label41, "label41");
            this.label41.Name = "label41";
            // 
            // STB_RLL_D
            // 
            resources.ApplyResources(this.STB_RLL_D, "STB_RLL_D");
            this.STB_RLL_D.Max = 1F;
            this.STB_RLL_D.Min = 0F;
            this.STB_RLL_D.Name = "STB_RLL_D";
            this.STB_RLL_D.ParamName = null;
            this.STB_RLL_D.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // STB_RLL_IMAX
            // 
            resources.ApplyResources(this.STB_RLL_IMAX, "STB_RLL_IMAX");
            this.STB_RLL_IMAX.Max = 1F;
            this.STB_RLL_IMAX.Min = 0F;
            this.STB_RLL_IMAX.Name = "STB_RLL_IMAX";
            this.STB_RLL_IMAX.ParamName = null;
            this.STB_RLL_IMAX.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // STB_RLL_I
            // 
            resources.ApplyResources(this.STB_RLL_I, "STB_RLL_I");
            this.STB_RLL_I.Max = 1F;
            this.STB_RLL_I.Min = 0F;
            this.STB_RLL_I.Name = "STB_RLL_I";
            this.STB_RLL_I.ParamName = null;
            this.STB_RLL_I.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label26
            // 
            resources.ApplyResources(this.label26, "label26");
            this.label26.Name = "label26";
            // 
            // label28
            // 
            resources.ApplyResources(this.label28, "label28");
            this.label28.Name = "label28";
            // 
            // label24
            // 
            resources.ApplyResources(this.label24, "label24");
            this.label24.Name = "label24";
            // 
            // STB_RLL_P
            // 
            resources.ApplyResources(this.STB_RLL_P, "STB_RLL_P");
            this.STB_RLL_P.Max = 1F;
            this.STB_RLL_P.Min = 0F;
            this.STB_RLL_P.Name = "STB_RLL_P";
            this.STB_RLL_P.ParamName = null;
            this.STB_RLL_P.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label46
            // 
            resources.ApplyResources(this.label46, "label46");
            this.label46.Name = "label46";
            // 
            // groupBox23
            // 
            this.groupBox23.Controls.Add(this.RATE_YAW_D);
            this.groupBox23.Controls.Add(this.label10);
            this.groupBox23.Controls.Add(this.RATE_YAW_IMAX);
            this.groupBox23.Controls.Add(this.label47);
            this.groupBox23.Controls.Add(this.RATE_YAW_I);
            this.groupBox23.Controls.Add(this.label77);
            this.groupBox23.Controls.Add(this.RATE_YAW_P);
            this.groupBox23.Controls.Add(this.label82);
            resources.ApplyResources(this.groupBox23, "groupBox23");
            this.groupBox23.Name = "groupBox23";
            this.groupBox23.TabStop = false;
            // 
            // RATE_YAW_D
            // 
            resources.ApplyResources(this.RATE_YAW_D, "RATE_YAW_D");
            this.RATE_YAW_D.Max = 1F;
            this.RATE_YAW_D.Min = 0F;
            this.RATE_YAW_D.Name = "RATE_YAW_D";
            this.RATE_YAW_D.ParamName = null;
            this.RATE_YAW_D.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // RATE_YAW_IMAX
            // 
            resources.ApplyResources(this.RATE_YAW_IMAX, "RATE_YAW_IMAX");
            this.RATE_YAW_IMAX.Max = 1F;
            this.RATE_YAW_IMAX.Min = 0F;
            this.RATE_YAW_IMAX.Name = "RATE_YAW_IMAX";
            this.RATE_YAW_IMAX.ParamName = null;
            this.RATE_YAW_IMAX.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label47
            // 
            resources.ApplyResources(this.label47, "label47");
            this.label47.Name = "label47";
            // 
            // RATE_YAW_I
            // 
            resources.ApplyResources(this.RATE_YAW_I, "RATE_YAW_I");
            this.RATE_YAW_I.Max = 1F;
            this.RATE_YAW_I.Min = 0F;
            this.RATE_YAW_I.Name = "RATE_YAW_I";
            this.RATE_YAW_I.ParamName = null;
            this.RATE_YAW_I.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label77
            // 
            resources.ApplyResources(this.label77, "label77");
            this.label77.Name = "label77";
            // 
            // RATE_YAW_P
            // 
            resources.ApplyResources(this.RATE_YAW_P, "RATE_YAW_P");
            this.RATE_YAW_P.Max = 1F;
            this.RATE_YAW_P.Min = 0F;
            this.RATE_YAW_P.Name = "RATE_YAW_P";
            this.RATE_YAW_P.ParamName = null;
            this.RATE_YAW_P.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label82
            // 
            resources.ApplyResources(this.label82, "label82");
            this.label82.Name = "label82";
            // 
            // lb_acc
            // 
            resources.ApplyResources(this.lb_acc, "lb_acc");
            this.lb_acc.Name = "lb_acc";
            // 
            // label18
            // 
            resources.ApplyResources(this.label18, "label18");
            this.label18.Name = "label18";
            // 
            // groupBox24
            // 
            this.groupBox24.Controls.Add(this.mavlinkNumericUpDown15);
            this.groupBox24.Controls.Add(this.RATE_PIT_D);
            this.groupBox24.Controls.Add(this.label11);
            this.groupBox24.Controls.Add(this.label38);
            this.groupBox24.Controls.Add(this.RATE_PIT_IMAX);
            this.groupBox24.Controls.Add(this.label84);
            this.groupBox24.Controls.Add(this.RATE_PIT_I);
            this.groupBox24.Controls.Add(this.label86);
            this.groupBox24.Controls.Add(this.RATE_PIT_P);
            this.groupBox24.Controls.Add(this.label87);
            resources.ApplyResources(this.groupBox24, "groupBox24");
            this.groupBox24.Name = "groupBox24";
            this.groupBox24.TabStop = false;
            // 
            // mavlinkNumericUpDown15
            // 
            resources.ApplyResources(this.mavlinkNumericUpDown15, "mavlinkNumericUpDown15");
            this.mavlinkNumericUpDown15.Max = 1F;
            this.mavlinkNumericUpDown15.Min = 0F;
            this.mavlinkNumericUpDown15.Name = "mavlinkNumericUpDown15";
            this.mavlinkNumericUpDown15.ParamName = null;
            this.mavlinkNumericUpDown15.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // RATE_PIT_D
            // 
            resources.ApplyResources(this.RATE_PIT_D, "RATE_PIT_D");
            this.RATE_PIT_D.Max = 1F;
            this.RATE_PIT_D.Min = 0F;
            this.RATE_PIT_D.Name = "RATE_PIT_D";
            this.RATE_PIT_D.ParamName = null;
            this.RATE_PIT_D.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // label38
            // 
            resources.ApplyResources(this.label38, "label38");
            this.label38.Name = "label38";
            // 
            // RATE_PIT_IMAX
            // 
            resources.ApplyResources(this.RATE_PIT_IMAX, "RATE_PIT_IMAX");
            this.RATE_PIT_IMAX.Max = 1F;
            this.RATE_PIT_IMAX.Min = 0F;
            this.RATE_PIT_IMAX.Name = "RATE_PIT_IMAX";
            this.RATE_PIT_IMAX.ParamName = null;
            this.RATE_PIT_IMAX.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label84
            // 
            resources.ApplyResources(this.label84, "label84");
            this.label84.Name = "label84";
            // 
            // RATE_PIT_I
            // 
            resources.ApplyResources(this.RATE_PIT_I, "RATE_PIT_I");
            this.RATE_PIT_I.Max = 1F;
            this.RATE_PIT_I.Min = 0F;
            this.RATE_PIT_I.Name = "RATE_PIT_I";
            this.RATE_PIT_I.ParamName = null;
            this.RATE_PIT_I.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label86
            // 
            resources.ApplyResources(this.label86, "label86");
            this.label86.Name = "label86";
            // 
            // RATE_PIT_P
            // 
            resources.ApplyResources(this.RATE_PIT_P, "RATE_PIT_P");
            this.RATE_PIT_P.Max = 1F;
            this.RATE_PIT_P.Min = 0F;
            this.RATE_PIT_P.Name = "RATE_PIT_P";
            this.RATE_PIT_P.ParamName = null;
            this.RATE_PIT_P.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label87
            // 
            resources.ApplyResources(this.label87, "label87");
            this.label87.Name = "label87";
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // groupBox25
            // 
            this.groupBox25.Controls.Add(this.mavlinkNumericUpDown14);
            this.groupBox25.Controls.Add(this.RATE_RLL_D);
            this.groupBox25.Controls.Add(this.label17);
            this.groupBox25.Controls.Add(this.label37);
            this.groupBox25.Controls.Add(this.RATE_RLL_IMAX);
            this.groupBox25.Controls.Add(this.label88);
            this.groupBox25.Controls.Add(this.RATE_RLL_I);
            this.groupBox25.Controls.Add(this.label90);
            this.groupBox25.Controls.Add(this.RATE_RLL_P);
            this.groupBox25.Controls.Add(this.label91);
            resources.ApplyResources(this.groupBox25, "groupBox25");
            this.groupBox25.Name = "groupBox25";
            this.groupBox25.TabStop = false;
            // 
            // mavlinkNumericUpDown14
            // 
            resources.ApplyResources(this.mavlinkNumericUpDown14, "mavlinkNumericUpDown14");
            this.mavlinkNumericUpDown14.Max = 1F;
            this.mavlinkNumericUpDown14.Min = 0F;
            this.mavlinkNumericUpDown14.Name = "mavlinkNumericUpDown14";
            this.mavlinkNumericUpDown14.ParamName = null;
            this.mavlinkNumericUpDown14.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // RATE_RLL_D
            // 
            resources.ApplyResources(this.RATE_RLL_D, "RATE_RLL_D");
            this.RATE_RLL_D.Max = 1F;
            this.RATE_RLL_D.Min = 0F;
            this.RATE_RLL_D.Name = "RATE_RLL_D";
            this.RATE_RLL_D.ParamName = null;
            this.RATE_RLL_D.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.label17.Name = "label17";
            // 
            // label37
            // 
            resources.ApplyResources(this.label37, "label37");
            this.label37.Name = "label37";
            // 
            // RATE_RLL_IMAX
            // 
            resources.ApplyResources(this.RATE_RLL_IMAX, "RATE_RLL_IMAX");
            this.RATE_RLL_IMAX.Max = 1F;
            this.RATE_RLL_IMAX.Min = 0F;
            this.RATE_RLL_IMAX.Name = "RATE_RLL_IMAX";
            this.RATE_RLL_IMAX.ParamName = null;
            this.RATE_RLL_IMAX.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label88
            // 
            resources.ApplyResources(this.label88, "label88");
            this.label88.Name = "label88";
            // 
            // RATE_RLL_I
            // 
            resources.ApplyResources(this.RATE_RLL_I, "RATE_RLL_I");
            this.RATE_RLL_I.Max = 1F;
            this.RATE_RLL_I.Min = 0F;
            this.RATE_RLL_I.Name = "RATE_RLL_I";
            this.RATE_RLL_I.ParamName = null;
            this.RATE_RLL_I.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label90
            // 
            resources.ApplyResources(this.label90, "label90");
            this.label90.Name = "label90";
            // 
            // RATE_RLL_P
            // 
            resources.ApplyResources(this.RATE_RLL_P, "RATE_RLL_P");
            this.RATE_RLL_P.Max = 1F;
            this.RATE_RLL_P.Min = 0F;
            this.RATE_RLL_P.Name = "RATE_RLL_P";
            this.RATE_RLL_P.ParamName = null;
            this.RATE_RLL_P.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label91
            // 
            resources.ApplyResources(this.label91, "label91");
            this.label91.Name = "label91";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 20000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 100;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.mavlinkNumericUpDown1);
            this.groupBox1.Controls.Add(this.LOITER_LAT_D);
            this.groupBox1.Controls.Add(this.LOITER_LAT_IMAX);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.LOITER_LAT_I);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.LOITER_LAT_P);
            this.groupBox1.Controls.Add(this.label4);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // mavlinkNumericUpDown1
            // 
            resources.ApplyResources(this.mavlinkNumericUpDown1, "mavlinkNumericUpDown1");
            this.mavlinkNumericUpDown1.Max = 1F;
            this.mavlinkNumericUpDown1.Min = 0F;
            this.mavlinkNumericUpDown1.Name = "mavlinkNumericUpDown1";
            this.mavlinkNumericUpDown1.ParamName = null;
            this.mavlinkNumericUpDown1.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // LOITER_LAT_D
            // 
            resources.ApplyResources(this.LOITER_LAT_D, "LOITER_LAT_D");
            this.LOITER_LAT_D.Max = 1F;
            this.LOITER_LAT_D.Min = 0F;
            this.LOITER_LAT_D.Name = "LOITER_LAT_D";
            this.LOITER_LAT_D.ParamName = null;
            this.LOITER_LAT_D.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // LOITER_LAT_IMAX
            // 
            resources.ApplyResources(this.LOITER_LAT_IMAX, "LOITER_LAT_IMAX");
            this.LOITER_LAT_IMAX.Max = 1F;
            this.LOITER_LAT_IMAX.Min = 0F;
            this.LOITER_LAT_IMAX.Name = "LOITER_LAT_IMAX";
            this.LOITER_LAT_IMAX.ParamName = null;
            this.LOITER_LAT_IMAX.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label19
            // 
            resources.ApplyResources(this.label19, "label19");
            this.label19.Name = "label19";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // LOITER_LAT_I
            // 
            resources.ApplyResources(this.LOITER_LAT_I, "LOITER_LAT_I");
            this.LOITER_LAT_I.Max = 1F;
            this.LOITER_LAT_I.Min = 0F;
            this.LOITER_LAT_I.Name = "LOITER_LAT_I";
            this.LOITER_LAT_I.ParamName = null;
            this.LOITER_LAT_I.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // LOITER_LAT_P
            // 
            resources.ApplyResources(this.LOITER_LAT_P, "LOITER_LAT_P");
            this.LOITER_LAT_P.Max = 1F;
            this.LOITER_LAT_P.Min = 0F;
            this.LOITER_LAT_P.Name = "LOITER_LAT_P";
            this.LOITER_LAT_P.ParamName = null;
            this.LOITER_LAT_P.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // BUT_rerequestparams
            // 
            resources.ApplyResources(this.BUT_rerequestparams, "BUT_rerequestparams");
            this.BUT_rerequestparams.Name = "BUT_rerequestparams";
            this.BUT_rerequestparams.UseVisualStyleBackColor = true;
            this.BUT_rerequestparams.Click += new System.EventHandler(this.BUT_rerequestparams_Click);
            // 
            // BUT_writePIDS
            // 
            resources.ApplyResources(this.BUT_writePIDS, "BUT_writePIDS");
            this.BUT_writePIDS.Name = "BUT_writePIDS";
            this.BUT_writePIDS.UseVisualStyleBackColor = true;
            this.BUT_writePIDS.Click += new System.EventHandler(this.BUT_writePIDS_Click);
            // 
            // myLabel3
            // 
            resources.ApplyResources(this.myLabel3, "myLabel3");
            this.myLabel3.Name = "myLabel3";
            this.myLabel3.resize = false;
            // 
            // myLabel2
            // 
            resources.ApplyResources(this.myLabel2, "myLabel2");
            this.myLabel2.Name = "myLabel2";
            this.myLabel2.resize = false;
            // 
            // myLabel1
            // 
            resources.ApplyResources(this.myLabel1, "myLabel1");
            this.myLabel1.Name = "myLabel1";
            this.myLabel1.resize = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.mavlinkNumericUpDown4);
            this.groupBox2.Controls.Add(this.THR_ACCEL_D);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label23);
            this.groupBox2.Controls.Add(this.THR_ACCEL_IMAX);
            this.groupBox2.Controls.Add(this.THR_ACCEL_I);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.THR_ACCEL_P);
            this.groupBox2.Controls.Add(this.label8);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // mavlinkNumericUpDown4
            // 
            resources.ApplyResources(this.mavlinkNumericUpDown4, "mavlinkNumericUpDown4");
            this.mavlinkNumericUpDown4.Max = 1F;
            this.mavlinkNumericUpDown4.Min = 0F;
            this.mavlinkNumericUpDown4.Name = "mavlinkNumericUpDown4";
            this.mavlinkNumericUpDown4.ParamName = null;
            this.mavlinkNumericUpDown4.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // THR_ACCEL_D
            // 
            resources.ApplyResources(this.THR_ACCEL_D, "THR_ACCEL_D");
            this.THR_ACCEL_D.Max = 1F;
            this.THR_ACCEL_D.Min = 0F;
            this.THR_ACCEL_D.Name = "THR_ACCEL_D";
            this.THR_ACCEL_D.ParamName = null;
            this.THR_ACCEL_D.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label23
            // 
            resources.ApplyResources(this.label23, "label23");
            this.label23.Name = "label23";
            // 
            // THR_ACCEL_IMAX
            // 
            resources.ApplyResources(this.THR_ACCEL_IMAX, "THR_ACCEL_IMAX");
            this.THR_ACCEL_IMAX.Max = 1F;
            this.THR_ACCEL_IMAX.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.THR_ACCEL_IMAX.Min = 0F;
            this.THR_ACCEL_IMAX.Name = "THR_ACCEL_IMAX";
            this.THR_ACCEL_IMAX.ParamName = null;
            this.THR_ACCEL_IMAX.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // THR_ACCEL_I
            // 
            resources.ApplyResources(this.THR_ACCEL_I, "THR_ACCEL_I");
            this.THR_ACCEL_I.Max = 1F;
            this.THR_ACCEL_I.Min = 0F;
            this.THR_ACCEL_I.Name = "THR_ACCEL_I";
            this.THR_ACCEL_I.ParamName = null;
            this.THR_ACCEL_I.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // THR_ACCEL_P
            // 
            resources.ApplyResources(this.THR_ACCEL_P, "THR_ACCEL_P");
            this.THR_ACCEL_P.Max = 1F;
            this.THR_ACCEL_P.Min = 0F;
            this.THR_ACCEL_P.Name = "THR_ACCEL_P";
            this.THR_ACCEL_P.ParamName = null;
            this.THR_ACCEL_P.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // BUT_refreshpart
            // 
            resources.ApplyResources(this.BUT_refreshpart, "BUT_refreshpart");
            this.BUT_refreshpart.Name = "BUT_refreshpart";
            this.BUT_refreshpart.UseVisualStyleBackColor = true;
            this.BUT_refreshpart.Click += new System.EventHandler(this.BUT_refreshpart_Click);
            // 
            // myLabel4
            // 
            resources.ApplyResources(this.myLabel4, "myLabel4");
            this.myLabel4.Name = "myLabel4";
            this.myLabel4.resize = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox1);
            this.groupBox3.Controls.Add(this.groupBox19);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.groupBox22);
            this.groupBox6.Controls.Add(this.groupBox21);
            this.groupBox6.Controls.Add(this.groupBox20);
            resources.ApplyResources(this.groupBox6, "groupBox6");
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.TabStop = false;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.myLabel2);
            this.groupBox8.Controls.Add(this.CH7_OPT);
            this.groupBox8.Controls.Add(this.myLabel1);
            this.groupBox8.Controls.Add(this.myLabel4);
            this.groupBox8.Controls.Add(this.TUNE);
            this.groupBox8.Controls.Add(this.CH8_OPT);
            this.groupBox8.Controls.Add(this.TUNE_HIGH);
            this.groupBox8.Controls.Add(this.TUNE_LOW);
            this.groupBox8.Controls.Add(this.myLabel5);
            this.groupBox8.Controls.Add(this.myLabel3);
            resources.ApplyResources(this.groupBox8, "groupBox8");
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.TabStop = false;
            // 
            // CH7_OPT
            // 
            this.CH7_OPT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CH7_OPT.DropDownWidth = 170;
            resources.ApplyResources(this.CH7_OPT, "CH7_OPT");
            this.CH7_OPT.FormattingEnabled = true;
            this.CH7_OPT.Name = "CH7_OPT";
            this.CH7_OPT.ParamName = null;
            this.CH7_OPT.SubControl = null;
            this.CH7_OPT.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // TUNE
            // 
            this.TUNE.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TUNE.DropDownWidth = 170;
            resources.ApplyResources(this.TUNE, "TUNE");
            this.TUNE.FormattingEnabled = true;
            this.TUNE.Name = "TUNE";
            this.TUNE.ParamName = null;
            this.TUNE.SubControl = null;
            this.TUNE.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // CH8_OPT
            // 
            this.CH8_OPT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CH8_OPT.DropDownWidth = 170;
            resources.ApplyResources(this.CH8_OPT, "CH8_OPT");
            this.CH8_OPT.FormattingEnabled = true;
            this.CH8_OPT.Name = "CH8_OPT";
            this.CH8_OPT.ParamName = null;
            this.CH8_OPT.SubControl = null;
            this.CH8_OPT.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // TUNE_HIGH
            // 
            resources.ApplyResources(this.TUNE_HIGH, "TUNE_HIGH");
            this.TUNE_HIGH.Max = 1F;
            this.TUNE_HIGH.Min = 0F;
            this.TUNE_HIGH.Name = "TUNE_HIGH";
            this.TUNE_HIGH.ParamName = null;
            this.TUNE_HIGH.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // TUNE_LOW
            // 
            resources.ApplyResources(this.TUNE_LOW, "TUNE_LOW");
            this.TUNE_LOW.Max = 1F;
            this.TUNE_LOW.Min = 0F;
            this.TUNE_LOW.Name = "TUNE_LOW";
            this.TUNE_LOW.ParamName = null;
            this.TUNE_LOW.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // myLabel5
            // 
            resources.ApplyResources(this.myLabel5, "myLabel5");
            this.myLabel5.Name = "myLabel5";
            this.myLabel5.resize = false;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.groupBox7);
            this.groupBox9.Controls.Add(this.groupBox2);
            this.groupBox9.Controls.Add(this.groupBox5);
            resources.ApplyResources(this.groupBox9, "groupBox9");
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.TabStop = false;
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.RATE_YAW_FILT);
            this.groupBox10.Controls.Add(this.RATE_PIT_FILT);
            this.groupBox10.Controls.Add(this.label18);
            this.groupBox10.Controls.Add(this.RATE_RLL_FILT);
            this.groupBox10.Controls.Add(this.label14);
            this.groupBox10.Controls.Add(this.label12);
            this.groupBox10.Controls.Add(this.groupBox24);
            this.groupBox10.Controls.Add(this.groupBox25);
            this.groupBox10.Controls.Add(this.groupBox23);
            resources.ApplyResources(this.groupBox10, "groupBox10");
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.TabStop = false;
            // 
            // RATE_YAW_FILT
            // 
            resources.ApplyResources(this.RATE_YAW_FILT, "RATE_YAW_FILT");
            this.RATE_YAW_FILT.Max = 1F;
            this.RATE_YAW_FILT.Min = 0F;
            this.RATE_YAW_FILT.Name = "RATE_YAW_FILT";
            this.RATE_YAW_FILT.ParamName = null;
            this.RATE_YAW_FILT.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // RATE_PIT_FILT
            // 
            resources.ApplyResources(this.RATE_PIT_FILT, "RATE_PIT_FILT");
            this.RATE_PIT_FILT.Max = 1F;
            this.RATE_PIT_FILT.Min = 0F;
            this.RATE_PIT_FILT.Name = "RATE_PIT_FILT";
            this.RATE_PIT_FILT.ParamName = null;
            this.RATE_PIT_FILT.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // RATE_RLL_FILT
            // 
            resources.ApplyResources(this.RATE_RLL_FILT, "RATE_RLL_FILT");
            this.RATE_RLL_FILT.Max = 1F;
            this.RATE_RLL_FILT.Min = 0F;
            this.RATE_RLL_FILT.Name = "RATE_RLL_FILT";
            this.RATE_RLL_FILT.ParamName = null;
            this.RATE_RLL_FILT.ValueUpdated += new System.EventHandler(this.numeric_ValueUpdated);
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.cb_pwm_log_rate);
            this.groupBox11.Controls.Add(this.cb_log_accZ_rate);
            this.groupBox11.Controls.Add(this.cb_log_pos_rate);
            this.groupBox11.Controls.Add(this.cb_log_angrate_rate);
            this.groupBox11.Controls.Add(this.cb_log_rcout);
            this.groupBox11.Controls.Add(this.cb_log_accz);
            this.groupBox11.Controls.Add(this.cb_log_pos);
            this.groupBox11.Controls.Add(this.cb_log_angrate);
            resources.ApplyResources(this.groupBox11, "groupBox11");
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.TabStop = false;
            // 
            // cb_pwm_log_rate
            // 
            this.cb_pwm_log_rate.FormattingEnabled = true;
            this.cb_pwm_log_rate.Items.AddRange(new object[] {
            resources.GetString("cb_pwm_log_rate.Items"),
            resources.GetString("cb_pwm_log_rate.Items1"),
            resources.GetString("cb_pwm_log_rate.Items2")});
            resources.ApplyResources(this.cb_pwm_log_rate, "cb_pwm_log_rate");
            this.cb_pwm_log_rate.Name = "cb_pwm_log_rate";
            // 
            // cb_log_accZ_rate
            // 
            this.cb_log_accZ_rate.FormattingEnabled = true;
            this.cb_log_accZ_rate.Items.AddRange(new object[] {
            resources.GetString("cb_log_accZ_rate.Items"),
            resources.GetString("cb_log_accZ_rate.Items1"),
            resources.GetString("cb_log_accZ_rate.Items2")});
            resources.ApplyResources(this.cb_log_accZ_rate, "cb_log_accZ_rate");
            this.cb_log_accZ_rate.Name = "cb_log_accZ_rate";
            // 
            // cb_log_pos_rate
            // 
            this.cb_log_pos_rate.FormattingEnabled = true;
            this.cb_log_pos_rate.Items.AddRange(new object[] {
            resources.GetString("cb_log_pos_rate.Items"),
            resources.GetString("cb_log_pos_rate.Items1")});
            resources.ApplyResources(this.cb_log_pos_rate, "cb_log_pos_rate");
            this.cb_log_pos_rate.Name = "cb_log_pos_rate";
            // 
            // cb_log_angrate_rate
            // 
            this.cb_log_angrate_rate.FormattingEnabled = true;
            this.cb_log_angrate_rate.Items.AddRange(new object[] {
            resources.GetString("cb_log_angrate_rate.Items"),
            resources.GetString("cb_log_angrate_rate.Items1"),
            resources.GetString("cb_log_angrate_rate.Items2")});
            resources.ApplyResources(this.cb_log_angrate_rate, "cb_log_angrate_rate");
            this.cb_log_angrate_rate.Name = "cb_log_angrate_rate";
            // 
            // cb_log_rcout
            // 
            resources.ApplyResources(this.cb_log_rcout, "cb_log_rcout");
            this.cb_log_rcout.Name = "cb_log_rcout";
            this.cb_log_rcout.UseVisualStyleBackColor = true;
            this.cb_log_rcout.CheckedChanged += new System.EventHandler(this.cb_log_rcout_CheckedChanged);
            // 
            // cb_log_accz
            // 
            resources.ApplyResources(this.cb_log_accz, "cb_log_accz");
            this.cb_log_accz.Name = "cb_log_accz";
            this.cb_log_accz.UseVisualStyleBackColor = true;
            this.cb_log_accz.CheckedChanged += new System.EventHandler(this.cb_log_accz_CheckedChanged);
            // 
            // cb_log_pos
            // 
            resources.ApplyResources(this.cb_log_pos, "cb_log_pos");
            this.cb_log_pos.Name = "cb_log_pos";
            this.cb_log_pos.UseVisualStyleBackColor = true;
            this.cb_log_pos.CheckedChanged += new System.EventHandler(this.cb_log_pos_CheckedChanged);
            // 
            // cb_log_angrate
            // 
            resources.ApplyResources(this.cb_log_angrate, "cb_log_angrate");
            this.cb_log_angrate.Name = "cb_log_angrate";
            this.cb_log_angrate.UseVisualStyleBackColor = true;
            this.cb_log_angrate.CheckedChanged += new System.EventHandler(this.cb_log_angrate_CheckedChanged);
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.num_gyr_Hz);
            this.groupBox12.Controls.Add(this.num_acc_Hz);
            this.groupBox12.Controls.Add(this.radioButton1);
            this.groupBox12.Controls.Add(this.rb_8KHz);
            this.groupBox12.Controls.Add(this.rb_1KHz);
            this.groupBox12.Controls.Add(this.label39);
            this.groupBox12.Controls.Add(this.lb_acc);
            this.groupBox12.Controls.Add(this.cb_8KHz_ft);
            this.groupBox12.Controls.Add(this.cb_1KHz_ft);
            resources.ApplyResources(this.groupBox12, "groupBox12");
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.TabStop = false;
            // 
            // num_gyr_Hz
            // 
            resources.ApplyResources(this.num_gyr_Hz, "num_gyr_Hz");
            this.num_gyr_Hz.Maximum = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.num_gyr_Hz.Name = "num_gyr_Hz";
            this.num_gyr_Hz.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // num_acc_Hz
            // 
            resources.ApplyResources(this.num_acc_Hz, "num_acc_Hz");
            this.num_acc_Hz.Maximum = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.num_acc_Hz.Name = "num_acc_Hz";
            this.num_acc_Hz.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // radioButton1
            // 
            resources.ApplyResources(this.radioButton1, "radioButton1");
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.TabStop = true;
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.rb_8KHz_CheckedChanged);
            // 
            // rb_8KHz
            // 
            resources.ApplyResources(this.rb_8KHz, "rb_8KHz");
            this.rb_8KHz.Name = "rb_8KHz";
            this.rb_8KHz.TabStop = true;
            this.rb_8KHz.UseVisualStyleBackColor = true;
            this.rb_8KHz.CheckedChanged += new System.EventHandler(this.rb_8KHz_CheckedChanged);
            // 
            // rb_1KHz
            // 
            resources.ApplyResources(this.rb_1KHz, "rb_1KHz");
            this.rb_1KHz.Name = "rb_1KHz";
            this.rb_1KHz.TabStop = true;
            this.rb_1KHz.UseVisualStyleBackColor = true;
            this.rb_1KHz.CheckedChanged += new System.EventHandler(this.rb_1KHz_CheckedChanged);
            // 
            // label39
            // 
            resources.ApplyResources(this.label39, "label39");
            this.label39.Name = "label39";
            // 
            // cb_8KHz_ft
            // 
            this.cb_8KHz_ft.FormattingEnabled = true;
            this.cb_8KHz_ft.Items.AddRange(new object[] {
            resources.GetString("cb_8KHz_ft.Items"),
            resources.GetString("cb_8KHz_ft.Items1"),
            resources.GetString("cb_8KHz_ft.Items2")});
            resources.ApplyResources(this.cb_8KHz_ft, "cb_8KHz_ft");
            this.cb_8KHz_ft.Name = "cb_8KHz_ft";
            // 
            // cb_1KHz_ft
            // 
            this.cb_1KHz_ft.FormattingEnabled = true;
            this.cb_1KHz_ft.Items.AddRange(new object[] {
            resources.GetString("cb_1KHz_ft.Items"),
            resources.GetString("cb_1KHz_ft.Items1")});
            resources.ApplyResources(this.cb_1KHz_ft, "cb_1KHz_ft");
            this.cb_1KHz_ft.Name = "cb_1KHz_ft";
            // 
            // groupBox13
            // 
            this.groupBox13.Controls.Add(this.btn_write_angle_max);
            this.groupBox13.Controls.Add(this.num_max_angle);
            this.groupBox13.Controls.Add(this.label48);
            resources.ApplyResources(this.groupBox13, "groupBox13");
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.TabStop = false;
            // 
            // btn_write_angle_max
            // 
            resources.ApplyResources(this.btn_write_angle_max, "btn_write_angle_max");
            this.btn_write_angle_max.Name = "btn_write_angle_max";
            this.btn_write_angle_max.UseVisualStyleBackColor = true;
            this.btn_write_angle_max.Click += new System.EventHandler(this.btn_write_angle_max_Click);
            // 
            // num_max_angle
            // 
            resources.ApplyResources(this.num_max_angle, "num_max_angle");
            this.num_max_angle.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.num_max_angle.Minimum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.num_max_angle.Name = "num_max_angle";
            this.num_max_angle.Value = new decimal(new int[] {
            18,
            0,
            0,
            0});
            // 
            // label48
            // 
            resources.ApplyResources(this.label48, "label48");
            this.label48.Name = "label48";
            // 
            // ConfigArducopter
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.groupBox10);
            this.Controls.Add(this.groupBox9);
            this.Controls.Add(this.groupBox13);
            this.Controls.Add(this.groupBox12);
            this.Controls.Add(this.groupBox11);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.BUT_refreshpart);
            this.Controls.Add(this.CHK_lockrollpitch);
            this.Controls.Add(this.BUT_rerequestparams);
            this.Controls.Add(this.BUT_writePIDS);
            this.Controls.Add(this.groupBox4);
            this.Name = "ConfigArducopter";
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mavlinkNumericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.THR_RATE_P)).EndInit();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.WPNAV_SPEED_UP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WPNAV_LOIT_SPEED)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WPNAV_SPEED_DN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WPNAV_RADIUS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WPNAV_SPEED)).EndInit();
            this.groupBox7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.THR_ALT_P)).EndInit();
            this.groupBox19.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PosXY_D)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PosXY_I)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PosXY_IMAX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HLD_LAT_P)).EndInit();
            this.groupBox20.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mavlinkNumericUpDown13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.STB_YAW_D)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.STB_YAW_IMAX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.STB_YAW_I)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.STB_YAW_P)).EndInit();
            this.groupBox21.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mavlinkNumericUpDown10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.STB_PIT_D)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.STB_PIT_IMAX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.STB_PIT_I)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.STB_PIT_P)).EndInit();
            this.groupBox22.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mavlinkNumericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.STB_RLL_D)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.STB_RLL_IMAX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.STB_RLL_I)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.STB_RLL_P)).EndInit();
            this.groupBox23.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.RATE_YAW_D)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RATE_YAW_IMAX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RATE_YAW_I)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RATE_YAW_P)).EndInit();
            this.groupBox24.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mavlinkNumericUpDown15)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RATE_PIT_D)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RATE_PIT_IMAX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RATE_PIT_I)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RATE_PIT_P)).EndInit();
            this.groupBox25.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mavlinkNumericUpDown14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RATE_RLL_D)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RATE_RLL_IMAX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RATE_RLL_I)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RATE_RLL_P)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mavlinkNumericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LOITER_LAT_D)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LOITER_LAT_IMAX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LOITER_LAT_I)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LOITER_LAT_P)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mavlinkNumericUpDown4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.THR_ACCEL_D)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.THR_ACCEL_IMAX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.THR_ACCEL_I)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.THR_ACCEL_P)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TUNE_HIGH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TUNE_LOW)).EndInit();
            this.groupBox9.ResumeLayout(false);
            this.groupBox10.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.RATE_YAW_FILT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RATE_PIT_FILT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RATE_RLL_FILT)).EndInit();
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_gyr_Hz)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_acc_Hz)).EndInit();
            this.groupBox13.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.num_max_angle)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

  

        #endregion

        private Controls.MyLabel myLabel3;
        private Controls.MavlinkNumericUpDown TUNE_LOW;
        private Controls.MavlinkNumericUpDown TUNE_HIGH;
        private Controls.MyLabel myLabel2;
        private Controls.MavlinkComboBox  TUNE;
        private Controls.MyLabel myLabel1;
        private Controls.MavlinkComboBox CH7_OPT;
        private System.Windows.Forms.GroupBox groupBox5;
        private Controls.MavlinkNumericUpDown THR_RATE_P;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.CheckBox CHK_lockrollpitch;
        private System.Windows.Forms.GroupBox groupBox4;
        private Controls.MavlinkNumericUpDown WPNAV_SPEED_UP;
        private System.Windows.Forms.Label label27;
        private Controls.MavlinkNumericUpDown WPNAV_LOIT_SPEED;
        private System.Windows.Forms.Label label9;
        private Controls.MavlinkNumericUpDown WPNAV_SPEED_DN;
        private System.Windows.Forms.Label label13;
        private Controls.MavlinkNumericUpDown WPNAV_RADIUS;
        private System.Windows.Forms.Label label15;
        private Controls.MavlinkNumericUpDown WPNAV_SPEED;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.GroupBox groupBox7;
        private Controls.MavlinkNumericUpDown THR_ALT_P;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.GroupBox groupBox19;
        private Controls.MavlinkNumericUpDown HLD_LAT_P;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.GroupBox groupBox20;
        private Controls.MavlinkNumericUpDown STB_YAW_P;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.GroupBox groupBox21;
        private Controls.MavlinkNumericUpDown STB_PIT_P;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.GroupBox groupBox22;
        private Controls.MavlinkNumericUpDown STB_RLL_P;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.GroupBox groupBox23;
        private Controls.MavlinkNumericUpDown RATE_YAW_D;
        private System.Windows.Forms.Label label10;
        private Controls.MavlinkNumericUpDown RATE_YAW_IMAX;
        private System.Windows.Forms.Label label47;
        private Controls.MavlinkNumericUpDown RATE_YAW_I;
        private System.Windows.Forms.Label label77;
        private Controls.MavlinkNumericUpDown RATE_YAW_P;
        private System.Windows.Forms.Label label82;
        private System.Windows.Forms.GroupBox groupBox24;
        private Controls.MavlinkNumericUpDown RATE_PIT_D;
        private System.Windows.Forms.Label label11;
        private Controls.MavlinkNumericUpDown RATE_PIT_IMAX;
        private System.Windows.Forms.Label label84;
        private Controls.MavlinkNumericUpDown RATE_PIT_I;
        private System.Windows.Forms.Label label86;
        private Controls.MavlinkNumericUpDown RATE_PIT_P;
        private System.Windows.Forms.Label label87;
        private System.Windows.Forms.GroupBox groupBox25;
        private Controls.MavlinkNumericUpDown RATE_RLL_D;
        private System.Windows.Forms.Label label17;
        private Controls.MavlinkNumericUpDown RATE_RLL_IMAX;
        private System.Windows.Forms.Label label88;
        private Controls.MavlinkNumericUpDown RATE_RLL_I;
        private System.Windows.Forms.Label label90;
        private Controls.MavlinkNumericUpDown RATE_RLL_P;
        private System.Windows.Forms.Label label91;
        private System.Windows.Forms.ToolTip toolTip1;
        private Controls.MyButton BUT_writePIDS;
        private Controls.MyButton BUT_rerequestparams;
        private System.Windows.Forms.GroupBox groupBox1;
        private Controls.MavlinkNumericUpDown LOITER_LAT_D;
        private System.Windows.Forms.Label label1;
        private Controls.MavlinkNumericUpDown LOITER_LAT_IMAX;
        private System.Windows.Forms.Label label2;
        private Controls.MavlinkNumericUpDown LOITER_LAT_I;
        private System.Windows.Forms.Label label3;
        private Controls.MavlinkNumericUpDown LOITER_LAT_P;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        private Controls.MavlinkNumericUpDown THR_ACCEL_D;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private Controls.MavlinkNumericUpDown THR_ACCEL_IMAX;
        private Controls.MavlinkNumericUpDown THR_ACCEL_I;
        private System.Windows.Forms.Label label7;
        private Controls.MavlinkNumericUpDown THR_ACCEL_P;
        private System.Windows.Forms.Label label8;
        private Controls.MyButton BUT_refreshpart;
        private Controls.MyLabel myLabel4;
        private Controls.MavlinkComboBox CH8_OPT;
        private Controls.MavlinkNumericUpDown RATE_YAW_FILT;
        private System.Windows.Forms.Label label18;
        private Controls.MavlinkNumericUpDown RATE_PIT_FILT;
        private System.Windows.Forms.Label label14;
        private Controls.MavlinkNumericUpDown RATE_RLL_FILT;
        private System.Windows.Forms.Label label12;
        private GroupBox groupBox3;
        private GroupBox groupBox6;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private GroupBox groupBox8;
        private GroupBox groupBox9;
        private Controls.MyLabel myLabel5;
        private Controls.MavlinkNumericUpDown mavlinkNumericUpDown1;
        private Label label19;
        private Controls.MavlinkNumericUpDown PosXY_IMAX;
        private Label label20;
        private Controls.MavlinkNumericUpDown mavlinkNumericUpDown3;
        private Label label21;
        private Controls.MavlinkNumericUpDown mavlinkNumericUpDown4;
        private Label label23;
        private Controls.MavlinkNumericUpDown STB_RLL_D;
        private Controls.MavlinkNumericUpDown STB_RLL_IMAX;
        private Controls.MavlinkNumericUpDown STB_RLL_I;
        private Label label26;
        private Label label28;
        private Label label24;
        private Controls.MavlinkNumericUpDown mavlinkNumericUpDown10;
        private Controls.MavlinkNumericUpDown STB_PIT_D;
        private Label label30;
        private Controls.MavlinkNumericUpDown STB_PIT_I;
        private Label label32;
        private Label label29;
        private Controls.MavlinkNumericUpDown mavlinkNumericUpDown13;
        private Controls.MavlinkNumericUpDown STB_YAW_D;
        private Label label34;
        private Controls.MavlinkNumericUpDown STB_YAW_I;
        private Label label36;
        private Label label33;
        private GroupBox groupBox10;
        private Label lb_acc;
        private Controls.MavlinkNumericUpDown mavlinkNumericUpDown15;
        private Label label38;
        private Controls.MavlinkNumericUpDown mavlinkNumericUpDown14;
        private Label label37;
        private Controls.MavlinkNumericUpDown PosXY_I;
        private Label lbdd;
        private Controls.MavlinkNumericUpDown PosXY_D;
        private Label label40;
        private Controls.MavlinkNumericUpDown mavlinkNumericUpDown2;
        private Label label41;
        private Controls.MavlinkNumericUpDown STB_YAW_IMAX;
        private Label label44;
        private Controls.MavlinkNumericUpDown STB_PIT_IMAX;
        private Label label43;
        private GroupBox groupBox11;
        private CheckBox cb_log_rcout;
        private CheckBox cb_log_accz;
        private CheckBox cb_log_pos;
        private CheckBox cb_log_angrate;
        private ComboBox cb_log_accZ_rate;
        private ComboBox cb_log_pos_rate;
        private ComboBox cb_log_angrate_rate;
        private ComboBox cb_pwm_log_rate;
        private GroupBox groupBox12;
        private ComboBox cb_8KHz_ft;
        private ComboBox cb_1KHz_ft;
        private RadioButton rb_8KHz;
        private RadioButton rb_1KHz;
        private Label label39;
        private NumericUpDown num_gyr_Hz;
        private NumericUpDown num_acc_Hz;
        private RadioButton radioButton1;
        private GroupBox groupBox13;
        private NumericUpDown num_max_angle;
        private Label label48;
        private Button btn_write_angle_max;
    }
}
