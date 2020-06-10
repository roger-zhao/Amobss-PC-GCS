using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using GaryPerkin.UserControls.Buttons;
using MissionPlanner.Controls;
using MissionPlanner.HIL;

namespace MissionPlanner.GCSViews.ConfigurationView
{
    public partial class ConfigMotorTest : UserControl, IActivate
    {
        public ConfigMotorTest()
        {
            InitializeComponent();

        }

        /*
#if (FRAME_CONFIG == QUAD_FRAME)
        MAV_TYPE_QUADROTOR,
#elif (FRAME_CONFIG == TRI_FRAME)
        MAV_TYPE_TRICOPTER,
#elif (FRAME_CONFIG == HEXA_FRAME || FRAME_CONFIG == Y6_FRAME)
        MAV_TYPE_HEXAROTOR,
#elif (FRAME_CONFIG == OCTA_FRAME || FRAME_CONFIG == OCTA_QUAD_FRAME)
        MAV_TYPE_OCTOROTOR,
#elif (FRAME_CONFIG == HELI_FRAME)
        MAV_TYPE_HELICOPTER,
#elif (FRAME_CONFIG == SINGLE_FRAME)  //because mavlink did not define a singlecopter, we use a rocket
        MAV_TYPE_ROCKET,
#elif (FRAME_CONFIG == COAX_FRAME)  //because mavlink did not define a singlecopter, we use a rocket
        MAV_TYPE_ROCKET,
#else
  #error Unrecognised frame type
#endif*/
        public void OnFocus(object sender, System.EventArgs e)
        {
            TabControl tc = (TabControl)sender;
            if (tc.SelectedTab == this.Parent)
            {
                //Parent-Tab is selected, do stuff...
                Activate();
            }

        }
        public int get_motormax_new()
        {
            int motor_max = 8;
            if (MainV2.comPort.fc_alive) // according to frame_class/type(type_x)
            {
                MainV2.Motor_frame_class f_class = (MainV2.Motor_frame_class)MainV2.comPort.GetParam("FRAME_CLASS");
                switch(f_class)
                {
                    case MainV2.Motor_frame_class.MOTOR_FRAME_QUAD:
                        motor_max = 4;
                        break;
                    case MainV2.Motor_frame_class.MOTOR_FRAME_HEXA:
                        motor_max = 6;
                        break;
                    case MainV2.Motor_frame_class.MOTOR_FRAME_OCTA:
                    default:
                        motor_max = 8;
                        break;
                }

            }
            return motor_max;
        }
        public void Activate()
        {
            Controls.Clear();
            Controls.Add(this.label1);
            Controls.Add(this.trackBar1);
            var x = 20;
            var y = 40;

            label1.Location = new Point(x, y + 4);
            trackBar1.Location = new Point(x + label1.Size.Width + 28, y);

            x = 100;
            y = 200;
            var motormax = this.get_motormax_new();
        

            int radius = 80;     // 轨迹半径
            double angle = 360;

            if(MainV2.comPort.fc_alive) // according to frame_class/type(type_x)
            {
                if ((MainV2.Motor_frame_class)MainV2.comPort.GetParam("FRAME_CLASS") != MainV2.Motor_frame_class.MOTOR_FRAME_UNDEFINED)
                {
                    if ((MainV2.Motor_frame_type)MainV2.comPort.GetParam("FRAME_TYPE") == MainV2.Motor_frame_type.MOTOR_FRAME_TYPE_X)
                    {
                        angle = 180 / motormax;
                    }
                }

            }

            int[] initpos = { x, y };        // 初始坐标

            GaryPerkin.UserControls.Buttons.RoundButton but = new GaryPerkin.UserControls.Buttons.RoundButton();

            for (var a = 1; a <= motormax; a++)
            {

                but = new GaryPerkin.UserControls.Buttons.RoundButton();

                but.Location = new Point(initpos[0] + (int)(radius * Math.Sin(angle * Math.PI / 180)), initpos[1] + -(int)(radius * Math.Cos(angle * Math.PI / 180)));
                angle -= 360 / motormax;    // 顺时针旋转用 +=，逆时针用 -=
                if (angle < 0)
                    angle = 360 + angle;

                but.Text = a.ToString() + "号\n电机";
                // but.Size = new Size(100, 37);
                but.Click += but_Click;
                but.Tag = a;
                but.BackColor = Color.DarkGoldenrod;
                but.ForeColor = Color.Black;
                // but.AutoSize = true;
                // but.AutoSizeMode = AutoSizeMode 
                Controls.Add(but);
            }


            // but = new MyButton();
            // but.Text = "测试所有电机";
            // but.Location = new Point(x, y);
            // but.Size = new Size(100, 37);
            // but.Click += but_TestAll;
            // Controls.Add(but);

            // y += 39;

            // but = new MyButton();
            // but.Text = "停止所有电机";
            // but.Location = new Point(x, y);
            // but.Size = new Size(100, 37);
            // but.Click += but_StopAll;
            // Controls.Add(but);

            // y += 39;

            but = new GaryPerkin.UserControls.Buttons.RoundButton();
            but.Text = "逐个测试电机";
            // but.AutoSize = true;
            but.Location = new Point(x, y);
            but.Click += but_TestAllSeq;
            /// DB ZhaoYJ@2017-03-09
            /// for: tmp hiden this butt
            /// TODO: make sure it's stable
            /// start
            // but.Enabled = true;
            but.Visible = true;
            but.BackColor = Color.Yellow;
            but.ForeColor = Color.Black;

            /// end
            Controls.Add(but);

            Utilities.ThemeManager.ApplyThemeTo(this);
        }

