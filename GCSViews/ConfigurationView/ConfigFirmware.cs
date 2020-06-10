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

namespace MissionPlanner.GCSViews.ConfigurationView
{
    partial class ConfigFirmware : MyUserControl, IActivate
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


        public ConfigFirmware()
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
            
            lbl_Custom_firmware_label.Visible = true;
            lbl_dlfw.Visible = true;
            
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
            Invoke((MethodInvoker) delegate { updateDisplayName(temp); });
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
            if (((Control) sender).Tag.GetType() != typeof (Firmware.software))
            {
                CustomMessageBox.Show(Strings.ErrorFirmwareFile, Strings.ERROR);
                return;
            }

            findfirmware((Firmware.software) ((Control) sender).Tag);
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

        //Show list of previous firmware versions (old CTRL+O shortcut)
        private void CMB_history_label_Click(object sender, EventArgs e)
        {
            // CMB_history.Enabled = false;

            //CMB_history.Items.Clear();
            //CMB_history.Items.AddRange(fw.gholdurls);
            //CMB_history.Items.AddRange(fw.gcoldurls);
            //CMB_history.DisplayMember = "Value";
            //CMB_history.ValueMember = "Key";
            //CMB_history.DataSource = fw.niceNames;

            //CMB_history.Enabled = true;
            //CMB_history.Visible = true;
            //CMB_history_label.Visible = false;
        }

