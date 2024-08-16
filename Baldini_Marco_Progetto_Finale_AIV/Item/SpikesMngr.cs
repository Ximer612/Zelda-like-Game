using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    static class SpikesMngr
    {
        static public bool CanDealDamage { get; private set; }
        static public Animation Animation;

        static public void Init()
        {
            Animation = new Animation(null, 2, 16, 16, 0.4f, true);
            Animation.Play();
        }

        static public void Update()
        {
            Animation.Update();
            CanDealDamage = Animation.Offset.X == 16 ? true : false;
        }
    }
}
