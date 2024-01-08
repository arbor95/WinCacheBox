using System;
using System.IO;
using System.Data.SQLite;
using System.Data;
using System.Data.Common;
using System.Data.SqlServerCe;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;

namespace WinCachebox
{
    public class Database
    {
        public enum SqlServerType { SqlServerCE, SQLite };


        public static Database Data = new Database();

        protected string sdfPfad = "";
        public SqlServerType serverType = SqlServerType.SqlServerCE;

        protected SQLiteConnection slconnection = null;

        protected SqlCeConnection connection = null;
        public long DatabaseId = 0;  // for Database replication

        public Database() : this(SqlServerType.SqlServerCE)
        {
        }
        public Database(SqlServerType serverType)
        {
            this.serverType = serverType;
        }

        public SqlCeConnection Connection
        {
            get
            {
                try
                {
                    if (connection != null && connection.State == ConnectionState.Broken)
                    {
                        connection.Dispose();
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        connection = null;
                    }

                    if (connection == null)
                        Initialize(false);


                    if (connection != null)
                    {
                        if (connection.State == ConnectionState.Closed)
                            connection.Open();
                    }

                    return connection;
                }
                catch (Exception ex)
                {
                    Global.AddLog("SqlCeConnection Error: " + ex.Message);
                    return null;
                }
            }
        }

        public SQLiteConnection SlConnection
        {
            get
            {
                try
                {
                    if (slconnection != null && slconnection.State == ConnectionState.Broken)
                    {
                        slconnection.Dispose();
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        slconnection = null;
                    }

                    if (slconnection == null)
                        Initialize(false);


                    if (slconnection.State == ConnectionState.Closed)
                        slconnection.Open();

                    return slconnection;
                }
                catch (Exception exc)
                {
                    exc.GetType(); //Warning vermeiden _ avoid warning
                    return null;
                }
            }
        }

        public bool Startup(bool onlyCacheBox)
        {
            return Startup("", onlyCacheBox);
        }

        public bool Startup(string sdfPfad, bool onlyCacheBox)
        {
            if (sdfPfad != "")
                this.sdfPfad = sdfPfad;
            else
                this.sdfPfad = Global.databaseName;

            string ext = Path.GetExtension(this.sdfPfad).ToUpper();
            if (ext == ".DB3")
                this.serverType = SqlServerType.SQLite;
            else
                this.serverType = SqlServerType.SqlServerCE;

            int databaseSchemeVersion = GetDatabaseSchemeVersion();

            //change to SQL 3.5. The database must be upgraded.
            if (databaseSchemeVersion < 1000)
            {
                if (!File.Exists(this.sdfPfad))
                {
                    Reset(onlyCacheBox);
                    Initialize(onlyCacheBox);
                    SetDatabaseSchemeVersion();
                    if (onlyCacheBox)
                    {
                        WriteConfigLong("DatabaseId", DateTime.Now.Ticks);
                    }
                    else
                    {
                        DatabaseId = DateTime.Now.Ticks;
                        WriteConfigLong("DatabaseId", DatabaseId);
                    }

                    return true;
                }
                else
                {
                    // check whether the database is alread 3.5
                    bool isAlready35 = true;
                    try
                    {
                        // try to open the database with 3.5
                        Initialize(onlyCacheBox);
                    }
                    catch (Exception)
                    {
                        // exception 
                        // -> database is 3.0!!!!
                        isAlready35 = false;
                    }
                    if (!isAlready35)
                    {
                        DialogResult result = MessageBox.Show("Database must be converted to SqlCe3.5 format", "Database", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.No)
                        {
                            return false;
                        }
                        else if (result == DialogResult.Yes)
                        {
                            SqlCeEngine engine = new SqlCeEngine("Data Source=" + this.sdfPfad + ";Max Database Size = 1024");
                            engine.Upgrade();

                            Initialize(onlyCacheBox);
                            AlterDatabase(databaseSchemeVersion);
                            SetDatabaseSchemeVersion();
                        }
                    }
                }
            }

            Initialize(onlyCacheBox);

            databaseSchemeVersion = GetDatabaseSchemeVersion();
            int databaseSchemeVersionWin = GetDatabaseSchemeVersionWin();

            if ((databaseSchemeVersion < Global.LatestDatabaseChange) || ((databaseSchemeVersionWin < Global.LatestDatabaseChangeWin) && (!onlyCacheBox)))
            {
                string title = "";
                if (databaseSchemeVersion < Global.LatestDatabaseChange)
                    title += "C";
                if ((databaseSchemeVersionWin < Global.LatestDatabaseChangeWin) && (!onlyCacheBox))
                    title += "W";

                DialogResult result = MessageBox.Show("Database was changed. Update database?", "Database - " + title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    return false;
                }
                else
                {
                    if (result == DialogResult.Yes)
                    {
                        // Datenbank soll konvertiert werden.
                        // Damit die Warnung nicht noch einmal erscheint
                        // wird die Schemaversion entsprechend gesetzt.

                        if (databaseSchemeVersion < Global.LatestDatabaseChange)
                        {
                            // the CacheBox part is changed
                            AlterDatabase(databaseSchemeVersion);
                            SetDatabaseSchemeVersion();
                        }
                        if ((databaseSchemeVersionWin < Global.LatestDatabaseChangeWin) && (!onlyCacheBox))
                        {
                            // the WinCachebox part is changed
                            AlterDatabaseWin(databaseSchemeVersionWin);
                            SetDatabaseSchemeVersionWin();
                        }
                    }
                }
            }
            // ab Version 400 DB-Strukturänderungen auf dem SQL Server 3.5 per ALTER Statements machne

            if (onlyCacheBox)
            {
                long tmpDatabaseId = ReadConfigLong("DatabaseId");
                if (tmpDatabaseId <= 0)
                {
                    tmpDatabaseId = DateTime.Now.Ticks;
                    WriteConfigLong("DatabaseId", tmpDatabaseId);
                }
            }
            else
            {
                // create or load DatabaseId for each Database
                Database.Data.DatabaseId = ReadConfigLong("DatabaseId");
                if (DatabaseId <= 0)
                {
                    DatabaseId = DateTime.Now.Ticks;
                    WriteConfigLong("DatabaseId", Database.Data.DatabaseId);
                }
            }
            return true;
        }

