
// Type: BrightIdeasSoftware.FreezeEventArgs


// Hacked by SystemAce

using System;

namespace BrightIdeasSoftware
{
  public class FreezeEventArgs : EventArgs
  {
    private int freezeLevel;

    public int FreezeLevel
    {
      get
      {
        return this.freezeLevel;
      }
      set
      {
        this.freezeLevel = value;
      }
    }

    public FreezeEventArgs(int freeze)
    {
      this.FreezeLevel = freeze;
    }
  }
}
