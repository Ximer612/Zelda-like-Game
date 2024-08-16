using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    class Enemy : Actor
    {
        public float VisionRadius { get; protected set; }
        protected float shootDistance;

        protected float halfConeAngle = MathHelper.DegreesToRadians(40);
        protected Vector2 pointToReach;

        protected StateMachine fsm;

        public float followSpeed;
        public float walkSpeed;

        public Player Rival;
        public GameObject Target;
        private string enemyID;

        private RandomTimer dyingTimer;
        public override int Energy { get => base.Energy; set { base.Energy = value; energyBar.ScaleNoZoom((float)value / (float)base.MaxEnergy);} }

        public Enemy(string enemyID) : base("SorcererIdle D", 1,1)
        {
            this.enemyID = enemyID;

            energyBar.Position = new Vector2(Position.X - 0.5f, Position.Y - 0.1f);
            energyBar.SetDrawLayer(DrawLayer.Foreground);

            RigidBody.Type = RigidBodyType.Enemy;
            bulletType = BulletType.EnemyBullet;

            // Pathfinding
            Agent = new Agent(this);

            // FSM Set
            VisionRadius = 8.0f;
            walkSpeed = 1.5f;
            followSpeed = walkSpeed * 2.0f;
            shootDistance = 6.0f;

            fsm = new StateMachine();
            fsm.AddState(StateEnum.WALK, new WalkState(this));
            fsm.AddState(StateEnum.FOLLOW, new FollowState(this));
            fsm.AddState(StateEnum.SHOOT, new ShootState(this));
            fsm.AddState(StateEnum.RECHARGE, new RechargeState(this));
            fsm.GoTo(StateEnum.WALK);

            Reset();
            IsActive = true;

            dyingTimer = new RandomTimer(1, 4);
            damageTimer = new RandomTimer(0.1f, 0.3f);
            dyingTimer.Reset();
        }

        protected override void LoadAnimations()
        {
            animationActorName = "Sorcerer";

            animations = new Dictionary<ActorAnimations, Animation>();

            Animation walk = new Animation(this, 4, 16, 16, 6, true);
            Animation attack = new Animation(this, 3, 16, 16, 12, true);
            Animation hurt = new Animation(this, 1, 16, 16, 6, false);
            Animation idle = new Animation(this, 1, 16, 16, 6, true);
            Animation death = new Animation(this, 1, 16, 16, 3, false);

            animations.Add(ActorAnimations.Walk, walk);
            animations.Add(ActorAnimations.Attack, attack);
            animations.Add(ActorAnimations.Hurt, hurt);
            animations.Add(ActorAnimations.Idle, idle);
            animations.Add(ActorAnimations.Death, death);

            components.Add(ComponentType.Animation, walk);

            PlayAnimation(ActorAnimations.Idle);
            AnimationsUpdate();
        }
        public void HeadToPoint()
        {
            Vector2 distVect = pointToReach - Position;

            if (distVect.LengthSquared <= 0.01f)
            {
                Agent.Target = null;
            }

            // Pathfinding
            if (Agent.Target == null)
            {
                Node randomNode = ((PlayScene)Game.CurrentScene).PathFindingMap.GetRandomNode();
                List<Node> path = ((PlayScene)Game.CurrentScene).PathFindingMap.GetPath((int)Position.X, (int)Position.Y, (int)randomNode.X, (int)randomNode.Y);
                Agent.SetPath(path);
            }

            Agent.Update(walkSpeed);
        }
        public bool CanAttackPlayer()
        {
            if (Rival == null || !Rival.IsAlive)
            {
                return false;
            }

            Vector2 distVect = Rival.Position - Position;

            return distVect.LengthSquared < shootDistance * shootDistance;
        }
        public void HeadToPlayer()
        {
            // Pathfinding
            if (Agent.Target == null)
            {
                List<Node> path = ((PlayScene)Game.CurrentScene).PathFindingMap.GetPath((int)Position.X, (int)Position.Y, (int)Rival.Position.X, (int)Rival.Position.Y);
                Agent.SetPath(path);
            }

            Agent.Update(followSpeed);
        }
        public Player GetVisiblePlayer()
        {
            Player player = PlayScene.Player;

                if (!player.IsAlive)
                {
                    return null;
                }

                foreach (Node n in Agent.SightCone)
                {
                    Node playerNode = ((PlayScene)Game.CurrentScene).PathFindingMap.GetNode((int)(player.Position.X + 0.5f), (int)(player.Position.Y + 0.5f));

                    if (playerNode == n)
                    {
                        return player;
                    }
                }

            return null;
        }
        public virtual PowerUp GetNearestPowerUp()
        {
            PowerUp nearest = null;
            float minDistance = float.MaxValue;

            for (int i = 0; i < PowerUpsMngr.PowerUps.Count; i++)
            {
                Vector2 distanceVector;

                if (IsPointVisible(PowerUpsMngr.PowerUps[i].Position, out distanceVector))
                {
                    if (distanceVector.LengthSquared < minDistance)
                    {
                        nearest = PowerUpsMngr.PowerUps[i];
                        minDistance = distanceVector.LengthSquared;
                    }
                }
            }

            return nearest;
        }
        public bool IsPointVisible(Vector2 point, out Vector2 distanceVector)
        {
            distanceVector = point - Position;

            if (distanceVector.LengthSquared <= VisionRadius * VisionRadius)
            {
                float pointAngle = (float)Math.Acos(MathHelper.Clamp(Vector2.Dot(LookingDirection, distanceVector.Normalized()), -1.0f, 1.0f));

                if (pointAngle <= halfConeAngle)
                {
                    return true;
                }
            }

            return false;
        }
        public void LookAtPlayer()
        {
            if (Rival != null)
            {
                Vector2 direction = Rival.Position - Position;
                direction.Normalize();
                LookingDirection = direction;
            }
        }
        public override void Update()
        {
            if (IsActive)
            {
                if (IsAlive)
                {
                    if (haveBeenHitted)
                    {
                        damageTimer.Tick();
                        PlayAnimation(ActorAnimations.Hurt);

                        if (damageTimer.IsOver())
                        {
                            haveBeenHitted = false;
                            sprite.SetAdditiveTint(0, 0, 0, 0);
                            PlayAnimation(ActorAnimations.Walk);
                        }
                    }
                    else
                    {
                        fsm.Update();
                    }

                    base.Update();
                    AnimationsUpdate();
                    energyBar.Position = new Vector2(Position.X - 1f, Position.Y - 0.1f);
                }
                else
                {
                    dyingTimer.Tick();
                    AnimationsUpdate();

                    if (dyingTimer.IsOver())
                    {
                        Destroy();
                    }
                    return;
                }

            }
        }
        public override void OnDie(bool removeFromSave=false)
        {
            energyBar.IsActive = false;

            if (removeFromSave)
            {
                //the enemy is dead by the player not by changing scene
                SaveGameManager.SaveGameDatas[((PlayScene)Game.CurrentScene).MapFileName][enemyID] = "True";
                PlayAnimation(ActorAnimations.Death);
                ItemTextMngr.EnemiesKilledCount++;

                string EnemiesKilledDoubleDigit = ItemTextMngr.EnemiesKilledCount < 10 ? 0 + "" + ItemTextMngr.EnemiesKilledCount.ToString() : ItemTextMngr.EnemiesKilledCount.ToString();

                SaveGameManager.SaveGameDatas["PlayerData"]["EnemiesKilled"] = EnemiesKilledDoubleDigit;
                ItemTextMngr.EnemiesKilledText.SetText("Enemies Killed:" + EnemiesKilledDoubleDigit);
            }
            else IsActive = false;
        }
    }
}
