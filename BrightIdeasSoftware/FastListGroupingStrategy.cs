
// Type: BrightIdeasSoftware.FastListGroupingStrategy


// Hacked by SystemAce

using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class FastListGroupingStrategy : AbstractVirtualGroups
  {
    private List<int> indexToGroupMap;

    public override IList<OLVGroup> GetGroups(GroupingParameters parmameters)
    {
      FastObjectListView folv = (FastObjectListView) parmameters.ListView;
      int capacity = 0;
      NullableDictionary<object, List<object>> nullableDictionary = new NullableDictionary<object, List<object>>();
      foreach (object rowObject in folv.FilteredObjects)
      {
        object groupKey = parmameters.GroupByColumn.GetGroupKey(rowObject);
        if (!nullableDictionary.ContainsKey(groupKey))
          nullableDictionary[groupKey] = new List<object>();
        nullableDictionary[groupKey].Add(rowObject);
        ++capacity;
      }
      ModelObjectComparer modelObjectComparer = new ModelObjectComparer(parmameters.SortItemsByPrimaryColumn ? parmameters.ListView.GetColumn(0) : parmameters.PrimarySort, parmameters.PrimarySortOrder, parmameters.SecondarySort, parmameters.SecondarySortOrder);
      foreach (object index in (IEnumerable) nullableDictionary.Keys)
        nullableDictionary[index].Sort((IComparer<object>) modelObjectComparer);
      List<OLVGroup> list = new List<OLVGroup>();
      foreach (object index in (IEnumerable) nullableDictionary.Keys)
      {
        string header = parmameters.GroupByColumn.ConvertGroupKeyToTitle(index);
        if (!string.IsNullOrEmpty(parmameters.TitleFormat))
        {
          int count = nullableDictionary[index].Count;
          string format = count == 1 ? parmameters.TitleSingularFormat : parmameters.TitleFormat;
          try
          {
            header = string.Format(format, (object) header, (object) count);
          }
          catch (FormatException ex)
          {
            header = "Invalid group format: " + format;
          }
        }
        OLVGroup group = new OLVGroup(header);
        group.Collapsible = folv.HasCollapsibleGroups;
        group.Key = index;
        group.SortValue = index as IComparable;
        group.Contents = (IList) nullableDictionary[index].ConvertAll<int>((Converter<object, int>) (x => folv.IndexOf(x)));
        group.VirtualItemCount = nullableDictionary[index].Count;
        if (parmameters.GroupByColumn.GroupFormatter != null)
          parmameters.GroupByColumn.GroupFormatter(group, parmameters);
        list.Add(group);
      }
      if (parmameters.GroupByOrder != SortOrder.None)
        list.Sort(parmameters.GroupComparer ?? (IComparer<OLVGroup>) new OLVGroupComparer(parmameters.GroupByOrder));
      this.indexToGroupMap = new List<int>(capacity);
      this.indexToGroupMap.AddRange((IEnumerable<int>) new int[capacity]);
      for (int index1 = 0; index1 < list.Count; ++index1)
      {
        foreach (int index2 in (List<int>) list[index1].Contents)
          this.indexToGroupMap[index2] = index1;
      }
      return (IList<OLVGroup>) list;
    }

    public override int GetGroupMember(OLVGroup group, int indexWithinGroup)
    {
      return (int) group.Contents[indexWithinGroup];
    }

    public override int GetGroup(int itemIndex)
    {
      return this.indexToGroupMap[itemIndex];
    }

    public override int GetIndexWithinGroup(OLVGroup group, int itemIndex)
    {
      return group.Contents.IndexOf((object) itemIndex);
    }
  }
}
