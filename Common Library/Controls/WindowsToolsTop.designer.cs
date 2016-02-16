namespace DamirM.CommonLibrary
{
    partial class WindowsToolsTop
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
            this.lName = new System.Windows.Forms.Label();
            this.lClose = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lName
            // 
            this.lName.AutoSize = true;
            this.lName.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.lName.ForeColor = System.Drawing.SystemColors.Info;
            this.lName.Location = new System.Drawing.Point(3, 3);
            this.lName.MaximumSize = new System.Drawing.Size(0, 15);
            this.lName.MinimumSize = new System.Drawing.Size(0, 15);
            this.lName.Name = "lName";
            this.lName.Size = new System.Drawing.Size(104, 15);
            this.lName.TabIndex = 1;
            this.lName.Text = "Tool Windows name";
            // 
            // lClose
            // 
            this.lClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lClose.AutoSize = true;
            this.lClose.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.lClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lClose.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lClose.Location = new System.Drawing.Point(130, 3);
            this.lClose.Name = "lClose";
            this.lClose.Size = new System.Drawing.Size(17, 16);
            this.lClose.TabIndex = 2;
            this.lClose.Tag = "";
            this.lClose.Text = "X";
            this.lClose.MouseLeave += new System.EventHandler(this.lClose_MouseLeave);
            this.lClose.MouseEnter += new System.EventHandler(this.lClose_MouseEnter);
            // 
            // WindowsToolsTop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.Controls.Add(this.lClose);
            this.Controls.Add(this.lName);
            this.MaximumSize = new System.Drawing.Size(0, 20);
            this.MinimumSize = new System.Drawing.Size(150, 20);
            this.Name = "WindowsToolsTop";
            this.Size = new System.Drawing.Size(150, 20);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lName;
        private System.Windows.Forms.Label lClose;
    }
}
