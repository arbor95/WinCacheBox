using System;
using System.Data;
using System.Data.Common;

namespace WinCachebox.Geocaching
{
    public class Waypoint
  {
    /// <summary>
    /// Id des dazugehörigen Caches in der Datenbank von geocaching.com
    /// </summary>
    public long CacheId;

    /// <summary>
    /// Waypoint Code
    /// </summary>
    public String GcCode;

    public Coordinate Coordinate = new Coordinate();
    /// <summary>
    /// Breitengrad
    /// </summary>
    public double Latitude { get { return Coordinate.Latitude; } set { Coordinate.Latitude = value; } }

    /// <summary>
    /// Längengrad
    /// </summary>
    public double Longitude { get { return Coordinate.Longitude; } set { Coordinate.Longitude = value; } }

    /// <summary>
    /// Titel des Wegpunktes
    /// </summary>
    public String Title;

    /// <summary>
    /// Kommentartext
    /// </summary>
    public String Description;

    /// <summary>
    /// Art des Wegpunkts
    /// </summary>
    public CacheTypes Type;

    /// <summary>
    /// true, falls der Wegpunkt vom Benutzer erstellt wurde
    /// </summary>
    public bool IsUserWaypoint;

    /// <summary>
    /// true, falls der Wegpunkt von der Synchronisation ausgeschlossen wird
    /// </summary>
    public bool IsSyncExcluded;

    /// <summary>
    /// Lösung einer QTA
    /// </summary>
    public String Clue;

    public Waypoint()
    {
      CacheId = -1;
      GcCode = "";
      Latitude = Longitude = 0;
      Description = "";
    }

    public Waypoint(DbDataReader reader)
    {
      GcCode = reader[0].ToString();
      CacheId = long.Parse(reader[1].ToString());
      Latitude = double.Parse(reader[2].ToString());
      Longitude = double.Parse(reader[3].ToString());
      Description = reader[4].ToString();
      Type = (CacheTypes)int.Parse(reader[5].ToString());
      IsSyncExcluded = bool.Parse(reader[6].ToString());
      IsUserWaypoint = bool.Parse(reader[7].ToString());
      Clue = reader[8].ToString().Trim();
      Title = reader[9].ToString().Trim();
    }

    public int CreateCheckSum()
    {
        // for Replication
        string sCheckSum = GcCode;
        sCheckSum += Global.FormatLatitudeDM(Latitude);
        sCheckSum += Global.FormatLongitudeDM(Longitude);
        sCheckSum += Description;
        sCheckSum += Type.ToString();
        sCheckSum += Clue;
        sCheckSum += Title;
        return (int)Global.sdbm(sCheckSum);
    }

    public string CreateConflictString()
    {
        string sCheckSum = GcCode + Environment.NewLine;
        sCheckSum += Global.FormatLatitudeDM(Latitude) + " / ";
        sCheckSum += Global.FormatLongitudeDM(Longitude) + Environment.NewLine;
        sCheckSum += Description + Environment.NewLine;
        sCheckSum += Type.ToString() + Environment.NewLine;
        sCheckSum += Clue + Environment.NewLine;
        sCheckSum += Title;
        return sCheckSum;
    }
    public Waypoint(String gcCode, CacheTypes type, String description, double latitude, double longitude, long cacheId, String clue, String title)
    {
      GcCode = gcCode;
      CacheId = cacheId;
      Latitude = latitude;
      Longitude = longitude;
      Description = description;
      Type = type;
      IsSyncExcluded = true;
      IsUserWaypoint = true;
      Clue = clue;
      Title = title;
    }

    public static bool GcCodeExists(String gcCode)
    {
        CBCommand command = Database.Data.CreateCommand("select count(GcCode) from Waypoint where GcCode=@gccode");
        command.ParametersAdd("@gccode", DbType.String, gcCode);
        int count = int.Parse(command.ExecuteScalar().ToString());
        command.Dispose();

        return count > 0;
    }

    public static String CreateFreeGcCode(String cacheGcCode)
    {
      String suffix = cacheGcCode.Substring(2);
      String firstCharCandidates = "CBXADEFGHIJKLMNOPQRSTUVWYZ0123456789";
      String secondCharCandidates = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

      for (int i = 0; i < firstCharCandidates.Length; i++)
        for (int j = 0; j < secondCharCandidates.Length; j++)
        {
          String gcCode = firstCharCandidates.Substring(i, 1) + secondCharCandidates.Substring(j, 1) + suffix;
          if (!GcCodeExists(gcCode))
            return gcCode;
        }
      throw new Exception("Alle GcCodes sind bereits vergeben! Dies sollte eigentlich nie vorkommen!");
    }

