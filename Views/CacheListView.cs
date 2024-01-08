using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using WinCachebox.Geocaching;
using System.Data.Common;
using WeifenLuo.WinFormsUI.Docking;

namespace WinCachebox.Views
{
    public partial class CacheListView : DockContent
  {
    public static CacheListView View = null;
    private Cache aktCache = null;
    private bool updateSelectedCache = true;
    private DataGridViewRow notCurrentRow = null;

    string smartSearch = "";
    Color mixColor = Color.White;

    private WinCachebox.Views.WaypointView waypointView = null;
    [Browsable(true)]
    public WinCachebox.Views.WaypointView WaypointView { get { return waypointView; } set { waypointView = value; } }
    public CacheListView()
    {
      if (waypointView == null)
        View = this;
      InitializeComponent();
      if (waypointView == null) contextMenuStrip1 = null;
      //this.cTyp.HeaderText = "";
      //this.cFound.HeaderText = "";
      this.cGcCode.HeaderText = Global.Translations.Get("GcCode");
      this.cName.HeaderText = Global.Translations.Get("cachename");
      this.cDistance.HeaderText = Global.Translations.Get("Distance");
      this.cCoordinate.HeaderText = Global.Translations.Get("coordinate");
      this.cCategory.HeaderText = Global.Translations.Get("category");
      this.cFirstImported.HeaderText = Global.Translations.Get("FirstImported");
      this.cLastImported.HeaderText = Global.Translations.Get("LastImported");
      this.Country.HeaderText = Global.Translations.Get("country");
      this.State.HeaderText = Global.Translations.Get("state");

      this.setAsCenterToolStripMenuItem.Text = Global.Translations.Get("AsCenter");
      this.markAsFavoriteToolStripMenuItem.Text = Global.Translations.Get("Set/UnSetFavorite");

    }

    public void FilterChanged()
    {
      fillCacheList();
    }

    public void fillCacheList()
    {
      SortedList<long, string> gpxFilenames = new SortedList<long, string>();
      CBCommand query = Database.Data.CreateCommand("select ID, GPXFilename from GPXFilenames");
      DbDataReader reader = query.ExecuteReader();
      while (reader.Read())
      {
        long id = reader.GetInt64(0);
        string gpxFilename = reader.GetString(1);
        gpxFilenames.Add(id, gpxFilename);
      }
      if (dgCaches.CurrentRow != null)
          dgCaches.CurrentRow.Tag = null;
      dgCaches.Rows.Clear();

      int i = 1;
      Cache newSelected = null;
      int row2select = -1;
      foreach (Cache cache in Cache.Query)
      {
        dgCaches.Rows.Add(1);
        dgCaches.Rows[i - 1].Tag = cache;
        // the first one to select
        if (newSelected == null) { newSelected = cache; row2select = i - 1; };
        // restore previously selected if possible
        if (Global.SelectedCache != null && cache.GcCode.Equals(Global.SelectedCache.GcCode))
            { newSelected = Global.SelectedCache;  row2select = i - 1; };
        dgCaches.Rows[i - 1].Cells[2].Value = cache.GcCode;
        dgCaches.Rows[i - 1].Cells[3].Value = cache.Name;

        string gpxFilename = "---";
        if (gpxFilenames.ContainsKey(cache.GPXFilename_ID))
          gpxFilename = gpxFilenames[cache.GPXFilename_ID];
        dgCaches.Rows[i - 1].Cells[6].Value = gpxFilename;

        dgCaches.Rows[i - 1].Cells[9].Value = cache.Country;
        dgCaches.Rows[i - 1].Cells[10].Value = cache.State;

        i++;
      }
      this.Text = Global.Translations.Get("cacheList", "Cache List") + " (" + Cache.Query.Count + ")";
      if (row2select > -1)
      {
          dgCaches.Rows[0].Selected = false;
          dgCaches.Rows[row2select].Selected = true;
          dgCaches.CurrentCell = dgCaches.Rows[row2select].Cells[0];
      }
      Global.SelectedCache = newSelected;
    }

    public void changeBackgroundColor(Color color)
    {
        mixColor = color;
    }

    private class DistanceText : DevAge.Drawing.VisualElements.TextGDI
    {
      private string text;
      public DistanceText(String text)
      {
        this.text = text;
        Value = text;
      }
      protected override void OnDraw(DevAge.Drawing.GraphicsCache graphics, RectangleF area)
      {
        Value = text;
        base.OnDraw(graphics, area);
      }
    }

    private void CacheListView_Load(object sender, EventArgs e)
    {
      Global.TargetChanged += new Global.TargetChangedHandler(OnTargetChanged);
    }

