using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using WinCachebox.Types;
using System.Text.RegularExpressions;

namespace WinCachebox.Geocaching
{
    public enum CacheTypes
    {
        Traditional = 0,
        Multi = 1,
        Mystery = 2,
        Camera = 3,
        Earth = 4,
        Event = 5,
        MegaEvent = 6,
        CITO = 7,
        Virtual = 8,
        Letterbox = 9,
        Wherigo = 10,
        ReferencePoint = 11,
        Wikipedia = 12,
        Undefined = 13,
        MultiStage = 14,
        MultiQuestion = 15,
        Trailhead = 16,
        ParkingArea = 17,
        Final = 18,
        Cache = 19,
        MyParking = 20,
        Munzee = 21
    };

    public class GpxImport
    {
        public BoundingBox BoundingBox = new BoundingBox();

        string TourName = String.Empty;
        string GPXFilename = String.Empty;

        FormImportPocketQuery parent;

        /// <summary>
        /// Wird verwendet, um das Type-Feld in einen Datenbank-konformen Typ
        /// umzuwandeln
        /// </summary>
        Dictionary<String, CacheTypes> typeLookup = new Dictionary<string, CacheTypes>();

        /// <summary>
        /// Wird verwendet, um das Size-Feld in einen Datenbank-konformen Typ
        /// umzuwandeln
        /// </summary>
        Dictionary<String, int> sizeLookup = new Dictionary<string, int>();

        /// <summary>
        /// Wird verwendet, um das Type-Feld der Logs nachzuschlagen
        /// </summary>
        Dictionary<String, short> logLookup = new Dictionary<string, short>();

        public GpxImport(FormImportPocketQuery parent)
        {
            this.parent = parent;

            // Cachetypen (Groundspeak, GSAK, OC)
            typeLookup.Add("multi-cache", CacheTypes.Multi);
            typeLookup.Add("multi", CacheTypes.Multi);
            typeLookup.Add("traditional cache", CacheTypes.Traditional);
            typeLookup.Add("Traditional", CacheTypes.Traditional);
            typeLookup.Add("unknown cache", CacheTypes.Mystery);
            typeLookup.Add("earthcache", CacheTypes.Earth);
            typeLookup.Add("virtual cache", CacheTypes.Virtual);
            typeLookup.Add("letterbox hybrid", CacheTypes.Letterbox);
            typeLookup.Add("webcam cache", CacheTypes.Camera);
            typeLookup.Add("event cache", CacheTypes.Event);
            typeLookup.Add("wherigo cache", CacheTypes.Wherigo);
            typeLookup.Add("cache in trash out event", CacheTypes.CITO);
            typeLookup.Add("mega-event cache", CacheTypes.MegaEvent);
            typeLookup.Add("giga-event cache", CacheTypes.MegaEvent);
            // waypoint types
            typeLookup.Add("waypoint|trailhead", CacheTypes.Trailhead);
            typeLookup.Add("waypoint|parking area", CacheTypes.ParkingArea);
            typeLookup.Add("waypoint|question to answer", CacheTypes.MultiQuestion);
            typeLookup.Add("waypoint|stages of a multicache", CacheTypes.MultiStage);
            typeLookup.Add("waypoint|reference point", CacheTypes.ReferencePoint);
            typeLookup.Add("waypoint|final location", CacheTypes.Final);
            typeLookup.Add("waypoint trailhead", CacheTypes.Trailhead);
            typeLookup.Add("waypoint parking area", CacheTypes.ParkingArea);
            typeLookup.Add("waypoint question to answer", CacheTypes.MultiQuestion);
            typeLookup.Add("waypoint stages of a multicache", CacheTypes.MultiStage);
            typeLookup.Add("waypoint reference point", CacheTypes.ReferencePoint);
            typeLookup.Add("waypoint final location", CacheTypes.Final);
            typeLookup.Add("waypoint|virtual stage", CacheTypes.MultiQuestion);
            typeLookup.Add("waypoint|physical stage", CacheTypes.MultiStage);

            // GeoToad
            typeLookup.Add("multicache", CacheTypes.Multi);
            typeLookup.Add("traditional", CacheTypes.Traditional);
            typeLookup.Add("unknown", CacheTypes.Mystery);
            typeLookup.Add("virtual", CacheTypes.Virtual);
            typeLookup.Add("webcam", CacheTypes.Camera);
            typeLookup.Add("event", CacheTypes.Event);
            typeLookup.Add("wherigo", CacheTypes.Wherigo);
            typeLookup.Add("mega-event", CacheTypes.MegaEvent);

            // GCTour
            typeLookup.Add("other", CacheTypes.Mystery);
            typeLookup.Add("mystery/puzzle cache", CacheTypes.Mystery);
            sizeLookup.Add("none", 0);
            sizeLookup.Add("very large", 4);
            typeLookup.Add("whereigo cache", CacheTypes.Wherigo);

            // Cachegröße. Hier hält sich GeoToad ausnahmsweise mal an den
            // Groundspeak-Standard
            sizeLookup.Add("micro", 1);
            sizeLookup.Add("small", 2);
            sizeLookup.Add("regular", 3);
            sizeLookup.Add("large", 4);
            sizeLookup.Add("not chosen", 0);
            sizeLookup.Add("virtual", 0);
            sizeLookup.Add("other", 0);

            // Log-Typen (Groundspeak, GSAK)
            logLookup.Add("found it", 0);
            logLookup.Add("found", 0);
            logLookup.Add("didn't find it", 1);
            logLookup.Add("not found", 1);
            logLookup.Add("write note", 2);
            logLookup.Add("publish listing", 3);
            logLookup.Add("enable listing", 4);
            logLookup.Add("needs maintenance", 5);
            logLookup.Add("temporarily disable listing", 6);
            logLookup.Add("owner maintenance", 7);
            logLookup.Add("update coordinates", 7);
            logLookup.Add("will attend", 8);
            logLookup.Add("attended", 9);
            logLookup.Add("webcam photo taken", 10);
            logLookup.Add("archive", 11);
            logLookup.Add("unarchive", 11);
            logLookup.Add("post reviewer note", 12);
            logLookup.Add("needs archived", 13);

            // GeoToad
            logLookup.Add("other", 2);
            logLookup.Add("note", 2);
            logLookup.Add("geocoins: ", 2);
            logLookup.Add("cache disabled!", 6);
            logLookup.Add("retract listing", 11);
        }

