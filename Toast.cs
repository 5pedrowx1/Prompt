using System;
using System.Drawing;
using System.Windows.Forms;

namespace Prompt
{
    public partial class Toast : Form
    {
        int toastX, toastY;
        const int startY = 100;
        const int fadeDuration = 1000;
        const int displayDuration = 2000;
        Timer fadeInTimer;
        Timer fadeOutTimer;

        public Toast(string type, string message)
        {
            InitializeComponent();
            lbType.Text = type;
            lbMessage.Text = message;

            switch (type)
            {
                case "Success":
                    toastBorder.BackColor = Color.Green;
                    picIcon.Image = Properties.Resources.success;
                    break;
                case "Error":
                    toastBorder.BackColor = Color.Red;
                    picIcon.Image = Properties.Resources.error;
                    break;
                case "Info":
                    toastBorder.BackColor = Color.Blue;
                    picIcon.Image = Properties.Resources.info;
                    break;
                case "Warning":
                    toastBorder.BackColor = Color.Yellow;
                    picIcon.Image = Properties.Resources.warning;
                    break;
            }

            fadeInTimer = new Timer();
            fadeInTimer.Interval = fadeDuration / 20;
            fadeInTimer.Tick += FadeIn;

            fadeOutTimer = new Timer();
            fadeOutTimer.Interval = fadeDuration / 20;
            fadeOutTimer.Tick += FadeOut;
        }

        private void Toast_Load(object sender, EventArgs e)
        {
            Position();
            this.Opacity = 0;
            fadeInTimer.Start();
        }

        private void FadeIn(object sender, EventArgs e)
        {
            if (this.Opacity < 1)
            {
                this.Opacity += 0.05;
            }
            else
            {
                fadeInTimer.Stop();
                Timer displayTimer = new Timer();
                displayTimer.Interval = displayDuration;
                displayTimer.Tick += (s, args) =>
                {
                    displayTimer.Stop();
                    fadeOutTimer.Start();
                };
                displayTimer.Start();
            }
        }

        private void FadeOut(object sender, EventArgs e)
        {
            if (this.Opacity > 0)
            {
                this.Opacity -= 0.05;
                toastY -= 1;
                this.Location = new Point(toastX, toastY);
            }
            else
            {
                fadeOutTimer.Stop();
                this.Close();
            }
        }

        private void Position()
        {
            int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int screenHeight = Screen.PrimaryScreen.WorkingArea.Height;

            toastX = screenWidth - this.Width;
            toastY = screenHeight - this.Height;

            this.Location = new Point(toastX, toastY);
        }
    }
}