    void OnTargetChanged(Cache cache, Waypoint waypoint)
    {
      updateSelectedCache = false;
      foreach (DataGridViewRow row in dgCaches.Rows)
      {
        if (row.Tag == Global.SelectedCache)
        {
          dgCaches.CurrentCell = row.Cells[0];
          break;
        }
      }
      updateSelectedCache = true;
    }

    public void RefreshDistances()
    {
      foreach (Cache cache in Cache.Query)
      {
        cache.Distance(true);
      }
      if (dgCaches.SortedColumn == cDistance)
      {
        dgCaches.Sort(cDistance, (dgCaches.SortOrder == SortOrder.Ascending) ? ListSortDirection.Ascending : ListSortDirection.Descending);
        dgCaches.FirstDisplayedScrollingRowIndex = dgCaches.CurrentRow.Index;
      }
    }

    private void setAsCenterToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Waypoint waypoint = Global.SelectedCache.GetFinalWaypoint;
      if (waypoint != null)
        Global.LastValidPosition = new Coordinate(waypoint.Coordinate);
      else
        Global.LastValidPosition = new Coordinate(Global.SelectedCache.Coordinate);
      Global.SetMarker(Global.LastValidPosition);
    }

    public void WpTypeChanged()
    {
      // can be called from outside when for example a final was added or deleted to change the Questionmark (red - blue)
      dgCaches.Refresh();
    }

    private void dgCaches_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
    {
      Cache cache1 = dgCaches.Rows[e.RowIndex1].Tag as Cache;
      Cache cache2 = dgCaches.Rows[e.RowIndex2].Tag as Cache;
      switch (e.Column.Index)
      {
        case 0:
          if (cache1.Type < cache2.Type)
              e.SortResult = -1;
          else if (cache1.Type > cache2.Type)
              e.SortResult = 1;
          else
          {
              bool hasfinal1 = cache1.HasFinalWaypoint;
              bool hasfinal2 = cache2.HasFinalWaypoint;
              if (hasfinal1 == hasfinal2)
                  e.SortResult = 0;
              else if (hasfinal1)
                  e.SortResult = 1;
              else
                  e.SortResult = -1;
          }
          e.Handled = true;
          break;
        case 4:
            if (cache1.CachedDistance < cache2.CachedDistance)
                e.SortResult = -1;
            else
                e.SortResult = 1;
            e.Handled = true;
          break;
        case 7:
          // FirstImported
          if (cache1.FirstImported < cache2.FirstImported)
            e.SortResult = -1;
          else if (cache2.FirstImported < cache1.FirstImported)
            e.SortResult = 1;
          else
            e.SortResult = 0;
          e.Handled = true;
          break;
        case 8:
          // LastImported
          DateTime last1 = Global.Categories.GetLastImported(cache1.GPXFilename_ID);
          DateTime last2 = Global.Categories.GetLastImported(cache2.GPXFilename_ID);
          if (last1 < last2)
            e.SortResult = -1;
          else if (last2 < last1)
            e.SortResult = 1;
          else
            e.SortResult = 0;
          e.Handled = true;
          break;
      }
    }

