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
using Renci.SshNet;
using Renci.SshNet.Common;
using System.Threading;
using LeastSquares;

namespace MissionPlanner.GCSViews.ConfigurationView
{
    partial class ConfigVoltCali : MyUserControl, IActivate
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static List<Firmware.software> softwares = new List<Firmware.software>();
        private readonly Firmware fw = new Firmware();
        private string custom_fw_dir = "";
        private string firmwareurl = "";
        private bool firstrun = true;
        private ProgressReporterDialogue pdr;
        private string drone_class = "";
        private string drone_type = "";


        public ConfigVoltCali()
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


        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //CTRL+R moved to pictureBoxRover_Click
            //CTRL+O moved to CMB_history_label_Click
            //CTRL+C moved to Custom_firmware_label_Click

            if (keyData == (Keys.Control | Keys.Q))
            {
                CustomMessageBox.Show(Strings.TrunkWarning, Strings.Trunk);
                firmwareurl = "https://raw.github.com/diydrones/binary/master/dev/firmwarelatest.xml";
                softwares.Clear();
                UpdateFWList();
                // CMB_history.Visible = false;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void UpdateFWList()
        {
            pdr = new ProgressReporterDialogue();

            pdr.DoWork -= pdr_DoWork;

            pdr.DoWork += pdr_DoWork;

            ThemeManager.ApplyThemeTo(pdr);

            pdr.RunBackgroundOperationAsync();

            pdr.Dispose();
        }

        private void pdr_DoWork(object sender, ProgressWorkerEventArgs e, object passdata = null)
        {
            var fw = new Firmware();
            fw.Progress -= fw_Progress1;
            fw.Progress += fw_Progress;
            softwares = fw.getFWList(firmwareurl);

            foreach (var soft in softwares)
            {
                updateDisplayNameInvoke(soft);
            }
        }

        /// <summary>
        ///     for updating fw list
        /// </summary>
        /// <param name="progress"></param>
        /// <param name="status"></param>
        private void fw_Progress(int progress, string status)
        {
            pdr.UpdateProgressAndStatus(progress, status);
        }

        /// <summary>
        ///     for when updating fw to hardware
        /// </summary>
        /// <param name="progress"></param>
        /// <param name="status"></param>
        private void fw_Progress1(int progress, string status)
        {
            var change = false;

            //if (progress != -1)
            //{
            //    if (this.progress.Value != progress)
            //    {
            //        this.progress.Value = progress;
            //        change = true;
            //    }
            //}
            //if (lbl_status.Text != status)
            //{
            //    lbl_status.Text = status;
            //    change = true;
            //}

            if (change)
                Refresh();
        }

        private void updateDisplayNameInvoke(Firmware.software temp)
        {
            Invoke((MethodInvoker)delegate { updateDisplayName(temp); });
        }

        private void updateDisplayName(Firmware.software temp)
        {
            if (temp.url2560.ToLower().Contains("AR2".ToLower()) ||
                temp.url2560.ToLower().Contains("apm1/APMRover".ToLower()))
            {
                // pictureBoxRover.Text = temp.name;
                // pictureBoxRover.Tag = temp;
            }
            else if (temp.url2560.ToLower().Contains("AP-".ToLower()) ||
                     temp.url2560.ToLower().Contains("apm1/ArduPlane".ToLower()))
            {
                // pictureBoxAPM.Text = temp.name;
                //pictureBoxAPM.Tag = temp;
            }
            else if (temp.url2560.ToLower().Contains("ac2-quad-".ToLower()) ||
                     temp.url2560.ToLower().Contains("1-quad/ArduCopter".ToLower()))
            {
                // pictureBoxQuad.Text = temp.name += " Quad";
                // pictureBoxQuad.Tag = temp;
            }
            else if (temp.url2560.ToLower().Contains("ac2-tri".ToLower()) ||
                     temp.url2560.ToLower().Contains("-tri/ArduCopter".ToLower()))
            {
                // pictureBoxTri.Text = temp.name += " Tri";
                // pictureBoxTri.Tag = temp;
            }
            else if (temp.url2560.ToLower().Contains("ac2-hexa".ToLower()) ||
                     temp.url2560.ToLower().Contains("-hexa/ArduCopter".ToLower()))
            {
                // pictureBoxHexa.Text = temp.name += " Hexa";
                // pictureBoxHexa.Tag = temp;
            }
            else if (temp.url2560.ToLower().Contains("ac2-y6".ToLower()) ||
                     temp.url2560.ToLower().Contains("-y6/ArduCopter".ToLower()))
            {
                // pictureBoxY6.Text = temp.name += " Y6";
                // pictureBoxY6.Tag = temp;
            }
            else if (temp.url2560.ToLower().Contains("ac2-heli-".ToLower()) ||
                     temp.url2560.ToLower().Contains("-heli/ArduCopter".ToLower()))
            {
                // pictureBoxHeli.Text = temp.name += " heli";
                // pictureBoxHeli.Tag = temp;
            }
            else if (temp.url2560.ToLower().Contains("ac2-octaquad-".ToLower()) ||
                     temp.url2560.ToLower()
                         .Contains("-octa-quad/ArduCopter".ToLower()))
            {
                // pictureBoxOctaQuad.Text = temp.name += " Octa Quad";
                // pictureBoxOctaQuad.Tag = temp;
            }
            else if (temp.url2560.ToLower().Contains("ac2-octa-".ToLower()) ||
                     temp.url2560.ToLower()
                         .Contains("-octa/ArduCopter".ToLower()))
            {
                // pictureBoxOcta.Text = temp.name += " Octa";
                // pictureBoxOcta.Tag = temp;
            }
            else if (temp.url2560_2.ToLower().Contains("antennatracker"))
            {
                // pictureAntennaTracker.Text = temp.name;
                // pictureAntennaTracker.Tag = temp;
            }
            else
            {
                log.Info("No Home " + temp.name + " " + temp.url2560);
            }
        }

        private void findfirmware(Firmware.software fwtoupload)
        {
            var dr = CustomMessageBox.Show(Strings.AreYouSureYouWantToUpload + fwtoupload.name + Strings.QuestionMark,
                Strings.Continue, MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    MainV2.comPort.BaseStream.Close();
                }
                catch
                {
                }
                fw.Progress -= fw_Progress;
                fw.Progress += fw_Progress1;

                // var history = (CMB_history.SelectedValue == null) ? "" : CMB_history.SelectedValue.ToString();
                var history = "";

                var updated = fw.update(MainV2.comPortName, fwtoupload, history);

                if (updated)
                {
                    if (fwtoupload.url2560_2 != null && fwtoupload.url2560_2.ToLower().Contains("copter") &&
                        fwtoupload.name.ToLower().Contains("3.1"))
                        CustomMessageBox.Show(Strings.WarningAC31, Strings.Warning);

                    if (fwtoupload.url2560_2 != null && fwtoupload.url2560_2.ToLower().Contains("copter") &&
                        fwtoupload.name.ToLower().Contains("3.2"))
                        CustomMessageBox.Show(Strings.WarningAC32, Strings.Warning);
                }
                else
                {
                    CustomMessageBox.Show(Strings.ErrorUploadingFirmware, Strings.ERROR);
                }
            }
        }

