using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using System.Globalization;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using WinCachebox.Geocaching;
using WinCachebox.Views;

namespace WinCachebox
{
    public class Global
    {
      #region Translations

      public static WinCachebox.LangStrings Translations;

        #endregion
        
        public static MapView.Route AktuelleRoute;

        public const string sCurrentRevision = "332";

        // Changes in the LatestDatabaseChange must increase the LatestDatabaseChangeWin too!!!!!!!!
        // because the Andorid Cachbox uses only the LatestDatabaseWin format!!!!!!!!
        public const int LatestDatabaseChange = 1027;
        public const int LatestDatabaseChangeWin = 1027;

        public const String RevisionSuffix = "";

        public static Int64 TrackDistance;

        public static FilterProperties LastFilter = null;
        public static FilterProperties PreviousFilter = null;
        public static Categories Categories = null;

        // Anzahl Caches, die angezeigt werden
        public static Int64 CacheCount = 0;

        /// <summary>
        /// Letzte Zeile des Bereiches
        /// </summary>
        public static Rectangle ViewArea = new Rectangle();

        /// <summary>
        /// Letzte bekannte Position
        /// </summary>
        public static Coordinate LastValidPosition = new Coordinate();

        /// <summary>
        /// Anzahl der übertragenen Bytes
        /// </summary>
        public static long TransferredBytes = 0;

        /// <summary>
        /// Zur Distanzberechnung verwendete Marke
        /// </summary>
        public static Coordinate Marker = new Coordinate();
        
        public static void SetMarker(Coordinate coord)
        {
          Marker = coord;
          CacheListView.View.RefreshDistances();
          CacheListView.View.Refresh();
          WaypointView.View.Refresh();
          MapView.View.Center = coord;
          MapView.View.Render(true);
        }

        public delegate void emptyDelegate();

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool GetDiskFreeSpaceEx(string lpDirectoryName,
           out ulong lpFreeBytesAvailable,
           out ulong lpTotalNumberOfBytes,
           out ulong lpTotalNumberOfFreeBytes);

        //[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        //public static extern int FindWindow(string lpClassName, string lpWindowName);
        const int SW_MAXIMIZE = 3;
        const int SW_MINIMIZE = 6;

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool ShowWindow(int hwnd, int nCmdShow);

        [DllImport("kernel32.dll", EntryPoint = "SetWindowPos", SetLastError = true)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        private static readonly IntPtr HWND_TOP = new IntPtr(0);
        private static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        private const UInt32 SWP_NOSIZE = 0x0001;
        private const UInt32 SWP_NOMOVE = 0x0002;
        private const UInt32 SWP_NOZORDER = 0x0004;
        private const UInt32 SWP_NOREDRAW = 0x0008;
        private const UInt32 SWP_NOACTIVATE = 0x0010;
        private const UInt32 SWP_FRAMECHANGED = 0x0020; /* The frame changed: send WM_NCCALCSIZE */
        private const UInt32 SWP_SHOWWINDOW = 0x0040;
        private const UInt32 SWP_HIDEWINDOW = 0x0080;
        private const UInt32 SWP_NOCOPYBITS = 0x0100;
        private const UInt32 SWP_NOOWNERZORDER = 0x0200; /* Don't do owner Z ordering */
        private const UInt32 SWP_NOSENDCHANGING = 0x0400; /* Don't send WM_WINDOWPOSCHANGING */

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool EnableWindow(int hwnd, bool enabled);
        const int SRCCOPY = 0xCC0020;

        [DllImport("kernel32.dll")]
        static extern IntPtr FindWindow(string class_name, string caption);

        [DllImport("kernel32.dll")]
        static extern int ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);

        public static Bitmap GetControlBitmap(Control control)
        {
            Bitmap img = new Bitmap(control.ClientSize.Width, control.ClientSize.Height);

            Graphics graphics = Graphics.FromImage(img);
            BitBlt(graphics.GetHdc(), 0, 0, control.ClientSize.Width, control.ClientSize.Height, control.CreateGraphics().GetHdc(), 0, 0, SRCCOPY);

            graphics.Dispose();

            return img;
        }

        /// <summary>
        /// Delegate des Handlers für das CacheChanged-Event
        /// </summary>
        /// <param name="sender">Location-Instanz, dessen Koordinaten
        /// sich geändert haben</param>
        public delegate void TargetChangedHandler(Cache cache, Waypoint waypoint);

        /// <summary>
        /// Ereignis, das bei veränderter Koordinate generiert wird.
        /// </summary>
        public static event TargetChangedHandler TargetChanged;

        /// <summary>
        /// true, falls die Applikation gestartet wurde und alles initialisiert ist
        /// </summary>
        public static bool Initialized = false;

