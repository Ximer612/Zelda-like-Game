using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    class Player : Actor
    {
        //References
        protected Controller controller;
        protected TextObject playerName;

        //Variables
        private Vector2 pointToReach;
        private bool isMousePressed;
        private float mouseWheelValue;
        private bool isFirePressed;

        //Propreties
        public Dictionary<ItemType,int> Inventory { get; protected set; }
        public Dictionary<ItemType, GUIitem> GUIitems { get; protected set; }
        public override int Energy { get => base.Energy; set { base.Energy = value; energyBar.Scale((float)value / (float)MaxEnergy); Zoom(); } }

        public Player(Controller ctrl): base("",1,1)
        {
            maxSpeed = 10;

            IsActive = true;
            
            bulletType = BulletType.PlayerBullet;
            controller = ctrl;

            RigidBody.Type = RigidBodyType.Player;
            RigidBody.AddCollisionType(RigidBodyType.Enemy | RigidBodyType.EnemyBullet | RigidBodyType.Spike);

            energyBar.Position = new Vector2(8, 1.15f);
            energyBar.SetOriginalPosition();

            Inventory = new Dictionary<ItemType, int>((int)ItemType.LAST);
            GUIitems = new Dictionary<ItemType, GUIitem>();

            for (int i = 0; i < (int)ItemType.LAST; i++)
            {
                Inventory[(ItemType)i] = 0;
            }

            X = int.Parse(SaveGameManager.SaveGameDatas["PlayerData"]["PlayerX"]);
            Y = int.Parse(SaveGameManager.SaveGameDatas["PlayerData"]["PlayerY"]);

            //generate GUI elements
            Vector2 guiElementPosition = Vector2.Zero;
            int firstItemToSave = ((int)ItemType.Coin);
            int lastItemToSave = ((int)ItemType.GreenKey);

            for (int i = firstItemToSave; i <= lastItemToSave; i++)
            {
                Inventory[(ItemType)i] = int.Parse(SaveGameManager.SaveGameDatas["PlayerData"][((ItemType)i).ToString()]);


                if ((ItemType)i == ItemType.Coin)
                {
                    GUIitems[(ItemType)i] = new GUIitem((ItemType)i, guiElementPosition, "coin", this);
                }
                else
                {
                    GUIitems[(ItemType)i] = new GUIitem((ItemType)i, guiElementPosition, "key", this);

                    string colorName = ((ItemType)i).ToString();
                    int index = colorName.IndexOf("Key");
                    colorName = colorName.Substring(0, index);
                    ColorType colorType;
                    Enum.TryParse(colorName, out colorType);

                    GUIitems[(ItemType)i].SetMultiplyColor(ColorsFactory.GetColor(colorType));
                }

                guiElementPosition.Y+=0.5f;

                if(i%3 == 0)
                {
                    guiElementPosition.X+=1.5f;
                    guiElementPosition.Y = 0f;
                }
                GUIitems[(ItemType)i].UpdateAmount();

            }

            //loads player name
            string nameOfPlayer = SaveGameManager.SaveGameDatas["PlayerData"]["Name"];

            playerName = new TextObject(new Vector2(energyBar.X-nameOfPlayer.Length * 0.27f, energyBar.Y));
            playerName.SetText(nameOfPlayer);

            damageTimer = new RandomTimer(1.4f, 1.6f);

            // Pathfinding
            Agent = new Agent(this);

            Reset();

            Game.OnSceneChange += () => Agent.ResetPath();
        }

        protected override void LoadAnimations()
        {
            animationActorName = "player";

            animations = new Dictionary<ActorAnimations, Animation>();

            Animation walk = new Animation(this, 3, 16, 16, 6,true);
            Animation attack = new Animation(this, 2, 16, 16, 6, false);
            Animation hurt = new Animation(this, 1, 16, 16, 6, false);
            Animation idle = new Animation(this, 1, 16, 16, 6, true);
            Animation jumpRoll = new Animation(this, 3, 16, 16, 6, true);
            Animation push = new Animation(this, 3, 16, 16, 6, true);
            Animation death = new Animation(this, 3, 16, 16, 6, false);

            animations.Add(ActorAnimations.Walk, walk);
            animations.Add(ActorAnimations.Attack, attack);
            animations.Add(ActorAnimations.Hurt, hurt);
            animations.Add(ActorAnimations.Idle, idle);
            animations.Add(ActorAnimations.JumpRoll, jumpRoll);
            animations.Add(ActorAnimations.Push, push);
            animations.Add(ActorAnimations.Death, death);

            components.Add(ComponentType.Animation, walk);

            PlayAnimation(ActorAnimations.Idle);
            AnimationsUpdate();
        }

        public void Input()
        {
            //movement
                if((controller.IsMoveButtonPressed() || Game.Window.MouseLeft))
                {
                    if(!isMousePressed)
                    {
                        pointToReach = CameraMngr.MainCamera.position - CameraMngr.MainCamera.pivot + Game.Window.MousePosition;
                        SetPointToHead();
                        isMousePressed = true;
                    }
                }
                else
                {
                    isMousePressed = false;
                }

                //shoot
            if (!haveBeenHitted)
            {
                if (Game.Window.MouseRight || controller.IsInteractButtonPressed())
                {
                    if (!isFirePressed && !animations[ActorAnimations.Attack].IsPlaying)
                    {
                        isFirePressed = true;

                        Vector2 mousePos = CameraMngr.MainCamera.position - CameraMngr.MainCamera.pivot + Game.Window.MousePosition;

                        Vector2 mousePosToPlayer = (mousePos - Position - new Vector2(0.5f)).Normalized();

                        bool haveShooted = Shoot(mousePosToPlayer);

                        LookingDirection = new Vector2((int)Math.Round(mousePosToPlayer.X, 0), (int)Math.Round(mousePosToPlayer.Y, 0));

                        if(haveShooted) PlayAnimation(ActorAnimations.Attack);
                    }
                }
                else if (isFirePressed)
                {
                    isFirePressed = false;
                }

                //dance
                if (Game.Window.GetKey(KeyCode.D))
                {
                    PlayAnimation(ActorAnimations.Push);
                }
            }

            //Zoom in and out by mouse
            if (mouseWheelValue != Game.Window.MouseWheel)
            {
                Zoom();
            }
        }

        private void Zoom()
        {
            int scrollValue = Math.Sign(mouseWheelValue - Game.Window.MouseWheel);
            scrollValue += (int)Game.Window.CurrentViewportOrthographicSize;
            scrollValue = MathHelper.Clamp(scrollValue, CameraMngr.ZoomMinValue, CameraMngr.ZoomMaxValue);

            CameraMngr.ActualScroolValue = scrollValue;
            mouseWheelValue = Game.Window.MouseWheel;
            CameraMngr.Zoom(CameraMngr.ActualScroolValue);

            Vector2 guiElementPosition = Vector2.Zero;
            int firstGUIItem = ((int)ItemType.Coin);
            int lastGUIItem = ((int)ItemType.GreenKey);

            float scroolValueOverOriginalOrthograpicSize = CameraMngr.ActualScroolValue / (float)Game.OriginalOrthograpicSize;

            energyBar.SetScale(scroolValueOverOriginalOrthograpicSize);
            energyBar.Scale(((float)energy / (float)MaxEnergy));

            for (int i = firstGUIItem; i <= lastGUIItem; i++)
            {
                GUIitems[(ItemType)i].SetScale(scroolValueOverOriginalOrthograpicSize);

                GUIitems[(ItemType)i].Position = guiElementPosition;

                guiElementPosition.Y += 0.5f * scroolValueOverOriginalOrthograpicSize;

                if (i % 3 == 0)
                {
                    guiElementPosition.X += 1.5f * scroolValueOverOriginalOrthograpicSize;
                    guiElementPosition.Y = 0f;
                }

            }

            GC.Collect();
        }
        public override void Update()
        {
            if(IsAlive)
            {
                if(haveBeenHitted)
                {
                    damageTimer.Tick();

                    if (damageTimer.IsOver())
                    {
                        haveBeenHitted = false;
                        RigidBody.AddCollisionType(RigidBodyType.Enemy);
                        RigidBody.AddCollisionType(RigidBodyType.Spike);
                        sprite.SetAdditiveTint(0, 0, 0, 0);
                        PlayAnimation(ActorAnimations.Idle);
                    }
                }

                base.Update();
                if (Agent.Target != null)
                {
                    HeadToPoint();
                }
                AnimationsUpdate();
            }
            else
            {
                PlayAnimation(ActorAnimations.Death);
            }
        }
        
        public override void OnDie(bool resetSave=false)
        {
            IsActive = false;
            ((PlayScene)Game.CurrentScene).OnPlayerDie();
        }
        public void HeadToPoint()
        {
            if(Agent.Update(maxSpeed))
            {
                CurrentAnimation = ActorAnimations.Idle;
            }

        }
        public void SetPointToHead()
        {
            Node pointToReachNode = ((PlayScene)Game.CurrentScene).PathFindingMap.GetNode((int)pointToReach.X, (int)pointToReach.Y);

            if (pointToReachNode == null || pointToReachNode.Cost == int.MaxValue || (pointToReachNode.X == Position.X && pointToReachNode.Y == Position.Y)) return;

            PlayAnimation(ActorAnimations.Walk);

            List<Node> path = ((PlayScene)Game.CurrentScene).PathFindingMap.GetPath((int)Position.X, (int)Position.Y, (int)pointToReachNode.X, (int)pointToReachNode.Y);
            Agent.SetPath(path);
        }

        public override void AddDamage(int dmg)
        {
            RigidBody.RemoveCollisionType(RigidBodyType.Enemy);
            RigidBody.RemoveCollisionType(RigidBodyType.Spike);
            PlayAnimation(ActorAnimations.JumpRoll);
            base.AddDamage(dmg);
        }
        public override void Draw()
        {
            if (IsActive)
            {
                Agent.DrawPath();

                sprite.DrawTexture(texture, (int)animations[CurrentAnimation].Offset.X, (int)animations[CurrentAnimation].Offset.Y, animations[CurrentAnimation].FrameWidth, animations[CurrentAnimation].FrameHeight);
            }
        }
        public void AddItem(ItemType type,int amount = 1)
        {
            Inventory[type]+=amount;

            SaveGameManager.SaveGameDatas["PlayerData"][type.ToString()] = Inventory[type].ToString();
        }
        public override void OnCollide(Collision collisionInfo)
        {
            if(collisionInfo.Collider is Enemy)
            {
                if (!((Enemy)collisionInfo.Collider).IsAlive)
                {
                    return;
                }
                if (!damageTimer.IsOver()) return;
                AddDamage(5);
                Agent.ResetPath();
            }
            else if(collisionInfo.Collider is EnemyBullet)
            {
                if (!damageTimer.IsOver()) return;
                AddDamage(10);
                Agent.ResetPath();
            }
            else if(collisionInfo.Collider is SpikeBlock)
            {
                if(SpikesMngr.CanDealDamage)
                {
                    if (!damageTimer.IsOver()) return;
                    AddDamage(15);
                }
                return;
            }
        }
        public void Clear()
        {
            Inventory.Clear();

            foreach (var guiItem in GUIitems)
            {
                guiItem.Value.Destroy();
                guiItem.Value.IsActive = false;
            }

            GUIitems.Clear();

            energyBar.Destroy();
            playerName.Clear();
        }
    }
}