        private void pictureBoxFW_Click(object sender, EventArgs e)
        {
            if (((Control)sender).Tag.GetType() != typeof(Firmware.software))
            {
                CustomMessageBox.Show(Strings.ErrorFirmwareFile, Strings.ERROR);
                return;
            }

            findfirmware((Firmware.software)((Control)sender).Tag);
        }

        private void up_LogEvent(string message, int level = 0)
        {
            //lbl_status.Text = message;
            Application.DoEvents();
        }

        private void up_ProgressEvent(double completed)
        {
            //progress.Value = (int) completed;
            Application.DoEvents();
        }

        private void port_Progress(int progress, string status)
        {
            log.InfoFormat("Progress {0} ", progress);
            //this.progress.Value = progress;
            //this.progress.Refresh();
        }

        private void CMB_history_SelectedIndexChanged(object sender, EventArgs e)
        {
            firmwareurl = ""; // fw.getUrl(CMB_history.SelectedValue.ToString(), "");

            softwares.Clear();
            UpdateFWList();
        }


        private void lbl_px4bl_Click(object sender, EventArgs e)
        {
            try
            {
                MainV2.comPort.Open(false);

                if (MainV2.comPort.BaseStream.IsOpen)
                {
                    MainV2.comPort.doReboot(true, false);
                    CustomMessageBox.Show("Please ignore the unplug and plug back in when uploading flight firmware.");
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                CustomMessageBox.Show("Failed to connect and send the reboot command", Strings.ERROR);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start(@"http://copter.ardupilot.com/wiki/connect-escs-and-motors/#motor_order_diagrams");
            }
            catch
            {
                CustomMessageBox.Show("http://copter.ardupilot.com/wiki/connect-escs-and-motors/#motor_order_diagrams", Strings.ERROR);
            }
        }

        private void picturebox_ph2_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(@"http://www.proficnc.com/?utm_source=missionplanner&utm_medium=click&utm_campaign=mission");
            }
            catch
            {
                CustomMessageBox.Show("http://www.proficnc.com/?utm_source=missionplanner&utm_medium=click&utm_campaign=mission", Strings.ERROR);
            }
        }
        private bool first_add = true;
        private System.Drawing.Point last_loc_ref = new System.Drawing.Point(0, 0);
        private System.Drawing.Point last_loc_split = new System.Drawing.Point(0, 0);
        private System.Drawing.Point last_loc_meas = new System.Drawing.Point(0, 0);
        private int delta_gap = 0;
        private Point[] coordinates = new Point[20];
        private class num_pair
        {

            public num_pair(float ref_inst, float meas_inst1, float meas_inst2)
            {
                num_ref = ref_inst;
                num_meas1 = meas_inst1;
                num_meas2 = meas_inst2;

            }
            public float num_ref;
            public float num_meas1;
            public float num_meas2;

        };
        private List<num_pair> list_num = new List<num_pair>();
        System.Windows.Forms.NumericUpDown curr_numUpDn;

        private void btn_add_points_Click(object sender, EventArgs e)
        {
            // get layout
            if(first_add)
            {
                delta_gap = this.num_ref1.Size.Height + 6;
                last_loc_ref.X = this.num_ref1.Location.X;
                last_loc_ref.Y = this.num_ref1.Location.Y + delta_gap;

                first_add = false;
                curr_numUpDn = this.num_ref1;
            }
            else
            {
                last_loc_ref.Y += delta_gap;

            }

            System.Windows.Forms.NumericUpDown num_ref_new = new System.Windows.Forms.NumericUpDown();
            float num_meas1, num_meas2;

            // 
            num_ref_new.Location = last_loc_ref;
            num_ref_new.Size = this.num_ref1.Size;
            num_ref_new.Increment = 0.05M;
            num_ref_new.DecimalPlaces = 2;

            num_meas1 = MainV2.comPort.MAV.cs.battery_voltage;
            num_meas2 = MainV2.comPort.MAV.cs.battery_voltage2;

            num_pair np_tmp = new num_pair((float)curr_numUpDn.Value, num_meas1, num_meas2);
            this.list_num.Add(np_tmp);

            this.Controls.Add(num_ref_new);
            curr_numUpDn = num_ref_new;
        }

        private void btn_volt_cali_Click(object sender, EventArgs e)
        {

            num_pair np_tmp = new num_pair((float)curr_numUpDn.Value, (float)MainV2.comPort.MAV.cs.battery_voltage, (float)MainV2.comPort.MAV.cs.battery_voltage2);
            this.list_num.Add(np_tmp);

            PointSet mainSet = new PointSet();

            int valid_data = 0;
            foreach(var num in this.list_num)
            {
                if(!(num.num_meas1.Equals(0) && num.num_meas2.Equals(0) && num.num_ref.Equals(0)))
                {
                    coordinates[valid_data].x = (double)(num.num_meas1);
                    coordinates[valid_data].y = (double)(num.num_ref);
                    valid_data++;
                }

            }

            for (int i = 0; (i < valid_data) && (i < 20); i++)
            {
                mainSet.Add(coordinates[i]);
            }

            StraightLine result = mainSet.FindApproximateSolution();

            if(Double.IsNaN(result.a) || Double.IsNaN(result.b))
            {
                MessageBox.Show("抱歉，校准值中有异常值，请检查后重新校准!", "校准值异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int try_times = 0;
            for (try_times = 0; try_times < 5; try_times++)
            {

                // save horizon spd 
                if (MainV2.comPort.setParam("BATT_VM", result.a))
                {

                    if (MainV2.comPort.setParam("BATT_V_OFS", result.b))
                    {
                        MessageBox.Show("恭喜，校准电压1成功!", "校准成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        break;

                    }

                }

            }

            PointSet mainSet2 = new PointSet();
            this.coordinates.Initialize();
            valid_data = 0;
            foreach (var num in this.list_num)
            {
                if (!(num.num_meas1.Equals(0) && num.num_meas2.Equals(0) && num.num_ref.Equals(0)))
                {
                    coordinates[valid_data].x = (double)(num.num_meas2);
                    coordinates[valid_data].y = (double)(num.num_ref);
                    valid_data++;
                }

            }

            for (int i = 0; (i < valid_data) && (i < 20); i++)
            {
                mainSet2.Add(coordinates[i]);
            }

            StraightLine result2 = mainSet2.FindApproximateSolution();

            if (Double.IsNaN(result2.a) || Double.IsNaN(result2.b))
            {
                MessageBox.Show("抱歉，校准值中有异常值，请检查后重新校准!", "校准值异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try_times = 0;
            for (try_times = 0; try_times < 5; try_times++)
            {

                // save horizon spd 
                if (MainV2.comPort.setParam("BATT2_VM", result2.a))
                {

                    if (MainV2.comPort.setParam("BATT2_V_OFS", result2.b))
                    {
                        MessageBox.Show("恭喜，校准电压2成功!", "校准成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        break;

                    }

                }

            }

            if (5 == try_times)
            {
                MessageBox.Show("抱歉，校准值保存到飞控失败，请尝试重新连接飞控后再次尝试!", "校准失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_cali_reset_Click(object sender, EventArgs e)
        {
            this.list_num.Clear();
            this.coordinates.Initialize();
            int try_times = 0;
            for (try_times = 0; try_times < 5; try_times++)
            {

                // save horizon spd 
                if (MainV2.comPort.setParam("BATT_VM", 1) && MainV2.comPort.setParam("BATT2_VM", 1))
                {

                    if (MainV2.comPort.setParam("BATT_V_OFS", 0) && MainV2.comPort.setParam("BATT2_V_OFS", 0))
                    {
                        MessageBox.Show("恭喜，重置校准值成功", "重置成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        break;

                    }

                }

            }

            if (5 == try_times)
            {
                MessageBox.Show("抱歉，校准值重置失败，请尝试重新连接飞控后再次尝试!", "重置失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}