
// Type: BrightIdeasSoftware.TypedColumn`1


// Hacked by SystemAce

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace BrightIdeasSoftware
{
  public class TypedColumn<T> where T : class
  {
    private OLVColumn column;

    

    public TypedColumn(OLVColumn column)
    {
      this.column = column;
    }

    public void GenerateAspectGetter()
    {
      if (string.IsNullOrEmpty(this.column.AspectName))
        return;
      
    }

  
    private void GenerateIL(Type type, string path, ILGenerator il)
    {
      il.Emit(OpCodes.Ldarg_0);
      string[] strArray = path.Split('.');
      for (int index = 0; index < strArray.Length; ++index)
      {
        type = this.GeneratePart(il, type, strArray[index], index == strArray.Length - 1);
        if (type == (Type) null)
          break;
      }
      if (type != (Type) null && type.IsValueType && !typeof (T).IsValueType)
        il.Emit(OpCodes.Box, type);
      il.Emit(OpCodes.Ret);
    }

    private Type GeneratePart(ILGenerator il, Type type, string pathPart, bool isLastPart)
    {
      MemberInfo memberInfo = new List<MemberInfo>((IEnumerable<MemberInfo>) type.GetMember(pathPart)).Find((Predicate<MemberInfo>) (x =>
      {
        if (x.MemberType == MemberTypes.Field || x.MemberType == MemberTypes.Property)
          return true;
        if (x.MemberType == MemberTypes.Method)
          return ((MethodBase) x).GetParameters().Length == 0;
        return false;
      }));
      if (memberInfo == (MemberInfo) null)
      {
        il.Emit(OpCodes.Pop);
        if (Munger.IgnoreMissingAspects)
          il.Emit(OpCodes.Ldnull);
        else
          il.Emit(OpCodes.Ldstr, string.Format("'{0}' is not a parameter-less method, property or field of type '{1}'", (object) pathPart, (object) type.FullName));
        return (Type) null;
      }
      Type localType = (Type) null;
      switch (memberInfo.MemberType)
      {
        case MemberTypes.Field:
          FieldInfo field = (FieldInfo) memberInfo;
          il.Emit(OpCodes.Ldfld, field);
          localType = field.FieldType;
          break;
        case MemberTypes.Method:
          MethodInfo meth = (MethodInfo) memberInfo;
          if (meth.IsVirtual)
            il.Emit(OpCodes.Callvirt, meth);
          else
            il.Emit(OpCodes.Call, meth);
          localType = meth.ReturnType;
          break;
        case MemberTypes.Property:
          PropertyInfo propertyInfo = (PropertyInfo) memberInfo;
          il.Emit(OpCodes.Call, propertyInfo.GetGetMethod());
          localType = propertyInfo.PropertyType;
          break;
      }
      if (localType.IsValueType && !isLastPart)
      {
        LocalBuilder local = il.DeclareLocal(localType);
        il.Emit(OpCodes.Stloc, local);
        il.Emit(OpCodes.Ldloca, local);
      }
      return localType;
    }
        
  }
}
