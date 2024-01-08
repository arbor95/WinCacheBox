using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data.Common;
using WinCachebox.Geocaching;
using System.Threading;
using GeocachingAPI;
using GeocachingAPI.Models;

namespace WinCachebox.Api
{
    public partial class GetCacheStatus : Form, IFormProgressReport
    {
        public static int UpdateCacheStatus()
        {
            GetCacheStatus gcs = new GetCacheStatus();
            gcs.bCancel.Text = "&" + Global.Translations.Get("cancel", "Cancel");
            gcs.ShowDialog();
            return 0;
        }

        Thread exportThread = null;
        public bool Cancel = false;
        public bool LowMemory = false;

        public GetCacheStatus()
        {
            InitializeComponent();
            exportThread = new Thread(new ThreadStart(threadEntryPoint));
            exportThread.Start();
        }

        void threadEntryPoint()
        {
            try
            {
                setTitle(Global.Translations.Get("f18", "Get Cache Status"));
                if (!Cancel)
                {
                    Geocaches g = new Geocaches(Groundspeak.getInstance().GetGeocachingAPIClient());
                    SortedList<String, Cache> caches = new SortedList<String, Cache>();

                    DbTransaction transaction = Database.Data.StartTransaction();
                    int start = Environment.TickCount;
                    try
                    {
                        String cacheCodes = "";
                        int count = 0;
                        for (int i = 0; i < Cache.Query.Count; i++)
                        {
                            if (Cancel)
                                return;
                            ProgressChanged("Get Cache Status", i, Cache.Query.Count);
                            Cache cache = Cache.Query[i];
                            if (!cache.GcCode.StartsWith("GC"))
                            {
                                // only Caches from Geocaching.com
                                continue;
                            }
                            cacheCodes = cacheCodes + "," + cache.GcCode;
                            caches.Add(cache.GcCode, cache);
                            count++;
                            if ((count >= 50) || (i == Cache.Query.Count - 1))
                            {
                                IList<Geocache> response;
                                do
                                {
                                    // only 50 caches in one query
                                    response = GeocachesExtensions.GetGeocaches(g, cacheCodes.Substring(1), Groundspeak.getInstance().apiVersion, true, "", "referenceCode,status,userData.foundDate");
                                    // todo perhaps handle favoritePoints, trackableCount
                                    if (response != null)
                                    {
                                        foreach (Geocache gc in response)
                                        {
                                            if (caches.ContainsKey(gc.ReferenceCode))
                                            {
                                                Cache rCache = caches[gc.ReferenceCode];
                                                if (gc.Status.Equals("Archived"))
                                                {
                                                    rCache.UpdateCacheStatus(false, true);
                                                }
                                                else if (gc.Status.Equals("Active"))
                                                {
                                                    rCache.UpdateCacheStatus(true, false);
                                                }
                                                else
                                                {
                                                    rCache.UpdateCacheStatus(false, false);
                                                }
                                                if (gc.UserData.FoundDate != null)
                                                {
                                                    rCache.Found = true;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        // GC only allows x queries every Minute. 
                                        int end = Environment.TickCount;
                                        if (end < start)
                                            end = start;
                                        int delay = 45000 - (end - start);
                                        if (delay > 0)
                                        {
                                            Thread.Sleep(delay);
                                        }
                                        start = Environment.TickCount;
                                    }
                                }
                                while (response == null); // endless
                                cacheCodes = "";
                                count = 0;
                                caches.Clear();
                            }
                        }
                        transaction.Commit();
                    }
                    catch (Exception exc)
                    {
                        exc.GetType(); //Warning vermeiden _ avoid warning
                        // transaction.Rollback();
                        transaction.Commit();
                        MessageBox.Show(exc.Message);
                        // return response.Status.StatusCode;
                    }
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

        delegate void ExportFinished();
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

        public void setTitle(string text)
        {
            if (InvokeRequired)
            {
                Invoke(new reportUncriticalErrorDelegate(setTitle), new object[] { text });
                return;
            }

            this.Text = text;
        }

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

        public bool PerformMemoryTest(string path, int neededKb)
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

        private void bCancel_Click(object sender, EventArgs e)
        {
            bCancel.Enabled = false;
            Cancel = true;
        }

        void showLowMemoryWarning()
        {
            MessageBox.Show("Device is running low on memory! This operation will now cancel!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
        }

        bool IFormProgressReport.Cancel
        {
            get { return Cancel; }
        }

    }
}
