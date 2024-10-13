using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Prompt;

public class CheckForUpdates
{
    private readonly Form1 form;
    private static readonly string currentVersion = "1.9";
    private static readonly string versionUrl = "https://pedrogamery.github.io/Prompt/version.json";

    public Form1 Form => form;

    public static string CurrentVersion => currentVersion;

    public CheckForUpdates(Form1 form)
    {
        this.form = form;
    }

    public async Task CheckAndUpdate()
    {
        try
        {
            using HttpClient client = new HttpClient();
            string jsonResponse = await client.GetStringAsync(versionUrl);
            JObject json = JObject.Parse(jsonResponse);
            string latestVersion = json["latestVersion"].ToString();
            string downloadUrl = json["downloadUrl"].ToString();

            if (new Version(latestVersion) > new Version(currentVersion))
            {
                Console.WriteLine($"Nova versão {latestVersion} disponível!");
                ShowUpdateNotification();
                await DownloadAndInstallUpdate(downloadUrl);
            }
            else
            {
                Console.WriteLine("Você já está na versão mais recente.");
            }
        }
        catch (Exception ex)
        {
            Logger.LogError($"Erro ao verificar atualizações: {ex.Message}");
        }
    }

    private void ShowToast(string type, string message)
    { 
        Toast toast = new Toast(type, message);
        toast.Show();
    }

    private void ShowUpdateNotification()
    {
        ShowToast("Info", "Nova Atualização disponival, sera Baixada agora!");
    }

    private async Task DownloadAndInstallUpdate(string downloadUrl)
    {
        try
        {
            string installerPath = Path.Combine(Path.GetTempPath(), "PromptSetup.exe");

            using (HttpClient client = new HttpClient())
            {
                byte[] installerData = await client.GetByteArrayAsync(downloadUrl);
                File.WriteAllBytes(installerPath, installerData);
            }
            Process.Start(installerPath);
            Environment.Exit(0);
        }
        catch (Exception ex)
        {
            Logger.LogError($"Erro ao baixar e instalar a atualização: {ex.Message}");
        }
    }
}