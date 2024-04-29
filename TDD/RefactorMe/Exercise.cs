using System.Diagnostics;
using System.Drawing;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RefactorMe
{
    public class Exercise
    {
        /*
         * 1. Zkuste odhadnout, co metoda ParseResponseSingleLine dělá.
         * 2. Použijte ReSharper opakovaně na metodu ParseResponseSingleLine, dokud sám neoznačí metodu za "hotovou".
         * 3. Porovnejte výsledný kód metody s původním kódem. Jak se liší, co pro vás tento výsledek představuje?
         */

        /// <summary>
        /// Parses the information obtained from server from json format to array.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static int[] ParseResponseSingleLine(string elevationResponse)
        {
            var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(elevationResponse);

            if (result == null)
                return null;

            if (result.ContainsKey("statusCode") && (long)result["statusCode"] != 200)
                throw new WebException(result["statusDescription"].ToString());

            if (!(result["resourceSets"] is JArray) || ((JArray)result["resourceSets"]).Count == 0)
                return null;

            var resourceSets = (JArray)result["resourceSets"];
            if (resourceSets[0]["resources"]?[0] != null)
            {
                var resources = resourceSets[0]["resources"][0];
                if (resources["elevations"] != null)
                {
                    var elevations = resources["elevations"].Select(x => (int)x).ToArray();
                    return elevations;
                }
            }

            return null;
        }

        /*
         * 1. Zkuste odhadnout co dela metoda UpdateZoomAndPan
         * 2. Jake code smells vidite?
         * 3. Odstrante je pomoci ReSharperu.
         */

        private void UpdateZoomAndPan(double LatLongLT, double LatLongRB)
        {
            #region Initialization

            IsRendering = false;

            // The bounds are not valid
            if ((ViewportSize.Width <= 0) || (Double.IsInfinity(ViewportSize.Width)) || (Double.IsNaN(ViewportSize.Width)) ||
                (ViewportSize.Height <= 0) || (Double.IsInfinity(ViewportSize.Height) || (Double.IsNaN(ViewportSize.Height))))
                return;

            // The bounds are not valid
            if ((ActualWidth <= 0) || (Double.IsInfinity(ActualWidth)) || (Double.IsNaN(ActualWidth)) ||
                (ActualHeight <= 0) || (Double.IsInfinity(ActualHeight)) || (Double.IsNaN(ActualHeight)))
                return;

            double xamlViewerZoom = XamlViewerZoom;
            uint zoomLevel = ZoomLevel;

            // Getting all reference values (margin, scale..)
            Size scrollBarSize = GetScrollBarSize();
            Size rootSize = new Size((int)ParentActualWidth, (int)ParentActualHeight);  //size of the parent display
            Size scFactor = GetScaleFactor();
            Size tileSize = GetTileSize(scFactor, true);

            // The bounds are not valid
            if ((tileSize.Width <= 0) || (Double.IsInfinity(tileSize.Width)) || (Double.IsNaN(tileSize.Width)) ||
                (tileSize.Height <= 0) || (Double.IsInfinity(tileSize.Height)) || (Double.IsNaN(tileSize.Height)))
                return;

            UInt32 tilesNum = Tile.MapTiles(zoomLevel);

            Size newSize;
            if (Double.IsInfinity(rootSize.Width) || Double.IsNaN(rootSize.Width) || rootSize.Width <= BaseMapSize)
                newSize = new Size(
                    (int)Math.Ceiling((decimal)Math.Min(tilesNum, (ViewportSize.Width / tileSize.Width) + 2)),
                    (int)Math.Ceiling((decimal)Math.Min((int)tilesNum, (ViewportSize.Height / tileSize.Height) + 2))
                );
            else
            {
                newSize = new Size(
                    (int)Math.Ceiling((decimal)(ViewportSize.Width / tileSize.Width + 2)),
                    (int)Math.Ceiling((decimal)Math.Min(tilesNum, (ViewportSize.Height / tileSize.Height) + 2))
                );
                //we need the number of tiles to be a multiple of 2 in order to make the tiling work properly
                if ((int)(newSize.Width) % 2 == 1)
                    newSize.Width += 1;
            }

            // The newSize is not valid
            if ((newSize.Width <= 0) || (Double.IsInfinity(newSize.Width)) || (Double.IsNaN(newSize.Width)) ||
                (newSize.Height <= 0) || (Double.IsInfinity(newSize.Height)) || (Double.IsNaN(newSize.Height)))
                return;

            Size partMapSize = new Size((int)(tileSize.Width * tilesNum), (int)(tileSize.Height * tilesNum));
            Point refPoint = ReferenceLatLongToPoint(partMapSize, scFactor, rootSize);//, dispMargin, scMargin);
            //LatLongLT = ViewportPointToLatLong(new Point(partMapSize.Width <= ViewportSize.Width ? 0 : -XamlViewerPan.X, partMapSize.Height <= ViewportSize.Height ? 0 : -XamlViewerPan.Y));
            //LatLongRB = ViewportPointToLatLong(new Point((partMapSize.Width <= ViewportSize.Width ? BaseMapSize : -XamlViewerPan.X + ViewportSize.Width) * XamlViewerZoom, (partMapSize.Height <= ViewportSize.Height ? BaseMapSize : -XamlViewerPan.Y + ViewportSize.Height) * xamlViewerZoom));

            Point center = new Point((-XamlViewerPan.X + (ViewportSize.Width / 2)) / ParentXamlViewer.Zoom / scFactor.Width, (-XamlViewerPan.Y + (ViewportSize.Height / 2)) / ParentXamlViewer.Zoom / scFactor.Height);

            LatLong c = RootPointToLatLong(center);
            LatLong c2 = RootPointToLatLong(new Point((int)(center.X + 10 / XamlViewerZoom), center.Y));

            Double longitude = c.Longitude;
            Double latitude = c.Latitude;


            #endregion

            #region Calculating the Margins around the map

            //pan of the control - should only be considered if the display is moved off-viewport
            //this is also the place when we need to take into account the possible default region (hence the refPoint) 
            Point mapPan = new Point(
                refPoint.X + (XamlViewerPan.X < 0 ? -XamlViewerPan.X : 0),
                refPoint.Y + (XamlViewerPan.Y < 0 ? -XamlViewerPan.Y : 0));

            //to compensate for the cases where the display width is not a multiple of tile size or the height is not equal to 512
            Point baseMargin = new Point(
                (int)(0.5 * (double)(rootSize.Width <= Tile.MapSize(1) ? rootSize.Width - Tile.MapSize(1) : rootSize.Width - Math.Ceiling((decimal)(rootSize.Width / Tile.MapSize(1))) * Tile.MapSize(1))),
                (int)(0.5 * (rootSize.Height - Tile.MapSize(1))));

            //the margin needs to be modified for zoom value
            Point margin = new Point((int)(baseMargin.X * scFactor.Width * xamlViewerZoom), (int)(baseMargin.Y * scFactor.Height * xamlViewerZoom));

            //the top left coordinate of the first tile that can be seen on screen
            Point offset = new Point(
                (int)(Math.Floor((decimal)((mapPan.X - margin.X) / tileSize.Width)) * tileSize.Width + margin.X),
                (int)(Math.Floor((decimal)(Math.Max(0, mapPan.Y - margin.Y) / tileSize.Height)) * tileSize.Height + margin.Y));

            //the tile index of the first tile that can be seen on screen
            //the middle of the containerPanel defines the left-top corner of tile at index (tilesNum/2, tilesNum/2)
            TileCoordinate refTile = new TileCoordinate(
                (Math.Round(tilesNum * 0.5 - (rootSize.Width * scFactor.Width * xamlViewerZoom * 0.5 - offset.X) / tileSize.Width) % tilesNum + tilesNum) % tilesNum,
                Math.Round(tilesNum * 0.5 - (rootSize.Height * scFactor.Height * xamlViewerZoom * 0.5 - offset.Y) / tileSize.Height));
            
            #endregion

            #region Updating the individual layers

            var layersCount = MapLayers.Count;
            {
                int wmOffsetY = 0;
                Dictionary<MapSystem, int> shownWMs = new Dictionary<MapSystem, int>();
                foreach (SingleMapLayer layer in MapLayers)
                {
                    layer.Left = XamlViewerPan.X;
                    layer.Top = XamlViewerPan.Y;
                    layer.CanvasWidth = rootSize.Width * scFactor.Width * xamlViewerZoom;
                    layer.CanvasHeight = rootSize.Height * scFactor.Height * xamlViewerZoom;

                    layer.LeftTopTile = refTile;
                    layer.TileCount = newSize;
                    layer.TileOffset = new Point(offset.X - refPoint.X, offset.Y - refPoint.Y);

                    layer.ZoomLevel = zoomLevel;
                    layer.WatermarkOffsetX = 0;

                    if ((bool)layer.HasWatermark && layer.Visible)
                    {
                        //this is here to prevent the same logo from appearing multiple times
                        if (!shownWMs.Keys.Contains(layer.MappingSystem))
                        {
                            shownWMs[layer.MappingSystem] = wmOffsetY;
                            wmOffsetY -= layer.GetWatermarkHeight();
                        }
                        layer.WatermarkOffsetY = shownWMs[layer.MappingSystem];
                    }
                    else
                        layer.WatermarkOffsetY = 0;

                    layer.Redraw(tileSize, new Size(), true, LatLongLT, LatLongRB);
                }
            }

            #endregion
        }

        #region Here Be Dragons

        public List<SingleMapLayer> MapLayers { get; set; }

        public object ActualLatitudeProp { get; set; }

        public uint ActualZoomLevel { get; set; }

        private LatLong RootPointToLatLong(Point center)
        {
            throw new NotImplementedException();
        }

        private Point ReferenceLatLongToPoint(Size partMapSize, Size scFactor, Size rootSize)
        {
            throw new NotImplementedException();
        }

        public int BaseMapSize { get; set; }

        private Size GetTileSize(Size scFactor, bool b)
        {
            return new Size(256, 256);
        }

        private Size GetScaleFactor()
        {
            return new Size(1, 1);
        }

        public double ParentActualHeight { get; set; }

        public double ParentActualWidth { get; set; }

        private Size GetScrollBarSize()
        {
            throw new NotImplementedException();
        }

        public uint ZoomLevel { get; set; }

        public double XamlViewerZoom { get; set; }

        public double ActualHeight { get; set; }

        public double ActualWidth { get; set; }

        public bool IsRendering { get; set; }
    }

    public class MapSystem
    {
    }

    public class SingleMapLayer
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public double CanvasWidth { get; set; }
        public double CanvasHeight { get; set; }
        public TileCoordinate LeftTopTile { get; set; }
        public Size TileCount { get; set; }
        public Point TileOffset { get; set; }
        public uint ZoomLevel { get; set; }
        public int WatermarkOffsetX { get; set; }
        public bool HasWatermark { get; set; }
        public bool Visible { get; set; }
        public MapSystem MappingSystem { get; set; }
        public int WatermarkOffsetY { get; set; }

        public int GetWatermarkHeight()
        {
            throw new NotImplementedException();
        }

        public void Redraw(Size tileSize, object viewportSize, object enableAnimation, object latLongLt, object latLongRb)
        {
            throw new NotImplementedException();
        }
    }

    public class TileCoordinate
    {
        public TileCoordinate(double tilesNum, double round)
        {
            throw new NotImplementedException();
        }
    }

    internal class UndoManager
    {
        public static bool SetEnabled(bool b)
        {
            throw new NotImplementedException();
        }
    }

    internal class LatLong
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }

    internal class ParentXamlViewer
    {
        public static int Zoom { get; set; }
    }

    internal class XamlViewerPan
    {
        public static int X { get; set; }
        public static int Y { get; set; }
    }

    internal class Tile
    {
        public static uint MapTiles(uint zoomLevel)
        {
            return 2;
        }

        public static int MapSize(int p0)
        {
            throw new NotImplementedException();
        }
    }

    internal class ViewportSize
    {
        public static int Width { get; set; }
        public static int Height { get; set; }
    }

    #endregion
}