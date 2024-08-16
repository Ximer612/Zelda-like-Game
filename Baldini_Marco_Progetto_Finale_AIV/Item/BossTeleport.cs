using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    class BossTeleport : TeleportItem
    {
        private int enemiesToKill;
        private int enemiesKilledOnEnterRoom;
        public BossTeleport(Scene scene, Vector2 position, Vector2 exitPosition, int enemiesToKill) : base(scene, position, exitPosition)
        {
            Type = ItemType.BossTeleport;

            sprite.SetMultiplyTint(0.6f, 0.5f, 0.5f, 1f);

            if (!PlayScene.FirstLoad)
            {
                enemiesKilledOnEnterRoom = ItemTextMngr.EnemiesKilledCount;
                this.enemiesToKill = enemiesKilledOnEnterRoom+enemiesToKill;
            }
        }

        public override void OnCollide(Collision collisionInfo)
        {
            if (ItemTextMngr.EnemiesKilledCount < enemiesToKill)
            {
                ItemTextMngr.SetText($"You need to kill ({enemiesToKill}) enemies to open this door!");
                PlayScene.Player.Y = (int)PlayScene.Player.Y + 1;
                PlayScene.Player.Agent.ResetPath();
                return;
            }

            PlayScene.Player.PlayAnimation(ActorAnimations.Push);
            base.OnCollide(collisionInfo);
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
