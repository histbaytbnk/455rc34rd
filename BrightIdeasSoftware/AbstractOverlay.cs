
// Type: BrightIdeasSoftware.AbstractOverlay


// Hacked by SystemAce

using System;
using System.ComponentModel;
using System.Drawing;

namespace BrightIdeasSoftware
{
  public class AbstractOverlay : ITransparentOverlay, IOverlay
  {
    private int transparency = 128;

    [Category("ObjectListView")]
    [NotifyParentProperty(true)]
    [Description("How transparent should this overlay be")]
    [DefaultValue(128)]
    public int Transparency
    {
      get
      {
        return this.transparency;
      }
      set
      {
        this.transparency = Math.Min((int) byte.MaxValue, Math.Max(0, value));
      }
    }

    public virtual void Draw(ObjectListView olv, Graphics g, Rectangle r)
    {
    }
  }
}
