using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    class TmxObjectLayer
    {
        private Item item;
        private string mapName;
        private int[] solidTilesIDs;

        public TmxObjectLayer(XmlNode objectLayerNode,List<Item> items,string filePath, int[] solidTilesIDs)
        {
            XmlNodeList objectsNodes = objectLayerNode.SelectNodes("object");

            mapName = filePath.Substring(filePath.LastIndexOf('/') + 1);
            mapName = mapName.Substring(0, mapName.LastIndexOf('.')); ;

            if (PlayScene.FirstLoad)
            {
                SaveGameManager.SaveGameDatas[mapName] = new Dictionary<string, string>();
            }
            else
            {
                SaveGameManager.LoadSave();
                this.solidTilesIDs = solidTilesIDs;
            }

            for (int i = 0; i < objectsNodes.Count; i++)
            {
                string objType = TmxMap.GetStringAttribute(objectsNodes[i], "type");

                ItemType itemType;
                Enum.TryParse(objType, out itemType);

                XmlNode propertiesNode = objectsNodes[i].SelectSingleNode("properties");
                string objName = TmxMap.GetStringAttribute(objectsNodes[i], "name");
                int objW = TmxMap.GetIntAttribute(objectsNodes[i], "width");
                int objH = TmxMap.GetIntAttribute(objectsNodes[i], "height");
                int objX = TmxMap.GetIntAttribute(objectsNodes[i], "x")/ objW;
                int objY = TmxMap.GetIntAttribute(objectsNodes[i], "y")/ objH;

                Vector2 position = new Vector2(objX, objY);

                item = null;
                bool interacted = false;

                string itemID = objName + i;

                switch (itemType)
                {
                    case ItemType.Teleport:
                        {
                            CreateTeleport(propertiesNode,position);
                            break;
                        }
                    case ItemType.Coin:
                        {
                            CreateCoin(itemID, position, ref interacted);
                            break;
                        }
                    case ItemType.RedKey:
                    case ItemType.BlueKey:
                    case ItemType.YellowKey:
                    case ItemType.GreenKey:
                        CreateKey(itemType,itemID, propertiesNode.FirstChild, position, ref interacted);
                        break;
                    case ItemType.BossTeleport:
                        CreateBossTeleport(propertiesNode, position);
                        break;
                    case ItemType.Spike:
                        CreateSpike(position);
                        break;
                    case ItemType.Null:
                        {
                            if (objName == "Stone")
                            {
                                CreateStone(position,itemID, ref interacted);

                            }
                            else if(objName == "Enemy")
                            {
                                CreateEnemy(position,itemID, ref interacted);
                                continue;
                            }
                        }
                        break;
                }

                if (!PlayScene.FirstLoad)
                {
                    items.Add(item);
                    item.XmlObjName = itemID;
                }

                
                SaveGameManager.SaveGameDatas[mapName][itemID] = interacted.ToString();
            }
        }

        void CreateEnemy(Vector2 position, string enemyID, ref bool killed)
        {

            if (PlayScene.FirstLoad)
            {
            
            SaveGameManager.SaveGameDatas[mapName][enemyID] = killed.ToString();
            return;
            }


            Enemy enemy = new Enemy(enemyID);
            enemy.Position = position;

            if (SaveGameManager.IsSaveGameFileExist)
            {
                if (SaveGameManager.SaveGameDatas[mapName][enemyID] == "True")
                {
                    killed = true;
                    enemy.OnDie(false);
                }
                else
                {
                    killed = false;
                    enemy.IsActive = true;
                }
            }

            SaveGameManager.SaveGameDatas[mapName][enemyID] = killed.ToString();

            ((PlayScene)Game.CurrentScene).Enemies.Add(enemy);
        }
        void CreateStone(Vector2 position, string itemID, ref bool interacted)
        {
            item = new StoneBlock(position);

            if (PlayScene.FirstLoad) return;

            if (SaveGameManager.IsSaveGameFileExist)
            {
                if (SaveGameManager.SaveGameDatas[mapName][itemID] == "True")
                {
                    item.IsActive = false;
                    interacted = true;
                }
                else
                {
                    item.IsActive = true;
                    ((PlayScene)Game.CurrentScene).ObjectToAddCollisions.Add(item);
                }
            }
            else
            {
                item.IsActive = true;
                ((PlayScene)Game.CurrentScene).ObjectToAddCollisions.Add(item);
            }
        }
        void CreateKey(ItemType keyType, string itemID, XmlNode colorNode, Vector2 position, ref bool taken)
        {
            string colorHEX = TmxMap.GetStringAttribute(colorNode, "value").Replace("#","");

            float r =  (float)Convert.ToInt32((colorHEX.Substring(2, 2)),16)/255;
            float g =  (float)Convert.ToInt32((colorHEX.Substring(4, 2)),16)/255;
            float b =  (float)Convert.ToInt32((colorHEX.Substring(6, 2)),16)/255;

            item = new KeyItem(keyType, position, new Vector4(r,g,b, 1));

            if (PlayScene.FirstLoad) return;

            if (SaveGameManager.IsSaveGameFileExist)
            {
                if (SaveGameManager.SaveGameDatas[mapName][itemID] == "True")
                {
                    item.IsActive = false;
                    taken = true;
                }
                else item.IsActive = true;
            }
            else item.IsActive = true;

        }
        void CreateTeleport(XmlNode propertiesNode,Vector2 position)
        {
            XmlNode propertyNode = propertiesNode.FirstChild;

            if (PlayScene.FirstLoad) return;

            bool locked = false;
            ItemType keyType = ItemType.LAST;

            string keyToOpen = TmxMap.GetStringAttribute(propertyNode, "value");
            propertyNode = propertyNode.NextSibling;

            if(keyToOpen != "")
            {
                Enum.TryParse(keyToOpen, out keyType);
                locked = true;
            }

            string sceneName = TmxMap.GetStringAttribute(propertyNode, "value");
            propertyNode = propertyNode.NextSibling;
            int exitX = TmxMap.GetIntAttribute(propertyNode, "value");
            propertyNode = propertyNode.NextSibling;
            int exitY = TmxMap.GetIntAttribute(propertyNode, "value");
            Vector2 exitPosition = new Vector2(exitX, exitY);
            if (locked)
            {
                item = new LockedTeleportItem(Game.Scenes[sceneName], position, exitPosition,keyType);
            }
            else
            {
                item = new TeleportItem(Game.Scenes[sceneName], position, exitPosition);
            }
            item.IsActive = true;
        }
        void CreateBossTeleport(XmlNode propertiesNode, Vector2 position)
        {
            XmlNode propertyNode = propertiesNode.FirstChild;

            if (PlayScene.FirstLoad) return;

            int enemiesToKill = TmxMap.GetIntAttribute(propertyNode, "value");
            propertyNode = propertyNode.NextSibling;
            string sceneName = TmxMap.GetStringAttribute(propertyNode, "value");
            propertyNode = propertyNode.NextSibling;
            int exitX = TmxMap.GetIntAttribute(propertyNode, "value");
            propertyNode = propertyNode.NextSibling;
            int exitY = TmxMap.GetIntAttribute(propertyNode, "value");
            Vector2 exitPosition = new Vector2(exitX, exitY);
            item = new BossTeleport(Game.Scenes[sceneName], position, exitPosition, enemiesToKill);
            item.IsActive = true;
        }
        void CreateSpike(Vector2 position)
        {
            if (PlayScene.FirstLoad) return;

            item = new SpikeBlock(position);
            item.IsActive = true;
        }
        void CreateCoin(string itemID, Vector2 position, ref bool taken)
        {

            item = new CoinItem(position);

            if (PlayScene.FirstLoad) return;
            
            if (SaveGameManager.IsSaveGameFileExist)
            {
                if (SaveGameManager.SaveGameDatas[mapName][itemID] == "True")
                {
                    item.IsActive = false;
                    taken = true;
                }
                else item.IsActive = true;
            }
            else item.IsActive = true;
        }
    }
}
