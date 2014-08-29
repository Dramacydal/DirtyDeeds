namespace Itchy
{
    partial class TestForm
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
            this.translucentPanel1 = new ItchyControls.TranslucentPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.translucentPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // translucentPanel1
            // 
            this.translucentPanel1.BackColor = System.Drawing.Color.Transparent;
            this.translucentPanel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.translucentPanel1.Controls.Add(this.button1);
            this.translucentPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.translucentPanel1.Location = new System.Drawing.Point(0, 0);
            this.translucentPanel1.Name = "translucentPanel1";
            this.translucentPanel1.Size = new System.Drawing.Size(671, 487);
            this.translucentPanel1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(305, 84);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MidnightBlue;
            this.ClientSize = new System.Drawing.Size(671, 487);
            this.ControlBox = false;
            this.Controls.Add(this.translucentPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "TestForm";
            this.ShowInTaskbar = false;
            this.Text = "TestForm";
            this.translucentPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ItchyControls.TranslucentPanel translucentPanel1;
        private System.Windows.Forms.Button button1;

    }
}