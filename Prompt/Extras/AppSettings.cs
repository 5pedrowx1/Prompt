using System;
using System.IO;
using System.Windows.Forms;

public class AppSettings
{
    //The Default Theme cuz the light on is ugly as Fuck
    public string Theme { get; set; } = "Escuro";

    //Is not working proprely.
    public string Language { get; set; } = "pt";

    //font size of the Prompt
    public int FontSize { get; set; } = 8;

    //Sets the Opacity of the Prompt
    public int Opacity { get; set; } = 100;

    //isFullscreen Yes or No?
    public bool IsFullscreen { get; set; } = false;

    // Defining the path where the settings will be stored in JSON format.
    private static readonly string settingsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");

    public static AppSettings Load()
    {
        if (File.Exists(settingsFilePath))
        {
            try
            {
                // Read the JSON content from the file.
                string json = File.ReadAllText(settingsFilePath);
                var settings = System.Text.Json.JsonSerializer.Deserialize<AppSettings>(json);

                // Return the settings if deserialization is successful; otherwise, return a new instance of AppSettings.
                return settings ?? new AppSettings();
            }
            catch (Exception ex) // Catch any exceptions that occur during file read or deserialization.
            {
                //tells the user to see the log file
                MessageBox.Show($"Falha ao carregar as settings. Veija application.log");
                Logger.LogError("Falha ao carregar as settings: " + ex.Message);
            }
        }
        return new AppSettings();
    }

    // Method to save the current application settings to the JSON file.
    public void Save()
    {
        try
        {
            string json = System.Text.Json.JsonSerializer.Serialize(this);
            File.WriteAllText(settingsFilePath, json);
        }
        catch (Exception ex) // Catch any exceptions that occur during file write.
        {
            //tells the user to see the log file
            MessageBox.Show($"Falha ao salvar as settings. Veija application.log");
            Logger.LogError("Falha ao salvar as settings: " + ex.Message);
        }
    }
}