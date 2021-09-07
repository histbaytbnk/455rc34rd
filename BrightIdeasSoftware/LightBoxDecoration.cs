
// Type: BrightIdeasSoftware.LightBoxDecoration


// Hacked by SystemAce

using System.Drawing;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class LightBoxDecoration : BorderDecoration
  {
    public LightBoxDecoration()
    {
      this.BoundsPadding = new Size(-1, 4);
      this.CornerRounding = 8f;
      this.FillBrush = (Brush) new SolidBrush(Color.FromArgb(72, Color.Black));
    }

    public override void Draw(ObjectListView olv, Graphics g, Rectangle r)
    {
      if (!r.Contains(olv.PointToClient(Cursor.Position)))
        return;
      Rectangle rowBounds = this.RowBounds;
      if (rowBounds.IsEmpty)
      {
        if (olv.View != View.Tile)
          return;
        g.FillRectangle(this.FillBrush, r);
      }
      else
      {
        using (Region region = new Region(r))
        {
          rowBounds.Inflate(this.BoundsPadding);
          region.Exclude(this.GetRoundedRect((RectangleF) rowBounds, this.CornerRounding));
          Region clip = g.Clip;
          g.Clip = region;
          g.FillRectangle(this.FillBrush, r);
          g.Clip = clip;
        }
      }
    }
  }
}
