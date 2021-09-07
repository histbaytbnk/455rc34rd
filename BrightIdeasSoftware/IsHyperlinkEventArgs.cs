
// Type: BrightIdeasSoftware.IsHyperlinkEventArgs


// Hacked by SystemAce

using System;

namespace BrightIdeasSoftware
{
  public class IsHyperlinkEventArgs : EventArgs
  {
    private ObjectListView listView;
    private object model;
    private OLVColumn column;
    private string text;
    private bool isHyperlink;
    public string Url;

    public ObjectListView ListView
    {
      get
      {
        return this.listView;
      }
      internal set
      {
        this.listView = value;
      }
    }

    public object Model
    {
      get
      {
        return this.model;
      }
      internal set
      {
        this.model = value;
      }
    }

    public OLVColumn Column
    {
      get
      {
        return this.column;
      }
      internal set
      {
        this.column = value;
      }
    }

    public string Text
    {
      get
      {
        return this.text;
      }
      internal set
      {
        this.text = value;
      }
    }

    public bool IsHyperlink
    {
      get
      {
        return this.isHyperlink;
      }
      set
      {
        this.isHyperlink = value;
      }
    }
  }
}
