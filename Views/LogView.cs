using System;
using System.Collections.Generic;
using System.Drawing;
using WinCachebox.Geocaching;
using WeifenLuo.WinFormsUI.Docking;

namespace WinCachebox.Views
{
    public partial class LogView : DockContent
  {
    public static LogView View = null;
    public Cache aktCache = null;
    public LogView()
    {
      View = this;
      InitializeComponent();
      Global.TargetChanged += new Global.TargetChangedHandler(OnTargetChanged);
      grid1.Selection.EnableMultiSelection = false;
    }

    public void SelectedCacheChanged()
    {
      fillLogs();
    }

    private void fillLogs()
    {
      if (Global.SelectedCache != null)
      {
        List<LogEntry> logs = Global.SelectedCache.Logs;
        grid1.AutoStretchColumnsToFitWidth = true;
        grid1.RowsCount = 0;
        grid1.FixedColumns = 0;
        grid1.FixedRows = 0;
        grid1.ColumnsCount = 1;
        int i = 0;
        foreach (LogEntry log in logs)
        {
          grid1.Rows.Insert(i);
          SourceGrid.Cells.Cell cell = new SourceGrid.Cells.Cell("");
          grid1[i, 0] = cell;
          grid1[i, 0].View = new CustomLogView(log, cell, grid1.Font);
          i++;
        }
        grid1.AutoSizeCells();
        // ohne dass die Liste in der Größe geändert wird, wird die Bildlaufleiste nicht gezeigt.
        // nicht perfekt, funktioniert aber erstmal...
        grid1.Width += 1;
        grid1.Width -= 1;
      }
    }

    void OnTargetChanged(Cache cache, Waypoint waypoint)
    {
      if (cache == aktCache)
        return;  // nur der Waypoint hat sich geändert...
      aktCache = cache;
      SelectedCacheChanged();
    }

    /// <summary>
    /// Customized View to draw a Log
    /// </summary>
    private class CustomLogView : SourceGrid.Cells.Views.Cell
    {
      private LogEntry logEntry;
      private Font font;
      int headerHeight = 18;
      SourceGrid.Cells.Cell cell;
      public CustomLogView(LogEntry logEntry, SourceGrid.Cells.Cell cell, Font font)
      {
        this.font = font;
        this.logEntry = logEntry;
        this.cell = cell;

        TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
//        base.Background = mBackground;
        Border = DevAge.Drawing.RectangleBorder.NoBorder;
        this.WordWrap = true;
      }
      protected override SizeF OnMeasureContent(DevAge.Drawing.MeasureHelper measure, SizeF maxSize)
      {
        int cellWidth = cell.Grid.DisplayRectangle.Width;
        StringFormat sf = new System.Drawing.StringFormat();
        SizeF sizeF = measure.Graphics.MeasureString(logEntry.Comment, font, cellWidth, sf);
        sizeF.Height += 20;
        sizeF.Width = cellWidth;
        return sizeF;
      }
      protected override void OnDraw(DevAge.Drawing.GraphicsCache graphics, RectangleF area)
      {
        base.OnDraw(graphics, area);

        graphics.Graphics.FillRectangle(Global.backBrushHead, area.X, area.Y, area.Width, headerHeight);
        int space = (logEntry.TypeIcon >= 0) ? Global.PutImageTargetHeight(graphics.Graphics, Global.LogIcons[logEntry.TypeIcon], 0, (int)area.Y, headerHeight) + 4 : 0;

        String dateString = logEntry.Timestamp.ToShortDateString();
        int dateWidth = (int)graphics.Graphics.MeasureString(dateString, font).Width;
        graphics.Graphics.DrawString(logEntry.Finder, Global.boldFont, Global.blackBrush, new RectangleF(space, area.Y, area.Width - space - dateWidth - 5, headerHeight));
        graphics.Graphics.DrawString(dateString, font, Global.blackBrush, area.Width - dateWidth, area.Y);
        graphics.Graphics.DrawLine(Global.blackPen, 0, area.Y + area.Height - 1, area.Width, area.Y + area.Height - 1);


        RectangleF rect = area;

        rect.Height -= headerHeight;
        rect.Offset(0, headerHeight);
        graphics.Graphics.DrawString(logEntry.Comment, font, Brushes.Black, rect);
      }
/*
      private BackVisualElement mBackground = new BackVisualElement();

      private class BackVisualElement : DevAge.Drawing.VisualElements.VisualElementBase
      {
        private LogEntry logEntry;
        #region Constuctor
        public BackVisualElement()
        {
//          this.logEntry = logEntry;
        }

        public BackVisualElement(BackVisualElement other)
          : base(other)
        {
          Round = other.Round;
        }
        #endregion

        public override object Clone()
        {
          return new BackVisualElement(this);
        }

        private double mRound = 0.5;
        public double Round
        {
          get { return mRound; }
          set { mRound = value; }
        }

        protected override void OnDraw(DevAge.Drawing.GraphicsCache graphics, RectangleF area)
        {
          DevAge.Drawing.RoundedRectangle rounded = new DevAge.Drawing.RoundedRectangle(Rectangle.Round(area), Round);
          using (System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(area, Color.RoyalBlue, Color.WhiteSmoke, System.Drawing.Drawing2D.LinearGradientMode.Vertical))
          {
            graphics.Graphics.FillRegion(brush, new Region(rounded.ToGraphicsPath()));
          }
          //Border
          DevAge.Drawing.Utilities.DrawRoundedRectangle(graphics.Graphics, rounded, Pens.RoyalBlue);
        }

        protected override SizeF OnMeasureContent(DevAge.Drawing.MeasureHelper measure, SizeF maxSize)
        {
          return SizeF.Empty;
        }
      }*/
    }

    private void grid1_Resize(object sender, EventArgs e)
    {
      grid1.AutoSizeCells();
    }
  }
}
