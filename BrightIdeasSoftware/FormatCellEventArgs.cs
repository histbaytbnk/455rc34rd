
// Type: BrightIdeasSoftware.FormatCellEventArgs


// Hacked by SystemAce

namespace BrightIdeasSoftware
{
  public class FormatCellEventArgs : FormatRowEventArgs
  {
    private int columnIndex = -1;
    private OLVColumn column;
    private OLVListSubItem subItem;

    public int ColumnIndex
    {
      get
      {
        return this.columnIndex;
      }
      internal set
      {
        this.columnIndex = value;
      }
    }

    public OLVColumn Column
    {
      get
      {
        return this.column;
      }
      internal set
      {
        this.column = value;
      }
    }

    public OLVListSubItem SubItem
    {
      get
      {
        return this.subItem;
      }
      internal set
      {
        this.subItem = value;
      }
    }

    public object CellValue
    {
      get
      {
        if (this.SubItem != null)
          return this.SubItem.ModelValue;
        return (object) null;
      }
    }
  }
}
