
// Type: BrightIdeasSoftware.ImageAdornment


// Hacked by SystemAce

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;

namespace BrightIdeasSoftware
{
  public class ImageAdornment : GraphicAdornment
  {
    private Image image;
    private bool shrinkToWidth;

    [DefaultValue(null)]
    [Category("ObjectListView")]
    [Description("The image that will be drawn")]
    [NotifyParentProperty(true)]
    public Image Image
    {
      get
      {
        return this.image;
      }
      set
      {
        this.image = value;
      }
    }

    [Category("ObjectListView")]
    [DefaultValue(false)]
    [Description("Will the image be shrunk to fit within its width?")]
    public bool ShrinkToWidth
    {
      get
      {
        return this.shrinkToWidth;
      }
      set
      {
        this.shrinkToWidth = value;
      }
    }

    public virtual void DrawImage(Graphics g, Rectangle r)
    {
      if (this.ShrinkToWidth)
        this.DrawScaledImage(g, r, this.Image, this.Transparency);
      else
        this.DrawImage(g, r, this.Image, this.Transparency);
    }

    public virtual void DrawImage(Graphics g, Rectangle r, Image image, int transparency)
    {
      if (image == null)
        return;
      this.DrawImage(g, r, image, image.Size, transparency);
    }

    public virtual void DrawImage(Graphics g, Rectangle r, Image image, Size sz, int transparency)
    {
      if (image == null)
        return;
      Rectangle alignedRectangle = this.CreateAlignedRectangle(r, sz);
      try
      {
        this.ApplyRotation(g, alignedRectangle);
        this.DrawTransparentBitmap(g, alignedRectangle, image, transparency);
      }
      finally
      {
        this.UnapplyRotation(g);
      }
    }

    public virtual void DrawScaledImage(Graphics g, Rectangle r, Image image, int transparency)
    {
      if (image == null)
        return;
      Size size = image.Size;
      if (image.Width > r.Width)
      {
        float num = (float) r.Width / (float) image.Width;
        size.Height = (int) ((double) image.Height * (double) num);
        size.Width = r.Width - 1;
      }
      this.DrawImage(g, r, image, size, transparency);
    }

    protected virtual void DrawTransparentBitmap(Graphics g, Rectangle r, Image image, int transparency)
    {
      ImageAttributes imageAttr = (ImageAttributes) null;
      if (transparency != (int) byte.MaxValue)
      {
        imageAttr = new ImageAttributes();
        float num = (float) transparency / (float) byte.MaxValue;
        float[][] numArray1 = new float[5][];
        float[][] numArray2 = numArray1;
        int index1 = 0;
        float[] numArray3 = new float[5];
        numArray3[0] = 1f;
        float[] numArray4 = numArray3;
        numArray2[index1] = numArray4;
        float[][] numArray5 = numArray1;
        int index2 = 1;
        float[] numArray6 = new float[5];
        numArray6[1] = 1f;
        float[] numArray7 = numArray6;
        numArray5[index2] = numArray7;
        float[][] numArray8 = numArray1;
        int index3 = 2;
        float[] numArray9 = new float[5];
        numArray9[2] = 1f;
        float[] numArray10 = numArray9;
        numArray8[index3] = numArray10;
        float[][] numArray11 = numArray1;
        int index4 = 3;
        float[] numArray12 = new float[5];
        numArray12[3] = num;
        float[] numArray13 = numArray12;
        numArray11[index4] = numArray13;
        float[][] numArray14 = numArray1;
        int index5 = 4;
        float[] numArray15 = new float[5];
        numArray15[4] = 1f;
        float[] numArray16 = numArray15;
        numArray14[index5] = numArray16;
        float[][] newColorMatrix = numArray1;
        imageAttr.SetColorMatrix(new ColorMatrix(newColorMatrix));
      }
      g.DrawImage(image, r, 0, 0, image.Size.Width, image.Size.Height, GraphicsUnit.Pixel, imageAttr);
    }
  }
}
