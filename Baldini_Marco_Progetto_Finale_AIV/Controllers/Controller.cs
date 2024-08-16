using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    abstract class Controller
    {
        protected int index;

        public Controller(int ctrlIndex)
        {
            index = ctrlIndex;
        }

        public abstract bool IsMoveButtonPressed();
        public abstract bool IsInteractButtonPressed();
    }
}
