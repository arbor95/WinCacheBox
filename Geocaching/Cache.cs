using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using WinCachebox.Types;

namespace WinCachebox.Geocaching
{
    public class Cache : IComparable
    {

        public class MysterySolution
        {
            public Cache Cache;
            public Waypoint Waypoint;
            public double Latitude;
            public double Longitude;
        }

        /// <summary>
        /// Koordinaten des Caches auf der Karte gelten in diesem Zoom
        /// </summary>
        public const int MapZoomLevel = 18;

        /// <summary>
        /// Koordinaten des Caches auf der Karte
        /// </summary>
        public double MapX;

        /// <summary>
        /// Koordinaten des Caches auf der Karte
        /// </summary>
        public double MapY;

        /// <summary>
        /// Id des Caches bei geocaching.com. Wird zumm Loggen benötigt und von
        /// geotoad nicht exportiert
        /// </summary>
        public String GcId;

        /// <summary>
        /// Id des Caches in der Datenbank von geocaching.com
        /// </summary>
        public long Id;

        /// <summary>
        /// Waypoint Code des Caches
        /// </summary>
        public String GcCode;

        /// <summary>
        /// Name des Caches
        /// </summary>
        public String Name;
        public String State;
        public String Country;

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
        /// Durchschnittliche Bewertung des Caches von GcVote
        /// </summary>
        public float Rating;

        /// <summary>
        /// Größe des Caches. Bei Wikipediaeinträgen enthält dieses Feld den Radius in m
        /// </summary>
        public int Size;

        /// <summary>
        /// Schwierigkeit des Caches
        /// </summary>
        public float Difficulty;

        /// <summary>
        /// Geländebewertung
        /// </summary>
        public float Terrain;

        /// <summary>
        /// Wurde der Cache archiviert?
        /// </summary>
        public bool Archived;

        /// <summary>
        /// Ist der Cache derzeit auffindbar?
        /// </summary>
        public bool Available;

        // Datum/Zeit, wann dieser Cache in CB aufgenommen wurde
        public DateTime FirstImported;

        /// Ist der Cache einer der Favoriten
        protected bool favorit;
        public bool Favorit
        {
            get
            {
                return favorit;
            }
            set
            {
                favorit = value;
                CBCommand command = Database.Data.CreateCommand("update Caches set Favorit=@favorit where Id=@id");
                command.ParametersAdd("@favorit", DbType.Boolean, value);
                command.ParametersAdd("@id", DbType.Int64, Id);
                command.ExecuteNonQuery();
                command.Dispose();
            }
        }


        /// hat der Cache Clues oder Notizen erfasst
        protected bool hasUserData;
        public bool HasUserData
        {
            get
            {
                return hasUserData;
            }
            set
            {
                hasUserData = value;
                CBCommand command = Database.Data.CreateCommand("update Caches set HasUserData=@hasUserData where Id=@id");
                command.ParametersAdd("@hasUserData", DbType.Boolean, value);
                command.ParametersAdd("@id", DbType.Int64, Id);
                command.ExecuteNonQuery();
                command.Dispose();
            }
        }


        public bool CorrectedCoordinates;

        /// <summary>
        ///  wenn ein Wegpunkt "Final" existiert, ist das mystery-Rätsel gelöst.
        /// </summary>
        public bool MysterySolved
        {
            get
            {
                if (this.CorrectedCoordinates)
                    return true;

                if (this.Type != CacheTypes.Mystery)
                    return false;

                bool x;
                x = false;

                List<Waypoint> wps = Waypoints;
                foreach (Waypoint wp in wps)
                {
                    if (wp.Type == CacheTypes.Final)
                    {
                        if (wp.Coordinate.IsValid())
                            x = true;
                    }
                };
                return x;
            }
        }

