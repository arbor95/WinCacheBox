using System;
using System.Collections.Generic;
using System.Globalization;

namespace WinCachebox
{
    public class FilterProperties
  {
    public int Finds = 0;

    public int Own = 0;

    public int NotAvailable = 0;

    public int Archived = 0;

    public int ContainsTravelbugs = 0;

    public int Favorites = 0;

    public int ListingChanged = 0;

    public int WithManualWaypoint = 0;

    public int HasUserData;

    public float MinDifficulty = 1;

    public float MaxDifficulty = 5;

    public float MinTerrain = 1;

    public float MaxTerrain = 5;

    public float MinContainerSize = 0;

    public float MaxContainerSize = 4;

    public float MinRating = 0;

    public float MaxRating = 5;

    public bool[] cacheTypes = new bool[] { true, true, true, true, true, true, true, true, true, true, true };

    public int[] attributesFilter = new int[] {0,0,0,0,0,0,0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    public List<Int64> GPXFilenameIds = new List<Int64>();

    public List<Int64> Categories = new List<Int64>();

    public string filterName = "";
    public string filterState = "";
    public string filterCountry = "";
    public int gpxAge = 0;

    const char seperator = ',';
    const char GPXseperator = '^';

    public override String ToString()
    {
      String result =
          Finds.ToString() + seperator +
          NotAvailable.ToString() + seperator +
          Archived.ToString() + seperator +
          Own.ToString() + seperator +
          ContainsTravelbugs.ToString() + seperator +
          Favorites.ToString() + seperator +
          HasUserData.ToString() + seperator +
          ListingChanged.ToString() + seperator +
          WithManualWaypoint.ToString() + seperator +
          MinDifficulty.ToString(NumberFormatInfo.InvariantInfo) + seperator +
          MaxDifficulty.ToString(NumberFormatInfo.InvariantInfo) + seperator + MinTerrain.ToString(NumberFormatInfo.InvariantInfo) + seperator + MaxTerrain.ToString(NumberFormatInfo.InvariantInfo) + seperator +
          MinContainerSize.ToString(NumberFormatInfo.InvariantInfo) + seperator + MaxContainerSize.ToString(NumberFormatInfo.InvariantInfo) + seperator +
          MinRating.ToString(NumberFormatInfo.InvariantInfo) + seperator +
          MaxRating.ToString(NumberFormatInfo.InvariantInfo);

      for (int i = 0; i < cacheTypes.Length; i++)
        result += seperator + cacheTypes[i].ToString();

      for (int i = 0; i < attributesFilter.Length; i++)
        result += seperator + attributesFilter[i].ToString();

      string tempGPX = string.Empty;
      for (int i = 0; i <= GPXFilenameIds.Count - 1; i++)
      {
        tempGPX += GPXseperator + GPXFilenameIds[i].ToString();
      }

      result += seperator + tempGPX;
      result += seperator + filterName;
      string tempCategory = string.Empty;
      foreach (int i in Categories)
      {
          tempCategory += GPXseperator + i.ToString();
      }
      result += seperator + tempCategory;

      result += seperator + filterState;
      result += seperator + filterCountry;

      result += seperator + gpxAge.ToString();
      return result;
    }

    public override bool Equals(object obj)
    {
      if (obj.GetType() != this.GetType())
        return false;

      return (obj as FilterProperties).ToString() == this.ToString();
    }

    public FilterProperties() { }

    public FilterProperties(String serialization)
    {
      try
      {
        String[] parts = serialization.Split(seperator);
        int cnt = 0;
        Finds = int.Parse(parts[cnt++]);
        NotAvailable = int.Parse(parts[cnt++]);
        Archived = int.Parse(parts[cnt++]);
        Own = int.Parse(parts[cnt++]);
        ContainsTravelbugs = int.Parse(parts[cnt++]);
        Favorites = int.Parse(parts[cnt++]);
        HasUserData = int.Parse(parts[cnt++]);
        ListingChanged = int.Parse(parts[cnt++]);
        WithManualWaypoint = int.Parse(parts[cnt++]);
        MinDifficulty = float.Parse(parts[cnt++], NumberFormatInfo.InvariantInfo);
        MaxDifficulty = float.Parse(parts[cnt++], NumberFormatInfo.InvariantInfo);
        MinTerrain = float.Parse(parts[cnt++], NumberFormatInfo.InvariantInfo);
        MaxTerrain = float.Parse(parts[cnt++], NumberFormatInfo.InvariantInfo);
        MinContainerSize = float.Parse(parts[cnt++], NumberFormatInfo.InvariantInfo);
        MaxContainerSize = float.Parse(parts[cnt++], NumberFormatInfo.InvariantInfo);
        MinRating = float.Parse(parts[cnt++], NumberFormatInfo.InvariantInfo);
        MaxRating = float.Parse(parts[cnt++], NumberFormatInfo.InvariantInfo);

        for (int i = 0; i < 11; i++)
          cacheTypes[i] = bool.Parse(parts[cnt++]);

        for (int i = 0; i < attributesFilter.Length; i++)
        {
            attributesFilter[i] = int.Parse(parts[cnt++]);
        }


        GPXFilenameIds.Clear();
        string tempGPX = parts[cnt++];
        String[] partsGPX = tempGPX.Split(GPXseperator);
        for (int i = 1; i < partsGPX.Length; i++)
        {
          GPXFilenameIds.Add(Convert.ToInt64(partsGPX[i]));
        }
        if (parts.Length > cnt)
          filterName = parts[cnt++];
        else
          filterName = "";
        string tmpCategory = parts[cnt++];
        string[] partsCat = tmpCategory.Split(GPXseperator);
        foreach (string sc in partsCat)
        {
            if (sc == "")
                continue;
            Categories.Add(Convert.ToInt64(sc));
        }

        if (parts.Length > cnt)
            filterState = parts[cnt++];
        else
            filterState = "";
        if (parts.Length > cnt)
            filterCountry = parts[cnt++];
        else
            filterCountry = "";

        if (parts.Length > cnt)
            int.TryParse(parts[cnt++], out gpxAge);
        else
            gpxAge = 0;


      }
      catch (Exception exc)
      {
#if DEBUG
        Global.AddLog("FilterProperties(" + serialization + "): " + exc.ToString());
#endif
      }
    }

    public String SqlWhere
    {
        get
        {
            List<String> andParts = new List<string>();

            if (Finds == 1)
                andParts.Add("Found=1");
            if (Finds == -1)
                andParts.Add("(Found=0 or Found is null)");

            if (NotAvailable == 1)
                andParts.Add("Available=0");
            if (NotAvailable == -1)
                andParts.Add("Available=1");

            if (Archived == 1)
                andParts.Add("Archived=1");
            if (Archived == -1)
                andParts.Add("Archived=0");

            if (Own == 1)
                andParts.Add("(Owner='" + Config.GetString("GcLogin").Replace("'", "''") + "')");
            if (Own == -1)
                andParts.Add("(not Owner='" + Config.GetString("GcLogin").Replace("'", "''") + "')");

            if (ContainsTravelbugs == 1)
                andParts.Add("NumTravelbugs > 0");
            if (ContainsTravelbugs == -1)
                andParts.Add("NumTravelbugs = 0");

            if (Favorites == 1)
                andParts.Add("Favorit=1");
            if (Favorites == -1)
                andParts.Add("(Favorit=0 or Favorit is null)");

            if (HasUserData == 1)
                andParts.Add("HasUserData=1");
            if (HasUserData == -1)
                andParts.Add("(HasUserData = 0 or HasUserData is null)");

            if (ListingChanged == 1)
                andParts.Add("ListingChanged=1");
            if (ListingChanged == -1)
                andParts.Add("(ListingChanged=0 or ListingChanged is null)");

            if (WithManualWaypoint == 1)
                andParts.Add(string.Format(" ID in ({0})", "select CacheId FROM Waypoint WHERE UserWaypoint = 1"));
            if (WithManualWaypoint == -1)
                andParts.Add(string.Format(" NOT ID in ({0})", "select CacheId FROM Waypoint WHERE UserWaypoint = 1"));

            andParts.Add("Difficulty >= " + ((short)(MinDifficulty * 2)).ToString());
            andParts.Add("Difficulty <= " + ((short)(MaxDifficulty * 2)).ToString());
            andParts.Add("Terrain >= " + ((short)(MinTerrain * 2)).ToString());
            andParts.Add("Terrain <= " + ((short)(MaxTerrain * 2)).ToString());
            andParts.Add("Size >= " + ((short)(MinContainerSize)).ToString());
            andParts.Add("Size <= " + ((short)(MaxContainerSize)).ToString());
            andParts.Add("Rating >= " + ((short)(MinRating * 100)).ToString());
            andParts.Add("Rating <= " + ((short)(MaxRating * 100)).ToString());

            /*
            String availability = "";
            if (AvailableCaches)
                availability += "Available=1";

            if (ArchivedCaches)
            {
                if (availability.Length > 0)
                    availability += " or ";
                availability += "Archived=1 or Available=0";
            }

            if (availability.Length > 0)
                andParts.Add("(" + availability + ")");
            */

            String csvTypes = String.Empty;
            for (int i = 0; i < 11; i++)
                if (cacheTypes[i])
                    csvTypes += i.ToString() + ",";

            if (csvTypes.Length > 0)
            {
                csvTypes = csvTypes.Substring(0, csvTypes.Length - 1);
                andParts.Add("Type in (" + csvTypes + ")");
            }





            for (int i = 0; i < attributesFilter.Length; i++)
            {
                if (attributesFilter[i] != 0)
                {
                    if (i < 62)
                    {
                        ulong shift = 1UL << (i + 1);

                        if (Database.Data.serverType == Database.SqlServerType.SQLite)
                        {
                            // SQLite kennt keinen Convert Befehl...
                            if (attributesFilter[i] == 1)
                                andParts.Add("AttributesPositive & " + shift + " > 0");
                            else
                                andParts.Add("AttributesNegative & " + shift + " > 0");
                        }
                        else
                        {
                            if (attributesFilter[i] == 1)
                                andParts.Add("AttributesPositive & Convert(bigint, " + shift + ") > 0");
                            else
                                andParts.Add("AttributesNegative & Convert(bigint, " + shift + ") > 0");
                        }
                    }
                    else
                    {
                        ulong shift = 1UL << (i -62);

                        if (Database.Data.serverType == Database.SqlServerType.SQLite)
                        {
                            if (attributesFilter[i] == 1)
                                andParts.Add("AttributesPositiveHigh & " + shift + " > 0");
                            else
                                andParts.Add("AttributesNegativeHigh & " + shift + " > 0");
                        } else
                        {
                            if (attributesFilter[i] == 1)
                                andParts.Add("AttributesPositiveHigh & Convert(bigint, " + shift + ") > 0");
                            else
                                andParts.Add("AttributesNegativeHigh & Convert(bigint, " + shift + ") > 0");
                        }

                    }

                   
                }
            }

            if (GPXFilenameIds.Count != 0)
            {
                string s = string.Empty;
                foreach (Int64 id in GPXFilenameIds)
                    s += id.ToString() + ",";
                s += "-1";
                andParts.Add("GPXFilename_Id not in (" + s + ")");
            }
            if (filterName != "")
            {
                andParts.Add("Name like '%" + filterName + "%'");
            }


            if (filterCountry != "")
            {
                bool empty = false;
                string countries = "";
                foreach (string item in filterCountry.Split(';'))
                {
                    if (item.Equals(Global.Translations.Get("empty")))
                    { empty = true; countries += "'' ,"; }
                    else countries += "'" + item + "' ,";
                }
                if (countries.Length > 0)
                    countries = countries.Substring(0, countries.Length - 2);

                if (empty)
                    andParts.Add("([Country] IN (" + countries + ") OR Country IS NULL OR Country IS '')");
                else
                    andParts.Add("[Country] IN (" + countries + ") ");
            }

            if (filterState != "")
            {
                bool empty = false;
                string states = "";
                foreach (string item in filterState.Split(';'))
                {
                    if (item.Equals(Global.Translations.Get("empty")))
                    { empty = true; states += "'' ,"; }
                    else 
                        states += "'" + item + "' ,";
                }
                if (states.Length > 0)
                    states = states.Substring(0, states.Length - 2);

                if (empty)
                    andParts.Add("([State] IN (" + states + ") OR State IS NULL OR State IS '')");
                else
                    andParts.Add("[State] IN (" + states + ") ");
            }
            //else
            //    if (filterCountry.Length > 0)
            //        andParts.Add("[State] IN ('get empty result') ");

            if (gpxAge != 0)
            {
                if (Database.Data.serverType == Database.SqlServerType.SQLite)
                    andParts.Add(string.Format(" GPXFilename_Id in ({0})", "Select ID FROM GPXFilenames WHERE (julianday(Date('now')) - julianday(Imported)) > " + gpxAge));
                else
                    andParts.Add(string.Format(" GPXFilename_Id in ({0})", "Select ID FROM GPXFilenames WHERE DateDIFF(day, Imported, getdate()) > " + gpxAge));
            }

            return String.Join(" and ", andParts.ToArray());
        }
    }

    public override int GetHashCode()
    {
      return SqlWhere.GetHashCode();
    }
  }
}
