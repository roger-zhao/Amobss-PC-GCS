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
    partial class ConfigFactory : MyUserControl, IActivate
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        bool active_OK = false;

        public ConfigFactory()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 将Unix时间戳转换为DateTime类型时间
        /// </summary>
        /// <param name="d">double 型数字</param>
        /// <returns>DateTime</returns>
        public static System.DateTime ConvertIntDateTime(double d)
        {
            System.DateTime time = System.DateTime.MinValue;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            time = startTime.AddSeconds(d);
            return time;
        }

        /// <summary>
        /// 将c# DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>double</returns>
        public static double ConvertDateTimeInt(System.DateTime time)
        {
            double intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            intResult = (time - startTime).TotalSeconds;
            return intResult;
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

            ThemeManager.ApplyThemeTo(gb_activate_date);
            //ThemeManager.ApplyThemeTo(max_height);
            ThemeManager.ApplyThemeTo(gb_expired_len);
            if (MainV2.connectOK)
            {
                float manu_dt_f = MainV2.comPort.GetParam("MANU_DT");
                float exp_dl_f = MainV2.comPort.GetParam("EXPIRED_DL");
                DateTime dt_factory = ConvertIntDateTime(manu_dt_f);
                this.num_act_year.Value = dt_factory.Year;
                this.num_act_month.Value = dt_factory.Month;
                this.num_act_day.Value = dt_factory.Day;

                if(!manu_dt_f.Equals(0.0f))
                {

                    float expire_len = exp_dl_f - manu_dt_f;
                    expire_len /= 24 * 60 * 60; // days
                    this.num_exp_year.Value = (uint)(expire_len / 365.0f);
                    this.num_exp_day.Value = (uint)((int)expire_len % 365);
                }

            }

            this.num_act_year.Enabled = false;
            this.num_act_month.Enabled = false;
            this.num_act_day.Enabled = false;
            this.num_exp_year.Enabled = false;
            this.num_exp_day.Enabled = false;
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
            if(!active_OK)
            {
                if (verify_code())
                {
                    this.num_act_year.Enabled = true;
                    this.num_act_month.Enabled = true;
                    this.num_act_day.Enabled = true;
                    this.num_exp_year.Enabled = true;
                    this.num_exp_day.Enabled = true;

                    this.mod.Text = "保  存";
                }
                return;
            }

            int try_times = 0;
            for (try_times = 0; try_times < 5; try_times++)
            {

                DateTime manu_dt = new DateTime((int)this.num_act_year.Value, (int)this.num_act_month.Value, (int)this.num_act_day.Value);
                double manu_dt_s = ConvertDateTimeInt(manu_dt);
                if (MainV2.comPort.setParam("MANU_DT", manu_dt_s)) //  && MainV2.comPort.setParam("WPNAV_LOIT_SPEED", float.Parse(this.horizon_spd.Value) * 100.0f))
                { 
                    float dt_dl = ((int)this.num_exp_year.Value * 365 + (int)this.num_exp_day.Value)*24*3600;
                    // save horizon spd 
                    if (MainV2.comPort.setParam("EXPIRED_DL", manu_dt_s + dt_dl)) //  && MainV2.comPort.setParam("WPNAV_LOIT_SPEED", float.Parse(this.horizon_spd.Value) * 100.0f))
                    {
                        CustomMessageBox.Show("设置使用期限成功!");
                        break;
                    }
                }

                if (5 == try_times)
                {
                    CustomMessageBox.Show("抱歉，设置使用期限失败，请尝试重新配置，或者重新连接飞控后再次尝试!");
                }
            }

            this.mod.Text = "输入密码修改期限";

        }

    }
}