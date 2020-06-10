using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using KMLib;
using KMLib.Feature;
using KMLib.Geometry;
using Core.Geometry;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Core;
using log4net;
using MissionPlanner.Comms;
using MissionPlanner.Utilities;
using System.Diagnostics;
using System.Threading;

using Tamir.SharpSsh;
using System.Collections;

using Microsoft.WindowsAPICodePack.Dialogs;
using Renci.SshNet;
using Renci.SshNet.Common;

namespace MissionPlanner.Log
{
    public partial class LogDownloadMavLink : Form
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        SerialStatus status = SerialStatus.Connecting;
        bool closed;
        string logfile = "";
        uint receivedbytes; // current log file
        uint tallyBytes; // previous downloaded logs
        uint totalBytes; // total expected
        List<MAVLink.mavlink_log_entry_t> logEntries;

        //List<Model> orientation = new List<Model>();

        Object thisLock = new Object();

        enum SerialStatus
        {
            Connecting,
            Createfile,
            Closefile,
            Reading,
            Waiting,
            Done
        }

        public LogDownloadMavLink()
        {
            InitializeComponent();

            //Added by HL below
            // DB ZhaoYJ@2017-02-22 for new log downloader
            // reserve some butts
            // RemoveSomeButton();
            //Added by HL above

            labelBytes.Text = "";

            ThemeManager.ApplyThemeTo(this);

            MissionPlanner.Utilities.Tracking.AddPage(this.GetType().ToString(), this.Text);            
        }

        //Added by HL below
        private void RemoveSomeButton()
        {
            this.tableLayoutPanel3.Controls.Remove(this.BUT_clearlogs);
            //this.tableLayoutPanel3.Controls.Remove(this.BUT_firstperson);
            this.tableLayoutPanel3.Controls.Remove(this.BUT_DLthese);
            //this.tableLayoutPanel3.Controls.Remove(this.BUT_bintolog);
            //this.tableLayoutPanel3.Controls.Remove(this.BUT_redokml);

            this.tableLayoutPanel1.Controls.Remove(this.LabelStatus);
        }
        //Added by HL above

        private void Log_Load(object sender, EventArgs e)
        {
            ClearSerialLog();
            LoadLogList();
        }

