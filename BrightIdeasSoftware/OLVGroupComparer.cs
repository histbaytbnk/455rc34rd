
// Type: BrightIdeasSoftware.OLVGroupComparer


// Hacked by SystemAce

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class OLVGroupComparer : IComparer<OLVGroup>
  {
    private SortOrder sortOrder;

    public OLVGroupComparer(SortOrder order)
    {
      this.sortOrder = order;
    }

    public int Compare(OLVGroup x, OLVGroup y)
    {
      int num = x.SortValue == null || y.SortValue == null ? string.Compare(x.Header, y.Header, StringComparison.CurrentCultureIgnoreCase) : x.SortValue.CompareTo((object) y.SortValue);
      if (this.sortOrder == SortOrder.Descending)
        num = -num;
      return num;
    }
  }
}
