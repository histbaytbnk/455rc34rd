
// Type: BrightIdeasSoftware.CompositeAnyFilter


// Hacked by SystemAce

using System.Collections.Generic;

namespace BrightIdeasSoftware
{
  public class CompositeAnyFilter : CompositeFilter
  {
    public CompositeAnyFilter(List<IModelFilter> filters)
      : base((IEnumerable<IModelFilter>) filters)
    {
    }

    public override bool FilterObject(object modelObject)
    {
      foreach (IModelFilter modelFilter in (IEnumerable<IModelFilter>) this.Filters)
      {
        if (modelFilter.Filter(modelObject))
          return true;
      }
      return false;
    }
  }
}
