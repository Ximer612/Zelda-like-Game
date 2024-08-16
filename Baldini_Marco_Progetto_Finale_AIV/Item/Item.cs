using Aiv.Audio;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    enum ItemType { Null, Teleport, Coin, RedKey, BlueKey, YellowKey, GreenKey, BossTeleport, Spike, LAST}

    abstract class Item : GameObject
    {
        protected float floatingCount;
        protected float yStart;
        public ItemType Type;
        public string XmlObjName;

        protected static RandomizeSoundEmitter pickupItemSoundEmitter;
        protected static RandomizeSoundEmitter enterNewSceneSoundEmitter;

        static Item()
        {
            pickupItemSoundEmitter = new RandomizeSoundEmitter(null);
            pickupItemSoundEmitter.AddClip("Pickup01");

            enterNewSceneSoundEmitter = new RandomizeSoundEmitter(null);
            enterNewSceneSoundEmitter.AddClip("Land01");
        }

        protected Item(string textureName,Vector2 position,ItemType type,int w = 1, int h = 1,DrawLayer layer=DrawLayer.Playground) : base(textureName, layer, w,h)
        {
            Type = type;
            //RIGIDBODY
            RigidBody = new RigidBody(this);
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this,0.3f, 0.3f);
            RigidBody.Collider.Offset = new Vector2(0.4f, 0.4f);
            RigidBody.Type = RigidBodyType.Item;
            RigidBody.AddCollisionType(RigidBodyType.Player);

            Position = position;
            yStart = Position.Y-0.5f;
        }

        public override void OnCollide(Collision collisionInfo)
        {
            //plays collected item audio
            pickupItemSoundEmitter.Play();
        }

        public override void Update()
        {
            //makes items floating
            floatingCount += Game.DeltaTime;
            Y = yStart +  0.25f * (float)Math.Sin(floatingCount * 5) + 0.5f;
        }
    }
}