        public bool HasFinalWaypoint { get { return GetFinalWaypoint != null; } }
        public Waypoint GetFinalWaypoint
        {
            get
            {
                if (this.Type != CacheTypes.Mystery)
                    return null;

                List<Waypoint> wps = Waypoints;
                foreach (Waypoint wp in wps)
                {
                    if (wp.Type == CacheTypes.Final)
                    {
                        if (wp.Coordinate.IsValid())
                            return wp;
                    }
                };

                return null;
            }
        }

        protected bool found;
        /// <summary>
        /// Wurde der Cache bereits gefunden?
        /// </summary>
        /// 
        public bool Found
        {
            get
            {
                return found;
            }
            set
            {
                found = value;
                CBCommand command = Database.Data.CreateCommand("update Caches set Found=@found where Id=@id");
                command.ParametersAdd("@found", DbType.Boolean, value);
                command.ParametersAdd("@id", DbType.Int64, Id);
                command.ExecuteNonQuery();
                command.Dispose();
            }
        }

        public int Vote
        {
            get
            {
                CBCommand command = Database.Data.CreateCommand("select Vote from Caches where Id=@id");
                command.ParametersAdd("@id", DbType.Int64, Id);
                String resultString = command.ExecuteScalar().ToString();
                int result = 0;
                try {
                    result = int.Parse(resultString);
                } catch (Exception ex) {
                    result = 0;
                }
                command.Dispose();
                return result;
            }

            set
            {
                CBCommand command = Database.Data.CreateCommand("update Caches set Vote=@vote, VotePending=@votepending where Id=@id");
                command.ParametersAdd("@vote", DbType.Int16, (short)value);
                command.ParametersAdd("@votepending", DbType.Boolean, true);
                command.ParametersAdd("@id", DbType.Int64, Id);
                command.ExecuteNonQuery();
                command.Dispose();
            }
        }

        public string Note
        {
            get
            {
                /*
                        SqlCeCommand command = new SqlCeCommand("select Notes from Caches where Id=@id", Database.Data.Connection);
                        command.Parameters.Add("@id", DbType.Int64).Value = Id;
                 */
                CBCommand command = Database.Data.CreateCommand("select Notes from Caches where Id=@id");
                command.ParametersAdd("@id", DbType.Int64, Id);
                String resultString = command.ExecuteScalar().ToString();
                command.Dispose();
                return resultString;
            }
            set
            {
                CBCommand command = Database.Data.CreateCommand("update Caches set Notes=@Note, HasUserData=@true where Id=@id");
                command.ParametersAdd("@Note", DbType.String, value);
                command.ParametersAdd("@true", DbType.Boolean, true);
                command.ParametersAdd("@id", DbType.Int64, Id);
                command.ExecuteNonQuery();
                command.Dispose();
            }


        }

        public string Solver
        {
            get
            {
                CBCommand command = Database.Data.CreateCommand("select Solver from Caches where Id=@id");
                command.ParametersAdd("@id", DbType.Int64, Id);
                String resultString = command.ExecuteScalar().ToString();
                // replace linux NewLine by Windows NewLine for SolverView
                resultString = resultString.Replace(Environment.NewLine, "\n");
                resultString = resultString.Replace("\n", Environment.NewLine);
                command.Dispose();
                return resultString;
            }
            set
            {
                CBCommand command = Database.Data.CreateCommand("update Caches set Solver=@Solver, HasUserData=@true where Id=@id");
                command.ParametersAdd("@Solver", DbType.String, value);
                command.ParametersAdd("@true", DbType.Boolean, true);
                command.ParametersAdd("@id", DbType.Int64, Id);
                command.ExecuteNonQuery();
                command.Dispose();
            }


        }


        // Name der Tour, wenn die GPX-Datei aus GCTour importiert wurde
        public string TourName;

        // Name der GPX-Datei aus der importiert wurde
        public Int32 GPXFilename_ID;

        /// <summary>
        /// Art des Caches
        /// </summary>
        public CacheTypes Type;

        /// <summary>
        /// Erschaffer des Caches
        /// </summary>
        public String PlacedBy;

