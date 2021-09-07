
// Type: BrightIdeasSoftware.Design.ObjectListViewDesigner


// Hacked by SystemAce

using BrightIdeasSoftware;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace BrightIdeasSoftware.Design
{
  public class ObjectListViewDesigner : ControlDesigner
  {
    private ControlDesigner listViewDesigner;
    private IDesignerFilter designerFilter;
    private MethodInfo listViewDesignGetHitTest;
    private MethodInfo listViewDesignWndProc;

    public override DesignerActionListCollection ActionLists
    {
      get
      {
        DesignerActionListCollection actionLists = this.listViewDesigner.ActionLists;
        if (actionLists.Count > 0 && !(actionLists[0] is ObjectListViewDesigner.ListViewActionListAdapter))
          actionLists[0] = (DesignerActionList) new ObjectListViewDesigner.ListViewActionListAdapter(this, actionLists[0]);
        return actionLists;
      }
    }

    public override ICollection AssociatedComponents
    {
      get
      {
        ArrayList arrayList = new ArrayList(base.AssociatedComponents);
        arrayList.AddRange(this.listViewDesigner.AssociatedComponents);
        return (ICollection) arrayList;
      }
    }

    public override void Initialize(IComponent component)
    {
      Type type = Type.GetType("System.Windows.Forms.Design.ListViewDesigner, System.Design") ?? Type.GetType("System.Windows.Forms.Design.ListViewDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
      if (type == (Type) null)
        throw new ArgumentException("Could not load ListViewDesigner");
      this.listViewDesigner = (ControlDesigner) Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public, (Binder) null, (object[]) null, (CultureInfo) null);
      this.designerFilter = (IDesignerFilter) this.listViewDesigner;
      this.listViewDesignGetHitTest = type.GetMethod("GetHitTest", BindingFlags.Instance | BindingFlags.NonPublic);
      this.listViewDesignWndProc = type.GetMethod("WndProc", BindingFlags.Instance | BindingFlags.NonPublic);
      TypeDescriptor.CreateAssociation((object) component, (object) this.listViewDesigner);
      IServiceContainer serviceContainer = (IServiceContainer) component.Site;
      if (serviceContainer != null && this.GetService(typeof (DesignerCommandSet)) == null)
        serviceContainer.AddService(typeof (DesignerCommandSet), (object) new ObjectListViewDesigner.CDDesignerCommandSet((ComponentDesigner) this));
      this.listViewDesigner.Initialize(component);
      base.Initialize(component);
      this.RemoveDuplicateDockingActionList();
    }

    public override void InitializeNewComponent(IDictionary defaultValues)
    {
      base.InitializeNewComponent(defaultValues);
      this.listViewDesigner.InitializeNewComponent(defaultValues);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.listViewDesigner != null)
        this.listViewDesigner.Dispose();
      base.Dispose(disposing);
    }

    private void RemoveDuplicateDockingActionList()
    {
      FieldInfo field = typeof (ControlDesigner).GetField("dockingAction", BindingFlags.Instance | BindingFlags.NonPublic);
      if (!(field != (FieldInfo) null))
        return;
      DesignerActionList actionList = (DesignerActionList) field.GetValue((object) this);
      if (actionList == null)
        return;
      DesignerActionService designerActionService = (DesignerActionService) this.GetService(typeof (DesignerActionService));
      if (designerActionService == null)
        return;
      designerActionService.Remove((IComponent) this.Control, actionList);
    }

    protected override void PreFilterProperties(IDictionary properties)
    {
      base.PreFilterProperties(properties);
      this.designerFilter.PreFilterProperties(properties);
      List<string> list = new List<string>((IEnumerable<string>) new string[7]
      {
        "BackgroundImage",
        "BackgroundImageTiled",
        "HotTracking",
        "HoverSelection",
        "LabelEdit",
        "VirtualListSize",
        "VirtualMode"
      });
      foreach (string str in (IEnumerable) properties.Keys)
      {
        if (str.StartsWith("ToolTip"))
          list.Add(str);
      }
      if (this.Control is TreeListView)
        list.AddRange((IEnumerable<string>) new string[8]
        {
          "GroupImageList",
          "GroupWithItemCountFormat",
          "GroupWithItemCountSingularFormat",
          "HasCollapsibleGroups",
          "SpaceBetweenGroups",
          "ShowGroups",
          "SortGroupItemsByPrimaryColumn",
          "ShowItemCountOnGroups"
        });
      foreach (string str in list)
      {
        PropertyDescriptor property = TypeDescriptor.CreateProperty(typeof (ObjectListView), (PropertyDescriptor) properties[(object) str], (Attribute) new BrowsableAttribute(false));
        properties[(object) str] = (object) property;
      }
    }

    protected override void PreFilterEvents(IDictionary events)
    {
      base.PreFilterEvents(events);
      this.designerFilter.PreFilterEvents(events);
      List<string> list = new List<string>((IEnumerable<string>) new string[8]
      {
        "AfterLabelEdit",
        "BeforeLabelEdit",
        "DrawColumnHeader",
        "DrawItem",
        "DrawSubItem",
        "RetrieveVirtualItem",
        "SearchForVirtualItem",
        "VirtualItemsSelectionRangeChanged"
      });
      if (this.Control is TreeListView)
        list.AddRange((IEnumerable<string>) new string[6]
        {
          "AboutToCreateGroups",
          "AfterCreatingGroups",
          "BeforeCreatingGroups",
          "GroupTaskClicked",
          "GroupExpandingCollapsing",
          "GroupStateChanged"
        });
      foreach (string str in list)
      {
        EventDescriptor @event = TypeDescriptor.CreateEvent(typeof (ObjectListView), (EventDescriptor) events[(object) str], (Attribute) new BrowsableAttribute(false));
        events[(object) str] = (object) @event;
      }
    }

    protected override void PostFilterAttributes(IDictionary attributes)
    {
      this.designerFilter.PostFilterAttributes(attributes);
      base.PostFilterAttributes(attributes);
    }

    protected override void PostFilterEvents(IDictionary events)
    {
      this.designerFilter.PostFilterEvents(events);
      base.PostFilterEvents(events);
    }

    protected override bool GetHitTest(Point point)
    {
      return (bool) this.listViewDesignGetHitTest.Invoke((object) this.listViewDesigner, new object[1]
      {
        (object) point
      });
    }

    protected override void WndProc(ref Message m)
    {
      switch (m.Msg)
      {
        case 78:
        case 8270:
          this.listViewDesignWndProc.Invoke((object) this.listViewDesigner, new object[1]
          {
            (object) m
          });
          break;
        default:
          base.WndProc(ref m);
          break;
      }
    }

    private class ListViewActionListAdapter : DesignerActionList
    {
      private ObjectListViewDesigner designer;
      private DesignerActionList wrappedList;

      public ImageList LargeImageList
      {
        get
        {
          return ((ListView) this.Component).LargeImageList;
        }
        set
        {
          this.SetValue((object) this.Component, "LargeImageList", (object) value);
        }
      }

      public ImageList SmallImageList
      {
        get
        {
          return ((ListView) this.Component).SmallImageList;
        }
        set
        {
          this.SetValue((object) this.Component, "SmallImageList", (object) value);
        }
      }

      public View View
      {
        get
        {
          return ((ListView) this.Component).View;
        }
        set
        {
          this.SetValue((object) this.Component, "View", (object) value);
        }
      }

      public ListViewActionListAdapter(ObjectListViewDesigner designer, DesignerActionList wrappedList)
        : base(wrappedList.Component)
      {
        this.designer = designer;
        this.wrappedList = wrappedList;
      }

      public override DesignerActionItemCollection GetSortedActionItems()
      {
        DesignerActionItemCollection sortedActionItems = this.wrappedList.GetSortedActionItems();
        sortedActionItems.RemoveAt(2);
        sortedActionItems.RemoveAt(0);
        return sortedActionItems;
      }

      private void EditValue(ComponentDesigner componentDesigner, IComponent iComponent, string propertyName)
      {
        Type.GetType("System.Windows.Forms.Design.EditorServiceContext, System.Design").InvokeMember("EditValue", BindingFlags.Static | BindingFlags.InvokeMethod, (Binder) null, (object) null, new object[3]
        {
          (object) componentDesigner,
          (object) iComponent,
          (object) propertyName
        });
      }

      private void SetValue(object target, string propertyName, object value)
      {
        TypeDescriptor.GetProperties(target)[propertyName].SetValue(target, value);
      }

      public void InvokeColumnsDialog()
      {
        this.EditValue((ComponentDesigner) this.designer, this.Component, "Columns");
      }
    }

    private class CDDesignerCommandSet : DesignerCommandSet
    {
      private readonly ComponentDesigner componentDesigner;

      public CDDesignerCommandSet(ComponentDesigner componentDesigner)
      {
        this.componentDesigner = componentDesigner;
      }

      public override ICollection GetCommands(string name)
      {
        if (this.componentDesigner != null)
        {
          if (name.Equals("Verbs"))
            return (ICollection) this.componentDesigner.Verbs;
          if (name.Equals("ActionLists"))
            return (ICollection) this.componentDesigner.ActionLists;
        }
        return base.GetCommands(name);
      }
    }
  }
}
