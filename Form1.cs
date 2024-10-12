using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Prompt
{
    public partial class Form1 : RoundedForm
    {
        private readonly CommandProcessor commandProcessor;
        private readonly HistoryControl historyControl;
        private readonly Settings settingsControl;
        private readonly AppSettings appSettings;
        private bool isViewingFileContent;
        private bool isEditingFileContent;
        private bool dragging;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public Form1()
        {
            InitializeComponent();
            string currentDirectory = Directory.GetCurrentDirectory();
            commandProcessor = new CommandProcessor(this, currentDirectory);
            lblCurrentDirectory.Text = currentDirectory;
            historyControl = new HistoryControl();
            settingsControl = new Settings(this) { Dock = DockStyle.Fill };
            settingsControl.ThemeChanged += SettingsControl_ThemeChanged;
            settingsControl.FontSizeChanged += SettingsControl_FontSizeChanged;

            appSettings = AppSettings.Load();
            this.Opacity = appSettings.Opacity / 100.0;

            pnlSettingsContainer.Controls.Add(settingsControl);
            ApplyTheme(this, settingsControl.IsDarkTheme);
            settingsControl.ApplyFullscreenMode();

            CustomizeButtons();
            this.MinimumSize = new Size(784, 461);
            Logger.Log("Aplicação Iniciada.");
            ShowVersionInToast();
        }

        private void ShowToast(string type, string message)
        {
            Toast toast = new Toast(type, message);
            toast.Show();
        }

        private void ShowVersionInToast()
        {
            CheckForUpdates updates = new CheckForUpdates(this);
            string version = CheckForUpdates.CurrentVersion;

            ShowToast("Info", $"Iniciando aplicativo na versão {version}");
        }

        private void CustomizeButtons()
        {
            btnClose.CustomizeRoundedButton();
            btnMinimize.CustomizeRoundedButton();
            btnMaximize.CustomizeRoundedButton();
        }

        private void TxtCommandInput_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    ProcessCommand();
                    e.SuppressKeyPress = true;
                    break;
                case Keys.Up:
                    NavigateHistoryUp();
                    e.SuppressKeyPress = true;
                    break;
                case Keys.Down:
                    NavigateHistoryDown();
                    e.SuppressKeyPress = true;
                    break;
            }
        }

        private void ProcessCommand()
        {
            string command = txtCommandInput.Text.Trim();
            if (!string.IsNullOrEmpty(command))
            {
                historyControl.AddCommand(command);
                txtCommandInput.Clear();
                commandProcessor.ProcessCommand(command);
                UpdateCurrentDirectoryLabel();
            }
        }

        private void NavigateHistoryUp() => NavigateHistory(historyControl.GetPreviousCommand());
        private void NavigateHistoryDown() => NavigateHistory(historyControl.GetNextCommand());

        private void NavigateHistory(string command)
        {
            txtCommandInput.Text = command ?? string.Empty;
            txtCommandInput.SelectionStart = txtCommandInput.Text.Length;
        }

        public void AppendColoredText(string text, Color color)
        {
            txtCommandOutput.SelectionStart = txtCommandOutput.TextLength;
            txtCommandOutput.SelectionLength = 0;
            txtCommandOutput.SelectionColor = color;
            txtCommandOutput.AppendText(text);
            txtCommandOutput.SelectionColor = txtCommandOutput.ForeColor;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.Z))
            {
                HandleCtrlZ();
                return true;
            }
            if (keyData == (Keys.Control | Keys.C))
            {
                HandleCtrlC();
                return true;
            }
            if (keyData == (Keys.Control | Keys.V))
            {
                HandleCtrlV();
                return true;
            }
            if (keyData == (Keys.Control | Keys.D))
            {
                HandleCtrlD();
                return true;
            }
            if (keyData == (Keys.Control | Keys.S))
            {
                HandleCtrlS();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void HandleCtrlZ()
        {
            if (isViewingFileContent | isEditingFileContent)
            {
                ClearScreen();
                isViewingFileContent = false;
                isEditingFileContent = false;
            }
            else if (commandProcessor.IsTaskRunning)
            {
                commandProcessor.CancelOperation();
            }
        }

        private void HandleCtrlD()
        {
            if (isViewingFileContent)
            {
                isEditingFileContent = true;
                txtCommandOutput.ReadOnly = false;
            }
        }

        private void HandleCtrlS()
        {
            if (isEditingFileContent)
            {
                string filePath = commandProcessor.GetCurrentFilePath();
                if (!string.IsNullOrEmpty(filePath))
                {
                    try
                    {
                        File.WriteAllText(filePath, txtCommandOutput.Text);
                        Logger.Log($"Alterações salvas em {filePath}.");
                        isEditingFileContent = false;
                        txtCommandOutput.ReadOnly = true;
                    }
                    catch (Exception ex)
                    {
                        Logger.Log($"Erro ao salvar o arquivo: {ex.Message}");
                    }
                }
            }
        }

        private void HandleCtrlC()
        {
            if (txtCommandOutput.SelectionLength > 0)
            {
                Clipboard.SetText(txtCommandOutput.SelectedText);
            }
        }

        private void HandleCtrlV()
        {
            txtCommandInput.Text = Clipboard.GetText();
            txtCommandInput.Focus();
        }

        public void ClearScreen()
        {
            txtCommandOutput.Clear();
            txtCommandOutput.BorderStyle = BorderStyle.None;
        }

        public void UpdateCurrentDirectoryLabel() => lblCurrentDirectory.Text = $"{commandProcessor.GetCurrentDirectory()}> ";

        public void SetViewingFileContent(bool isViewing) => isViewingFileContent = isViewing;

        private void TxtCommandOutput_KeyDown(object sender, KeyEventArgs e)
        {
            if (isEditingFileContent)
            {
                return;
            }
            txtCommandInput.Focus();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
            Logger.Log("Aplicação Encerrada.");
        }

        private void BtnMaximize_Click(object sender, EventArgs e) => WindowState = WindowState == FormWindowState.Maximized ? FormWindowState.Normal : FormWindowState.Maximized;

        private void BtnMinimize_Click(object sender, EventArgs e) => WindowState = FormWindowState.Minimized;

        public void UpdateFontSize(int newSize)
        {
            if (txtCommandOutput.Font.Size != newSize)
            {
                txtCommandOutput.Font = new Font(txtCommandOutput.Font.FontFamily, newSize);
                txtCommandOutput.Refresh();
            }
        }

        private void SettingsControl_ThemeChanged(object sender, EventArgs e)
        {
            ApplyTheme(this, settingsControl.IsDarkTheme);
            appSettings.Theme = settingsControl.IsDarkTheme ? "Escuro" : "Claro";
            appSettings.Save();
        }

        private void SettingsControl_FontSizeChanged(object sender, EventArgs e) => UpdateFontSize(settingsControl.FontSize);

        private void Settings_Click(object sender, EventArgs e)
        {
            pnlSettingsContainer.Visible = !pnlSettingsContainer.Visible;
            settingsControl.Dock = DockStyle.Fill;
        }

        public void ApplyTheme(Control control, bool isDarkTheme)
        {
            if (isDarkTheme)
                ThemeManager.ApplyDarkTheme(control);
            else
                ThemeManager.ApplyLightTheme(control);

            foreach (Control childControl in control.Controls)
                ApplyTheme(childControl, isDarkTheme);
        }

        private void PnlTitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void PnlTitleBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void PnlTitleBar_MouseUp(object sender, MouseEventArgs e) => dragging = false;

        private void Btn_MouseEnter(object sender, EventArgs e) => ChangeButtonColor(sender, Color.Gray);

        private void Btn_MouseLeave(object sender, EventArgs e) => ChangeButtonColor(sender, Color.Black);

        private void ChangeButtonColor(object sender, Color color)
        {
            if (sender is Button btn)
                btn.BackColor = color;
        }

        private new void MouseWheel(object sender, MouseEventArgs e)
        {
            int scrollAmount = e.Delta > 10 ? SB_LINEUP : SB_LINEDOWN;
            SendMessage(txtCommandOutput.Handle, WM_VSCROLL, scrollAmount, 10);
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x84;
            const int RESIZE_HANDLE_SIZE = 50;

            if (m.Msg == WM_NCHITTEST)
            {
                base.WndProc(ref m);

                Point cursorPos = PointToClient(Cursor.Position);

                if (cursorPos.Y <= RESIZE_HANDLE_SIZE)
                {
                    if (cursorPos.X <= RESIZE_HANDLE_SIZE)
                        m.Result = (IntPtr)HTTOPLEFT;
                    else if (cursorPos.X >= this.ClientSize.Width - RESIZE_HANDLE_SIZE)
                        m.Result = (IntPtr)HTTOPRIGHT;
                    else
                        m.Result = (IntPtr)HTTOP;
                }
                else if (cursorPos.Y >= this.ClientSize.Height - RESIZE_HANDLE_SIZE)
                {
                    if (cursorPos.X <= RESIZE_HANDLE_SIZE)
                        m.Result = (IntPtr)HTBOTTOMLEFT;
                    else if (cursorPos.X >= this.ClientSize.Width - RESIZE_HANDLE_SIZE)
                        m.Result = (IntPtr)HTBOTTOMRIGHT;
                    else
                        m.Result = (IntPtr)HTBOTTOM;
                }
                else if (cursorPos.X <= RESIZE_HANDLE_SIZE)
                {
                    m.Result = (IntPtr)HTLEFT;
                }
                else if (cursorPos.X >= this.ClientSize.Width - RESIZE_HANDLE_SIZE)
                {
                    m.Result = (IntPtr)HTRIGHT;
                }
                return;
            }

            base.WndProc(ref m);
        }

        private const int WM_VSCROLL = 0x0115;
        private const int SB_LINEUP = 0;
        private const int SB_LINEDOWN = 1;
        private const int HTLEFT = 10;
        private const int HTRIGHT = 11;
        private const int HTTOP = 12;
        private const int HTTOPLEFT = 13;
        private const int HTTOPRIGHT = 14;
        private const int HTBOTTOM = 15;
        private const int HTBOTTOMLEFT = 16;
        private const int HTBOTTOMRIGHT = 17;


        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForUpdates updates = new CheckForUpdates(this);
            _ = updates.CheckAndUpdate();
        }
    }
}