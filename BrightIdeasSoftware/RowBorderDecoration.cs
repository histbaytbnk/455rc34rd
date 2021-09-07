
// Type: BrightIdeasSoftware.RowBorderDecoration


// Hacked by SystemAce

using System.Drawing;

namespace BrightIdeasSoftware
{
  public class RowBorderDecoration : BorderDecoration
  {
    private int leftColumn = -1;
    private int rightColumn = -1;

    public int LeftColumn
    {
      get
      {
        return this.leftColumn;
      }
      set
      {
        this.leftColumn = value;
      }
    }

    public int RightColumn
    {
      get
      {
        return this.rightColumn;
      }
      set
      {
        this.rightColumn = value;
      }
    }

    protected override Rectangle CalculateBounds()
    {
      Rectangle rowBounds = this.RowBounds;
      if (this.ListItem == null)
        return rowBounds;
      if (this.LeftColumn >= 0)
      {
        Rectangle subItemBounds = this.ListItem.GetSubItemBounds(this.LeftColumn);
        if (!subItemBounds.IsEmpty)
        {
          rowBounds.Width = rowBounds.Right - subItemBounds.Left;
          rowBounds.X = subItemBounds.Left;
        }
      }
      if (this.RightColumn >= 0)
      {
        Rectangle subItemBounds = this.ListItem.GetSubItemBounds(this.RightColumn);
        if (!subItemBounds.IsEmpty)
          rowBounds.Width = subItemBounds.Right - rowBounds.Left;
      }
      return rowBounds;
    }
  }
}
