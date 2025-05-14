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
        public float Size;
        public Color Color;
        public float Life;

        public Particle(PointF position, PointF velocity, Color color, float size, float life = 99999)
        {
            Position = position;
            Velocity = velocity;
            Color = color;
            Size = size;
            Life = life;
        }

        public virtual void Update()
        {
            Position = new PointF(Position.X + Velocity.X, Position.Y + Velocity.Y);
            Life -= 1f; // уменьшается очень медленно
        }

        public virtual void Draw(Graphics g)
        {
            using (Brush b = new SolidBrush(Color))
            {
                g.FillEllipse(b, Position.X - Size / 2, Position.Y - Size / 2, Size, Size);
            }
        }
    }

    public class FoodParticle : Particle
    {
        public FoodParticle(PointF position, Color color, float size)
            : base(position, new PointF(0, 0), color, size, 99999) { }
    }

    public class FishParticle : Particle
    {
        public FishParticle(PointF position, PointF velocity, Color color, float size)
            : base(position, velocity, color, size, 99999) { }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw(Graphics g)
        {
            using (Brush b = new SolidBrush(Color.FromArgb((int)Math.Min(255, (Life / 100f) * 255), Color)))
            {
                g.FillEllipse(b, Position.X - Size / 2, Position.Y - Size / 2, Size, Size);
            }
        }
    }
}


