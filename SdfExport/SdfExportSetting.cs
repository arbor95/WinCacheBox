using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Data;
using System.Data.Common;
using System.Windows.Forms;
using WinCachebox.Geocaching;
using System.Drawing;

namespace WinCachebox.SdfExport
{
    public class SdfExportSetting
    {
        public long Id = -1;
        public FilterProperties Filter;
        public string ExportPath;
        public double MaxDistance;
        public Location Location;
        public string Description;
        public bool Update;
        public bool ExportImages;
        public bool ExportSpoilers;
        public bool ExportMaps;
        public bool ExportMapPacks;
        public bool OwnRepository;
        public bool SaveToDatabase = true;
        public List<Cache> Caches = new List<Cache>();
        public string MapPacks;
        public int MaxLogs;
        public int cacheCountAproximate;

        public SdfExportSetting()
        {
            Update = true;
            ExportImages = true;
            ExportSpoilers = true;
            ExportMaps = true;
            ExportMapPacks = true;
            OwnRepository = false;
            MapPacks = "";
            MaxLogs = 10;
            cacheCountAproximate = 0;
        }

        public SdfExportSetting(DbDataReader reader)
        {
            Id = long.Parse(reader[0].ToString());
            Description = reader[1].ToString().Trim();
            ExportPath = reader[2].ToString().Trim();
            MaxDistance = double.Parse(reader[3].ToString());
            long locationId = long.Parse(reader[4].ToString());
            Location = null;
            foreach (Location loc in Geocaching.Location.Locations)
            {
                if (loc.Id == locationId)
                {
                    Location = loc;
                    break;
                }
            }
            string filter = reader[5].ToString();
            Filter = new FilterProperties(filter);
            Update = bool.Parse(reader[6].ToString());
            ExportImages = bool.Parse(reader[7].ToString());
            ExportSpoilers = bool.Parse(reader[8].ToString());
            ExportMaps = bool.Parse(reader[9].ToString());
            if (!reader.IsDBNull(10))
                OwnRepository = bool.Parse(reader[10].ToString());
            else
                OwnRepository = false;
            if (!reader.IsDBNull(11))
                ExportMapPacks = bool.Parse(reader[11].ToString());
            else
                ExportMapPacks = false;

            if (!reader.IsDBNull(12))
                MapPacks = reader[12].ToString();
            else
                MapPacks = "";

            if (!reader.IsDBNull(13))
                MaxLogs = int.Parse(reader[13].ToString());
            else
                MaxLogs = 10;
        }

        public bool Edit()
        {
            SdfExportSettingsForm ses = new SdfExportSettingsForm(this);
            return (ses.ShowDialog() == System.Windows.Forms.DialogResult.OK);
        }

        public void Write()
        {
            // write this settings into Database
            if (Id >= 0)
            {
                // ID schon vorhanden -> Update
                CBCommand command = Database.Data.CreateCommand("update SdfExport set [Description]=@Description, [ExportPath]=@ExportPath, [MaxDistance]=@MaxDistance, [LocationID]=@LocationId, [Filter]=@Filter, [Update]=@Update, [ExportImages]=@ExportImages, [ExportSpoilers]=@ExportSpoilers, [ExportMaps]=@ExportMaps, [OwnRepository]=@OwnRepository, [ExportMapPacks]=@ExportMapPacks, [MapPacks]=@MapPacks, [MaxLogs]=@MaxLogs where Id=@Id");
                command.ParametersAdd("@Id", DbType.Int64, Id);
                command.ParametersAdd("@Description", DbType.String, Description);
                command.ParametersAdd("@ExportPath", DbType.String, ExportPath);
                command.ParametersAdd("@MaxDistance", DbType.Double, MaxDistance);
                long locationId = -1;
                if (Location != null)
                    locationId = Location.Id;
                command.ParametersAdd("@LocationID", DbType.Int64, locationId);
                command.ParametersAdd("@Filter", DbType.String, Filter.ToString());
                command.ParametersAdd("@Update", DbType.Boolean, Update);
                command.ParametersAdd("@ExportImages", DbType.Boolean, ExportImages);
                command.ParametersAdd("@ExportSpoilers", DbType.Boolean, ExportSpoilers);
                command.ParametersAdd("@ExportMaps", DbType.Boolean, ExportMaps);
                command.ParametersAdd("@OwnRepository", DbType.Boolean, OwnRepository);
                command.ParametersAdd("@ExportMapPacks", DbType.Boolean, ExportMapPacks);
                command.ParametersAdd("@MapPacks", DbType.String, MapPacks);
                command.ParametersAdd("@MaxLogs", DbType.Int32, MaxLogs);
                command.ExecuteNonQuery();
                command.Dispose();
            }
            else
            {
                // neuer Eintrag
                CBCommand command = Database.Data.CreateCommand("insert into SdfExport ([Description], [ExportPath], [MaxDistance], [LocationID], [Filter], [Update], [ExportImages], [ExportSpoilers], [ExportMaps], [OwnRepository], [ExportMapPacks], [MapPacks], [MaxLogs]) values (@Description, @ExportPath, @MaxDistance, @LocationID, @Filter, @Update, @ExportImages, @ExportSpoilers, @ExportMaps, @OwnRepository, @ExportMapPacks, @MapPacks, @MaxLogs)");
                command.ParametersAdd("@Description", DbType.String, Description);
                command.ParametersAdd("@ExportPath", DbType.String, ExportPath);
                command.ParametersAdd("@MaxDistance", DbType.Double, MaxDistance);
                long locationId = -1;
                if (Location != null)
                    locationId = Location.Id;
                command.ParametersAdd("@LocationID", DbType.Int64, locationId);
                command.ParametersAdd("@Filter", DbType.String, Filter.ToString());
                command.ParametersAdd("@Update", DbType.Boolean, Update);
                command.ParametersAdd("@ExportImages", DbType.Boolean, ExportImages);
                command.ParametersAdd("@ExportSpoilers", DbType.Boolean, ExportSpoilers);
                command.ParametersAdd("@ExportMaps", DbType.Boolean, ExportMaps);
                command.ParametersAdd("@OwnRepository", DbType.Boolean, OwnRepository);
                command.ParametersAdd("@ExportMapPacks", DbType.Boolean, ExportMapPacks);
                command.ParametersAdd("@MapPacks", DbType.String, MapPacks);
                command.ParametersAdd("@MaxLogs", DbType.Int32, MaxLogs);
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                    Global.AddLog(ex.Message);
                }
                command.Dispose();
                // Id auslesen
                command = Database.Data.CreateCommand("select max(Id) from SdfExport");
                Id = Convert.ToInt32(command.ExecuteScalar().ToString());
                command.Dispose();
            }
        }

