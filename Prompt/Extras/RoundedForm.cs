using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class RoundedForm : Form
{
    // Field to hold the radius for the rounded corners.
    private readonly int borderRadius;
    public RoundedForm() : this(25) { }

    // Constructor that allows specifying a custom border radius.
    public RoundedForm(int borderRadius)
    {
        this.borderRadius = borderRadius; // Sets the border radius to the provided value.
        this.FormBorderStyle = FormBorderStyle.None; // Removes the default window border.

    }

    private void UpdateRegion()
    {
        // Creates a new GraphicsPath to define the shape of the form.
        using GraphicsPath path = new GraphicsPath();
        path.StartFigure(); // Starts defining the shape.

        // Adds arcs to the path to create rounded corners.
        // Top-left corner.
        path.AddArc(0, 0, borderRadius, borderRadius, 180, 90);
        // Top-right corner.
        path.AddArc(this.Width - borderRadius, 0, borderRadius, borderRadius, 270, 90);
        // Bottom-right corner.
        path.AddArc(this.Width - borderRadius, this.Height - borderRadius, borderRadius, borderRadius, 0, 90);
        // Bottom-left corner.
        path.AddArc(0, this.Height - borderRadius, borderRadius, borderRadius, 90, 90);

        path.CloseFigure(); // Completes the shape by closing the path.

        // Sets the form's region to the newly created rounded shape.
        this.Region = new Region(path);
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        UpdateRegion(); // Reapplies the rounded corners after resizing.
    }

    // Overrides the OnPaint method to customize the form's drawing behavior.
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias; // Enables anti-aliasing for smoother edges.

        // Fills the form's background with its current BackColor.
        using SolidBrush brush = new SolidBrush(this.BackColor);
        e.Graphics.FillRectangle(brush, this.ClientRectangle); // Fills the entire client area.
    }
}
