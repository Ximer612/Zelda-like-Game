using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    class JoypadController : Controller
    {
        public JoypadController(int ctrlIndex) : base(ctrlIndex)
        {

        }

        public override bool IsMoveButtonPressed()
        {
            return Game.Window.JoystickX(index);
        }

        public override bool IsInteractButtonPressed()
        {
            return Game.Window.JoystickA(index);
        }
    }
}
