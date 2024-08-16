using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    class CoinItem : Item
    {
        public CoinItem(Vector2 position, int w = 1, int h = 1) : base("coin", position, ItemType.Coin, w, h)
        {

        }

        public override void OnCollide(Collision collisionInfo)
        {
            PlayScene.Player.AddItem(Type);
            PlayScene.Player.GUIitems[Type].UpdateAmount();
            SaveGameManager.SaveGameDatas[((PlayScene)Game.CurrentScene).MapFileName][XmlObjName] = "True";
            IsActive = false;

            base.OnCollide(collisionInfo); 
        }
    }
}
