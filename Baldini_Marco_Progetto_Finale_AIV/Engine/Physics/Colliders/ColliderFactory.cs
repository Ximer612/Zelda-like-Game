using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    static class ColliderFactory
    {
        public static CircleCollider CreateCircleFor(GameObject obj, bool innerCircle = true)
        {
            float radius;

            if(innerCircle)
            {
                if(obj.HalfWidth < obj.HalfHeight)
                {
                    radius = obj.HalfWidth;
                }
                else
                {
                    radius = obj.HalfHeight;
                }
            }
            else
            {
                radius = (float)Math.Sqrt(obj.HalfWidth * obj.HalfWidth + obj.HalfHeight * obj.HalfHeight);
            }

            return new CircleCollider(obj.RigidBody, radius);
        }

        public static BoxCollider CreateBoxFor(GameObject obj, float width = 0.9f, float height = 0.9f)
        {
            return new BoxCollider(obj.RigidBody, width, height);
        }
    }
}
