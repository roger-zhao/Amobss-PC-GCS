

/// DB ZhaoYJ@2017-03-09
/// for: simplify the GCS components
/// TODO: make sure it's stable
/// start
#define SIMPLE_SETUP
/// end

using System;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using MissionPlanner.Controls;
using MissionPlanner.Controls.BackstageView;
using MissionPlanner.GCSViews.ConfigurationView;
using MissionPlanner.Utilities;
using System.Resources;

namespace MissionPlanner.GCSViews
{
    public partial class InitialSetup : MyUserControl, IActivate
    {
        internal static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string lastpagename = "";

        public InitialSetup()
        {
            InitializeComponent();
        }

        public bool isConnected
        {
            get { return MainV2.comPort.BaseStream.IsOpen; }
        }

        public bool isDisConnected
        {
            get { return !MainV2.comPort.BaseStream.IsOpen; }
        }

        public bool isTracker
        {
            get { return isConnected && MainV2.comPort.MAV.cs.firmware == MainV2.Firmwares.ArduTracker; }
        }

        public bool isCopter
        {
            get { return isConnected && MainV2.comPort.MAV.cs.firmware == MainV2.Firmwares.ArduCopter2; }
        }

        public bool isHeli
        {
            get { return isConnected && MainV2.comPort.MAV.aptype == MAVLink.MAV_TYPE.HELICOPTER; }
        }

        public bool isQuadPlane
        {
            get
            {
                return isConnected && isPlane &&
                       MainV2.comPort.MAV.param.ContainsKey("Q_ENABLE") &&
                       (MainV2.comPort.MAV.param["Q_ENABLE"].Value == 1.0);
            }
        }

        public bool isPlane
        {
            get
            {
                return isConnected &&
                       (MainV2.comPort.MAV.cs.firmware == MainV2.Firmwares.ArduPlane ||
                        MainV2.comPort.MAV.cs.firmware == MainV2.Firmwares.Ateryx);
            }
        }

        public bool isRover
        {
            get { return isConnected && MainV2.comPort.MAV.cs.firmware == MainV2.Firmwares.ArduRover; }
        }

        private BackstageViewPage AddBackstageViewPage(Type userControl, string headerText, bool enabled = true,
    BackstageViewPage Parent = null, bool advanced = false)
        {
            try
            {
                if (enabled)
                    return backstageView.AddPage(userControl, headerText, Parent, advanced);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }

            return null;
        }

        public void Activate()
        {
        }




