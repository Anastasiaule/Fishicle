using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace Fishicle
{
  public partial class Form1 : Form
    {
        private Timer timer = new Timer();
        private Emitter emitter;
        private Fish playerFish;
        private float playerSize = 40;
        private int score = 0;
        private PointF mousePos = new PointF(400, 300);
        private Random rand = new Random();

        public Form1()
        {
            InitializeComponent();
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;

            emitter = new Emitter(PointF.Empty);

            // Создание рыбки игрока
            playerFish = new Fish(new PointF(300, 300), new PointF(0, 0), Color.Blue, playerSize);

            timer.Interval = 16;
            timer.Tick += (s, e) =>
            {
                UpdateGame();
                pictureBox1.Invalidate();
            };
            timer.Start();

            pictureBox1.MouseMove += (s, e) => mousePos = e.Location;
            pictureBox1.Paint += pictureBox1_Paint;
        }

        private void UpdateGame()
        {
            emitter.Update();

            // Движение игрока
            var dir = new PointF(mousePos.X - playerFish.Position.X, mousePos.Y - playerFish.Position.Y);
            float length = (float)Math.Sqrt(dir.X * dir.X + dir.Y * dir.Y);
            if (length > 0)
            {
                dir.X /= length;
                dir.Y /= length;
            }
            playerFish.Velocity = new PointF(dir.X * 4, dir.Y * 4);
            playerFish.Update();


            // Поедание еды
            foreach (var food in emitter.particles.ToArray())
            {
                float dx = playerFish.Position.X - food.Position.X;
                float dy = playerFish.Position.Y - food.Position.Y;
                float dist = (float)Math.Sqrt(dx * dx + dy * dy);
                if (dist < 15f)
                {
                    emitter.particles.Remove(food);
                    score += 10;
                    playerSize += 1;
                    playerFish.Size = playerSize;
                    playerFish.CreateBody();
                }
            }

            // Столкновения с врагами
            foreach (var enemy in emitter.enemies.ToArray())
            {
                var enemyHead = enemy.Position;
                float dx = playerFish.Position.X - enemyHead.X;
                float dy = playerFish.Position.Y - enemyHead.Y;
                float dist = (float)Math.Sqrt(dx * dx + dy * dy);

                float enemySize = enemy.Size;

                if (dist < 20f)
                {
                    if (playerSize > enemySize)
                    {
                        // Создаем "взрыв" кровавых частиц
                        for (int i = 0; i < 20; i++)
                        {
                            float angle = (float)(rand.NextDouble() * Math.PI * 2);
                            float speed = (float)(rand.NextDouble() * 4 + 1);
                            var vel = new PointF((float)Math.Cos(angle) * speed, (float)Math.Sin(angle) * speed);

                            emitter.temporaryParticles.Add(new FoodParticle(
                                enemy.Position,
                                vel,
                                Color.FromArgb(200, Color.Red),
                                10f,
                                50f
                            ));

                        }

                        emitter.enemies.Remove(enemy);
                        score += 10;
                        playerSize += 2;
                        playerFish.Size = playerSize;
                    }
                    else
                    {
                        timer.Stop();
                        MessageBox.Show("Вы проиграли!");
                        Application.Exit();
                    }
                }
            }



            if (score >= 1000)
            {
                timer.Stop();
                MessageBox.Show("вы победитель!");
                Application.Exit();
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            emitter.Draw(g);
            playerFish.Draw(g);

            g.DrawString($"Счёт: {score}", new Font("Arial", 16), Brushes.Black, 10, 10);
        }
    }
}