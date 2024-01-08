using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using WinCachebox.Geocaching;

namespace WinCachebox.SdfExport
{
    public partial class SdfExportSettingsForm : Form
  {
    private SdfExportSetting sdfExportSetting;
    private FilterProperties tmpFilter;
    List<Cache> caches = new List<Cache>();

    public SdfExportSettingsForm(SdfExportSetting sdfExportSetting)
    {
        this.sdfExportSetting = sdfExportSetting;
        if (sdfExportSetting.Filter != null)
            tmpFilter = new FilterProperties(sdfExportSetting.Filter.ToString());
        else
            tmpFilter = new FilterProperties(Global.LastFilter.ToString());

        InitializeComponent();
        this.Text = Global.Translations.Get("database", "_Database") + " " + Global.Translations.Get("export", "_Export");
        label1.Text = Global.Translations.Get("Description", "_Description") + ":";
        label2.Text = Global.Translations.Get("filter", "_Filter") + " " + Global.Translations.Get("settings", "_Settings") + ":";
        bFilter.Text = Global.Translations.Get("filter", "_Filter");
        label3.Text = Global.Translations.Get("max") + Global.Translations.Get("Distance") + ":";
        label5.Text = Global.Translations.Get("km", "_km") + " (0 = " + Global.Translations.Get("all", "_all") + ")     " + Global.Translations.Get("from", "_from");
        gbDatabase.Text = Global.Translations.Get("database", "_Database") + ":";
        chkOwnRepository.Text = Global.Translations.Get("ownRepos", "_own Repository");
        chkUpdate.Text = Global.Translations.Get("update", "_Update");
        gbExport.Text = Global.Translations.Get("export", "_Export") + ":"; ;
        chkImages.Text = Global.Translations.Get("Images", "_Images");
        chkSpoilers.Text = Global.Translations.Get("spoiler", "_Spoiler");
        chkMaps.Text = Global.Translations.Get("Map", "_Maps");
        chkMapPacks.Text = Global.Translations.Get("mapPack", "_mapPacks");
        lbMaxLogs.Text = Global.Translations.Get("max", "_Max. ") + Global.Translations.Get("logs", "_Logs") + ":"; ;
        lb0forall.Text = " (0 = " + Global.Translations.Get("all", "_all") + ")";
        button1.Text = Global.Translations.Get("ok", "_Ok");
        button2.Text = Global.Translations.Get("cancel", "_Cancel");
    }

    private void calcNumCaches()
    {
      double maxDistance = 0;
      try
      {
        maxDistance = Convert.ToDouble(tbMaxDistance.Text);
      }
      catch (Exception)
      {
        maxDistance = 0;
      }
      int num = 0;
      // Marker merken
      Coordinate lastMarker = Global.Marker;
      if (cbLocation.SelectedIndex > 0)
      {
        // Location als Marker setzen für Distanz-Berechnung 
        Global.Marker = Geocaching.Location.Locations[cbLocation.SelectedIndex - 1].Coordinate;
      }
      sdfExportSetting.Caches.Clear();
      foreach (Cache cache in caches)
      {
        // Distanz überprüfen
        if ((maxDistance > 0) && (cache.Distance(true) > maxDistance * 1000))
          continue;
        num++;
        sdfExportSetting.Caches.Add(cache);
      }
      gbSelection.Text = Global.Translations.Get("selection", "_Selection") + ": "  + num.ToString() + " " + Global.Translations.Get("Cache", "_Cache");
      // lastMarker wiederherstellen
      Global.Marker = lastMarker;
    }