        private void HardwareConfig_Load(object sender, EventArgs e)
        {
            ResourceManager rm = new ResourceManager(this.GetType());


#if SIMPLE_SETUP
            // AddBackstageViewPage(typeof(ConfigFirmwareDisabled), rm.GetString("backstageViewPagefw.Text"), isConnected);
            // var mand = AddBackstageViewPage(typeof(ConfigMandatory), rm.GetString("backstageViewPagemand.Text"), isConnected);
            /// Radio cali
            // AddBackstageViewPage(typeof(ConfigRadioInput), rm.GetString("backstageViewPageradio.Text"), isConnected);


            ConfigRadioInput conf_radioin = new ConfigRadioInput();
            conf_radioin.Dock = DockStyle.Fill;
            TabPage rc_tp = new TabPage();//Create new tabpage
            rc_tp.Controls.Add(conf_radioin);
            rc_tp.Text = "遥控校准";
            tabControl_Setup.TabPages.Add(rc_tp);
            tabControl_Setup.SelectedIndexChanged += new EventHandler(conf_radioin.OnFocus); ;

            /// ESC cali
            //Added by paul below
            // AddBackstageViewPage(typeof(ConfigESCCalibration), rm.GetString("backstageViewPageESC.Text"), isConnected);
            ConfigESCCalibration conf_escali = new ConfigESCCalibration();
            conf_escali.Dock = DockStyle.Fill;
            TabPage escali_tp = new TabPage();//Create new tabpage
            escali_tp.Controls.Add(conf_escali);
            escali_tp.Text = "电调校准";
            tabControl_Setup.TabPages.Add(escali_tp);
            tabControl_Setup.SelectedIndexChanged += new EventHandler(conf_escali.OnFocus); ;

            //Added by paul above

            /// Motor test
            // AddBackstageViewPage(typeof(ConfigMotorTest), rm.GetString("backstageViewPageMotorTest.Text"), isConnected);
            ConfigMotorTest conf_mtest = new ConfigMotorTest();
            conf_mtest.Dock = DockStyle.Fill;
            TabPage mtest_tp = new TabPage();//Create new tabpage
            mtest_tp.Controls.Add(conf_mtest);
            mtest_tp.Text = "电机测试";
            tabControl_Setup.TabPages.Add(mtest_tp);
            tabControl_Setup.SelectedIndexChanged += new EventHandler(conf_mtest.OnFocus); ;

            /// ACCEL cali 
            // AddBackstageViewPage(typeof(ConfigAccelerometerCalibration), rm.GetString("backstageViewPageaccel.Text"), isConnected);
            ConfigAccelerometerCalibration conf_accali = new ConfigAccelerometerCalibration();
            conf_accali.Dock = DockStyle.Fill;
            TabPage accali_tp = new TabPage();//Create new tabpage
            accali_tp.Controls.Add(conf_accali);
            accali_tp.Text = "IMU校准";
            tabControl_Setup.TabPages.Add(accali_tp);
            tabControl_Setup.SelectedIndexChanged += new EventHandler(conf_accali.OnFocus); ;

            /// Compass cali
            // AddBackstageViewPage(typeof(ConfigHWCompass), rm.GetString("backstageViewPagecompass.Text"), isConnected);
            ConfigHWCompass conf_magcali = new ConfigHWCompass();
            conf_magcali.Dock = DockStyle.Fill;
            TabPage magcali_tp = new TabPage();//Create new tabpage
            magcali_tp.Controls.Add(conf_magcali);
            magcali_tp.Text = "指南针校准";
            tabControl_Setup.TabPages.Add(magcali_tp);
            tabControl_Setup.SelectedIndexChanged += new EventHandler(conf_magcali.OnFocus); 

            ConfigFirmware conf_fw = new ConfigFirmware();
            conf_fw.Dock = DockStyle.Fill;
            TabPage fw_tp = new TabPage();//Create new tabpage
            fw_tp.Controls.Add(conf_fw);
            fw_tp.Text = "机型选择";
            tabControl_Setup.TabPages.Add(fw_tp);
            tabControl_Setup.SelectedIndexChanged += new EventHandler(conf_fw.OnFocus);
            /// config flmode
            // AddBackstageViewPage(typeof(ConfigFlightModes), rm.GetString("backstageViewPageflmode.Text"), isConnected);
            // fly hgt spd
            // AddBackstageViewPage(typeof(ConfigFlyHgtSpd), rm.GetString("backstageViewPageFlyHgtSpd.Text"), true);
            ConfigVoltCali conf_voltcali = new ConfigVoltCali();
            conf_voltcali.Dock = DockStyle.Fill;
            TabPage voltcali_tp = new TabPage();//Create new tabpage
            voltcali_tp.Controls.Add(conf_voltcali);
            voltcali_tp.Text = "电压校准";
            tabControl_Setup.TabPages.Add(voltcali_tp);
            tabControl_Setup.SelectedIndexChanged += new EventHandler(conf_voltcali.OnFocus);

#else
            AddBackstageViewPage(typeof(ConfigFirmwareDisabled), rm.GetString("backstageViewPagefw.Text"), isConnected);
            AddBackstageViewPage(typeof(ConfigFirmware), rm.GetString("backstageViewPagefw.Text"), isDisConnected);
            AddBackstageViewPage(typeof(ConfigWizard), rm.GetString("backstageViewPagewizard.Text"));
            var mand = AddBackstageViewPage(typeof(ConfigMandatory), rm.GetString("backstageViewPagemand.Text"), isConnected);
            AddBackstageViewPage(typeof(ConfigTradHeli), rm.GetString("backstageViewPagetradheli.Text"), isHeli, mand);
            AddBackstageViewPage(typeof(ConfigFrameType), rm.GetString("backstageViewPageframetype.Text"), false/*isCopter*/, mand);
            AddBackstageViewPage(typeof(ConfigAccelerometerCalibration), rm.GetString("backstageViewPageaccel.Text"), isConnected, mand);
            AddBackstageViewPage(typeof(ConfigHWCompass), rm.GetString("backstageViewPagecompass.Text"), isConnected, mand);
            AddBackstageViewPage(typeof(ConfigRadioInput), rm.GetString("backstageViewPageradio.Text"), isConnected, mand);
            //Added by paul below
            AddBackstageViewPage(typeof(ConfigESCCalibration), rm.GetString("backstageViewPageESC.Text"), isConnected, mand);
            //Added by paul above
            AddBackstageViewPage(typeof(ConfigFlightModes), rm.GetString("backstageViewPageflmode.Text"), isConnected, mand);
            AddBackstageViewPage(typeof(ConfigFailSafe), rm.GetString("backstageViewPagefs.Text"), isConnected, mand);

            var opt = AddBackstageViewPage(typeof(ConfigOptional), rm.GetString("backstageViewPageopt.Text"));
            AddBackstageViewPage(typeof(Sikradio), rm.GetString("backstageViewPageSikradio.Text"), true, opt);
            AddBackstageViewPage(typeof(ConfigBatteryMonitoring), rm.GetString("backstageViewPagebatmon.Text"), isConnected, opt);
            AddBackstageViewPage(typeof(ConfigBatteryMonitoring2), rm.GetString("backstageViewPageBatt2.Text"), isConnected, opt);
            AddBackstageViewPage(typeof(ConfigHWUAVCAN), "UAVCAN", isConnected, opt);            
            AddBackstageViewPage(typeof(ConfigCompassMot), rm.GetString("backstageViewPagecompassmot.Text"), isConnected, opt);
            AddBackstageViewPage(typeof(ConfigHWRangeFinder), rm.GetString("backstageViewPagesonar.Text"), isConnected, opt);
            AddBackstageViewPage(typeof(ConfigHWAirspeed), rm.GetString("backstageViewPageairspeed.Text"), isConnected, opt);
            AddBackstageViewPage(typeof(ConfigHWPX4Flow), rm.GetString("backstageViewPagePX4Flow.Text"), true, opt);
            AddBackstageViewPage(typeof(ConfigHWOptFlow), rm.GetString("backstageViewPageoptflow.Text"), isConnected, opt);
            AddBackstageViewPage(typeof(ConfigHWOSD), rm.GetString("backstageViewPageosd.Text"), isConnected, opt);
            AddBackstageViewPage(typeof(ConfigMount), rm.GetString("backstageViewPagegimbal.Text"), isConnected, opt);
            AddBackstageViewPage(typeof(ConfigAntennaTracker), rm.GetString("backstageViewPageAntTrack.Text"), isTracker, opt);
            AddBackstageViewPage(typeof(ConfigMotorTest), rm.GetString("backstageViewPageMotorTest.Text"), isConnected, opt);
            AddBackstageViewPage(typeof(ConfigHWBT), rm.GetString("backstageViewPagehwbt.Text"), true, opt);
            AddBackstageViewPage(typeof(ConfigHWParachute), rm.GetString("backstageViewPageParachute.Text"), isConnected, opt);
            AddBackstageViewPage(typeof(ConfigHWESP8266), rm.GetString("backstageViewPageESP.Text"), isConnected, opt);
            AddBackstageViewPage(typeof(Antenna.Tracker), "Antenna Tracker", true, opt);
#endif
            // // remeber last page accessed
            // foreach (BackstageViewPage page in backstageView.Pages)
            // {
            //     if (page.LinkText == lastpagename && page.Show)
            //     {
            //         backstageView.ActivatePage(page);
            //         break;
            //     }
            // }

            ThemeManager.ApplyThemeTo(this);
        }

        private void TabControl_Setup_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void TabControl1_ControlAdded(object sender, ControlEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HardwareConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backstageView.SelectedPage != null)
                lastpagename = backstageView.SelectedPage.LinkText;

            backstageView.Close();
        }
    }
}