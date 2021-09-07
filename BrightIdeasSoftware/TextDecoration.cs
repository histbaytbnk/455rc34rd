
// Type: BrightIdeasSoftware.TextDecoration


// Hacked by SystemAce

using System.Drawing;

namespace BrightIdeasSoftware
{
  public class TextDecoration : TextAdornment, IDecoration, IOverlay
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

    public TextDecoration()
    {
      this.Alignment = ContentAlignment.MiddleRight;
    }

    public TextDecoration(string text)
      : this()
    {
      this.Text = text;
    }

    public TextDecoration(string text, int transparency)
      : this()
    {
      this.Text = text;
      this.Transparency = transparency;
    }

    public TextDecoration(string text, ContentAlignment alignment)
      : this()
    {
      this.Text = text;
      this.Alignment = alignment;
    }

    public TextDecoration(string text, int transparency, ContentAlignment alignment)
      : this()
    {
      this.Text = text;
      this.Transparency = transparency;
      this.Alignment = alignment;
    }

    public virtual void Draw(ObjectListView olv, Graphics g, Rectangle r)
    {
      this.DrawText(g, this.CalculateItemBounds(this.ListItem, this.SubItem));
    }
  }
}
