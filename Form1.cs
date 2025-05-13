using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fishicle
{
    public partial class Form1 : Form
    {
        private Timer timer;
        private Emitter player;
        private List<Particle> food = new List<Particle>();
        private List<Emitter> enemies = new List<Emitter>();
        private Random rand = new Random();

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Width = 800; this.Height = 600;

            player = new Emitter(new PointF(400, 300));

            // spawn initial food
            for (int i = 0; i < 50; i++) SpawnFood();
            // spawn enemies
            for (int i = 0; i < 5; i++) enemies.Add(new Emitter(RandomPoint()));

            timer = new Timer { Interval = 16 };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            PointF mouse = PointToClient(MousePosition);
            player.Update(true, mouse);

            // update enemies
            foreach (var em in enemies) em.Update(false, PointF.Empty);

            // check food collisions
            for (int i = food.Count - 1; i >= 0; i--)
            {
                var f = food[i];
                if (player.FishParticles.Any(p => p.DistanceTo(f) < p.Radius + f.Radius))
                {
                    food.RemoveAt(i);
                    player.Scale += 0.02f;
                    player.FishColor = f.Color;
                    player.FishParticles.ForEach(p => p.Color = f.Color);
                    SpawnFood();
                }
            }

            // check fish collisions
            float playerSize = player.GetSize();
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                var en = enemies[i];
                float dist = Distance(player.Center, en.Center);
                if (dist < playerSize)
                {
                    // eat enemy
                    enemies.RemoveAt(i);
                    player.Scale += 0.1f;
                    enemies.Add(new Emitter(RandomPoint()));
                }
                else if (dist < en.GetSize())
                {
                    // player eaten
                    timer.Stop();
                    MessageBox.Show("Game Over");
                    Application.Exit();
                }
            }

            Invalidate();
        }

        private void SpawnFood()
        {
            float x = rand.Next(50, Width - 50);
            float y = rand.Next(50, Height - 50);
            int r = 4;
            var color = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
            var f = new Particle(x, y, r, color);
            food.Add(f);
        }

        private PointF RandomPoint()
        {
            return new PointF(rand.Next(100, Width - 100), rand.Next(100, Height - 100));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            // draw food
            foreach (var f in food) f.Draw(g);
            // draw enemies
            foreach (var en in enemies) en.Draw(g);
            // draw player
            player.Draw(g);
        }

        private float Distance(PointF a, PointF b)
        {
            float dx = a.X - b.X, dy = a.Y - b.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }
    }

}
