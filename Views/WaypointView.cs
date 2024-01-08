using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WinCachebox.Geocaching;
using WeifenLuo.WinFormsUI.Docking;

namespace WinCachebox.Views
{
    public partial class WaypointView : DockContent
  {
    public static WaypointView View;
    internal Cache aktCache = null;
    internal Waypoint aktWaypoint = null;
    bool ignoreSelectionChanged = false;
    private bool local = false;
    [Browsable(true)]
    public bool Local { get { return local; } set { local = value; } }
    public WaypointView()
    {
      if (!local)
        View = this;
      InitializeComponent();
      if (local)
        contextMenuStrip1 = null;

      gWaypoints.SelectionMode = SourceGrid.GridSelectionMode.Row;
      gWaypoints.Selection.EnableMultiSelection = false;
      Global.TargetChanged += new Global.TargetChangedHandler(OnTargetChanged);
      gWaypoints.Selection.SelectionChanged += new SourceGrid.RangeRegionChangedEventHandler(Selection_SelectionChanged);
    }

    public void OnTargetChanged(Cache cache, Waypoint waypoint)
    {
      if (ignoreSelectionChanged)
        return;
      if (cache == null) return;
      if ((aktCache != cache) || (gWaypoints.RowsCount != cache.Waypoints.Count + 1))
      {
        aktCache = cache;
        fillCacheList();
      }
      
      aktWaypoint = waypoint;
      SelectedCacheChanged();
    }

    void Selection_SelectionChanged(object sender, SourceGrid.RangeRegionChangedEventArgs e)
    {
      if (ignoreSelectionChanged)
        return;
      if (e.AddedRange == null)
        return;
      if (e.AddedRange.Count <= 0)
        return;
      if (e.AddedRange[0].Start.Row < 0)
        return;
      rowChanged(e.AddedRange[0].Start.Row);
    }

    private void rowChanged(int row)
    {
      ignoreSelectionChanged = true;
      Cache cache = gWaypoints.Rows[row].Tag as Cache;
      Waypoint waypoint = gWaypoints.Rows[row].Tag as Waypoint;
      if (cache != null)
      {
        if (!local)
        {
          if (Global.SelectedWaypoint != null)
            Global.SelectedWaypoint = null;
          if (cache != Global.SelectedCache)
            Global.SelectedCache = cache;
        }
        aktWaypoint = null;
      }
      else if ((waypoint != null) && (Global.SelectedWaypoint != waypoint))
      {
        if (!local)
        {
          Global.SelectedWaypoint = waypoint;
        }
        aktWaypoint = waypoint;
      }
      ignoreSelectionChanged = false;
      gWaypoints.Focus();
    }

    public void SelectedCacheChanged()
    {
      foreach (SourceGrid.GridRow row in gWaypoints.Rows)
      {
        if (row.Tag == null)
          continue;
        if (Global.SelectedWaypoint != null)
        {
          if ((row.Tag as Waypoint) == aktWaypoint)
          {
            SourceGrid.Position pos = new SourceGrid.Position(row.Index, 0);
            ignoreSelectionChanged = true;
            gWaypoints.Selection.Focus(pos, true);
            ignoreSelectionChanged = false;
          }
        }
        else
        {
          if ((row.Tag as Cache) == Global.SelectedCache)
          {
            SourceGrid.Position pos = new SourceGrid.Position(row.Index, 0);
            ignoreSelectionChanged = true;
            gWaypoints.Selection.Focus(pos, true);
            ignoreSelectionChanged = false;

          }
        }
      }
    }

    public void fillCacheList()
    {
      if (aktCache == null) return;
      List<Waypoint> waypoints = aktCache.Waypoints;

      gWaypoints.RowsCount = 0;
      gWaypoints.FixedRows = 0;
      gWaypoints.ColumnsCount = 1;
      gWaypoints.Columns[0].Width = 200;

      int i = 0;

      // zuerst Cache noch einfügen
      Cache cache = aktCache;
      gWaypoints.Rows.Insert(i);
      gWaypoints.Rows[i].Height = 32;
      gWaypoints.Rows[i].Tag = aktCache;
      SourceGrid.Cells.Cell cell = new SourceGrid.Cells.Cell();
      gWaypoints[i, 0] = cell; 
      cell.View = new CustomWaypointView(cache, null, cell, gWaypoints.Font);
      i++;

      foreach (Waypoint waypoint in waypoints)
      {
        gWaypoints.AutoStretchColumnsToFitWidth = true;
        gWaypoints.Rows.Insert(i);
        gWaypoints.Rows[i].Height = 32;
        gWaypoints.Rows[i].Tag = waypoint;
        cell = new SourceGrid.Cells.Cell();
        gWaypoints[i, 0] = cell;
        cell.View = new CustomWaypointView(cache, waypoint, cell, gWaypoints.Font);
        i++;
      }
      gWaypoints.AutoSizeCells();
      // ohne dass die Liste in der Größe geändert wird, wird die Bildlaufleiste nicht gezeigt.
      // nicht perfekt, funktioniert aber erstmal...
      gWaypoints.Width += 1;
      gWaypoints.Width -= 1;

/*      SourceGrid.Position pos = new SourceGrid.Position(1, 0);
      gCaches.Selection.SelectCell(pos, true);
      rowChanged(1);*/
    }