        protected static Cache selectedCache = null;
        public static Cache SelectedCache
        {
            set
            {
                //if (selectedCache != value)
                //{
                selectedCache = value;
                selectedWaypoint = null;

                if (TargetChanged != null)
                    TargetChanged.Invoke(selectedCache, null);
                //}
            }
            get
            {
                return selectedCache;
            }
        }

        public static Boolean autoResort = false;

        protected static Cache nearestCache = null;
        public static Cache NearestCache
        {
            set
            {
                nearestCache = value;
            }
            get
            {
                return nearestCache;
            }
        }

        protected static Waypoint selectedWaypoint = null;
        public static Waypoint SelectedWaypoint
        {
            set
            {
                //if (selectedWaypoint != value)
                //{
                selectedWaypoint = value;

                if (TargetChanged != null)
                    TargetChanged.Invoke(selectedCache, selectedWaypoint);
                //}
            }
            get
            {
                return selectedWaypoint;
            }
        }

        // neue Funktion, um selectedCache und selectedWaypoint gleichzeitig zu setzen
        // Wenn ein Mystery-Cache in der Cacheliste selected wird, soll gleich der Final aktiv werden 
        public static void SetSelectedWaypoint(Cache cache, Waypoint waypoint)
        {
          selectedCache = cache;
          selectedWaypoint = waypoint;
          if (TargetChanged != null)
            TargetChanged.Invoke(selectedCache, selectedWaypoint);
        }

        /// <summary>
        /// Instanz des GPS-Parsers
        /// </summary>
        public static Locator.Locator Locator = null;

        /// <summary>
        /// Pfad des Verzeichnisses, in dem Cachebox ausgeführt wird
        /// </summary>
        public static String AppPath = Path.GetDirectoryName(Application.ExecutablePath);
        public static String configFileName = "\\wincachebox.config";
        public static String databaseName;
        //public static String AppPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\WinCachebox";
        public static String ExePath = Path.GetDirectoryName(Application.ExecutablePath);


        /// <summary>
        /// Prüft, ob eine Internetverbindung besteht
        /// </summary>
        /// <returns>true, falls eine Internetverbindung besteht, sonst false</returns>
        public bool CheckInternetConnectivity()
        {
            bool ret = false;

            try
            {
                string hostName = System.Net.Dns.GetHostName();

                System.Net.IPHostEntry hostEntry = System.Net.Dns.GetHostEntry(hostName);
                string hostIPAdd = hostEntry.AddressList[0].ToString();

                ret = hostIPAdd != System.Net.IPAddress.Parse("127.0.0.1").ToString();
            }
            catch
            {
                return false;
            }

            return ret;
        }

        // Zeichenprimitive
        public static Pen blackPen = new Pen(Color.Black);
        public static Brush blackBrush = new SolidBrush(Color.Black);
        public static Brush backBrushSelected = new SolidBrush(Color.Wheat);
        public static Brush backBrushHead = new SolidBrush(Color.FromArgb(201, 233, 206));
        public static Brush backBrushControl = new SolidBrush(Color.FromArgb(140, 158, 101));
        public static Font boldFont = new Font(FontFamily.GenericSansSerif, 9, FontStyle.Bold);
        public static Font normalFont = new Font(FontFamily.GenericSansSerif, 9, FontStyle.Regular);

        public static Brush whiteBrush = new SolidBrush(Color.White);

        public static List<Bitmap> LogIcons = new List<Bitmap>();
        public static List<Bitmap> CacheIconsBig = new List<Bitmap>();
        public static List<Bitmap> CacheIconsBigFound = new List<Bitmap>();
        public static List<Bitmap> CacheIconsSolvedMystery = new List<Bitmap>();
        public static Bitmap[] Icons = null;
        public static List<Bitmap> MapIcons = new List<Bitmap>();
        public static Bitmap[] SmallStarIcons;
        public static List<Bitmap> BatteryIcons = new List<Bitmap>();
        public static List<Bitmap> StarIcons = new List<Bitmap>();
        public static List<Bitmap> ContainerSizeIcons = new List<Bitmap>();
        public static List<List<Bitmap>> NewMapIcons = new List<List<Bitmap>>();
        public static List<List<Bitmap>> NewMapOverlay = new List<List<Bitmap>>();


        public static Bitmap LoadRessourceBitmap(String name)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string strRes = "WinCachebox.icons." + name;
            Stream stream = assembly.GetManifestResourceStream(strRes);
            return new Bitmap(stream);
        }

        private static void ReadWriteStream(Stream readStream, Stream writeStream)
        {
            int Length = 256;
            Byte[] buffer = new Byte[Length];
            int bytesRead = readStream.Read(buffer, 0, Length);

            while (bytesRead > 0)
            {
                writeStream.Write(buffer, 0, bytesRead);
                bytesRead = readStream.Read(buffer, 0, Length);
            }
            readStream.Close();
            writeStream.Close();
        }

