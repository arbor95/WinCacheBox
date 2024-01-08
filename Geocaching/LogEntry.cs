using System;
using System.Data.Common;

namespace WinCachebox.Geocaching
{
    public class LogEntry
  {
    /// <summary>
    /// Benutzername des Loggers
    /// </summary>
    public String Finder = String.Empty;

    /// <summary>
    /// Logtyp, z.B. "Found it!"
    /// </summary>
    public short Type = -1;

    /// <summary>
    /// Index des zu verwendenden Bildchens
    /// </summary>
    public int TypeIcon = -1;

    /// <summary>
    /// Geschriebener Text
    /// </summary>
    public String Comment = String.Empty;

    /// <summary>
    /// Zeitpunkt
    /// </summary>
    public DateTime Timestamp = DateTime.MinValue;

    /// <summary>
    /// Id des Caches
    /// </summary>
    public long CacheId = -1;

    /// <summary>
    /// Id des Logs
    /// </summary>
    public long Id = -1;

    public LogEntry(DbDataReader reader, bool filterBbCode)
    {
      CacheId = reader.GetInt64(0);
      Timestamp = reader.GetDateTime(1);
      Finder = reader.GetString(2);
      TypeIcon = reader.GetInt16(3);
      Comment = reader.GetString(4);
      Id = reader.GetInt64(5);

      if (filterBbCode)
      {
        int lIndex;

        while ((lIndex = Comment.IndexOf('[')) >= 0)
        {
          int rIndex = Comment.IndexOf(']', lIndex);

          if (rIndex == -1)
            break;

          Comment = Comment.Substring(0, lIndex) + Comment.Substring(rIndex + 1);
        }
      }
    }
  }
}
