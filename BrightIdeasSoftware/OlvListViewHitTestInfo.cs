
// Type: BrightIdeasSoftware.OlvListViewHitTestInfo


// Hacked by SystemAce

using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class OlvListViewHitTestInfo
  {
    private int headerDividerIndex = -1;
    public HitTestLocation HitTestLocation;
    public HitTestLocationEx HitTestLocationEx;
    public OLVGroup Group;
    public object UserData;
    private OLVListItem item;
    private OLVListSubItem subItem;
    private ListViewHitTestLocations location;
    private ObjectListView listView;
    private int columnIndex;

    public OLVListItem Item
    {
      get
      {
        return this.item;
      }
      internal set
      {
        this.item = value;
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

    public ListViewHitTestLocations Location
    {
      get
      {
        return this.location;
      }
      internal set
      {
        this.location = value;
      }
    }

    public ObjectListView ListView
    {
      get
      {
        return this.listView;
      }
      internal set
      {
        this.listView = value;
      }
    }

    public object RowObject
    {
      get
      {
        if (this.Item != null)
          return this.Item.RowObject;
        return (object) null;
      }
    }

    public int RowIndex
    {
      get
      {
        if (this.Item != null)
          return this.Item.Index;
        return -1;
      }
    }

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

    public int HeaderDividerIndex
    {
      get
      {
        return this.headerDividerIndex;
      }
      internal set
      {
        this.headerDividerIndex = value;
      }
    }

    public OLVColumn Column
    {
      get
      {
        int columnIndex = this.ColumnIndex;
        if (columnIndex >= 0 && this.ListView != null)
          return this.ListView.GetColumn(columnIndex);
        return (OLVColumn) null;
      }
    }

    public OlvListViewHitTestInfo(OLVListItem olvListItem, OLVListSubItem subItem, int flags, OLVGroup group, int iColumn)
    {
      this.item = olvListItem;
      this.subItem = subItem;
      this.location = OlvListViewHitTestInfo.ConvertNativeFlagsToDotNetLocation(olvListItem, flags);
      this.HitTestLocationEx = (HitTestLocationEx) flags;
      this.Group = group;
      this.ColumnIndex = iColumn;
      this.ListView = olvListItem == null ? (ObjectListView) null : (ObjectListView) olvListItem.ListView;
      switch (this.location)
      {
        case ListViewHitTestLocations.Image:
          this.HitTestLocation = HitTestLocation.Image;
          break;
        case ListViewHitTestLocations.Label:
          this.HitTestLocation = HitTestLocation.Text;
          break;
        case ListViewHitTestLocations.StateImage:
          this.HitTestLocation = HitTestLocation.CheckBox;
          break;
        default:
          if ((this.HitTestLocationEx & HitTestLocationEx.LVHT_EX_GROUP_COLLAPSE) == HitTestLocationEx.LVHT_EX_GROUP_COLLAPSE)
          {
            this.HitTestLocation = HitTestLocation.GroupExpander;
            break;
          }
          if ((this.HitTestLocationEx & HitTestLocationEx.LVHT_EX_GROUP_MINUS_FOOTER_AND_BKGRD) != (HitTestLocationEx) 0)
          {
            this.HitTestLocation = HitTestLocation.Group;
            break;
          }
          this.HitTestLocation = HitTestLocation.Nothing;
          break;
      }
    }

    public OlvListViewHitTestInfo(ObjectListView olv, int iColumn, bool isOverCheckBox, int iDivider)
    {
      this.ListView = olv;
      this.ColumnIndex = iColumn;
      this.HeaderDividerIndex = iDivider;
      this.HitTestLocation = isOverCheckBox ? HitTestLocation.HeaderCheckBox : (iDivider < 0 ? HitTestLocation.Header : HitTestLocation.HeaderDivider);
    }

    private static ListViewHitTestLocations ConvertNativeFlagsToDotNetLocation(OLVListItem hitItem, int flags)
    {
      if ((8 & flags) == 8)
        return (ListViewHitTestLocations) (247 & flags | (hitItem == null ? 256 : 512));
      return (ListViewHitTestLocations) (flags & (int) ushort.MaxValue);
    }

    public override string ToString()
    {
      return string.Format("HitTestLocation: {0}, HitTestLocationEx: {1}, Item: {2}, SubItem: {3}, Location: {4}, Group: {5}, ColumnIndex: {6}", (object) this.HitTestLocation, (object) this.HitTestLocationEx, (object) this.item, (object) this.subItem, (object) this.location, (object) this.Group, (object) this.ColumnIndex);
    }

    internal class HeaderHitTestInfo
    {
      public int ColumnIndex;
      public bool IsOverCheckBox;
      public int OverDividerIndex;
    }
  }
}
