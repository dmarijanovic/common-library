namespace DamirM.CommonLibrary
{
    partial class InputBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InputBox));
            this.btnUredu = new System.Windows.Forms.Button();
            this.brnPonisti = new System.Windows.Forms.Button();
            this.tbInputText = new System.Windows.Forms.TextBox();
            this.lDisplayText = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnUredu
            // 
            this.btnUredu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUredu.Location = new System.Drawing.Point(184, 67);
            this.btnUredu.Name = "btnUredu";
            this.btnUredu.Size = new System.Drawing.Size(75, 23);
            this.btnUredu.TabIndex = 1;
            this.btnUredu.Text = "&U redu";
            this.btnUredu.UseVisualStyleBackColor = true;
            this.btnUredu.Click += new System.EventHandler(this.btnUredu_Click);
            // 
            // brnPonisti
            // 
            this.brnPonisti.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.brnPonisti.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.brnPonisti.Location = new System.Drawing.Point(265, 67);
            this.brnPonisti.Name = "brnPonisti";
            this.brnPonisti.Size = new System.Drawing.Size(75, 23);
            this.brnPonisti.TabIndex = 2;
            this.brnPonisti.Text = "&Poništi";
            this.brnPonisti.UseVisualStyleBackColor = true;
            this.brnPonisti.Click += new System.EventHandler(this.brnPonisti_Click);
            // 
            // tbInputText
            // 
            this.tbInputText.Location = new System.Drawing.Point(16, 29);
            this.tbInputText.Name = "tbInputText";
            this.tbInputText.Size = new System.Drawing.Size(240, 20);
            this.tbInputText.TabIndex = 0;
            // 
            // lDisplayText
            // 
            this.lDisplayText.AutoSize = true;
            this.lDisplayText.Location = new System.Drawing.Point(14, 13);
            this.lDisplayText.Name = "lDisplayText";
            this.lDisplayText.Size = new System.Drawing.Size(213, 13);
            this.lDisplayText.TabIndex = 4;
            this.lDisplayText.Text = "Da bi ste potvrdili brisanje upišite rijeè \'obriši\'";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(57, 50);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.lDisplayText);
            this.panel1.Controls.Add(this.tbInputText);
            this.panel1.Location = new System.Drawing.Point(78, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(262, 62);
            this.panel1.TabIndex = 7;
            // 
            // InputBox
            // 
            this.AcceptButton = this.btnUredu;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.brnPonisti;
            this.ClientSize = new System.Drawing.Size(344, 95);
            this.Controls.Add(this.btnUredu);
            this.Controls.Add(this.brnPonisti);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "InputBox";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Mobilis - Potvrda";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnUredu;
        private System.Windows.Forms.Button brnPonisti;
        private System.Windows.Forms.TextBox tbInputText;
        private System.Windows.Forms.Label lDisplayText;
        private System.Windows.Forms.Panel panel1;
    }
}