using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    class GameOverScene : TitleScene
    {
        TextObject[] gameOverTexts;
        bool pressedRestart;
        public GameOverScene() : base("Assets/TILESET/PixelPackTOPDOWN8BIT.png", Aiv.Fast2D.KeyCode.N)
        {

        }

        public override void Start()
        {
            pressedRestart = false;

            PlayScene.PlayMusic(MusicName.Eerie);

            gameOverTexts = new TextObject[3];
            gameOverTexts[0] = new TextObject(new OpenTK.Vector2(2,1), "GAME OVER");
            gameOverTexts[1] = new TextObject(new OpenTK.Vector2(2,6), "Y = RESTART");
            gameOverTexts[2] = new TextObject(new OpenTK.Vector2(2,8), "N = EXIT");
            gameOverTexts[0].SetScale(5);
            gameOverTexts[1].SetScale(3);
            gameOverTexts[2].SetScale(3);

            Game.Window.SetDefaultViewportOrthographicSize(Game.OriginalOrthograpicSize);
            Game.Window.SetClearColor(0, 0, 0, 1);

            base.Start();
        }

        public override void Input()
        {
            base.Input();

            if(IsPlaying && Game.Window.GetKey(Aiv.Fast2D.KeyCode.Y))
            {
                NextScene = Game.Scenes[SaveGameManager.SaveGameDatas["PlayerData"]["CurrentScene"]];
                IsPlaying = false;
                pressedRestart = true;
            }
        }

        public override Scene OnExit()
        {
            if (!pressedRestart) NextScene = null;

            IsPlaying = false;
            texture = null;
            sprite = null;

            for (int i = 0; i < gameOverTexts.Length; i++)
            {
                gameOverTexts[i].Clear();
                gameOverTexts[i] = null;
            }

            PlayScene.Restart();

            return NextScene;

        }

        public override void Draw()
        {
            for (int i = 0; i < gameOverTexts.Length; i++)
            {
                gameOverTexts[i].Draw();
            }
        }
    }
}