        // Update der DB-Struktur - neue Felder für CacheBox hinzufügen
        public void AlterDatabase(int oldDatabaseSchemeVersion)
        {
            if (oldDatabaseSchemeVersion < 1002)
            {
                // Änderungen zwischen Version 370 und 399 - hier nur als Beispiel, da mit der Änderung des SQL-Servers die DB eh neu gebaut werden muss
                ExecuteStatementWithoutWarning("alter table Caches add Notes ntext;");
                ExecuteStatementWithoutWarning("alter table Waypoint add Title ntext;");
                ExecuteStatementWithoutWarning("alter table Caches add Favorit bit;");
                ExecuteStatementWithoutWarning("alter table Caches add AttributesPositive bigint;");
                ExecuteStatementWithoutWarning("alter table Caches add AttributesNegative bigint;");
                ExecuteStatementWithoutWarning("alter table Caches add TourName nchar(255)");
                ExecuteStatementWithoutWarning("alter table Caches add GPXFilename_Id bigInt");
                ExecuteStatementWithoutWarning("create table GPXFilenames (Id Bigint IDENTITY(1,1) PRIMARY KEY NOT NULL, GPXFilename nvarchar(255), Imported datetime default getdate(), Name nvarchar(255), CacheCount int);");
                ExecuteStatementWithoutWarning("create index GPXFilename_Id_idx on Caches(GPXFilename_Id)");
                ExecuteStatementWithoutWarning("create table PocketQueries (Id Bigint IDENTITY(1,1) PRIMARY KEY NOT NULL, PQName nvarchar(255), CreationTimeOfPQ datetime)");
                ExecuteStatementWithoutWarning("create index GPXFilename_Id_idx on Caches(GPXFilename_Id)");
                ExecuteStatementWithoutWarning("alter table GPXFilenames add Name nvarchar(255)");
                ExecuteStatementWithoutWarning("alter table GPXFilenames add CacheCount int");
                ExecuteStatementWithoutWarning("alter table Caches add HasUserData bit");
                ExecuteStatementWithoutWarning("alter table Caches add ListingCheckSum int default 0");
                ExecuteStatementWithoutWarning("alter table Caches add ListingChanged bit");
                ExecuteStatementWithoutWarning("alter table Caches add ImagesUpdated bit");
                ExecuteStatementWithoutWarning("alter table Caches add DescriptionImagesUpdated bit");
                ExecuteStatementWithoutWarning("alter table Caches add CorrectedCoordinates bit");
                ExecuteStatementWithoutWarning("create table CelltowerLocation (CellId nvarchar(20) PRIMARY KEY NOT NULL, Latitude float, Longitude float)");
                ExecuteStatementWithoutWarning("alter table Caches add Solver ntext");
                ExecuteStatementWithoutWarning("create table Config ([Key] nvarchar (30) NOT NULL, Value nvarchar (255) NULL)");
                ExecuteStatementWithoutWarning("create index Key_idx ON Config([Key])");

                // for Database Replication
                ExecuteStatementWithoutWarning("CREATE TABLE [Replication] ([Id] bigint IDENTITY(1,1) NOT NULL, [ChangeType] int NOT NULL, [CacheId] bigint NOT NULL, [WpGcCode] nvarchar (12) NULL, [SolverCheckSum] int NULL, [NotesCheckSum] int NULL, [WpCoordCheckSum] int NULL)");
                ExecuteStatementWithoutWarning("CREATE INDEX [Replication_idx] ON [Replication] ([Id] ASC);");
                ExecuteStatementWithoutWarning("CREATE INDEX [ReplicationCache_idx] ON [Replication] ([CacheId] ASC);");
            }
        }
        // Update der DB-Struktur - neuze Felder für WinCachebox hinzufügen
        public void AlterDatabaseWin(int oldDatabaseSchemeVersionWin)
        {
            if (oldDatabaseSchemeVersionWin < 1005)
            {
                ExecuteStatementWithoutWarning("create table Locations (Id Bigint IDENTITY(1,1) PRIMARY KEY NOT NULL, Name nvarchar(255), Latitude float NULL, Longitude float NULL);");
                ExecuteStatementWithoutWarning("create index Locations_Id_idx on Locations(Location_Id)");
                // saving of the SdfExportSettings
                ExecuteStatementWithoutWarning("create table SdfExport ([Id] Bigint IDENTITY(1,1) PRIMARY KEY NOT NULL, [Description] nvarchar(255) NULL, [ExportPath] nvarchar(255) NULL, [MaxDistance] float NULL, [LocationID] Bigint NULL, [Filter] ntext NULL, [Update] bit NULL)");
                ExecuteStatementWithoutWarning("create index SdfExport_idx ON SdfExport([Id])");
            }
            if (oldDatabaseSchemeVersionWin < 1006)
            {
                ExecuteStatementWithoutWarning("alter table Caches add FirstImported datetime NULL");
                // LastImported aller bestehenden Caches auf das aktuelle Datum setzen
                CBCommand command = CreateCommand("update Caches set FirstImported=@firstImported");
                command.ParametersAdd("firstImported", DbType.DateTime, DateTime.Now.Subtract(TimeSpan.FromDays(2)));
                command.ExecuteNonQuery();
                command.Dispose();
            }
            if (oldDatabaseSchemeVersionWin < 1007)
            {
                ExecuteStatementWithoutWarning("CREATE TABLE [Category] ([Id] bigint IDENTITY(1,1) PRIMARY KEY NOT NULL, [GpxFilename] nvarchar(255) NULL, [Pinned] bit NULL default 0, [CacheCount] int NULL);");
                ExecuteStatementWithoutWarning("CREATE INDEX [Category_idx] ON [Category] ([Id] ASC);");
                ExecuteStatementWithoutWarning("ALTER TABLE [GpxFilenames] ADD [CategoryId] bigint NULL;");
                // GpxFilenames mit Kategorien verknüpfen
                Dictionary<long, string> gpxFilenames = new Dictionary<long, string>();
                Dictionary<string, long> categories = new Dictionary<string, long>();
                CBCommand query = CreateCommand("select ID, GPXFilename from GPXFilenames");
                DbDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    long id = reader.GetInt64(0);
                    string gpxFilename = reader.GetString(1);
                    gpxFilenames.Add(id, gpxFilename);
                }
                foreach (KeyValuePair<long, string> kvp in gpxFilenames)
                {
                    if (!categories.ContainsKey(kvp.Value))
                    {
                        // neue Category hinzufügen
                        CBCommand addquery = CreateCommand("insert into [Category] (GpxFilename) values (@gpxFilename)");
                        addquery.ParametersAdd("@gpxFilename", DbType.String, kvp.Value);
                        addquery.ExecuteNonQuery();
                        addquery.Dispose();
                        addquery = CreateCommand("select max(Id) from [Category]");
                        long categoryId = Convert.ToInt32(addquery.ExecuteScalar().ToString());
                        addquery.Dispose();
                        // und merken
                        categories.Add(kvp.Value, categoryId);
                    }
                    if (categories.ContainsKey(kvp.Value))
                    {
                        CBCommand uquery = CreateCommand("update [GpxFilenames] set CategoryId=@CategoryId where Id=@Id");
                        uquery.ParametersAdd("@CategoryId", DbType.String, categories[kvp.Value]);
                        uquery.ParametersAdd("@Id", DbType.Int64, kvp.Key);
                        uquery.ExecuteNonQuery();
                        uquery.Dispose();
                    }
                }

            }
            if (oldDatabaseSchemeVersionWin < 1008)
            {
                ExecuteStatementWithoutWarning("alter table SdfExport ADD [ExportImages] bit NULL, [ExportSpoilers] bit NULL, [ExportMaps] bit NULL ");
                // set Default values
                CBCommand command = CreateCommand("update SdfExport set ExportImages=@ExportImages, ExportSpoilers=@ExportSpoilers, ExportMaps=@ExportMaps");
                command.ParametersAdd("ExportImages", DbType.Boolean, true);
                command.ParametersAdd("ExportSpoilers", DbType.Boolean, true);
                command.ParametersAdd("ExportMaps", DbType.Boolean, true);
                command.ExecuteNonQuery();
                command.Dispose();
            }
            if (oldDatabaseSchemeVersionWin < 1009)
            {
                ExecuteStatementWithoutWarning("alter table SdfExport ADD [OwnRepository] bit NULL");
            }
            if (oldDatabaseSchemeVersionWin < 1010)
            {
                ExecuteStatementWithoutWarning("alter table SdfExport ADD [ExportMapPacks] bit NULL");
            }
            if (oldDatabaseSchemeVersionWin < 1012)
            {
                ExecuteStatementWithoutWarning("ALTER TABLE [Caches] ADD [state] nvarchar(50) NULL;");
                ExecuteStatementWithoutWarning("ALTER TABLE [Caches] ADD [country] nvarchar(50) NULL;");
            }
            if (oldDatabaseSchemeVersionWin < 1013)
            {
                ExecuteStatementWithoutWarning("alter table SdfExport ADD [MapPacks] nvarchar(512) NULL");
            }
            if (oldDatabaseSchemeVersionWin < 1014)
            {
                ExecuteStatementWithoutWarning("alter table SdfExport Add [MaxLogs] int NULL");
            }
            if (oldDatabaseSchemeVersionWin < 1016)
            {
                // for compatibility to Cachebox for Android
                ExecuteStatementWithoutWarning("ALTER TABLE [CACHES] ADD [ApiStatus] smallint NULL default 0");
            }
            if (oldDatabaseSchemeVersionWin < 1017)
            {
                ExecuteStatementWithoutWarning("create table Locations (Id Bigint IDENTITY(1,1) PRIMARY KEY NOT NULL, Name nvarchar(255), Latitude float NULL, Longitude float NULL);");

                if (serverType == SqlServerType.SqlServerCE)
                    ExecuteStatementWithoutWarning("CREATE TABLE [Trackable] ([Id] Bigint IDENTITY(1,1) PRIMARY KEY NOT NULL, [Archived] bit NULL, [GcCode] nvarchar (12) NULL, [CacheId] bigint NULL, [CurrentGoal] ntext, [CurrentOwnerName] nvarchar (255) NULL, [DateCreated] datetime NULL, [Description] ntext, [IconUrl] nvarchar (255) NULL, [ImageUrl] nvarchar (255) NULL, [name] nvarchar (255) NULL, [OwnerName] nvarchar (255), [Url] nvarchar (255) NULL);");
                else
                    ExecuteStatementWithoutWarning("CREATE TABLE [Trackable] ([Id] integer not null primary key autoincrement, [Archived] bit NULL, [GcCode] nvarchar (12) NULL, [CacheId] bigint NULL, [CurrentGoal] ntext, [CurrentOwnerName] nvarchar (255) NULL, [DateCreated] datetime NULL, [Description] ntext, [IconUrl] nvarchar (255) NULL, [ImageUrl] nvarchar (255) NULL, [name] nvarchar (255) NULL, [OwnerName] nvarchar (255), [Url] nvarchar (255) NULL);");
                ExecuteStatementWithoutWarning("CREATE INDEX [cacheid_idx] ON [Trackable] ([CacheId] ASC);");
                ExecuteStatementWithoutWarning("CREATE TABLE [TbLogs] ([Id] integer not null primary key autoincrement, [TrackableId] integer not NULL, [CacheID] bigint NULL, [GcCode] nvarchar (12) NULL, [LogIsEncoded] bit NULL DEFAULT 0, [LogText] ntext, [LogTypeId] bigint NULL, [LoggedByName] nvarchar (255) NULL, [Visited] datetime NULL);");
                ExecuteStatementWithoutWarning("CREATE INDEX [trackableid_idx] ON [TbLogs] ([TrackableId] ASC);");
                ExecuteStatementWithoutWarning("CREATE INDEX [trackablecacheid_idx] ON [TBLOGS] ([CacheId] ASC);");

            }
            if (oldDatabaseSchemeVersionWin < 1018)
            {
                // Bei SQLite wurde vergessen dies hinzuzufügen
                if (serverType == SqlServerType.SQLite)
                    ExecuteStatementWithoutWarning("ALTER TABLE [SdfExport] ADD [MapPacks] nvarchar(512) NULL");
            }
            if (oldDatabaseSchemeVersionWin < 1019)
            {
                // neue Felder für die erweiterten Attribute einfügen
                ExecuteStatementWithoutWarning("ALTER TABLE [CACHES] ADD [AttributesPositiveHigh] bigint NULL default 0");
                ExecuteStatementWithoutWarning("ALTER TABLE [CACHES] ADD [AttributesNegativeHigh] bigint NULL default 0");

                // Die Nummerierung der Attribute stimmte nicht mit der von Groundspeak überein. Bei 16 und 45 wurde jeweils eine Nummber übersprungen
                CBCommand query = CreateCommand("select Id, AttributesPositive, AttributesNegative from Caches");
                DbDataReader reader = query.ExecuteReader();

                DbTransaction trans = StartTransaction();
                while (reader.Read())
                {
                    long id = reader.GetInt64(0);
                    ulong attributesPositive = (ulong)reader.GetInt64(1);
                    ulong attributesNegative = (ulong)reader.GetInt64(2);

                    attributesPositive = convertAttribute(attributesPositive);
                    attributesNegative = convertAttribute(attributesNegative);

                    CBCommand uquery = CreateCommand("update [Caches] set AttributesPositive=@AttributesPositive, AttributesNegative=@AttributesNegative where Id=@Id");
                    uquery.ParametersAdd("@Id", DbType.Int64, id);
                    uquery.ParametersAdd("@AttributesPositive", DbType.Int64, attributesPositive);
                    uquery.ParametersAdd("@AttributesNegative", DbType.Int64, attributesNegative);
                    uquery.ExecuteNonQuery();
                    uquery.Dispose();
                }
                trans.Commit();

            }
            if (oldDatabaseSchemeVersionWin < 1020)
            {
                ExecuteStatementWithoutWarning("ALTER TABLE [Config] ADD [LongString] ntext NULL");

            }
            if (oldDatabaseSchemeVersionWin < 1021)
            {
                ExecuteStatementWithoutWarning("ALTER TABLE [Caches] ALTER COLUMN [GcCode] nvarchar(15); ");

                ExecuteStatementWithoutWarning("ALTER TABLE [Waypoint] DROP CONSTRAINT Waypoint_PK ");
                ExecuteStatementWithoutWarning("ALTER TABLE [Waypoint] ALTER COLUMN [GcCode] nvarchar(15) NOT NULL; ");
                ExecuteStatementWithoutWarning("ALTER TABLE [Waypoint] ADD CONSTRAINT  [Waypoint_PK] PRIMARY KEY ([GcCode]); ");

                ExecuteStatementWithoutWarning("ALTER TABLE [Replication] ALTER COLUMN [WpGcCode] nvarchar(15); ");
                ExecuteStatementWithoutWarning("ALTER TABLE [Trackable] ALTER COLUMN [GcCode] nvarchar(15); ");
                ExecuteStatementWithoutWarning("ALTER TABLE [TbLogs] ALTER COLUMN [GcCode] nvarchar(15); ");
            }
            if (oldDatabaseSchemeVersionWin < 1022)
            {
                // Image Table
                if (serverType == SqlServerType.SqlServerCE)
                    ExecuteStatementWithoutWarning("CREATE TABLE [Images] ([Id] integer not null primary key autoincrement, [CacheId] bigint NULL, [GcCode] nvarchar (15) NULL, [Description] ntext, [Name] nvarchar (255) NULL, [ImageUrl] nvarchar (255) NULL, [IsCacheImage] bit NULL);");
                else
                    ExecuteStatementWithoutWarning("CREATE TABLE [Images] ([Id] Bigint IDENTITY(1,1) PRIMARY KEY NOT NULL, [CacheId] bigint NULL, [GcCode] nvarchar (15) NULL, [Description] ntext, [Name] nvarchar (255) NULL, [ImageUrl] nvarchar (255) NULL, [IsCacheImage] bit NULL);");
                ExecuteStatementWithoutWarning("CREATE INDEX [images_cacheid_idx] ON [Images] ([CacheId] ASC);");
                ExecuteStatementWithoutWarning("CREATE INDEX [images_gccode_idx] ON [Images] ([GcCode] ASC);");
                ExecuteStatementWithoutWarning("CREATE INDEX [images_iscacheimage_idx] ON [Images] ([IsCacheImage] ASC);");
                ExecuteStatementWithoutWarning("CREATE UNIQUE INDEX [images_imageurl_idx] ON [Images] ([ImageUrl] ASC);");
            }
            if (oldDatabaseSchemeVersionWin < 1024)
            {
                // Waypoint is Startpoint
                ExecuteStatementWithoutWarning("ALTER TABLE [Waypoint] ADD COLUMN [IsStart] BOOLEAN DEFAULT 'false' NULL");
            }
            if (oldDatabaseSchemeVersionWin < 1026)
            {
                // add one column for short description
                // [ShortDescription] ntext NULL
                ExecuteStatementWithoutWarning("ALTER TABLE [Caches] ADD [ShortDescription] ntext NULL;");
            }

