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
    partial class ConfigFirmware : MyUserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigFirmware));
            this.lbl_Custom_firmware_label = new System.Windows.Forms.Label();
            this.lbl_dlfw = new System.Windows.Forms.Label();
            this.type_cmb = new System.Windows.Forms.ComboBox();
            this.frame_cmb = new System.Windows.Forms.ComboBox();
            this.v_type = new System.Windows.Forms.Label();
            this.v_frame = new System.Windows.Forms.Label();
            this.vehicle_info = new System.Windows.Forms.Label();
            this.release_info = new System.Windows.Forms.Label();
            this.SN_info = new System.Windows.Forms.Label();
            this.fw_ver = new System.Windows.Forms.Label();
            this.split_vertical = new System.Windows.Forms.GroupBox();
            this.mod_type_frame = new System.Windows.Forms.Label();
            this.lb_recovery = new System.Windows.Forms.Label();
            this.btn_refresh_sn = new MissionPlanner.Controls.MyButton();
            this.btn_sn = new MissionPlanner.Controls.MyButton();
            this.tb_sn_new = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lbl_Custom_firmware_label
            // 
            resources.ApplyResources(this.lbl_Custom_firmware_label, "lbl_Custom_firmware_label");
            this.lbl_Custom_firmware_label.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.lbl_Custom_firmware_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_Custom_firmware_label.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbl_Custom_firmware_label.Name = "lbl_Custom_firmware_label";
            this.lbl_Custom_firmware_label.Click += new System.EventHandler(this.Custom_firmware_label_Click);
            // 
            // lbl_dlfw
            // 
            resources.ApplyResources(this.lbl_dlfw, "lbl_dlfw");
            this.lbl_dlfw.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lbl_dlfw.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_dlfw.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbl_dlfw.Name = "lbl_dlfw";
            this.lbl_dlfw.Click += new System.EventHandler(this.lbl_dlfw_Click);
            // 
            // type_cmb
            // 
            this.type_cmb.FormattingEnabled = true;
            this.type_cmb.Items.AddRange(new object[] {
            resources.GetString("type_cmb.Items"),
            resources.GetString("type_cmb.Items1"),
            resources.GetString("type_cmb.Items2")});
            resources.ApplyResources(this.type_cmb, "type_cmb");
            this.type_cmb.Name = "type_cmb";
            this.type_cmb.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // frame_cmb
            // 
            this.frame_cmb.FormattingEnabled = true;
            this.frame_cmb.Items.AddRange(new object[] {
            resources.GetString("frame_cmb.Items"),
            resources.GetString("frame_cmb.Items1"),
            resources.GetString("frame_cmb.Items2")});
            resources.ApplyResources(this.frame_cmb, "frame_cmb");
            this.frame_cmb.Name = "frame_cmb";
            this.frame_cmb.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // v_type
            // 
            resources.ApplyResources(this.v_type, "v_type");
            this.v_type.Name = "v_type";
            // 
            // v_frame
            // 
            resources.ApplyResources(this.v_frame, "v_frame");
            this.v_frame.Name = "v_frame";
            // 
            // vehicle_info
            // 
            resources.ApplyResources(this.vehicle_info, "vehicle_info");
            this.vehicle_info.Name = "vehicle_info";
            // 
            // release_info
            // 
            resources.ApplyResources(this.release_info, "release_info");
            this.release_info.Name = "release_info";
            // 
            // SN_info
            // 
            resources.ApplyResources(this.SN_info, "SN_info");
            this.SN_info.Name = "SN_info";
            // 
            // fw_ver
            // 
            resources.ApplyResources(this.fw_ver, "fw_ver");
            this.fw_ver.Name = "fw_ver";
            // 
            // split_vertical
            // 
            this.split_vertical.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.split_vertical.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            resources.ApplyResources(this.split_vertical, "split_vertical");
            this.split_vertical.Name = "split_vertical";
            this.split_vertical.TabStop = false;
            // 
            // mod_type_frame
            // 
            resources.ApplyResources(this.mod_type_frame, "mod_type_frame");
            this.mod_type_frame.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.mod_type_frame.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mod_type_frame.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mod_type_frame.Name = "mod_type_frame";
            this.mod_type_frame.Click += new System.EventHandler(this.mod_type_frame_Click);
            // 
            // lb_recovery
            // 
            resources.ApplyResources(this.lb_recovery, "lb_recovery");
            this.lb_recovery.BackColor = System.Drawing.Color.Red;
            this.lb_recovery.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lb_recovery.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lb_recovery.Name = "lb_recovery";
            this.lb_recovery.Click += new System.EventHandler(this.lb_recovery_Click);
            // 
            // btn_refresh_sn
            // 
            resources.ApplyResources(this.btn_refresh_sn, "btn_refresh_sn");
            this.btn_refresh_sn.Name = "btn_refresh_sn";
            this.btn_refresh_sn.UseVisualStyleBackColor = true;
            this.btn_refresh_sn.Click += new System.EventHandler(this.btn_refresh_sn_Click);
            // 
            // btn_sn
            // 
            resources.ApplyResources(this.btn_sn, "btn_sn");
            this.btn_sn.Name = "btn_sn";
            this.btn_sn.UseVisualStyleBackColor = true;
            this.btn_sn.Click += new System.EventHandler(this.btn_sn_Click);
            // 
            // tb_sn_new
            // 
            resources.ApplyResources(this.tb_sn_new, "tb_sn_new");
            this.tb_sn_new.Name = "tb_sn_new";
            this.tb_sn_new.TextChanged += new System.EventHandler(this.tb_sn_TextChanged);
            // 
            // ConfigFirmware
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.Controls.Add(this.tb_sn_new);
            this.Controls.Add(this.btn_sn);
            this.Controls.Add(this.btn_refresh_sn);
            this.Controls.Add(this.lb_recovery);
            this.Controls.Add(this.mod_type_frame);
            this.Controls.Add(this.split_vertical);
            this.Controls.Add(this.fw_ver);
            this.Controls.Add(this.SN_info);
            this.Controls.Add(this.release_info);
            this.Controls.Add(this.vehicle_info);
            this.Controls.Add(this.v_frame);
            this.Controls.Add(this.v_type);
            this.Controls.Add(this.frame_cmb);
            this.Controls.Add(this.type_cmb);
            this.Controls.Add(this.lbl_dlfw);
            this.Controls.Add(this.lbl_Custom_firmware_label);
            this.Name = "ConfigFirmware";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private Label lbl_Custom_firmware_label;
        private Label lbl_dlfw;
        private ComboBox type_cmb;
        private ComboBox frame_cmb;
        private Label v_type;
        private Label v_frame;
        private Label vehicle_info;
        private Label release_info;
        private Label SN_info;
        private Label fw_ver;
        private GroupBox split_vertical;
        private Label mod_type_frame;
        private Label lb_recovery;
        private Controls.MyButton btn_refresh_sn;
        private Controls.MyButton btn_sn;
        private TextBox tb_sn_new;
    }
}