        private int get_motormax()
        {
            var motormax = 8;

            if (MainV2.comPort.MAV.aptype == MAVLink.MAV_TYPE.GROUND_ROVER)
            {
                return 4;
            }

            var enable = MainV2.comPort.MAV.param.ContainsKey("FRAME") || MainV2.comPort.MAV.param.ContainsKey("Q_FRAME_TYPE") || MainV2.comPort.MAV.param.ContainsKey("FRAME_TYPE");

            if (!enable)
            {
                Enabled = false;
                return motormax;
            }

            MAVLink.MAV_TYPE type = MAVLink.MAV_TYPE.QUADROTOR;
            int frame_type = 0; // + frame

            if (MainV2.comPort.MAV.param.ContainsKey("Q_FRAME_CLASS"))
            {
                var value = (int)MainV2.comPort.MAV.param["Q_FRAME_CLASS"].Value;
                switch (value)
                {
                    case 0:
                    case 1:
                        type = MAVLink.MAV_TYPE.QUADROTOR;
                        break;
                    case 2:
                    case 5:
                        type = MAVLink.MAV_TYPE.HEXAROTOR;
                        break;
                    case 3:
                    case 4:
                        type = MAVLink.MAV_TYPE.OCTOROTOR;
                        break;
                    case 6:
                        type = MAVLink.MAV_TYPE.HELICOPTER;
                        break;
                    case 7:
                        type = MAVLink.MAV_TYPE.TRICOPTER;
                        break;
                }

                frame_type = (int)MainV2.comPort.MAV.param["Q_FRAME_TYPE"].Value;
            }
            else if (MainV2.comPort.MAV.param.ContainsKey("FRAME"))
            {
                type = MainV2.comPort.MAV.aptype;
                frame_type = (int)MainV2.comPort.MAV.param["FRAME"].Value;
            }
            else if (MainV2.comPort.MAV.param.ContainsKey("FRAME_TYPE"))
            {
                type = MainV2.comPort.MAV.aptype;
                frame_type = (int)MainV2.comPort.MAV.param["FRAME_TYPE"].Value;
            }


            var motors = new Motor[0];

            if (type == MAVLink.MAV_TYPE.TRICOPTER)
            {
                motormax = 4;

                motors = Motor.build_motors(MAVLink.MAV_TYPE.TRICOPTER, frame_type);
            }
            else if (type == MAVLink.MAV_TYPE.QUADROTOR)
            {
                motormax = 4;

                motors = Motor.build_motors(MAVLink.MAV_TYPE.QUADROTOR, frame_type);
            }
            else if (type == MAVLink.MAV_TYPE.HEXAROTOR)
            {
                motormax = 6;

                motors = Motor.build_motors(MAVLink.MAV_TYPE.HEXAROTOR, frame_type);
            }
            else if (type == MAVLink.MAV_TYPE.OCTOROTOR)
            {
                motormax = 8;

                motors = Motor.build_motors(MAVLink.MAV_TYPE.OCTOROTOR, frame_type);
            }
            else if (type == MAVLink.MAV_TYPE.HELICOPTER)
            {
                motormax = 0;
            }


            return motormax;
        }

        private void but_TestAll(object sender, EventArgs e)
        {
            int speed = (int)this.trackBar1.Value;
            int time = (int) NUM_duration.Value;

            int motormax = this.get_motormax();
            for (int i = 1; i <= motormax; i++)
            {
                testMotor(i, speed, time);
            }
        }

        private void but_TestAllSeq(object sender, EventArgs e)
        {
            int motormax = this.get_motormax_new();
            int speed = (int)this.trackBar1.Value;
            int time = (int) NUM_duration.Value;

            for (int i = 1; i <= motormax; i++)
            {
                testMotor(i, speed, time);
                Thread.Sleep(time*1500);
            }
        }

        private void but_StopAll(object sender, EventArgs e)
        {
            int motormax = this.get_motormax();
            for (int i = 1; i <= motormax; i++)
            {
                testMotor(i, 0, 0);
            }
        }

        private void but_Click(object sender, EventArgs e)
        {
            int speed = (int)this.trackBar1.Value;
            int time = (int) NUM_duration.Value;
            try
            {
                var motor = (int) ((GaryPerkin.UserControls.Buttons.RoundButton) sender).Tag;
                this.testMotor(motor, speed, time);
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Failed to test motor\n" + ex);
            }
        }

        private void testMotor(int motor, int speed, int time,int motorcount = 0)
        {
            try
            {
                if (
                    !MainV2.comPort.doMotorTest(motor, MAVLink.MOTOR_TEST_THROTTLE_TYPE.MOTOR_TEST_THROTTLE_PERCENT,
                        speed, time, motorcount))
                {
                    CustomMessageBox.Show("抱歉，飞控执行测试电机失败！");
                }
            }
            catch
            {
                CustomMessageBox.Show(Strings.ErrorCommunicating + "\n 电机: " + motor, Strings.ERROR);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // try
            // {
            //     Process.Start("http://copter.ardupilot.com/wiki/connect-escs-and-motors/");
            // }
            // catch
            // {
            //     CustomMessageBox.Show("Bad default system association", Strings.ERROR);
            // }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            this.label1.Text = "油门大小: " + this.trackBar1.Value.ToString() + "%";
        }
    }
}