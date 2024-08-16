using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    class PlayerBullet : Bullet
    {
        public PlayerBullet() : base("sword",1,1)
        {
            maxSpeed = 15f;
            damage = 35;
            Type = BulletType.PlayerBullet;
            RigidBody.Type = RigidBodyType.PlayerBullet;
            RigidBody.AddCollisionType(RigidBodyType.EnemyBullet | RigidBodyType.Enemy);
        }

        public override void OnCollide(Collision collisionInfo)
        {
            if (collisionInfo.Collider is Enemy)
            {
                if (((Enemy)collisionInfo.Collider).IsAlive)
                    ((Enemy)collisionInfo.Collider).AddDamage(damage);
                else return;
            }
                BulletMngr.RestoreBullet(this);
        }
    }
}
