using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO.Ports;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;
using System.Net;

namespace MissionPlanner.GCSViews.ConfigurationView
{
    partial class ConfigFlyHgtSpd : MyUserControl
    {

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigFlyHgtSpd));
            this.horizon_spd = new MissionPlanner.Controls.RangeControl();
            this.lineSeparator1 = new MissionPlanner.Controls.LineSeparator();
            this.note = new MissionPlanner.Controls.MyLabel();
            this.lineSeparator2 = new MissionPlanner.Controls.LineSeparator();
            this.cb_yaw_mode = new System.Windows.Forms.ComboBox();
            this.myLabel1 = new MissionPlanner.Controls.MyLabel();
            this.send_to = new MissionPlanner.Controls.MyButton();
            this.lineSeparator3 = new MissionPlanner.Controls.LineSeparator();
            this.lb_reset = new MissionPlanner.Controls.MyButton();
            this.des_vel_decel_s = new MissionPlanner.Controls.RangeControl();
            this.rng_angle_max = new MissionPlanner.Controls.RangeControl();
            this.SuspendLayout();
            // 
            // horizon_spd
            // 
            this.horizon_spd.DescriptionText = "可以拖动下面的滚动条来调整巡航速度（单位：米/秒）";
            this.horizon_spd.DisplayScale = 1F;
            this.horizon_spd.Increment = 1F;
            this.horizon_spd.LabelText = "巡航速度";
            resources.ApplyResources(this.horizon_spd, "horizon_spd");
            this.horizon_spd.MaxRange = 10F;
            this.horizon_spd.MinRange = 5F;
            this.horizon_spd.Name = "horizon_spd";
            this.horizon_spd.Value = "5";
            // 
            // lineSeparator1
            // 
            this.lineSeparator1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lineSeparator1.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lineSeparator1, "lineSeparator1");
            this.lineSeparator1.Name = "lineSeparator1";
            // 
            // note
            // 
            resources.ApplyResources(this.note, "note");
            this.note.Name = "note";
            this.note.resize = false;
            // 
            // lineSeparator2
            // 
            this.lineSeparator2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lineSeparator2.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lineSeparator2, "lineSeparator2");
            this.lineSeparator2.Name = "lineSeparator2";
            // 
            // cb_yaw_mode
            // 
            resources.ApplyResources(this.cb_yaw_mode, "cb_yaw_mode");
            this.cb_yaw_mode.FormattingEnabled = true;
            this.cb_yaw_mode.Items.AddRange(new object[] {
            resources.GetString("cb_yaw_mode.Items"),
            resources.GetString("cb_yaw_mode.Items1")});
            this.cb_yaw_mode.Name = "cb_yaw_mode";
            // 
            // myLabel1
            // 
            resources.ApplyResources(this.myLabel1, "myLabel1");
            this.myLabel1.Name = "myLabel1";
            this.myLabel1.resize = false;
            // 
            // send_to
            // 
            resources.ApplyResources(this.send_to, "send_to");
            this.send_to.Name = "send_to";
            this.send_to.UseVisualStyleBackColor = true;
            this.send_to.Click += new System.EventHandler(this.send_to_Click);
            // 
            // lineSeparator3
            // 
            this.lineSeparator3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lineSeparator3.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lineSeparator3, "lineSeparator3");
            this.lineSeparator3.Name = "lineSeparator3";
            // 
            // lb_reset
            // 
            resources.ApplyResources(this.lb_reset, "lb_reset");
            this.lb_reset.Name = "lb_reset";
            this.lb_reset.UseVisualStyleBackColor = true;
            this.lb_reset.Click += new System.EventHandler(this.reset_Click);
            // 
            // des_vel_decel_s
            // 
            this.des_vel_decel_s.DescriptionText = "可以拖动下面的滚动条来调整自动悬停时刹车速度（单位：秒）";
            this.des_vel_decel_s.DisplayScale = 1F;
            this.des_vel_decel_s.Increment = 0.1F;
            this.des_vel_decel_s.LabelText = "自动悬停刹车时间";
            resources.ApplyResources(this.des_vel_decel_s, "des_vel_decel_s");
            this.des_vel_decel_s.MaxRange = 10F;
            this.des_vel_decel_s.MinRange = 0F;
            this.des_vel_decel_s.Name = "des_vel_decel_s";
            this.des_vel_decel_s.Value = "0";
            // 
            // rng_angle_max
            // 
            this.rng_angle_max.DescriptionText = "可以拖动下面的滚动条来调整自动航线刹车幅度\r\n（0: 幅度最小，10: 幅度最大）";
            this.rng_angle_max.DisplayScale = 1F;
            this.rng_angle_max.Increment = 1F;
            this.rng_angle_max.LabelText = "自动航线刹车幅度";
            resources.ApplyResources(this.rng_angle_max, "rng_angle_max");
            this.rng_angle_max.MaxRange = 10F;
            this.rng_angle_max.MinRange = 0F;
            this.rng_angle_max.Name = "rng_angle_max";
            this.rng_angle_max.Value = "0";
            // 
            // ConfigFlyHgtSpd
            // 
            this.Controls.Add(this.rng_angle_max);
            this.Controls.Add(this.lb_reset);
            this.Controls.Add(this.send_to);
            this.Controls.Add(this.myLabel1);
            this.Controls.Add(this.cb_yaw_mode);
            this.Controls.Add(this.note);
            this.Controls.Add(this.lineSeparator3);
            this.Controls.Add(this.lineSeparator2);
            this.Controls.Add(this.lineSeparator1);
            this.Controls.Add(this.des_vel_decel_s);
            this.Controls.Add(this.horizon_spd);
            this.Name = "ConfigFlyHgtSpd";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);

        }
        private Controls.RangeControl horizon_spd;

        private Controls.LineSeparator lineSeparator1;
        private Controls.MyLabel note;
        private Controls.LineSeparator lineSeparator2;
        private ComboBox cb_yaw_mode;
        private Controls.MyLabel myLabel1;
        private Controls.MyButton send_to;
        private Controls.LineSeparator lineSeparator3;
        private Controls.MyButton lb_reset;
        private Controls.RangeControl des_vel_decel_s;
        private Controls.RangeControl rng_angle_max;
    }
}