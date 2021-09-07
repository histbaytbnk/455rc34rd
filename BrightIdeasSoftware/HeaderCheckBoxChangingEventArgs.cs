
// Type: BrightIdeasSoftware.HeaderCheckBoxChangingEventArgs


// Hacked by SystemAce

using System.ComponentModel;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class HeaderCheckBoxChangingEventArgs : CancelEventArgs
  {
    private OLVColumn column;
    private CheckState newCheckState;

    public OLVColumn Column
    {
      get
      {
        return this.column;
      }
      internal set
      {
        this.column = value;
      }
    }

    public CheckState NewCheckState
    {
      get
      {
        return this.newCheckState;
      }
      set
      {
        this.newCheckState = value;
      }
    }
  }
}
