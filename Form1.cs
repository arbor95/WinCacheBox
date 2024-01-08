using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using WinCachebox.CacheWolf;
using WinCachebox.Geocaching;
using WinCachebox.Map;
using WinCachebox.Views;
using Microsoft.Win32;
using WeifenLuo.WinFormsUI.Docking;

namespace WinCachebox
{
    public partial class Form1 : Form
    {
        private SplashForm splash;
        private RegistryKey registry = null;
        public static Form1 MainForm = null;
        public MapView mapview = null;
        private bool mapviewLoaded = false;
        public CacheListView viewCacheList = null;
        private bool viewCacheListLoaded = false;
        public DescriptionView viewDescription = null;
        private bool viewDescriptionLoaded = false;
        public LogView viewLogs = null;
        private bool viewLogsLoaded = false;
        public SpoilerView viewSpoiler = null;
        private bool viewSpoilerLoaded = false;
        public NotesView viewNotes = null;
        private bool viewNotesLoaded = false;
        public HintView viewHint = null;
        private bool viewHintLoaded = false;
        public SolverView viewSolver = null;
        private bool viewSolverLoaded = false;
        public WaypointView viewWaypoints = null;
        private bool viewWaypointsLoaded = false;
        private Views.CacheInfoView cacheInfoView1 = new WinCachebox.Views.CacheInfoView();