        /// <summary>
        /// Verantwortlicher
        /// </summary>
        public String Owner;

        /// <summary>
        /// Datum, an dem der Cache versteckt wurde
        /// </summary>
        public DateTime DateHidden;

        /// <summary>
        /// URL des Caches
        /// </summary>
        public String Url;

        public String shortDescription;

        public int FavPoints;


        public List<LogEntry> Logs
        {
            get
            {
                List<LogEntry> result = new List<LogEntry>();

                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                CBCommand command = Database.Data.CreateCommand("select CacheId, Timestamp, Finder, Type, Comment, Id from Logs where CacheId=@cacheid order by Timestamp desc");
                command.ParametersAdd("@cacheid", DbType.Int64, this.Id);
                DbDataReader reader = command.ExecuteReader();

                while (reader.Read())
                    result.Add(new LogEntry(reader, true));

                reader.Dispose();
                command.Dispose();

                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

                return result;
            }
        }

        /// <summary>
        /// Entfernung von der letzten gültigen Position
        /// </summary>
        public float Distance(bool useFinal)
        {
            Coordinate fromPos = (Global.Marker.Valid) ? Global.Marker : Global.LastValidPosition;
            Waypoint waypoint = null;
            if (useFinal)
                waypoint = this.GetFinalWaypoint;
            // Wenn ein Mystery-Cache einen Final-Waypoint hat, soll die Diszanzberechnung vom Final aus gemacht werden
            // If a mystery has a final waypoint, the distance will be calculated to the final not the the cache coordinates
            float tmpDistance;
            if (waypoint != null)
                tmpDistance = (float)Datum.WGS84.Distance(fromPos.Latitude, fromPos.Longitude, waypoint.Latitude, waypoint.Longitude);
            else
                tmpDistance = (float)Datum.WGS84.Distance(fromPos.Latitude, fromPos.Longitude, Latitude, Longitude);
            if (useFinal)
                cachedDistance = tmpDistance;
            return (float)tmpDistance;
        }

        /// <summary>
        /// Falls keine erneute Distanzberechnung nötig ist nehmen wir diese Distanz
        /// </summary>
        protected float cachedDistance = 0;
        public float CachedDistance
        {
            get
            {
                if (cachedDistance != 0)
                    return cachedDistance;
                else
                    return Distance(true);
            }
        }

        /// <summary>
        /// Anzahl der Travelbugs und Coins, die sich in diesem Cache befinden
        /// </summary>
        public int NumTravelbugs;

        public String Description
        {
            get
            {
                CBCommand command = Database.Data.CreateCommand("select Description from Caches where Id=@id");
                command.ParametersAdd("@id", DbType.Int64, this.Id);
                String result = command.ExecuteScalar().ToString();
                command.Dispose();
                return result;
            }
        }

        protected String hint = String.Empty;
        public String Hint
        {
            get
            {
                if (String.IsNullOrEmpty(hint))
                {
                    CBCommand command = Database.Data.CreateCommand("select Hint from Caches where Id=@id");
                    command.ParametersAdd("@id", DbType.Int64, this.Id);
                    hint = command.ExecuteScalar().ToString();
                    command.Dispose();
                }
                return hint;
            }
        }

        protected List<Waypoint> waypoints = null;
        public List<Waypoint> Waypoints
        {
            set
            {
                waypoints = value;
            }
            get
            {
                if (waypoints == null)
                {
                    waypoints = new List<Waypoint>();

                    CBCommand command = Database.Data.CreateCommand("select GcCode, CacheId, Latitude, Longitude, Description, Type, SyncExclude, UserWaypoint, Clue, Title from Waypoint where CacheId=@cacheid");
                    command.ParametersAdd("@cacheid", DbType.Int64, Id);
                    DbDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                        waypoints.Add(new Waypoint(reader));

                    reader.Dispose();
                    command.Dispose();
                }

                return waypoints;
            }
        }

