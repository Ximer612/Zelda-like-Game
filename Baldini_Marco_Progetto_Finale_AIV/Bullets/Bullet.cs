using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    enum BulletType { PlayerBullet, EnemyBullet, LAST }

    abstract class Bullet : GameObject
    {
        protected int damage = 25;
        public BulletType Type { get; protected set; }

        protected float maxSpeed;

        public Bullet(string texturePath, int spriteWidth=1, int spriteHeight=1) : base(texturePath,DrawLayer.Foreground,spriteWidth,spriteHeight)
        {
            maxSpeed = 5f;
            RigidBody = new RigidBody(this);
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this,0.4f,0.4f);
            RigidBody.Collider.Offset = new Vector2(0.15f, 0.15f);

            sprite.pivot = new Vector2(HalfWidth, HalfHeight);

            Game.OnSceneChange += () => BulletMngr.RestoreBullet(this);
        }

        public void Shoot(Vector2 shootPos, Vector2 shootDir)
        {
            sprite.position = shootPos;

            RigidBody.Velocity = shootDir * maxSpeed;
            Forward = shootDir;
            sprite.EulerRotation += 45;
        }

        public virtual void Reset()
        {
            IsActive = false;
        }

        public override void Update()
        {
            if (IsActive)
            {
                Vector2 cameraDist = Position - Game.ScreenCenter;

                if (cameraDist.LengthSquared > Game.HalfDiagonalSquared)
                {
                    BulletMngr.RestoreBullet(this);
                }
            }
        }
    }
}