    private void WaypointView_Load(object sender, EventArgs e)
    {
    }

    
    /// <summary>
    /// Customized View to draw a Log
    /// </summary>
    private class CustomWaypointView : SourceGrid.Cells.Views.Cell
    {
      private Cache cache;
      private Waypoint waypoint;
      private Font font;
      SourceGrid.Cells.Cell cell;
      Bitmap bitmap = null;
      string Titel = "";
      string Coords = "";
      string Description = "";
      public CustomWaypointView(Cache cache, Waypoint waypoint, SourceGrid.Cells.Cell cell, Font font)
      {
        this.font = font;
        this.cache = cache;
        this.waypoint = waypoint;
        this.cell = cell;

        TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
        Border = DevAge.Drawing.RectangleBorder.NoBorder;
        this.WordWrap = true;
      }
      private void update()
      {
        if (waypoint == null)
        {
          // erster Eintrag, der Cache
          if (cache.Found)
            bitmap = Global.CacheIconsBigFound[(int)cache.Type];
          else
            bitmap = Global.CacheIconsBig[(int)cache.Type];
          if (cache.MysterySolved && !cache.Archived)
          {
            bitmap = Global.MapIcons[21];
          }
          Titel = cache.Name;
          Description = "";
          Coords = cache.Coordinate.FormatCoordinate();
        }
        else
        {
          // alle weiteren Waypoints
          bitmap = Global.CacheIconsBig[(int)waypoint.Type];
          Titel = waypoint.Title;
          Description = waypoint.Description;
          Coords = waypoint.Coordinate.FormatCoordinate();
        }
      }
      protected override SizeF OnMeasureContent(DevAge.Drawing.MeasureHelper measure, SizeF maxSize)
      {
        update();
        int cellWidth = cell.Grid.DisplayRectangle.Width;
        StringFormat sf = new System.Drawing.StringFormat();
        SizeF sizeF = new SizeF(cellWidth, 0);

        sizeF.Height += measure.Graphics.MeasureString(Titel, Global.boldFont, cellWidth - 36).Height;
        if(Description != "")
          sizeF.Height += measure.Graphics.MeasureString(Description, Global.normalFont, cellWidth - 36).Height;
        sizeF.Height += measure.Graphics.MeasureString(Coords, Global.normalFont, cellWidth - 36).Height;


        sizeF.Height = Math.Max(sizeF.Height, 32);
        return sizeF;
      }
      protected override void OnDraw(DevAge.Drawing.GraphicsCache graphics, RectangleF area)
      {
        update();
        base.OnDraw(graphics, area);
        if (bitmap != null)
          Global.PutImageTargetHeight(graphics.Graphics, bitmap, 0, (int)area.Y, 32);
        
        // write Title 
        RectangleF rect = area;
        rect.Width -= 36;
        rect.X += 36;
        graphics.Graphics.DrawString(Titel, Global.boldFont, Global.blackBrush, rect);
        rect.Y += (int)graphics.Graphics.MeasureString(Titel, Global.boldFont, (int)rect.Width).Height;
        // write Description
        if (Description != "")
        {
          graphics.Graphics.DrawString(Description, Global.normalFont, Global.blackBrush, rect);
          rect.Y += (int)graphics.Graphics.MeasureString(Description, Global.normalFont, (int)rect.Width).Height;
        }
        // write Coords 
        graphics.Graphics.DrawString(Coords, Global.normalFont, Global.blackBrush, rect);
        // Distance
        string dist = "";
        if (waypoint != null)
          dist = UnitFormatter.DistanceString(waypoint.Distance);
        else
          dist = UnitFormatter.DistanceString(cache.Distance(false));
        SizeF textSize = graphics.Graphics.MeasureString(dist, Global.normalFont);
        rect.X = rect.Left + rect.Width - 10 - textSize.Width;
        graphics.Graphics.DrawString(dist, Global.normalFont, Global.blackBrush, rect);
        
        graphics.Graphics.DrawLine(Global.blackPen, 0, area.Y + area.Height - 1, area.Width, area.Y + area.Height - 1);

      }
    }

    private void gWaypoints_Resize(object sender, EventArgs e)
    {
      gWaypoints.AutoSizeCells();
    }

    private void editToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (aktWaypoint != null)
      {
        if (Views.Forms.EditWaypoint.EditWaypointDialog(aktWaypoint, aktCache))
        {
          Global.SelectedWaypoint = aktWaypoint;
          aktWaypoint.UpdateDatabase();
          fillCacheList();
          SelectedCacheChanged();
          CacheListView.View.WpTypeChanged();
          if (aktWaypoint.Type == CacheTypes.Final)
          {
            // MysterySolutions anpassen
            for (int msi = 0; msi < Cache.MysterySolutions.Count; msi++)
            {
              Cache.MysterySolution sol = Cache.MysterySolutions[msi];
              if ((sol.Cache == aktCache) && (sol.Waypoint == aktWaypoint))
              {
                sol.Latitude = aktWaypoint.Latitude;
                sol.Longitude = aktWaypoint.Longitude;
              }
            }
          }
        }
      }
    }

