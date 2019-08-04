using System;
using static System.Math;
using System.Drawing;
using System.Windows.Forms;
using static System.Console;

using static Square;
public enum Square{ 
	Cross=1,
	Nought=2,
	Empty=0
}
public delegate void Call();

public class TicTacToe {
	public event Call changed;
	public Square [,] board = new Square [3, 3];
	private Square sym;
	private int player = 1;
	public int winner = 0, wx1 = 0, wy1 = 0, wx2 = 0, wy2 = 0; // winner and coordinates 
	private int count;
	private int [] combinations = new int[] {2, 2, 2, 1, 1, 2, 3, 0 };//2 Nought, 2 Cross, 1 Nought, 3 Empty ( which means in line - 2 Noughts => 1 Empty, etc.)

	private void ComputerStep(){
	
		for ( int i = 0; i < combinations.Length; i += 2 )//checking every single combinations, where computer need to pay attention
			if ( LineDivision( combinations[i], combinations[i + 1]) ){// calling the submethod
				if ( Win(player) ){}//checking after a step of the computer 
				return;
			}

		Random rand = new Random(); //create random coordinates when did't find important combinations
		while ( true ){
			int x = rand.Next(3);
			int y = rand.Next(3);
			if ( CanPlace(x, y) ){ // checking if can place 
				board[x, y] = Nought;
				if ( Win(player) )//checking after a step of the computer 
					{} 
				return;
			}
		}
	}

	private bool LineDivision (int c, int symb){//divide the board into the lines (rows, columns, diagonals)
		//calling submethod to check, if the line is our combination 
	
		count = c;
		sym = (Square)symb;

		for ( int i = 0; i < 3; ++i ){//rows and colomns
			if ( CheckCombination(i, 0, 0, 1) ) return true;
			else count = c;
			if ( CheckCombination(0, i, 1, 0) ) return true;
			else count = c;
		}

		if ( CheckCombination(0, 0, 1, 1) )  return true;//first diagonal
			else count = c;

		if ( CheckCombination(0, 2, 1, -1) ) return true;//second diagonal
		else count = c;

		return false;
	
	}

	private bool CheckCombination ( int x, int y, int x_e, int y_e){
	
		for ( int j = 0; j < 3; ++j ){//comparing combination and line
			if ( board[x + j*x_e, y + j*y_e] == sym ) --count;
			else if ( board[x + j*x_e, y + j*y_e] != Empty ) return false;
		}

		Random r = new Random(); // use random, to choose empty space in a line 
		if ( count == 0 ){
			while ( true ){
				int j = r.Next(3);
				if ( CanPlace(x + j*x_e, y + j*y_e) ){
					board[x + j*x_e, y + j*y_e] = Nought;
					return true;
				}
			}
		}
		return false;
	
	}

	private bool CanPlace ( int x, int y ){
	
		if ( board[x, y] == Empty ) return true;
		else return false;
	
	} 

	private bool Win ( int player ){
	
		for ( int i = 0; i < 3; ++i)//rows and colomns
			if ( (CheckLine(i, 0, 0, 1) || CheckLine(0, i, 1, 0) ) ){
				winner = player;
				return true;
			}

		if  ( (CheckLine(0, 0, 1, 1) || CheckLine(0, 2, 1, -1) ) ){//diagonals
			winner = player;
			return true;
		}

		if ( IsFull() ){
			winner = 3;
			return true;
		}

		return false;
	}

	private bool CheckLine ( int x, int y, int x_e, int y_e ){
		
		for ( int j = 0; j < 3; ++j )
			if ( (int)board[x + j*x_e, y + j*y_e] != player ) return false;

		wx2 = x + x_e;//saving coordinates of the second point in the line
		wy2 = y + y_e;
		wx1 = x;
		wy1 = y;
		return true;
	}

	private bool IsFull (){
	
		for ( int i = 0; i < 3; ++i )
			for ( int j = 0; j < 3; ++j )
				if ( board[i, j] == Empty ) return false;
		return true;
	}

	public bool PersonsStep ( int y, int x, int level ){//checking person's step and 
		//control the game depending on the level
	
		if ( CanPlace(x, y) ){
			board[x, y] = (Square)player;
			changed();
			if ( !Win(player) ){
				player = 3 - player;
				if (level == 1){
					ComputerStep(); 
					player = 3 - player;
				}
			}
			changed();
			return true;
		}
		else return false;
	}

}

class View : Form{

	TicTacToe t;
	private Pen p = new Pen(Color.Black,0);
	private Font drawFont = new Font("Comic Sans MS", 0.75f);
	private StringFormat drawFormat = new StringFormat();
	private SolidBrush drawBrush = new SolidBrush(Color.Black);
	bool flag = true;
	int level = 0;
	private Button button1 = new Button();
	private Button button2 = new Button();
	
	View(){
		Text = "Tic Tac Toe";
		StartPosition = FormStartPosition.CenterScreen;
		MouseDown += onMouseDown;
		Paint += onPaint;
		DoubleBuffered = true;
		ClientSize = new Size(400,400);
		init();
	}

	void init(){
		t = new TicTacToe();
		Invalidate();
		t.changed += Invalidate;
	}

	void onMouseDown ( object sender, MouseEventArgs args ) {
	
		if (flag){
			Invalidate();
			flag = false;
		}
		else{
			if ( t.winner > 0 ){
				level = 0;
				init(); //update if it's the end of the game
			} 
			else{
				int x = ((int) (args.X + 50 )) / 100 - 1;
				int y = ((int) (args.Y + 50 )) / 100 - 1;
				if ( ( x >= 0 && x <= 2) && ( y >= 0 && y <=2 ) ) 
					t.PersonsStep( x, y, level );
				}
		}
	}

