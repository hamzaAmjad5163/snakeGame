using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace snakegame
{
    public partial class Form1 : Form
    {
        private List<circle> snake = new List<circle>();
        private circle food = new circle();
        int maxwidth;
        int maxheight;
        int score;
        int highscore;
        Random ran = new Random();
        bool goLeft, goRight,godown,goup;
        public Form1()
        {
            InitializeComponent();
            new settings();
        }

        private void keydown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Left && settings.directions != "right")
            {
                goLeft = true;
            }

            if (e.KeyCode == Keys.Right && settings.directions != "left")
            {
                goRight = true;
            }

            if (e.KeyCode == Keys.Down && settings.directions != "up")
            {
                godown = true;
            }

            if (e.KeyCode == Keys.Up && settings.directions != "down")
            {
                goup = true;
            }
        }

        private void keyup(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left )
            {
                goLeft = false;
            }

            if (e.KeyCode == Keys.Right )
            {
                goRight = false;
            }

            if (e.KeyCode == Keys.Down)
            {
                godown = false;
            }

            if (e.KeyCode == Keys.Up )
            {
                goup = false;
            }
        }

        private void startgame(object sender, EventArgs e)
        {
            restart();
        }

        private void screenshot(object sender, EventArgs e)
        {
            Label caption = new Label();
            caption.Text = "Total Scored: "+ " " + score+ "And Highscore is: "+highscore;
            caption.Font = new Font("ariel",10,FontStyle.Bold);
            caption.ForeColor = Color.DarkRed;
            caption.AutoSize = false;
            caption.Width = piccanvas.Width;
            caption.Height = 30;
            caption.TextAlign = ContentAlignment.MiddleCenter;
            piccanvas.Controls.Add(caption);

            SaveFileDialog save = new SaveFileDialog();
            save.FileName = "snakegame";
            save.DefaultExt = "jpg";
            save.Filter = "jpg image file | *.jpg";
            save.ValidateNames = true;

            if (save.ShowDialog() == DialogResult.OK)
            {
                int width = Convert.ToInt32(piccanvas.Width);
                int height = Convert.ToInt32(piccanvas.Height);
                Bitmap bmp = new Bitmap(width, height);
                piccanvas.DrawToBitmap(bmp, new Rectangle(0, 0, width, height));
                bmp.Save(save.FileName,ImageFormat.Jpeg);
                piccanvas.Controls.Remove(caption);
            }

        }

        private void gameEvent(object sender, EventArgs e)
        {
            if (goRight)
            {
                settings.directions = "right";
            }
            if (goLeft)
            {
                settings.directions = "left";
            }
            if (godown)
            {
                settings.directions = "down";
            }
            if (goup)
            {
                settings.directions = "up";
            }
            for (int i = snake.Count - 1; i >= 0; i--)
            { 
                if(i == 0)
                {
                    switch (settings.directions)
                    {
                        case "left":
                            snake[i].x--;
                            break;
                        case "right":
                            snake[i].x++;
                            break;
                        case "down":
                            snake[i].y++;
                            break;
                        case "up":
                            snake[i].y--;
                            break;
                        
                    }
                    if(snake[i].x < 0)
                    {
                        snake[i].x = maxwidth;
                    }
                    if( snake[i].x > maxwidth)
                    {
                        snake[i].x = 0;
                    }
                    if (snake[i].y < 0)
                    {
                        snake[i].y = maxheight;
                    }
                    if (snake[i].y > maxheight)
                    {
                        snake[i].y = 0;
                    }
                    if (snake[i].x == food.x && snake[i].y == food.y)
                    {
                        eat();
                    }
                    for(int j = 1; j < snake.Count; j++)
                    {
                        if (snake[i].x == snake[j].x && snake[i].y == snake[j].y)
                        {
                            gameOver(); 
                        }
                    }
                }
                else
                {
                    snake[i].x = snake[i-1].x;
                    snake[i].y = snake[i - 1].y;
                }
            }piccanvas.Invalidate();
            }

        private void updatePicturebox(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;
            Brush snakecolour;
            for (int i = 0; i < snake.Count; i++)
            {
                if (i == 0)
                {
                    snakecolour = Brushes.DarkBlue;
                }
                else
                {
                    snakecolour = Brushes.DarkGreen;
                }
                canvas.FillEllipse(snakecolour,new Rectangle(snake[i].x * settings.width, snake[i].y*settings.height,settings.width,settings.height));

            }
            canvas.FillEllipse(Brushes.DarkRed, new Rectangle(food.x * settings.width, food.y * settings.height, settings.width, settings.height));

        }
        private void restart()
        {
            maxwidth = piccanvas.Width / settings.width - 1;
            maxheight = piccanvas.Height / settings.height - 1;
            snake.Clear();
            button1.Enabled = false;
            button2.Enabled = false;
            score = 0;
            txtscore.Text = "Score: " + score;
            circle head = new circle { x = 10, y = 5 };
            snake.Add(head);
            for(int i = 0; i < 10; i++)
            {
                circle body = new circle ();
                snake.Add (body);
            }
            food =new circle { x = ran.Next(2,maxwidth),y = ran.Next(2,maxheight) };
            gametimer.Start();
        }
        private void eat()
        {
            score += 1;
            txtscore.Text = "score: "+score;
            circle body = new circle
            {
                x = snake[snake.Count - 1].x,
                y = snake[snake.Count - 1].y
            };
            snake.Add(body);
            food = new circle { x = ran.Next(2, maxwidth), y = ran.Next(2, maxheight) };

        }
        private void gameOver()
        {
            gametimer.Stop();
            button1.Enabled = true;
            button2.Enabled = true;

            if(score > highscore)
            {
                highscore = score;
                txthighscore.Text = "High Score: "+Environment.NewLine+ highscore;
                txthighscore.ForeColor = Color.Red;
                txthighscore.TextAlign = ContentAlignment.MiddleCenter;
            }
        }
    }
}
