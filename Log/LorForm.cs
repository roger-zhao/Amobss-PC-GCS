using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tamir.SharpSsh;

namespace FileExplorer
{
    public partial class LogForm : Form
    {
        string substringDirectory; // store last part of full path name
        private string substringFile;

        public LogForm()
        {
            InitializeComponent();
            // getLogList_func();
            cprobar_download.Visible = false ;
            cprobar_download.Value = 0;
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        public void PopulateTreeView(
            string directoryValue, TreeNode parentNode)
        {
            string[] directoryArray =
                Directory.GetDirectories(directoryValue);

            try
            {
                string[] fileArray = Directory.GetFiles(directoryValue);
                if (fileArray.Length != 0)
                {
                    foreach (string file in fileArray)
                    {
                        substringFile = Path.GetFileName(file);
                        TreeNode myNode2 = new TreeNode(substringFile);
                        parentNode.Nodes.Add(myNode2);
                    }
                }
                if (directoryArray.Length != 0)
                {
                    foreach (string directory in directoryArray)
                    {
                        substringDirectory =
                            Path.GetFileNameWithoutExtension(directory);
                        TreeNode myNode = new TreeNode(substringDirectory);

                        parentNode.Nodes.Add(myNode);

                        PopulateTreeView(directory,myNode);
                    }
                }

            }
            catch (UnauthorizedAccessException)
            {
                parentNode.Nodes.Add("Acess denied");

            }
        }

        class st_log_item
        {
            public st_log_item(string name, string date, string time, string size)
            {
                log_name = name;
                log_date = date;
                log_time = time;
                log_size = size;
            }

            public string log_name;
            public string log_date;
            public string log_time;
            public string log_size; // Byte
        };

        private List<st_log_item> logList = new List<st_log_item>();

        private void getLogList_func()
        {

            directoryTreeView.Nodes.Clear();
            logList.Clear();
            // get log file list

            string farring_IP = "11.0.0.1";
            string user_name = "root";
            string farring_pwd = " ";

            using (var client = new SshClient(farring_IP, user_name, farring_pwd))
            {
                // connect
                try
                {
                    client.Connect();
                }
                catch (Exception ex)
                {
                    CustomMessageBox.Show("无法连接到飞控，请确认是否已连接飞控WiFi热点[0]!");
                    return;
                }

                if (!client.IsConnected)
                {
                    CustomMessageBox.Show("无法连接到飞控，请确认是否已连接飞控WiFi热点[1]!");
                    return;
                }
                try
                {
                    string log_file_path = "/var/APM/logs/";
                    string res = client.RunCommand("ls -tr --full-time " + log_file_path).Result;

                    // avoid SN is NULL
                    if (0 == res.Length)
                    {
                        MessageBox.Show("抱歉，暂时没有飞行日志", "飞行日志列表为空", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    string[] info = res.Split('\n');
                    /*
                     * 
                     * total 3906080
                    -rwxr-xr-x 1 root root  12193792 2014-05-15 02:37:48.000000000 +0000 100.FLOG
                    -rwxr-xr-x 1 root root    122880 2014-05-15 02:38:12.000000000 +0000 101.FLOG
                    -rwxr-xr-x 1 root root  21479424 2014-05-15 02:45:30.000000000 +0000 102.FLOG
                    -rwxr-xr-x 1 root root 250888192 2014-05-15 03:39:12.000000000 +0000 103.FLOG
                    -rwxr-xr-x 1 root root  57352192 2014-05-15 02:37:30.000000000 +0000 104.FLOG
                     */

                    foreach (string info_item in info) // line
                    {
                        if (info_item.Contains(".FLOG"))
                        // if (info_item.Contains(".BIN"))
                        {
                            string[] log_info = info_item.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            st_log_item log_item = new st_log_item(log_info[8], log_info[5], log_info[6].Substring(0, 8), log_info[4]);
                            logList.Add(log_item);
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("获取飞行日志列表出错，请尝试重新获取", "获取错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                client.Disconnect();
                client.Dispose();
            }

            string date_group = "";
            int node_cnt = 0;
            // noded log file as tree(according to date)
            foreach (var log_var in logList) // date is in order-incr
            {
                if (!log_var.log_date.Equals(date_group)) // new top node
                {
                    string root_node = log_var.log_date;
                    directoryTreeView.Nodes.Add(root_node);
                    date_group = log_var.log_date;
                }
                node_cnt++;
                DateTime log_dateTimeUTC = DateTime.ParseExact(log_var.log_date + " " + log_var.log_time, "yyyy-MM-dd HH:mm:ss", null);
                DateTime log_dateTime = log_dateTimeUTC.AddHours(8); // UTC to BeiJing
                log_var.log_time = log_dateTime.TimeOfDay.ToString();

                TreeNode log_file_node = new TreeNode("(" + node_cnt.ToString() + ")" + log_dateTime + " [" + (Double.Parse(log_var.log_size) * 9.53674E-7).ToString("0.00") + "MB]");
                directoryTreeView.Nodes[directoryTreeView.GetNodeCount(false) - 1].Nodes.Add(log_file_node);
            }

            // populate
        }

        private void getLogList_Click(object sender, EventArgs e)
        {
            getLogList_func();
        }

        private List<int> selectedLogList = new List<int>();
        private st_log_item downloading_log;
        private ulong downloading_log_size = 0;
        private int downloading_log_idx = 0;
        private bool next_log_download = true;
        private void downLog_Click(object sender, EventArgs e)
        {
             
            if (selectedLogList.Count == 0)
            {
                MessageBox.Show("抱歉，请选中需要下载的日志", "日志文件未选中", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // AB ZhaoYJ@2017-03-28 for user-defined dir to save log
            // ========= start =========
            int fristLogIdx = selectedLogList.FirstOrDefault() - 1;
            string firstLogDate = logList[fristLogIdx].log_date + " " + logList[fristLogIdx].log_time;
            string save_file = firstLogDate.Replace(":", "-") + ".FLOG";

            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                RestoreDirectory = true,
                Filter = "飞行日志|*.FLOG",
                Title = "保存飞行日志文件"
            };

            string save_dir = "";
            saveFileDialog1.FileName += save_file;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                save_dir = Path.GetDirectoryName(saveFileDialog1.FileName.ToString());
            }
            else
            {
                return;
            }

            string host = "11.0.0.1";
            string user = "root";

            cprobar_download.Visible = true;

            using (var sftp = new SftpClient(host, user, " "))
            {
                sftp.Connect();

                string path = "/var/APM/logs/";

                try
                {
                    foreach (int select_idx in selectedLogList)
                    {

                        // while (!next_log_download) ;

                        int log_idx = select_idx - 1;
                        downloading_log = logList[log_idx];

                        cprobar_download.Value = 0;
                        cprobar_download.Text = "日志" + select_idx.ToString();
                        downloading_log_idx = select_idx;
                        downloading_log_size = ulong.Parse(downloading_log.log_size);

                        string log_ts = logList[log_idx].log_date + " " + logList[log_idx].log_time;
                        string log_file = log_ts.Replace(":", "-") + ".FLOG";

                        string save_log_file = save_dir + "\\" + log_file;
                        // string save_file = @save_path + "1.bin";
                        try
                        {
                            Stream fstream = File.OpenWrite(save_log_file);
                            {
                                string remote_file = path + logList[log_idx].log_name;
                                sftp.DownloadFile(remote_file, fstream, doneloading);
                                fstream.Flush();
                                fstream.Close();
                            }
                            cprobar_download.Value = 100;

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("日志文件 (" + log_idx.ToString() + ") " + "可能已不存在，请重新刷新日志列表", "文件不存在", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        }
                        // var logname = GetLog(entry.id, fileName);
                        // CreateLog(logname);
                    }

                    MessageBox.Show("日志文件下载成功", "完成下载", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("日志文件下载失败", "下载失败", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                }

                sftp.Disconnect();
                sftp.Dispose();
            }
        }

        private void doneloading(ulong obj)
        {
            if (downloading_log_size == 0)
            {
                cprobar_download.Value = 100;
            }
            else
            {
                next_log_download = false ;
                cprobar_download.Value = ((int)(((double)obj/ (double)downloading_log_size)*100))%100;
            }

        }

        private void directoryTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            inputTextBox.Text = e.Node.FullPath;
            TreeView tv = sender as TreeView;
            if (tv != null)
            {
                TreeNode selected = e.Node;
                if (selected != null)
                {
                    string s_log_idx = Regex.Match(selected.Text, @"\(([^)]*)\)").Groups[1].Value;
                    if (s_log_idx != "") // child node
                    {
                        int log_idx = int.Parse(s_log_idx);
                        if (selected.Checked)
                        {
                            if (!selectedLogList.Contains(log_idx))
                            {
                                selectedLogList.Add(log_idx);
                            }
                        }
                        else
                        {
                            if (selectedLogList.Contains(log_idx))
                            {
                                selectedLogList.Remove(log_idx);
                            }
                        }
                    }      
                }

            }
        }
        private void directoryTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {

        }

        private void openButton_Click(object sender, EventArgs e)
        {

        }

    }
}
