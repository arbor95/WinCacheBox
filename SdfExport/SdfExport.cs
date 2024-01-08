using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

namespace WinCachebox.SdfExport
{
    public partial class SdfExport : Form, IFormProgressReport
    {
        Thread exportThread = null;
        private List<SdfExportSetting> list;
        public bool Cancel = false;
        public bool LowMemory = false;
        private DateTime dtInit;

        public SdfExport(List<SdfExportSetting> list)
        {
            this.list = list;
            InitializeComponent();
            bCancel.Text = Global.Translations.Get("cancel", "_Cancel");

            exportThread = new Thread(new ThreadStart(threadEntryPoint));
            exportThread.Start();
        }

        void threadEntryPoint()
        {
            try
            {
                dtInit = DateTime.Now;

                if (!Cancel)
                {
                    setTitle(Global.Translations.Get("export", "_Exporting SDF"));
                    int maxCount = 0;
                    foreach (SdfExportSetting setting in list)
                        maxCount += setting.cacheCountAproximate;

                    int startCount = 0;
                    int aktCount = 0;
                    int aktDatabaseCount = 0;
                    foreach (SdfExportSetting setting in list)
                    {
                        setting.LoadCaches();

                        int tmpCount = setting.Export(this, startCount, maxCount);
                        if (tmpCount > 0)
                        {
                            aktCount += tmpCount;
                            aktDatabaseCount++;
                        }
                        startCount += setting.Caches.Count;
                    }
                    MessageBox.Show(aktCount
                        + " " + Global.Translations.Get("Cache", "_Cache")
                        + " " + Global.Translations.Get("exported", "_exported")
                        + " " + Global.Translations.Get("for", "_for")
                        + " " + aktDatabaseCount
                        + " " + Global.Translations.Get("databases", "_Databases") + ".");
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
                    Invoke(new ExportFinished(Close));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bCancel.Enabled = false;
            Cancel = true;
        }

        delegate void ExportFinished();
        delegate void reportUncriticalErrorDelegate(string error);

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

        if (activity == Global.Translations.Get("export"))
        {
            TimeSpan tsTotal = (DateTime.Now - dtInit);
            double missingSecs = (double)total / (double)(processed) * (double)tsTotal.TotalSeconds - (double)tsTotal.TotalSeconds;
            TimeSpan missing = new TimeSpan(0, 0, (int)missingSecs);

            activity = Global.Translations.Get("export") + " " + (processed) + "/" + total + "\r\ntime elapsed: " + tsTotal.Hours.ToString("00") + ":" + tsTotal.Minutes.ToString("00") + ":" + tsTotal.Seconds.ToString("00") + "\r\ntime to finish: " + missing.Hours.ToString("00") + ":" + missing.Minutes.ToString("00") + ":" + missing.Seconds.ToString("00");
        }
        labelAction.Text = activity;
    }
        delegate void progressChangedTotalDelegate(string activity, int processed, int total);
        public void ProgressChangedTotal(string activity, int processed, int total)
        {
            if (InvokeRequired)
            {
                Invoke(new progressChangedTotalDelegate(ProgressChangedTotal), new object[] { activity, processed, total });
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


        bool IFormProgressReport.Cancel
        {
            get { return Cancel; }
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

        void showLowMemoryWarning()
        {
            MessageBox.Show("Device is running low on memory! This operation will now cancel!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
        }

        #endregion
    }
}
