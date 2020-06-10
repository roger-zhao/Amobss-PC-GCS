namespace MissionPlanner.Log
{
    partial class LogDownloadMavLink
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogDownloadMavLink));
            this.TXT_seriallog = new System.Windows.Forms.TextBox();
            this.CHK_logs = new System.Windows.Forms.CheckedListBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.labelBytes = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.BUT_clearlogs = new MissionPlanner.Controls.MyButton();
            this.BUT_DLall = new MissionPlanner.Controls.MyButton();
            this.BUT_DLthese = new MissionPlanner.Controls.MyButton();
            this.LabelStatus = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.downsyslog = new MissionPlanner.Controls.MyButton();
            this.sysmem = new MissionPlanner.Controls.MyButton();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // TXT_seriallog
            // 
            resources.ApplyResources(this.TXT_seriallog, "TXT_seriallog");
            this.TXT_seriallog.Name = "TXT_seriallog";
            this.tableLayoutPanel1.SetRowSpan(this.TXT_seriallog, 2);
            this.TXT_seriallog.TextChanged += new System.EventHandler(this.TXT_seriallog_TextChanged);
            // 
            // CHK_logs
            // 
            resources.ApplyResources(this.CHK_logs, "CHK_logs");
            this.CHK_logs.CheckOnClick = true;
            this.CHK_logs.FormattingEnabled = true;
            this.CHK_logs.Name = "CHK_logs";
            this.CHK_logs.SelectedIndexChanged += new System.EventHandler(this.CHK_logs_SelectedIndexChanged);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.TXT_seriallog, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.CHK_logs, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.LabelStatus, 1, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.progressBar1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.labelBytes, 1, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // progressBar1
            // 
            resources.ApplyResources(this.progressBar1, "progressBar1");
            this.progressBar1.Name = "progressBar1";
            // 
            // labelBytes
            // 
            resources.ApplyResources(this.labelBytes, "labelBytes");
            this.labelBytes.Name = "labelBytes";
            this.labelBytes.Click += new System.EventHandler(this.labelBytes_Click);
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this.BUT_clearlogs, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.BUT_DLall, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.BUT_DLthese, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.downsyslog, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.sysmem, 2, 1);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // BUT_clearlogs
            // 
            resources.ApplyResources(this.BUT_clearlogs, "BUT_clearlogs");
            this.BUT_clearlogs.Name = "BUT_clearlogs";
            this.toolTip1.SetToolTip(this.BUT_clearlogs, resources.GetString("BUT_clearlogs.ToolTip"));
            this.BUT_clearlogs.UseVisualStyleBackColor = true;
            this.BUT_clearlogs.Click += new System.EventHandler(this.BUT_clearlogs_Click);
            // 
            // BUT_DLall
            // 
            resources.ApplyResources(this.BUT_DLall, "BUT_DLall");
            this.BUT_DLall.Name = "BUT_DLall";
            this.BUT_DLall.UseVisualStyleBackColor = true;
            this.BUT_DLall.Click += new System.EventHandler(this.BUT_DLall_Click);
            // 
            // BUT_DLthese
            // 
            resources.ApplyResources(this.BUT_DLthese, "BUT_DLthese");
            this.BUT_DLthese.Name = "BUT_DLthese";
            this.toolTip1.SetToolTip(this.BUT_DLthese, resources.GetString("BUT_DLthese.ToolTip"));
            this.BUT_DLthese.UseVisualStyleBackColor = true;
            this.BUT_DLthese.Click += new System.EventHandler(this.BUT_DLthese_Click);
            // 
            // LabelStatus
            // 
            resources.ApplyResources(this.LabelStatus, "LabelStatus");
            this.LabelStatus.ForeColor = System.Drawing.Color.SeaGreen;
            this.LabelStatus.Name = "LabelStatus";
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 400;
            this.toolTip1.ShowAlways = true;
            // 
            // downsyslog
            // 
            resources.ApplyResources(this.downsyslog, "downsyslog");
            this.downsyslog.Name = "downsyslog";
            this.downsyslog.UseVisualStyleBackColor = true;
            this.downsyslog.Click += new System.EventHandler(this.downsyslog_Click);
            // 
            // sysmem
            // 
            resources.ApplyResources(this.sysmem, "sysmem");
            this.sysmem.Name = "sysmem";
            this.sysmem.UseVisualStyleBackColor = true;
            this.sysmem.Click += new System.EventHandler(this.sysmem_Click);
            // 
            // LogDownloadMavLink
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "LogDownloadMavLink";
            this.Load += new System.EventHandler(this.Log_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.MyButton BUT_DLall;
        private Controls.MyButton BUT_DLthese;
        private Controls.MyButton BUT_clearlogs;
        private System.Windows.Forms.CheckedListBox CHK_logs;
        private System.Windows.Forms.TextBox TXT_seriallog;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label LabelStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label labelBytes;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.ToolTip toolTip1;
        private Controls.MyButton downsyslog;
        private Controls.MyButton sysmem;
    }
}