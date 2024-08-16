using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    class EnemyOLD : Actor
    {
       //FSM
       public float VisionRay { get; }
       protected float halfConeAngle = MathHelper.DegreesToRadians(40);
       public float AttackRay { get; }
       public float ChaseSpeed { get; }
       public float WalkSpeed { get; protected set; }
       public Player Rival { get; set; }
       //protected StateMachine stateMachine { get; }

       protected Vector2 pointToReach;
       protected ProgressBar nrgBar;
       protected TextObject name;

       public override int Energy { get => base.Energy; set { base.Energy = value; nrgBar.Scale((float)value / (float)MaxEnergy); } }

       //protected float timerToFindPlayer;

       public EnemyOLD(string textureName="enemy_1", int w = 0, int h = 0):base(textureName,w,h)
       {
           //sprite.SetAdditiveTint(0.0f, 0.0f, 1.0f, 0.0f);

           RigidBody = new RigidBody(this);
           RigidBody.Type = RigidBodyType.Enemy;
           RigidBody.Collider = ColliderFactory.CreateBoxFor(this);
           IsActive = true;

           //FSM
           VisionRay = 8.0f;
           AttackRay = 3.0f;
           WalkSpeed = 1.5f;
           ChaseSpeed = WalkSpeed*2;

           /*stateMachine = new StateMachine();
           stateMachine.AddState(StateEnum.WALK, new StateWalk(this));
           stateMachine.AddState(StateEnum.CHASE, new StateChase(this));
           stateMachine.AddState(StateEnum.SHOOT, new StateShoot(this));
           stateMachine.AddState(StateEnum.RECHARGE, new StateRechargeHealth(this));
           stateMachine.GoTo(StateEnum.WALK);*/

           bulletType = BulletType.EnemyBullet;

           nrgBar = new ProgressBar("barFrame", "blueBar", new Vector2(Game.PixelsToUnits(4.0f), Game.PixelsToUnits(4.0f)));
           nrgBar.Position = new Vector2(8, 0.75f);

           Vector2 playerNamePos = new Vector2(nrgBar.X - "Enemy".Length * 0.27f, nrgBar.Y);

           name = new TextObject(playerNamePos, $"Enemy", FontMngr.GetFont(), 0);
           name.IsActive = true;

            // Pathfinding
            Agent = new Agent(this);

            Reset();
       }

       public void ComputerRandomPoint()
       {
           float randX = RandomGenerator.GetRandomFloat() * (Game.Window.OrthoWidth-2)+1;

           float randY = RandomGenerator.GetRandomFloat() * (Game.Window.OrthoHeight-2)+1;

           pointToReach = new Vector2(randX, randY);
       }
       public List<Player> GetVisiblePlayers()
       {
            List<Player> players = new List<Player>();
            players.Add(PlayScene.Player);
           List<Player> visiblePlayers = new List<Player>();

           for (int i = 0; i < players.Count; i++)
           {
               if (!players[i].IsAlive) continue;

               Vector2 dist = players[i].Position - this.Position;
               if (dist.LengthSquared < VisionRay * VisionRay)
               {
                   float angleCos = Vector2.Dot(Forward, dist.Normalized());
                   angleCos = MathHelper.Clamp(angleCos, -1, 1);

                   float playerAngle = (float)Math.Acos(angleCos);
                   if (playerAngle <= halfConeAngle)
                   {
                       visiblePlayers.Add(players[i]);
                   }
               }
           }

               return visiblePlayers;
       }
       public List<EnergyPowerUp> GetVisiblePowerUps()
       {
            /*List<EnergyPowerUp> powerUps = PowerUpsMngr.VisiblePowerUps;

            if (powerUps.Count < 1) return null;

            List<EnergyPowerUp> visiblePowerUps = new List<EnergyPowerUp>();


            if (powerUps.Count < 2) {
                //only 1 powerup
                visiblePowerUps.Add(powerUps[0]); 
                return visiblePowerUps; }

            //powers ups are 2 ore more            

            for (int i = 0; i < powerUps.Count; i++)
            {
                EnergyPowerUp energyPowerUp = powerUps[i];

                Vector2 dist = energyPowerUp.Position - this.Position;
                if (dist.LengthSquared < VisionRay * VisionRay)
                {
                    float angleCos = Vector2.Dot(Forward, dist.Normalized());
                    angleCos = MathHelper.Clamp(angleCos, -1, 1);

                    float playerAngle = (float)Math.Acos(angleCos);
                    if (playerAngle <= halfConeAngle)
                    {
                        visiblePowerUps.Add(energyPowerUp);
                    }
                }
            }

            return visiblePowerUps;*/

            return null;
       }
       public EnergyPowerUp GetBestPowerUp()
       {
           List<EnergyPowerUp> visiblePowerUps = GetVisiblePowerUps();

           if (visiblePowerUps == null) return null;

           EnergyPowerUp bestPowerUp = null;

           if (visiblePowerUps.Count < 1) { return null; }

           if (visiblePowerUps.Count > 1)
           {
               //fuzzy logic to find best power up

               float fuzzyMax = -1;

               for (int i = 0; i < visiblePowerUps.Count; i++)
               {

                   // Distance
                   Vector2 distFromPowerUp = Position - visiblePowerUps[i].Position;
                   float fuzzyDistance = 1 - distFromPowerUp.LengthSquared / (VisionRay * VisionRay);

                   if (fuzzyDistance > fuzzyMax)
                   {
                       fuzzyMax = fuzzyDistance;
                       bestPowerUp = visiblePowerUps[i];
                   }
               }

           }
           else
           {
               //just one player
               bestPowerUp = visiblePowerUps[0];
           }


           return bestPowerUp;
       }
       public bool ShouldTakeRecharge()
       {
           /*if (NearestPowerUp == null)*/ return false;

           if (Rival == null) return true;

           //distanza powerup
           Vector2 distFromPowerUp = Position /*- NearestPowerUp.Position*/;

           //distanza rivale
           Vector2 distFromRival = Position - Rival.Position;

           float fuzzyDistance = distFromPowerUp.LengthSquared - distFromRival.LengthSquared;

           Console.WriteLine("dist 4 power up " + (distFromPowerUp.LengthSquared * 0.1));
           Console.WriteLine("dist 4 rival up " + (distFromRival.LengthSquared * 0.1));

           //1= ricaricarsi 0=nemico

           //Console.WriteLine(fuzzyDistance);

           //energia nemico in relazione a quella del playersk
           float fuzzyHp = (Rival.Energy / (float)Rival.MaxEnergy) - (energy / (float)MaxEnergy);
           //more fuzzy value more wants to heal himself

           return false;
       }
       public Player GetBestPlayerToFight()
       {
           List<Player> visiblePlayers = GetVisiblePlayers();
           Player bestPlayer = null;

           if (visiblePlayers.Count < 1) { return bestPlayer; }

           if (visiblePlayers.Count > 1)
           {
               //fuzzy logic to find best player
               float fuzzyMax = -1.0f;

               for (int i = 0; i < visiblePlayers.Count; i++)
               {

                   // Distance
                   Vector2 distFromPlayer = Position - visiblePlayers[i].Position;
                   float fuzzyDistance = 1 - distFromPlayer.LengthSquared / (VisionRay * VisionRay);


                   // Energy
                   float fuzzyEnergy = 1 - visiblePlayers[i].Energy / visiblePlayers[i].MaxEnergy;

                   // Angle
                   float playerAngle = (float)Math.Acos(MathHelper.Clamp(Vector2.Dot(visiblePlayers[i].Forward,distFromPlayer.Normalized()),-1,1));
                   float fuzzyAngle = 1 - (playerAngle / (float)Math.PI);

                   // Sum
                   float fuzzySum = fuzzyDistance + fuzzyEnergy + fuzzyAngle;

                   if(fuzzySum > fuzzyMax)
                   {
                       fuzzyMax = fuzzySum;
                       bestPlayer = visiblePlayers[i];
                   }

               }

           }
           else
           {
               //just one player
               bestPlayer = visiblePlayers[0];
           }


           return bestPlayer;
       }
       public bool CanAttackPlayer()
       {
           if (Rival == null)
           {
               return false; 
           }

               Vector2 dist = Rival.Position - this.Position;
               return dist.LengthSquared < AttackRay * AttackRay;

       }
       public void LookAtPlayer()
       {
           if (Rival == null)
           {
               return;
           }

           Vector2 dir = Rival.Position - this.Position;
           //Forward = dir;

       }
       public void SetPoint(Vector2 point)
       {
           pointToReach = point;
       }
       public bool HeadToPoint()
       {
           Vector2 dist = pointToReach - Position;
           if (dist.LengthSquared <= 0.01) { ComputerRandomPoint(); return true; }
           RigidBody.Velocity = dist.Normalized() * WalkSpeed;
           return false;
       }
       public void HeadToPlayer()
       {
           if (Rival == null)
           {
               return;
           }

           Vector2 dir = (Rival.Position - this.Position).Normalized();
           RigidBody.Velocity = dir * ChaseSpeed;
       }
       public void EnableChaseColor()
       {
           sprite.SetAdditiveTint(200, 10, 10, 0);
       } 
       public void EnableShootColor()
       {
           sprite.SetAdditiveTint(10, 200, 10, 0);
       }
       public void DisableCustomColor()
       {
           sprite.SetAdditiveTint(0.0f, 0.0f, 1.0f, 0.0f);
       }
       public override void OnDie()
       {
           IsActive = false;
       }
       public override void Update()
       {
           if (IsActive)
           {
               if(RigidBody.Velocity != Vector2.Zero)
               {
                   //Forward = RigidBody.Velocity;
               }
               base.Update();

                HeadToPoint();
                //stateMachine.Update();

               Node actualNode = ((PlayScene)Game.CurrentScene).PathFindingMap.GetNode(((int)X), ((int)Y));
               actualNode.SetCost(int.MaxValue);
           }
       }

        public override void Draw()
        {
            base.Draw();
            Agent.Draw();
        }
    }
}
