using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;

namespace Baldini_Marco_Progetto_Finale_AIV
{

    class KeyboardController : Controller
    {
        protected KeysList keysConfig;
        public KeyboardController(int ctrlIndex, KeysList keys) : base(ctrlIndex)
        {
            keysConfig = keys;
        }        

        public override bool IsMoveButtonPressed()
        {
            return Game.Window.GetKey(keysConfig.GetKey(KeyName.LeftButton));
        }

        public override bool IsInteractButtonPressed()
        {
            return Game.Window.GetKey(keysConfig.GetKey(KeyName.RightButton));
        }
    }
}