    private void dgCaches_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
    {
        if (e.RowIndex < 0)
            return;
        Cache cache = dgCaches.Rows[e.RowIndex].Tag as Cache;
        if (cache == null)
            return;
        if (e.PaintParts != DataGridViewPaintParts.All)
          return;
        bool archived = (cache.Archived || !cache.Available);
        switch (e.ColumnIndex)
        {
            case 0:
                Global.DrawListBackground(e.Graphics, e.CellBounds, dgCaches.Rows[e.RowIndex].Selected, (dgCaches.CurrentRow == dgCaches.Rows[e.RowIndex]) && (dgCaches.CurrentRow != notCurrentRow), mixColor);
                Bitmap image = Global.MapIcons[(int)cache.Type];
                if (cache.MysterySolved && !cache.Archived)
                {
                    image = Global.MapIcons[21];
                }
                if (image != null)
                {
                    ImageAttributes attr = new ImageAttributes();
                    attr.SetColorKey(Color.White, Color.White);
                    Global.PutImageTargetHeight(e.Graphics, image, e.CellBounds.X+2, e.CellBounds.Y+2, image.Height, attr);
                }

                if (image != null && cache.Favorit)
                {
                    ImageAttributes attr = new ImageAttributes();
                    attr.SetColorKey(Global.Icons[19].GetPixel(0, 0), Global.Icons[19].GetPixel(0, 0));
                    Global.PutImageTargetHeight(e.Graphics, Global.Icons[19], e.CellBounds.X + 2, e.CellBounds.Y + 2, 10, attr);
                }

                if (image != null && cache.Archived)
                {
                    ImageAttributes attr = new ImageAttributes();
                    attr.SetColorKey(Global.Icons[24].GetPixel(0, 0), Global.Icons[24].GetPixel(0, 0));
                    Global.PutImageTargetHeight(e.Graphics, Global.Icons[24], e.CellBounds.X + 12, e.CellBounds.Y + 8, 12, attr);
                }

                e.Handled = true;
                break;
            case 1:
                Global.DrawListBackground(e.Graphics, e.CellBounds, dgCaches.Rows[e.RowIndex].Selected, (dgCaches.CurrentRow == dgCaches.Rows[e.RowIndex]) && (dgCaches.CurrentRow != notCurrentRow), mixColor);
                image = null;
                Bitmap TBIcon = Global.Icons[0];
                if (cache.Owner == Config.GetString("GcLogin"))
                    image = Global.MapIcons[20];
                else if (cache.Found)
                  image = Global.MapIcons[19];
                else
                  image = new Bitmap(16, 16);
                if (image != null)
                {
                    ImageAttributes attr = new ImageAttributes();
                    attr.SetColorKey(Color.White, Color.White);
                    Global.PutImageTargetHeight(e.Graphics, image, e.CellBounds.X+2, e.CellBounds.Y+2, image.Height, attr);
                }

                if (cache.NumTravelbugs > 0)
                {
                    ImageAttributes attr = new ImageAttributes();
                    attr.SetColorKey(Color.White, Color.White);
                    Global.PutImageTargetHeight(e.Graphics, TBIcon, e.CellBounds.X + 25, e.CellBounds.Y + 3, TBIcon.Height, attr);

                    if (cache.NumTravelbugs > 1)
                    {
                        string n = " x " + cache.NumTravelbugs.ToString();
                        Font fontN = dgCaches.DefaultCellStyle.Font;
                        e.Graphics.DrawString(n, fontN, Brushes.Black, new PointF(e.CellBounds.Left + 35, e.CellBounds.Top + 2));
                    }

                }

                e.Handled = true;
                break;
            case 3:
                Global.DrawListBackground(e.Graphics, e.CellBounds, dgCaches.Rows[e.RowIndex].Selected, (dgCaches.CurrentRow == dgCaches.Rows[e.RowIndex]) && (dgCaches.CurrentRow != notCurrentRow), mixColor);
                string s = cache.Name;
                Font font = dgCaches.DefaultCellStyle.Font;
                if (archived)
                  font = new System.Drawing.Font(font, FontStyle.Strikeout);
                e.Graphics.DrawString(s, font, Brushes.Black, new PointF(e.CellBounds.Left+2, e.CellBounds.Top+2));
                e.Handled = true;
                break;
            case 4:
                Global.DrawListBackground(e.Graphics, e.CellBounds, dgCaches.Rows[e.RowIndex].Selected, (dgCaches.CurrentRow == dgCaches.Rows[e.RowIndex]) && (dgCaches.CurrentRow != notCurrentRow), mixColor);
                string dist = UnitFormatter.DistanceString(cache.CachedDistance);
                e.Graphics.DrawString(dist, dgCaches.DefaultCellStyle.Font, Brushes.Black, new PointF(e.CellBounds.Left+2, e.CellBounds.Top+2));
                e.Handled = true;
                break;
            case 5:
                Global.DrawListBackground(e.Graphics, e.CellBounds, dgCaches.Rows[e.RowIndex].Selected, (dgCaches.CurrentRow == dgCaches.Rows[e.RowIndex]) && (dgCaches.CurrentRow != notCurrentRow), mixColor);
                s = cache.Coordinate.FormatCoordinate();
                e.Graphics.DrawString(s, dgCaches.DefaultCellStyle.Font, Brushes.Black, new PointF(e.CellBounds.Left + 2, e.CellBounds.Top + 2));
                e.Handled = true;
                break;
            case 7:
                // FirstImported
                Global.DrawListBackground(e.Graphics, e.CellBounds, dgCaches.Rows[e.RowIndex].Selected, (dgCaches.CurrentRow == dgCaches.Rows[e.RowIndex]) && (dgCaches.CurrentRow != notCurrentRow), mixColor);
                s = cache.FirstImported.ToShortDateString();
                e.Graphics.DrawString(s, dgCaches.DefaultCellStyle.Font, Brushes.Black, new PointF(e.CellBounds.Left + 2, e.CellBounds.Top + 2));
                e.Handled = true;
                break;
            case 8:
                // LastImported
                Global.DrawListBackground(e.Graphics, e.CellBounds, dgCaches.Rows[e.RowIndex].Selected, (dgCaches.CurrentRow == dgCaches.Rows[e.RowIndex]) && (dgCaches.CurrentRow != notCurrentRow), mixColor);
                DateTime lastImported = Global.Categories.GetLastImported(cache.GPXFilename_ID);
                s = lastImported.ToShortDateString();
                e.Graphics.DrawString(s, dgCaches.DefaultCellStyle.Font, Brushes.Black, new PointF(e.CellBounds.Left + 2, e.CellBounds.Top + 2));
                e.Handled = true;
                break;
            default:
                Global.DrawListBackground(e.Graphics, e.CellBounds, dgCaches.Rows[e.RowIndex].Selected, (dgCaches.CurrentRow == dgCaches.Rows[e.RowIndex]) && (dgCaches.CurrentRow != notCurrentRow), mixColor);
                s = e.Value as string;
                e.Graphics.DrawString(s, dgCaches.DefaultCellStyle.Font, Brushes.Black, new PointF(e.CellBounds.Left+2, e.CellBounds.Top+2));
                e.Handled = true;

                break;
        }
    }

