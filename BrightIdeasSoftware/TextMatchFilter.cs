
// Type: BrightIdeasSoftware.TextMatchFilter


// Hacked by SystemAce

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;

namespace BrightIdeasSoftware
{
  public class TextMatchFilter : AbstractModelFilter
  {
    private StringComparison stringComparison = StringComparison.InvariantCultureIgnoreCase;
    private List<TextMatchFilter.TextMatchingStrategy> MatchingStrategies = new List<TextMatchFilter.TextMatchingStrategy>();
    private OLVColumn[] columns;
    private OLVColumn[] additionalColumns;
    private ObjectListView listView;
    private RegexOptions? regexOptions;

    public OLVColumn[] Columns
    {
      get
      {
        return this.columns;
      }
      set
      {
        this.columns = value;
      }
    }

    public OLVColumn[] AdditionalColumns
    {
      get
      {
        return this.additionalColumns;
      }
      set
      {
        this.additionalColumns = value;
      }
    }

    public IEnumerable<string> ContainsStrings
    {
      get
      {
        foreach (TextMatchFilter.TextMatchingStrategy matchingStrategy in this.MatchingStrategies)
          yield return matchingStrategy.Text;
      }
      set
      {
        this.MatchingStrategies = new List<TextMatchFilter.TextMatchingStrategy>();
        if (value == null)
          return;
        foreach (string text in value)
          this.MatchingStrategies.Add((TextMatchFilter.TextMatchingStrategy) new TextMatchFilter.TextContainsMatchingStrategy(this, text));
      }
    }

    public bool HasComponents
    {
      get
      {
        return this.MatchingStrategies.Count > 0;
      }
    }

    public ObjectListView ListView
    {
      get
      {
        return this.listView;
      }
      set
      {
        this.listView = value;
      }
    }

    public IEnumerable<string> PrefixStrings
    {
      get
      {
        foreach (TextMatchFilter.TextMatchingStrategy matchingStrategy in this.MatchingStrategies)
          yield return matchingStrategy.Text;
      }
      set
      {
        this.MatchingStrategies = new List<TextMatchFilter.TextMatchingStrategy>();
        if (value == null)
          return;
        foreach (string text in value)
          this.MatchingStrategies.Add((TextMatchFilter.TextMatchingStrategy) new TextMatchFilter.TextBeginsMatchingStrategy(this, text));
      }
    }

