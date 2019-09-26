using System;
using static System.Math;
using static Square;

public delegate void Call();

namespace TicTacToe{

	public partial class Hidden{

		public event Call changed;

		public int size=0; //size of the board 
		public int winscore; //quantity symbols in line for win
		private int player = 1; //player turn 
		public int winner = 0; //winner

		public bool PersonsStep(int y, int x, int level){
			//checking person's step and 
			//control the game depending on the level

			if (CanPlace(x, y)){

				board[x, y] = (Square)player;
				changed();
				if (!Win(player)){
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

		private bool CanPlace(int x, int y){

			if (board[x, y] == Empty) return true;
			else return false;

		}

		private bool Win(int player){//check the win

			for (int i = 0; i < size; ++i) //rows and colomns
				if ((WinInLine(i, 0, 0, 1) || WinInLine(0, i, 1, 0))){
					winner = player;
					return true;
				}

			if (WinInDiagonal()){//diagonals
				winner = player;
				return true;
			}

			if (IsFull()){//if the board is full
				winner = 3;
				return true;
			}

			return false;
		}

		private bool WinInLine(int x, int y, int x_e, int y_e){

			if (size <= 5){
				for (int j = 0; j < size; ++j)
					if ((int)board[x + j * x_e, y + j * y_e] != player)
						return false;
			}
			 else{
				int k=0;
				for (int i = 0; i < size;++i){
					if (k == 5) break;
					if ((int)board[x + i * x_e, y + i * y_e] == player) 
						++k;
					else k=0;
					if (k+size-i < 5) 
					return false;
				} 
			}
			return true;
		}

		private bool WinInDiagonal(){

			for ( int i =0; i <= size - winscore; ++i )
				for ( int j = 0; j <= size - winscore; ++j ){
					int s = 0;
					while (s != winscore){
						if ((int)board[i + s, j + s] != player) 
							break;
						else ++s;
					}
					if ( s == winscore ) 
						return true;
					s = 0;
					while ( s != winscore ){
						if ((int)board[i + s, j + winscore -1 -s ] != player) 
							break;
						else ++s;
					}
					if ( s==winscore)  
						return true;
				}
			return false;
		}

		private bool IsFull(){

			for (int i = 0; i < size; ++i)
				for (int j = 0; j < size; ++j)
					if (board[i, j] == Empty)
						return false;
			return true;
		}
	}
}
