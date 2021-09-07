
// Type: BrightIdeasSoftware.HyperlinkStyle


// Hacked by SystemAce

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class HyperlinkStyle : Component
  {
    private CellStyle normalStyle;
    private CellStyle overStyle;
    private CellStyle visitedStyle;
    private Cursor overCursor;

    [Category("Appearance")]
    [Description("How should hyperlinks be drawn")]
    public CellStyle Normal
    {
      get
      {
        return this.normalStyle;
      }
      set
      {
        this.normalStyle = value;
      }
    }

    [Description("How should hyperlinks be drawn when the mouse is over them?")]
    [Category("Appearance")]
    public CellStyle Over
    {
      get
      {
        return this.overStyle;
      }
      set
      {
        this.overStyle = value;
      }
    }

    [Category("Appearance")]
    [Description("How should hyperlinks be drawn after they have been clicked")]
    public CellStyle Visited
    {
      get
      {
        return this.visitedStyle;
      }
      set
      {
        this.visitedStyle = value;
      }
    }

    [Category("Appearance")]
    [Description("What cursor should be shown when the mouse is over a link?")]
    public Cursor OverCursor
    {
      get
      {
        return this.overCursor;
      }
      set
      {
        this.overCursor = value;
      }
    }

    public HyperlinkStyle()
    {
      this.Normal = new CellStyle();
      this.Normal.ForeColor = Color.Blue;
      this.Over = new CellStyle();
      this.Over.FontStyle = FontStyle.Underline;
      this.Visited = new CellStyle();
      this.Visited.ForeColor = Color.Purple;
      this.OverCursor = Cursors.Hand;
    }
  }
}
