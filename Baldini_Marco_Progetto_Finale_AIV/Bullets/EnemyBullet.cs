using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    class EnemyBullet : Bullet
    {
        public EnemyBullet() : base("sword")
        {
            maxSpeed = 3f;

            int r = RandomGenerator.GetRandomInt(50, 255);
            int g = RandomGenerator.GetRandomInt(50, 255);
            int b = RandomGenerator.GetRandomInt(50, 255);

            sprite.SetAdditiveTint(r, g, b, 1);

            Type = BulletType.EnemyBullet;
            RigidBody.Type = RigidBodyType.EnemyBullet;
            RigidBody.AddCollisionType(RigidBodyType.PlayerBullet |  RigidBodyType.Player);
        }

        public override void OnCollide(Collision collisionInfo)
        {
            BulletMngr.RestoreBullet(this);
        }
    }
}
