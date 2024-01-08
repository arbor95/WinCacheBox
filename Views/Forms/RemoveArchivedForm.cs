using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace WinCachebox.Views.Forms
{
    public partial class RemoveArchivedForm : Form
  {
    int total = 0;
    int progress = 0;

    public RemoveArchivedForm()
    {
      InitializeComponent();
      total = progressBar1.Maximum = Geocaching.Cache.Query.Count;

      new Thread(new ThreadStart(threadEntryProc)).Start();
    }

    delegate void reportProgressDelegate(int num);
    void reportProgress(int num)
    {
      if (InvokeRequired)
      {
        Invoke(new reportProgressDelegate(reportProgress), new object[] { num });
        return;
      }

      progressBar1.Value = num;
      labelProgress.Text = "(" + progress.ToString() + " / " + total.ToString() + ")";
    }

    delegate void finishedDelegate();

    void finished()
    {
      if (InvokeRequired)
      {
        Invoke(new finishedDelegate(finished));
        return;
      }

      labelProgress.Text = "Finished!";

      Geocaching.Cache.Query = new List<Geocaching.Cache>();

      Global.SelectedCache = null;

      this.Close();
    }

    void threadEntryProc()
    {
      int numRemovedFoundCaches = 0;

      for (int i = 0; i < Geocaching.Cache.Query.Count && !cancel; i++)
      {
        long cacheId = Geocaching.Cache.Query[i].Id;
        String gcCode = Geocaching.Cache.Query[i].GcCode;
        bool deleted = false;

        DbTransaction tx = Database.Data.StartTransaction();

        // Cache löschen
        CBCommand killCache = Database.Data.CreateCommand("delete from Caches where Id=@id");
        killCache.ParametersAdd("@id", DbType.Int64, Geocaching.Cache.Query[i].Id);

        // Logs löschen
        CBCommand killLogs = Database.Data.CreateCommand("delete from Logs where CacheId=@cacheid");
        killLogs.ParametersAdd("@cacheid", DbType.Int64, cacheId);

        CBCommand killWaypoints = Database.Data.CreateCommand("delete from Waypoint where CacheId=@cacheid");
        killWaypoints.ParametersAdd("@cacheid", DbType.Int64, cacheId);

        CBCommand killFieldNotes = null;
        if (Database.Data.serverType == Database.SqlServerType.SqlServerCE)
        {
            killFieldNotes = Database.Data.CreateCommand("delete from FieldNotes where CacheId=@cacheid");
            killFieldNotes.ParametersAdd("@cacheid", DbType.Int64, cacheId);
        } 

        try
        {
          killCache.ExecuteNonQuery();
          killLogs.ExecuteNonQuery();
          killWaypoints.ExecuteNonQuery();
          if (killFieldNotes != null)
            killFieldNotes.ExecuteNonQuery();
          tx.Commit();
          deleted = true;
        }
        catch
        {
          tx.Rollback();
        }
        finally
        {
          killCache.Dispose();
          killLogs.Dispose();
          killWaypoints.Dispose();
          if (killFieldNotes != null)
            killFieldNotes.Dispose();
        }

        if (deleted)
        {
          if (Geocaching.Cache.Query[i].Found)
            numRemovedFoundCaches++;

          // Bilder löschen
          String gc = gcCode;
          if (gc.Length >= 4)
          {
              gc = gcCode.Substring(0, 4);
          }
          if (Directory.Exists(Config.GetString("DescriptionImageFolder") + "\\" + gc))
          {
              String[] files = Directory.GetFiles(Config.GetString("DescriptionImageFolder") + "\\" + gcCode.Substring(0, 4), gcCode + "*");
              foreach (String file in files)
                  File.Delete(file);
          }

          if (Directory.Exists(Config.GetString("SpoilerFolder") + "\\" + gc))
          {
              // Spoiler Sync
              String[] files = Directory.GetFiles(Config.GetString("SpoilerFolder") + "\\" + gc, gcCode + "*");
              try
              {
                 foreach (String file in files)
                                  File.Delete(file);
              }
              catch { }
             
          }
        }
        reportProgress(++progress);
      }

      // Fundzahl anpassen
      Config.Set("FoundOffset", Config.GetInt("FoundOffset") + numRemovedFoundCaches);
      Config.AcceptChanges();

      finished();
    }

    bool cancel = false;

    private void button1_Click(object sender, EventArgs e)
    {
      cancel = true;
      button1.Enabled = false;
    }
  }
}
