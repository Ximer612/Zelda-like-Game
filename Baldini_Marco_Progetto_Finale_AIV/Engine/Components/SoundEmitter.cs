using Aiv.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    class SoundEmitter : Component
    {
        protected AudioSource source;
        public AudioClip Clip { get; protected set; }
        public float Pitch { get => source.Pitch; set => source.Pitch = value; }
        public float Volume { get => source.Volume; set => source.Volume = value; }

        public SoundEmitter(GameObject owner,string clipName) : base(owner)
        {
            source = new AudioSource();
            Clip = AssetsMngr.GetClip(clipName);
        }

        public void Play(float volume=1f, float pitch=1f,AudioClip clipToPlay=null, bool loop=false)
        {
            if (clipToPlay != null) Clip = clipToPlay;

            Volume = volume;
            Pitch = pitch;
            source.Play(Clip,loop);
        }

        public void Stop()
        {
            source.Stop();
        }

        protected void RandomizePitch()
        {
            source.Pitch = RandomGenerator.GetRandomFloat() * 0.4f + 0.8f;
        }
    }
}
