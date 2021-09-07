
// Type: BrightIdeasSoftware.HighlightTextRenderer


// Hacked by SystemAce

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class HighlightTextRenderer : BaseRenderer
  {
    private float cornerRoundness = 3f;
    private bool useRoundedRectangle = true;
    private Brush fillBrush;
    private TextMatchFilter filter;
    private Pen framePen;

    [DefaultValue(3f)]
    [Description("How rounded will be the corners of the text match frame?")]
    [Category("Appearance")]
    public float CornerRoundness
    {
      get
      {
        return this.cornerRoundness;
      }
      set
      {
        this.cornerRoundness = value;
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Brush FillBrush
    {
      get
      {
        return this.fillBrush;
      }
      set
      {
        this.fillBrush = value;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public TextMatchFilter Filter
    {
      get
      {
        return this.filter;
      }
      set
      {
        this.filter = value;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public Pen FramePen
    {
      get
      {
        return this.framePen;
      }
      set
      {
        this.framePen = value;
      }
    }

    [Description("Will the frame around a text match will have rounded corners?")]
    [DefaultValue(true)]
    [Category("Appearance")]
    public bool UseRoundedRectangle
    {
      get
      {
        return this.useRoundedRectangle;
      }
      set
      {
        this.useRoundedRectangle = value;
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Obsolete("Set the Filter directly rather than just the text", true)]
    public string TextToHighlight
    {
      get
      {
        return string.Empty;
      }
      set
      {
      }
    }

    [Obsolete("Set the Filter directly rather than just this setting", true)]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public StringComparison StringComparison
    {
      get
      {
        return StringComparison.CurrentCultureIgnoreCase;
      }
      set
      {
      }
    }

    protected bool ShouldDrawHighlighting
    {
      get
      {
        if (this.Column == null)
          return true;
        if (this.Column.Searchable && this.Filter != null)
          return this.Filter.HasComponents;
        return false;
      }
    }

    public HighlightTextRenderer()
    {
      this.FramePen = Pens.DarkGreen;
      this.FillBrush = Brushes.Yellow;
    }

    public HighlightTextRenderer(TextMatchFilter filter)
      : this()
    {
      this.Filter = filter;
    }

    [Obsolete("Use HighlightTextRenderer(TextMatchFilter) instead", true)]
    public HighlightTextRenderer(string text)
    {
    }

    protected override Rectangle HandleGetEditRectangle(Graphics g, Rectangle cellBounds, OLVListItem item, int subItemIndex, Size preferredSize)
    {
      return this.StandardGetEditRectangle(g, cellBounds, preferredSize);
    }

    protected override void DrawTextGdi(Graphics g, Rectangle r, string txt)
    {
      if (this.ShouldDrawHighlighting)
        this.DrawGdiTextHighlighting(g, r, txt);
      base.DrawTextGdi(g, r, txt);
    }

    protected virtual void DrawGdiTextHighlighting(Graphics g, Rectangle r, string txt)
    {
      TextFormatFlags flags = TextFormatFlags.NoPrefix | TextFormatFlags.VerticalCenter | TextFormatFlags.PreserveGraphicsTranslateTransform;
      int num = 6;
      Font font = this.Font;
      foreach (CharacterRange characterRange in this.Filter.FindAllMatchedRanges(txt))
      {
        Size size1 = Size.Empty;
        if (characterRange.First > 0)
        {
          string text = txt.Substring(0, characterRange.First);
          size1 = TextRenderer.MeasureText((IDeviceContext) g, text, font, r.Size, flags);
          size1.Width -= num;
        }
        string text1 = txt.Substring(characterRange.First, characterRange.Length);
        Size size2 = TextRenderer.MeasureText((IDeviceContext) g, text1, font, r.Size, flags);
        size2.Width -= num;
        float x = (float) (r.X + size1.Width + 1);
        float y = (float) this.AlignVertically(r, size2.Height);
        this.DrawSubstringFrame(g, x, y, (float) size2.Width, (float) size2.Height);
      }
    }

    protected virtual void DrawSubstringFrame(Graphics g, float x, float y, float width, float height)
    {
      if (this.UseRoundedRectangle)
      {
        using (GraphicsPath roundedRect = this.GetRoundedRect(x, y, width, height, 3f))
        {
          if (this.FillBrush != null)
            g.FillPath(this.FillBrush, roundedRect);
          if (this.FramePen == null)
            return;
          g.DrawPath(this.FramePen, roundedRect);
        }
      }
      else
      {
        if (this.FillBrush != null)
          g.FillRectangle(this.FillBrush, x, y, width, height);
        if (this.FramePen == null)
          return;
        g.DrawRectangle(this.FramePen, x, y, width, height);
      }
    }

    protected override void DrawTextGdiPlus(Graphics g, Rectangle r, string txt)
    {
      if (this.ShouldDrawHighlighting)
        this.DrawGdiPlusTextHighlighting(g, r, txt);
      base.DrawTextGdiPlus(g, r, txt);
    }

    protected virtual void DrawGdiPlusTextHighlighting(Graphics g, Rectangle r, string txt)
    {
      List<CharacterRange> list = new List<CharacterRange>(this.Filter.FindAllMatchedRanges(txt));
      if (list.Count == 0)
        return;
      using (StringFormat formatForGdiPlus = this.StringFormatForGdiPlus)
      {
        RectangleF layoutRect = (RectangleF) r;
        formatForGdiPlus.SetMeasurableCharacterRanges(list.ToArray());
        foreach (Region region in g.MeasureCharacterRanges(txt, this.Font, layoutRect, formatForGdiPlus))
        {
          RectangleF bounds = region.GetBounds(g);
          this.DrawSubstringFrame(g, bounds.X - 1f, bounds.Y - 1f, bounds.Width + 2f, bounds.Height);
        }
      }
    }

    protected GraphicsPath GetRoundedRect(float x, float y, float width, float height, float diameter)
    {
      return this.GetRoundedRect(new RectangleF(x, y, width, height), diameter);
    }

    protected GraphicsPath GetRoundedRect(RectangleF rect, float diameter)
    {
      GraphicsPath graphicsPath = new GraphicsPath();
      if ((double) diameter > 0.0)
      {
        RectangleF rect1 = new RectangleF(rect.X, rect.Y, diameter, diameter);
        graphicsPath.AddArc(rect1, 180f, 90f);
        rect1.X = rect.Right - diameter;
        graphicsPath.AddArc(rect1, 270f, 90f);
        rect1.Y = rect.Bottom - diameter;
        graphicsPath.AddArc(rect1, 0.0f, 90f);
        rect1.X = rect.Left;
        graphicsPath.AddArc(rect1, 90f, 90f);
        graphicsPath.CloseFigure();
      }
      else
        graphicsPath.AddRectangle(rect);
      return graphicsPath;
    }
  }
}
