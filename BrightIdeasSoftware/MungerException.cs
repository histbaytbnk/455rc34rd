
// Type: BrightIdeasSoftware.MungerException


// Hacked by SystemAce

using System;

namespace BrightIdeasSoftware
{
  public class MungerException : ApplicationException
  {
    private readonly SimpleMunger munger;
    private readonly object target;

    public SimpleMunger Munger
    {
      get
      {
        return this.munger;
      }
    }

    public object Target
    {
      get
      {
        return this.target;
      }
    }

    public MungerException(SimpleMunger munger, object target, Exception ex)
      : base("Munger failed", ex)
    {
      this.munger = munger;
      this.target = target;
    }
  }
}