    private void dgCaches_SelectionChanged(object sender, EventArgs e)
    {
      timer1.Enabled = false;
      smartSearch = "";

      if (!updateSelectedCache)
        return;
      if (dgCaches.CurrentRow == null)
        return;
      Cache cache = dgCaches.CurrentRow.Tag as Cache;
      if (cache == null)
        return;
      if (cache != Global.SelectedCache)
      {
        // wenn der Cache einen Final Waypoint hat, soll dieser gleich aktiviert werden!
        Waypoint final = cache.GetFinalWaypoint;
        aktCache = cache;
        if (waypointView == null)
        {
          Global.SetSelectedWaypoint(cache, final);
        }
        else
        {
          waypointView.OnTargetChanged(cache, final);
        }
      }
      dgCaches.Focus();
    }

    private void dgCaches_Sorted(object sender, EventArgs e)
    {
      dgCaches.FirstDisplayedScrollingRowIndex = dgCaches.CurrentRow.Index;
    }

    private void dgCaches_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
    {
      if (dgCaches.CurrentRow == null)
        return;
      if (dgCaches.CurrentRow == e.Row)
        return;
      notCurrentRow = dgCaches.CurrentRow;
      dgCaches.InvalidateRow(dgCaches.CurrentRow.Index);
      notCurrentRow = null;
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
      smartSearch = "";
      timer1.Enabled = false;
    }

    private void dgCaches_KeyPress(object sender, KeyPressEventArgs e)
    {
      timer1.Enabled = false;
      smartSearch += e.KeyChar;
      string oldSmartSearch = smartSearch;
      bool found = false;
      for (int i = dgCaches.CurrentRow.Index; i < dgCaches.RowCount; i++)
      {
        Cache cache = dgCaches.Rows[i].Tag as Cache;
        if (cache == null)
          continue;
        if (cache.Name.ToLower().Contains(smartSearch.ToLower()))
        {
          dgCaches.CurrentCell = dgCaches.Rows[i].Cells[3];
          dgCaches_SelectionChanged(dgCaches, e);
          dgCaches.FirstDisplayedScrollingRowIndex = dgCaches.CurrentRow.Index;
          found = true;
          break;
        }
      }
      if (!found)
      {
        // search from the beginning
        for (int i = 0; i < dgCaches.CurrentRow.Index; i++)
        {
          Cache cache = dgCaches.Rows[i].Tag as Cache;
          if (cache == null)
            continue;
          if (cache.Name.ToLower().Contains(smartSearch.ToLower()))
          {
            dgCaches.CurrentCell = dgCaches.Rows[i].Cells[3];
            dgCaches_SelectionChanged(dgCaches, e);
            dgCaches.FirstDisplayedScrollingRowIndex = dgCaches.CurrentRow.Index;
            found = true;
            break;
          }
        }
      }
      smartSearch = oldSmartSearch;
      timer1.Enabled = true;
    }

    public string GetRegistryString()
    {
      string result = "";
      foreach (DataGridViewColumn col in dgCaches.Columns)
      {
        result += col.Index.ToString() + '#' + col.Width.ToString() + "#" + col.DisplayIndex.ToString() + "|";
      }
      return result;
    }

    public void SetRegistryString(string settings)
    {
      string[] cols = settings.Split('|');

      foreach (string col in cols)
      {
        string[] vals = col.Split('#');
        try
        {
          int index = -1;
          int width = -1;
          int displayIndex = -1;
          if (vals.Length > 0)
            index = Convert.ToInt32(vals[0]);
          if (vals.Length > 1)
            width = Convert.ToInt32(vals[1]);
          if (vals.Length > 2)
            displayIndex = Convert.ToInt32(vals[2]);

          if ((index >= 0) && (index < dgCaches.Columns.Count))
          {
            DataGridViewColumn column = dgCaches.Columns[index];
            if (width >= 0) column.Width = width;
            if (displayIndex >= 0) column.DisplayIndex = displayIndex;
          }
        }
        catch (Exception)
        {
        }
      }

    }

    private void markAsFavoriteToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (Global.SelectedCache != null)
            Global.SelectedCache.Favorit = !Global.SelectedCache.Favorit;
    }

    private void dgCaches_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {

    }
  }
}
