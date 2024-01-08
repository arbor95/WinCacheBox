using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Windows.Forms;

namespace WinCachebox.CBSolver
{
    public class CoordinateEntity : Entity
  {
    string gcCode;
    public CoordinateEntity(int id, string gcCode) : base(id)
    {
      this.gcCode = gcCode;
    }

    public override void GetAllEntities(List<Entity> list)
    {      
    }

    public override void ReplaceTemp(Entity source, Entity dest)
    {
    }

    public override string Berechne()
    {
      // Coordinaten des Caches aus der DB auslesen
        CBCommand command = Database.Data.CreateCommand("select Latitude, Longitude from Caches where GcCode=@GcCode");
      command.ParametersAdd("@GcCode", DbType.String, gcCode);
      DbDataReader reader = command.ExecuteReader();
      if (reader.Read())
      {
        double Latitude = double.Parse(reader[0].ToString());
        double Longitude = double.Parse(reader[1].ToString());
        Coordinate coord = new Coordinate(Latitude, Longitude);
        if (coord.Valid)
        {
          reader.Dispose();
          command.Dispose();
          return coord.FormatCoordinate();
        }
      }
      reader.Dispose();
      command.Dispose();
      // kein Cache mit diesem GC gefunden -> Waypoint suchen
      command = Database.Data.CreateCommand("select Latitude, Longitude from Waypoint where GcCode=@GcCode");
      command.ParametersAdd("@GcCode", DbType.String, gcCode);
      reader = command.ExecuteReader();
      if (reader.Read())
      {
        double Latitude = double.Parse(reader[0].ToString());
        double Longitude = double.Parse(reader[1].ToString());
        Coordinate coord = new Coordinate(Latitude, Longitude);
        if (coord.Valid)
        {
          reader.Dispose();
          command.Dispose();
          return coord.FormatCoordinate();
        }
      }
      reader.Dispose();
      command.Dispose();

      return "Fehler";
    }

    public string SetCoordinate(string sCoord)
    {
      Coordinate coord = new Coordinate(sCoord);
      if (!coord.Valid)
        return "Koordinate not valid";

      Geocaching.Waypoint waypoint = null;
      // Suchen, ob dieser Waypoint bereits vorhanden ist.
      CBCommand command = Database.Data.CreateCommand("select GcCode, CacheId, Latitude, Longitude, Description, Type, SyncExclude, UserWaypoint, Clue, Title from Waypoint where GcCode=@GcCode");
      command.ParametersAdd("@gcCode", DbType.String, gcCode);
      DbDataReader reader = command.ExecuteReader();
      while (reader.Read())
          waypoint = new Geocaching.Waypoint(reader);
      if (waypoint == null)
      {
          return gcCode + " does not exist!";
      }
      reader.Dispose();
      command.Dispose();

      // evtl. bereits geladenen Waypoint aktualisieren
      Geocaching.Cache cache = Geocaching.Cache.GetCacheByCacheId(waypoint.CacheId);
      if (cache != null)
      {
          Geocaching.Waypoint cacheWaypoint = cache.GetWaypointByGcCode(gcCode);
          if (cacheWaypoint != null)
          {
              if (Global.SelectedCache != cache)
              {
                  // Zuweisung soll an einen Waypoint eines anderen als dem aktuellen Cache gemacht werden
                  // Sicherheitsabfrage, ob diese Zuweisung richtig ist!
                  string s = "Change Coordinates of a waypoint which does not belong to the actual cache?";
                  s += Environment.NewLine;
                  s += "Cache: [" + cache.Name + "]";
                  s += Environment.NewLine;
                  s += "Waypoint: [" + cacheWaypoint.Title + "]";
                  s += Environment.NewLine;
                  s += "Coordinates: [" + coord.FormatCoordinate() + "]";


                  if (MessageBox.Show(s, "Change Coordinates", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                  {
                      return "Refused to change Coordinates of Waypoint [" + cacheWaypoint.Title + "] of cache [" + cache.Name + "] with Coordinates [" + coord.FormatCoordinate() + "]";
                  }
              }
              cacheWaypoint.Latitude = coord.Latitude;
              cacheWaypoint.Longitude = coord.Longitude;
              for (int msi = 0; msi < Geocaching.Cache.MysterySolutions.Count; msi++)
              {
                  Geocaching.Cache.MysterySolution sol = Geocaching.Cache.MysterySolutions[msi];
                  if ((sol.Cache == cache) && (sol.Waypoint == cacheWaypoint))
                  {
                      sol.Latitude = coord.Latitude;
                      sol.Longitude = coord.Longitude;
                  }
              }
              if (Global.SelectedCache == cache)
                  Views.WaypointView.View.Refresh();
          }
      }

      waypoint.Coordinate = coord;
      waypoint.UpdateDatabase();

      return gcCode + "=" + coord.FormatCoordinate();
    }

    public override string ToString()
    {
      return "Gc" + id + ":(" + gcCode + ")";
    }

    public string GcCode { get { return gcCode; } }
  }
}
