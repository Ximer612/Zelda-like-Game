using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    static class BulletMngr
    {
        private static Queue<Bullet>[] bullets;
        public static void Init(int queueSize=24)
        {
            bullets = new Queue<Bullet>[(int)BulletType.LAST];

            Type[] bulletTypes = new Type[bullets.Length];
            bulletTypes[0] = typeof(PlayerBullet);
            bulletTypes[1] = typeof(EnemyBullet);

            for (int i = 0; i < bullets.Length; i++)
            {
                bullets[i] = new Queue<Bullet>(queueSize);

                for (int j = 0; j < queueSize; j++)
                {
                    Bullet b = (Bullet)Activator.CreateInstance(bulletTypes[i]);
                    bullets[i].Enqueue(b);
                }
            }
        }

        public static Bullet GetBullet(BulletType type)
        {
            int index = (int)type;

            if (bullets[index].Count > 0)
            {
                Bullet bullet = bullets[index].Dequeue();
                bullet.IsActive = true;
                return bullet;
            }

            return null;
        }

        public static void RestoreBullet(Bullet bullet)
        {
            bullet.Reset();
            bullets[(int)bullet.Type].Enqueue(bullet);
        }

        public static void ClearAll()
        {
            for (int i = 0; i < bullets.Length; i++)
            {
                bullets[i].Clear();
            }
        }
    }
}
