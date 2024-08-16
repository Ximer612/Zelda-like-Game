using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    class ShootState : State
    {
        private Enemy owner;

        private float shootTimeLimit = 0.5f;
        private float shootCoolDown = 0.0f;

        private RandomTimer checkForNewPlayer;
        private RandomTimer checkForPowerUp;

        public ShootState(Enemy enemy)
        {
            this.owner = enemy;
            checkForNewPlayer = new RandomTimer(0.2f, 1.2f);
            checkForPowerUp = new RandomTimer(0.4f, 1.35f);
        }

        public override void OnEnter()
        {
            this.owner.RigidBody.Velocity = Vector2.Zero;
            owner.PlayAnimation(ActorAnimations.Attack);
        }

        protected virtual bool ContinueAttack(PowerUp nearestPowerUp)
        {
            float rechargeNrgFuzzy = 1 - (float)owner.Energy / (float)owner.MaxEnergy;
            float rechargeSum = rechargeNrgFuzzy;

            float attackNrgFuzzy = Math.Min((float)owner.Energy / (float)owner.Rival.Energy, 1);
            float attackSum = attackNrgFuzzy;

            return attackSum > rechargeSum;
        }

        public override void Update()
        {
            checkForNewPlayer.Tick();

            if(checkForNewPlayer.IsOver())
            {
                if(owner.Rival == null)
                {
                    owner.Rival = owner.GetVisiblePlayer();
                    checkForNewPlayer.Reset();
                }
            }

            checkForPowerUp.Tick();

            if(checkForPowerUp.IsOver())
            {
                PowerUp p = owner.GetNearestPowerUp();

                if(p != null)
                {
                    if(owner.Rival == null || !ContinueAttack(p))
                    {
                        owner.Target = p;
                        stateMachine.GoTo(StateEnum.RECHARGE);
                        checkForPowerUp.Reset();
                        return;
                    }
                }
            }

            shootCoolDown -= Game.Window.DeltaTime;

            if(owner.Rival == null || !owner.CanAttackPlayer())
            {
                stateMachine.GoTo(StateEnum.WALK);
            }
            else
            {
                owner.LookAtPlayer();

                if(shootCoolDown <= 0.0f)
                {
                    shootCoolDown = shootTimeLimit;
                    owner.Shoot(owner.LookingDirection);
                }
                    owner.LookingDirection = new Vector2((int)Math.Round(owner.LookingDirection.X, 0), (int)Math.Round(owner.LookingDirection.Y, 0));
            }
        }
    }
}
