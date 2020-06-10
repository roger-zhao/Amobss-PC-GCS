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
    partial class ConfigPumpSpd : MyUserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigPumpSpd));
            this.pump_spd = new MissionPlanner.Controls.RangeControl();
            this.sprayer_spd = new MissionPlanner.Controls.RangeControl();
            this.lineSeparator1 = new MissionPlanner.Controls.LineSeparator();
            this.reset = new MissionPlanner.Controls.MyButton();
            this.sendto = new MissionPlanner.Controls.MyButton();
            this.testmode = new MissionPlanner.Controls.MyButton();
            this.note = new MissionPlanner.Controls.MyLabel();
            this.note_tips = new System.Windows.Forms.RichTextBox();
            this.lineSeparator2 = new MissionPlanner.Controls.LineSeparator();
            this.line_spaces = new MissionPlanner.Controls.RangeControl();
            this.SuspendLayout();
            // 
            // pump_spd
            // 
            this.pump_spd.DescriptionText = "可以拖动下面的滚动条来调整水泵速度，值越大速度越快";
            this.pump_spd.DisplayScale = 1F;
            this.pump_spd.Increment = 1F;
            this.pump_spd.LabelText = "水泵速度";
            resources.ApplyResources(this.pump_spd, "pump_spd");
            this.pump_spd.MaxRange = 2000F;
            this.pump_spd.MinRange = 1000F;
            this.pump_spd.Name = "pump_spd";
            this.pump_spd.Value = "1500";
            // 
            // sprayer_spd
            // 
            this.sprayer_spd.DescriptionText = "可以拖动下面的滚动条来调整喷头速度,值越大喷速越快";
            this.sprayer_spd.DisplayScale = 1F;
            this.sprayer_spd.Increment = 1F;
            this.sprayer_spd.LabelText = "喷头速度";
            resources.ApplyResources(this.sprayer_spd, "sprayer_spd");
            this.sprayer_spd.MaxRange = 2000F;
            this.sprayer_spd.MinRange = 1000F;
            this.sprayer_spd.Name = "sprayer_spd";
            this.sprayer_spd.Value = "1500";
            // 
            // lineSeparator1
            // 
            this.lineSeparator1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lineSeparator1.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lineSeparator1, "lineSeparator1");
            this.lineSeparator1.Name = "lineSeparator1";
            // 
            // reset
            // 
            resources.ApplyResources(this.reset, "reset");
            this.reset.Name = "reset";
            this.reset.UseVisualStyleBackColor = true;
            this.reset.Click += new System.EventHandler(this.reset_Click);
            // 
            // sendto
            // 
            resources.ApplyResources(this.sendto, "sendto");
            this.sendto.Name = "sendto";
            this.sendto.UseVisualStyleBackColor = true;
            this.sendto.Click += new System.EventHandler(this.sendto_Click);
            // 
            // testmode
            // 
            resources.ApplyResources(this.testmode, "testmode");
            this.testmode.Name = "testmode";
            this.testmode.UseVisualStyleBackColor = true;
            this.testmode.Click += new System.EventHandler(this.testmode_Click);
            // 
            // note
            // 
            resources.ApplyResources(this.note, "note");
            this.note.Name = "note";
            this.note.resize = false;
            // 
            // note_tips
            // 
            this.note_tips.ForeColor = System.Drawing.Color.Yellow;
            resources.ApplyResources(this.note_tips, "note_tips");
            this.note_tips.Name = "note_tips";
            // 
            // lineSeparator2
            // 
            this.lineSeparator2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lineSeparator2.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lineSeparator2, "lineSeparator2");
            this.lineSeparator2.Name = "lineSeparator2";
            // 
            // line_spaces
            // 
            this.line_spaces.DescriptionText = "可以拖动下面的滚动条来调整垄宽（单位：米）";
            this.line_spaces.DisplayScale = 1F;
            this.line_spaces.Increment = 0.5F;
            this.line_spaces.LabelText = "作业垄宽";
            resources.ApplyResources(this.line_spaces, "line_spaces");
            this.line_spaces.MaxRange = 10F;
            this.line_spaces.MinRange = 0F;
            this.line_spaces.Name = "line_spaces";
            this.line_spaces.Value = "5";
            // 
            // ConfigPumpSpd
            // 
            this.Controls.Add(this.note_tips);
            this.Controls.Add(this.note);
            this.Controls.Add(this.testmode);
            this.Controls.Add(this.sendto);
            this.Controls.Add(this.reset);
            this.Controls.Add(this.lineSeparator2);
            this.Controls.Add(this.lineSeparator1);
            this.Controls.Add(this.line_spaces);
            this.Controls.Add(this.sprayer_spd);
            this.Controls.Add(this.pump_spd);
            this.Name = "ConfigPumpSpd";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);

        }
        private Controls.RangeControl pump_spd;
        private Controls.RangeControl sprayer_spd;
        private Controls.LineSeparator lineSeparator1;
        private Controls.MyButton reset;
        private Controls.MyButton sendto;
        private Controls.MyButton testmode;
        private Controls.MyLabel note;
        private RichTextBox note_tips;
        private Controls.LineSeparator lineSeparator2;
        // private Controls.MyLabel line_space;
        private Controls.RangeControl line_spaces;
    }
}