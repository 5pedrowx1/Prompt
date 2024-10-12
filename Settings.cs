using System;
using System.Windows.Forms;

namespace Prompt
{
    public partial class Settings : UserControl
    {
        private readonly AppSettings appSettings;
        private readonly LanguageManager languageManager;

        // Reference to the main form (Form1)
        public Form1 MainForm { get; set; }

        // Property for managing font size from the NumericUpDown (nudFontSize)
        public int FontSize
        {
            get { return (int)nudFontSize.Value; }
            set
            {
                // Only update if the value has actually changed
                if (FontSize != value)
                {
                    nudFontSize.Value = value;
                    // Raise event when font size changes
                    OnFontSizeChanged(EventArgs.Empty);
                }
            }
        }

        // Property for determining whether the dark theme is selected
        public bool IsDarkTheme
        {
            get { return appSettings.Theme == "Escuro"; }
            set
            {
                // Update the theme only if there is a change
                if (IsDarkTheme != value)
                {
                    appSettings.Theme = value ? "Escuro" : "Claro";
                    OnThemeChanged(EventArgs.Empty); // Trigger theme changed event
                    ApplyTheme(); // Apply the selected theme
                }
            }
        }

        // Event that is raised when the theme changes
        public event EventHandler ThemeChanged;
        // Event that is raised when the font size changes
        public event EventHandler FontSizeChanged;

        // Method to raise the ThemeChanged event
        protected virtual void OnThemeChanged(EventArgs e)
        {
            ThemeChanged?.Invoke(this, e);
        }

        // Method to raise the FontSizeChanged event
        protected virtual void OnFontSizeChanged(EventArgs e)
        {
            FontSizeChanged?.Invoke(this, e);
        }

        // Default constructor that loads settings and language manager
        public Settings()
        {
            InitializeComponent();
            appSettings = AppSettings.Load(); // Load the application settings
            languageManager = new LanguageManager(); // Initialize language manager
            LoadSettings(); // Load settings into UI controls
            trkTransparency.Value = appSettings.Opacity; // Set the transparency slider value
        }

        // Constructor that also accepts a reference to the main form
        public Settings(Form1 mainForm) : this()
        {
            MainForm = mainForm ?? throw new ArgumentNullException(nameof(mainForm));
        }

        // Event handler for when the user changes the theme from the combo box
        private void CmbTheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedTheme = cmbTheme.SelectedItem?.ToString() ?? "Claro";
            appSettings.Theme = selectedTheme; // Save the selected theme
            bool isDarkTheme = selectedTheme.Equals("Escuro", StringComparison.OrdinalIgnoreCase);
            ApplyTheme(); // Apply the selected theme

            MainForm?.ApplyTheme(this, isDarkTheme); // Apply the theme to the main form
            appSettings.Save(); // Save the settings to file
            ThemeChanged?.Invoke(this, EventArgs.Empty); // Raise the theme changed event
        }

        // Method to apply the selected theme (dark or light)
        private void ApplyTheme()
        {
            if (IsDarkTheme)
            {
                ThemeManager.ApplyDarkTheme(this); // Apply dark theme to the control
            }
            else
            {
                ThemeManager.ApplyLightTheme(this); // Apply light theme to the control
            }
        }

        // Event handler for when the font size is changed by the user
        private void NudFontSize_ValueChanged(object sender, EventArgs e)
        {
            int fontSize = (int)nudFontSize.Value;
            appSettings.FontSize = fontSize; // Save the new font size in settings
            appSettings.Save(); // Persist the changes

            MainForm?.UpdateFontSize(fontSize); // Update the font size in the main form
            OnFontSizeChanged(EventArgs.Empty); // Raise the font size changed event
        }

        //not working...
        // Save the selected language to the app settings
        private void SaveLanguage()
        {
            appSettings.Language = cmbTheme.SelectedItem?.ToString() ?? "pt";
            appSettings.Save(); // Save the language settings
        }

        // Load the settings from the configuration file into the UI controls
        private void LoadSettings()
        {
            if (appSettings != null)
            {
                // Set the selected theme in the combo box
                cmbTheme.SelectedItem = appSettings.Theme ?? "Claro";
                ApplyTheme();

                //not working....
                // Set the selected language in the combo box
                cmbTheme.SelectedItem = appSettings.Language ?? "pt";
                languageManager.ApplyCulture(this, appSettings.Language ?? "pt"); // Apply the language

                // Set the font size from settings
                nudFontSize.Value = appSettings.FontSize > 0 ? appSettings.FontSize : 12;

                // Set the fullscreen checkbox
                chkFullscreen.Checked = appSettings.IsFullscreen;

                // Apply the fullscreen mode
                ApplyFullscreenMode();

                // Set the transparency if MainForm is available
                if (MainForm != null)
                {
                    MainForm.Opacity = appSettings.Opacity / 100.0;
                    trkTransparency.Value = appSettings.Opacity > 0 ? appSettings.Opacity : 100;
                }
            }
            else
            {
                // Display an error message if settings failed to load
                MessageBox.Show("As configurações não puderam ser carregadas.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.LogError("As configurações não puderam ser carregadas.");
            }
        }

        //not working......
        // Event handler for when the user selects a different language
        private void CmbLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveLanguage(); // Save the selected language
            string selectedLanguage = cmbTheme.SelectedItem?.ToString() ?? "pt";
            languageManager.ApplyCulture(this, selectedLanguage); // Apply the selected language
        }

        // Event handler for adjusting transparency when the trackbar is scrolled
        private void TrkTransparency_Scroll(object sender, EventArgs e)
        {
            if (MainForm != null)
            {
                MainForm.Opacity = trkTransparency.Value / 100.0; // Update the form's opacity
            }

            appSettings.Opacity = trkTransparency.Value; // Save the new opacity value
            appSettings.Save(); // Persist the changes
        }

        // Event handler for when the fullscreen checkbox is changed
        private void ChkFullscreen_CheckedChanged(object sender, EventArgs e)
        {
            appSettings.IsFullscreen = chkFullscreen.Checked; // Save the fullscreen setting
            appSettings.Save(); // Persist the changes
            ApplyFullscreenMode(); // Apply fullscreen mode
        }

        public void ApplyFullscreenMode()
        {
            if (MainForm == null)
            {
                return;
            }

            // Apply maximized state if fullscreen is checiked, otherwse set normal
            if (chkFullscreen != null && chkFullscreen.Checked)
            {
                MainForm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                MainForm.WindowState = FormWindowState.Normal;
            }
            appSettings.Save(); // Save the window state
        }
    }
}