            if (oldDatabaseSchemeVersionWin < 1027)
            {
                // add one column for Favorite Points
                // [FavPoints] SMALLINT 0
                ExecuteStatementWithoutWarning("ALTER TABLE [CACHES] ADD [FavPoints] smallint NULL default 0;");

            }

        }

        private ulong convertAttribute(ulong att)
        {
            // Die Nummerierung der Attribute stimmte nicht mit der von Groundspeak überein. Bei 16 und 45 wurde jeweils eine Nummber übersprungen
            ulong result = 0;
            // Maske für die untersten 15 bit
            ulong mask = 0;
            for (int i = 0; i < 16; i++)
                mask += (ulong)1 << i;
            // unterste 15 bit ohne Verschiebung kopieren
            result = att & mask;
            // Maske für die Bits 16-45 
            mask = 0;
            for (int i = 16; i < 45; i++)
                mask += (ulong)1 << i;
            ulong tmp = att & mask;
            // Bits 16-44 um eins verschieben
            tmp = tmp << 1;
            // und zum Result kopieren
            result += tmp;
            // Maske für die Bits 45-45 
            mask = 0;
            for (int i = 45; i < 63; i++)
                mask += (ulong)1 << i;
            tmp = att & mask;
            // Bits 45-63 um 2 verschieben
            tmp = tmp << 2;
            // und zum Result kopieren
            result += tmp;

            return result;
        }

