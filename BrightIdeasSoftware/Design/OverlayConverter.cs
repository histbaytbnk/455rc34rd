
// Type: BrightIdeasSoftware.Design.OverlayConverter


// Hacked by SystemAce

using BrightIdeasSoftware;
using System;
using System.ComponentModel;
using System.Globalization;

namespace BrightIdeasSoftware.Design
{
  internal class OverlayConverter : ExpandableObjectConverter
  {
    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
    {
      if (!(destinationType == typeof (string)))
        return base.CanConvertTo(context, destinationType);
      return true;
    }

    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
      if (destinationType == typeof (string))
      {
        ImageOverlay imageOverlay = value as ImageOverlay;
        if (imageOverlay != null)
        {
          if (imageOverlay.Image != null)
            return (object) "(set)";
          return (object) "(none)";
        }
        TextOverlay textOverlay = value as TextOverlay;
        if (textOverlay != null)
        {
          if (!string.IsNullOrEmpty(textOverlay.Text))
            return (object) "(set)";
          return (object) "(none)";
        }
      }
      return base.ConvertTo(context, culture, value, destinationType);
    }
  }
}
