using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;

namespace WinCachebox.Geocaching
{
    class DescriptionImageGrabber
  {
        public struct Segment
        {
            public int start;
            public int ende;
            public String text;
        }    
        private Dictionary<String, String> urlLookup = new Dictionary<string, string>();
        IFormProgressReport parent = null;
        public DescriptionImageGrabber(IFormProgressReport parent)
        {
            this.parent = parent;
        }

        public static List<Uri> GetImageUris(String html, String baseUrl)
        {
            List<Uri> result = new List<Uri>();

            try
            {
                Uri baseUri;
                try
                {
                    baseUri = new Uri(baseUrl);
                }
                catch (Exception)
                {
#if DEBUG
                    Global.AddLog("DescriptionImageGrabber.GetImageUris: Cannot resolve base Url '" + baseUrl + "'");
#endif
                    return result;
                }

                String htmlNoSpaces = RemoveSpaces(html);

                List<Segment> imgTags = Segmentize(htmlNoSpaces, "<img", ">");

                foreach (Segment img in imgTags)
                {
                    List<Segment> srcAttribs = Segmentize(img.text, "src=\"", "\"");
                    List<Segment> srcAttribs2 = Segmentize(img.text, "src=\'", "\'");

                    srcAttribs.AddRange(srcAttribs2);


                    if (srcAttribs.Count == 1)
                    {
                        try
                        {
                            Uri imageUri = new Uri(baseUri, srcAttribs[0].text);

                            if (imageUri.AbsoluteUri.EndsWith(".gif", StringComparison.OrdinalIgnoreCase) ||
                               imageUri.AbsoluteUri.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                               imageUri.AbsoluteUri.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                               imageUri.AbsoluteUri.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                               imageUri.AbsoluteUri.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase)
                              )
                            {
                                if (!result.Contains(imageUri))
                                    result.Add(imageUri);
                            }
                        }
                        catch (Exception) { }
                    }
                }

            }
            catch (Exception exc)
            {
#if DEBUG
                Global.AddLog("DescriptionImageGrabber.GetImageUris: " + exc.ToString() + "\n\n in HTML:" + html + "\n\n");
#endif
            }

            return result;
        }

