using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Aiv.Fast2D;
using OpenTK;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    class TmxTileLayer : IDrawable
    {
        private Sprite layerSprite;
        private Texture layerTexture;
        private TmxTileset tileset;
        private int rows, cols;
        private int tileW, tileH;
        public string[] IDs { get; }
        public int[] SolidTilesIDs { get; }
        public DrawLayer DrawLayer { get; set; }

        public TmxTileLayer(XmlNode layerNode, XmlNode tilesetNode, TmxTileset tileset, int cols, int rows, int tileW, int tileH , DrawLayer layer = DrawLayer.Background)
        {
            XmlNode dataNode = layerNode.SelectSingleNode("data");
            string csvData = dataNode.InnerText;
            csvData = csvData.Replace("\r\n", "").Replace("\n", "").Replace(" ", "");

            string[] Ids = csvData.Split(',');
            IDs = Ids;

            DrawLayer = layer;

            XmlNode tilesetNodes = layerNode.ParentNode.SelectSingleNode("tileset");

            XmlNodeList tilesNodes = tilesetNode.SelectNodes("tile");

            this.cols = cols;
            this.rows = rows;
            this.tileW = tileW;
            this.tileH = tileH;
            this.tileset = tileset;

            // Create a single texture for the whole map

            int unitWidth = TmxMap.Width * tileW;
            int unitHeight = TmxMap.Height * tileW;

            int bytesPerPixel = 4;
             
            layerTexture = new Texture(unitWidth, unitHeight,true);

            byte[] mapBitmap = new byte[unitWidth * unitHeight * bytesPerPixel];
            Texture tilesetTexture = AssetsMngr.GetTexture(tileset.TextureName);
            byte[] tilesetBitmap = tilesetTexture.Bitmap;

            int tilesetBitmapRowLength = tileset.Width * bytesPerPixel;
            int mapBitmapRowLength = unitWidth * bytesPerPixel;

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    int tileId = int.Parse(IDs[r * cols + c]);

                    if(tileId > 0)
                    {
                        // Get correct tilesetBitmap's section starting index
                        // Get tilesetBitmapIndex offset X in pixels and convert it in bytes
                        int tilesetXOff = tileset.GetAtIndex(tileId).X * bytesPerPixel;
                        // Get tilesetBitmapIndex offset Y in pixels and convert it in bytes
                        int tilesetYOff = tileset.GetAtIndex(tileId).Y * tilesetBitmapRowLength;
                        // Calculate tilesetBitmap starting index
                        int tilesetBitmapIndexInitial = tilesetYOff + tilesetXOff;

                        // Get correct mapBitmap's section starting index
                        int mapXOff = c * tileW * bytesPerPixel;
                        int mapYOff = r * tileH * mapBitmapRowLength;

                        int mapBitmapIndexInitial = mapXOff + mapYOff;

                        // This loop is to copy each single tile from tilesetBitmap to mapBitmap
                        // Loop through each row copying a tile length every time (32 pixels = 32 * 4 bytes)
                        for (int i = 0; i < tileH; i++)
                        {
                            // How tilesetBitmapIndexInitial increments
                            int tilesetBitmapIndexUpdate = i * tilesetBitmapRowLength;
                            // How mapBitmapIndexInitial increments
                            int mapBitmapIndexUpdate = i * mapBitmapRowLength;

                            // Copy tilesetBitmap's tile section to mapBitmap in correct position
                            Array.Copy(tilesetBitmap,                                           // source array
                                        tilesetBitmapIndexInitial + tilesetBitmapIndexUpdate,    // source index
                                        mapBitmap,                                               // dest array
                                        mapBitmapIndexInitial + mapBitmapIndexUpdate,             // dest index
                                        tileW * bytesPerPixel);                                  // length
                        }
                    }
                }
            }

            layerTexture.Update(mapBitmap);

            layerSprite = new Sprite(TmxMap.Width, TmxMap.Height);

            if (!PlayScene.FirstLoad) DrawMngr.AddItem(this);
        }

        public void Draw()
        {
            layerSprite.DrawTexture(layerTexture);
        }
    }
}
