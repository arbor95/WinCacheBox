using System;
using System.IO;
using System.Windows.Forms;

namespace WinCachebox.Views.Forms
{
    public partial class Settings : Form
  {
    public Settings()
    {
      InitializeComponent();
      this.Text = Global.Translations.Get("settings", "settings");
        
    }

    private void Settings_Load(object sender, EventArgs e)
    {
        Fill_comboBoxLanguage();

        tbLogin.Text = Config.GetString("GcLogin");
//        txtAccessToken.Text = Config.GetString("accessTokenEnc");
        tbGcVotePassword.Text = Config.GetStringEncrypted("GcVotePassword");

        // Map page
        checkBoxImportOsm.Checked = Config.GetBool("ImportLayerOsm");
        checkBoxImportOTM.Checked = Config.GetBool("ImportLayerOTM");
        checkBoxImportOcm.Checked = Config.GetBool("ImportLayerOcm");
        checkBoxImportMQ.Checked = Config.GetBool("ImportLayerMQ");

        comboBoxMinLevel.SelectedIndex = Config.GetInt("OsmMinLevel");
        comboBoxMaxLevel.SelectedIndex = Config.GetInt("OsmMaxLevel");
        comboBoxMaxImportLevel.SelectedIndex = Config.GetInt("OsmMaxImportLevel");
        checkBoxDpiAwareRendering.Checked = Config.GetBool("OsmDpiAwareRendering");
        comboBoxZoomCross.Text = Config.GetString("ZoomCross");

        int coverage = Config.GetInt("OsmCoverage");
        switch (coverage)
        {
            case 250:
                comboBoxCoverage.SelectedIndex = 0;
                break;
            case 500:
                comboBoxCoverage.SelectedIndex = 1;
                break;
            case 1000:
                comboBoxCoverage.SelectedIndex = 2;
                break;
            case 2000:
                comboBoxCoverage.SelectedIndex = 3;
                break;
            case 5000:
                comboBoxCoverage.SelectedIndex = 4;
                break;
        }

        txtSpoilerTags.Text = Config.GetString("SpoilersDescriptionTags");
        cbImportLogImages.Checked = Config.GetBool("ImportLogImages");
        chkSpoilerRotate.Checked = Config.GetBool("ExportSpoilersRotate");
        numSpoilerMax.Value = Config.GetInt("ExportSpoilersMaxPixels");
        numImagesMax.Value = Config.GetInt("ExportImagesMaxPixels");

        // Logs
        int logLife = Config.GetInt("LogMaxMonthAge");
        int logMinCount = Config.GetInt("LogMinCount");
        tabPage1.Text = Global.Translations.Get("Misc", "Misc");
        tabPage2.Text = Global.Translations.Get("Map", "Map");
        tabPage3.Text = Global.Translations.Get("Images", "Images");
        tabPage4.Text = Global.Translations.Get("Logs", "Logs");
        label1.Text = Global.Translations.Get("gcUsername", "Login at Geocaching.com");
        button3.Text = Global.Translations.Get("GetAPIKey", "Get API Key");
        label3.Text = Global.Translations.Get("GcVotePassword", "GcVote Password");
        lSelLang.Text = Global.Translations.Get("SelectLanguage", "Select Language:");
        button1.Text = Global.Translations.Get("ok", "OK");
        button2.Text = Global.Translations.Get("cancel", "Cancel");
        label4.Text = Global.Translations.Get("MapDetailLevels", "Map Detail Levels");
        label5.Text = Global.Translations.Get("Layers", "Layers");
        checkBoxImportOsm.Text = Global.Translations.Get("Mapnik", "Mapnik");
        checkBoxImportOTM.Text = Global.Translations.Get("OpenTopoMap", "OpenTopoMap");
        checkBoxImportOcm.Text = Global.Translations.Get("CycleMap", "Cycle Map");
        checkBoxImportMQ.Text = "Stamen";
        label6.Text = Global.Translations.Get("Min", "Min");
        label7.Text = Global.Translations.Get("Import", "Import");
        label8.Text = Global.Translations.Get("Max", "Max");
        label9.Text = Global.Translations.Get("Area", "Area");
        checkBoxDpiAwareRendering.Text = Global.Translations.Get("dpiRendering", "DPI-aware rendering");
        label10.Text = Global.Translations.Get("ShowCross", "Show cross when zoom level >");
        label11.Text = Global.Translations.Get("SpoilerTags", "Spoiler Tags to Load (split with ;)");
        label13.Text = Global.Translations.Get("SpoilerMax", "Spoiler Export Max Size (px)");
        label14.Text = Global.Translations.Get("ImagesMax", "Images Export Max Size (px)");
        chkSpoilerRotate.Text = Global.Translations.Get("SpoilerRotate", "Spoiler Export Rotate?");
        cbDeleteLogs.Text = Global.Translations.Get("DeleteLogs", "Delete Logs");
        label12.Text = Global.Translations.Get("KeepLogs", "but keep at least");
        cbLogLife.Items.Clear();
        cbLogLife.Items.Add(Global.Translations.Get("immediately", "immediately"));
        cbLogLife.Items.Add(Global.Translations.Get("1month", "after 1 month"));
        cbLogLife.Items.Add(Global.Translations.Get("2months", "after 2 months"));
        cbLogLife.Items.Add(Global.Translations.Get("3months", "after 3 months"));
        cbLogLife.Items.Add(Global.Translations.Get("4months", "after 4 months"));
        cbLogLife.Items.Add(Global.Translations.Get("5months", "after 5 months"));
        cbLogLife.Items.Add(Global.Translations.Get("6months", "after 6 months"));
        cbMinLogs.Items.Clear();
        cbMinLogs.Items.Add(Global.Translations.Get("0logs", "0 logs"));
        cbMinLogs.Items.Add(Global.Translations.Get("5logs", "5 logs"));
        cbMinLogs.Items.Add(Global.Translations.Get("10logs", "10 logs"));
        cbMinLogs.Items.Add(Global.Translations.Get("15logs", "15 logs"));
        cbMinLogs.Items.Add(Global.Translations.Get("20logs", "20 logs"));
        cbMinLogs.Items.Add(Global.Translations.Get("25logs", "25 logs"));
        // must be behind translation
        if (logLife == 99999)
        {
            cbDeleteLogs.Checked = false;
            cbLogLife.Enabled = false;
            cbLogLife.SelectedIndex = cbLogLife.Items.Count - 1;
            cbMinLogs.Enabled = false;
            cbMinLogs.SelectedIndex = cbMinLogs.Items.Count - 1;
        }
        else
        {
            cbDeleteLogs.Checked = true;
            cbLogLife.Enabled = true;
            cbLogLife.SelectedIndex = logLife;
            cbMinLogs.Enabled = true;
            cbMinLogs.SelectedIndex = logMinCount;
        }
    }

