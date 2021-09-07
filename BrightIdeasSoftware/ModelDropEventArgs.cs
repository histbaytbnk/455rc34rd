
// Type: BrightIdeasSoftware.ModelDropEventArgs


// Hacked by SystemAce

using System.Collections;

namespace BrightIdeasSoftware
{
  public class ModelDropEventArgs : OlvDropEventArgs
  {
    private ArrayList toBeRefreshed = new ArrayList();
    private IList dragModels;
    private ObjectListView sourceListView;
    private object targetModel;

    public IList SourceModels
    {
      get
      {
        return this.dragModels;
      }
      internal set
      {
        this.dragModels = value;
        TreeListView treeListView = this.SourceListView as TreeListView;
        if (treeListView == null)
          return;
        foreach (object model in (IEnumerable) this.SourceModels)
        {
          object parent = treeListView.GetParent(model);
          if (!this.toBeRefreshed.Contains(parent))
            this.toBeRefreshed.Add(parent);
        }
      }
    }

    public ObjectListView SourceListView
    {
      get
      {
        return this.sourceListView;
      }
      internal set
      {
        this.sourceListView = value;
      }
    }

    public object TargetModel
    {
      get
      {
        return this.targetModel;
      }
      internal set
      {
        this.targetModel = value;
      }
    }

    public void RefreshObjects()
    {
      TreeListView treeListView = this.SourceListView as TreeListView;
      if (treeListView != null)
      {
        foreach (object model in (IEnumerable) this.SourceModels)
        {
          object parent = treeListView.GetParent(model);
          if (!this.toBeRefreshed.Contains(parent))
            this.toBeRefreshed.Add(parent);
        }
      }
      this.toBeRefreshed.AddRange((ICollection) this.SourceModels);
      if (this.ListView == this.SourceListView)
      {
        this.toBeRefreshed.Add(this.TargetModel);
        this.ListView.RefreshObjects((IList) this.toBeRefreshed);
      }
      else
      {
        this.SourceListView.RefreshObjects((IList) this.toBeRefreshed);
        this.ListView.RefreshObject(this.TargetModel);
      }
    }
  }
}
