
// Type: BrightIdeasSoftware.CheckStateRenderer


// Hacked by SystemAce

using System.Drawing;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class CheckStateRenderer : BaseRenderer
  {
    public override void Render(Graphics g, Rectangle r)
    {
      this.DrawBackground(g, r);
      if (this.Column == null)
        return;
      r = this.ApplyCellPadding(r);
      CheckState checkState = this.Column.GetCheckState(this.RowObject);
      if (this.IsPrinting)
      {
        string index = "checkbox-checked";
        if (checkState == CheckState.Unchecked)
          index = "checkbox-unchecked";
        if (checkState == CheckState.Indeterminate)
          index = "checkbox-indeterminate";
        this.DrawAlignedImage(g, r, this.ListView.SmallImageList.Images[index]);
      }
      else
      {
        r = this.CalculateCheckBoxBounds(g, r);
        CheckBoxRenderer.DrawCheckBox(g, r.Location, this.GetCheckBoxState(checkState));
      }
    }

    protected override Rectangle HandleGetEditRectangle(Graphics g, Rectangle cellBounds, OLVListItem item, int subItemIndex, Size preferredSize)
    {
      return this.CalculatePaddedAlignedBounds(g, cellBounds, preferredSize);
    }

    protected override void HandleHitTest(Graphics g, OlvListViewHitTestInfo hti, int x, int y)
    {
      if (!this.CalculateCheckBoxBounds(g, this.Bounds).Contains(x, y))
        return;
      hti.HitTestLocation = HitTestLocation.CheckBox;
    }
  }
}