    private void button1_Click(object sender, EventArgs e)
    {
        Config.Set("GcLogin", tbLogin.Text);
//        Config.Set("accessTokenEnc", txtAccessToken.Text);
        Config.SetEncrypted("GcVotePassword", tbGcVotePassword.Text);

        // Map page
        if (!Global.Initialized)
            return;

        Config.Set("OsmMinLevel", comboBoxMinLevel.SelectedIndex);
        Config.Set("OsmMaxImportLevel", comboBoxMaxImportLevel.SelectedIndex);
        Config.Set("OsmMaxLevel", comboBoxMaxLevel.SelectedIndex);

        Config.Set("OsmDpiAwareRendering", checkBoxDpiAwareRendering.Checked);

        int[] coverage = new int[] { 250, 500, 1000, 2000, 5000 };
        Config.Set("OsmCoverage", coverage[comboBoxCoverage.SelectedIndex]);
        Config.Set("ImportLayerOsm", checkBoxImportOsm.Checked);
        Config.Set("ImportLayerOTM", checkBoxImportOTM.Checked);
        Config.Set("ImportLayerOcm", checkBoxImportOcm.Checked);
        Config.Set("ImportLayerMQ", checkBoxImportMQ.Checked);
        Config.Set("ZoomCross", comboBoxZoomCross.Text);

        Config.Set("SpoilersDescriptionTags", txtSpoilerTags.Text);
        Config.Set("ImportLogImages", cbImportLogImages.Checked);
        Config.Set("ExportSpoilersRotate", chkSpoilerRotate.Checked);
        Config.Set("ExportSpoilersMaxPixels", Convert.ToInt32(numSpoilerMax.Value));
        Config.Set("ExportImagesMaxPixels", Convert.ToInt32(numImagesMax.Value));

        // Logs
        Config.Set("LogMaxMonthAge", (cbDeleteLogs.Checked) ? cbLogLife.SelectedIndex : 99999);
        Config.Set("LogMinCount", (cbDeleteLogs.Checked) ? cbMinLogs.SelectedIndex : 99999);

        Config.AcceptChanges();
    }

