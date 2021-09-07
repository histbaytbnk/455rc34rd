
// Type: BrightIdeasSoftware.CompositeFilter


// Hacked by SystemAce

using System.Collections.Generic;

namespace BrightIdeasSoftware
{
  public abstract class CompositeFilter : IModelFilter
  {
    private IList<IModelFilter> filters = (IList<IModelFilter>) new List<IModelFilter>();

    public IList<IModelFilter> Filters
    {
      get
      {
        return this.filters;
      }
      set
      {
        this.filters = value;
      }
    }

    public CompositeFilter()
    {
    }

    public CompositeFilter(IEnumerable<IModelFilter> filters)
    {
      foreach (IModelFilter modelFilter in filters)
      {
        if (modelFilter != null)
          this.Filters.Add(modelFilter);
      }
    }

    public virtual bool Filter(object modelObject)
    {
      if (this.Filters == null || this.Filters.Count == 0)
        return true;
      return this.FilterObject(modelObject);
    }

    public abstract bool FilterObject(object modelObject);
  }
}
