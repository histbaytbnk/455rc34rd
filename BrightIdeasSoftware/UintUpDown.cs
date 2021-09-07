
// Type: BrightIdeasSoftware.UintUpDown


// Hacked by SystemAce

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  [ToolboxItem(false)]
  internal class UintUpDown : NumericUpDown
  {
    

    public UintUpDown()
    {
      this.DecimalPlaces = 0;
      this.Minimum = new Decimal(0);
      this.Maximum = new Decimal(9999999);
    }
  }
}