    #region Language

    private struct Lang
    {
      public Lang(string New_Name, string New_Path)
      {
        Name = New_Name;
        Path = New_Path;
      }

      public string Name;
      public string Path;

      public override string ToString()
      {
        return Name;
      }
    }

    private void Fill_comboBoxLanguage()
    {
      string[] AvailableList = Directory.GetFiles(Global.ExePath + "\\data\\lang", "*.lan");
      comboBoxLanguage.Items.Clear();
      int counter = 0;
      int SelIndex = 0;
      foreach (String tmpPath in AvailableList)
      {
        string tmpName = Global.Translations.getLangNameFromFile(tmpPath);
        comboBoxLanguage.Items.Add(new Lang(tmpName, tmpPath));
        if (Path.GetFileNameWithoutExtension(tmpPath) == Config.GetString("SelectedLanguage")) { SelIndex = counter; }
        counter++;
      }
      comboBoxLanguage.SelectedIndex = SelIndex;
    }

    private void comboBoxLanguage_SelectedIndexChanged(object sender, EventArgs e)
    {
      string selPath = ((Lang)((ComboBox)sender).SelectedItem).Path;
      if (Path.GetFileNameWithoutExtension(selPath) != Config.GetString("SelectedLanguage"))
      {
        Config.Set("SelectedLanguage", Path.GetFileNameWithoutExtension(selPath));
        Global.Translations.ReadTranslationsFile(selPath);
      }
    }

    #endregion

    private void comboBoxMinLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (comboBoxMinLevel.SelectedIndex > comboBoxMaxLevel.SelectedIndex)
            comboBoxMaxLevel.SelectedIndex = comboBoxMinLevel.SelectedIndex;

    }

    private void comboBoxMaxImportLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (comboBoxMaxImportLevel.SelectedIndex < comboBoxMinLevel.SelectedIndex)
            comboBoxMaxImportLevel.SelectedIndex = comboBoxMinLevel.SelectedIndex;

        if (comboBoxMaxImportLevel.SelectedIndex > comboBoxMaxLevel.SelectedIndex)
            comboBoxMaxImportLevel.SelectedIndex = comboBoxMaxLevel.SelectedIndex;
    }

    private void comboBoxMaxLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (comboBoxMaxLevel.SelectedIndex < comboBoxMinLevel.SelectedIndex)
            comboBoxMinLevel.SelectedIndex = comboBoxMaxLevel.SelectedIndex;
    }

    private void comboBoxCoverage_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    private void comboBoxZoomCross_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    private void cbDeleteLogs_CheckedChanged(object sender, EventArgs e)
    {
        cbLogLife_SelectedValueChanged(sender, e);

        cbLogLife.Enabled = cbDeleteLogs.Checked;
        cbMinLogs.Enabled = cbDeleteLogs.Checked;
    }

    private void cbLogLife_SelectedValueChanged(object sender, EventArgs e)
    {
        if (cbLogLife.SelectedIndex == 0 && cbMinLogs.SelectedIndex == 0 && cbDeleteLogs.Checked)
            if (MessageBox.Show(Global.Translations.Get("DelLogs"), Global.Translations.Get("PleaseConfirm"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                cbDeleteLogs.Checked = false;
    }

    private void button3_Click(object sender, EventArgs e)
    {
        String act = Api.GetAccessToken.Execute();
        if (act.Length > 0)
        {
            try
            {
                    Config.Set("accessTokenEnc", act);
                    Geocaching.Groundspeak.getInstance().SetGeocachingAPIClient(Config.GetAccessToken());
                    tbLogin.Text = Geocaching.Groundspeak.getInstance().UserInfo("username").Username;
            }
            catch ( Exception exc)
            {
                Global.AddLog("Query Username: " + exc.ToString());
            }
        }
    }

    private void cbLogLife_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    private void cbMinLogs_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
  }

}
