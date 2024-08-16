using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    enum CollisionType { None, RectsIntersection, CirclesIntersection, RectCircleIntersection, LAST }

    struct Collision
    {
        public GameObject Collider; //other in OnCollide
        public Vector2 Delta; // delta = differenziale degli oggetti dopo essere compenetrati
        public CollisionType Type;
    }
}
