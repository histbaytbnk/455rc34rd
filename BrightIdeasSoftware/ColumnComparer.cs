
// Type: BrightIdeasSoftware.ColumnComparer


// Hacked by SystemAce

using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class ColumnComparer : IComparer, IComparer<OLVListItem>
  {
    private OLVColumn column;
    private SortOrder sortOrder;
    private ColumnComparer secondComparer;

    public ColumnComparer(OLVColumn col, SortOrder order)
    {
      this.column = col;
      this.sortOrder = order;
    }

    public ColumnComparer(OLVColumn col, SortOrder order, OLVColumn col2, SortOrder order2)
      : this(col, order)
    {
      if (col == col2)
        return;
      this.secondComparer = new ColumnComparer(col2, order2);
    }

    public int Compare(object x, object y)
    {
      return this.Compare((OLVListItem) x, (OLVListItem) y);
    }

    public int Compare(OLVListItem x, OLVListItem y)
    {
      if (this.sortOrder == SortOrder.None)
        return 0;
      object x1 = this.column.GetValue(x.RowObject);
      object y1 = this.column.GetValue(y.RowObject);
      bool flag1 = x1 == null || x1 == DBNull.Value;
      bool flag2 = y1 == null || y1 == DBNull.Value;
      int num = flag1 || flag2 ? (!flag1 || !flag2 ? (flag1 ? -1 : 1) : 0) : this.CompareValues(x1, y1);
      if (this.sortOrder == SortOrder.Descending)
        num = -num;
      if (num == 0 && this.secondComparer != null)
        num = this.secondComparer.Compare(x, y);
      return num;
    }

    public int CompareValues(object x, object y)
    {
      string strA = x as string;
      if (strA != null)
        return string.Compare(strA, (string) y, StringComparison.CurrentCultureIgnoreCase);
      IComparable comparable = x as IComparable;
      if (comparable != null)
        return comparable.CompareTo(y);
      return 0;
    }
  }
}
