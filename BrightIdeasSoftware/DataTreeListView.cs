
// Type: BrightIdeasSoftware.DataTreeListView


// Hacked by SystemAce

using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace BrightIdeasSoftware
{
  public class DataTreeListView : TreeListView
  {
    private TreeDataSourceAdapter adapter;

    [Category("Data")]
    [TypeConverter("System.Windows.Forms.Design.DataSourceConverter, System.Design")]
    public virtual object DataSource
    {
      get
      {
        return this.Adapter.DataSource;
      }
      set
      {
        this.Adapter.DataSource = value;
      }
    }

    [Editor("System.Windows.Forms.Design.DataMemberListEditor, System.Design", typeof (UITypeEditor))]
    [Category("Data")]
    [DefaultValue("")]
    public virtual string DataMember
    {
      get
      {
        return this.Adapter.DataMember;
      }
      set
      {
        this.Adapter.DataMember = value;
      }
    }

    [DefaultValue(null)]
    [Description("The name of the property/column that holds the key of a row")]
    [Category("Data")]
    public virtual string KeyAspectName
    {
      get
      {
        return this.Adapter.KeyAspectName;
      }
      set
      {
        this.Adapter.KeyAspectName = value;
      }
    }

    [Category("Data")]
    [Description("The name of the property/column that holds the key of the parent of a row")]
    [DefaultValue(null)]
    public virtual string ParentKeyAspectName
    {
      get
      {
        return this.Adapter.ParentKeyAspectName;
      }
      set
      {
        this.Adapter.ParentKeyAspectName = value;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public virtual object RootKeyValue
    {
      get
      {
        return this.Adapter.RootKeyValue;
      }
      set
      {
        this.Adapter.RootKeyValue = value;
      }
    }

    [DefaultValue(null)]
    [Description("The parent id value that identifies a row as a root object")]
    [Category("Data")]
    public virtual string RootKeyValueString
    {
      get
      {
        return Convert.ToString(this.Adapter.RootKeyValue);
      }
      set
      {
        this.Adapter.RootKeyValue = (object) value;
      }
    }

    [Description("Should the keys columns (id and parent id) be shown to the user?")]
    [Category("Data")]
    [DefaultValue(true)]
    public virtual bool ShowKeyColumns
    {
      get
      {
        return this.Adapter.ShowKeyColumns;
      }
      set
      {
        this.Adapter.ShowKeyColumns = value;
      }
    }

    protected TreeDataSourceAdapter Adapter
    {
      get
      {
        if (this.adapter == null)
          this.adapter = new TreeDataSourceAdapter(this);
        return this.adapter;
      }
      set
      {
        this.adapter = value;
      }
    }
  }
}
