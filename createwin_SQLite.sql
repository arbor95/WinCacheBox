CREATE TABLE [Locations] ([Id] integer not null primary key autoincrement, [Name] nvarchar (255) NULL, [Latitude] float NULL, [Longitude] float NULL);
CREATE INDEX [Locatioins_idx] ON [Locations] ([Id] ASC);

CREATE TABLE [SdfExport] ([Id]  integer not null primary key autoincrement, [Description] nvarchar(255) NULL, [ExportPath] nvarchar(255) NULL, [MaxDistance] float NULL, [LocationID] Bigint NULL, [Filter] ntext NULL, [Update] bit NULL, [ExportImages] bit NULL, [ExportSpoilers] bit NULL, [ExportMaps] bit NULL, [OwnRepository] bit NULL, [ExportMapPacks] bit NULL, [MapPacks] nvarchar(512) NULL, [MaxLogs] int NULL);
CREATE INDEX [SdfExport_idx] ON [SdfExport] ([Id] ASC);

ALTER TABLE [CACHES] ADD [FirstImported] datetime NULL;

CREATE TABLE [Category] ([Id]  integer not null primary key autoincrement, [GpxFilename] nvarchar(255) NULL, [Pinned] bit NULL default 0, [CacheCount] int NULL);
CREATE INDEX [Category_idx] ON [Category] ([Id] ASC);

ALTER TABLE [GpxFilenames] ADD [CategoryId] bigint NULL;

ALTER TABLE [Caches] add [state] nvarchar(50) NULL;
ALTER TABLE [Caches] add [country] nvarchar(50) NULL;

CREATE TABLE [Trackable] ([Id] integer not null primary key autoincrement, [Archived] bit NULL, [GcCode] nvarchar (15) NULL, [CacheId] bigint NULL, [CurrentGoal] ntext, [CurrentOwnerName] nvarchar (255) NULL, [DateCreated] datetime NULL, [Description] ntext, [IconUrl] nvarchar (255) NULL, [ImageUrl] nvarchar (255) NULL, [name] nvarchar (255) NULL, [OwnerName] nvarchar (255), [Url] nvarchar (255) NULL);
CREATE INDEX [cacheid_idx] ON [Trackable] ([CacheId] ASC);
CREATE TABLE [TbLogs] ([Id] integer not null primary key autoincrement, [TrackableId] integer not NULL, [CacheID] bigint NULL, [GcCode] nvarchar (15) NULL, [LogIsEncoded] bit NULL DEFAULT 0, [LogText] ntext, [LogTypeId] bigint NULL, [LoggedByName] nvarchar (255) NULL, [Visited] datetime NULL);
CREATE INDEX [trackableid_idx] ON [TbLogs] ([TrackableId] ASC);
CREATE INDEX [trackablecacheid_idx] ON [TBLOGS] ([CacheId] ASC);

CREATE TABLE [Images] ([Id] integer not null primary key autoincrement, [CacheId] bigint NULL, [GcCode] nvarchar (15) NULL, [Description] ntext, [Name] nvarchar (255) NULL, [ImageUrl] nvarchar (255) NULL, [IsCacheImage] bit NULL);
CREATE INDEX [images_cacheid_idx] ON [Images] ([CacheId] ASC);
CREATE INDEX [images_gccode_idx] ON [Images] ([GcCode] ASC);
CREATE INDEX [images_iscacheimage_idx] ON [Images] ([IsCacheImage] ASC);
CREATE UNIQUE INDEX [images_imageurl_idx] ON [Images] ([ImageUrl] ASC);
