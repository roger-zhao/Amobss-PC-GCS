using System;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MissionPlanner.Controls;
using MissionPlanner.Utilities;

namespace MissionPlanner.GCSViews.ConfigurationView
{
    public partial class ConfigArducopter : UserControl, IActivate
    {
        // from http://stackoverflow.com/questions/2512781/winforms-big-paragraph-tooltip/2512895#2512895
        private const int maximumSingleLineTooltipLength = 50;
        private static Hashtable tooltips = new Hashtable();
        private readonly Hashtable changes = new Hashtable();
        internal bool startup = true;

        public ConfigArducopter()
        {
            InitializeComponent();
            ThemeManager.ApplyThemeTo(this);
            Activate();
        }

        public void Activate()
        {
            if (!MainV2.comPort.BaseStream.IsOpen)
            {
                Enabled = false;
                return;
            }
            if (MainV2.comPort.MAV.cs.firmware == MainV2.Firmwares.ArduCopter2)
            {
                Enabled = true;
            }
            else
            {
                Enabled = false;
                return;
            }

            startup = true;

            changes.Clear();

            // ensure the fields are populated before setting them
            TUNE.setup(
                ParameterMetaDataRepository.GetParameterOptionsInt("TUNE", MainV2.comPort.MAV.cs.firmware.ToString())
                    .ToList(), "TUNE", MainV2.comPort.MAV.param);
            CH7_OPT.setup(
                ParameterMetaDataRepository.GetParameterOptionsInt("CH7_OPT", MainV2.comPort.MAV.cs.firmware.ToString())
                    .ToList(), "CH7_OPT", MainV2.comPort.MAV.param);
            CH8_OPT.setup(
                ParameterMetaDataRepository.GetParameterOptionsInt("CH8_OPT", MainV2.comPort.MAV.cs.firmware.ToString())
                    .ToList(), "CH8_OPT", MainV2.comPort.MAV.param);

            TUNE_LOW.setup(0, 10000, 1000, 0.01f, "TUNE_LOW", MainV2.comPort.MAV.param);
            TUNE_HIGH.setup(0, 10000, 1000, 0.01f, "TUNE_HIGH", MainV2.comPort.MAV.param);

            // HLD_LAT_P.setup(0, 0, 1, 0.001f, new[] {"HLD_LAT_P", "POS_XY_P"}, MainV2.comPort.MAV.param);
            HLD_LAT_P.setup(0, 10, 1, 0.001f, new[] { "HLD_LAT_P", "PSC_POSXY_P" }, MainV2.comPort.MAV.param);
            // PosXY_I.setup(0, 10, 1, 0.001f, new[] { "PosXY_I", "PSC_PXY_I" }, MainV2.comPort.MAV.param);
            // PosXY_IMAX.setup(0, 5000, 1, 1, new[] { "PosXY_IMAX", "PSC_PXY_IMAX" }, MainV2.comPort.MAV.param);
            // PosXY_D.setup(0, 10, 1, 0.001f, new[] { "PosXY_D", "PSC_PXY_D" }, MainV2.comPort.MAV.param);

            LOITER_LAT_D.setup(0, 10, 1, 0.001f, new[] { "LOITER_LAT_D", "PSC_VELXY_D" }, MainV2.comPort.MAV.param);
            LOITER_LAT_I.setup(0, 10, 1, 0.001f, new[] {"LOITER_LAT_I", "PSC_VELXY_I" }, MainV2.comPort.MAV.param);
            LOITER_LAT_IMAX.setup(0, 5000, 1, 1, new[] {"LOITER_LAT_IMAX", "PSC_VELXY_IMAX" }, MainV2.comPort.MAV.param);
            LOITER_LAT_P.setup(0, 10, 1, 0.001f, new[] {"LOITER_LAT_P", "PSC_VELXY_P" }, MainV2.comPort.MAV.param);

            RATE_PIT_D.setup(0, 10, 1, 0.0001f, new[] {"RATE_PIT_D", "ATC_RAT_PIT_D"}, MainV2.comPort.MAV.param);
            RATE_PIT_I.setup(0, 10, 1, 0.001f, new[] {"RATE_PIT_I", "ATC_RAT_PIT_I"}, MainV2.comPort.MAV.param);
            if (MainV2.comPort.MAV.param.ContainsKey("ATC_RAT_PIT_IMAX")) // 3.4 changes scaling
                RATE_PIT_IMAX.setup(0, 10, 1, 0.1f, new[] {"ATC_RAT_PIT_IMAX"}, MainV2.comPort.MAV.param);
            else
                RATE_PIT_IMAX.setup(0, 10, 10, 1f, new[] {"RATE_PIT_IMAX"}, MainV2.comPort.MAV.param);
            RATE_PIT_P.setup(0, 10, 1, 0.001f, new[] {"RATE_PIT_P", "ATC_RAT_PIT_P"}, MainV2.comPort.MAV.param);
            RATE_PIT_FILT.setup(0, 10, 1, 0.001f, new[] { "RATE_PIT_FILT", "ATC_RAT_PIT_FILT" }, MainV2.comPort.MAV.param);

            RATE_RLL_D.setup(0, 10, 1, 0.0001f, new[] {"RATE_RLL_D", "ATC_RAT_RLL_D"}, MainV2.comPort.MAV.param);
            RATE_RLL_I.setup(0, 10, 1, 0.001f, new[] {"RATE_RLL_I", "ATC_RAT_RLL_I"}, MainV2.comPort.MAV.param);
            if (MainV2.comPort.MAV.param.ContainsKey("ATC_RAT_RLL_IMAX")) // 3.4 changes scaling
                RATE_RLL_IMAX.setup(0, 10, 1, 0.1f, new[] {"ATC_RAT_RLL_IMAX"}, MainV2.comPort.MAV.param);
            else
                RATE_RLL_IMAX.setup(0, 10, 10, 1f, new[] {"RATE_RLL_IMAX"}, MainV2.comPort.MAV.param);
            RATE_RLL_P.setup(0, 10, 1, 0.001f, new[] {"RATE_RLL_P", "ATC_RAT_RLL_P"}, MainV2.comPort.MAV.param);
            RATE_RLL_FILT.setup(0, 10, 1, 0.001f, new[] { "RATE_RLL_FILT", "ATC_RAT_RLL_FILT" }, MainV2.comPort.MAV.param);

            RATE_YAW_D.setup(0, 10, 1, 0.0001f, new[] {"RATE_YAW_D", "ATC_RAT_YAW_D"}, MainV2.comPort.MAV.param);
            RATE_YAW_I.setup(0, 10, 1, 0.001f, new[] {"RATE_YAW_I", "ATC_RAT_YAW_I"}, MainV2.comPort.MAV.param);
            if (MainV2.comPort.MAV.param.ContainsKey("ATC_RAT_YAW_IMAX")) // 3.4 changes scaling
                RATE_YAW_IMAX.setup(0, 10, 1, 0.1f, new[] {"ATC_RAT_YAW_IMAX"}, MainV2.comPort.MAV.param);
            else
                RATE_YAW_IMAX.setup(0, 10, 10, 1f, new[] {"RATE_YAW_IMAX"}, MainV2.comPort.MAV.param);
            RATE_YAW_P.setup(0, 10, 1, 0.001f, new[] {"RATE_YAW_P", "ATC_RAT_YAW_P"}, MainV2.comPort.MAV.param);
            RATE_YAW_FILT.setup(0, 10, 1, 0.001f, new[] { "RATE_YAW_FILT", "ATC_RAT_YAW_FILT" }, MainV2.comPort.MAV.param);

            STB_PIT_P.setup(0, 10, 1, 0.001f, new[] {"STB_PIT_P", "ATC_ANG_PIT_P"}, MainV2.comPort.MAV.param);
            // STB_PIT_I.setup(0, 10, 1, 0.001f, new[] { "STB_PIT_I", "ATC_ANG_P_I" }, MainV2.comPort.MAV.param);
            // STB_PIT_IMAX.setup(0, 30, 1, 0.1f, new[] { "STB_PIT_IMAX", "ATC_ANG_P_IMAX" }, MainV2.comPort.MAV.param);
            // STB_PIT_D.setup(0, 10, 1, 0.001f, new[] { "STB_PIT_D", "ATC_ANG_P_D" }, MainV2.comPort.MAV.param);
            STB_RLL_P.setup(0, 10, 1, 0.001f, new[] {"STB_RLL_P", "ATC_ANG_RLL_P"}, MainV2.comPort.MAV.param);
            // STB_RLL_IMAX.setup(0, 30, 1, 0.1f, new[] { "STB_RLL_IMAX", "ATC_ANG_R_IMAX" }, MainV2.comPort.MAV.param);
            // STB_RLL_D.setup(0, 10, 1, 0.001f, new[] { "STB_RLL_D", "ATC_ANG_R_D" }, MainV2.comPort.MAV.param);
            // STB_RLL_I.setup(0, 10, 1, 0.001f, new[] { "STB_RLL_I", "ATC_ANG_R_I" }, MainV2.comPort.MAV.param);
            STB_YAW_P.setup(0, 10, 1, 0.001f, new[] {"STB_YAW_P", "ATC_ANG_YAW_P"}, MainV2.comPort.MAV.param);
            // STB_YAW_I.setup(0, 10, 1, 0.001f, new[] { "STB_YAW_I", "ATC_ANG_Y_I" }, MainV2.comPort.MAV.param);
            // STB_YAW_IMAX.setup(0, 30, 1, 0.1f, new[] { "STB_YAW_IMAX", "ATC_ANG_Y_IMAX" }, MainV2.comPort.MAV.param);
            // STB_YAW_D.setup(0, 10, 1, 0.001f, new[] { "STB_YAW_D", "ATC_ANG_Y_D" }, MainV2.comPort.MAV.param);

            THR_ACCEL_D.setup(0, 10, 1, 0.001f, new[] {"THR_ACCEL_D", "PSC_ACCZ_D" }, MainV2.comPort.MAV.param);
            THR_ACCEL_I.setup(0, 10, 1, 0.001f, new[] {"THR_ACCEL_I", "PSC_ACCZ_I" }, MainV2.comPort.MAV.param);
            THR_ACCEL_IMAX.setup(0, 5000, 1, 1f, new[] {"THR_ACCEL_IMAX", "PSC_ACCZ_IMAX" }, MainV2.comPort.MAV.param);
            THR_ACCEL_P.setup(0, 10, 1, 0.001f, new[] {"THR_ACCEL_P", "PSC_ACCZ_P" }, MainV2.comPort.MAV.param);
            THR_ALT_P.setup(0, 10, 1, 0.001f, new[] {"THR_ALT_P", "PSC_POSZ_P" }, MainV2.comPort.MAV.param);
            THR_RATE_P.setup(0, 10, 1, 0.001f, new[] {"THR_RATE_P", "PSC_VELZ_P" }, MainV2.comPort.MAV.param);

            WPNAV_LOIT_SPEED.setup(0, 0, 1, 0.001f, "WPNAV_LOIT_SPEED", MainV2.comPort.MAV.param);
            WPNAV_RADIUS.setup(0, 0, 1, 0.001f, "WPNAV_RADIUS", MainV2.comPort.MAV.param);
            WPNAV_SPEED.setup(0, 0, 1, 0.001f, "WPNAV_SPEED", MainV2.comPort.MAV.param);
            WPNAV_SPEED_DN.setup(0, 0, 1, 0.001f, "WPNAV_SPEED_DN", MainV2.comPort.MAV.param);
            WPNAV_SPEED_UP.setup(0, 0, 1, 0.001f, "WPNAV_SPEED_UP", MainV2.comPort.MAV.param);

            // unlock entries if they differ
            if (RATE_RLL_P.Value != RATE_PIT_P.Value || RATE_RLL_I.Value != RATE_PIT_I.Value
                || RATE_RLL_D.Value != RATE_PIT_D.Value || RATE_RLL_IMAX.Value != RATE_PIT_IMAX.Value)
            {
                CHK_lockrollpitch.Checked = false;
            }

            if (MainV2.comPort.MAV.param["H_SWASH_TYPE"] != null)
            {
                CHK_lockrollpitch.Checked = false;
            }

            startup = false;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.S))
            {
                BUT_writePIDS_Click(null, null);
                return true;
            }

