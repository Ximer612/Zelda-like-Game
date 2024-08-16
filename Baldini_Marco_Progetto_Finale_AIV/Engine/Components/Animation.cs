using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    class Animation : Component, IUpdatable
    {
        protected int numFrames;
        protected float frameDuration;
        protected bool isPlaying;
        protected int currentFrame;
        protected float elapsedTime;

        public bool IsPlaying { get => isPlaying; }
        public int FrameWidth { get; protected set; }
        public int FrameHeight { get; protected set; }
        public bool Loop;
        public Vector2 Offset { get; protected set; }
        public Animation(GameObject owner, int numFrames, int frameWidth, int frameHeight, float framesPerSecond, bool loop=false, bool isEnabled=true) : base(owner)
        {
            this.numFrames = numFrames;
            this.FrameWidth = frameWidth;
            this.FrameHeight = frameHeight;

            Loop = loop;

            frameDuration = 1 / framesPerSecond;
            Offset = Vector2.Zero;

            IsEnabled = isEnabled;

            UpdateMngr.AddItem(this);
        }

        public virtual void Play()
        {
            isPlaying = true;
        }

        public virtual void Stop()
        {
            isPlaying = false;
            currentFrame = 0;
            elapsedTime = 0;
        }

        public virtual void Pause()
        {
            isPlaying = false;
        }

        public virtual void OnAnimationEnd()
        {
            isPlaying = false;
        }

        public virtual void Restart()
        {
            currentFrame = 0;
            elapsedTime = 0;
            isPlaying = true;
            Offset = Vector2.Zero;
        }

        public void Update()
        {
            if(IsEnabled &&  isPlaying)
            {
                elapsedTime += Game.DeltaTime;

                if(elapsedTime > frameDuration)
                {
                    //next frame
                    currentFrame++;
                    elapsedTime = 0;


                    if(currentFrame >= numFrames)
                    {
                        if (Loop) currentFrame = 0;
                        else
                        {
                            OnAnimationEnd();
                            return;
                        }
                    }

                    Offset = new Vector2(FrameWidth * currentFrame,0);
                }
            }

        }
    }
}
