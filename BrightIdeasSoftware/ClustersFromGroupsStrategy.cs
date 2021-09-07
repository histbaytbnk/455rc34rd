
// Type: BrightIdeasSoftware.ClustersFromGroupsStrategy


// Hacked by SystemAce

namespace BrightIdeasSoftware
{
  public class ClustersFromGroupsStrategy : ClusteringStrategy
  {
    public override object GetClusterKey(object model)
    {
      return this.Column.GetGroupKey(model);
    }

    public override string GetClusterDisplayLabel(ICluster cluster)
    {
      string s = this.Column.ConvertGroupKeyToTitle(cluster.ClusterKey);
      if (string.IsNullOrEmpty(s))
        s = ClusteringStrategy.EMPTY_LABEL;
      return this.ApplyDisplayFormat(cluster, s);
    }
  }
}
