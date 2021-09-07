
// Type: BrightIdeasSoftware.OLVListSubItem


// Hacked by SystemAce

using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  [Browsable(false)]
  public class OLVListSubItem : ListViewItem.ListViewSubItem
  {
    private Rectangle? cellPadding;
    private StringAlignment? cellVerticalAlignment;
    private object modelValue;
    private IList<IDecoration> decorations;
    private object imageSelector;
    private string url;
    internal ImageRenderer.AnimationState AnimationState;

    public Rectangle? CellPadding
    {
      get
      {
        return this.cellPadding;
      }
      set
      {
        this.cellPadding = value;
      }
    }

    public StringAlignment? CellVerticalAlignment
    {
      get
      {
        return this.cellVerticalAlignment;
      }
      set
      {
        this.cellVerticalAlignment = value;
      }
    }

    public object ModelValue
    {
      get
      {
        return this.modelValue;
      }
      private set
      {
        this.modelValue = value;
      }
    }

    public bool HasDecoration
    {
      get
      {
        if (this.decorations != null)
          return this.decorations.Count > 0;
        return false;
      }
    }

    public IDecoration Decoration
    {
      get
      {
        if (!this.HasDecoration)
          return (IDecoration) null;
        return this.Decorations[0];
      }
      set
      {
        this.Decorations.Clear();
        if (value == null)
          return;
        this.Decorations.Add(value);
      }
    }

    public IList<IDecoration> Decorations
    {
      get
      {
        if (this.decorations == null)
          this.decorations = (IList<IDecoration>) new List<IDecoration>();
        return this.decorations;
      }
    }

    public object ImageSelector
    {
      get
      {
        return this.imageSelector;
      }
      set
      {
        this.imageSelector = value;
      }
    }

    public string Url
    {
      get
      {
        return this.url;
      }
      set
      {
        this.url = value;
      }
    }

    public OLVListSubItem()
    {
    }

    public OLVListSubItem(object modelValue, string text, object image)
    {
      this.ModelValue = modelValue;
      this.Text = text;
      this.ImageSelector = image;
    }
  }
}
