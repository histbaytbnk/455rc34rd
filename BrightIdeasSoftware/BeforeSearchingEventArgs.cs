
// Type: BrightIdeasSoftware.BeforeSearchingEventArgs


// Hacked by SystemAce

namespace BrightIdeasSoftware
{
  public class BeforeSearchingEventArgs : CancellableEventArgs
  {
    public string StringToFind;
    public int StartSearchFrom;

    public BeforeSearchingEventArgs(string stringToFind, int startSearchFrom)
    {
      this.StringToFind = stringToFind;
      this.StartSearchFrom = startSearchFrom;
    }
  }
}
