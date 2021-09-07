
// Type: BrightIdeasSoftware.ModelFilter


// Hacked by SystemAce

namespace BrightIdeasSoftware
{
  public class ModelFilter : IModelFilter
  {
    private System.Predicate<object> predicate;

    protected System.Predicate<object> Predicate
    {
      get
      {
        return this.predicate;
      }
      set
      {
        this.predicate = value;
      }
    }

    public ModelFilter(System.Predicate<object> predicate)
    {
      this.Predicate = predicate;
    }

    public virtual bool Filter(object modelObject)
    {
      if (this.Predicate != null)
        return this.Predicate(modelObject);
      return true;
    }
  }
}
