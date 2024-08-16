using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    class GUIitem : GUIElement
    {
        protected int amount;
        protected TextObject amountText;
        ItemType itemType;

        public GUIitem(ItemType item, Vector2 position, string textureName, GameObject owner = null) : base(position, textureName, owner)
        {
            itemType = item;
            amountText = new TextObject(new Vector2(position.X + 0.5f, position.Y + 0.15f));
            amount = int.Parse(SaveGameManager.SaveGameDatas["PlayerData"][item.ToString()]);
            IsActive = true;
            amountText.IsActive = true;
        }

        public override void Destroy()
        {
            amountText.Clear();
            amountText = null;
            base.Destroy();
        }

        public void UpdateAmount()
        {
            amount = int.Parse(SaveGameManager.SaveGameDatas["PlayerData"][itemType.ToString()]);
            amountText.SetText(amount.ToString());
        }

        public void SetScale(float amount)
        {
            sprite.scale = new Vector2(amount);
        }
    }
}
