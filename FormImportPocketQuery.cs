using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using WinCachebox.MailProcessor;
using WinCachebox.Geocaching;
using WinCachebox.Map;
using System.Data.Common;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using GeocachingAPI;
using GeocachingAPI.Models;

namespace WinCachebox
{
    public partial class FormImportPocketQuery : Form, IFormProgressReport
    {
        public bool LowMemory = false;

        public bool Cancel = false;

        // Instanzen der eingesetzten Importer
        DescriptionImageGrabber grabber = null;
        GpxImport importer = null;
        TileImport tileimport = null;
        //    CellIdImport cellIdImport = null;

        Thread importThread = null;

        /// <summary>
        /// true, falls die Verbindung ins Internet erfolgreich hergestellt wurde
        /// </summary>
        bool setup = false;
        bool internetRequired = true;

        public FormImportPocketQuery()
        {
            InitializeComponent();
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - Width / 2, Screen.PrimaryScreen.WorkingArea.Height / 2 - Height / 2);

            grabber = new DescriptionImageGrabber(this);
            importer = new GpxImport(this);
            tileimport = new TileImport(this);
            //      cellIdImport = new CellIdImport(this);

            importThread = new Thread(new ThreadStart(threadEntryPoint));
            importThread.Start();
        }

        delegate void ImportFinished();

        public static int GetNumberOfFoundsInDb()
        {
            CBCommand query = Database.Data.CreateCommand("select count(Found) from Caches where Found = @found");
            query.ParametersAdd("@found", DbType.Boolean, true);

            object ofounds = query.ExecuteScalar();
            int founds = Convert.ToInt32(ofounds);
            query.Dispose();
            return founds;
        }