        public void Reset(bool onlyCacheBox)
        {
            if (File.Exists(sdfPfad))
                File.Delete(sdfPfad);

            switch (serverType)
            {
                case SqlServerType.SqlServerCE:
                    String connectionString = "Max Database Size = 4091; Data Source=" + sdfPfad;
                    SqlCeEngine engine = new SqlCeEngine(connectionString);
                    engine.CreateDatabase();
                    engine.Dispose();
                    ExecuteSqlScript("WinCachebox.create.sql");
                    if (!onlyCacheBox)
                        ExecuteSqlScript("WinCachebox.createwin.sql");
                    break;
                case SqlServerType.SQLite:
                    SQLiteConnection.CreateFile(sdfPfad);
                    ExecuteSqlScript("WinCachebox.create_SQLite.sql");
                    if (!onlyCacheBox)
                        ExecuteSqlScript("WinCachebox.createwin_SQLite.sql");
                    break;
            }


            // write DatabaseSchemeVersion
            CBCommand command = CreateCommand("Insert into Config([Key], [Value]) values (@Key, @Value)");
            command.ParametersAdd("@Key", DbType.String, "DatabaseSchemeVersion");
            command.ParametersAdd("@Value", DbType.String, Global.LatestDatabaseChange.ToString());
            command.ExecuteNonQuery();
            command.Dispose();
            if (!onlyCacheBox)
            {
                command = CreateCommand("Insert into Config([Key], [Value]) values (@Key, @Value)");
                command.ParametersAdd("@Key", DbType.String, "DatabaseSchemeVersionWin");
                command.ParametersAdd("@Value", DbType.String, Global.LatestDatabaseChangeWin.ToString());
                command.ExecuteNonQuery();
                command.Dispose();
            }
        }