    private void SdfExportSettingsForm_Load(object sender, EventArgs e)
    {
        if (!sdfExportSetting.SaveToDatabase)
        {
            label1.Visible = false;
            tbDescription.Visible = false;
        }
        if (sdfExportSetting.Update)
            chkUpdate.Checked = true;
        else
            chkUpdate.Checked = false;

        chkImages.Checked = sdfExportSetting.ExportImages;
        chkSpoilers.Checked = sdfExportSetting.ExportSpoilers;
        chkMaps.Checked = sdfExportSetting.ExportMaps;
        chkMapPacks.Checked = sdfExportSetting.ExportMapPacks;

        if (true || chkMapPacks.Checked)
        {
            foreach (Map.Pack pack in WinCachebox.Views.MapView.Manager.mapPacks)
            {
                if (!lstMapPacks.Items.Contains(pack.Layer.Name) && pack.Layer.Name != "Mapnik" && pack.Layer.Name != "OpenTopoMap" && pack.Layer.Name != "OSM Cycle Map" && pack.Layer.Name != "Stamen")
                {
                    if (sdfExportSetting.MapPacks.Contains(pack.Layer.Name))
                        lstMapPacks.Items.Add(pack.Layer.Name, true);
                    else
                        lstMapPacks.Items.Add(pack.Layer.Name, false);
                }
            }
        }

        chkOwnRepository.Checked = sdfExportSetting.OwnRepository;

        tbDescription.Text = sdfExportSetting.Description;

        tbExportPfad.Text = sdfExportSetting.ExportPath;
        
        tbMaxDistance.Text = sdfExportSetting.MaxDistance.ToString();
        cbLocation.Items.Add(Global.Translations.Get("f26", "_akt. Center"));
        foreach (Location location in Geocaching.Location.Locations)
        {
            cbLocation.Items.Add(location.Name);
            if (sdfExportSetting.Location == location)
                cbLocation.SelectedIndex = cbLocation.Items.Count - 1;
        }
        if (cbLocation.SelectedIndex < 0)
            cbLocation.SelectedIndex = 0;
        nudMaxLogs.Value = sdfExportSetting.MaxLogs;

        Global.Categories.ReadFromFilter(tmpFilter);
        Global.Categories.WriteToFilter(tmpFilter);
        Cache.LoadCaches(tmpFilter.SqlWhere, caches);
        calcNumCaches();
    }

    private void bSetDestination_Click(object sender, EventArgs e)
    {
        fsdExport.DefaultExt = "db3";
        fsdExport.FileName = "CacheBox.db3";
        fsdExport.Filter = "Android DB|*.db3|CacheBox DB|*.sdf";

        if (File.Exists(tbExportPfad.Text))
        {
            fsdExport.FileName = Path.GetFileName(tbExportPfad.Text);
            fsdExport.DefaultExt = Path.GetExtension(tbExportPfad.Text);
            if (fsdExport.DefaultExt.Equals("db3")) fsdExport.FilterIndex = 2;
            fsdExport.InitialDirectory = Path.GetDirectoryName(tbExportPfad.Text);
        }

      if (fsdExport.ShowDialog() != System.Windows.Forms.DialogResult.OK)
        return;
      tbExportPfad.Text = fsdExport.FileName;
    }

    private void bFilter_Click(object sender, EventArgs e)
    {
      Views.FilterForm ffilter = new Views.FilterForm(tmpFilter);
      if (ffilter.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
          tmpFilter = ffilter.getFilter();
          Cache.LoadCaches(tmpFilter.SqlWhere, caches);
          calcNumCaches();
      }
    }

    private void button1_Click(object sender, EventArgs e)
    {
        double maxDistance = 0;
        try
        {
            maxDistance = Convert.ToDouble(tbMaxDistance.Text);
        }
        catch (Exception)
        {
            maxDistance = 0;
        }
        if (tbExportPfad.Text == "")
        {
            bSetDestination_Click(sender, e);
            if (tbExportPfad.Text == "")
            {
                DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }

        }

        // Einstellungen in sdfExportSettings speichern
        sdfExportSetting.Description = tbDescription.Text;
        sdfExportSetting.ExportPath = tbExportPfad.Text;
        sdfExportSetting.MaxDistance = maxDistance;
        sdfExportSetting.Filter = tmpFilter;
        sdfExportSetting.Location = null;
        if (cbLocation.SelectedIndex > 0)
            sdfExportSetting.Location = Geocaching.Location.Locations[cbLocation.SelectedIndex - 1];
        sdfExportSetting.Update = chkUpdate.Checked;

        sdfExportSetting.ExportImages = chkImages.Checked;
        sdfExportSetting.ExportSpoilers = chkSpoilers.Checked;
        sdfExportSetting.ExportMaps = chkMaps.Checked;
        sdfExportSetting.OwnRepository = chkOwnRepository.Checked;
        sdfExportSetting.ExportMapPacks = chkMapPacks.Checked;

        sdfExportSetting.MapPacks = "";
        foreach (string item in lstMapPacks.CheckedItems)
            sdfExportSetting.MapPacks += item + "|";
        sdfExportSetting.MaxLogs = (int)nudMaxLogs.Value;
        if (sdfExportSetting.SaveToDatabase)
            sdfExportSetting.Write();
    }

    private void tbMaxDistance_TextChanged(object sender, EventArgs e)
    {
        calcNumCaches();
    }

    private void cbLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
      calcNumCaches();
    }

    private void chkMapPacks_CheckedChanged(object sender, EventArgs e)
    {
        lstMapPacks.Enabled = chkMapPacks.Checked;
    }

  }
}
