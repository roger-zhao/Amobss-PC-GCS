using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MissionPlanner.Controls;
using System.Collections;

namespace MissionPlanner.GCSViews.ConfigurationView
{
    public partial class ConfigESCCalibration : UserControl, IActivate
    {
        public ConfigESCCalibration()
        {
            InitializeComponent();
        }

        public void AddColor()
        {
            richTextBox1.Select(0, 6);
            richTextBox1.SelectionColor = Color.Red;
            richTextBox1.SelectionFont = new Font("宋体", 16);

            richTextBox1.Select(27, 10);
            richTextBox1.SelectionColor = Color.Red;

            richTextBox1.Select(42, 9);
            richTextBox1.SelectionColor = Color.Orange;

            richTextBox1.Select(74, 7);
            richTextBox1.SelectionColor = Color.Red;

            richTextBox1.Select(131, 20);
            richTextBox1.SelectionColor = Color.Orange;

            buttonStart.Text = "确定";
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
            //bool RcCalmin = true;
            //bool RcCalmax = true;
            //var copy = new Hashtable((Hashtable) MainV2.comPort.MAV.param);

            //foreach (string item in copy.Keys)
            //{
            //    if (item == "RC3_MIN" && MainV2.comPort.MAV.param[item].ToString() == "1100")
            //    {
            //        RcCalmin = false;
            //    }
            //    else if (item == "RC3_MAX" && MainV2.comPort.MAV.param[item].ToString() == "1900")
            //    {
            //        RcCalmax = false;
            //    }
            //    else if (item == "ESC_CALIBRATION")
            //    {
            //        if (MainV2.comPort.MAV.param[item].ToString() == "2")
            //        {
            //            buttonStart.Text = "Ready";
            //            buttonStart.Enabled = false;
            //        }
            //        else
            //        {
            //            buttonStart.Text = "Start";
            //        }
            //    }
            //}

            //if (!(RcCalmax || RcCalmin))
            //{
            //    labelRC.Visible = true;
            //    buttonStart.Enabled = false;
            //}
            //else
            //{
            //    labelRC.Visible = false;
            //    buttonStart.Enabled = true;
            //}
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text != "DTJZ")
            {
                CustomMessageBox.Show("电调校准校验码错误，请重新输入！");
                return;
            }

            // clear text
            this.textBox1.Clear();

            if (MainV2.comPort.GetParam("ESC_CALIBRATION").Equals(65))
            {
                CustomMessageBox.Show("电调校准已经发送完成，请重新启动飞控板，以完成电调校准！\n\n" +
                    "具体流程如下：\n\n" +
                    "1.点击“OK”后，将飞控电源、动力电源关闭；\n\n" +
                    "2.重新将动力电源、飞控电源依次开启；\n\n" +
                    "3.等待飞控自动校准电调，（注：此过程中油门会输出最大值，请操作者注意安全！）；\n\n" +
                    "4.在确认电调发出校准成功的声音后，将飞行器飞控电源、动力电源关闭，电调校准完成。");
                return;
            }

            if (MainV2.comPort.setParam("ESC_CALIBRATION", 65))
            {
                CustomMessageBox.Show("具体流程如下：\n" +
                "1.点击“OK”后，将飞控电源、动力电源关闭；\n\n" +
                "2.重新将动力电源、飞控电源依次开启；\n\n" +
                "3.等待飞控自动校准电调，（注：此过程中油门会输出最大值，请操作者注意安全！）；\n\n" +
                "4.在确认电调发出校准成功的声音后，将飞行器飞控电源、动力电源关闭，电调校准完成。");
            }
            else
            {
                CustomMessageBox.Show("向飞控发送电调校准命令失败，请尝试重新连接飞控后，再进行电调校准");

            }

            //try
            //{
            //    if (!MainV2.comPort.setParam("ESC_CALIBRATION", 2))
            //    {
            //        CustomMessageBox.Show("Set param error. Please ensure your version is AC3.3+.");
            //        return;
            //    }
            //}
            //catch
            //{
            //    CustomMessageBox.Show("Set param error. Please ensure your version is AC3.3+.");
            //        return;
            //}

            //buttonStart.Text = "Ready";
            //buttonStart.Enabled = false;
        }
    }
}