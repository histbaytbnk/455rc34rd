
// Type: BrightIdeasSoftware.OneOfFilter


// Hacked by SystemAce

using System.Collections;

namespace BrightIdeasSoftware
{
  public class OneOfFilter : IModelFilter
  {
    private AspectGetterDelegate valueGetter;
    private IList possibleValues;

    public virtual AspectGetterDelegate ValueGetter
    {
      get
      {
        return this.valueGetter;
      }
      set
      {
        this.valueGetter = value;
      }
    }

    public virtual IList PossibleValues
    {
      get
      {
        return this.possibleValues;
      }
      set
      {
        this.possibleValues = value;
      }
    }

    public OneOfFilter(AspectGetterDelegate valueGetter)
      : this(valueGetter, (ICollection) new ArrayList())
    {
    }

    public OneOfFilter(AspectGetterDelegate valueGetter, ICollection possibleValues)
    {
      this.ValueGetter = valueGetter;
      this.PossibleValues = (IList) new ArrayList(possibleValues);
    }

    public virtual bool Filter(object modelObject)
    {
      if (this.ValueGetter == null || this.PossibleValues == null || this.PossibleValues.Count == 0)
        return false;
      object result1 = this.ValueGetter(modelObject);
      IEnumerable enumerable = result1 as IEnumerable;
      if (result1 is string || enumerable == null)
        return this.DoesValueMatch(result1);
      foreach (object result2 in enumerable)
      {
        if (this.DoesValueMatch(result2))
          return true;
      }
      return false;
    }

    protected virtual bool DoesValueMatch(object result)
    {
      return this.PossibleValues.Contains(result);
    }
  }
}
