using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Aiv.Fast2D;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    class ProgressBar : GameObject
    {
        protected Vector2 barOffset;

        protected Sprite barSprite;
        protected Texture barTexture;

        protected float barWidth;

        public override Vector2 Position { get => base.Position; set { base.Position = value; barSprite.position = value + barOffset; } }
        private Vector2 originalPosition;

        private static float actualScaleValue = 1;
        public ProgressBar(string frameTextureName, string barTextureName, Vector2 innerBarOffset) :base(frameTextureName,DrawLayer.GUI)
        {
            sprite.pivot = Vector2.Zero;
            IsActive = true;

            barOffset = innerBarOffset;

            barTexture = AssetsMngr.GetTexture(barTextureName);
            barSprite = new Sprite(Game.PixelsToUnits(barTexture.Width), Game.PixelsToUnits(barTexture.Height));

            barWidth = barTexture.Width;

            sprite.Camera = CameraMngr.GetCamera("GUI");
            barSprite.Camera = CameraMngr.GetCamera("GUI");

            SetScale(1);
        }

        public void SetOriginalPosition()
        {
            originalPosition = Position;
        }

        public void SetDrawLayer(DrawLayer layer)
        {
            DrawLayer = layer;

            if(layer != DrawLayer.GUI)
            {
                sprite.Camera = CameraMngr.MainCamera;
                barSprite.Camera = CameraMngr.MainCamera;
            }
        }

        public virtual void Scale(float scale)
        {
            barSprite.SetMultiplyTint((1 - scale) * 50, scale * 2, scale, 1);

            scale = MathHelper.Clamp(scale, 0, 1);

            barSprite.scale.X = actualScaleValue * scale * 1;
            barWidth = (barTexture.Width * scale);
        }

        public virtual void ScaleNoZoom(float scale)
        {
            scale = MathHelper.Clamp(scale, 0, 1);

            barSprite.scale.X = scale;
            barWidth = barTexture.Width * scale;

            barSprite.SetMultiplyTint((1 - scale) * 50, scale * 2, scale, 1);
        }

        public void SetScale(float amount)
        {
            amount = Math.Min(amount, Game.OriginalOrthograpicSize);

            sprite.scale = new Vector2(amount);
            barSprite.scale = new Vector2(amount);

            sprite.position  = originalPosition * amount;
            barSprite.position  = ( originalPosition + barOffset )*amount;

            actualScaleValue = amount;
        }

        public override void Draw()
        {
            if(IsActive)
            {
                base.Draw();
                barSprite.DrawTexture(barTexture, 0, 0, (int)barWidth, barTexture.Height);
            }
        }
    }
}
