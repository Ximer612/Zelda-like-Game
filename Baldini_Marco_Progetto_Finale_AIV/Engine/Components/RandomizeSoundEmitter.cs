using Aiv.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    class RandomizeSoundEmitter : Component
    {
        protected AudioSource source;
        protected List<AudioClip> clips;
        public RandomizeSoundEmitter(GameObject owner) : base(owner)
        {
            source = new AudioSource();
            clips = new List<AudioClip>();
        }

        public void AddClip(string clipName)
        {
            clips.Add(AssetsMngr.GetClip(clipName));
        }

        public void Play()
        {
            RandomizePitch();
            source.Play(GetRandomClip());
        }

        public void Play(float volume)
        {
            source.Volume = volume;
            Play();
        }

        public void Play(AudioClip clipName, float volume=1)
        {
            source.Volume = volume;
            RandomizePitch();
            source.Play(clipName);
        }

        protected void RandomizePitch()
        {
            source.Pitch = RandomGenerator.GetRandomFloat() * 0.4f + 0.8f;
        }

        protected AudioClip GetRandomClip()
        {
            return clips[RandomGenerator.GetRandomInt(0, clips.Count)];
        }
    }
}