        protected bool listingChanged;
        public bool ListingChanged
        {
            get
            {
                return listingChanged;
            }

            set
            {
                listingChanged = value;
                CBCommand command = Database.Data.CreateCommand("update Caches set ListingChanged=@ListingChanged where Id=@id");
                command.ParametersAdd("@ListingChanged", DbType.Boolean, value);
                command.ParametersAdd("@id", DbType.Int64, Id);
                command.ExecuteNonQuery();
                command.Dispose();

            }
        }





        protected DLong attributesPositive = null;

        public DLong AttributesPositive
        {
            get
            {
                if (attributesPositive == null && this.Id > 0)
                {
                    CBCommand command = Database.Data.CreateCommand("select AttributesPositive, AttributesPositiveHigh from Caches where Id=@id");
                    command.ParametersAdd("@id", DbType.Int64, this.Id);
                    DbDataReader reader = command.ExecuteReader();

                    ulong l = 0;
                    ulong h = 0;

                    while (reader.Read())
                    {
                        l = (ulong)reader.GetInt64(0);
                        h = (ulong)reader.GetInt64(1);
                    }

                    attributesPositive = new DLong(h, l);

                    command.Dispose();
                }
                return attributesPositive;
            }

            set
            {
                attributesPositive = value;
            }
        }

        protected DLong attributesNegative = null;

        public DLong AttributesNegative
        {
            get
            {
                if (attributesNegative == null && this.Id > 0)
                {
                    CBCommand command = Database.Data.CreateCommand("select AttributesNegative, AttributesNegativeHigh from Caches where Id=@id");
                    command.ParametersAdd("@id", DbType.Int64, this.Id);
                    DbDataReader reader = command.ExecuteReader();

                    ulong l = 0;
                    ulong h = 0;

                    while (reader.Read())
                    {
                        l = (ulong)reader.GetInt64(0);
                        h = (ulong)reader.GetInt64(1);
                    }

                    attributesNegative = new DLong(h, l);


                    command.Dispose();
                }
                return attributesNegative;
            }

            set
            {
                attributesNegative = value;
            }
        }




        public Cache(DbDataReader reader)
        {
            Id = long.Parse(reader[0].ToString());
            GcCode = reader[1].ToString().Trim();
            Latitude = double.Parse(reader[2].ToString());
            Longitude = double.Parse(reader[3].ToString());
            Name = reader[4].ToString().Trim();
            Size = int.Parse(reader[5].ToString());
            Difficulty = ((float)short.Parse(reader[6].ToString())) / 2;
            Terrain = ((float)short.Parse(reader[7].ToString())) / 2;
            Archived = bool.Parse(reader[8].ToString());
            Available = bool.Parse(reader[9].ToString());
            found = bool.Parse(reader[10].ToString());
            Type = (CacheTypes)(short.Parse(reader[11].ToString()));
            PlacedBy = reader[12].ToString().Trim();
            Owner = reader[13].ToString().Trim();

            //String datum = reader[14].ToString().Substring(0, 10);
            //DateHidden = DateTime.Parse(datum);
            DateHidden = (DateTime)reader[14];
            Url = reader[15].ToString().Trim();
            NumTravelbugs = int.Parse(reader[16].ToString());
            GcId = reader[17].ToString().Trim();
            Rating = (float)short.Parse(reader[18].ToString()) / 100.0f;
            if (reader[19].ToString() == "True")
                Favorit = true;
            else
                favorit = false;
            TourName = reader[20].ToString().Trim();

            if (reader[21].ToString() != "")
                GPXFilename_ID = Convert.ToInt32(reader[21].ToString());
            else
                GPXFilename_ID = -1;

            if (reader[22].ToString() == "True")
                hasUserData = true;
            else
                hasUserData = false;

            if (reader[23].ToString() == "True")
                listingChanged = true;
            else
                listingChanged = false;

            if (reader[24].ToString() == "True")
                CorrectedCoordinates = true;
            else
                CorrectedCoordinates = false;

            if (reader.IsDBNull(25))
                FirstImported = DateTime.Now;
            else
                FirstImported = reader.GetDateTime(25);

            if (reader.FieldCount >= 28)
            {
                if (!reader.IsDBNull(26))
                    State = reader[26].ToString().Trim();
                else
                    State = "";

                if (!reader.IsDBNull(27))
                    Country = reader[27].ToString().Trim();
                else
                    Country = "";
                FavPoints = int.Parse(reader[28].ToString());
                shortDescription = reader[29].ToString().Trim();

            }

            MapX = 256.0 * Map.Descriptor.LongitudeToTileX(MapZoomLevel, Longitude);
            MapY = 256.0 * Map.Descriptor.LatitudeToTileY(MapZoomLevel, Latitude);
        }

