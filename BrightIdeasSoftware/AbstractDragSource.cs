
// Type: BrightIdeasSoftware.AbstractDragSource


// Hacked by SystemAce

using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class AbstractDragSource : IDragSource
  {
    public virtual object StartDrag(ObjectListView olv, MouseButtons button, OLVListItem item)
    {
      return (object) null;
    }

    public virtual DragDropEffects GetAllowedEffects(object data)
    {
      return DragDropEffects.None;
    }

    public virtual void EndDrag(object dragObject, DragDropEffects effect)
    {
    }
  }
}
