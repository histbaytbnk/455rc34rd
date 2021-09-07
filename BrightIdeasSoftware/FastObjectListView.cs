
// Type: BrightIdeasSoftware.FastObjectListView


// Hacked by SystemAce

using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class FastObjectListView : VirtualObjectListView
  {
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public override IEnumerable FilteredObjects
    {
      get
      {
        return (IEnumerable) ((FastObjectListDataSource) this.VirtualListDataSource).FilteredObjectList;
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override IEnumerable Objects
    {
      get
      {
        return (IEnumerable) ((FastObjectListDataSource) this.VirtualListDataSource).ObjectList;
      }
      set
      {
        base.Objects = value;
      }
    }

    public FastObjectListView()
    {
      this.VirtualListDataSource = (IVirtualListDataSource) new FastObjectListDataSource(this);
      this.GroupingStrategy = (IVirtualGroups) new FastListGroupingStrategy();
    }

    public override void Unsort()
    {
      this.ShowGroups = false;
      this.PrimarySortColumn = (OLVColumn) null;
      this.PrimarySortOrder = SortOrder.None;
      this.SetObjects(this.Objects);
    }
  }
}
