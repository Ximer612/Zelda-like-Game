using Aiv.Audio;
using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    enum MusicName { Adventure, Dark_Cave, Eerie, LAST}

    class PlayScene : Scene
    {
        static protected SoundEmitter backgroundMusic;
        static public bool FirstLoad { get; private set; }
        public List<Item> Items;
        public static Player Player { get; protected set; }
        protected TmxMap tiledMap;
        public Map PathFindingMap { get; protected set; }
        public string MapFileName { get; protected set; }

        public List<GameObject> ObjectToAddCollisions;
        public List<Enemy> Enemies;

        public Vector4 ClearColor;

        public static void Init()
        {
            LoadAssets(SaveGameManager.SaveGameDatas["PlayerData"]["PlayerType"]);

            Controller ctrl = Game.GetController();
            Player = new Player(ctrl);

            CameraMngr.Target = Player;
            CameraMngr.MainCamera.position = Player.Position;

            FirstLoad = true;

            backgroundMusic = new SoundEmitter(null, "Track01");

            InitMaps();

            backgroundMusic.Play(0.5f,loop:true);

            DrawMngr.AddFX("blackBandUnderGUIFX", new BlackBandUnderGUIFX());
            BulletMngr.Init();
            PowerUpsMngr.Init();
            ItemTextMngr.Init();

            //DebugMngr.SetShouldDraw(true);
        }

        public static void CheckMusicToPlay()
        {
            if (Game.CurrentScene == Game.Scenes["Undergound_Inside"] || Game.CurrentScene == Game.Scenes["BossRoom_Inside"])
            {
                PlayMusic(MusicName.Dark_Cave);
            }
            else if (Game.CurrentScene == Game.Scenes["City_Outside"])
            {
                PlayMusic(MusicName.Eerie);
            }
            else
            {
                PlayMusic(MusicName.Adventure);
            }
        }

        public static  void PlayMusic(MusicName name)
        {
            int musicID = ((int)name) + 1;

            AudioClip clipToPlay = AssetsMngr.GetClip("Track0" + musicID);

            if (backgroundMusic.Clip == clipToPlay)
                return;

            backgroundMusic.Play(0.5f,1, clipToPlay,true);
        }

        protected void LoadMap()
        {
            int[] cells = tiledMap.SolidTilesIDs;

            PathFindingMap = new Map(TmxMap.Width, TmxMap.Height, cells);


            if (FirstLoad) return;

            //first load of the map the player is

            for (int i = 0; i < ObjectToAddCollisions.Count; i++)
            {
                PathFindingMap.ToggleNode(((int)ObjectToAddCollisions[i].X), ((int)ObjectToAddCollisions[i].Y));
            }

            ObjectToAddCollisions.Clear();

            Game.Window.SetClearColor(ClearColor);
            SpikesMngr.Init();
        }

        public PlayScene(string mapFileName, Vector3 clearColor)
        {
            this.MapFileName = mapFileName;
            ClearColor = new Vector4(clearColor.X/255,clearColor.Y/255,clearColor.Z/255,255/255);
        }

        protected static void InitMaps()
        {
            foreach (var scene in Game.Scenes)
            {
                if (scene.Value is PlayScene)
                ((PlayScene)scene.Value).Start();

            }
            FirstLoad = false;
        }

        protected static void LoadAssets(string playerTextureName)
        {
            string assetPath;

            AssetsMngr.AddTexture("tileset", "Assets/TILESET/PixelPackTOPDOWN8BIT.png");
            AssetsMngr.AddTexture("barFrame", "Assets/SPRITES/GUI/loadingBar_frame.png");
            AssetsMngr.AddTexture("blueBar", "Assets/SPRITES/GUI/loadingBar_bar.png");

            //ITEMS

            assetPath = "Assets/SPRITES/ITEMS/item8BIT_";

            AssetsMngr.AddTexture("teleportArrow", assetPath+"arrow.png");
            AssetsMngr.AddTexture("coin", assetPath + "coin.png");
            AssetsMngr.AddTexture("key", assetPath + "key.png");
            AssetsMngr.AddTexture("floppyDisk", assetPath + "floppydisk.png");
            AssetsMngr.AddTexture("sword", assetPath + "sword.png");
            AssetsMngr.AddTexture("stone", assetPath + "stone.png");
            AssetsMngr.AddTexture("energyPowerUp", assetPath + "heart.png");
            AssetsMngr.AddTexture("spike", assetPath + "spike.png");

            //ANIMATIONS

            //PLAYER ANIMATIONS
            assetPath = "Assets/SPRITES/HEROS/spritesheets/HEROS8bit_"+playerTextureName + " ";
            AssetsMngr.AddTexture("playerDeath", assetPath + "Death.png");
            string[] animationsName = new string[] { "Attack", "Hurt", "Idle", "JumpRoll", "Push", "Walk"};

            for (int i = 0; i < animationsName.Length; i++)
            {
                AssetsMngr.AddTexture("player" + animationsName[i] + " D", assetPath + animationsName[i] + " D.png");
                AssetsMngr.AddTexture("player" + animationsName[i] + " R", assetPath + animationsName[i] + " R.png");
                AssetsMngr.AddTexture("player" + animationsName[i] + " U", assetPath + animationsName[i] + " U.png");
            }

            // OTHER ANIMATIONS
            string[] multiDirectionalEnemiesNames = new string[] { "Snake", "Sorcerer" };

            assetPath = "Assets/SPRITES/ENEMIES/spritesheets/ENEMIES8bit_";

            animationsName = new string[] { "Attack", "Hurt", "Idle", "Walk"};

            for (int i = 0; i < multiDirectionalEnemiesNames.Length; i++)
            {
                for (int j = 0; j < animationsName.Length; j++)
                {
                    try
                    {
                        AssetsMngr.AddTexture(multiDirectionalEnemiesNames[i] + animationsName[j] + " D", assetPath + multiDirectionalEnemiesNames[i] + " " + animationsName[j] + " D.png");
                        AssetsMngr.AddTexture(multiDirectionalEnemiesNames[i] + animationsName[j] + " R", assetPath + multiDirectionalEnemiesNames[i] + " " + animationsName[j] + " R.png");
                        AssetsMngr.AddTexture(multiDirectionalEnemiesNames[i] + animationsName[j] + " U", assetPath + multiDirectionalEnemiesNames[i] + " " + animationsName[j] + " U.png");
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }

                AssetsMngr.AddTexture(multiDirectionalEnemiesNames[i] +"Death", assetPath + multiDirectionalEnemiesNames[i] + " Death.png");
            }

            // AUDIO CLIPS

            //MUSIC
            assetPath = "Assets/MUSIC/1BITTopDownMusics - ";
            AssetsMngr.AddClip("Track01", assetPath+ "Track 01 (1BIT Adventure).wav");
            AssetsMngr.AddClip("Track02", assetPath+ "Track 02 (1BIT Dark Cave).wav");
            AssetsMngr.AddClip("Track03", assetPath+ "Track 03 (1BIT Eerie).wav");

            //SFX
            assetPath = "Assets/SFX/";
            AssetsMngr.AddClip("Attack01", assetPath + "Attack01.wav");
            AssetsMngr.AddClip("Attack02", assetPath + "Attack02.wav");
            AssetsMngr.AddClip("Attack03", assetPath + "Attack03.wav");
            AssetsMngr.AddClip("Death01", assetPath + "Death01.wav");
            AssetsMngr.AddClip("Death02", assetPath + "Death02.wav");
            AssetsMngr.AddClip("Hurt01", assetPath + "Hurt01.wav");
            AssetsMngr.AddClip("Hurt02", assetPath + "Hurt02.wav");
            AssetsMngr.AddClip("Land01", assetPath + "Land01.wav");
            AssetsMngr.AddClip("Pickup01", assetPath + "Pickup01.wav");
            AssetsMngr.AddClip("MenuBack01", assetPath + "MenuBack01.wav");
            AssetsMngr.AddClip("MenuCursor01", assetPath + "MenuCursor01.wav");
            AssetsMngr.AddClip("MenuValid01", assetPath + "MenuValid01.wav");

        }

        public override void Input()
        {
            if (Player.IsAlive)
                Player.Input();
        }

        public override void Update()
        {
            if (!Player.IsAlive)
                IsPlaying = false;

            PhysicsMngr.Update();
            UpdateMngr.Update();
            PowerUpsMngr.Update();
            PhysicsMngr.CheckCollisions();
            CameraMngr.Update();
            SpikesMngr.Update();
        }

        public static void InitCameras()
        {
            // CAMERA INITS
            int cameraValue = (int)(Game.Window.CurrentViewportOrthographicSize * 0.5f);
            int tileSize = 32;

            CameraLimits limits = new CameraLimits(tileSize - cameraValue, cameraValue, tileSize - cameraValue, cameraValue);

            CameraMngr.Init(null, limits);
            CameraMngr.AddCamera("GUI", new Camera());
        }

        public override Scene OnExit()
        {
            CameraMngr.MainCamera.position = Player.Position;
            for (int i = 0; i < Items.Count; i++)
            {
                Items[i].Destroy();
            }
            Items.Clear();
            PathFindingMap = null;
            tiledMap.ClearAll();
            tiledMap = null;

            for (int i = 0; i < Enemies.Count; i++)
            {
                Enemies[i].OnDie(false);
            }

            Enemies.Clear();

            GC.Collect();
            return base.OnExit();
        }

        public override void Draw()
        {
            DrawMngr.Draw();
            ItemTextMngr.Draw();
            //DebugMngr.Draw();
        }
        public virtual void OnPlayerDie()
        {
            if (!Player.IsAlive)
            {
                Game.GoToScene(Game.Scenes["gameOverScene"]);
            }
        }

        public override void Start()
        {
            Enemies = new List<Enemy>();
            ObjectToAddCollisions = new List<GameObject>();
            Items = new List<Item>();
            Player.Agent.ResetPath();

            base.Start();

            tiledMap = new TmxMap("Assets/Maps/"+ MapFileName+".tmx", "Assets/TILESET/PixelPackTileset.xml", Items);

            LoadMap();

            CheckMusicToPlay();
        }

        public static void Restart()
        {
            for (int i = 0; i < CameraMngr.TextsObjects.Count; i++)
            {
                CameraMngr.TextsObjects[i].Clear();
            }

            CameraMngr.TextsObjects.Clear();
            ItemTextMngr.ClearAll();

            SaveGameManager.LoadSave();

            backgroundMusic.Stop();
            Controller ctrl = Game.GetController();
            Player.Clear();
            CameraMngr.ClearAll();
            ItemTextMngr.Init();

            Player = new Player(ctrl);

            CameraMngr.Target = Player;
            CameraMngr.MainCamera.position = Player.Position;

            backgroundMusic.Play(0.5f, loop: true);


            GC.Collect();
        }
    }
}
