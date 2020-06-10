namespace MissionPlanner.GCSViews
{
    partial class UserConfig
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserConfig));
            this.backstageView = new MissionPlanner.Controls.BackstageView.BackstageView();
            this.tabControl_userapp = new System.Windows.Forms.TabControl();
            this.SuspendLayout();
            // 
            // backstageView
            // 
            resources.ApplyResources(this.backstageView, "backstageView");
            this.backstageView.HighlightColor1 = System.Drawing.SystemColors.Highlight;
            this.backstageView.HighlightColor2 = System.Drawing.SystemColors.MenuHighlight;
            this.backstageView.Name = "backstageView";
            this.backstageView.WidthMenu = 172;
            // 
            // tabControl_userapp
            // 
            resources.ApplyResources(this.tabControl_userapp, "tabControl_userapp");
            this.tabControl_userapp.Name = "tabControl_userapp";
            this.tabControl_userapp.SelectedIndex = 0;
            // 
            // UserConfig
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tabControl_userapp);
            this.Controls.Add(this.backstageView);
            this.MinimumSize = new System.Drawing.Size(1000, 450);
            this.Name = "UserConfig";
            resources.ApplyResources(this, "$this");
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UserConfig_FormClosing);
            this.Load += new System.EventHandler(this.UserConfig_Load);
            this.ResumeLayout(false);

        }

        #endregion

        internal Controls.BackstageView.BackstageView backstageView;
        private System.Windows.Forms.TabControl tabControl_userapp;
    }
}
