using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    interface IDrawable
    {
        DrawLayer DrawLayer { get; }
        void Draw();
    }
}
