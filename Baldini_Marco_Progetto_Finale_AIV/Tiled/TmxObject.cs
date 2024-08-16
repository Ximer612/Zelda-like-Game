using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    class TmxObject : GameObject
    {
        int id;

        int xOff, yOff;

        int w, h;

        public TmxObject(int id, int offsetX, int offsetY, int w, int h, bool solid) : base("tileset", DrawLayer.Middleground, 1, 1)
        {
            this.id = id;
            this.w = w;
            this.h = h;
            xOff = offsetX;
            yOff = offsetY;

            if(solid)
            {
                RigidBody = new RigidBody(this);
                RigidBody.Collider = ColliderFactory.CreateBoxFor(this);
            }

            IsActive = true;

        }

        public override void Draw()
        {
            if(IsActive)
            {
                sprite.DrawTexture(texture,xOff,yOff,w,h);
            }
        }
    }
}