        public static void SaveRessourceBitmap(String name, String saveFilename)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string strRes = "Cachebox.icons." + name;
            Stream stream = assembly.GetManifestResourceStream(strRes);

            FileStream writeStream = new FileStream(saveFilename, FileMode.Create, FileAccess.Write);
            ReadWriteStream(stream, writeStream);
        }

        public static void InitIcons()
        {
            // NewMapIcons[0] contains the 8x8 Bitmaps
            NewMapIcons.Add(new List<Bitmap>());
            NewMapIcons[0].Add(LoadRessourceBitmap("Map_8x8_Green.png"));
            NewMapIcons[0].Add(LoadRessourceBitmap("Map_8x8_Yellow.png"));
            NewMapIcons[0].Add(LoadRessourceBitmap("Map_8x8_Red.png"));
            NewMapIcons[0].Add(LoadRessourceBitmap("Map_8x8_White.png"));
            NewMapIcons[0].Add(LoadRessourceBitmap("Map_8x8_Blue.png"));
            NewMapIcons[0].Add(LoadRessourceBitmap("Map_8x8_Violet.png"));
            NewMapIcons[0].Add(LoadRessourceBitmap("Map_8x8_Found.png"));
            NewMapIcons[0].Add(LoadRessourceBitmap("Map_8x8_Own.png"));
            // NewMapIcons[1] contains the 13x13 Bitmaps
            NewMapIcons.Add(new List<Bitmap>());
            NewMapIcons[1].Add(LoadRessourceBitmap("Map_13x13_Green.png"));
            NewMapIcons[1].Add(LoadRessourceBitmap("Map_13x13_Yellow.png"));
            NewMapIcons[1].Add(LoadRessourceBitmap("Map_13x13_Red.png"));
            NewMapIcons[1].Add(LoadRessourceBitmap("Map_13x13_White.png"));
            NewMapIcons[1].Add(LoadRessourceBitmap("Map_13x13_Blue.png"));
            NewMapIcons[1].Add(LoadRessourceBitmap("Map_13x13_Violet.png"));
            NewMapIcons[1].Add(LoadRessourceBitmap("Map_13x13_Found.png"));
            NewMapIcons[1].Add(LoadRessourceBitmap("Map_13x13_Own.png"));
            // NewMapIcons[2] contains the normal 20x20 Bitmaps
            NewMapIcons.Add(new List<Bitmap>());
            for (int i = 0; i <= 22; i++)
                NewMapIcons[2].Add(LoadRessourceBitmap("Map_20x20_" + i.ToString() + ".png"));

            // Overlays for Icons
            NewMapOverlay.Add(new List<Bitmap>());  // 8x8
            NewMapOverlay[0].Add(LoadRessourceBitmap("Map_8x8_ShaddowRect.png"));
            NewMapOverlay[0].Add(LoadRessourceBitmap("Map_8x8_ShaddowRound.png"));
            NewMapOverlay[0].Add(LoadRessourceBitmap("Map_8x8_ShaddowStar.png"));
            NewMapOverlay[0].Add(LoadRessourceBitmap("Map_8x8_Strikeout.png"));

            NewMapOverlay.Add(new List<Bitmap>());  // 13x13
            NewMapOverlay[1].Add(LoadRessourceBitmap("Map_13x13_ShaddowRect.png"));
            NewMapOverlay[1].Add(LoadRessourceBitmap("Map_13x13_ShaddowRound.png"));
            NewMapOverlay[1].Add(LoadRessourceBitmap("Map_13x13_ShaddowStar.png"));
            NewMapOverlay[1].Add(LoadRessourceBitmap("Map_13x13_Strikeout.png"));

            NewMapOverlay.Add(new List<Bitmap>());  // 20x20
            NewMapOverlay[2].Add(LoadRessourceBitmap("Map_20x20_ShaddowRect.png"));
            NewMapOverlay[2].Add(LoadRessourceBitmap("Map_20x20_Selected.png"));
            NewMapOverlay[2].Add(LoadRessourceBitmap("Map_20x20_ShaddowRect_Deact.png"));
            NewMapOverlay[2].Add(LoadRessourceBitmap("Map_20x20_Selected_Deact.png"));


            ContainerSizeIcons.Add(LoadRessourceBitmap("other.gif"));
            ContainerSizeIcons.Add(LoadRessourceBitmap("micro.gif"));
            ContainerSizeIcons.Add(LoadRessourceBitmap("small.gif"));
            ContainerSizeIcons.Add(LoadRessourceBitmap("regular.gif"));
            ContainerSizeIcons.Add(LoadRessourceBitmap("large.gif"));

            for (int i = 0; i <= 4; i++)
                BatteryIcons.Add(LoadRessourceBitmap("bat" + i.ToString() + ".png"));

            for (int i = 0; i <= 18; i++)
                CacheIconsBig.Add(LoadRessourceBitmap("big-" + i.ToString() + ".png"));

            for (int i = 0; i <= 10; i++)
                CacheIconsBigFound.Add(LoadRessourceBitmap("big-" + i.ToString() + "-found.png"));

            for (int i = 0; i <= 13; i++)
                LogIcons.Add(LoadRessourceBitmap("log" + i.ToString() + ".png"));

            for (int i = 0; i <= 22; i++)
                MapIcons.Add(LoadRessourceBitmap("map" + i.ToString() + ".png"));

            CacheIconsSolvedMystery.Add(LoadRessourceBitmap("big-2-solved.png"));
            CacheIconsSolvedMystery.Add(LoadRessourceBitmap("big-2-solved-found.png"));

            StarIcons.Add(LoadRessourceBitmap("stars0.gif"));
            StarIcons.Add(LoadRessourceBitmap("stars0_5.gif"));
            StarIcons.Add(LoadRessourceBitmap("stars1.gif"));
            StarIcons.Add(LoadRessourceBitmap("stars1_5.gif"));
            StarIcons.Add(LoadRessourceBitmap("stars2.gif"));
            StarIcons.Add(LoadRessourceBitmap("stars2_5.gif"));
            StarIcons.Add(LoadRessourceBitmap("stars3.gif"));
            StarIcons.Add(LoadRessourceBitmap("stars3_5.gif"));
            StarIcons.Add(LoadRessourceBitmap("stars4.gif"));
            StarIcons.Add(LoadRessourceBitmap("stars4_5.gif"));
            StarIcons.Add(LoadRessourceBitmap("stars5.gif"));

            Icons = new Bitmap[] { LoadRessourceBitmap("bug.png"),
                                   LoadRessourceBitmap("addwaypoint.png"),
                                   LoadRessourceBitmap("icon_smile.gif"),
                                   LoadRessourceBitmap("download.png"),
                                   LoadRessourceBitmap("icon_sad.gif"),
                                   LoadRessourceBitmap("maintenance.png"),
                                   LoadRessourceBitmap("checkbox-checked.png"),
                                   LoadRessourceBitmap("checkbox-unchecked.png"),
                                   LoadRessourceBitmap("sonne.png"),
                                   LoadRessourceBitmap("mond.png"),
                                   LoadRessourceBitmap("travelbug.png"),
                                   LoadRessourceBitmap("collapse.png"),
                                   LoadRessourceBitmap("expand.png"),
                                   LoadRessourceBitmap("enabled.png"),
                                   LoadRessourceBitmap("disabled.png"),
                                   LoadRessourceBitmap("retrieve_tb.png"),
                                   LoadRessourceBitmap("drop_tb.png"),
                                   LoadRessourceBitmap("star.png"),
                                   LoadRessourceBitmap("earth.png"),
                                   LoadRessourceBitmap("favorit.png"),
                                   LoadRessourceBitmap("file.png"),
                                   LoadRessourceBitmap("UserData.jpg"),
                                   LoadRessourceBitmap("delete.jpg"),  // 22
                                   LoadRessourceBitmap("archiv.png"), // 23
                                   LoadRessourceBitmap("not_available.jpg"),  // 24
                                   LoadRessourceBitmap("checkbox-crossed.png"), // 25
                                   LoadRessourceBitmap("map22.png"), // 26
                                   LoadRessourceBitmap("filepinned.png"), // 27
            };

            SmallStarIcons = new Bitmap[] { LoadRessourceBitmap("smallstars-0.png"),
                                            LoadRessourceBitmap("smallstars-0_5.png"),
                                            LoadRessourceBitmap("smallstars-1.png"),
                                            LoadRessourceBitmap("smallstars-1_5.png"),
                                            LoadRessourceBitmap("smallstars-2.png"),
                                            LoadRessourceBitmap("smallstars-2_5.png"),
                                            LoadRessourceBitmap("smallstars-3.png"),
                                            LoadRessourceBitmap("smallstars-3_5.png"),
                                            LoadRessourceBitmap("smallstars-4.png"),
                                            LoadRessourceBitmap("smallstars-4_5.png"),
                                            LoadRessourceBitmap("smallstars-5.png") };
        }

        public static int PutImageTargetHeight(Graphics graphics, Image image, int x, int y, int height)
        {
            return PutImageTargetHeight(graphics, image, x, y, height, null);
        }

        /// <summary>
        /// Zeichnet das Bild und skaliert es proportional so, dass es die
        /// übergebene füllt.
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="image"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="height"></param>
        public static int PutImageTargetHeight(Graphics graphics, Image image, int x, int y, int height, System.Drawing.Imaging.ImageAttributes attribute)
        {
            float scale = (float)height / (float)image.Height;
            int width = (int)Math.Round(image.Width * scale);

            Rectangle dstRect = new Rectangle(x, y, width, height);

            if (attribute == null)
            {
                Rectangle srcRect = new Rectangle(0, 0, image.Width, image.Height);
                graphics.DrawImage(image, dstRect, srcRect, GraphicsUnit.Pixel);
            }
            else
                graphics.DrawImage(image, dstRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attribute);

            return width;
        }

        public static void DrawListBackground(Graphics graphics, Rectangle cellBounds, bool selected, bool currentRow, Color mixColor)
        {
            Color back = Color.White;
            if (selected)
                back = Color.FromArgb(195, 225, 255);
            if (mixColor != Color.White)
            {
              byte A = (byte)(back.A - (mixColor.A - back.A) / 2);
              byte R = (byte)(back.R - (mixColor.R - back.R) / 2);
              byte G = (byte)(back.G - (mixColor.G - back.G) / 2);
              byte B = (byte)(back.B - (mixColor.B - back.B) / 2);
              back = Color.FromArgb(A, R, G, B);
            }
            graphics.FillRectangle(new SolidBrush(back), cellBounds);
            graphics.DrawLine(Pens.Silver, cellBounds.X, cellBounds.Y + cellBounds.Height - 1, cellBounds.X + cellBounds.Width, cellBounds.Y + cellBounds.Height - 1);
            graphics.DrawLine(Pens.Silver, cellBounds.X + cellBounds.Width - 1, cellBounds.Y, cellBounds.X + cellBounds.Width - 1, cellBounds.Y + cellBounds.Height - 1);

            if (currentRow)
            {
                graphics.DrawLine(Pens.Black, cellBounds.X, cellBounds.Y, cellBounds.X + cellBounds.Width, cellBounds.Y);
                graphics.DrawLine(Pens.Black, cellBounds.X, cellBounds.Y+1, cellBounds.X + cellBounds.Width, cellBounds.Y+1);
                graphics.DrawLine(Pens.Black, cellBounds.X, cellBounds.Y + cellBounds.Height - 1, cellBounds.X + cellBounds.Width, cellBounds.Y + cellBounds.Height - 1);
                graphics.DrawLine(Pens.Black, cellBounds.X, cellBounds.Y + cellBounds.Height - 2, cellBounds.X + cellBounds.Width, cellBounds.Y + cellBounds.Height - 2);
            }
        }

        /// <summary>
        /// Führt eine Rot13-Kodierung durch
        /// </summary>
        /// <param name="message">Zu ver- bzw. entschlüsselnder Text</param>
        /// <returns></returns>
        public static String Rot13(String message)
        {
            String alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            String lookup = "nopqrstuvwxyzabcdefghijklmNOPQRSTUVWXYZABCDEFGHIJKLM";

            String result = String.Empty;

            for (int i = 0; i < message.Length; i++)
            {
                String curChar = message.Substring(i, 1);
                int idx = alphabet.IndexOf(curChar);

                if (idx < 0)
                    result += curChar;
                else
                    result += lookup.Substring(idx, 1);
            }

            return result;
        }

        public static Control GetTopMostParent(Control ctrl)
        {
            while (ctrl.Parent != null)
                ctrl = ctrl.Parent;
            return ctrl;
        }

        class LockClass { };
        static LockClass lockObject = new LockClass();

        /// <summary>
        /// Fügt die übergebene Zeile in debug.txt ein
        /// </summary>
        /// <param name="line">anzuhängener Text</param>
        public static void AddLog(String line)
        {
            lock (lockObject)
            {
                StreamWriter sw = File.AppendText(Global.AppPath + "\\debug.txt");
                sw.WriteLine(DateTime.Now.ToShortTimeString() + " " + line);
                sw.Close();
            }
        }

        /// <summary>
        /// Liefert die Anzahl der freien Bytes auf dem Datenträger
        /// </summary>
        /// <returns>Anzahl der freien Bytes</returns>
        public static ulong GetAvailableDiscSpace(String path)
        {
            ulong available = 0;
            ulong total = 0;
            ulong totalFree = 0;
            GetDiskFreeSpaceEx(path, out available, out total, out totalFree);

            return available;
        }

        /// <summary>
        /// Formatiert die übergebene Anzahl an Bytes in ein menschen lesbares Format
        /// </summary>
        /// <param name="bytes">Anzahl der Bytes</param>
        /// <returns>Ausgabe, etwa 3.67 kb</returns>
        public static String GetLengthString(long bytes)
        {
            if (bytes < 1024)
                return bytes.ToString() + " bytes";

            double length = (double)bytes / 1024.0;

            if (length < 1024)
                return String.Format(NumberFormatInfo.InvariantInfo, "{0:0.00} kb", length);

            length /= 1024;
            return String.Format(NumberFormatInfo.InvariantInfo, "{0:0.00} mb", length);

        }

        static String FormatDM(double coord, String positiveDirection, String negativeDirection)
        {
            int deg = (int)coord;
            double frac = coord - deg;
            double min = frac * 60;

            String result = Math.Abs(deg).ToString() + "° " + String.Format(NumberFormatInfo.InvariantInfo, "{0:0.000}'", Math.Abs(min));

            if (coord < 0)
                result += negativeDirection;
            else
                result += positiveDirection;

            return result;
        }

        static public String FormatLatitudeDM(double latitude)
        {
            return FormatDM(latitude, "N", "S");
        }

        static public String FormatLongitudeDM(double longitude)
        {
            return FormatDM(longitude, "E", "W");
        }

        static System.Windows.Forms.Timer timerPreventPowerDown = null;

        /// <summary>
        /// Ermöglicht Power down oder eben nicht
        /// </summary>
        /// <param name="state">true, falls das Abschalten unterdrückt werden soll, sonst false</param>
        public static void SetPowerDown(bool state)
        {
            if (state)
            {
                timerPreventPowerDown = new System.Windows.Forms.Timer
                {
                    Interval = 15000
                };
                timerPreventPowerDown.Tick += new EventHandler(timerPreventPowerDown_Tick);
                timerPreventPowerDown.Enabled = true;
            }
            else
            {
                if (timerPreventPowerDown != null)
                {
                    timerPreventPowerDown.Enabled = state;
                    timerPreventPowerDown.Dispose();
                }
            }
        }

        [DllImport("kernel32.dll")]
        static extern void SystemIdleTimerReset();

        static void timerPreventPowerDown_Tick(object sender, EventArgs e)
        {
            SystemIdleTimerReset();
        }

