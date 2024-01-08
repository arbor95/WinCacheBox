CREATE TABLE [Caches] ([Id] bigint NOT NULL,[GcCode] nvarchar (15) NULL,[GcId] nvarchar (255) NULL,[Latitude] float NULL,[Longitude] float NULL,[Name] nchar (255) NULL,[Size] int NULL,[Difficulty] smallint NULL,[Terrain] smallint NULL,[Archived] bit NULL,[Available] bit NULL,[Found] bit NULL,[Type] smallint NULL,[PlacedBy] nvarchar (255) NULL,[Owner] nvarchar (255) NULL,[DateHidden] datetime NULL,[Hint] ntext NULL,[ShortDescription] ntext NULL,[Description] ntext NULL,[Url] nchar (255) NULL,[FavPoints] smallint NULL default 0,[NumTravelbugs] smallint NULL,[Rating] smallint NULL,[Vote] smallint NULL,[VotePending] bit NULL,[Notes] ntext NULL,[Solver] ntext NULL,[Favorit] bit NULL,[AttributesPositive] bigint NULL,[AttributesNegative] bigint NULL,[TourName] nchar (255) NULL,[GPXFilename_Id] bigint NULL,[HasUserData] bit NULL,[ListingCheckSum] int NULL DEFAULT 0,[ListingChanged] bit NULL,[ImagesUpdated] bit NULL,[DescriptionImagesUpdated] bit NULL,[CorrectedCoordinates] bit NULL);
ALTER TABLE [Caches] ADD CONSTRAINT  [Caches_PK] PRIMARY KEY ([Id]);
CREATE INDEX [archived_idx] ON [Caches] ([Archived] ASC);
CREATE INDEX [AttributesNegative_idx] ON [Caches] ([AttributesNegative] ASC);
CREATE INDEX [AttributesPositive_idx] ON [Caches] ([AttributesPositive] ASC);
CREATE INDEX [available_idx] ON [Caches] ([Available] ASC);
CREATE INDEX [Difficulty_idx] ON [Caches] ([Difficulty] ASC);
CREATE INDEX [Favorit_idx] ON [Caches] ([Favorit] ASC);
CREATE INDEX [found_idx] ON [Caches] ([Found] ASC);
CREATE INDEX [GPXFilename_Id_idx] ON [Caches] ([GPXFilename_Id] ASC);
CREATE INDEX [HasUserData_idx] ON [Caches] ([HasUserData] ASC);
CREATE INDEX [ListingChanged_idx] ON [Caches] ([ListingChanged] ASC);
CREATE INDEX [NumTravelbugs_idx] ON [Caches] ([NumTravelbugs] ASC);
CREATE INDEX [placedby_idx] ON [Caches] ([PlacedBy] ASC);
CREATE INDEX [Rating_idx] ON [Caches] ([Rating] ASC);
CREATE INDEX [Size_idx] ON [Caches] ([Size] ASC);
CREATE INDEX [Terrain_idx] ON [Caches] ([Terrain] ASC);
CREATE INDEX [Type_idx] ON [Caches] ([Type] ASC);

CREATE TABLE [CelltowerLocation] ([CellId] nvarchar (20) NOT NULL,[Latitude] float NULL,[Longitude] float NULL);
ALTER TABLE [CelltowerLocation] ADD CONSTRAINT  [CelltowerLocation_PK] PRIMARY KEY ([CellId]);

CREATE TABLE [FieldNotes] ([CacheId] bigint NULL,[Timestamp] datetime NULL,[Type] smallint NULL,[FoundNumber] int NULL,[Comment] ntext NULL);

CREATE TABLE [GPXFilenames] ([Id] bigint IDENTITY(1,1) NOT NULL,[GPXFilename] nvarchar (255) NULL,[Imported] datetime NULL DEFAULT getdate(),[Name] nvarchar (255) NULL,[CacheCount] int NULL);
ALTER TABLE [GPXFilenames] ADD CONSTRAINT  [GPXFilenames_PK] PRIMARY KEY ([Id]);

CREATE TABLE [Logs] ([Id] bigint NOT NULL,[CacheId] bigint NULL,[Timestamp] datetime NULL,[Finder] nvarchar (128) NULL,[Type] smallint NULL,[Comment] ntext NULL);
ALTER TABLE [Logs] ADD CONSTRAINT  [Logs_PK] PRIMARY KEY ([Id]);
CREATE INDEX [log_idx] ON [Logs] ([CacheId] ASC);
CREATE INDEX [timestamp_idx] ON [Logs] ([Timestamp] ASC);

CREATE TABLE [PocketQueries] ([Id] bigint IDENTITY(1,1) NOT NULL,[PQName] nvarchar (255) NULL,[CreationTimeOfPQ] datetime NULL);
ALTER TABLE [PocketQueries] ADD CONSTRAINT  [PocketQueries_PK] PRIMARY KEY ([Id]);

CREATE TABLE [Waypoint] ([GcCode] nvarchar (15) NOT NULL,[CacheId] bigint NULL,[Latitude] float NULL,[Longitude] float NULL,[Description] ntext NULL,[Clue] ntext NULL,[Type] smallint NULL,[SyncExclude] bit NULL,[UserWaypoint] bit NULL,[Title] ntext NULL);
ALTER TABLE [Waypoint] ADD COLUMN [IsStart] BOOLEAN DEFAULT 'false' NULL;
ALTER TABLE [Waypoint] ADD CONSTRAINT  [Waypoint_PK] PRIMARY KEY ([GcCode]);
CREATE INDEX [UserWaypoint_idx] ON [Waypoint] ([UserWaypoint] ASC);

CREATE TABLE [Config] ([Key] nvarchar (30) NOT NULL, [Value] nvarchar (255) NULL, [LongString] ntext NULL);
CREATE INDEX [Key_idx] ON [Config] ([Key] ASC);

CREATE TABLE [Replication] ([Id] bigint IDENTITY(1,1) NOT NULL, [ChangeType] int NOT NULL, [CacheId] bigint NOT NULL, [WpGcCode] nvarchar (15) NULL, [SolverCheckSum] int NULL, [NotesCheckSum] int NULL, [WpCoordCheckSum] int NULL);
CREATE INDEX [Replication_idx] ON [Replication] ([Id] ASC);
CREATE INDEX [ReplicationCache_idx] ON [Replication] ([CacheId] ASC);

ALTER TABLE [CACHES] ADD [ApiStatus] smallint NULL default 0
ALTER TABLE [CACHES] ADD [AttributesPositiveHigh] bigint NULL default 0
ALTER TABLE [CACHES] ADD [AttributesNegativeHigh] bigint NULL default 0

ALTER TABLE [Waypoint] ADD COLUMN [IsStart] BOOLEAN DEFAULT 'false' NULL