
// Type: BrightIdeasSoftware.MultiImageRenderer


// Hacked by SystemAce

using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class MultiImageRenderer : BaseRenderer
  {
    private int maxNumberImages = 10;
    private int maximumValue = 100;
    private object imageSelector;
    private int minimumValue;

    [DefaultValue(-1)]
    [Category("Behavior")]
    [Description("The index of the image that should be drawn")]
    public int ImageIndex
    {
      get
      {
        if (this.imageSelector is int)
          return (int) this.imageSelector;
        return -1;
      }
      set
      {
        this.imageSelector = (object) value;
      }
    }

    [Category("Behavior")]
    [DefaultValue(null)]
    [Description("The index of the image that should be drawn")]
    public string ImageName
    {
      get
      {
        return this.imageSelector as string;
      }
      set
      {
        this.imageSelector = (object) value;
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public object ImageSelector
    {
      get
      {
        return this.imageSelector;
      }
      set
      {
        this.imageSelector = value;
      }
    }

    [Category("Behavior")]
    [Description("The maximum number of images that this renderer should draw")]
    [DefaultValue(10)]
    public int MaxNumberImages
    {
      get
      {
        return this.maxNumberImages;
      }
      set
      {
        this.maxNumberImages = value;
      }
    }

    [Description("Values less than or equal to this will have 0 images drawn")]
    [DefaultValue(0)]
    [Category("Behavior")]
    public int MinimumValue
    {
      get
      {
        return this.minimumValue;
      }
      set
      {
        this.minimumValue = value;
      }
    }

    [Category("Behavior")]
    [DefaultValue(100)]
    [Description("Values greater than or equal to this will have MaxNumberImages images drawn")]
    public int MaximumValue
    {
      get
      {
        return this.maximumValue;
      }
      set
      {
        this.maximumValue = value;
      }
    }

    public MultiImageRenderer()
    {
    }

    public MultiImageRenderer(object imageSelector, int maxImages, int minValue, int maxValue)
      : this()
    {
      this.ImageSelector = imageSelector;
      this.MaxNumberImages = maxImages;
      this.MinimumValue = minValue;
      this.MaximumValue = maxValue;
    }

    public override void Render(Graphics g, Rectangle r)
    {
      this.DrawBackground(g, r);
      r = this.ApplyCellPadding(r);
      Image image = this.GetImage(this.ImageSelector);
      if (image == null)
        return;
      IConvertible convertible = this.Aspect as IConvertible;
      if (convertible == null)
        return;
      double num1 = convertible.ToDouble((IFormatProvider) NumberFormatInfo.InvariantInfo);
      int num2 = num1 > (double) this.MinimumValue ? (num1 >= (double) this.MaximumValue ? this.MaxNumberImages : 1 + (int) ((double) this.MaxNumberImages * (num1 - (double) this.MinimumValue) / (double) this.MaximumValue)) : 0;
      int width = image.Width;
      int height = image.Height;
      if (r.Height < image.Height)
      {
        width = (int) ((double) image.Width * (double) r.Height / (double) image.Height);
        height = r.Height;
      }
      Rectangle inner = r;
      inner.Width = this.MaxNumberImages * (width + this.Spacing) - this.Spacing;
      inner.Height = height;
      inner = this.AlignRectangle(r, inner);
      Color backgroundColor = this.GetBackgroundColor();
      for (int index = 0; index < num2; ++index)
      {
        if (this.ListItem.Enabled)
          g.DrawImage(image, inner.X, inner.Y, width, height);
        else
          ControlPaint.DrawImageDisabled(g, image, inner.X, inner.Y, backgroundColor);
        inner.X += width + this.Spacing;
      }
    }

    protected override Rectangle HandleGetEditRectangle(Graphics g, Rectangle cellBounds, OLVListItem item, int subItemIndex, Size preferredSize)
    {
      return this.CalculatePaddedAlignedBounds(g, cellBounds, preferredSize);
    }
  }
}
