using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    class Timer : IUpdatable
    {
        float timeToWait { get; set; }
        float remaningTime { get; set; }
        public bool isPaused { get; protected set; }
        public bool isPlaying { get; protected set; }

        public Timer(float timeToWait)
        {
            this.timeToWait = timeToWait;
        }

        public virtual void Update()
        {            
            remaningTime -= Game.DeltaTime;

            if(remaningTime <1)
            {
                isPlaying = false;
                Alarm();
            }
        }

        public virtual void Alarm()
        {

        }

        public virtual void Stop()
        {
            remaningTime = 0;
        }

        public virtual void Start()
        {
            remaningTime = timeToWait;
        }

        public virtual void Play()
        {
            isPlaying = true;
            isPaused = false;
        }

        public virtual void Pause()
        {
            isPlaying = false;
            isPaused = true;
        }
    }
}
