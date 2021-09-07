
// Type: BrightIdeasSoftware.Design.OLVColumnCollectionEditor


// Hacked by SystemAce

using BrightIdeasSoftware;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace BrightIdeasSoftware.Design
{
  public class OLVColumnCollectionEditor : CollectionEditor
  {
    public OLVColumnCollectionEditor(Type t)
      : base(t)
    {
    }

    protected override Type CreateCollectionItemType()
    {
      return typeof (OLVColumn);
    }

    public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
    {
      if (context == null)
        throw new ArgumentNullException("context");
      if (provider == null)
        throw new ArgumentNullException("provider");
      ObjectListView objectListView = context.Instance as ObjectListView;
      base.EditValue(context, provider, (object) objectListView.AllColumns);
      List<OLVColumn> filteredColumns = objectListView.GetFilteredColumns(View.Details);
      objectListView.Columns.Clear();
      objectListView.Columns.AddRange((ColumnHeader[]) filteredColumns.ToArray());
      return (object) objectListView.Columns;
    }

    protected override string GetDisplayText(object value)
    {
      OLVColumn olvColumn = value as OLVColumn;
      if (olvColumn == null || string.IsNullOrEmpty(olvColumn.AspectName))
        return base.GetDisplayText(value);
      return string.Format("{0} ({1})", (object) base.GetDisplayText(value), (object) olvColumn.AspectName);
    }
  }
}