        public Form1()
        {
            if (!DesignMode)
                MainForm = this;
            // 
            // cacheInfoView1 hierher, weil der Ansichts-Designer das CacheInfoView nicht mehr öffnete 
            // Scheint jetzt aber wieder zu funktionieren (wie lange?)
            // 
            this.cacheInfoView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cacheInfoView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cacheInfoView1.Location = new System.Drawing.Point(0, 0);
            this.cacheInfoView1.Name = "cacheInfoView1";
            this.cacheInfoView1.Size = new System.Drawing.Size(877, 60);
            this.cacheInfoView1.TabIndex = 0;

            // first search the local application folder for a configFileName
            // if here a configFileName is found -> portable mode without registry access!
            Global.AppPath = "";
            try
            {
                string configFile = Path.GetDirectoryName(Application.ExecutablePath);
                if (File.Exists(configFile + Global.configFileName))
                {
                    // if there the configFileName exists -> use it an start the programm without dialog
                    Global.AppPath = configFile;
                }



                if (Global.AppPath == "")
                {
                    // if no local config -> search in the registry for a ConfigFile entry
                    registry = Registry.CurrentUser.OpenSubKey("Software\\Ging-Buh\\WinCachebox", RegistryKeyPermissionCheck.ReadWriteSubTree);
                    if (registry == null)
                        registry = Registry.CurrentUser.CreateSubKey("Software\\Ging-Buh\\WinCachebox", RegistryKeyPermissionCheck.ReadWriteSubTree);

                    configFile = (string)registry.GetValue("ConfigFile");
                    if ((configFile != null) && (configFile != "") && (File.Exists(configFile + Global.configFileName)))
                    {
                        // Registry-Entry exists
                        // and configFileName exists
                        // -> start Programm normal without dialog
                        Global.AppPath = configFile;
                    }
                }
                if (Global.AppPath == "")
                {
                    // Search in user-data folder for the configFileName file
                    configFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WinCachebox";
                    if (File.Exists(configFile + Global.configFileName))
                    {
                        // if there the configFileName exists -> use it an start the programm without dialog
                        Global.AppPath = configFile;
                    }
                }

                if (Global.AppPath == "")
                    Views.Forms.SelectConfigFolder.Show();
                if (Global.AppPath == "")
                {
                    // no database folder is selected -> terminate application
                    Environment.Exit(-1);
                    return;
                }
            }
            catch (Exception)
            {
                Environment.Exit(-1);
            }
            if (!Directory.Exists(Global.AppPath))
            {
                try
                {
                    Directory.CreateDirectory(Global.AppPath);
                }
                catch (Exception ex)
                {
                    // sould not happen!
                    MessageBox.Show(Global.Translations.Get("f30", "Database folder [") + Global.AppPath + Global.Translations.Get("f31", "] could not be created!\n") + ex.Message);
                    Application.Exit();
                }

            }

            // if the configFolder has changed -> save it in the registry
            if (registry != null)
            {
                if ((string)registry.GetValue("ConfigFile") != Global.AppPath)
                    registry.SetValue("ConfigFile", Global.AppPath);
            }

            string proxy = Config.GetString("Proxy");
            if (proxy != "")
            {
                Global.Proxy = new System.Net.WebProxy(proxy, Config.GetInt("ProxyPort"));

                string proxyUserName = Config.GetString("ProxyUserName");
                if (proxyUserName != "")
                {
                    Global.Proxy.Credentials = new System.Net.NetworkCredential(proxyUserName, Config.GetString("ProxyPassword"), Config.GetString("ProxyDomain"));
                }
            }

            // System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            Global.LastValidPosition = new Coordinate(Config.GetDouble("MapInitLatitude"), Config.GetDouble("MapInitLongitude"));
#if DEBUG
            Global.AddLog("--- cachebox startup, rev " + Global.sCurrentRevision + " ----------------------------------------------------------");
#endif

            int initCnt = 1;

            // Splash Screen anzeigen
            splash = new SplashForm();
            splash.Show();
            Application.DoEvents();

            // Changes for MultiDB Support
            Global.databaseName = Config.GetString("DatabasePath");
            String DataPath = Global.AppPath; //  Path.GetDirectoryName(Global.databaseName); // or use Global.AppPath ?
            String DBType = Path.GetExtension(Global.databaseName).Substring(1);
            String DBName = Path.GetFileNameWithoutExtension(Global.databaseName);
            String[] args = Environment.GetCommandLineArgs();
            bool useOwnRepository = false;
            // use DB from CommandLine or from Config
            if (args.Length > 1)
                if (!args[1].Equals("")) DBName = args[1];
            if (args.Length > 2)
                if (!args[2].Equals("")) DBType = args[2];
            if (args.Length > 3 && args[3].Equals("own"))
                useOwnRepository = true;
            else
            {
                // check if subpath DBName exists for Repositories --> useOwnRepository = true;
                if (Directory.Exists(DataPath + "\\Repositories\\" + DBName))
                    useOwnRepository = true;
            }

            // Datenbank öffnen
            splash.updateAction("Opening Database...", initCnt++);
            OpenDataBase(DataPath, DBName, DBType, useOwnRepository);

            //Load Language
            splash.updateAction("Loading Language...", initCnt);
            Global.Translations = new LangStrings();
            try
            {
                Global.Translations.ReadTranslationsFile(Global.ExePath + "\\data\\lang\\" + Config.GetString("SelectedLanguage") + ".lan");
            }
            catch (Exception exc)
            {
            }

            checkDirectory(Config.GetString("TileCacheFolder"));
            checkDirectory(Config.GetString("MapPackFolder"));
            checkDirectory(Config.GetString("DescriptionImageFolder"));
            checkDirectory(Config.GetString("SpoilerFolder"));

            checkDirectory(Config.GetString("PocketQueryFolder"));
            checkDirectory(Config.GetString("UserImageFolder"));
            checkDirectory(Config.GetString("TrackFolder"));

            Global.TrackDistance = Config.GetInt("TrackDistance");

            // Benutzerinterface aufbauen
            splash.updateAction("Initializing User Interface...", initCnt++);

            Global.InitIcons();
            //      pictureBox1.Image = imageListGpsSignal.Images[0];


            Map.Descriptor.Init();
            // Views initialisieren
            //      addView(new AboutViewPanel());
            //      addView(new CompassViewPanel());
            //      addView(CacheListPanel = new ListViewPanel());
            //      addView(new DescriptionView());

            // Map Packs laden
            splash.updateAction("Loading Map Packs...", initCnt++);
            String mapPackFolder = (Config.GetString("MapPackFolder") == "") ? Global.AppPath + "\\Repository\\Maps" : Config.GetString("MapPackFolder");
            String[] mapPacks = System.IO.Directory.GetFiles(mapPackFolder);
            foreach (String mapPack in mapPacks)
            {
                MapView.Manager.LoadMapPack(mapPack);
            }

            splash.updateAction("Loading Routes...", initCnt++);
            //      addView(mapview);
            //      addView(WaypointListPanel = new WaypointViewPanel());
            //      addView(new LogView());
            //      TabContainerView settingsView = new TabContainerView();
            //      settingsView.AddView(new GpsSettings(settingsView), "GPS");
            //      settingsView.AddView(new MapSettings(settingsView), "Map");

            //      TabContainerView dataView = new TabContainerView();
            //      dataView.AddView(dataSettingsCleanupView = new DataSettings(dataView), "Maintenance");
            //      dataView.AddView(new MailSettings(dataView), "Mail");
            /*      settingsView.AddView(dataView, "Data");
                  settingsView.AddView(new MiscSettings(settingsView), "Misc");
                  addView(settingsView);

                  addView(new RadarView());
                  addView((spoilerView = new SpoilerView()));
                  TabContainerView filterView = new TabContainerView();
                  filterView.AddView(new FilterPresets(), "Presets");
                  filterSettings = new FilterSettings();
                  filterView.AddView(filterSettings, "Settings");
                  filterView.AddButton(ApplyFilterFromEditor, "Apply Filter");
                  addView(filterView);
                  addView(new NotesView());
                  addView(TrackListPanel = new TrackViewPanel(mapview));

                  // Menüs initialisieren
                  infoContext = new ClickContext(this, holdButton1, 0);
                  infoContext.Add(new ClickButton("Description", null, ShowDescription, null, null), true);
                  waypointButton = new ClickButton("Waypoints", null, ShowWaypoints, null, null, false);
                  infoContext.Add(waypointButton, true);
                  notesButton = new ClickButton("Notes", null, ShowNotes, null, null, false);
                  infoContext.Add(notesButton, true);
                  infoContext.Add(FavoritButton = new ClickButton("Favorit", Global.Icons[7], showFavoritChanged, null, null), false);

                  navContext = new ClickContext(this, holdButton2, 0);
                  navContext.Add(new ClickButton("Map", null, ShowMap, null, null), true);
                  navContext.Add(new ClickButton("Compass", null, ShowCompass, null, null), true);
                  navContext.Add((radarButton = new ClickButton("Radar", null, ShowRadar, null, RadarButtonHold, false)), true);
                  navContext.Add(new ClickButton("Tracks", null, ShowTracks, null, null), true);

                  findContext = new ClickContext(this, holdButton3, 0);
                  findContext.Add((logButton = new ClickButton("Show Logs", null, ShowLogView, null, null)), true);
                  findContext.Add(fieldNoteButton = new ClickButton("My Field Notes", null, ShowFieldNotesContext, null, null), false);
                  hintButton = new ClickButton("Hint", null, ShowHint, null, null, false);
                  findContext.Add(hintButton, false);
                  findContext.Add((spoilerButton = new ClickButton("Spoiler", null, ShowSpoiler, null, DownloadSpoiler, false)), true);

                  miscContext = new ClickContext(this, holdButton4, 0);
                  miscContext.Add(new ClickButton("About cachebox!", null, ShowAbout, null, null), true);
                  miscContext.Add(new ClickButton("Settings", null, ShowSettings, null, null), true);
                  miscContext.Add((toolsButton = new ClickButton("Tools", null, ShowToolsContext, null, null)), false);
                  miscContext.Add(new ClickButton("Import", null, ImportGPX, null, null), false);

                  miscContext.Add(new ClickButton("Quit / Hide", null, Quit, null, HideApplication), false);

                  fieldNotesContext = new ClickContext(this);
                  fieldNotesContext.Add(new ClickButton("Found!", Global.Icons[2], FieldNoteFound, null, null), false);
                  fieldNotesContext.Add(new ClickButton("Did not find!", Global.Icons[4], FieldNoteNotFound, null, null), false);
                  fieldNotesContext.Add(new ClickButton("Needs Maintenance", Global.Icons[5], FieldNoteMaintenance, null, null), false);
                  fieldNotesContext.Add(new ClickButton("Clear current field note", null, ClearFieldNotes, null, null), false);
                  fieldNotesContext.Add(new ClickButton("Show Recent Notes", null, ShowRecentFieldNotes, null, null), false);

                  if (!Config.GetBool("SaveFieldNotesHtml"))
                    fieldNotesContext.Items[4].Visible = false;

                  filterContext = new ClickContext(this, holdButton5, 0);
                  filterContext.Add(cacheListButton = new ClickButton("List", null, ShowCacheList, null, null), true);
                  filterContext.Add(filterButton = new ClickButton("Filter", null, ShowFilterSettings, null, null, true), true);
                  filterContext.Add(searchButton = new ClickButton("Search", null, SearchCaches, null, null), false);
                  filterContext.Add(new ClickButton("Resort List", null, Resort, null, null), false);
                  filterContext.Add(AutoResortButton = new ClickButton("Auto Resort", Global.Icons[7], AutoResortChanged, null, null), false);

                  trackLogContext = new ClickContext(this, pictureBox1, 0);
                  trackLogContext.Add(recordButton = new ClickButton("Record", Global.LoadRessourceBitmap("rec_record.png"), StartRecording, null, null), false);
                  trackLogContext.Add(pauseButton = new ClickButton("Pause", Global.LoadRessourceBitmap("rec_pause.png"), PauseRecording, null, null), false);
                  trackLogContext.Add(stopButton = new ClickButton("Stop", Global.LoadRessourceBitmap("rec_stop.png"), StopRecording, null, null), false);

                  toolsContext = new ClickContext(this);
                  toolsContext.Add(new ClickButton("Export", null, ShowExportContext, null, null, true), false);
                  toolsContext.Add(new ClickButton("Track Recorder", null, ShowTrackRecorderContext, null, null, true), false);

                  toolsContext.Add(new ClickButton("Voice Recorder", null, ShowVoiceRecorder, null, null), false);
                  toolsContext.Add(createPocketQueryButton = new ClickButton("Create Pocket Query", null, CreatePocketQuery, null, null), false);
                  toolsContext.Add(uploadFieldNotesButton = new ClickButton("Upload Fieldnotes", null, UploadFieldnotes, null, null), false);
                  exportContext = new ClickContext(this);
                  exportContext.Add(new ClickButton("TomTom", null, WriteOV2, null, null), false);
                  exportContext.Add(new ClickButton("GPX", null, WriteGPX, null, null), false);
                  RegistryKey googleKey = Registry.LocalMachine.OpenSubKey("Software\\Google\\GoogleMaps");
                  if (googleKey != null)
                  {
                    exportContext.Add((googleMapButton = new ClickButton("Google Map", null, ShowGoogleMaps, null, null, false)), true);
                    googleKey.Close();
                  }

                  // Knopf mit Camera nur anbieten, wenn das System auch ne Kamera hat.
                  toolsContext.Add(cameraImageButton = new ClickButton("Take Photo", null, OpenCameraStill, null, null), false);
                  toolsContext.Add(cameraVideoButton = new ClickButton("Record Video", null, OpenCameraVideo, null, null), false);
                  toolsContext.Add(new ClickButton("Check for Updates", null, ConfirmUpdateApplication, null, null), false);

                  Locator.Locator.LocationDataReceived += new Locator.Locator.LocationDataReceivedHandler(locator_LocationDataReceived);
                  Locator.Locator.PositionChanged += new Locator.Locator.LocationDataReceivedHandler(Locator_PositionChanged);
                  Global.TargetChanged += OnCacheChanged;

                  mapview.UpdateLayerButtons();


            */
            // Hier wird auch der WaitCursor wieder ausgestellt
            splash.updateAction("Loading Caches...", initCnt++);
            LoadCategories();
            //      ShowAbout(null);

            splash.Close();
            splash.Dispose();

            // Initialisierung des GPS
            // Das muss hier gemacht werden, damit bei Benutzern
            // mit Bluetooth-Geräten der Verbindungsbildschirm erscheint
            //      Global.StartGps();
            //      ShowAbout(null);


            // Cacheliste setzen, damit man von Anfang an Caches über die
            // Karte auswählen kann
            //      CacheListPanel.OnShow();

            //      updateRecorderButtonAccessibility();
            //      timerUpdateBattery(null, null);

            InitializeComponent();

            // init Views with DockPanel...
            // CacheListView
            viewCacheList = new CacheListView
            {
                CloseButtonVisible = false,
                Text = Global.Translations.Get("cacheList", "Cache List")
            };
            // MapView
            mapview = new MapView();
            mapview.Initialize();
            mapview.CurrentLayer = MapView.Manager.GetLayerByName(Config.GetString("CurrentMapLayer"), Config.GetString("CurrentMapLayer"), "");
            mapview.CloseButtonVisible = false;
            mapview.Text = Global.Translations.Get("mapview", "Map View");
            // Description View
            viewDescription = new DescriptionView
            {
                CloseButtonVisible = false,
                Text = Global.Translations.Get("Description", "Description")
            };
            // Log View
            viewLogs = new LogView
            {
                CloseButtonVisible = false,
                Text = Global.Translations.Get("Logs", "Logs")
            };
            // Spoiler View
            viewSpoiler = new SpoilerView
            {
                CloseButtonVisible = false,
                Text = Global.Translations.Get("spoiler", "Spoiler")
            };
            // Notes View
            viewNotes = new NotesView
            {
                CloseButtonVisible = false,
                Text = Global.Translations.Get("Notes", "Notes")
            };
            // Hint View
            viewHint = new HintView
            {
                CloseButtonVisible = false,
                Text = Global.Translations.Get("hint", "Hint")
            };
            // Sovler View
            viewSolver = new SolverView
            {
                CloseButtonVisible = false,
                Text = Global.Translations.Get("Solver", "Solver")
            };
            // WaypointsView
            viewWaypoints = new WaypointView
            {
                CloseButtonVisible = false,
                Text = Global.Translations.Get("Waypoints", "Waypoints")
            };
            loadDockPanel("");

            if (!viewCacheListLoaded)
                viewCacheList.Show(dockPanel1, DockState.Document);
            if (!mapviewLoaded)
                mapview.Show(viewCacheList.Pane, null);
            if (!viewDescriptionLoaded)
                viewDescription.Show(viewCacheList.Pane, DockAlignment.Right, 0.5);
            if (!viewLogsLoaded)
                viewLogs.Show(viewDescription.Pane, null);
            if (!viewSpoilerLoaded)
                viewSpoiler.Show(viewDescription.Pane, null);
            if (!viewNotesLoaded)
                viewNotes.Show(viewDescription.Pane, null);
            if (!viewHintLoaded)
                viewHint.Show(viewDescription.Pane, null);
            if (!viewSolverLoaded)
                viewSolver.Show(viewDescription.Pane, null);
            if (!viewWaypointsLoaded)
                viewWaypoints.Show(viewCacheList.Pane, DockAlignment.Bottom, 0.2);


            viewCacheList.Show();
            viewDescription.Show();

            txtDistance.Text = Config.GetString("DistanceFilter");

            ApplyFilter(Global.LastFilter);
            foreach (Layer layer in MapView.Manager.Layers)
            {
                ToolStripMenuItem tsmi = new ToolStripMenuItem(layer.Name);
                miMapLayer.DropDownItems.Add(tsmi);
                tsmi.Click += new EventHandler(tsi_Click);
                if (MapView.View.CurrentLayer.Name == layer.Name)
                    tsmi.Checked = true;
                tsmi.Tag = layer;
            }
            mapview.OnShow();
            SetTitle(useOwnRepository);
            openDBMenu.Text= Global.Translations.Get("openDBMenu");
            fileToolStripMenuItem.Text = Global.Translations.Get("f3", "&File");
            viewToolStripMenuItem.Text = Global.Translations.Get("f4", "&View");
            databaseToolStripMenuItem.Text = "&" + Global.Translations.Get("database", "&Database");
            settingsToolStripMenuItem.Text = "&" + Global.Translations.Get("settings", "Settings");
            aboutToolStripMenuItem.Text = Global.Translations.Get("f7", "&Help");
            // f3 File submenu
            gPToolStripMenuItem.Text = Global.Translations.Get("import", "Import");
            cacheWolfImportToolStripMenuItem.Text = Global.Translations.Get("f9", "CacheWolf Import");
            sDFExportToolStripMenuItem.Text = "&" + Global.Translations.Get("export", "_Export");
            batchSDFExportToolStripMenuItem.Text = "&" + Global.Translations.Get("batchExportImport", "Batch Export/Import");
            importUserDataFromSDFToolStripMenuItem.Text = Global.Translations.Get("import") + " &" + Global.Translations.Get("userdata");
            exitToolStripMenuItem.Text = Global.Translations.Get("f13", "Exit");
            // f4 View submenu
            miMapLayer.Text = Global.Translations.Get("f14", "&Map-Layer");
            filterToolStripMenuItem.Text = Global.Translations.Get("f15", "&Filter");
            resetFilterToolStripMenuItem.Text = Global.Translations.Get("f28", "&Reset Filter");
            // f5 Database submenu
            archiveFilterselectionToolStripMenuItem.Text = Global.Translations.Get("f16", "Archive Filter Selection");
            deleteFilterSelectionToolStripMenuItem.Text = Global.Translations.Get("f17", "Delete Filter Selection");
            updateCacheStatusFilterSelectionToolStripMenuItem.Text = Global.Translations.Get("f18", "Update Cache Status (Filter Selection)");
            cCompactDatabaseToolStripMenuItem.Text = Global.Translations.Get("f19", "&Compact Database");
            repairDatabaseToolStripMenuItem.Text = Global.Translations.Get("f20", "&Repair Database");
            resetListingChangedToolStripMenuItem.Text = Global.Translations.Get("reset")
                 + " \"" + Global.Translations.Get("listing")
                 + " " + Global.Translations.Get("changed") + "\""
                ;
            resetImagesUpdatedToolStripMenuItem.Text = Global.Translations.Get("reset")
                 + " \"" + Global.Translations.Get("spoiler")
                 + " " + Global.Translations.Get("changed") + "\""
                ;
            // f6 Settings submenu
            locationsToolStripMenuItem.Text = "&" + Global.Translations.Get("Locations");
            settingsToolStripMenuItem1.Text = "&" + Global.Translations.Get("settings");
            resetLayoutToolStripMenuItem.Text = Global.Translations.Get("f23", "Reset Layout");
            // f7 help submenu
            aboutToolStripMenuItem1.Text = Global.Translations.Get("f24", "&About WinCachebox");
            //
            toolStripLabel1.Text = Global.Translations.Get("Location", "Location");
        }

