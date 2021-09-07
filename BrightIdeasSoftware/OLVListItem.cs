
// Type: BrightIdeasSoftware.OLVListItem


// Hacked by SystemAce

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class OLVListItem : ListViewItem
  {
    private Rectangle? cellPadding;
    private StringAlignment? cellVerticalAlignment;
    private IList<IDecoration> decorations;
    private bool enabled;
    private object imageSelector;
    private object rowObject;

    public new Rectangle Bounds
    {
      get
      {
        try
        {
          return base.Bounds;
        }
        catch (ArgumentException ex)
        {
          return Rectangle.Empty;
        }
      }
    }

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

    public new bool Checked
    {
      get
      {
        return base.Checked;
      }
      set
      {
        if (this.Checked == value)
          return;
        if (value)
          ((ObjectListView) this.ListView).CheckObject(this.RowObject);
        else
          ((ObjectListView) this.ListView).UncheckObject(this.RowObject);
      }
    }

    public CheckState CheckState
    {
      get
      {
        switch (this.StateImageIndex)
        {
          case 0:
            return CheckState.Unchecked;
          case 1:
            return CheckState.Checked;
          case 2:
            return CheckState.Indeterminate;
          default:
            return CheckState.Unchecked;
        }
      }
      set
      {
        switch (value)
        {
          case CheckState.Unchecked:
            this.StateImageIndex = 0;
            break;
          case CheckState.Checked:
            this.StateImageIndex = 1;
            break;
          case CheckState.Indeterminate:
            this.StateImageIndex = 2;
            break;
        }
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
        if (this.HasDecoration)
          return this.Decorations[0];
        return (IDecoration) null;
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

    public bool Enabled
    {
      get
      {
        return this.enabled;
      }
      internal set
      {
        this.enabled = value;
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
        if (value is int)
          this.ImageIndex = (int) value;
        else if (value is string)
          this.ImageKey = (string) value;
        else
          this.ImageIndex = -1;
      }
    }

    public object RowObject
    {
      get
      {
        return this.rowObject;
      }
      set
      {
        this.rowObject = value;
      }
    }

    public OLVListItem(object rowObject)
    {
      this.rowObject = rowObject;
    }

    public OLVListItem(object rowObject, string text, object image)
      : base(text, -1)
    {
      this.rowObject = rowObject;
      this.imageSelector = image;
    }

    public virtual OLVListSubItem GetSubItem(int index)
    {
      if (index >= 0 && index < this.SubItems.Count)
        return (OLVListSubItem) this.SubItems[index];
      return (OLVListSubItem) null;
    }

    public virtual Rectangle GetSubItemBounds(int subItemIndex)
    {
      if (subItemIndex == 0)
      {
        Rectangle bounds = this.Bounds;
        Point scrolledColumnSides = NativeMethods.GetScrolledColumnSides(this.ListView, subItemIndex);
        bounds.X = scrolledColumnSides.X + 1;
        bounds.Width = scrolledColumnSides.Y - scrolledColumnSides.X;
        return bounds;
      }
      OLVListSubItem subItem = this.GetSubItem(subItemIndex);
      if (subItem != null)
        return subItem.Bounds;
      return new Rectangle();
    }
  }
}