        void LoadLogList()
        {
            if (!MainV2.comPort.BaseStream.IsOpen)
            {
                AppendSerialLog(LogStrings.NotConnected);
                BUT_clearlogs.Enabled = false;
                return;
            }
            else
            {
                BUT_clearlogs.Enabled = true;
            }

            CHK_logs.Items.Clear();

            AppendSerialLog(LogStrings.FetchingLogfileList);

            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                try
                {
                    this.logEntries = MainV2.comPort.GetLogList();
                    RunOnUIThread(LoadCheckedList);
                }
                catch (Exception ex)
                {
                    AppendSerialLog(LogStrings.UnhandledException + ex.ToString());
                }

            });
        }

        private void LoadCheckedList()
        {
            if (logEntries != null)
            {
                foreach (var item in logEntries)
                {
                    try
                    {
                        string sizeMB = ((float)item.size / 1024 / 1024).ToString("0.00");
                        string caption = item.id + " " + GetItemCaption(item) + "  (" + sizeMB + " MB)";
                        AddCheckedListBoxItem(caption);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                }

                if (logEntries.Count == 0)
                {
                    AppendSerialLog(LogStrings.NoLogsFound);
                }
                else
                {
                    AppendSerialLog(string.Format(LogStrings.SomeLogsFound, logEntries.Count));
                    AppendSerialLog("\n=================================================");

                }
            }
            status = SerialStatus.Done;
        }

        DateTime GetItemCaption(MAVLink.mavlink_log_entry_t item)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(item.time_utc).ToLocalTime();
        }


        void AddCheckedListBoxItem(string caption)
        {
            RunOnUIThread(new Action(() =>
            {
                if (!CHK_logs.Items.Contains(caption))
                {
                    CHK_logs.Items.Add(caption);
                }
            }));
        }


        void RunOnUIThread(Action a)
        {
            if (closed || this.IsDisposed)
            {
                return;
            }
            this.BeginInvoke(new Action(() =>
            {
                try
                {
                    a();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(LogStrings.UnhandledException + e.ToString());
                }
            }));
        }

        private void BUT_DLall_Click(object sender, EventArgs e)
        {

#if HL
            //Added by HL below
            try
            {
                //string host = "192.168.7.2";
                //string user = "root";
                //string pass = "root";

                string host = "11.0.0.1";
                string user = "root";
                string pass = "9HKBtB4K";

                Sftp sftp = new Sftp(host, user);
                sftp.Password = pass;
                sftp.Connect();

                string path = "/var/APM/logs/";
                ArrayList arrayList = sftp.GetFileList(path);
                foreach (var item in arrayList)
                {
                    if (item is string)
                    {
                        string fileName = item as string;
                        if (fileName.EndsWith(".BIN"))
                        {
                            sftp.Get(path + fileName, save_dir);
                            break;
                        }
                    }
                }

                sftp.Close();
                CustomMessageBox.Show("日志文件下载成功，请到LogData目录下查看", Strings.OK);
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(ex.Message, Strings.ERROR);
            }
            //Added by HL above
#else
            // AB ZhaoYJ@2017-02-23 for save log into dedicated path and file name
            // ========= start =========
            if (status == SerialStatus.Done)
            {
                if (CHK_logs.Items.Count == 0)
                {
                    // try again...
                    LoadLogList();
                    return;
                }
                BUT_DLall.Enabled = false;
                BUT_DLthese.Enabled = false;
                int[] toDownload = GetAllLogIndices().ToArray();

                try
                {
                    Directory.CreateDirectory(Settings.Instance.LogDir);
                }
                catch (Exception ex)
                {
                    AppendSerialLog(string.Format(LogStrings.LogDirectoryError, Settings.Instance.LogDir) + "\r\n" + ex.Message);
                    return;
                }
                
                // AB ZhaoYJ@2017-03-28 for user-defined dir to save log
                // ========= start =========
                string save_dir = "";

                // get save path from user
                var first_entry = logEntries[toDownload[0]]; // mavlink_log_entry_t

                DateTime fristFileDate = GetItemCaption(first_entry);

                string save_file = "[飞行日志]" + fristFileDate.Year + "年" + fristFileDate.Month + "月" + fristFileDate.Day + "日" + fristFileDate.Hour + "时" +
                    fristFileDate.Minute + "分" + fristFileDate.Second + "秒" + ".BIN";

                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.RestoreDirectory = true;
                saveFileDialog1.Filter = "飞行日志|*.BIN";
                saveFileDialog1.Title = "保存飞行日志文件";
                saveFileDialog1.FileName += save_file;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    save_dir = Path.GetDirectoryName(saveFileDialog1.FileName.ToString());
                }
                else
                {
                    return;
                }

                AppendSerialLog(string.Format(LogStrings.DownloadStarting, save_dir));

                System.Threading.Thread t11 = new System.Threading.Thread(
                        delegate ()
                        {
                            DownloadThread(toDownload, save_dir);
                        });
                t11.Name = "Log Download All thread";
                t11.Start();
            }
            // ========= end =========
