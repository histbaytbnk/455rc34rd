
// Type: BrightIdeasSoftware.DateTimeClusteringStrategy


// Hacked by SystemAce

using System;
using System.Globalization;

namespace BrightIdeasSoftware
{
  public class DateTimeClusteringStrategy : ClusteringStrategy
  {
    private DateTimePortion portions = DateTimePortion.Year | DateTimePortion.Month;
    private string format;

    public string Format
    {
      get
      {
        return this.format;
      }
      set
      {
        this.format = value;
      }
    }

    public DateTimePortion Portions
    {
      get
      {
        return this.portions;
      }
      set
      {
        this.portions = value;
      }
    }

    public DateTimeClusteringStrategy()
      : this(DateTimePortion.Year | DateTimePortion.Month, "MMMM yyyy")
    {
    }

    public DateTimeClusteringStrategy(DateTimePortion portions, string format)
    {
      this.Portions = portions;
      this.Format = format;
    }

    public override object GetClusterKey(object model)
    {
      DateTime? nullable = this.Column.GetValue(model) as DateTime?;
      if (!nullable.HasValue)
        return (object) null;
      return (object) new DateTime((this.Portions & DateTimePortion.Year) == DateTimePortion.Year ? nullable.Value.Year : 1, (this.Portions & DateTimePortion.Month) == DateTimePortion.Month ? nullable.Value.Month : 1, (this.Portions & DateTimePortion.Day) == DateTimePortion.Day ? nullable.Value.Day : 1, (this.Portions & DateTimePortion.Hour) == DateTimePortion.Hour ? nullable.Value.Hour : 0, (this.Portions & DateTimePortion.Minute) == DateTimePortion.Minute ? nullable.Value.Minute : 0, (this.Portions & DateTimePortion.Second) == DateTimePortion.Second ? nullable.Value.Second : 0);
    }

    public override string GetClusterDisplayLabel(ICluster cluster)
    {
      DateTime? nullable = cluster.ClusterKey as DateTime?;
      return this.ApplyDisplayFormat(cluster, nullable.HasValue ? this.DateToString(nullable.Value) : ClusteringStrategy.NULL_LABEL);
    }

    protected virtual string DateToString(DateTime dateTime)
    {
      if (string.IsNullOrEmpty(this.Format))
        return dateTime.ToString((IFormatProvider) CultureInfo.CurrentUICulture);
      try
      {
        return dateTime.ToString(this.Format);
      }
      catch (FormatException ex)
      {
        return string.Format("Bad format string '{0}' for value '{1}'", (object) this.Format, (object) dateTime);
      }
    }
  }
}
