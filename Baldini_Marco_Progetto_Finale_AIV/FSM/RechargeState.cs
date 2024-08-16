using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    class RechargeState : State
    {
        private Enemy enemy;

        public RechargeState(Enemy enemy)
        {
            this.enemy = enemy;
        }

        public override void OnEnter()
        {
            if(enemy.Target != null && enemy.Target.IsActive)
            {
                List<Node> path = ((PlayScene)Game.CurrentScene).PathFindingMap.GetPath((int)enemy.Position.X, (int)enemy.Position.Y, (int)enemy.Target.Position.X, (int)enemy.Target.Position.Y);
                enemy.Agent.SetPath(path);
            }
        }

        public override void Update()
        {
            if(enemy.Target == null || !enemy.Target.IsActive || enemy.Agent.Target == null)
            {
                enemy.Target = null;

                if(enemy.Rival != null && enemy.Rival.IsActive)
                {
                    stateMachine.GoTo(StateEnum.FOLLOW);
                }
                else
                {
                    stateMachine.GoTo(StateEnum.WALK);
                }
            }
            else
            {
                enemy.Agent.Update(enemy.followSpeed);
            }
        }
    }
}
