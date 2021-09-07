
// Type: BrightIdeasSoftware.ComboBoxItem


// Hacked by SystemAce

namespace BrightIdeasSoftware
{
  public class ComboBoxItem
  {
    private readonly string description;
    private readonly object key;

    public object Key
    {
      get
      {
        return this.key;
      }
    }

    public ComboBoxItem(object key, string description)
    {
      this.key = key;
      this.description = description;
    }

    public override string ToString()
    {
      return this.description;
    }
  }
}
