
// Type: BrightIdeasSoftware.CellBorderDecoration


// Hacked by SystemAce

using System.Drawing;

namespace BrightIdeasSoftware
{
  public class CellBorderDecoration : BorderDecoration
  {
    protected override Rectangle CalculateBounds()
    {
      return this.CellBounds;
    }
  }
}
