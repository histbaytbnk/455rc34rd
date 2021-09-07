
// Type: BrightIdeasSoftware.EditorRegistry


// Hacked by SystemAce

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class EditorRegistry
  {
    private Dictionary<Type, EditorCreatorDelegate> creatorMap = new Dictionary<Type, EditorCreatorDelegate>();
    private EditorCreatorDelegate firstChanceCreator;
    private EditorCreatorDelegate defaultCreator;

    public EditorRegistry()
    {
      this.InitializeStandardTypes();
    }

    private void InitializeStandardTypes()
    {
      this.Register(typeof (bool), typeof (BooleanCellEditor));
      this.Register(typeof (short), typeof (IntUpDown));
      this.Register(typeof (int), typeof (IntUpDown));
      this.Register(typeof (long), typeof (IntUpDown));
      this.Register(typeof (ushort), typeof (UintUpDown));
      this.Register(typeof (uint), typeof (UintUpDown));
      this.Register(typeof (ulong), typeof (UintUpDown));
      this.Register(typeof (float), typeof (FloatCellEditor));
      this.Register(typeof (double), typeof (FloatCellEditor));
      this.Register(typeof (DateTime), (EditorCreatorDelegate) ((model, column, value) => (Control) new DateTimePicker()
      {
        Format = DateTimePickerFormat.Short
      }));
      this.Register(typeof (bool), (EditorCreatorDelegate) ((model, column, value) =>
      {
        CheckBox checkBox = (CheckBox) new BooleanCellEditor2();
        checkBox.ThreeState = column.TriStateCheckBoxes;
        return (Control) checkBox;
      }));
    }

    public void Register(Type type, Type controlType)
    {
      this.Register(type, (EditorCreatorDelegate) ((model, column, value) => controlType.InvokeMember("", BindingFlags.CreateInstance, (Binder) null, (object) null, (object[]) null) as Control));
    }

    public void Register(Type type, EditorCreatorDelegate creator)
    {
      this.creatorMap[type] = creator;
    }

    public void RegisterDefault(EditorCreatorDelegate creator)
    {
      this.defaultCreator = creator;
    }

    public void RegisterFirstChance(EditorCreatorDelegate creator)
    {
      this.firstChanceCreator = creator;
    }

    public Control GetEditor(object model, OLVColumn column, object value)
    {
      if (this.firstChanceCreator != null)
      {
        Control control = this.firstChanceCreator(model, column, value);
        if (control != null)
          return control;
      }
      Type key = value == null ? column.DataType : value.GetType();
      if (key != (Type) null && this.creatorMap.ContainsKey(key))
      {
        Control control = this.creatorMap[key](model, column, value);
        if (control != null)
          return control;
      }
      if (value != null && value.GetType().IsEnum)
        return this.CreateEnumEditor(value.GetType());
      if (this.defaultCreator != null)
        return this.defaultCreator(model, column, value);
      return (Control) null;
    }

    protected Control CreateEnumEditor(Type type)
    {
      return (Control) new EnumCellEditor(type);
    }
  }
}
