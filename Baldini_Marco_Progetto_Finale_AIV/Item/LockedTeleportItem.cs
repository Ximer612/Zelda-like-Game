using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    class LockedTeleportItem : TeleportItem
    {
        ItemType keyNeeded;
        ColorType colorKey;

        public LockedTeleportItem(Scene scene, Vector2 position, Vector2 exitPosition, ItemType keyNeeded) : base(scene, position, exitPosition)
        {
            if (!PlayScene.FirstLoad && keyNeeded != ItemType.LAST)
            {
                string colorName = keyNeeded.ToString();
                int index = colorName.IndexOf("Key");
                colorName = colorName.Substring(0, index);
                ColorType colorType;
                Enum.TryParse(colorName, out colorType);
                colorKey = colorType;
                this.keyNeeded = keyNeeded;
            }
        }

        public override void OnCollide(Collision collisionInfo)
        {
            if (PlayScene.Player.Inventory[keyNeeded] < 1)
            {
                ItemTextMngr.SetText($"You need the ({colorKey} key) to open this door!");
                PlayScene.Player.Y = (int)PlayScene.Player.Y + 1;
                PlayScene.Player.Agent.ResetPath();
                return;
            }

            base.OnCollide(collisionInfo);
        }

        public override void Draw()
        {
            if (PlayScene.Player.Inventory[keyNeeded] > 0)
            {
                if ((PlayScene.Player.Position - new Vector2(X, yStart)).LengthSquared < viewRaySquared)
                    base.Draw();
            }
        }
    }
}
