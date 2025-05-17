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
        public FoodParticle(PointF position, PointF velocity, Color color, float size, float life = 99999f)
            : base(position, velocity, color, size, life)
        {
        }

        // Визуально можно переопределить, если нужно
        public override void Draw(Graphics g)
        {
            int alpha = (int)(255 * (Life / 100f)); // если Life от 100 до 0
            if (alpha < 0) alpha = 0;
            if (alpha > 255) alpha = 255;

            Color fadedColor = Color.FromArgb(alpha, Color.R, Color.G, Color.B);

            using (Brush b = new SolidBrush(fadedColor))
            {
                g.FillEllipse(b, Position.X - Size / 2, Position.Y - Size / 2, Size, Size);
            }
        }

    }


    public class FishParticle 
    {
        public PointF Position;
        public PointF Velocity;
        public Color Color;
        public float Size;
        public PointF RelativePosition;
        public float Angle;
        public PointF Offset;

      

        public FishParticle(PointF position, PointF velocity, Color color, float size, PointF relative = default)
        {
            Position = position;
            Velocity = velocity;
            Color = color;
            Size = size;
            RelativePosition = relative;

        }

        public void UpdatePosition(PointF basePos, float angle)
        {
            Angle = angle;

            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);

            Position = new PointF(
                basePos.X + RelativePosition.X * cos - RelativePosition.Y * sin,
                basePos.Y + RelativePosition.X * sin + RelativePosition.Y * cos
            );
        }

        public void Draw(Graphics g)
        {
            int alpha = 160; // постоянная полупрозрачность

            var color = Color.FromArgb(alpha, Color);
            using (var b = new SolidBrush(color))
            {
                g.FillEllipse(b,
                    Position.X + Offset.X - Size / 2,
                    Position.Y + Offset.Y - Size / 2,
                    Size,
                    Size);
            }
        }
    }
}


