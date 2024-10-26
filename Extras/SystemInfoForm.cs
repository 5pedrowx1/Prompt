using System;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;

namespace Prompt.Extras
{
    public partial class SystemInfoForm : RoundedForm
    {
        public SystemInfoForm()
        {
            InitializeComponent();
            CustomizeButtons();
            DisplaySystemInfo();
        }

        private void CustomizeButtons()
        {
            btnClose.CustomizeRoundedButton();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private ManagementObjectCollection GetWMIValues(string className)
        {
            using var searcher = new ManagementObjectSearcher($"SELECT * FROM {className}");
            return searcher.Get();
        }

        private void DisplaySystemInfo()
        {
            // Nome do Host
            LblHostName.Text = $"Nome do Host: {Environment.MachineName}";

            // Informações do SO
            var osCollection = GetWMIValues("Win32_OperatingSystem");
            foreach (ManagementObject os in osCollection)
            {
                LblOSName.Text = $"Nome do SO: {GetSafeValue(os, "Caption")}";
                LblOSVersion.Text = $"Versão do SO: {GetSafeValue(os, "Version")}";
                LblOSManufacturer.Text = $"Fabricante do SO: {GetSafeValue(os, "Manufacturer")}";
                LblOSConfiguration.Text = $"Configuração do SO: {GetSafeValue(os, "OSArchitecture")}";
                LblRegisteredOwner.Text = $"Proprietário Registrado: {GetSafeValue(os, "RegisteredUser")}";
                LblProductID.Text = $"ID do Produto: {GetSafeValue(os, "SerialNumber")}";
                LblInstallDate.Text = os["InstallDate"] != null ?
                    $"Data de Instalação Original: {ManagementDateTimeConverter.ToDateTime(os["InstallDate"].ToString()):dd/MM/yyyy}" :
                    "Data de Instalação Original: Indisponível";
                LblSystemBootTime.Text = os["LastBootUpTime"] != null ?
                    $"Hora de Inicialização do Sistema: {ManagementDateTimeConverter.ToDateTime(os["LastBootUpTime"].ToString()):dd/MM/yyyy HH:mm:ss}" :
                    "Hora de Inicialização do Sistema: Indisponível";
            }

            // Informações do Sistema
            var systemCollection = GetWMIValues("Win32_ComputerSystem");
            foreach (ManagementObject system in systemCollection)
            {
                LblSystemManufacturer.Text = $"Fabricante do Sistema: {GetSafeValue(system, "Manufacturer")}";
                LblSystemModel.Text = $"Modelo do Sistema: {GetSafeValue(system, "Model")}";
                LblSystemType.Text = $"Tipo de Sistema: {GetSafeValue(system, "SystemType")}";
                LblTotalPhysicalMemory.Text = $"Memória Física Total: {FormatBytes(GetSafeUInt64(system, "TotalPhysicalMemory"))}";
                LblAvailablePhysicalMemory.Text = $"Memória Física Disponível: {FormatBytes(GetSafeUInt64(system, "FreePhysicalMemory"))}";
            }

            // Informações do Processador
            var processorCollection = GetWMIValues("Win32_Processor");
            string processorNames = string.Join(", ", processorCollection.Cast<ManagementObject>().Select(p => GetSafeValue(p, "Name")));
            LblProcessor.Text = $"Processador(es): {processorNames}";

            // Informações da BIOS
            var biosCollection = GetWMIValues("Win32_BIOS");
            foreach (ManagementObject bios in biosCollection)
            {
                LblBIOSVersion.Text = $"Versão da BIOS: {GetSafeValue(bios, "Version")}";
            }

            // Diretórios do Windows e Sistema
            LblWindowsDirectory.Text = $"Diretório do Windows: {Environment.GetFolderPath(Environment.SpecialFolder.Windows)}";
            LblSystemDirectory.Text = $"Diretório do Sistema: {Environment.SystemDirectory}";

            // Dispositivo de Inicialização
            if (osCollection.Count > 0)
            {
                LblBootDevice.Text = $"Dispositivo de Inicialização: {GetSafeValue(osCollection.Cast<ManagementObject>().First(), "BootDevice")}";
            }

            // Informações de Memória Virtual
            LblVirtualMemory.Text = $"Memória Virtual: {FormatBytes(GetSafeUInt64(systemCollection.Cast<ManagementObject>().First(), "TotalVirtualMemorySize"))}";

            // Informações do Domínio e Servidor de Logon
            foreach (ManagementObject system in systemCollection)
            {
                LblDomain.Text = $"Domínio: {GetSafeValue(system, "Domain")}";
                LblLogonServer.Text = $"Servidor de Logon: {GetSafeValue(system, "LogonServer")}";
            }

            // Informações das Placas de Rede
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            LblNetworkCards.Text = "Placa(s) de Rede: " + string.Join(", ", networkInterfaces.Select(nic => nic.Name));
        }

        private string GetSafeValue(ManagementObject obj, string propertyName)
        {
            try
            {
                return obj[propertyName]?.ToString() ?? "Indisponível";
            }
            catch
            {
                return "Indisponível";
            }
        }

        private ulong GetSafeUInt64(ManagementObject obj, string propertyName)
        {
            try
            {
                return (ulong)(obj[propertyName] ?? 0);
            }
            catch
            {
                return 0;
            }
        }

        private string FormatBytes(ulong bytes)
        {
            string[] sizes = { "Bytes", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }

            return $"{len:0.##} {sizes[order]}";
        }
    }
}