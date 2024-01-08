using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SourceGrid;

namespace WinCachebox.SdfExport
{
    public partial class SDFBatchExport : Form
  {
    public static bool Export()
    {
      SDFBatchExport batch = new SDFBatchExport();
      return (batch.ShowDialog() == DialogResult.OK);
        // Harry1999: aukommentiert, da der code niemals erreicht werden kann... und deshalb ein Warning erzeugt
        // Global.Categories.ReadFromFilter(Global.LastFilter); 

      
    }

    private SdfExportSettings settings;
    private SdfExportSetting aktSetting = null;
    public SDFBatchExport()
    {
      InitializeComponent();
    
      settings = new SdfExportSettings();
      settings.Read();
      this.Text = button1.Text = Global.Translations.Get("batchExportImport", "_Batch Export/Import");
      bEdit.Text = "&" + Global.Translations.Get("edit", "_Edit");
      bNew.Text = "&" + Global.Translations.Get("new", "_New");
      bDelete.Text = "&" + Global.Translations.Get("delete");
      bImport.Text = "&" + Global.Translations.Get("import", "_Import");
      button1.Text = "&" + Global.Translations.Get("export", "_Export");
      button2.Text = "&" + Global.Translations.Get("cancel", "_Cancel");
    }

    private void fill()
    {
      if ((aktSetting == null) && (settings.Count > 0))
        aktSetting = settings[0];

      gBatch.RowsCount = 0;
      gBatch.FixedRows = 1;
     
      gBatch.ColumnsCount = 11;
      gBatch.Columns[0].Width = 100;
      gBatch.Columns[1].Width = 50;
      gBatch.Columns[2].Width = 40;
      gBatch.Columns[3].Width = 40;
      gBatch.Columns[4].Width = 20;
      gBatch.Columns[5].Width = 20;
      gBatch.Columns[6].Width = 20;
      gBatch.Columns[7].Width = 20;
      gBatch.Columns[8].Width = 20;
      gBatch.Columns[9].Width = 20;
      gBatch.Columns[10].Width = 100;

      gBatch.Rows.Insert(0);
      gBatch[0, 0] = new SourceGrid.Cells.ColumnHeader(Global.Translations.Get("Description", "_Description"));
      gBatch[0, 1] = new SourceGrid.Cells.ColumnHeader(Global.Translations.Get("Location", "_Location"));
      gBatch[0, 2] = new SourceGrid.Cells.ColumnHeader(Global.Translations.Get("max") + Global.Translations.Get("Distance"));
      gBatch[0, 3] = new SourceGrid.Cells.ColumnHeader(Global.Translations.Get("filter", "_Filter"));
      gBatch[0, 4] = new SourceGrid.Cells.ColumnHeader(Global.Translations.Get("database", "_Database"));
      gBatch[0, 5] = new SourceGrid.Cells.ColumnHeader(Global.Translations.Get("Images", "_Images"));
      gBatch[0, 6] = new SourceGrid.Cells.ColumnHeader(Global.Translations.Get("spoiler", "_Spoilers"));
      gBatch[0, 7] = new SourceGrid.Cells.ColumnHeader(Global.Translations.Get("Map", "_Maps"));
      gBatch[0, 8] = new SourceGrid.Cells.ColumnHeader(Global.Translations.Get("mapPack", "_MapPacks"));
      gBatch[0, 9] = new SourceGrid.Cells.ColumnHeader(Global.Translations.Get("ownRepos", "_Own Repos."));
      gBatch[0, 10] = new SourceGrid.Cells.ColumnHeader(Global.Translations.Get("export", "Export") + " " + Global.Translations.Get("path", "_path"));

      int i = 1;
      foreach (SdfExportSetting setting in settings)
      {
        gBatch.Rows.Insert(i);
        gBatch.Rows[i].Tag = setting;

        gBatch[i, 0] = new SourceGrid.Cells.Cell(setting.Description);
        string s = Global.Translations.Get("f26", "_Act. Position");
        if (setting.Location != null)
          s = setting.Location.Name;
        gBatch[i, 1] = new SourceGrid.Cells.Cell(s);
        s = Global.Translations.Get("all", "_all");
        if (setting.MaxDistance > 0)
            s = setting.MaxDistance.ToString() + " " + Global.Translations.Get("km", "_km");
        gBatch[i, 2] = new SourceGrid.Cells.Cell(s);
        gBatch[i, 3] = new SourceGrid.Cells.Cell(setting.cacheCountAproximate.ToString() + " " + Global.Translations.Get("Cache", "_Caches"));
        s = Global.Translations.Get("new", "_Create New");
        if (setting.Update)
            s = Global.Translations.Get("update", "_update Database");
        gBatch[i, 4] = new SourceGrid.Cells.Cell(s);

        s = Global.Translations.Get("no", "_No");
        if (setting.ExportImages)
            s = Global.Translations.Get("yes", "_Yes");
        gBatch[i, 5] = new SourceGrid.Cells.Cell(s);
        s = Global.Translations.Get("no", "_No");
        if (setting.ExportSpoilers)
            s = Global.Translations.Get("yes", "_Yes");
        gBatch[i, 6] = new SourceGrid.Cells.Cell(s);
        s = Global.Translations.Get("no", "_No");
        if (setting.ExportMaps)
            s = Global.Translations.Get("yes", "_Yes");
        gBatch[i, 7] = new SourceGrid.Cells.Cell(s);

        s = Global.Translations.Get("no", "_No");
        if (setting.ExportMapPacks)
            s = Global.Translations.Get("Yes", "_Yes");
        gBatch[i, 8] = new SourceGrid.Cells.Cell(s);

        s = Global.Translations.Get("no", "_No");
        if (setting.OwnRepository)
            s = Global.Translations.Get("yes", "_Yes");
        gBatch[i, 9] = new SourceGrid.Cells.Cell(s);

        gBatch[i, 10] = new SourceGrid.Cells.Cell(setting.ExportPath);

          i++;

      }
      gBatch.AutoSizeCells();

      foreach (SourceGrid.GridRow row in gBatch.Rows)
      {
        if (row.Tag == aktSetting)
        {
          SourceGrid.Position pos = new SourceGrid.Position(row.Index, 0);
          gBatch.Selection.Focus(pos, true);
        }
      }
    }

