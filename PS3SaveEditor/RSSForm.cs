﻿
// Type: PS3SaveEditor.RSSForm


// Hacked by SystemAce

using PS3SaveEditor.Resources;
using Rss;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PS3SaveEditor
{
  public class RSSForm : Form
  {
    private IContainer components;
    private Panel panel1;
    private ListBox lstRSSFeeds;
    private Button btnOk;
    private Panel panel2;
    private LinkLabel lnkTitle;
    private WebBrowser webBrowser1;
    private Label lblTitle;

    public RSSForm(RssChannel channel)
    {
      this.InitializeComponent();
      this.BackColor = Color.FromArgb(80, 29, 11);
      this.panel1.BackColor = Color.FromArgb((int) sbyte.MaxValue, 204, 204, 204);
      this.lstRSSFeeds.DrawMode = DrawMode.OwnerDrawFixed;
      this.lstRSSFeeds.DrawItem += new DrawItemEventHandler(this.lstRSSFeeds_DrawItem);
      this.CenterToScreen();
      this.Text = Util.PRODUCT_NAME;
      this.Load += new EventHandler(this.RSSForm_Load);
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.LostFocus += new EventHandler(this.RSSForm_LostFocus);
      this.lstRSSFeeds.SelectedIndexChanged += new EventHandler(this.lstRSSFeeds_SelectedIndexChanged);
      this.lnkTitle.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnkTitle_LinkClicked);
      try
      {
        if (channel.Items.Count > 0)
          this.lstRSSFeeds.DataSource = (object) channel.Items;
        else
          this.lstRSSFeeds.DataSource = (object) null;
      }
      catch (Exception ex)
      {
      }
    }

    private void lstRSSFeeds_DrawItem(object sender, DrawItemEventArgs e)
    {
      if (e.Index < 0)
        return;
      e.DrawBackground();
      if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
      {
        e = new DrawItemEventArgs(e.Graphics, e.Font, e.Bounds, e.Index, e.State ^ DrawItemState.Selected, e.ForeColor, Color.FromArgb(0, 175, (int) byte.MaxValue));
        e.Graphics.DrawString(this.lstRSSFeeds.Items[e.Index].ToString(), e.Font, Brushes.White, (RectangleF) e.Bounds, StringFormat.GenericDefault);
      }
      else
        e.Graphics.DrawString(this.lstRSSFeeds.Items[e.Index].ToString(), e.Font, Brushes.Black, (RectangleF) e.Bounds, StringFormat.GenericDefault);
      e.DrawFocusRectangle();
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
      using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(this.ClientRectangle, Color.FromArgb(0, 138, 213), Color.FromArgb(0, 44, 101), 90f))
        e.Graphics.FillRectangle((Brush) linearGradientBrush, this.ClientRectangle);
    }

    private void lnkTitle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      Process.Start(new ProcessStartInfo(this.lnkTitle.Links[0].LinkData as string));
    }

    private void RSSForm_LostFocus(object sender, EventArgs e)
    {
      this.Focus();
    }

    private void lstRSSFeeds_SelectedIndexChanged(object sender, EventArgs e)
    {
      RssItem rssItem = (RssItem) this.lstRSSFeeds.SelectedItem;
      if (rssItem.Link != (Uri) null)
      {
        this.lnkTitle.Text = rssItem.Title;
        this.lnkTitle.Links.Clear();
        this.lnkTitle.Links.Add(0, rssItem.Title.Length, (object) rssItem.Link.ToString());
        this.lnkTitle.Visible = true;
        this.lblTitle.Visible = false;
      }
      else
      {
        this.lblTitle.Text = rssItem.Title;
        this.lnkTitle.Visible = false;
        this.lblTitle.Visible = true;
      }
      this.webBrowser1.DocumentText = "<html><body style='font-size:12px;overflow-y:auto'>" + rssItem.Description + "</body></html>";
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetForegroundWindow(IntPtr hwnd);

    private void RSSForm_Load(object sender, EventArgs e)
    {
      if (this.lstRSSFeeds.DataSource == null)
      {
        this.Close();
      }
      else
      {
        this.Show();
        RSSForm.SetForegroundWindow(this.Handle);
      }
    }

    private void RSSForm_ResizeEnd(object sender, EventArgs e)
    {
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panel1 = new Panel();
      this.panel2 = new Panel();
      this.lblTitle = new Label();
      this.webBrowser1 = new WebBrowser();
      this.lnkTitle = new LinkLabel();
      this.btnOk = new Button();
      this.lstRSSFeeds = new ListBox();
      this.panel1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panel1.BackColor = Color.FromArgb(102, 102, 102);
      this.panel1.Controls.Add((Control) this.panel2);
      this.panel1.Controls.Add((Control) this.btnOk);
      this.panel1.Controls.Add((Control) this.lstRSSFeeds);
      this.panel1.Location = new Point(10, 10);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(604, 420);
      this.panel1.TabIndex = 0;
      this.panel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panel2.BackColor = Color.White;
      this.panel2.BorderStyle = BorderStyle.FixedSingle;
      this.panel2.Controls.Add((Control) this.lblTitle);
      this.panel2.Controls.Add((Control) this.webBrowser1);
      this.panel2.Controls.Add((Control) this.lnkTitle);
      this.panel2.Location = new Point(12, 97);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(581, 284);
      this.panel2.TabIndex = 2;
      this.lblTitle.AutoSize = true;
      this.lblTitle.Font = new Font("Microsoft Sans Serif", 14f, FontStyle.Bold);
      this.lblTitle.ForeColor = Color.Black;
      this.lblTitle.Location = new Point(9, 10);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Size = new Size(0, 24);
      this.lblTitle.TabIndex = 4;
      this.webBrowser1.AllowWebBrowserDrop = false;
      this.webBrowser1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.webBrowser1.IsWebBrowserContextMenuEnabled = false;
      this.webBrowser1.Location = new Point(3, 40);
      this.webBrowser1.MinimumSize = new Size(20, 20);
      this.webBrowser1.Name = "webBrowser1";
      this.webBrowser1.ScriptErrorsSuppressed = true;
      this.webBrowser1.Size = new Size(571, 240);
      this.webBrowser1.TabIndex = 3;
      this.lnkTitle.AutoSize = true;
      this.lnkTitle.Font = new Font("Microsoft Sans Serif", 14f, FontStyle.Bold);
      this.lnkTitle.ForeColor = Color.White;
      this.lnkTitle.Location = new Point(9, 10);
      this.lnkTitle.Name = "lnkTitle";
      this.lnkTitle.Size = new Size(0, 24);
      this.lnkTitle.TabIndex = 2;
      this.btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnOk.Location = new Point(263, 389);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(75, 23);
      this.btnOk.TabIndex = 1;
      this.btnOk.Text = "OK";
      this.btnOk.UseVisualStyleBackColor = false;
      this.lstRSSFeeds.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lstRSSFeeds.FormattingEnabled = true;
      this.lstRSSFeeds.IntegralHeight = false;
      this.lstRSSFeeds.Location = new Point(12, 12);
      this.lstRSSFeeds.Name = "lstRSSFeeds";
      this.lstRSSFeeds.Size = new Size(581, 82);
      this.lstRSSFeeds.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(96f, 96f);
      this.AutoScaleMode = AutoScaleMode.Dpi;
      this.BackColor = Color.Black;
      this.ClientSize = new Size(624, 442);
      this.Controls.Add((Control) this.panel1);
      this.FormBorderStyle = FormBorderStyle.Fixed3D;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "RSSForm";
      this.SizeGripStyle = SizeGripStyle.Hide;
      this.Text = "RSSForm";
      this.ResizeEnd += new EventHandler(this.RSSForm_ResizeEnd);
      this.panel1.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
