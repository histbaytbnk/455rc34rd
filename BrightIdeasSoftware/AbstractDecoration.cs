
// Type: BrightIdeasSoftware.AbstractDecoration


// Hacked by SystemAce

using System.Drawing;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class AbstractDecoration : IDecoration, IOverlay
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

    public Rectangle RowBounds
    {
      get
      {
        if (this.ListItem == null)
          return Rectangle.Empty;
        return this.ListItem.Bounds;
      }
    }

    public Rectangle CellBounds
    {
      get
      {
        if (this.ListItem == null || this.SubItem == null)
          return Rectangle.Empty;
        return this.ListItem.GetSubItemBounds(this.ListItem.SubItems.IndexOf((ListViewItem.ListViewSubItem) this.SubItem));
      }
    }

    public virtual void Draw(ObjectListView olv, Graphics g, Rectangle r)
    {
    }
  }
}