    private void SDFBatchExport_Load(object sender, EventArgs e)
    {
      gBatch.SelectionMode = SourceGrid.GridSelectionMode.Row;
      gBatch.Selection.EnableMultiSelection = true;
      gBatch.Selection.SelectionChanged += new SourceGrid.RangeRegionChangedEventHandler(Selection_SelectionChanged);
      foreach (SdfExportSetting setting in settings)
          setting.LoadCachesCount();
      fill();

      if (settings.Count == 0)
        bNew_Click(sender, e);
    }
  
    void Selection_SelectionChanged(object sender, SourceGrid.RangeRegionChangedEventArgs e)
    {
      if (e.AddedRange == null)
        return;
      if (e.AddedRange.Count <= 0)
        return;
      if (e.AddedRange[0].Start.Row < 1)
        return;
      rowChanged(e.AddedRange[0].Start.Row);
    }
  
    private void rowChanged(int row)
    {
      if (gBatch.Rows[row] == null)
        return;
      SdfExportSetting setting = gBatch.Rows[row].Tag as SdfExportSetting;
      if (setting == null)
        return;
      if (setting != aktSetting)
      {
        aktSetting = setting;
      }
    }

    private void bEdit_Click(object sender, EventArgs e)
    {
      if (aktSetting == null)
        return;
      if (aktSetting.Edit())
        fill();
    }

    private void bNew_Click(object sender, EventArgs e)
    {
      SdfExportSetting setting = new SdfExportSetting();
      if (setting.Edit())
      {
        settings.Add(setting);
        aktSetting = setting;
        fill();
      }
    }

    private void bDelete_Click(object sender, EventArgs e)
    {
      if (aktSetting == null)
        return;
      if (MessageBox.Show("Delete the SDFExport Setting [" + aktSetting.Description + "]?", Global.Translations.Get("delete"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
      {
        settings.Remove(aktSetting);
        aktSetting.Delete();
        if (settings.Count > 0)
        {
          aktSetting = settings[0];
        }
        fill();
      }
    }

    private void button1_Click(object sender, EventArgs e)
    {
        List<SdfExportSetting> list = new List<SdfExportSetting>();
        RangeRegion region = gBatch.Selection.GetSelectionRegion();
        foreach (Range range in region)
        {
            for (int i = range.Start.Row; i <= range.End.Row; i++)
            {
            SdfExportSetting setting = gBatch.Rows[i].Tag as SdfExportSetting;
            if (setting == null)
                continue;
            list.Add(setting);
            }
        }
        if (list.Count == 0)
            return;

        int importCount = 0;
        foreach (SdfExportSetting exsett in list)
        {
            int ic = exsett.Import(true);
            if (ic < 0)
            return;   // User clicked Cancel
            importCount += ic;
        }

        SdfExport export = new SdfExport(list);
        export.ShowDialog();
        if (importCount > 0)
            MessageBox.Show(importCount.ToString() + " changes imported!");
        WinCachebox.Geocaching.Cache.LoadCaches(Global.LastFilter.SqlWhere);
    }

    private void bImport_Click(object sender, EventArgs e)
    {
      List<SdfExportSetting> list = new List<SdfExportSetting>();
      RangeRegion region = gBatch.Selection.GetSelectionRegion();
      foreach (Range range in region)
      {
        for (int i = range.Start.Row; i <= range.End.Row; i++)
        {
          SdfExportSetting setting = gBatch.Rows[i].Tag as SdfExportSetting;
          if (setting == null)
            continue;
          list.Add(setting);
        }
      }
      if (list.Count == 0)
        return;
      int importCount = 0;
      foreach (SdfExportSetting setting in list)
      {
        importCount += setting.Import(false);
      }
      if (importCount > 0)
      {
        WinCachebox.Geocaching.Cache.LoadCaches(Global.LastFilter.SqlWhere);
        Views.CacheListView.View.fillCacheList();
        MessageBox.Show(importCount.ToString() + " changes imported!");
      }
/*      SdfExport export = new SdfExport(list);
      export.ShowDialog();*/
    }
  }
}
