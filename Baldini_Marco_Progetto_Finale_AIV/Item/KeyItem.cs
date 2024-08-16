using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    class KeyItem : Item
    {
        public KeyItem(ItemType type, Vector2 position, Vector4 color, int w = 1, int h = 1) : base("key", position, type, w, h)
        {
            sprite.SetMultiplyTint(color);
        }

        public override void OnCollide(Collision collisionInfo)
        {
            PlayScene.Player.AddItem(Type);
            PlayScene.Player.GUIitems[Type].UpdateAmount();
            SaveGameManager.SaveGameDatas[((PlayScene)Game.CurrentScene).MapFileName][XmlObjName] = "True";
            SaveGameManager.SavePlayer();
            IsActive = false;

            base.OnCollide(collisionInfo);
        }
    }
}