        //Load custom firmware (old CTRL+C shortcut)
        private void Custom_firmware_label_Click(object sender, EventArgs e)
        {
            if (!MainV2.comPort.MAV.cs.armed) // make sure not armed, WiFi mode will stop app
            {
                if (MainV2.comPort.setParam("WF_EN", 1))
                {
                    MessageBox.Show("升级飞控版本：\n\n  请使用电脑无线网卡连接WiFi热点“FA-xxxx”", "提示：升级方法说明", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            using (var fd = new OpenFileDialog { Filter = "飞控版本 (*.fc;)|*.fc;|All files (*.*)|*.*" })
            {
                if (Directory.Exists(custom_fw_dir))
                    fd.InitialDirectory = custom_fw_dir;
                fd.ShowDialog();
                if (File.Exists(fd.FileName))
                {
                    fw.UploadFlash(fd.FileName, drone_class, drone_type, 0);
                }
            }

        }

        private void lbl_devfw_Click(object sender, EventArgs e)
        {
            CustomMessageBox.Show(Strings.BetaWarning, Strings.Beta);
            firmwareurl = "https://raw.github.com/diydrones/binary/master/dev/firmware2.xml";
            softwares.Clear();
            UpdateFWList();
            //CMB_history.Visible = false;
        }
        

        private void lbl_dlfw_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("http://firmware.ardupilot.org/");
            }
            catch
            {
                CustomMessageBox.Show("Can not open url http://firmware.ardupilot.org/", Strings.ERROR);
            }
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // TODO: set vehicle type: quad, hexa, octa
            if (type_cmb.Items != null && type_cmb.Items.Count > 0 && type_cmb.SelectedItem != null)
                drone_class = this.type_cmb.Text;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // TODO: set vehicle frame: +, x, v           
            if (frame_cmb.Items != null && frame_cmb.Items.Count > 0 && frame_cmb.SelectedItem != null)
                drone_type = this.frame_cmb.Text;
        }

        private void mod_type_frame_Click(object sender, EventArgs e)
        {
            // validation
            // validate copter type
            int int_drone_class = 0;
            switch (drone_class)
            {
                case "四轴飞行器":
                    int_drone_class = (int)MainV2.Motor_frame_class.MOTOR_FRAME_QUAD;
                    break;
                case "六轴飞行器":
                    // update app
                    int_drone_class = (int)MainV2.Motor_frame_class.MOTOR_FRAME_HEXA;
                    break;
                case "八轴飞行器":
                    int_drone_class = (int)MainV2.Motor_frame_class.MOTOR_FRAME_OCTA;
                    break;
            }

           if (!int_drone_class.Equals(MainV2.comPort.GetParam("FRAME_CLASS")))
            {
                // cancel
                if (MessageBox.Show("请确认：飞行器类型变更!!! \n\n从<" + MainV2.comPort.MAV.cs.copter_type + "> 升级为 <" + drone_class + ">？", "确认",
MessageBoxButtons.OKCancel) != DialogResult.OK)
                {
                    MessageBox.Show("已取消更改机型!");
                    return;
                }
            }


            // validate copter frame
            int int_drone_type = drone_type.Equals("十字型") ? (int)MainV2.Motor_frame_type.MOTOR_FRAME_TYPE_PLUS : (drone_type.Equals("X 字型") ? (int)MainV2.Motor_frame_type.MOTOR_FRAME_TYPE_X : -1);
            if (int_drone_type.Equals(-1))
            {
                MessageBox.Show("机架类型选择错误，暂时只支持 <十字型> 和 <X 字型> 机架，请重新选择!");
                return;
            }

            // read frame current for validating
            int orig_type = Convert.ToInt32(MainV2.comPort.GetParam("FRAME_TYPE"));
            if (!orig_type.Equals(int_drone_type))
            {
                if (MessageBox.Show("请确认：机架类型变更!!! \n\n从 < " + ((orig_type.Equals((int)MainV2.Motor_frame_type.MOTOR_FRAME_TYPE_PLUS)) ? "十字型" : "X 字型") + " > 升级为 < " + drone_type + " >？", "确认",
MessageBoxButtons.OKCancel) != DialogResult.OK)
                {
                    MessageBox.Show( "已取消更改机型!");
                    return;
                }
                // CustomMessageBox.Show("请确认：机架类型变更!!! \n\n从<" + ((orig_frame.Equals(0)) ? "十字型" : "X 字型") + "> 升级为 <" + vframe + ">？");
            }

            try
            {
                
                int try_times = 0;
                for (try_times = 0; try_times < 5; try_times++)
                {
                    MainV2.comPort.setParam("FRAME_CLASS", int_drone_class);
                    Thread.Sleep(50);

                    if (Convert.ToInt32(MainV2.comPort.GetParam("FRAME_CLASS")).Equals(int_drone_class))
                    {

                        MainV2.comPort.setParam("FRAME_TYPE", int_drone_type);
                        Thread.Sleep(1000);

                        if (Convert.ToInt32(MainV2.comPort.GetParam("FRAME_TYPE")).Equals(int_drone_type))
                        {
                            break;
                        }
                    }
                }

                if (5 == try_times)
                {
                    MessageBox.Show("抱歉，飞控机型更改失败，请尝试重新连接飞控后，再次配置[12]!", "机型配置失败",
    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // MessageBox.Show("");
                    MessageBox.Show("恭喜，飞控机型配置成功，请重新启动飞控!", "机型配置成功",
MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch
            {
                MessageBox.Show("抱歉，飞控机型更改失败，请尝试重新连接飞控后，再次配置[13]!", "机型配置失败",
MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void lb_recovery_Click(object sender, EventArgs e)
        {

            if (!Password.VerifyPassword())
            {
                MessageBox.Show("密码输入错误，请重新输入!");
                return;
            }

            string farring_IP = Strings.GCS_ip;
            string user_name = Strings.GCS_user;
            string farring_pwd = Strings.GCS_pwd;

            using (var client = new SshClient(farring_IP, user_name, farring_pwd))
            {
                // connect
                try
                {
                    client.Connect();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("无法连接到飞控，请确认是否已连接飞控[0]!");
                    return;
                }

                if (!client.IsConnected)
                {
                    MessageBox.Show("无法连接到飞控，请确认是否已连接飞控[1]!");
                    return;
                }

                try
                {
                    client.RunCommand("rm -f /var/APM/ArduCopter_3_6.stg; sync;");
                    string cmd = "echo " + DateTime.Now.ToString() + ": recovery to manufactoring!" + " >> " + " /var/APM/logs/update_log";
                    client.RunCommand(cmd);

                    MessageBox.Show("恭喜，恢复出厂设置成功!");

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Strings.ERROR);
                }

                client.Disconnect();
                client.Dispose();
            }

        }

        private void btn_refresh_sn_Click(object sender, EventArgs e)
        {
            // if (firstrun)
            {
                int try_conn = 0;
                while (!MainV2.connectOK)
                {
                    if (try_conn++ > 2)
                    {
                        CustomMessageBox.Show("抱歉，飞控信息刷新失败，请尝试重新连接飞控后，再次刷新!");
                        return;
                    }
                    MainV2.instance.connectFC("Serial");
                }

                // first display SN num
                string sub_sn_num = ((int)(MainV2.comPort.GetParam("VEHICEL_ID"))).ToString("d6");
                this.SN_info.Text = "产品序号:   30" + sub_sn_num;

                string sub_fw_ver = (MainV2.comPort.GetParam("FW_VER")).ToString("0.00");
                this.fw_ver.Text = "飞控版本:   " + sub_fw_ver;

                try
                {
                    float manu_dt_f = MainV2.comPort.GetParam("MANU_DT");
                    float exp_dl_f = MainV2.comPort.GetParam("EXPIRED_DL");
                    DateTime dt_factory = ConvertIntDateTime(manu_dt_f);
                    this.release_info.Text = "出厂日期:   " + dt_factory.ToString("yyyy-MM-dd HH:mm");


                    int f_type = Convert.ToInt32(MainV2.comPort.GetParam("FRAME_TYPE")); // x, +
                    int f_class = Convert.ToInt32(MainV2.comPort.GetParam("FRAME_CLASS")); // quad,hexa,octa

                    if (f_class <= (int)MainV2.Motor_frame_class.MOTOR_FRAME_OCTA)
                    {
                        this.type_cmb.SelectedIndex = f_class - 1;
                    }
                    if (f_type <= (int)MainV2.Motor_frame_type.MOTOR_FRAME_TYPE_X)
                    {
                        this.frame_cmb.SelectedIndex = f_type;
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Strings.ERROR);
                }

            }
        }

        private void btn_sn_Click(object sender, EventArgs e)
        {
            tb_sn_TextChanged(sender, e); // support flash now for tmply
            // updateSN();
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

        public void updateSN()
        {
            string farring_IP = Strings.GCS_ip;
            string user_name = Strings.GCS_user;
            string farring_pwd = Strings.GCS_pwd;

            if (tb_sn_new.Text.Length < 8)
            {
                MessageBox.Show("序列号长度有误，请重试!");
                tb_sn_new.Text = "";
                return;
            }

            using (var client = new SshClient(farring_IP, user_name, farring_pwd))
            {
                // connect 
                client.Connect();
                if (!client.IsConnected)
                {
                    MessageBox.Show("无法连接到飞控板，请重试!");
                    return;
                }

                try
                {
                    string SN_str = tb_sn_new.Text.Substring(2, 6);

                    // pre_cmd = "sed -ir '/exit 1/c ' /root/WU810N-AP/control_ap";
                    
                    string pre_cmd = "sed -ir '/AP_SSID=/c " + "AP_SSID=FA-" + SN_str + "' /root/WU810N-AP/control_ap";
                    client.RunCommand(pre_cmd);
                    pre_cmd = "sed -ir '/ssid=/c " + "ssid=FA-" + SN_str + "' /root/hostapd.conf";
                    client.RunCommand(pre_cmd);

                    long manu_dt = (long)ConvertDateTimeInt(DateTime.Now);
                    pre_cmd = "cat /var/APM/user_data";
                    string user_data = client.RunCommand(pre_cmd).Result;
                    string[] data_list = user_data.Split(' ');
                    string user_data_new = "";
                    if(data_list.Length < 6)
                    {
                        user_data_new = " 1000 0 1000 0";
                    }
                    else
                    {
                        for(int ii = 2; ii < data_list.Length; ii++)
                        {
                            user_data_new += " " + data_list[ii];
                        }
                    }

                    // pre_cmd = "echo " + user_data_new + " > /var/APM/user_data";
                    pre_cmd = "echo " + int.Parse(SN_str).ToString() + " " + manu_dt.ToString() + user_data_new.Replace("\n", "") + " > /var/APM/user_data";
                    client.RunCommand(pre_cmd);

                    pre_cmd = "sync; sync;"; 
                    client.RunCommand(pre_cmd);

                    MessageBox.Show("恭喜，序列号配置成功，请等待飞控重新启动!");
                    tb_sn_new.Text = "";

                }
                catch(Exception e)
                {
                    MessageBox.Show("抱歉，序列号配置失败，请尝试重新连接飞控后，再次升级飞控[13]!" + e.Message);
                }

                // update CHANGE.TXT
                // notify user to reboot

                client.Disconnect();
                client.Dispose();
            }

        }

        private void tb_sn_TextChanged(object sender, EventArgs e)
        {
            if ((tb_sn_new.Text.Length == 8) && tb_sn_new.Text.Contains("30"))
            {
                fw.UploadFlash("./FW-V3.20-20180511.fc", drone_class, drone_type, 0);

                updateSN();
            }
        }
    }
}