        public void ExecuteSqlScript(String ressourceName)
        {
            StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(ressourceName));

            while (!reader.EndOfStream)
            {
                String cmd = reader.ReadLine();
                if (cmd.Trim().Length > 0)
                {
                    try
                    {
                        CBCommand command = CreateCommand(cmd);
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        string teste = ex.ToString();
                    }
                }
            }

            reader.Close();
        }

        // führe ein SQL-Statement aus
        public int ExecuteStatement(String Statement)
        {
            CBCommand command = CreateCommand(Statement);
            int result = command.ExecuteNonQuery();
            command.Dispose();
            return result;
        }

        // führe ein SQL-Statement ganz ohne Fehlermeldungen aus
        public void ExecuteStatementWithoutWarning(String Statement)
        {
            try
            {
                ExecuteStatement(Statement);
            }
            catch (Exception exc)
            {
                // keine Meldung, mach einfach nix
#if DEBUG
                Global.AddLog("ExecuteStatementWithoutWarning: (" + Statement + ") " + exc.ToString());
#endif
            }
        }

        public void ResetConnection()
        {
            if (connection != null)
            {
                connection.Close();
                connection.Dispose();
                connection = null;
            }
            if (slconnection != null)
            {
                slconnection.Close();
                slconnection.Dispose();
                slconnection = null;
            }
        }

        public void Dispose()
        {
            if (connection != null)
            {
                connection.Close();
                connection.Dispose();
            }
            if (slconnection != null)
            {
                slconnection.Close();
                slconnection.Dispose();
                slconnection = null;
            }
        }

        /// <summary>
        /// Initialisiert die Datenbank. Wurde hierhin ausgelagert, damit
        /// es im Splash Screen als Aktion angezeigt werden kann.
        /// </summary>
        public void Initialize(bool onlyCacheBox)
        {
            switch (serverType)
            {
                case SqlServerType.SqlServerCE:
                    if (connection == null)
                    {
                        if (!File.Exists(sdfPfad))
                            Reset(onlyCacheBox);

                        connection = new SqlCeConnection("Data Source=" + sdfPfad + ";Max Database Size = 4091");
                        if (connection.State == ConnectionState.Closed)
                            connection.Open();
                    }
                    break;
                case SqlServerType.SQLite:
                    if (slconnection == null)
                    {
                        if (!File.Exists(sdfPfad))
                            Reset(onlyCacheBox);

                        slconnection = new SQLiteConnection("Data Source=" + sdfPfad + ";Pooling=false;FailIfMissing=false;Cache Size=3000");
                        if (slconnection.State == ConnectionState.Closed)
                            slconnection.Open();
                    }
                    break;
            }

        }

        public void GPXFilenameUpdateCacheCount()
        {
            // welche GPXFilenamen sind in der DB erfasst
            CBCommand command = CreateCommand("select GPXFilename_ID, Count(*) as CacheCount from Caches where GPXFilename_ID is not null Group by GPXFilename_ID");
            DbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Int64 GPXFilename_ID = reader.GetInt64(0);
                Int64 CacheCount = reader.GetInt32(1);
                ExecuteStatement("Update GPXFilenames set CacheCount = " + CacheCount.ToString() + " where ID = " + GPXFilename_ID);
            };
            ExecuteStatement("delete from GPXFilenames where Cachecount is NULL or CacheCount = 0 ");
            ExecuteStatement("delete from GPXFilenames where ID not in (Select GPXFilename_ID From Caches)");

            command.Dispose();
            reader.Dispose();
            Global.Categories = new Geocaching.Categories();
            Global.Categories.ReadFromFilter(Global.LastFilter);
            Global.Categories.DeleteEmptyCategories();
        }

        public void DeleteOldLogs()
        {
            int minToKeep = Config.GetInt("LogMinCount") * 5;

            List<Int64> oldLogCaches = new List<Int64>();

            CBCommand command = CreateCommand("select cacheid from logs WHERE Timestamp < @timestamp GROUP BY CacheId HAVING COUNT(Id) > @minToKeep");
            command.ParametersAdd("@timestamp", DbType.Date, DateTime.Now.AddMonths(-Config.GetInt("LogMaxMonthAge")));
            command.ParametersAdd("@minToKeep", DbType.Int32, minToKeep);
            DbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                oldLogCaches.Add(reader.GetInt64(0));
            };

            foreach (Int64 oldLogCache in oldLogCaches)
            {
                List<Int64> minLogIds = new List<Int64>();

                CBCommand command2 = CreateCommand("select id from logs where cacheid = @cacheid order by Timestamp desc");
                command2.ParametersAdd("@cacheid", DbType.Int64, oldLogCache);
                DbDataReader reader2 = command2.ExecuteReader();
                int count = 0;
                while (reader2.Read())
                {
                    if (count == minToKeep)
                        break;
                    minLogIds.Add(reader2.GetInt64(0));
                    count++;
                };

                command2.Dispose();
                reader2.Dispose();

                StringBuilder sb = new StringBuilder();
                foreach (Int64 id in minLogIds)
                    sb.Append(id).Append(",");

                CBCommand command3 = CreateCommand("delete from Logs where Timestamp<@timestamp and cacheid = @cacheid and id not in (" + sb.Remove(sb.Length - 1, 1).ToString() + ")");
                command3.ParametersAdd("@timestamp", DbType.Date, DateTime.Now.AddMonths(-Config.GetInt("LogMaxMonthAge")));
                command3.ParametersAdd("@cacheid", DbType.Int64, oldLogCache);
                command3.ExecuteNonQuery();
                command3.Dispose();
            }

            command.Dispose();
            reader.Dispose();
        }

        public void Repair()
        {
            switch (serverType)
            {
                case SqlServerType.SQLite:
                    CBCommand command = CreateCommand("vacuum");
                    command.ExecuteNonQuery();
                    return;
                case SqlServerType.SqlServerCE:
                    SqlCeEngine engine = new SqlCeEngine(connection.ConnectionString);
                    if (engine != null)
                    {
                        string connectionString = connection.ConnectionString;
                        connection.Close();
                        engine.Repair(connection.ConnectionString, RepairOption.RecoverAllPossibleRows);
                        connection.Open();
                    }
                    return;
                default:
                    return;
            }


        }
        public void Compact()
        {
            switch (serverType)
            {
                case SqlServerType.SQLite:
                    CBCommand command = CreateCommand("vacuum");
                    command.ExecuteNonQuery();
                    return;
                case SqlServerType.SqlServerCE:
                    SqlCeEngine engine = new SqlCeEngine(connection.ConnectionString);
                    if (engine != null)
                    {
                        string connectionString = connection.ConnectionString;
                        connection.Close();
                        engine.Compact(connection.ConnectionString);
                        connection.Open();
                    }
                    return;
                default:
                    return;
            }


        }

        public int GetDatabaseSchemeVersion()
        {
            int result = -1;
            if (!File.Exists(sdfPfad))
                return result;
            CBCommand command = CreateCommand("select Value from Config where [Key] = @Key");
            command.ParametersAdd("@Key", DbType.String, "DatabaseSchemeVersion");
            try
            {
                DbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string databaseSchemeVersion = reader.GetString(0);
                    result = Convert.ToInt32(databaseSchemeVersion);
                };
                reader.Dispose();
            }
            catch (Exception)
            {
                result = -1;
            }
            command.Dispose();

            return result;
        }

        public int GetDatabaseSchemeVersionWin()
        {
            int result = -1;
            if (!File.Exists(sdfPfad))
                return result;

            CBCommand command = CreateCommand("select Value from Config where [Key] = @Key");
            command.ParametersAdd("@Key", DbType.String, "DatabaseSchemeVersionWin");
            try
            {
                DbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string databaseSchemeVersion = reader.GetString(0);
                    result = Convert.ToInt32(databaseSchemeVersion);
                };
                reader.Dispose();
            }
            catch (Exception)
            {
                result = -1;
            }
            command.Dispose();

            return result;
        }

        public void SetDatabaseSchemeVersion()
        {
            int anz = 0;
            try
            {
                anz = ExecuteStatement("Update Config set Value = " + Global.LatestDatabaseChange.ToString() + " where [Key] = 'DatabaseSchemeVersion'");
            }
            catch (Exception exc)
            {
                exc.GetType(); //Warning vermeiden _ avoid warning
            }
            if (anz == 0)
            {
                // Update does not work
                // -> new entry
                CBCommand command = CreateCommand("Insert into Config([Key], [Value]) values (@Key, @Value)");
                command.ParametersAdd("@Key", DbType.String, "DatabaseSchemeVersion");
                command.ParametersAdd("@Value", DbType.String, Global.LatestDatabaseChange.ToString());
                command.ExecuteNonQuery();
                command.Dispose();
            }
        }

        public void SetDatabaseSchemeVersionWin()
        {
            int anz = 0;
            try
            {
                anz = ExecuteStatement("Update Config set Value = " + Global.LatestDatabaseChangeWin.ToString() + " where [Key] = 'DatabaseSchemeVersionWin'");
            }
            catch (Exception exc)
            {
                exc.GetType(); //Warning vermeiden _ avoid warning
            }
            if (anz == 0)
            {
                // Update does not work
                // -> new entry
                CBCommand command = CreateCommand("Insert into Config([Key], [Value]) values (@Key, @Value)");
                command.ParametersAdd("@Key", DbType.String, "DatabaseSchemeVersionWin");
                command.ParametersAdd("@Value", DbType.String, Global.LatestDatabaseChangeWin.ToString());
                anz = command.ExecuteNonQuery();
                command.Dispose();
            }
        }

        public void WriteConfigString(string key, string value)
        {
            int anz = 0;
            try
            {
                if (value.Length == 0)
                {
                    anz = ExecuteStatement("Delete from Config where [Key] = '" + key + "'");
                }
                else
                {
                    anz = ExecuteStatement("Update Config set Value = '" + value + "' where [Key] = '" + key + "'");
                }
            }
            catch (Exception exc)
            {
                exc.GetType(); //Warning vermeiden _ avoid warning
            }
            if (anz == 0)
            {
                // Update does not work
                // -> new entry
                CBCommand command = CreateCommand("Insert into Config([Key], [Value]) values (@Key, @Value)");
                command.ParametersAdd("@Key", DbType.String, key);
                command.ParametersAdd("@Value", DbType.String, value);
                anz = command.ExecuteNonQuery();
                command.Dispose();
            }
        }

        public string ReadConfigString(string key)
        {
            string result = "";
            CBCommand command = CreateCommand("select Value from Config where [Key] = @Key");
            command.ParametersAdd("@Key", DbType.String, key);
            try
            {
                DbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result = reader.GetString(0);
                };
                reader.Dispose();
            }
            catch (Exception)
            {
                result = "";
            }
            command.Dispose();

            return result;
        }
        public void WriteConfigLong(string key, long value)
        {
            WriteConfigString(key, value.ToString());
        }

        public long ReadConfigLong(string key)
        {
            string value = ReadConfigString(key);
            try
            {
                return Convert.ToInt64(value);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static void AddParamter(DbCommand command, string parameterName, DbType type, object value)
        {
            if (command is SqlCeCommand)
                (command as SqlCeCommand).Parameters.Add(parameterName, type).Value = value;
            else if (command is SQLiteCommand)
                (command as SQLiteCommand).Parameters.Add(parameterName, type).Value = value;
        }

        public CBCommand CreateCommand(string commandText)
        {
            switch (serverType)
            {
                case SqlServerType.SqlServerCE:
                    return new CBCommandSqlCE(Connection, commandText);
                case SqlServerType.SQLite:
                    return new CBCommandSQLite(SlConnection, commandText);
                default:
                    return null;
            }
        }

        public DbTransaction StartTransaction()
        {
            switch (serverType)
            {
                case SqlServerType.SqlServerCE:
                    return connection.BeginTransaction();
                case SqlServerType.SQLite:
                    return slconnection.BeginTransaction();
                default:
                    return null;
            }
        }
    }

    public abstract class CBCommand
    {
        public CBCommand()
        {
        }
        public virtual void Dispose()
        {
        }
        abstract public DbDataReader ExecuteReader();
        abstract public int ExecuteNonQuery();
        abstract public object ExecuteScalar();
        abstract public void ParametersAdd(string parameterName, DbType type, object value);
        abstract public void ChangeParamter(string parameterName, object value);
    }
    public class CBCommandSqlCE : CBCommand
    {
        private SqlCeCommand command;
        public CBCommandSqlCE(SqlCeConnection connection, string commandText)
        {
            command = connection.CreateCommand();
            command.CommandText = commandText;
            command.CommandType = CommandType.Text;
        }
        public override void Dispose()
        {
            command.Dispose();
        }
        public override DbDataReader ExecuteReader()
        {
            return command.ExecuteReader();
        }
        public override int ExecuteNonQuery()
        {
            return command.ExecuteNonQuery();
        }
        public override object ExecuteScalar()
        {
            return command.ExecuteScalar();
        }
        public override void ParametersAdd(string parameterName, DbType type, object value)
        {
            command.Parameters.Add(parameterName, type).Value = value;
        }
        public override void ChangeParamter(string parameterName, object value)
        {
            command.Parameters[parameterName].Value = value;
        }
    }
    public class CBCommandSQLite : CBCommand
    {
        private SQLiteCommand command;
        public CBCommandSQLite(SQLiteConnection connection, string commandText)
        {
            command = connection.CreateCommand();
            command.CommandText = commandText;
            command.CommandType = CommandType.Text;
        }
        public override void Dispose()
        {
            command.Dispose();
        }
        public override DbDataReader ExecuteReader()
        {
            return command.ExecuteReader();
        }
        public override int ExecuteNonQuery()
        {
            return command.ExecuteNonQuery();
        }
        public override object ExecuteScalar()
        {
            return command.ExecuteScalar();
        }
        public override void ParametersAdd(string parameterName, DbType type, object value)
        {
            command.Parameters.Add(parameterName, type).Value = value;
        }
        public override void ChangeParamter(string parameterName, object value)
        {
            command.Parameters[parameterName].Value = value;
        }
    }
}
