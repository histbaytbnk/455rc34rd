
// Type: BrightIdeasSoftware.ImageOverlay


// Hacked by SystemAce

using System;
using System.ComponentModel;
using System.Drawing;

namespace BrightIdeasSoftware
{
  [TypeConverter("BrightIdeasSoftware.Design.OverlayConverter")]
  public class ImageOverlay : ImageAdornment, ITransparentOverlay, IOverlay
  {
    private int insetX = 20;
    private int insetY = 20;

    [Category("ObjectListView")]
    [DefaultValue(20)]
    [NotifyParentProperty(true)]
    [Description("The horizontal inset by which the position of the overlay will be adjusted")]
    public int InsetX
    {
      get
      {
        return this.insetX;
      }
      set
      {
        this.insetX = Math.Max(0, value);
      }
    }

    [DefaultValue(20)]
    [NotifyParentProperty(true)]
    [Category("ObjectListView")]
    [Description("Gets or sets the vertical inset by which the position of the overlay will be adjusted")]
    public int InsetY
    {
      get
      {
        return this.insetY;
      }
      set
      {
        this.insetY = Math.Max(0, value);
      }
    }

    public ImageOverlay()
    {
      this.Alignment = ContentAlignment.BottomRight;
    }

    public virtual void Draw(ObjectListView olv, Graphics g, Rectangle r)
    {
      Rectangle r1 = r;
      r1.Inflate(-this.InsetX, -this.InsetY);
      this.DrawImage(g, r1, this.Image, (int) byte.MaxValue);
    }
  }
}
