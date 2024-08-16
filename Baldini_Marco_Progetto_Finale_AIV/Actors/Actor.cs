using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    enum ActorAnimations { Attack, Hurt, Idle, Walk, Death, JumpRoll, Push, LAST }
    abstract class Actor : GameObject
    {
        protected BulletType bulletType;
        protected float maxSpeed;

        //HEALTH
        protected int energy;
        public int MaxEnergy { get; protected set; }
        public bool IsAlive { get { return energy > 0; } }
        public virtual int Energy { get => energy; set { energy = MathHelper.Clamp(value, 0, MaxEnergy); } }
        protected ProgressBar energyBar;

        //PATHFINDING
        public Agent Agent { get; set; }
        public Vector2 LookingDirection { get; set; }

        //ANIMATIONS
        protected Dictionary<ActorAnimations, Animation> animations;
        public ActorAnimations CurrentAnimation { get; protected set; }
        protected string currentAnimationName;
        protected string animationActorName;

        //DAMAGE WHITE COLOR
        protected RandomTimer damageTimer;
        protected bool haveBeenHitted;

        //SOUNDS
        protected SoundEmitter soundEmitter;

        public Actor(string texturePath,float w = 0, float h = 0): base(texturePath,w:w,h:h)
        {
            // Set RB
            RigidBody = new RigidBody(this);
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this, 0.8f, 0.8f);
            RigidBody.Collider.Offset = new Vector2(0.3f, 0.3f);

            float unitDist = Game.PixelsToUnits(4);
            energyBar = new ProgressBar("barFrame", "blueBar", new Vector2(unitDist));
            energyBar.IsActive = true;

            LookingDirection = new Vector2(1, 0);
            MaxEnergy = 100;

            LoadAnimations();

            soundEmitter = new SoundEmitter(this, "");
            components.Add(ComponentType.SoundEmitter, soundEmitter);
        }

        abstract protected void LoadAnimations();

        public virtual void PlayAnimation(ActorAnimations animation)
        {
            animations[animation].Restart();
            CurrentAnimation = animation;
            currentAnimationName = CurrentAnimation.ToString();
            animations[animation].Play();
        }

        protected virtual void AnimationsUpdate()
        {
            if(CurrentAnimation == ActorAnimations.Death)
            {
                texture = AssetsMngr.GetTexture(animationActorName + currentAnimationName);
                sprite.FlipX = false;
                return;
            }

            if (LookingDirection.X == 1)
            {
                texture = AssetsMngr.GetTexture(animationActorName + currentAnimationName + " R");
                sprite.FlipX = false;
            }
            else if (LookingDirection.X == -1)
            {
                texture = AssetsMngr.GetTexture(animationActorName + currentAnimationName + " R");
                sprite.FlipX = true;
            }
            else if (LookingDirection.Y == -1)
            {
                texture = AssetsMngr.GetTexture(animationActorName + currentAnimationName + " U");
                sprite.FlipX = false;
            }
            else if (LookingDirection.Y == 1)
            {
                texture = AssetsMngr.GetTexture(animationActorName + currentAnimationName + " D");
                sprite.FlipX = false;
            }
            else
            {
                PlayAnimation(ActorAnimations.Idle);
            }

            if (!animations[CurrentAnimation].IsPlaying)
            {
                PlayAnimation(ActorAnimations.Idle);
            }
        }

        public virtual bool Shoot(Vector2 direction)
        {
            if (IsActive)
            {
                Bullet b = BulletMngr.GetBullet(bulletType);
                if (b != null)
                {
                    b.IsActive = true;

                    Vector2 shootPos = new Vector2(X + LookingDirection.X * 0.05f + 0.5f, Y + LookingDirection.Y * 0.05f + 0.5f);

                    b.Shoot(shootPos, direction);

                    int randomIndex = RandomGenerator.GetRandomInt(1, 4);
                    soundEmitter.Play(0.5f, RandomGenerator.GetRandomFloat() + 1, AssetsMngr.GetClip("Attack0"+randomIndex));

                    return true;
                }
            }
            return false;
        }

        public virtual void AddDamage(int dmg)
        {
            Energy -= dmg;
            int randomIndex = RandomGenerator.GetRandomInt(1, 3);
            soundEmitter.Play(0.5f, RandomGenerator.GetRandomFloat() + 1, AssetsMngr.GetClip("Hurt0" + randomIndex));

            if (Energy <= 0)
            {
                randomIndex = RandomGenerator.GetRandomInt(1, 3);
                soundEmitter.Play(0.5f, RandomGenerator.GetRandomFloat() + 1, AssetsMngr.GetClip("Death0" + randomIndex));
                OnDie(true);
            }

            damageTimer.Reset();
            sprite.SetAdditiveTint(200, 200, 200, 0);
            haveBeenHitted = true;
        }

        public virtual void AddEnergy(int nrg)
        {
            Energy += nrg;
        }

        public abstract void OnDie(bool removeFromSave);

        public virtual void Reset()
        {
            Energy = MaxEnergy;
        }

        
        public override void Update()
        {
            if(IsActive && RigidBody.Velocity != Vector2.Zero && IsAlive)
            {
                Forward = RigidBody.Velocity;
            }
        }

        public override void Draw()
        {
            if (IsActive)
            {
                sprite.DrawTexture(texture, (int)animations[CurrentAnimation].Offset.X, (int)animations[CurrentAnimation].Offset.Y, animations[CurrentAnimation].FrameWidth, animations[CurrentAnimation].FrameHeight);
            }
        }
    }
}
