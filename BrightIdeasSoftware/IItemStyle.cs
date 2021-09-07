
// Type: BrightIdeasSoftware.IItemStyle


// Hacked by SystemAce

using System.Drawing;

namespace BrightIdeasSoftware
{
  public interface IItemStyle
  {
    Font Font { get; set; }

    FontStyle FontStyle { get; set; }

    Color ForeColor { get; set; }

    Color BackColor { get; set; }
  }
}
