using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    static class Game
    {
        public static Window Window;
        public static Scene CurrentScene { get; private set; }
        public static float DeltaTime { get { return Window.DeltaTime; } }

        private static KeyboardController keyboardCtrl;
        public static float UnitSize { get; private set; }
        public static float OptimalScreenHeight { get; private set; }
        public static float OptimalUnitSize { get; private set; }
        public static Vector2 ScreenCenter { get; private set; }
        public static float HalfDiagonalSquared { get => ScreenCenter.LengthSquared; }
        public static int OriginalOrthograpicSize;

        public static Dictionary<string,Scene> Scenes { get; private set; }
        public static event Action OnSceneChange;

        public static void Init()
        {
            Window = new Window(720, 720, "Baldini Marco Progetto Finale AIV");
            Window.SetVSync(false);
            Window.SetDefaultViewportOrthographicSize(16);

            OriginalOrthograpicSize = ((int)Window.CurrentViewportOrthographicSize);

            OptimalScreenHeight = 1024;//best resolution
            UnitSize = Window.Height / Window.OrthoHeight;
            OptimalUnitSize = OptimalScreenHeight / Window.OrthoHeight;

            ScreenCenter = new Vector2(OriginalOrthograpicSize, OriginalOrthograpicSize);

            // CONTROLLERS
            //create default keys for keyboard controller
            List<KeyCode> keys = new List<KeyCode>();
            keys.Add(KeyCode.Z);
            keys.Add(KeyCode.X);

            KeysList keysList = new KeysList(keys);
            keyboardCtrl = new KeyboardController(0, keysList);

            // FONTS INIT
            FontMngr.Init();
            Font sonicFont = FontMngr.AddFont("sonicFont", "Assets/TEXT/sonicFontSheet.png", 8, ' ', 16, 16);
            Font stdFont = FontMngr.AddFont("stdFont", "Assets/TEXT/textSheet.png", 15, ' ', 20, 20);
            Font comic = FontMngr.AddFont("comics", "Assets/TEXT/comics.png", 10, ' ', 61, 65);

            PlayScene.InitCameras();

            // SAVEGAME 
            SaveGameManager.Init();
            SaveGameManager.LoadSave();

            // LOAD SCENE
            Scenes = new Dictionary<string, Scene>();

            PlayScene islandWorldScene = new PlayScene("Island_Outside", new Vector3(20,68,145));
            Scenes[islandWorldScene.MapFileName] = islandWorldScene;
            PlayScene cityWorldScene = new PlayScene("City_Outside", new Vector3(57, 41, 70));
            Scenes[cityWorldScene.MapFileName] = cityWorldScene;
            PlayScene blueHouseScene = new PlayScene("BlueHouse_Inside", new Vector3(0, 0, 0));
            Scenes[blueHouseScene.MapFileName] = blueHouseScene;
            PlayScene redHouseScene = new PlayScene("RedHouse_Inside", new Vector3(0, 0, 0));
            Scenes[redHouseScene.MapFileName] = redHouseScene;
            PlayScene undergoundScene = new PlayScene("Undergound_Inside", new Vector3(0, 0, 0));
            Scenes[undergoundScene.MapFileName] = undergoundScene;
            PlayScene bossRoomScene = new PlayScene("BossRoom_Inside", new Vector3(0, 0, 0));
            Scenes[bossRoomScene.MapFileName] = bossRoomScene;
            PlayScene gardenWorldScene = new PlayScene("Garden_Outside", new Vector3(72, 139, 212));
            Scenes[gardenWorldScene.MapFileName] = gardenWorldScene;

            GameOverScene gameOverScene = new GameOverScene();
            Scenes["gameOverScene"] = gameOverScene;
            gameOverScene.NextScene = islandWorldScene;

            if (!SaveGameManager.IsSaveGameFileExist)
            {
                NewGameScene newGameScene = new NewGameScene();
                newGameScene.NextScene = islandWorldScene;
                CurrentScene = newGameScene;
            }
            else
            {
                InitMainGame();
            }
        }

        public static void InitMainGame()
        {
            PlayScene.Init();
            CurrentScene = Scenes[SaveGameManager.SaveGameDatas["PlayerData"]["CurrentScene"]];
        }
        public static float PixelsToUnits(float pixelsSize)
        {
            return pixelsSize / OptimalUnitSize;
        }
        public static Controller GetController()
        {
            return keyboardCtrl;
        }
        public static void Play()
        {
            CurrentScene.Start();

            while (Window.IsOpened)
            {
                // Show FPS on Window Title Bar
                Window.SetTitle($"Baldini Marco Progetto Finale AIV");

                // Exit when ESC is pressed
                if (Window.GetKey(KeyCode.Esc))
                {
                    break;
                }

                if (!CurrentScene.IsPlaying)
                {
                    Scene nextScene = CurrentScene.OnExit();

                    if(nextScene != null)
                    {
                        CurrentScene = nextScene;
                        CurrentScene.Start();
                    }
                    else
                    {
                        return;
                    }
                }

                // INPUT
                CurrentScene.Input();

                // UPDATE
                CurrentScene.Update();

                // DRAW
                CurrentScene.Draw();

                Window.Update();
            }

        }
        public static void GoToScene(Scene scene)
        {
            CurrentScene.NextScene = scene;
            CurrentScene.IsPlaying = false;

            OnSceneChange.Invoke();
        }
    }
}
