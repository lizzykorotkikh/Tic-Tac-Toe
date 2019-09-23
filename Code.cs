using System;
using static System.Math;
using static Square;
public enum Square
{
	 Cross = 1,
	Nought = 2,
	Empty = 0
}
public struct Pair
{
	public int count;
	public Square symbol;

	public Pair(int c, Square s)
	{
		count = c;
		symbol = s;
	}
}

public delegate void Call();

namespace TicTacToe
{
		
	public class Hidden
	{
		
		public int size=0;
		public int winscore;
		public event Call changed;
		public Square[,] board;
		private Square sym;
		private int player = 1;
		public int winner = 0; // winner
		public Pair [] combinations;
		
		public void CreateCombinations(int winscore){
			
			combinations[0]=new Pair (winscore-1, Nought);
			combinations[1] = new Pair ( winscore-1, Cross);
			for (int i=2; i<=winscore-1; ++i){
				combinations[i]=new Pair(winscore-i, Nought);
			}
			combinations[winscore]=new Pair (winscore, Empty);
			}
			
			//it means in line - 2 Noughts => 1 Empty, etc.*/
		
		
		
		private void ComputerStep(){
			

			for (int i = 0; i < combinations.Length; ++i) //checking every single combinations, where computer need to pay attention
				if (LineDivision(combinations[i].count, combinations[i].symbol))
				{
					// calling the submethod
					if (Win(player))
					{
					} //checking after a step of the computer 

					return;
				}

			Random rand = new Random(); //create random coordinates when did't find important combinations
			while (true)
			{
				int x = rand.Next(size);
				int y = rand.Next(size);
				if (CanPlace(x, y))
				{
					// checking if can place 
					board[x, y] = Nought;
					if (Win(player)) //checking after a step of the computer 
					{
					}
					return;
				}
			}
		}

		private bool LineDivision(int count, Square symb){
			//divide the board into the lines (rows, columns, diagonals)
			//calling submethod to check, if the line is our combination 
			sym = symb;
			for (int i = 0; i < size; ++i)
			{
				//rows and colomns
				if (CheckLines(i, 0, 0, 1, count)) return true;
				if (CheckLines(0, i, 1, 0, count)) return true;
			   if (CheckDiagonals(i,count)) return true; //diagonals

			}
			return false;
		}

		private bool CheckLines(int x, int y, int x_e, int y_e, int count){
			int c = count;
			bool flag = true;
			int i =0;
			//comparing combination and line
			while (i<=size-winscore){
				for (int j = i; j < i+winscore; ++j){
				
					if (board[x + j * x_e, y + j * y_e] == sym) --c;
					else if (board[x + j * x_e, y + j * y_e] != Empty) {flag = false; break;}
				}
				if ((flag)&&(c==0)) break; 
				else flag=true;
				++i;
				c=count;
			}
			
			Random r = new Random(); // use random, to choose empty space in a line 
			if (c == 0)
			{
				while (true)
				{
					int j = r.Next(i, i+winscore);
					if (CanPlace(x + j * x_e, y + j * y_e))
					{
						board[x + j * x_e, y + j * y_e] = Nought;
						return true;
					}
				}
			}

			return false;
		}

		private bool CanPlace(int x, int y){

			if (board[x, y] == Empty) return true;
			else return false;

		}
		
		private bool CheckDiagonals (int i, int count){
			int c = count;
			int q=0;
			Random r = new Random();
			bool flag=true;
			
			if (i<=size-winscore){
				for (int j= 0; j<= size-winscore;++j){
					int s=0;
					while (s!=winscore){
						if (board[i+s,j+s] == sym) --c;
						else if (board[i+s,j+s] != Empty) {
							flag = false; 
							break;
							}
						++s;
					}
					if ((flag)&&(c==0)) {q=j; break;}
					else flag=true;
					c=count;
					
				}
			}
			if ((i>=winscore-1)&&(c!=0)){
				for (int j= 0; j<= size -winscore;++j){
					int s=0;
					while (s!=winscore){
						if (board[i-s,j+s] == sym) --c;
						else if (board[i-s,j+s] != Empty) {flag = false; break;}
						++s;
					}
					if ((flag)&&(c==0)) {q=j;break;}
					else flag=true;
					c=count;
				}			
			}
			
			// use random, to choose empty space in a line 
			if (c == 0)
			{
				while (true)
				{
					int j = r.Next(winscore);
					if (i<=size-winscore){
						if (CanPlace(i+j, q+j)){
							board[i+j, q+j] = Nought;
							return true;
						}
					}
					else if (i>=winscore-1){
						if (CanPlace(i-j, q+j)){
							board[i-j, q+j] = Nought;
							return true;
						}
					}
				}
			}

			return false;
		}
		
		private bool WinDiagonals(){
			
			for (int i =0; i<= size -winscore;++i)
				for (int j= 0; j<= size -winscore;++j){
					int s=0;
					while (s!=winscore){
						if ((int)board[i+s,j+s]!=player) break;
						else ++s;
					}
					if (s==winscore) 
								return true;//remind coordinates and return true}
					
					s=0;
					while (s!=winscore){
						if ((int)board[i+s,j+winscore -1-s]!=player) break;
						else ++s;
					}
					if (s==winscore)  
								return true;
			}
			return false;
		}
		
		private bool Win(int player){

			for (int i = 0; i < size; ++i) //rows and colomns
				if ((WinLine(i, 0, 0, 1) || WinLine(0, i, 1, 0)))
				{
					winner = player;
					return true;
				}

			if (WinDiagonals())
			{
				//diagonals
				winner = player;
				return true;
			}

			if (IsFull())
			{
				winner = 3;
				return true;
			}

			return false;
		}

		private bool WinLine(int x, int y, int x_e, int y_e){
			if (size<=5){
			for (int j = 0; j < size; ++j)
				if ((int)board[x + j * x_e, y + j * y_e] != player)
					return false;
					}
			 else {
				 int k=0;
				 for (int i = 0; i < size;++i){
					 if (k==5) break;
					 if ((int)board[x + i * x_e, y + i * y_e] == player) ++k;
					 else k=0;
					 if (k+size-i<5) return false;
					 } 
				 }

			return true;
		}

		private bool IsFull(){//done

			for (int i = 0; i < size; ++i)
				for (int j = 0; j < size; ++j)
					if (board[i, j] == Empty)
						return false;
			return true;
		}

		public bool PersonsStep(int y, int x, int level){
			//checking person's step and 
			//control the game depending on the level

			if (CanPlace(x, y))
			{
				board[x, y] = (Square)player;
				changed();
				if (!Win(player))
				{
					player = 3 - player;
					if (level == 1)
					{
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
}
