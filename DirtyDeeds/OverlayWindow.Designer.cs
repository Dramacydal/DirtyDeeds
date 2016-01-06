namespace DD
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
            this.dirtyDeedsLabel = new System.Windows.Forms.Label();
            this.statsHolder = new DirtyDeedsControls.TranslucentPanel();
            this.statsTranslucentPanel = new DirtyDeedsControls.TranslucentPanel();
            this.statsTextBox = new DirtyDeedsControls.TransparentRichTextBox();
            this.statsRefreshButton = new System.Windows.Forms.Button();
            this.statsExpandButton = new DirtyDeedsControls.ExpandButton();
            this.logHolder = new DirtyDeedsControls.TranslucentPanel();
            this.logClearButton = new System.Windows.Forms.Button();
            this.logExpandButton = new DirtyDeedsControls.ExpandButton();
            this.logTranslucentPanel = new DirtyDeedsControls.TranslucentPanel();
            this.logTextBox = new DirtyDeedsControls.TransparentRichTextBox();
            this.mobInfoTranslucentPanel = new DirtyDeedsControls.TranslucentPanel();
            this.mobInfoRichTextBox = new DirtyDeedsControls.TransparentRichTextBox();
            this.mobInfoButton = new System.Windows.Forms.Button();
            this.statsHolder.SuspendLayout();
            this.statsTranslucentPanel.SuspendLayout();
            this.logHolder.SuspendLayout();
            this.logTranslucentPanel.SuspendLayout();
            this.mobInfoTranslucentPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // dirtyDeedsLabel
            // 
            this.dirtyDeedsLabel.AutoSize = true;
            this.dirtyDeedsLabel.Font = new System.Drawing.Font("Franklin Gothic Medium", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dirtyDeedsLabel.ForeColor = System.Drawing.Color.Maroon;
            this.dirtyDeedsLabel.Location = new System.Drawing.Point(605, 893);
            this.dirtyDeedsLabel.Name = "dirtyDeedsLabel";
            this.dirtyDeedsLabel.Size = new System.Drawing.Size(40, 28);
            this.dirtyDeedsLabel.TabIndex = 3;
            this.dirtyDeedsLabel.Text = "DD";
            this.dirtyDeedsLabel.Visible = false;
            // 
            // statsHolder
            // 
            this.statsHolder.BackColor = System.Drawing.Color.Transparent;
            this.statsHolder.Controls.Add(this.statsTranslucentPanel);
            this.statsHolder.Controls.Add(this.statsRefreshButton);
            this.statsHolder.Controls.Add(this.statsExpandButton);
            this.statsHolder.Location = new System.Drawing.Point(645, 30);
            this.statsHolder.Name = "statsHolder";
            this.statsHolder.Size = new System.Drawing.Size(165, 369);
            this.statsHolder.TabIndex = 8;
            this.statsHolder.Visible = false;
            // 
            // statsTranslucentPanel
            // 
            this.statsTranslucentPanel.BackColor = System.Drawing.Color.Transparent;
            this.statsTranslucentPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.statsTranslucentPanel.Controls.Add(this.statsTextBox);
            this.statsTranslucentPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.statsTranslucentPanel.Location = new System.Drawing.Point(0, 23);
            this.statsTranslucentPanel.Name = "statsTranslucentPanel";
            this.statsTranslucentPanel.Size = new System.Drawing.Size(157, 346);
            this.statsTranslucentPanel.TabIndex = 1;
            // 
            // statsTextBox
            // 
            this.statsTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.statsTextBox.DetectUrls = false;
            this.statsTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statsTextBox.Enabled = false;
            this.statsTextBox.Font = new System.Drawing.Font("Candara", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.statsTextBox.ForeColor = System.Drawing.Color.White;
            this.statsTextBox.Location = new System.Drawing.Point(0, 0);
            this.statsTextBox.Name = "statsTextBox";
            this.statsTextBox.Size = new System.Drawing.Size(155, 344);
            this.statsTextBox.TabIndex = 0;
            this.statsTextBox.Text = "";
            this.statsTextBox.WordWrap = false;
            // 
            // statsRefreshButton
            // 
            this.statsRefreshButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.statsRefreshButton.Location = new System.Drawing.Point(95, 3);
            this.statsRefreshButton.Name = "statsRefreshButton";
            this.statsRefreshButton.Size = new System.Drawing.Size(50, 19);
            this.statsRefreshButton.TabIndex = 2;
            this.statsRefreshButton.Text = "Refresh";
            this.statsRefreshButton.UseVisualStyleBackColor = true;
            this.statsRefreshButton.Visible = false;
            this.statsRefreshButton.Click += new System.EventHandler(this.statsRefreshButton_Click);
            // 
            // statsExpandButton
            // 
            this.statsExpandButton.ChildPanel = this.statsTranslucentPanel;
            this.statsExpandButton.Expanded = false;
            this.statsExpandButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.statsExpandButton.Location = new System.Drawing.Point(145, 3);
            this.statsExpandButton.Name = "statsExpandButton";
            this.statsExpandButton.Size = new System.Drawing.Size(19, 19);
            this.statsExpandButton.TabIndex = 0;
            this.statsExpandButton.Text = " /\\";
            this.statsExpandButton.UseVisualStyleBackColor = true;
            this.statsExpandButton.Click += new System.EventHandler(this.statsExpandButton_Click);
            // 
            // logHolder
            // 
            this.logHolder.BackColor = System.Drawing.Color.Transparent;
            this.logHolder.Controls.Add(this.logClearButton);
            this.logHolder.Controls.Add(this.logExpandButton);
            this.logHolder.Controls.Add(this.logTranslucentPanel);
            this.logHolder.Location = new System.Drawing.Point(87, 405);
            this.logHolder.Name = "logHolder";
            this.logHolder.Size = new System.Drawing.Size(537, 97);
            this.logHolder.TabIndex = 7;
            this.logHolder.Visible = false;
            // 
            // logClearButton
            // 
            this.logClearButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 5F);
            this.logClearButton.Location = new System.Drawing.Point(0, 62);
            this.logClearButton.Name = "logClearButton";
            this.logClearButton.Size = new System.Drawing.Size(19, 20);
            this.logClearButton.TabIndex = 9;
            this.logClearButton.Text = "O";
            this.logClearButton.UseVisualStyleBackColor = true;
            this.logClearButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // logExpandButton
            // 
            this.logExpandButton.ChildPanel = this.logTranslucentPanel;
            this.logExpandButton.Expanded = true;
            this.logExpandButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.logExpandButton.Location = new System.Drawing.Point(0, 81);
            this.logExpandButton.Name = "logExpandButton";
            this.logExpandButton.Size = new System.Drawing.Size(19, 16);
            this.logExpandButton.TabIndex = 4;
            this.logExpandButton.Text = " \\/";
            this.logExpandButton.UseVisualStyleBackColor = true;
            this.logExpandButton.Click += new System.EventHandler(this.logExpandButton_Click);
            // 
            // logTranslucentPanel
            // 
            this.logTranslucentPanel.AutoScroll = true;
            this.logTranslucentPanel.BackColor = System.Drawing.Color.Transparent;
            this.logTranslucentPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.logTranslucentPanel.Controls.Add(this.logTextBox);
            this.logTranslucentPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.logTranslucentPanel.Location = new System.Drawing.Point(19, 0);
            this.logTranslucentPanel.Name = "logTranslucentPanel";
            this.logTranslucentPanel.Size = new System.Drawing.Size(518, 97);
            this.logTranslucentPanel.TabIndex = 2;
            // 
            // logTextBox
            // 
            this.logTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.logTextBox.DetectUrls = false;
            this.logTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logTextBox.Enabled = false;
            this.logTextBox.Font = new System.Drawing.Font("Candara", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.logTextBox.ForeColor = System.Drawing.Color.White;
            this.logTextBox.Location = new System.Drawing.Point(0, 0);
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.logTextBox.ShortcutsEnabled = false;
            this.logTextBox.Size = new System.Drawing.Size(516, 95);
            this.logTextBox.TabIndex = 1;
            this.logTextBox.Text = "";
            this.logTextBox.WordWrap = false;
            // 
            // mobInfoTranslucentPanel
            // 
            this.mobInfoTranslucentPanel.BackColor = System.Drawing.Color.Transparent;
            this.mobInfoTranslucentPanel.Controls.Add(this.mobInfoButton);
            this.mobInfoTranslucentPanel.Controls.Add(this.mobInfoRichTextBox);
            this.mobInfoTranslucentPanel.Location = new System.Drawing.Point(87, 30);
            this.mobInfoTranslucentPanel.Name = "mobInfoTranslucentPanel";
            this.mobInfoTranslucentPanel.Size = new System.Drawing.Size(431, 152);
            this.mobInfoTranslucentPanel.TabIndex = 9;
            // 
            // mobInfoRichTextBox
            // 
            this.mobInfoRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mobInfoRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mobInfoRichTextBox.Location = new System.Drawing.Point(0, 0);
            this.mobInfoRichTextBox.Name = "mobInfoRichTextBox";
            this.mobInfoRichTextBox.Size = new System.Drawing.Size(431, 152);
            this.mobInfoRichTextBox.TabIndex = 0;
            this.mobInfoRichTextBox.Text = "";
            // 
            // mobInfoButton
            // 
            this.mobInfoButton.Location = new System.Drawing.Point(1, 4);
            this.mobInfoButton.Name = "mobInfoButton";
            this.mobInfoButton.Size = new System.Drawing.Size(24, 23);
            this.mobInfoButton.TabIndex = 1;
            this.mobInfoButton.UseVisualStyleBackColor = true;
            // 
            // OverlayWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MidnightBlue;
            this.ClientSize = new System.Drawing.Size(1400, 1212);
            this.Controls.Add(this.mobInfoTranslucentPanel);
            this.Controls.Add(this.statsHolder);
            this.Controls.Add(this.logHolder);
            this.Controls.Add(this.dirtyDeedsLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "OverlayWindow";
            this.ShowInTaskbar = false;
            this.Text = "OverlayWindow";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.MidnightBlue;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OverlayWindow_FormClosing);
            this.Load += new System.EventHandler(this.OverlayWindow_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.OverlayWindow_Paint);
            this.statsHolder.ResumeLayout(false);
            this.statsTranslucentPanel.ResumeLayout(false);
            this.logHolder.ResumeLayout(false);
            this.logTranslucentPanel.ResumeLayout(false);
            this.mobInfoTranslucentPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public DirtyDeedsControls.TransparentRichTextBox logTextBox;
        private DirtyDeedsControls.TranslucentPanel logTranslucentPanel;
        private System.Windows.Forms.Label dirtyDeedsLabel;
        private DirtyDeedsControls.ExpandButton logExpandButton;
        private DirtyDeedsControls.TranslucentPanel logHolder;
        private DirtyDeedsControls.TranslucentPanel statsHolder;
        private DirtyDeedsControls.ExpandButton statsExpandButton;
        private DirtyDeedsControls.TranslucentPanel statsTranslucentPanel;
        private System.Windows.Forms.Button statsRefreshButton;
        private DirtyDeedsControls.TransparentRichTextBox statsTextBox;
        private System.Windows.Forms.Button logClearButton;
        public DirtyDeedsControls.TranslucentPanel mobInfoTranslucentPanel;
        public System.Windows.Forms.Button mobInfoButton;
        public DirtyDeedsControls.TransparentRichTextBox mobInfoRichTextBox;
    }
}