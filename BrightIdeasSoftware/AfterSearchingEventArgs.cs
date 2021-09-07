
// Type: BrightIdeasSoftware.AfterSearchingEventArgs


// Hacked by SystemAce

using System;

namespace BrightIdeasSoftware
{
  public class AfterSearchingEventArgs : EventArgs
  {
    private string stringToFind;
    public bool Handled;
    private int indexSelected;

    public string StringToFind
    {
      get
      {
        return this.stringToFind;
      }
    }

    public int IndexSelected
    {
      get
      {
        return this.indexSelected;
      }
    }

    public AfterSearchingEventArgs(string stringToFind, int indexSelected)
    {
      this.stringToFind = stringToFind;
      this.indexSelected = indexSelected;
    }
  }
}
