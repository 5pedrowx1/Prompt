using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Prompt
{
    partial class Form1 : RoundedForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Designer de Windows Forms

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.lblCurrentDirectory = new System.Windows.Forms.Label();
            this.pnlTitleBar = new System.Windows.Forms.Panel();
            this.pnlCommandOutput = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnMinimize = new System.Windows.Forms.Button();
            this.btnMaximize = new System.Windows.Forms.Button();
            this.pnlSettingsContainer = new System.Windows.Forms.Panel();
            this.pnlCommandInput = new System.Windows.Forms.Panel();
            this.Settings = new System.Windows.Forms.Button();
            this.txtCommandInput = new System.Windows.Forms.TextBox();
            this.txtCommandOutput = new System.Windows.Forms.RichTextBox();
            this.pnlTitleBar.SuspendLayout();
            this.pnlCommandInput.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblCurrentDirectory
            // 
            resources.ApplyResources(this.lblCurrentDirectory, "lblCurrentDirectory");
            this.lblCurrentDirectory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.lblCurrentDirectory.Name = "lblCurrentDirectory";
            this.lblCurrentDirectory.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PnlTitleBar_MouseDown);
            this.lblCurrentDirectory.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PnlTitleBar_MouseMove);
            this.lblCurrentDirectory.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PnlTitleBar_MouseUp);
            // 
            // pnlTitleBar
            // 
            this.pnlTitleBar.BackColor = System.Drawing.Color.Black;
            this.pnlTitleBar.Controls.Add(this.pnlCommandOutput);
            this.pnlTitleBar.Controls.Add(this.lblCurrentDirectory);
            this.pnlTitleBar.Controls.Add(this.btnClose);
            this.pnlTitleBar.Controls.Add(this.btnMinimize);
            this.pnlTitleBar.Controls.Add(this.btnMaximize);
            resources.ApplyResources(this.pnlTitleBar, "pnlTitleBar");
            this.pnlTitleBar.Name = "pnlTitleBar";
            this.pnlTitleBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PnlTitleBar_MouseDown);
            this.pnlTitleBar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PnlTitleBar_MouseMove);
            this.pnlTitleBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PnlTitleBar_MouseUp);
            // 
            // pnlCommandOutput
            // 
            resources.ApplyResources(this.pnlCommandOutput, "pnlCommandOutput");
            this.pnlCommandOutput.Name = "pnlCommandOutput";
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.ForeColor = System.Drawing.Color.Black;
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            this.btnClose.MouseEnter += new System.EventHandler(this.Btn_MouseEnter);
            this.btnClose.MouseLeave += new System.EventHandler(this.Btn_MouseLeave);
            // 
            // btnMinimize
            // 
            resources.ApplyResources(this.btnMinimize, "btnMinimize");
            this.btnMinimize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnMinimize.FlatAppearance.BorderSize = 0;
            this.btnMinimize.ForeColor = System.Drawing.Color.Black;
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.UseVisualStyleBackColor = false;
            this.btnMinimize.Click += new System.EventHandler(this.BtnMinimize_Click);
            this.btnMinimize.MouseEnter += new System.EventHandler(this.Btn_MouseEnter);
            this.btnMinimize.MouseLeave += new System.EventHandler(this.Btn_MouseLeave);
            // 
            // btnMaximize
            // 
            resources.ApplyResources(this.btnMaximize, "btnMaximize");
            this.btnMaximize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnMaximize.FlatAppearance.BorderSize = 0;
            this.btnMaximize.ForeColor = System.Drawing.Color.Black;
            this.btnMaximize.Name = "btnMaximize";
            this.btnMaximize.UseVisualStyleBackColor = false;
            this.btnMaximize.Click += new System.EventHandler(this.BtnMaximize_Click);
            this.btnMaximize.MouseEnter += new System.EventHandler(this.Btn_MouseEnter);
            this.btnMaximize.MouseLeave += new System.EventHandler(this.Btn_MouseLeave);
            // 
            // pnlSettingsContainer
            // 
            resources.ApplyResources(this.pnlSettingsContainer, "pnlSettingsContainer");
            this.pnlSettingsContainer.Name = "pnlSettingsContainer";
            // 
            // pnlCommandInput
            // 
            this.pnlCommandInput.BackColor = System.Drawing.Color.Black;
            this.pnlCommandInput.Controls.Add(this.Settings);
            this.pnlCommandInput.Controls.Add(this.txtCommandInput);
            resources.ApplyResources(this.pnlCommandInput, "pnlCommandInput");
            this.pnlCommandInput.Name = "pnlCommandInput";
            // 
            // Settings
            // 
            resources.ApplyResources(this.Settings, "Settings");
            this.Settings.Name = "Settings";
            this.Settings.UseVisualStyleBackColor = true;
            this.Settings.Click += new System.EventHandler(this.Settings_Click);
            // 
            // txtCommandInput
            // 
            this.txtCommandInput.BackColor = System.Drawing.Color.Black;
            this.txtCommandInput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtCommandInput, "txtCommandInput");
            this.txtCommandInput.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.txtCommandInput.Name = "txtCommandInput";
            this.txtCommandInput.TabStop = false;
            this.txtCommandInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtCommandInput_KeyDown);
            // 
            // txtCommandOutput
            // 
            this.txtCommandOutput.BackColor = System.Drawing.Color.Black;
            this.txtCommandOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtCommandOutput, "txtCommandOutput");
            this.txtCommandOutput.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.txtCommandOutput.HideSelection = false;
            this.txtCommandOutput.Name = "txtCommandOutput";
            this.txtCommandOutput.ReadOnly = true;
            this.txtCommandOutput.ShowSelectionMargin = true;
            this.txtCommandOutput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtCommandOutput_KeyDown);
            this.txtCommandOutput.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.MouseWheel);
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.pnlSettingsContainer);
            this.Controls.Add(this.txtCommandOutput);
            this.Controls.Add(this.pnlCommandInput);
            this.Controls.Add(this.pnlTitleBar);
            this.Name = "Form1";
            this.pnlTitleBar.ResumeLayout(false);
            this.pnlTitleBar.PerformLayout();
            this.pnlCommandInput.ResumeLayout(false);
            this.pnlCommandInput.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblCurrentDirectory;
        private System.Windows.Forms.Panel pnlTitleBar;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.Button btnMaximize;
        private System.Windows.Forms.Panel pnlCommandOutput;
        private System.Windows.Forms.Panel pnlCommandInput;
        private System.Windows.Forms.Button Settings;
        private System.Windows.Forms.TextBox txtCommandInput;
        private System.Windows.Forms.Panel pnlSettingsContainer;
        private System.Windows.Forms.RichTextBox txtCommandOutput;
    }
}