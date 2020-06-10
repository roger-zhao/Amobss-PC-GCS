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
    partial class ConfigFlyHgtSpd : MyUserControl, IActivate
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ConfigFlyHgtSpd()
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
            log.Info("Enter flyhgtspd!");

            horizon_spd.AttachEvents();
            horizon_spd.ValueChanged += RNG_ValueChanged;
            // max_height.AttachEvents();
            // max_height.ValueChanged += RNG_ValueChanged;
            des_vel_decel_s.AttachEvents();
            des_vel_decel_s.ValueChanged += RNG_ValueChanged;
            rng_angle_max.AttachEvents();
            rng_angle_max.ValueChanged += RNG_ValueChanged;
            ThemeManager.ApplyThemeTo(horizon_spd);
            ThemeManager.ApplyThemeTo(rng_angle_max);
            ThemeManager.ApplyThemeTo(des_vel_decel_s);
            if (MainV2.connectOK)
            {
                this.horizon_spd.Value = (MainV2.comPort.GetParam("WPNAV_SPEED")/100).ToString();
                // BRK_ACCEL
                this.rng_angle_max.Value = (MainV2.comPort.GetParam("WPNAV_BRK_G")*10).ToString();
                this.des_vel_decel_s.Value = (MainV2.comPort.GetParam("WPNAV_BRK_DELAY")).ToString();
                this.cb_yaw_mode.Text = ((int)(MainV2.comPort.GetParam("WP_YAW_BEHAVIOR")) == 0) ? "机头保持不变" : "机头跟随航线";
            }
        }

        private void RNG_ValueChanged(object sender, string Name, string Value)
        {
            // this.des_vel_decel_s.Value = Value.ToString();
        }

        private void reset_Click(object sender, EventArgs e)
        {
            this.horizon_spd.Value = "5";
            this.rng_angle_max.Value = "0.8";
            this.des_vel_decel_s.Value = "0.2";
            this.cb_yaw_mode.Text = "机头跟随航线";
            // this.alt_max_enable.CheckState = CheckState.Unchecked;
        }

        private void send_to_Click(object sender, EventArgs e)
        {
            // send mav msg to FC: extend MSG_ID_DO_SET_SERVO
            // param3: 0: original, 
            // 3: set pwm value when semi-auto, full-auto & manual using, 
            // param4: sprayer pwm, param5: pump pwm
            log.Info(" set horizon spd: " + this.horizon_spd.Value);
            // log.Info(" set max_height spd: " + this.max_height.Value);
            // log.Info(" set des_vel_decel_s: " + this.my1.Value);

            int try_times = 0;
            for (try_times = 0; try_times < 5; try_times++)
            {

                // save horizon spd 
                if (MainV2.comPort.setParam("WPNAV_SPEED", float.Parse(this.horizon_spd.Value)*100.0f) && MainV2.comPort.setParam("WPNAV_LOIT_SPEED", float.Parse(this.horizon_spd.Value) * 100.0f))
                {

                    float yaw_mode = this.cb_yaw_mode.Text.Equals("机头保持不变") ?0:1; // = ((int)(MainV2.comPort.GetParam("WP_YAW_BEHAVIOR")) == 0) ?  : "航向跟随航线";

                    if (MainV2.comPort.setParam("WP_YAW_BEHAVIOR", yaw_mode))
                    {

                        if(MainV2.comPort.setParam("WPNAV_BRK_DELAY", float.Parse(this.des_vel_decel_s.Value)))
                        {

                            if(MainV2.comPort.setParam("WPNAV_BRK_G", float.Parse(this.rng_angle_max.Value)/10.0))
                            {
                                CustomMessageBox.Show("保存到飞控成功!");
                                break;
                            }

                        }


#if FENCE_ALT_MAX
                    // save alt_max
                    if (this.alt_max_enable.Checked)
                    {
                        if (MainV2.comPort.setParam("FENCE_ENABLE", 1) && MainV2.comPort.setParam("FENCE_ALT_MAX", float.Parse(this.max_height.Value)))
                        {
                            CustomMessageBox.Show("保存到飞控成功!");
                            break;
                        }
#endif
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