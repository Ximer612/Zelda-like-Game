﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    abstract class State
    {
        public virtual void OnEnter() { }
        public virtual void OnExit() { }
        public abstract void Update();

        protected StateMachine stateMachine;

        public void SetStateMachine(StateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }
    }
}
