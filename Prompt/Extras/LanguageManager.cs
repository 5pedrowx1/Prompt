using System;
using System.Globalization;
using System.Resources;
using System.Threading;
using System.Windows.Forms;

//this bullshit is not working so if you can fix it just say it.

public class LanguageManager
{
    // ResourceManager is used to retrieve localized strings from resource files.
    private readonly ResourceManager resourceManager;

    // Constructor initializes the ResourceManager to load resources from the "Prompt.Settings" file.
    public LanguageManager()
    {
        resourceManager = new ResourceManager("Prompt.Settings", typeof(LanguageManager).Assembly);
    }

    // Applies a specific culture to the provided control and its child controls, based on the cultureCode.
    public void ApplyCulture(Control control, string cultureCode)
    {
        if (control == null) throw new ArgumentNullException(nameof(control));

        // Sets the culture information based on the provided culture code (e.g., "en" for English, "es" for Spanish, "pt" for Portuguese).
        CultureInfo cultureInfo = cultureCode switch
        {
            "en" => new CultureInfo("en"),
            "es" => new CultureInfo("es"),
            "pt" => new CultureInfo("pt"),
            _ => new CultureInfo("pt"),
        };

        // Updates the thread's UI culture to the specified culture.
        Thread.CurrentThread.CurrentUICulture = cultureInfo;
        UpdateControlText(control);
    }

    // Updates the text of the control based on its localized string from the resource file.
    private void UpdateControlText(Control control)
    {
        // If the control is null, exit the method.
        if (control == null) return;

        // The resource key is the control's name (e.g., the control's Name property will be used to retrieve the appropriate string).
        string resourceKey = control.Name;
        // Attempts to get the localized string for the control using its name as the resource key.
        string localizedString = resourceManager.GetString(resourceKey, Thread.CurrentThread.CurrentUICulture);

        // Checks the type of control and updates its text if applicable (Label, Button, CheckBox, GroupBox).
        if (control is Label lbl)
        {
            lbl.Text = localizedString ?? lbl.Text;
        }
        else if (control is Button btn)
        {
            btn.Text = localizedString ?? btn.Text;
        }
        else if (control is CheckBox chk)
        {
            chk.Text = localizedString ?? chk.Text;
        }
        else if (control is GroupBox gb)
        {
            gb.Text = localizedString ?? gb.Text;
        }

        // Recursively updates all child controls' text by calling UpdateControlText on each child control.
        foreach (Control childControl in control.Controls)
        {
            UpdateControlText(childControl);
        }
    }
}