        void OpenDataBase(string DataPath, string DBName, string DBType, bool useOwnRepository)
        {

            Global.databaseName = DataPath + "\\" + DBName + "." + DBType;
            Config.Set("DatabasePath", Global.databaseName);

            // todo ? perhaps don't write back to config file, use Global - values instead
            if (useOwnRepository)
            {
                Config.Set("SpoilerFolder", DataPath + "\\Repositories\\" + DBName + "\\Spoilers");
                Config.Set("TileCacheFolder", DataPath + "\\Repositories\\" + DBName + "\\Cache");
                Config.Set("DescriptionImageFolder", DataPath + "\\Repositories\\" + DBName + "\\Images");
                Config.Set("MapPackFolder", DataPath + "\\Repositories\\" + DBName + "\\Maps");
                checkDirectory(Config.GetString("TileCacheFolder"));
                checkDirectory(Config.GetString("MapPackFolder"));
                checkDirectory(Config.GetString("DescriptionImageFolder"));
                checkDirectory(Config.GetString("SpoilerFolder"));
            }
            else
            {
                Config.Set("SpoilerFolder", DataPath + "\\Repository\\Spoilers");
                Config.Set("TileCacheFolder", DataPath + "\\Cache");
                Config.Set("DescriptionImageFolder", DataPath + "\\Repository\\Images");
                Config.Set("MapPackFolder", DataPath + "\\Repository\\Maps");
            }

            try
            {
                if (!Database.Data.Startup(false))
                {
                    splash.Close();
                    Application.Exit();
                    return;
                }
            }
            catch (Exception exc)
            {
#if DEBUG
                Global.AddLog("Main.InitializeDatabase: " + exc.ToString());
#endif
                MessageBox.Show(Global.Translations.Get("ErrDbStartup", "Error during database startup!"), Global.Translations.Get(Global.Translations.Get("Error")), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                splash.Close();
                Application.Exit();
            }
            // und dann schreiben wir noch die Einträge für ownRepository in die Datenbank
            if (useOwnRepository)
            {
                Database.Data.WriteConfigString("TileCacheFolder", "?\\" + DBName + "\\Cache");
                Database.Data.WriteConfigString("SpoilerFolder", "?\\" + DBName + "\\Spoilers");
                Database.Data.WriteConfigString("DescriptionImageFolder", "?\\" + DBName + "\\Images");
                Database.Data.WriteConfigString("MapPackFolder", "?\\" + DBName + "\\Maps");
            }
            else
            {
                // und werfen sie gegbenenfalls wieder raus
                Database.Data.WriteConfigString("TileCacheFolder", "");
                Database.Data.WriteConfigString("SpoilerFolder", "");
                Database.Data.WriteConfigString("DescriptionImageFolder", "");
                Database.Data.WriteConfigString("MapPackFolder", "");
            }
        }

        void LoadCategories()
        {
            Global.Categories = new Categories();
            Global.LastFilter = null;
            if (Config.GetString("Filter").Length == 0)
            {
                Global.LastFilter = new FilterProperties(FilterPresets.presets[0]);
            }
            else
            {
                Global.LastFilter = new FilterProperties(Config.GetString("Filter"));
            }

            Database.Data.GPXFilenameUpdateCacheCount();
        }

        void SetTitle(bool useOwnRepository)
        {
            this.Text = Global.Translations.Get("f1", "CacheBox for Windows ") + "(" + Global.databaseName;
            if (useOwnRepository) this.Text += "; " + Global.Translations.Get("ownRepos", "own Repository");
            this.Text += ")";
        }

        void tsi_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = sender as ToolStripMenuItem;
            if (tsmi != null)
            {
                Layer layer = tsmi.Tag as Layer;
                if (layer != null)
                {
                    MapView.View.SetCurrentLayer(layer);
                    foreach (ToolStripMenuItem tsmi2 in miMapLayer.DropDownItems)
                    {
                        if (tsmi2 != null)
                            tsmi2.Checked = false;
                    }
                    tsmi.Checked = true;
                }
            }
        }

