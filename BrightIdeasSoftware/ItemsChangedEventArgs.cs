
// Type: BrightIdeasSoftware.ItemsChangedEventArgs


// Hacked by SystemAce

using System;

namespace BrightIdeasSoftware
{
  public class ItemsChangedEventArgs : EventArgs
  {
    private int oldObjectCount;
    private int newObjectCount;

    public int OldObjectCount
    {
      get
      {
        return this.oldObjectCount;
      }
    }

    public int NewObjectCount
    {
      get
      {
        return this.newObjectCount;
      }
    }

    public ItemsChangedEventArgs()
    {
    }

    public ItemsChangedEventArgs(int oldObjectCount, int newObjectCount)
    {
      this.oldObjectCount = oldObjectCount;
      this.newObjectCount = newObjectCount;
    }
  }
}
