namespace Itchy
{
    partial class OverlayWindow
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
            disposing = true;
            syncThread.Abort();

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
            this.button1 = new System.Windows.Forms.Button();
            this.transparentRichTextBox = new ItchyControls.TransparentRichTextBox();
            this.translucentPanel1 = new ItchyControls.TranslucentPanel();
            this.translucentPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 26);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // transparentRichTextBox
            // 
            this.transparentRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.transparentRichTextBox.DetectUrls = false;
            this.transparentRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.transparentRichTextBox.Enabled = false;
            this.transparentRichTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.transparentRichTextBox.ForeColor = System.Drawing.Color.Red;
            this.transparentRichTextBox.Location = new System.Drawing.Point(0, 0);
            this.transparentRichTextBox.Name = "transparentRichTextBox";
            this.transparentRichTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.transparentRichTextBox.Size = new System.Drawing.Size(200, 80);
            this.transparentRichTextBox.TabIndex = 1;
            this.transparentRichTextBox.Text = "sdfasdf\nasd\nfasd\nf\nsdf\nsd\nfsd\n";
            // 
            // translucentPanel1
            // 
            this.translucentPanel1.BackColor = System.Drawing.Color.Transparent;
            this.translucentPanel1.Controls.Add(this.transparentRichTextBox);
            this.translucentPanel1.Location = new System.Drawing.Point(12, 270);
            this.translucentPanel1.Name = "translucentPanel1";
            this.translucentPanel1.Size = new System.Drawing.Size(200, 80);
            this.translucentPanel1.TabIndex = 2;
            // 
            // OverlayWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(615, 377);
            this.Controls.Add(this.translucentPanel1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "OverlayWindow";
            this.ShowInTaskbar = false;
            this.Text = "OverlayWindow";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.DimGray;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OverlayWindow_FormClosing);
            this.Load += new System.EventHandler(this.OverlayWindow_Load);
            this.translucentPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private ItchyControls.TransparentRichTextBox transparentRichTextBox;
        private ItchyControls.TranslucentPanel translucentPanel1;
    }
}