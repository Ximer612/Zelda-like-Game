using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    static class SaveGameManager
    {
        private static string saveGameFile = "SavedProgress.xml";
        public static bool IsSaveGameFileExist { get; private set; }

        public static Dictionary<string, Dictionary<string, string>> SaveGameDatas;

        static public void Init()
        {
            SaveGameDatas = new Dictionary<string, Dictionary<string, string>>();

            //default playerdata
            SaveGameDatas["PlayerData"] = new Dictionary<string, string>();
            SaveGameDatas["PlayerData"]["CurrentScene"] = "Island_Outside";
            SaveGameDatas["PlayerData"]["PlayerX"] = "16";
            SaveGameDatas["PlayerData"]["PlayerY"] = "16";
            SaveGameDatas["PlayerData"]["EnemiesKilled"] = "00";

            //sets all player items to 0
            for (int i = ((int)ItemType.Coin); i <= ((int)ItemType.GreenKey); i++)
            {

                SaveGameDatas["PlayerData"][((ItemType)i).ToString()] = "0";

            }

            IsSaveGameFileExist = File.Exists(saveGameFile);
        }

        static public void SaveGame()
        {
            XmlTextWriter textWriter = new XmlTextWriter(saveGameFile, null);
            // Opens the document  
            textWriter.WriteStartDocument();
            textWriter.WriteStartElement("SavedGame");

            // Write dictionary elements  
            foreach (var item in SaveGameDatas)
            {
                textWriter.WriteString("\n ");
                textWriter.WriteStartElement(item.Key);

                foreach (var value in item.Value)
                {
                    textWriter.WriteString("\n  ");
                    textWriter.WriteStartElement(value.Key);
                    textWriter.WriteString(value.Value);
                    textWriter.WriteEndElement();
                }
                textWriter.WriteString("\n ");
                textWriter.WriteEndElement();
            }

            textWriter.WriteEndDocument();
            // close writer  
            textWriter.Close();

            IsSaveGameFileExist = File.Exists(saveGameFile);
        }

        static public void LoadSave()
        {
            if (File.Exists(saveGameFile))
            {
                XmlDocument xmlDoc = new XmlDocument();

                try
                {
                    xmlDoc.Load(saveGameFile);
                }
                catch (XmlException e)
                {
                    Console.WriteLine("XML Exception: " + e.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Generic Exception: " + e.Message);
                }

                XmlNode savedGameNode = xmlDoc.SelectSingleNode("SavedGame");

                XmlNode currentNode = savedGameNode.FirstChild;
                do
                {

                    XmlNode currentChildNode = currentNode.FirstChild;

                    SaveGameDatas[currentNode.Name] = new Dictionary<string, string>();

                    do
                    {
                        SaveGameDatas[currentNode.Name][currentChildNode.Name] = currentChildNode.InnerText;
                        currentChildNode = currentChildNode.NextSibling;
                    }while (currentChildNode != null);

                    currentNode = currentNode.NextSibling;
                } while (currentNode != null);
            }
        }

        static public void SavePlayer(bool getTeleportedScene=false)
        {
            string scene;

            if (!getTeleportedScene) scene = ((PlayScene)Game.CurrentScene).MapFileName;
            else scene = ((PlayScene)Game.CurrentScene.NextScene).MapFileName;

            SaveGameDatas["PlayerData"]["PlayerX"] = Math.Round(PlayScene.Player.X).ToString();
            SaveGameDatas["PlayerData"]["PlayerY"] = Math.Round(PlayScene.Player.Y).ToString();
            SaveGameDatas["PlayerData"]["CurrentScene"] = scene;
        }
    }
}
