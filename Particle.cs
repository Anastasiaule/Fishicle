using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fishicle
{
    public class Particle
    {
        public PointF Position;
        public PointF Velocity;
        public float Life;
        public Color Color;
        public float Size;

        public Particle(PointF position, PointF velocity, float life, Color color, float size)
        {
            Position = position;
            Velocity = velocity;
            Life = life;
            Color = color;
            Size = size;
        }

        public virtual void Update()
        {
            Position = new PointF(Position.X + Velocity.X, Position.Y + Velocity.Y);
            Life -= 1;
        }

        public virtual void Draw(Graphics g)
        {
            int alpha = (int)(255 * (Life / 100f));
            alpha = Math.Max(0, Math.Min(255, alpha));

            using (Brush b = new SolidBrush(Color.FromArgb(alpha, Color)))
            {
                g.FillEllipse(b, Position.X - Size / 2, Position.Y - Size / 2, Size, Size);
            }
        }
    }

    public class FoodParticle : Particle
    {
        private static Random rand = new Random();

        public FoodParticle(PointF position)
            : base(
                position,
                new PointF((float)(rand.NextDouble() * 2 - 1), (float)(rand.NextDouble() * 2 - 1)),
                100,
                Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256)),
                5f
            )
        {
        }
    }

    public class FishParticle : Particle
    {
        public FishParticle(PointF position, PointF velocity, Color color, float size)
            : base(position, velocity, 100, color, size)
        {
        }
    }

}