        public Cache()
        {
            // TODO: Complete member initialization
        }

        public override int GetHashCode()
        {
            return (int)Id;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType())
                return false;

            return ((Cache)obj).Id == this.Id;
        }

        /// <summary>
        /// Die Caches, auf die die letzte Suchanfrage passte
        /// </summary>
        public static List<Cache> Query = null;

        public static List<MysterySolution> MysterySolutions = null;

        public static void LoadCaches(String where)
        {
            LoadCaches(where, null);
        }

        public static void LoadCaches(String where, int distance)
        {
            LoadCaches(where, null, distance);
        }

        public static Cache GetCacheByCacheId(long cacheId)
        {
            foreach (Cache cache in Query)
            {
                if (cache.Id == cacheId)
                    return cache;
            }
            return null;
        }

        public static void LoadCaches(String where, List<Cache> caches)
        {
            LoadCaches(where, caches, Database.Data, false, 0);
        }

        public static void LoadCaches(String where, List<Cache> caches, int distance)
        {
            LoadCaches(where, caches, Database.Data, false, distance);
        }

        public static int LoadCachesCount(String where, int distance)
        {
            int tickStart = Environment.TickCount;

            Coordinate fromPos = (Global.Marker.Valid) ? Global.Marker : Global.LastValidPosition;

            string lat = fromPos.Latitude.ToString().Replace(",", ".");
            string lon = fromPos.Longitude.ToString().Replace(",", ".");
            string scommand = "select count(0) from Caches ";
            if (distance > 0)
            {
                if (where.Length > 0)
                    where += " AND ";
                where += " Acos(Cos(Latitude * PI() / 180) * Cos(Longitude * PI() / 180) * Cos(" + lat + " * PI() / 180) * Cos(" + lon + " * PI() / 180) + Cos(Latitude * PI() / 180) * Sin(Longitude * PI() / 180) * Cos(" + lat + " * PI() / 180) * Sin(" + lon + " * PI() / 180) + Sin(Latitude * PI() / 180) * Sin(" + lat + " * PI() / 180)) * 6362.7 <= " + distance;
            }
            
            CBCommand command = Database.Data.CreateCommand(scommand + ((where.Length > 0) ? "where " + where : where));
            object res = command.ExecuteScalar();

            command.Dispose();
            int tickEnd = Environment.TickCount;

            Global.CacheCount = Convert.ToInt32(res);

            return Convert.ToInt32(res);
        }


