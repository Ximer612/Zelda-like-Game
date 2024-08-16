using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    static class PowerUpsMngr
    {
        public static List<PowerUp> PowerUps { get; private set; }
        private static int listSize = 8;
        private static float nextSpawn;
        public static SoundEmitter SoundEmitter { get; private set; }

        public static void Init()
        {
            PowerUps = new List<PowerUp>(listSize);

            for (int i = 0; i < listSize; i++)
            {
                PowerUps.Add(new EnergyPowerUp());
            }

            nextSpawn = RandomGenerator.GetRandomFloat() * 2 + 3;

            Game.OnSceneChange += () => ClearAll();

            SoundEmitter = new SoundEmitter(null, "Pickup01");
        }

        public static void Update()
        {
            nextSpawn -= Game.DeltaTime;

            if (nextSpawn <= 0)
            {
                SpawnPowerUp();
                nextSpawn = RandomGenerator.GetRandomFloat() * 3 + 4;
            }
        }

        public static void SpawnPowerUp()
        {
            for (int i = 0; i < PowerUps.Count; i++)
            {
                if (!PowerUps[i].IsActive)
                {
                    PowerUps[i].Position = GetRandomPoint();
                    PowerUps[i].IsActive = true;
                    break;
                }
            }
        }

        private static Vector2 GetRandomPoint()
        {
            Node rand = null;

            do
            {
                rand = ((PlayScene)Game.CurrentScene).PathFindingMap.GetRandomNode();
            } while (rand.Cost == int.MaxValue || rand.X < 5 || rand.X > CameraMngr.CameraLimits.MaxX || rand.Y < 5 || rand.Y > CameraMngr.CameraLimits.MaxY);

            return new Vector2(rand.X, rand.Y);
        }

        public static void ClearAll()
        {
            for (int i = 0; i < PowerUps.Count; i++)
            {
                PowerUps[i].IsActive = false;
            }
        }
    }
}
