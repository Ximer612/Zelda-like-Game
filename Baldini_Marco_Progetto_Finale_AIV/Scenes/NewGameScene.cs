using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Audio;
using Aiv.Fast2D;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    class NewGameScene : Scene
    {
        protected KeyCode confirmKey;
        protected KeyCode pressedKey;

        protected bool CanTypeChar;

        protected TextObject playerNameTextObj;
        protected TextObject insertYourNameTextObj;
        protected TextObject playerTypeTextObj;
        protected TextObject insertYourTypeTextObj;

        protected TextObject actualTextObj;
        protected TextObject actualInsertTextObj;

        protected string inputString;
        protected bool isChoosingPlayerType;
        protected int maxLengthString;

        protected RandomizeSoundEmitter randomizeSoundEmitter;
        protected AudioClip error;
        protected AudioClip confirm;

        public NewGameScene(KeyCode exit = KeyCode.Return)
        {
            this.confirmKey = exit;
        }

        public override void Start()
        {
            insertYourNameTextObj = new TextObject(new OpenTK.Vector2(1, 3), "Insert your name:");
            playerNameTextObj = new TextObject(new OpenTK.Vector2(5.5f, 3), "");
            maxLengthString = 15;

            insertYourTypeTextObj = new TextObject(new OpenTK.Vector2(1, 5), "Are you a Boy a Girl or a Dog? (insert the first char):");
            playerTypeTextObj = new TextObject(new OpenTK.Vector2(1 + insertYourTypeTextObj.Text.Length * 0.255f, 5), "");
            inputString = "";

            actualInsertTextObj = insertYourNameTextObj;
            actualTextObj = playerNameTextObj;

            LoadAssets();

            randomizeSoundEmitter = new RandomizeSoundEmitter(null);
            error = AssetsMngr.GetClip("MenuBack01");
            confirm = AssetsMngr.GetClip("MenuCursor01");

            base.Start();
        }

        public override void Input()
        {
            if(Game.Window.GetKey(confirmKey))
            {
                if(inputString != null && inputString != "" && !isChoosingPlayerType)
                {
                    SaveGameManager.SaveGameDatas["PlayerData"]["Name"] = inputString;

                    isChoosingPlayerType = true;
                    maxLengthString = 1;
                    inputString = "";
                    actualTextObj = playerTypeTextObj;

                    randomizeSoundEmitter.Play(confirm);
                }

                else if(isChoosingPlayerType && inputString != null && inputString != "")
                {
                    if(inputString == "B") SaveGameManager.SaveGameDatas["PlayerData"]["PlayerType"] = "Adventurer";
                    else if(inputString == "G") SaveGameManager.SaveGameDatas["PlayerData"]["PlayerType"] = "Princess";
                    else if(inputString == "D") SaveGameManager.SaveGameDatas["PlayerData"]["PlayerType"] = "Dog";
                    else
                    {
                        randomizeSoundEmitter.Play(error);
                        //he dosen't have written the right letter
                        inputString = "";
                        actualTextObj.SetText("");
                        return;
                    }

                    randomizeSoundEmitter.Play(confirm);
                    IsPlaying = false;
                }
            }

            if (inputString.Length >= maxLengthString) return;

            //check is the key is keeped pressed
            if (Game.Window.GetKey(pressedKey))
            {
                if (CanTypeChar)
                {
                    inputString += pressedKey;
                    actualTextObj.SetText(inputString);
                    CanTypeChar = false;
                }
                return;
            }
            else CanTypeChar = true;

            //find the key is being pressed
            for (int i = ((int)KeyCode.A); i <= ((int)KeyCode.Z); i++)
            {
                KeyCode keyCode = (KeyCode)i;

                if (Game.Window.GetKey(keyCode))
                {
                    CanTypeChar = true;
                    pressedKey = keyCode;
                    break;
                }
            }
        }

        public override Scene OnExit()
        {
            playerNameTextObj.Clear();
            insertYourNameTextObj.Clear();
            playerTypeTextObj.Clear();
            insertYourTypeTextObj.Clear();

            actualTextObj = null;
            actualInsertTextObj = null;
            playerNameTextObj = null;
            playerTypeTextObj = null;
            insertYourNameTextObj = null;
            insertYourTypeTextObj = null;

            Game.InitMainGame();
            return base.OnExit();
        }

        public override void Draw()
        {
            DrawMngr.Draw();
        }

        private void LoadAssets()
        {
            string assetPath = "Assets/SFX/";
            AssetsMngr.AddClip("MenuBack01", assetPath + "MenuBack01.wav");
            AssetsMngr.AddClip("MenuCursor01", assetPath + "MenuCursor01.wav");
            AssetsMngr.AddClip("MenuValid01", assetPath + "MenuValid01.wav");
        }
    }
}
