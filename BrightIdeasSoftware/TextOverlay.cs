
// Type: BrightIdeasSoftware.TextOverlay


// Hacked by SystemAce

using System;
using System.ComponentModel;
using System.Drawing;

namespace BrightIdeasSoftware
{
  [TypeConverter("BrightIdeasSoftware.Design.OverlayConverter")]
  public class TextOverlay : TextAdornment, ITransparentOverlay, IOverlay
  {
    private int insetX = 20;
    private int insetY = 20;

    [Category("ObjectListView")]
    [NotifyParentProperty(true)]
    [Description("The horizontal inset by which the position of the overlay will be adjusted")]
    [DefaultValue(20)]
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

    [NotifyParentProperty(true)]
    [Category("ObjectListView")]
    [Description("Gets or sets the vertical inset by which the position of the overlay will be adjusted")]
    [DefaultValue(20)]
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

    [Obsolete("Use CornerRounding instead", false)]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool RoundCorneredBorder
    {
      get
      {
        return (double) this.CornerRounding > 0.0;
      }
      set
      {
        if (value)
          this.CornerRounding = 16f;
        else
          this.CornerRounding = 0.0f;
      }
    }

    public TextOverlay()
    {
      this.Alignment = ContentAlignment.BottomRight;
    }

    public virtual void Draw(ObjectListView olv, Graphics g, Rectangle r)
    {
      if (string.IsNullOrEmpty(this.Text))
        return;
      Rectangle r1 = r;
      r1.Inflate(-this.InsetX, -this.InsetY);
      this.DrawText(g, r1, this.Text, (int) byte.MaxValue);
    }
  }
}
