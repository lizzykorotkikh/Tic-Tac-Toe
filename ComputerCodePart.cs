using System;
using static System.Math;
using static Square;

public enum Square{
	 Cross = 1,
	Nought = 2,
	Empty = 0
}

public struct Pair
{
	public int count;
	public Square symbol;

	public Pair(int c, Square s)	{
		count = c;
		symbol = s;
	}
}

namespace TicTacToe{

	public partial class Hidden{

		public Square[,] board;
		private Square sym;
		public Pair [] combinations;

		public void CreateCombinations( int winscore ){//combinations for analysis
			int j;
			combinations[ 0 ] = new Pair ( winscore - 1, Nought );
			combinations[ 1 ] = new Pair ( winscore - 1, Cross );
			if (size>=5) {combinations[ 2 ] = new Pair ( 3, Cross ); j = 3;}
				else j=2;
			for ( int i = j; i <= winscore - 1; ++i ){
				combinations[ i ] = new Pair( winscore - i, Nought);
			}
			
			combinations[ winscore ] = new Pair ( winscore, Empty );
		}//it means in line - 2 Noughts => 1 Empty, etc.*/

		private void ComputerStep(){

			for (int i = 0; i < combinations.Length; ++i) //checking every single combinations, where computer need to pay attention
				if (LineDivision( combinations[i].count, combinations[i].symbol)){
					// calling the submethod
					if (Win(player)){
					} //checking after a step of the computer 
				return;
				}

			Random rand = new Random(); //create random coordinates when did't find important combinations
			while (true){
				int x = rand.Next(size);
				int y = rand.Next(size);
				if (CanPlace(x, y)){
					// checking if can place 
					board[x, y] = Nought;
					if (Win(player)){ 
					}//checking after a step of the computer
					return;
				}
			}
		}

		private bool LineDivision( int count, Square symb ){
			//divide the board into the lines (rows, columns, diagonals)
			//calling submethod to check, if the line is our combination 
			sym = symb;
			
			for (int i = 0; i < size; ++i){
				//rows and colomns
				if (CheckLines(i, 0, 0, 1, count)) return true;
				if (CheckLines(0, i, 1, 0, count)) return true;
				if (CheckDiagonals( i, count )) return true; //diagonals
			}
			return false;
		}

		private bool CheckLines( int x, int y, int x_e, int y_e, int count ){
			int c = count;
			bool flag = true;
			int i = 0;
			//comparing combination and line
			while (i <= size - winscore){
				for (int j = i; j < i + winscore; ++j){
					if (board[x + j * x_e, y + j * y_e] == sym) 
						--c;
					else if (board[x + j * x_e, y + j * y_e] != Empty){
						flag = false; 
						break;
					}
				}
				if (( flag ) && (c == 0)) 
					break; 
				else 
					flag = true;
				++i;
				c = count;
			}
			
			Random r = new Random(); // use random, to choose empty space in a line 
			if (c == 0){
				while (true)	{
					int j = r.Next(i, i + winscore);
					if (CanPlace(x + j * x_e, y + j * y_e))	{
						board[x + j * x_e, y + j * y_e] = Nought;
						return true;
					}
				}
			}
			return false;
		}

		private bool CheckDiagonals ( int i, int count ){
			int c = count;
			int q = 0;
			Random r = new Random();
			bool flag = true;
			
			if (i <= size - winscore){//from left to right
				for (int j= 0; j <= size - winscore; ++j){
					int s=0;
					while (s != winscore){
						if (board[i+s,j+s] == sym) 
							--c;
						else if (board[i+s,j+s] != Empty){
							flag = false; 
							break;
						}
						++s;
					}
					if (( flag ) && (c == 0)){
						q = j;
						break;
					}
					else flag = true;
					c = count;
				}
			}
			
			if (c == 0){
				while (true){
					 int j = r.Next(winscore);
					if (i <= size - winscore){
						Console.WriteLine(j+" "+i+" "+q);
						if (CanPlace(i + j, q + j)){
							board[i + j, q + j] = Nought;
							return true;
						}
					}
				}
			}

			if (( i >= winscore - 1) && (c != 0)){//from right to left
				for (int j = 0; j <= size - winscore; ++j){
					int s = 0;
					while (s != winscore){
						if (board[i-s,j+s] == sym) 
							--c;
						else if (board[i - s,j + s] != Empty){
							flag = false; 
							break;
						}
						++s;
					}
					if (( flag ) && (c == 0)){
						q = j;
						break;
					}
					else flag = true;
					c = count;
				}
			}
			
			// use random, to choose empty space in a line 
			if (c == 0){
				while (true){
					int j = r.Next(winscore);
					if (i >= winscore - 1){
						//Console.WriteLine(j+" "+i+" "+q);
						if (CanPlace(i - j, q + j)){
							board[i - j, q + j] = Nought;
							return true;
						}
					}
				}
			}
			return false;
		}

	}

}
