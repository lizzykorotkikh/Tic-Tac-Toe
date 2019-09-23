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
		int casesize;
		int length;
        private TextBox TextBox1 = new TextBox(){
			Width = 300,
			Height = 100,
			BackColor = Color.White,
			ForeColor = Color.Black, 
			Location = new Point(50, 150) 
			};
		private Button button1 = new Button(){
			
			Location = new System.Drawing.Point(75, 125),
            Size = new System.Drawing.Size(250, 60),
            Text = "With a computer",
            BackColor = Color.DarkOrchid,
            FlatStyle = FlatStyle.Popup,
            ForeColor = Color.Black,
            Font = new Font("Comic Sans MS", 13f)
			};
		private Button button2 = new Button(){
			
			Location = new System.Drawing.Point(75, 225),
            Size = new System.Drawing.Size(250, 60),
            Text = "With a friend",
            BackColor = Color.DarkOrchid,
            FlatStyle = FlatStyle.Popup,
            ForeColor = Color.Black,
            Font = new Font("Comic Sans MS", 13f)
			
			
			};
		private Label label = new Label(){
			AutoSize = false, 
			TextAlign = ContentAlignment.MiddleCenter, 
			 Dock = DockStyle.None,
			Location = new Point(25, 25) ,
			Width = 350,
			Height = 50,
			Font = new Font("Comic Sans MS", 15),
			ForeColor = Color.Magenta
			};
        
        private int level = 0; 

        
		private Pen p = new Pen(Color.Black, 0);
		
        public static void Main(){
            Application.EnableVisualStyles();
            Application.Run(new Opend());
        }

        public Opend()
        {
			 Text = "Tic Tac Toe";
            StartPosition = FormStartPosition.CenterScreen;
            DoubleBuffered = true;
            ResizeRedraw = true;
            ClientSize = new Size(400, 400);
			MouseDown += onMouseDown;
            Paint += onPaint;
            init();
        }
        void InitializeBoxLabel () {
			Controls.Add(TextBox1);
			label.Text = "Choose the size of the board from 3 to 10";
			Controls.Add(label);
		}
        
        void TextBoxKeyPress(object sender, KeyPressEventArgs e){
			
			if (e.KeyChar == (char)Keys.Enter){
				if (TextBox1.Text!=""){
					if (int.Parse(TextBox1.Text)>=3 && int.Parse(TextBox1.Text)<=10){
						t.size =int.Parse(TextBox1.Text); 
						if (t.size>5) t.winscore=5;
						else t.winscore=t.size;
						t.board = new Square [t.size,t.size];
						Controls.Remove(TextBox1); 
						ChoosingLevel();
					} 
				else MessageBox.Show("Wrong size"); 
				TextBox1.Clear();
				}
			 }
			else if (!(e.KeyChar >= 48 && e.KeyChar <= 57) ) {
				 e.Handled = true;
				MessageBox.Show("Incorrect character");
				}
         }
         
         void ChoosingLevel(){
			label.Text = "Choose who you want to play with:";
            InitializeButtons();
            button1.Click += new EventHandler(Button1Click);
            button2.Click += new EventHandler(Button2Click);
        }
         
        private void InitializeButtons(){
            Controls.Add(button1);
            Controls.Add(button2);
        }

        private void Button1Click(object sender, System.EventArgs e){
            level = 1;
            t.combinations = new Pair [t.winscore+1];
			t.CreateCombinations(t.winscore);
            Controls.Remove(button1);
            Controls.Remove(button2);
            Controls.Remove(label); 
        }

        private void Button2Click(object sender, System.EventArgs e){
           level = 2;
            Controls.Remove(button1);
            Controls.Remove(button2);
            Controls.Remove(label); 
        }
         
         void init(){
            t = new Hidden();
            level=0;
            InitializeBoxLabel();
            TextBox1.KeyPress += new KeyPressEventHandler(TextBoxKeyPress);
            Invalidate();
            t.changed += Invalidate;
        }
         
          void onMouseDown(object sender, MouseEventArgs args)
        {

                if (t.winner > 0)
                {
					Invalidate();
                    init(); //update if it's the end of the game
                }
                else if (level!=0)
                {
                    int x = ((int)(args.X-50))/casesize;
                    int y = ((int)(args.Y-50))/casesize;
                    
                    if ((x >= 0 && x < t.size) && (y >= 0 && y < t.size))
                        t.PersonsStep(x, y, level);
                }
        }
         
         
         void onPaint(object sender, PaintEventArgs arg){
			Graphics g = arg.Graphics;
			g.TranslateTransform(50, 50);
			if (level != 0)Game(arg.Graphics);
        }
         
         void Game(Graphics g){
			
			 casesize=300/t.size;
			length = casesize*t.size;
            g.DrawRectangle(new Pen(Color.Red, 0), 0, 0, length, length);
            for (int i = casesize; i < length; i+=casesize)
            {
                g.DrawLine(p, i, 0, i, length);
                g.DrawLine(p, 0, i, length, i);
            }

            for (int x = 0; x < t.size; ++x)
                for (int y = 0; y < t.size; ++y)
                {
                    switch (t.board[x, y])
                    {
                        case Cross: drawCross(g, x, y); break;
                        case Nought: drawNought(g, x, y); break;
                    }
                }

        }

        void drawNought(Graphics g, int y, int x)
        {
            g.DrawEllipse(p, x*casesize , y*casesize , casesize, casesize);
        }

        void drawCross(Graphics g, int y, int x)
        {
            g.DrawLine(p, x*casesize, y*casesize, x*casesize+casesize, y*casesize+casesize);
            g.DrawLine(p, x *casesize+casesize, y *casesize, x *casesize, y*casesize+casesize);
        }
         
    }
}

