using System;
using System.Collections.Generic;
using System.Globalization;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using WinCachebox.Map;
using WeifenLuo.WinFormsUI.Docking;

namespace WinCachebox.Views
{
    public partial class MapView : DockContent
    {
        const int projectionZoomLevel = 15;

        public class Route
        {
            internal Pen Pen;
            internal List<PointD> Points;
            internal String Name;
            internal String FileName;
            internal bool ShowRoute = false;

            internal Route(Pen pen, String name)
            {
                Pen = pen;
                Points = new List<PointD>();
                Name = name;
            }

        }

        public List<Route> Routes = new List<Route>();

        // Read track from gpx file
        // attention it is possible that a gpx file contains more than 1 <trk> segments
        // in this case all segments was connectet to one track
        public Route LoadRoute(String file, Pen pen, double minDistanceMeters)
        {

            try
            {
                BinaryReader reader = new BinaryReader(File.Open(file, FileMode.Open));

                Route route = new Route(pen, null)
                {
                    FileName = Path.GetFileName(file)
                };

                long length = reader.BaseStream.Length;

                String line = null;
                bool inBody = false;
                bool inTrk = false;
                bool ReadName = false;

                Coordinate lastAcceptedCoordinate = null;

                StringBuilder sb = new StringBuilder();
                while (reader.BaseStream.Position < length)
                {

                    char nextChar = reader.ReadChar();
                    sb.Append(nextChar);

                    if (nextChar == '>')
                    {
                        line = sb.ToString().Trim().ToLower();
                        sb = new StringBuilder();

                        // Read Routename form gpx file
                        // attention it is possible that a gpx file contains more than 1 <trk> segments
                        // In this case the first name was used
                        if (ReadName && (route.Name == null))
                        {
                            route.Name = line.Substring(0, line.IndexOf("</name>"));
                            ReadName = false;
                            continue;
                        }

                        if (!inTrk)
                        {
                            // Begin of the Track detected?
                            if (line.IndexOf("<trk>") > -1)
                                inTrk = true;

                            continue;
                        }
                        else
                        {
                            // found <name>?
                            if (line.IndexOf("<name>") > -1)
                            {
                                ReadName = true;
                                continue;
                            }
                        }


                        if (!inBody)
                        {
                            // Anfang der Trackpoints gefunden?
                            if (line.IndexOf("<trkseg>") > -1)
                                inBody = true;

                            continue;
                        }

                        // Ende gefunden?
                        if (line.IndexOf("</trkseg>") > 0)
                            break;

                        if (line.IndexOf("<trkpt") > -1)
                        {
                            // Trackpoint lesen
                            int lonIdx = line.IndexOf("lon=\"") + 5;
                            int latIdx = line.IndexOf("lat=\"") + 5;

                            int lonEndIdx = line.IndexOf("\"", lonIdx);
                            int latEndIdx = line.IndexOf("\"", latIdx);

                            String latStr = line.Substring(latIdx, latEndIdx - latIdx);
                            String lonStr = line.Substring(lonIdx, lonEndIdx - lonIdx);

                            double lat = double.Parse(latStr, NumberFormatInfo.InvariantInfo);
                            double lon = double.Parse(lonStr, NumberFormatInfo.InvariantInfo);

                            if (lastAcceptedCoordinate != null)
                                if (Datum.WGS84.Distance(lat, lon, lastAcceptedCoordinate.Latitude, lastAcceptedCoordinate.Longitude) < minDistanceMeters)
                                    continue;

                            lastAcceptedCoordinate = new Coordinate(lat, lon);

                            PointD projectedPoint = new PointD(Descriptor.LongitudeToTileX(projectionZoomLevel, lon),
                                Descriptor.LatitudeToTileY(projectionZoomLevel, lat));

                            route.Points.Add(projectedPoint);
                        }
                    }
                }

                reader.Close();
                if (route.Points.Count < 2)
                    route.Name = "no Route segment found";

                route.ShowRoute = true;

                return route;
            }

            catch (Exception exc)
            {
#if DEBUG
                Global.AddLog("RouteOverlay.LoadRoute: " + exc.ToString());
#endif
                MessageBox.Show(exc.ToString(), Global.Translations.Get("Error"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return null;
            }

        }

        internal void RenderRoute(Graphics graphics, Bitmap bitmap, Descriptor desc)
        {
            double tileX = desc.X * 256 * dpiScaleFactorX;
            double tileY = desc.Y * 256 * dpiScaleFactorY;
            List<Point> points = new List<Point>();

            for (int i = 0; i < Routes.Count; i++)
            {
                //int lastX = -999;
                //int lastY = -999;
                //bool lastIn = true;
                //bool aktIn = false;

                if (Routes[i].ShowRoute)
                {
                    Pen pen = Routes[i].Pen;

                    double adjustmentX = Math.Pow(2, desc.Zoom - projectionZoomLevel) * 256 * dpiScaleFactorX;
                    double adjustmentY = Math.Pow(2, desc.Zoom - projectionZoomLevel) * 256 * dpiScaleFactorY;

                    int step = Routes[i].Points.Count / 600;
                    if (step < 1)
                        step = 1;

                    for (int j = 0; j < (Routes[i].Points.Count); j = j + step)
                    {
                        int x1 = (int)(Routes[i].Points[j].X * adjustmentX - tileX);
                        int y1 = (int)(Routes[i].Points[j].Y * adjustmentY - tileY);

                        //if (Routes[i].Points.Count > 360)
                        //{
                        //    aktIn = (x1 >= -bitmap.Width) && (x1 <= bitmap.Width * 2) && (y1 >= -bitmap.Height) && (y1 <= bitmap.Height * 2);

                        //    if (aktIn)
                        //    {
                        //        if (!lastIn)  // wenn letzter Punkt nicht innerhalb war -> trotzdem hinzuf?gen, damit die Linie vollst?ndig wird
                        //            points.Add(new Point((int)lastX, (int)lastY));
                        //        if ((x1 != lastX) || (y1 != lastY))
                        //            points.Add(new Point(x1, y1));
                        //    }
                        //    else
                        //    {
                        //        if (lastIn)
                        //        {
                        //            // wenn der letzte Punkt noch sichtbar war, aktuellen Punkt hinzuf?gen, obwohl er ausserhalb ist, damit die Linie abgeschlossen wird
                        //            points.Add(new Point(x1, y1));
                        //            // Linienzug zeichnen
                        //            graphics.DrawLines(pen, points.ToArray());
                        //            points.Clear();
                        //        }
                        //    }

                        //    lastX = x1;
                        //    lastY = y1;
                        //    lastIn = aktIn;
                        //}
                        //else
                        {
                            points.Add(new Point(x1, y1));
                        }
                    }
                    // letzte Punkte bis zum aktuellen noch zeichnen
                    if (points.Count > 0)
                        graphics.DrawLines(pen, points.ToArray());
                    points.Clear();
                }
            }
        }

        public Route GenP2PRoute(double FromLat, double FromLon, double ToLat, double ToLon, Pen pen)
        {
            Route route = new Route(pen, null)
            {
                Name = "Point 2 Point Route"
            };
            PointD projectedPoint = new PointD(Descriptor.LongitudeToTileX(projectionZoomLevel, FromLon), Descriptor.LatitudeToTileY(projectionZoomLevel, FromLat));
            route.Points.Add(projectedPoint);
            projectedPoint = new PointD(Descriptor.LongitudeToTileX(projectionZoomLevel, ToLon), Descriptor.LatitudeToTileY(projectionZoomLevel, ToLat));
            route.Points.Add(projectedPoint);
            route.ShowRoute = true;
            return route;
        }

        public Route GenCircleRoute(double FromLat, double FromLon, double Distance, Pen pen)
        {
            Route route = new Route(pen, null)
            {
                Name = "Circle Route"
            };
            Coordinate GEOPosition = new Coordinate
            {
                Latitude = FromLat,
                Longitude = FromLon
            };
            Coordinate Projektion = new Coordinate();
            for (int i = 0; i <= 360; i++)
            {
                Projektion = Coordinate.Project(GEOPosition.Latitude, GEOPosition.Longitude, (double)i, Distance);

                PointD projectedPoint = new PointD(Descriptor.LongitudeToTileX(projectionZoomLevel, Projektion.Longitude),
                                            Descriptor.LatitudeToTileY(projectionZoomLevel, Projektion.Latitude));
                route.Points.Add(projectedPoint);
            }
            route.ShowRoute = true;
            return route;
        }

        public Route GenProjectRoute(double FromLat, double FromLon, double Distance, double Bearing, Pen pen)
        {
            Route route = new Route(pen, null)
            {
                Name = "Projected Route"
            };
            Coordinate GEOPosition = new Coordinate
            {
                Latitude = FromLat,
                Longitude = FromLon
            };
            PointD projectedPoint = new PointD(Descriptor.LongitudeToTileX(projectionZoomLevel, GEOPosition.Longitude), Descriptor.LatitudeToTileY(projectionZoomLevel, GEOPosition.Latitude));
            route.Points.Add(projectedPoint);
            Coordinate Projektion = new Coordinate();
            Projektion = Coordinate.Project(GEOPosition.Latitude, GEOPosition.Longitude, Bearing, Distance);
            projectedPoint = new PointD(Descriptor.LongitudeToTileX(projectionZoomLevel, Projektion.Longitude), Descriptor.LatitudeToTileY(projectionZoomLevel, Projektion.Latitude));
            route.Points.Add(projectedPoint);
            route.ShowRoute = true;
            return route;
        }
    }
}
