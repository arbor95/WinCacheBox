using System;
using System.Windows.Forms;

namespace WinCachebox
{
    public partial class ImportSettingsForm : Form
  {
    public ImportSettingsForm()
    {
      InitializeComponent();
      this.Text = Global.Translations.Get("import");
      this.button1.Text = Global.Translations.Get("ok");
      this.button2.Text = Global.Translations.Get("cancel");
      this.checkBoxGcVote.Text = Global.Translations.Get("gcvote","GcVote Cache Ratings (Filter Selection)");
      this.checkBoxImportCellIds.Text = Global.Translations.Get("cellids", "Cell Ids");
      this.checkBoxImportGPX.Text = Global.Translations.Get("GPX");
      this.checkBoxImportGpxFromMail.Text = Global.Translations.Get("gpxfrommail", "check Mails");
      this.checkBoxImportMaps.Text = Global.Translations.Get("maps");
      this.checkBoxPreloadImages.Text = Global.Translations.Get("preloadimages", "Description Images (Filter Selection)");
      this.checkImportPQfromGC.Text = Global.Translations.Get("pqfromgc", "Pocket Queries (gc.com)");
      this.checkDeleteGPX.Text = Global.Translations.Get("delete") + " " + Global.Translations.Get("after") + " " + Global.Translations.Get("import");
    }

    private void ImportSettingsForm_Load(object sender, EventArgs e)
    {
      checkBoxImportMaps.Checked = Config.GetBool("CacheMapData");
      checkBoxPreloadImages.Checked = Config.GetBool("CacheImageData");
      checkBoxImportGPX.Checked = Config.GetBool("ImportGpx");
      checkDeleteGPX.Checked = !Config.GetBool("DontDeleteGpx");
      checkBoxGcVote.Checked = Config.GetBool("ImportRatings");
      checkBoxImportCellIds.Checked = Config.GetBool("ImportCellIds");

      if (Config.GetString("PopHost").Length > 0 && Config.GetStringEncrypted("PopLogin").Length > 0 && Config.GetStringEncrypted("PopPassword").Length > 0)
      {
        checkBoxImportGpxFromMail.Checked = Config.GetBool("ImportGpxFromMail");
        checkBoxImportGpxFromMail.Enabled = true;
      }
      else
      {
        checkBoxImportGpxFromMail.Enabled = false;
        checkBoxImportGpxFromMail.Checked = false;
      }

      if (Config.GetAccessToken().Length > 0)
      {
        checkImportPQfromGC.Checked = Config.GetBool("ImportPQsFromGeocachingCom");
        checkImportPQfromGC.Enabled = true;

        checkBoxPreloadImages.Checked = Config.GetBool("CacheImageData");
        checkBoxPreloadImages.Enabled = true;
      }
      else
      {
        checkImportPQfromGC.Checked = false;
        checkImportPQfromGC.Enabled = false;

        checkBoxPreloadImages.Checked = false;
        checkBoxPreloadImages.Enabled = false;
      }

      if (checkImportPQfromGC.Checked == true)
      {
        checkBoxImportGPX.Checked = true;
        checkBoxImportGPX.Enabled = false;
      }
    }

    private void button2_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      button1.Enabled = false;
//      FormMain.mapview.ClearCachedTiles();

      if (!Config.GetBool("CacheImageData") && checkBoxPreloadImages.Checked)
      {
        // only show warn message, if the user changed the state from disable to enable.
/*        if (MessageBox.Show("Download of additional/spoiler images is done on personal responsibility. Read the GEOCACHING.COM SITE TERMS OF USE AGREEMENT (5). Really download?", "Import additional images", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
        {
          Config.Set("GCAdditionalImageDownload", true);
        }
        else
        {
          Config.Set("GCAdditionalImageDownload", false);
        }
*/
          Config.Set("GCAdditionalImageDownload", true);
      }
      Config.Set("CacheMapData", checkBoxImportMaps.Checked);
      Config.Set("CacheImageData", checkBoxPreloadImages.Checked);
      Config.Set("ImportGpx", checkBoxImportGPX.Checked);
      Config.Set("ImportGpxFromMail", checkBoxImportGpxFromMail.Checked);
      Config.Set("ImportPQsFromGeocachingCom", checkImportPQfromGC.Checked);
      Config.Set("ImportRatings", checkBoxGcVote.Checked);
      Config.Set("ImportCellIds", checkBoxImportCellIds.Checked);
      Config.Set("DontDeleteGpx", !checkDeleteGPX.Checked);


      Config.AcceptChanges();

      bool internetRequired = checkBoxImportMaps.Checked || checkBoxPreloadImages.Checked || checkBoxImportGpxFromMail.Checked || checkBoxGcVote.Checked || checkBoxImportCellIds.Checked;

      if (!Config.GetBool("AllowInternetAccess") && internetRequired)
      {
        if (MessageBox.Show("Cachebox needs internet access in order to perform the selected operations! If you do not have a data plan be sure the device is connected to the internet via Activesync, Windows Mobile Device Center or Wifi! Do you want to proceed?", "Internet connection required!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
        {
          this.DialogResult = DialogResult.Cancel;
          this.Close();
          return;
        }
      }

      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void checkBoxImportGPX_CheckStateChanged(object sender, EventArgs e)
    {
        // is it really possible gpx from mail
        if (Config.GetString("PopHost").Length > 0 && Config.GetStringEncrypted("PopLogin").Length > 0 && Config.GetStringEncrypted("PopPassword").Length > 0)
        {
            checkBoxImportGpxFromMail.Checked = Config.GetBool("ImportGpxFromMail");
            checkBoxImportGpxFromMail.Enabled = true;
        }
        else
        {
            checkBoxImportGpxFromMail.Enabled = false;
            checkBoxImportGpxFromMail.Checked = false;
        }
    }

    private void checkImportPQfromGC_CheckStateChanged(object sender, EventArgs e)
    {
      if (checkImportPQfromGC.Checked)
      {
        checkBoxImportGPX.Checked = true;
        checkBoxImportGPX.Enabled = false;
      }
      else
      {
        checkBoxImportGPX.Enabled = true;
      }

    }

    private void checkBoxImportCellIds_CheckedChanged(object sender, EventArgs e)
    {

    }



  }
}
