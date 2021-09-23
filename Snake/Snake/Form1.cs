using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        private int rI, rJ;
        private PictureBox fruit;
        private PictureBox stone;
        private PictureBox[] snake = new PictureBox[400];
        private Label labelScore;
        private int dirX, dirY;
        private int width = 900;   //window size
        private int height = 800;
        private int SizeOfSides = 40;
        private int score=0;
        public Form1()
        {
            InitializeComponent();
            this.Text = "Snake";
            this.Width = width;
            this.Height = height;
            dirX = 1;
            dirY = 0;
            labelScore = new Label();
            labelScore.Text = "Score: 0";
            labelScore.Location = new Point(810, 10);
            this.Controls.Add(labelScore);
            snake[0] = new PictureBox();
            snake[0].Location = new Point(201, 201);
            snake[0].Size = new Size(SizeOfSides-1, SizeOfSides-1);
            snake[0].BackColor = Color.Green;
            this.Controls.Add(snake[0]);
            fruit = new PictureBox();
            fruit.BackColor = Color.Red;
            fruit.Size = new Size(SizeOfSides, SizeOfSides);
            stone = new PictureBox();
            stone.BackColor = Color.Gray;
            stone.Size = new Size(SizeOfSides, SizeOfSides);
            GenerateMap();
            GenerateFruit();
            GenerateStone();
            timer.Tick += new EventHandler(Update);
            timer.Interval = 200;
            timer.Start();
            this.KeyDown += new KeyEventHandler(OKP);
        }

        private void GenerateFruit()
        {
            Random r = new Random();
            rI = r.Next(0, height-SizeOfSides);
            int tempI = rI % SizeOfSides;
            rI -= tempI;
            rJ = r.Next(0, height - SizeOfSides);
            int tempJ = rJ % SizeOfSides;
            rJ -= tempJ;
            rI++;
            rJ++;
            fruit.Location = new Point(rI, rJ);
            this.Controls.Add(fruit);
        }

        private void GenerateStone()
        {
            Random r = new Random();
            rI = r.Next(0, height - SizeOfSides);
            int tempI = rI % SizeOfSides;
            rI -= tempI;
            rJ = r.Next(0, height - SizeOfSides);
            int tempJ = rJ % SizeOfSides;
            rJ -= tempJ;
            rI++;
            rJ++;
            stone.Location = new Point(rI, rJ);
            this.Controls.Add(stone);
        }

        private void CheckBorders()
        {
            if (snake[0].Location.X < 0)
            {
                for(int i = 1; i <= score; i++)
                {
                    this.Controls.Remove(snake[i]);            
                }
                score = 0;
                labelScore.Text = "Score: " + score;
                dirX = 1;
            }
            if (snake[0].Location.X > height)
            {
                for (int i = 1; i <= score; i++)
                {
                    this.Controls.Remove(snake[i]);
                }
                score = 0;
                labelScore.Text = "Score: " + score;
                dirX = -1;
            }
            if (snake[0].Location.Y < 0)
            {
                for (int i = 1; i <= score; i++)
                {
                    this.Controls.Remove(snake[i]);
                }
                score = 0;
                labelScore.Text = "Score: " + score;
                dirY = 1;
            }
            if (snake[0].Location.Y > height)
            {
                for (int i = 1; i <= score; i++)
                {
                    this.Controls.Remove(snake[i]);
                }
                score = 0;
                labelScore.Text = "Score: " + score;
                dirY = -1;
            }
        }

        private void EatItself()  //function with the effects of self-bite
        {
            for(int i=1; i< score; i++)
            {
                if(snake[0].Location == snake[i].Location)
                {
                    for (int j = i; j <= score; j++)
                        this.Controls.Remove(snake[j]);
                    score = score - (score - i + 1);
                    labelScore.Text = "Score: " + score;
                }
            }
        }

        private void EatFruit()
        {
            if(snake[0].Location.X == rI && snake[0].Location.Y == rJ)
            {
                labelScore.Text = "Score: " + ++score;
                snake[score] = new PictureBox();
                snake[score].Location = new Point(snake[score - 1].Location.X + 40 * dirX, snake[score - 1].Location.Y - 40 * dirY);
                snake[score].Size = new Size(SizeOfSides-1, SizeOfSides-1);
                snake[score].BackColor = Color.Green;
                this.Controls.Add(snake[score]);
                GenerateFruit();
            }
        }

        private void EatStone()
        {
            if (snake[0].Location.X == rI && snake[0].Location.Y == rJ)
            {
                for (int i = 1; i <= score; i++)
                {
                    this.Controls.Remove(snake[i]);
                }
                score = 0;
                labelScore.Text = "Score: " + score;
            }
        }

        private void GenerateMap()     //creating map
        {
            for(int i = 0; i < width / SizeOfSides; i++)    //creating horizontal lines
            {
                PictureBox pic = new PictureBox();
                pic.BackColor = Color.Black;
                pic.Location = new Point(0, SizeOfSides * i);
                pic.Size = new Size(width - 100, 1);
                this.Controls.Add(pic);
            }
            for (int i = 0; i <= height / SizeOfSides; i++)   //creating vertical lines
            {
                PictureBox pic = new PictureBox();
                pic.BackColor = Color.Black;
                pic.Location = new Point( SizeOfSides * i,0);
                pic.Size = new Size(1, width);
                this.Controls.Add(pic);
            }
        }

        private void MoveSnake()
        {
            for(int i = score; i >= 1; i--)
            {
                snake[i].Location = snake[i - 1].Location;
            }
            snake[0].Location = new Point(snake[0].Location.X + dirX * (SizeOfSides), snake[0].Location.Y + dirY * (SizeOfSides));
            EatItself();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Update(Object myObject,EventArgs eventsArgs)
        {
            CheckBorders();
            EatFruit();
            EatStone();
            MoveSnake();
        }

        private void OKP(object sender, KeyEventArgs e) //processing keystrokes for control
        {
            switch (e.KeyCode.ToString())
            {
                case "Right":
                    dirX = 1;
                    dirY = 0;
                    break;
                case "Left":
                    dirX = -1;
                    dirY = 0;
                    break;
                case "Up":
                    dirY = -1;
                    dirX = 0;
                    break;
                case "Down":
                    dirY = 1;
                    dirX = 0;
                    break;
            }
        }
    }
}
