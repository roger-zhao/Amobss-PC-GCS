namespace FileExplorer
{
    partial class LogForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogForm));
            this.directoryTreeView = new System.Windows.Forms.TreeView();
            this.btn_getlist = new System.Windows.Forms.Button();
            this.inputTextBox = new System.Windows.Forms.TextBox();
            this.btn_downLog = new System.Windows.Forms.Button();
            this.btn_getSysInfo = new System.Windows.Forms.Button();
            this.btn_dispSysInfo = new System.Windows.Forms.Button();
            this.panel1 = new BSE.Windows.Forms.Panel();
            this.cprobar_download = new CircularProgressBar.CircularProgressBar();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // directoryTreeView
            // 
            this.directoryTreeView.CheckBoxes = true;
            this.directoryTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.directoryTreeView.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.directoryTreeView.Location = new System.Drawing.Point(1, 28);
            this.directoryTreeView.Name = "directoryTreeView";
            this.directoryTreeView.Size = new System.Drawing.Size(511, 361);
            this.directoryTreeView.TabIndex = 0;
            this.directoryTreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.directoryTreeView_NodeMouseClick);
            this.directoryTreeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.directoryTreeView_NodeMouseDoubleClick);
            // 
            // btn_getlist
            // 
            this.btn_getlist.Location = new System.Drawing.Point(9, 423);
            this.btn_getlist.Name = "btn_getlist";
            this.btn_getlist.Size = new System.Drawing.Size(94, 21);
            this.btn_getlist.TabIndex = 1;
            this.btn_getlist.Text = "获取日志信息";
            this.btn_getlist.UseVisualStyleBackColor = true;
            this.btn_getlist.Click += new System.EventHandler(this.getLogList_Click);
            // 
            // inputTextBox
            // 
            this.inputTextBox.Location = new System.Drawing.Point(11, 216);
            this.inputTextBox.Name = "inputTextBox";
            this.inputTextBox.Size = new System.Drawing.Size(165, 21);
            this.inputTextBox.TabIndex = 2;
            this.inputTextBox.Text = "C:\\Users\\Ceceilia\\Documents";
            this.inputTextBox.Visible = false;
            // 
            // btn_downLog
            // 
            this.btn_downLog.Location = new System.Drawing.Point(109, 423);
            this.btn_downLog.Name = "btn_downLog";
            this.btn_downLog.Size = new System.Drawing.Size(94, 21);
            this.btn_downLog.TabIndex = 1;
            this.btn_downLog.Text = "下载选中日志";
            this.btn_downLog.UseVisualStyleBackColor = true;
            this.btn_downLog.Click += new System.EventHandler(this.downLog_Click);
            // 
            // btn_getSysInfo
            // 
            this.btn_getSysInfo.Location = new System.Drawing.Point(209, 423);
            this.btn_getSysInfo.Name = "btn_getSysInfo";
            this.btn_getSysInfo.Size = new System.Drawing.Size(94, 21);
            this.btn_getSysInfo.TabIndex = 1;
            this.btn_getSysInfo.Text = "下载系统日志";
            this.btn_getSysInfo.UseVisualStyleBackColor = true;
            this.btn_getSysInfo.Click += new System.EventHandler(this.getLogList_Click);
            // 
            // btn_dispSysInfo
            // 
            this.btn_dispSysInfo.Location = new System.Drawing.Point(309, 422);
            this.btn_dispSysInfo.Name = "btn_dispSysInfo";
            this.btn_dispSysInfo.Size = new System.Drawing.Size(94, 21);
            this.btn_dispSysInfo.TabIndex = 1;
            this.btn_dispSysInfo.Text = "显示系统信息";
            this.btn_dispSysInfo.UseVisualStyleBackColor = true;
            this.btn_dispSysInfo.Click += new System.EventHandler(this.getLogList_Click);
            // 
            // panel1
            // 
            this.panel1.AssociatedSplitter = null;
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.CaptionFont = new System.Drawing.Font("Microsoft YaHei UI", 11.75F, System.Drawing.FontStyle.Bold);
            this.panel1.CaptionHeight = 27;
            this.panel1.Controls.Add(this.directoryTreeView);
            this.panel1.CustomColors.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(184)))), ((int)(((byte)(184)))));
            this.panel1.CustomColors.CaptionCloseIcon = System.Drawing.SystemColors.ControlText;
            this.panel1.CustomColors.CaptionExpandIcon = System.Drawing.SystemColors.ControlText;
            this.panel1.CustomColors.CaptionGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.panel1.CustomColors.CaptionGradientEnd = System.Drawing.SystemColors.ButtonFace;
            this.panel1.CustomColors.CaptionGradientMiddle = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.panel1.CustomColors.CaptionSelectedGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.panel1.CustomColors.CaptionSelectedGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.panel1.CustomColors.CaptionText = System.Drawing.SystemColors.ControlText;
            this.panel1.CustomColors.CollapsedCaptionText = System.Drawing.SystemColors.ControlText;
            this.panel1.CustomColors.ContentGradientBegin = System.Drawing.SystemColors.ButtonFace;
            this.panel1.CustomColors.ContentGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.panel1.CustomColors.InnerBorderColor = System.Drawing.SystemColors.Window;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Font = new System.Drawing.Font("华文细黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panel1.Image = null;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.MinimumSize = new System.Drawing.Size(27, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(513, 390);
            this.panel1.TabIndex = 3;
            this.panel1.Text = "日志列表";
            this.panel1.ToolTipTextCloseIcon = null;
            this.panel1.ToolTipTextExpandIconPanelCollapsed = null;
            this.panel1.ToolTipTextExpandIconPanelExpanded = null;
            // 
            // cprobar_download
            // 
            this.cprobar_download.AnimationFunction = WinFormAnimation.KnownAnimationFunctions.Liner;
            this.cprobar_download.AnimationSpeed = 500;
            this.cprobar_download.BackColor = System.Drawing.Color.Transparent;
            this.cprobar_download.Font = new System.Drawing.Font("华文细黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cprobar_download.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cprobar_download.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cprobar_download.InnerMargin = 2;
            this.cprobar_download.InnerWidth = -1;
            this.cprobar_download.Location = new System.Drawing.Point(426, 396);
            this.cprobar_download.MarqueeAnimationSpeed = 2000;
            this.cprobar_download.Name = "cprobar_download";
            this.cprobar_download.OuterColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.cprobar_download.OuterMargin = -25;
            this.cprobar_download.OuterWidth = 26;
            this.cprobar_download.ProgressColor = System.Drawing.Color.DeepSkyBlue;
            this.cprobar_download.ProgressWidth = 18;
            this.cprobar_download.SecondaryFont = new System.Drawing.Font("宋体", 18F);
            this.cprobar_download.Size = new System.Drawing.Size(75, 73);
            this.cprobar_download.StartAngle = 270;
            this.cprobar_download.SubscriptColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.cprobar_download.SubscriptMargin = new System.Windows.Forms.Padding(10, -35, 0, 0);
            this.cprobar_download.SubscriptText = "";
            this.cprobar_download.SuperscriptColor = System.Drawing.Color.Black;
            this.cprobar_download.SuperscriptMargin = new System.Windows.Forms.Padding(4, 18, 0, 0);
            this.cprobar_download.SuperscriptText = "";
            this.cprobar_download.TabIndex = 1;
            this.cprobar_download.Text = "进度";
            this.cprobar_download.TextMargin = new System.Windows.Forms.Padding(2, 2, 0, 0);
            // 
            // LogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(513, 474);
            this.Controls.Add(this.cprobar_download);
            this.Controls.Add(this.btn_dispSysInfo);
            this.Controls.Add(this.btn_getSysInfo);
            this.Controls.Add(this.btn_downLog);
            this.Controls.Add(this.btn_getlist);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.inputTextBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LogForm";
            this.Text = "日志查看器";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView directoryTreeView;
        private System.Windows.Forms.Button btn_getlist;
        private System.Windows.Forms.TextBox inputTextBox;
        private System.Windows.Forms.Button btn_downLog;
        private System.Windows.Forms.Button btn_getSysInfo;
        private System.Windows.Forms.Button btn_dispSysInfo;
        private BSE.Windows.Forms.Panel panel1;
        private CircularProgressBar.CircularProgressBar cprobar_download;
    }
}