            return false;
        }

        private static string AddNewLinesForTooltip(string text)
        {
            if (text.Length < maximumSingleLineTooltipLength)
                return text;
            var lineLength = (int) Math.Sqrt(text.Length)*2;
            var sb = new StringBuilder();
            var currentLinePosition = 0;
            for (var textIndex = 0; textIndex < text.Length; textIndex++)
            {
                // If we have reached the target line length and the next      
                // character is whitespace then begin a new line.   
                if (currentLinePosition >= lineLength &&
                    char.IsWhiteSpace(text[textIndex]))
                {
                    sb.Append(Environment.NewLine);
                    currentLinePosition = 0;
                }
                // If we have just started a new line, skip all the whitespace.    
                if (currentLinePosition == 0)
                    while (textIndex < text.Length && char.IsWhiteSpace(text[textIndex]))
                        textIndex++;
                // Append the next character.     
                if (textIndex < text.Length) sb.Append(text[textIndex]);
                currentLinePosition++;
            }
            return sb.ToString();
        }

        private void disableNumericUpDownControls(Control inctl)
        {
            foreach (Control ctl in inctl.Controls)
            {
                if (ctl.Controls.Count > 0)
                {
                    disableNumericUpDownControls(ctl);
                }
                if (ctl.GetType() == typeof (NumericUpDown))
                {
                    ctl.Enabled = false;
                }
            }
        }

        internal void EEPROM_View_float_TextChanged(object sender, EventArgs e)
        {
            if (startup)
                return;

            float value = 0;
            var name = ((Control) sender).Name;

            // do domainupdown state check
            try
            {
                if (sender.GetType() == typeof (MavlinkNumericUpDown))
                {
                    value = ((MAVLinkParamChanged) e).value;
                    changes[name] = value;
                }
                else if (sender.GetType() == typeof (MavlinkComboBox))
                {
                    value = ((MAVLinkParamChanged) e).value;
                    changes[name] = value;
                }
                ((Control) sender).BackColor = Color.Blue;
            }
            catch (Exception)
            {
                ((Control) sender).BackColor = Color.Red;
            }

            try
            {
                // enable roll and pitch pairing for ac2
                if (CHK_lockrollpitch.Checked)
                {
                    if (name.StartsWith("RATE_") || name.StartsWith("STB_") || name.StartsWith("ACRO_"))
                    {
                        if (name.Contains("_RLL_"))
                        {
                            var newname = name.Replace("_RLL_", "_PIT_");
                            var arr = Controls.Find(newname, true);
                            changes[newname] = value;

                            if (arr.Length > 0)
                            {
                                arr[0].Text = ((Control) sender).Text;
                                arr[0].BackColor = Color.Green;
                            }
                        }
                        else if (name.Contains("_PIT_"))
                        {
                            var newname = name.Replace("_PIT_", "_RLL_");
                            var arr = Controls.Find(newname, true);
                            changes[newname] = value;

                            if (arr.Length > 0)
                            {
                                arr[0].Text = ((Control) sender).Text;
                                arr[0].BackColor = Color.Green;
                            }
                        }
                    }
                }
                // keep nav_lat and nav_lon paired
                if (name.Contains("NAV_LAT_"))
                {
                    var newname = name.Replace("NAV_LAT_", "NAV_LON_");
                    var arr = Controls.Find(newname, true);
                    changes[newname] = value;

                    if (arr.Length > 0)
                    {
                        arr[0].Text = ((Control) sender).Text;
                        arr[0].BackColor = Color.Green;
                    }
                }
                // keep loiter_lat and loiter_lon paired
                if (name.Contains("LOITER_LAT_"))
                {
                    var newname = name.Replace("LOITER_LAT_", "LOITER_LON_");
                    var arr = Controls.Find(newname, true);
                    changes[newname] = value;

                    if (arr.Length > 0)
                    {
                        arr[0].Text = ((Control) sender).Text;
                        arr[0].BackColor = Color.Green;
                    }
                }
            }
            catch
            {
            }
        }

        private void BUT_writePIDS_Click(object sender, EventArgs e)
        {
            var temp = (Hashtable) changes.Clone();

            foreach (string value in temp.Keys)
            {
                try
                {
                    if ((float) changes[value] > (float) MainV2.comPort.MAV.param[value]*2.0f)
                        if (
                            MessageBox.Show(value + " 参数比上次值大两倍，确定修改？",
                                "PID参数变化过大", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            try
                            {
                                // set control as well
                                var textControls = Controls.Find(value, true);
                                if (textControls.Length > 0)
                                {
                                    // restore old value
                                    textControls[0].Text = MainV2.comPort.MAV.param[value].Value.ToString();
                                    textControls[0].BackColor = Color.FromArgb(0x43, 0x44, 0x45);
                                }
                            }
                            catch
                            {
                            }
                            return;
                        }

                    MainV2.comPort.setParam(value, (float) changes[value]);

                    changes.Remove(value);

                    try
                    {
                        // set control as well
                        var textControls = Controls.Find(value, true);
                        if (textControls.Length > 0)
                        {
                            textControls[0].BackColor = Color.DarkGreen;
                            textControls[0].ForeColor = Color.White;
                        }
                    }
                    catch
                    {
                    }
                }
                catch
                {
                    CustomMessageBox.Show(string.Format(Strings.ErrorSetValueFailed, value), Strings.ERROR);
                }
            }
        }

        /// <summary>
        ///     Handles the Click event of the BUT_rerequestparams control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        protected void BUT_rerequestparams_Click(object sender, EventArgs e)
        {
            if (!MainV2.comPort.BaseStream.IsOpen)
                return;

            ((Control) sender).Enabled = false;

            try
            {
                MainV2.comPort.getParamList();
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(Strings.ErrorReceivingParams + ex, Strings.ERROR);
            }


            ((Control) sender).Enabled = true;


            Activate();
        }

        private void BUT_refreshpart_Click(object sender, EventArgs e)
        {
            if (!MainV2.comPort.BaseStream.IsOpen)
                return;

            ((Control) sender).Enabled = false;


            updateparam(this);

            ((Control) sender).Enabled = true;


            Activate();
        }

        private void updateparam(Control parentctl)
        {
            foreach (Control ctl in parentctl.Controls)
            {
                if (typeof (MavlinkNumericUpDown) == ctl.GetType() || typeof (ComboBox) == ctl.GetType())
                {
                    try
                    {
                        MainV2.comPort.GetParam(ctl.Name);
                    }
                    catch
                    {
                    }
                }

                if (ctl.Controls.Count > 0)
                {
                    updateparam(ctl);
                }
            }
        }

        private void numeric_ValueUpdated(object sender, EventArgs e)
        {
            EEPROM_View_float_TextChanged(sender, e);
        }

        private void cb_log_angrate_CheckedChanged(object sender, EventArgs e)
        {
            if(cb_log_angrate.Checked)
            {
                if (cb_log_angrate_rate.Text.Equals(""))
                {
                    cb_log_angrate_rate.Text = "100";
                }
                // volt then mean connected
                if (MainV2.comPort.MAV.cs._battery_voltage.Equals(0.0f))
                {
                    MessageBox.Show("未连接飞控");
                    return;
                }
                int dev_opt = (int)MainV2.comPort.GetParam("DEV_OPTIONS");
                float value = dev_opt | 0x10;
                float log_rate = (float)(400/byte.Parse(cb_log_angrate_rate.Text));
                if (MainV2.comPort.setParam("DEV_OPTIONS", value))
                {
                    if (MainV2.comPort.setParam("ATC_LOG_RATE", log_rate))
                    {
                        if(MainV2.comPort.GetParam("DEV_OPTIONS").Equals(value))
                        {
                            if (MainV2.comPort.GetParam("ATC_LOG_RATE").Equals(log_rate))
                            {
                                MessageBox.Show("使能log成功", "完成", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("使能log失败1", "错误", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                    }

                }
                else
                {
                    MessageBox.Show("使能log失败", "错误", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                }
            }
            else
            {
                // volt then mean connected
                if (MainV2.comPort.MAV.cs._battery_voltage.Equals(0.0f))
                {
                    MessageBox.Show("未连接飞控");
                    return;
                }
                int dev_opt = (int)MainV2.comPort.GetParam("DEV_OPTIONS");
                if (MainV2.comPort.setParam("DEV_OPTIONS", dev_opt & (~0x10)))
                {

                }
                else
                {
                    MessageBox.Show("关闭log失败", "错误", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                }
            }

        }

        private void cb_log_pos_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_log_pos.Checked)
            {
                if (cb_log_pos_rate.Text.Equals(""))
                {
                    cb_log_pos_rate.Text = "100";
                }
                // volt then mean connected
                if (MainV2.comPort.MAV.cs._battery_voltage.Equals(0.0f))
                {
                    MessageBox.Show("未连接飞控");
                    return;
                }
                int dev_opt = (int)MainV2.comPort.GetParam("DEV_OPTIONS");

                float value = dev_opt | 0x20;
                float log_rate = (float)(50 / byte.Parse(cb_log_pos_rate.Text));

                if (MainV2.comPort.setParam("DEV_OPTIONS", value))
                {
                    if (MainV2.comPort.setParam("PSC_LOG_RATE", log_rate))
                    {
                        if (MainV2.comPort.GetParam("DEV_OPTIONS").Equals(value))
                        {
                            if (MainV2.comPort.GetParam("PSC_LOG_RATE").Equals(log_rate))
                            {
                                MessageBox.Show("使能log成功", "完成", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("使能log失败1", "错误", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                    }

                }
                else
                {
                    MessageBox.Show("使能log失败", "错误", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                }
            }
            else
            {
                // volt then mean connected
                if (MainV2.comPort.MAV.cs._battery_voltage.Equals(0.0f))
                {
                    MessageBox.Show("未连接飞控");
                    return;
                }
                int dev_opt = (int)MainV2.comPort.GetParam("DEV_OPTIONS");
                if (MainV2.comPort.setParam("DEV_OPTIONS", dev_opt & (~0x20)))
                {

                }
                else
                {
                    MessageBox.Show("关闭log失败", "错误", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                }
            }
 
        }

        private void cb_log_accz_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_log_accz.Checked)
            {
                if(cb_log_accZ_rate.Text.Equals(""))
                {
                    cb_log_accZ_rate.Text = "100";
                }
                // volt then mean connected
                if (MainV2.comPort.MAV.cs._battery_voltage.Equals(0.0f))
                {
                    MessageBox.Show("未连接飞控");
                    return;
                }
                int dev_opt = (int)MainV2.comPort.GetParam("DEV_OPTIONS");
                float value = dev_opt | 0x200;
                float log_rate = (float)(400 / byte.Parse(cb_log_accZ_rate.Text));
                if (MainV2.comPort.setParam("DEV_OPTIONS", value))
                {
                    if (MainV2.comPort.setParam("PSC_LOG_RATE", log_rate))
                    {
                        if (MainV2.comPort.GetParam("DEV_OPTIONS").Equals(value))
                        {
                            if (MainV2.comPort.GetParam("PSC_LOG_RATE").Equals(log_rate))
                            {
                                MessageBox.Show("使能log成功", "完成", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("使能log失败1", "错误", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                    }

                }
                else
                {
                    MessageBox.Show("使能log失败", "错误", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                }
            }
            else
            {
                if (cb_log_accZ_rate.Text.Equals(""))
                {
                    cb_log_accZ_rate.Text = "100";
                }
                // volt then mean connected
                if (MainV2.comPort.MAV.cs._battery_voltage.Equals(0.0f))
                {
                    MessageBox.Show("未连接飞控");
                    return;
                }

                int dev_opt = (int)MainV2.comPort.GetParam("DEV_OPTIONS");
                if (MainV2.comPort.setParam("DEV_OPTIONS", dev_opt & (~0x200)))
                {

                }
                else
                {
                    MessageBox.Show("关闭log失败", "错误", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                }
            }
        }

        private void cb_log_rcout_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_log_rcout.Checked)
            {
                if (cb_pwm_log_rate.Text.Equals(""))
                {
                    cb_pwm_log_rate.Text = "100";
                }
                // volt then mean connected
                if (MainV2.comPort.MAV.cs._battery_voltage.Equals(0.0f))
                {
                    MessageBox.Show("未连接飞控");
                    return;
                }
                int dev_opt = (int)MainV2.comPort.GetParam("DEV_OPTIONS");
                float log_rate = (float)(400 / byte.Parse(cb_pwm_log_rate.Text));
                float value = dev_opt | 0x20000;

                if (MainV2.comPort.setParam("DEV_OPTIONS", value))
                {
                    if (MainV2.comPort.GetParam("DEV_OPTIONS").Equals((float)(value)))
                    {
                        if (MainV2.comPort.GetParam("PWM_LOG_RATE").Equals(log_rate))
                        {
                            MessageBox.Show("使能log成功", "完成", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("使能log失败", "错误", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                }
            }
            else
            {
                // volt then mean connected
                if (MainV2.comPort.MAV.cs._battery_voltage.Equals(0.0f))
                {
                    MessageBox.Show("未连接飞控");
                    return;
                }
                int dev_opt = (int)MainV2.comPort.GetParam("DEV_OPTIONS");
                if (MainV2.comPort.setParam("DEV_OPTIONS", dev_opt & (~0x20000)))
                {

                }
                else
                {
                    MessageBox.Show("关闭log失败", "错误", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                }
            }
        }

        private void rb_1KHz_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_1KHz.Checked)
            {
                if (cb_1KHz_ft.Text.Equals(""))
                {
                    cb_1KHz_ft.Text = "default";
                }
                // volt then mean connected
                if (MainV2.comPort.MAV.cs._battery_voltage.Equals(0.0f))
                {
                    MessageBox.Show("未连接飞控");
                    return;
                }
                int dev_opt = (int)MainV2.comPort.GetParam("DEV_OPTIONS");
                int value = dev_opt & (~0x100);
                int ft = 0;
                if(cb_1KHz_ft.SelectedIndex == 1)
                {
                    ft = 0x10000;
                    value &= (~0x80);
                }
                else if (cb_1KHz_ft.SelectedIndex == 2)
                {
                    ft = 0x80;
                    value &= (~0x10000);
                }
                else
                {
                    value &= (~0x80);
                    value &= (~0x10000);
                }
                value |= ft;
                int acc_hz = (int)num_acc_Hz.Value + 1; // 1: chbyII
                int gyr_hz = (int)num_gyr_Hz.Value + 1; // 1: chbyII
                if (MainV2.comPort.setParam("DEV_OPTIONS", (float)value))
                {
                    if (MainV2.comPort.setParam("INS_ACC_UF", (float)acc_hz))
                    {
                        if (MainV2.comPort.setParam("INS_GYRO_UF", (float)gyr_hz))
                        {
                            if (MainV2.comPort.GetParam("DEV_OPTIONS").Equals(value))
                            {
                                if (MainV2.comPort.GetParam("INS_ACC_UF").Equals(acc_hz))
                                {
                                    if (MainV2.comPort.GetParam("INS_GYRO_UF").Equals(gyr_hz))
                                    {
                                        MessageBox.Show("配置滤波器成功", "完成", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                                    }
                                }
                            }
                        }

                    }
                    else
                    {
                        MessageBox.Show("配置滤波器失败1", "错误", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                    }

                }
                else
                {
                    MessageBox.Show("配置滤波器失败", "错误", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                }
            }
        }

        private void rb_8KHz_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_8KHz.Checked)
            {
                if (cb_8KHz_ft.Text.Equals(""))
                {
                    cb_8KHz_ft.Text = "default";
                }
                // volt then mean connected
                if (MainV2.comPort.MAV.cs._battery_voltage.Equals(0.0f))
                {
                    MessageBox.Show("未连接飞控");
                    return;
                }
                int dev_opt = (int)MainV2.comPort.GetParam("DEV_OPTIONS");
                int value = dev_opt | 0x100;
                int ft = 0;
                if (cb_8KHz_ft.SelectedIndex == 1)
                {
                    ft = 0x8000;
                    value &= (~0x40);
                    

                }
                else if (cb_8KHz_ft.SelectedIndex == 2)
                {
                    ft = 0x40;
                    value &= (~0x8000);
                }
                else
                {
                    value &= (~0x8000);
                    value &= (~0x40);
                }

                value |= ft;
                int acc_hz = (int)num_acc_Hz.Value + 1; // 1: chbyII
                int gyr_hz = (int)num_gyr_Hz.Value + 1; // 1: chbyII
                if (MainV2.comPort.setParam("DEV_OPTIONS", (float)value))
                {
                    if (MainV2.comPort.setParam("INS_ACC_UF_8", (float)acc_hz))
                    {
                        if (MainV2.comPort.setParam("INS_GYRO_UF_8", (float)gyr_hz))
                        {
                            if (MainV2.comPort.GetParam("DEV_OPTIONS").Equals(value))
                            {
                                if (MainV2.comPort.GetParam("INS_ACC_UF_8").Equals(acc_hz))
                                {
                                    if (MainV2.comPort.GetParam("INS_GYRO_UF_8").Equals(gyr_hz))
                                    {
                                        MessageBox.Show("配置滤波器成功", "完成", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                                    }
                                }
                            }
                        }

                    }
                    else
                    {
                        MessageBox.Show("配置滤波器失败1", "错误", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                    }

                }
                else
                {
                    MessageBox.Show("配置滤波器失败", "错误", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                }
            }

        }

        private void btn_write_angle_max_Click(object sender, EventArgs e)
        {

            // volt then mean connected
            if (MainV2.comPort.MAV.cs._battery_voltage.Equals(0.0f))
            {
                MessageBox.Show("未连接飞控");
                return;
            }

            float value = (float)num_max_angle.Value;
            if (value > 30)
            {
                value = 30;
            }
            if(value < 8)
            {
                value = 8;
            }

            if (MainV2.comPort.setParam("PSC_ANGM", value))
            {
                if (MainV2.comPort.setParam("ANGLE_MAX", value*100))
                {
                    if (MainV2.comPort.setParam("WPNAV_LOIT_ANGM", value))
                    {
                        if (MainV2.comPort.GetParam("PSC_ANGM").Equals(value)
                            && MainV2.comPort.GetParam("ANGLE_MAX").Equals(value * 100)
                            && MainV2.comPort.GetParam("WPNAV_LOIT_ANGM").Equals(value)
                            )
                        {
                            MessageBox.Show("配置飞行限幅成功", "完成", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        }
                    }
                }
            }
        }
    }
}