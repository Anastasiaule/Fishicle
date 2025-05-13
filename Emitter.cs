using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishicle
{
    public class Emitter
    {
        public List<Particle> particles = new List<Particle>(); // еда
        public List<List<FishParticle>> enemies = new List<List<FishParticle>>(); // враги
        public PointF EmitPosition;
        private Random rand = new Random();

        public Emitter(PointF pos)
        {
            EmitPosition = pos;
        }

        public void Update()
        {
            // Обновляем еду
            foreach (var p in particles.ToArray())
            {
                p.Update();
                if (p.Life <= 0)
                    particles.Remove(p);
            }

            // Обновляем врагов
            foreach (var enemy in enemies.ToList())
            {
                foreach (var part in enemy)
                    part.Update();

                // Удаление, если все частицы врага вышли за экран
                bool outOfScreen = enemy.All(p =>
                    p.Position.X < -50 || p.Position.X > 850 ||
                    p.Position.Y < -50 || p.Position.Y > 650);

                if (outOfScreen)
                    enemies.Remove(enemy);
            }

            // Регенерация еды
            while (particles.Count < 100)
            {
                particles.Add(new FoodParticle(new PointF(rand.Next(800), rand.Next(600))));
            }

            // Регенерация врагов
            while (enemies.Count < 10)
            {
                float y = rand.Next(100, 500);
                float size = rand.Next(30, 80);
                bool fromLeft = rand.Next(2) == 0;
                float vx = fromLeft ? rand.Next(1, 3) : -rand.Next(1, 3);
                float vy = rand.Next(-1, 2);

                List<FishParticle> enemyFish = new List<FishParticle>();
                for (int i = 0; i < 10; i++)
                {
                    var offset = new PointF(i * 5, (float)Math.Sin(i * 0.5f) * 5);
                    var posX = fromLeft ? -size + offset.X : 800 + size - offset.X;
                    var pos = new PointF(posX, y + offset.Y);

                    enemyFish.Add(new FishParticle(
                        pos,
                        new PointF(vx, vy),
                        Color.Red,
                        size / 10f
                    ));
                }

                enemies.Add(enemyFish);
            }
        }

        public void Draw(Graphics g)
        {
            foreach (var p in particles)
                p.Draw(g);

            foreach (var enemy in enemies)
                foreach (var part in enemy)
                    part.Draw(g);
        }
    }

}