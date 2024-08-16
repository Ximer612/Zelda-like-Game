using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    abstract class PowerUp : GameObject
    {
        public PowerUp(string textureName,float w=0.8f, float h=0.8f) : base(textureName, DrawLayer.Foreground,w,h)
        {
            RigidBody = new RigidBody(this);
            RigidBody.Type = RigidBodyType.PowerUp;
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this,0.8f,0.8f);
            RigidBody.Collider.Offset = new Vector2(0.25f, 0.25f);
            RigidBody.AddCollisionType(RigidBodyType.Player | RigidBodyType.Enemy);
        }

        public override void OnCollide(Collision collisionInfo)
        {
            PowerUpsMngr.SoundEmitter.Play(pitch:RandomGenerator.GetRandomFloat() + 1);
        }
    }
}
