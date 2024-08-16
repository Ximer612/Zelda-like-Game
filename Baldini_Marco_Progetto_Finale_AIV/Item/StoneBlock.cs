using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    class StoneBlock : Item
    {
        public StoneBlock(Vector2 position, int w = 1, int h = 1) : base("stone", position, ItemType.Null, w, h,DrawLayer.Middleground)
        {
            RigidBody.AddCollisionType(RigidBodyType.PlayerBullet | RigidBodyType.EnemyBullet);
            RigidBody.RemoveCollisionType(RigidBodyType.Player);
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this, 0.8f, 0.8f);
            RigidBody.Collider.Offset = new Vector2(0.3f, 0.3f);
            UpdateMngr.RemoveItem(this);
        }
        public override void OnCollide(Collision collisionInfo)
        {
            ((PlayScene)Game.CurrentScene).PathFindingMap.ToggleNode((int)Position.X, (int)Position.Y);

            SaveGameManager.SaveGameDatas[((PlayScene)Game.CurrentScene).MapFileName][XmlObjName] = "True";
            BulletMngr.RestoreBullet((Bullet)collisionInfo.Collider);
            Destroy();
        }
    }
}
