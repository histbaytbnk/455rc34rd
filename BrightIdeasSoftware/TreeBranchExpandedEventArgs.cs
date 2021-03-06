
// Type: BrightIdeasSoftware.TreeBranchExpandedEventArgs


// Hacked by SystemAce

using System;

namespace BrightIdeasSoftware
{
  public class TreeBranchExpandedEventArgs : EventArgs
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

    public TreeBranchExpandedEventArgs(object model, OLVListItem item)
    {
      this.Model = model;
      this.Item = item;
    }
  }
}
