namespace Prompt
{
    partial class Toast
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Toast));
            this.picIcon = new System.Windows.Forms.PictureBox();
            this.lbType = new System.Windows.Forms.Label();
            this.toastBorder = new System.Windows.Forms.Panel();
            this.lbMessage = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // picIcon
            // 
            this.picIcon.Image = global::Prompt.Properties.Resources.success;
            this.picIcon.Location = new System.Drawing.Point(12, 12);
            this.picIcon.Name = "picIcon";
            this.picIcon.Size = new System.Drawing.Size(33, 35);
            this.picIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picIcon.TabIndex = 0;
            this.picIcon.TabStop = false;
            // 
            // lbType
            // 
            this.lbType.AutoSize = true;
            this.lbType.Font = new System.Drawing.Font("Yu Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbType.Location = new System.Drawing.Point(51, 9);
            this.lbType.Name = "lbType";
            this.lbType.Size = new System.Drawing.Size(39, 17);
            this.lbType.TabIndex = 1;
            this.lbType.Text = "Type";
            // 
            // toastBorder
            // 
            this.toastBorder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.toastBorder.ForeColor = System.Drawing.Color.Black;
            this.toastBorder.Location = new System.Drawing.Point(-3, -2);
            this.toastBorder.Name = "toastBorder";
            this.toastBorder.Size = new System.Drawing.Size(10, 65);
            this.toastBorder.TabIndex = 3;
            // 
            // lbMessage
            // 
            this.lbMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbMessage.Font = new System.Drawing.Font("Yu Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMessage.Location = new System.Drawing.Point(55, 31);
            this.lbMessage.Multiline = true;
            this.lbMessage.Name = "lbMessage";
            this.lbMessage.ReadOnly = true;
            this.lbMessage.Size = new System.Drawing.Size(231, 20);
            this.lbMessage.TabIndex = 4;
            // 
            // Toast
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(298, 59);
            this.Controls.Add(this.lbMessage);
            this.Controls.Add(this.toastBorder);
            this.Controls.Add(this.lbType);
            this.Controls.Add(this.picIcon);
            this.Font = new System.Drawing.Font("Yu Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Toast";
            this.Text = "Toast";
            this.Load += new System.EventHandler(this.Toast_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picIcon;
        private System.Windows.Forms.Label lbType;
        private System.Windows.Forms.Panel toastBorder;
        private System.Windows.Forms.TextBox lbMessage;
    }
}