using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    class RandomTimer
    {

        private float timeMin;
        private float timeMax;
        public float RemaningSeconds { get; private set; }

        public RandomTimer(float timeMiN, float timeMaX)
        {
            timeMax = timeMaX;
            timeMin = timeMiN;
        }

        public void Reset()
        {
            RemaningSeconds = RandomGenerator.GetRandomFloat() * (timeMax - timeMin) + timeMin;
        }

        public void Cancel()
        {
            RemaningSeconds = 0.0f;
        }

        public void Tick() //Update
        {
            RemaningSeconds -= Game.DeltaTime;

            if(RemaningSeconds <= 0.0f)
            {
                RemaningSeconds = 0.0f;
            }
        }

        public bool IsOver()
        {
            return RemaningSeconds <= 0.0f;
        }
    }
}
