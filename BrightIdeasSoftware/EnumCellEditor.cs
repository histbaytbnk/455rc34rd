
// Type: BrightIdeasSoftware.EnumCellEditor


// Hacked by SystemAce

using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  [ToolboxItem(false)]
  public class EnumCellEditor : ComboBox
  {
    public EnumCellEditor(Type type)
    {
      this.DropDownStyle = ComboBoxStyle.DropDownList;
      this.ValueMember = "Key";
      ArrayList arrayList = new ArrayList();
      foreach (object key in Enum.GetValues(type))
        arrayList.Add((object) new ComboBoxItem(key, Enum.GetName(type, key)));
      this.DataSource = (object) arrayList;
    }
  }
}