    public void WriteToDatabase()
    {
        CBCommand command = Database.Data.CreateCommand("insert into Waypoint(GcCode, CacheId, Latitude, Longitude, Description, Type, SyncExclude, UserWaypoint, Clue, Title) values (@gccode, @cacheid, @latitude, @longitude, @description, @type, @syncexclude, @userwaypoint, @clue, @title)");
        command.ParametersAdd("@gccode", DbType.String, GcCode);
        command.ParametersAdd("@cacheid", DbType.Int64, CacheId);
        command.ParametersAdd("@latitude", DbType.Double, Latitude);
        command.ParametersAdd("@longitude", DbType.Double, Longitude);
        command.ParametersAdd("@description", DbType.String, Description);
        command.ParametersAdd("@type", DbType.Int16, Type);
        command.ParametersAdd("@syncexclude", DbType.Boolean, IsSyncExcluded);
        command.ParametersAdd("@userwaypoint", DbType.Boolean, IsUserWaypoint);
        command.ParametersAdd("@clue", DbType.String, Clue);
        command.ParametersAdd("@title", DbType.String, Title);
        command.ExecuteNonQuery();

        CBCommand commandUserData = Database.Data.CreateCommand("update Caches set HasUserData=@hasUserData where Id=@id");
        commandUserData.ParametersAdd("@hasUserData", DbType.Boolean, true);
        commandUserData.ParametersAdd("@id", DbType.Int64, CacheId);
        commandUserData.ExecuteNonQuery();
        commandUserData.Dispose();

    }

    public void UpdateDatabase()
    {
        CBCommand command = Database.Data.CreateCommand("update Waypoint set Latitude=@latitude, Longitude=@longitude, Description=@description, Type=@type, SyncExclude=@syncexclude, UserWaypoint=@userwaypoint,Clue=@clue, Title=@title where (CacheId=@cacheid and GcCode=@gccode)");
        command.ParametersAdd("@gccode", DbType.String, GcCode);
        command.ParametersAdd("@cacheid", DbType.Int64, CacheId);
        command.ParametersAdd("@latitude", DbType.Double, Latitude);
        command.ParametersAdd("@longitude", DbType.Double, Longitude);
        command.ParametersAdd("@description", DbType.String, Description);
        command.ParametersAdd("@type", DbType.Int16, Type);
        command.ParametersAdd("@syncexclude", DbType.Boolean, IsSyncExcluded);
        command.ParametersAdd("@userwaypoint", DbType.Boolean, IsUserWaypoint);
        command.ParametersAdd("@clue", DbType.String, Clue);
        command.ParametersAdd("@title", DbType.String, Title);
        command.ExecuteNonQuery();
        command.Dispose();

        CBCommand commandUserData = Database.Data.CreateCommand("update Caches set HasUserData=@hasUserData where Id=@id");
        commandUserData.ParametersAdd("@hasUserData", DbType.Boolean, true);
        commandUserData.ParametersAdd("@id", DbType.Int64, CacheId);
        commandUserData.ExecuteNonQuery();
        commandUserData.Dispose();
    }

    public void DeleteFromDatabase()
    {
        CBCommand command = Database.Data.CreateCommand("delete from Waypoint where GcCode=@gccode");
        command.ParametersAdd("@gccode", DbType.String, GcCode);
        command.ExecuteNonQuery();
        command.Dispose();
    }

    /// <summary>
    /// Entfernung von der letzten gültigen Position
    /// </summary>
    public float Distance
    {
      get
      {
        Coordinate fromPos = (Global.Marker.Valid) ? Global.Marker : Global.LastValidPosition;
        return (float)Datum.WGS84.Distance(Latitude, Longitude, fromPos.Latitude, fromPos.Longitude);
      }
    }


    public override int GetHashCode()
    {
      return GcCode.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      if (obj.GetType() != this.GetType())
        return false;

      return ((Waypoint)obj).GcCode == this.GcCode;
    }

  }
}
