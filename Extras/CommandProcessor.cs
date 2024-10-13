using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Prompt
{
    public class CommandProcessor
    {
        private readonly Form1 form;
        private readonly HistoryControl historyControl;
        private string currentDirectory;
        private string filePath;
        private bool isTaskRunning = false;
        private readonly object lockObject = new object();
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly CommandDescriptions commandDescriptions = new CommandDescriptions();

        public string GetCurrentFilePath()
        {
            return filePath;
        }

        public bool IsTaskRunning
        {
            get { return isTaskRunning; }
        }

        public CommandProcessor(Form1 form, string initialDirectory)
        {
            this.form = form;
            this.currentDirectory = initialDirectory;
            historyControl = new HistoryControl();
        }

        public async void ProcessCommand(string command)
        {
            historyControl.AddCommand(command);
            string[] parts = command.Split(new[] { ' ' }, 2);
            string cmd = parts[0].ToLower();
            string arg = parts.Length > 1 ? parts[1].Trim() : "";

            switch (cmd)
            {
                case "data":
                    Data(arg);
                    break;

                case "cls":
                    form.ClearScreen();
                    break;

                case "cd":
                    ChangeDirectory(arg);
                    break;

                case "ls":
                    ListDirectory(arg);
                    break;

                case "mkdir":
                    CreateDirectory(arg);
                    break;

                case "rmdir":
                    RemoveDirectory(arg);
                    break;

                case "echo":
                    form.AppendColoredText(arg + Environment.NewLine, Color.FromArgb(128, 255, 128));
                    break;

                case "copy":
                    CopyFile(arg);
                    break;

                case "move":
                    MoveFile(arg);
                    break;

                case "del":
                    DeleteFile(arg);
                    break;

                case "ren":
                    RenameFile(arg);
                    break;

                case "ver":
                    DisplayFileContent(arg);
                    break;

                case "find":
                    FindInFile(arg);
                    break;

                case "tree":
                    await DisplayTree(arg);
                    break;

                case "attr":
                    ModifyAttributes(arg);
                    break;

                case "ping":
                    await ExecutePing(arg);
                    break;

                case "ipconfig":
                    ShowIpConfig();
                    break;

                case "lprocessos":
                    ListProcesses();
                    break;

                case "matar":
                    KillProcess(arg);
                    break;

                case "quem":
                    ShowUserName();
                    break;

                case "recente":
                    DisplayHistory();
                    break;

                case "open":
                    string newDirectory = currentDirectory;
                    if (arg == ".")
                    {
                        OpenCurrentDirectory(newDirectory);
                        Logger.Log("Windows Explorer Aberto em " + currentDirectory);
                    }
                    else
                    {
                        form.AppendColoredText(("Comando desconhecido: ") + command + Environment.NewLine, Color.Red);
                    }
                    break;

                case "help":
                    ProcessHelpCommand();
                    break;

                case "sair":
                    System.Windows.Forms.Application.Exit();
                    break;

                default:
                    form.AppendColoredText(("Comando desconhecido: ") + command + Environment.NewLine, Color.Red);
                    break;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEMTIME
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek;
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMilliseconds;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetSystemTime(ref SYSTEMTIME st);

        public void Data(string arg)
        {
            if (string.IsNullOrWhiteSpace(arg))
            {
                string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                form.AppendColoredText("Data atual: " + currentDate + Environment.NewLine, Color.FromArgb(128, 255, 128));
                return;
            }

            try
            {
                if (DateTime.TryParseExact(arg, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime newDate))
                {
                    DateTime currentTime = DateTime.UtcNow;

                    SYSTEMTIME st = new SYSTEMTIME
                    {
                        wYear = (ushort)newDate.Year,
                        wMonth = (ushort)newDate.Month,
                        wDay = (ushort)newDate.Day,
                        wHour = (ushort)currentTime.Hour,
                        wMinute = (ushort)currentTime.Minute,
                        wSecond = (ushort)currentTime.Second,
                        wMilliseconds = (ushort)currentTime.Millisecond
                    };

                    if (SetSystemTime(ref st))
                    {
                        form.AppendColoredText("Data do sistema alterada para: " + newDate.ToString("yyyy-MM-dd") + " " + currentTime.ToString("HH:mm:ss") + " UTC" + Environment.NewLine, Color.FromArgb(128, 255, 128));
                        Logger.Log("Data do sistema alterada para: " + newDate.ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        form.AppendColoredText("Erro: Falha ao alterar a data do sistema. Tente Executar como administrador." + Environment.NewLine, Color.Red);
                    }
                }
                else
                {
                    form.AppendColoredText("Erro: Formato de data inválido. Use o formato yyyy-MM-dd." + Environment.NewLine, Color.Red);
                }
            }
            catch (Exception ex)
            {
                form.AppendColoredText("Erro ao definir a data. Veja application.log" + Environment.NewLine, Color.Red);
                Logger.LogError("Erro ao definir a data: " + ex.Message);
            }
        }

        private void ChangeDirectory(string path)
        {
            string newDirectory = currentDirectory;

            if (string.IsNullOrWhiteSpace(path) || path == "~")
            {
                newDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            }
            else if (path == "..")
            {
                newDirectory = Directory.GetParent(currentDirectory)?.FullName ?? currentDirectory;
            }
            else
            {
                string combinedPath = Path.Combine(currentDirectory, path);
                if (Directory.Exists(combinedPath))
                {
                    newDirectory = Path.GetFullPath(combinedPath);
                }
                else if (Directory.Exists(path))
                {
                    newDirectory = Path.GetFullPath(path);
                }
                else
                {
                    form.AppendColoredText("O sistema não pode encontrar o caminho especificado: " + path + Environment.NewLine, Color.Red);
                }
            }
            currentDirectory = newDirectory;
        }

        private void ListDirectory(string options)
        {
            try
            {
                bool showHidden = options.Contains("-o");

                var dirs = Directory.GetDirectories(currentDirectory);
                var files = Directory.GetFiles(currentDirectory);

                if (!showHidden)
                {
                    dirs = dirs.Where(d => !new DirectoryInfo(d).Attributes.HasFlag(FileAttributes.Hidden)).ToArray();
                    files = files.Where(f => !new FileInfo(f).Attributes.HasFlag(FileAttributes.Hidden)).ToArray();
                }

                form.ClearScreen();

                form.AppendColoredText("Arquivos:" + Environment.NewLine, Color.FromArgb(128, 255, 128));
                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file);
                    string hiddenPrefix = fileInfo.Attributes.HasFlag(FileAttributes.Hidden) ? "[OCULTO] " : "";
                    Color fileColor = fileInfo.Attributes.HasFlag(FileAttributes.Hidden) ? Color.Red : Color.FromArgb(128, 255, 128);
                    form.AppendColoredText($"  {hiddenPrefix}{Path.GetFileName(file)}" + Environment.NewLine, fileColor);
                }

                form.AppendColoredText("Diretórios:" + Environment.NewLine, Color.Yellow);
                foreach (var dir in dirs)
                {
                    var dirInfo = new DirectoryInfo(dir);
                    string hiddenPrefix = dirInfo.Attributes.HasFlag(FileAttributes.Hidden) ? "[OCULTO] " : "";
                    Color dirColor = dirInfo.Attributes.HasFlag(FileAttributes.Hidden) ? Color.Red : Color.Yellow;
                    form.AppendColoredText($"  {hiddenPrefix}{Path.GetFileName(dir)}" + Environment.NewLine, dirColor);
                }
            }
            catch (Exception ex)
            {
                form.AppendColoredText("Erro ao listar diretório. Veija application.log " + Environment.NewLine, Color.Red);
                Logger.LogError("Erro ao listar diretório: " + ex.Message);
            }
        }

        private void CreateDirectory(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                string newDir = Path.Combine(currentDirectory, path);
                try
                {
                    Directory.CreateDirectory(newDir);
                    form.AppendColoredText($"Diretório criado: {newDir}{Environment.NewLine}", Color.FromArgb(128, 255, 128));
                }
                catch (Exception ex)
                {
                    form.AppendColoredText("Erro ao criar diretório. Veija application.log " + Environment.NewLine, Color.Red);
                    Logger.LogError("Erro ao criar diretório: " + ex.Message);
                }
            }
            else
            {
                form.AppendColoredText($"Sintaxe do comando: mkdir [diretório]{Environment.NewLine}", Color.Red);
            }
        }

        private void RemoveDirectory(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                string dirToRemove = Path.Combine(currentDirectory, path);
                try
                {
                    if (Directory.Exists(dirToRemove) && Directory.GetFiles(dirToRemove).Length == 0 && Directory.GetDirectories(dirToRemove).Length == 0)
                    {
                        Directory.Delete(dirToRemove);
                        form.AppendColoredText($"Diretório removido: {dirToRemove}{Environment.NewLine}", Color.FromArgb(128, 255, 128));
                    }
                    else
                    {
                        form.AppendColoredText($"O diretório não está vazio ou não existe.{Environment.NewLine}", Color.Red);
                    }
                }
                catch (Exception ex)
                {
                    form.AppendColoredText("Erro ao remover diretório. Veija application.log " + Environment.NewLine, Color.Red);
                    Logger.LogError("Erro ao remover diretório: " + ex.Message);
                }
            }
            else
            {
                form.AppendColoredText($"Sintaxe do comando: rmdir [diretório]{Environment.NewLine}", Color.Red);
            }
        }

        private void CopyFile(string args)
        {
            string[] parts = args.Split(new[] { ' ' }, 2);
            if (parts.Length < 2)
            {
                form.AppendColoredText("Sintaxe do comando: copy [origem] [destino]" + Environment.NewLine, Color.Red);
                return;
            }

            string source = Path.Combine(currentDirectory, parts[0]);
            string destination = Path.Combine(currentDirectory, parts[1]);

            try
            {
                File.Copy(source, destination, true);
                form.AppendColoredText($"Arquivo copiado para {destination}{Environment.NewLine}", Color.FromArgb(128, 255, 128));
            }
            catch (Exception ex)
            {
                form.AppendColoredText("Erro ao copiar arquivo. Veija application.log" + Environment.NewLine, Color.Red);
                Logger.LogError("Erro ao copiar arquivo: " + ex.Message);
            }
        }

        private void MoveFile(string args)
        {
            string[] parts = args.Split(new[] { ' ' }, 2);

            if (parts.Length < 2)
            {
                form.AppendColoredText("Uso: move [origem] [destino]\n", Color.Red);
                return;
            }

            string sourcePath = parts[0].Trim();
            string destinationPath = parts[1].Trim();

            try
            {
                System.IO.File.Move(sourcePath, destinationPath);
                form.AppendColoredText($"Arquivo movido de {sourcePath} para {destinationPath}\n", Color.Green);
            }
            catch (System.IO.FileNotFoundException)
            {
                form.AppendColoredText($"Arquivo não encontrado: {sourcePath}\n", Color.Red);
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                form.AppendColoredText($"Diretório não encontrado: {System.IO.Path.GetDirectoryName(destinationPath)}\n", Color.Red);
            }
            catch (System.IO.IOException ex)
            {
                form.AppendColoredText($"Erro ao mover o arquivo: {ex.Message}\n", Color.Red);
            }
            catch (Exception ex)
            {
                Logger.LogError("Erro Inesperado: " + ex.Message);
            }
        }

        private void DeleteFile(string arg)
        {
            if (string.IsNullOrWhiteSpace(arg))
            {
                form.AppendColoredText("Sintaxe do comando incorreta. Uso: del -temp para limpar temporários, del -log para apagar o log ou del C:\\Seu\\caminho." + Environment.NewLine, Color.Red);
                return;
            }

            try
            {
                if (arg.Equals("-temp", StringComparison.OrdinalIgnoreCase))
                {
                    string tempUser = Path.GetTempPath();
                    string tempSystem = @"C:\Windows\Temp";

                    form.AppendColoredText("Limpando arquivos temporários do usuário..." + Environment.NewLine, Color.FromArgb(128, 255, 128));
                    ApagarDiretorio(tempUser, out int sucessoUser, out int falhaUser);
                    form.AppendColoredText("Limpando arquivos temporários do sistema..." + Environment.NewLine, Color.FromArgb(128, 255, 128));
                    ApagarDiretorio(tempSystem, out int sucessoSystem, out int falhaSystem);

                    form.AppendColoredText($"Arquivos temporários apagados: Sucesso {sucessoUser + sucessoSystem}, Sem Sucesso {falhaUser + falhaSystem}" + Environment.NewLine, Color.FromArgb(128, 255, 128));
                }
                else if (arg.Equals("-log", StringComparison.OrdinalIgnoreCase))
                {
                    string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "application.log");

                    if (File.Exists(logFilePath))
                    {
                        File.Delete(logFilePath);
                        form.AppendColoredText("Arquivo de log 'application.log' foi deletado com sucesso." + Environment.NewLine, Color.FromArgb(128, 255, 128));
                    }
                    else
                    {
                        form.AppendColoredText("O arquivo de log 'application.log' não foi encontrado." + Environment.NewLine, Color.Red);
                    }
                }
                else
                {
                    if (File.Exists(arg))
                    {
                        File.Delete(arg);
                        form.AppendColoredText($"Arquivo '{arg}' foi apagado." + Environment.NewLine, Color.FromArgb(128, 255, 128));
                    }
                    else if (Directory.Exists(arg))
                    {
                        ApagarDiretorio(arg, out int sucesso, out int falha);
                        form.AppendColoredText($"Pasta '{arg}' foi apagada: Sucesso {sucesso}, Sem Sucesso {falha}." + Environment.NewLine, Color.FromArgb(128, 255, 128));
                    }
                    else
                    {
                        form.AppendColoredText($"Erro: O caminho '{arg}' não existe." + Environment.NewLine, Color.Red);
                    }
                }
            }
            catch (Exception ex)
            {
                form.AppendColoredText("Erro ao apagar arquivos ou pastas. Veja application.log " + Environment.NewLine, Color.Red);
                Logger.LogError("Erro ao apagar arquivos/pastas: " + ex.Message);
            }
        }

        private void ApagarDiretorio(string diretorio, out int arquivosSucesso, out int arquivosFalha)
        {
            arquivosSucesso = 0;
            arquivosFalha = 0;

            try
            {
                if (Directory.Exists(diretorio))
                {
                    DirectoryInfo di = new DirectoryInfo(diretorio);

                    foreach (FileInfo file in di.GetFiles())
                    {
                        try
                        {
                            file.Delete();
                            arquivosSucesso++;
                            form.AppendColoredText($"Arquivo deletado: {file.FullName}" + Environment.NewLine, Color.FromArgb(128, 255, 128));
                        }
                        catch (IOException ex)
                        {
                            arquivosFalha++;
                            Logger.LogError($"Erro ao deletar arquivo {file.FullName}: {ex.Message}");
                        }
                        catch (UnauthorizedAccessException ex)
                        {
                            arquivosFalha++;
                            Logger.LogError($"Acesso negado ao tentar deletar {file.FullName}: {ex.Message}");
                        }
                        catch (Exception ex)
                        {
                            arquivosFalha++;
                            form.AppendColoredText($"Erro ao deletar arquivo {file.FullName}. Veja application.log" + Environment.NewLine, Color.Red);
                            Logger.LogError("Erro ao deletar arquivo " + file.FullName + ": " + ex.Message);
                        }
                    }

                    foreach (DirectoryInfo dir in di.GetDirectories())
                    {
                        try
                        {
                            dir.Delete(true);
                            arquivosSucesso++;
                            form.AppendColoredText($"Pasta deletada: {dir.FullName}" + Environment.NewLine, Color.FromArgb(128, 255, 128));
                        }
                        catch (IOException ex)
                        {
                            arquivosFalha++;
                            Logger.LogError($"Erro ao deletar pasta {dir.FullName}: {ex.Message}");
                        }
                        catch (UnauthorizedAccessException ex)
                        {
                            arquivosFalha++;
                            Logger.LogError($"Acesso negado ao tentar deletar {dir.FullName}: {ex.Message}");
                        }
                        catch (Exception ex)
                        {
                            arquivosFalha++;
                            form.AppendColoredText($"Erro ao deletar pasta {dir.FullName}. Veja application.log" + Environment.NewLine, Color.Red);
                            Logger.LogError("Erro ao deletar pasta " + dir.FullName + ": " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                form.AppendColoredText($"Erro ao apagar diretório {diretorio}. Veja application.log" + Environment.NewLine, Color.Red);
                Logger.LogError("Erro ao apagar diretório: " + ex.Message);
            }
        }

        private void RenameFile(string args)
        {
            string[] parts = args.Split(new[] { ' ' }, 2);
            if (parts.Length < 2)
            {
                form.AppendColoredText("Sintaxe do comando: ren [arquivo] [novo_nome]" + Environment.NewLine, Color.Red);
                return;
            }

            string source = Path.Combine(currentDirectory, parts[0]);
            string destination = Path.Combine(currentDirectory, parts[1]);

            try
            {
                File.Move(source, destination);
                form.AppendColoredText($"Arquivo renomeado para {destination}{Environment.NewLine}", Color.FromArgb(128, 255, 128));
            }
            catch (Exception ex)
            {
                form.AppendColoredText("Erro ao renomear arquivo. Veija application.log" + Environment.NewLine, Color.Red);
                Logger.LogError("Erro ao deletar arquivo: " + ex.Message);
            }
        }

        private void DisplayFileContent(string fileName)
        {
            filePath = Path.Combine(currentDirectory, fileName);
            try
            {
                if (File.Exists(filePath))
                {
                    string content = File.ReadAllText(filePath);
                    form.ClearScreen();
               
                    form.AppendColoredText(content + Environment.NewLine, Color.FromArgb(128, 255, 128));
                    form.SetViewingFileContent(true);
                }
                else
                {
                    form.AppendColoredText("Arquivo não encontrado: " + fileName + Environment.NewLine, Color.Red);
                }
            }
            catch (Exception ex)
            {
                form.AppendColoredText("Erro ao ler arquivo. Veija application.log" + Environment.NewLine, Color.Red);
                Logger.LogError("Erro ao ler arquivo: " + ex.Message);
            }
        }

        private void FindInFile(string args)
        {
            string[] parts = args.Split(new[] { ' ' }, 2);
            if (parts.Length < 2)
            {
                form.AppendColoredText("Sintaxe do comando: find [termo] [arquivo]" + Environment.NewLine, Color.Red);
                return;
            }

            string term = parts[0];
            string filePath = Path.Combine(currentDirectory, parts[1]);

            try
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    if (line.Contains(term))
                    {
                        form.AppendColoredText(line + Environment.NewLine, Color.FromArgb(128, 255, 128));
                    }
                }
            }
            catch (Exception ex)
            {
                form.AppendColoredText("Erro ao procurar no arquivo. Veija application.log" + Environment.NewLine, Color.Red);
                Logger.LogError("Erro ao procurar no arquivo: " + ex.Message);
            }
        }

        private async Task DisplayTree(string path)
        {
            if (isTaskRunning)
            {
                form.AppendColoredText("Uma instância de outro comando já está em execução." + Environment.NewLine, Color.Red);
                return;
            }

            string directory = string.IsNullOrEmpty(path) ? currentDirectory : Path.Combine(currentDirectory, path);

            if (Directory.Exists(directory))
            {
                form.ClearScreen();

                using var cts = new CancellationTokenSource();
                CancellationTokenSource previousCts;

                lock (lockObject)
                {
                    previousCts = cancellationTokenSource;
                    cancellationTokenSource = cts;
                }

                try
                {
                    isTaskRunning = true;
                    await DisplayTreeRecursiveAsync(directory, "", cts.Token);
                }
                catch (OperationCanceledException)
                {
                    form.AppendColoredText("[Operação Cancelada]" + Environment.NewLine, Color.Red);
                }
                finally
                {
                    lock (lockObject)
                    {
                        if (previousCts != null && previousCts != cts)
                        {
                            previousCts.Dispose();
                        }
                    }

                    isTaskRunning = false;
                }
            }
            else
            {
                form.AppendColoredText("O sistema não pode encontrar o caminho especificado: " + directory + Environment.NewLine, Color.Red);
            }
        }

        private async Task DisplayTreeRecursiveAsync(string path, string indent, CancellationToken cancellationToken)
        {
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(path);

                form.AppendColoredText(indent + Path.GetFileName(path) + Environment.NewLine, Color.Yellow);

                FileInfo[] files = dirInfo.GetFiles();
                foreach (FileInfo file in files)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    form.AppendColoredText(indent + "  " + file.Name + Environment.NewLine, Color.FromArgb(128, 255, 128));
                    await Task.Delay(1, cancellationToken);
                }

                DirectoryInfo[] subDirs = dirInfo.GetDirectories();
                foreach (DirectoryInfo subDir in subDirs)
                {
                    await DisplayTreeRecursiveAsync(subDir.FullName, indent + "  ", cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                form.AppendColoredText(indent + "[Operação Cancelada]" + Environment.NewLine, Color.Red);
            }
            catch (UnauthorizedAccessException)
            {
                form.AppendColoredText(indent + "[Acesso Negado] " + Path.GetFileName(path) + Environment.NewLine, Color.Red);
            }
            catch (Exception ex)
            {
                form.AppendColoredText(indent + "[Erro] " + ex.Message + Environment.NewLine, Color.Red);
            }
        }

        private void ModifyAttributes(string args)
        {
            string[] parts = args.Split(new[] { ' ' }, 2);
            if (parts.Length < 2)
            {
                form.AppendColoredText("Sintaxe do comando: attr [+/-O] [arquivo/diretório]" + Environment.NewLine, Color.Red);
                return;
            }

            string flag = parts[0];
            string path = Path.Combine(currentDirectory, parts[1]);

            try
            {
                FileAttributes attributes = File.GetAttributes(path);

                if (flag == "+O")
                {
                    File.SetAttributes(path, attributes | FileAttributes.Hidden);
                    form.AppendColoredText("Atributo oculto adicionado." + Environment.NewLine, Color.FromArgb(128, 255, 128));
                }
                else if (flag == "-O")
                {
                    File.SetAttributes(path, attributes & ~FileAttributes.Hidden);
                    form.AppendColoredText("Atributo oculto removido." + Environment.NewLine, Color.FromArgb(128, 255, 128));
                }
                else
                {
                    form.AppendColoredText("Flag desconhecida: " + flag + Environment.NewLine, Color.Red);
                }
            }
            catch (Exception ex)
            {
                form.AppendColoredText("Erro ao modificar atributos: " + ex.Message + Environment.NewLine, Color.Red);
            }
        }

        private async Task ExecutePing(string options)
        {
            if (isTaskRunning)
            {
                form.AppendColoredText("Uma instância de outro comando já está em execução." + Environment.NewLine, Color.Red);
                return;
            }

            bool continuousPing = options.Contains("-t");
            bool resolveAddressToName = options.Contains("-a");
            int echoCount = 4;
            int bufferSize = 32;
            int ttl = 128;
            int timeout = 4000;
            bool dontFragment = options.Contains("-f");
            string targetName = string.Empty;

            var matchEchoCount = System.Text.RegularExpressions.Regex.Match(options, @"-n (\d+)");
            if (matchEchoCount.Success)
            {
                echoCount = int.Parse(matchEchoCount.Groups[1].Value);
            }

            var matchBufferSize = System.Text.RegularExpressions.Regex.Match(options, @"-l (\d+)");
            if (matchBufferSize.Success)
            {
                bufferSize = int.Parse(matchBufferSize.Groups[1].Value);
            }

            var matchTTL = System.Text.RegularExpressions.Regex.Match(options, @"-i (\d+)");
            if (matchTTL.Success)
            {
                ttl = int.Parse(matchTTL.Groups[1].Value);
            }

            var matchTimeout = System.Text.RegularExpressions.Regex.Match(options, @"-w (\d+)");
            if (matchTimeout.Success)
            {
                timeout = int.Parse(matchTimeout.Groups[1].Value);
            }

            string[] optionParts = options.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            targetName = optionParts.LastOrDefault(part => !part.StartsWith("-"));

            if (string.IsNullOrEmpty(targetName))
            {
                form.AppendColoredText("Erro: Nome do alvo não especificado." + Environment.NewLine, Color.Red);
                return;
            }

            var pingOptions = new PingOptions
            {
                Ttl = ttl,
                DontFragment = dontFragment
            };

            byte[] buffer = new byte[bufferSize];
            new Random().NextBytes(buffer);

            int sent = 0, received = 0, lost = 0;
            long minTime = long.MaxValue, maxTime = 0, totalTime = 0;

            using var cts = new CancellationTokenSource();
            CancellationTokenSource previousCts;

            lock (lockObject)
            {
                previousCts = cancellationTokenSource;
                cancellationTokenSource = cts;
            }

            try
            {
                isTaskRunning = true;

                using (Ping pingSender = new Ping())
                {
                    PingReply reply;
                    int pingCount = continuousPing ? int.MaxValue : echoCount;

                    for (int i = 0; i < pingCount; i++)
                    {
                        if (cts.Token.IsCancellationRequested)
                        {
                            form.AppendColoredText("[Operação Cancelada]" + Environment.NewLine, Color.Red);
                            return;
                        }

                        try
                        {
                            reply = await pingSender.SendPingAsync(targetName, timeout, buffer, pingOptions);
                            sent++;

                            if (reply.Status == IPStatus.Success)
                            {
                                received++;
                                long roundtripTime = reply.RoundtripTime;
                                minTime = Math.Min(minTime, roundtripTime);
                                maxTime = Math.Max(maxTime, roundtripTime);
                                totalTime += roundtripTime;

                                string resolvedAddress = resolveAddressToName ? $" ({reply.Address})" : "";
                                form.AppendColoredText($"Resposta de {reply.Address}{resolvedAddress}: bytes={reply.Buffer.Length} tempo={reply.RoundtripTime}ms TTL={reply.Options.Ttl}" + Environment.NewLine, Color.FromArgb(128, 255, 128));
                            }
                            else
                            {
                                lost++;
                                form.AppendColoredText($"Falha ao pingar {targetName}: {reply.Status}" + Environment.NewLine, Color.Red);
                            }
                        }
                        catch (PingException ex)
                        {
                            form.AppendColoredText($"Erro ao pingar. Veija application.log" + Environment.NewLine, Color.Red);
                            Logger.LogError("Erro ao Pingar: " + ex.Message);
                        }

                        if (!continuousPing && i >= echoCount - 1)
                        {
                            break;
                        }
                    }
                }

                if (sent > 0)
                {
                    long avgTime = received > 0 ? totalTime / received : 0;
                    form.AppendColoredText($"Estatísticas de Ping para {targetName}:" + Environment.NewLine, Color.Yellow);
                    form.AppendColoredText($"    Pacotes: Enviados = {sent}, Recebidos = {received}, Perdidos = {lost} ({(lost * 100) / sent}% perda)" + Environment.NewLine, Color.Yellow);
                    form.AppendColoredText("Tempo aproximado de ida e volta em milissegundos:" + Environment.NewLine, Color.Yellow);
                    form.AppendColoredText($"    Mínimo = {minTime}ms, Máximo = {maxTime}ms, Média = {avgTime}ms" + Environment.NewLine, Color.Yellow);
                }
                else
                {
                    form.AppendColoredText("Nenhum pacote foi enviado." + Environment.NewLine, Color.Red);
                }
            }
            catch (Exception ex)
            {
                form.AppendColoredText($"Erro ao executar o ping. Veija application.log" + Environment.NewLine, Color.Red);
                Logger.LogError("Erro ao executar o ping: " + ex.Message);
            }
            finally
            {
                lock (lockObject)
                {
                    if (previousCts != null && previousCts != cts)
                    {
                        previousCts.Dispose();
                    }
                }

                isTaskRunning = false;
            }
        }

        private void ShowIpConfig()
        {
            try
            {
                var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

                form.AppendColoredText("Configuração de IP:" + Environment.NewLine, Color.Yellow);

                foreach (var networkInterface in networkInterfaces)
                {
                    if (networkInterface.OperationalStatus == OperationalStatus.Up)
                    {
                        DisplayNetworkInterfaceInfo(networkInterface);
                    }
                    else
                    {
                        form.AppendColoredText($"Interface: {networkInterface.Description} (Inativa)" + Environment.NewLine, Color.Red);
                    }
                }
            }
            catch (NetworkInformationException netEx)
            {
                form.AppendColoredText("Erro ao obter configurações de IP. Veja application.log" + Environment.NewLine, Color.Red);
                Logger.LogError("Erro de informações de rede: " + netEx.Message);
            }
            catch (Exception ex)
            {
                form.AppendColoredText("Erro inesperado ao obter configurações de IP. Veja application.log" + Environment.NewLine, Color.Red);
                Logger.LogError("Erro inesperado ao obter configurações de IP: " + ex.Message);
            }
        }

        private void DisplayNetworkInterfaceInfo(NetworkInterface networkInterface)
        {
            var ipProps = networkInterface.GetIPProperties();
            var ipAddresses = ipProps.UnicastAddresses.Where(a => a.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
            var gatewayAddresses = ipProps.GatewayAddresses;

            form.AppendColoredText($"Interface: {networkInterface.Description}" + Environment.NewLine, Color.Cyan);

            foreach (var ipAddress in ipAddresses)
            {
                form.AppendColoredText($"  Endereço IP: {ipAddress.Address}" + Environment.NewLine, Color.FromArgb(128, 255, 128));
                form.AppendColoredText($"  Máscara de Sub-rede: {ipAddress.IPv4Mask}" + Environment.NewLine, Color.FromArgb(128, 255, 128));
            }

            foreach (var gateway in gatewayAddresses)
            {
                form.AppendColoredText($"  Gateway Padrão: {gateway.Address}" + Environment.NewLine, Color.FromArgb(128, 255, 128));
            }

            form.AppendColoredText(Environment.NewLine, Color.FromArgb(128, 255, 128));
        }

        private void ListProcesses()
        {
            string header = "Nome do Processo\tPID\tUso de Memória (MB)\tTítulo da Janela";
            form.AppendColoredText(header + Environment.NewLine, Color.Yellow);
            form.AppendColoredText(new string('-', header.Length) + Environment.NewLine, Color.Yellow);

            Process[] processList = Process.GetProcesses();

            foreach (Process process in processList)
            {
                try
                {
                    string processDetails = $"{process.ProcessName,-24}\t{process.Id,-8}\t{FormatMemoryUsage(process.WorkingSet64),-15}\t{TruncateString(process.MainWindowTitle, 40)}";
                    form.AppendColoredText(processDetails + Environment.NewLine, Color.White);
                }
                catch (Exception ex)
                {
                    form.AppendColoredText($"Erro ao obter informações do processo {process.ProcessName}: {ex.Message}" + Environment.NewLine, Color.Red);
                    Logger.LogError("Erro ao obter informações do processo " + process.ProcessName + ":" + ex.Message);
                }
            }
        }

        private void KillProcess(string processNameOrPID)
        {
            if (string.IsNullOrWhiteSpace(processNameOrPID))
            {
                form.AppendColoredText("Sintaxe do comando incorreta. Uso: matar <nome_do_processo | PID>" + Environment.NewLine, Color.Red);
                return;
            }

            try
            {
                if (int.TryParse(processNameOrPID, out int pid))
                {
                    Process process = Process.GetProcessById(pid);
                    process.Kill();
                    form.AppendColoredText($"Processo (PID: {pid}) foi terminado." + Environment.NewLine, Color.FromArgb(128, 255, 128));
                    Logger.Log("Processo PID: " + pid + " foi terminado.");
                }
                else
                {
                    Process[] processesByName = Process.GetProcessesByName(processNameOrPID);

                    if (processesByName.Length > 0)
                    {
                        foreach (Process process in processesByName)
                        {
                            process.Kill();
                            form.AppendColoredText($"Processo {process.ProcessName} (PID: {process.Id}) foi terminado." + Environment.NewLine, Color.FromArgb(128, 255, 128));
                            Logger.Log("Processo " + process.ProcessName + " foi terminado.");
                        }
                    }
                    else
                    {
                        form.AppendColoredText("Processo não encontrado: " + processNameOrPID + Environment.NewLine, Color.Red);
                    }
                }
            }
            catch (Exception ex)
            {
                form.AppendColoredText("Erro ao tentar terminar o processo. Veija application.log" + Environment.NewLine, Color.Red);
                Logger.LogError("Erro ao tentar terminar o processo: " + ex.Message);
            }
        }

        private string TruncateString(string value, int maxLength)
        {
            if (value.Length > maxLength)
            {
                return value.Substring(0, maxLength - 3) + "...";
            }
            return value.PadRight(maxLength);
        }

        private string FormatMemoryUsage(long memoryInBytes)
        {
            double memoryInMB = memoryInBytes / (1024.0 * 1024.0);
            return memoryInMB.ToString("N2") + " MB";
        }


        private void OpenCurrentDirectory(string currentDirectory)
        {
            if (System.IO.Directory.Exists(currentDirectory))
            {
                Process.Start("explorer.exe", currentDirectory);
            }
            else
            {
                Console.WriteLine("Diretório não encontrado: " + currentDirectory);
            }
        }

        private void ProcessHelpCommand()
        {
            form.ClearScreen();
            form.AppendColoredText("Lista de Comandos Disponíveis:" + Environment.NewLine + Environment.NewLine, Color.Cyan);

            foreach (var command in commandDescriptions.GetAllCommands())
            {
                var description = commandDescriptions.GetDescription(command);

                form.AppendColoredText($"Comando: {command}" + Environment.NewLine, Color.Yellow);
                form.AppendColoredText($"{description}" + Environment.NewLine + Environment.NewLine, Color.LightGreen);
            }
        }

        public void CancelOperation()
        {
            if (isTaskRunning)
            {
                CancellationTokenSource cts;

                lock (lockObject)
                {
                    cts = cancellationTokenSource;
                }

                cts?.Cancel();
            }
            else
            {
                form.AppendColoredText("[Nenhuma tarefa em execução]" + Environment.NewLine, Color.Red);
            }
        }

        public string GetCurrentDirectory()
        {
            return currentDirectory;
        }

        private void ShowUserName()
        {
            string userName = Environment.UserName;
            form.AppendColoredText($"Usuário: {userName}{Environment.NewLine}", Color.FromArgb(128, 255, 128));
        }

        private void DisplayHistory()
        {
            form.AppendColoredText("Histórico de Comandos:" + Environment.NewLine, Color.Yellow);

            if (historyControl.CommandCount == 0)
            {
                form.AppendColoredText("Nenhum comando foi executado ainda." + Environment.NewLine, Color.Gray);
            }
            else
            {
                for (int i = 0; i < historyControl.CommandCount; i++)
                {
                    string cmd = historyControl.GetCommandFromHistory(i);
                    form.AppendColoredText(cmd + Environment.NewLine, Color.White);
                }
            }
        }
    }
}