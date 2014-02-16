namespace Demo
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
            this.label1 = new System.Windows.Forms.Label();
            this.cbEye = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbScene = new System.Windows.Forms.ComboBox();
            this.btnRun = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Eye";
            // 
            // cbEye
            // 
            this.cbEye.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEye.FormattingEnabled = true;
            this.cbEye.Items.AddRange(new object[] {
            "Direct3D 9",
            "Direct3D 11",
            "OpenGL 4.x"});
            this.cbEye.Location = new System.Drawing.Point(75, 12);
            this.cbEye.Name = "cbEye";
            this.cbEye.Size = new System.Drawing.Size(121, 21);
            this.cbEye.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Scene";
            // 
            // cbScene
            // 
            this.cbScene.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbScene.FormattingEnabled = true;
            this.cbScene.Items.AddRange(new object[] {
            "Earthlit Night"});
            this.cbScene.Location = new System.Drawing.Point(75, 48);
            this.cbScene.Name = "cbScene";
            this.cbScene.Size = new System.Drawing.Size(121, 21);
            this.cbScene.TabIndex = 3;
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(12, 85);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(184, 25);
            this.btnRun.TabIndex = 4;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(215, 122);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.cbScene);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbEye);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "Beholder Demo";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbEye;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbScene;
        private System.Windows.Forms.Button btnRun;
    }
}

