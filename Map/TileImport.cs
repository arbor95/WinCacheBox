using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Windows.Forms;

namespace WinCachebox.Map
{
    public class TileImport
    {
        /// <summary>
        /// Anzahl der on the fly geladenen Kacheln
        /// </summary>
        public static int LoadedTiles = 0;

        /// <summary>
        /// Zoomstufe, auf der die Kartenberechnungen durchgeführt werden
        /// </summary>
        public const int MapZoom = 18;

        /// <summary>
        /// Maximale Zoomstufe, die vorgeladen werden soll
        /// </summary>
        public int MaxZoom = 15;

        /// <summary>
        /// Minimale Zoomstufe, die vorgeladen werden soll
        /// </summary>
        public int MinZoom = 5;

        ///// <summary>
        ///// Hashtabelle mit zu ladenden Kacheln
        ///// </summary>
        //protected Dictionary<Descriptor, bool> tilesToFetch = new Dictionary<Descriptor, bool>();

        ///// <summary>
        ///// Liste mit zu ladenden Kacheln. Wird von Worker-Threads geleert.
        ///// </summary>
        //Descriptor[] loaderList = null;

        ///// <summary>
        ///// Anzahl insgesamt zu ladender Kacheln
        ///// </summary>
        //int numTiles = 0;

        /// <summary>
        /// Anzahl fehlgeschlagener Kachelanforderungen
        /// </summary>
        static int numFails = 0;

        int osmCoverage = 1000;

        FormImportPocketQuery parent;

        int numThreads = 2;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="parent"></param>
        public TileImport(FormImportPocketQuery parent)
        {
            this.parent = parent;
            MaxZoom = Config.GetInt("OsmMaxImportLevel");
            MinZoom = Config.GetInt("OsmMinLevel");
            osmCoverage = Config.GetInt("OsmCoverage");
        }

        int loaderListIndex = 0;

        /// <summary>
        /// Yo hier gehts los
        /// </summary>
        public void Import()
        {
            numThreads = 0;
            numFails = 0;

            Thread[] loaderThreads = new Thread[2];

            initTileIterator();

            if (Config.GetBool("CacheMapData"))
            {
                for (int i = 0; i < loaderThreads.Length; i++)
                    try
                    {
#if DEBUG
                        Global.AddLog("TileImport.loaderThreadEntryPoint: Starting Thread " + i.ToString());
#endif
                        loaderThreads[i] = new Thread(new ThreadStart(loaderThreadEntryPoint));
                        loaderThreads[i].Start();
                    }
                    catch (Exception exc)
                    {
#if DEBUG
                        Global.AddLog("TileImport.loaderThreadEntryPoint: Cannot create thread: " + exc.ToString());
#endif
                    }

                loaderThreadEntryPoint();

                while (!parent.Cancel && numThreads > 0)
                    Thread.Sleep(1000);

                for (int i = 0; i < loaderThreads.Length; i++)
                    if (loaderThreads[i] != null)
                        loaderThreads[i].Abort();

                disposeTileIterator();
            }
        }


        //        void deleteUnusedTiles()
        //        {
        //            if (!Directory.Exists(Global.AppPath + "\\Repository\\Tiles"))
        //                return;

        //            String[] dirs = Directory.GetDirectories(Global.AppPath + "\\Repository\\Tiles");

        //            for (int i = 0; i < dirs.Length; i++)
        //            {
        //                if (parent.Cancel)
        //                    break;

        //                parent.ProgressChanged("Deleting unused tiles", i + 1, dirs.Length);

        //                String[] files = Directory.GetFiles(dirs[i]);
        //                foreach (String file in files)
        //                {
        //                    try
        //                    {
        //                        int y = int.Parse(dirs[i].Substring(dirs[i].LastIndexOf("\\") + 1));
        //                        String filename = file.Substring(file.LastIndexOf("\\") + 1);
        //                        int ldot = filename.IndexOf("_");
        //                        int rdot = filename.LastIndexOf(".");
        //                        int zoom = int.Parse(filename.Substring(0, ldot));
        //                        int x = int.Parse(filename.Substring(ldot + 1, rdot - ldot - 1));

