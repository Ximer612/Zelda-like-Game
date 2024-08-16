using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Aiv.Fast2D;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    class TmxMap : IDrawable
    {
        protected string tmxFilePath;
        protected string tilesetXmlPath;
        public DrawLayer DrawLayer { get;}

        // Tileset
        protected TmxTileset tileset;
        // MultiLayers
        protected TmxTileLayer[] tileLayers;
        public int[] SolidTilesIDs { get; protected set; }

        public static int Width;
        public static int Height;

        public TmxMap(string filePath, string tilesetPath, List<Item> items, bool addItemsToMap = true)
        {
            // CREATE AND LOAD XML DOCUMENT FROM TMX MAP FILE
            tmxFilePath = filePath;

            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(tmxFilePath);
            }
            catch(XmlException e)
            {
                Console.WriteLine("XML Exception: " + e.Message);
            }
            catch(Exception e)
            {
                Console.WriteLine("Generic Exception: " + e.Message);
            }

            // PROCEED TO XML DOCUMENT NODES PARSING
            // Map Node and Attributes
            XmlNode mapNode = xmlDoc.SelectSingleNode("map");
            int mapCols = GetIntAttribute(mapNode, "width");
            int mapRows = GetIntAttribute(mapNode, "height");
            int mapTileW = GetIntAttribute(mapNode, "tilewidth");
            int mapTileH = GetIntAttribute(mapNode, "tileheight");

            tilesetXmlPath = tilesetPath;

            XmlDocument xmlTilesetDoc = new XmlDocument();

            try
            {
                xmlTilesetDoc.Load(tilesetPath);
            }
            catch (XmlException e)
            {
                Console.WriteLine("XML Exception: " + e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Generic Exception: " + e.Message);
            }

            // Tileset Node and Attributes
            XmlNode tilesetNode = xmlTilesetDoc.SelectSingleNode("tileset");
            int tilesetTileW = GetIntAttribute(tilesetNode, "tilewidth");
            int tilesetTileH = GetIntAttribute(tilesetNode, "tileheight");
            int tileCount = GetIntAttribute(tilesetNode, "tilecount");
            int tilesetCols = GetIntAttribute(tilesetNode, "columns");
            int tilesetRows = tileCount / tilesetCols;

            // Create Tileset from collected data
            tileset = new TmxTileset("tileset", tilesetCols, tilesetRows, tilesetTileW, tilesetTileH, 0, 0);

            Width = GetIntAttribute(mapNode, "width");
            Height = GetIntAttribute(mapNode, "height");

            // MULTILAYER (TILES & TILEOBJECTS LAYERS)
            XmlNodeList layersNodes = mapNode.SelectNodes("layer");

            tileLayers = new TmxTileLayer[layersNodes.Count];

            for (int i = 0; i < layersNodes.Count; i++)
            {

                string layerName = GetStringAttribute(layersNodes[i], "name");
                DrawLayer drawLayer = DrawLayer.Background;

                if (layerName == "LayerForeground") drawLayer = DrawLayer.Foreground;

                tileLayers[i] = new TmxTileLayer(layersNodes[i], tilesetNode, tileset, mapCols, mapRows, mapTileW, mapTileH , drawLayer);

                if (layerName == "LayerSolidTiles")
                {
                    TmxTileObjectLayer tileObjectLayer = new TmxTileObjectLayer(layersNodes[i], tilesetNode, tileset);
                    SolidTilesIDs = tileObjectLayer.SolidTilesIDs;
                }

            }

            // Map Drawing Settings
            if(!PlayScene.FirstLoad)DrawMngr.AddItem(this);


            //Create items
            new TmxObjectLayer(mapNode.SelectSingleNode("objectgroup"),items,tmxFilePath, SolidTilesIDs);
        }

        public static int GetIntAttribute(XmlNode node, string attrName)
        {
            return int.Parse(GetStringAttribute(node, attrName));
        }

        public static bool GetBoolAttribute(XmlNode node, string attrName)
        {
            return bool.Parse(GetStringAttribute(node, attrName));
        }

        public static string GetStringAttribute(XmlNode node, string attrName)
        {
            return node.Attributes.GetNamedItem(attrName).Value;
        }

        public void Draw()
        {
            if (tileLayers == null) return;

            for ( int i = 0; i < tileLayers.Length; i++)
            {
                tileLayers[i].Draw();
            }
        }
        public void ClearAll()
        {
            for (int i = 0; i < tileLayers.Length; i++)
            {
                DrawMngr.RemoveItem(tileLayers[i]);
            }

            DrawMngr.RemoveItem(this);
            tmxFilePath = null;
            tilesetXmlPath = null;
            tileset = null;
            tileLayers = null;
            SolidTilesIDs = null;
        }
    }
}
