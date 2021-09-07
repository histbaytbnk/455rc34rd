
// Type: BrightIdeasSoftware.ListFilter


// Hacked by SystemAce

using System.Collections;

namespace BrightIdeasSoftware
{
  public class ListFilter : AbstractListFilter
  {
    private ListFilter.ListFilterDelegate function;

    public ListFilter.ListFilterDelegate Function
    {
      get
      {
        return this.function;
      }
      set
      {
        this.function = value;
      }
    }

    public ListFilter(ListFilter.ListFilterDelegate function)
    {
      this.Function = function;
    }

    public override IEnumerable Filter(IEnumerable modelObjects)
    {
      if (this.Function == null)
        return modelObjects;
      return this.Function(modelObjects);
    }

    public delegate IEnumerable ListFilterDelegate(IEnumerable rowObjects);
  }
}