        //                        Descriptor tile = new Descriptor(x, y, zoom);
        //                        if (tilesToFetch.ContainsKey(tile))
        //                        {
        //                            tilesToFetch.Remove(tile);
        //                            continue;
        //                        }

        //                        // Kachel wird nicht gebraucht und kann gelöscht werden
        //                        File.Delete(file);
        //                    }
        //                    catch (Exception exc)
        //                    {
        //#if DEBUG
        //                        Global.AddLog("TileImport.deleteUnusedTiles: " + exc.ToString() + "\n\n");
        //#endif
        //                    }
        //                }
        //            }
        //        }

        int numTotalCaches = 0;
        int currentCacheIndex = 0;
        int layerIndex = 0;
        DbDataReader iteratorReader = null;
        List<Layer> layersToImport = null;

        void initTileIterator()
        {
            layersToImport = new List<Layer>();

            if (Config.GetBool("ImportLayerOsm"))
                layersToImport.Add(WinCachebox.Views.MapView.Manager.GetLayerByName("Mapnik", "Mapnik", ""));

            if (Config.GetBool("ImportLayerOTM"))
                layersToImport.Add(WinCachebox.Views.MapView.Manager.GetLayerByName("OpenTopoMap", "OpenTopoMap", ""));

            if (Config.GetBool("ImportLayerOcm"))
                layersToImport.Add(WinCachebox.Views.MapView.Manager.GetLayerByName("OSM Cycle Map", "Open Cycle Map", ""));

            if (Config.GetBool("ImportLayerMQ"))
                layersToImport.Add(WinCachebox.Views.MapView.Manager.GetLayerByName("Stamen", "Stamen", ""));

            currentCacheIndex = 0;
            layerIndex = 0;


            string where = Global.LastFilter.SqlWhere;

            CBCommand command = Database.Data.CreateCommand("select Count(Id) from Caches " + ((where.Length > 0) ? "where " + where : where));
            object ob = command.ExecuteScalar();
            numTotalCaches = int.Parse(ob.ToString());
            command.Dispose();

            //SqlCeCommand query = new SqlCeCommand("select count(Id) from Caches", Database.Data.Connection);
            //numTotalCaches = int.Parse(query.ExecuteScalar().ToString());
            //query.Dispose();



            //command = new SqlCeCommand("select Id, Description, Name, GcCode, Url, ImagesUpdated, DescriptionImagesUpdated from Caches " + ((where.Length > 0) ? "where " + where : where), Database.Data.Connection);
            //SqlCeDataReader reader = command.ExecuteReader();

            command = Database.Data.CreateCommand("select Name, Latitude, Longitude from Caches " + ((where.Length > 0) ? "where " + where : where));
            iteratorReader = command.ExecuteReader();

            iteratorCacheTiles = null;
        }

        void disposeTileIterator()
        {
            if (iteratorReader != null)
            {
                iteratorReader.Dispose();
                iteratorReader = null;
            }
            iteratorCacheTiles = null;
        }

