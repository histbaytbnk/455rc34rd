
// Type: BrightIdeasSoftware.Cluster


// Hacked by SystemAce

using System;

namespace BrightIdeasSoftware
{
  public class Cluster : ICluster, IComparable
  {
    private int count;
    private string displayLabel;
    private object clusterKey;

    public int Count
    {
      get
      {
        return this.count;
      }
      set
      {
        this.count = value;
      }
    }

    public string DisplayLabel
    {
      get
      {
        return this.displayLabel;
      }
      set
      {
        this.displayLabel = value;
      }
    }

    public object ClusterKey
    {
      get
      {
        return this.clusterKey;
      }
      set
      {
        this.clusterKey = value;
      }
    }

    public Cluster(object key)
    {
      this.Count = 1;
      this.ClusterKey = key;
    }

    public override string ToString()
    {
      return this.DisplayLabel ?? "[empty]";
    }

    public int CompareTo(object other)
    {
      if (other == null || other == DBNull.Value)
        return 1;
      ICluster cluster = other as ICluster;
      if (cluster == null)
        return 1;
      string strA = this.ClusterKey as string;
      if (strA != null)
        return string.Compare(strA, cluster.ClusterKey as string, StringComparison.CurrentCultureIgnoreCase);
      IComparable comparable = this.ClusterKey as IComparable;
      if (comparable != null)
        return comparable.CompareTo(cluster.ClusterKey);
      return -1;
    }
  }
}
