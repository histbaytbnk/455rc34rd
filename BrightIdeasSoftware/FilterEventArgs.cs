
// Type: BrightIdeasSoftware.FilterEventArgs


// Hacked by SystemAce

using System;
using System.Collections;

namespace BrightIdeasSoftware
{
  public class FilterEventArgs : EventArgs
  {
    public IEnumerable Objects;
    public IEnumerable FilteredObjects;

    public FilterEventArgs(IEnumerable objects)
    {
      this.Objects = objects;
    }
  }
}
