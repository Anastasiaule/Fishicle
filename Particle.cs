using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishicle
{
    public class Particle
    {
        public float X, Y;
        public float SpeedX, SpeedY;
        public int Radius;
        public Color Color;
        public bool Alive = true;

        public Particle(float x, float y, int radius, Color color)
        {
            X = x; Y = y; Radius = radius; Color = color;
            // random drift for food
            var rnd = new Random();
            SpeedX = (float)(rnd.NextDouble() * 1 - 0.5);
            SpeedY = (float)(rnd.NextDouble() * 1 - 0.5);
        }

        public virtual void Update()
        {
            X += SpeedX;
            Y += SpeedY;
        }

        public virtual void Draw(Graphics g)
        {
            using (var brush = new SolidBrush(Color))
                g.FillEllipse(brush, X - Radius, Y - Radius, Radius * 2, Radius * 2);
        }

        public float DistanceTo(Particle other)
        {
            float dx = X - other.X, dy = Y - other.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }
    }
    public class ParticleFish : Particle
    {
        public PointF BaseOffset;
        public PointF GlobalCenter;
        public float Scale = 1f;
        public float Speed = 0.1f;

        public ParticleFish(PointF baseOffset) : base(0, 0, 2, Color.Orange)
        {
            BaseOffset = baseOffset;
        }

        public override void Update()
        {
            // target for this particle
            float tx = GlobalCenter.X + BaseOffset.X * Scale;
            float ty = GlobalCenter.Y + BaseOffset.Y * Scale;
            float dx = tx - X, dy = ty - Y;
            X += dx * Speed;
            Y += dy * Speed;
        }
    }
}
