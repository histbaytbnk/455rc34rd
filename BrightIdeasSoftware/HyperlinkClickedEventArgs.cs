
// Type: BrightIdeasSoftware.HyperlinkClickedEventArgs


// Hacked by SystemAce

namespace BrightIdeasSoftware
{
  public class HyperlinkClickedEventArgs : CellEventArgs
  {
    private string url;

    public string Url
    {
      get
      {
        return this.url;
      }
      set
      {
        this.url = value;
      }
    }
  }
}
