using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    class FollowState : State
    {
        private Enemy enemy;
        private RandomTimer checkForNewPlayer;
        private RandomTimer checkForPowerUp;

        public FollowState(Enemy enemy)
        {
            this.enemy = enemy;

            checkForNewPlayer = new RandomTimer(0.2f, 1.2f);
            checkForPowerUp = new RandomTimer(0.4f, 1.35f);

            enemy.Agent.Target = null;
        }

        public override void OnEnter()
        {
            enemy.PlayAnimation(ActorAnimations.Walk);
        }

        protected virtual bool ContinueFollow(PowerUp nearestPowerUp)
        {
            float rechargeDistFuzzy = 1 - (nearestPowerUp.Position - enemy.Position).LengthSquared / (enemy.VisionRadius * enemy.VisionRadius);
            float rechargeNrgFuzzy = 1 - (float)enemy.Energy / (float)enemy.MaxEnergy;
            float rechargeSum = rechargeDistFuzzy + rechargeNrgFuzzy;

            float followDistFuzzy = 1 - (enemy.Rival.Position - enemy.Position).LengthSquared / (enemy.VisionRadius * enemy.VisionRadius);
            float followNrgFuzzy = Math.Min((float)enemy.Energy / (float)enemy.Rival.Energy, 1);
            float followSum = followDistFuzzy + followNrgFuzzy;

            return followSum > rechargeSum;
        }

        public override void Update()
        {
            checkForNewPlayer.Tick();

            if(checkForNewPlayer.IsOver() || enemy.Rival == null)
            {
                enemy.Rival = enemy.GetVisiblePlayer();

                checkForNewPlayer.Reset();
            }

            checkForPowerUp.Tick();

            if(checkForPowerUp.IsOver())
            {
                PowerUp p = enemy.GetNearestPowerUp();

                if (p != null)
                {
                    if (enemy.Rival == null || !ContinueFollow(p))
                    {
                        enemy.Target = p;
                        stateMachine.GoTo(StateEnum.RECHARGE);
                        checkForPowerUp.Reset();
                        return;
                    }
                }
            }
            
            if(enemy.Rival == null || !enemy.Rival.IsAlive)
            {
                stateMachine.GoTo(StateEnum.WALK);
            }
            else if(enemy.CanAttackPlayer())
            {
                stateMachine.GoTo(StateEnum.SHOOT);
            }
            else
            {
                enemy.HeadToPlayer();

            }
        }
    }
}
