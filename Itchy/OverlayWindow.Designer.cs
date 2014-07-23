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
            this.transparentRichTextBox1 = new TransparentRichTextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // transparentRichTextBox1
            // 
            this.transparentRichTextBox1.ForeColor = System.Drawing.Color.Red;
            this.transparentRichTextBox1.Location = new System.Drawing.Point(13, 122);
            this.transparentRichTextBox1.Name = "transparentRichTextBox1";
            this.transparentRichTextBox1.Size = new System.Drawing.Size(155, 96);
            this.transparentRichTextBox1.TabIndex = 1;
            this.transparentRichTextBox1.Text = "sdfasdf\nasd\nfasd\nf\nsdf\nsd\nfsd\n";
            // 
            // OverlayWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.transparentRichTextBox1);
            this.Controls.Add(this.button1);
            this.Enabled = false;
            this.Name = "OverlayWindow";
            this.ShowInTaskbar = false;
            this.Text = "OverlayWindow";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.DimGray;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OverlayWindow_FormClosing);
            this.Load += new System.EventHandler(this.OverlayWindow_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private TransparentRichTextBox transparentRichTextBox1;
    }
}