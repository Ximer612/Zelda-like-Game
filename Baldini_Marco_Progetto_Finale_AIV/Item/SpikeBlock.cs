using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    class SpikeBlock : Item
    {
        public SpikeBlock(Vector2 position, int w = 1, int h = 1) : base("spike", position, ItemType.Null, w, h,DrawLayer.Middleground)
        {
            RigidBody.Type = RigidBodyType.Spike;

            UpdateMngr.RemoveItem(this);
        }

        public override void OnCollide(Collision collisionInfo)
        {

        }

        public override void Draw()
        {
            sprite.DrawTexture(texture, (int)SpikesMngr.Animation.Offset.X, (int)SpikesMngr.Animation.Offset.Y, SpikesMngr.Animation.FrameWidth, SpikesMngr.Animation.FrameHeight);
        }
    }
}
