/// AB ZhaoYJ@2017-03-17
/// for: EKF status add
/// start
//#define EKF_STATUS_DISP
/// end

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MissionPlanner.Controls
{
    public partial class EKFStatus : Form
    {
        public EKFStatus()
        {
            InitializeComponent();

            //Added by HL below
            ModifyLayout();
            //Added by HL above


            Utilities.ThemeManager.ApplyThemeTo(this);

            timer1.Start();
        }

        //Added by HL below
        private void ModifyLayout()
        {

#if EKF_STATUS_DISP

            this.tableLayoutPanel1.Controls.Remove(this.label8);
            this.tableLayoutPanel1.Controls.Remove(this.ekfcompass);
            this.tableLayoutPanel1.Controls.Remove(this.ekfterrain);
            this.tableLayoutPanel1.Controls.Remove(this.label1);
            this.tableLayoutPanel1.Controls.Remove(this.label5);
            this.tableLayoutPanel1.Controls.Remove(this.label6);
            this.tableLayoutPanel1.Controls.Remove(this.label7);

#else
            this.tableLayoutPanel1.Controls.Remove(this.ekfvel);
            this.tableLayoutPanel1.Controls.Remove(this.ekfposv);
            this.tableLayoutPanel1.Controls.Remove(this.ekfposh);
            this.tableLayoutPanel1.Controls.Remove(this.label8);
            this.tableLayoutPanel1.Controls.Remove(this.ekfcompass);
            this.tableLayoutPanel1.Controls.Remove(this.ekfterrain);
            this.tableLayoutPanel1.Controls.Remove(this.label1);
            //this.tableLayoutPanel1.Controls.Remove(this.label2);
            this.tableLayoutPanel1.Controls.Remove(this.label3);
            this.tableLayoutPanel1.Controls.Remove(this.label4);
            this.tableLayoutPanel1.Controls.Remove(this.label5);
            this.tableLayoutPanel1.Controls.Remove(this.label6);
            this.tableLayoutPanel1.Controls.Remove(this.label7);
            this.tableLayoutPanel1.Controls.Add(this.ekfvel, 1, 1);

            this.tableLayoutPanel1.ColumnStyles.RemoveAt(7);
            this.tableLayoutPanel1.ColumnStyles.RemoveAt(6);
            this.tableLayoutPanel1.ColumnStyles.RemoveAt(5);
            this.tableLayoutPanel1.ColumnStyles.RemoveAt(4);
            this.tableLayoutPanel1.ColumnStyles.RemoveAt(3);
            this.tableLayoutPanel1.ColumnStyles.RemoveAt(2);
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Dock = DockStyle.None;
            this.tableLayoutPanel1.Size = new System.Drawing.Size(120, 274);
            this.label2.Text = "";

            System.Windows.Forms.Label labelText = new System.Windows.Forms.Label();
            labelText.AutoSize = true;
            tableLayoutPanel1.SetColumnSpan(labelText, 3);
            labelText.Dock = System.Windows.Forms.DockStyle.Fill;
            labelText.Name = "labelText";
            labelText.TabIndex = 0;
            labelText.Text = "振动指标";
            labelText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.tableLayoutPanel1.Controls.Add(labelText, 0, 0);
#endif
        }

        public System.Windows.Forms.TableLayoutPanel GetTableLayoutPanel()
        {
            return this.tableLayoutPanel1;
        }
        //Added by HL above

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ekfvel.Value = (int) (MainV2.comPort.MAV.cs.ekfvelv*100);
            ekfposh.Value = (int) (MainV2.comPort.MAV.cs.ekfposhor*100);
            ekfposv.Value = (int) (MainV2.comPort.MAV.cs.ekfposvert*100);
            ekfcompass.Value = (int) (MainV2.comPort.MAV.cs.ekfcompv*100);
            ekfterrain.Value = (int) (MainV2.comPort.MAV.cs.ekfteralt*100);

            // restore colours
            Utilities.ThemeManager.ApplyThemeTo(this);

            foreach (var item in new VerticalProgressBar2[] {ekfvel, ekfposh, ekfposv, ekfcompass, ekfterrain})
            {
                if (item.Value > 50)
                    item.ValueColor = Color.Orange;

                if (item.Value > 80)
                    item.ValueColor = Color.Red;
            }

            label7.Text = "";

            for (int a = 1; a < (int) MAVLink.EKF_STATUS_FLAGS.ENUM_END; a = a << 1)
            {
                int currentbit = (MainV2.comPort.MAV.cs.ekfflags & a);

                var currentflag = (MAVLink.EKF_STATUS_FLAGS) Enum.Parse(typeof (MAVLink.EKF_STATUS_FLAGS), a.ToString());

                label7.Text += currentflag.ToString().Replace("EKF_", "").ToLower() + " " +
                               (currentbit > 0 ? "On " : "Off") + "\r\n";
            }
        }

        private void ekfposv_Click(object sender, EventArgs e)
        {

        }

        private void ekfposh_Click(object sender, EventArgs e)
        {

        }
    }
}