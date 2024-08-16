using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    struct CameraLimits
    {
        public float MaxX;
        public float MaxY;
        public float MinX;
        public float MinY;

        public CameraLimits(float maxX, float minX, float maxY, float minY)
        {
            MaxX = maxX;
            MinX = minX;
            MaxY = maxY;
            MinY = minY;
        }
    }

    static class CameraMngr
    {
        static Dictionary<string, Tuple<Camera, float>> cameras;
        public static Camera MainCamera;
        public static GameObject Target;
        public static float CameraSpeed = 5;
        public static CameraLimits CameraLimits;
        public static float HalfDiagonalSquared { get => MainCamera.pivot.LengthSquared; }

        public static List<TextObject> TextsObjects;

        public static int ZoomMaxValue = 24;
        public static int ZoomMinValue = 8;

        public static int ActualScroolValue;

        public static void Init(GameObject target, CameraLimits limits)
        {
            MainCamera = new Camera();
            MainCamera.position = new Vector2(Game.Window.OrthoWidth * 0.5f, Game.Window.OrthoHeight * 0.5f);
            MainCamera.pivot = new Vector2(Game.Window.OrthoWidth * 0.5f, Game.Window.OrthoHeight * 0.5f);
            Target = target;
            CameraLimits = limits;

            cameras = new Dictionary<string, Tuple<Camera, float>>();

            TextsObjects = new List<TextObject>();
        }

        public static void Update()
        {
            Vector2 oldCameraPos = MainCamera.position;
            Vector2 targetPosition = new Vector2(Target.Position.X + 0.5f, Target.Position.Y + 0.5f);
            MainCamera.position = Vector2.Lerp(MainCamera.position, targetPosition, Game.Window.DeltaTime * CameraSpeed);
            FixPosition();

            Vector2 cameraDelta = MainCamera.position - oldCameraPos;

            if(cameraDelta != Vector2.Zero)
            {
                foreach (var item in cameras)
                {
                    item.Value.Item1.position += cameraDelta * item.Value.Item2;
                }
            }
        }

        public static void Zoom(int value)
        {
            Game.Window.SetDefaultViewportOrthographicSize(value);
            MainCamera.pivot = new Vector2(Game.Window.OrthoWidth * 0.5f, Game.Window.OrthoHeight * 0.5f);
            DrawMngr.RecreateRenderTexture();

            float valueOverOrthoSize = value / (float)Game.OriginalOrthograpicSize;

            for (int i = 0; i < TextsObjects.Count; i++)
            {
                TextsObjects[i].SetScaleByZoom(valueOverOrthoSize);
            }

            value = Math.Max(Game.OriginalOrthograpicSize, value);

            CameraLimits.MinX = value * 0.5f;
            CameraLimits.MinY = value * 0.5f;
            CameraLimits.MaxX = value * 1.5f;
            CameraLimits.MaxY = value * 1.5f;
        }

        public static void AddCamera(string cameraName, Camera camera=null, float cameraSpeed=0)
        {
            if (camera == null)
            {
                camera = new Camera(MainCamera.position.X, MainCamera.position.Y);
                camera.pivot = MainCamera.pivot;
            }

            cameras[cameraName] = new Tuple<Camera, float>(camera, cameraSpeed);
        }

        public static Camera GetCamera(string cameraName)
        {
            if (cameras.ContainsKey(cameraName))
            {
                //return camera associated with the given cameraName
                return cameras[cameraName].Item1;
            }
            return null;
        }

        private static void FixPosition()
        {
            MainCamera.position.X = MathHelper.Clamp(MainCamera.position.X, CameraLimits.MinX, CameraLimits.MaxX);
            MainCamera.position.Y = MathHelper.Clamp(MainCamera.position.Y, CameraLimits.MinY, CameraLimits.MaxY);
        }

        public static void ClearAll()
        {
            for (int i = 0; i < TextsObjects.Count; i++)
            {
                TextsObjects[i].Clear();
            }
        }
    }
}
