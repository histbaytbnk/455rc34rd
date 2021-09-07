
// Type: BrightIdeasSoftware.GroupStateChangedEventArgs


// Hacked by SystemAce

using System;

namespace BrightIdeasSoftware
{
  public class GroupStateChangedEventArgs : EventArgs
  {
    private readonly OLVGroup group;
    private readonly GroupState oldState;
    private readonly GroupState newState;

    public bool Collapsed
    {
      get
      {
        if ((this.oldState & GroupState.LVGS_COLLAPSED) != GroupState.LVGS_COLLAPSED)
          return (this.newState & GroupState.LVGS_COLLAPSED) == GroupState.LVGS_COLLAPSED;
        return false;
      }
    }

    public bool Focused
    {
      get
      {
        if ((this.oldState & GroupState.LVGS_FOCUSED) != GroupState.LVGS_FOCUSED)
          return (this.newState & GroupState.LVGS_FOCUSED) == GroupState.LVGS_FOCUSED;
        return false;
      }
    }

    public bool Selected
    {
      get
      {
        if ((this.oldState & GroupState.LVGS_SELECTED) != GroupState.LVGS_SELECTED)
          return (this.newState & GroupState.LVGS_SELECTED) == GroupState.LVGS_SELECTED;
        return false;
      }
    }

    public bool Uncollapsed
    {
      get
      {
        if ((this.oldState & GroupState.LVGS_COLLAPSED) == GroupState.LVGS_COLLAPSED)
          return (this.newState & GroupState.LVGS_COLLAPSED) != GroupState.LVGS_COLLAPSED;
        return false;
      }
    }

    public bool Unfocused
    {
      get
      {
        if ((this.oldState & GroupState.LVGS_FOCUSED) == GroupState.LVGS_FOCUSED)
          return (this.newState & GroupState.LVGS_FOCUSED) != GroupState.LVGS_FOCUSED;
        return false;
      }
    }

    public bool Unselected
    {
      get
      {
        if ((this.oldState & GroupState.LVGS_SELECTED) == GroupState.LVGS_SELECTED)
          return (this.newState & GroupState.LVGS_SELECTED) != GroupState.LVGS_SELECTED;
        return false;
      }
    }

    public OLVGroup Group
    {
      get
      {
        return this.group;
      }
    }

    public GroupState OldState
    {
      get
      {
        return this.oldState;
      }
    }

    public GroupState NewState
    {
      get
      {
        return this.newState;
      }
    }

    public GroupStateChangedEventArgs(OLVGroup group, GroupState oldState, GroupState newState)
    {
      this.group = group;
      this.oldState = oldState;
      this.newState = newState;
    }
  }
}