    private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
    {
      miEdit.Enabled = aktWaypoint != null;
      miDelete.Enabled = aktWaypoint != null;
    }

    private void gWaypoints_DoubleClick(object sender, EventArgs e)
    {
      editToolStripMenuItem_Click(sender, e);
    }

    private void newWaypointToolStripMenuItem_Click(object sender, EventArgs e)
    {
      String newGcCode = Waypoint.CreateFreeGcCode(Global.SelectedCache.GcCode);
      Waypoint waypoint = new Waypoint(newGcCode, CacheTypes.ReferencePoint, "", 0, 0, aktCache.Id, "", "New Waypoint");
      if (aktWaypoint != null)
        waypoint.Coordinate = new Coordinate(aktWaypoint.Coordinate);
      else if (aktCache != null)
        waypoint.Coordinate = new Coordinate(aktCache.Coordinate);
      if (Views.Forms.EditWaypoint.EditWaypointDialog(waypoint, aktCache))
      {
        aktCache.Waypoints.Add(waypoint);
        aktWaypoint = waypoint;
        waypoint.WriteToDatabase();
        Global.SelectedWaypoint = waypoint;
        fillCacheList();
        SelectedCacheChanged();
        CacheListView.View.WpTypeChanged();
        // add new final to MysterySolutions
        if (waypoint.Type == CacheTypes.Final)
        {
                    // only add this waypoint to MysterySolutions when it is a Final
                    Cache.MysterySolution sol = new Cache.MysterySolution
                    {
                        Cache = aktCache,
                        Waypoint = waypoint,
                        Latitude = waypoint.Latitude,
                        Longitude = waypoint.Longitude
                    };
                    Cache.MysterySolutions.Add(sol);
        }
//        Cache.LoadMysterySolutions();
      }
    }

    private void löschenToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (aktWaypoint == null)
        return;
      if (MessageBox.Show("Delete the Waypoint " + aktWaypoint.Title + "?", Global.Translations.Get("delete"), MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
      {
        aktCache.Waypoints.Remove(aktWaypoint);
        aktWaypoint.DeleteFromDatabase();
        aktWaypoint = null;
        Global.SelectedWaypoint = null;
        fillCacheList();
        SelectedCacheChanged();
        CacheListView.View.RefreshDistances();
        CacheListView.View.WpTypeChanged();
//        Cache.LoadMysterySolutions();
        for (int msi = 0; msi < Cache.MysterySolutions.Count; msi++)
        {
          Cache.MysterySolution sol = Cache.MysterySolutions[msi];
          if ((sol.Cache == aktCache) && (sol.Waypoint == aktWaypoint))
          {
            Cache.MysterySolutions.RemoveAt(msi);
          }
        }

      }
    }

    private void setAsCenterToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (aktWaypoint != null)
        Global.LastValidPosition = new Coordinate(aktWaypoint.Coordinate);
      else
        Global.LastValidPosition = new Coordinate(aktCache.Coordinate);
      Global.SetMarker(Global.LastValidPosition);
    }

    private void projectionToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Views.Forms.ProjectionForm projectionForm = new Views.Forms.ProjectionForm(aktCache, aktWaypoint);
      if (projectionForm.ShowDialog() == DialogResult.OK)
      {
        Coordinate coord = projectionForm.Result;

        String newGcCode = Waypoint.CreateFreeGcCode(Global.SelectedCache.GcCode);
        Waypoint waypoint = new Waypoint(newGcCode, CacheTypes.ReferencePoint, "", coord.Latitude, coord.Longitude, aktCache.Id, "", "Projection");
        if (aktWaypoint != null)
          waypoint.Title = aktWaypoint.Title + " (projected)";
        else if (aktCache != null)
          waypoint.Title = aktCache.Name + " (projected)";
        waypoint.Title = projectionForm.Description + " (projected)";
        waypoint.Description = projectionForm.Notes(true);
        if (Views.Forms.EditWaypoint.EditWaypointDialog(waypoint, aktCache))
        {
          aktCache.Waypoints.Add(waypoint);
          aktWaypoint = waypoint;
          waypoint.WriteToDatabase();
          Global.SelectedWaypoint = waypoint;
          fillCacheList();
          SelectedCacheChanged();
          CacheListView.View.WpTypeChanged();
                    //          Cache.LoadMysterySolutions();
                    // add new final to MysterySolutions
                    Cache.MysterySolution sol = new Cache.MysterySolution
                    {
                        Cache = aktCache,
                        Waypoint = waypoint,
                        Latitude = waypoint.Latitude,
                        Longitude = waypoint.Longitude
                    };
                    Cache.MysterySolutions.Add(sol);
        }
      }
    }

    private void copyGcCodeToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (aktWaypoint == null)
            return;
        Clipboard.SetDataObject(aktWaypoint.GcCode, true);
    }

    private void copyCoordinatesToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (aktWaypoint == null)
            return;
        Clipboard.SetDataObject(aktWaypoint.Coordinate.FormatCoordinate(), true);

    }
  }

}
