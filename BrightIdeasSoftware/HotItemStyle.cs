
// Type: BrightIdeasSoftware.HotItemStyle


// Hacked by SystemAce

using System.ComponentModel;

namespace BrightIdeasSoftware
{
  public class HotItemStyle : SimpleItemStyle
  {
    private IOverlay overlay;
    private IDecoration decoration;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public IOverlay Overlay
    {
      get
      {
        return this.overlay;
      }
      set
      {
        this.overlay = value;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public IDecoration Decoration
    {
      get
      {
        return this.decoration;
      }
      set
      {
        this.decoration = value;
      }
    }
  }
}
