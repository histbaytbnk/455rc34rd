
// Type: BrightIdeasSoftware.SimpleDragSource


// Hacked by SystemAce

using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class SimpleDragSource : IDragSource
  {
    private bool refreshAfterDrop;

    public bool RefreshAfterDrop
    {
      get
      {
        return this.refreshAfterDrop;
      }
      set
      {
        this.refreshAfterDrop = value;
      }
    }

    public SimpleDragSource()
    {
    }

    public SimpleDragSource(bool refreshAfterDrop)
    {
      this.RefreshAfterDrop = refreshAfterDrop;
    }

    public virtual object StartDrag(ObjectListView olv, MouseButtons button, OLVListItem item)
    {
      if (button != MouseButtons.Left)
        return (object) null;
      return this.CreateDataObject(olv);
    }

    public virtual DragDropEffects GetAllowedEffects(object data)
    {
      return DragDropEffects.All | DragDropEffects.Link;
    }

    public virtual void EndDrag(object dragObject, DragDropEffects effect)
    {
      OLVDataObject olvDataObject = dragObject as OLVDataObject;
      if (olvDataObject == null || !this.RefreshAfterDrop)
        return;
      olvDataObject.ListView.RefreshObjects(olvDataObject.ModelObjects);
    }

    protected virtual object CreateDataObject(ObjectListView olv)
    {
      OLVDataObject olvDataObject = new OLVDataObject(olv);
      olvDataObject.CreateTextFormats();
      return (object) olvDataObject;
    }
  }
}
