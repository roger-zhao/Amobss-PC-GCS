

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
    public partial class UserConfig : MyUserControl, IActivate
    {
        internal static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string lastpagename = "";

        public UserConfig()
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




        private void UserConfig_Load(object sender, EventArgs e)
        {
            // ResourceManager rm = new ResourceManager(this.GetType());

            // AddBackstageViewPage(typeof(ConfigFirmwareDisabled), rm.GetString("backstageViewPagefw.Text"), isConnected);
            // var mand = AddBackstageViewPage(typeof(ConfigMandatory), rm.GetString("backstageViewPagemand.Text"), isConnected);

            // fly hgt spd
            // AddBackstageViewPage(typeof(ConfigFlyHgtSpd), rm.GetString("backstageViewPageFlyHgtSpd.Text"), true);
            ConfigFlyHgtSpd conf_flyspd = new ConfigFlyHgtSpd();
            conf_flyspd.Dock = DockStyle.Fill;
            TabPage flyspd_tp = new TabPage();//Create new tabpage
            flyspd_tp.Controls.Add(conf_flyspd);
            flyspd_tp.Text = "飞行设置";
            tabControl_userapp.TabPages.Add(flyspd_tp);
            tabControl_userapp.SelectedIndexChanged += new EventHandler(conf_flyspd.OnFocus) ;


            /// pump spd
            // AddBackstageViewPage(typeof(ConfigPumpSpd), rm.GetString("backstageViewPagePumpSpd.Text"), true);
            ConfigPumpSpd conf_pumpspd = new ConfigPumpSpd();
            conf_pumpspd.Dock = DockStyle.Fill;
            TabPage pumpspd_tp = new TabPage();//Create new tabpage
            pumpspd_tp.Controls.Add(conf_pumpspd);
            pumpspd_tp.Text = "植保设置";
            tabControl_userapp.TabPages.Add(pumpspd_tp);
            tabControl_userapp.SelectedIndexChanged += new EventHandler(conf_pumpspd.OnFocus); ;

            // fail safe
            // AddBackstageViewPage(typeof(ConfigFailsafeNew), rm.GetString("backstageViewPageFailsafe.Text"), true);
            ConfigFailsafeNew conf_fs = new ConfigFailsafeNew();
            conf_fs.Dock = DockStyle.Fill;
            TabPage fs_tp = new TabPage();//Create new tabpage
            fs_tp.Controls.Add(conf_fs);
            fs_tp.Text = "安全设置";
            tabControl_userapp.TabPages.Add(fs_tp);
            tabControl_userapp.SelectedIndexChanged += new EventHandler(conf_fs.OnFocus); ;

            // factory active stuff
            // AddBackstageViewPage(typeof(ConfigFactory), rm.GetString("backstageViewPageFactory.Text"), true);
            ConfigFactory conf_factory = new ConfigFactory();
            conf_factory.Dock = DockStyle.Fill;
            TabPage factory_tp = new TabPage();//Create new tabpage
            factory_tp.Controls.Add(conf_factory);
            factory_tp.Text = "激活设置";
            // factory_tp.Font = new System.Drawing.Font("微软雅黑", 11);
            tabControl_userapp.TabPages.Add(factory_tp);
            tabControl_userapp.SelectedIndexChanged += new EventHandler(conf_factory.OnFocus); ;

            /// Motor test
            // AddBackstageViewPage(typeof(ConfigMotorTest), rm.GetString("backstageViewPageMotorTest.Text"), true);

            /// config flmode
            // AddBackstageViewPage(typeof(ConfigFlightModes), rm.GetString("backstageViewPageflmode.Text"), isConnected);

            // remeber last page accessed
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

        private void UserConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backstageView.SelectedPage != null)
                lastpagename = backstageView.SelectedPage.LinkText;

            backstageView.Close();
        }
    }
}