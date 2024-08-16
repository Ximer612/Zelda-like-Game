using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    class EnergyPowerUp : PowerUp
    {
        public EnergyPowerUp() : base("energyPowerUp")
        {

        }

        public override void OnCollide(Collision collisionInfo)
        {
            ((Actor)collisionInfo.Collider).AddEnergy(25);
            IsActive = false;
            base.OnCollide(collisionInfo);
        }
    }
}
