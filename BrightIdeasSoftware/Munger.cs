
// Type: BrightIdeasSoftware.Munger


// Hacked by SystemAce

using System.Collections.Generic;

namespace BrightIdeasSoftware
{
  public class Munger
  {
    private static bool ignoreMissingAspects = true;
    private string aspectName;
    private IList<SimpleMunger> aspectParts;

    public static bool IgnoreMissingAspects
    {
      get
      {
        return Munger.ignoreMissingAspects;
      }
      set
      {
        Munger.ignoreMissingAspects = value;
      }
    }

    public string AspectName
    {
      get
      {
        return this.aspectName;
      }
      set
      {
        this.aspectName = value;
        this.aspectParts = (IList<SimpleMunger>) null;
      }
    }

    private IList<SimpleMunger> Parts
    {
      get
      {
        if (this.aspectParts == null)
          this.aspectParts = this.BuildParts(this.AspectName);
        return this.aspectParts;
      }
    }

    public Munger()
    {
    }

    public Munger(string aspectName)
    {
      this.AspectName = aspectName;
    }

    public static bool PutProperty(object target, string propertyName, object value)
    {
      try
      {
        return new Munger(propertyName).PutValue(target, value);
      }
      catch (MungerException ex)
      {
      }
      return false;
    }

    public object GetValue(object target)
    {
      if (this.Parts.Count == 0)
        return (object) null;
      try
      {
        return this.EvaluateParts(target, this.Parts);
      }
      catch (MungerException ex)
      {
        if (Munger.IgnoreMissingAspects)
          return (object) null;
        return (object) string.Format("'{0}' is not a parameter-less method, property or field of type '{1}'", (object) ex.Munger.AspectName, (object) ex.Target.GetType());
      }
    }

    public object GetValueEx(object target)
    {
      if (this.Parts.Count == 0)
        return (object) null;
      return this.EvaluateParts(target, this.Parts);
    }

    public bool PutValue(object target, object value)
    {
      if (this.Parts.Count == 0)
        return false;
      SimpleMunger simpleMunger = this.Parts[this.Parts.Count - 1];
      if (this.Parts.Count > 1)
      {
        List<SimpleMunger> list = new List<SimpleMunger>((IEnumerable<SimpleMunger>) this.Parts);
        list.RemoveAt(list.Count - 1);
        try
        {
          target = this.EvaluateParts(target, (IList<SimpleMunger>) list);
        }
        catch (MungerException ex)
        {
          this.ReportPutValueException(ex);
          return false;
        }
      }
      if (target != null)
      {
        try
        {
          return simpleMunger.PutValue(target, value);
        }
        catch (MungerException ex)
        {
          this.ReportPutValueException(ex);
        }
      }
      return false;
    }

    private IList<SimpleMunger> BuildParts(string aspect)
    {
      List<SimpleMunger> list = new List<SimpleMunger>();
      if (!string.IsNullOrEmpty(aspect))
      {
        string str1 = aspect;
        char[] chArray = new char[1]
        {
          '.'
        };
        foreach (string str2 in str1.Split(chArray))
          list.Add(new SimpleMunger(str2.Trim()));
      }
      return (IList<SimpleMunger>) list;
    }

    private object EvaluateParts(object target, IList<SimpleMunger> parts)
    {
      foreach (SimpleMunger simpleMunger in (IEnumerable<SimpleMunger>) parts)
      {
        if (target != null)
          target = simpleMunger.GetValue(target);
        else
          break;
      }
      return target;
    }

    private void ReportPutValueException(MungerException ex)
    {
    }
  }
}
