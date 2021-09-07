
// Type: BrightIdeasSoftware.ItemsRemovingEventArgs


// Hacked by SystemAce

using System.Collections;

namespace BrightIdeasSoftware
{
  public class ItemsRemovingEventArgs : CancellableEventArgs
  {
    public ICollection ObjectsToRemove;

    public ItemsRemovingEventArgs(ICollection objectsToRemove)
    {
      this.ObjectsToRemove = objectsToRemove;
    }
  }
}
