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
        Timer timer = new Timer();
        Emitter emitter;
        PointF playerPos = new PointF(400, 300);
        float playerSize = 20f;
        Color playerColor = Color.Blue;
        int score = 0;
        bool gameOver = false;

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Width = 800;
            this.Height = 600;

            emitter = new Emitter(playerPos);

            timer.Interval = 20;
            timer.Tick += Timer_Tick;
            timer.Start();

            this.MouseMove += Form1_MouseMove;
            this.Paint += Form1_Paint;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (gameOver) return;

            emitter.Update();
            playerPos = this.PointToClient(Cursor.Position);

            // check food collisions
            foreach (var f in emitter.particles.ToList())
            {
                float dx = f.Position.X - playerPos.X;
                float dy = f.Position.Y - playerPos.Y;
                float dist = (float)Math.Sqrt(dx * dx + dy * dy);
                if (dist < (playerSize + f.Size) / 2)
                {
                    emitter.particles.Remove(f);
                    playerSize += 0.5f;
                    playerColor = f.Color;
                    score++;
                }
            }

            // check enemy collisions
            foreach (var eFish in emitter.enemies.ToList())
            {
                float dx = eFish.Position.X - playerPos.X;
                float dy = eFish.Position.Y - playerPos.Y;
                float dist = (float)Math.Sqrt(dx * dx + dy * dy);
                if (dist < (playerSize + eFish.Size) / 2)
                {
                    if (playerSize > eFish.Size)
                    {
                        emitter.enemies.Remove(eFish);
                        playerSize += 1.0f;
                        score += 10;
                    }
                    else
                    {
                        gameOver = true;
                        timer.Stop();
                        MessageBox.Show("Game Over!");
                    }
                }
            }

            this.Invalidate();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            playerPos = e.Location;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            emitter.Draw(e.Graphics);

            if (!gameOver)
            {
                using (Brush b = new SolidBrush(playerColor))
                {
                    e.Graphics.FillEllipse(b, playerPos.X - playerSize / 2, playerPos.Y - playerSize / 2, playerSize, playerSize);
                }
            }

            using (Brush b = new SolidBrush(Color.Black))
            {
                e.Graphics.DrawString("Score: " + score, this.Font, b, 10, 10);
            }
        }
    }
}
