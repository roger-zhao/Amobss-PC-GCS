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
    partial class ConfigFailsafeNew : MyUserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigFailsafeNew));
            this.lineSeparator1 = new MissionPlanner.Controls.LineSeparator();
            this.note = new MissionPlanner.Controls.MyLabel();
            this.mod = new MissionPlanner.Controls.MyButton();
            this.gb_activate_date = new System.Windows.Forms.GroupBox();
            this.cb_rc_fs = new System.Windows.Forms.ComboBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.volt_fs = new MissionPlanner.Controls.RangeControl();
            this.gb_activate_date.SuspendLayout();
            this.SuspendLayout();
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
            // mod
            // 
            resources.ApplyResources(this.mod, "mod");
            this.mod.Name = "mod";
            this.mod.UseVisualStyleBackColor = true;
            this.mod.Click += new System.EventHandler(this.send_to_Click);
            // 
            // gb_activate_date
            // 
            this.gb_activate_date.Controls.Add(this.cb_rc_fs);
            resources.ApplyResources(this.gb_activate_date, "gb_activate_date");
            this.gb_activate_date.Name = "gb_activate_date";
            this.gb_activate_date.TabStop = false;
            // 
            // cb_rc_fs
            // 
            this.cb_rc_fs.FormattingEnabled = true;
            this.cb_rc_fs.Items.AddRange(new object[] {
            resources.GetString("cb_rc_fs.Items"),
            resources.GetString("cb_rc_fs.Items1")});
            resources.ApplyResources(this.cb_rc_fs, "cb_rc_fs");
            this.cb_rc_fs.Name = "cb_rc_fs";
            // 
            // volt_fs
            // 
            this.volt_fs.DescriptionText = "修改该值，以适配搭载不同电池飞行器的低电压报警";
            this.volt_fs.DisplayScale = 1F;
            this.volt_fs.Increment = 0.1F;
            this.volt_fs.LabelText = "低电压报警值";
            resources.ApplyResources(this.volt_fs, "volt_fs");
            this.volt_fs.MaxRange = 100F;
            this.volt_fs.MinRange = 5F;
            this.volt_fs.Name = "volt_fs";
            this.volt_fs.Value = "22.20";
            // 
            // ConfigFailsafeNew
            // 
            this.Controls.Add(this.volt_fs);
            this.Controls.Add(this.gb_activate_date);
            this.Controls.Add(this.mod);
            this.Controls.Add(this.note);
            this.Controls.Add(this.lineSeparator1);
            this.Name = "ConfigFailsafeNew";
            resources.ApplyResources(this, "$this");
            this.gb_activate_date.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private Controls.LineSeparator lineSeparator1;
        private Controls.MyLabel note;
        private Controls.MyButton mod;
        private GroupBox gb_activate_date;
        private BackgroundWorker backgroundWorker1;
        private ComboBox cb_rc_fs;
        private Controls.RangeControl volt_fs;
    }
}