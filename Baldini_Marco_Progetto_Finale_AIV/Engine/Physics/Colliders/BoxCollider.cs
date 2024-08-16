using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    class BoxCollider : Collider
    {
        protected float halfWidth;
        protected float halfHeight;
        public float Width { get { return halfWidth * 2f; } }
        public float Height { get { return halfHeight * 2f; } }

        public BoxCollider(RigidBody owner, float w, float h) : base(owner)
        {
            halfWidth = w * 0.5f;
            halfHeight = h * 0.5f;

            DebugMngr.AddItem(this);
        }

        public override bool Collides(Collider collider, ref Collision collisionInfo)
        {
            return collider.Collides(this, ref collisionInfo);
        }

        public override bool Collides(CircleCollider circle, ref Collision collisionInfo)
        {
            float deltaX = circle.Position.X - Math.Max(Position.X - halfWidth,  Math.Min(circle.Position.X, Position.X + halfWidth));
            float deltaY = circle.Position.Y - Math.Max(Position.Y - halfHeight, Math.Min(circle.Position.Y, Position.Y + halfHeight));

            return (deltaX * deltaX + deltaY * deltaY) < (circle.Radius * circle.Radius);
        }

        public override bool Collides(BoxCollider other, ref Collision collisionInfo)
        {
            Vector2 dist = other.Position - Position;

            float deltaX = Math.Abs(dist.X) - (other.halfWidth + halfWidth);

            //No Horizontal Collision
            if(deltaX > 0)
            {
                return false;
            }

            float deltaY = Math.Abs(dist.Y) - (other.halfHeight + halfHeight);

            // No Vertical Collision
            if(deltaY > 0)
            {
                return false;
            }

            // Collision is Happening
            collisionInfo.Type = CollisionType.RectsIntersection;
            collisionInfo.Delta = new Vector2(-deltaX, -deltaY);//- because delta always negative

            return true;
        }

        public override bool Collides(CompoundCollider other, ref Collision collisionInfo)
        {
            return other.Collides(this, ref collisionInfo);
        }
    }
}
