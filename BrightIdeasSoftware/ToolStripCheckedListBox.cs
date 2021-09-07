
// Type: BrightIdeasSoftware.ToolStripCheckedListBox


// Hacked by SystemAce

using System.Drawing;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class ToolStripCheckedListBox : ToolStripControlHost
  {
    public CheckedListBox CheckedListBoxControl
    {
      get
      {
        return this.Control as CheckedListBox;
      }
    }

    public CheckedListBox.ObjectCollection Items
    {
      get
      {
        return this.CheckedListBoxControl.Items;
      }
    }

    public bool CheckedOnClick
    {
      get
      {
        return this.CheckedListBoxControl.CheckOnClick;
      }
      set
      {
        this.CheckedListBoxControl.CheckOnClick = value;
      }
    }

    public CheckedListBox.CheckedItemCollection CheckedItems
    {
      get
      {
        return this.CheckedListBoxControl.CheckedItems;
      }
    }

    public event ItemCheckEventHandler ItemCheck;

    public ToolStripCheckedListBox()
      : base((Control) new CheckedListBox())
    {
      this.CheckedListBoxControl.MaximumSize = new Size(400, 700);
      this.CheckedListBoxControl.ThreeDCheckBoxes = true;
      this.CheckedListBoxControl.CheckOnClick = true;
      this.CheckedListBoxControl.SelectionMode = SelectionMode.One;
    }

    public void AddItem(object item, bool isChecked)
    {
      this.Items.Add(item);
      if (!isChecked)
        return;
      this.CheckedListBoxControl.SetItemChecked(this.Items.Count - 1, true);
    }

    public void AddItem(object item, CheckState state)
    {
      this.Items.Add(item);
      this.CheckedListBoxControl.SetItemCheckState(this.Items.Count - 1, state);
    }

    public CheckState GetItemCheckState(int i)
    {
      return this.CheckedListBoxControl.GetItemCheckState(i);
    }

    public void SetItemState(int i, CheckState checkState)
    {
      if (i < 0 || i >= this.Items.Count)
        return;
      this.CheckedListBoxControl.SetItemCheckState(i, checkState);
    }

    public void CheckAll()
    {
      for (int index = 0; index < this.Items.Count; ++index)
        this.CheckedListBoxControl.SetItemChecked(index, true);
    }

    public void UncheckAll()
    {
      for (int index = 0; index < this.Items.Count; ++index)
        this.CheckedListBoxControl.SetItemChecked(index, false);
    }

    protected override void OnSubscribeControlEvents(Control c)
    {
      base.OnSubscribeControlEvents(c);
      ((CheckedListBox) c).ItemCheck += new ItemCheckEventHandler(this.OnItemCheck);
    }

    protected override void OnUnsubscribeControlEvents(Control c)
    {
      base.OnUnsubscribeControlEvents(c);
      ((CheckedListBox) c).ItemCheck -= new ItemCheckEventHandler(this.OnItemCheck);
    }

    private void OnItemCheck(object sender, ItemCheckEventArgs e)
    {
      if (this.ItemCheck == null)
        return;
      this.ItemCheck((object) this, e);
    }
  }
}
