
// Type: BrightIdeasSoftware.GraphicAdornment


// Hacked by SystemAce

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class GraphicAdornment
  {
    private ContentAlignment adornmentCorner = ContentAlignment.MiddleCenter;
    private ContentAlignment alignment = ContentAlignment.BottomRight;
    private Size offset = new Size();
    private ContentAlignment referenceCorner = ContentAlignment.MiddleCenter;
    private int transparency = 128;
    private int rotation;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public ContentAlignment AdornmentCorner
    {
      get
      {
        return this.adornmentCorner;
      }
      set
      {
        this.adornmentCorner = value;
      }
    }

    [Category("ObjectListView")]
    [NotifyParentProperty(true)]
    [Description("How will the adornment be aligned")]
    [DefaultValue(ContentAlignment.BottomRight)]
    public ContentAlignment Alignment
    {
      get
      {
        return this.alignment;
      }
      set
      {
        this.alignment = value;
        this.ReferenceCorner = value;
        this.AdornmentCorner = value;
      }
    }

    [Category("ObjectListView")]
    [DefaultValue(typeof (Size), "0,0")]
    [Description("The offset by which the position of the adornment will be adjusted")]
    public Size Offset
    {
      get
      {
        return this.offset;
      }
      set
      {
        this.offset = value;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public ContentAlignment ReferenceCorner
    {
      get
      {
        return this.referenceCorner;
      }
      set
      {
        this.referenceCorner = value;
      }
    }

    [Description("The degree of rotation that will be applied to the adornment.")]
    [DefaultValue(0)]
    [NotifyParentProperty(true)]
    [Category("ObjectListView")]
    public int Rotation
    {
      get
      {
        return this.rotation;
      }
      set
      {
        this.rotation = value;
      }
    }

    [DefaultValue(128)]
    [Category("ObjectListView")]
    [Description("The transparency of this adornment. 0 is completely transparent, 255 is completely opaque.")]
    public int Transparency
    {
      get
      {
        return this.transparency;
      }
      set
      {
        this.transparency = Math.Min((int) byte.MaxValue, Math.Max(0, value));
      }
    }

    public virtual Point CalculateAlignedPosition(Point pt, Size size, ContentAlignment corner)
    {
      switch (corner)
      {
        case ContentAlignment.BottomCenter:
          return new Point(pt.X - size.Width / 2, pt.Y - size.Height);
        case ContentAlignment.BottomRight:
          return new Point(pt.X - size.Width, pt.Y - size.Height);
        case ContentAlignment.MiddleRight:
          return new Point(pt.X - size.Width, pt.Y - size.Height / 2);
        case ContentAlignment.BottomLeft:
          return new Point(pt.X, pt.Y - size.Height);
        case ContentAlignment.TopLeft:
          return pt;
        case ContentAlignment.TopCenter:
          return new Point(pt.X - size.Width / 2, pt.Y);
        case ContentAlignment.TopRight:
          return new Point(pt.X - size.Width, pt.Y);
        case ContentAlignment.MiddleLeft:
          return new Point(pt.X, pt.Y - size.Height / 2);
        case ContentAlignment.MiddleCenter:
          return new Point(pt.X - size.Width / 2, pt.Y - size.Height / 2);
        default:
          return pt;
      }
    }

    public virtual Rectangle CreateAlignedRectangle(Rectangle r, Size sz)
    {
      return this.CreateAlignedRectangle(r, sz, this.ReferenceCorner, this.AdornmentCorner, this.Offset);
    }

    public virtual Rectangle CreateAlignedRectangle(Rectangle r, Size sz, ContentAlignment corner, ContentAlignment referenceCorner, Size offset)
    {
      return new Rectangle(this.CalculateAlignedPosition(this.CalculateCorner(r, referenceCorner), sz, corner) + offset, sz);
    }

    public virtual Point CalculateCorner(Rectangle r, ContentAlignment corner)
    {
      switch (corner)
      {
        case ContentAlignment.BottomCenter:
          return new Point(r.X + r.Width / 2, r.Bottom);
        case ContentAlignment.BottomRight:
          return new Point(r.Right, r.Bottom);
        case ContentAlignment.MiddleRight:
          return new Point(r.Right, r.Top + r.Height / 2);
        case ContentAlignment.BottomLeft:
          return new Point(r.Left, r.Bottom);
        case ContentAlignment.TopLeft:
          return new Point(r.Left, r.Top);
        case ContentAlignment.TopCenter:
          return new Point(r.X + r.Width / 2, r.Top);
        case ContentAlignment.TopRight:
          return new Point(r.Right, r.Top);
        case ContentAlignment.MiddleLeft:
          return new Point(r.Left, r.Top + r.Height / 2);
        case ContentAlignment.MiddleCenter:
          return new Point(r.X + r.Width / 2, r.Top + r.Height / 2);
        default:
          return r.Location;
      }
    }

    public virtual Rectangle CalculateItemBounds(OLVListItem item, OLVListSubItem subItem)
    {
      if (item == null)
        return Rectangle.Empty;
      if (subItem == null)
        return item.Bounds;
      return item.GetSubItemBounds(item.SubItems.IndexOf((ListViewItem.ListViewSubItem) subItem));
    }

    protected virtual void ApplyRotation(Graphics g, Rectangle r)
    {
      if (this.Rotation == 0)
        return;
      g.ResetTransform();
      Matrix matrix = new Matrix();
      matrix.RotateAt((float) this.Rotation, (PointF) new Point(r.Left + r.Width / 2, r.Top + r.Height / 2));
      g.Transform = matrix;
    }

    protected virtual void UnapplyRotation(Graphics g)
    {
      if (this.Rotation == 0)
        return;
      g.ResetTransform();
    }
  }
}
