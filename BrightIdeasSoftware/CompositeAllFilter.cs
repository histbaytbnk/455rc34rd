
// Type: BrightIdeasSoftware.CompositeAllFilter


// Hacked by SystemAce

using System.Collections.Generic;

namespace BrightIdeasSoftware
{
  public class CompositeAllFilter : CompositeFilter
  {
    public CompositeAllFilter(List<IModelFilter> filters)
      : base((IEnumerable<IModelFilter>) filters)
    {
    }

    public override bool FilterObject(object modelObject)
    {
      foreach (IModelFilter modelFilter in (IEnumerable<IModelFilter>) this.Filters)
      {
        if (!modelFilter.Filter(modelObject))
          return false;
      }
      return true;
    }
  }
}
