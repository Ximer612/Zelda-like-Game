using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    abstract class GUIElement : GameObject
    {
        public bool IsSelected { get; set; }
        protected GameObject owner;
        protected Vector2 offset;
        protected GUIElement(Vector2 position, string textureName, GameObject owner=null) : base(textureName, DrawLayer.GUI,0.5f,0.5f)
        {
            this.owner = owner;
            Position = position;
            sprite.Camera = CameraMngr.GetCamera("GUI");
            offset = position - owner.Position;
            IsActive = true;
        }

        public void SetColor(Vector4 color)
        {
            sprite.SetAdditiveTint(color);
        }

        public void SetMultiplyColor(Vector4 color)
        {
            sprite.SetMultiplyTint(color);
        }
    }
}
