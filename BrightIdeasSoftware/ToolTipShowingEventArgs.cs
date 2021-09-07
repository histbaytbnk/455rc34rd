
// Type: BrightIdeasSoftware.ToolTipShowingEventArgs


// Hacked by SystemAce

using System.Drawing;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class ToolTipShowingEventArgs : CellEventArgs
  {
    private ToolTipControl toolTipControl;
    public string Text;
    public RightToLeft RightToLeft;
    public bool? IsBalloon;
    public Color? BackColor;
    public Color? ForeColor;
    public string Title;
    public ToolTipControl.StandardIcons? StandardIcon;
    public int? AutoPopDelay;
    public Font Font;

    public ToolTipControl ToolTipControl
    {
      get
      {
        return this.toolTipControl;
      }
      internal set
      {
        this.toolTipControl = value;
      }
    }
  }
}
