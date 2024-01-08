using System;
using System.Windows.Forms;
using System.Threading;
using WinCachebox.Geocaching;

namespace WinCachebox.Views.Forms
{
    public partial class FormDownloadCacheImages : Form, IFormProgressReport
  {
    public bool LowMemory = false;

    public bool Cancel = false;

    long CacheId = 0;

    // Instanzen der eingesetzten Importer
    DescriptionImageGrabber grabber = null;
    Thread importThread = null;

    public FormDownloadCacheImages(long cacheId)
    {
      InitializeComponent();

      CacheId = cacheId;

      String accessToken = Config.GetStringEncrypted("accessToken");

      if (accessToken.Equals("") && false)
      {
        MessageBox.Show("You didn't specify your GC Access Token!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
        Cancel = true;
      }

      grabber = new DescriptionImageGrabber(this);

      importThread = new Thread(new ThreadStart(threadEntryPoint));
      importThread.Start();
    }

    delegate void ImportFinished();

    void showLowMemoryWarning()
    {
      MessageBox.Show("Device is running low on memory! This operation will now cancel!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
    }

    /// <summary>
    /// Hier wird der Datenimport abgewickelt
    /// </summary>
    void threadEntryPoint()
    {
      try
      {
        if (!Cancel)
        {
          setTitle("Importing Images");
          grabber.GrabImagesOfOneCache(CacheId);
        }
      }
      catch (Exception exc)
      {
#if DEBUG
        Global.AddLog(exc.ToString());
        MessageBox.Show(exc.ToString());
#endif
      }
      finally
      {
        if (InvokeRequired)
          Invoke(new ImportFinished(Close));
      }
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      buttonCancel.Enabled = false;
      Cancel = true;
    }




    #region IFormProgressReport Members

    delegate void progressChangedDelegate(string activity, int processed, int total);
    public void ProgressChanged(string activity, int processed, int total)
    {
        if (InvokeRequired)
        {
            Invoke(new progressChangedDelegate(ProgressChanged), new object[] { activity, processed, total });
            return;
        }

        if (total == -1)
            progressBar1.Value = 0;
        else
        {
            if (total < progressBar1.Value)
                progressBar1.Value = 0;

            progressBar1.Maximum = total;
            progressBar1.Value = Math.Min(total, processed);
        }

        labelAction.Text = activity;
    }

    delegate void progressChangeTotaldDelegate(string activity, int processed, int total);
    public void ProgressChangedTotal(string activity, int processed, int total)
    {
        if (InvokeRequired)
        {
            Invoke(new progressChangeTotaldDelegate(ProgressChangedTotal), new object[] { activity, processed, total });
            return;
        }

        if (total == -1)
            progressBar1.Value = 0;
        else
        {
            if (total < progressBar1.Value)
                progressBar1.Value = 0;

            progressBar1.Maximum = total;
            progressBar1.Value = Math.Min(total, processed);
        }

        labelAction.Text = activity;
    }

    delegate void reportUncriticalErrorDelegate(string error);
    public void ReportUncriticalError(string error)
    {
      if (InvokeRequired)
      {
        Invoke(new reportUncriticalErrorDelegate(ReportUncriticalError), new object[] { error });
        return;
      }
      labelError.Text = error;
      labelError.Invalidate();
    }

    public void setTitle(String text)
    {
      if (InvokeRequired)
      {
        Invoke(new reportUncriticalErrorDelegate(setTitle), new object[] { text });
        return;
      }

      this.Text = text;
    }

    /// <summary>
    /// Diese Funktion soll von Importern nach dem Speichern großer Dateien
    /// aufgerufen werden
    /// </summary>
    /// <returns>true, falls ein Speicherproblem vorliegt</returns>
    public bool PerformMemoryTest(String path, int neededKb)
    {
      bool oldMemoryState;

      lock (this)
      {
        oldMemoryState = LowMemory;
        LowMemory = Global.GetAvailableDiscSpace(path) < (ulong)((long)1024 * neededKb);

        if (!oldMemoryState && LowMemory)
          Cancel = true;
      }

      if (!oldMemoryState && LowMemory)
      {
        if (InvokeRequired)
          Invoke(new Global.emptyDelegate(showLowMemoryWarning));
        else
          showLowMemoryWarning();
      }

      return LowMemory;
    }

    bool IFormProgressReport.Cancel
    {
      get { return Cancel; }
    }

    #endregion
  }
}