        private void checkDirectory(string directory)
        {
            if (!System.IO.Directory.Exists(directory))
                System.IO.Directory.CreateDirectory(directory);
        }

        public void ApplyFilter(FilterProperties props)
        {
            Cursor.Current = Cursors.WaitCursor;
#if DEBUG
            Global.AddLog("Main.ApplyFilter: " + props.SqlWhere);
#endif
            int MaxDistance = 0;

            try { MaxDistance = Convert.ToInt32(txtDistance.Text); }
            catch (Exception) { }

            if (MaxDistance > 0)
            {
                //List<Cache> caches = new List<Cache>();
                Geocaching.Cache.LoadCaches(props.SqlWhere, MaxDistance);

                //if (caches != null)
                //{
                //    Cache.Query = new List<Cache>();
                //    Global.CacheCount = 0;
                //}

                //foreach (Cache cache in caches)
                //{
                //    if ((MaxDistance > 0) && (cache.Distance(true) > MaxDistance * 1000))
                //        continue;

                //    Cache.Query.Add(cache);
                //    ++Global.CacheCount;
                //}
            }
            else
                Geocaching.Cache.LoadCaches(props.SqlWhere);

            CacheListView.View.FilterChanged();

            Cursor.Current = Cursors.Default;
            if ((props.ToString() == "") || (props.ToString().Equals(new FilterProperties(FilterPresets.presets[0]).ToString())))
            {
                // all Caches visible
                if (Global.PreviousFilter == null) toolStripButton1.Visible = false;
                toolStripButton1.Text = Global.Translations.Get("f29", "Redo LastFilter");
                viewCacheList.changeBackgroundColor(SystemColors.Window); // Color.white
            }
            else
            {
                // Caches filtered
                Global.PreviousFilter = props; // remember for redo
                toolStripButton1.Visible = true;
                toolStripButton1.Text = Global.Translations.Get("f28", "Reset Filter");
                viewCacheList.changeBackgroundColor(Color.LightYellow);
            };
        }

