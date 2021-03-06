
// Type: Be.Windows.Forms.BytePositionInfo


// Hacked by SystemAce

namespace Be.Windows.Forms
{
  internal struct BytePositionInfo
  {
    private int _characterPosition;
    private long _index;

    public int CharacterPosition
    {
      get
      {
        return this._characterPosition;
      }
    }

    public long Index
    {
      get
      {
        return this._index;
      }
    }

    public BytePositionInfo(long index, int characterPosition)
    {
      this._index = index;
      this._characterPosition = characterPosition;
    }
  }
}
