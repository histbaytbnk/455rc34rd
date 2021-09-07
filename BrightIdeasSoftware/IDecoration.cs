
// Type: BrightIdeasSoftware.IDecoration


// Hacked by SystemAce

namespace BrightIdeasSoftware
{
  public interface IDecoration : IOverlay
  {
    OLVListItem ListItem { get; set; }

    OLVListSubItem SubItem { get; set; }
  }
}
