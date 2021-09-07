
// Type: BrightIdeasSoftware.BaseRenderer


// Hacked by SystemAce

using PS3SaveEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace BrightIdeasSoftware
{
  [ToolboxItem(true)]
  [Browsable(true)]
  public class BaseRenderer : AbstractRenderer
  {
    private int spacing = 1;
    private bool useGdiTextRendering = true;
    private bool canWrap;
    private Rectangle? cellPadding;
    private StringAlignment? cellVerticalAlignment;
    private ImageList imageList;
    private object aspect;
    private Rectangle bounds;
    private OLVColumn column;
    private DrawListViewItemEventArgs drawItemEventArgs;
    private DrawListViewSubItemEventArgs eventArgs;
    private Font font;
    private bool isItemSelected;
    private bool isPrinting;
    private OLVListItem listItem;
    private ObjectListView objectListView;
    private object rowObject;
    private OLVListSubItem listSubItem;
    private Brush textBrush;
    private bool useCustomCheckboxImages;

    [DefaultValue(false)]
    [Description("Can the renderer wrap text that does not fit completely within the cell")]
    [Category("Appearance")]
    public bool CanWrap
    {
      get
      {
        return this.canWrap;
      }
      set
      {
        this.canWrap = value;
        if (!this.canWrap)
          return;
        this.UseGdiTextRendering = false;
      }
    }

    [Category("ObjectListView")]
    [Description("The number of pixels that renderer will leave empty around the edge of the cell")]
    [DefaultValue(null)]
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

    [DefaultValue(null)]
    [Category("ObjectListView")]
    [Description("How will cell values be vertically aligned?")]
    public virtual StringAlignment? CellVerticalAlignment
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

    [Browsable(false)]
    protected virtual Rectangle? EffectiveCellPadding
    {
      get
      {
        if (this.cellPadding.HasValue)
          return new Rectangle?(this.cellPadding.Value);
        if (this.OLVSubItem != null && this.OLVSubItem.CellPadding.HasValue)
          return new Rectangle?(this.OLVSubItem.CellPadding.Value);
        if (this.ListItem != null && this.ListItem.CellPadding.HasValue)
          return new Rectangle?(this.ListItem.CellPadding.Value);
        if (this.Column != null && this.Column.CellPadding.HasValue)
          return new Rectangle?(this.Column.CellPadding.Value);
        if (this.ListView != null && this.ListView.CellPadding.HasValue)
          return new Rectangle?(this.ListView.CellPadding.Value);
        return new Rectangle?();
      }
    }

    [Browsable(false)]
    protected virtual StringAlignment EffectiveCellVerticalAlignment
    {
      get
      {
        if (this.cellVerticalAlignment.HasValue)
          return this.cellVerticalAlignment.Value;
        if (this.OLVSubItem != null && this.OLVSubItem.CellVerticalAlignment.HasValue)
          return this.OLVSubItem.CellVerticalAlignment.Value;
        if (this.ListItem != null && this.ListItem.CellVerticalAlignment.HasValue)
          return this.ListItem.CellVerticalAlignment.Value;
        if (this.Column != null && this.Column.CellVerticalAlignment.HasValue)
          return this.Column.CellVerticalAlignment.Value;
        if (this.ListView != null)
          return this.ListView.CellVerticalAlignment;
        return StringAlignment.Center;
      }
    }

    [Description("The image list from which keyed images will be fetched for drawing.")]
    [Category("Appearance")]
    [DefaultValue(null)]
    public ImageList ImageList
    {
      get
      {
        return this.imageList;
      }
      set
      {
        this.imageList = value;
      }
    }

    [Description("When rendering multiple images, how many pixels should be between each image?")]
    [Category("Appearance")]
    [DefaultValue(1)]
    public int Spacing
    {
      get
      {
        return this.spacing;
      }
      set
      {
        this.spacing = value;
      }
    }

    [DefaultValue(true)]
    [Category("Appearance")]
    [Description("Should text be rendered using GDI routines?")]
    public bool UseGdiTextRendering
    {
      get
      {
        if (!this.IsPrinting)
          return this.useGdiTextRendering;
        return false;
      }
      set
      {
        this.useGdiTextRendering = value;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public object Aspect
    {
      get
      {
        if (this.aspect == null)
          this.aspect = this.column.GetValue(this.rowObject);
        return this.aspect;
      }
      set
      {
        this.aspect = value;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public Rectangle Bounds
    {
      get
      {
        return this.bounds;
      }
      set
      {
        this.bounds = value;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public OLVColumn Column
    {
      get
      {
        return this.column;
      }
      set
      {
        this.column = value;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public DrawListViewItemEventArgs DrawItemEvent
    {
      get
      {
        return this.drawItemEventArgs;
      }
      set
      {
        this.drawItemEventArgs = value;
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public DrawListViewSubItemEventArgs Event
    {
      get
      {
        return this.eventArgs;
      }
      set
      {
        this.eventArgs = value;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public Font Font
    {
      get
      {
        if (this.font != null || this.ListItem == null)
          return this.font;
        if (this.SubItem == null || this.ListItem.UseItemStyleForSubItems)
          return this.ListItem.Font;
        return this.SubItem.Font;
      }
      set
      {
        this.font = value;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public ImageList ImageListOrDefault
    {
      get
      {
        return this.ImageList ?? this.ListView.SmallImageList;
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsDrawBackground
    {
      get
      {
        return !this.IsPrinting;
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsItemSelected
    {
      get
      {
        return this.isItemSelected;
      }
      set
      {
        this.isItemSelected = value;
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsPrinting
    {
      get
      {
        return this.isPrinting;
      }
      set
      {
        this.isPrinting = value;
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public OLVListItem ListItem
    {
      get
      {
        return this.listItem;
      }
      set
      {
        this.listItem = value;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public ObjectListView ListView
    {
      get
      {
        return this.objectListView;
      }
      set
      {
        this.objectListView = value;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public OLVListSubItem OLVSubItem
    {
      get
      {
        return this.listSubItem;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
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

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public OLVListSubItem SubItem
    {
      get
      {
        return this.listSubItem;
      }
      set
      {
        this.listSubItem = value;
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Brush TextBrush
    {
      get
      {
        if (this.textBrush == null)
          return (Brush) new SolidBrush(this.GetForegroundColor());
        return this.textBrush;
      }
      set
      {
        this.textBrush = value;
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool UseCustomCheckboxImages
    {
      get
      {
        return this.useCustomCheckboxImages;
      }
      set
      {
        this.useCustomCheckboxImages = value;
      }
    }

    protected virtual bool IsCheckBoxDisabled
    {
      get
      {
        if (this.ListItem != null && !this.ListItem.Enabled)
          return true;
        if (!this.ListView.RenderNonEditableCheckboxesAsDisabled)
          return false;
        if (this.ListView.CellEditActivation == ObjectListView.CellEditActivateMode.None)
          return true;
        if (this.Column != null)
          return !this.Column.IsEditable;
        return false;
      }
    }

    protected bool IsItemHot
    {
      get
      {
        if (this.ListView != null && this.ListItem != null && this.ListView.HotRowIndex == this.ListItem.Index && this.ListView.HotColumnIndex == (this.Column == null ? 0 : this.Column.Index))
          return this.ListView.HotCellHitLocation == HitTestLocation.CheckBox;
        return false;
      }
    }

    private bool ColumnIsPrimary
    {
      get
      {
        if (this.Column != null)
          return this.Column.Index == 0;
        return false;
      }
    }

    protected TextFormatFlags CellVerticalAlignmentAsTextFormatFlag
    {
      get
      {
        switch (this.EffectiveCellVerticalAlignment)
        {
          case StringAlignment.Near:
            return TextFormatFlags.Default;
          case StringAlignment.Center:
            return TextFormatFlags.VerticalCenter;
          case StringAlignment.Far:
            return TextFormatFlags.Bottom;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
    }

    protected virtual StringFormat StringFormatForGdiPlus
    {
      get
      {
        StringFormat stringFormat = new StringFormat();
        stringFormat.LineAlignment = this.EffectiveCellVerticalAlignment;
        stringFormat.Trimming = StringTrimming.EllipsisCharacter;
        stringFormat.Alignment = this.Column == null ? StringAlignment.Near : this.Column.TextStringAlign;
        if (!this.CanWrap)
          stringFormat.FormatFlags = StringFormatFlags.NoWrap;
        return stringFormat;
      }
    }

    private void ClearState()
    {
      this.Event = (DrawListViewSubItemEventArgs) null;
      this.DrawItemEvent = (DrawListViewItemEventArgs) null;
      this.Aspect = (object) null;
      this.Font = (Font) null;
      this.TextBrush = (Brush) null;
    }

    protected virtual Rectangle AlignRectangle(Rectangle outer, Rectangle inner)
    {
      Rectangle rectangle = new Rectangle(outer.Location, inner.Size);
      if (inner.Width < outer.Width)
        rectangle.X = this.AlignHorizontally(outer, inner);
      if (inner.Height < outer.Height)
        rectangle.Y = this.AlignVertically(outer, inner);
      return rectangle;
    }

    protected int AlignHorizontally(Rectangle outer, Rectangle inner)
    {
      switch (this.Column == null ? 0 : (int) this.Column.TextAlign)
      {
        case 0:
          return outer.Left + 1;
        case 1:
          return outer.Right - inner.Width - 1;
        case 2:
          return outer.Left + (outer.Width - inner.Width) / 2;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    protected int AlignVertically(Rectangle outer, Rectangle inner)
    {
      return this.AlignVertically(outer, inner.Height);
    }

    protected int AlignVertically(Rectangle outer, int innerHeight)
    {
      switch (this.EffectiveCellVerticalAlignment)
      {
        case StringAlignment.Near:
          return outer.Top + 1;
        case StringAlignment.Center:
          return outer.Top + (outer.Height - innerHeight) / 2;
        case StringAlignment.Far:
          return outer.Bottom - innerHeight - 1;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    protected virtual Rectangle CalculateAlignedRectangle(Graphics g, Rectangle r)
    {
      if (this.Column == null || this.Column.TextAlign == HorizontalAlignment.Left)
        return r;
      int width = this.CalculateCheckBoxWidth(g) + this.CalculateImageWidth(g, this.GetImageSelector()) + this.CalculateTextWidth(g, this.GetText());
      if (width >= r.Width)
        return r;
      return this.AlignRectangle(r, new Rectangle(0, 0, width, r.Height));
    }

    protected Rectangle CalculateCheckBoxBounds(Graphics g, Rectangle cellBounds)
    {
      Size size = !this.UseCustomCheckboxImages || this.ListView.StateImageList == null ? CheckBoxRenderer.GetGlyphSize(g, CheckBoxState.CheckedNormal) : this.ListView.StateImageList.ImageSize;
      return this.AlignRectangle(cellBounds, new Rectangle(0, 0, size.Width, size.Height));
    }

    protected virtual int CalculateCheckBoxWidth(Graphics g)
    {
      if (!this.ListView.CheckBoxes || !this.ColumnIsPrimary)
        return 0;
      if (this.UseCustomCheckboxImages && this.ListView.StateImageList != null)
        return this.ListView.StateImageList.ImageSize.Width;
      return CheckBoxRenderer.GetGlyphSize(g, CheckBoxState.UncheckedNormal).Width + 6;
    }

    protected virtual int CalculateImageWidth(Graphics g, object imageSelector)
    {
      if (imageSelector == null || imageSelector == DBNull.Value)
        return 0;
      ImageList imageListOrDefault = this.ImageListOrDefault;
      if (imageListOrDefault != null)
      {
        int num = -1;
        if (imageSelector is int)
        {
          num = (int) imageSelector;
        }
        else
        {
          string key = imageSelector as string;
          if (key != null)
            num = imageListOrDefault.Images.IndexOfKey(key);
        }
        if (num >= 0)
          return imageListOrDefault.ImageSize.Width;
      }
      Image image = imageSelector as Image;
      if (image != null)
        return image.Width;
      return 0;
    }

    protected virtual int CalculateTextWidth(Graphics g, string txt)
    {
      if (string.IsNullOrEmpty(txt))
        return 0;
      if (this.UseGdiTextRendering)
      {
        Size proposedSize = new Size(int.MaxValue, int.MaxValue);
        return TextRenderer.MeasureText((IDeviceContext) g, txt, this.Font, proposedSize, TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix).Width;
      }
      using (StringFormat format = new StringFormat())
      {
        format.Trimming = StringTrimming.EllipsisCharacter;
        return 1 + (int) g.MeasureString(txt, this.Font, int.MaxValue, format).Width;
      }
    }

    public virtual Color GetBackgroundColor()
    {
      if (!this.ListView.Enabled)
        return SystemColors.Control;
      if (this.IsItemSelected && !this.ListView.UseTranslucentSelection && this.ListView.FullRowSelect)
      {
        if (this.ListView.Focused)
          return this.ListView.HighlightBackgroundColorOrDefault;
        if (!this.ListView.HideSelection)
          return this.ListView.UnfocusedHighlightBackgroundColorOrDefault;
      }
      if (this.SubItem == null || this.ListItem.UseItemStyleForSubItems)
        return this.ListItem.BackColor;
      return this.SubItem.BackColor;
    }

    public virtual Color GetForegroundColor()
    {
      if (this.IsItemSelected && !this.ListView.UseTranslucentSelection && (this.ColumnIsPrimary || this.ListView.FullRowSelect))
      {
        if (this.ListView.Focused)
          return this.ListView.HighlightForegroundColorOrDefault;
        if (!this.ListView.HideSelection)
          return this.ListView.UnfocusedHighlightForegroundColorOrDefault;
      }
      if (this.SubItem == null || this.ListItem.UseItemStyleForSubItems)
        return this.ListItem.ForeColor;
      return this.SubItem.ForeColor;
    }

    protected virtual Image GetImage()
    {
      return this.GetImage(this.GetImageSelector());
    }

    protected virtual Image GetImage(object imageSelector)
    {
      if (imageSelector == null || imageSelector == DBNull.Value)
        return (Image) null;
      ImageList imageListOrDefault = this.ImageListOrDefault;
      if (imageListOrDefault != null)
      {
        if (imageSelector is int)
        {
          int index = (int) imageSelector;
          if (index < 0 || index >= imageListOrDefault.Images.Count)
            return (Image) null;
          return imageListOrDefault.Images[index];
        }
        string key = imageSelector as string;
        if (key != null)
        {
          if (imageListOrDefault.Images.ContainsKey(key))
            return imageListOrDefault.Images[key];
          return (Image) null;
        }
      }
      return imageSelector as Image;
    }

    protected virtual object GetImageSelector()
    {
      if (!this.ColumnIsPrimary)
        return this.OLVSubItem.ImageSelector;
      return this.ListItem.ImageSelector;
    }

    protected virtual string GetText()
    {
      if (this.SubItem != null)
        return this.SubItem.Text;
      return this.ListItem.Text;
    }

    protected virtual Color GetTextBackgroundColor()
    {
      if (this.IsItemSelected && !this.ListView.UseTranslucentSelection && (this.ColumnIsPrimary || this.ListView.FullRowSelect))
      {
        if (this.ListView.Focused)
          return this.ListView.HighlightBackgroundColorOrDefault;
        if (!this.ListView.HideSelection)
          return this.ListView.UnfocusedHighlightBackgroundColorOrDefault;
      }
      if (this.SubItem == null || this.ListItem.UseItemStyleForSubItems)
        return this.ListItem.BackColor;
      return this.SubItem.BackColor;
    }

    public override bool RenderItem(DrawListViewItemEventArgs e, Graphics g, Rectangle itemBounds, object rowObject)
    {
      this.ClearState();
      this.DrawItemEvent = e;
      this.ListItem = (OLVListItem) e.Item;
      this.SubItem = (OLVListSubItem) null;
      this.ListView = (ObjectListView) this.ListItem.ListView;
      this.Column = this.ListView.GetColumn(0);
      this.RowObject = rowObject;
      this.Bounds = itemBounds;
      this.IsItemSelected = this.ListItem.Selected && this.ListItem.Enabled;
      return this.OptionalRender(g, itemBounds);
    }

    public override bool RenderSubItem(DrawListViewSubItemEventArgs e, Graphics g, Rectangle cellBounds, object rowObject)
    {
      this.ClearState();
      this.Event = e;
      this.ListItem = (OLVListItem) e.Item;
      this.SubItem = (OLVListSubItem) e.SubItem;
      this.ListView = (ObjectListView) this.ListItem.ListView;
      this.Column = (OLVColumn) e.Header;
      this.RowObject = rowObject;
      this.Bounds = cellBounds;
      this.IsItemSelected = this.ListItem.Selected && this.ListItem.Enabled;
      bool flag = this.OptionalRender(g, cellBounds);
      using (Pen pen = new Pen(Color.FromArgb(160, 160, 160), 1f))
        g.DrawLine(pen, cellBounds.Left, cellBounds.Bottom - 1, cellBounds.Right, cellBounds.Bottom - 1);
      return flag;
    }

    public override void HitTest(OlvListViewHitTestInfo hti, int x, int y)
    {
      this.ClearState();
      this.ListView = hti.ListView;
      this.ListItem = hti.Item;
      this.SubItem = hti.SubItem;
      this.Column = hti.Column;
      this.RowObject = hti.RowObject;
      this.IsItemSelected = this.ListItem.Selected && this.ListItem.Enabled;
      this.Bounds = this.SubItem != null ? this.ListItem.GetSubItemBounds(this.Column.Index) : this.ListItem.Bounds;
      using (Graphics graphics = this.ListView.CreateGraphics())
        this.HandleHitTest(graphics, hti, x, y);
    }

    public override Rectangle GetEditRectangle(Graphics g, Rectangle cellBounds, OLVListItem item, int subItemIndex, Size preferredSize)
    {
      this.ClearState();
      this.ListView = (ObjectListView) item.ListView;
      this.ListItem = item;
      this.SubItem = item.GetSubItem(subItemIndex);
      this.Column = this.ListView.GetColumn(subItemIndex);
      this.RowObject = item.RowObject;
      this.IsItemSelected = this.ListItem.Selected && this.ListItem.Enabled;
      this.Bounds = cellBounds;
      return this.HandleGetEditRectangle(g, cellBounds, item, subItemIndex, preferredSize);
    }

    public virtual bool OptionalRender(Graphics g, Rectangle r)
    {
      if (this.ListView.View != View.Details)
        return false;
      this.Render(g, r);
      return true;
    }

    public virtual void Render(Graphics g, Rectangle r)
    {
      this.StandardRender(g, r);
    }

    protected virtual void HandleHitTest(Graphics g, OlvListViewHitTestInfo hti, int x, int y)
    {
      Rectangle bounds = this.CalculateAlignedRectangle(g, this.Bounds);
      this.StandardHitTest(g, hti, bounds, x, y);
    }

    protected virtual Rectangle HandleGetEditRectangle(Graphics g, Rectangle cellBounds, OLVListItem item, int subItemIndex, Size preferredSize)
    {
      if (this.GetType() == typeof (BaseRenderer))
        return this.StandardGetEditRectangle(g, cellBounds, preferredSize);
      return cellBounds;
    }

    protected void StandardRender(Graphics g, Rectangle r)
    {
      this.DrawBackground(g, r);
      if (this.ColumnIsPrimary)
      {
        r.X += 3;
        --r.Width;
      }
      r = this.ApplyCellPadding(r);
      this.DrawAlignedImageAndText(g, r);
      if (!ObjectListView.ShowCellPaddingBounds)
        return;
      g.DrawRectangle(Pens.Purple, r);
    }

    public virtual Rectangle ApplyCellPadding(Rectangle r)
    {
      Rectangle? effectiveCellPadding = this.EffectiveCellPadding;
      if (!effectiveCellPadding.HasValue)
        return r;
      Rectangle rectangle = effectiveCellPadding.Value;
      r.Width -= rectangle.Right;
      r.Height -= rectangle.Bottom;
      r.Offset(rectangle.Location);
      return r;
    }

    protected void StandardHitTest(Graphics g, OlvListViewHitTestInfo hti, Rectangle bounds, int x, int y)
    {
      Rectangle r = bounds;
      if (this.ColumnIsPrimary && !(this is TreeListView.TreeRenderer))
      {
        r.X += 3;
        --r.Width;
      }
      Rectangle cellBounds = this.ApplyCellPadding(r);
      int num1 = 0;
      if (this.ColumnIsPrimary && this.ListView.CheckBoxes)
      {
        Rectangle rectangle = this.CalculateCheckBoxBounds(g, cellBounds);
        rectangle.Inflate(2, 2);
        if (rectangle.Contains(x, y))
        {
          hti.HitTestLocation = HitTestLocation.CheckBox;
          return;
        }
        num1 = rectangle.Width;
      }
      cellBounds.X += num1;
      cellBounds.Width -= num1;
      int num2 = this.CalculateImageWidth(g, this.GetImageSelector());
      Rectangle rectangle1 = cellBounds;
      rectangle1.Width = num2;
      if (rectangle1.Contains(x, y))
      {
        if (this.Column != null && this.Column.Index > 0 && this.Column.CheckBoxes)
          hti.HitTestLocation = HitTestLocation.CheckBox;
        else
          hti.HitTestLocation = HitTestLocation.Image;
      }
      else
      {
        cellBounds.X += num2;
        cellBounds.Width -= num2;
        int num3 = this.CalculateTextWidth(g, this.GetText());
        rectangle1 = cellBounds;
        rectangle1.Width = num3;
        if (rectangle1.Contains(x, y))
          hti.HitTestLocation = HitTestLocation.Text;
        else
          hti.HitTestLocation = HitTestLocation.InCell;
      }
    }

    protected Rectangle StandardGetEditRectangle(Graphics g, Rectangle cellBounds, Size preferredSize)
    {
      Rectangle cellBounds1 = this.CalculateAlignedRectangle(g, cellBounds);
      Rectangle rectangle = this.CalculatePaddedAlignedBounds(g, cellBounds1, preferredSize);
      int num = this.CalculateCheckBoxWidth(g) + this.CalculateImageWidth(g, this.GetImageSelector());
      if (this.ColumnIsPrimary && this.ListItem.IndentCount > 0)
      {
        int width = this.ListView.SmallImageSize.Width;
        num += width * this.ListItem.IndentCount;
      }
      if (num > 0)
      {
        rectangle.X += num;
        rectangle.Width = Math.Max(rectangle.Width - num, 40);
      }
      return rectangle;
    }

    protected Rectangle CalculatePaddedAlignedBounds(Graphics g, Rectangle cellBounds, Size preferredSize)
    {
      Rectangle outer = this.ApplyCellPadding(cellBounds);
      return this.AlignRectangle(outer, new Rectangle(0, 0, outer.Width, preferredSize.Height));
    }

    protected virtual void DrawAlignedImage(Graphics g, Rectangle r, Image image)
    {
      if (image == null)
        return;
      Rectangle inner = new Rectangle(r.Location, image.Size);
      if (image.Height > r.Height)
      {
        float num = (float) r.Height / (float) image.Height;
        inner.Width = (int) ((double) image.Width * (double) num);
        inner.Height = r.Height - 1;
      }
      Rectangle rect = this.AlignRectangle(r, inner);
      if (this.ListItem.Enabled)
        g.DrawImage(image, rect);
      else
        ControlPaint.DrawImageDisabled(g, image, rect.X, rect.Y, this.GetBackgroundColor());
    }

    protected virtual void DrawAlignedImageAndText(Graphics g, Rectangle r)
    {
      this.DrawImageAndText(g, this.CalculateAlignedRectangle(g, r));
    }

    protected virtual void DrawBackground(Graphics g, Rectangle r)
    {
      if (!this.IsDrawBackground)
        return;
      using (Brush brush = (Brush) new SolidBrush(this.GetBackgroundColor()))
        g.FillRectangle(brush, r.X - 1, r.Y - 1, r.Width + 2, r.Height + 2);
    }

    protected virtual int DrawCheckBox(Graphics g, Rectangle r)
    {
      if (!(this.RowObject is cheat))
        return 0;
      if (this.IsPrinting || this.UseCustomCheckboxImages)
      {
        int stateImageIndex = this.ListItem.StateImageIndex;
        if (this.ListView.StateImageList == null || stateImageIndex < 0 || stateImageIndex >= this.ListView.StateImageList.Images.Count)
          return 0;
        return this.DrawImage(g, r, (object) this.ListView.StateImageList.Images[stateImageIndex]) + 4;
      }
      r = this.CalculateCheckBoxBounds(g, r);
      CheckBoxState checkBoxState = this.GetCheckBoxState(this.ListItem.CheckState);
      object parent = (this.ListView as TreeListView).GetParent(this.RowObject);
      if (parent is group)
      {
        if ((parent as group).options == "one")
          RadioButtonRenderer.DrawRadioButton(g, r.Location, checkBoxState == CheckBoxState.CheckedNormal ? RadioButtonState.CheckedNormal : RadioButtonState.UncheckedNormal);
        else
          CheckBoxRenderer.DrawCheckBox(g, r.Location, checkBoxState);
      }
      else
        CheckBoxRenderer.DrawCheckBox(g, r.Location, checkBoxState);
      return CheckBoxRenderer.GetGlyphSize(g, checkBoxState).Width + 6;
    }

    protected virtual CheckBoxState GetCheckBoxState(CheckState checkState)
    {
      if (this.IsCheckBoxDisabled)
      {
        switch (checkState)
        {
          case CheckState.Unchecked:
            return CheckBoxState.UncheckedDisabled;
          case CheckState.Checked:
            return CheckBoxState.CheckedDisabled;
          default:
            return CheckBoxState.MixedDisabled;
        }
      }
      else if (this.IsItemHot)
      {
        switch (checkState)
        {
          case CheckState.Unchecked:
            return CheckBoxState.UncheckedHot;
          case CheckState.Checked:
            return CheckBoxState.CheckedHot;
          default:
            return CheckBoxState.MixedHot;
        }
      }
      else
      {
        switch (checkState)
        {
          case CheckState.Unchecked:
            return CheckBoxState.UncheckedNormal;
          case CheckState.Checked:
            return CheckBoxState.CheckedNormal;
          default:
            return CheckBoxState.MixedNormal;
        }
      }
    }

    protected virtual int DrawImage(Graphics g, Rectangle r, object imageSelector)
    {
      if (imageSelector == null || imageSelector == DBNull.Value)
        return 0;
      ImageList smallImageList = this.ListView.SmallImageList;
      if (smallImageList != null)
      {
        int index = -1;
        if (imageSelector is int)
        {
          index = (int) imageSelector;
          if (index >= smallImageList.Images.Count)
            index = -1;
        }
        else
        {
          string key = imageSelector as string;
          if (key != null)
            index = smallImageList.Images.IndexOfKey(key);
        }
        if (index >= 0)
        {
          if (this.IsPrinting)
          {
            imageSelector = (object) smallImageList.Images[index];
          }
          else
          {
            if (smallImageList.ImageSize.Height < r.Height)
              r.Y = this.AlignVertically(r, new Rectangle(Point.Empty, smallImageList.ImageSize));
            Rectangle rectangle = new Rectangle(r.X - this.Bounds.X, r.Y - this.Bounds.Y, r.Width, r.Height);
            NativeMethods.DrawImageList(g, smallImageList, index, rectangle.X, rectangle.Y, this.IsItemSelected, !this.ListItem.Enabled);
            return smallImageList.ImageSize.Width;
          }
        }
      }
      Image image = imageSelector as Image;
      if (image == null)
        return 0;
      if (image.Size.Height < r.Height)
        r.Y = this.AlignVertically(r, new Rectangle(Point.Empty, image.Size));
      if (this.ListItem.Enabled)
        g.DrawImageUnscaled(image, r.X, r.Y);
      else
        ControlPaint.DrawImageDisabled(g, image, r.X, r.Y, this.GetBackgroundColor());
      return image.Width;
    }

    protected virtual void DrawImageAndText(Graphics g, Rectangle r)
    {
      if (this.ListView.CheckBoxes && this.ColumnIsPrimary)
      {
        int num = this.DrawCheckBox(g, r);
        r.X += num;
        r.Width -= num;
      }
      int num1 = this.DrawImage(g, r, this.GetImageSelector());
      r.X += num1;
      r.Width -= num1;
      this.DrawText(g, r, this.GetText());
    }

    protected virtual int DrawImages(Graphics g, Rectangle r, ICollection imageSelectors)
    {
      List<Image> list = new List<Image>();
      foreach (object imageSelector in (IEnumerable) imageSelectors)
      {
        Image image = this.GetImage(imageSelector);
        if (image != null)
          list.Add(image);
      }
      int width = 0;
      int num = 0;
      foreach (Image image in list)
      {
        width += image.Width + this.Spacing;
        num = Math.Max(num, image.Height);
      }
      Rectangle rectangle = this.AlignRectangle(r, new Rectangle(0, 0, width, num));
      Color backgroundColor = this.GetBackgroundColor();
      Point location = rectangle.Location;
      foreach (Image image in list)
      {
        if (this.ListItem.Enabled)
          g.DrawImage(image, location);
        else
          ControlPaint.DrawImageDisabled(g, image, location.X, location.Y, backgroundColor);
        location.X += image.Width + this.Spacing;
      }
      return width;
    }

    public virtual void DrawText(Graphics g, Rectangle r, string txt)
    {
      if (string.IsNullOrEmpty(txt))
        return;
      if (this.UseGdiTextRendering)
        this.DrawTextGdi(g, r, txt);
      else
        this.DrawTextGdiPlus(g, r, txt);
    }

    protected virtual void DrawTextGdi(Graphics g, Rectangle r, string txt)
    {
      Color backColor = Color.Transparent;
      if (this.IsDrawBackground && this.IsItemSelected && (this.ColumnIsPrimary && !this.ListView.FullRowSelect))
        backColor = this.GetTextBackgroundColor();
      TextFormatFlags flags = TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix | TextFormatFlags.PreserveGraphicsTranslateTransform | this.CellVerticalAlignmentAsTextFormatFlag;
      if (!this.CanWrap)
        flags |= TextFormatFlags.SingleLine;
      TextRenderer.DrawText((IDeviceContext) g, txt, this.Font, r, this.GetForegroundColor(), backColor, flags);
    }

    protected virtual void DrawTextGdiPlus(Graphics g, Rectangle r, string txt)
    {
      using (StringFormat formatForGdiPlus = this.StringFormatForGdiPlus)
      {
        Font font = this.Font;
        if (this.IsDrawBackground && this.IsItemSelected && (this.ColumnIsPrimary && !this.ListView.FullRowSelect))
        {
          SizeF sizeF = g.MeasureString(txt, font, r.Width, formatForGdiPlus);
          Rectangle rect = r;
          rect.Width = (int) sizeF.Width + 1;
          using (Brush brush = (Brush) new SolidBrush(this.ListView.HighlightBackgroundColorOrDefault))
            g.FillRectangle(brush, rect);
        }
        RectangleF layoutRectangle = (RectangleF) r;
        g.DrawString(txt, font, this.TextBrush, layoutRectangle, formatForGdiPlus);
      }
    }
  }
}
