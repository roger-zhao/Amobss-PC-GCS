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
    public partial class Vibration : Form
    {
        public Vibration()
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
            this.tableLayoutPanel1.Controls.Remove(this.VibBarX);
            this.tableLayoutPanel1.Controls.Remove(this.VibBarZ);
            this.tableLayoutPanel1.Controls.Remove(this.VibBarY);
            this.tableLayoutPanel1.Controls.Remove(this.label1);
            this.tableLayoutPanel1.Controls.Remove(this.label3);
            this.tableLayoutPanel1.Controls.Remove(this.label5);
            this.tableLayoutPanel1.Controls.Remove(this.label4);
            this.tableLayoutPanel1.Controls.Remove(this.label7);
            this.tableLayoutPanel1.Controls.Remove(this.label2);
            this.tableLayoutPanel1.Controls.Remove(this.label6);
            this.tableLayoutPanel1.Controls.Remove(this.label8);
            this.tableLayoutPanel1.Controls.Remove(this.txt_clip0);
            this.tableLayoutPanel1.Controls.Remove(this.txt_clip1);
            this.tableLayoutPanel1.Controls.Remove(this.txt_clip2);
            this.tableLayoutPanel1.Controls.Add(this.VibBarZ, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label5, 1, 5);
            this.label5.Text = "";

            this.tableLayoutPanel1.ColumnStyles.RemoveAt(5);
            this.tableLayoutPanel1.ColumnStyles.RemoveAt(4);
            this.tableLayoutPanel1.ColumnStyles.RemoveAt(3);
            this.tableLayoutPanel1.ColumnStyles.RemoveAt(2);
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Dock = DockStyle.None;
            this.tableLayoutPanel1.Size = new System.Drawing.Size(120, 274);

            System.Windows.Forms.Label labelText = new System.Windows.Forms.Label();
            labelText.AutoSize = true;
            tableLayoutPanel1.SetColumnSpan(labelText, 3);
            labelText.Dock = System.Windows.Forms.DockStyle.Fill;
            labelText.Name = "labelText";
            labelText.TabIndex = 0;
            labelText.Text = "位置指标";
            labelText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.tableLayoutPanel1.Controls.Add(labelText, 0, 0);
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
            VibBarX.Value = (int) MainV2.comPort.MAV.cs.vibex;
            VibBarY.Value = (int) MainV2.comPort.MAV.cs.vibey;
            //--VibBarZ.Value = (int) MainV2.comPort.MAV.cs.vibez;
            VibBarZ.Value = (int)(MainV2.comPort.MAV.cs.ekfposhor * 100);

            txt_clip0.Text = MainV2.comPort.MAV.cs.vibeclip0.ToString();
            txt_clip1.Text = MainV2.comPort.MAV.cs.vibeclip1.ToString();
            txt_clip2.Text = MainV2.comPort.MAV.cs.vibeclip2.ToString();

            foreach (var item in new VerticalProgressBar2[] { VibBarZ })
            {
                if (item.Value > 50)
                    item.ValueColor = Color.Orange;

                if (item.Value > 80)
                    item.ValueColor = Color.Red;
            }
        }
    }
}