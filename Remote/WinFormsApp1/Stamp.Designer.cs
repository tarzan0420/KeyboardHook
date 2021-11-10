
namespace WinFormsApp1
{
    partial class Stamp
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {            
            this.MainLabel = new System.Windows.Forms.Label();
            this.radioIsoTrue = new System.Windows.Forms.RadioButton();
            this.radioIsoFalse = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // MainLabel
            // 
            this.MainLabel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.MainLabel.Location = new System.Drawing.Point(12, 30);
            this.MainLabel.Name = "MainLabel";
            this.MainLabel.Size = new System.Drawing.Size(245, 44);
            this.MainLabel.TabIndex = 0;
            this.MainLabel.Text = "In attesa...";
            this.MainLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // radioIsoTrue
            // 
            this.radioIsoTrue.AutoSize = true;
            this.radioIsoTrue.Location = new System.Drawing.Point(31, 106);
            this.radioIsoTrue.Name = "radioIsoTrue";
            this.radioIsoTrue.Size = new System.Drawing.Size(75, 19);
            this.radioIsoTrue.TabIndex = 1;
            this.radioIsoTrue.TabStop = true;
            this.radioIsoTrue.Text = "Iso = true";
            this.radioIsoTrue.UseVisualStyleBackColor = true;
            this.radioIsoTrue.Click += new System.EventHandler(this.OnIsoTrueClick);
            // 
            // radioIsoFalse
            // 
            this.radioIsoFalse.AutoSize = true;
            this.radioIsoFalse.Location = new System.Drawing.Point(164, 106);
            this.radioIsoFalse.Name = "radioIsoFalse";
            this.radioIsoFalse.Size = new System.Drawing.Size(78, 19);
            this.radioIsoFalse.TabIndex = 2;
            this.radioIsoFalse.TabStop = true;
            this.radioIsoFalse.Text = "Iso = false";
            this.radioIsoFalse.UseVisualStyleBackColor = true;
            this.radioIsoFalse.Click += new System.EventHandler(this.OnIsoFalseClick);
            // 
            // Stamp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(271, 153);
            this.Controls.Add(this.radioIsoFalse);
            this.Controls.Add(this.radioIsoTrue);
            this.Controls.Add(this.MainLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "Stamp";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "GESAP-Remoting";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Load += new System.EventHandler(this.Stamp_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label MainLabel;
        private System.Windows.Forms.RadioButton radioIsoTrue;
        private System.Windows.Forms.RadioButton radioIsoFalse;
    }
}

