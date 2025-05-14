using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishicle
{
    public class Fish
    {
        public List<FishParticle> Parts = new List<FishParticle>();
        public PointF Position;
        public PointF Velocity;
        public float Size;
        public Color BaseColor;
        private Random rand = new Random();

        public Fish(PointF pos, PointF vel, Color color, float size)
        {
            Position = pos;
            Velocity = vel;
            Size = size;
            BaseColor = color;
            CreateBody();
        }

        public void CreateBody()
        {
            Parts.Clear();
            Random rand = new Random();

            // ====== Основное тело (контейнерный овал) ======
            int bodyParticles = 150;
            float bodyRadiusX = Size * 0.3f;
            float bodyRadiusY = Size * 0.25f;

            for (int i = 0; i < bodyParticles; i++)
            {
                float angle = (float)(rand.NextDouble() * Math.PI * 2);
                float r = (float)Math.Sqrt(rand.NextDouble()); // равномерное распределение
                float offsetX = (float)Math.Cos(angle) * bodyRadiusX * r;
                float offsetY = (float)Math.Sin(angle) * bodyRadiusY * r;

                Parts.Add(new FishParticle(
                    Position,
                    Velocity,
                    Color.FromArgb(120 + rand.Next(120), BaseColor),
                    Size / 10f,
                    new PointF(offsetX, offsetY)
                ));
            }

            for (int i = 0; i < 50; i++)
            {
                // Генерация случайных координат для треугольника
                float x = -Size * 0.4f + (float)(rand.NextDouble() * Size * 0.1f);  // x в более широком диапазоне
                float y = (float)((rand.NextDouble() - 0.5f) * Size * -0.5f);        // y для удлинения треугольника

                // Сужение x в зависимости от y, чтобы кончик был поуже, но с другой стороны
                x *= 1 + Math.Abs(y) / (Size * 0.6f); // Увеличиваем x по мере удаления от центра

                Parts.Add(new FishParticle(
                    Position,
                    Velocity,
                    Color.FromArgb(100 + rand.Next(100), BaseColor),
                    Size / 10f,
                    new PointF(x, y)
                ));
            }





            // ====== Верхний плавник ======
            for (int i = 0; i < 20; i++)
            {
                float x = Size * -0.1f + (float)(rand.NextDouble() * Size * 0.1f);
                float y = Size * 0.3f + (float)(rand.NextDouble() * Size * 0.1f);

                Parts.Add(new FishParticle(
                    Position,
                    Velocity,
                    Color.FromArgb(100 + rand.Next(100), BaseColor),
                    Size / 10f,
                    new PointF(x, y)
                ));
            }

            // ====== Нижний плавник ======
            for (int i = 0; i < 20; i++)
            {
                float x = Size * -0.1f + (float)(rand.NextDouble() * Size * 0.1f);
                float y = -Size * 0.3f - (float)(rand.NextDouble() * Size * 0.1f);

                Parts.Add(new FishParticle(
                    Position,
                    Velocity,
                    Color.FromArgb(100 + rand.Next(100), BaseColor),
                    Size / 10f,
                    new PointF(x, y)
                ));
            }

            // ====== Глаз (фиксированный) ======
            Parts.Add(new FishParticle(
                Position,
                Velocity,
                Color.Black,
                Size / 10f,
                new PointF(Size * 0.2f, -Size * 0.1f)
            ));
        }





        public void Update()
        {
            Velocity = new PointF(
                Velocity.X + (float)(rand.NextDouble() - 0.5) * 0.1f,
                Velocity.Y + (float)(rand.NextDouble() - 0.5) * 0.1f
            );

            float speed = (float)Math.Sqrt(Velocity.X * Velocity.X + Velocity.Y * Velocity.Y);
            if (speed > 3)
            {
                Velocity = new PointF(Velocity.X * 3 / speed, Velocity.Y * 3 / speed);
            }

            Position = new PointF(Position.X + Velocity.X, Position.Y + Velocity.Y);

            float angle = (float)Math.Atan2(Velocity.Y, Velocity.X);
            foreach (var part in Parts)
            {
                part.Size = Size / 10f;
                part.UpdatePosition(Position, angle);
            }
        }

        public void Draw(Graphics g)
        {
            foreach (var p in Parts.OrderBy(p => p.RelativePosition.X))
                p.Draw(g);
        }
    }
}

