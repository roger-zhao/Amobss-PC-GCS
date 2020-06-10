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
    partial class ConfigFactory : MyUserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigFactory));
            this.lineSeparator1 = new MissionPlanner.Controls.LineSeparator();
            this.note = new MissionPlanner.Controls.MyLabel();
            this.mod = new MissionPlanner.Controls.MyButton();
            this.num_act_year = new System.Windows.Forms.NumericUpDown();
            this.gb_activate_date = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.num_act_day = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.num_act_month = new System.Windows.Forms.NumericUpDown();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.gb_expired_len = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.num_exp_day = new System.Windows.Forms.NumericUpDown();
            this.num_exp_year = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.num_act_year)).BeginInit();
            this.gb_activate_date.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_act_day)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_act_month)).BeginInit();
            this.gb_expired_len.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_exp_day)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_exp_year)).BeginInit();
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
            // num_act_year
            // 
            resources.ApplyResources(this.num_act_year, "num_act_year");
            this.num_act_year.Maximum = new decimal(new int[] {
            2038,
            0,
            0,
            0});
            this.num_act_year.Minimum = new decimal(new int[] {
            1970,
            0,
            0,
            0});
            this.num_act_year.Name = "num_act_year";
            this.num_act_year.Value = new decimal(new int[] {
            2017,
            0,
            0,
            0});
            // 
            // gb_activate_date
            // 
            this.gb_activate_date.Controls.Add(this.label5);
            this.gb_activate_date.Controls.Add(this.label4);
            this.gb_activate_date.Controls.Add(this.num_act_day);
            this.gb_activate_date.Controls.Add(this.label3);
            this.gb_activate_date.Controls.Add(this.num_act_month);
            this.gb_activate_date.Controls.Add(this.num_act_year);
            resources.ApplyResources(this.gb_activate_date, "gb_activate_date");
            this.gb_activate_date.Name = "gb_activate_date";
            this.gb_activate_date.TabStop = false;
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // num_act_day
            // 
            resources.ApplyResources(this.num_act_day, "num_act_day");
            this.num_act_day.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.num_act_day.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_act_day.Name = "num_act_day";
            this.num_act_day.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // num_act_month
            // 
            resources.ApplyResources(this.num_act_month, "num_act_month");
            this.num_act_month.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.num_act_month.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_act_month.Name = "num_act_month";
            this.num_act_month.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // gb_expired_len
            // 
            this.gb_expired_len.Controls.Add(this.label2);
            this.gb_expired_len.Controls.Add(this.label1);
            this.gb_expired_len.Controls.Add(this.num_exp_day);
            this.gb_expired_len.Controls.Add(this.num_exp_year);
            resources.ApplyResources(this.gb_expired_len, "gb_expired_len");
            this.gb_expired_len.Name = "gb_expired_len";
            this.gb_expired_len.TabStop = false;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // num_exp_day
            // 
            resources.ApplyResources(this.num_exp_day, "num_exp_day");
            this.num_exp_day.Maximum = new decimal(new int[] {
            365,
            0,
            0,
            0});
            this.num_exp_day.Name = "num_exp_day";
            this.num_exp_day.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // num_exp_year
            // 
            resources.ApplyResources(this.num_exp_year, "num_exp_year");
            this.num_exp_year.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.num_exp_year.Name = "num_exp_year";
            // 
            // ConfigFactory
            // 
            this.Controls.Add(this.gb_expired_len);
            this.Controls.Add(this.gb_activate_date);
            this.Controls.Add(this.mod);
            this.Controls.Add(this.note);
            this.Controls.Add(this.lineSeparator1);
            this.Name = "ConfigFactory";
            resources.ApplyResources(this, "$this");
            ((System.ComponentModel.ISupportInitialize)(this.num_act_year)).EndInit();
            this.gb_activate_date.ResumeLayout(false);
            this.gb_activate_date.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_act_day)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_act_month)).EndInit();
            this.gb_expired_len.ResumeLayout(false);
            this.gb_expired_len.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_exp_day)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_exp_year)).EndInit();
            this.ResumeLayout(false);

        }

        private Controls.LineSeparator lineSeparator1;
        private Controls.MyLabel note;
        private Controls.MyButton mod;
        private NumericUpDown num_act_year;
        private GroupBox gb_activate_date;
        private NumericUpDown num_act_day;
        private NumericUpDown num_act_month;
        private BackgroundWorker backgroundWorker1;
        private GroupBox gb_expired_len;
        private NumericUpDown num_exp_year;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private NumericUpDown num_exp_day;
    }
}