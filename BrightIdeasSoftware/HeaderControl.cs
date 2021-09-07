﻿
// Type: BrightIdeasSoftware.HeaderControl


// Hacked by SystemAce

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace BrightIdeasSoftware
{
  public class HeaderControl : NativeWindow
  {
    private int columnIndexCheckBoxMouseDown = -1;
    private int columnShowingTip = -1;
    private int lastCheckBoxUnderCursor = -1;
    private ObjectListView listView;
    private ToolTipControl toolTip;
    private bool wordWrap;
    private bool cachedNeedsCustomDraw;
    private IntPtr fontHandle;

    public int ColumnIndexUnderCursor
    {
      get
      {
        return NativeMethods.GetColumnUnderPoint(this.Handle, this.ScrolledCursorPosition);
      }
    }

    public new IntPtr Handle
    {
      get
      {
        return NativeMethods.GetHeaderControl((System.Windows.Forms.ListView) this.ListView);
      }
    }

    [Obsolete("Use HeaderStyle.Hot.FontStyle instead")]
    public FontStyle HotFontStyle
    {
      get
      {
        return FontStyle.Regular;
      }
      set
      {
      }
    }

    public Rectangle ClientRectangle
    {
      get
      {
        Rectangle r = new Rectangle();
        NativeMethods.GetClientRect(this.Handle, ref r);
        return r;
      }
    }

    protected bool IsCursorOverLockedDivider
    {
      get
      {
        int dividerUnderPoint = NativeMethods.GetDividerUnderPoint(this.Handle, this.ScrolledCursorPosition);
        if (dividerUnderPoint < 0 || dividerUnderPoint >= this.ListView.Columns.Count)
          return false;
        OLVColumn column = this.ListView.GetColumn(dividerUnderPoint);
        if (!column.IsFixedWidth)
          return column.FillsFreeSpace;
        return true;
      }
    }

    private Point ScrolledCursorPosition
    {
      get
      {
        Point point = this.ListView.PointToClient(Cursor.Position);
        point.X += this.ListView.LowLevelScrollPosition.X;
        return point;
      }
    }

    protected ObjectListView ListView
    {
      get
      {
        return this.listView;
      }
      set
      {
        this.listView = value;
      }
    }

    public int MaximumHeight
    {
      get
      {
        return this.ListView.HeaderMaximumHeight;
      }
    }

    public ToolTipControl ToolTip
    {
      get
      {
        if (this.toolTip == null)
          this.CreateToolTip();
        return this.toolTip;
      }
      protected set
      {
        this.toolTip = value;
      }
    }

    public bool WordWrap
    {
      get
      {
        return this.wordWrap;
      }
      set
      {
        this.wordWrap = value;
      }
    }

    protected TextFormatFlags TextFormatFlags
    {
      get
      {
        TextFormatFlags textFormatFlags1 = TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix | TextFormatFlags.WordEllipsis | TextFormatFlags.PreserveGraphicsTranslateTransform;
        TextFormatFlags textFormatFlags2 = !this.WordWrap ? textFormatFlags1 | TextFormatFlags.SingleLine : textFormatFlags1 | TextFormatFlags.WordBreak;
        if (this.ListView.RightToLeft == RightToLeft.Yes)
          textFormatFlags2 |= TextFormatFlags.RightToLeft;
        return textFormatFlags2;
      }
    }

    public HeaderControl(ObjectListView olv)
    {
      this.ListView = olv;
      this.AssignHandle(NativeMethods.GetHeaderControl((System.Windows.Forms.ListView) olv));
    }

    protected int GetColumnCheckBoxUnderCursor()
    {
      Point scrolledCursorPosition = this.ScrolledCursorPosition;
      int columnUnderPoint = NativeMethods.GetColumnUnderPoint(this.Handle, scrolledCursorPosition);
      if (!this.IsPointOverHeaderCheckBox(columnUnderPoint, scrolledCursorPosition))
        return -1;
      return columnUnderPoint;
    }

    protected bool IsPointOverHeaderCheckBox(int columnIndex, Point pt)
    {
      if (columnIndex < 0 || columnIndex >= this.ListView.Columns.Count)
        return false;
      OLVColumn column = this.ListView.GetColumn(columnIndex);
      if (!this.HasCheckBox(column))
        return false;
      Rectangle checkBoxBounds = this.GetCheckBoxBounds(column);
      checkBoxBounds.Inflate(1, 1);
      return checkBoxBounds.Contains(pt);
    }

    protected int CalculateHeight(Graphics g)
    {
      TextFormatFlags textFormatFlags = this.TextFormatFlags;
      int indexUnderCursor = this.ColumnIndexUnderCursor;
      float val1 = 0.0f;
      for (int index = 0; index < this.ListView.Columns.Count; ++index)
      {
        OLVColumn column = this.ListView.GetColumn(index);
      }
      if (this.MaximumHeight != -1)
        return Math.Min(this.MaximumHeight, (int) val1);
      return (int) val1;
    }

   

    public Rectangle GetCheckBoxBounds(OLVColumn column)
    {
      Rectangle headerDrawRect = this.GetHeaderDrawRect(column.Index);
      using (Graphics graphics = this.ListView.CreateGraphics())
        return this.CalculateCheckBoxBounds(graphics, headerDrawRect);
    }

    public bool HasCheckBox(OLVColumn column)
    {
      if (!column.HeaderCheckBox)
        return column.HeaderTriStateCheckBox;
      return true;
    }

    protected bool HasSortIndicator(OLVColumn column)
    {
      if (!this.ListView.ShowSortIndicators || column != this.ListView.LastSortColumn)
        return false;
      return this.ListView.LastSortOrder != SortOrder.None;
    }

    protected bool HasFilterIndicator(OLVColumn column)
    {
      if (this.ListView.UseFiltering && this.ListView.UseFilterIndicator)
        return column.HasFilterIndicator;
      return false;
    }

    protected bool HasNonThemedSortIndicator(OLVColumn column)
    {
      if (!this.ListView.ShowSortIndicators || VisualStyleRenderer.IsSupported && VisualStyleRenderer.IsElementDefined(VisualStyleElement.Header.SortArrow.SortedUp))
        return false;
      return this.HasSortIndicator(column);
    }

    public Rectangle GetItemRect(int itemIndex)
    {
      NativeMethods.RECT r = new NativeMethods.RECT();
      NativeMethods.SendMessageRECT(this.Handle, 4615, itemIndex, ref r);
      return Rectangle.FromLTRB(r.left, r.top, r.right, r.bottom);
    }

    public Rectangle GetHeaderDrawRect(int itemIndex)
    {
      Rectangle itemRect = this.GetItemRect(itemIndex);
      itemRect.Inflate(-3, 0);
      itemRect.Y -= 2;
      return itemRect;
    }

    public void Invalidate()
    {
      NativeMethods.InvalidateRect(this.Handle, 0, true);
    }

    public void Invalidate(OLVColumn column)
    {
      NativeMethods.InvalidateRect(this.Handle, 0, true);
    }

    protected virtual void CreateToolTip()
    {
      this.ToolTip = new ToolTipControl();
      this.ToolTip.Create(this.Handle);
      this.ToolTip.AddTool((IWin32Window) this);
      this.ToolTip.Showing += new EventHandler<ToolTipShowingEventArgs>(this.ListView.HeaderToolTipShowingCallback);
    }

    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    protected override void WndProc(ref Message m)
    {
      switch (m.Msg)
      {
        case 512:
          if (!this.HandleMouseMove(ref m))
            return;
          break;
        case 513:
          if (!this.HandleLButtonDown(ref m))
            return;
          break;
        case 514:
          if (!this.HandleLButtonUp(ref m))
            return;
          break;
        case 675:
          if (!this.HandleMouseLeave(ref m))
            return;
          break;
        case 4613:
          if (!this.HandleLayout(ref m))
            return;
          break;
        case 2:
          if (!this.HandleDestroy(ref m))
            return;
          break;
        case 32:
          if (!this.HandleSetCursor(ref m))
            return;
          break;
        case 78:
          if (!this.HandleNotify(ref m))
            return;
          break;
      }
      base.WndProc(ref m);
    }

    private bool HandleReflectNotify(ref Message m)
    {
      NativeMethods.NMHDR nmhdr = (NativeMethods.NMHDR) m.GetLParam(typeof (NativeMethods.NMHDR));
      return true;
    }

    protected bool HandleLButtonDown(ref Message m)
    {
      this.columnIndexCheckBoxMouseDown = this.GetColumnCheckBoxUnderCursor();
      if (this.columnIndexCheckBoxMouseDown < 0)
        return true;
      this.Invalidate();
      m.Result = (IntPtr) 1;
      return false;
    }

    protected bool HandleLButtonUp(ref Message m)
    {
      if (this.columnIndexCheckBoxMouseDown < 0 || this.columnIndexCheckBoxMouseDown != this.GetColumnCheckBoxUnderCursor())
        return true;
      this.ListView.ToggleHeaderCheckBox(this.ListView.GetColumn(this.columnIndexCheckBoxMouseDown));
      return true;
    }

    protected bool HandleSetCursor(ref Message m)
    {
      if (!this.IsCursorOverLockedDivider)
        return true;
      m.Result = (IntPtr) 1;
      return false;
    }

    protected bool HandleMouseMove(ref Message m)
    {
      if (this.ListView.TriggerCellOverEventsWhenOverHeader)
        this.ListView.HandleMouseMove(new Point(m.LParam.ToInt32() & (int) ushort.MaxValue, m.LParam.ToInt32() >> 16 & (int) ushort.MaxValue));
      int indexUnderCursor = this.ColumnIndexUnderCursor;
      if (indexUnderCursor != this.columnShowingTip && !this.ListView.IsDesignMode)
      {
        this.ToolTip.PopToolTip((IWin32Window) this);
        this.columnShowingTip = indexUnderCursor;
      }
      int checkBoxUnderCursor = this.GetColumnCheckBoxUnderCursor();
      if (checkBoxUnderCursor != this.lastCheckBoxUnderCursor)
      {
        this.Invalidate();
        this.lastCheckBoxUnderCursor = checkBoxUnderCursor;
      }
      return true;
    }

    protected bool HandleMouseLeave(ref Message m)
    {
      if (this.ListView.TriggerCellOverEventsWhenOverHeader)
        this.ListView.HandleMouseMove(new Point(-1, -1));
      return true;
    }

    protected bool HandleNotify(ref Message m)
    {
      if (m.LParam == IntPtr.Zero)
        return false;
      switch (((NativeMethods.NMHDR) m.GetLParam(typeof (NativeMethods.NMHDR))).code)
      {
        case -530:
          return this.ToolTip.HandleGetDispInfo(ref m);
        case -522:
          return this.ToolTip.HandlePop(ref m);
        case -521:
          return this.ToolTip.HandleShow(ref m);
        default:
          return false;
      }
    }

    internal virtual bool HandleHeaderCustomDraw(ref Message m)
    {
      NativeMethods.NMCUSTOMDRAW nmcustomdraw = (NativeMethods.NMCUSTOMDRAW) m.GetLParam(typeof (NativeMethods.NMCUSTOMDRAW));
      switch (nmcustomdraw.dwDrawStage)
      {
        case 1:
          this.cachedNeedsCustomDraw = this.NeedsCustomDraw();
          m.Result = (IntPtr) 32;
          return true;
        case 65537:
          int num = nmcustomdraw.dwItemSpec.ToInt32();
          OLVColumn column = this.ListView.GetColumn(num);
          if (this.cachedNeedsCustomDraw)
          {
            using (Graphics g = Graphics.FromHdc(nmcustomdraw.hdc))
            {
              g.TextRenderingHint = ObjectListView.TextRenderingHint;
              this.CustomDrawHeaderCell(g, num, nmcustomdraw.uItemState);
            }
            m.Result = (IntPtr) 4;
          }
          else
          {
            bool isPressed = (nmcustomdraw.uItemState & 1) == 1;
            bool isHot = num == this.ColumnIndexUnderCursor;
            this.fontHandle = this.CalculateFont(column, isHot, isPressed).ToHfont();
            NativeMethods.SelectObject(nmcustomdraw.hdc, this.fontHandle);
            m.Result = (IntPtr) 18;
          }
          return true;
        case 65538:
          if (this.fontHandle != IntPtr.Zero)
          {
            NativeMethods.DeleteObject(this.fontHandle);
            this.fontHandle = IntPtr.Zero;
            break;
          }
          break;
      }
      return false;
    }

    protected bool HandleLayout(ref Message m)
    {
      if (this.ListView.HeaderStyle == ColumnHeaderStyle.None)
        return true;
      NativeMethods.HDLAYOUT hdlayout = (NativeMethods.HDLAYOUT) m.GetLParam(typeof (NativeMethods.HDLAYOUT));
      NativeMethods.RECT rect = (NativeMethods.RECT) Marshal.PtrToStructure(hdlayout.prc, typeof (NativeMethods.RECT));
      NativeMethods.WINDOWPOS windowpos = (NativeMethods.WINDOWPOS) Marshal.PtrToStructure(hdlayout.pwpos, typeof (NativeMethods.WINDOWPOS));
      using (Graphics graphics = this.ListView.CreateGraphics())
      {
        graphics.TextRenderingHint = ObjectListView.TextRenderingHint;
        int num = this.CalculateHeight(graphics);
        windowpos.hwnd = this.Handle;
        windowpos.hwndInsertAfter = IntPtr.Zero;
        windowpos.flags = 32;
        windowpos.x = rect.left;
        windowpos.y = rect.top;
        windowpos.cx = rect.right - rect.left;
        windowpos.cy = num;
        rect.top = num;
        Marshal.StructureToPtr((object) rect, hdlayout.prc, false);
        Marshal.StructureToPtr((object) windowpos, hdlayout.pwpos, false);
      }

      return false;
    }

    protected bool HandleDestroy(ref Message m)
    {
      if (this.toolTip != null)
        this.toolTip.Showing -= new EventHandler<ToolTipShowingEventArgs>(this.ListView.HeaderToolTipShowingCallback);
      return false;
    }

    protected bool NeedsCustomDraw()
    {
      if (this.WordWrap)
        return true;
      if (this.ListView.HeaderUsesThemes)
        return false;
      if (this.NeedsCustomDraw(this.ListView.HeaderFormatStyle))
        return true;
      foreach (OLVColumn column in this.ListView.Columns)
      {
        if (column.HasHeaderImage || !column.ShowTextInHeader || (column.IsHeaderVertical || this.HasFilterIndicator(column)) || (this.HasCheckBox(column) || column.TextAlign != column.HeaderTextAlign || this.NeedsCustomDraw(column.HeaderFormatStyle)))
          return true;
      }
      return false;
    }

    private bool NeedsCustomDraw(HeaderFormatStyle style)
    {
      if (style == null)
        return false;
      if (!this.NeedsCustomDraw(style.Normal) && !this.NeedsCustomDraw(style.Hot))
        return this.NeedsCustomDraw(style.Pressed);
      return true;
    }

    private bool NeedsCustomDraw(HeaderStateStyle style)
    {
      if (style == null)
        return false;
      if (!style.BackColor.IsEmpty || (double) style.FrameWidth > 0.0 && !style.FrameColor.IsEmpty)
        return true;
      if (!style.ForeColor.IsEmpty)
        return style.ForeColor != Color.Black;
      return false;
    }

    protected void CustomDrawHeaderCell(Graphics g, int columnIndex, int itemState)
    {
      OLVColumn column = this.ListView.GetColumn(columnIndex);
      bool flag = this.HasCheckBox(column);
      bool isHot1 = columnIndex == this.lastCheckBoxUnderCursor;
      bool isPressed1 = isHot1 && Control.MouseButtons == MouseButtons.Left;
      bool isHot2 = columnIndex == this.ColumnIndexUnderCursor && (!flag || !isHot1);
      bool isPressed2 = (itemState & 1) == 1;
      HeaderStateStyle stateStyle = this.CalculateStateStyle(column, isHot2, isPressed2);
      Rectangle itemRect = this.GetItemRect(columnIndex);
      if (column.HeaderDrawing != null && !column.HeaderDrawing(g, itemRect, columnIndex, column, isPressed2, stateStyle))
        return;
      if (this.ListView.HeaderUsesThemes && VisualStyleRenderer.IsSupported && VisualStyleRenderer.IsElementDefined(VisualStyleElement.Header.Item.Normal))
        this.DrawThemedBackground(g, itemRect, columnIndex, isPressed2, isHot2);
      else
        this.DrawUnthemedBackground(g, itemRect, columnIndex, isPressed2, isHot2, stateStyle);
      Rectangle r = this.GetHeaderDrawRect(columnIndex);
      if (this.HasSortIndicator(column))
      {
        if (this.ListView.HeaderUsesThemes && VisualStyleRenderer.IsSupported && VisualStyleRenderer.IsElementDefined(VisualStyleElement.Header.SortArrow.SortedUp))
          this.DrawThemedSortIndicator(g, r);
        else
          r = this.DrawUnthemedSortIndicator(g, r);
      }
      if (this.HasFilterIndicator(column))
        r = this.DrawFilterIndicator(g, r);
      if (flag)
        r = this.DrawCheckBox(g, r, column.HeaderCheckState, column.HeaderCheckBoxDisabled, isHot1, isPressed1);
      this.DrawHeaderImageAndText(g, r, column, stateStyle);
    }

    private Rectangle DrawCheckBox(Graphics g, Rectangle r, CheckState checkState, bool isDisabled, bool isHot, bool isPressed)
    {
      CheckBoxState checkBoxState = this.GetCheckBoxState(checkState, isDisabled, isHot, isPressed);
      Rectangle rectangle = this.CalculateCheckBoxBounds(g, r);
      CheckBoxRenderer.DrawCheckBox(g, rectangle.Location, checkBoxState);
      int num = rectangle.Right + 3;
      r.Width -= num - r.X;
      r.X = num;
      return r;
    }

    private Rectangle CalculateCheckBoxBounds(Graphics g, Rectangle cellBounds)
    {
      Size glyphSize = CheckBoxRenderer.GetGlyphSize(g, CheckBoxState.CheckedNormal);
      int num = (cellBounds.Height - glyphSize.Height) / 2;
      return new Rectangle(cellBounds.X + 3, cellBounds.Y + num, glyphSize.Width, glyphSize.Height);
    }

    private CheckBoxState GetCheckBoxState(CheckState checkState, bool isDisabled, bool isHot, bool isPressed)
    {
      if (isDisabled)
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
      else if (isPressed)
      {
        switch (checkState)
        {
          case CheckState.Unchecked:
            return CheckBoxState.UncheckedPressed;
          case CheckState.Checked:
            return CheckBoxState.CheckedPressed;
          default:
            return CheckBoxState.MixedPressed;
        }
      }
      else if (isHot)
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

    protected void DrawUnthemedBackground(Graphics g, Rectangle r, int columnIndex, bool isPressed, bool isHot, HeaderStateStyle stateStyle)
    {
      if (stateStyle.BackColor.IsEmpty)
      {
        if (VisualStyleRenderer.IsSupported && VisualStyleRenderer.IsElementDefined(VisualStyleElement.Header.Item.Normal))
          this.DrawThemedBackground(g, r, columnIndex, isPressed, isHot);
        else
          ControlPaint.DrawBorder3D(g, r, Border3DStyle.RaisedInner);
      }
      else
      {
        using (Brush brush = (Brush) new SolidBrush(stateStyle.BackColor))
          g.FillRectangle(brush, r);
      }
      if (stateStyle.FrameColor.IsEmpty || (double) stateStyle.FrameWidth <= 0.0)
        return;
      RectangleF rectangleF = (RectangleF) r;
      rectangleF.Inflate(-stateStyle.FrameWidth, -stateStyle.FrameWidth);
      using (Pen pen = new Pen(stateStyle.FrameColor, stateStyle.FrameWidth))
        g.DrawRectangle(pen, rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
    }

    protected void DrawThemedBackground(Graphics g, Rectangle r, int columnIndex, bool isPressed, bool isHot)
    {
      int part = 1;
      if (columnIndex == 0 && VisualStyleRenderer.IsElementDefined(VisualStyleElement.Header.ItemLeft.Normal))
        part = 2;
      if (columnIndex == this.ListView.Columns.Count - 1 && VisualStyleRenderer.IsElementDefined(VisualStyleElement.Header.ItemRight.Normal))
        part = 3;
      int state = 1;
      if (isPressed)
        state = 3;
      else if (isHot)
        state = 2;
      new VisualStyleRenderer("HEADER", part, state).DrawBackground((IDeviceContext) g, r);
    }

    protected void DrawThemedSortIndicator(Graphics g, Rectangle r)
    {
      VisualStyleRenderer visualStyleRenderer = (VisualStyleRenderer) null;
      if (this.ListView.LastSortOrder == SortOrder.Ascending)
        visualStyleRenderer = new VisualStyleRenderer(VisualStyleElement.Header.SortArrow.SortedUp);
      if (this.ListView.LastSortOrder == SortOrder.Descending)
        visualStyleRenderer = new VisualStyleRenderer(VisualStyleElement.Header.SortArrow.SortedDown);
      if (visualStyleRenderer == null)
        return;
      Size partSize = visualStyleRenderer.GetPartSize((IDeviceContext) g, ThemeSizeType.True);
      Point location = visualStyleRenderer.GetPoint(PointProperty.Offset);
      if (location.X == 0 && location.Y == 0)
        location = new Point(r.X + r.Width / 2 - partSize.Width / 2, r.Y);
      visualStyleRenderer.DrawBackground((IDeviceContext) g, new Rectangle(location, partSize));
    }

    protected Rectangle DrawUnthemedSortIndicator(Graphics g, Rectangle r)
    {
      Point point = new Point(r.Right - 16 - 2, r.Top + (r.Height - 16) / 2);
      Point[] points = new Point[3]
      {
        point,
        point,
        point
      };
      if (this.ListView.LastSortOrder == SortOrder.Ascending)
      {
        points[0].Offset(2, 10);
        points[1].Offset(8, 3);
        points[2].Offset(14, 10);
      }
      else
      {
        points[0].Offset(2, 4);
        points[1].Offset(8, 10);
        points[2].Offset(14, 4);
      }
      g.FillPolygon(Brushes.SlateGray, points);
      r.Width = r.Width - 16;
      return r;
    }

    protected Rectangle DrawFilterIndicator(Graphics g, Rectangle r)
    {
      int num = this.CalculateFilterIndicatorWidth(r);
      if (num <= 0)
        return r;
      r.Width -= num;
      return r;
    }

    private int CalculateFilterIndicatorWidth(Rectangle r)
    {
      return 0;
    }

    protected void DrawHeaderImageAndText(Graphics g, Rectangle r, OLVColumn column, HeaderStateStyle stateStyle)
    {
      TextFormatFlags flags = this.TextFormatFlags | TextFormatFlags.VerticalCenter;
      if (column.HeaderTextAlign == HorizontalAlignment.Center)
        flags |= TextFormatFlags.HorizontalCenter;
      if (column.HeaderTextAlign == HorizontalAlignment.Right)
        flags |= TextFormatFlags.Right;
      Font f = this.ListView.HeaderUsesThemes ? this.ListView.Font : stateStyle.Font ?? this.ListView.Font;
      Color color = this.ListView.HeaderUsesThemes ? Color.Black : stateStyle.ForeColor;
      if (color.IsEmpty)
        color = Color.Black;
      if (column.IsHeaderVertical)
        HeaderControl.DrawVerticalText(g, r, column, f, color);
      else if (column.HasHeaderImage && r.Width > column.ImageList.ImageSize.Width * 2)
        this.DrawImageAndText(g, r, column, flags, f, color, 3);
      else
        this.DrawText(g, r, column, flags, f, color);
    }

    private void DrawText(Graphics g, Rectangle r, OLVColumn column, TextFormatFlags flags, Font f, Color color)
    {
      if (!column.ShowTextInHeader)
        return;
      TextRenderer.DrawText((IDeviceContext) g, column.Text, f, r, color, Color.Transparent, flags);
    }

    private void DrawImageAndText(Graphics g, Rectangle r, OLVColumn column, TextFormatFlags flags, Font f, Color color, int imageTextGap)
    {
      Rectangle r1 = r;
      r1.X += column.ImageList.ImageSize.Width + imageTextGap;
      r1.Width -= column.ImageList.ImageSize.Width + imageTextGap;
      Size size = Size.Empty;
      if (column.ShowTextInHeader)
        size = TextRenderer.MeasureText((IDeviceContext) g, column.Text, f, r1.Size, flags);
      int y = r.Top + (r.Height - column.ImageList.ImageSize.Height) / 2;
      int num = r1.Left;
      if (column.HeaderTextAlign == HorizontalAlignment.Center)
        num = r1.Left + (r1.Width - size.Width) / 2;
      if (column.HeaderTextAlign == HorizontalAlignment.Right)
        num = r1.Right - size.Width;
      int x = num - (column.ImageList.ImageSize.Width + imageTextGap);
      column.ImageList.Draw(g, x, y, column.ImageList.Images.IndexOfKey(column.HeaderImageKey));
      this.DrawText(g, r1, column, flags, f, color);
    }

    private static void DrawVerticalText(Graphics g, Rectangle r, OLVColumn column, Font f, Color color)
    {
      try
      {
        Matrix matrix = new Matrix();
        matrix.RotateAt(-90f, (PointF) new Point(r.X, r.Bottom));
        matrix.Translate(0.0f, (float) r.Height);
        g.Transform = matrix;
        StringFormat format = new StringFormat(StringFormatFlags.NoWrap);
        format.Alignment = StringAlignment.Near;
        format.LineAlignment = column.HeaderTextAlignAsStringAlignment;
        Rectangle rectangle = r;
        rectangle.Width = r.Height;
        rectangle.Height = r.Width;
        using (Brush brush = (Brush) new SolidBrush(color))
          g.DrawString(column.Text, f, brush, (RectangleF) rectangle, format);
      }
      finally
      {
        g.ResetTransform();
      }
    }

    protected HeaderFormatStyle CalculateHeaderStyle(OLVColumn column)
    {
      return column.HeaderFormatStyle ?? this.ListView.HeaderFormatStyle ?? new HeaderFormatStyle();
    }

    protected HeaderStateStyle CalculateStateStyle(OLVColumn column, bool isHot, bool isPressed)
    {
      HeaderFormatStyle headerFormatStyle = this.CalculateHeaderStyle(column);
      if (this.ListView.IsDesignMode)
        return headerFormatStyle.Normal;
      if (isPressed)
        return headerFormatStyle.Pressed;
      if (isHot)
        return headerFormatStyle.Hot;
      return headerFormatStyle.Normal;
    }

    protected Font CalculateFont(OLVColumn column, bool isHot, bool isPressed)
    {
      return this.CalculateStateStyle(column, isHot, isPressed).Font ?? this.ListView.Font;
    }

    internal OlvListViewHitTestInfo.HeaderHitTestInfo HitTest(int x, int y)
    {
      if (!this.ClientRectangle.Contains(x, y))
        return (OlvListViewHitTestInfo.HeaderHitTestInfo) null;
      Point pt = new Point(x + this.ListView.LowLevelScrollPosition.X, y);
      OlvListViewHitTestInfo.HeaderHitTestInfo headerHitTestInfo = new OlvListViewHitTestInfo.HeaderHitTestInfo();
      headerHitTestInfo.ColumnIndex = NativeMethods.GetColumnUnderPoint(this.Handle, pt);
      headerHitTestInfo.IsOverCheckBox = this.IsPointOverHeaderCheckBox(headerHitTestInfo.ColumnIndex, pt);
      headerHitTestInfo.OverDividerIndex = NativeMethods.GetDividerUnderPoint(this.Handle, pt);
      return headerHitTestInfo;
    }
  }
}
