
// Type: BrightIdeasSoftware.TypedObjectListView`1


// Hacked by SystemAce

using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class TypedObjectListView<T> where T : class
  {
    private ObjectListView olv;

    public virtual T CheckedObject
    {
      get
      {
        return (T) this.olv.CheckedObject;
      }
    }

    public virtual IList<T> CheckedObjects
    {
      get
      {
        IList checkedObjects = this.olv.CheckedObjects;
        List<T> list = new List<T>(checkedObjects.Count);
        foreach (object obj in (IEnumerable) checkedObjects)
          list.Add((T) obj);
        return (IList<T>) list;
      }
      set
      {
        this.olv.CheckedObjects = (IList) value;
      }
    }

    public virtual ObjectListView ListView
    {
      get
      {
        return this.olv;
      }
      set
      {
        this.olv = value;
      }
    }

    public virtual IList<T> Objects
    {
      get
      {
        List<T> list = new List<T>(this.olv.GetItemCount());
        for (int index = 0; index < this.olv.GetItemCount(); ++index)
          list.Add(this.GetModelObject(index));
        return (IList<T>) list;
      }
      set
      {
        this.olv.SetObjects((IEnumerable) value);
      }
    }

    public virtual T SelectedObject
    {
      get
      {
        return (T) this.olv.SelectedObject;
      }
      set
      {
        this.olv.SelectedObject = (object) value;
      }
    }

    public virtual IList<T> SelectedObjects
    {
      get
      {
        List<T> list = new List<T>(this.olv.SelectedIndices.Count);
        foreach (int index in this.olv.SelectedIndices)
          list.Add((T) this.olv.GetModelObject(index));
        return (IList<T>) list;
      }
      set
      {
        this.olv.SelectedObjects = (IList) value;
      }
    }

  
    public virtual HeaderToolTipGetterDelegate HeaderToolTipGetter
    {
      get
      {
        return this.olv.HeaderToolTipGetter;
      }
      set
      {
        this.olv.HeaderToolTipGetter = value;
      }
    }

    public TypedObjectListView(ObjectListView olv)
    {
      this.olv = olv;
    }

    public virtual TypedColumn<T> GetColumn(int i)
    {
      return new TypedColumn<T>(this.olv.GetColumn(i));
    }

    public virtual TypedColumn<T> GetColumn(string name)
    {
      return new TypedColumn<T>(this.olv.GetColumn(name));
    }

    public virtual T GetModelObject(int index)
    {
      return (T) this.olv.GetModelObject(index);
    }

    public virtual void GenerateAspectGetters()
    {
      for (int i = 0; i < this.ListView.Columns.Count; ++i)
        this.GetColumn(i).GenerateAspectGetter();
    }
        
        
  }
}
