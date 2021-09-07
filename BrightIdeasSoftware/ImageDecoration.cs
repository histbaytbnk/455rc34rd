
// Type: BrightIdeasSoftware.ImageDecoration


// Hacked by SystemAce

using System.Drawing;

namespace BrightIdeasSoftware
{
  public class ImageDecoration : ImageAdornment, IDecoration, IOverlay
  {
    private OLVListItem listItem;
    private OLVListSubItem subItem;

    public OLVListItem ListItem
    {
      get
      {
        return this.listItem;
      }
      set
      {
        this.listItem = value;
      }
    }

    public OLVListSubItem SubItem
    {
      get
      {
        return this.subItem;
      }
      set
      {
        this.subItem = value;
      }
    }

    public ImageDecoration()
    {
      this.Alignment = ContentAlignment.MiddleRight;
    }

    public ImageDecoration(Image image)
      : this()
    {
      this.Image = image;
    }

    public ImageDecoration(Image image, int transparency)
      : this()
    {
      this.Image = image;
      this.Transparency = transparency;
    }

    public ImageDecoration(Image image, ContentAlignment alignment)
      : this()
    {
      this.Image = image;
      this.Alignment = alignment;
    }

    public ImageDecoration(Image image, int transparency, ContentAlignment alignment)
      : this()
    {
      this.Image = image;
      this.Transparency = transparency;
      this.Alignment = alignment;
    }

    public virtual void Draw(ObjectListView olv, Graphics g, Rectangle r)
    {
      this.DrawImage(g, this.CalculateItemBounds(this.ListItem, this.SubItem));
    }
  }
}