        private DeserializeDockContent m_deserializeDockContent;
        private IDockContent GetContentFromPersistString(string persistString)
        {
            if (persistString == typeof(CacheListView).ToString())
            {
                viewCacheListLoaded = true;
                return viewCacheList;
            }
            else if (persistString == typeof(WaypointView).ToString())
            {
                viewWaypointsLoaded = true;
                return viewWaypoints;
            }
            else if (persistString == typeof(MapView).ToString())
            {
                mapviewLoaded = true;
                return mapview;
            }
            else if (persistString == typeof(DescriptionView).ToString())
            {
                viewDescriptionLoaded = true;
                return viewDescription;
            }
            else if (persistString == typeof(LogView).ToString())
            {
                viewLogsLoaded = true;
                return viewLogs;
            }
            else if (persistString == typeof(SpoilerView).ToString())
            {
                viewSpoilerLoaded = true;
                return viewSpoiler;
            }
            else if (persistString == typeof(NotesView).ToString())
            {
                viewNotesLoaded = true;
                return viewNotes;
            }
            else if (persistString == typeof(HintView).ToString())
            {
                viewHintLoaded = true;
                return viewHint;
            }
            else if (persistString == typeof(SolverView).ToString())
            {
                viewSolverLoaded = true;
                return viewSolver;
            }
            else
                return null;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.panel2.Controls.Add(this.cacheInfoView1);

            registry = Registry.CurrentUser.OpenSubKey("Software\\Ging-Buh\\WinCachebox", RegistryKeyPermissionCheck.ReadWriteSubTree);
            if (registry != null)
            {
                try
                {
                    this.Left = (int)registry.GetValue("WindowLeft", 225);
                    this.Top = (int)registry.GetValue("WindowTop", 225);
                    this.Width = (int)registry.GetValue("WindowWidth", 893);
                    this.Height = (int)registry.GetValue("WindowHeight", 579);
                    this.viewCacheList.SetRegistryString((string)registry.GetValue("ListViewPanel", ""));
                    if ((int)registry.GetValue("Maximized", 0) == 1)
                        this.WindowState = FormWindowState.Maximized;

                    if (this.WindowState != FormWindowState.Maximized && this.Left == -32000 && this.Top == -32000)
                    {
                        this.Left = 225;
                        this.Top = 225;
                        this.Width = 893;
                        this.Height = 579;
                    }
                }
                catch { }
            }
            //      mapview.OnShow();
            FillLocations();
        }
        private void loadDockPanel(string layout)
        {
            // Read Window position out of registry
            registry = Registry.CurrentUser.OpenSubKey("Software\\Ging-Buh\\WinCachebox", RegistryKeyPermissionCheck.ReadWriteSubTree);
            if (registry != null)
            {
                try
                {
                    // DockPanel Positionen
                    string s = (string)registry.GetValue("DockPanels", "");
                    if (s == "")
                        return;
                    // s = "<?xml version=\"1.0\" encoding=\"us-ascii\"?>" + Environment.NewLine + s;
                    if (layout != "")
                        s = layout;
                    dockPanel1.SuspendLayout(true);
                    m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
                    MemoryStream stream = new MemoryStream(s.Length);
                    StreamWriter sw = new StreamWriter(stream, Encoding.ASCII);
                    sw.Write(s);
                    sw.Flush();
                    stream.Position = 0;


                    viewCacheList.DockPanel = null;
                    mapview.DockPanel = null;
                    viewDescription.DockPanel = null;
                    viewLogs.DockPanel = null;
                    viewSpoiler.DockPanel = null;
                    viewNotes.DockPanel = null;
                    viewHint.DockPanel = null;
                    viewSolver.DockPanel = null;
                    viewWaypoints.DockPanel = null;

                    dockPanel1.LoadFromXml(stream, m_deserializeDockContent);

                    sw.Close();


                    dockPanel1.ResumeLayout(true, true);

                }
                catch
                {
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string s = "alsdfjaldf asdf lasd flas flasj lfas dfals fals fas fasl f";
            Graphics graphics = this.CreateGraphics();

            StringFormat sf = new StringFormat();
            SizeF bereich = new SizeF(800, 100);
            SizeF size = graphics.MeasureString(s, this.Font, bereich, sf);

            graphics.Dispose();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // SelectedCache nochmal setzen, damit Solver und Notes gespeichert werden, falls geändert...
            Global.SelectedCache = Global.SelectedCache;
            Config.Set("MapInitLatitude", Global.LastValidPosition.Latitude);
            Config.Set("MapInitLongitude", Global.LastValidPosition.Longitude);
            Config.AcceptChanges();

            // store window positions in registry
            registry = Registry.CurrentUser.OpenSubKey("Software\\Ging-Buh\\WinCachebox", RegistryKeyPermissionCheck.ReadWriteSubTree);
            if (registry == null)
                registry = Registry.CurrentUser.CreateSubKey("Software\\Ging-Buh\\WinCachebox", RegistryKeyPermissionCheck.ReadWriteSubTree);

            registry.SetValue("WindowLeft", this.Left, RegistryValueKind.DWord);
            registry.SetValue("WindowTop", this.Top, RegistryValueKind.DWord);
            registry.SetValue("WindowWidth", this.Width, RegistryValueKind.DWord);
            registry.SetValue("WindowHeight", this.Height, RegistryValueKind.DWord);
            registry.SetValue("Maximized", this.WindowState == FormWindowState.Maximized, RegistryValueKind.DWord);
            registry.SetValue("ListViewPanel", this.viewCacheList.GetRegistryString(), RegistryValueKind.String);

            MemoryStream stream = new MemoryStream();
            dockPanel1.SaveAsXml(stream, Encoding.ASCII, true);
            stream.Position = 0;
            StreamReader reader = new StreamReader(stream);
            string s = reader.ReadToEnd();
            registry.SetValue("DockPanels", s, RegistryValueKind.String);
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Views.AboutForm about = new AboutForm();
            about.ShowDialog();
        }

        private void settingsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Views.Forms.Settings settings = new Views.Forms.Settings();
            settings.ShowDialog();
        }

        private void repairDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Database.Data.Repair();
        }

