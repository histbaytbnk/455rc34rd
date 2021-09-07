
// Type: BrightIdeasSoftware.ItemsChangingEventArgs


// Hacked by SystemAce

using System.Collections;

namespace BrightIdeasSoftware
{
  public class ItemsChangingEventArgs : CancellableEventArgs
  {
    private IEnumerable oldObjects;
    public IEnumerable NewObjects;

    public IEnumerable OldObjects
    {
      get
      {
        return this.oldObjects;
      }
    }

    public ItemsChangingEventArgs(IEnumerable oldObjects, IEnumerable newObjects)
    {
      this.oldObjects = oldObjects;
      this.NewObjects = newObjects;
    }
  }
}