        public void Delete()
        {
            CBCommand command = Database.Data.CreateCommand("delete from SdfExport where Id=@id");
            command.ParametersAdd("@Id", DbType.Int64, Id);
            command.ExecuteNonQuery();
            command.Dispose();
        }

        public int Export(IFormProgressReport parent, int startCount, int maxCount)
        {
            int aktCount = 0;
            if (parent.Cancel)
                return 0;

            if (!Directory.Exists(Path.GetDirectoryName(ExportPath)))
            {
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(ExportPath));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Global.Translations.Get("path") + " [" + Path.GetDirectoryName(ExportPath) + Global.Translations.Get(Global.Translations.Get("f31", "] can not be created!")), Global.Translations.Get(Global.Translations.Get("Error")));
                    Global.AddLog("SDF-Export: Path can not be created - " + ex.Message);
                    return 0;
                }
            }
            try
            {
                parent.ProgressChanged(Global.Translations.Get("openDB", "_Opening Database..."), startCount, maxCount);
                Database export = new Database();
                bool update = Update;
                // wenn Update gew‰hlt ist, Datenbank aber nicht existiert -> Neue anlegen
                if (update && (!File.Exists(ExportPath)))
                    update = false;
                if (update)
                {
                    export.Startup(ExportPath, false);
                }
                else
                {
                    if (File.Exists(ExportPath))
                    {
                        // Habe doch vorher schon "neu" machen angew‰hlt --> Frage h‰lt nur auf
                        /*
                        if (MessageBox.Show(Global.Translations.Get("overwriteDB","_Overwrite the existing Database") + " [" + ExportPath + "]?", Global.Translations.Get("Warning"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                            return 0;
                        */
                        File.Delete(ExportPath);
                    }
                    export.Startup(ExportPath, false);
                }

                // for Replication
                // write DatabaseId of this MasterDatabase into the destination SDF as MasterDatabaseId
                export.WriteConfigLong("MasterDatabaseId", Database.Data.DatabaseId);

                // GpxFilenames aus Source auslesen
                CBCommand query = Database.Data.CreateCommand("select ID, GPXFilename, Imported, Cachecount, CategoryId from GPXFilenames");
                DbDataReader reader = query.ExecuteReader();
                // Categories aus Source auslesen
                Categories sourceCategories = new Categories();


                List<Map.Pack> mapPackList = new List<Map.Pack>();

                DbTransaction transaction = export.StartTransaction();
                try
                {
                    while (reader.Read())
                    {
                        // GpxFilenamen aus Source auslesen
                        long id = reader.GetInt64(0);
                        string gpxFilename = reader.GetString(1);
                        DateTime imported = reader.GetDateTime(2);
                        // Int64 cacheCount = reader.GetInt32(3);
                        // Int64 categoryId = reader.GetInt64(4);
                        Category sourceCategory = sourceCategories.GetCategoryByGpxFilenameId(id);
                        if (sourceCategory == null) continue;

                        long newGpxId = 0;
                        // alle Caches von diesem GpxFilename exportieren
                        bool first = true;

                        //TimeSpan tsCache = new TimeSpan();
                        //TimeSpan tsWaypoint = new TimeSpan();
                        //TimeSpan tsLogs = new TimeSpan();
                        //TimeSpan tsImages = new TimeSpan();
                        //TimeSpan tsSpoilers = new TimeSpan();

                        //DateTime dtInit = DateTime.Now;
                        foreach (Cache cache in Caches)
                        {
                            if (parent.Cancel)
                                break;
                            // immer nur die Caches des aktuellen GpxFilenamen id
                            if (cache.GPXFilename_ID != id)
                                continue;

                            aktCount++;
                            //TimeSpan tsTotal = (DateTime.Now - dtInit);
                            //double missingSecs = (double)maxCount / (double)(startCount + aktCount) * (double)tsTotal.TotalSeconds - (double)tsTotal.TotalSeconds;
                            //TimeSpan missing = new TimeSpan(0, 0, (int)missingSecs);

                            parent.ProgressChanged(Global.Translations.Get("export"), startCount + aktCount, maxCount);
                            //+ tsCache.TotalSeconds + " wps: " + tsWaypoint.TotalSeconds + "\r\nlogs: " + tsLogs.TotalSeconds + " imgs: " + tsImages.TotalSeconds + "\r\nsplr: " + tsSpoilers.TotalSeconds, startCount + aktCount, maxCount);

                            //DateTime dtStart = DateTime.Now;

                            // GpxFilenamen in der Dest DB anlegen oder dessen ID auslesen (falls schon vorhanden)
                            // der GpxFilename in der Dest DB wird anhand dem Namen und des Datums gesucht.
                            if (first)
                            {
                                newGpxId = -1;
                                if (update)
                                {
                                    // suchen, ob der GpxFilename schon in der DB ist.
                                    CBCommand searchCommand = export.CreateCommand("select max(ID) from GPXFilenames where (GPXFilename=@GPXFilename and Imported=@Imported)");
                                    searchCommand.ParametersAdd("@GPXFilename", DbType.String, gpxFilename);
                                    searchCommand.ParametersAdd("@Imported", DbType.DateTime, imported);
                                    try
                                    {
                                        newGpxId = Convert.ToInt32(searchCommand.ExecuteScalar().ToString());
                                        // keine Exception -> GpxFilename gefunden!
                                    }
                                    catch (Exception)
                                    {
                                        // GpxFilename nicht gefunden
                                        newGpxId = -1;
                                    }
                                    searchCommand.Dispose();
                                }
                                if (newGpxId < 0)
                                {
                                    // Pr¸fen, ob die Category schon drin ist
                                    long newCatId = -1;
                                    CBCommand searchCommand = export.CreateCommand("select max(ID) from Category where (GPXFilename=@GPXFilename)");
                                    searchCommand.ParametersAdd("@GPXFilename", DbType.String, sourceCategory.GpxFilename);
                                    try
                                    {
                                        newCatId = Convert.ToInt32(searchCommand.ExecuteScalar().ToString());
                                        // keine Exception -> GpxFilename gefunden!
                                    }
                                    catch (Exception exc)
                                    {
                                        exc.GetType(); //Warning vermeiden _ avoid warning
                                        // Cache noch nicht vorhanden
                                        newCatId = -1;
                                    }

                                    // Category muﬂ auch noch geschrieben werden
                                    if (newCatId < 0)
                                    {
                                        CBCommand Catcommand;
                                        Catcommand = export.CreateCommand("insert into Category(GPXFilename, Pinned) values (@GPXFilename, @Pinned)");
                                        Catcommand.ParametersAdd("@GPXFilename", DbType.String, sourceCategory.GpxFilename);
                                        Catcommand.ParametersAdd("@Pinned", DbType.Boolean, sourceCategory.pinned);
                                        Catcommand.ExecuteNonQuery();
                                        Catcommand.Dispose();
                                        Catcommand = export.CreateCommand("Select max(ID) from Category");
                                        newCatId = Convert.ToInt32(Catcommand.ExecuteScalar().ToString());
                                        Catcommand.Dispose();
                                    }

                                    // neuen Eintrag schreiben
                                    CBCommand selectCommand = export.CreateCommand("insert into GPXFilenames(GPXFilename, Imported, CategoryId) values (@GPXFilename, @Imported, @CategoryId)");
                                    selectCommand.ParametersAdd("@GPXFilename", DbType.String, gpxFilename);
                                    selectCommand.ParametersAdd("@Imported", DbType.DateTime, imported);
                                    selectCommand.ParametersAdd("@CategoryId", DbType.Int64, newCatId);
                                    selectCommand.ExecuteNonQuery();
                                    selectCommand.Dispose();
                                    // neue GpxFilenameID auslesen
                                    selectCommand = export.CreateCommand("Select max(ID) from GPXFilenames");
                                    newGpxId = Convert.ToInt32(selectCommand.ExecuteScalar().ToString());
                                    selectCommand.Dispose();


                                }
                                first = false;
                            }

                            long newCacheId = -1;
                            CBCommand command = null;
                            if (update)
                            {
                                // suche, ob dieser Cache schon in der Export-DB vorhanden war (anhand Id)
                                CBCommand search = export.CreateCommand("select max(Id) from Caches where Id=@Id");
                                search.ParametersAdd("@Id", DbType.Int64, cache.Id);
                                try
                                {
                                    newCacheId = Convert.ToInt64(search.ExecuteScalar().ToString());
                                }
                                catch (Exception exc)
                                {
                                    exc.GetType(); //Warning vermeiden _ avoid warning
                                    // Cache noch nicht vorhanden
                                    newCacheId = -1;
                                }
                                search.Dispose();
                                if (newCacheId >= 0)
                                {
                                    command = export.CreateCommand("update Caches set GcCode=@gccode, Latitude=@latitude, Longitude=@longitude, Name=@name, Size=@size, Difficulty=@difficulty, Terrain=@terrain, Archived=@archived, Available=@available, Found=@found, Type=@type, PlacedBy=@placedby, Owner=@owner, DateHidden=@datehidden, Hint=@hint, Description=@description, Url=@url, NumTravelbugs=@numtravelbugs, GcId=@gcid,AttributesPositiveHigh=@AttributesPositiveHigh, AttributesPositive=@AttributesPositive, AttributesNegativeHigh=@AttributesNegativeHigh, AttributesNegative=@AttributesNegative, TourName=@TourName, GPXFilename_ID=@GPXFilename_ID, ListingCheckSum=@ListingCheckSum, ListingChanged=@ListingChanged, ImagesUpdated=@ImagesUpdated, DescriptionImagesUpdated=@DescriptionImagesUpdated, CorrectedCoordinates=@CorrectedCoordinates, Notes=@Notes, Solver=@Solver, Favorit=@Favorit, HasUserData=@HasUserData, FirstImported=@FirstImported, State=@State, Country=@Country, FavPoints=@FavPoints, ShortDescription=@ShortDescription where Id=@id");
                                }
                            }

                            if (command == null)
                            {
                                // kein Update dieses Caches mˆglich -> neu anlegen
                                command = export.CreateCommand("insert into Caches(Id, GcCode, Latitude, Longitude, Name, Size, Difficulty, Terrain, Archived, Available, Found, Type, PlacedBy, Owner, DateHidden, Hint, Description, Url, NumTravelbugs, GcId, Rating, Vote, VotePending, Notes, Solver, Favorit, AttributesPositive,AttributesPositiveHigh, AttributesNegative,AttributesNegativeHigh, TourName, GPXFilename_ID, HasUserData, ListingCheckSum, ListingChanged, ImagesUpdated, DescriptionImagesUpdated, CorrectedCoordinates, FirstImported, State, Country, FavPoints, ShortDescription) values(@id, @gccode, @latitude, @longitude, @name, @size, @difficulty, @terrain, @archived, @available, @found, @type, @placedby, @owner, @datehidden, @hint, @description, @url, @numtravelbugs, @gcid, @rating, @vote, @votepending, @notes, @solver, @Favorit, @AttributesPositive, @AttributesPositiveHigh, @AttributesNegative, @AttributesNegativeHigh, @TourName, @GPXFilename_ID, @HasUserData, @ListingCheckSum, @ListingChanged, @ImagesUpdated, @DescriptionImagesUpdated, @CorrectedCoordinates, @FirstImported, @State, @Country, @FavPoints, @ShortDescription)");
                            }

                            command.ParametersAdd("@id", DbType.Int64, cache.Id);
                            // GcCode, 
                            command.ParametersAdd("@gccode", DbType.String, cache.GcCode);
                            // Latitude, 
                            command.ParametersAdd("@latitude", DbType.Double, cache.Latitude);
                            // Longitude, 
                            command.ParametersAdd("@longitude", DbType.Double, cache.Longitude);
                            // Name, 
                            command.ParametersAdd("@name", DbType.String, cache.Name);
                            // GcId, 
                            command.ParametersAdd("@gcid", DbType.String, cache.GcId);
                            // Size, 
                            command.ParametersAdd("@size", DbType.Int64, cache.Size);
                            // Difficulty, 
                            command.ParametersAdd("@difficulty", DbType.Int16, cache.Difficulty * 2);
                            // Terrain, 
                            command.ParametersAdd("@terrain", DbType.Int16, cache.Terrain * 2);
                            // Archived, 
                            command.ParametersAdd("@archived", DbType.Boolean, cache.Archived);
                            // Available, 
                            command.ParametersAdd("@available", DbType.Boolean, cache.Available);
                            // Found, 
                            command.ParametersAdd("@found", DbType.Boolean, cache.Found);
                            // Type, 
                            command.ParametersAdd("@type", DbType.Int16, cache.Type);
                            // PlacedBy, 
                            command.ParametersAdd("@placedby", DbType.String, cache.PlacedBy);
                            // Owner, 
                            command.ParametersAdd("@owner", DbType.String, cache.Owner);
                            // DateHidden, 
                            command.ParametersAdd("@datehidden", DbType.DateTime, cache.DateHidden);
                            // Hint, 
                            command.ParametersAdd("@hint", DbType.String, cache.Hint);
                            // SchortDescription, 
                            command.ParametersAdd("@ShortDescription", DbType.String, cache.shortDescription);
                            // Description, 
                            command.ParametersAdd("@description", DbType.String, cache.Description);
                            // Url, 
                            command.ParametersAdd("@url", DbType.String, cache.Url);
                            // FavPoints, 
                            command.ParametersAdd("@FavPoints", DbType.Int16, cache.FavPoints);
                            // NumTravelbugs, 
                            command.ParametersAdd("@numtravelbugs", DbType.Int16, cache.NumTravelbugs);
                            // Rating, 
                            command.ParametersAdd("@rating", DbType.Single, cache.Rating * 100f);
                            // Vote, 
                            command.ParametersAdd("@vote", DbType.Int16, cache.Vote);
                            // VotePending, 
                            command.ParametersAdd("@votepending", DbType.Boolean, false);
                            // Notes, 
                            command.ParametersAdd("@notes", DbType.String, cache.Note);
                            // Solver, 
                            command.ParametersAdd("@solver", DbType.String, cache.Solver);
                            // Favorit, 
                            command.ParametersAdd("@Favorit", DbType.Boolean, cache.Favorit);
                            // AttributesPositive, 
                            command.ParametersAdd("@AttributesPositive", DbType.Int64, cache.AttributesPositive.getLow());
                            command.ParametersAdd("@AttributesPositiveHigh", DbType.Int64, cache.AttributesPositive.getHigh());
                            // AttributesNegative, 
                            command.ParametersAdd("@AttributesNegative", DbType.Int64, cache.AttributesNegative.getLow());
                            command.ParametersAdd("@AttributesNegativeHigh", DbType.Int64, cache.AttributesNegative.getHigh());
                            // TourName, 
                            command.ParametersAdd("@TourName", DbType.String, cache.TourName);
                            // GPXFilename_ID, 
                            command.ParametersAdd("@GPXFilename_ID", DbType.Int64, newGpxId);
                            // HasUserData, 
                            command.ParametersAdd("@HasUserData", DbType.Boolean, cache.HasUserData);
                            // ListingCheckSum, 
                            command.ParametersAdd("@ListingCheckSum", DbType.Int32, 0);
                            // ListingChanged, 
                            command.ParametersAdd("@ListingChanged", DbType.Boolean, cache.ListingChanged);
                            // ImagesUpdated, 
                            command.ParametersAdd("@ImagesUpdated", DbType.Boolean, true);
                            // DescriptionImagesUpdated
                            command.ParametersAdd("@DescriptionImagesUpdated", DbType.Boolean, true);
                            // CorrectedCoordinates
                            command.ParametersAdd("@CorrectedCoordinates", DbType.Boolean, cache.CorrectedCoordinates);
                            // FirstImported
                            command.ParametersAdd("@FirstImported", DbType.DateTime, cache.FirstImported);
                            // State
                            command.ParametersAdd("@State", DbType.String, cache.State);
                            // Country
                            command.ParametersAdd("@Country", DbType.String, cache.Country);


                            command.ExecuteNonQuery();
                            command.Dispose();

                            // alle vorhandenen Waypoints lˆschen
                            if (update)
                            {
                                CBCommand delCommand = export.CreateCommand("delete from [Waypoint] where CacheId=@cacheId");
                                delCommand.ParametersAdd("@cacheId", DbType.Int64, cache.Id);
                                delCommand.ExecuteNonQuery();
                                delCommand.Dispose();
                            }

                            //tsCache = tsCache + (DateTime.Now - dtStart);
                            //dtStart = DateTime.Now;

                            // Waypoints schreiben
                            foreach (Waypoint waypoint in cache.Waypoints)
                            {
                                command = null;
                                if (update)
                                {
                                    CBCommand search = export.CreateCommand("select max(GcCode) from Waypoint where GcCode=@GcCode");
                                    search.ParametersAdd("@GcCode", DbType.String, waypoint.GcCode);
                                    string wpid = "";
                                    try
                                    {
                                        wpid = search.ExecuteScalar().ToString();
                                        // keine Exception -> Waypoint gefunden!
                                        command = export.CreateCommand("update Waypoint set CacheId=@cacheid,Latitude=@latitude,Longitude=@longitude,Description=@description,Type=@type,Title=@title where GcCode=@gccode and SyncExclude=0");
                                    }
                                    catch (Exception exc)
                                    {
                                        exc.GetType(); //Warning vermeiden _ avoid warning
                                        // Waypoint nicht gefunden
                                        wpid = "";
                                        command = null;
                                    }
                                    search.Dispose();
                                    if (wpid == "")
                                        command = null;
                                }

                                if (command == null)
                                    // neuen WP anlegen
                                    command = export.CreateCommand("insert into Waypoint(GcCode,CacheId,Latitude,Longitude,Description,Type,SyncExclude,UserWaypoint,Title) values(@gccode,@cacheid,@latitude,@longitude,@description,@type,0,@UserWaypoint,@title)");
                                command.ParametersAdd("@cacheid", DbType.Int64, cache.Id);
                                command.ParametersAdd("@gccode", DbType.String, waypoint.GcCode);
                                command.ParametersAdd("@latitude", DbType.Double, waypoint.Latitude);
                                command.ParametersAdd("@longitude", DbType.Double, waypoint.Longitude);
                                command.ParametersAdd("@Title", DbType.String, waypoint.Title);
                                command.ParametersAdd("@description", DbType.String, waypoint.Description);
                                command.ParametersAdd("@type", DbType.Int16, waypoint.Type);
                                command.ParametersAdd("@UserWaypoint", DbType.Boolean, waypoint.IsUserWaypoint);
                                command.ExecuteNonQuery();
                                command.Dispose();
                            }

                            //tsWaypoint = tsWaypoint + (DateTime.Now - dtStart);
                            //dtStart = DateTime.Now;

                            // Logs schreiben
                            int logsCount = 0;
                            foreach (LogEntry log in cache.Logs)
                            {
                                // limit the number of Logs to export if MaxLogs > 0
                                if ((MaxLogs > 0) && (logsCount >= MaxLogs))
                                    break;
                                logsCount++;
                                bool insert = true;
                                if (update)
                                {
                                    // Pr¸fen, ob dieser Log schon vorhanden ist
                                    CBCommand searchLog = export.CreateCommand("select max(Id) from Logs where (Timestamp=@Timestamp and Finder=@Finder)");
                                    searchLog.ParametersAdd("@Timestamp", DbType.DateTime, log.Timestamp);
                                    searchLog.ParametersAdd("@Finder", DbType.String, log.Finder);
                                    try
                                    {
                                        string sss = searchLog.ExecuteScalar().ToString();
                                        if (sss.Length > 0)
                                        {
                                            insert = false;
                                        }
                                    }
                                    catch (Exception exc)
                                    {
                                        exc.GetType(); //Warning vermeiden _ avoid warning
                                        insert = true;
                                    }
                                }
                                if (insert)
                                {
                                    command = export.CreateCommand("insert into Logs(CacheId, Timestamp, Finder, Type, Comment, id) values (@cacheid, @timestamp, @finder, @type, @comment, @id)");
                                    command.ParametersAdd("@cacheid", DbType.Int64, cache.Id);
                                    command.ParametersAdd("@id", DbType.Int64, log.Id);
                                    command.ParametersAdd("@timestamp", DbType.DateTime, log.Timestamp);
                                    command.ParametersAdd("@finder", DbType.String, log.Finder);
                                    command.ParametersAdd("@type", DbType.Int16, log.TypeIcon);
                                    command.ParametersAdd("@comment", DbType.String, log.Comment);
                                    command.ExecuteNonQuery();
                                    command.Dispose();
                                }
                            }

                            //tsLogs = tsLogs + (DateTime.Now - dtStart);
                            //dtStart = DateTime.Now;

                            if (OwnRepository)
                            {
                                export.WriteConfigString("TileCacheFolder", "?\\" + Path.GetFileNameWithoutExtension(ExportPath) + "\\Cache");
                                export.WriteConfigString("SpoilerFolder", "?\\" + Path.GetFileNameWithoutExtension(ExportPath) + "\\Spoilers");
                                export.WriteConfigString("DescriptionimageFolder", "?\\" + Path.GetFileNameWithoutExtension(ExportPath) + "\\Images");
                                export.WriteConfigString("MapPackFolder", "?\\" + Path.GetFileNameWithoutExtension(ExportPath) + "\\Maps");
                            }

                            #region ExportImages
                            if (ExportImages)
                            {
                                // Images kopieren
                                string imagePath = Config.GetString("DescriptionImageFolder") + "\\" + cache.GcCode.Substring(0, 4);
                                string imagePathDest = Path.GetDirectoryName(ExportPath) + "\\Repository\\Images\\" + cache.GcCode.Substring(0, 4);
                                if (OwnRepository)
                                    imagePathDest = Path.GetDirectoryName(ExportPath) + "\\Repositories\\" + Path.GetFileNameWithoutExtension(ExportPath) + "\\Images\\" + cache.GcCode.Substring(0, 4);
                                if (!Directory.Exists(imagePath))
                                    Directory.CreateDirectory(imagePath);
                                string pattern = cache.GcCode.Substring(2, cache.GcCode.Length - 2).ToUpper();
                                string[] files = Directory.GetFiles(imagePath, "??" + pattern + "*");
                                foreach (string file in files)
                                {
                                    string fn = Path.GetFileNameWithoutExtension(file);
                                    if (fn.Length < pattern.Length + 2)
                                        continue;   // Dateiname zu kurz, Bild kann nicht zu diesem Cache gehˆren!
                                    if (fn.Substring(2, pattern.Length).ToUpper() == pattern)
                                    {
                                        // Datei kopieren, falls noch nicht vorhanden
                                        string destfilename = imagePathDest + "\\" + Path.GetFileName(file);
                                        if (!File.Exists(destfilename))
                                        {
                                            if (!Directory.Exists(Path.GetDirectoryName(destfilename)))
                                                Directory.CreateDirectory(Path.GetDirectoryName(destfilename));

                                            if (!file.EndsWith(".1st") && !file.EndsWith(".changed") && !file.EndsWith(".txt") && !file.EndsWith(".gif"))
                                                try
                                                {
                                                    ResizeImage(file, destfilename, Config.GetInt("ExportImagesMaxPixels"), Config.GetInt("ExportImagesMaxPixels"), true, false);
                                                }
                                                catch (Exception)
                                                { }
                                            else
                                                File.Copy(file, destfilename);
                                        }
                                    }
                                }
                            }
                            #endregion

                            //tsImages = tsImages + (DateTime.Now - dtStart);
                            //dtStart = DateTime.Now;

                            #region ExportSpoilers
                            if (ExportSpoilers)
                            {
                                // Spoiler kopieren
                                string imagePath = Config.GetString("SpoilerFolder") + "\\" + cache.GcCode.Substring(0, 4);
                                string imagePathDest = Path.GetDirectoryName(ExportPath) + "\\Repository\\Spoilers\\" + cache.GcCode.Substring(0, 4);
                                if (OwnRepository)
                                    imagePathDest = Path.GetDirectoryName(ExportPath) + "\\Repositories\\" + Path.GetFileNameWithoutExtension(ExportPath) + "\\Spoilers\\" + cache.GcCode.Substring(0, 4);
                                if (!Directory.Exists(imagePath))
                                    Directory.CreateDirectory(imagePath);
                                string pattern = cache.GcCode.Substring(2, cache.GcCode.Length - 2).ToUpper();
                                string[] files = Directory.GetFiles(imagePath, "??" + pattern + "*");
                                foreach (string file in files)
                                {
                                    string fn = Path.GetFileNameWithoutExtension(file);
                                    if (fn.Length < pattern.Length + 2)
                                        continue;   // Dateiname zu kurz, Bild kann nicht zu diesem Cache gehˆren!
                                    if (fn.Substring(2, pattern.Length).ToUpper() == pattern)
                                    {
                                        // Datei kopieren, falls noch nicht vorhanden
                                        string destfilename = imagePathDest + "\\" + Path.GetFileName(file);
                                        if (!File.Exists(destfilename))
                                        {
                                            if (!Directory.Exists(Path.GetDirectoryName(destfilename)))
                                                Directory.CreateDirectory(Path.GetDirectoryName(destfilename));

                                            if (!file.EndsWith(".1st") && !file.EndsWith(".changed") && !file.EndsWith(".txt"))
                                                try
                                                {
                                                    ResizeImage(file, destfilename, Config.GetInt("ExportSpoilersMaxPixels"), Config.GetInt("ExportSpoilersMaxPixels"), true, Config.GetBool("ExportSpoilersRotate"));
                                                }
                                                catch (Exception)
                                                { }
                                            else
                                                File.Copy(file, destfilename);
                                        }
                                    }
                                }
                            }
                            #endregion

                            //tsSpoilers = tsSpoilers + (DateTime.Now - dtStart);
                            //dtStart = DateTime.Now;

                            #region ExportMaps
                            if (ExportMaps)
                            {
                                //MAP CACHE
                                List<Map.Layer> layersToExport = null;
                                layersToExport = new List<Map.Layer>();

                                if (Config.GetBool("ImportLayerOsm"))
                                    layersToExport.Add(WinCachebox.Views.MapView.Manager.GetLayerByName("Mapnik", "Mapnik", ""));

                                if (Config.GetBool("ImportLayerOTM"))
                                    layersToExport.Add(WinCachebox.Views.MapView.Manager.GetLayerByName("OpenTopoMap", "OpenTopoMap", ""));

                                if (Config.GetBool("ImportLayerOcm"))
                                    layersToExport.Add(WinCachebox.Views.MapView.Manager.GetLayerByName("OSM Cycle Map", "Open Cycle Map", ""));

                                if (Config.GetBool("ImportLayerMQ"))
                                    layersToExport.Add(WinCachebox.Views.MapView.Manager.GetLayerByName("Stamen", "Stamen", ""));


                                foreach (Map.Layer layer in layersToExport)
                                {
                                    int MaxZoom = Config.GetInt("OsmMaxImportLevel");
                                    int MinZoom = Config.GetInt("OsmMinLevel");
                                    int osmCoverage = Config.GetInt("OsmCoverage");

                                    List<Map.Descriptor> iteratorCacheTiles = new List<Map.Descriptor>();

                                    for (int zoom = MinZoom; zoom <= MaxZoom; zoom++)
                                    {
                                        WinCachebox.Geocaching.BoundingBox area = expandCoordinates(cache.Latitude, cache.Longitude, (int)Math.Ceiling(Convert.ToDouble(osmCoverage) / (zoom + 1 - MinZoom)));

                                        int xFrom = (int)Math.Floor(Map.Descriptor.LongitudeToTileX(zoom, area.MinLongitude));
                                        int xTo = (int)Math.Ceiling(Map.Descriptor.LongitudeToTileX(zoom, area.MaxLongitude));
                                        int yFrom = (int)Math.Floor(Map.Descriptor.LatitudeToTileY(zoom, area.MinLatitude));
                                        int yTo = (int)Math.Ceiling(Map.Descriptor.LatitudeToTileY(zoom, area.MaxLatitude));

                                        for (int x = xFrom; x <= xTo; x++)
                                            for (int y = yFrom; y <= yTo; y++)
                                                iteratorCacheTiles.Add(new Map.Descriptor(x, y, zoom));
                                    }

                                    foreach (Map.Descriptor tile in iteratorCacheTiles)
                                    {
                                        String filename = layer.GetLocalFilename(tile);

                                        if (File.Exists(filename))
                                        {
                                            string mapPathDest = Path.GetDirectoryName(ExportPath) + "\\Cache\\";
                                            if (OwnRepository)
                                                mapPathDest = Path.GetDirectoryName(ExportPath) + "\\Repositories\\" + Path.GetFileNameWithoutExtension(ExportPath) + "\\Cache\\";
                                            if (!Directory.Exists(mapPathDest))
                                                Directory.CreateDirectory(mapPathDest);

                                            mapPathDest += layer.Name + "\\";
                                            if (!Directory.Exists(mapPathDest))
                                                Directory.CreateDirectory(mapPathDest);

                                            mapPathDest += tile.Zoom.ToString() + "\\";
                                            if (!Directory.Exists(mapPathDest))
                                                Directory.CreateDirectory(mapPathDest);

                                            mapPathDest += tile.X.ToString() + "\\";
                                            if (!Directory.Exists(mapPathDest))
                                                Directory.CreateDirectory(mapPathDest);

                                            //mapPathDest += tile.Y.ToString() + ".png";

                                            string destfilename = mapPathDest + tile.Y.ToString() + ".png";
                                            if (!File.Exists(destfilename))
                                            {
                                                File.Copy(filename, destfilename);
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region ExportMapPacksCreateList
                            //MAP PACKS
                            if (ExportMapPacks)
                            {
                                string pathDestMapPacksBase = Path.GetDirectoryName(ExportPath) + "\\Repository\\Maps";

                                foreach (Map.Pack pack in WinCachebox.Views.MapView.Manager.mapPacks)
                                {
                                    int maxZoom = 0;
                                    foreach (Map.BoundingBox bbox in pack.BoundingBoxes)
                                    {
                                        if (maxZoom < bbox.Zoom)
                                            maxZoom = bbox.Zoom;
                                    }

                                    List<Map.BoundingBox> list = pack.BoundingBoxes.FindAll(i => i.Zoom == maxZoom).ToList();

                                    foreach (Map.BoundingBox bbox in list)
                                    {
                                        if (mapPackList.Contains(pack))
                                            break;

                                        if (bbox.Contains((int)WinCachebox.Map.Descriptor.LongitudeToTileX(bbox.Zoom, cache.Longitude), (int)WinCachebox.Map.Descriptor.LatitudeToTileY(bbox.Zoom, cache.Latitude)))
                                        {
                                            if (!File.Exists(pathDestMapPacksBase + "\\" + Path.GetFileName(pack.Filename)))
                                                mapPackList.Add(pack);
                                        }
                                    }
                                }
                            }
                            #endregion

                        }

                        if (parent.Cancel)
                            break;
                    }

                    if (ExportMapPacks)
                    {
                        string pathDestMapPacks = Path.GetDirectoryName(ExportPath) + "\\Repository\\Maps";
                        int count = 1;
                        foreach (Map.Pack pack in mapPackList)
                        {
                            if (MapPacks == "" || MapPacks.Contains(pack.Layer.Name))
                            {
                                parent.ProgressChanged(""
                                    + Global.Translations.Get("export") + " "
                                    + Global.Translations.Get("mapPack") + ": "
                                    + Global.Translations.Get("wait") + ". "
                                    + Global.Translations.Get("file") + " "
                                    + count
                                     + " " + Global.Translations.Get("of") + " "
                                    + mapPackList.Count, count, mapPackList.Count);
                                if (OwnRepository)
                                    pathDestMapPacks = Path.GetDirectoryName(ExportPath) + "\\Repositories\\" + Path.GetFileNameWithoutExtension(ExportPath) + "\\Maps";

                                if (!Directory.Exists(pathDestMapPacks))
                                    Directory.CreateDirectory(pathDestMapPacks);
                                if (!File.Exists(pathDestMapPacks + "\\" + Path.GetFileName(pack.Filename)))
                                    File.Copy(pack.Filename, pathDestMapPacks + "\\" + Path.GetFileName(pack.Filename));
                            }
                            count++;

                        }
                        mapPackList.Clear();
                    }

                    transaction.Commit();
                    export.Dispose();
                    return aktCount;
                }
                catch (Exception exc)
                {
                    transaction.Rollback();
                    MessageBox.Show(Global.Translations.Get("Error") + " "
                    + Global.Translations.Get("export") + Environment.NewLine + exc.Message);
                    Global.AddLog("SDF-Export: Error while exporting: " + exc.Message);
                    return aktCount;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(Global.Translations.Get("Error") + " "
                    + Global.Translations.Get("export") + Environment.NewLine + exc.Message);
                Global.AddLog("SDF-Export: Error while exporting: " + exc.Message);
                return aktCount;
            }
        }

        public void ResizeImage(string OriginalFile, string NewFile, int NewWidth, int MaxHeight, bool OnlyResizeIfWider, bool rotate)
        {
            System.Drawing.Image FullsizeImage = System.Drawing.Image.FromFile(OriginalFile);

            FullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
            FullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);

            if (OnlyResizeIfWider)
            {
                if (FullsizeImage.Width <= NewWidth)
                {
                    NewWidth = FullsizeImage.Width;
                }
            }

            int NewHeight = FullsizeImage.Height * NewWidth / FullsizeImage.Width;
            if (NewHeight > MaxHeight)
            {
                NewWidth = FullsizeImage.Width * MaxHeight / FullsizeImage.Height;
                NewHeight = MaxHeight;
            }

            //if (!rotate && NewWidth == FullsizeImage.Width)
            //{
            //    File.Copy(OriginalFile, NewFile);
            //    FullsizeImage.Dispose();
            //    return;
            //}
            //else
            //{
                System.Drawing.Image NewImage = FullsizeImage.GetThumbnailImage(NewWidth, NewHeight, null, IntPtr.Zero);
                FullsizeImage.Dispose();

                if (rotate && NewImage.Width > NewImage.Height)
                {
                    Graphics g = Graphics.FromImage(NewImage);
                    g.DrawString("ROTATED", new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 9, System.Drawing.FontStyle.Bold), System.Drawing.Brushes.Red, new System.Drawing.Point(1, 1));
                    g.Dispose();

                    NewImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
                }

                System.Drawing.Imaging.ImageFormat format = System.Drawing.Imaging.ImageFormat.Jpeg;

                if (NewFile.EndsWith(".png"))
                    format = System.Drawing.Imaging.ImageFormat.Png;
                else if (NewFile.EndsWith(".tif") || NewFile.EndsWith(".tiff"))
                    format = System.Drawing.Imaging.ImageFormat.Tiff;
                else if (NewFile.EndsWith(".wmf"))
                    format = System.Drawing.Imaging.ImageFormat.Wmf;
                else if (NewFile.EndsWith(".emf"))
                    format = System.Drawing.Imaging.ImageFormat.Emf;
                else if (NewFile.EndsWith(".bmp"))
                    format = System.Drawing.Imaging.ImageFormat.Bmp;

                NewImage.Save(NewFile, format);

                NewImage.Dispose();
            //}
        }


        public int Import(bool onlyAsk)
        {
            int importCount = 0;

            if (!File.Exists(ExportPath))
            {
                if (onlyAsk)
                    return 0;
                MessageBox.Show(Global.Translations.Get("database") + " [" + ExportPath + "] " + Global.Translations.Get("notexists") + "!");
                return 0;
            }

            Database import = new Database();
            try
            {
                if (!import.Startup(ExportPath, false)) return -1;
            }
            catch (Exception)
            {
                return 0;
            }
            try
            {
                // read Replication database
                CBCommand rcommand = import.CreateCommand("select * from [Replication]");
                DbDataReader reader = rcommand.ExecuteReader();
                List<Replication> lReplication = new List<Replication>();
                while (reader.Read())
                {
                    try
                    {
                        Replication repl = new Replication(reader);
                        lReplication.Add(repl);
                    }
                    catch (Exception exc)
                    {
                        exc.GetType(); //Warning vermeiden _ avoid warning
                    }
                }
                reader.Dispose();
                rcommand.Dispose();

                if (onlyAsk && (lReplication.Count > 0))
                {
                    DialogResult question = MessageBox.Show(Global.Translations.Get("changes") + " ["
                        + ExportPath
                        + "] " + Global.Translations.Get("synchron"),
                        Global.Translations.Get("synchronize"),
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    if (question == DialogResult.Cancel)
                        return -1;
                    else if (question == DialogResult.No)
                        return 0;
                    // else synchronize...
                }
                foreach (Replication repl in lReplication)
                {
                    if (repl.Import(import))
                    {
                        // delete this replication entry from the database
                        CBCommand dcommand = import.CreateCommand("delete from [Replication] where CacheId=@cacheId and ChangeType=@changeType");
                        dcommand.ParametersAdd("@cacheId", DbType.Int64, repl.CacheId);
                        dcommand.ParametersAdd("@changeType", DbType.Int32, repl.ChangeType);
                        dcommand.ExecuteNonQuery();
                        dcommand.Dispose();
                        importCount++;
                    }
                }
            }
            finally
            {
                import.Dispose();
            }
            return importCount;
        }

        protected WinCachebox.Geocaching.BoundingBox expandCoordinates(double latitude, double longitude, int size)
        {
            WinCachebox.Geocaching.BoundingBox result = new WinCachebox.Geocaching.BoundingBox(latitude, longitude);

            double latRad = (latitude * Math.PI) / 180;
            double lonRad = (longitude * Math.PI) / 180;

            double radius = Math.Cos(latRad) * 6378137;
            double umfangKleinkreis = 2 * Math.PI * radius;

            double diffLon = (size * 360) / umfangKleinkreis;
            double diffLat = size / ((Math.PI * 6378137) / 180);

            result.MinLatitude -= diffLat;
            result.MaxLatitude += diffLat;
            result.MinLongitude -= diffLon;
            result.MaxLongitude += diffLon;

            return result;
        }

        public void LoadCaches()
        {
            List<Cache> tmpCaches = new List<Cache>();
            Global.Categories.ReadFromFilter(Filter);
            Global.Categories.WriteToFilter(Filter);
            Cache.LoadCaches(Filter.SqlWhere, tmpCaches);

            // filter Distance around Location
            Coordinate lastMarker = Global.Marker;
            if (Location != null)
            {
                // Location als Marker setzen f¸r Distanz-Berechnung 
                Global.Marker = Location.Coordinate;
            }
            Caches.Clear();
            foreach (Cache cache in tmpCaches)
            {
                // Distanz ¸berpr¸fen
                if ((MaxDistance > 0) && (cache.Distance(true) > MaxDistance * 1000))
                    continue;
                Caches.Add(cache);
            }
            // lastMarker wiederherstellen
            Global.Marker = lastMarker;
        }


        public void LoadCachesCount()
        {
            Global.Categories.ReadFromFilter(Filter);
            Global.Categories.WriteToFilter(Filter);

            Coordinate lastMarker = Global.Marker;
            if (Location != null)
                Global.Marker = Location.Coordinate;

            cacheCountAproximate = Cache.LoadCachesCount(Filter.SqlWhere, Convert.ToInt32(MaxDistance));

            Global.Marker = lastMarker;
        }
    }

    public class SdfExportSettings : List<SdfExportSetting>
    {
        public void Read()
        {
            CBCommand command = Database.Data.CreateCommand("select Id, [Description], [ExportPath], [MaxDistance], [LocationID], [Filter], [Update], [ExportImages], [ExportSpoilers], [ExportMaps], [OwnRepository], [ExportMapPacks], [MapPacks], [MaxLogs] from SdfExport");
            DbDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                SdfExportSetting setting = new SdfExportSetting(reader);
                this.Add(setting);
            }

            reader.Dispose();
            command.Dispose();
        }
    }

}
