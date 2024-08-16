using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    class WalkState : State
    {
        private Enemy enemy;
        private RandomTimer checkForVisiblePlayer;
        private RandomTimer checkForVisiblePowerUp;

        public WalkState(Enemy enemy)
        {
            this.enemy = enemy;
            checkForVisiblePlayer = new RandomTimer(0.2f, 0.5f);
            checkForVisiblePowerUp = new RandomTimer(0.5f, 1.4f);
        }

        public override void OnEnter()
        {
            checkForVisiblePlayer.Cancel();
            checkForVisiblePowerUp.Reset();
            enemy.PlayAnimation(ActorAnimations.Walk);
        }

        public override void Update()
        {
            checkForVisiblePlayer.Tick();

            if(checkForVisiblePlayer.IsOver())
            {
                Player p = enemy.GetVisiblePlayer();

                checkForVisiblePlayer.Reset();

                if (p != null)
                {
                    enemy.Rival = p;
                    stateMachine.GoTo(StateEnum.FOLLOW);
                    enemy.Agent.Target = null;
                    return;
                }
            }

            if(enemy.Energy < enemy.MaxEnergy)
            {
                checkForVisiblePowerUp.Tick();

                if(checkForVisiblePowerUp.IsOver())
                {
                    PowerUp p = enemy.GetNearestPowerUp();

                    if(p != null)
                    {
                        enemy.Target = p;
                        stateMachine.GoTo(StateEnum.RECHARGE);
                        checkForVisiblePowerUp.Reset();
                        return;
                    }

                    checkForVisiblePowerUp.Reset();
                }
            }

            enemy.HeadToPoint();
        }
    }
}