	void onPaint (object sender, PaintEventArgs arg){
	
		Graphics g = arg.Graphics;

		g.TranslateTransform(50, 50);
		g.ScaleTransform(25,25);
		g.FillRectangle( Brushes.Ivory, -2, -3, 100, 100 );

		if ( flag )
			Instructions( arg.Graphics );
		else if ( level != 0 )
				Game (arg.Graphics);
			else ChoosingLevel(arg.Graphics);
	
}

	void Instructions ( Graphics g ){
	
		string welcome = @"Welcome! ";
		string name = @"This is the ""Tic Tac Toe"" game. ";
		string rules_1 = @"There are some rules before the game started:";
		string rules_2 = @" 1. The game is played on a grid that's 3 squares by 3 squares.		
							 2. You are X, your friend (or the computer in this case) is O.
							     Players take turns putting their marks in empty squares.
							 3. The first player to get 3 of her marks in a row 
							     (up, down, across, or diagonally) is the winner.
							 4. When all 9 squares are full, the game is over. 
							     If no player has 3 marks in a row, 
							     the game ends in a tie.";
		string cont = @"*To continue click somewhere";

		g.DrawString ( welcome, drawFont, new SolidBrush(Color.Turquoise), 4, -1.5f, drawFormat );
		g.DrawString ( name, drawFont, new SolidBrush(Color.LimeGreen), -1, 0, drawFormat );
		g.DrawString ( rules_1, new Font("Arial Black", 0.41f), drawBrush, -0.75f, 2, drawFormat );
		g.DrawString ( rules_2, new Font("Common serif fonts", 0.35f), drawBrush, -0.70f, 3, drawFormat );
		g.DrawString ( cont, new Font("Comic Sans MS", 0.35f), new SolidBrush(Color.Red), 7, 12, drawFormat );
	
	}

	void ChoosingLevel ( Graphics g){
	
		string choose = @"                     Choose 
		       who you want to play with:";

		g.DrawString ( choose, drawFont, new SolidBrush(Color.Black), -1.73f, -1, drawFormat );
		g.DrawString ( choose, drawFont, new SolidBrush(Color.Magenta), -1.75f, -1, drawFormat );

		InitializeButtons();
		button1.Click += new EventHandler( Button1Click );
		button2.Click += new EventHandler( Button2Click );
		Invalidate();
	}

	private void InitializeButtons(){
	
		button1.Location = new System.Drawing.Point(75, 125);
		button1.Size = new System.Drawing.Size(250,60);
		button1.Text = "With a computer";
		button1.BackColor = Color.DarkOrchid;
		button1.FlatStyle = FlatStyle.Popup;
		button1.ForeColor = Color.Black;
		button1.Font = new Font("Comic Sans MS", 13f);

		button2.Location = new System.Drawing.Point(75, 225);
		button2.Size = new System.Drawing.Size(250,60);
		button2.Text = "With a friend";
		button2.BackColor = Color.DarkOrchid;
		button2.FlatStyle = FlatStyle.Popup;
		button2.ForeColor = Color.Black;
		button2.Font = new Font("Comic Sans MS", 13f);

		Controls.Add( button1 );
		Controls.Add( button2 );
	
	}

	private void Button1Click ( object sender, System.EventArgs e ){
		level = 1;
		Controls.Remove( button1 );
		Controls.Remove( button2 );
	}

	private void Button2Click ( object sender, System.EventArgs e ){
		level = 2;
		Controls.Remove( button1 );
		Controls.Remove( button2 );
	}

	void Game ( Graphics g ){
	
		if ( t.winner > 0 && t.winner < 3){
			g.FillRectangle( new SolidBrush(Color.Yellow), t.wy1*4, t.wx1*4, 4, 4); 
			g.FillRectangle( new SolidBrush(Color.Yellow), t.wy2*4, t.wx2*4, 4, 4); 
			g.FillRectangle( new SolidBrush(Color.Yellow), (t.wy2 + (t.wy2 - t.wy1))*4, ( t.wx2 + (t.wx2 - t.wx1))*4, 4, 4); 
		}

		int s = 0;
		g.DrawRectangle( new Pen(Color.Red, 0), 0, 0, 12, 12);
		for ( float i = 4f; i <= 8f; i += 4 ){
			g.DrawLine( p, i, 0, i, 12 );
			g.DrawLine( p, 0, i, 12, i );
		}

		 for (int x = 0; x < 3; ++x)
			for (int y = 0; y < 3; ++y) {
				switch ( t.board[x, y] ){
					case Cross: drawCross(g, x, y); break;
					case Nought: drawNought(g, x, y); break;
					case Empty: ++s; break;
				}
			}

		if (s == 9){
			if ( level == 1 ){
				string level1 = @"This level is for playing with a computer. The first player will be X. 
					                      The second player (computer) will be O.";
				g.DrawString ( level1, new Font("Comic Sans MS", 0.35f), new SolidBrush(Color.DarkBlue), -0.75f, -2.25f, drawFormat );
			}
			else{
				string level2 = @"This level is with two players. The first player will be X.
					                        The second player will be O.";
				g.DrawString ( level2, new Font("Comic Sans MS", 0.35f), new SolidBrush(Color.DarkBlue), 0, -2.25f, drawFormat );
			}
		}
	
	}

	void drawNought ( Graphics g, int y, int x ){
		g.DrawEllipse (p, x*4+0.5f, y*4+0.5f, 3, 3);
	}

	void drawCross ( Graphics g, int y, int x ){
		g.DrawLine (p, x*4+0.5f, y*4+0.5f, x*4+3+0.5f, y*4+3+0.5f);
		g.DrawLine (p, x*4+0.5f, y*4+3+0.5f, x*4+3+0.5f, y*4+0.5f);
	}

	static void Main(){
		Application.Run(new View());
	}
}
