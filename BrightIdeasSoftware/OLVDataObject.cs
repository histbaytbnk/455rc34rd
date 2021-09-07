
// Type: BrightIdeasSoftware.OLVDataObject


// Hacked by SystemAce

using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class OLVDataObject : DataObject
  {
    private IList modelObjects = (IList) new ArrayList();
    private bool includeHiddenColumns;
    private bool includeColumnHeaders;
    private ObjectListView objectListView;

    public bool IncludeHiddenColumns
    {
      get
      {
        return this.includeHiddenColumns;
      }
    }

    public bool IncludeColumnHeaders
    {
      get
      {
        return this.includeColumnHeaders;
      }
    }

    public ObjectListView ListView
    {
      get
      {
        return this.objectListView;
      }
    }

    public IList ModelObjects
    {
      get
      {
        return this.modelObjects;
      }
    }

    public OLVDataObject(ObjectListView olv)
      : this(olv, olv.SelectedObjects)
    {
    }

    public OLVDataObject(ObjectListView olv, IList modelObjects)
    {
      this.objectListView = olv;
      this.modelObjects = modelObjects;
      this.includeHiddenColumns = olv.IncludeHiddenColumnsInDataTransfer;
      this.includeColumnHeaders = olv.IncludeColumnHeadersInCopy;
    }

    public void CreateTextFormats()
    {
      IList<OLVColumn> list = this.IncludeHiddenColumns ? (IList<OLVColumn>) this.ListView.AllColumns : (IList<OLVColumn>) this.ListView.ColumnsInDisplayOrder;
      StringBuilder stringBuilder1 = new StringBuilder();
      StringBuilder stringBuilder2 = new StringBuilder("<table>");
      if (this.includeColumnHeaders)
      {
        stringBuilder2.Append("<tr><td>");
        foreach (OLVColumn olvColumn in (IEnumerable<OLVColumn>) list)
        {
          if (olvColumn != list[0])
          {
            stringBuilder1.Append("\t");
            stringBuilder2.Append("</td><td>");
          }
          string text = olvColumn.Text;
          stringBuilder1.Append(text);
          stringBuilder2.Append(text);
        }
        stringBuilder1.AppendLine();
        stringBuilder2.AppendLine("</td></tr>");
      }
      foreach (object rowObject in (IEnumerable) this.ModelObjects)
      {
        stringBuilder2.Append("<tr><td>");
        foreach (OLVColumn olvColumn in (IEnumerable<OLVColumn>) list)
        {
          if (olvColumn != list[0])
          {
            stringBuilder1.Append("\t");
            stringBuilder2.Append("</td><td>");
          }
          string stringValue = olvColumn.GetStringValue(rowObject);
          stringBuilder1.Append(stringValue);
          stringBuilder2.Append(stringValue);
        }
        stringBuilder1.AppendLine();
        stringBuilder2.AppendLine("</td></tr>");
      }
      stringBuilder2.AppendLine("</table>");
      this.SetData((object) stringBuilder1.ToString());
      this.SetText(this.ConvertToHtmlFragment(stringBuilder2.ToString()), TextDataFormat.Html);
    }

    public string CreateHtml()
    {
      IList<OLVColumn> list = (IList<OLVColumn>) this.ListView.ColumnsInDisplayOrder;
      StringBuilder stringBuilder = new StringBuilder("<table>");
      foreach (object rowObject in (IEnumerable) this.ModelObjects)
      {
        stringBuilder.Append("<tr><td>");
        foreach (OLVColumn olvColumn in (IEnumerable<OLVColumn>) list)
        {
          if (olvColumn != list[0])
            stringBuilder.Append("</td><td>");
          string stringValue = olvColumn.GetStringValue(rowObject);
          stringBuilder.Append(stringValue);
        }
        stringBuilder.AppendLine("</td></tr>");
      }
      stringBuilder.AppendLine("</table>");
      return stringBuilder.ToString();
    }

    private string ConvertToHtmlFragment(string fragment)
    {
      string str1 = "http://www.codeproject.com/KB/list/ObjectListView.aspx";
      int length = string.Format("Version:1.0\r\nStartHTML:{0,8}\r\nEndHTML:{1,8}\r\nStartFragment:{2,8}\r\nEndFragment:{3,8}\r\nStartSelection:{2,8}\r\nEndSelection:{3,8}\r\nSourceURL:{4}\r\n{5}", (object) 0, (object) 0, (object) 0, (object) 0, (object) str1, (object) "").Length;
      string str2 = string.Format("<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\"><HTML><HEAD></HEAD><BODY><!--StartFragment-->{0}<!--EndFragment--></BODY></HTML>", (object) fragment);
      int num1 = length + str2.IndexOf(fragment);
      int num2 = num1 + fragment.Length;
      return string.Format("Version:1.0\r\nStartHTML:{0,8}\r\nEndHTML:{1,8}\r\nStartFragment:{2,8}\r\nEndFragment:{3,8}\r\nStartSelection:{2,8}\r\nEndSelection:{3,8}\r\nSourceURL:{4}\r\n{5}", (object) length, (object) (length + str2.Length), (object) num1, (object) num2, (object) str1, (object) str2);
    }
  }
}
