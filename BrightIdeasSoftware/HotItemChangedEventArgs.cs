
// Type: BrightIdeasSoftware.HotItemChangedEventArgs


// Hacked by SystemAce

using System;

namespace BrightIdeasSoftware
{
  public class HotItemChangedEventArgs : EventArgs
  {
    private bool handled;
    private HitTestLocation newHotCellHitLocation;
    private HitTestLocationEx hotCellHitLocationEx;
    private int newHotColumnIndex;
    private int newHotRowIndex;
    private OLVGroup hotGroup;
    private HitTestLocation oldHotCellHitLocation;
    private HitTestLocationEx oldHotCellHitLocationEx;
    private int oldHotColumnIndex;
    private int oldHotRowIndex;
    private OLVGroup oldHotGroup;

    public bool Handled
    {
      get
      {
        return this.handled;
      }
      set
      {
        this.handled = value;
      }
    }

    public HitTestLocation HotCellHitLocation
    {
      get
      {
        return this.newHotCellHitLocation;
      }
      internal set
      {
        this.newHotCellHitLocation = value;
      }
    }

    public virtual HitTestLocationEx HotCellHitLocationEx
    {
      get
      {
        return this.hotCellHitLocationEx;
      }
      internal set
      {
        this.hotCellHitLocationEx = value;
      }
    }

    public int HotColumnIndex
    {
      get
      {
        return this.newHotColumnIndex;
      }
      internal set
      {
        this.newHotColumnIndex = value;
      }
    }

    public int HotRowIndex
    {
      get
      {
        return this.newHotRowIndex;
      }
      internal set
      {
        this.newHotRowIndex = value;
      }
    }

    public OLVGroup HotGroup
    {
      get
      {
        return this.hotGroup;
      }
      internal set
      {
        this.hotGroup = value;
      }
    }

    public HitTestLocation OldHotCellHitLocation
    {
      get
      {
        return this.oldHotCellHitLocation;
      }
      internal set
      {
        this.oldHotCellHitLocation = value;
      }
    }

    public virtual HitTestLocationEx OldHotCellHitLocationEx
    {
      get
      {
        return this.oldHotCellHitLocationEx;
      }
      internal set
      {
        this.oldHotCellHitLocationEx = value;
      }
    }

    public int OldHotColumnIndex
    {
      get
      {
        return this.oldHotColumnIndex;
      }
      internal set
      {
        this.oldHotColumnIndex = value;
      }
    }

    public int OldHotRowIndex
    {
      get
      {
        return this.oldHotRowIndex;
      }
      internal set
      {
        this.oldHotRowIndex = value;
      }
    }

    public OLVGroup OldHotGroup
    {
      get
      {
        return this.oldHotGroup;
      }
      internal set
      {
        this.oldHotGroup = value;
      }
    }

    public override string ToString()
    {
      return string.Format("NewHotCellHitLocation: {0}, HotCellHitLocationEx: {1}, NewHotColumnIndex: {2}, NewHotRowIndex: {3}, HotGroup: {4}", (object) this.newHotCellHitLocation, (object) this.hotCellHitLocationEx, (object) this.newHotColumnIndex, (object) this.newHotRowIndex, (object) this.hotGroup);
    }
  }
}
