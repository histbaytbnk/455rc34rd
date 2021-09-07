
// Type: BrightIdeasSoftware.AbstractVirtualListDataSource


// Hacked by SystemAce

using System;
using System.Collections;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class AbstractVirtualListDataSource : IVirtualListDataSource, IFilterableDataSource
  {
    protected VirtualObjectListView listView;

    public AbstractVirtualListDataSource(VirtualObjectListView listView)
    {
      this.listView = listView;
    }

    public virtual object GetNthObject(int n)
    {
      return (object) null;
    }

    public virtual int GetObjectCount()
    {
      return -1;
    }

    public virtual int GetObjectIndex(object model)
    {
      return -1;
    }

    public virtual void PrepareCache(int from, int to)
    {
    }

    public virtual int SearchText(string value, int first, int last, OLVColumn column)
    {
      return -1;
    }

    public virtual void Sort(OLVColumn column, SortOrder order)
    {
    }

    public virtual void AddObjects(ICollection modelObjects)
    {
    }

    public virtual void RemoveObjects(ICollection modelObjects)
    {
    }

    public virtual void SetObjects(IEnumerable collection)
    {
    }

    public virtual void UpdateObject(int index, object modelObject)
    {
    }

    public static int DefaultSearchText(string value, int first, int last, OLVColumn column, IVirtualListDataSource source)
    {
      if (first <= last)
      {
        for (int n = first; n <= last; ++n)
        {
          if (column.GetStringValue(source.GetNthObject(n)).StartsWith(value, StringComparison.CurrentCultureIgnoreCase))
            return n;
        }
      }
      else
      {
        for (int n = first; n >= last; --n)
        {
          if (column.GetStringValue(source.GetNthObject(n)).StartsWith(value, StringComparison.CurrentCultureIgnoreCase))
            return n;
        }
      }
      return -1;
    }

    public virtual void ApplyFilters(IModelFilter modelFilter, IListFilter listFilter)
    {
    }
  }
}
