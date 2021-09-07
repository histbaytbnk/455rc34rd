
// Type: BrightIdeasSoftware.Version1Renderer


// Hacked by SystemAce

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  [ToolboxItem(false)]
  internal class Version1Renderer : AbstractRenderer
  {
    public RenderDelegate RenderDelegate;

    public Version1Renderer(RenderDelegate renderDelegate)
    {
      this.RenderDelegate = renderDelegate;
    }

    public override bool RenderSubItem(DrawListViewSubItemEventArgs e, Graphics g, Rectangle cellBounds, object rowObject)
    {
      if (this.RenderDelegate == null)
        return base.RenderSubItem(e, g, cellBounds, rowObject);
      return this.RenderDelegate((EventArgs) e, g, cellBounds, rowObject);
    }
  }
}
