
// Type: BrightIdeasSoftware.TreeDataSourceAdapter


// Hacked by SystemAce

using System.Collections;
using System.ComponentModel;

namespace BrightIdeasSoftware
{
  public class TreeDataSourceAdapter : DataSourceAdapter
  {
    private bool showKeyColumns = true;
    private string keyAspectName;
    private string parentKeyAspectName;
    private object rootKeyValue;
    private readonly DataTreeListView treeListView;
    private Munger keyMunger;
    private Munger parentKeyMunger;

    public virtual string KeyAspectName
    {
      get
      {
        return this.keyAspectName;
      }
      set
      {
        if (this.keyAspectName == value)
          return;
        this.keyAspectName = value;
        this.keyMunger = new Munger(this.KeyAspectName);
        this.InitializeDataSource();
      }
    }

    public virtual string ParentKeyAspectName
    {
      get
      {
        return this.parentKeyAspectName;
      }
      set
      {
        if (this.parentKeyAspectName == value)
          return;
        this.parentKeyAspectName = value;
        this.parentKeyMunger = new Munger(this.ParentKeyAspectName);
        this.InitializeDataSource();
      }
    }

    public virtual object RootKeyValue
    {
      get
      {
        return this.rootKeyValue;
      }
      set
      {
        if (object.Equals(this.rootKeyValue, value))
          return;
        this.rootKeyValue = value;
        this.InitializeDataSource();
      }
    }

    public virtual bool ShowKeyColumns
    {
      get
      {
        return this.showKeyColumns;
      }
      set
      {
        this.showKeyColumns = value;
      }
    }

    protected DataTreeListView TreeListView
    {
      get
      {
        return this.treeListView;
      }
    }

    public TreeDataSourceAdapter(DataTreeListView tlv)
      : base((ObjectListView) tlv)
    {
      this.treeListView = tlv;
      this.treeListView.CanExpandGetter = (BrightIdeasSoftware.TreeListView.CanExpandGetterDelegate) (model => this.CalculateHasChildren(model));
      this.treeListView.ChildrenGetter = (BrightIdeasSoftware.TreeListView.ChildrenGetterDelegate) (model => this.CalculateChildren(model));
    }

    protected override void InitializeDataSource()
    {
      base.InitializeDataSource();
      this.TreeListView.RebuildAll(true);
    }

    protected override void SetListContents()
    {
      this.TreeListView.Roots = this.CalculateRoots();
    }

    protected override bool ShouldCreateColumn(PropertyDescriptor property)
    {
      if (!this.ShowKeyColumns && (property.Name == this.KeyAspectName || property.Name == this.ParentKeyAspectName))
        return false;
      return base.ShouldCreateColumn(property);
    }

    protected override void HandleListChangedItemChanged(ListChangedEventArgs e)
    {
      if (e.PropertyDescriptor != null && (e.PropertyDescriptor.Name == this.KeyAspectName || e.PropertyDescriptor.Name == this.ParentKeyAspectName))
        this.InitializeDataSource();
      else
        base.HandleListChangedItemChanged(e);
    }

    protected override void ChangePosition(int index)
    {
      for (object model = this.CalculateParent(this.CurrencyManager.List[index]); model != null && !this.TreeListView.IsExpanded(model); model = this.CalculateParent(model))
        this.TreeListView.Expand(model);
      base.ChangePosition(index);
    }

    private IEnumerable CalculateRoots()
    {
      foreach (object model in (IEnumerable) this.CurrencyManager.List)
      {
        object parentKey = this.GetParentValue(model);
        if (object.Equals(this.RootKeyValue, parentKey))
          yield return model;
      }
    }

    private bool CalculateHasChildren(object model)
    {
      object keyValue = this.GetKeyValue(model);
      if (keyValue == null)
        return false;
      foreach (object model1 in (IEnumerable) this.CurrencyManager.List)
      {
        object parentValue = this.GetParentValue(model1);
        if (object.Equals(keyValue, parentValue))
          return true;
      }
      return false;
    }

    private IEnumerable CalculateChildren(object model)
    {
      object keyValue = this.GetKeyValue(model);
      if (keyValue != null)
      {
        foreach (object model1 in (IEnumerable) this.CurrencyManager.List)
        {
          object parentKey = this.GetParentValue(model1);
          if (object.Equals(keyValue, parentKey))
            yield return model1;
        }
      }
    }

    private object CalculateParent(object model)
    {
      object parentValue = this.GetParentValue(model);
      if (parentValue == null)
        return (object) null;
      foreach (object model1 in (IEnumerable) this.CurrencyManager.List)
      {
        object keyValue = this.GetKeyValue(model1);
        if (object.Equals(parentValue, keyValue))
          return model1;
      }
      return (object) null;
    }

    private object GetKeyValue(object model)
    {
      if (this.keyMunger != null)
        return this.keyMunger.GetValue(model);
      return (object) null;
    }

    private object GetParentValue(object model)
    {
      if (this.parentKeyMunger != null)
        return this.parentKeyMunger.GetValue(model);
      return (object) null;
    }
  }
}
