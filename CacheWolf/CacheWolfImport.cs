using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using WinCachebox.Geocaching;



namespace WinCachebox.CacheWolf
{
    public partial class CacheWolfImport : Form
    {
        private bool finalToCache = false;
        Dictionary<String, int> logTypen = new Dictionary<string, int>();
        private string cacheWolfDaten = Config.GetString("CacheWolfPath");
        List<cwCache> cwCaches = new List<cwCache>();
        SortedList<string, int> aktCaches = new SortedList<string, int>();
        SortedList<string, long> gpxFilenames = new SortedList<string, long>();
        SortedList<string, DateTime> gpxFilenameDatum = new SortedList<string, DateTime>();
        SortedList<string, DateTime> gpxLastImportDatum = new SortedList<string, DateTime>();
        public string Fehler = "";
        public int erfolgreich = 0;
        public int fehlerhaft = 0;
        DateTime indexDatum;
        DateTime aktGPXDatum;

        public CacheWolfImport()
        {
            InitializeComponent();
            this.Text = Global.Translations.Get("c0","CacheWolfImport");
            label4.Text = Global.Translations.Get("c1","Select CacheWolf Profile/s:");
            button1.Text = "&" + Global.Translations.Get("c2", "Import");
            button2.Text = "&" + Global.Translations.Get("cancel", "Cancel");
            button3.Text = "&" + Global.Translations.Get("ok", "Ok");
            button4.Text = "&" + Global.Translations.Get("c5", "Change");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            aktCaches.Clear();

            listBox1.Items.Clear();
            int geändert = 0;
            int neu = 0;
            // alle GPXFilenames auslesen
            gpxFilenames.Clear();
            CBCommand selectCommand = Database.Data.CreateCommand("select GPXFilename, ID, Imported from GPXFilenames");
            DbDataReader reader = selectCommand.ExecuteReader();
            selectCommand.Dispose();
            while (reader.Read())
            {
                string gpxFilename = reader.GetString(0);
                long id = reader.GetInt64(1);
                DateTime date = reader.GetDateTime(2);
                if (!gpxFilenames.ContainsKey(gpxFilename.ToLower()))
                    gpxFilenames.Add(gpxFilename.ToLower(), id);
                if (!gpxFilenameDatum.ContainsKey(gpxFilename.ToLower()))
                    gpxFilenameDatum.Add(gpxFilename.ToLower(), date);
                if (!gpxLastImportDatum.ContainsKey(gpxFilename.ToLower()))
                    gpxLastImportDatum.Add(gpxFilename.ToLower(), date);
                else
                    if (date > gpxLastImportDatum[gpxFilename.ToLower()])
                        gpxLastImportDatum[gpxFilename.ToLower()] = date;
            }
            reader.Close();
            // alle Caches auslesen
            selectCommand = Database.Data.CreateCommand("Select GcCode, listingCheckSum from Caches");
            reader = selectCommand.ExecuteReader();
            selectCommand.Dispose();
            while (reader.Read())
            {
                string gccode = reader.GetString(0);
                int listingCheckSum = reader.GetInt32(1);
                aktCaches.Add(gccode, listingCheckSum);
            }
            reader.Close();
            int maxLogID = 0;
            try
            {
                selectCommand = Database.Data.CreateCommand("Select max(ID) from Logs");
                maxLogID = Convert.ToInt32(selectCommand.ExecuteScalar().ToString());
                selectCommand.Dispose();
            }
            catch (Exception)
            {
                maxLogID = 1;
            }

            DbTransaction transaction = Database.Data.StartTransaction();
            try
            {
                foreach (string kategorie in cbProfile.Items)
                {
                    if (cbProfile.Text != Global.Translations.Get("c6","_All Profiles"))
                    {
                        if (kategorie != cbProfile.Text)
                            continue;
                    }
                    else
                    {
                        // der Erste Eintrag "Alle Profile" ist keine Kategorie zum Einlesen...
                        if (kategorie == Global.Translations.Get("c6","_All Profiles"))
                            continue;
                    }
                    
                    long GPXFilename_ID = 0;
                    Category category = Global.Categories.GetCategory(kategorie);
                    if (category == null)
                        continue;   // should not happen!!!

                    label3.Text = Global.Translations.Get("c7", "category: ") + kategorie + Global.Translations.Get("c8", " (new)");
                    listBox1.Items.Insert(0, Global.Translations.Get("c7", "category: ") + kategorie + Global.Translations.Get("c8", " (new)"));
                    selectCommand = Database.Data.CreateCommand("insert into GPXFilenames(GPXFilename, Imported, CategoryId) values (@GPXFilename, @Imported, @CategoryId)");
                    selectCommand.ParametersAdd("@GPXFilename", DbType.String, kategorie);
                    selectCommand.ParametersAdd("@Imported", DbType.DateTime, DateTime.Now);
                    selectCommand.ParametersAdd("@CategoryId", DbType.Int64, category.Id);
                    selectCommand.ExecuteNonQuery();
                    selectCommand.Dispose();

                    selectCommand = Database.Data.CreateCommand("Select max(ID) from GPXFilenames");
                    GPXFilename_ID = Convert.ToInt32(selectCommand.ExecuteScalar().ToString());
                    selectCommand.Dispose();

                    if (gpxLastImportDatum.ContainsKey(kategorie.ToLower()))
                    {
                        // nur seit dem letzten Import geänderte Cache einlesen
                        aktGPXDatum = gpxLastImportDatum[kategorie.ToLower()];
                    }
                    else
                    {
                        // alle Caches importieren (oder die Abfrage auf geändert überspringen)
                        aktGPXDatum = new DateTime(2000, 1, 1);
                    }
                    Application.DoEvents();

                    import(kategorie);
                    // cwCache ist gefüllt...

                    int nummer = 0;
                    progressBar1.Value = 0;
                    progressBar1.Maximum = 100;
                    foreach (cwCache cwCache in cwCaches)
                    {
                        nummer++;
                        double percent = nummer * 100 / cwCaches.Count;
                        if (Math.Round(percent) != progressBar1.Value)
                            progressBar1.Value = (int)Math.Round(percent);

                        label1.Text = Global.Translations.Get("c9", "Caches importieren");
                        label2.Text = nummer.ToString() + " - " + cwCaches.Count.ToString() + " - " + cwCache.GetName();

                        Application.DoEvents();
                        if (!cwCache.Einlesen())
                        {
                            listBox1.Items.Insert(1, "ERROR 1: " + nummer.ToString() + " - " + cwCaches.Count.ToString() + " - " + cwCache.GetName());
                            continue;
                        }
                                                
                        CBCommand command;
                        int listingCheckSum = 0;
                        if (aktCaches.ContainsKey(cwCache.wayp))
                        {
                            // Update
                            command = Database.Data.CreateCommand("update Caches set GcCode=@gccode, Latitude=@latitude, Longitude=@longitude, Name=@name, Size=@size, Difficulty=@difficulty, Terrain=@terrain, Archived=@archived, Available=@available, Found=@found, Type=@type, PlacedBy=@placedby, Owner=@owner, DateHidden=@datehidden, Hint=@hint, Description=@description, Country=@country, State=@state, Url=@url, NumTravelbugs=@numtravelbugs, GcId=@gcid, AttributesPositive=@AttributesPositive, AttributesNegative=@AttributesNegative, AttributesPositiveHigh=@AttributesPositiveHigh, AttributesNegativeHigh=@AttributesNegativeHigh, TourName=@TourName, GPXFilename_ID=@GPXFilename_ID, ListingCheckSum=@ListingCheckSum, ListingChanged=@ListingChanged, ImagesUpdated=@ImagesUpdated, DescriptionImagesUpdated=@DescriptionImagesUpdated, CorrectedCoordinates=@CorrectedCoordinates, FavPoints=@FavPoints where Id=@id");
                            listBox1.Items.Insert(1, "U: " + nummer.ToString() + " - " + cwCache.GetName());
                            geändert++;
                            listingCheckSum = aktCaches[cwCache.wayp];
                        }
                        else
                        {
                            // Neu
                            command = Database.Data.CreateCommand("insert into Caches(Id, GcCode, Latitude, Longitude, Name, Size, Difficulty, Terrain, Archived, Available, Found, Type, PlacedBy, Owner, DateHidden, Hint, Description, Country, State, Url, NumTravelbugs, GcId, Rating, Vote, VotePending, Favorit, AttributesPositive, AttributesNegative, AttributesPositiveHigh, AttributesNegativeHigh, TourName, GPXFilename_ID, HasUserData, ListingCheckSum, ListingChanged, ImagesUpdated, DescriptionImagesUpdated, CorrectedCoordinates, FirstImported, FavPoints) values(@id, @gccode, @latitude, @longitude, @name, @size, @difficulty, @terrain, @archived, @available, @found, @type, @placedby, @owner, @datehidden, @hint, @description, @country, @state, @url, @numtravelbugs, @gcid, @rating, @vote, @votepending, @Favorit, @AttributesPositive, @AttributesNegative, @AttributesPositiveHigh, @AttributesNegativeHigh, @TourName, @GPXFilename_ID, @HasUserData, @ListingCheckSum, @ListingChanged, @ImagesUpdated, @DescriptionImagesUpdated, @CorrectedCoordinates, @FirstImported, @FavPoints)");
                            listBox1.Items.Insert(1, "N: " + nummer.ToString() + " - " + cwCache.GetName());
                            neu++;
                        }
                        // FavPoints
                        command.ParametersAdd("FavPoints", DbType.Int16, cwCache.num_recommended);
                        // FirstImported
                        command.ParametersAdd("FirstImported", DbType.DateTime, DateTime.Now);
                        // Id,
                        command.ParametersAdd("@id", DbType.Int64, cwCache.GetID());
                        // GcCode, 
                        command.ParametersAdd("@gccode", DbType.String, cwCache.wayp);
                        // Latitude, 
                        command.ParametersAdd("@latitude", DbType.Double, cwCache.GetLat(finalToCache));
                        // Longitude, 
                        command.ParametersAdd("@longitude", DbType.Double, cwCache.GetLon(finalToCache));
                        // Name, 
                        command.ParametersAdd("@name", DbType.String, cwCache.GetName());
                        // GcId, 
                        command.ParametersAdd("@gcid", DbType.String, cwCache.wayp);
                        // Size, 
                        command.ParametersAdd("@size", DbType.Int64, cwCache.GetSize());
                        // Difficulty,
                        command.ParametersAdd("@difficulty", DbType.Int16, cwCache.GetDifficulty());
                        // Terrain, 
                        command.ParametersAdd("@terrain", DbType.Int16, cwCache.GetTerrain());
                        // Type, 
                        command.ParametersAdd("@type", DbType.Int16, cwCache.GetTyp());
                        // Archived, 
                        command.ParametersAdd("@archived", DbType.Boolean, cwCache.GetBool(2));
                        // Available, 
                        command.ParametersAdd("@available", DbType.Boolean, cwCache.GetBool(1));
                        // Found, 
                        command.ParametersAdd("@found", DbType.Boolean, cwCache.GetBool(6));
                        // Type, 
                        //command.ParametersAdd("@type", DbType.Boolean, cwCache.GetTyp());
                        // PlacedBy, 
                        command.ParametersAdd("@placedby", DbType.String, HtmlTools.StripHTML(HttpUtility.HtmlDecode(cwCache.owner)));
                        // Owner, 
                        command.ParametersAdd("@owner", DbType.String, HtmlTools.StripHTML(HttpUtility.HtmlDecode(cwCache.owner)));
                        // DateHidden, 
                        DateTime dateHidden = new DateTime();
                        try
                        {
                            dateHidden = DateTime.Parse(cwCache.hidden);
                        }
                        catch (Exception)
                        {
                        }
                        command.ParametersAdd("@datehidden", DbType.DateTime, dateHidden);
                        // Hint,
                        String tmp = cwCache.hints;
                        tmp = tmp.Replace("<br>", Environment.NewLine);
                        command.ParametersAdd("@hint", DbType.String, tmp);
                        // Description, 
                        string chtml = cwCache.html;
                        command.ParametersAdd("@description", DbType.String, chtml);
                        command.ParametersAdd("@country", DbType.String, cwCache.country);
                        command.ParametersAdd("@state", DbType.String, cwCache.state);
                        // Url, 
                        command.ParametersAdd("@url", DbType.String, cwCache.url);
                        // NumTravelbugs, 
                        command.ParametersAdd("@numtravelbugs", DbType.Int16, 0);
                        // Rating, 
                        command.ParametersAdd("@rating", DbType.Single, 0);
                        // Vote, 
                        command.ParametersAdd("@vote", DbType.Int16, 0);
                        // VotePending, 
                        command.ParametersAdd("@votepending", DbType.Boolean, false);
                        // Favorit, 
                        command.ParametersAdd("@Favorit", DbType.Boolean, false);

                        long attributesLongPositive = 0;
                        long attributesLongNegative = 0;
                        long attributesLongPositiveHigh = 0;
                        long attributesLongNegativeHigh = 0;
                        // todo CW-Attribute -> CB-Attribute
                        getAttributes(cwCache.attributesYes, cwCache.attributesNo, cwCache.attributesYes1, cwCache.attributesNo1, ref attributesLongPositive, ref attributesLongNegative, ref attributesLongPositiveHigh, ref attributesLongNegativeHigh);
                        // AttributesPositive, 
                        command.ParametersAdd("@AttributesPositive", DbType.Int64, attributesLongPositive);
                        // AttributesNegative, 
                        command.ParametersAdd("@AttributesNegative", DbType.Int64, attributesLongNegative);
                        // AttributesPositive, 
                        command.ParametersAdd("@AttributesPositiveHigh", DbType.Int64, attributesLongPositiveHigh);
                        // AttributesNegative, 
                        command.ParametersAdd("@AttributesNegativeHigh", DbType.Int64, attributesLongNegativeHigh);
                        // TourName, 
                        command.ParametersAdd("@TourName", DbType.String, "");
                        // GPXFilename_ID, 
                        command.ParametersAdd("@GPXFilename_ID", DbType.Int64, GPXFilename_ID);
                        // HasUserData, 
                        command.ParametersAdd("@HasUserData", DbType.Boolean, false);
                        // ListingCheckSum, 
                        string recentOwnerLogString = cwCache.recentOwnerLogString();
                        string stringForListingCheckSum = chtml;
                        int newListingCheckSum = (int)(Global.sdbm(stringForListingCheckSum) + Global.sdbm(recentOwnerLogString));
                        command.ParametersAdd("@ListingCheckSum", DbType.Int32, newListingCheckSum);
                        // ListingChanged, 
                        bool listingUpdated = false;
                        if (listingCheckSum != newListingCheckSum)
                            listingUpdated = true;
                        command.ParametersAdd("@ListingChanged", DbType.Boolean, listingUpdated);
                        // ImagesUpdated, 
                        command.ParametersAdd("@ImagesUpdated", DbType.Boolean, true);   // wird auf true gesetzt, da Bilder von CacheWolf mit importiert werden
                        // DescriptionImagesUpdated
                        command.ParametersAdd("@DescriptionImagesUpdated", DbType.Boolean, true);   // wird auf true gesetzt, da Bilder von CacheWolf mit importiert werden
                        // CorrectedCoordinates
                        if (cwCache.status.Equals("gelöst") || cwCache.GetBool(15))
                        {
                            command.ParametersAdd("@CorrectedCoordinates", DbType.Boolean, true);
                        }
                        else {
                            command.ParametersAdd("@CorrectedCoordinates", DbType.Boolean, false);
                            // command.ParametersAdd("@CorrectedCoordinates", DbType.Boolean, cwCache.GetFinalWaypoint() != null);
                        }
                        try
                        {
                            command.ExecuteNonQuery();
                            command.Dispose();
                        }
                        catch (Exception)
                        {
                            continue;
                        }

                        // update Solver only when empty
                        // read old solver value
                        command = Database.Data.CreateCommand("select Solver from Caches where Id=@id");
                        command.ParametersAdd("@id", DbType.Int64, cwCache.GetID());
                        String resultString = command.ExecuteScalar().ToString();
                        command.Dispose();
                        if ((cwCache.solver != "") && (resultString == "") && (cwCache.solver != resultString))
                        {
                            command = Database.Data.CreateCommand("update Caches set Solver=@solver where Id=@id");
                            command.ParametersAdd("@id", DbType.Int64, cwCache.GetID());
                            command.ParametersAdd("@solver", DbType.String, cwCache.solver);
                            command.ExecuteNonQuery();
                            command.Dispose();
                        }
                        // update Notes only when empty
                        // read old Notes value
                        command = Database.Data.CreateCommand("select Notes from Caches where Id=@id");
                        command.ParametersAdd("@id", DbType.Int64, cwCache.GetID());
                        resultString = command.ExecuteScalar().ToString();
                        command.Dispose();
                        if ((cwCache.notes != "") && (resultString == "") && (cwCache.notes != resultString))
                        {
                            command = Database.Data.CreateCommand("update Caches set Notes=@notes where Id=@id");
                            command.ParametersAdd("@id", DbType.Int64, cwCache.GetID());
                            command.ParametersAdd("@notes", DbType.String, cwCache.notes);
                            command.ExecuteNonQuery();
                            command.Dispose();
                        }

                        // Bilder
                        // vorherige Bilder löschen
                        command = Database.Data.CreateCommand("delete from Images where cacheid=@cacheid");
                        command.ParametersAdd("@cacheid", DbType.Int64, cwCache.GetID());
                        command.ExecuteNonQuery();
                        command.Dispose();

                        //0        Id               integer            1                            1    
                        //1        CacheId          bigint             0                            0    
                        //2        GcCode           nvarchar (15)      0                            0    
                        //3        Description      ntext              0                            0    
                        //4        Name             nvarchar (255)     0                            0    
                        //5        ImageUrl         nvarchar (255)     0                            0    
                        //6        IsCacheImage     bit                0                            0    
                        foreach (cwCacheImage image in cwCache.images)
                        {
                            if (File.Exists(cacheWolfDaten + "\\" + kategorie + "\\" + image.file))
                            {
                                if (!image.url.StartsWith("http"))
                                {
                                    image.url = "http://www.geocaching.com" + image.url;
                                }
                                if (chtml.Contains(image.url) || image.imText)
                                {
                                    // Dies ist ein Image in der Beschreibung des Caches
                                    image.imText = true;
                                    try
                                    {                                    
                                        string destination = Geocaching.DescriptionImageGrabber.BuildImageFilename(cwCache.wayp, new Uri(image.url));
                                        command = Database.Data.CreateCommand("insert into Images(CacheId, GcCode, Description, Name, ImageUrl, IsCacheImage) values (@cacheid, @gccode, @description, @name, @imageurl, @iscacheimage)");
                                        command.ParametersAdd("@cacheid", DbType.Int64, cwCache.GetID());
                                        command.ParametersAdd("@gccode", DbType.String, cwCache.wayp);
                                        String sep = "\n";
                                        if (image.imgtext.Length == 0)
                                            sep = "";
                                        command.ParametersAdd("@description", DbType.String, image.imgtext + sep + image.description);
                                        command.ParametersAdd("@name", DbType.String, destination);
                                        command.ParametersAdd("@imageurl", DbType.String, image.url);
                                        command.ParametersAdd("@iscacheimage", DbType.Boolean, true);
                                        command.ExecuteNonQuery();
                                        command.Dispose();
                                        if (!File.Exists(destination))
                                        {
                                            if (!Directory.Exists(Path.GetDirectoryName(destination)))
                                                Directory.CreateDirectory(Path.GetDirectoryName(destination));
                                            try
                                            {
                                                File.Copy(cacheWolfDaten + "\\" + kategorie + "\\" + image.file, destination);
                                            }
                                            catch (Exception)
                                            {
                                                Global.AddLog("Error copying image " + image.file);
                                            }
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        Global.AddLog("Error copying image " + image.file);
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        // alle anderen Images in den Spoiler-Ordner kopieren
                                        String destination = Geocaching.DescriptionImageGrabber.BuildAdditionalImageFilename(cwCache.wayp, image.imgtext, new Uri(image.url));
                                        command = Database.Data.CreateCommand("insert into Images(CacheId, GcCode, Description, Name, ImageUrl, IsCacheImage) values (@cacheid, @gccode, @description, @name, @imageurl, @iscacheimage)");
                                        command.ParametersAdd("@cacheid", DbType.Int64, cwCache.GetID());
                                        command.ParametersAdd("@gccode", DbType.String, cwCache.wayp);
                                        String sep = "\n";
                                        if (image.imgtext.Length == 0)
                                            sep = "";
                                        command.ParametersAdd("@description", DbType.String, image.imgtext + sep + image.description);
                                        command.ParametersAdd("@name", DbType.String, destination);
                                        command.ParametersAdd("@imageurl", DbType.String, image.url);
                                        command.ParametersAdd("@iscacheimage", DbType.Boolean, false);
                                        command.ExecuteNonQuery();
                                        command.Dispose();
                                        if (!File.Exists(destination))
                                        {
                                            if (!Directory.Exists(Path.GetDirectoryName(destination)))
                                                Directory.CreateDirectory(Path.GetDirectoryName(destination));
                                            try
                                            {
                                                File.Copy(cacheWolfDaten + "\\" + kategorie + "\\" + image.file, destination);
                                            }
                                            catch (Exception)
                                            {
                                                Global.AddLog("Error copying image " + image.file);
                                            }
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        Global.AddLog("Error copying spoiler " + image.file);
                                    }
                                }
                            }

                        }

                        // vorherige Logs löschen
                        command = Database.Data.CreateCommand("delete from Logs where cacheid=@cacheid");
                        command.ParametersAdd("@cacheid", DbType.Int64, cwCache.GetID());
                        command.ExecuteNonQuery();
                        command.Dispose();
                        // LOGS importieren
                        //<img src='icon_smile.gif'>&nbsp;2010-11-06 by mimiundmau<br>Heute im MÃ¼nchner Norden / Osten mit F-Maxi unterwegs.Irgendwie und SOWIESO muÃŸte diese Dose mit.<br>TFTC-Mimiundmau(M+M)
                        int anzLogs = 0;
                        foreach (string log in cwCache.logs)
                        {
                            //              if (anzLogs >= 10)
                            //                break;  // nur die letzten 10 logs importieren...
                            anzLogs++;
                            string slogType = "";
                            string sDatum = "";
                            string sFinder = "";
                            string sText = "";
                            Regex regLog = new Regex("logID=\"((?s).*?)\" finderID=\"((?s).*?)\" <!\\[CDATA\\[<img src='(icon_|)((?s).*?)(.gif|.png)'>&nbsp;((?s).*?)\\s*by\\s*((?s).*?)<br>((?s).*?)\\]\\]>");
                            Match matchLog = regLog.Match(log);
                            if (!matchLog.Success)
                                continue;
                            string slogId = matchLog.Groups[1].Value;
                            string sfinderId = matchLog.Groups[2].Value;
                            slogType = matchLog.Groups[4].Value;
                            sDatum = matchLog.Groups[6].Value;
                            sFinder = matchLog.Groups[7].Value;
                            sText = matchLog.Groups[8].Value;
                            if (sText.Length > 3999)
                                sText = sText.Substring(0, 3998);
                            int logType = 3;
                            if (logTypen.ContainsKey(slogType))
                                logType = logTypen[slogType];
                            else
                                logType = 2;
                            try 
                            {
                                command = Database.Data.CreateCommand("insert into Logs(CacheId, Timestamp, Finder, Type, Comment, id) values (@cacheid, @timestamp, @finder, @type, @comment, @id)");
                                command.ParametersAdd("@cacheid", DbType.Int64, cwCache.GetID());
                                command.ParametersAdd("@id", DbType.Int64, Convert.ToInt32(slogId));
                                command.ParametersAdd("@timestamp", DbType.DateTime, DateTime.Parse(sDatum));
                                command.ParametersAdd("@finder", DbType.String, HttpUtility.HtmlDecode(sFinder));
                                command.ParametersAdd("@type", DbType.Int16, logType);
                                sText = sText.Replace("<p>", Environment.NewLine);
                                sText = sText.Replace("<br>", Environment.NewLine);
                                sText = HtmlTools.StripHTML(HttpUtility.HtmlDecode(sText));
                                // smilys entfernen
                                // <img src='http://www.geocaching.com/images/icons/icon_smile_big.gif' border=0 align=middle>
                                sText = entferneRegEx(sText, "<img src='((?s).*?)' border=0 align=middle>");
                                command.ParametersAdd("@comment", DbType.String, sText);

                                command.ExecuteNonQuery();
                                command.Dispose();
                            }
                            catch
                            {
                                // dann halt nicht   vermutlich defektes logdatum
                                Global.AddLog("Error getting a log for: "+cwCache.wayp);
                            }
                        }
                        // Waypoints importieren
                        List<string> aktWaypoints = new List<string>();
                        command = Database.Data.CreateCommand("select GcCode from Waypoint where CacheId=@cacheid");
                        command.ParametersAdd("@cacheid", DbType.Int64, cwCache.GetID());
                        reader = command.ExecuteReader();
                        command.Dispose();
                        while (reader.Read())
                        {
                            string waypoint = reader.GetString(0);
                            aktWaypoints.Add(waypoint);
                        }
                        reader.Close();
                        foreach (cwCache wayp in cwCache.waypoints)
                        {
                            bool isFinal = wayp.IsFinalWaypoint();
                            if (finalToCache)
                            {
                                if ((isFinal) && (wayp.html == ""))
                                    continue;   // Final ist nicht notwendig, da dessen Koordinaten bereits in den Cache eingetragen sind...
                            }
                            if (aktWaypoints.Contains(wayp.wayp))
                            {
                                command = Database.Data.CreateCommand("update Waypoint set CacheId=@cacheid,Latitude=@latitude,Longitude=@longitude,Description=@description,Type=@type,Title=@title where GcCode=@gccode and SyncExclude=0");
                            }
                            else
                            {
                                command = Database.Data.CreateCommand("insert into Waypoint(GcCode,CacheId,Latitude,Longitude,Description,Type,SyncExclude,UserWaypoint,Title) values(@gccode,@cacheid,@latitude,@longitude,@description,@type,0,0,@title)");
                            }
                            command.ParametersAdd("@cacheid", DbType.Int64, cwCache.GetID());
                            command.ParametersAdd("@gccode", DbType.String, wayp.wayp);
                            command.ParametersAdd("@latitude", DbType.Double, wayp.GetLat(false));
                            command.ParametersAdd("@longitude", DbType.Double, wayp.GetLon(false));
                            command.ParametersAdd("@Title", DbType.String, wayp.GetName());
                            string html = wayp.html;
                            if (wayp.notes != "")
                                html += Environment.NewLine + Environment.NewLine + wayp.notes;
                            // <br> durch NewLine ersetzen
                            html = html.Replace("<br>", Environment.NewLine);
                            command.ParametersAdd("@description", DbType.String, html);
                            // Finale Waypoints hier nur als Reference ausgeben, da die Final-Koordinaten im Cache eingetragen sind.
                            // eigentlich bräuchte es die Finalen überhaupt nicht!!!
                            if ((isFinal) && (finalToCache))
                                command.ParametersAdd("@type", DbType.Int16, 11);
                            else
                                command.ParametersAdd("@type", DbType.Int16, wayp.GetTyp());
                            command.ExecuteNonQuery();
                            command.Dispose();
                        }
                    }
                    // datum der letzten Änderung noch anpassen
                    /*        selectCommand = new SqlCeCommand("update GPXFilenames set Imported=@Imported where Id=@id", Database.Connection);
                            selectCommand.Parameters.Add("@Imported", DbType.DateTime).Value = indexDatum;
                            selectCommand.Parameters.Add("@id", DbType.Int32).Value = GPXFilename_ID;
                            selectCommand.ExecuteNonQuery();
                            selectCommand.Dispose();*/

                }

                label1.Text = Global.Translations.Get("c10","new: ") + neu.ToString();
                label2.Text = Global.Translations.Get("c11", "changed: ") + geändert.ToString();
                label3.Text = Global.Translations.Get("c12", "ready");
                Application.DoEvents();
                Database.Data.GPXFilenameUpdateCacheCount();
                GC.Collect();

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                MessageBox.Show(Global.Translations.Get("c17", "Fehler: ") + ex.Message);
            }
            
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = true;
        }

        private void CacheWolfImport_Load(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = cacheWolfDaten;
            if (cacheWolfDaten.Equals(""))
            {
                if (folderBrowserDialog1.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;
                if (folderBrowserDialog1.SelectedPath != cacheWolfDaten)
                {
                    cacheWolfDaten = folderBrowserDialog1.SelectedPath;
                    Config.Set("CacheWolfPath", cacheWolfDaten);
                }
            }

            //      taCacheCategory.Fill(dsGeoScout.CacheCategory);
            label5.Text = Global.Translations.Get("c13", "Data directory: ") + cacheWolfDaten;
            fillCategoryList();
        }

        private void fillCategoryList()
        {
            cbProfile.Items.Clear();
            cbProfile.Items.Add(Global.Translations.Get("c6","_All Profiles"));

            try
            {
                string[] kats = Directory.GetDirectories(cacheWolfDaten);
                foreach (string kat in kats)
                {
                    string pfad = kat.Substring(1 + kat.LastIndexOf('\\'));
                    if (pfad != "")
                        if (!pfad.EndsWith("maps"))
                            cbProfile.Items.Add(pfad);
                }
                if (cbProfile.Items.Count > 0)
                    cbProfile.SelectedIndex = 0;
            }
            catch (System.IO.IOException exc)
            {
                MessageBox.Show(Global.Translations.Get("c17", "Fehler: ") + exc.Message);
                exc.GetType(); //Warning vermeiden _ avoid warning 
            }

        }


        private string getRegex(string reg, string text)
        {
            Regex regex = new Regex(reg);
            Match match = regex.Match(text);
            if (!match.Success)
                return "";
            if (match.Groups.Count < 2)
                return "";
            string result = match.Groups[1].Value;
            return result;
        }
        private void import(string directory)
        {
            logTypen.Clear();
            cwCaches.Clear();
            logTypen.Add("smile", 0);
            logTypen.Add("2", 0); // found
            logTypen.Add("sad", 1);
            logTypen.Add("3", 1); // DNF
            logTypen.Add("note", 2);
            logTypen.Add("4", 2); // wite note
            logTypen.Add("greenlight", 3);
            logTypen.Add("24", 3); // Publish Listing
            logTypen.Add("enabled", 4);
            logTypen.Add("23", 4); // enabled
            logTypen.Add("needsmaint", 5);
            logTypen.Add("45", 5); // Needs Maintenance
            logTypen.Add("disabled", 6);
            logTypen.Add("22", 6); // disabled
            logTypen.Add("maint", 7);
            logTypen.Add("46", 7); // Owner Maintenance
            logTypen.Add("rsvp", 8);
            logTypen.Add("9", 8); // will attend
            logTypen.Add("attended", 9);
            logTypen.Add("10", 9); // attended
            logTypen.Add("camera", 10);
            logTypen.Add("11", 10); // webcam photo taken
            logTypen.Add("redlight", 11);
            logTypen.Add("25", 11); // Retract Listing
            logTypen.Add("remove", 13);
            logTypen.Add("7", 13); // sba

            logTypen.Add("5", 2); // Archive
            logTypen.Add("12", 2); // Unarchived
            logTypen.Add("74", 2); // Announcement
            logTypen.Add("47", 2); // Update Coordinates
            logTypen.Add("18", 2); // Post Reviewer Note

            string indexFile = cacheWolfDaten + '\\' + directory + "\\index.xml";
            if (!(File.Exists(indexFile)))
                return;

            // Datum der letzten Änderung an den Daten
            indexDatum = File.GetLastWriteTime(indexFile);

            using (TextReader text = new StreamReader(indexFile))
            {
                string alles = text.ReadToEnd();
                string regStr = "<CACHE\\s*name\\s*=\\s*\"((?s).*?)\"\\s*";
                regStr += "owner\\s*=\\s*\"((?s).*?)\"\\s*";
                regStr += "lat\\s*=\\s*\"((?s).*?)\"\\s*";
                regStr += "lon\\s*=\\s*\"((?s).*?)\"\\s*";
                regStr += "hidden\\s*=\\s*\"((?s).*?)\"\\s*";
                regStr += "wayp\\s*=\\s*\"((?s).*?)\"\\s*";
                regStr += "status\\s*=\\s*\"((?s).*?)\"\\s*";
                regStr += "ocCacheID\\s*=\\s*\"((?s).*?)\"\\s*";
                regStr += "lastSyncOC\\s*=\\s*\"((?s).*?)\"\\s*";
                regStr += "num_recommended\\s*=\\s*\"((?s).*?)\"\\s*";
                regStr += "num_found\\s*=\\s*\"((?s).*?)\"\\s*";
                regStr += "attributesYes\\s*=\\s*\"((?s).*?)\"\\s*";
                regStr += "attributesNo\\s*=\\s*\"((?s).*?)\"\\s*";
                regStr += "boolFields\\s*=\\s*\"((?s).*?)\"\\s*";
                regStr += "byteFields\\s*=\\s*\"((?s).*?)\"\\s*";
                regStr += "attributesYes1\\s*=\\s*\"((?s).*?)\"\\s*";
                regStr += "attributesNo1\\s*=\\s*\"((?s).*?)\"\\s*";
                regStr += "/>";
                Regex regex = new Regex(regStr);

                MatchCollection matches = regex.Matches(alles);
                int ind = -1;
                progressBar1.Value = 0;
                progressBar1.Maximum = 10;
                foreach (Match match in matches)
                {
                    ind++;
                    double percent = ind * 100 / matches.Count;
                    if (Math.Round(percent / 10) != progressBar1.Value)
                        progressBar1.Value = (int)Math.Round(percent / 10);
                    label1.Text = Global.Translations.Get("c14","reading waypoints:");
                    label2.Text = (ind + 1).ToString() + " - " + matches.Count.ToString();
                    Application.DoEvents();
                    cwCache cwCache = new cwCache();
                    cwCaches.Add(cwCache);
                    if (match.Groups.Count > 1)
                        cwCache.name = match.Groups[1].Value;
                    if (match.Groups.Count > 2)
                        cwCache.owner = match.Groups[2].Value;
                    if (match.Groups.Count > 3)
                        cwCache.lat = match.Groups[3].Value;
                    if (match.Groups.Count > 4)
                        cwCache.lon = match.Groups[4].Value;
                    if (match.Groups.Count > 5)
                        cwCache.hidden = match.Groups[5].Value;
                    if (match.Groups.Count > 6)
                        cwCache.wayp = match.Groups[6].Value;
                    if (match.Groups.Count > 7)
                        cwCache.status = match.Groups[7].Value;
                    if (match.Groups.Count > 8)
                        cwCache.ocCacheID = match.Groups[6].Value;
                    if (match.Groups.Count > 9)
                        cwCache.lastSyncOC = match.Groups[9].Value;
                    if (match.Groups.Count > 10)
                        cwCache.num_recommended = match.Groups[10].Value;
                    if (match.Groups.Count > 11)
                        cwCache.num_found = match.Groups[11].Value;
                    if (match.Groups.Count > 12)
                            cwCache.attributesYes = long.Parse(match.Groups[12].Value);
                    if (match.Groups.Count > 13)
                        cwCache.attributesNo = long.Parse(match.Groups[13].Value);
                    if (match.Groups.Count > 14)
                        cwCache.boolFields = match.Groups[14].Value;
                    if (match.Groups.Count > 15)
                        cwCache.byteFields = match.Groups[15].Value;
                    if (match.Groups.Count > 16)
                        cwCache.attributesYes1 = long.Parse(match.Groups[16].Value);
                    if (match.Groups.Count > 17)
                        cwCache.attributesNo1 = long.Parse(match.Groups[17].Value);

                    // alle Daten hier ausgelesen
                    // nun muss noch die Cache.xml ausgelesen werden...
                    string cacheFile = cacheWolfDaten + '\\' + directory + "\\" + cwCache.wayp + ".xml";

                    cwCache.fileName = cacheFile;

                    if (!(File.Exists(cacheFile)))
                    {
                        cwCache.fileExists = false;
                        continue;
                    }
                    cwCache.fileExists = true;
                    cwCache.changed = File.GetLastWriteTime(cacheFile);
                }
            }

            // Waypoints den Cachen zuordnen
            label1.Text = Global.Translations.Get("c15", "connecting waypoints");
            label2.Text = "";
            Application.DoEvents();
            List<cwCache> löschen = new List<cwCache>();
            progressBar1.Value = 0;
            progressBar1.Maximum = 10;
            int id = -1;
            foreach (cwCache cWaypoint in cwCaches)
            {
                id++;
                double percent = id * 100 / cwCaches.Count;
                if (Math.Round(percent / 10) != progressBar1.Value)
                    progressBar1.Value = (int)Math.Round(percent / 10);

                string prefixWaypoint = cWaypoint.wayp.Substring(0, 2).ToUpper();
                string nameWaypoint = cWaypoint.wayp.Substring(2, cWaypoint.wayp.Length - 2).ToUpper();
                if (!cWaypoint.fileExists)
                {
                    löschen.Add(cWaypoint);
                    continue;
                }
                short t = cWaypoint.getOrgTyp();
                if (!(t>49 && t<60)) continue;  // ist echter Cache

                foreach (cwCache cCache in cwCaches)
                {
                    // Suche des zugehörigen Caches
                    string prefixCache = cCache.wayp.Substring(0, 2).ToUpper();
                    string nameCache = cCache.wayp.Substring(2, cCache.wayp.Length - 2).ToUpper();
                    if (!cCache.fileExists) continue;
                    if (nameWaypoint != nameCache) continue; // Name passt nicht
                    short ty = cCache.getOrgTyp();
                    if ((ty > 49 && ty < 60)) continue;  // ist Waypoint
                    cCache.waypoints.Add(cWaypoint);
                    cWaypoint.parent = cCache;
                    break;
                }
                if (cWaypoint.parent == null)
                    cWaypoint.parent = null;
                löschen.Add(cWaypoint);
            }
            label1.Text = Global.Translations.Get("c16","removing unchanged waypoints");
            Application.DoEvents();
            foreach (cwCache cwc in cwCaches)
            {

                // Caches, die nicht geändert wurden, wieder entfernen, die müssen nicht importiert werden!
                // Blacklistet wieder entfernen
                if (!cwc.IsChanged(aktGPXDatum) || cwc.GetBool(4))
                        löschen.Add(cwc);
            }
            foreach (cwCache cwc in löschen)
            {
                cwCaches.Remove(cwc);
            }
        }

        internal class cwCacheImage
        {
            internal string file = "";
            internal string url = "";
            internal string imgtext = "";
            internal string description = "";
            internal bool imText = false;
        }
        internal class cwCache
        {
            internal string name = "";
            internal string owner = "";
            internal string lat = "";
            internal string lon = "";
            internal string hidden = "";
            internal string wayp = "";
            internal string status = "";
            internal string ocCacheID = "";
            internal string lastSyncOC = "";
            internal string num_recommended = "";
            internal string num_found = "";
            internal long attributesYes = 0;
            internal long attributesNo = 0;
            internal string boolFields = "";
            internal string byteFields = "";
            internal long attributesYes1 = 0;
            internal long attributesNo1 = 0;
            internal string fileName = "";
            internal bool fileExists = false;
            internal string html = "";
            internal string version = "";
            internal string country = "";
            internal string state = "";
            internal string hints = "";
            internal List<string> logs = new List<string>();
            internal string ownlogid = "";
            internal string ownlog = "";
            internal string notes = "";
            internal string solver = "";
            internal string url = "";
            internal List<cwCacheImage> images = new List<cwCacheImage>();
            // internal List<string> atts = new List<string>();
            internal List<cwCache> waypoints = new List<cwCache>();
            internal cwCache parent = null;
            internal DateTime changed = DateTime.Now;
            internal bool IsChanged(DateTime datum)
            {
                // datum ist das Datum des letzten CW-Imports
                if (changed > datum)
                    return true;
                foreach (cwCache cwc in waypoints)
                {
                    if (cwc.changed > datum)
                        return true;
                }
                return false;
            }
            internal bool Einlesen()
            {
                foreach (cwCache cwc in waypoints)
                {
                    cwc.Einlesen();
                }

                string cacheDaten = "";
                using (TextReader cacheText = new StreamReader(fileName))
                {
                    cacheDaten = cacheText.ReadToEnd();
                }
                if (cacheDaten == "")
                {
                    return false;
                }
                fileExists = true;

                Regex creg = new Regex("<VERSION value\\s*=\\s*\"((?s).*?)\"/>");
                Match cmatch = creg.Match(cacheDaten);
                if (cmatch.Success)
                {
                    this.version = cmatch.Groups[1].Value;
                }
                creg = new Regex("<DETAILS><!\\[CDATA\\[((?s).*?)\\]\\]></DETAILS>");
                cmatch = creg.Match(cacheDaten);
                if (cmatch.Success)
                {
                    this.html = cmatch.Groups[1].Value;
                }

                creg = new Regex("<COUNTRY><!\\[CDATA\\[((?s).*?)\\]\\]></COUNTRY>");
                cmatch = creg.Match(cacheDaten);
                if (cmatch.Success)
                {
                    this.country = cmatch.Groups[1].Value;
                }
                creg = new Regex("<STATE><!\\[CDATA\\[((?s).*?)\\]\\]></STATE>");
                cmatch = creg.Match(cacheDaten);
                if (cmatch.Success)
                {
                    this.state = cmatch.Groups[1].Value;
                }
                creg = new Regex("<HINTS><!\\[CDATA\\[((?s).*?)\\]\\]></HINTS>");
                cmatch = creg.Match(cacheDaten);
                if (cmatch.Success)
                {
                    this.hints = cmatch.Groups[1].Value;
                }
                //creg = new Regex("<LOG>((?s).*?)<!\\[CDATA\\[((?s).*?)</LOG>");
                creg = new Regex("<LOG>((?s).*?)</LOG>");
                MatchCollection cmatches = creg.Matches(cacheDaten);
                foreach (Match cmmatch in cmatches)
                {
                    this.logs.Add(cmmatch.Groups[1].Value);
                }
                creg = new Regex("<OWNLOGID>((?s).*?)</OWNLOGID>");
                cmatch = creg.Match(cacheDaten);
                if (cmatch.Success)
                {
                    this.ownlogid = cmatch.Groups[1].Value;
                }
                creg = new Regex("<OWNLOG><!\\[CDATA\\[((?s).*?)\\]\\]></OWNLOG>");
                cmatch = creg.Match(cacheDaten);
                if (cmatch.Success)
                {
                    this.ownlog = cmatch.Groups[1].Value;
                }
                creg = new Regex("<NOTES><!\\[CDATA\\[((?s).*?)\\]\\]></NOTES>");
                cmatch = creg.Match(cacheDaten);
                if (cmatch.Success)
                {
                    this.notes = cmatch.Groups[1].Value;
                }
                creg = new Regex("<SOLVER><!\\[CDATA\\[((?s).*?)\\]\\]></SOLVER>");
                cmatch = creg.Match(cacheDaten);
                if (cmatch.Success)
                {
                    this.solver = cmatch.Groups[1].Value.Replace("\n", Environment.NewLine);

                }
                creg = new Regex("<URL><!\\[CDATA\\[((?s).*?)\\]\\]></URL>");
                cmatch = creg.Match(cacheDaten);
                if (cmatch.Success)
                {
                    this.url = cmatch.Groups[1].Value;
                }
                // Wenn die URL nicht in der CacheWolf-XML vorhanden ist -> für GC.com eine generieren
                if (url == "")
                    url = @"https://coord.info/" + this.wayp;

                if (this.version.Equals("3"))
                {
                    creg = new Regex("<IMG>((?s).*?)<URL>((?s).*?)</URL></IMG>");
                    cmatches = creg.Matches(cacheDaten);
                    foreach (Match cmmatch in cmatches)
                    {
                        cwCacheImage image = new cwCacheImage
                        {
                            file = cmmatch.Groups[1].Value
                        };
                        if (image.file.EndsWith("/"))
                            image.file = image.file.Remove(image.file.Length - 1);
                        image.url = cmmatch.Groups[2].Value;
                        if (image.url.EndsWith("/"))
                            image.url = image.url.Remove(image.url.Length - 1);
                        this.images.Add(image);
                    }

                    creg = new Regex("<IMGTEXT>((?s).*?)</IMGTEXT>");
                    cmatches = creg.Matches(cacheDaten);
                    for (int i = 0; i < cmatches.Count; i++)
                    {
                        Match cmmatch = cmatches[i];
                        if (i < this.images.Count)
                        {
                            cwCacheImage image = this.images[i];
                            string imgtext = cmmatch.Groups[1].Value;
                            // evtl. desc noch suchen, falls vorhanden
                            creg = new Regex("((?s).*?)<DESC>((?s).*?)</DESC>");
                            Match imatch = creg.Match(imgtext);
                            if (imatch.Success)
                            {
                                // mit desc
                                image.imgtext = HttpUtility.HtmlDecode(imatch.Groups[1].Value);
                                image.description = HttpUtility.HtmlDecode(imatch.Groups[2].Value);
                            }
                            else
                            {
                                // ohne desc
                                image.imgtext = HttpUtility.HtmlDecode(imgtext);
                                image.description = "";
                            }
                        }
                    }

                    int userImagesStart = this.images.Count;

                    creg = new Regex("<USERIMG>((?s).*?)</USERIMG>");
                    cmatches = creg.Matches(cacheDaten);
                    foreach (Match cmmatch in cmatches)
                    {
                        cwCacheImage image = new cwCacheImage
                        {
                            file = cmmatch.Groups[1].Value
                        };
                        if (image.file.EndsWith("/"))
                            image.file = image.file.Remove(image.file.Length - 1);
                        image.url = "file:///" + image.file;
                        this.images.Add(image);
                    }
                    creg = new Regex("<USERIMGTEXT>((?s).*?)</USERIMGTEXT>");
                    cmatches = creg.Matches(cacheDaten);
                    for (int i = userImagesStart; i < this.images.Count; i++)
                    {
                        cwCacheImage image = this.images[i];
                        Match cmmatch = cmatches[i - userImagesStart];
                        string imgtext = cmmatch.Groups[1].Value;
                        image.imgtext = HttpUtility.HtmlDecode(imgtext);
                        image.description = "add by User.";
                    }
                }
                else if (this.version.Equals("4"))
                {
                    creg = new Regex("<IMG((?s).*?)</IMG>");
                    cmatches = creg.Matches(cacheDaten);
                    foreach (Match cmmatch in cmatches)
                    {
                        cwCacheImage image = new cwCacheImage();

                        string imgtext = cmmatch.Groups[1].Value;
                        string[] entries = null;
                        entries = imgtext.Split('>');

                        creg = new Regex(" SRC=\"(.*?)\" URL=\"(.*?)\"( TITLE=\"(.*?)\")?( CMT=\"(.*?)\")?");
                        // imatch.Groups[1] = 0 --> FROMUNKNOWN, = 1 --> FROMDESCRIPTION, = 2 --> FROMSPOILER, = 3 --> FROMLOG
                        Match imatch = creg.Match(entries[0]);
                        if (imatch.Success)
                        {
                            image.file = entries[1];
                            if (image.file.EndsWith("/"))
                                image.file = image.file.Remove(image.file.Length - 1);
                            image.url = imatch.Groups[2].Value;
                            if (image.url.EndsWith("/"))
                                image.url = image.url.Remove(image.url.Length - 1);
                            String src = imatch.Groups[1].Value;
                            if (src.Equals("0") || src.Equals("1"))
                            {
                                image.imText = true;
                            }
                            image.imgtext = HttpUtility.HtmlDecode(imatch.Groups[4].Value);
                            image.description = HttpUtility.HtmlDecode(imatch.Groups[6].Value);
                            if (imatch.Groups[1].Equals("4"))
                            {
                                image.description = "add by User.";
                            }
                            this.images.Add(image);
                        }

                    }

                }

                return true;
            }
            /*      internal string CorrectString(string s)
                  {
                    string result = s;
                    result = result.Replace("&#228;", "ä");
                    result = result.Replace("&#246;", "ö");
                    result = result.Replace("&#252;", "ü");
                    result = result.Replace("&#196;", "Ä");
                    result = result.Replace("&#214;", "Ö");
                    result = result.Replace("&#220;", "Ü");
                    result = result.Replace("&#223;", "ß");
                    result = result.Replace("&apos;", "'");
                    result = result.Replace("&quot;", "\"");
                    result = result.Replace("&amp;", "&");
                    result = result.Replace("lt;", "<");
                    result = result.Replace("gt;", ">");
                    return result;
                  }*/
            internal string GetName()
            {
                return HtmlTools.StripHTML(HttpUtility.HtmlDecode(name));
            }
            internal long GetID()
            {
                // Tabellen-ID aus GcCode generieren
                char[] dummy = wayp.ToCharArray();
                if (wayp.ToLower().StartsWith("mz"))
                {
                    System.Security.Cryptography.MD5 md5Hash = System.Security.Cryptography.MD5.Create();
                    byte[] Data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(wayp));
                    /*                     
                    StringBuilder sBuilder = new StringBuilder();                    
                    int i = 0;
                    for (i = 0; i <= Data.Length - 1; i++)
                    {
                        sBuilder.Append(Data[i].ToString("x2"));
                    }                    
                    */
                    return BitConverter.ToInt64(Data, 0);
                }
                else
                {
                    byte[] byteDummy = new byte[8];
                    for (int i = 0; i < 8; i++)
                        if (i < wayp.Length)
                            byteDummy[i] = (byte)dummy[i];
                        else
                            byteDummy[i] = 0;

                    return BitConverter.ToInt64(byteDummy, 0);
                }
            }
            internal Int64 GetSize()
            {
                Int64 cSize = 0;
                try
                {
                    long bytes = Convert.ToInt64(byteFields);
                    short size = (short)((bytes >> 24) & 255);

                    switch (size)
                    {
                        case 0: // unknown
                            cSize = 0;
                            break;
                        case 1: // other
                            cSize = 0;
                            break;
                        case 2: // micror
                            cSize = 1;
                            break;
                        case 3: // small
                            cSize = 2;
                            break;
                        case 4: // regular
                            cSize = 3;
                            break;
                        case 5: // large
                            cSize = 4;
                            break;
                        case 6: // very large
                            cSize = 4;
                            break;
                        default:
                            cSize = 0;
                            break;
                    }
                }
                catch (Exception)
                {

                }
                return cSize;
            }
            internal short GetDifficulty()
            {
                try
                {
                    long bytes = Convert.ToInt64(byteFields);
                    short size = (short)((bytes & 255) * 2 / 10);
                    if (size == 0) size = 2;
                    return size;
                }
                catch (Exception)
                {
                    return 2;
                }
            }
            internal short GetTerrain()
            {
                try
                {
                    long bytes = Convert.ToInt64(byteFields);
                    short size = (short)(((bytes >> 8) & 255) * 2 / 10);
                    if (size == 0) size = 2;
                    return size;
                }
                catch (Exception)
                {
                    return 2;
                }
            }
            internal bool GetBool(int nr)
            {
                try
                {
                    long bits = Convert.ToInt64(boolFields);
                    bool b = (bool)(((bits >> nr) & 1) == 1);
                    return b;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            internal short getOrgTyp()
            {
                try
                {
                    long bytes = Convert.ToInt64(byteFields);
                    return (short)((bytes >> 16) & 255);
                }
                catch (Exception)
                {
                    return 0;
                }

            }

            internal int GetTyp()
            {
                short typ = 13;

                switch (getOrgTyp())
                {
                    case 0: // Custom
                        typ = 0; break; // erlaubt --> Tradi
                    case 2: // Traditional
                        typ = 0; break;
                    case 3: // Multi
                        typ = 1; break;
                    case 4: // Virtual
                        typ = 8; break;
                    case 5: // Letterbox
                        typ = 9; break;
                    case 6: // Event
                        typ = 5; break;
                    case 8: // Unknown
                        typ = 2; break;
                    case 10: // Drive in
                        typ = 13; break;
                    case 11: // Webcam
                        typ = 3; break;
                    case 12: // Locationless
                        typ = 13; break;
                    case 13: // CiTo
                        typ = 7; break;
                    case 100: // Mega Event
                        typ = 6; break;
                    case 101: // Wher I go
                        typ = 10; break;
                    case 102: // APE
                        typ = 0; break;
                    case 104: // Earth
                        typ = 4; break;
                    case 106: // GigaEvent 
                        typ = 22; break;
                    case 50: // Parking
                        typ = 17; break;
                    case 51: // Stage
                        typ = 14; break;
                    case 52: // Question
                        typ = 15; break;
                    case 53: // Final
                        typ = 18; break;
                    case 54: // Trailhead
                        typ = 16; break;
                    case 55: // Reference
                        typ = 11; break;
                    case -1: // Error
                        typ = 13; break;
                    default:
                        typ = 0; // Traditional
                        break;
                }
                /*
                 * Stimmt nicht mehr: Es gibt auch Finals mit = 0 / 0
                if (typ == 18)
                {
                    // Final
                    // wenn bei Final-Waypoint keine Koordinaten (0-0) eingegeben sind, dann soll der Wegpunkt nicht als Final
                    // eingetragen werden, da sonst in CacheBox dieser als gelöst angegeben wird
                    if ((GetLat(false) == 0.0) && (GetLon(false) == 0.0))
                        typ = 15;   // Frage
                }
                 */

                return typ;
            }

            internal bool IsFinalWaypoint()
            {
                if (GetTyp() != 18)
                    return false;
                if ((GetLat(false) == 0) && (GetLon(false) == 0))
                    return false;
                return true;
            }
            internal cwCache GetFinalWaypoint()
            {
                cwCache result = null;
                foreach (cwCache wp in waypoints)
                {
                    if (wp.IsFinalWaypoint())
                        result = wp;
                }
                return result;
            }

            internal double GetLat(bool useFinal)
            {
                useFinal = false;
                double result = 0;
                if (useFinal)
                {
                    cwCache final = GetFinalWaypoint();
                    if (final != null)
                    {
                        result = final.GetLat(false);
                        return result;
                    }
                }
                try
                {
                    string sep = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                    if (sep != ".")
                        lat = lat.Replace(".", sep);
                    if (sep != ",")
                        lat = lat.Replace(",", sep);
                    result = double.Parse(lat);
                    if ((result > 90.1) || (result < -90.1))
                        result = 0;
                }
                catch (Exception)
                {
                }
                return result;
            }
            internal double GetLon(bool useFinal)
            {
                useFinal = false;
                double result = 0;
                if (useFinal)
                {
                    cwCache final = GetFinalWaypoint();
                    if (final != null)
                    {
                        result = final.GetLon(false);
                        return result;
                    }
                }
                try
                {
                    string sep = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                    if (sep != ".")
                        lon = lon.Replace(".", sep);
                    if (sep != ",")
                        lon = lon.Replace(",", sep);
                    result = double.Parse(lon);
                    if ((result > 360.1) || (result < -360.1))
                        result = 0;
                }
                catch (Exception)
                {
                }
                return result;
            }
            internal string recentOwnerLogString()
            {
                string result = "";
                foreach (string log in logs)
                {
                    string sFinder = "";
                    string sText = "";
                    Regex regLog = new Regex("<img src='icon_((?s).*?).gif'>&nbsp;((?s).*?)\\s*by\\s*((?s).*?)<br>((?s).*?)\\]\\]>");
                    Match matchLog = regLog.Match(log);
                    if (!matchLog.Success)
                        continue;
                    sFinder = matchLog.Groups[3].Value;
                    sText = matchLog.Groups[4].Value;
                    if (sFinder == owner)
                    {
                        result += sText;
                    }
                }
                return result;
            }

        }

        private static String[,] attRef = { 
		{ "00", "2502", "available", "38", "13", "Available at all times" },// 02 available 24-7
		{ "01", "2504", "bicycles", "0", "32", "Bicycles" },// 04 bikes allowed
		{ "02", "2506", "boat", "52", "4", "Boat" },// 06 Wasserfahrzeug
		// {"03","2508","cactus","0","0",""},//08 removed 14.08.10 araber95
		{ "04", "2510", "campfires", "0", "38", "Campfires" },// 10 campfires allowed
		{ "05", "2512", "camping", "0", "31", "Camping available" },// 12 Camping allowed
		{ "06", "2514", "cliff", "11", "21", "Cliff / falling rocks" },// 14 falling-rocks nearby
		{ "07", "2516", "climbing", "28", "10", "Difficult climbing" },// 16 easy climbing(OC-28), difficult climbing(GC-10)
		{ "08", "2518", "compass", "47", "147", "Compass" }, // OC special
		{ "09", "2520", "cow", "0", "43", "Watch for livestock" },// 20 watch for livestock
		{ "10", "2522", "danger", "9", "23", "Dangerous area" },// 22 dangerous area
		{ "11", "2524", "dogs", "0", "1", "Dogs" },// 24 dogs allowed
		{ "12", "2526", "fee", "36", "2", "Access or parking fee" },// 26 access/parking fees
		{ "13", "2528", "hiking", "25", "9","Significant hike"},//28 significant hike
		//{ "13", "2528", "hiking", "25", "125", "Long walk" }, // OC special
		{ "14", "2530", "horses", "0", "37", "Horses" },// 30 horses allowed
		{ "15", "2532", "hunting", "12", "22", "Hunting" },// 32 hunting area
		{ "16", "2534", "jeeps", "0", "35", "Off-road vehicles" },// 34 off-road vehicles allowed
		{ "17", "2536", "kids", "59", "6", "Recommended for kids" },// 36 kid friendly
		{ "18", "2538", "mine", "15", "20", "Abandoned mines" },// 38
		{ "19", "2540", "motorcycles", "0", "33", "Motorcycles" },// 40 motorcycles allowed
		{ "20", "2542", "night", "1", "14", "Recommended at night" },// 42 recommended at night
		{ "21", "2544", "onehour", "0", "7", "Takes less than an hour" },// 44 takes less than one hour
		{ "22", "2546", "parking", "18", "25", "Parking available" },// 46 parking available
		{ "23", "2548", "phone", "22", "29", "Telephone nearby" },// 48 telephone nearby
		{ "24", "2550", "picnic", "0", "30", "Picnic tables nearby" },// 50 picnic tables available
		{ "25", "2552", "poisonoak", "16", "17", "Poison plants" },// 52 Giftige Pflanzen
		{ "26", "2554", "public", "19", "26", "Public transportation" },// 54 public transit available
		{ "27", "2556", "quads", "0", "34", "Quads" },// 56 quads allowed
		{ "28", "2558", "rappelling", "49", "3", "Climbing gear" },// 58 climbing gear Kletterausrüstung
		{ "29", "2560", "restrooms", "21", "28", "Public restrooms nearby" },// 60 restrooms available
		{ "30", "2562", "scenic", "0", "8", "Scenic view" },// 62 scenic view
		{ "31", "2564", "scuba", "51", "5", "Scuba gear" },// 64 Tauchausrüstung
		// {"32","2566","snakes","0","18","Snakes"},//66 araber95 replaced by Dangerous Animals 14.08.10
		{ "32", "2566", "dangerousanimals", "0", "18", "Dangerous Animals" },// 66
		{ "33", "2568", "snowmobiles", "0", "36", "Snowmobiles" },// 68
		{ "34", "2570", "stealth", "0", "40", "Stealth required" },// 70 stealth required (Heimlich,List,Schläue)
		{ "35", "2572", "stroller", "0", "41", "Stroller accessible" },// 72 stroller accessible
		{ "36", "2574", "swimming", "29", "12", "May require swimming" },// 74
		{ "37", "2576", "thorn", "13", "39", "Thorns" },// 76 thorns!
		{ "38", "2578", "ticks", "14", "19", "Ticks" },// 78 ticks!
		{ "39", "2580", "wading", "26", "11", "May require wading" },// 80 may require wading
		{ "40", "2582", "water", "20", "27", "Drinking water nearby" },// 82 drinking water nearby
		{ "41", "2584", "wheelchair", "0", "24", "Wheelchair accessible" },// 84 wheelchair accessible
		{ "42", "2586", "winter", "44", "15", "Available during winter" },// 86 available in winter 132 Schneesicheres Versteck
		{ "43", "2588", "firstaid", "0", "42", "Firstaid" }, // GC: Cachewartung notwendig (Auto Attribut) , OC: erste Hilfe
		{ "44", "2590", "flashlight", "48", "44", "Flashlight required" }, // 90 Flashlight required
		{ "45", "2592", "aircraft", "53", "153", "Aircraft" }, // OC special //38 GC removed
		{ "46", "2594", "animals", "17", "0", "" },// 94 Giftige/gef%e4hrliche Tiere
		{ "47", "2596", "arith_prob", "56", "156", "Arithmetical problem" }, // OC special
		{ "48", "2598", "ask", "58", "158", "Ask owner for start conditions" }, // OC special
		{ "49", "2600", "car", "24", "0", "" },// 100 Nahe beim Auto
		{ "50", "2602", "cave", "50", "150", "Cave equipment" }, // OC special
		{ "51", "2604", "date", "42", "142", "All seasons" }, // OC special
		{ "52", "2606", "day", "40", "140", "by day only" }, // OC special
		{ "53", "2608", "indoor", "33", "133", "Within enclosed rooms (caves, buildings etc.)" }, // OC special
		{ "54", "2610", "interestsign", "30", "130", "Point of interest" }, // OC special
		{ "55", "2612", "letter", "8", "108", "Letterbox (needs stamp)" }, // OC special
		{ "56", "2614", "moving", "31", "131", "Moving target" }, // OC special
		{ "57", "2616", "naturschutz", "43", "143", "Breeding season / protected nature" }, // OC special
		{ "58", "2618", "nogps", "35", "135", "Without GPS (letterboxes, cistes, compass juggling ...)" }, // OC special
		{ "59", "2620", "oconly", "6", "106", "Only loggable at Opencaching" },// 120 Nur bei Opencaching logbar
		{ "60", "2622", "othercache", "57", "157", "Other cache type" }, // OC special
		{ "61", "2624", "overnight", "37", "137", "Overnight stay necessary" }, // OC special
		{ "62", "2644", "train", "10", "110", "Active railway nearby" }, // OC special
		{ "63", "2630", "riddle", "55", "0", "" },//
		{ "64", "2646", "webcam", "32", "132", "Webcam" }, // OC special
		{ "65", "2634", "steep", "27", "127", "Hilly area" }, // OC special
		{ "66", "2636", "submerged", "34", "134", "In the water" }, // OC special
		{ "67", "2638", "tide", "41", "141", "Tide" }, // OC special
		{ "68", "2640", "time", "39", "139", "Only available at specified times" }, // OC special
		{ "69", "2642", "tools", "46", "0", "Special Tool required" },//
		{ "70", "2648", "wiki", "54", "154", "Investigation" }, // OC special
		{ "71", "2650", "wwwlink", "7", "107", "Hyperlink to another caching portal only" }, // OC special
		{ "72", "2652", "landf", "0", "45", "Lost And Found Tour" },
		{ "73", "2654", "rv", "0", "46", "Truck Driver/RV" },
		{ "74", "2656", "field_puzzle", "0", "47", "Field Puzzle" },//
		{ "75", "2658", "uv", "0", "48", "UV Light required" }, // added by araber95 14.8.10
		{ "76", "2660", "snowshoes", "0", "49", "Snowshoes" }, // added by araber95 14.8.10"
		{ "77", "2662", "skiis", "0", "50", "Cross Country Skis" }, // added by araber95 14.8.10
		{ "78", "2664", "s-tool", "0", "51", "Special Tool required" }, // added by araber95 14.8.10
		{ "79", "2666", "nightcache", "0", "52", "Night Cache" }, // added by araber95 14.8.10
		{ "80", "2668", "parkngrab", "0", "53", "Park and grab" }, // added by araber95 14.8.10
		{ "81", "2670", "abandonedbuilding", "0", "54", "Abandoned structure" }, // added by araber95 14.8.10
		{ "82", "2672", "hike_short", "0", "55", "Short hike" }, // added by araber95 14.8.10
		{ "83", "2674", "hike_med", "0", "56", "Medium Hike" }, // added by araber95 14.8.10
		{ "84", "2676", "hike_long", "0", "57", "Long Hike" }, // added by araber95 14.8.10
		{ "85", "2678", "fuel", "0", "58", "Fuel nearby" }, // changed by araber95 14.08.10
		{ "86", "2680", "food", "0", "59", "Food nearby" }, // changed by araber95 14.08.10
		{ "87", "2682", "wirelessbeacon", "0", "60", "Wireless Beacon" }, // added by araber95 27.10.10
		{ "88", "2584", "firstaid", "23", "123", "First aid available" }, // OC special
		{ "89", "2686", "sponsored", "0", "61", "Sponsored Cache" },
		{ "90", "2688", "frontyard", "0", "65", "Front Yard (Private Residence)" },
		{ "91", "2690", "seasonal", "0", "62", "Seasonal Access" },
		{ "92", "2692", "teamwork", "0", "66", "Teamwork Required" },
		{ "93", "2694", "touristOK", "0", "63", "Tourist Friendly" },
		{ "94", "2696", "treeclimbing", "0", "64", "Tree Climbing" },
	    { "95", "2698", "geotour", "0", "67", "GeoTour" }, //
	    { "96", "2700", "partnership", "0", "61", "Partnership Cache" }, //
        };

            private static int[] cwRef = new int[97];
        private bool isInit = false;
        private void fillCWRef()
        {
            for (int j = 0; j < attRef.GetLength(0); j++)
            {
                cwRef[Int32.Parse(attRef[j, 0])] = j;
            }
            isInit = true;
        }
                        
        private void getAttributes(long attributesYes, long attributesNo, long attributesYes1, long attributesNo1, ref long attributesLongPositive, ref long attributesLongNegative, ref long attributesLongPositiveHigh, ref long attributesLongNegativeHigh) 
        {
            if (!isInit) fillCWRef();
            toGCatt(attributesYes, false, ref attributesLongPositiveHigh, ref attributesLongPositive);
            toGCatt(attributesYes1, true, ref attributesLongPositiveHigh, ref attributesLongPositive);
            toGCatt(attributesNo, false, ref attributesLongNegativeHigh, ref attributesLongNegative);
            toGCatt(attributesNo1, true, ref attributesLongNegativeHigh, ref attributesLongNegative);
        }
        private void toGCatt(long CWatt, bool upper, ref long high, ref long low)
        {
            int offset = 0;
            if (upper) offset = 64;
            for (int i = 0; i < 64; i++)
            {
                Boolean gesetzt = (CWatt & 1L << i) > 0;
                if (gesetzt)
                {
                    int gcID = Int32.Parse(attRef[cwRef[i+offset], 4]);
                    if (gcID > 62)
                    {
                        // >= 100 OC special ohne entsprechende gcID
                        if (gcID < 100) 
                            high = high | 1L << (gcID - 63);
                    }
                    else
                    {
                        // 0 keine entsprechende gcID
                        if (gcID > 0) 
                            low = low | 1L << gcID;
                    }
                }
            }
        }

        private string entferneRegEx(string text, string sregex)
        {
            Regex reg = new Regex(sregex);
            MatchCollection matches = reg.Matches(text);
            int diff = 0;
            foreach (Match match in matches)
            {
                text = text.Remove(match.Index - diff, match.Length);
                diff += match.Length;
            }
            return text;
        }

        private void cbProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = false;
        }

        private void nudZurück_ValueChanged(object sender, EventArgs e)
        {
            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = cacheWolfDaten;
            if (folderBrowserDialog1.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;
            if (folderBrowserDialog1.SelectedPath != cacheWolfDaten)
            {
                cacheWolfDaten = folderBrowserDialog1.SelectedPath;
                Config.Set("CacheWolfPath", cacheWolfDaten);
            }

            //      taCacheCategory.Fill(dsGeoScout.CacheCategory);
            label5.Text = Global.Translations.Get("c13", "Data directory: ") + cacheWolfDaten;
            fillCategoryList();
        }

    }
}
