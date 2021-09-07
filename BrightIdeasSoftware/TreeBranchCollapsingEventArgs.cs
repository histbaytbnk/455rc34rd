
// Type: BrightIdeasSoftware.TreeBranchCollapsingEventArgs


// Hacked by SystemAce

namespace BrightIdeasSoftware
{
  public class TreeBranchCollapsingEventArgs : CancellableEventArgs
  {
    private object model;
    private OLVListItem item;

    public object Model
    {
      get
      {
        return this.model;
      }
      private set
      {
        this.model = value;
      }
    }

    public OLVListItem Item
    {
      get
      {
        return this.item;
      }
      private set
      {
        this.item = value;
      }
    }

    public TreeBranchCollapsingEventArgs(object model, OLVListItem item)
    {
      this.Model = model;
      this.Item = item;
    }
  }
}
