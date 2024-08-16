using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    enum ComponentType { SoundEmitter, RandomizeSoundEmitter, Animation}

    abstract class Component
    {
        public GameObject GameObject { get; protected set; }
        public bool IsEnabled { get; set; }

        public Component(GameObject owner)
        {
            GameObject = owner;
        }

        public bool IsEnabledWithGameObject()
        {
            if (GameObject != null)
                return IsEnabled && GameObject.IsActive;
            else return false;
        }

    }
}
