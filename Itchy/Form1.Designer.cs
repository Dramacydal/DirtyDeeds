namespace Itchy
{
    partial class Form1
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.expandButton1 = new ItchyControls.ExpandButton();
            this.translucentPanel1 = new ItchyControls.TranslucentPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.translucentPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(210, 120);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 0;
            // 
            // expandButton1
            // 
            this.expandButton1.ChildPanel = this.translucentPanel1;
            this.expandButton1.Expanded = true;
            this.expandButton1.Location = new System.Drawing.Point(83, 154);
            this.expandButton1.Name = "expandButton1";
            this.expandButton1.Size = new System.Drawing.Size(187, 23);
            this.expandButton1.TabIndex = 1;
            this.expandButton1.Text = "expandButton1 \\/";
            this.expandButton1.UseVisualStyleBackColor = true;
            // 
            // translucentPanel1
            // 
            this.translucentPanel1.BackColor = System.Drawing.Color.Transparent;
            this.translucentPanel1.Controls.Add(this.button1);
            this.translucentPanel1.Location = new System.Drawing.Point(83, 201);
            this.translucentPanel1.Name = "translucentPanel1";
            this.translucentPanel1.Size = new System.Drawing.Size(200, 100);
            this.translucentPanel1.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(33, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(111, 54);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 381);
            this.Controls.Add(this.translucentPanel1);
            this.Controls.Add(this.expandButton1);
            this.Controls.Add(this.comboBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.translucentPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private ItchyControls.ExpandButton expandButton1;
        private ItchyControls.TranslucentPanel translucentPanel1;
        private System.Windows.Forms.Button button1;

    }
}