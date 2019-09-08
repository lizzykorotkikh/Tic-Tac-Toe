using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using static Square;


namespace TicTacToe
{
    internal class Opend : Form
    {

        Hidden t;
        private Pen p = new Pen(Color.Black, 0);
        private Font drawFont = new Font("Comic Sans MS", 0.75f);
        private StringFormat drawFormat = new StringFormat();
        private SolidBrush drawBrush = new SolidBrush(Color.Black);
        bool flag = true;
        int level = 0;
        private Button button1 = new Button();
        private Button button2 = new Button();
        StreamReader file = new StreamReader("Text.txt");  

        Opend()
        {
            Text = "Tic Tac Toe";
            StartPosition = FormStartPosition.CenterScreen;
            MouseDown += onMouseDown;
            Paint += onPaint;
            DoubleBuffered = true;
            ClientSize = new Size(400, 400);
            init();
        }

        void init()
        {
            t = new Hidden();
            Invalidate();
            t.changed += Invalidate;
        }

        void onMouseDown(object sender, MouseEventArgs args)
        {

            if (flag)
            {
                Invalidate();
                flag = false;
            }
            else
            {
                if (t.winner > 0)
                {
                    level = 0;
                    init(); //update if it's the end of the game
                }
                else
                {
                    int x = ((int)(args.X + 50)) / 100 - 1;
                    int y = ((int)(args.Y + 50)) / 100 - 1;
                    if ((x >= 0 && x <= 2) && (y >= 0 && y <= 2))
                        t.PersonsStep(x, y, level);
                }
            }
        }

        void onPaint(object sender, PaintEventArgs arg)
        {

            Graphics g = arg.Graphics;
            g.FillRectangle(Brushes.Ivory, 0, 0, 400, 400);
            g.TranslateTransform(50, 50);
            g.ScaleTransform(25, 25);


            if (flag)
                Instructions(arg.Graphics);
            else if (level != 0)
                Game(arg.Graphics);
            else ChoosingLevel(arg.Graphics);

        }

        void Instructions(Graphics g){
			
			StreamReader file = new StreamReader("Text.txt");  
			g.DrawString(file.ReadLine(), new Font("Comic Sans MS", 0.35f), new SolidBrush(Color.Red), 7, 12, drawFormat);
			g.DrawString(file.ReadLine(), drawFont, new SolidBrush(Color.Turquoise), 4, -1.5f, drawFormat);
			g.DrawString(file.ReadLine(), drawFont, new SolidBrush(Color.LimeGreen), -1, 0, drawFormat);
			g.DrawString(file.ReadLine(), new Font("Arial Black", 0.41f), drawBrush, -0.75f, 2, drawFormat);
			int i = 3;
			while( i<11){
				g.DrawString(file.ReadLine(), new Font("Common serif fonts", 0.35f), drawBrush, -0.70f, i, drawFormat);
				++i;
			}  
			file.Close();
		}

        void ChoosingLevel(Graphics g){


            

	string choose = @"Choose who you want to play with:";
		g.DrawString ( choose, drawFont, new SolidBrush(Color.Black), -1.73f, -1, drawFormat );
		g.DrawString ( choose, drawFont, new SolidBrush(Color.Magenta), -1.75f, -1, drawFormat );

            InitializeButtons();
            button1.Click += new EventHandler(Button1Click);
            button2.Click += new EventHandler(Button2Click);
   Invalidate();
        }

        private void InitializeButtons()
        {

            button1.Location = new System.Drawing.Point(75, 125);
            button1.Size = new System.Drawing.Size(250, 60);
            button1.Text = "With a computer";
            button1.BackColor = Color.DarkOrchid;
            button1.FlatStyle = FlatStyle.Popup;
            button1.ForeColor = Color.Black;
            button1.Font = new Font("Comic Sans MS", 13f);

            button2.Location = new System.Drawing.Point(75, 225);
            button2.Size = new System.Drawing.Size(250, 60);
            button2.Text = "With a friend";
            button2.BackColor = Color.DarkOrchid;
            button2.FlatStyle = FlatStyle.Popup;
            button2.ForeColor = Color.Black;
            button2.Font = new Font("Comic Sans MS", 13f);

            Controls.Add(button1);
            Controls.Add(button2);

        }

        private void Button1Click(object sender, System.EventArgs e)
        {
            level = 1;
            Controls.Remove(button1);
            Controls.Remove(button2);
        }

        private void Button2Click(object sender, System.EventArgs e)
        {
            level = 2;
            Controls.Remove(button1);
            Controls.Remove(button2);
        }

        void Game(Graphics g)
        {
            g.ResetTransform();
            g.TranslateTransform(50, 50);
            g.ScaleTransform(100, 100);

            if (t.winner > 0 && t.winner < 3)
            { 
                g.FillRectangle(new SolidBrush(Color.Yellow), t.c1.x ,t.c1.y, 1, 1);
                g.FillRectangle(new SolidBrush(Color.Yellow), t.c2.x, t.c2.y, 1, 1);
                g.FillRectangle(new SolidBrush(Color.Yellow), t.c3.x, t.c3.y, 1, 1);
 
            }

            int s = 0;
            g.DrawRectangle(new Pen(Color.Red, 0), 0, 0, 3, 3);
            for (float i = 1f; i < 3f; ++i)
            {
                g.DrawLine(p, i, 0, i, 3);
                g.DrawLine(p, 0, i, 3, i);
            }

            for (int x = 0; x < 3; ++x)
                for (int y = 0; y < 3; ++y)
                {
                    switch (t.board[x, y])
                    {
                        case Cross: drawCross(g, x, y); break;
                        case Nought: drawNought(g, x, y); break;
                        case Empty: ++s; break;
                    }
                }

            if (s == 9)
            {
                GraphicsState transState = g.Save();
                g.ResetTransform();
                if (level == 1)
                {
					string line = File.ReadLines("Text.txt").Skip(12).Take(1).First();
                    g.DrawString(line, new Font("Comic Sans MS", 9), new SolidBrush(Color.DarkBlue), new RectangleF(10, 10, 380, 20));
                    line = File.ReadLines("Text.txt").Skip(13).Take(1).First();
                    g.DrawString(line, new Font("Comic Sans MS", 9), new SolidBrush(Color.DarkBlue), new RectangleF(10, 30, 380, 20));
					 
                }
                else
                { 
                    string line = File.ReadLines("Text.txt").Skip(14).Take(1).First();
                    g.DrawString(line, new Font("Comic Sans MS", 9), new SolidBrush(Color.DarkBlue), new RectangleF(40, 10, 350, 20));
                    line = File.ReadLines("Text.txt").Skip(15).Take(1).First();
                    g.DrawString(line, new Font("Comic Sans MS", 9), new SolidBrush(Color.DarkBlue), new RectangleF(40, 30, 350, 20));
                }
                g.Restore(transState);
            }

        }

        void drawNought(Graphics g, int y, int x)
        {
            g.DrawEllipse(p, x + 0.1f, y + 0.1f, 0.8f, 0.8f);
        }

        void drawCross(Graphics g, int y, int x)
        {
            g.DrawLine(p, x + 0.1f, y + 0.1f, x + 0.9f, y + 0.9f);
            g.DrawLine(p, x + 0.9f, y + 0.1f, x + 0.1f, y + 0.9f);
        }

        static void Main()
        {
            Application.Run(new Opend());
        }
    }
}
