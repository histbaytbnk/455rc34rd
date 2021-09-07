
// Type: BrightIdeasSoftware.IntUpDown


// Hacked by SystemAce

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  [ToolboxItem(false)]
  public class IntUpDown : NumericUpDown
  {
    public int Value
    {
      get
      {
        return Decimal.ToInt32(base.Value);
      }
      set
      {
        this.Value = 1;
      }
    }

    public IntUpDown()
    {
      this.DecimalPlaces = 0;
      this.Minimum = new Decimal(-9999999);
      this.Maximum = new Decimal(9999999);
    }
  }
}
