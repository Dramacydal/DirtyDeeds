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
            gameCheckThread.Abort();

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
            this.itchyLabel = new System.Windows.Forms.Label();
            this.logExpandButton = new ItchyControls.ExpandButton();
            this.logTranslucentPanel = new ItchyControls.TranslucentPanel();
            this.logTextBox = new ItchyControls.TransparentRichTextBox();
            this.logTranslucentPanel.SuspendLayout();
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
            // itchyLabel
            // 
            this.itchyLabel.AutoSize = true;
            this.itchyLabel.Font = new System.Drawing.Font("Microsoft Tai Le", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.itchyLabel.ForeColor = System.Drawing.Color.Maroon;
            this.itchyLabel.Location = new System.Drawing.Point(286, 597);
            this.itchyLabel.Name = "itchyLabel";
            this.itchyLabel.Size = new System.Drawing.Size(37, 14);
            this.itchyLabel.TabIndex = 3;
            this.itchyLabel.Text = "ITCHY";
            // 
            // logExpandButton
            // 
            this.logExpandButton.ChildPanel = this.logTranslucentPanel;
            this.logExpandButton.Expanded = true;
            this.logExpandButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.logExpandButton.Location = new System.Drawing.Point(114, 557);
            this.logExpandButton.Name = "logExpandButton";
            this.logExpandButton.Size = new System.Drawing.Size(18, 16);
            this.logExpandButton.TabIndex = 4;
            this.logExpandButton.Text = " \\/";
            this.logExpandButton.UseVisualStyleBackColor = true;
            // 
            // logTranslucentPanel
            // 
            this.logTranslucentPanel.AutoScroll = true;
            this.logTranslucentPanel.BackColor = System.Drawing.Color.Transparent;
            this.logTranslucentPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.logTranslucentPanel.Controls.Add(this.logTextBox);
            this.logTranslucentPanel.Location = new System.Drawing.Point(133, 533);
            this.logTranslucentPanel.Name = "logTranslucentPanel";
            this.logTranslucentPanel.Size = new System.Drawing.Size(556, 40);
            this.logTranslucentPanel.TabIndex = 2;
            // 
            // logTextBox
            // 
            this.logTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.logTextBox.DetectUrls = false;
            this.logTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logTextBox.Enabled = false;
            this.logTextBox.Font = new System.Drawing.Font("Tahoma", 8.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.logTextBox.ForeColor = System.Drawing.Color.SandyBrown;
            this.logTextBox.Location = new System.Drawing.Point(0, 0);
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.logTextBox.ShortcutsEnabled = false;
            this.logTextBox.Size = new System.Drawing.Size(554, 38);
            this.logTextBox.TabIndex = 1;
            this.logTextBox.Text = "";
            this.logTextBox.WordWrap = false;
            // 
            // OverlayWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(800, 647);
            this.Controls.Add(this.logExpandButton);
            this.Controls.Add(this.itchyLabel);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.logTranslucentPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "OverlayWindow";
            this.ShowInTaskbar = false;
            this.Text = "OverlayWindow";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.Black;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OverlayWindow_FormClosing);
            this.Load += new System.EventHandler(this.OverlayWindow_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.OverlayWindow_Paint);
            this.logTranslucentPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        public ItchyControls.TransparentRichTextBox logTextBox;
        private ItchyControls.TranslucentPanel logTranslucentPanel;
        private System.Windows.Forms.Label itchyLabel;
        private ItchyControls.ExpandButton logExpandButton;
    }
}