        private void cCompactDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Database.Data.Compact();
        }

        private void filterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FilterForm filter = new FilterForm(Global.LastFilter);
            if (filter.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Global.LastFilter = filter.getFilter();
                ApplyFilter(Global.LastFilter);
                //                CacheListView.View.fillCacheList();
                Config.Set("Filter", Global.LastFilter.ToString());
            }
        }

        private void gPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportSettingsForm formImportSettings = new ImportSettingsForm();
            if (formImportSettings.ShowDialog() == DialogResult.OK)
            {
                FormImportPocketQuery fipq = new FormImportPocketQuery();
                fipq.ShowDialog();
                ApplyFilter(Global.LastFilter);
            }
        }

        private void cacheWolfImportToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            CacheWolfImport form = new CacheWolfImport();
            form.ShowDialog();
            ApplyFilter(Global.LastFilter);
        }

        private void sDFExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SdfExport.SdfExportSetting exs = new SdfExport.SdfExportSetting();
            List<SdfExport.SdfExportSetting> list = new List<SdfExport.SdfExportSetting>();
            list.Add(exs);
            exs.SaveToDatabase = false;
            if (exs.Edit())
            {
                SdfExport.SdfExport exp = new SdfExport.SdfExport(list);
                exp.ShowDialog();
            }
        }

        public void RC4(ref Byte[] bytes, Byte[] key)
        {
            Byte[] s = new Byte[256];
            Byte[] k = new Byte[256];
            Byte temp;
            int i, j;

            for (i = 0; i < 256; i++)
            {
                s[i] = (Byte)i;
                k[i] = key[i % key.GetLength(0)];
            }

            j = 0;
            for (i = 0; i < 256; i++)
            {
                j = (j + s[i] + k[i]) % 256;
                temp = s[i];
                s[i] = s[j];
                s[j] = temp;
            }

            i = j = 0;
            for (int x = 0; x < bytes.GetLength(0); x++)
            {
                i = (i + 1) % 256;
                j = (j + s[i]) % 256;
                temp = s[i];
                s[i] = s[j];
                s[j] = temp;
                int t = (s[i] + s[j]) % 256;
                bytes[x] ^= s[t];
            }
        }
        private byte[] StringToByteArray(string str)
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            return enc.GetBytes(str);
        }

        private string ByteArrayToString(byte[] arr)
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            return enc.GetString(arr);
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Byte[] Key = { 25, 64, 1, 45, 54, 45 };
            string s = "Mein Passwort";

            byte[] b = StringToByteArray(s);
            RC4(ref b, Key);
            string encrypted = Convert.ToBase64String(b);

            b = Convert.FromBase64String(encrypted);
            RC4(ref b, Key);
            string decrypted = ByteArrayToString(b);

            this.Close();
        }

        private void miMapLayer_DropDownOpening(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem tsi in miMapLayer.DropDownItems)
            {
                Layer layer = tsi.Tag as Layer;
                if (layer == null)
                    continue;
                tsi.Checked = layer == MapView.View.CurrentLayer;
            }
        }

        private void deleteFilterSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Global.Translations.Get("f34", "You are about to delete all caches which currently match your filter settings. Are you sure you want to proceed?"), Global.Translations.Get("f35", "Delete caches"), MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                Views.Forms.RemoveArchivedForm rtf = new Views.Forms.RemoveArchivedForm();
                rtf.ShowDialog();
                Database.Data.GPXFilenameUpdateCacheCount();
                //Reset filter as the last one used will return 0 results
                Global.LastFilter = new FilterProperties(FilterPresets.presets[0]);
                Config.Set("Filter", Global.LastFilter.ToString());
                Config.AcceptChanges();
                ApplyFilter(Global.LastFilter);
            }
        }

        private void archiveFilterselectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Global.Translations.Get("f36", "You are about to mark all caches which currently match your filter settings as 'archived'. Are you sure you want to proceed?"), Global.Translations.Get("f37", "Mark caches"), MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                string where = Global.LastFilter.SqlWhere;
                Cursor.Current = Cursors.WaitCursor;
                CBCommand command = Database.Data.CreateCommand("update Caches set Archived=@true, Available=@false where " + where);
                command.ParametersAdd("@true", DbType.Boolean, true);
                command.ParametersAdd("@false", DbType.Boolean, false);
                command.ExecuteNonQuery();
                command.Dispose();
                Cursor.Current = Cursors.Default;
                ApplyFilter(Global.LastFilter);
            }
        }

        private void FillLocations()
        {
            dbLocations.DropDownItems.Clear();
            WinCachebox.Geocaching.Location.LoadLocations();

            // Eintrag für aktuelle Position (Marker)
            ToolStripItem it = dbLocations.DropDownItems.Add("Act. Position", null, onLocationClick);
            it.Text = Global.Translations.Get("f26", "Act. Location");
            dbLocations.Text = it.Text;

            it.Tag = null;
            foreach (Location location in Geocaching.Location.Locations)
            {
                it = dbLocations.DropDownItems.Add(location.Name, null, onLocationClick);
                it.Tag = location;
            }
        }

        private void onLocationClick(object sender, EventArgs e)
        {
            ToolStripItem it = sender as ToolStripItem;
            if (it == null)
                return;
            Location location = it.Tag as Location;
            if (location == null)
            {
                dbLocations.Text = it.Text;
            }
            else
            {
                dbLocations.Text = it.Text;
                Global.SetMarker(location.Coordinate);
            }
        }

        private void dbLocations_DropDownOpening(object sender, EventArgs e)
        {
            FillLocations();
        }

        private void locationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Views.Forms.LocationsForm.Edit();
        }

        private void batchSDFExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SdfExport.SDFBatchExport.Export();
        }

        private void resetListingChangedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Database.Data.ExecuteStatement("Update Caches set ListingChanged=0 where ListingChanged=1");
            Global.LastFilter = new FilterProperties(FilterPresets.presets[0]);
            ApplyFilter(Global.LastFilter);
            Config.Set("Filter", Global.LastFilter.ToString());
            Cursor.Current = Cursors.Default;
        }

        private void resetLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dockPanel1.SuspendLayout();
            viewCacheList.DockPanel = null;
            mapview.DockPanel = null;
            viewDescription.DockPanel = null;
            viewLogs.DockPanel = null;
            viewSpoiler.DockPanel = null;
            viewNotes.DockPanel = null;
            viewHint.DockPanel = null;
            viewSolver.DockPanel = null;
            viewWaypoints.DockPanel = null;

            viewCacheList.Show(dockPanel1, DockState.Document);
            mapview.Show(viewCacheList.Pane, null);
            viewDescription.Show(viewCacheList.Pane, DockAlignment.Right, 0.5);
            viewLogs.Show(viewDescription.Pane, null);
            viewSpoiler.Show(viewDescription.Pane, null);
            viewNotes.Show(viewDescription.Pane, null);
            viewHint.Show(viewDescription.Pane, null);
            viewSolver.Show(viewDescription.Pane, null);
            viewWaypoints.Show(viewCacheList.Pane, DockAlignment.Bottom, 0.2);
            viewCacheList.Show();
            viewDescription.Show();
            dockPanel1.ResumeLayout();
        }

        private void importUserDataFromSDFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ofdUserData.Filter = "Android DB|*.db3|CacheBox DB|*.sdf";
            if (Global.databaseName.EndsWith("sdf"))
            {
                ofdUserData.DefaultExt = "sdf";
                ofdUserData.FilterIndex = 1;
            }
            else
            {
                ofdUserData.DefaultExt = "db3";
                ofdUserData.FilterIndex = 2;
            }
            if (ofdUserData.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.Cursor = Cursors.WaitCursor;
                Database dbImport = new Database();
                dbImport.Startup(ofdUserData.FileName, true);

                List<Cache> cCaches = new List<Cache>();
                Cache.LoadCaches("", cCaches);

                List<Cache> cImport = new List<Cache>();

                int MaxDistance = 0;
                try { MaxDistance = Convert.ToInt32(txtDistance.Text); }
                catch (Exception) { }
                Cache.LoadCaches("HasUserData=1", cImport, dbImport, true, MaxDistance);
                int changes = 0;
                bool cancel = false;
                foreach (Cache cache in cImport)
                {
                    foreach (Cache orgCache in cCaches)
                    {
                        if (orgCache.GcId == cache.GcId)
                        {
                            bool changed = false;
                            Database org = Database.Data;
                            Database.Data = dbImport;
                            string solver = cache.Solver;
                            string note = cache.Note;
                            Database.Data = org;
                            string orgSolver = orgCache.Solver;
                            if (solver != orgSolver)
                            {
                                if (orgSolver != "")
                                {
                                    ChangeTypeEnum changeTyp = ChangeTypeEnum.SolverText;
                                    ReplicationConflictForm.ReplicationConflictResult repResult = ReplicationConflictForm.ShowConflict(cache.Name, changeTyp, orgSolver, solver, false);
                                    if (repResult == ReplicationConflictForm.ReplicationConflictResult.DoNotSolve)
                                    {
                                        cancel = true;
                                        break;
                                    }
                                    if (repResult == ReplicationConflictForm.ReplicationConflictResult.UseCopy)
                                    {
                                        orgCache.Solver = solver;
                                        changed = true;
                                    }
                                }
                                else
                                {
                                    orgCache.Solver = solver;
                                    changed = true;
                                }
                            }
                            string orgNote = orgCache.Note;
                            if (note != orgNote)
                            {
                                if (orgNote != "")
                                {
                                    ChangeTypeEnum changeTyp = ChangeTypeEnum.NotesText;
                                    ReplicationConflictForm.ReplicationConflictResult repResult = ReplicationConflictForm.ShowConflict(cache.Name, changeTyp, orgNote, note, false);
                                    if (repResult == ReplicationConflictForm.ReplicationConflictResult.DoNotSolve)
                                    {
                                        cancel = true;
                                        break;
                                    }
                                    if (repResult == ReplicationConflictForm.ReplicationConflictResult.UseCopy)
                                    {
                                        orgCache.Note = note;
                                        changed = true;
                                    }
                                }
                                else
                                {
                                    orgCache.Note = note;
                                    changed = true;
                                }
                            }
                            foreach (Waypoint waypoint in cache.Waypoints)
                            {
                                Waypoint orgWaypoint = orgCache.GetWaypointByGcCode(waypoint.GcCode);
                                if (orgWaypoint == null)
                                {
                                    // dieser Waypoint ist noch nicht drin -> Importieren
                                    orgCache.Waypoints.Add(waypoint);
                                    waypoint.CacheId = orgCache.Id;
                                    waypoint.WriteToDatabase();
                                    changed = true;
                                }
                                else
                                {
                                    string swaypoint = waypoint.CreateConflictString();
                                    string sorgWaypoint = orgWaypoint.CreateConflictString();
                                    if (swaypoint != sorgWaypoint)
                                    {
                                        ChangeTypeEnum changeTyp = ChangeTypeEnum.WaypointChanged;
                                        ReplicationConflictForm.ReplicationConflictResult repResult = ReplicationConflictForm.ShowConflict(cache.Name, changeTyp, sorgWaypoint, swaypoint, false);
                                        if (repResult == ReplicationConflictForm.ReplicationConflictResult.DoNotSolve)
                                        {
                                            cancel = true;
                                            break;
                                        }
                                        if (repResult == ReplicationConflictForm.ReplicationConflictResult.UseCopy)
                                        {
                                            waypoint.UpdateDatabase();
                                            changed = true;
                                        }
                                    }
                                }
                            }
                            if (changed)
                                changes++;
                            break;
                        }
                    }
                    if (cancel)
                        break;
                }
                dbImport.Dispose();
                ApplyFilter(Global.LastFilter);
                this.Cursor = Cursors.Default;
                MessageBox.Show(changes.ToString() + Global.Translations.Get("f29", " Cache(s) changed successful!"));
            }
        }

        private void updateCacheStatusFilterSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Api.GetCacheStatus.UpdateCacheStatus();
        }

        private void resetFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            FilterProperties fp = new FilterProperties(FilterPresets.presets[0]);
            if (!Global.LastFilter.ToString().Equals(fp.ToString()))
            {
                Global.LastFilter = fp;
                ApplyFilter(Global.LastFilter);
                Config.Set("Filter", Global.LastFilter.ToString());
            }
            Cursor.Current = Cursors.Default;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (toolStripButton1.Text.Equals(Global.Translations.Get("f28", "Reset Filter")))
            {
                resetFilterToolStripMenuItem_Click(sender, e);
            }
            else
            {
                Cursor.Current = Cursors.WaitCursor;
                Global.LastFilter = Global.PreviousFilter;
                ApplyFilter(Global.LastFilter);
                Config.Set("Filter", Global.LastFilter.ToString());
                Cursor.Current = Cursors.Default;
            }
        }

        private void resetImagesUpdatedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Database.Data.ExecuteStatement("Update Caches set ImagesUpdated=0 where ImagesUpdated=1");
            Database.Data.ExecuteStatement("Update Caches set DescriptionImagesUpdated=0 where DescriptionImagesUpdated=1");
            Global.LastFilter = new FilterProperties(FilterPresets.presets[0]);
            ApplyFilter(Global.LastFilter);
            Config.Set("Filter", Global.LastFilter.ToString());
            Cursor.Current = Cursors.Default;
        }

        private void databaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            resetListingChangedToolStripMenuItem.Text = Global.Translations.Get("reset")
                 + " \"" + Global.Translations.Get("listing")
                 + " " + Global.Translations.Get("changed") + "\""
                ;
            resetImagesUpdatedToolStripMenuItem.Text = Global.Translations.Get("reset")
                 + " \"" + Global.Translations.Get("spoiler")
                 + " " + Global.Translations.Get("changed") + "\""
                ;
        }

        private void txtDistance_TextChanged(object sender, EventArgs e)
        {
            string texto = "";
            foreach (char c in txtDistance.Text.ToCharArray())
            {
                if (char.IsNumber(c))
                    texto += c;
            }
            txtDistance.Text = texto;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ApplyFilter(Global.LastFilter);

            Config.Set("DistanceFilter", txtDistance.Text);
            Cursor.Current = Cursors.Default;
        }

        private void OpenDB_Clicked(object sender, EventArgs e)
        {
            Views.Forms.OpenDB openDBForm = new Views.Forms.OpenDB();
            if (openDBForm.ShowDialog() == DialogResult.OK)
            {
                string DBName = openDBForm.SelectedDB.SubItems[0].Text;
                string DBType = openDBForm.SelectedDB.SubItems[1].Text;
                bool useOwnRepository = openDBForm.SelectedDB.SubItems[2].Text.Equals("true");
                Database.Data.ResetConnection();
                OpenDataBase(Path.GetDirectoryName(Global.databaseName), DBName, DBType, useOwnRepository);
                SetTitle(useOwnRepository);
                // TODO check if to change maps
                LoadCategories();
                Cache.LoadCaches(Global.LastFilter.SqlWhere);
                viewCacheList.fillCacheList();
            }
        }
    }
}
