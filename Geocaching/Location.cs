using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace WinCachebox.Geocaching
{
    public class Location
  {
    public long Id;

    public string Name;

    public Coordinate Coordinate = new Coordinate();
    /// <summary>
    /// Breitengrad
    /// </summary>
    public double Latitude { get { return Coordinate.Latitude; } set { Coordinate.Latitude = value; } }

    /// <summary>
    /// L?ngengrad
    /// </summary>
    public double Longitude { get { return Coordinate.Longitude; } set { Coordinate.Longitude = value; } }

    public Location(string name, double latitude, double longitude)
    {
      this.Name = name;
      this.Id = -1;
      this.Coordinate = new Coordinate(latitude, longitude);
    }

    public Location(DbDataReader reader)
    {
      Id = long.Parse(reader[0].ToString());
      Name = reader[1].ToString().Trim();
      Latitude = double.Parse(reader[2].ToString());
      Longitude = double.Parse(reader[3].ToString());
    }

    public bool Edit()
    {
      if (Views.Forms.LocationEdit.Edit(this))
      {
        Write();
        return true;
      }
      else 
        return false;
    }

    public void Write()
    {
      if (Id >= 0)
      {
        // ID schon vorhanden -> Update
          CBCommand command = Database.Data.CreateCommand("update Locations set Name=@Name, Latitude=@Latitude, Longitude=@Longitude where Id=@id");
          command.ParametersAdd("@Id", DbType.Int64, Id);
          command.ParametersAdd("@Name", DbType.String, Name);
          command.ParametersAdd("@Latitude", DbType.Double, Latitude);
          command.ParametersAdd("@Longitude", DbType.Double, Longitude);
          command.ExecuteNonQuery();
          command.Dispose();
      }
      else
      {
        // neuer Eintrag
          CBCommand command = Database.Data.CreateCommand("insert into Locations (Name, Latitude, Longitude) values (@Name, @Latitude, @Longitude)");
          command.ParametersAdd("@Name", DbType.String, Name);
          command.ParametersAdd("@Latitude", DbType.Double, Latitude);
          command.ParametersAdd("@Longitude", DbType.Double, Longitude);
          command.ExecuteNonQuery();
          command.Dispose();
          // Id auslesen
          command = Database.Data.CreateCommand("select max(Id) from Locations");
          Id = Convert.ToInt32(command.ExecuteScalar().ToString());
          command.Dispose();
      }
    }

    public void Delete()
    {
        CBCommand command = Database.Data.CreateCommand("delete from Locations where Id=@id");
        command.ParametersAdd("@Id", DbType.Int64, Id);
        command.ExecuteNonQuery();
        command.Dispose();
    }

    public static List<Location> Locations = new List<Location>();

    public static void LoadLocations()
    {
        CBCommand command = Database.Data.CreateCommand("select Id, Name, Latitude, Longitude from Locations");
        DbDataReader reader = command.ExecuteReader();
        Locations = new List<Location>();

        while (reader.Read())
        {
            Location location = new Location(reader);
            Locations.Add(location);
        }

        reader.Dispose();
        command.Dispose();
    }
  }
}