        void showLowMemoryWarning()
        {
            MessageBox.Show("Device is running low on memory! This operation will now cancel!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
        }

        /// <summary>
        /// Hier wird der Datenimport abgewickelt
        /// </summary>
        void threadEntryPoint()
        {
            int numberDbFounds = GetNumberOfFoundsInDb();

            try
            {
                setup = false;

                internetRequired = Config.GetBool("CacheMapData") || Config.GetBool("CacheImageData") || Config.GetBool("ImportGpxFromMail") || Config.GetBool("ImportRatings") || Config.GetBool("ImportPQsFromGeocachingCom");

                if (internetRequired)
                {
                    setTitle("Checking for updates...");
                }

                // GPX-Dateien importieren
                if (Config.GetBool("ImportGpx"))
                {
                    if (Config.GetBool("ImportGpxFromMail") &&
                        Config.GetString("PopHost").Length > 0 &&
                        Config.GetStringEncrypted("PopLogin").Length > 0 &&
                        Config.GetStringEncrypted("PopPassword").Length > 0)
                    {
                        checkMails();
                    }

                    if (Config.GetBool("ImportPQsFromGeocachingCom") &&
                        Config.GetAccessToken().Length > 0)
                    {
                        downloadPQsFromGeocachingCom();
                    }

                    if (!Cancel && Directory.GetFiles(Config.GetString("PocketQueryFolder"), "*.zip").Length > 0)
                    {
                        setTitle("Unzipping...");
                        UnzipZippedGpx();
                    }

                    if (!Cancel)
                    {
                        String[] filesSorted = GetFilesToLoad();
                        DbTransaction transaction = Database.Data.StartTransaction();
                        importer.ImportGpx(filesSorted);
                        transaction.Commit();
                    }

                    // Alte Logs löschen. Macht nur Sinn, wenn auch welche dazu
                    // gekommen sind
                    if (!Cancel && Config.GetInt("LogMaxMonthAge") != 99999)
                    {
                        ProgressChanged("Deleting out-dated logs...", 0, 1);
                        Database.Data.DeleteOldLogs();
                        ProgressChanged("Deleting out-dated logs...", 1, 1);
                    }
                }

                if (!Cancel && Config.GetBool("ImportRatings"))
                {
                    sendVotes();
                    getRatings();
                }

                if (!Cancel && Config.GetBool("CacheImageData"))
                {
                    setTitle("Importing Images");
                    grabber.GrabImages();
                }

                if (!Cancel && Config.GetBool("CacheMapData"))
                {
                    setTitle("Importing Map");
                    tileimport.Import();
                }

            }
            catch (Exception exc)
      {
#if DEBUG
        Global.AddLog(exc.Message + " - " + exc.ToString());
#endif
        MessageBox.Show(exc.Message + "\r\n" + exc.ToString(), "Error while importing!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
      }
      finally
      {
        killEmptyDirectories(Config.GetString("PocketQueryFolder"));

        //int nowInDb = GetNumberOfFoundsInDb();
        //int dbOffset = numberDbFounds - nowInDb;
        //int newOffset = Config.GetInt("FoundOffset") + dbOffset;

        //try
        //{
        //  if (Config.GetBool("GCAutoSyncCachesFound") && (internetRequired) && groundspeak.Login(gcLogin, gcPass))
        //  {
        //    setTitle("Loading finds from gc.com");

        //    int groundSpeakRealValue = groundspeak.GetCachesFound();
        //    if (groundSpeakRealValue != -1)
        //    {
        //      newOffset = groundSpeakRealValue - nowInDb;
        //    }
        //  }
        //}
        //catch (TimeoutException)
        //{
        //  ReportUncriticalError("Server Timeout...");
        //}
        //catch (System.Net.Sockets.SocketException)
        //{
        //  ReportUncriticalError("Cannot resolve Host!");
        //}
        //catch (System.Net.WebException)
        //{
        //  ReportUncriticalError("Cannot resolve Host!");
        //}

        //Config.Set("FoundOffset", (newOffset > 0) ? newOffset : 0);
        //Config.AcceptChanges();

        setup = false;

        if (InvokeRequired)
          Invoke(new ImportFinished(Close));

      }
        }


        private void sendVotes()
        {
            CBCommand query = Database.Data.CreateCommand("select Id, GcCode, Url, Vote from Caches where VotePending=@votepending");
            query.ParametersAdd("@votepending", DbType.Boolean, true);
            DbDataReader reader = query.ExecuteReader();

            int cnt = 1;

            while (reader.Read())
            {
                setTitle("Submitting Votes...");
                ProgressChanged("Sending Vote #" + cnt.ToString(), 0, 1);

                if (!Geocaching.GcVote.SendVotes(reader.GetInt64(0), reader.GetString(2), reader.GetString(1), reader.GetInt16(3)))
                {
                    ReportUncriticalError("Cannot send Vote #" + cnt.ToString());
                }
                cnt++;
            }

            reader.Dispose();
            query.Dispose();
        }

        void getRatings()
        {
            setTitle("Querying Cache Ratings...");

            string where = Global.LastFilter.SqlWhere;

            CBCommand query = Database.Data.CreateCommand("select count(GcCode) from Caches " + ((where.Length > 0) ? "where " + where : where));
            int count = int.Parse(query.ExecuteScalar().ToString());
            query.Dispose();

            query = Database.Data.CreateCommand("select Id, GcCode, VotePending, Url from Caches " + ((where.Length > 0) ? "where " + where : where));
            DbDataReader reader = query.ExecuteReader();

            int packetSize = 100;
            int progress = 0;
            int failCount = 0;

            for (int i = 0; i < count && !Cancel; i++)
            {
                List<String> requests = new List<String>();
                Dictionary<String, bool> resetVote = new Dictionary<string, bool>();
                Dictionary<String, long> idLookup = new Dictionary<string, long>();
                for (int j = i; j < count && j < (i + packetSize); j++)
                {
                    if (!reader.Read())
                    {
                        i = count;
                        continue;
                    }

                    String waypointCode = reader.GetString(1);
                    if (!waypointCode.StartsWith("GC"))
                        continue;

                    //String Url = reader.GetString(3);
                    //String guid = Url.Substring(Url.IndexOf("guid=") + 5);
                    requests.Add(waypointCode);
                    idLookup.Add(waypointCode, reader.GetInt64(0));
                    resetVote.Add(waypointCode, reader.IsDBNull(2) || !reader.GetBoolean(2));
                }

                ProgressChanged("Querying GcVote...", progress, count);
                try
                {
                    List<GcVote.RatingData> ratingData = GcVote.GetRating(Config.GetString("GcLogin"), Config.GetStringEncrypted("GcVotePassword"), requests);

                    if (ratingData == null)
                        ReportUncriticalError("Query " + (i + 1).ToString() + " failed...");
                    else
                    {
                        foreach (GcVote.RatingData data in ratingData)
                        {
                            if (Cancel)
                                break;

                            if (idLookup.ContainsKey(data.Waypoint))
                            {
                                CBCommand update;

                                if (resetVote.ContainsKey(data.Waypoint))
                                {
                                    update = Database.Data.CreateCommand("update Caches set Rating=@rating, Vote=@vote, VotePending=@votepending where Id=@id");
                                    update.ParametersAdd("@vote", DbType.Int16, (short)data.Vote);
                                    update.ParametersAdd("@votepending", DbType.Boolean, false);
                                }
                                else
                                    update = Database.Data.CreateCommand("update Caches set Rating=@rating where Id=@id");

                                update.ParametersAdd("@rating", DbType.Single, (short)(data.Rating * 100));
                                update.ParametersAdd("@id", DbType.String, idLookup[data.Waypoint]);

                                update.ExecuteNonQuery();
                                update.Dispose();
                            }

                            progress++;
                            ProgressChanged("Writing Ratings (" + progress.ToString() + " / " + count.ToString() + ")", progress, count);
                        }
                    }
                    progress += packetSize - ratingData.Count;
                }
                catch (Exception exc)
                {
#if DEBUG
                    String requestString = "";
                    foreach (String req in requests)
                        requestString += req + ",";

                    Global.AddLog("GcVote.GetRating(" + Config.GetString("GcLogin") + "," + requestString.Substring(0, requestString.Length - 1) + "): " + exc.ToString());
#endif
                    failCount++;
                    ReportUncriticalError(failCount.ToString() + ((failCount == 1) ? " request failed!" : " requests failed!"));
                    progress += packetSize;
                }
            }
            reader.Dispose();
            query.Dispose();
        }

        void downloadPQsFromGeocachingCom()
        {
            try
            {
                setTitle("Pocket Queries @ Geocaching.com");
                ProgressChanged("Connecting...", 0, 1);
                IUsers u = new Users(Groundspeak.getInstance().GetGeocachingAPIClient());
                ProgressChanged("List available Pocket Queries...", 0, 1);
                IList<GeocacheList> availablePocketQueries = UsersExtensions.GetLists(u, "me", Groundspeak.getInstance().apiVersion, "pq", 0, 50, "referenceCode,name,lastUpdatedDateUtc,count");
                if (availablePocketQueries.Count > 0)
                {
                    FormDownloadPocketQuery formDownloadPocketQuery = new FormDownloadPocketQuery();

                    formDownloadPocketQuery.InitListOfAvailablePQs(availablePocketQueries);

                    if (DialogResult.OK == formDownloadPocketQuery.ShowDialog())
                    {
                        int i = 0;
                        List<GeocacheList> selectedPocketQueries = formDownloadPocketQuery.GetListOfSelectedPQs();

                        foreach (GeocacheList pocketQuery in selectedPocketQueries)
                        {
                            ProgressChanged("Download: " + pocketQuery.Name + " (" + (i + 1).ToString() + " / " + selectedPocketQueries.Count + ")", i, selectedPocketQueries.Count);
                            i++;

                            Groundspeak.getInstance().DownloadSinglePocketQuery(pocketQuery);

                            CBCommand PQcommand;
                            PQcommand = Database.Data.CreateCommand("insert into PocketQueries(PQName,CreationTimeOfPQ) values (@PQName,@CreationTimeOfPQ)");
                            PQcommand.ParametersAdd("@PQName", DbType.String, pocketQuery.Name);

                            DateTime creationDateTime;
                            creationDateTime = pocketQuery.LastUpdatedDateUtc.Value;

                            PQcommand.ParametersAdd("@CreationTimeOfPQ", DbType.Date, creationDateTime);
                            PQcommand.ExecuteNonQuery();
                            PQcommand.Dispose();

                            PerformMemoryTest(Config.GetString("PocketQueryFolder"), 5000);

                            if (Cancel)
                            {
                                break;
                            }
                        }
                    }
                }
                else
                {
                    ReportUncriticalError("No pq available");
                }
            }
            catch (TimeoutException)
            {
                ReportUncriticalError("Server Timeout? ...");
            }
            catch (System.Net.Sockets.SocketException)
            {
                ReportUncriticalError("Cannot resolve Host!");
            }
            catch (System.Net.WebException)
            {
                ReportUncriticalError("Cannot resolve Host!");
            }
            catch (Exception exc)
            {
                ReportUncriticalError("Wrong GC username or password?...");
#if DEBUG
                ReportUncriticalError(exc.ToString());
                Global.AddLog("FormImportPocketQuery.downloadPQsFromGeocachingCom: " + exc.ToString());
#endif
            }
        }

        void checkMails()
        {
            try
            {
                setTitle("Checking Mails...");

                ProgressChanged("Connecting...", 0, 1);
                Client client = new Client(Config.GetString("PopHost"), Config.GetStringEncrypted("PopLogin"), Config.GetStringEncrypted("PopPassword"));

                if (!client.Connected)
                    ReportUncriticalError("Cannot log in...");
                else
                {
                    ProgressChanged("Getting list...", 0, 1);
                    List<Mail> rawList = null;
                    List<Mail> mails = null;
                    try
                    {
                        mails = client.GetHeaders(Config.GetBool("PopSkipOutdatedGpx"), out rawList);
                    }
                    catch (Exception)
                    {
                        ReportUncriticalError("Cannot fetch headers from server...");
                    }

                    if (mails != null)
                    {

                        mails = filterMails(mails);
                        rawList = filterMails(rawList);

                        for (int i = 0; i < mails.Count && !Cancel; i++)
                        {
                            ProgressChanged("Loading attachments " + (i + 1).ToString() + " / " + mails.Count.ToString(), i, mails.Count);

                            if (!PerformMemoryTest(Config.GetString("PocketQueryFolder"), 2048 + 1024))
                            {
                                try
                                {
                                    client.FetchAttachments(mails[i], Config.GetString("PocketQueryFolder"), "application/", false);

                                    // Bei Bedarf Mail löschen
                                    if (Config.GetBool("PopDeleteProcessedQueries"))
                                    {
                                        client.DeleteMessage(mails[i].Index);
                                        rawList.Remove(mails[i]);
                                    }

                                    PerformMemoryTest(Config.GetString("PocketQueryFolder"), 2048);
                                }
                                catch (Exception)
                                {
                                    ReportUncriticalError("Cannot fetch attachments from mail " + (i + 1).ToString());
                                }
                            }
                            Application.DoEvents();
                        }
                    }

                    // Restliche Queries löschen
                    if (!Cancel && Config.GetBool("PopDeleteProcessedQueries"))
                        foreach (Mail mail in rawList)
                            client.DeleteMessage(mail.Index);

                    client.Dispose();

                }
            }
            catch (TimeoutException)
            {
                ReportUncriticalError("Server Timeout...");
            }
            catch (System.Net.Sockets.SocketException)
            {
                ReportUncriticalError("Cannot resolve Host!");
            }
            catch (Exception exc)
            {
#if DEBUG
                ReportUncriticalError(exc.ToString());
                Global.AddLog("FormImportPocketQuery.checkMails: " + exc.ToString());
#endif
            }
        }

        private List<Mail> filterMails(List<Mail> mails)
        {
            List<Mail> result = new List<Mail>();

            if (mails == null)
                return result;

            for (int i = 0; i < mails.Count; i++)
            {
                Mail mail = mails[i];
                if (mail.Subject.IndexOf("[GEO] Pocket Query:") != -1)
                    result.Add(mail);
            }

            return result;
        }

        private void UnzipZippedGpx()
        {
            String directoryPath = Config.GetString("PocketQueryFolder");

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            String[] files = Directory.GetFiles(directoryPath, "*.zip");

            FastZip fastZip = new FastZip();

            ProgressChanged("Processing file 1 / " + files.Length.ToString(), 0, files.Length);
            int cnt = 0;
            foreach (String file in files)
            {
                if (Cancel)
                    break;

                // Zielverzeichnis ermitteln und erzeugen
                int endIdx = file.LastIndexOf('.');
                int startIdx = file.LastIndexOf('\\');
                String targetDirectory = Config.GetString("PocketQueryFolder") + file.Substring(startIdx, endIdx - startIdx);
                if (!Directory.Exists(targetDirectory))
                    Directory.CreateDirectory(targetDirectory);

                if (!PerformMemoryTest(Config.GetString("PocketQueryFolder"), 2048 + 1024 + 512))
                {
                    // Zip dorthin entpacken!
                    try
                    {
                        fastZip.ExtractZip(file, targetDirectory, ".+gpx");
                    }
                    catch (Exception)
                    {
                        ReportUncriticalError("Cannot unzip " + file);
                    }
                    File.Delete(file);

                    cnt++;
                    ProgressChanged("Processing file " + (Math.Min(files.Length, cnt + 1)).ToString() + " / " + (files.Length).ToString(), cnt, files.Length);
                }

                if (Cancel)
                    break;
            }
        }

        void killEmptyDirectories(String directory)
        {
            killEmptyDirectories(directory, 0);
        }

        void killEmptyDirectories(String directory, int depth)
        {
            String[] directories = Directory.GetDirectories(directory);
            foreach (String dir in directories)
                killEmptyDirectories(dir, depth + 1);

            if (Directory.GetDirectories(directory).Length == 0 &&
                Directory.GetFiles(directory).Length == 0 &&
                depth > 0)
                Directory.Delete(directory);
        }

        void recursiveDirectoryReader(String directory, ref List<String> files)
        {
            String[] localFiles = Directory.GetFiles(directory, "*.gpx");
            foreach (String localFile in localFiles)
                files.Add(localFile);

            String[] directories = Directory.GetDirectories(directory);
            foreach (String dir in directories)
                recursiveDirectoryReader(dir, ref files);
        }

        private String[] GetFilesToLoad()
        {
            // GPX sortieren
            String directoryPath = Config.GetString("PocketQueryFolder");

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            List<String> files = new List<string>();
            recursiveDirectoryReader(directoryPath, ref files);

            String[] filesSorted = new String[files.Count];

            int idx = 0;
            for (int wptsWanted = 0; wptsWanted < 2; wptsWanted++)
                foreach (String file in files)
                {
                    bool isWaypointFile = file.ToLower().EndsWith("-wpts.gpx");
                    if (isWaypointFile == (wptsWanted == 1))
                        filesSorted[idx++] = file;
                }

            return filesSorted;
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
        delegate void progressChangedTotalDelegate(string activity, int processed, int total);
        public void ProgressChangedTotal(string activity, int processed, int total)
        {
            if (InvokeRequired)
            {
                Invoke(new progressChangedTotalDelegate(ProgressChangedTotal), new object[] { activity, processed, total });
                return;
            }

            if (total == -1)
                progressBar2.Value = 0;
            else
            {
                if (total < progressBar2.Value)
                    progressBar2.Value = 0;

                progressBar2.Maximum = total;
                progressBar2.Value = Math.Min(total, processed);
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
                LowMemory = Global.GetAvailableDiscSpace(Path.GetDirectoryName(path)) < ((ulong)(1024 * neededKb));

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
