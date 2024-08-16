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
    class TeleportItem : Item
    {
        protected Scene scene;
        protected Vector2 exitPoint;
        protected static float viewRaySquared = 4*4;
        protected static AudioClip enterDoorAudio;
        public TeleportItem(Scene scene, Vector2 position, Vector2 exitPosition) : base("teleportArrow", position, ItemType.Teleport)
        {
            this.scene = scene;
            exitPoint = exitPosition;
            enterDoorAudio = AssetsMngr.GetClip("Land01");
        }

        public override void OnCollide(Collision collisionInfo)
        {
            PlayScene.CheckMusicToPlay();
            Game.GoToScene(scene);
            PlayScene.Player.Position = exitPoint;
            SaveGameManager.SavePlayer(true);
            SaveGameManager.SaveGame();
            IsActive = false;

            enterNewSceneSoundEmitter.Play(2);
        }

        public override void Draw()
        {
            if ((PlayScene.Player.Position - new Vector2(X, yStart)).LengthSquared < viewRaySquared)
            {
                    base.Draw();
            }
        }
    }
}
