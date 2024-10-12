using System.Windows.Forms;

namespace Prompt
{
    partial class Settings : UserControl
    {
        private Label lblTheme, lblFontSize, lblLanguage, lblFullscreen;
        private NumericUpDown nudFontSize;
        private CheckBox chkFullscreen;

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.lblTheme = new System.Windows.Forms.Label();
            this.lblFontSize = new System.Windows.Forms.Label();
            this.nudFontSize = new System.Windows.Forms.NumericUpDown();
            this.lblLanguage = new System.Windows.Forms.Label();
            this.lblFullscreen = new System.Windows.Forms.Label();
            this.chkFullscreen = new System.Windows.Forms.CheckBox();
            this.lblTransparency = new System.Windows.Forms.Label();
            this.trkTransparency = new System.Windows.Forms.TrackBar();
            this.GrupInterface = new System.Windows.Forms.GroupBox();
            this.CmbLanguage = new System.Windows.Forms.ComboBox();
            this.cmbTheme = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudFontSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkTransparency)).BeginInit();
            this.GrupInterface.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTheme
            // 
            this.lblTheme.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.lblTheme, "lblTheme");
            this.lblTheme.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.lblTheme.Name = "lblTheme";
            // 
            // lblFontSize
            // 
            resources.ApplyResources(this.lblFontSize, "lblFontSize");
            this.lblFontSize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.lblFontSize.Name = "lblFontSize";
            // 
            // nudFontSize
            // 
            this.nudFontSize.BackColor = System.Drawing.Color.Black;
            this.nudFontSize.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nudFontSize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            resources.ApplyResources(this.nudFontSize, "nudFontSize");
            this.nudFontSize.Minimum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.nudFontSize.Name = "nudFontSize";
            this.nudFontSize.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.nudFontSize.ValueChanged += new System.EventHandler(this.NudFontSize_ValueChanged);
            // 
            // lblLanguage
            // 
            resources.ApplyResources(this.lblLanguage, "lblLanguage");
            this.lblLanguage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.lblLanguage.Name = "lblLanguage";
            // 
            // lblFullscreen
            // 
            resources.ApplyResources(this.lblFullscreen, "lblFullscreen");
            this.lblFullscreen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.lblFullscreen.Name = "lblFullscreen";
            // 
            // chkFullscreen
            // 
            resources.ApplyResources(this.chkFullscreen, "chkFullscreen");
            this.chkFullscreen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.chkFullscreen.Name = "chkFullscreen";
            this.chkFullscreen.CheckedChanged += new System.EventHandler(this.ChkFullscreen_CheckedChanged);
            // 
            // lblTransparency
            // 
            resources.ApplyResources(this.lblTransparency, "lblTransparency");
            this.lblTransparency.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.lblTransparency.Name = "lblTransparency";
            // 
            // trkTransparency
            // 
            this.trkTransparency.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.trkTransparency, "trkTransparency");
            this.trkTransparency.Maximum = 100;
            this.trkTransparency.Name = "trkTransparency";
            this.trkTransparency.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trkTransparency.Value = 50;
            this.trkTransparency.Scroll += new System.EventHandler(this.TrkTransparency_Scroll);
            // 
            // GrupInterface
            // 
            this.GrupInterface.Controls.Add(this.CmbLanguage);
            this.GrupInterface.Controls.Add(this.cmbTheme);
            this.GrupInterface.Controls.Add(this.lblTheme);
            this.GrupInterface.Controls.Add(this.lblTransparency);
            this.GrupInterface.Controls.Add(this.nudFontSize);
            this.GrupInterface.Controls.Add(this.trkTransparency);
            this.GrupInterface.Controls.Add(this.lblFontSize);
            this.GrupInterface.Controls.Add(this.lblFullscreen);
            this.GrupInterface.Controls.Add(this.chkFullscreen);
            this.GrupInterface.Controls.Add(this.lblLanguage);
            resources.ApplyResources(this.GrupInterface, "GrupInterface");
            this.GrupInterface.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.GrupInterface.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.GrupInterface.Name = "GrupInterface";
            this.GrupInterface.TabStop = false;
            // 
            // CmbLanguage
            // 
            this.CmbLanguage.BackColor = System.Drawing.Color.Black;
            this.CmbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.CmbLanguage, "CmbLanguage");
            this.CmbLanguage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.CmbLanguage.Name = "CmbLanguage";
            this.CmbLanguage.SelectedIndexChanged += new System.EventHandler(this.CmbLanguage_SelectedIndexChanged);
            // 
            // cmbTheme
            // 
            this.cmbTheme.BackColor = System.Drawing.Color.Black;
            this.cmbTheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmbTheme, "cmbTheme");
            this.cmbTheme.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.cmbTheme.Items.AddRange(new object[] {
            resources.GetString("cmbTheme.Items"),
            resources.GetString("cmbTheme.Items1")});
            this.cmbTheme.Name = "cmbTheme";
            this.cmbTheme.SelectedIndexChanged += new System.EventHandler(this.CmbTheme_SelectedIndexChanged);
            // 
            // Settings
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Controls.Add(this.GrupInterface);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.Name = "Settings";
            ((System.ComponentModel.ISupportInitialize)(this.nudFontSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkTransparency)).EndInit();
            this.GrupInterface.ResumeLayout(false);
            this.GrupInterface.PerformLayout();
            this.ResumeLayout(false);

        }

        private Label lblTransparency;
        private TrackBar trkTransparency;
        private GroupBox GrupInterface;
        private ComboBox cmbTheme;
        private ComboBox CmbLanguage;
    }
}