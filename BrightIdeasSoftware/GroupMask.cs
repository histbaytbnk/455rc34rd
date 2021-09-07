﻿
// Type: BrightIdeasSoftware.GroupMask


// Hacked by SystemAce

using System;

namespace BrightIdeasSoftware
{
  [Flags]
  public enum GroupMask
  {
    LVGF_NONE = 0,
    LVGF_HEADER = 1,
    LVGF_FOOTER = 2,
    LVGF_STATE = 4,
    LVGF_ALIGN = 8,
    LVGF_GROUPID = 16,
    LVGF_SUBTITLE = 256,
    LVGF_TASK = 512,
    LVGF_DESCRIPTIONTOP = 1024,
    LVGF_DESCRIPTIONBOTTOM = 2048,
    LVGF_TITLEIMAGE = 4096,
    LVGF_EXTENDEDIMAGE = 8192,
    LVGF_ITEMS = 16384,
    LVGF_SUBSET = 32768,
    LVGF_SUBSETITEMS = 65536,
  }
}