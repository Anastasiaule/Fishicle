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
        public List<FoodParticle> particles = new List<FoodParticle>();
        public List<Fish> enemies = new List<Fish>();
        public PointF EmitPosition;
        private Random rand = new Random();

        public Emitter(PointF pos)
        {
            EmitPosition = pos;
        }

        public void Update()
        {
            foreach (var p in particles)
                p.Update();

            particles.RemoveAll(p => p.Position.X < -10 || p.Position.X > 850 || p.Position.Y < -10 || p.Position.Y > 650);

            foreach (var enemy in enemies)
                enemy.Update();

            enemies.RemoveAll(e => e.Position.X < -100 || e.Position.X > 900 || e.Position.Y < -100 || e.Position.Y > 700);

            while (particles.Count < 80)
            {
                float angle = (float)(rand.NextDouble() * 2 * Math.PI);
                float speed = 0.3f + (float)rand.NextDouble() * 1.2f;
                var velocity = new PointF((float)Math.Cos(angle) * speed, (float)Math.Sin(angle) * speed);
                var size = rand.Next(6, 12);
                var color = Color.FromArgb(
                    rand.Next(150, 256),
                    rand.Next(100, 200),
                    rand.Next(200, 256),
                    rand.Next(100, 200)
                );

                particles.Add(new FoodParticle(
                    new PointF(rand.Next(800), rand.Next(600)),
                    velocity,
                    color,
                    size
                ));
            }

            while (enemies.Count < 5)
            {
                float y = rand.Next(100, 500);
                float size = rand.Next(30, 150);
                bool fromLeft = rand.Next(2) == 0;
                float vx = fromLeft ? rand.Next(1, 3) : -rand.Next(1, 3);
                var enemy = new Fish(
                    new PointF(fromLeft ? -50 : 850, y),
                    new PointF(vx, rand.Next(-1, 2)),
                    Color.Red,
                    size
                );
                enemies.Add(enemy);
            }
        }

        public void Draw(Graphics g)
        {
            foreach (var p in particles)
                p.Draw(g);
            foreach (var e in enemies)
                e.Draw(g);
        }
    }

}