        public void GrabImages()
        {
            bool gcAdditionalImageDownload = Config.GetBool("GCAdditionalImageDownload");
            int gcRequestDelay_ms = Config.GetInt("GCRequestDelay") * 1000 * 0;

            bool gsLoggedIn = false;

            try
            {
                /*        gsLoggedIn = groundspeak.Login(gcLogin, gcPass);

                        if (!gsLoggedIn)
                        {
                          parent.ReportUncriticalError("Wrong GC username or password. Check settings!");
                          System.Threading.Thread.Sleep(2000);
                        }*/
            }
            catch (TimeoutException)
            {
                parent.ReportUncriticalError("Server Timeout...");
            }
            catch (System.Net.Sockets.SocketException)
            {
                parent.ReportUncriticalError("Cannot resolve Host!");
            }
            catch (System.Net.WebException)
            {
                parent.ReportUncriticalError("Cannot resolve Host!");
            }

            if (!Directory.Exists(Config.GetString("DescriptionImageFolder")))
                Directory.CreateDirectory(Config.GetString("DescriptionImageFolder"));

            string where = Global.LastFilter.SqlWhere;

            CBCommand command = Database.Data.CreateCommand("select Count(Id) from Caches " + ((where.Length > 0) ? "where " + where : where));
            object ob = command.ExecuteScalar();
            int numCaches = 0;
            numCaches = int.Parse(ob.ToString());

            command.Dispose();

            command = Database.Data.CreateCommand("select Id, Description, Name, GcCode, Url, ImagesUpdated, DescriptionImagesUpdated from Caches " + ((where.Length > 0) ? "where " + where : where));
            DbDataReader reader = command.ExecuteReader();

            int cnt = -1;
            while (reader.Read() && !parent.Cancel)
            {
                cnt++;

                long id = reader.GetInt64(0);
                String name = reader.GetString(2);
                String gcCode = reader.GetString(3);

                bool additionalImagesUpdated = false;
                bool descriptionImagesUpdated = false;

                if (!reader.IsDBNull(5))
                {
                    additionalImagesUpdated = reader.GetBoolean(5);
                }

                if (!reader.IsDBNull(6))
                {
                    descriptionImagesUpdated = reader.GetBoolean(6);
                }
                /*
                 * 2014-06-22 - Ging-Buh - wird nicht mehr benötigt, da die Info, ob Spoiler geladen werden müssen ausschließlich aus der DB kommt.
                 * Wenn schon Dateien vorhanden sind heißt das nicht, dass die Spoiler nicht nochmal geladen werden müssen
                        bool dbUpdate = false;

                        if (!descriptionImagesUpdated)
                        {
                          descriptionImagesUpdated = CheckLocalImages(Config.GetString("DescriptionImageFolder"), gcCode);

                          if (descriptionImagesUpdated)
                          {
                            dbUpdate = true;
                          }
                        }

                        if (!additionalImagesUpdated)
                        {
                          additionalImagesUpdated = CheckLocalImages(Config.GetString("SpoilerFolder"), gcCode);

                          if (additionalImagesUpdated)
                          {
                            dbUpdate = true;
                          }
                        }

                        if (dbUpdate)
                        {
                            CBCommand imagesUpdatedCommand = Database.Data.CreateCommand("update Caches set ImagesUpdated=@ImagesUpdated, DescriptionImagesUpdated=@DescriptionImagesUpdated where Id=@id");
                            imagesUpdatedCommand.ParametersAdd("@ImagesUpdated", DbType.Boolean, additionalImagesUpdated);
                            imagesUpdatedCommand.ParametersAdd("@DescriptionImagesUpdated", DbType.Boolean, descriptionImagesUpdated);
                            imagesUpdatedCommand.ParametersAdd("@id", DbType.Int64, id);
                            imagesUpdatedCommand.ExecuteNonQuery();
                            imagesUpdatedCommand.Dispose();
                        }
                */
                GrabImagesSelectedByCache(descriptionImagesUpdated, additionalImagesUpdated, gcAdditionalImageDownload, gcRequestDelay_ms, gsLoggedIn, numCaches, reader, cnt, name, gcCode);
            }

            reader.Dispose();
            command.Dispose();
        }

        public void GrabImagesOfOneCache(long CacheId)
        {
            //bool gsLoggedIn = false;

            //try
            //{
            //  gsLoggedIn = groundspeak.Login(gcLogin, gcPass);

            //  if (!gsLoggedIn)
            //  {
            //    parent.ReportUncriticalError("Wrong GC username or password. Check settings!");
            //    System.Threading.Thread.Sleep(2000);
            //  }
            //}
            //catch (TimeoutException)
            //{
            //  parent.ReportUncriticalError("Server Timeout...");
            //}
            //catch (System.Net.Sockets.SocketException)
            //{
            //  parent.ReportUncriticalError("Cannot resolve Host!");
            //}
            //catch (System.Net.WebException)
            //{
            //  parent.ReportUncriticalError("Cannot resolve Host!");
            //}

            int numCaches = 1;

            CBCommand command = Database.Data.CreateCommand("select Id, Description, Name, GcCode, Url, ImagesUpdated, DescriptionImagesUpdated from Caches where id=@id");
            command.ParametersAdd("@id", DbType.Int64, CacheId);
            DbDataReader reader = command.ExecuteReader();

            int cnt = -1;
            bool gcAdditionalImageDownload = true;
            int gcRequestDelay_ms = 0; // We do not want to wait for a single cache listing request.

            while (reader.Read() && !parent.Cancel)
            {
                cnt++;

                String name = reader.GetString(2);
                String gcCode = reader.GetString(3);

                // always download images for GrabImagesOfOneCache, even if they already exists.

                bool additionalImagesUpdated = false;
                bool descriptionImagesUpdated = false;

                GrabImagesSelectedByCache(descriptionImagesUpdated, additionalImagesUpdated, gcAdditionalImageDownload, gcRequestDelay_ms, true, numCaches, reader, cnt, name, gcCode);

                DeleteChangedImageInformation(Config.GetString("DescriptionImageFolder"), gcCode);
                DeleteChangedImageInformation(Config.GetString("SpoilerFolder"), gcCode);
            }

            reader.Dispose();
            command.Dispose();
        }