        public static void LoadCaches(String where, List<Cache> caches, Database database, bool onlyCacheBox, int distance)
        {
            int tickStart = Environment.TickCount;
            MysterySolutions = new List<MysterySolution>();
            // zuerst alle Waypoints einlesen
            SortedList<long, List<Waypoint>> waypoints = new SortedList<long, List<Waypoint>>();
            List<Waypoint> wpList = new List<Waypoint>();
            long aktCacheID = -1;

            CBCommand cbcommand = database.CreateCommand("select GcCode, CacheId, Latitude, Longitude, Description, Type, SyncExclude, UserWaypoint, Clue, Title from Waypoint order by CacheId");
            DbDataReader cbreader = cbcommand.ExecuteReader();

            while (cbreader.Read())
            {
                Waypoint wp = new Waypoint(cbreader);
                if (wp.CacheId != aktCacheID)
                {
                    aktCacheID = wp.CacheId;
                    wpList = new List<Waypoint>();
                    waypoints.Add(aktCacheID, wpList);
                }
                wpList.Add(wp);
            }

            cbreader.Dispose();
            cbcommand.Dispose();



            string ssubcommand = ", DateHidden";  // FirstImported is not available in onlyCacheBox Database. Use DateHidden instead so that the result is compatible
            if (!onlyCacheBox)
                ssubcommand = ", FirstImported, [State], Country, FavPoints, ShortDescription";
            string scommand = "select Id, GcCode, Latitude, Longitude, Name, Size, Difficulty, Terrain, Archived, Available, Found, Type, PlacedBy, Owner, DateHidden, Url, NumTravelbugs, GcId, Rating, Favorit, TourName, GpxFilename_ID, HasUserData, ListingChanged, CorrectedCoordinates" + ssubcommand + " from Caches ";


            CBCommand command = database.CreateCommand(scommand + ((where.Length > 0) ? "where " + where : where));
            DbDataReader reader = command.ExecuteReader();

            if (caches == null)
            {
                Query = new List<Cache>();
                Global.CacheCount = 0;
            }
            else
                caches.Clear();

            while (reader.Read())
            {
                Cache cache = new Cache(reader);
                if ((distance > 0 && cache.Distance(true) <= distance * 1000) || distance == 0)
                {
                    if (caches == null)
                    {
                        Query.Add(cache);
                        ++Global.CacheCount;
                    }
                    else
                        caches.Add(cache);


                    if (waypoints.ContainsKey(cache.Id))
                    {
                        cache.waypoints = waypoints[cache.Id];
                        waypoints.Remove(cache.Id);
                        if (cache.Type == CacheTypes.Multi || cache.Type == CacheTypes.Mystery || cache.Type == CacheTypes.Wherigo)
                        {
                            foreach (Waypoint wp in cache.waypoints)
                            {
                                if (wp.Type == CacheTypes.Final)
                                {
                                    MysterySolution solution = new MysterySolution
                                    {
                                        Cache = cache,
                                        Waypoint = wp,
                                        Latitude = wp.Latitude,
                                        Longitude = wp.Longitude
                                    };
                                    MysterySolutions.Add(solution);
                                }
                            }
                        }

                    }
                    else
                        cache.waypoints = new List<Waypoint>();
                }

            }

            reader.Dispose();
            command.Dispose();
            if (caches == null)
            {
                Query.Sort();
            }
            else
                caches.Sort();
            int tickEnd = Environment.TickCount;
        }

        public Waypoint GetWaypointByGcCode(string gcCode)
        {
            foreach (Waypoint wp in Waypoints)
                if (wp.GcCode == gcCode)
                    return wp;

            return null;
        }

        /// <summary>
        /// Zum Sortieren von Caches nach Distanz
        /// </summary>
        /// <param name="obj">Cache, mit dem die Distanz verglichen werden soll</param>
        /// <returns>-1, falls obj näher ist als die Instanz, 1 falls sie weiter entfernt ist und sonst 0.</returns>
        public int CompareTo(object obj)
        {
            System.Diagnostics.Debug.Assert(obj is Cache, "Falscher Typ: " + obj.ToString() + " ist kein Cache!");

            double d1 = (obj as Cache).CachedDistance;

            if (d1 < CachedDistance)
                return 1;

            if (d1 > CachedDistance)
                return -1;

            return 0;
        }

        List<String> spoilerRessources = null;
        public List<String> SpoilerRessources
        {
            get
            {
                if (spoilerRessources == null)
                {
                    ReloadSpoilerRessources();
                }

                return spoilerRessources;
            }

            set
            {
                spoilerRessources = value;
            }
        }

