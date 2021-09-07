﻿
// Type: BrightIdeasSoftware.VirtualObjectListView


// Hacked by SystemAce

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class VirtualObjectListView : ObjectListView
  {
    private bool checkedObjectsMustStillExistInList = true;
    private int lastRetrieveVirtualItemIndex = -1;
    private IVirtualGroups groupingStrategy;
    private bool showGroups;
    private IVirtualListDataSource virtualListDataSource;
    private static FieldInfo virtualListSizeFieldInfo;
    private OwnerDataCallbackImpl ownerDataCallbackImpl;
    private OLVListItem lastRetrieveVirtualItem;

    [Browsable(false)]
    public override bool CanShowGroups
    {
      get
      {
        if (ObjectListView.IsVistaOrLater)
          return this.GroupingStrategy != null;
        return false;
      }
    }

    [DefaultValue(false)]
    [Description("Should the list view show checkboxes?")]
    [Category("Appearance")]
    public new bool CheckBoxes
    {
      get
      {
        return base.CheckBoxes;
      }
      set
      {
        try
        {
          base.CheckBoxes = value;
        }
        catch (InvalidOperationException ex)
        {
          this.StateImageList = (ImageList) null;
          this.VirtualMode = false;
          base.CheckBoxes = value;
          this.VirtualMode = true;
          this.ShowGroups = this.ShowGroups;
          this.BuildList(true);
        }
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public override IList CheckedObjects
    {
      get
      {
        if (!this.CheckBoxes)
          return (IList) new ArrayList();
        if (this.VirtualListDataSource == null)
          return (IList) new ArrayList();
        if (this.CheckStateGetter != null)
          return base.CheckedObjects;
        ArrayList arrayList = new ArrayList();
        foreach (KeyValuePair<object, CheckState> keyValuePair in this.CheckStateMap)
        {
          if (keyValuePair.Value == CheckState.Checked && (!this.CheckedObjectsMustStillExistInList || this.VirtualListDataSource.GetObjectIndex(keyValuePair.Key) >= 0))
            arrayList.Add(keyValuePair.Key);
        }
        return (IList) arrayList;
      }
      set
      {
        if (!this.CheckBoxes)
          return;
        if (this.CheckStateGetter != null)
        {
          base.CheckedObjects = value;
        }
        else
        {
          Stopwatch.StartNew();
          Hashtable hashtable = new Hashtable(this.GetItemCount());
          if (value != null)
          {
            foreach (object index in (IEnumerable) value)
              hashtable[index] = (object) true;
          }
          this.BeginUpdate();
          object[] array = new object[this.CheckStateMap.Count];
          this.CheckStateMap.Keys.CopyTo(array, 0);
          foreach (object obj in array)
          {
            if (!hashtable.Contains(obj))
              this.SetObjectCheckedness(obj, CheckState.Unchecked);
          }
          foreach (object modelObject in (IEnumerable) hashtable.Keys)
            this.SetObjectCheckedness(modelObject, CheckState.Checked);
          this.EndUpdate();
        }
      }
    }

    protected internal bool CheckedObjectsMustStillExistInList
    {
      get
      {
        return this.checkedObjectsMustStillExistInList;
      }
      set
      {
        this.checkedObjectsMustStillExistInList = value;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public override IEnumerable FilteredObjects
    {
      get
      {
        for (int i = 0; i < this.GetItemCount(); ++i)
          yield return this.GetModelObject(i);
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public IVirtualGroups GroupingStrategy
    {
      get
      {
        return this.groupingStrategy;
      }
      set
      {
        this.groupingStrategy = value;
      }
    }

    public override bool IsFiltering
    {
      get
      {
        if (base.IsFiltering)
          return this.VirtualListDataSource is IFilterableDataSource;
        return false;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public override IEnumerable Objects
    {
      get
      {
        try
        {
          if (this.IsFiltering)
            ((IFilterableDataSource) this.VirtualListDataSource).ApplyFilters((IModelFilter) null, (IListFilter) null);
          return this.FilteredObjects;
        }
        finally
        {
          if (this.IsFiltering)
            ((IFilterableDataSource) this.VirtualListDataSource).ApplyFilters(this.ModelFilter, this.ListFilter);
        }
      }
      set
      {
        base.Objects = value;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public virtual RowGetterDelegate RowGetter
    {
      get
      {
        return ((VirtualListVersion1DataSource) this.virtualListDataSource).RowGetter;
      }
      set
      {
        ((VirtualListVersion1DataSource) this.virtualListDataSource).RowGetter = value;
      }
    }

    [Category("Appearance")]
    [Description("Should the list view show items in groups?")]
    [DefaultValue(true)]
    public override bool ShowGroups
    {
      get
      {
        if (ObjectListView.IsVistaOrLater)
          return this.showGroups;
        return false;
      }
      set
      {
        this.showGroups = value;
        if (!this.Created || value)
          return;
        this.DisableVirtualGroups();
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public virtual IVirtualListDataSource VirtualListDataSource
    {
      get
      {
        return this.virtualListDataSource;
      }
      set
      {
        this.virtualListDataSource = value;
        this.CustomSorter = (SortDelegate) ((column, sortOrder) =>
        {
          this.ClearCachedInfo();
          this.virtualListDataSource.Sort(column, sortOrder);
        });
        this.BuildList(false);
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    protected new virtual int VirtualListSize
    {
      get
      {
        return base.VirtualListSize;
      }
      set
      {
        if (value == this.VirtualListSize || value < 0)
          return;
        if (VirtualObjectListView.virtualListSizeFieldInfo == (FieldInfo) null)
          VirtualObjectListView.virtualListSizeFieldInfo = typeof (ListView).GetField("virtualListSize", BindingFlags.Instance | BindingFlags.NonPublic);
        VirtualObjectListView.virtualListSizeFieldInfo.SetValue((object) this, (object) value);
        if (!this.IsHandleCreated || this.DesignMode)
          return;
        NativeMethods.SetItemCount((ListView) this, value);
      }
    }

    public VirtualObjectListView()
    {
      this.VirtualMode = true;
      this.CacheVirtualItems += new CacheVirtualItemsEventHandler(this.HandleCacheVirtualItems);
      this.RetrieveVirtualItem += new RetrieveVirtualItemEventHandler(this.HandleRetrieveVirtualItem);
      this.SearchForVirtualItem += new SearchForVirtualItemEventHandler(this.HandleSearchForVirtualItem);
      this.VirtualListDataSource = (IVirtualListDataSource) new VirtualListVersion1DataSource(this);
      this.PersistentCheckBoxes = true;
    }

    public override int GetItemCount()
    {
      return this.VirtualListSize;
    }

    public override object GetModelObject(int index)
    {
      if (this.VirtualListDataSource != null && index >= 0 && index < this.GetItemCount())
        return this.VirtualListDataSource.GetNthObject(index);
      return (object) null;
    }

    public override int IndexOf(object modelObject)
    {
      if (this.VirtualListDataSource == null || modelObject == null)
        return -1;
      return this.VirtualListDataSource.GetObjectIndex(modelObject);
    }

    public override OLVListItem ModelToItem(object modelObject)
    {
      if (this.VirtualListDataSource == null || modelObject == null)
        return (OLVListItem) null;
      int objectIndex = this.VirtualListDataSource.GetObjectIndex(modelObject);
      if (objectIndex < 0)
        return (OLVListItem) null;
      return this.GetItem(objectIndex);
    }

    public override void AddObjects(ICollection modelObjects)
    {
      if (this.VirtualListDataSource == null)
        return;
      ItemsAddingEventArgs e = new ItemsAddingEventArgs(modelObjects);
      this.OnItemsAdding(e);
      if (e.Canceled)
        return;
      try
      {
        this.BeginUpdate();
        this.VirtualListDataSource.AddObjects(e.ObjectsToAdd);
        this.BuildList();
      }
      finally
      {
        this.EndUpdate();
      }
    }

    public override void ClearObjects()
    {
      if (this.InvokeRequired)
      {
        this.Invoke((Delegate) new MethodInvoker(((ObjectListView) this).ClearObjects));
      }
      else
      {
        this.CheckStateMap.Clear();
        this.SetObjects((IEnumerable) new ArrayList());
      }
    }

    public virtual void EnsureNthGroupVisible(int groupIndex)
    {
      if (!this.ShowGroups)
        return;
      if (groupIndex <= 0 || groupIndex >= this.OLVGroups.Count)
      {
        NativeMethods.Scroll((ListView) this, 0, -NativeMethods.GetScrollPosition((ListView) this, false));
      }
      else
      {
        OLVGroup group = this.OLVGroups[groupIndex - 1];
        Rectangle itemRect = this.GetItemRect(this.GroupingStrategy.GetGroupMember(group, group.VirtualItemCount - 1));
        NativeMethods.Scroll((ListView) this, 0, itemRect.Y + itemRect.Height / 2);
      }
    }

    public override void RefreshObjects(IList modelObjects)
    {
 
      {
        if (this.VirtualListDataSource == null)
          return;
        try
        {
          this.BeginUpdate();
          this.ClearCachedInfo();
          foreach (object obj in (IEnumerable) modelObjects)
          {
            int objectIndex = this.VirtualListDataSource.GetObjectIndex(obj);
            if (objectIndex >= 0)
            {
              this.VirtualListDataSource.UpdateObject(objectIndex, obj);
              this.RedrawItems(objectIndex, objectIndex, true);
            }
          }
        }
        finally
        {
          this.EndUpdate();
        }
      }
    }

    public override void RefreshSelectedObjects()
    {
      foreach (int num in this.SelectedIndices)
        this.RedrawItems(num, num, true);
    }

    public override void RemoveObjects(ICollection modelObjects)
    {
      if (this.VirtualListDataSource == null)
        return;
      ItemsRemovingEventArgs e = new ItemsRemovingEventArgs(modelObjects);
      this.OnItemsRemoving(e);
      if (e.Canceled)
        return;
      try
      {
        this.BeginUpdate();
        this.VirtualListDataSource.RemoveObjects(e.ObjectsToRemove);
        this.BuildList();
        this.UnsubscribeNotifications((IEnumerable) e.ObjectsToRemove);
      }
      finally
      {
        this.EndUpdate();
      }
    }

    public override void SelectObject(object modelObject, bool setFocus)
    {
      if (this.VirtualListDataSource == null)
        return;
      int objectIndex = this.VirtualListDataSource.GetObjectIndex(modelObject);
      if (objectIndex < 0 || objectIndex >= this.VirtualListSize || this.SelectedIndices.Count == 1 && this.SelectedIndices[0] == objectIndex)
        return;
      this.SelectedIndices.Clear();
      this.SelectedIndices.Add(objectIndex);
      if (!setFocus)
        return;
      this.SelectedItem.Focused = true;
    }

    public override void SelectObjects(IList modelObjects)
    {
      if (this.VirtualListDataSource == null)
        return;
      this.SelectedIndices.Clear();
      if (modelObjects == null)
        return;
      foreach (object model in (IEnumerable) modelObjects)
      {
        int objectIndex = this.VirtualListDataSource.GetObjectIndex(model);
        if (objectIndex >= 0 && objectIndex < this.VirtualListSize)
          this.SelectedIndices.Add(objectIndex);
      }
    }

    public override void SetObjects(IEnumerable collection, bool preserveState)
    {

      {
        if (this.VirtualListDataSource == null)
          return;
        ItemsChangingEventArgs e = new ItemsChangingEventArgs((IEnumerable) null, collection);
        this.OnItemsChanging(e);
        if (e.Canceled)
          return;
        this.BeginUpdate();
        try
        {
          this.VirtualListDataSource.SetObjects(e.NewObjects);
          this.BuildList();
          this.UpdateNotificationSubscriptions(e.NewObjects);
        }
        finally
        {
          this.EndUpdate();
        }
      }
    }

    protected override CheckState? GetCheckState(object modelObject)
    {
      if (this.CheckStateGetter != null)
        return base.GetCheckState(modelObject);
      CheckState checkState;
      if (modelObject != null && this.CheckStateMap.TryGetValue(modelObject, out checkState))
        return new CheckState?(checkState);
      return new CheckState?(CheckState.Unchecked);
    }

    public override void BuildList(bool shouldPreserveSelection)
    {
      this.UpdateVirtualListSize();
      this.ClearCachedInfo();
      if (this.ShowGroups)
        this.BuildGroups();
      else
        this.Sort();
      this.Invalidate();
    }

    public override void ClearCachedInfo()
    {
      this.lastRetrieveVirtualItemIndex = -1;
    }

    protected override void CreateGroups(IEnumerable<OLVGroup> groups)
    {
      NativeMethods.ClearGroups(this);
      this.EnableVirtualGroups();
      foreach (OLVGroup olvGroup in groups)
        olvGroup.InsertGroupNewStyle((ObjectListView) this);
    }

    protected void DisableVirtualGroups()
    {
      NativeMethods.ClearGroups(this);
      NativeMethods.SendMessage(this.Handle, 4253, 0, 0);
      NativeMethods.SendMessage(this.Handle, 4283, 0, 0);
    }

    protected void EnableVirtualGroups()
    {
      if (this.ownerDataCallbackImpl == null)
        this.ownerDataCallbackImpl = new OwnerDataCallbackImpl(this);
      IntPtr interfaceForObject = Marshal.GetComInterfaceForObject((object) this.ownerDataCallbackImpl, typeof (IOwnerDataCallback));
      NativeMethods.SendMessage(this.Handle, 4283, interfaceForObject, 0);
      Marshal.Release(interfaceForObject);
      NativeMethods.SendMessage(this.Handle, 4253, 1, 0);
    }

    public override int GetDisplayOrderOfItemIndex(int itemIndex)
    {
      if (!this.ShowGroups)
        return itemIndex;
      int group = this.GroupingStrategy.GetGroup(itemIndex);
      int num = 0;
      for (int index = 0; index < group - 1; ++index)
        num += this.OLVGroups[index].VirtualItemCount;
      return num + this.GroupingStrategy.GetIndexWithinGroup(this.OLVGroups[group], itemIndex);
    }

    public override OLVListItem GetLastItemInDisplayOrder()
    {
      if (!this.ShowGroups)
        return base.GetLastItemInDisplayOrder();
      if (this.OLVGroups.Count > 0)
      {
        OLVGroup group = this.OLVGroups[this.OLVGroups.Count - 1];
        if (group.VirtualItemCount > 0)
          return this.GetItem(this.GroupingStrategy.GetGroupMember(group, group.VirtualItemCount - 1));
      }
      return (OLVListItem) null;
    }

    public override OLVListItem GetNthItemInDisplayOrder(int n)
    {
      if (!this.ShowGroups || this.OLVGroups == null || this.OLVGroups.Count == 0)
        return this.GetItem(n);
      foreach (OLVGroup group in (IEnumerable<OLVGroup>) this.OLVGroups)
      {
        if (n < group.VirtualItemCount)
          return this.GetItem(this.GroupingStrategy.GetGroupMember(group, n));
        n -= group.VirtualItemCount;
      }
      return (OLVListItem) null;
    }

    public override OLVListItem GetNextItem(OLVListItem itemToFind)
    {
      if (!this.ShowGroups)
        return base.GetNextItem(itemToFind);
      if (this.OLVGroups == null || this.OLVGroups.Count == 0)
        return (OLVListItem) null;
      if (itemToFind == null)
        return this.GetItem(this.GroupingStrategy.GetGroupMember(this.OLVGroups[0], 0));
      int group = this.GroupingStrategy.GetGroup(itemToFind.Index);
      int indexWithinGroup = this.GroupingStrategy.GetIndexWithinGroup(this.OLVGroups[group], itemToFind.Index);
      if (indexWithinGroup < this.OLVGroups[group].VirtualItemCount - 1)
        return this.GetItem(this.GroupingStrategy.GetGroupMember(this.OLVGroups[group], indexWithinGroup + 1));
      if (group < this.OLVGroups.Count - 1)
        return this.GetItem(this.GroupingStrategy.GetGroupMember(this.OLVGroups[group + 1], 0));
      return (OLVListItem) null;
    }

    public override OLVListItem GetPreviousItem(OLVListItem itemToFind)
    {
      if (!this.ShowGroups)
        return base.GetPreviousItem(itemToFind);
      if (this.OLVGroups == null || this.OLVGroups.Count == 0)
        return (OLVListItem) null;
      if (itemToFind == null)
      {
        OLVGroup group = this.OLVGroups[this.OLVGroups.Count - 1];
        return this.GetItem(this.GroupingStrategy.GetGroupMember(group, group.VirtualItemCount - 1));
      }
      int group1 = this.GroupingStrategy.GetGroup(itemToFind.Index);
      int indexWithinGroup = this.GroupingStrategy.GetIndexWithinGroup(this.OLVGroups[group1], itemToFind.Index);
      if (indexWithinGroup > 0)
        return this.GetItem(this.GroupingStrategy.GetGroupMember(this.OLVGroups[group1], indexWithinGroup - 1));
      if (group1 <= 0)
        return (OLVListItem) null;
      OLVGroup group2 = this.OLVGroups[group1 - 1];
      return this.GetItem(this.GroupingStrategy.GetGroupMember(group2, group2.VirtualItemCount - 1));
    }

    protected override IList<OLVGroup> MakeGroups(GroupingParameters parms)
    {
      if (this.GroupingStrategy == null)
        return (IList<OLVGroup>) new List<OLVGroup>();
      return this.GroupingStrategy.GetGroups(parms);
    }

    public virtual OLVListItem MakeListViewItem(int itemIndex)
    {
      OLVListItem olvListItem = new OLVListItem(this.GetModelObject(itemIndex));
      this.FillInValues(olvListItem, olvListItem.RowObject);
      this.PostProcessOneRow(itemIndex, this.GetDisplayOrderOfItemIndex(itemIndex), olvListItem);
      if (this.HotRowIndex == itemIndex)
        this.UpdateHotRow(olvListItem);
      return olvListItem;
    }

    protected override void PostProcessRows()
    {
    }

    protected override CheckState PutCheckState(object modelObject, CheckState state)
    {
      state = base.PutCheckState(modelObject, state);
      this.CheckStateMap[modelObject] = state;
      return state;
    }

    public override void RefreshItem(OLVListItem olvi)
    {
      this.ClearCachedInfo();
      this.RedrawItems(olvi.Index, olvi.Index, true);
    }

    protected virtual void SetVirtualListSize(int newSize)
    {
      if (newSize < 0 || this.VirtualListSize == newSize)
        return;
      int virtualListSize = this.VirtualListSize;
      this.ClearCachedInfo();
      try
      {
        if (newSize == 0)
        {
          if (this.TopItemIndex > 0)
            this.TopItemIndex = 0;
        }
      }
      catch (Exception ex)
      {
      }
      try
      {
        this.VirtualListSize = newSize;
      }
      catch (ArgumentOutOfRangeException ex)
      {
      }
      catch (NullReferenceException ex)
      {
      }
      this.OnItemsChanged(new ItemsChangedEventArgs(virtualListSize, this.VirtualListSize));
    }

    protected override void TakeOwnershipOfObjects()
    {
    }

    protected override void UpdateFiltering()
    {
      IFilterableDataSource filterableDataSource = this.VirtualListDataSource as IFilterableDataSource;
      if (filterableDataSource == null)
        return;
      this.BeginUpdate();
      try
      {
        int virtualListSize = this.VirtualListSize;
        filterableDataSource.ApplyFilters(this.ModelFilter, this.ListFilter);
        this.BuildList();
      }
      finally
      {
        this.EndUpdate();
      }
    }

    public virtual void UpdateVirtualListSize()
    {
      if (this.VirtualListDataSource == null)
        return;
      this.SetVirtualListSize(this.VirtualListDataSource.GetObjectCount());
    }

    protected virtual void HandleCacheVirtualItems(object sender, CacheVirtualItemsEventArgs e)
    {
      if (this.VirtualListDataSource == null)
        return;
      this.VirtualListDataSource.PrepareCache(e.StartIndex, e.EndIndex);
    }

    protected virtual void HandleRetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
    {
      if (this.lastRetrieveVirtualItemIndex != e.ItemIndex)
      {
        this.lastRetrieveVirtualItemIndex = e.ItemIndex;
        this.lastRetrieveVirtualItem = this.MakeListViewItem(e.ItemIndex);
      }
      e.Item = (ListViewItem) this.lastRetrieveVirtualItem;
    }

    protected virtual void HandleSearchForVirtualItem(object sender, SearchForVirtualItemEventArgs e)
    {
      if (this.VirtualListDataSource == null)
        return;
      int startSearchFrom = Math.Min(e.StartIndex, this.VirtualListDataSource.GetObjectCount() - 1);
      BeforeSearchingEventArgs e1 = new BeforeSearchingEventArgs(e.Text, startSearchFrom);
      this.OnBeforeSearching(e1);
      if (e1.Canceled)
        return;
      int matchingRow = this.FindMatchingRow(e1.StringToFind, e1.StartSearchFrom, e.Direction);
      this.OnAfterSearching(new AfterSearchingEventArgs(e1.StringToFind, matchingRow));
      if (matchingRow == -1)
        return;
      e.Index = matchingRow;
    }

    protected override int FindMatchInRange(string text, int first, int last, OLVColumn column)
    {
      return this.VirtualListDataSource.SearchText(text, first, last, column);
    }
  }
}
