using System;
using System.Drawing;
using System.Windows.Forms;
using static Square;

namespace TicTacToe{
	internal partial class Opend : Form{

		Hidden t;
		private int casesize;
		private int length;
		private int level = 0; 
		private Pen p = new Pen(Color.Black, 0);

		public Opend(){
			Text = "Tic Tac Toe";
			MouseDown += onMouseDown;
			Paint += onPaint;
			Init();
		}

		void InitializeBoxLabel(){
			Controls.Add( Text_Box );
			Label_.Text = "Choose the size of the board from 3";
			Controls.Add(Label_);
		}

		void TextBoxKeyPress(object sender, KeyPressEventArgs e){
			
			if (e.KeyChar == (char)Keys.Enter){
				if (Text_Box.Text!=""){
					if (int.Parse(Text_Box.Text) >= 3){
						t.size =int.Parse(Text_Box.Text); 
						if (t.size > 5) t.winscore = 5;
						else t.winscore = t.size;
						t.board = new Square [ t.size, t.size ];
						Controls.Remove(Text_Box); 
						ChoosingLevel();
					} 
				else 
					MessageBox.Show("Wrong size"); 
				Text_Box.Clear();
				}
			}
			else if ((e.KeyChar==8)&&(Text_Box.Text.Length>0))
					Text_Box.Text = Text_Box.Text.Substring(0, Text_Box.Text.Length - 1);
				else if (!(e.KeyChar >= 48 && e.KeyChar <= 57) ){
					e.Handled = true;
					MessageBox.Show("Incorrect character");
				}
				
		}

		private void ChoosingLevel(){
			Label_.Text = "Choose who you want to play with:";
			InitializeButtons();
			FirstButton.Click += new EventHandler(ButtonFirstClick);
			SecondButton.Click += new EventHandler(ButtonSecondClick);
		}

		private void InitializeButtons(){
			Controls.Add(FirstButton);
			Controls.Add(SecondButton);
		}

		private void ButtonFirstClick(object sender, System.EventArgs e){
			level = 1;
			if (t.size>=5) t.combinations = new Pair [t.winscore + 2];
			else t.combinations = new Pair [t.winscore + 1];
			t.CreateCombinations(t.winscore);
			Controls.Remove(FirstButton);
			Controls.Remove(SecondButton);
			Controls.Remove(Label_); 
		}

		private void ButtonSecondClick(object sender, System.EventArgs e){
			level = 2;
			Controls.Remove(FirstButton);
			Controls.Remove(SecondButton);
			Controls.Remove(Label_); 
		}

		private void Init(){
			t = new Hidden();
			ClientSize = new Size(400, 400);
			StartPosition = FormStartPosition.CenterScreen;
			level = 0;
			t.winner = 0;
			InitializeBoxLabel();
			DoubleBuffered = true;
			Label_.Width = 350;
			Text_Box.KeyPress += new KeyPressEventHandler(TextBoxKeyPress);
			Invalidate();
			t.changed += Refresh;
		}

		private void onMouseDown(object sender, MouseEventArgs args){
			if (t.winner > 0)
				Init();
			else if (level != 0){
				int x = ((int)(args.X-50))/casesize;
				int y = ((int)(args.Y-50))/casesize;
				if ((x >= 0 && x < t.size) && (y >= 0 && y < t.size))
					t.PersonsStep(x, y, level);
			}
		}

		private void WinMessage(){

			switch (t.winner){
				case 1:
					 if (level==1) 
						Label_.Text = "You win!";
					else Label_.Text = Enum.GetName(typeof(Square), t.winner)+", you win!";
				break;
				case 2:
					if (level == 1) 
						Label_.Text ="You lose!";
					else Label_.Text = Enum.GetName(typeof(Square), t.winner)+", you win!";
				break;
				case 3:
					Label_.Text = "Draw";
				break;
			}
	
			Controls.Add(Label_);
		}

		private void onPaint(object sender, PaintEventArgs arg){
			if (level != 0)
				Game(arg.Graphics);
		}

		private void Game(Graphics g){
			if (t.size >20){
				int width = Screen.GetWorkingArea(this).Width;
				int height = Screen.GetWorkingArea(this).Height;
				ClientSize = new Size(width, height);
				casesize=(height-100)/t.size;
				Label_.Width = height;
			}
			else casesize=300/t.size;
			length = casesize*t.size;
			g.TranslateTransform(50, 50);
			
			g.DrawRectangle(new Pen(Color.Red, 0), 0, 0, length, length);
			for (int i = casesize; i < length; i += casesize){
				g.DrawLine(p, i, 0, i, length);
				g.DrawLine(p, 0, i, length, i);
			}
			for (int x = 0; x < t.size; ++x)
				for (int y = 0; y < t.size; ++y){
					switch (t.board[x, y]){
						case Cross: drawCross(g, x, y); break;
						case Nought: drawNought(g, x, y); break;
					}
				}
				
				if (t.winner == 0)
				this.Invalidate();
				if (t.winner > 0)
				WinMessage();
			
		}

		private void drawNought(Graphics g, int y, int x){
			g.DrawEllipse(p, x*casesize , y*casesize , casesize, casesize);
		}

		private void drawCross(Graphics g, int y, int x){
			g.DrawLine(p, x*casesize, y*casesize, x*casesize+casesize, y*casesize+casesize);
			g.DrawLine(p, x *casesize+casesize, y *casesize, x *casesize, y*casesize+casesize);
		}

		public static void Main(){
			Application.EnableVisualStyles();
			Application.Run(new Opend());
		}
	}
}

