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
    partial class ConfigPumpSpd : MyUserControl, IActivate
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ConfigPumpSpd()
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
            log.Info("Enter UserApp!");

            sprayer_spd.AttachEvents();
            sprayer_spd.ValueChanged += RNG_ValueChanged;
            pump_spd.AttachEvents();
            pump_spd.ValueChanged += RNG_ValueChanged;
            line_spaces.AttachEvents();
            line_spaces.ValueChanged += RNG_ValueChanged;

            ThemeManager.ApplyThemeTo(sprayer_spd);
            ThemeManager.ApplyThemeTo(pump_spd);
            ThemeManager.ApplyThemeTo(line_spaces);

            if (MainV2.connectOK)
            {
                this.sprayer_spd.Value = (MainV2.comPort.GetParam("CROP_SPD")).ToString();
                this.pump_spd.Value = (MainV2.comPort.GetParam("PMP_SPD")).ToString();
                this.line_spaces.Value = (MainV2.comPort.GetParam("AB_DIS")).ToString();
            }
        }

        private void RNG_ValueChanged(object sender, string Name, string Value)
        {
        }

        private void reset_Click(object sender, EventArgs e)
        {
            this.sprayer_spd.Value = "1500";
            this.pump_spd.Value = "1500";
            this.line_spaces.Value = "5";
        }

        private void sendto_Click(object sender, EventArgs e)
        {
            // send mav msg to FC: extend MSG_ID_DO_SET_SERVO
            // param3: 0: original, 
            // 3: set pwm value when semi-auto, full-auto & manual using, 
            // param4: sprayer pwm, param5: pump pwm
            log.Info(" set sprayer spd: " + this.sprayer_spd.Value);
            log.Info(" set pump spd: " + this.pump_spd.Value);
            log.Info(" set lines space: " + this.line_spaces.Value);

            int try_times = 0;
            for(try_times = 0; try_times < 5; try_times++)
            {
                
                if(MainV2.comPort.setParam("CROP_SPD", float.Parse(this.sprayer_spd.Value)) 
                    && MainV2.comPort.setParam("PMP_SPD", float.Parse(this.pump_spd.Value))
                    && MainV2.comPort.setParam("AB_DIS", float.Parse(this.line_spaces.Value))
                    )
                {
                    CustomMessageBox.Show("保存到飞控成功!");
                    break;
                }
                
            }
            if(5 == try_times)
            {
                CustomMessageBox.Show("抱歉，保存到飞控失败，请尝试重新配置，或者重新连接飞控后再次尝试!");
            }
        }

        private void testmode_Click(object sender, EventArgs e)
        {
            // change text to notify user to exit test mode
            this.note_tips.BackColor = System.Drawing.Color.DarkBlue;
            this.note_tips.ForeColor = System.Drawing.Color.Yellow;

            // current is non-test mode
            if(this.testmode.Text.Equals("进入测试模式"))
            {
                log.Info(" test mode");

                // this.testmode.Size = new System.Drawing.Size(this.testmode.Width+20, this.testmode.Width+10); 
                // send mav msg to FC: extend MSG_ID_DO_SET_SERVO
                // param3: 0: original, 
                // 3: set pwm value when semi-auto, full-auto & manual using, 
                // param4: sprayer pwm, param5: pump pwm
                // diff with sendto: param6: 0x5 -> test mode (no armed need when test sprayer & pump)
                log.Info(" test mode: set sprayer spd: " + this.sprayer_spd.Value);
                log.Info(" test mode: set pump spd: " + this.pump_spd.Value);

                int try_times = 0;
                for (try_times = 0; try_times < 5; try_times++)
                {
                    // no need to setParam, using do_set_servo as before to enable test_mode
                    if (MainV2.comPort.doCommand(MAVLink.MAV_CMD.DO_SET_SERVO, 0, 0, 0x3, float.Parse(this.sprayer_spd.Value), float.Parse(this.pump_spd.Value), 0x5, 0))
                    {
                        CustomMessageBox.Show("现在进入测试模式!");

                        this.testmode.Text = "退出测试模式";
                        this.testmode.TextColor = System.Drawing.Color.Red;

                        break;
                    }

                }
                if (5 == try_times)
                {
                    CustomMessageBox.Show("抱歉，无法进入测试模式，请重新尝试，或者重新连接飞控后再次尝试!");
                }
            }
            else // exit test mode
            {

                // this.testmode.Size = new System.Drawing.Size(this.testmode.Width-20, this.testmode.Width-10);
                // send mav msg to FC: extend MSG_ID_DO_SET_SERVO
                // param3: 0: original, 
                // 3: set pwm value when semi-auto, full-auto & manual using, 
                // param4: sprayer pwm, param5: pump pwm
                // diff with sendto: param6: 0x5 -> test mode (no armed need when test sprayer & pump)
                log.Info(" exit test mode");

                int try_times = 0;
                for (try_times = 0; try_times < 5; try_times++)
                {
                    // sent default value when exit test mode
                    if (MainV2.comPort.doCommand(MAVLink.MAV_CMD.DO_SET_SERVO, 0, 0, 0x3, 1000.0f, 1000.0f, 0x0, 0))
                    {
                        CustomMessageBox.Show("退出测试模式!");
                        this.testmode.Text = "进入测试模式";
                        this.testmode.TextColor = this.sendto.TextColor;
                        break;
                    }

                }
                if (5 == try_times)
                {
                    CustomMessageBox.Show("抱歉，无法退出测试模式，请重新尝试，或者重新连接飞控后再次尝试!");
                }
            }
        }
    }
}