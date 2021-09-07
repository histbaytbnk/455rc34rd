
// Type: BrightIdeasSoftware.VirtualListVersion1DataSource


// Hacked by SystemAce

namespace BrightIdeasSoftware
{
  public class VirtualListVersion1DataSource : AbstractVirtualListDataSource
  {
    private RowGetterDelegate rowGetter;

    public RowGetterDelegate RowGetter
    {
      get
      {
        return this.rowGetter;
      }
      set
      {
        this.rowGetter = value;
      }
    }

    public VirtualListVersion1DataSource(VirtualObjectListView listView)
      : base(listView)
    {
    }

    public override object GetNthObject(int n)
    {
      if (this.RowGetter == null)
        return (object) null;
      return this.RowGetter(n);
    }

    public override int SearchText(string value, int first, int last, OLVColumn column)
    {
      return AbstractVirtualListDataSource.DefaultSearchText(value, first, last, column, (IVirtualListDataSource) this);
    }
  }
}
