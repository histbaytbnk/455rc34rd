
// Type: BrightIdeasSoftware.BillboardOverlay


// Hacked by SystemAce

using System;
using System.Drawing;

namespace BrightIdeasSoftware
{
  public class BillboardOverlay : TextOverlay
  {
    private Point location;

    public Point Location
    {
      get
      {
        return this.location;
      }
      set
      {
        this.location = value;
      }
    }

    public BillboardOverlay()
    {
      this.Transparency = (int) byte.MaxValue;
      this.BackColor = Color.PeachPuff;
      this.TextColor = Color.Black;
      this.BorderColor = Color.Empty;
      this.Font = new Font("Tahoma", 10f);
    }

    public override void Draw(ObjectListView olv, Graphics g, Rectangle r)
    {
      if (string.IsNullOrEmpty(this.Text))
        return;
      Rectangle textRect = this.CalculateTextBounds(g, r, this.Text);
      textRect.Location = this.Location;
      if (textRect.Right > r.Width)
        textRect.X = Math.Max(r.Left, r.Width - textRect.Width);
      if (textRect.Bottom > r.Height)
        textRect.Y = Math.Max(r.Top, r.Height - textRect.Height);
      this.DrawBorderedText(g, textRect, this.Text, (int) byte.MaxValue);
    }
  }
}
