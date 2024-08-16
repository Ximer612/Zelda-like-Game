using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    static class ItemTextMngr
    {
        static private TextObject messageText { get; set; }
        static public TextObject EnemiesKilledText { get; private set; }
        static public int EnemiesKilledCount { get; set; }
        static private float showTextTimer = 6;
        static private float showTextCounter;
        static private bool startCounting;

        public static void Init()
        {
            messageText = new TextObject(new Vector2(3, 0.15f));
            messageText.SetText("Insert here the phrase that will show when interacted with an item");
            messageText.IsActive = false;
            messageText.SetScaleByZoom(16 / (float)Game.OriginalOrthograpicSize);
            showTextCounter = showTextTimer;

            EnemiesKilledText = new TextObject(new Vector2(11.25f, 1.15f));

            if(SaveGameManager.IsSaveGameFileExist)
            {
                EnemiesKilledCount = int.Parse(SaveGameManager.SaveGameDatas["PlayerData"]["EnemiesKilled"]);

                string enemiesKilledDoubleDigit = EnemiesKilledCount < 10 ? 0 + "" + EnemiesKilledCount.ToString() : EnemiesKilledCount.ToString();

                EnemiesKilledText.SetText("Enemies Killed:" + enemiesKilledDoubleDigit);

            }
            else EnemiesKilledText.SetText("Enemies Killed: 00");
            EnemiesKilledText.IsActive = true;

        }

        public static void Draw()
        {
            if (!startCounting) return;

            showTextCounter -= Game.DeltaTime;

            if(showTextCounter < 0)
            {
                messageText.IsActive = false;
                startCounting = false;
                showTextCounter = showTextTimer;
            }
        }

        public static void SetText(string text)
        {
            messageText.SetText(text);
            startCounting = true;
            messageText.IsActive = true;

            showTextCounter = showTextTimer;
        }

        public static void ClearAll()
        {
            messageText.Clear();
            messageText = null;
            EnemiesKilledText.Clear();
            EnemiesKilledText = null;
        }
    }
}
