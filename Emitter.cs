using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishicle
{
    class Emitter
    {
        public List<ParticleFish> FishParticles = new List<ParticleFish>();
        public float Scale = 1f;
        public PointF Center;
        public Color FishColor = Color.Orange;
        private List<PointF> shapePoints;
        private Random rand = new Random();

        public Emitter(PointF center)
        {
            Center = center;
            GenerateShape();
            InitializeParticles();
        }

        // define base fish shape
        private void GenerateShape()
        {
            shapePoints = new List<PointF>();
            // body
            for (float x = -10; x <= 10; x += 0.5f)
            {
                float y = (float)(Math.Sin(x / 2) * 5);
                shapePoints.Add(new PointF(x, y));
            }
            // tail
            shapePoints.Add(new PointF(-11, 0));
            shapePoints.Add(new PointF(-12, -2));
            shapePoints.Add(new PointF(-12, 2));
        }

        private void InitializeParticles()
        {
            FishParticles.Clear();
            foreach (var pt in shapePoints)
            {
                var fish = new ParticleFish(pt)
                {
                    Scale = Scale,
                    GlobalCenter = Center,
                    Radius = 4,
                    Color = FishColor,
                    Speed = 0.1f
                };
                // random offset initial
                fish.X = Center.X + pt.X * Scale + rand.Next(-20, 20);
                fish.Y = Center.Y + pt.Y * Scale + rand.Next(-20, 20);
                FishParticles.Add(fish);
            }
        }

        public void Update(bool followMouse, PointF mousePos)
        {
            // move center if player
            if (followMouse) Center = mousePos;

            // animate scale (growth) applied in ParticleFish
            foreach (var p in FishParticles)
            {
                p.Scale = Scale;
                p.GlobalCenter = Center;
                p.Update();
            }
        }

        public void Draw(Graphics g)
        {
            foreach (var p in FishParticles)
                p.Draw(g);
        }

        // approximate fish size by bounding circle
        public float GetSize()
        {
            // calculate maximum distance of shape points from center
            float maxDist = 0;
            foreach (var pt in shapePoints)
            {
                float dist = (float)Math.Sqrt(pt.X * pt.X + pt.Y * pt.Y) * Scale;
                if (dist > maxDist)
                    maxDist = dist;
            }
            return maxDist;
        }
    }
}
