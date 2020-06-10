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
    partial class ConfigVoltCali : MyUserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigVoltCali));
            this.btn_volt_cali = new MissionPlanner.Controls.MyButton();
            this.lineSeparator1 = new MissionPlanner.Controls.LineSeparator();
            this.lb_ref = new System.Windows.Forms.Label();
            this.btn_add_points = new MissionPlanner.Controls.MyButton();
            this.num_ref1 = new System.Windows.Forms.NumericUpDown();
            this.btn_clr = new MissionPlanner.Controls.MyButton();
            ((System.ComponentModel.ISupportInitialize)(this.num_ref1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_volt_cali
            // 
            resources.ApplyResources(this.btn_volt_cali, "btn_volt_cali");
            this.btn_volt_cali.Name = "btn_volt_cali";
            this.btn_volt_cali.UseVisualStyleBackColor = true;
            this.btn_volt_cali.Click += new System.EventHandler(this.btn_volt_cali_Click);
            // 
            // lineSeparator1
            // 
            resources.ApplyResources(this.lineSeparator1, "lineSeparator1");
            this.lineSeparator1.Name = "lineSeparator1";
            // 
            // lb_ref
            // 
            resources.ApplyResources(this.lb_ref, "lb_ref");
            this.lb_ref.Name = "lb_ref";
            // 
            // btn_add_points
            // 
            resources.ApplyResources(this.btn_add_points, "btn_add_points");
            this.btn_add_points.Name = "btn_add_points";
            this.btn_add_points.UseVisualStyleBackColor = true;
            this.btn_add_points.Click += new System.EventHandler(this.btn_add_points_Click);
            // 
            // num_ref1
            // 
            this.num_ref1.DecimalPlaces = 2;
            this.num_ref1.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            resources.ApplyResources(this.num_ref1, "num_ref1");
            this.num_ref1.Name = "num_ref1";
            // 
            // btn_clr
            // 
            resources.ApplyResources(this.btn_clr, "btn_clr");
            this.btn_clr.Name = "btn_clr";
            this.btn_clr.UseVisualStyleBackColor = true;
            this.btn_clr.Click += new System.EventHandler(this.btn_cali_reset_Click);
            // 
            // ConfigVoltCali
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.Controls.Add(this.num_ref1);
            this.Controls.Add(this.lb_ref);
            this.Controls.Add(this.lineSeparator1);
            this.Controls.Add(this.btn_add_points);
            this.Controls.Add(this.btn_clr);
            this.Controls.Add(this.btn_volt_cali);
            this.Name = "ConfigVoltCali";
            ((System.ComponentModel.ISupportInitialize)(this.num_ref1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private Controls.MyButton btn_volt_cali;
        private Controls.LineSeparator lineSeparator1;
        private Label lb_ref;
        private Controls.MyButton btn_add_points;
        private NumericUpDown num_ref1;
        private Controls.MyButton btn_clr;
    }
}