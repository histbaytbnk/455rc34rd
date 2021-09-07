
// Type: BrightIdeasSoftware.CreateGroupsEventArgs


// Hacked by SystemAce

using System;
using System.Collections.Generic;

namespace BrightIdeasSoftware
{
  public class CreateGroupsEventArgs : EventArgs
  {
    private GroupingParameters parameters;
    private IList<OLVGroup> groups;
    private bool canceled;

    public GroupingParameters Parameters
    {
      get
      {
        return this.parameters;
      }
    }

    public IList<OLVGroup> Groups
    {
      get
      {
        return this.groups;
      }
      set
      {
        this.groups = value;
      }
    }

    public bool Canceled
    {
      get
      {
        return this.canceled;
      }
      set
      {
        this.canceled = value;
      }
    }

    public CreateGroupsEventArgs(GroupingParameters parms)
    {
      this.parameters = parms;
    }
  }
}
