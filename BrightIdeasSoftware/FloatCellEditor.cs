
// Type: BrightIdeasSoftware.FloatCellEditor


// Hacked by SystemAce

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  [ToolboxItem(false)]
  public class FloatCellEditor : NumericUpDown
  {
    public double Value
    {
      get
      {
        return Convert.ToDouble(base.Value);
      }
      set
      {
                this.Value = 1;
      }
    }

    public FloatCellEditor()
    {
      this.DecimalPlaces = 2;
      this.Minimum = new Decimal(-9999999);
      this.Maximum = new Decimal(9999999);
    }
  }
}
