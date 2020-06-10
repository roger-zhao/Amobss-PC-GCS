using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using MissionPlanner.Arduino;
using MissionPlanner.Controls;
using MissionPlanner.Utilities;

namespace MissionPlanner.GCSViews.ConfigurationView
{
    partial class ConfigFailsafeNew : MyUserControl, IActivate
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        bool active_OK = false;

        public ConfigFailsafeNew()
        {
            InitializeComponent();
        }
        public void OnFocus(object sender, System.EventArgs e)
        {
            TabControl tc = (TabControl)sender;
            if (tc.SelectedTab == this.Parent)
            {
                //Parent-Tab is selected, do stuff...
                Activate();
            }

        }
        public void Activate()
        {
            log.Info("Enter failsafenew!");
            ThemeManager.ApplyThemeTo(gb_activate_date);
            //ThemeManager.ApplyThemeTo(max_height);
            volt_fs.AttachEvents();
            if (MainV2.connectOK)
            {
                float rc_fs = MainV2.comPort.GetParam("RC_FS_NON_AUTO");
                float fs_batt_volt = MainV2.comPort.GetParam("FS_VOLT");

                this.cb_rc_fs.Text = this.cb_rc_fs.Items[(int)rc_fs].ToString();
                this.volt_fs.Value = fs_batt_volt.ToString();
            }
        }

        private void RNG_ValueChanged(object sender, string Name, string Value)
        {
            // this.des_vel_decel_s.Value = Value.ToString();
        }

        private void reset_Click(object sender, EventArgs e)
        {

        }

        private bool verify_code()
        {
            string vcode = "F";
            string correct_code = "FHADMIN";
            if (InputBox.Show("输入激活密码", "请输入激活密码（请从飞行器供应商索取)", ref vcode, true) ==
        System.Windows.Forms.DialogResult.OK)
            {

                if (!vcode.Equals(correct_code))
                {
                    CustomMessageBox.Show("激活失败，无法执行控制命令！", "激活失败");
                    return false;

                }
                else
                {
                    active_OK = true;
                    CustomMessageBox.Show("激活成功，开始执行控制命令！", "激活成功");
                    return true;
                }

            }
            return false;

        }


        private void send_to_Click(object sender, EventArgs e)
        {

            int try_times = 0;
            for (try_times = 0; try_times < 5; try_times++)
            {

                float fs_non_auto = this.cb_rc_fs.SelectedItem.ToString().Equals(this.cb_rc_fs.Items[0].ToString())?0:1;
                // save horizon spd 
                if (MainV2.comPort.setParam("RC_FS_NON_AUTO", fs_non_auto))
                {

                    if (MainV2.comPort.setParam("FS_VOLT", float.Parse(this.volt_fs.Value)))
                    {
                        CustomMessageBox.Show("保存到飞控成功!");
                        break;
                    }

                }

                if (5 == try_times)
                {
                    CustomMessageBox.Show("抱歉，保存到飞控失败，请尝试重新配置，或者重新连接飞控后再次尝试!");
                }
            }
        
        }

    }
}