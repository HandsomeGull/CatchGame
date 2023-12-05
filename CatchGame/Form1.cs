using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CatchGame
{
    public partial class Form1 : Form
    {
        Rectangle hero = new Rectangle(280, 540, 40, 10);
        int heroSpeed = 10;

        List<Rectangle> balls = new List<Rectangle>();
        int ballSize = 10;
        int ballSpeed = 8;

        int score = 0;
        int time = 500;

        bool leftDown = false;
        bool rightDown = false;

        SolidBrush greenBrush = new SolidBrush(Color.Green);
        SolidBrush whiteBrush = new SolidBrush(Color.White);

        Random randGen = new Random();
        int randValue = 0;

        int groundHeight = 50;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftDown = true;
                    break;
                case Keys.Right:
                    rightDown = true;
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftDown = false;
                    break;
                case Keys.Right:
                    rightDown = false;
                    break;
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //move hero
            if(leftDown == true && hero.X > 0)
            {
                hero.X -= heroSpeed;
            }

            if (rightDown == true && hero.X < this.Width - hero.Width)
            {
                hero.X += heroSpeed;
            }

            //is it time to make a new ball?
            randValue = randGen.Next(1, 101);

            if (randValue < 11) // 10% of a green ball
            {
                int x = randGen.Next(10,this.Width - ballSize * 2);

                Rectangle newBall = new Rectangle(x, 0, ballSize, ballSize);
                balls.Add(newBall);
            }

            //move all the balls
            for (int i = 0; i < balls.Count(); i++)
            {
                //get the new position of y based on speed
                int y = balls[i].Y + ballSpeed;

                //replace the rectangle in the list with updated one
                balls[i] = new Rectangle(balls[i].X, y, ballSize, ballSize);
            }

            //remove balls if they go beyond the play area
            for (int i = 0; i < balls.Count(); i++)
            {
                if (balls[i].Y > hero.Y)
                {
                    balls.Remove(balls[i]); 
                }
            }

            //check for collision between ball and player
            for (int i = 0; i < balls.Count(); i++)
            {
                if (balls[i].IntersectsWith(hero))
                {
                    score += 5;
                    balls.RemoveAt(i);
                }
            }

            //decrease the time
            time--;

            //end the game if its time
            if (time == 0)
            {
                gameTimer.Enabled = false;
            }

            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //update labels
            timeLabel.Text = $"Time Left: {time}";
            scoreLabel.Text = $"Score: {score}";

            //draw ground
            e.Graphics.FillRectangle(greenBrush, 0, this.Height - groundHeight, this.Width, groundHeight);

            //draw hero
            e.Graphics.FillRectangle(whiteBrush, hero);

            //draw balls
            for (int i = 0; i < balls.Count(); i++)
            {
                e.Graphics.FillEllipse(greenBrush, balls[i]);
            }

        }
    }
}
