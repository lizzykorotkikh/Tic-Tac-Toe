using System;
using static System.Math;
using static Square;
public enum Square
{
     Cross = 1,
    Nought = 2,
    Empty = 0
}
struct Pair
{
    public int count;
    public Square symbol;

    public Pair(int c, Square s)
    {
        count = c;
        symbol = s;
    }
}

public struct Coordiantes 
{
 public int x;
  public int y;
  public Coordiantes(int a, int b)
    {
        x = b;
        y = a;
    }
  
}


    public delegate void Call();

namespace TicTacToe
{
		
    public class Hidden
    {
		//coordinates of winner's line
			public Coordiantes c1;
			public Coordiantes c2;
			public Coordiantes c3;

        public event Call changed;
        public Square[,] board = new Square[3, 3];
        private Square sym;
        private int player = 1;
        public int winner = 0; // winner

        private Pair[] combinations = new Pair[]
        {
            new Pair(2, Nought),
            new Pair(2, Cross),
            new Pair(1, Nought),
            new Pair(3, Empty)
        }; // means in line - 2 Noughts => 1 Empty, etc.

        private void ComputerStep()
        {

            for (int i = 0;
                i < combinations.Length;
                ++i) //checking every single combinations, where computer need to pay attention
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
                int x = rand.Next(3);
                int y = rand.Next(3);
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

        private bool LineDivision(int count, Square symb)
        {
            //divide the board into the lines (rows, columns, diagonals)
            //calling submethod to check, if the line is our combination 
            sym = symb;
            for (int i = 0; i < 3; ++i)
            {
                //rows and colomns
                if (CheckCombination(i, 0, 0, 1, count)) return true;
                if (CheckCombination(0, i, 1, 0, count)) return true;
            }

            if (CheckCombination(0, 0, 1, 1, count)) return true; //first diagonal

            if (CheckCombination(0, 2, 1, -1, count)) return true; //second diagonal

            return false;
        }

        private bool CheckCombination(int x, int y, int x_e, int y_e, int count)
        {
            int c = count;
            for (int j = 0; j < 3; ++j)
            {
                //comparing combination and line
                if (board[x + j * x_e, y + j * y_e] == sym) --c;
                else if (board[x + j * x_e, y + j * y_e] != Empty) return false;
            }

            Random r = new Random(); // use random, to choose empty space in a line 
            if (c == 0)
            {
                while (true)
                {
                    int j = r.Next(3);
                    if (CanPlace(x + j * x_e, y + j * y_e))
                    {
                        board[x + j * x_e, y + j * y_e] = Nought;
                        return true;
                    }
                }
            }

            return false;
        }

        private bool CanPlace(int x, int y)
        {

            if (board[x, y] == Empty) return true;
            else return false;

        }

        private bool Win(int player)
        {

            for (int i = 0; i < 3; ++i) //rows and colomns
                if ((CheckLine(i, 0, 0, 1) || CheckLine(0, i, 1, 0)))
                {
                    winner = player;
                    return true;
                }

            if ((CheckLine(0, 0, 1, 1) || CheckLine(0, 2, 1, -1)))
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

        private bool CheckLine(int x, int y, int x_e, int y_e)
        {

            for (int j = 0; j < 3; ++j)
                if ((int)board[x + j * x_e, y + j * y_e] != player)
                    return false;
//saving coordinates 
 c1 = new Coordiantes(x,y);
 c2 = new Coordiantes(x+x_e,y+y_e);
 c3 = new Coordiantes( 2*c2.y-c1.y, 2*c2.x-c1.x);

            return true;
        }

        private bool IsFull()
        {

            for (int i = 0; i < 3; ++i)
                for (int j = 0; j < 3; ++j)
                    if (board[i, j] == Empty)
                        return false;
            return true;
        }

        public bool PersonsStep(int y, int x, int level)
        {
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