#endif
        }


        string GetLog(ushort no, string fileName)
        {
            log.Info("GetLog " + no);

            MainV2.comPort.Progress += comPort_Progress;

            status = SerialStatus.Reading;

            // used for log fn
            MAVLink.MAVLinkMessage hbpacket = MainV2.comPort.getHeartBeat();

            if (hbpacket != null)
                log.Info("Got hbpacket length: " + hbpacket.Length);

            // get df log from mav
            using (var ms = MainV2.comPort.GetLog(no))
            {
                if (ms != null)
                    log.Info("Got Log length: " + ms.Length);

                ms.Seek(0, SeekOrigin.Begin);

                status = SerialStatus.Done;

                MAVLink.mavlink_heartbeat_t hb = (MAVLink.mavlink_heartbeat_t)MainV2.comPort.DebugPacket(hbpacket);

                logfile = Settings.Instance.LogDir + Path.DirectorySeparatorChar
                          + MainV2.comPort.MAV.aptype.ToString() + Path.DirectorySeparatorChar
                          + hbpacket.sysid + Path.DirectorySeparatorChar + no + " " + MakeValidFileName(fileName) + ".bin";

                // make log dir
                Directory.CreateDirectory(Path.GetDirectoryName(logfile));

                log.Info("about to write: " + logfile);
                // save memorystream to file
                using (BinaryWriter bw = new BinaryWriter(File.OpenWrite(logfile)))
                {
                    byte[] buffer = new byte[256 * 1024];
                    while (ms.Position < ms.Length)
                    {
                        int read = ms.Read(buffer, 0, buffer.Length);
                        bw.Write(buffer, 0, read);
                    }
                }
            }

            log.Info("about to convertbin: " + logfile);

            // create ascii log
            BinaryLog.ConvertBin(logfile, logfile + ".log");

            //update the new filename
            logfile = logfile + ".log";

            // rename file if needed
            log.Info("about to GetFirstGpsTime: " + logfile);
            // get gps time of assci log
            DateTime logtime = new DFLog().GetFirstGpsTime(logfile);

            // rename log is we have a valid gps time
            if (logtime != DateTime.MinValue)
            {
                string newlogfilename = Settings.Instance.LogDir + Path.DirectorySeparatorChar
                                        + MainV2.comPort.MAV.aptype.ToString() + Path.DirectorySeparatorChar
                                        + hbpacket.sysid + Path.DirectorySeparatorChar +
                                        logtime.ToString("yyyy-MM-dd HH-mm-ss") + ".log";
                try
                {
                    File.Move(logfile, newlogfilename);
                    // rename bin as well
                    File.Move(logfile.Replace(".log", ""), newlogfilename.Replace(".log", ".bin"));
                    logfile = newlogfilename;
                }
                catch
                {
                    CustomMessageBox.Show(Strings.ErrorRenameFile + " " + logfile + "\nto " + newlogfilename,
                        Strings.ERROR);
                }
            }

            MainV2.comPort.Progress -= comPort_Progress;

            return logfile;
        }

        protected override void OnClosed(EventArgs e)
        {
            this.closed = true;
            MainV2.comPort.Progress -= comPort_Progress;

            base.OnClosed(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (status == SerialStatus.Reading)
            {
                if (CustomMessageBox.Show(LogStrings.CancelDownload, "Cancel Download", MessageBoxButtons.YesNo) ==
                    System.Windows.Forms.DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            base.OnClosing(e);
        }

        private string MakeValidFileName(string fileName)
        {
            return fileName.Replace('/', '-').Replace('\\', '-').Replace(':', '-').Replace('?', ' ').Replace('"', '\'').Replace('<', '[').Replace('>', ']').Replace('|', ' ');
        }

        void comPort_Progress(int progress, string status)
        {
            receivedbytes = (uint)progress;

            UpdateProgress(0, totalBytes, tallyBytes + receivedbytes);
        }

        void CreateLog(string logfile)
        {
            TextReader tr = new StreamReader(logfile);
            //
            AppendSerialLog(string.Format(LogStrings.CreatingKmlPrompt, Path.GetFileName(logfile)));

            LogOutput lo = new LogOutput();

            while (tr.Peek() != -1)
            {
                lo.processLine(tr.ReadLine());
            }

            tr.Close();

            try
            {
                lo.writeKML(logfile + ".kml");
            }
            catch
            {
            } // usualy invalid lat long error
            status = SerialStatus.Done;
        }

        // AB ZhaoYJ@2017-02-23 for user-defined dir to save log
        // private void DownloadThread(int[] selectedLogs)
        private void DownloadThread(int[] selectedLogs, string save_path)
        {
            try
            {
                status = SerialStatus.Reading;

                totalBytes = 0;
                tallyBytes = 0;
                // receivedbytes = 0;

                foreach (int a in selectedLogs)
                {
                    var entry = logEntries[a]; // mavlink_log_entry_t
                    totalBytes += entry.size;
                }

                    // string[] nameParts = fileName.Split(' ', '-', ':', '/');

                    // string save_file = @save_path + "1.bin";

                UpdateProgress(0, totalBytes, 0);

                // AB ZhaoYJ@2017-02-23 for user-defined dir to save log
                // download log files via SFTP, remove mavlink downloader because its slow speed
                // ========= start ==========
                try
                {
                    //string host = "192.168.7.2";
                    //string user = "root";
                    //string pass = "root";

                    string host = "11.0.0.1";
                    string user = "root";
                    string pass = "9HKBtB4K";

                    Sftp sftp = new Sftp(host, user);
                    sftp.Password = pass;
                    sftp.Connect();

                    string path = "/var/APM/logs/";

                    foreach (int a in selectedLogs)
                    {

                        var entry = logEntries[a]; // mavlink_log_entry_t

                        DateTime fileDate = GetItemCaption(entry);

                        // string[] nameParts = fileName.Split(' ', '-', ':', '/');

                        AppendSerialLog(string.Format(LogStrings.FetchingLog, fileDate.ToString()));

                        string save_file = @save_path + "\\" + "[飞行日志]" + fileDate.Year + "年" + fileDate.Month + "月" + fileDate.Day + "日" + fileDate.Hour + "时" +
                            fileDate.Minute + "分" + fileDate.Second + "秒" + ".BIN";
                        // string save_file = @save_path + "1.bin";

                        try
                        {
                            sftp.Get(path + entry.id.ToString() + ".BIN", save_file);
                        }
                        catch (Exception ex)
                        {
                            CustomMessageBox.Show("日志文件 [" + entry.id.ToString() + " " + fileDate + "] 可能已不存在，请重新刷新日志列表", Strings.ERROR);
                        }
                        // var logname = GetLog(entry.id, fileName);
                        // CreateLog(logname);

                        tallyBytes += logEntries[a].size;
                        // receivedbytes = 0;
                        UpdateProgress(0, totalBytes, tallyBytes);
                    }
                    sftp.Close();
                    CustomMessageBox.Show("日志文件下载成功", Strings.Done);

                }
                catch (Exception ex)
                {
                    CustomMessageBox.Show(ex.Message, Strings.ERROR);
                }
                // ========= end ==========


                UpdateProgress(0, totalBytes, totalBytes);
                AppendSerialLog("=================================================");

                AppendSerialLog("\n所有日志文件下载成功.");
                Console.Beep();
            }
            catch (Exception ex)
            {
                AppendSerialLog("Error in log " + ex.Message);
            }

            RunOnUIThread(() =>
                {
                    BUT_DLall.Enabled = true;
                    BUT_DLthese.Enabled = true;
                    status = SerialStatus.Done;
                });
        }

        IEnumerable<int> GetSelectedLogIndices()
        {
            foreach (int i in CHK_logs.CheckedIndices)
            {
                yield return i;
            }
        }

        IEnumerable<int> GetAllLogIndices()
        {
            for (int i = 0, n = logEntries.Count; i < n; i++)
            {
                yield return i;
            }
        }

        private void UpdateProgress(uint min, uint max, uint current)
        {
            RunOnUIThread(() =>
            {
                progressBar1.Minimum = (int)min;
                progressBar1.Maximum = (int)max;
                progressBar1.Value = (int)current;
                progressBar1.Visible = (current < max);

                if (current < max)
                {
                    labelBytes.Text = current.ToString();
                }
                else
                {
                    labelBytes.Text = "";
                }
            });

        }

        private void BUT_DLthese_Click(object sender, EventArgs e)
        {
            log.Info("Download log now... ");


            //using (var fbd = new FolderBrowserDialog()) //  openFileDialog_alllog = new OpenFileDialog())
            //// using(var ofd = new OpenFileDialog())
            //{
            //    DialogResult result = fbd.ShowDialog();
            //    save_dir = fbd.SelectedPath;
            //    if (!(result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath)))
            //    {
            //        System.Windows.Forms.MessageBox.Show("目录 " + save_dir + "不存在！");
            //        return;
            //    }
            //}
            // ========= end =========


            if (status == SerialStatus.Done)
            {
                int[] toDownload = GetSelectedLogIndices().ToArray();
                if (toDownload.Length == 0)
                {
                    AppendSerialLog(LogStrings.NothingSelected);
                }
                else
                {

                    // AB ZhaoYJ@2017-03-28 for user-defined dir to save log
                    // ========= start =========
                    string save_dir = "";

                    // get save path from user
                    var first_entry = logEntries[toDownload[0]]; // mavlink_log_entry_t

                    DateTime fristFileDate = GetItemCaption(first_entry);

                    string save_file = "[飞行日志]" + fristFileDate.Year + "年" + fristFileDate.Month + "月" + fristFileDate.Day + "日" + fristFileDate.Hour + "时" +
                        fristFileDate.Minute + "分" + fristFileDate.Second + "秒" + ".BIN";

                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.RestoreDirectory = true;
                    saveFileDialog1.Filter = "飞行日志|*.BIN";
                    saveFileDialog1.Title = "保存飞行日志文件";
                    saveFileDialog1.FileName += save_file;
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        save_dir = Path.GetDirectoryName(saveFileDialog1.FileName.ToString());
                    }
                    else
                    {
                        return;
                    }

                    AppendSerialLog(string.Format(LogStrings.DownloadStarting, save_dir));

                    BUT_DLall.Enabled = false;
                    BUT_DLthese.Enabled = false;
                    System.Threading.Thread t11 = new System.Threading.Thread(delegate () { DownloadThread(toDownload, save_dir); });
                    t11.Name = "Log download single thread";
                    t11.Start();
                }
            }
        }

        private void BUT_clearlogs_Click(object sender, EventArgs e)
        {
            if (CustomMessageBox.Show(LogStrings.Confirmation, "sure", MessageBoxButtons.YesNo) ==
                System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    MainV2.comPort.EraseLog();
                    AppendSerialLog(LogStrings.EraseComplete);
                    status = SerialStatus.Done;
                    CHK_logs.Items.Clear();
                }
                catch (Exception ex)
                {
                    CustomMessageBox.Show(ex.Message, Strings.ERROR);
                }
            }
        }

        private void BUT_redokml_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
            {
                openFileDialog1.Filter = "*.log|*.log";
                openFileDialog1.FilterIndex = 2;
                openFileDialog1.RestoreDirectory = true;
                openFileDialog1.Multiselect = true;
                try
                {
                    openFileDialog1.InitialDirectory = Settings.Instance.LogDir + Path.DirectorySeparatorChar;
                }
                catch
                {
                } // incase dir doesnt exist

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    foreach (string logfile in openFileDialog1.FileNames)
                    {
                        AppendSerialLog(Environment.NewLine + Environment.NewLine +
                            string.Format(LogStrings.ProcessingLog, logfile));
                        this.Refresh();
                        LogOutput lo = new LogOutput();
                        try
                        {
                            TextReader tr = new StreamReader(logfile);

                            while (tr.Peek() != -1)
                            {
                                lo.processLine(tr.ReadLine());
                            }

                            tr.Close();
                        }
                        catch (Exception ex)
                        {
                            AppendSerialLog(LogStrings.ErrorProcessingLogfile + Environment.NewLine + ex.ToString());
                        }

                        lo.writeKML(logfile + ".kml");

                        AppendSerialLog(LogStrings.Done);
                    }
                }
            }
        }

        private void AppendSerialLog(string msg)
        {
            RunOnUIThread(new Action(() =>
            {
                TXT_seriallog.AppendText(msg + Environment.NewLine);
            }));
        }


        private void ClearSerialLog()
        {
            RunOnUIThread(new Action(() =>
            {
                TXT_seriallog.Clear();
            }));
        }

        private void BUT_firstperson_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
            {
                openFileDialog1.Filter = "*.log|*.log";
                openFileDialog1.FilterIndex = 2;
                openFileDialog1.RestoreDirectory = true;
                openFileDialog1.Multiselect = true;
                try
                {
                    Directory.CreateDirectory(Settings.Instance.LogDir);
                    openFileDialog1.InitialDirectory = Settings.Instance.LogDir + Path.DirectorySeparatorChar;
                }
                catch
                {
                } // incase dir cannot be created

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    foreach (string logfile in openFileDialog1.FileNames)
                    {
                        AppendSerialLog(Environment.NewLine + Environment.NewLine +
                            string.Format(LogStrings.ProcessingLog, logfile));
                        this.Refresh();

                        LogOutput lo = new LogOutput();

                        try
                        {
                            TextReader tr = new StreamReader(logfile);

                            while (tr.Peek() != -1)
                            {
                                lo.processLine(tr.ReadLine());
                            }

                            tr.Close();
                        }
                        catch (Exception ex)
                        {
                            AppendSerialLog(LogStrings.ErrorProcessingLogfile + Environment.NewLine + ex.Message);
                            continue;
                        }

                        lo.writeKMLFirstPerson(logfile + "-fp.kml");

                        AppendSerialLog(LogStrings.Done);
                    }
                }
            }
        }

        private void BUT_bintolog_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Binary Log|*.bin";

                ofd.ShowDialog();

                if (File.Exists(ofd.FileName))
                {
                    using (SaveFileDialog sfd = new SaveFileDialog())
                    {
                        sfd.Filter = "log|*.log";

                        DialogResult res = sfd.ShowDialog();

                        if (res == System.Windows.Forms.DialogResult.OK)
                        {
                            BinaryLog.ConvertBin(ofd.FileName, sfd.FileName);
                        }
                    }
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void TXT_seriallog_TextChanged(object sender, EventArgs e)
        {

        }

        private void CHK_logs_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void labelBytes_Click(object sender, EventArgs e)
        {

        }

        private void downsyslog_Click(object sender, EventArgs e)
        {
            // if (status == SerialStatus.Done)
            {
                // AB ZhaoYJ@2017-03-28 for user-defined dir to save log
                // ========= start =========
                string save_dir = "";

                DateTime now = System.DateTime.Now;

                string save_file = "[系统日志]" + now.Year + "年" + now.Month + "月" + now.Day + "日" + now.Hour + "时" +
                    now.Minute + "分" + now.Second + "秒" + ".slog";

                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.RestoreDirectory = true;
                saveFileDialog1.Filter = "系统日志|*.slog";
                saveFileDialog1.Title = "保存系统日志文件";
                saveFileDialog1.FileName += save_file;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    save_dir = Path.GetDirectoryName(saveFileDialog1.FileName.ToString());
                }
                else
                {
                    return;
                }

                // download syslog
                //update version
                string farring_IP = "11.0.0.1";
                string user_name = "root";
                string farring_pwd = "9HKBtB4K";


                using (var client = new SshClient(farring_IP, user_name, farring_pwd))
                {
                    // connect
                    try
                    {
                        client.Connect();
                    }
                    catch (Exception ex)
                    {
                        CustomMessageBox.Show(ex.Message, Strings.ERROR);
                    }
                    if (!client.IsConnected)
                    {
                        CustomMessageBox.Show("无法连接到飞控板，请重试!");
                        return;
                    }


                    try
                    {
                        // tar syslog
                        client.RunCommand("cd /var/APM/logs");
                        client.RunCommand("rm -rf /var/APM/logs/tmp_syslog; mkdir -p /var/APM/logs/tmp_syslog;");
                        client.RunCommand("cp -rf /var/log/ /var/APM/logs/tmp_syslog; cp -f /var/APM/logs/run.log /var/APM/logs/tmp_syslog;  cp -f /var/APM/logs/update_log /var/APM/logs/tmp_syslog; cp -f /root/app /var/APM/logs/tmp_syslog; cp -f /var/APM/ArduCopter.stg /var/APM/logs/tmp_syslog; cp -f /root/SN /var/APM/logs/tmp_syslog");
                        client.RunCommand("cd /var/APM/logs; tar -czf tmp.tar.gz tmp_syslog;sync;sync");
                    }
                    catch (Exception ex)
                    {
                        CustomMessageBox.Show(ex.Message, Strings.ERROR);
                    }

                    // get syslog
                    var sftp_inst = new SftpClient(farring_IP, 22, user_name, farring_pwd);
                    sftp_inst.Connect();
                    if (sftp_inst.IsConnected)
                    {
                        var fileStream = File.OpenWrite(@save_dir + "\\" + save_file);
                        if (fileStream != null)
                        {
                            // 
                            try
                            {
                                sftp_inst.DownloadFile("/var/APM/logs/tmp.tar.gz", fileStream);
                                sftp_inst.Disconnect();
                                sftp_inst.Dispose();
                            }
                            catch (Exception ex)
                            {
                                CustomMessageBox.Show(ex.Message, Strings.ERROR);
                            }

                        }
                    }
                    else
                    {
                        CustomMessageBox.Show("无法连接到飞控板，请重试!");
                        return;
                    }

                    try
                    {
                        // clean up
                        client.RunCommand("rm -fr /var/APM/logs/tmp.tar.gz /var/APM/logs/tmp_syslog");

                        client.Disconnect();
                        client.Dispose();
                    }
                    catch (Exception ex)
                    {
                        CustomMessageBox.Show(ex.Message, Strings.ERROR);
                    }

                    CustomMessageBox.Show("系统日志文件下载成功", Strings.Done);

                }
            }
        }

        private void sysmem_Click(object sender, EventArgs e)
        {
            // for debug
            // string test_str = "Filesystem      Size  Used Avail Use% Mounted on\nudev             10M     0   10M   0% /dev\ntmpfs            99M  440K   98M   1% /run\n/dev/mmcblk1p2  3.4G  2.4G  843M  74% /\ntmpfs           246M     0  246M   0% /dev/shm\ntmpfs           246M     0  246M   0% /sys/fs/cgroup\ntmpfs           100M     0  100M   0% /run/user\ntmpfs           5.0M     0  5.0M   0% /run/lock\n/dev/mmcblk0p1   30G  2.8G   27G  10% /var/APM/logs\n/dev/mmcblk1p1   96M   76M   21M  79% /boot/uboot\n";

            // TODO: check system memory, status...
            // download syslog
            //update version
            string farring_IP = "11.0.0.1";
            string user_name = "root";
            string farring_pwd = "9HKBtB4K";


            using (var client = new SshClient(farring_IP, user_name, farring_pwd))
            {
                // connect
                try
                {
                    client.Connect();
                }
                catch (Exception ex)
                {
                    CustomMessageBox.Show(ex.Message, Strings.ERROR);
                }
                if (!client.IsConnected)
                {
                    CustomMessageBox.Show("无法连接到飞控板，请重试!");
                    return;
                }
                try
                {
                    string res = client.RunCommand("df -h").Result;

                    string[] info = res.Split('\n');
                    const int padding_len = 12;
                    string result_str = "内存名称".PadRight(padding_len) + "总大小".PadRight(padding_len - 4) + "已使用".PadRight(padding_len - 3) + "未使用".PadRight(padding_len - 3) + "内存占用率".PadRight(padding_len / 2) + "\n\n";
                    string res1;
                    foreach (string info_item in info)
                    {
                        // TF card
                        if (info_item.Contains("/dev/mmcblk0p1"))
                        {
                            res1 = @info_item;
                            int unused_idx = res1.LastIndexOf("%") + 1;
                            int str_len = res1.Length;
                            string unused_str = res1.Substring(unused_idx, (str_len - unused_idx));
                            res1 = res1.Replace(unused_str, "");
                            res1 = res1.Replace("/dev/mmcblk0p1", "日志分区".PadRight(padding_len/4));
                            res1 = Regex.Replace(res1, @"\s+", " ");
                            result_str += res1.Replace(" ", "         ") + "\n\n";

                        }
                        else if (info_item.Contains("/dev/mmcblk1p1"))
                        {
                            res1 = info_item;
                            int unused_idx = res1.LastIndexOf("%") + 1;
                            int str_len = res1.Length;
                            string unused_str = res1.Substring(unused_idx, (str_len - unused_idx));
                            res1 = res1.Replace(unused_str, "");
                            res1 = res1.Replace("/dev/mmcblk1p1", "启动分区".PadRight(padding_len/4));
                            res1 = Regex.Replace(res1, @"\s+", " ");
                            result_str += res1.Replace(" ", "         ") + "\n\n";
                        }
                        if (info_item.Contains("/dev/mmcblk1p2"))
                        {
                            res1 = info_item;
                            int unused_idx = res1.LastIndexOf("%") + 1;
                            int str_len = res1.Length;
                            string unused_str = res1.Substring(unused_idx, (str_len - unused_idx));
                            res1 = res1.Replace(unused_str, "");
                            res1 = res1.Replace("/dev/mmcblk1p2", "系统分区".PadRight(padding_len/4));
                            res1 = Regex.Replace(res1, @"\s+", " ");
                            result_str += res1.Replace(" ", "        ") + "\n\n";
                        }
                    }
                    CustomMessageBox.Show(result_str, Strings.Done);

                }
                catch (Exception ex)
                {
                    CustomMessageBox.Show(ex.Message, Strings.ERROR);
                }

                client.Disconnect();
                client.Dispose();
            }
        }
    }
}