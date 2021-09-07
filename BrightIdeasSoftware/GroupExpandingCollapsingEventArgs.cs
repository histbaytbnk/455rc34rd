
// Type: BrightIdeasSoftware.GroupExpandingCollapsingEventArgs


// Hacked by SystemAce

using System;

namespace BrightIdeasSoftware
{
  public class GroupExpandingCollapsingEventArgs : CancellableEventArgs
  {
    private readonly OLVGroup olvGroup;

    public OLVGroup Group
    {
      get
      {
        return this.olvGroup;
      }
    }

    public bool IsExpanding
    {
      get
      {
        return this.Group.Collapsed;
      }
    }

    public GroupExpandingCollapsingEventArgs(OLVGroup group)
    {
      if (group == null)
        throw new ArgumentNullException("group");
      this.olvGroup = group;
    }
  }
}
