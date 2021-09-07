
// Type: BrightIdeasSoftware.FlagBitSetFilter


// Hacked by SystemAce

using System;
using System.Collections;
using System.Collections.Generic;

namespace BrightIdeasSoftware
{
  public class FlagBitSetFilter : OneOfFilter
  {
    private List<ulong> possibleValuesAsUlongs = new List<ulong>();

    public override IList PossibleValues
    {
      get
      {
        return base.PossibleValues;
      }
      set
      {
        base.PossibleValues = value;
        this.ConvertPossibleValues();
      }
    }

    public FlagBitSetFilter(AspectGetterDelegate valueGetter, ICollection possibleValues)
      : base(valueGetter, possibleValues)
    {
      this.ConvertPossibleValues();
    }

    private void ConvertPossibleValues()
    {
      this.possibleValuesAsUlongs = new List<ulong>();
      foreach (object obj in (IEnumerable) this.PossibleValues)
        this.possibleValuesAsUlongs.Add(Convert.ToUInt64(obj));
    }

    protected override bool DoesValueMatch(object result)
    {
      try
      {
        ulong num1 = Convert.ToUInt64(result);
        foreach (ulong num2 in this.possibleValuesAsUlongs)
        {
          if (((long) num1 & (long) num2) == (long) num2)
            return true;
        }
        return false;
      }
      catch (InvalidCastException ex)
      {
        return false;
      }
      catch (FormatException ex)
      {
        return false;
      }
    }
  }
}
