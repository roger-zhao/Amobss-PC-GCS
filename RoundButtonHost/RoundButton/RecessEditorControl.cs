using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;


namespace GaryPerkin.UserControls.Buttons
{
    [ToolboxItem(false)]
    public partial class RecessEditorControl : UserControl
    {
        public RecessEditorControl()
        {
            InitializeComponent();
        }

        private IWindowsFormsEditorService  editorService = null;
        private int recess = 2;

        public RecessEditorControl(IWindowsFormsEditorService editorService)
        {
          InitializeComponent();
          this.editorService = editorService;
        }

        public int Recess
        {
            get { return recess; }
            set { recess = value; }
        }

        private void pictureBox0_Click(object sender, EventArgs e)
        {
            recess = 0;

            // Close the UI editor upon value selection
            editorService.CloseDropDown();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            recess = 2;

            // Close the UI editor upon value selection
            editorService.CloseDropDown();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            recess = 3;

            // Close the UI editor upon value selection
            editorService.CloseDropDown();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            recess = 4;

            // Close the UI editor upon value selection
            editorService.CloseDropDown();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            recess = 5;

            // Close the UI editor upon value selection
            editorService.CloseDropDown();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            // Get currently selected control
            PictureBox selected = new PictureBox(); ;
            bool nonstandard = false;

            switch (recess)
            {
                case 0:
                    selected = this.pictureBox0;
                    break;
                case 2:
                    selected = this.pictureBox2;
                    break;
                case 3:
                    selected = this.pictureBox3;
                    break;
                case 4:
                    selected = this.pictureBox4;
                    break;
                case 5:
                    selected = this.pictureBox5;
                    break;
                default:
                    nonstandard = true;
                    break;
            }

            if (!nonstandard)
            {
                // Paint the border
                Graphics g = pe.Graphics;
                using (Pen pen = new Pen(Color.Gray, 1))
                {
                    pen.DashStyle = DashStyle.Dot;
                    Rectangle rect = new Rectangle(new Point(selected.Left - 2, selected.Top - 2), 
                                     new Size(selected.Width + 4, selected.Height + 4));
                    g.DrawRectangle(pen, rect);
                }
            }
        }

        /// <summary>
        /// Move the selected recess with Left/Right keys
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RecessEditorControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                recess = recess - 1;
                if (recess == 1)
                    recess = 0;
                if (recess == -1)
                    recess = 5;
                this.Invalidate();
            }
            else if (e.KeyCode == Keys.Right)
            {
                recess = recess + 1;
                if (recess == 1)
                    recess = 2;
                if (recess == 6)
                    recess = 0;
                this.Invalidate();
            }
        }
    }
}
