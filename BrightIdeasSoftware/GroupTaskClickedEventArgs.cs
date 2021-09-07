
// Type: BrightIdeasSoftware.GroupTaskClickedEventArgs


// Hacked by SystemAce

using System;

namespace BrightIdeasSoftware
{
  public class GroupTaskClickedEventArgs : EventArgs
  {
    private readonly OLVGroup group;

    public OLVGroup Group
    {
      get
      {
        return this.group;
      }
    }

    public GroupTaskClickedEventArgs(OLVGroup group)
    {
      this.group = group;
    }
  }
}
