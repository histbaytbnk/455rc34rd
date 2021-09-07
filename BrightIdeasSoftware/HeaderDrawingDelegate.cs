
// Type: BrightIdeasSoftware.HeaderDrawingDelegate


// Hacked by SystemAce

using System.Drawing;

namespace BrightIdeasSoftware
{
  public delegate bool HeaderDrawingDelegate(Graphics g, Rectangle r, int columnIndex, OLVColumn column, bool isPressed, HeaderStateStyle stateStyle);
}
