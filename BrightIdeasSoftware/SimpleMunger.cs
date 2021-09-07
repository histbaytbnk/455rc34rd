
// Type: BrightIdeasSoftware.SimpleMunger


// Hacked by SystemAce

using System;
using System.Reflection;

namespace BrightIdeasSoftware
{
  public class SimpleMunger
  {
    private readonly string aspectName;
    private Type cachedTargetType;
    private string cachedName;
    private int cachedNumberParameters;
    private FieldInfo resolvedFieldInfo;
    private PropertyInfo resolvedPropertyInfo;
    private MethodInfo resolvedMethodInfo;
    private PropertyInfo indexerPropertyInfo;

    public string AspectName
    {
      get
      {
        return this.aspectName;
      }
    }

    public SimpleMunger(string aspectName)
    {
      this.aspectName = aspectName;
    }

    public object GetValue(object target)
    {
      if (target == null)
        return (object) null;
      this.ResolveName(target, this.AspectName, 0);
      try
      {
        if (this.resolvedPropertyInfo != (PropertyInfo) null)
          return this.resolvedPropertyInfo.GetValue(target, (object[]) null);
        if (this.resolvedMethodInfo != (MethodInfo) null)
          return this.resolvedMethodInfo.Invoke(target, (object[]) null);
        if (this.resolvedFieldInfo != (FieldInfo) null)
          return this.resolvedFieldInfo.GetValue(target);
        if (this.indexerPropertyInfo != (PropertyInfo) null)
          return this.indexerPropertyInfo.GetValue(target, new object[1]
          {
            (object) this.AspectName
          });
      }
      catch (Exception ex)
      {
        throw new MungerException(this, target, ex);
      }
      throw new MungerException(this, target, (Exception) new MissingMethodException());
    }

    public bool PutValue(object target, object value)
    {
      if (target == null)
        return false;
      this.ResolveName(target, this.AspectName, 1);
      try
      {
        if (this.resolvedPropertyInfo != (PropertyInfo) null)
        {
          this.resolvedPropertyInfo.SetValue(target, value, (object[]) null);
          return true;
        }
        if (this.resolvedMethodInfo != (MethodInfo) null)
        {
          this.resolvedMethodInfo.Invoke(target, new object[1]
          {
            value
          });
          return true;
        }
        if (this.resolvedFieldInfo != (FieldInfo) null)
        {
          this.resolvedFieldInfo.SetValue(target, value);
          return true;
        }
        if (this.indexerPropertyInfo != (PropertyInfo) null)
        {
          this.indexerPropertyInfo.SetValue(target, value, new object[1]
          {
            (object) this.AspectName
          });
          return true;
        }
      }
      catch (Exception ex)
      {
        throw new MungerException(this, target, ex);
      }
      return false;
    }

    private void ResolveName(object target, string name, int numberMethodParameters)
    {
      if (this.cachedTargetType == target.GetType() && this.cachedName == name && this.cachedNumberParameters == numberMethodParameters)
        return;
      this.cachedTargetType = target.GetType();
      this.cachedName = name;
      this.cachedNumberParameters = numberMethodParameters;
      this.resolvedFieldInfo = (FieldInfo) null;
      this.resolvedPropertyInfo = (PropertyInfo) null;
      this.resolvedMethodInfo = (MethodInfo) null;
      this.indexerPropertyInfo = (PropertyInfo) null;
      foreach (PropertyInfo propertyInfo in target.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
      {
        if (propertyInfo.Name == name)
        {
          this.resolvedPropertyInfo = propertyInfo;
          return;
        }
        if (this.indexerPropertyInfo == (PropertyInfo) null && propertyInfo.Name == "Item")
        {
          ParameterInfo[] parameters = propertyInfo.GetGetMethod().GetParameters();
          if (parameters.Length > 0)
          {
            Type parameterType = parameters[0].ParameterType;
            if (parameterType == typeof (string) || parameterType == typeof (object))
              this.indexerPropertyInfo = propertyInfo;
          }
        }
      }
      foreach (FieldInfo fieldInfo in target.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public))
      {
        if (fieldInfo.Name == name)
        {
          this.resolvedFieldInfo = fieldInfo;
          return;
        }
      }
      foreach (MethodInfo methodInfo in target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public))
      {
        if (methodInfo.Name == name && methodInfo.GetParameters().Length == numberMethodParameters)
        {
          this.resolvedMethodInfo = methodInfo;
          break;
        }
      }
    }
  }
}
