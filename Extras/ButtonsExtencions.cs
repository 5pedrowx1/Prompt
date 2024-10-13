using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Prompt
{
    //This shit is just to put the round buttons, it doesn't matter what's going on
    public static class ButtonExtensions
    {
        public static void CustomizeRoundedButton(this Button button, int radius = 10)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Paint += (sender, e) =>
            {
                Button btn = sender as Button;
                GraphicsPath path = new GraphicsPath();
                path.AddArc(0, 0, radius, radius, 180, 90);
                path.AddArc(btn.Width - radius, 0, radius, radius, 270, 90);
                path.AddArc(btn.Width - radius, btn.Height - radius, radius, radius, 0, 90);
                path.AddArc(0, btn.Height - radius, radius, radius, 90, 90);
                path.CloseFigure();
                btn.Region = new Region(path);

                Pen pen = new Pen(Color.Black, 2);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawPath(pen, path);
            };
        }
    }
}