    public RegexOptions RegexOptions
    {
      get
      {
        if (!this.regexOptions.HasValue)
        {
          switch (this.StringComparison)
          {
            case StringComparison.CurrentCulture:
              this.regexOptions = new RegexOptions?(RegexOptions.None);
              break;
            case StringComparison.CurrentCultureIgnoreCase:
              this.regexOptions = new RegexOptions?(RegexOptions.IgnoreCase);
              break;
            case StringComparison.InvariantCulture:
            case StringComparison.Ordinal:
              this.regexOptions = new RegexOptions?(RegexOptions.CultureInvariant);
              break;
            case StringComparison.InvariantCultureIgnoreCase:
            case StringComparison.OrdinalIgnoreCase:
              this.regexOptions = new RegexOptions?(RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
              break;
            default:
              this.regexOptions = new RegexOptions?(RegexOptions.None);
              break;
          }
        }
        return this.regexOptions.Value;
      }
      set
      {
        this.regexOptions = new RegexOptions?(value);
      }
    }

    public IEnumerable<string> RegexStrings
    {
      get
      {
        foreach (TextMatchFilter.TextMatchingStrategy matchingStrategy in this.MatchingStrategies)
          yield return matchingStrategy.Text;
      }
      set
      {
        this.MatchingStrategies = new List<TextMatchFilter.TextMatchingStrategy>();
        if (value == null)
          return;
        foreach (string text in value)
          this.MatchingStrategies.Add((TextMatchFilter.TextMatchingStrategy) new TextMatchFilter.TextRegexMatchingStrategy(this, text));
      }
    }

    public StringComparison StringComparison
    {
      get
      {
        return this.stringComparison;
      }
      set
      {
        this.stringComparison = value;
      }
    }

    public TextMatchFilter(ObjectListView olv)
    {
      this.ListView = olv;
    }

    public TextMatchFilter(ObjectListView olv, string text)
    {
      this.ListView = olv;
      this.ContainsStrings = (IEnumerable<string>) new string[1]
      {
        text
      };
    }

    public TextMatchFilter(ObjectListView olv, string text, StringComparison comparison)
    {
      this.ListView = olv;
      this.ContainsStrings = (IEnumerable<string>) new string[1]
      {
        text
      };
      this.StringComparison = comparison;
    }

    public static TextMatchFilter Regex(ObjectListView olv, params string[] texts)
    {
      return new TextMatchFilter(olv)
      {
        RegexStrings = (IEnumerable<string>) texts
      };
    }

    public static TextMatchFilter Prefix(ObjectListView olv, params string[] texts)
    {
      return new TextMatchFilter(olv)
      {
        PrefixStrings = (IEnumerable<string>) texts
      };
    }

    public static TextMatchFilter Contains(ObjectListView olv, params string[] texts)
    {
      return new TextMatchFilter(olv)
      {
        ContainsStrings = (IEnumerable<string>) texts
      };
    }

    protected virtual IEnumerable<OLVColumn> IterateColumns()
    {
      if (this.Columns == null)
      {
        foreach (OLVColumn olvColumn in this.ListView.Columns)
          yield return olvColumn;
      }
      else
      {
        foreach (OLVColumn olvColumn in this.Columns)
          yield return olvColumn;
      }
      if (this.AdditionalColumns != null)
      {
        foreach (OLVColumn olvColumn in this.AdditionalColumns)
          yield return olvColumn;
      }
    }

    public override bool Filter(object modelObject)
    {
      if (this.ListView == null || !this.HasComponents)
        return true;
      foreach (OLVColumn olvColumn in this.IterateColumns())
      {
        if (olvColumn.IsVisible && olvColumn.Searchable)
        {
          string stringValue = olvColumn.GetStringValue(modelObject);
          foreach (TextMatchFilter.TextMatchingStrategy matchingStrategy in this.MatchingStrategies)
          {
            if (string.IsNullOrEmpty(matchingStrategy.Text) || matchingStrategy.MatchesText(stringValue))
              return true;
          }
        }
      }
      return false;
    }

    public IEnumerable<CharacterRange> FindAllMatchedRanges(string cellText)
    {
      List<CharacterRange> list = new List<CharacterRange>();
      foreach (TextMatchFilter.TextMatchingStrategy matchingStrategy in this.MatchingStrategies)
      {
        if (!string.IsNullOrEmpty(matchingStrategy.Text))
          list.AddRange(matchingStrategy.FindAllMatchedRanges(cellText));
      }
      return (IEnumerable<CharacterRange>) list;
    }

    public bool IsIncluded(OLVColumn column)
    {
      if (this.Columns == null)
        return column.ListView == this.ListView;
      foreach (OLVColumn olvColumn in this.Columns)
      {
        if (olvColumn == column)
          return true;
      }
      return false;
    }

    protected abstract class TextMatchingStrategy
    {
      private TextMatchFilter textFilter;
      private string text;

      public StringComparison StringComparison
      {
        get
        {
          return this.TextFilter.StringComparison;
        }
      }

      public TextMatchFilter TextFilter
      {
        get
        {
          return this.textFilter;
        }
        set
        {
          this.textFilter = value;
        }
      }

      public string Text
      {
        get
        {
          return this.text;
        }
        set
        {
          this.text = value;
        }
      }

      public abstract IEnumerable<CharacterRange> FindAllMatchedRanges(string cellText);

      public abstract bool MatchesText(string cellText);
    }

    protected class TextContainsMatchingStrategy : TextMatchFilter.TextMatchingStrategy
    {
      public TextContainsMatchingStrategy(TextMatchFilter filter, string text)
      {
        this.TextFilter = filter;
        this.Text = text;
      }

      public override bool MatchesText(string cellText)
      {
        return cellText.IndexOf(this.Text, this.StringComparison) != -1;
      }

      public override IEnumerable<CharacterRange> FindAllMatchedRanges(string cellText)
      {
        List<CharacterRange> list = new List<CharacterRange>();
        for (int First = cellText.IndexOf(this.Text, this.StringComparison); First != -1; First = cellText.IndexOf(this.Text, First + this.Text.Length, this.StringComparison))
          list.Add(new CharacterRange(First, this.Text.Length));
        return (IEnumerable<CharacterRange>) list;
      }
    }

    protected class TextBeginsMatchingStrategy : TextMatchFilter.TextMatchingStrategy
    {
      public TextBeginsMatchingStrategy(TextMatchFilter filter, string text)
      {
        this.TextFilter = filter;
        this.Text = text;
      }

      public override bool MatchesText(string cellText)
      {
        return cellText.StartsWith(this.Text, this.StringComparison);
      }

      public override IEnumerable<CharacterRange> FindAllMatchedRanges(string cellText)
      {
        List<CharacterRange> list = new List<CharacterRange>();
        if (cellText.StartsWith(this.Text, this.StringComparison))
          list.Add(new CharacterRange(0, this.Text.Length));
        return (IEnumerable<CharacterRange>) list;
      }
    }

    protected class TextRegexMatchingStrategy : TextMatchFilter.TextMatchingStrategy
    {
      private static Regex InvalidRegexMarker = new Regex(".*");
      private Regex regex;

      public RegexOptions RegexOptions
      {
        get
        {
          return this.TextFilter.RegexOptions;
        }
      }

      protected Regex Regex
      {
        get
        {
          if (this.regex == null)
          {
            try
            {
              this.regex = new Regex(this.Text, this.RegexOptions);
            }
            catch (ArgumentException ex)
            {
              this.regex = TextMatchFilter.TextRegexMatchingStrategy.InvalidRegexMarker;
            }
          }
          return this.regex;
        }
        set
        {
          this.regex = value;
        }
      }

      protected bool IsRegexInvalid
      {
        get
        {
          return this.Regex == TextMatchFilter.TextRegexMatchingStrategy.InvalidRegexMarker;
        }
      }

      public TextRegexMatchingStrategy(TextMatchFilter filter, string text)
      {
        this.TextFilter = filter;
        this.Text = text;
      }

      public override bool MatchesText(string cellText)
      {
        if (this.IsRegexInvalid)
          return true;
        return this.Regex.Match(cellText).Success;
      }

      public override IEnumerable<CharacterRange> FindAllMatchedRanges(string cellText)
      {
        List<CharacterRange> list = new List<CharacterRange>();
        if (!this.IsRegexInvalid)
        {
          foreach (Match match in this.Regex.Matches(cellText))
          {
            if (match.Length > 0)
              list.Add(new CharacterRange(match.Index, match.Length));
          }
        }
        return (IEnumerable<CharacterRange>) list;
      }
    }
  }
}
