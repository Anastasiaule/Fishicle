using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
        private List<FishParticle> playerFish = new List<FishParticle>();
        private float playerSize = 20;
        private int score = 0;
        private PointF mousePos = new PointF(400, 300);
        private Random rand = new Random();
        public Form1()
        {
            InitializeComponent();

            // Установка фонового изображения (если файл существует)
         
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;

            emitter = new Emitter(PointF.Empty);

            for (int i = 0; i < 10; i++)
            {
                var offset = new PointF(i * 5, (float)Math.Sin(i * 0.5f) * 5);
                var color = Color.FromArgb(
                    rand.Next(150, 256),
                    Color.Blue
                );
                var size = playerSize / 10f * (1 - i * 0.03f);

                playerFish.Add(new FishParticle(
                    new PointF(400 + offset.X, 300 + offset.Y),
                    PointF.Empty,
                    color,
                    size
                ));
            }

            timer.Interval = 16;
            timer.Tick += (s, e) =>
            {
                UpdateGame();
                pictureBox1.Invalidate(); // обновляем PictureBox
            };
            timer.Start();

            pictureBox1.MouseMove += (s, e) => mousePos = e.Location;
            pictureBox1.Paint += pictureBox1_Paint;
        }

        private void UpdateGame()
        {
            emitter.Update();

            // Двигаем игрока
            for (int i = playerFish.Count - 1; i > 0; i--)
            {
                playerFish[i].Position = playerFish[i - 1].Position;
            }
            var head = playerFish[0];
            var dir = new PointF(mousePos.X - head.Position.X, mousePos.Y - head.Position.Y);
            float length = (float)Math.Sqrt(dir.X * dir.X + dir.Y * dir.Y);
            if (length > 0)
            {
                dir.X /= length;
                dir.Y /= length;
            }
            head.Position = new PointF(head.Position.X + dir.X * 4, head.Position.Y + dir.Y * 4);

            // Едим еду
            foreach (var food in emitter.particles.ToArray())
            {
                float dx = head.Position.X - food.Position.X;
                float dy = head.Position.Y - food.Position.Y;
                float dist = (float)Math.Sqrt(dx * dx + dy * dy);
                if (dist < 15f)

                {
                    emitter.particles.Remove(food);
                    score += 10;
                    playerSize += 1;
                    foreach (var part in playerFish)
                        part.Size = playerSize / 10f;
                }
            }

            // Столкновения с врагами
            foreach (var enemy in emitter.enemies.ToArray())
            {
                var enemyHead = enemy[0];
                float dx = head.Position.X - enemyHead.Position.X;
                float dy = head.Position.Y - enemyHead.Position.Y;
                float dist = (float)Math.Sqrt(dx * dx + dy * dy);

                float enemySize = enemyHead.Size * 10;

                if (dist < 25f)

                {
                    if (playerSize > enemySize)
                    {
                        emitter.enemies.Remove(enemy);
                        score += 10;
                        playerSize += 2;
                        foreach (var part in playerFish)
                            part.Size = playerSize / 10f;
                    }
                    else
                    {
                        timer.Stop();
                        MessageBox.Show("Ты проиграла!");
                        Application.Exit();
                    }
                }
            }

            if (score >= 2000)
            {
                timer.Stop();
                MessageBox.Show("Ты выиграла!");
                Application.Exit();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            emitter.Draw(e.Graphics);
            foreach (var part in playerFish)
                part.Draw(e.Graphics);

            e.Graphics.DrawString($"Счёт: {score}", new Font("Arial", 16), Brushes.Black, 10, 10);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            emitter.Draw(g);

            foreach (var part in playerFish)
                part.Draw(g);

            g.DrawString($"Счёт: {score}", new Font("Arial", 16), Brushes.Black, 10, 10);
        }

    }
}
