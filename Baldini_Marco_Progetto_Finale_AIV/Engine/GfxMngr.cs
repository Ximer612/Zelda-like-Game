using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using Aiv.Audio;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    enum SFXType { Explostion_1, LAST }
    static class AssetsMngr
    {
        private static Dictionary<string, Texture> textures;
        private static Dictionary<string, AudioClip> clips;
        private static List<GameObject>[] SFXs;
        static AssetsMngr()
        {
            textures = new Dictionary<string, Texture>();
            clips = new Dictionary<string, AudioClip>();

            //InitSFX();
        }

        public static void InitSFX()
        {
            SFXs = new List<GameObject>[(int)SFXType.LAST];
            //SFXs[(int)SFXType.Explostion_1] = new List<GameObject>();
            //SFXs[(int)SFXType.Explostion_1].Add(new Explosion_1());

        }

        public static Texture AddTexture(string name, string path)
        {
            Texture t = new Texture(path,true);

            if (t != null)
            {
                textures[name] = t;
            }

            return t;
        }

        public static AudioClip AddClip(string name, string path)
        {
            AudioClip c = new AudioClip(path);

            if (c != null)
            {
                clips[name] = c;
            }

            return c;
        }

        public static AudioClip GetClip(string name)
        {
            AudioClip c = null;

            if (clips.ContainsKey(name))
            {
                c = clips[name];
            }

            return c;
        }
        public static Texture GetTexture(string name)
        {
            Texture t = null;

            if (textures.ContainsKey(name))
            {
                t = textures[name];
            }

            return t;
        }

        public static GameObject GetSFX(SFXType type)
        {
            GameObject fx = null;

            int listIndex = (int)type;

            for (int i = 0; i < SFXs[listIndex].Count; i++)
            {
                if (!SFXs[listIndex][i].IsActive)
                    return SFXs[listIndex][i];
            }

            return fx;
        }

        public static void ClearAll()
        {
            textures.Clear();
            clips.Clear();
        }
    }
}
