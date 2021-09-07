
// Type: BrightIdeasSoftware.FlagRenderer


// Hacked by SystemAce

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;

namespace BrightIdeasSoftware
{
  public class FlagRenderer : BaseRenderer
  {
    private List<int> keysInOrder = new List<int>();
    private Dictionary<int, object> imageMap = new Dictionary<int, object>();

    public void Add(object key, object imageSelector)
    {
      int index = ((IConvertible) key).ToInt32((IFormatProvider) NumberFormatInfo.InvariantInfo);
      this.imageMap[index] = imageSelector;
      this.keysInOrder.Remove(index);
      this.keysInOrder.Add(index);
    }

    public override void Render(Graphics g, Rectangle r)
    {
      this.DrawBackground(g, r);
      IConvertible convertible = this.Aspect as IConvertible;
      if (convertible == null)
        return;
      r = this.ApplyCellPadding(r);
      int num = convertible.ToInt32((IFormatProvider) NumberFormatInfo.InvariantInfo);
      ArrayList arrayList = new ArrayList();
      foreach (int index in this.keysInOrder)
      {
        if ((num & index) == index)
        {
          Image image = this.GetImage(this.imageMap[index]);
          if (image != null)
            arrayList.Add((object) image);
        }
      }
      if (arrayList.Count <= 0)
        return;
      this.DrawImages(g, r, (ICollection) arrayList);
    }

    protected override void HandleHitTest(Graphics g, OlvListViewHitTestInfo hti, int x, int y)
    {
      IConvertible convertible = this.Aspect as IConvertible;
      if (convertible == null)
        return;
      int num = convertible.ToInt32((IFormatProvider) NumberFormatInfo.InvariantInfo);
      Point location = this.Bounds.Location;
      foreach (int index in this.keysInOrder)
      {
        if ((num & index) == index)
        {
          Image image = this.GetImage(this.imageMap[index]);
          if (image != null)
          {
            if (new Rectangle(location, image.Size).Contains(x, y))
            {
              hti.UserData = (object) index;
              break;
            }
            location.X += image.Width + this.Spacing;
          }
        }
      }
    }
  }
}
