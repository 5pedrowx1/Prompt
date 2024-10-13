using System.Drawing;
using System.Windows.Forms;

public static class ThemeManager
{
    public static bool IsDarkTheme { get; private set; }

    // Method to apply a dark theme to a control and its children.
    public static void ApplyDarkTheme(Control control)
    {
        // Set the background and foreground color for the parent control.
        control.BackColor = Color.FromArgb(0, 0, 0);
        control.ForeColor = Color.FromArgb(128, 255, 128);
        ApplyThemeToControls(control, true); // Apply dark theme to child controls.
    }

    // Method to apply a light theme to a control and its children.
    public static void ApplyLightTheme(Control control)
    {
        // Set the background and foreground color for the parent control.
        control.BackColor = Color.White;
        control.ForeColor = Color.Black;
        ApplyThemeToControls(control, false);
    }

    // Helper method to apply a theme recursively to all child controls.
    private static void ApplyThemeToControls(Control control, bool isDarkTheme)
    {
        foreach (Control childControl in control.Controls)
        {
            ApplyThemeToControl(childControl, isDarkTheme); // Apply theme to each child.
        }
    }

    // Method to apply the appropriate theme (dark or light) to a single control.
    public static void ApplyThemeToControl(Control control, bool isDarkTheme)
    {
        if (isDarkTheme)
        {
            ApplyDarkThemeToControl(control); // Apply dark theme.
        }
        else
        {
            ApplyLightThemeToControl(control); // Apply light theme.
        }

        // Recursively apply the theme to child controls.
        foreach (Control childControl in control.Controls)
        {
            ApplyThemeToControl(childControl, isDarkTheme);
        }
    }

    // Method to apply dark theme specifics for various control types.
    private static void ApplyDarkThemeToControl(Control control)
    {
        IsDarkTheme = true; // Set the theme flag.

        // Apply dark theme for specific controls.
        if (control is Button btn)
        {
            btn.BackColor = Color.FromArgb(128, 255, 128);
            btn.ForeColor = Color.Black;
        }
        else if (control is Label lbl)
        {
            lbl.ForeColor = Color.FromArgb(128, 255, 128);
        }
        else if (control is ComboBox cmb)
        {
            cmb.BackColor = Color.FromArgb(45, 45, 48);
            cmb.ForeColor = Color.White;
        }
        else if (control is NumericUpDown nud)
        {
            nud.BackColor = Color.FromArgb(45, 45, 48);
            nud.ForeColor = Color.White;
        }
        else if (control is CheckBox chk)
        {
            chk.ForeColor = Color.FromArgb(128, 255, 128);
        }
        else if (control is TextBox txt)
        {
            txt.BackColor = Color.Black;
            txt.ForeColor = Color.FromArgb(128, 255, 128);
        }
        else if (control is GroupBox gb)
        {
            gb.ForeColor = Color.FromArgb(128, 255, 128);
        }
        else if (control is TrackBar tb)
        {
            tb.BackColor = Color.Black;
            tb.ForeColor = Color.FromArgb(128, 255, 128);
        }
        else if (control is Panel p)
        {
            p.BackColor = Color.Black;
        }
        else if (control is RichTextBox rtb)
        {
            rtb.BackColor = Color.Black;
        }
    }

    // Method to apply light theme specifics for various control types.
    private static void ApplyLightThemeToControl(Control control)
    {
        IsDarkTheme = false; // Set the theme flag.

        // Apply light theme for specific controls.
        if (control is Button btn)
        {
            btn.BackColor = Color.LightGray;
            btn.ForeColor = Color.Black;
        }
        else if (control is Label lbl)
        {
            lbl.ForeColor = Color.Black;
        }
        else if (control is ComboBox cmb)
        {
            cmb.BackColor = Color.White;
            cmb.ForeColor = Color.Black;
        }
        else if (control is NumericUpDown nud)
        {
            nud.BackColor = Color.White;
            nud.ForeColor = Color.Black;
        }
        else if (control is CheckBox chk)
        {
            chk.ForeColor = Color.Black;
        }
        else if (control is TextBox txt)
        {
            txt.BackColor = Color.White;
            txt.ForeColor = Color.Black; 
        }
        else if (control is GroupBox gb)
        {
            gb.ForeColor = Color.Black;
        }
        else if (control is TrackBar tb)
        {
            tb.BackColor = Color.White;
            tb.ForeColor = Color.Black;
        }
        else if (control is Panel p)
        {
            p.BackColor = Color.Gray;
        }
        else if (control is RichTextBox rtb)
        {
            rtb.BackColor = Color.White; 
        }
    }
}