        private struct CacheInfo
        {
            public long id;
            public int ListingCheckSum;
            public bool ListingChanged;
            public bool ImagesUpdated;
            public bool DescriptionImagesUpdated;
            public bool Found;
            public bool CorrectedCoordinates;
            public double Latitude;
            public double Longitude;
            public bool favorite;
            public long GpxFilename_Id;
        }

        public void ImportGpx(String[] files)
        {
            parent.setTitle("Importing GPX");
            parent.ProgressChanged("Indexing DB", 0, -1);
            Dictionary<String, CacheInfo> existsCache = new Dictionary<String, CacheInfo>();

            CBCommand query = Database.Data.CreateCommand("select GcCode, Id, ListingCheckSum, ImagesUpdated, DescriptionImagesUpdated, ListingChanged, Found, CorrectedCoordinates, Latitude, Longitude, GpxFilename_Id, Favorit from Caches");
            DbDataReader reader = query.ExecuteReader();
            CacheInfo cacheInfo = new CacheInfo();
            while (reader.Read())
            {
                try
                {
                    cacheInfo.id = reader.GetInt64(1);

                    if (reader.IsDBNull(2))
                    {
                        cacheInfo.ListingCheckSum = 0;
                    }
                    else
                    {
                        cacheInfo.ListingCheckSum = reader.GetInt32(2);
                    }

                    if (reader.IsDBNull(3))
                    {
                        cacheInfo.ImagesUpdated = false;
                    }
                    else
                    {
                        cacheInfo.ImagesUpdated = reader.GetBoolean(3);
                    }

                    if (reader.IsDBNull(4))
                    {
                        cacheInfo.DescriptionImagesUpdated = false;
                    }
                    else
                    {
                        cacheInfo.DescriptionImagesUpdated = reader.GetBoolean(4);
                    }

                    if (reader.IsDBNull(5))
                    {
                        cacheInfo.ListingChanged = false;
                    }
                    else
                    {
                        cacheInfo.ListingChanged = reader.GetBoolean(5);
                    }

                    if (reader.IsDBNull(6))
                    {
                        cacheInfo.Found = false;
                    }
                    else
                    {
                        cacheInfo.Found = reader.GetBoolean(6);
                    }

                    if (reader.IsDBNull(7))
                    {
                        cacheInfo.CorrectedCoordinates = false;
                    }
                    else
                    {
                        cacheInfo.CorrectedCoordinates = reader.GetBoolean(7);
                    }

                    if (reader.IsDBNull(8))
                    {
                        cacheInfo.Latitude = 361;
                    }
                    else
                    {
                        cacheInfo.Latitude = reader.GetDouble(8);
                    }
                    if (reader.IsDBNull(9))
                    {
                        cacheInfo.Longitude = 361;
                    }
                    else
                    {
                        cacheInfo.Longitude = reader.GetDouble(9);
                    }

                    cacheInfo.GpxFilename_Id = reader.GetInt64(10);

                    if (reader.IsDBNull(11))
                    {
                        cacheInfo.favorite = false;
                    }
                    else
                    {
                        cacheInfo.favorite = reader.GetBoolean(11);
                    }

                    existsCache.Add(reader.GetString(0), cacheInfo);
                }
                catch (Exception)
                {
                    continue;
                }
            }

            reader.Dispose();
            query.Dispose();

            // Wegpunkte laden
            Dictionary<String, object> existsWaypoint = new Dictionary<string, object>();
            CBCommand waypointQuery = Database.Data.CreateCommand("select GcCode from Waypoint");
            reader = waypointQuery.ExecuteReader();

            while (reader.Read())
                existsWaypoint.Add(reader.GetString(0).Trim(), null);

            //// Set mit IDs von existierenden Logs erstellen
            //DataSet dataSetTemp = new DataSet();
            //SqlCeDataAdapter adapterTemp = new SqlCeDataAdapter();
            //adapterTemp.SelectCommand = new SqlCeCommand("select Id from Logs", Database.Connection);
            //new SqlCeCommandBuilder(adapterTemp);
            //adapterTemp.Fill(dataSetTemp);

            //Dictionary<Int64, object> existsLog = new Dictionary<long, object>();
            //foreach (DataRow row in dataSetTemp.Tables[0].Rows)
            //    existsLog.Add(Int64.Parse(row["Id"].ToString()), null);

            //dataSetTemp.Dispose();
            //adapterTemp.Dispose();

            int fileIdx = 1;
            foreach (String file in files)
            {
                parent.setTitle("Importing GPX (" + fileIdx + " / " + files.Length + ")");

                string fileForCategory = file;

                fileIdx++;

                // Loggen, aus welche GPX Datei gelesen wurde
                // Category suchen, die zu der GpxDatei gehört
                Category category = Global.Categories.GetCategory(fileForCategory);
                if (category == null)
                    continue;   // should not happen!!!

                GpxFilename gpxFilename = category.NewGpxFilename(fileForCategory);
                if (gpxFilename == null)
                    continue;


                DateTime now = DateTime.Now;
                // Liste zum zwischenspeichern der GpxFilename, die durch diesen Import erzeugt wurden
                Dictionary<long, GpxFilename> NewGpxFilenames = new Dictionary<long, GpxFilename>();

                StreamReader streamReader = null;

                try
                {
                    streamReader = initializeReader(file);

                    string Name = string.Empty;
                    int cachesSinceLastMemoryTest = 0;

                    // Waypoint lesen
                    while (!parent.Cancel)
                    {
                        // Einzelnen Waypoint einlesen
                        XmlDocument xml = new XmlDocument();

                        String xmlLine = readXmlWaypoint(streamReader);
                        if (xmlLine == null || xmlLine.Length == 0)
                            break;

                        try
                        {
                            xml.Load(new StringReader(xmlLine));
                        }
                        catch (XmlException)
                        {
                            continue;
                        }


                        XmlNodeList wps = xml.GetElementsByTagName("wpt");

                        // Fortschritt berechnen
                        int progressCurrent = (int)(streamReader.BaseStream.Position / 1024);
                        int progressTotal = (int)(streamReader.BaseStream.Length / 1024);

                        if (wps.Count > 0)
                        {
                            // Daten in Datenbank übertragen
                            XmlNode node = wps[0];
                            XmlElement element = (XmlElement)node;
                            String GcCode = element.GetElementsByTagName("name")[0].InnerText.ToUpper();

                            if (element.GetElementsByTagName("type").Count == 0 || element.GetElementsByTagName("type")[0].InnerText.ToLower().IndexOf("waypoint") == -1)
                            {
                                XmlElement cacheElements;
                                if (element.GetElementsByTagName("groundspeak:cache").Count > 0)
                                    cacheElements = (XmlElement)element.GetElementsByTagName("groundspeak:cache")[0];
                                else
                                    cacheElements = (XmlElement)element.GetElementsByTagName("geocache")[0];

                                if (cacheElements == null)
                                    cacheElements = element;

                                // Tabellen-ID aus GcCode generieren
                                char[] dummy = GcCode.ToCharArray();
                                byte[] byteDummy = new byte[8];
                                for (int i = 0; i < 8; i++)
                                    if (i < GcCode.Length)
                                        byteDummy[i] = (byte)dummy[i];
                                    else
                                        byteDummy[i] = 0;

                                long id = BitConverter.ToInt64(byteDummy, 0);

                                CBCommand command;
                                bool doNotUpdateKoords = false;
                                //String notes = String.Empty;
                                bool cacheExists = existsCache.ContainsKey(GcCode);
                                if (cacheExists)
                                {
                                    command = Database.Data.CreateCommand("update Caches set GcCode=@gccode, Latitude=@latitude, Longitude=@longitude, Name=@name, Size=@size, Difficulty=@difficulty, Terrain=@terrain, Archived=@archived, Available=@available, Found=@found, Type=@type, PlacedBy=@placedby, Owner=@owner, DateHidden=@datehidden, Hint=@hint, Description=@description, Url=@url, NumTravelbugs=@numtravelbugs, GcId=@gcid, AttributesPositive=@AttributesPositive, AttributesPositiveHigh=@AttributesPositiveHigh, AttributesNegative=@AttributesNegative, AttributesNegativeHigh=@AttributesNegativeHigh, TourName=@TourName, GPXFilename_ID=@GPXFilename_ID, ListingCheckSum=@ListingCheckSum, ListingChanged=@ListingChanged, ImagesUpdated=@ImagesUpdated, DescriptionImagesUpdated=@DescriptionImagesUpdated, CorrectedCoordinates=@CorrectedCoordinates, [state]=@state, country=@country, Favorit=@Favorit where Id=@id");
                                    doNotUpdateKoords = existsCache[GcCode].CorrectedCoordinates;
                                    cacheInfo = existsCache[GcCode];
                                }
                                else
                                {
                                    cacheInfo.id = id;
                                    cacheInfo.ListingCheckSum = 0;
                                    cacheInfo.ImagesUpdated = false;
                                    cacheInfo.DescriptionImagesUpdated = false;
                                    cacheInfo.ListingChanged = false;
                                    cacheInfo.Found = false;

                                    existsCache.Add(GcCode, cacheInfo);

                                    command = Database.Data.CreateCommand("insert into Caches(Id, GcCode, Latitude, Longitude, Name, Size, Difficulty, Terrain, Archived, Available, Found, Type, PlacedBy, Owner, DateHidden, Hint, Description, Url, NumTravelbugs, GcId, Rating, Vote, VotePending, AttributesPositive, AttributesNegative, TourName, GPXFilename_ID, ListingCheckSum, ListingChanged, ImagesUpdated, DescriptionImagesUpdated, CorrectedCoordinates, FirstImported, [state], country, Favorit) values(@id, @gccode, @latitude, @longitude, @name, @size, @difficulty, @terrain, @archived, @available, @found, @type, @placedby, @owner, @datehidden, @hint, @description, @url, @numtravelbugs, @gcid, @rating, @vote, @votepending, @AttributesPositive, @AttributesNegative, @TourName, @GPXFilename_ID, @ListingCheckSum, @ListingChanged, @ImagesUpdated, @DescriptionImagesUpdated, @CorrectedCoordinates, @FirstImported, @state, @country, @Favorit)");
                                    command.ParametersAdd("@rating", DbType.Single, 0);
                                    command.ParametersAdd("@vote", DbType.Int16, 0);
                                    command.ParametersAdd("@votepending", DbType.Boolean, false);
                                }
                                command.ParametersAdd("FirstImported", DbType.DateTime, now);
                                command.ParametersAdd("@id", DbType.Int64, id);
                                command.ParametersAdd("@gccode", DbType.String, GcCode);

                                double latitude;
                                double longitude;

                                try
                                {
                                    latitude = Double.Parse(element.Attributes.GetNamedItem("lat").InnerText, NumberFormatInfo.InvariantInfo);
                                    longitude = Double.Parse(element.Attributes.GetNamedItem("lon").InnerText, NumberFormatInfo.InvariantInfo);
                                }
                                catch (Exception)
                                {
                                    throw new Exception("Error parsing cache coordinates.");
                                }

                                BoundingBox.Add(latitude, longitude);

                                command.ParametersAdd("@latitude", DbType.Double, latitude);
                                command.ParametersAdd("@longitude", DbType.Double, longitude);

                                // Kompatibilität zu GSAK
                                String name = getElement(element, "groundspeak:name", "urlname", "name");
                                String GcId = cacheElements.GetAttribute("id");
                                command.ParametersAdd("@gcid", DbType.String, GcId);

                                command.ParametersAdd("@name", DbType.String, name.Trim());
                                try
                                {
                                    if (GcCode.IndexOf("MZ") == 0) //Cache ist ein Munzee
                                    {
                                        command.ParametersAdd("@difficulty", DbType.Int16, (short)(0));
                                        command.ParametersAdd("@terrain", DbType.Int16, (short)(0));
                                    }
                                    else
                                    {
                                        command.ParametersAdd("@difficulty", DbType.Int16, (short)(float.Parse(getElementDefault(cacheElements, "2", "groundspeak:difficulty", "difficulty"), NumberFormatInfo.InvariantInfo) * 2));
                                        command.ParametersAdd("@terrain", DbType.Int16, (short)(float.Parse(getElementDefault(cacheElements, "2", "groundspeak:terrain", "terrain"), NumberFormatInfo.InvariantInfo) * 2));
                                    }
                                }
                                catch (Exception)
                                {
                                    throw new Exception("Error parsing Difficulty / Terrain.");
                                }
                                if (cacheElements.GetAttribute("status").Length > 0)
                                {
                                    String status = cacheElements.GetAttribute("status");
                                    command.ParametersAdd("@available", DbType.Boolean, status == "Available");
                                    command.ParametersAdd("@archived", DbType.Boolean, status == "Archived");
                                }
                                else
                                {
                                    command.ParametersAdd("@available", DbType.Boolean, bool.Parse(getAttributeDefault(cacheElements, "true", "available")));
                                    command.ParametersAdd("@archived", DbType.Boolean, bool.Parse(getAttributeDefault(cacheElements, "true", "archived")));
                                }

                                // Import von Cachewolf lieferte keine gefundenen. Durch .ToLower() werden jetzt die gefundenen als gefunden markiert...
                                // only import found status, when cache was already found in CB.
                                if (existsCache[GcCode].Found == false)
                                {
                                    command.ParametersAdd("@found", DbType.Boolean, getElement(element, "sym").ToLower() == "Geocache Found".ToLower());
                                }
                                else
                                {
                                    command.ParametersAdd("@found", DbType.Boolean, true);
                                }

                                // use always "owner" for non groundspeak gpx files.
                                string placedBy = getElement(cacheElements, "groundspeak:placed_by", "owner");
                                string owner = getElement(cacheElements, "groundspeak:owner", "owner");

                                command.ParametersAdd("@placedby", DbType.String, placedBy);
                                command.ParametersAdd("@owner", DbType.String, owner);

                                DateTime dateHidden = DateTime.Now;
                                try
                                {
                                    dateHidden = DateTime.Parse(getElementDefault(element, "2001-05-01", "time"), NumberFormatInfo.InvariantInfo);
                                }
                                catch (Exception)
                                {
                                    parent.ReportUncriticalError("Malformed date format detected!");
                                }

                                command.ParametersAdd("@datehidden", DbType.DateTime, dateHidden);

                                if (element.GetElementsByTagName("groundspeak:encoded_hints").Count > 0)
                                    command.ParametersAdd("@hint", DbType.String, element.GetElementsByTagName("groundspeak:encoded_hints")[0].InnerText);
                                else
                                    command.ParametersAdd("@hint", DbType.String, getElement(cacheElements, "hints"));

                                String short_html = "";

                                if (cacheElements.GetElementsByTagName("groundspeak:short_description") != null)
                                {
                                    short_html = getElement(cacheElements, "groundspeak:short_description").Trim();

                                    if (short_html.Length > 0)
                                        short_html += "<br><br>";

                                    if (cacheElements.GetElementsByTagName("groundspeak:short_description").Count > 0
                                    && cacheElements.GetElementsByTagName("groundspeak:short_description")[0].Attributes["html"] != null
                                    && bool.Parse(cacheElements.GetElementsByTagName("groundspeak:short_description")[0].Attributes["html"].Value) == false)
                                        short_html = short_html.Replace("\r\n", "<br>");
                                }
                                String html = "";

                                if (cacheElements.GetElementsByTagName("groundspeak:long_description") != null)
                                {
                                    if (cacheElements.GetElementsByTagName("groundspeak:long_description").Count > 0
                                        && cacheElements.GetElementsByTagName("groundspeak:long_description")[0].Attributes["html"] != null
                                        && bool.Parse(cacheElements.GetElementsByTagName("groundspeak:long_description")[0].Attributes["html"].Value) == true)
                                        html = short_html + getElement(cacheElements, "groundspeak:long_description", "description").Trim();
                                    else
                                        html = short_html + getElement(cacheElements, "groundspeak:long_description", "description").Trim().Replace("\r\n", "<br>");
                                }

                                // HTML-Code der Beschreibung anpassen
                                HtmlTools.BBCodeToHtml(ref html);

                                // Make the Additional Waypoints more readable
                                if (html.LastIndexOf("<p>Additional Hidden Waypoints</p>") >= 0 || html.LastIndexOf("<p>Additional Waypoints</p>") >= 0)
                                {
                                    int index = 0;
                                    if (html.LastIndexOf("<p>Additional Hidden Waypoints</p>") >= 0)
                                    {
                                        index = html.LastIndexOf("<p>Additional Hidden Waypoints</p>") + "<p>Additional Hidden Waypoints</p>".Length;
                                    }
                                    else
                                    {
                                        index = html.LastIndexOf("<p>Additional Waypoints</p>") + "<p>Additional Waypoints</p>".Length;
                                    }

                                    html = html.Insert(index, "<table border=1><tr><td>");

                                    while (index < html.Length)
                                    {
                                        index = html.IndexOf("<br />", index + 1);
                                        if (index == -1)
                                            break;
                                        index = html.IndexOf("<br />", index + 1);
                                        // Gefahr von Endlosschleife!!!
                                        if (index == -1)
                                            break;
                                        index = html.IndexOf("<br />", index + 1);
                                        // Gefahr von Endlosschleife!!!
                                        if (index == -1)
                                            break;
                                        index += 6;

                                        if (index < html.Length)
                                            html = html.Insert(index, "</td></tr><tr><td>");
                                    }

                                    html = html.Insert(html.Length, "</td></tr></table>");

                                }

                                String stringForListingCheckSum = html;

                                // URL nachschlagen (GSAK-konform)
                                String url = getElement(element, "url").Trim();

                                if (url.Length == 0 && element.GetElementsByTagName("link").Count > 0)
                                {
                                    XmlElement link = (XmlElement)element.GetElementsByTagName("link")[0];
                                    url = link.GetAttribute("href");
                                }

                                if (url.Length == 0)
                                    parent.ReportUncriticalError("Cannot read cache url!");

                                command.ParametersAdd("@url", DbType.String, url);

                                // Cachetyp bestimmen
                                String type = getElement(cacheElements, "groundspeak:type", "type").ToLower();

                                if (GcCode.IndexOf("MZ") == 0) //Cache ist ein Munzee
                                {
                                    command.ParametersAdd("@type", DbType.Int16, CacheTypes.Munzee);
                                }
                                else
                                {
                                    if (typeLookup.ContainsKey(type))
                                        command.ParametersAdd("@type", DbType.Int16, typeLookup[type]);
                                    else
                                    {
                                        parent.ReportUncriticalError("Unknown cache type \"" + type + "\", defaulting to 'unknown'");
#if DEBUG
                                        Global.AddLog("\n\nGpxImport: unknown type " + type + " - cache : " + name + " \n\n");

#endif
                                        command.ParametersAdd("@type", DbType.Int16, CacheTypes.Mystery);
                                    }
                                }

                                String size = getElement(cacheElements, "groundspeak:container", "container").ToLower();

                                if (size == "")
                                    command.ParametersAdd("@size", DbType.Int64, 0);
                                else
                                    if (sizeLookup.ContainsKey(size))
                                        command.ParametersAdd("@size", DbType.Int64, (long)sizeLookup[size]);
                                    else
                                    {
                                        parent.ReportUncriticalError("Unknown cache size \"" + size + "\"");
#if DEBUG
                    Global.AddLog("\n\ngpxImport: unknown size " + size + "\n\n");
#endif
                                        command.ParametersAdd("@size", DbType.Int64, 0);
                                    }

                                String country = getElement(cacheElements, "groundspeak:country", "country");
                                command.ParametersAdd("@country", DbType.String, country);
                                String state = getElement(cacheElements, "groundspeak:state", "state");
                                command.ParametersAdd("@state", DbType.String, state);
                                
                                // Anzahl der Travelbugs lesen
                                if (cacheElements.GetElementsByTagName("groundspeak:travelbugs").Count > 0)
                                {

                                    XmlElement tbElements = (XmlElement)cacheElements.GetElementsByTagName("groundspeak:travelbugs")[0];
                                    command.ParametersAdd("@numtravelbugs", DbType.Int16, tbElements.GetElementsByTagName("groundspeak:travelbug").Count);
                                }
                                else
                                    command.ParametersAdd("@numtravelbugs", DbType.Int16, 0);




                                XmlNodeList attributes = cacheElements.GetElementsByTagName("groundspeak:attribute");

                                if (attributes.Count == 0)
                                    attributes = cacheElements.GetElementsByTagName("attribute");


                                Cache tmp = new Cache
                                {
                                    AttributesNegative = new DLong(0, 0),
                                    AttributesPositive = new DLong(0, 0)
                                };

                                if (attributes.Count > 0)
                                {


                                   


                                    for (int i = 0; i < attributes.Count; i++)
                                    {
                                        XmlElement attributeXml = (XmlElement)attributes[i];
                                        XmlNode attributeNode = attributeXml.Attributes.GetNamedItem("id");
                                        if (attributeNode == null)
                                            continue;

                                        int attributeId;
                                        int attributeInc;

                                        try
                                        {
                                            attributeId = Int16.Parse(getAttributeDefault(attributeXml, "id", "id"));
                                            attributeInc = Int16.Parse(getAttributeDefault(attributeXml, "inc", "inc"));
                                        }
                                        catch (Exception)
                                        {
                                            //                                            throw new Exception("Error parsing cache attributes.");
                                            // Import muss wegen eines fehlerhaften (oder leeren) Attributes nicht gleich komplett abgebrochen werden
                                            // Dieses Attribut kann einfach ignoriert und der Import fortgesetzt werden
                                            continue;
                                        }

                                        if (attributeInc == 1)
                                        {
                                            tmp.addAttributePositive(Attributes.getAttributeEnumByGcComId(attributeId));
                                        }
                                        else
                                        {
                                            tmp.addAttributeNegative(Attributes.getAttributeEnumByGcComId(attributeId));
                                        }
                                    }
                                }

                                ulong AttributePositiveLow = tmp.AttributesPositive.getLow();
                                ulong AttributePositiveHigh = tmp.AttributesPositive.getHigh();

                                ulong AttributesNegativeLow = tmp.AttributesNegative.getLow();
                                ulong AttributesNegativeHigh = tmp.AttributesNegative.getHigh();

                                command.ParametersAdd("@AttributesPositive", DbType.Int64, AttributePositiveLow);
                                command.ParametersAdd("@AttributesPositiveHigh", DbType.Int64, AttributePositiveHigh);
                                command.ParametersAdd("@AttributesNegative", DbType.Int64, AttributesNegativeLow);
                                command.ParametersAdd("@AttributesNegativeHigh", DbType.Int64, AttributesNegativeHigh);

                                bool CorrectedCoordinates = false;
                                String notes = String.Empty;
                                // GSAK-Notizen übernehmen
                                //XmlElement extensions;
                                //if (((XmlElement)wps[0]).GetElementsByTagName("extensions").Count > 0)
                                //{
                                //XmlElement extElement = (XmlElement)((XmlElement)wps[0]).GetElementsByTagName("extensions")[0];

                                XmlElement extData = (XmlElement)((XmlElement)wps[0]).GetElementsByTagName("gsak:wptExtension")[0];
                                //XmlElement extData = ((XmlElement)wps[0]).GetElementsByTagName("gsak:wptExtension")[0];
                                if (extData != null)
                                {
                                    //XmlElement extData = (XmlElement)extElement.GetElementsByTagName("gsak:wptExtension")[0];

                                    if (getElement(extData, "gsak:LatBeforeCorrect").Length > 0)
                                        CorrectedCoordinates = true;


                                    if (getElement(extData, "gsak:UserData").Length > 0)
                                        notes = "UserNote 1: " + getElement(extData, "gsak:UserData") + "<br>";
                                    if (getElement(extData, "gsak:User2").Length > 0)
                                        notes += "UserNote 2: " + getElement(extData, "gsak:User2") + "<br>";
                                    if (getElement(extData, "gsak:User3").Length > 0)
                                        notes += "UserNote 3: " + getElement(extData, "gsak:User3") + "<br>";
                                    if (getElement(extData, "gsak:User4").Length > 0)
                                        notes += "UserNote 4S: " + getElement(extData, "gsak:User4");

                                    if (getElement(extData, "gsak:County").Length > 0)
                                        notes += "<b><font color=red>COUNTY: " + getElement(extData, "gsak:County") + "</font></b>";

                                    string customData = getElement(extData, "gsak:CustomData");
                                    if (customData.Length > 0)
                                    {
                                        Match match = Regex.Match(customData, @"Custom_Start:GEOPT_Premios_GPS:Custom_Data:(\d+):Custom_End|Custom_Start:GEOPT_BESTOF:Custom_Data:(\d+):Custom_End", RegexOptions.IgnoreCase);
                                        if (match.Success)
                                        {
                                            string year = "";
                                            year = match.Groups[1].Value;

                                            command.ParametersAdd("@Favorit", DbType.Boolean, true);

                                            if (year != "")
                                                notes += "<br><b><font color=red><a href=\"http://www.geopt.org\">www.geopt.org</a> - Prémios GPS " + year + "</font></b>";
                                            else
                                            {
                                                year = match.Groups[2].Value;

                                                if (year != "")
                                                    notes += "<br><b><font color=red><a href=\"http://www.geopt.org\">www.geopt.org</a> - GEOPT.ORG BEST OF " + year + "</font></b>";
                                            }
                                        }
                                        else
                                            command.ParametersAdd("@Favorit", DbType.Boolean, false);
                                    }
                                    else
                                    {
                                        command.ParametersAdd("@Favorit", DbType.Boolean, existsCache[GcCode].favorite);
                                    }
                                }
                                else
                                {
                                    command.ParametersAdd("@Favorit", DbType.Boolean, existsCache[GcCode].favorite);
                                }

                                //}
                                if (doNotUpdateKoords && (!CorrectedCoordinates))
                                {
                                    // if a Cache has already CorrectedCoordinates (from GSAK-Import), do not update Koords here 
                                    // when this import does not contain CorrectedCoords (eg. from PQ-Import)
                                    if (existsCache[GcCode].Latitude < 360)
                                        command.ChangeParamter("@latitude", existsCache[GcCode].Latitude);
                                    if (existsCache[GcCode].Longitude < 360)
                                        command.ChangeParamter("@longitude", existsCache[GcCode].Longitude);
                                    // CorrectedCoordinates must be true
                                    CorrectedCoordinates = true;
                                }

                                if (notes.Length > 0)
                                    html = notes + "<br><br>" + html;

                                command.ParametersAdd("@description", DbType.String, html);


                                //command.Parameters.Add("@notes", SqlDbType.NText).Value = notes;
                                command.ParametersAdd("@TourName", DbType.String, TourName.Trim());

                                long newGpxFilename_Id = gpxFilename.Id;
                                if (cacheExists)
                                {
                                    // Prüfen, ob die Categorie, der diese Cache angehört gepinnt ist!
                                    Category cacheCat = Global.Categories.GetCategoryByGpxFilenameId(cacheInfo.GpxFilename_Id);
                                    if ((cacheCat != null) && (cacheCat != category) && (cacheCat.pinned))
                                    {
                                        // dieser Cache darf nicht zur aktuellen Category hinzugefügt werden!
                                        // evtl. muss zur cacheCat ein neuer GpxFilename angelegt werden!
                                        GpxFilename newGpxFilename = null;
                                        if (NewGpxFilenames.ContainsKey(cacheInfo.GpxFilename_Id))
                                        {
                                            newGpxFilename = NewGpxFilenames[cacheInfo.GpxFilename_Id];
                                            newGpxFilename_Id = newGpxFilename.Id;
                                        }
                                        else
                                        {
                                            newGpxFilename = cacheCat.NewGpxFilename(cacheCat.GpxFilename);
                                            NewGpxFilenames.Add(cacheInfo.GpxFilename_Id, newGpxFilename);
                                            newGpxFilename_Id = newGpxFilename.Id;
                                        }
                                    }

                                }
                                command.ParametersAdd("@GPXFilename_ID", DbType.Int64, newGpxFilename_Id);


                                // Listing und Image Metadaten ermitteln und aktualisieren.

                                XmlNodeList allLogs = cacheElements.GetElementsByTagName("groundspeak:log");

                                string recentOwnerLogString = "";

                                if (allLogs.Count > 0)
                                {
                                    for (int i = 0; i < allLogs.Count; i++)
                                    {
                                        XmlElement logXml = (XmlElement)allLogs[i];
                                        XmlNode logNode = logXml.Attributes.GetNamedItem("id");
                                        if (logNode == null)
                                            continue;

                                        String Comment = getElement(logXml, "groundspeak:text", "text");
                                        String Finder = getElement(logXml, "groundspeak:finder", "geocacher").Trim();

                                        if (Finder.Equals(owner))
                                        {
                                            recentOwnerLogString += Comment;
                                            break;
                                        }
                                    }
                                }

                                int ListingCheckSum = (int)(Global.sdbm(stringForListingCheckSum) + Global.sdbm(recentOwnerLogString));

                                bool ListingChanged = existsCache[GcCode].ListingChanged;
                                bool ImagesUpdated = existsCache[GcCode].ImagesUpdated;
                                bool DescriptionImagesUpdated = existsCache[GcCode].DescriptionImagesUpdated;
                                if (GcCode == "GC1RC3D") 
                                {
                                    String s = "";
                                }

                                if (existsCache[GcCode].ListingCheckSum == 0)
                                {
                                    ImagesUpdated = false;
                                    DescriptionImagesUpdated = false;
                                }
                                else if (ListingCheckSum != existsCache[GcCode].ListingCheckSum)
                                {
                                    int oldStyleListingCheckSum = stringForListingCheckSum.GetHashCode() + recentOwnerLogString.GetHashCode();

                                    if (oldStyleListingCheckSum != existsCache[GcCode].ListingCheckSum)
                                    {
                                        ListingChanged = true;
                                        ImagesUpdated = false;
                                        DescriptionImagesUpdated = false;

                                        // 2014-06-22 - Ging-Buh - .changed Files are no longer used. Information about images to be downloaded is stored in DB in imagesUpdated and additionalImagesUpdated
//                                        CreateChangedListingFile(Config.GetString("DescriptionImageFolder") + "\\" + GcCode.Substring(0, 4) + "\\" + GcCode + ".changed");
//                                        CreateChangedListingFile(Config.GetString("SpoilerFolder") + "\\" + GcCode.Substring(0, 4) + "\\" + GcCode + ".changed");
                                    }
                                    else
                                    {
                                        // old Style Hash codes must also be converted to sdbm, so force update Description Images but without creating changed files.
                                        DescriptionImagesUpdated = false;
                                        ImagesUpdated = false;
                                    }
                                }

                                command.ParametersAdd("@ListingCheckSum", DbType.Int32, ListingCheckSum);
                                command.ParametersAdd("@ListingChanged", DbType.Boolean, ListingChanged);
                                command.ParametersAdd("@ImagesUpdated", DbType.Boolean, ImagesUpdated);
                                command.ParametersAdd("@DescriptionImagesUpdated", DbType.Boolean, DescriptionImagesUpdated);
                                command.ParametersAdd("@CorrectedCoordinates", DbType.Boolean, CorrectedCoordinates);

                                command.ExecuteNonQuery();
                                command.Dispose();


                                // Cache in Datenbank eintragen
                                if (!cacheExists)
                                    parent.ProgressChanged("adding " + name, progressCurrent, progressTotal);
                                else
                                    parent.ProgressChanged("updating " + name, progressCurrent, progressTotal);

                                // Logs lesen
                                if (allLogs.Count == 0)
                                    allLogs = cacheElements.GetElementsByTagName("log");

                                if (allLogs.Count > 0)
                                    for (int i = 0; i < allLogs.Count; i++)
                                    {
                                        XmlElement logXml = (XmlElement)allLogs[i];
                                        XmlNode logNode = logXml.Attributes.GetNamedItem("id");
                                        if (logNode == null)
                                            continue;

                                        Int64 logId;
                                        try
                                        {
                                            logId = Int64.Parse(logNode.InnerText);
                                        }
                                        catch (Exception)
                                        {
                                            // wenn die Log-ID keine Zahl ist dann diese in eine eindeutige Zahl wandeln
                                            char[] dummy2 = logNode.InnerText.ToCharArray();
                                            byte[] byteDummy2 = new byte[8];
                                            for (int i2 = 0; i2 < 8; i2++)
                                                if (i2 < logNode.InnerText.Length)
                                                    byteDummy2[i2] = (byte)dummy2[i2];
                                                else
                                                    byteDummy2[i2] = 0;

                                            logId = BitConverter.ToInt64(byteDummy2, 0);
                                        }
                                        String Comment = getElement(logXml, "groundspeak:text", "text");
                                        String Finder = getElement(logXml, "groundspeak:finder", "geocacher").Trim();

                                        //if (logId < 0 && Finder == "GSAK")
                                        //  continue;

                                        if (logId < 0 && Finder == "GSAK")
                                        {
                                            html += "<br>UserNotes: " + Comment;
                                            CBCommand noteCommand = Database.Data.CreateCommand("update Caches set Description=@description where Id=@id");
                                            noteCommand.ParametersAdd("@description", DbType.String, html);
                                            noteCommand.ParametersAdd("@id", DbType.Int64, id);
                                            noteCommand.ExecuteNonQuery();
                                            noteCommand.Dispose();
                                            continue;
                                        }

                                        //check if the log doesn't exist already in the DB
                                        if (cacheExists)
                                        {
                                            CBCommand checkLogCommand = Database.Data.CreateCommand("select count(Id) from Logs where Id=@id");
                                            checkLogCommand.ParametersAdd("@id", DbType.Int64, logId);
                                            object oresultLines = checkLogCommand.ExecuteScalar();
                                            int resultLines = Convert.ToInt32(oresultLines);
                                            checkLogCommand.Dispose();

                                            if (resultLines > 0)
                                                continue;
                                        }


                                        String time = getElementDefault(logXml, "2001-01-01", "groundspeak:date", "date", "time");
                                        DateTime Timestamp = DateTime.Now;
                                        try
                                        {
                                            Timestamp = DateTime.Parse(time, NumberFormatInfo.InvariantInfo);
                                        }
                                        catch (Exception)
                                        {
                                            if (time.StartsWith("1900"))
                                            {
                                                Timestamp = DateTime.Parse("2001-01-01", NumberFormatInfo.InvariantInfo);
                                            }
                                            else
                                            {
                                                parent.ReportUncriticalError(name + ": Malformed date format detected! " + time);
                                            }
                                        }

                                        short LogType = 0;
                                        String LogTypeText = getElement(logXml, "groundspeak:type", "type").ToLower();

                                        if (logLookup.ContainsKey(LogTypeText))
                                            LogType = logLookup[LogTypeText];
#if DEBUG
                                        else if (!LogTypeText.StartsWith("unknown logtype maxlog")) {
                                                Global.AddLog("\n\nGpxImport: Unknown Log Type: " + LogTypeText + "\n\n");
                                        };
#endif

                                        // insert
                                        CBCommand insertLogCommand = Database.Data.CreateCommand("insert into Logs(CacheId, Timestamp, Finder, Type, Comment, Id) values (@cacheid, @timestamp, @finder, @type, @comment, @id)");
                                        insertLogCommand.ParametersAdd("@cacheid", DbType.Int64, id);
                                        insertLogCommand.ParametersAdd("@id", DbType.Int64, logId);
                                        insertLogCommand.ParametersAdd("@timestamp", DbType.DateTime, Timestamp);
                                        insertLogCommand.ParametersAdd("@finder", DbType.String, Finder);
                                        insertLogCommand.ParametersAdd("@type", DbType.Int16, LogType);
                                        insertLogCommand.ParametersAdd("@comment", DbType.String, Comment);

                                        try
                                        {
                                            insertLogCommand.ExecuteNonQuery();
                                        }
                                        catch (Exception) { }
                                        finally
                                        {
                                            insertLogCommand.Dispose();
                                        }
                                    }
                            }
                            else
                            {
                                // Waypoint

                                GcCode = GcCode.Trim();

                                // Ist der Wegpunkt neu?
                                // Gibt es den dazugehörigen Cache schon?
                                String CacheGC = "GC" + GcCode.Substring(2);
                                String CacheOC = "OC" + GcCode.Substring(2);

                                if (element.GetElementsByTagName("gsak:Parent").Count > 0)
                                {
                                    string gsakParent = element.GetElementsByTagName("gsak:Parent")[0].InnerText;
                                    if (gsakParent != null && gsakParent.Length > 0)
                                        CacheGC = gsakParent.Trim();
                                }



                                if (existsCache.ContainsKey(CacheGC) || existsCache.ContainsKey(CacheOC))
                                {
                                    CBCommand command;
                                    bool waypointExists = existsWaypoint.ContainsKey(GcCode);
                                    if (waypointExists)
                                        // Veränderte WPs von der Synchronisation ausschliessen
                                        command = Database.Data.CreateCommand("update Waypoint set CacheId=@cacheid,Latitude=@latitude,Longitude=@longitude,Description=@description,Type=@type,Title=@title where GcCode=@gccode and SyncExclude=0");
                                    else
                                    {
                                        // Erzeuge neuen Eintrag
                                        existsWaypoint.Add(GcCode, null);
                                        command = Database.Data.CreateCommand("insert into Waypoint(GcCode,CacheId,Latitude,Longitude,Description,Type,SyncExclude,UserWaypoint,Title) values(@gccode,@cacheid,@latitude,@longitude,@description,@type,0,0,@title)");
                                    }

                                    command.ParametersAdd("@cacheid", DbType.Int64, (existsCache.ContainsKey(CacheGC)) ? existsCache[CacheGC].id : existsCache[CacheOC].id);
                                    command.ParametersAdd("@gccode", DbType.String, GcCode);
                                    command.ParametersAdd("@latitude", DbType.Double, Double.Parse(element.Attributes.GetNamedItem("lat").InnerText, NumberFormatInfo.InvariantInfo));
                                    command.ParametersAdd("@longitude", DbType.Double, Double.Parse(element.Attributes.GetNamedItem("lon").InnerText, NumberFormatInfo.InvariantInfo));
                                    XmlNodeList xnl = element.GetElementsByTagName("cmt");
                                    String desc = "";
                                    if (xnl.Count > 0)
                                    {
                                        desc = element.GetElementsByTagName("cmt")[0].InnerText;
                                    }
                                    //if (desc.Length > 4000)
                                    //    desc = desc.Substring(0, 4000);

                                    String Title = element.GetElementsByTagName("desc")[0].InnerText;
                                    command.ParametersAdd("@Title", DbType.String, Title);

                                    command.ParametersAdd("@description", DbType.String, desc);

                                    String type = element.GetElementsByTagName("type")[0].InnerText.ToLower();
                                    if (typeLookup.ContainsKey(type))
                                        command.ParametersAdd("@type", DbType.Int16, typeLookup[type]);
                                    else
                                    {
                                        command.ParametersAdd("@type", DbType.Int16, typeLookup["waypoint|question to answer"]);
                                        Global.AddLog("GpxImport: Unbekannter Waypoint Type: " + type);
                                    }

                                    command.ExecuteNonQuery();
                                    command.Dispose();

                                    if (waypointExists)
                                        parent.ProgressChanged("updating waypoint " + GcCode, progressCurrent, progressTotal);
                                    else
                                        parent.ProgressChanged("adding waypoint " + GcCode, progressCurrent, progressTotal);
                                }
                            }
                        }

                        // Speichertest durchführen
                        cachesSinceLastMemoryTest++;
                        if (cachesSinceLastMemoryTest == 10)
                        {
                            Application.DoEvents();
                            cachesSinceLastMemoryTest = 0;
                            parent.PerformMemoryTest(Global.databaseName, 1024);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to parse GPX. " + ex.InnerException.Message);
                }
                finally
                {
                    if (streamReader != null)
                        streamReader.Close();
                }

                // GPX-Datei ausgelesen und kann gelöscht werden
                if (!parent.Cancel)
                {
                    if (!Config.GetBool("DontDeleteGpx"))
                    {
                        System.IO.FileInfo info = new FileInfo(file);
                        info.Attributes = info.Attributes & ~FileAttributes.ReadOnly; // remove readonly
                        info.Delete();
                    }
                }
            }

            Database.Data.GPXFilenameUpdateCacheCount();
            //Database.Connection.Close(); No good idea to close the DB here. Do this only at program shutdown.

            // Dies ist ein guter Zeitpunkt dafür
            GC.Collect();
        }

        private static void CreateChangedListingFile(string changedFileString)
        {
            if (!File.Exists(changedFileString))
            {
                string changedFileDir = changedFileString.Substring(0, changedFileString.LastIndexOf("\\"));

                if (!Directory.Exists(changedFileDir))
                {
                    Directory.CreateDirectory(changedFileDir);
                }

                FileInfo changedFileInfo = new FileInfo(changedFileString);
                StreamWriter ChangedListingTextFile = changedFileInfo.CreateText();
                ChangedListingTextFile.WriteLine("Listing Changed!");
                ChangedListingTextFile.Close();
            }
        }

        internal String getAttributeDefault(XmlElement xml, String defaultValue, String attributeName)
        {
            String result = xml.GetAttribute(attributeName);
            if (result == "")
                return defaultValue;
            else
                return result;
        }

        internal String getElement(XmlElement xml, params String[] names)
        {
            for (int i = 0; i < names.Length; i++)
            {
                XmlNodeList result = xml.GetElementsByTagName(names[i]);
                if (result.Count > 0)
                    return result[0].InnerText;
            }
            return String.Empty;
        }

        internal String getElementDefault(XmlElement xml, String defaultValue, params String[] names)
        {
            String result = getElement(xml, names);
            if (result == "")
                return defaultValue;
            else
                return result;
        }

        /// <summary>
        /// Nimmt eine überschüssig gelesene Zeile auf
        /// </summary>
        String readerRestOfLine = String.Empty;
        StreamReader initializeReader(String file)
        {
            readerRestOfLine = String.Empty;

            StreamReader reader = new StreamReader(file);

            // bis zu erstem Waypoint vorspulen und gleichzeit schauen, ob die Datei von GCTour kommt -> teil des Dateinames = TourName
            while (true)
            {
                readerRestOfLine = reader.ReadLine();

                if (readerRestOfLine == null)
                {
                    reader.Close();
                    return null;
                }

                if (readerRestOfLine.Trim().ToLower().IndexOf("author>gctour</author>") > 0)
                {
                    int Start = file.IndexOf(".");
                    int End = file.IndexOf(".", file.IndexOf(".") + 1);
                    if (End > Start && Start > 1 && End > 1)
                        TourName = file.Substring(Start + 1, End - Start - 1);
                    else
                        TourName = String.Empty;
                }
                GPXFilename = System.IO.Path.GetFileName(file);

                int wptIdx = readerRestOfLine.Trim().ToLower().IndexOf("<wpt");

                if (wptIdx >= 0)
                {
                    readerRestOfLine = readerRestOfLine.Substring(wptIdx);
                    break;
                }
            }

            // left readerRestOfLine
            return reader;
        }

        /// <summary>
        /// Reads the next waypoint from Reader.
        /// </summary>
        /// <param name="reader">Reader for the waypoint.</param>
        /// <returns>XML-Code of the waypoint</returns>
        String readXmlWaypoint(StreamReader reader)
        {
            String resultXml = (readerRestOfLine != null) ? readerRestOfLine : "";
            readerRestOfLine = String.Empty;

            int wptIdx;
            int oldPos = 0;
            while ((wptIdx = resultXml.IndexOf("</wpt>", oldPos)) == -1)
            {
                oldPos = resultXml.Length;

                String line = null;
                if (reader != null)
                    line = reader.ReadLine();

                if (line == null)
                    return resultXml;

                resultXml += line + "\r\n";
            }

            readerRestOfLine = resultXml.Substring(wptIdx + 6);
            resultXml = resultXml.Substring(0, wptIdx + 6);

            // Ungültige Zeichen von GeoToad entfernen
            resultXml = RemoveTroublesomeCharacters(resultXml);

            return resultXml;
        }

        /// <summary>
        /// Removes control characters and other non-UTF-8 characters
        /// </summary>
        /// <param name="inString">The string to process</param>
        /// <returns>A string with no control characters or entities above 0x00FD</returns>
        public static string RemoveTroublesomeCharacters(string inString)
        {
            if (inString == null) return null;

            StringBuilder newString = new StringBuilder();
            char ch;

            for (int i = 0; i < inString.Length; i++)
            {
                ch = inString[i];
                // remove any characters outside the valid UTF-8 range as well as all control characters
                // except tabs and new lines
                if (!(ch < 0x0A))
                    newString.Append(ch);
            }
            return newString.ToString();
        }
    }
}
