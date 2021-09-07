
// Type: BrightIdeasSoftware.MappedImageRenderer


// Hacked by SystemAce

using System;
using System.Collections;
using System.Drawing;

namespace BrightIdeasSoftware
{
  public class MappedImageRenderer : BaseRenderer
  {
    private Hashtable map;
    private object nullImage;

    public MappedImageRenderer()
    {
      this.map = new Hashtable();
    }

    public MappedImageRenderer(object key, object image)
      : this()
    {
      this.Add(key, image);
    }

    public MappedImageRenderer(object key1, object image1, object key2, object image2)
      : this()
    {
      this.Add(key1, image1);
      this.Add(key2, image2);
    }

    public MappedImageRenderer(object[] keysAndImages)
      : this()
    {
      if (keysAndImages.GetLength(0) % 2 != 0)
        throw new ArgumentException("Array must have key/image pairs");
      int index = 0;
      while (index < keysAndImages.GetLength(0))
      {
        this.Add(keysAndImages[index], keysAndImages[index + 1]);
        index += 2;
      }
    }

    public static MappedImageRenderer Boolean(object trueImage, object falseImage)
    {
      return new MappedImageRenderer((object) true, trueImage, (object) false, falseImage);
    }

    public static MappedImageRenderer TriState(object trueImage, object falseImage, object nullImage)
    {
      return new MappedImageRenderer(new object[6]
      {
        (object) true,
        trueImage,
        (object) false,
        falseImage,
        null,
        nullImage
      });
    }

    public void Add(object value, object image)
    {
      if (value == null)
        this.nullImage = image;
      else
        this.map[value] = image;
    }

    public override void Render(Graphics g, Rectangle r)
    {
      this.DrawBackground(g, r);
      r = this.ApplyCellPadding(r);
      ICollection imageSelectors = this.Aspect as ICollection;
      if (imageSelectors == null)
        this.RenderOne(g, r, this.Aspect);
      else
        this.RenderCollection(g, r, imageSelectors);
    }

    protected void RenderCollection(Graphics g, Rectangle r, ICollection imageSelectors)
    {
      ArrayList arrayList = new ArrayList();
      foreach (object key in (IEnumerable) imageSelectors)
      {
        Image image = key != null ? (!this.map.ContainsKey(key) ? (Image) null : this.GetImage(this.map[key])) : this.GetImage(this.nullImage);
        if (image != null)
          arrayList.Add((object) image);
      }
      this.DrawImages(g, r, (ICollection) arrayList);
    }

    protected void RenderOne(Graphics g, Rectangle r, object selector)
    {
      Image image = (Image) null;
      if (selector == null)
        image = this.GetImage(this.nullImage);
      else if (this.map.ContainsKey(selector))
        image = this.GetImage(this.map[selector]);
      if (image == null)
        return;
      this.DrawAlignedImage(g, r, image);
    }
  }
}
