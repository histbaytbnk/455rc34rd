
// Type: BrightIdeasSoftware.ItemsAddingEventArgs


// Hacked by SystemAce

using System.Collections;

namespace BrightIdeasSoftware
{
  public class ItemsAddingEventArgs : CancellableEventArgs
  {
    public ICollection ObjectsToAdd;

    public ItemsAddingEventArgs(ICollection objectsToAdd)
    {
      this.ObjectsToAdd = objectsToAdd;
    }
  }
}
