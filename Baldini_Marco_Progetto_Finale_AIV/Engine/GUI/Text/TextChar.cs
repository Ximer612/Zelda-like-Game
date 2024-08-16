using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    class TextChar : GameObject
    {
        protected char character;
        protected Font font;
        protected Vector2 textureOffset;
        public Vector2 Scale { get => sprite.scale; set => sprite.scale = value; }
        public char Character { get { return character; } set { character = value; ComputeOffset(); } }

        public TextChar(Vector2 spritePosition, char character, Font font) : base(font.TextureName, DrawLayer.GUI, Game.PixelsToUnits(font.CharacterWidth), Game.PixelsToUnits(font.CharacterHeight))
        {
            sprite.position = spritePosition;
            this.font = font;
            Character = character;
            sprite.pivot = Vector2.Zero;
            sprite.Camera = CameraMngr.GetCamera("GUI");
        }

        protected void ComputeOffset()
        {
            textureOffset = font.GetOffset(character);
        }

        public override void Draw()
        {
            if (IsActive)
            {
                sprite.DrawTexture(texture, (int)textureOffset.X, (int)textureOffset.Y, font.CharacterWidth, font.CharacterHeight);
            }
        }
    }
}
