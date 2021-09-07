
// Type: BrightIdeasSoftware.TailFilter


// Hacked by SystemAce

using System;
using System.Collections;

namespace BrightIdeasSoftware
{
  public class TailFilter : AbstractListFilter
  {
    private int count;

    public int Count
    {
      get
      {
        return this.count;
      }
      set
      {
        this.count = value;
      }
    }

    public TailFilter()
    {
    }

    public TailFilter(int numberOfObjects)
    {
      this.Count = numberOfObjects;
    }

    public override IEnumerable Filter(IEnumerable modelObjects)
    {
      if (this.Count <= 0)
        return modelObjects;
      ArrayList arrayList = ObjectListView.EnumerableToArray(modelObjects, false);
      if (this.Count > arrayList.Count)
        return (IEnumerable) arrayList;
      object[] objArray = new object[this.Count];
      arrayList.CopyTo(arrayList.Count - this.Count, (Array) objArray, 0, this.Count);
      return (IEnumerable) new ArrayList((ICollection) objArray);
    }
  }
}