        public void ReloadSpoilerRessources()
        {
            spoilerRessources = new List<string>();

            string path = Config.GetString("SpoilerFolder");
            if (GcCode.Length < 4)
                return; // avoid Problem with GcCode with length < 4 chars
            string imagePath = path + "\\" + GcCode.Substring(0, 4);
            bool imagePathDirExists = Directory.Exists(imagePath);

            string[] oldFilesStructure = Directory.GetFiles(path, GcCode + "*");

            if (oldFilesStructure.Length != 0)
            {
                if (!imagePathDirExists)
                {
                    Directory.CreateDirectory(imagePath);
                }

                foreach (string oldFile in oldFilesStructure)
                {
                    string newFile = imagePath + "\\" + oldFile.Substring(oldFile.LastIndexOf("\\") + 1);

                    if (!File.Exists(newFile))
                    {
                        File.Move(oldFile, newFile);
                    }
                    else
                    {
                        File.Delete(oldFile);
                    }
                }
            }

            String directory = path + "\\" + GcCode.Substring(0, 4);

            if (!Directory.Exists(directory))
                return;

            String[] dummy = Directory.GetFiles(directory, GcCode.ToUpper() + "*.*");
            foreach (String image in dummy)
            {
                String imgFile = image.ToLower();
                if (imgFile.EndsWith(".jpg") || imgFile.EndsWith(".jpeg") || imgFile.EndsWith(".bmp") || imgFile.EndsWith(".png") || imgFile.EndsWith(".gif"))
                    spoilerRessources.Add(image);

            }

            // Add own taken photo
            directory = Config.GetString("UserImageFolder");

            if (!Directory.Exists(directory))
                return;

            String[] dummy1 = Directory.GetFiles(directory, "*" + GcCode.ToUpper() + "*.*");
            foreach (String photo in dummy1)
            {
                String imgFile = photo.ToLower();
                String TestString = Path.GetDirectoryName(photo) + "\\GPS_" + Path.GetFileName(photo);
                if (!File.Exists(TestString)) //only add if no GPS_.... file exists
                {
                    if (imgFile.EndsWith(".jpg") || imgFile.EndsWith(".jpeg") || imgFile.EndsWith(".bmp") || imgFile.EndsWith(".png") || imgFile.EndsWith(".gif"))
                        spoilerRessources.Add(photo);
                }

            }

        }

        public bool SpoilerExists
        {
            get
            {
                return SpoilerRessources.Count > 0;
            }
        }

        public void UpdateCacheStatus(bool Available, bool Archived)
        {
            if ((this.Available == Available) && (this.Archived == Archived))
                return;
            this.Available = Available;
            this.Archived = Archived;
            CBCommand command = Database.Data.CreateCommand("update Caches set Available=@available, Archived=@archived where Id=@id");
            command.ParametersAdd("@available", DbType.Boolean, Available);
            command.ParametersAdd("@archived", DbType.Boolean, Archived);
            command.ParametersAdd("@id", DbType.Int64, Id);
            command.ExecuteNonQuery();
            command.Dispose();
        }


        private List<Attributes> AttributeList = null;

        public List<Attributes> getAttributes()
        {
            if (AttributeList == null)
            {
                AttributeList = Attributes.getAttributes(
                    this.AttributesPositive, this.AttributesNegative);
            }

            return AttributeList;
        }

        public void addAttributeNegative(Attributes attribute)
        {
            if (attributesNegative == null)
                attributesNegative = new DLong(0, 0);
            attributesNegative.BitOr(Attributes.GetAttributeDlong(attribute.Attribute));
        }


        public void addAttributePositive(Attributes attribute)
        {
            if (attributesPositive == null)
                attributesPositive = new DLong(0, 0);
            attributesPositive.BitOr(Attributes.GetAttributeDlong(attribute.Attribute));
        }


    }
}