//        public static Thread threadGps = null;
        public static void StartGps()
        {

/*            threadGps = new Thread(new ThreadStart(threadStartGps));
            threadGps.Priority = ThreadPriority.Normal;
            threadGps.Start();*/
        }
      /*
        static void threadStartGps()
        {
            Locator = null;

            Locator = new Locator.CelltowerLocator();
            Locator.Open();

            if (Config.GetBool("GpsScanStarted"))
            {
                if (MessageBox.Show("Previous scan for GPS-devices failed. Do you want to skip it?", "Skip GPS scanning?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    Config.Set("GpsDriver", "none");
                    Config.Set("GpsScanStarted", false);
                    Config.AcceptChanges();
                }
                else
                {
                    Config.Set("GpsDriver", "");
                    Config.Set("GpsScanStarted", false);
                    Config.AcceptChanges();
                }
            }

            String driver = Config.GetString("GpsDriver");

            if (driver == "" || (driver.ToLower() == "nmea" && Config.GetString("GpsSettings") == ""))
            {
                // Keine Treibereinstellung gefunden! Suchen!
                try
                {
                    Locator = new Locator.WidLocator();
                    if (Locator.Scan())
                    {
                        Config.Set("GpsDriver", "WindowsIntermediate");
                        Config.AcceptChanges();
                        Global.Locator.Open();
                        return;
                    }
                }
                catch (Exception exc)
                {
#if DEBUG
                    Global.AddLog("StartGps(): Cannot access WID: " + exc.ToString());
#endif
                }
                finally
                {
                    if (Locator != null)
                        Locator.Dispose();
                }

                Locator = new Locator.NmeaLocator();
                if (Global.Locator.Scan())
                {
                    Config.Set("GpsDriver", "Nmea");
                    Config.Set("GpsSettings", Locator.ConnectionString);
                    Config.AcceptChanges();
                    Locator.Open();
                    return;
                }

                Locator = new Locator.DummyLocator();
                return;
            }

            // Treiber bekannt! Diesen laden!
            if (driver.ToLower() == "windowsintermediate")
            {
                Locator = new Locator.WidLocator();
                Locator.Open();
                return;
            }

            if (driver.ToLower() == "nmea")
            {
                Locator = new Locator.NmeaLocator();
                Locator.Open(Config.GetString("GpsSettings"));
                return;
            }

            if (driver.ToLower() == "none")
            {
                Locator = new Locator.DummyLocator();
                return;
            }

            Global.AddLog("Unknown GPS driver: " + driver);
            Locator = new Locator.DummyLocator();
        }
      */
        public static void HideApplication(Form form)
        {

            Global.ShowWindow((int)form.Handle, 6);

            // Das HWND stimmt schon, aber
            //   - auf y=0 setzen führt dazu, dass es unter dem Systemmenu
            //     hängt
            //   - irgendwie wird Cachebox immer wieder in den Fordergrund gebracht
            //IntPtr hWnd = FindWindow("DesktopExplorerWindow", "Desktop");
            //Global.SetWindowPos(hWnd, HWND_BOTTOM, 0, 0, form.Width, form.Height, SWP_SHOWWINDOW);
        }

        public static String RemoveInvalidFatChars(String str)
        {
            String[] invalidChars = new String[] { ":", "\\", "/", "<", ">", "?", "*", "|", "\"", ";" };

            for (int i = 0; i < invalidChars.Length; i++)
                str = str.Replace(invalidChars[i], "");

            return str;
        }

        /// <summary>
        /// Reads data from a stream until the end is reached. The
        /// data is returned as a byte array. An IOException is
        /// thrown if any of the underlying IO calls fail.
        /// </summary>
        /// <param name="stream">The stream to read data from</param>
        /// <param name="initialLength">The initial buffer length</param>
        public static byte[] ReadFully(Stream stream, int initialLength)
        {
            // If we've been passed an unhelpful initial length, just
            // use 32K.
            if (initialLength < 1)
            {
                initialLength = 32768;
            }

            byte[] buffer = new byte[initialLength];
            int read = 0;

            int chunk;
            while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
            {
                read += chunk;

                // If we've reached the end of our buffer, check to see if there's
                // any more information
                if (read == buffer.Length)
                {
                    int nextByte = stream.ReadByte();

                    // End of stream? If so, we're done
                    if (nextByte == -1)
                    {
                        return buffer;
                    }

                    // Nope. Resize the buffer, put in the byte we've just
                    // read, and continue
                    byte[] newBuffer = new byte[buffer.Length * 2];
                    Array.Copy(buffer, newBuffer, buffer.Length);
                    newBuffer[read] = (byte)nextByte;
                    buffer = newBuffer;
                    read++;
                }
            }
            // Buffer is now too big. Shrink it.
            byte[] ret = new byte[read];
            Array.Copy(buffer, ret, read);

            stream.Close();

            return ret;
        }

        /// <summary>
        ///     SDBM-Hash algorithm for storing hash values into the database. This is neccessary to be compatible to the CacheBox@Home project. Because the
        ///     standard .net Hash algorithm differs from compact edition to the normal edition.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static UInt32 sdbm(String str)
        {
            UInt32 hash = 0;

            foreach (char c in str)
            {
                hash = c + (hash << 6) + (hash << 16) - hash;
            }

            return hash;
        }

        public class SYSTEM_POWER_STATUS_EX
        {
            public byte ACLineStatus;
            public byte BatteryFlag;
            public byte BatteryLifePercent;
            public byte Reserved1;
            public uint BatteryLifeTime;
            public uint BatteryFullLifeTime;
            public byte Reserved2;
            public byte BackupBatteryFlag;
            public byte BackupBatteryLifePercent;
            public byte Reserved3;
            public uint BackupBatteryLifeTime;
            public uint BackupBatteryFullLifeTime;
        }

        // Nach http://msdn.microsoft.com/en-us/library/ms172518.aspx
        // Windows CE compliant
        [DllImport("kernel32")]
        public static extern uint GetSystemPowerStatusEx(SYSTEM_POWER_STATUS_EX lpSystemPowerStatus, bool fUpdate);

        public static System.Net.IWebProxy Proxy = null;

        public struct MEMORYSTATUS
        {
            public UInt32 dwLength;
            public UInt32 dwMemoryLoad;
            public UInt32 dwTotalPhys;
            public UInt32 dwAvailPhys;
            public UInt32 dwTotalPageFile;
            public UInt32 dwAvailPageFile;
            public UInt32 dwTotalVirtual;
            public UInt32 dwAvailVirtual;
        }

        [DllImport("kernel32")]
        public static extern void GlobalMemoryStatus(ref MEMORYSTATUS buf);

        [DllImport("kernel32.dll")]
        public static extern int GetSystemMemoryDivision
        (
            ref UInt32 lpdwStorePages,
            ref UInt32 lpdwRamPages,
            ref UInt32 lpdwPageSize
        );

        public static long GetAvailableMemory()
        {
            MEMORYSTATUS memStatus = new MEMORYSTATUS();
            GlobalMemoryStatus(ref memStatus);

            return (long)memStatus.dwAvailPhys;
        }

        public static void AddMemoryLog()
        {
            MEMORYSTATUS memStatus = new MEMORYSTATUS();
            GlobalMemoryStatus(ref memStatus);

            AddLog("Memory Log:\n    Memory Load: " + memStatus.dwMemoryLoad.ToString() +
                "\n    Total Physical: " + memStatus.dwTotalPhys.ToString() +
                "\n    Available Physical: " + memStatus.dwAvailPhys.ToString() +
                "\n    Total Page File: " + memStatus.dwTotalPageFile.ToString() +
                "\n    Available Page File: " + memStatus.dwAvailPageFile.ToString() +
                "\n    Total Virtual: " + memStatus.dwTotalVirtual.ToString() +
                "\n    Available Virtual: " + memStatus.dwAvailVirtual.ToString() + "\n\n");
        }

        /// <summary>
        /// The AlphaBlend function displays bitmaps that have transparent or semitransparent pixels.
        /// </summary>
        /// <param name="hdcDest">[in] Handle to the destination device context.</param>
        /// <param name="xDest">[in] Specifies the x-coordinate, in logical units, of the upper-left corner of the destination rectangle.</param>
        /// <param name="yDest">[in] Specifies the y-coordinate, in logical units, of the upper-left corner of the destination rectangle.</param>
        /// <param name="cxDest">[in] Specifies the width, in logical units, of the destination rectangle.</param>
        /// <param name="cyDest">[in] Specifies the height, in logical units, of the destination rectangle.</param>
        /// <param name="hdcSrc">[in] Handle to the source device context.</param>
        /// <param name="xSrc">[in] Specifies the x-coordinate, in logical units, of the upper-left corner of the source rectangle.</param>
        /// <param name="ySrc">[in] Specifies the y-coordinate, in logical units, of the upper-left corner of the source rectangle.</param>
        /// <param name="cxSrc">[in] Specifies the width, in logical units, of the source rectangle.</param>
        /// <param name="cySrc">[in] Specifies the height, in logical units, of the source rectangle.</param>
        /// <param name="blendFunction">[in] Specifies the alpha-blending function for source and destination bitmaps, a global alpha value to be applied to the entire source bitmap, and format information for the source bitmap. The source and destination blend functions are currently limited to AC_SRC_OVER. See the BLENDFUNCTION and EMRALPHABLEND structures.</param>
        /// <returns>If the function succeeds, the return value is TRUE.  If the function fails, the return value is FALSE. </returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool AlphaBlend(
            IntPtr hdcDest,
            int xDest,
            int yDest,
            int cxDest,
            int cyDest,
            IntPtr hdcSrc,
            int xSrc,
            int ySrc,
            int cxSrc,
            int cySrc,
            BlendFunction blendFunction);

        /// <summary>
        /// This structure controls blending by specifying the blending functions for source and destination bitmaps.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct BlendFunction
        {
            /// <summary>
            /// Specifies the source blend operation. Currently, the only source and destination blend operation that has been defined is AC_SRC_OVER. For details, see the following Remarks section.
            /// </summary>
            public byte BlendOp;

            /// <summary>
            /// Must be zero.
            /// </summary>
            public byte BlendFlags;

            /// <summary>
            /// Specifies an alpha transparency value to be used on the entire source bitmap. The SourceConstantAlpha value is combined with any per-pixel alpha values in the source bitmap. If you set SourceConstantAlpha to 0, it is assumed that your image is transparent. When you only want to use per-pixel alpha values, set the SourceConstantAlpha value to 255 (opaque) .
            /// </summary>
            public byte SourceConstantAlpha;

            /// <summary>
            /// This member controls the way the source and destination bitmaps are interpreted. The following table shows the value for AlphaFormat.
            /// ---
            /// AC_SRC_ALPHA   This flag is set when the bitmap has an Alpha channel (that is, per-pixel alpha). Because this API uses premultiplied alpha, the red, green and blue channel values in the bitmap must be premultiplied with the alpha channel value. For example, if the alpha channel value is x, the red, green and blue channels must be multiplied by x and divided by 0xff before the call.
            /// </summary>
            public byte AlphaFormat;

            /// <summary>
            ///     Initializes a new instance of the <see cref="BlendFunction"/> structure.
            /// </summary>
            /// <param name="alphaConst">Specifies an alpha transparency value to be used on the entire source bitmap.
            /// </param>
            /// <param name="alphaFormat">Alpha flag
            /// </param>
            public BlendFunction(byte alphaConst, byte alphaFormat)
            {
                this.BlendOp = 0;
                this.BlendFlags = 0;
                this.SourceConstantAlpha = alphaConst;
                this.AlphaFormat = alphaFormat;
            }            
        }
        public static string DecimalSeparator
        {
          get { return CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.ToString(); } 
        }
       
        public static NumberFormatInfo NumberFormatInfo
        {
          get
          {
            return CultureInfo.CurrentCulture.NumberFormat;
          }
        }
    }
}
