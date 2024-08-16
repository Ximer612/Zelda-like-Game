using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    enum DrawLayer { Background, Middleground, Playground, Foreground, GUI, LAST}
    static class DrawMngr
    {
        private static List<IDrawable>[] items;

        private static RenderTexture sceneTexture;
        private static Sprite sceneSprite;
        private static Dictionary<string, PostProcessingEffect> postFXs;

        public static Vector2 RenderTexturePosition { get => sceneSprite.position; }

        static DrawMngr()
        {
            items = new List<IDrawable>[(int)DrawLayer.LAST];

            for (int i = 0; i < items.Length; i++)
            {
                items[i] = new List<IDrawable>();
            }

            //PostFX
            postFXs = new Dictionary<string, PostProcessingEffect>();

            //RenderTexture
            sceneTexture = new RenderTexture(Game.Window.Width, Game.Window.Height);
            sceneSprite = new Sprite(Game.Window.OrthoWidth, Game.Window.OrthoHeight);
            sceneSprite.Camera = CameraMngr.GetCamera("GUI");
        }

        public static void AddItem(IDrawable item)
        {
            items[(int)item.DrawLayer].Add(item);
        }

        public static void RemoveItem(IDrawable item)
        {
            items[(int)item.DrawLayer].Remove(item);
        }

        public static void AddFX(string fxName, PostProcessingEffect fx)
        {
            postFXs.Add(fxName, fx);
        }

        public static void RemoveFX(string fxName)
        {
            postFXs.Remove(fxName);
        }

        public static void ClearAll()
        {
            for (int i = 0; i < items.Length; i++)
            {
                items[i].Clear();
            }

            postFXs.Clear();
        }

        public static void Draw()
        {
            Game.Window.RenderTo(sceneTexture);

            for (int i = 0; i < items.Length; i++)
            {
                if (i == ((int)DrawLayer.GUI))
                {
                    ApplyPostFX();
                    Game.Window.RenderTo(null);
                    sceneSprite.DrawTexture(sceneTexture);
                }

                for (int j = 0; j < items[i].Count; j++)
                {
                    items[i][j].Draw();
                }
            }
        }

        private static void ApplyPostFX()
        {
            foreach (var item in postFXs)
            {
                sceneTexture.ApplyPostProcessingEffect(item.Value);
            }
        }

        public static void RecreateRenderTexture()
        {
            sceneTexture = new RenderTexture(Game.Window.Width, Game.Window.Height);
            sceneSprite = new Sprite(Game.Window.OrthoWidth, Game.Window.OrthoHeight);
            sceneSprite.Camera = CameraMngr.GetCamera("GUI");
        }
    }
}
