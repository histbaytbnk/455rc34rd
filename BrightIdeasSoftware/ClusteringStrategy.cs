
// Type: BrightIdeasSoftware.ClusteringStrategy


// Hacked by SystemAce

using System.Collections;

namespace BrightIdeasSoftware
{
  public class ClusteringStrategy : IClusteringStrategy
  {
    public static string NULL_LABEL = "[null]";
    public static string EMPTY_LABEL = "[empty]";
    private static string defaultDisplayLabelFormatSingular = "{0} ({1} item)";
    private static string defaultDisplayLabelFormatPural = "{0} ({1} items)";
    private OLVColumn column;
    private string displayLabelFormatSingular;
    private string displayLabelFormatPural;

    public static string DefaultDisplayLabelFormatSingular
    {
      get
      {
        return ClusteringStrategy.defaultDisplayLabelFormatSingular;
      }
      set
      {
        ClusteringStrategy.defaultDisplayLabelFormatSingular = value;
      }
    }

    public static string DefaultDisplayLabelFormatPlural
    {
      get
      {
        return ClusteringStrategy.defaultDisplayLabelFormatPural;
      }
      set
      {
        ClusteringStrategy.defaultDisplayLabelFormatPural = value;
      }
    }

    public OLVColumn Column
    {
      get
      {
        return this.column;
      }
      set
      {
        this.column = value;
      }
    }

    public string DisplayLabelFormatSingular
    {
      get
      {
        return this.displayLabelFormatSingular;
      }
      set
      {
        this.displayLabelFormatSingular = value;
      }
    }

    public string DisplayLabelFormatPlural
    {
      get
      {
        return this.displayLabelFormatPural;
      }
      set
      {
        this.displayLabelFormatPural = value;
      }
    }

    public ClusteringStrategy()
    {
      this.DisplayLabelFormatSingular = ClusteringStrategy.DefaultDisplayLabelFormatSingular;
      this.DisplayLabelFormatPlural = ClusteringStrategy.DefaultDisplayLabelFormatPlural;
    }

    public virtual object GetClusterKey(object model)
    {
      return this.Column.GetValue(model);
    }

    public virtual ICluster CreateCluster(object clusterKey)
    {
      return (ICluster) new Cluster(clusterKey);
    }

    public virtual string GetClusterDisplayLabel(ICluster cluster)
    {
      string s = this.Column.ValueToString(cluster.ClusterKey) ?? ClusteringStrategy.NULL_LABEL;
      if (string.IsNullOrEmpty(s))
        s = ClusteringStrategy.EMPTY_LABEL;
      return this.ApplyDisplayFormat(cluster, s);
    }

    public virtual IModelFilter CreateFilter(IList valuesChosenForFiltering)
    {
      return (IModelFilter) new OneOfFilter(new AspectGetterDelegate(this.GetClusterKey), (ICollection) valuesChosenForFiltering);
    }

    protected virtual string ApplyDisplayFormat(ICluster cluster, string s)
    {
      string format = cluster.Count == 1 ? this.DisplayLabelFormatSingular : this.DisplayLabelFormatPlural;
      if (!string.IsNullOrEmpty(format))
        return string.Format(format, (object) s, (object) cluster.Count);
      return s;
    }
  }
}
