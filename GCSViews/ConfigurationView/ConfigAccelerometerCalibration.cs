using System;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using log4net;
using MissionPlanner.Controls;
using System.Text;

namespace MissionPlanner.GCSViews.ConfigurationView
{
    public partial class ConfigAccelerometerCalibration : UserControl, IActivate, IDeactivate
    {
        private const float DisabledOpacity = 0.2F;
        private const float EnabledOpacity = 1.0F;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private byte count;

        bool _incalibrate = false;

        public ConfigAccelerometerCalibration()
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
            BUT_calib_accell.Enabled = true;
            _incalibrate = false;
        }

        public void Deactivate()
        {
            MainV2.comPort.giveComport = false;
            _incalibrate = false;
        }

        private void BUT_calib_accell_Click(object sender, EventArgs e)
        {
            if (_incalibrate)
            {
                count++;
                try
                {
                    MainV2.comPort.sendPacket(new MAVLink.mavlink_command_ack_t {command = 1, result = count},
                        MainV2.comPort.sysidcurrent, MainV2.comPort.compidcurrent);
                }
                catch
                {
                    CustomMessageBox.Show(Strings.CommandFailed, Strings.ERROR);
                    return;
                }

                return;
            }

            try
            {
                count = 0;

                Log.Info("Sending accel command (mavlink 1.0)");

                MainV2.comPort.doCommand(MAVLink.MAV_CMD.PREFLIGHT_CALIBRATION, 0, 0, 0, 0, 1, 0, 0);

                _incalibrate = true;

                MainV2.comPort.SubscribeToPacketType(MAVLink.MAVLINK_MSG_ID.STATUSTEXT, receivedPacket);

                BUT_calib_accell.Text = "按任意键继续"; // Strings.Click_when_Done;
            }
            catch (Exception ex)
            {
                _incalibrate = false;
                Log.Error("Exception on level", ex);
                CustomMessageBox.Show("Failed to level", Strings.ERROR);
            }
        }

        private bool receivedPacket(MAVLink.MAVLinkMessage arg)
        {
            if (arg.msgid == (uint)MAVLink.MAVLINK_MSG_ID.STATUSTEXT)
            {
                var message = ASCIIEncoding.ASCII.GetString(arg.ToStructure<MAVLink.mavlink_statustext_t>().text);

                UpdateUserMessage(message);

                if (message.ToLower().Contains("calibration successful") ||
                 message.ToLower().Contains("calibration failed"))
                {
                    try
                    {
                        Invoke((MethodInvoker)delegate
                        {
                            if (message.ToLower().Contains("calibration successful"))
                            {
                                BUT_calib_accell.Text = "校准完成"; // Strings.Done;
                            }
                            else if (message.ToLower().Contains("calibration failed"))
                            {
                                BUT_calib_accell.Text = "校准失败"; // Strings.Done;
                                BUT_calib_accell.TextColor = System.Drawing.Color.Red; 
                            }

                            BUT_calib_accell.Enabled = false;
                        });

                        _incalibrate = false;
                        MainV2.comPort.UnSubscribeToPacketType(MAVLink.MAVLINK_MSG_ID.STATUSTEXT, receivedPacket);
                    }
                    catch
                    {
                    }
                }
            }

            return true;
        }

        public void UpdateUserMessage(string message)
        {
            Invoke((MethodInvoker) delegate
            {
                if (message.ToLower().Contains("place vehicle") || message.ToLower().Contains("calibration"))
                {
                    /// AB ZhaoYJ@2017-03-09
                    /// for: zh-Hans
                    /// TODO: 
                    /// start
                    if (message.ToLower().Contains("level"))
                    {
                        lbl_Accel_user.Text = "请水平放置飞行器，静待1到2秒后，按任意键完成水平面校准";
                    }
                    else if (message.ToLower().Contains("left side"))
                    {
                        lbl_Accel_user.Text = "请将机头左面朝下放置飞行器，静待1到2秒后，按任意键完成左面校准";
                    }
                    else if (message.ToLower().Contains("right side"))
                    {
                        lbl_Accel_user.Text = "请将机头右面朝下放置飞行器，静待1到2秒后，按任意键完成右面校准";
                    }
                    else if (message.ToLower().Contains("nose down"))
                    {
                        lbl_Accel_user.Text = "请将机头朝下放置飞行器，静待1到2秒后，按任意键完成朝下校准";
                    }
                    else if (message.ToLower().Contains("nose up"))
                    {
                        lbl_Accel_user.Text = "请将机头朝上放置飞行器，静待1到2秒后，按任意键完成朝上校准";
                    }
                    else if (message.ToLower().Contains("its back"))
                    {
                        lbl_Accel_user.Text = "请倒置飞行器（正面朝下），静待1到2秒后，按任意键完成背面校准";
                    }
                    else if (message.ToLower().Contains("successful"))
                    {
                        lbl_Accel_user.Text = "     校准成功     ";
                    }
                    else if (message.ToLower().Contains("failed"))
                    {
                        // lbl_Accel_user.Text = "抱歉，校准失败，请尝试重新校准";
                        lbl_Accel_user.Text = message;
                    }
                    else
                    {
                        lbl_Accel_user.Text = message;
                    }
                    /// end
                }

            });
        }

        private void BUT_level_Click(object sender, EventArgs e)
        {
            try
            {
                Log.Info("Sending level command (mavlink 1.0)");
                MainV2.comPort.doCommand(MAVLink.MAV_CMD.PREFLIGHT_CALIBRATION, 0, 0, 0, 0, 2, 0, 0);

                BUT_level.Text = Strings.Completed;
            }
            catch (Exception ex)
            {
                Log.Error("Exception on level", ex);
                CustomMessageBox.Show("Failed to level", Strings.ERROR);
            }
        }

        private void lbl_Accel_user_Click(object sender, EventArgs e)
        {

        }
    }
}