        private void GrabImagesSelectedByCache(bool descriptionImagesUpdated, bool additionalImagesUpdated, bool gcAdditionalImageDownload, int gcRequestDelay_ms, bool gsLoggedIn, int numCaches, DbDataReader reader, int cnt, String name, String gcCode)
        {
            bool importLogImages = Config.GetBool("ImportLogImages");
            parent.ProgressChangedTotal("Scanning " + name, cnt, numCaches);
            try
            {
                long id = reader.GetInt64(0);
                String description = reader.GetString(1);
                bool imageLoadError = false;

                if (!descriptionImagesUpdated)
                {
                    List<Uri> imgUris = GetImageUris(description, reader.GetString(4));

                    int i = 0;
                    foreach (Uri uri in imgUris)
                    {
                        // Suppress power down
                        if (i % 10 == 0)
                            Application.DoEvents();

                        if (uri.IsFile)
                            continue;
                        if (uri.IsLoopback)
                            continue;
                        if (uri.IsUnc)
                            continue;

                        String local = BuildImageFilename(gcCode, uri);
                        String oldLocalFile = BuildOldImageFilename(gcCode, uri);

                        if (File.Exists(oldLocalFile))
                        {
                            File.Delete(oldLocalFile);
                        }

                        parent.ProgressChanged("Loading " + name + " (Image " + (i + 1).ToString() + "/" + imgUris.Count.ToString() + ")", i + 1, imgUris.Count);

                        // build URL

                        for (int j = 0; j < 1 && !parent.Cancel; j++)
                        {
                            if (Download(uri.ToString(), local))
                            {
                                // Next image
                                DeleteMissingImageInformation(local);
                                parent.PerformMemoryTest(Config.GetString("DescriptionImageFolder"), 1024);
                                break;
                            }
                            else
                            {
                                parent.ReportUncriticalError(uri + " failed to load");

                                imageLoadError = HandleMissingImages(imageLoadError, uri, local);

#if DEBUG
                                Global.AddLog("\n\nDescriptionImageGrabber: Cannot load " + uri + "\n\n");
#endif
                                System.Threading.Thread.Sleep(1000);
                            }

                            GC.Collect();
                        }
                        i++;
                    }

                    descriptionImagesUpdated = true;

                    if (imageLoadError == false)
                    {
                        CBCommand imagesUpdatedCommand = Database.Data.CreateCommand("update Caches set DescriptionImagesUpdated=@DescriptionImagesUpdated where Id=@id");
                        imagesUpdatedCommand.ParametersAdd("@DescriptionImagesUpdated", DbType.Boolean, descriptionImagesUpdated);
                        imagesUpdatedCommand.ParametersAdd("@id", DbType.Int64, id);
                        imagesUpdatedCommand.ExecuteNonQuery();
                        imagesUpdatedCommand.Dispose();
                    }
                }

                if (!additionalImagesUpdated)
                {
                    //Get additional images

                    // Liste aller Spoiler Images für diesen Cache erstellen
                    // anhand dieser Liste kann überprüft werden, ob ein Spoiler schon geladen ist und muss nicht ein 2. mal geladen werden.
                    // Außerdem können anhand dieser Liste veraltete Spoiler identifiziert werden, die gelöscht werden können / müssen
                    List<string> afiles = new List<string>();
                    try
                    {
                        string[] files = Directory.GetFiles(Config.GetString("SpoilerFolder") + "\\" + gcCode.Substring(0, 4), gcCode + "*");

                        foreach (string file in files)
                            afiles.Add(file);
                    }
                    catch (Exception)
                    {
                    }

                    if (/*gsLoggedIn && */gcAdditionalImageDownload)
                    {
                        Dictionary<string, ImageLink> allimgDict = Groundspeak.getInstance().GetAllImageLinks(gcCode, importLogImages);

                        //          if (singleDownloadMode)
                        {
                            /*            FormSelectImagesForDownload formSelectImagesForDownload = new FormSelectImagesForDownload();
                                        formSelectImagesForDownload.InitListOfImagesForDownload(allimgDict);

                                        if (DialogResult.OK == formSelectImagesForDownload.ShowDialog())
                                        {
                                          allimgDict = formSelectImagesForDownload.GetDictionaryOfSelectedImages();
                                        }
                                        else
                                        {
                                          return;
                                        }*/
                        }

                        int i = 0;
                        foreach (string key in allimgDict.Keys)
                        {
                            Uri uri = allimgDict[key].uri;

                            string decodedImageName = HttpUtility.HtmlDecode(key);

                            // Supress power down
                            if (i % 10 == 0)
                                Application.DoEvents();

                            if (uri.IsFile)
                                continue;
                            if (uri.IsLoopback)
                                continue;
                            if (uri.IsUnc)
                                continue;

                            String local = BuildAdditionalImageFilename(gcCode, decodedImageName, uri);
                            if (File.Exists(local))
                            {
                                try
                                {
                                    // Spoiler ohne den Hash im Dateinamen löschen
                                    File.Delete(local);
                                }
                                catch (Exception)
                                {
                                    // Datei evtl. in Spoiler View geöffnet??? Kann nicht gelöscht werden.
                                }
                            }

                            // Local Filename mit Hash erzeugen, damit Änderungen der Datei ohne Änderungen des Dateinamens erkannt werden können
                            // Hier erst die alten Version mit den Klammern als Eingrenzung des Hash
                            // Dies hier machen, damit die Namen der Spoiler ins neue System Konvertiert werden können.
                            String localOld = BuildAdditionalImageFilenameHash(gcCode, decodedImageName, uri);
                            // Neuen Local Filename mit Hash erzeugen, damit Änderungen der Datei ohne Änderungen des Dateinamens erkannt werden
                            // können
                            // Hier jetzt mit @ als Eingrenzung des Hashs
                            // Local Filename mit Hash erzeugen, damit Änderungen der Datei ohne Änderungen des Dateinamens erkannt werden können
                            local = BuildAdditionalImageFilenameHashNew(gcCode, decodedImageName, uri);
                            if (File.Exists(localOld))
                            {
                                // wenn ein Spoiler im alten Format mit Hash gefunden wurde dann wird dieser ins neue Format umbenannt und muss nicht erneut geladen werden
                                try
                                {
                                    File.Move(localOld, local);
                                    afiles.Add(local);
                                }
                                catch (Exception)
                                {
                                }
                            }


                            String filename = local;
                            // überprüfen, ob dieser Spoiler bereits geladen wurde
                            if (afiles.Contains(filename))
                            {
                                // wenn ja, dann aus der Liste der aktuell vorhandenen Spoiler entfernen und mit dem nächsten Spoiler weiter machen
                                // dieser Spoiler muss jetzt nicht mehr geladen werden da er schon vorhanden ist.
                                afiles.Remove(filename);
                                continue;
                            }


                            parent.ProgressChanged("Loading " + name + ": " + decodedImageName + " (Image " + (i + 1).ToString() + "/" + allimgDict.Count.ToString() + ")", i + 1, allimgDict.Count);

                            // build URL

                            for (int j = 0; j < 1 && !parent.Cancel; j++)
                            {
                                if (Download(uri.ToString(), local))
                                {
                                    // Next image
                                    DeleteMissingImageInformation(local);
                                    parent.PerformMemoryTest(Config.GetString("SpoilerFolder"), 1024);
                                    break;
                                }
                                else
                                {
                                    parent.ReportUncriticalError(uri + " failed to load");

                                    imageLoadError = HandleMissingImages(imageLoadError, uri, local);

#if DEBUG
                                    Global.AddLog("\n\nAdditionalImageGrabber: Cannot load " + uri + "\n\n");
#endif

                                    System.Threading.Thread.Sleep(1000);
                                }

                                GC.Collect();
                            }
                            i++;
                        }

                        additionalImagesUpdated = true;

                        if (imageLoadError == false)
                        {
                            CBCommand imagesUpdatedCommand = Database.Data.CreateCommand("update Caches set ImagesUpdated=@ImagesUpdated where Id=@id");
                            imagesUpdatedCommand.ParametersAdd("@ImagesUpdated", DbType.Boolean, additionalImagesUpdated);
                            imagesUpdatedCommand.ParametersAdd("@id", DbType.Int64, id);
                            imagesUpdatedCommand.ExecuteNonQuery();
                            imagesUpdatedCommand.Dispose();

                            // veraltete, vom Owner entfernte Spoiler auch hier entfernen

                            // jetzt können noch alle "alten" Spoiler gelöscht werden. "alte" Spoiler sind die, die auf der SD vorhanden sind, aber
                            // nicht als Link über die API gemeldet wurden
                            // Alle Spoiler in der Liste afiles sind "alte"
                            foreach (String file in afiles)
                            {
                                String fileNameWithOutExt = Path.GetFileNameWithoutExtension(file);
                                // Testen, ob dieser Dateiname einen gültigen ACB Hash hat (eingeschlossen zwischen @....@>
                                if (fileNameWithOutExt.EndsWith("@") && fileNameWithOutExt.Contains("@"))
                                {
                                    // file enthält kompletten Pfad
                                    // Spoiler gespeichert wurden
                                    try
                                    {
                                        File.Delete(file);
                                    }
                                    catch (Exception ex)
                                    {
                                        Global.AddLog("DescriptionImageGrabber - GrabImagesSelectedByCache - DeleteSpoiler: " + ex.Message);
                                    }
                                }
                            }

                        }

                        int gcRequestDelay_s = gcRequestDelay_ms / 1000 * 0;

                        for (i = 0; i < gcRequestDelay_s; i++)
                        {
                            parent.ProgressChanged("Delay request " + i + " of " + gcRequestDelay_s + " seconds.", i, gcRequestDelay_s);
                            System.Threading.Thread.Sleep(1000 /* ms */); // Sorry, but this neccessary to be confirm to the groundspeak terms of use.
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Global.AddLog(ex.Message);
                // Wenn keine Description vorhanden ist (selbst in ACB angelegter Cache dann wirft das Auslesen der Description einen Fehler
            }
        }

        private static bool HandleMissingImages(bool imageLoadError, Uri uri, String local)
        {
            try
            {
                if (!File.Exists(local + "_broken_link.txt"))
                {
                    if (File.Exists(local + ".1st"))
                    {
                        // After first try, we can be sure that the image cannot be loaded.
                        // At this point mark the image as loaded and go ahead.
                        File.Move(local + ".1st", local + "_broken_link.txt");
                    }
                    else
                    {
                        // Crate a local file for marking it that it could not load one time.
                        // Maybe the link is broken temporarely. So try it next time once again.
                        FileInfo missingImageInfo = new FileInfo(local + ".1st");
                        StreamWriter missingImageTextFile = missingImageInfo.CreateText();
                        missingImageTextFile.WriteLine("Could not load image from:");
                        missingImageTextFile.WriteLine(uri);
                        missingImageTextFile.Close();
                        imageLoadError = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Global.AddLog("HandleMissingImages (uri=" + uri + ") (local=" + local + ") - " + ex.ToString());
            }
            return imageLoadError;
        }

        private static void DeleteMissingImageInformation(String local)
        {
            if (File.Exists(local + "_broken_link.txt"))
            {
                File.Delete(local + "_broken_link.txt");
            }

            if (File.Exists(local + ".1st"))
            {
                File.Delete(local + ".1st");
            }
        }

        static void DeleteChangedImageInformation(String path, String gcCode)
        {
            string imagePath = path + "\\" + gcCode.Substring(0, 4);

            String file = imagePath + "\\" + gcCode + ".changed";

            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }

        // no longer used!!!
        private static bool CheckLocalImages(string path, string GcCode)
        {
            bool retval = true;

            if (GcCode.Length < 4)
                return true; // avoid Problem with GcCode with length < 4 chars

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

            if (imagePathDirExists)
            {
                string[] files = Directory.GetFiles(imagePath, GcCode + "*");

                if (files.Length != 0)
                {
                    foreach (string file in files)
                    {
                        if (file.EndsWith(".1st") || file.EndsWith(".changed"))
                        {
                            if (file.EndsWith(".changed"))
                            {
                                File.Delete(file);
                            }

                            retval = false;
                        }
                    }
                }
                else
                {
                    retval = false;
                }
            }
            else
            {
                retval = false;
            }

            return retval;
        }

        public static bool Download(String uri, String local)
        {
            try
            {
                string localDir = local.Substring(0, local.LastIndexOf("\\"));

                if (!Directory.Exists(localDir))
                {
                    Directory.CreateDirectory(localDir);
                }

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uri.Replace("&amp;", "&"));
                webRequest.Timeout = 15000;
                webRequest.AllowAutoRedirect = true;
                webRequest.Proxy = Global.Proxy;

                // *** Retrieve request info headers
                WebResponse webResponse = webRequest.GetResponse();
                if (!webRequest.HaveResponse)
                    return false;

                byte[] result = Global.ReadFully(webResponse.GetResponseStream(), 64000);

                Global.TransferredBytes += result.Length;

                FileStream fs;

                if (File.Exists(local))
                    fs = new FileStream(local, FileMode.Truncate);
                else
                    fs = new FileStream(local, FileMode.CreateNew | FileMode.CreateNew);

                fs.Write(result, 0, result.Length); ;

                fs.Close();

                return true;
            }
            catch (Exception exc)
            {
                exc.GetType(); //Warning vermeiden _ avoid warning
                return false;
            }
        }

        public static List<Segment> Segmentize(String text, String leftSeperator, String rightSeperator)
        {
            List<Segment> result = new List<Segment>();

            int idx = 0;

            while (true)
            {
                int leftIndex = text.ToLower().IndexOf(leftSeperator, idx);

                if (leftIndex == -1)
                    break;

                leftIndex += leftSeperator.Length;

                int rightIndex = text.ToLower().IndexOf(rightSeperator, leftIndex);

                if (rightIndex == -1)
                    break;

                // Abschnitt gefunden
                Segment curSegment;
                curSegment.start = leftIndex;
                curSegment.ende = rightIndex;
                curSegment.text = text.Substring(leftIndex, rightIndex - leftIndex);
                result.Add(curSegment);

                idx = rightIndex;
            }

            return result;
        }

        public static String RemoveSpaces(String line)
        {
            String dummy = line.Replace("\n", "");
            dummy = dummy.Replace("\r", "");
            dummy = dummy.Replace(" ", "");
            return dummy;
        }

        public static String BuildImageFilename(String GcCode, Uri uri)
        {
            string imagePath = Config.GetString("DescriptionImageFolder") + "\\" + GcCode.Substring(0, 4);

            //String uriName = url.Substring(url.LastIndexOf('/') + 1);
            int idx = uri.AbsolutePath.LastIndexOf('.');
            String extension = (idx >= 0) ? uri.AbsolutePath.Substring(idx) : ".";

            return imagePath + "\\" + GcCode + Global.sdbm(uri.AbsolutePath).ToString() + extension;
        }

        public static String BuildOldImageFilename(String GcCode, Uri uri)
        {
            string imagePath = Config.GetString("DescriptionImageFolder") + "\\" + GcCode.Substring(0, 4);

            int idx = uri.AbsolutePath.LastIndexOf('.');
            String extension = (idx >= 0) ? uri.AbsolutePath.Substring(idx) : ".";

            return imagePath + "\\" + GcCode + uri.AbsolutePath.GetHashCode() + extension;
        }

        public static String BuildAdditionalImageFilename(String GcCode, string ImageName, Uri uri)
        {
            string imagePath = Config.GetString("SpoilerFolder") + "\\" + GcCode.Substring(0, 4);

            ImageName = Regex.Replace(ImageName, "[/:*?\"<>|]", string.Empty);
            ImageName = ImageName.Replace("\\", string.Empty);
            ImageName = ImageName.Replace("\r", string.Empty);
            ImageName = ImageName.Replace("\n", string.Empty);
            ImageName = ImageName.Trim();

            int idx = uri.AbsolutePath.LastIndexOf('.');
            String extension = (idx >= 0) ? uri.AbsolutePath.Substring(idx) : ".";

            return imagePath + "\\" + GcCode + " - " + ImageName + extension;
        }

        /**
         * Alte Version mit den Klammern als Eingrenzung des Hashs. Dies funktioniert nicht, da die Klammern nicht in URL's verwendet werden
         * dürfen (CBServer)
         */
        public static String BuildAdditionalImageFilenameHash(String GcCode, string ImageName, Uri uri)
        {
            string imagePath = Config.GetString("SpoilerFolder") + "\\" + GcCode.Substring(0, 4);

            ImageName = Regex.Replace(ImageName, "[/:*?\"<>|]", string.Empty);
            ImageName = ImageName.Replace("\\", string.Empty);
            ImageName = ImageName.Replace("\r", string.Empty);
            ImageName = ImageName.Replace("\n", string.Empty);
            ImageName = ImageName.Trim();

            int idx = uri.AbsolutePath.LastIndexOf('.');
            String extension = (idx >= 0) ? uri.AbsolutePath.Substring(idx) : ".";

            return imagePath + "\\" + GcCode + " - " + ImageName + " ([{" + Global.sdbm(uri.AbsolutePath) + "}])" + extension;
        }

        /**
         * Neue Version, mit @ als Eingrenzung des Hashs, da die Klammern nicht als URL's verwendet werden dürfen
         **/
        public static String BuildAdditionalImageFilenameHashNew(String GcCode, string ImageName, Uri uri)
        {
            string imagePath = Config.GetString("SpoilerFolder") + "\\" + GcCode.Substring(0, 4);

            ImageName = Regex.Replace(ImageName, "[/:*?\"<>|]", string.Empty);
            ImageName = ImageName.Replace("\\", string.Empty);
            ImageName = ImageName.Replace("\r", string.Empty);
            ImageName = ImageName.Replace("\n", string.Empty);
            ImageName = ImageName.Trim();

            int idx = uri.AbsolutePath.LastIndexOf('.');
            String extension = (idx >= 0) ? uri.AbsolutePath.Substring(idx) : ".";

            return imagePath + "\\" + GcCode + " - " + ImageName + " @" + Global.sdbm(uri.AbsolutePath) + "@" + extension;
        }

        public static String BuildCWAdditionalImageFilename(String GcCode, string ImageName, Uri uri)
        {
            string imagePath = Config.GetString("SpoilerFolder") + "\\" + GcCode.Substring(0, 4);

            ImageName = Regex.Replace(ImageName, "[/:*?\"<>|]", string.Empty);
            ImageName = ImageName.Replace("\\", string.Empty);
            ImageName = ImageName.Replace("\r", string.Empty);
            ImageName = ImageName.Replace("\n", string.Empty);
            ImageName = ImageName.Trim();

            int idx = uri.AbsolutePath.LastIndexOf('.');
            String extension = (idx >= 0) ? uri.AbsolutePath.Substring(idx) : ".";

            return imagePath + "\\" + GcCode + " - " + ImageName + " - " + Global.sdbm(uri.AbsolutePath).ToString() + extension;
        }

        public static String ResolveImages(Cache Cache, String html, bool suppressNonLocalMedia, out List<String> NonLocalImages, out List<String> NonLocalImagesUrl)
        {
            NonLocalImages = new List<string>();
            NonLocalImagesUrl = new List<string>();


            Uri baseUri;
            try
            {
                baseUri = new Uri(Cache.Url);
            }
            catch (Exception exc)
            {
#if DEBUG
                Global.AddLog("DescriptionImageGrabber.ResolveImages: failed to resolve '" + Cache.Url + "': " + exc.ToString());
#endif
                baseUri = null;
            }

            if (baseUri == null)
            {
                Cache.Url = "http://www.geocaching.com/seek/cache_details.aspx?wp=" + Cache.GcCode.ToString();
                try
                {
                    baseUri = new Uri(Cache.Url);
                }
                catch (Exception exc)
                {
#if DEBUG
                    Global.AddLog("DescriptionImageGrabber.ResolveImages: failed to resolve '" + Cache.Url + "': " + exc.ToString());
#endif
                    return html;
                }
            }

            String htmlNoSpaces = RemoveSpaces(html);

            List<Segment> imgTags = Segmentize(html, "<img", ">");

            int delta = 0;

            foreach (Segment img in imgTags)
            {
                int srcStart = -1;
                int srcEnd = -1;
                int srcIdx = img.text.ToLower().IndexOf("src=");
                if (srcIdx != -1) srcStart = img.text.IndexOf('"', srcIdx + 4);
                if (srcStart != -1) srcEnd = img.text.IndexOf('"', srcStart + 1);

                if (srcIdx != -1 && srcStart != -1 && srcEnd != -1)
                {
                    String src = img.text.Substring(srcStart + 1, srcEnd - srcStart - 1);
                    try
                    {
                        Uri imgUri = new Uri(baseUri, src);
                        String localFile = BuildImageFilename(Cache.GcCode, imgUri);
                        String oldLocalFile = BuildOldImageFilename(Cache.GcCode, imgUri);

                        if (File.Exists(localFile))
                        {
                            int idx = 0;

                            while ((idx = html.IndexOf(src, idx)) >= 0)
                            {
                                if (idx >= (img.start + delta) && (idx <= img.ende + delta))
                                {
                                    String head = html.Substring(0, img.start + delta);
                                    String tail = html.Substring(img.ende + delta);
                                    String uri = "file://" + localFile;
                                    String body = img.text.Replace(src, uri);

                                    delta += (uri.Length - src.Length);
                                    html = head + body + tail;
                                }
                                idx++;
                            }
                        }
                        else if (File.Exists(oldLocalFile))
                        {
                            // upgrade old file to new once.
                            File.Move(oldLocalFile, localFile);
                        }
                        else
                        {
                            NonLocalImages.Add(localFile);
                            NonLocalImagesUrl.Add(imgUri.ToString());

                            if (suppressNonLocalMedia)
                            {
                                // Wenn nicht-lokale Inhalte unterdrückt werden sollen,
                                // wird das <img>-Tag vollständig entfernt
                                html = html.Substring(0, img.start - 4 + delta) + html.Substring(img.ende + 1 + delta);
                                delta -= 5 + img.ende - img.start;
                            }
                        }
                    }
                    catch (Exception exc)
                    {
#if DEBUG
                        Global.AddLog("DescriptionImageGrabber.ResolveImages: failed to resolve relative uri. Base '" + baseUri + "', relative '" + src + "': " + exc.ToString());
#endif
                    }
                }
            }

            return html;
        }
    }
}
