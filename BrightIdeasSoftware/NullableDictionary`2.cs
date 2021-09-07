
// Type: BrightIdeasSoftware.NullableDictionary`2


// Hacked by SystemAce

using System.Collections;
using System.Collections.Generic;

namespace BrightIdeasSoftware
{
  internal class NullableDictionary<TKey, TValue> : Dictionary<TKey, TValue>
  {
    private bool hasNullKey;
    private TValue nullValue;

    public new TValue this[TKey key]
    {
      get
      {
        if ((object) key != null)
          return base[key];
        if (this.hasNullKey)
          return this.nullValue;
        throw new KeyNotFoundException();
      }
      set
      {
        if ((object) key == null)
        {
          this.hasNullKey = true;
          this.nullValue = value;
        }
        else
          base[key] = value;
      }
    }

    public IList Keys
    {
      get
      {
        ArrayList arrayList = new ArrayList((ICollection) base.Keys);
        if (this.hasNullKey)
          arrayList.Add((object) null);
        return (IList) arrayList;
      }
    }

    public IList<TValue> Values
    {
      get
      {
        List<TValue> list = new List<TValue>((IEnumerable<TValue>) base.Values);
        if (this.hasNullKey)
          list.Add(this.nullValue);
        return (IList<TValue>) list;
      }
    }

    public new bool ContainsKey(TKey key)
    {
      if ((object) key != null)
        return base.ContainsKey(key);
      return this.hasNullKey;
    }
  }
}
