
// Type: BrightIdeasSoftware.BooleanCellEditor


// Hacked by SystemAce

using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  [ToolboxItem(false)]
  public class BooleanCellEditor : ComboBox
  {
    public BooleanCellEditor()
    {
      this.DropDownStyle = ComboBoxStyle.DropDownList;
      this.ValueMember = "Key";
      this.DataSource = (object) new ArrayList()
      {
        (object) new ComboBoxItem((object) false, "False"),
        (object) new ComboBoxItem((object) true, "True")
      };
    }
  }
}
