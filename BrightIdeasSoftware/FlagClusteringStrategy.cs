
// Type: BrightIdeasSoftware.FlagClusteringStrategy


// Hacked by SystemAce

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace BrightIdeasSoftware
{
  public class FlagClusteringStrategy : ClusteringStrategy
  {
    private long[] values;
    private string[] labels;

    public long[] Values
    {
      get
      {
        return this.values;
      }
      private set
      {
        this.values = value;
      }
    }

    public string[] Labels
    {
      get
      {
        return this.labels;
      }
      private set
      {
        this.labels = value;
      }
    }

    public FlagClusteringStrategy(Type enumType)
    {
      if (enumType == (Type) null)
        throw new ArgumentNullException("enumType");
      if (!enumType.IsEnum)
        throw new ArgumentException("Type must be enum", "enumType");
      if (enumType.GetCustomAttributes(typeof (FlagsAttribute), false) == null)
        throw new ArgumentException("Type must have [Flags] attribute", "enumType");
      List<long> list1 = new List<long>();
      foreach (object obj in Enum.GetValues(enumType))
        list1.Add(Convert.ToInt64(obj));
      List<string> list2 = new List<string>();
      foreach (string str in Enum.GetNames(enumType))
        list2.Add(str);
      this.SetValues(list1.ToArray(), list2.ToArray());
    }

    public FlagClusteringStrategy(long[] values, string[] labels)
    {
      this.SetValues(values, labels);
    }

    private void SetValues(long[] flags, string[] flagLabels)
    {
      if (flags == null || flags.Length == 0)
        throw new ArgumentNullException("flags");
      if (flagLabels == null || flagLabels.Length == 0)
        throw new ArgumentNullException("flagLabels");
      if (flags.Length != flagLabels.Length)
        throw new ArgumentException("values and labels must have the same number of entries", "flags");
      this.Values = flags;
      this.Labels = flagLabels;
    }

    public override object GetClusterKey(object model)
    {
      List<long> list = new List<long>();
      try
      {
        long num1 = Convert.ToInt64(this.Column.GetValue(model));
        foreach (long num2 in this.Values)
        {
          if ((num2 & num1) == num2)
            list.Add(num2);
        }
        return (object) list;
      }
      catch (InvalidCastException ex)
      {
        return (object) list;
      }
      catch (FormatException ex)
      {
        return (object) list;
      }
    }

    public override string GetClusterDisplayLabel(ICluster cluster)
    {
      long num = Convert.ToInt64(cluster.ClusterKey);
      for (int index = 0; index < this.Values.Length; ++index)
      {
        if (num == this.Values[index])
          return this.ApplyDisplayFormat(cluster, this.Labels[index]);
      }
      return this.ApplyDisplayFormat(cluster, num.ToString((IFormatProvider) CultureInfo.CurrentUICulture));
    }

    public override IModelFilter CreateFilter(IList valuesChosenForFiltering)
    {
      return (IModelFilter) new FlagBitSetFilter(new AspectGetterDelegate(((ClusteringStrategy) this).GetClusterKey), (ICollection) valuesChosenForFiltering);
    }
  }
}
