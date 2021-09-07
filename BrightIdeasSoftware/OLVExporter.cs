
// Type: BrightIdeasSoftware.OLVExporter


// Hacked by SystemAce

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BrightIdeasSoftware
{
  public class OLVExporter
  {
    private bool includeColumnHeaders = true;
    private IList modelObjects = (IList) new ArrayList();
    private bool includeHiddenColumns;
    private ObjectListView objectListView;
    private Dictionary<OLVExporter.ExportFormat, string> results;

    public bool IncludeHiddenColumns
    {
      get
      {
        return this.includeHiddenColumns;
      }
      set
      {
        this.includeHiddenColumns = value;
      }
    }

    public bool IncludeColumnHeaders
    {
      get
      {
        return this.includeColumnHeaders;
      }
      set
      {
        this.includeColumnHeaders = value;
      }
    }

    public ObjectListView ListView
    {
      get
      {
        return this.objectListView;
      }
      set
      {
        this.objectListView = value;
      }
    }

    public IList ModelObjects
    {
      get
      {
        return this.modelObjects;
      }
      set
      {
        this.modelObjects = value;
      }
    }

    public OLVExporter()
    {
    }

    public OLVExporter(ObjectListView olv)
      : this(olv, olv.Objects)
    {
    }

    public OLVExporter(ObjectListView olv, IEnumerable objectsToExport)
    {
      if (olv == null)
        throw new ArgumentNullException("olv");
      if (objectsToExport == null)
        throw new ArgumentNullException("objectsToExport");
      this.ListView = olv;
      this.ModelObjects = (IList) ObjectListView.EnumerableToArray(objectsToExport, true);
    }

    public string ExportTo(OLVExporter.ExportFormat format)
    {
      if (this.results == null)
        this.Convert();
      return this.results[format];
    }

    public void Convert()
    {
      IList<OLVColumn> list1 = this.IncludeHiddenColumns ? (IList<OLVColumn>) this.ListView.AllColumns : (IList<OLVColumn>) this.ListView.ColumnsInDisplayOrder;
      StringBuilder sb1 = new StringBuilder();
      StringBuilder sb2 = new StringBuilder();
      StringBuilder sb3 = new StringBuilder("<table>");
      if (this.IncludeColumnHeaders)
      {
        List<string> list2 = new List<string>();
        foreach (OLVColumn olvColumn in (IEnumerable<OLVColumn>) list1)
          list2.Add(olvColumn.Text);
        this.WriteOneRow(sb1, (IEnumerable<string>) list2, "", "\t", "", (OLVExporter.StringToString) null);
        this.WriteOneRow(sb3, (IEnumerable<string>) list2, "<tr><td>", "</td><td>", "</td></tr>", new OLVExporter.StringToString(OLVExporter.HtmlEncode));
        this.WriteOneRow(sb2, (IEnumerable<string>) list2, "", ",", "", new OLVExporter.StringToString(OLVExporter.CsvEncode));
      }
      foreach (object rowObject in (IEnumerable) this.ModelObjects)
      {
        List<string> list2 = new List<string>();
        foreach (OLVColumn olvColumn in (IEnumerable<OLVColumn>) list1)
          list2.Add(olvColumn.GetStringValue(rowObject));
        this.WriteOneRow(sb1, (IEnumerable<string>) list2, "", "\t", "", (OLVExporter.StringToString) null);
        this.WriteOneRow(sb3, (IEnumerable<string>) list2, "<tr><td>", "</td><td>", "</td></tr>", new OLVExporter.StringToString(OLVExporter.HtmlEncode));
        this.WriteOneRow(sb2, (IEnumerable<string>) list2, "", ",", "", new OLVExporter.StringToString(OLVExporter.CsvEncode));
      }
      sb3.AppendLine("</table>");
      this.results = new Dictionary<OLVExporter.ExportFormat, string>();
      this.results[OLVExporter.ExportFormat.TabSeparated] = sb1.ToString();
      this.results[OLVExporter.ExportFormat.CSV] = sb2.ToString();
      this.results[OLVExporter.ExportFormat.HTML] = sb3.ToString();
    }

    private void WriteOneRow(StringBuilder sb, IEnumerable<string> strings, string startRow, string betweenCells, string endRow, OLVExporter.StringToString encoder)
    {
      sb.Append(startRow);
      bool flag = true;
      foreach (string str in strings)
      {
        if (!flag)
          sb.Append(betweenCells);
        sb.Append(encoder == null ? str : encoder(str));
        flag = false;
      }
      sb.AppendLine(endRow);
    }

    private static string CsvEncode(string text)
    {
      if (text == null)
        return (string) null;
      StringBuilder stringBuilder = new StringBuilder("\"");
      stringBuilder.Append(text.Replace("\"", "\"\""));
      stringBuilder.Append("\"");
      return stringBuilder.ToString();
    }

    private static string HtmlEncode(string text)
    {
      if (text == null)
        return (string) null;
      StringBuilder stringBuilder = new StringBuilder(text.Length);
      int length = text.Length;
      for (int index = 0; index < length; ++index)
      {
        switch (text[index])
        {
          case '"':
            stringBuilder.Append("&quot;");
            break;
          case '&':
            stringBuilder.Append("&amp;");
            break;
          case '<':
            stringBuilder.Append("&lt;");
            break;
          case '>':
            stringBuilder.Append("&gt;");
            break;
          default:
            if ((int) text[index] > 159)
            {
              stringBuilder.Append("&#");
              stringBuilder.Append(((int) text[index]).ToString((IFormatProvider) CultureInfo.InvariantCulture));
              stringBuilder.Append(";");
              break;
            }
            stringBuilder.Append(text[index]);
            break;
        }
      }
      return stringBuilder.ToString();
    }

    public enum ExportFormat
    {
      TSV = 1,
      TabSeparated = 1,
      CSV = 2,
      HTML = 3,
    }

    private delegate string StringToString(string str);
  }
}
