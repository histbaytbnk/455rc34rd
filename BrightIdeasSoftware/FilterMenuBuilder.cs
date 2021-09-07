
// Type: BrightIdeasSoftware.FilterMenuBuilder


// Hacked by SystemAce

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class FilterMenuBuilder
  {
    public static string APPLY_LABEL = "Apply";
    public static string CLEAR_ALL_FILTERS_LABEL = "Clear All Filters";
    public static string FILTERING_LABEL = "Filtering";
    public static string SELECT_ALL_LABEL = "Select All";
    private bool treatNullAsDataValue = true;
    private int maxObjectsToConsider = 10000;
    private bool alreadyInHandleItemChecked;

    public bool TreatNullAsDataValue
    {
      get
      {
        return this.treatNullAsDataValue;
      }
      set
      {
        this.treatNullAsDataValue = value;
      }
    }

    public int MaxObjectsToConsider
    {
      get
      {
        return this.maxObjectsToConsider;
      }
      set
      {
        this.maxObjectsToConsider = value;
      }
    }

    public virtual ToolStripDropDown MakeFilterMenu(ToolStripDropDown strip, ObjectListView listView, OLVColumn column)
    {
      if (strip == null)
        throw new ArgumentNullException("strip");
      if (listView == null)
        throw new ArgumentNullException("listView");
      if (column == null)
        throw new ArgumentNullException("column");
      if (!column.UseFiltering || column.ClusteringStrategy == null || listView.Objects == null)
        return strip;
      List<ICluster> clusters = this.Cluster(column.ClusteringStrategy, listView, column);
      if (clusters.Count > 0)
      {
        this.SortClusters(column.ClusteringStrategy, clusters);
        strip.Items.Add((ToolStripItem) this.CreateFilteringMenuItem(column, clusters));
      }
      return strip;
    }

    protected virtual List<ICluster> Cluster(IClusteringStrategy strategy, ObjectListView listView, OLVColumn column)
    {
      NullableDictionary<object, ICluster> map = new NullableDictionary<object, ICluster>();
      int num = 0;
      foreach (object model in listView.ObjectsForClustering)
      {
        this.ClusterOneModel(strategy, map, model);
        if (num++ > this.MaxObjectsToConsider)
          break;
      }
      foreach (ICluster cluster in (IEnumerable<ICluster>) map.Values)
        cluster.DisplayLabel = strategy.GetClusterDisplayLabel(cluster);
      return new List<ICluster>((IEnumerable<ICluster>) map.Values);
    }

    private void ClusterOneModel(IClusteringStrategy strategy, NullableDictionary<object, ICluster> map, object model)
    {
      object clusterKey = strategy.GetClusterKey(model);
      IEnumerable enumerable = clusterKey as IEnumerable;
      if (clusterKey is string || enumerable == null)
        enumerable = (IEnumerable) new object[1]
        {
          clusterKey
        };
      ArrayList arrayList = new ArrayList();
      foreach (object obj in enumerable)
      {
        if (obj == null || obj == DBNull.Value)
        {
          if (this.TreatNullAsDataValue)
            arrayList.Add((object) null);
        }
        else
          arrayList.Add(obj);
      }
      foreach (object index in arrayList)
      {
        if (map.ContainsKey(index))
          ++map[index].Count;
        else
          map[index] = strategy.CreateCluster(index);
      }
    }

    protected virtual void SortClusters(IClusteringStrategy strategy, List<ICluster> clusters)
    {
      clusters.Sort();
    }

    protected virtual ToolStripMenuItem CreateFilteringMenuItem(OLVColumn column, List<ICluster> clusters)
    {
      ToolStripCheckedListBox stripCheckedListBox = new ToolStripCheckedListBox();
      stripCheckedListBox.Tag = (object) column;
      foreach (ICluster cluster in clusters)
        stripCheckedListBox.AddItem((object) cluster, column.ValuesChosenForFiltering.Contains(cluster.ClusterKey));
      if (!string.IsNullOrEmpty(FilterMenuBuilder.SELECT_ALL_LABEL))
      {
        int count = stripCheckedListBox.CheckedItems.Count;
        if (count == 0)
          stripCheckedListBox.AddItem((object) FilterMenuBuilder.SELECT_ALL_LABEL, CheckState.Unchecked);
        else
          stripCheckedListBox.AddItem((object) FilterMenuBuilder.SELECT_ALL_LABEL, count == clusters.Count ? CheckState.Checked : CheckState.Indeterminate);
      }
      stripCheckedListBox.ItemCheck += new ItemCheckEventHandler(this.HandleItemCheckedWrapped);
      return new ToolStripMenuItem(FilterMenuBuilder.FILTERING_LABEL, (Image) null, new ToolStripItem[2]
      {
        (ToolStripItem) new ToolStripSeparator(),
        (ToolStripItem) stripCheckedListBox
      });
    }

    private void HandleItemCheckedWrapped(object sender, ItemCheckEventArgs e)
    {
      if (this.alreadyInHandleItemChecked)
        return;
      try
      {
        this.alreadyInHandleItemChecked = true;
        this.HandleItemChecked(sender, e);
      }
      finally
      {
        this.alreadyInHandleItemChecked = false;
      }
    }

    protected virtual void HandleItemChecked(object sender, ItemCheckEventArgs e)
    {
      ToolStripCheckedListBox checkedList = sender as ToolStripCheckedListBox;
      if (checkedList == null)
        return;
      OLVColumn olvColumn = checkedList.Tag as OLVColumn;
      if (olvColumn == null || !(olvColumn.ListView is ObjectListView))
        return;
      int selectAllIndex = checkedList.Items.IndexOf((object) FilterMenuBuilder.SELECT_ALL_LABEL);
      if (selectAllIndex < 0)
        return;
      this.HandleSelectAllItem(e, checkedList, selectAllIndex);
    }

    protected virtual void HandleSelectAllItem(ItemCheckEventArgs e, ToolStripCheckedListBox checkedList, int selectAllIndex)
    {
      if (e.Index == selectAllIndex)
      {
        if (e.NewValue == CheckState.Checked)
          checkedList.CheckAll();
        if (e.NewValue != CheckState.Unchecked)
          return;
        checkedList.UncheckAll();
      }
      else
      {
        int count = checkedList.CheckedItems.Count;
        if (checkedList.GetItemCheckState(selectAllIndex) != CheckState.Unchecked)
          --count;
        if (e.NewValue != e.CurrentValue)
        {
          if (e.NewValue == CheckState.Checked)
            ++count;
          else
            --count;
        }
        if (count == 0)
          checkedList.SetItemState(selectAllIndex, CheckState.Unchecked);
        else if (count == checkedList.Items.Count - 1)
          checkedList.SetItemState(selectAllIndex, CheckState.Checked);
        else
          checkedList.SetItemState(selectAllIndex, CheckState.Indeterminate);
      }
    }

    protected virtual void ClearAllFilters(OLVColumn column)
    {
      ObjectListView objectListView = column.ListView as ObjectListView;
      if (objectListView == null || objectListView.IsDisposed)
        return;
      objectListView.ResetColumnFiltering();
    }

    protected virtual void EnactFilter(ToolStripCheckedListBox checkedList, OLVColumn column)
    {
      ObjectListView objectListView = column.ListView as ObjectListView;
      if (objectListView == null || objectListView.IsDisposed)
        return;
      ArrayList arrayList = new ArrayList();
      foreach (object obj in checkedList.CheckedItems)
      {
        ICluster cluster = obj as ICluster;
        if (cluster != null)
          arrayList.Add(cluster.ClusterKey);
      }
      column.ValuesChosenForFiltering = (IList) arrayList;
      objectListView.UpdateColumnFiltering();
    }
  }
}