        Stack<Descriptor> iteratorCacheTiles = null;
        bool requestTile(out Descriptor desc, out Layer layer)
        {
            lock (this)
            {
                if (layersToImport != null && layerIndex >= layersToImport.Count)
                {
                    iteratorCacheTiles.Pop();
                    layerIndex = 0;
                }

                if (iteratorCacheTiles == null || iteratorCacheTiles.Count == 0)
                {
                    layerIndex = 0;

                    currentCacheIndex++;

                    iteratorCacheTiles = new Stack<Descriptor>();

                    while (iteratorCacheTiles.Count == 0)
                    {
                        if (!iteratorReader.Read())
                        {
                            desc = null;
                            layer = null;
                            return false;
                        }

                        double lat = iteratorReader.GetDouble(1);
                        double lon = iteratorReader.GetDouble(2);

                        for (int zoom = MinZoom; zoom <= MaxZoom; zoom++)
                        {
                            WinCachebox.Geocaching.BoundingBox area = expandCoordinates(lat, lon, (int)Math.Ceiling(Convert.ToDouble(osmCoverage) / (zoom + 1 - MinZoom)));

                            int xFrom = (int)Math.Floor(Map.Descriptor.LongitudeToTileX(zoom, area.MinLongitude));
                            int xTo = (int)Math.Ceiling(Map.Descriptor.LongitudeToTileX(zoom, area.MaxLongitude));
                            int yFrom = (int)Math.Floor(Map.Descriptor.LatitudeToTileY(zoom, area.MinLatitude));
                            int yTo = (int)Math.Ceiling(Map.Descriptor.LatitudeToTileY(zoom, area.MaxLatitude));
                            //int xFrom = (int)Math.Floor(Descriptor.LongitudeToTileX(zoom, area.MinLongitude));
                            //int xTo = (int)Math.Ceiling(Descriptor.LongitudeToTileX(zoom, area.MaxLongitude));
                            //int yFrom = (int)Math.Ceiling(Descriptor.LatitudeToTileY(zoom, area.MaxLatitude));
                            //int yTo = (int)Math.Floor(Descriptor.LatitudeToTileY(zoom, area.MinLatitude));

                            for (int x = xFrom; x <= xTo; x++)
                                for (int y = yFrom; y <= yTo; y++)
                                    iteratorCacheTiles.Push(new Descriptor(x, y, zoom));
                        }
                    }
                }

                layer = layersToImport[layerIndex++];
                desc = iteratorCacheTiles.Peek();

                return true;
            }
        }

        /// <summary>
        /// Läd die in loaderList eingetragenen Kacheln
        /// </summary>
        void loaderThreadEntryPoint()
        {
            lock (this)
                numThreads++;

            try
            {
                while (!parent.Cancel)
                {
                    Descriptor tile;
                    Layer layer;

                    if (requestTile(out tile, out layer))
                    {
                        if (!Manager.ExistsTile(tile))
                            continue;

                        if ((loaderListIndex % 10) == 0)
                        {
                            // Power Down verhindern
                            Application.DoEvents();

                            if (parent.PerformMemoryTest(Config.GetString("TileCacheFolder"), 1024))
                                break;
                        }

                        parent.ProgressChanged("Cache " + currentCacheIndex.ToString() + " / " + numTotalCaches.ToString() + "\r\nTile: " + tile.X.ToString() + "," + tile.Y.ToString() + " / Zoom: " + tile.Zoom.ToString(), currentCacheIndex, numTotalCaches);

                        if (!Views.MapView.Manager.CacheTile(layer, tile))
                        {
                            numFails++;
                            parent.ReportUncriticalError(numFails.ToString() + " tiles skipped");
                        }
                    }
                    else
                        break;
                }
            }
            catch (Exception exc)
            {
#if DEBUG
                Global.AddLog("TileImport.loaderThreadEntryPoint: " + exc.ToString() + "\n\n");
#endif
            }
            finally
            {
                lock (this)
                    numThreads--;
#if DEBUG
                Global.AddLog("TileImport.loaderThreadEntryPoint: Thread ended");
#endif
            }
        }

        /// <summary>
        /// Trägt die Kachel mit der übergebenen Koordinate und alle Vorfahren
        /// von dieser in tilesToFetch ein
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="zoom">Zoom</param>
        //void scheduleTile(int x, int y, int zoom)
        //{
        //    Descriptor tile = new Descriptor(x, y, zoom);

        //    if (!tilesToFetch.ContainsKey(tile))
        //    {
        //        tilesToFetch.Add(tile, false);

        //        if (zoom > MinZoom)
        //            scheduleTile(x >> 1 , y >> 1, zoom - 1);
        //    }
        //}

        /// <summary>
        /// Berechnet eine BoundingBox mit der angegebenen Kantenlänge
        /// um die angegebene Koordinate
        /// </summary>
        /// <param name="latitude">Latitude</param>
        /// <param name="longitude">Longitude</param>
        /// <param name="size">Größe der Bounding Box in Metern</param>
        /// <returns>Bounding Box</returns>
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


        //public static String fetchReport = String.Empty;

    }
}
