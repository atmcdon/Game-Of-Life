using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Game_Of_Life
{
    // singleton class that controls and "plays" the game/sim
    internal class Life
    {

        private const char DEAD = '-';
        private const char LIVE = '@';
        //This will allow this ti be a const but just not need to 
        // have a value when inits
        private readonly int MaxRows;
        private readonly int MaxColumns;

        //Show char for console app
        // storage for the 2 game boards.
        private char[,] displayBoard;
        private char[,] resultsBoard;

        Stopwatch watch;

        public Life(int rows, int cols) {
            watch = new Stopwatch();
            MaxRows = rows;
            MaxColumns = cols;
            
            //allocate storage for the 2 game boards
            displayBoard = new char[MaxRows,MaxColumns];
            resultsBoard = new char[MaxRows,MaxColumns];

            //init game board
            InitializeGameBoard(displayBoard);
            InitializeGameBoard(resultsBoard);

            // choose a start state for an interseting "game"
            //place starting configuration
            ConfiguredStartingPattern(MaxRows / 2, MaxColumns / 3);


        }

        public void Play(int numGenerations) 
        {
            watch.Start();
            for (int i = 1; i <= numGenerations; i++) 
            {
                // 1 - print game board (display to the user)
                PrintGameBoard(i);

                // 2 - apply the rules of the game
                // process the (current) game board and store the results in the resultsBoard
                ProcessMainGameBoard();
                

                // 3 - swap the two boards to prepare to start over for the next generation.
                SwapGameBoards ();
            }
            watch.Stop();
            Console.WriteLine($"Elapsed Time{watch.Elapsed}");
        }

        private void ProcessMainGameBoard()
        {
            for (int r = 0; r < MaxRows; r++)
            {
                for (int c = 0; c < MaxColumns; c++)
                {
                    //apply game rules
                    resultsBoard[r, c] = ApplyGameRules(r, c);

                }


            }
        }

        private char ApplyGameRules(int r, int c)
        {
            //Try to clearify the game rules
            /*Any live cell with fewer than two live neighbors dies, as if by underpopulation.
             *              If live and < 2 lives DEAD
             * 
              Any live cell with two or three live neighbors lives on to the next generation.
                            if live cell == 2 || 3 LIVE
              Any live cell with more than three live neighbors dies, as if by overpopulation.
                            LIVE > three neighbors live neighbors died
              Any dead cell with exactly three live neighbors becomes a live cell, as if by reproduction.*/
            int neighborCount = CountNeighborCells(r, c);
            if (neighborCount == 2)
            {
                return displayBoard[r, c];
            }
            else if (neighborCount == 3)
            {
                return LIVE;
            }
            /*else if (neighborCount > 3)
            {
                return DEAD;
            }*/



            return DEAD;
        }

        // First Take - brute force checking r,c for each exceptional condition == 8
        private int CountNeighborCells(int r, int c)
        {
            int neighbors = 0;
            if (r == 0 && c == 0)
            {
                //if (displayBoard[r - 1, c - 1] == LIVE) neighbors++;
                //if (displayBoard[r - 1, c] == LIVE) neighbors++;
                //if (displayBoard[r - 1, c + 1] == LIVE) neighbors++;
                //if (displayBoard[r, c - 1] == LIVE) neighbors++;
                if (displayBoard[r, c + 1] == LIVE) neighbors++;
                //if (displayBoard[r + 1, c - 1] == LIVE) neighbors++;
                if (displayBoard[r + 1, c] == LIVE) neighbors++;
                if (displayBoard[r + 1, c + 1] == LIVE) neighbors++;
            }
            else if (r == 0 && c == MaxColumns - 1)
            {
                //if (displayBoard[r - 1, c - 1] == LIVE) neighbors++;
                //if (displayBoard[r - 1, c] == LIVE) neighbors++;
                //if (displayBoard[r - 1, c + 1] == LIVE) neighbors++;
                if (displayBoard[r, c - 1] == LIVE) neighbors++;
                //if (displayBoard[r, c + 1] == LIVE) neighbors++;
                if (displayBoard[r + 1, c - 1] == LIVE) neighbors++;
                if (displayBoard[r + 1, c] == LIVE) neighbors++;
                //if (displayBoard[r + 1, c + 1] == LIVE) neighbors++;
            }
            else if (r == MaxRows - 1 && c == MaxColumns - 1)
            {
                if (displayBoard[r - 1, c - 1] == LIVE) neighbors++;
                if (displayBoard[r - 1, c] == LIVE) neighbors++;
                //if (displayBoard[r - 1, c + 1] == LIVE) neighbors++;
                if (displayBoard[r, c - 1] == LIVE) neighbors++;
                //if (displayBoard[r, c + 1] == LIVE) neighbors++;
                //if (displayBoard[r + 1, c - 1] == LIVE) neighbors++;
                //if (displayBoard[r + 1, c] == LIVE) neighbors++;
                //if (displayBoard[r + 1, c + 1] == LIVE) neighbors++;
            }
            else if (r == MaxRows - 1 && c == 0)
            {
                //if (displayBoard[r - 1, c - 1] == LIVE) neighbors++;
                if (displayBoard[r - 1, c] == LIVE) neighbors++;
                if (displayBoard[r - 1, c + 1] == LIVE) neighbors++;
                //if (displayBoard[r, c - 1] == LIVE) neighbors++;
                if (displayBoard[r, c + 1] == LIVE) neighbors++;
                //if (displayBoard[r + 1, c - 1] == LIVE) neighbors++;
                //if (displayBoard[r + 1, c] == LIVE) neighbors++;
                //if (displayBoard[r + 1, c + 1] == LIVE) neighbors++;
            }
            else if (r == 0)
            {
                //if (displayBoard[r - 1, c - 1] == LIVE) neighbors++;
                //if (displayBoard[r - 1, c] == LIVE) neighbors++;
                //if (displayBoard[r - 1, c + 1] == LIVE) neighbors++;
                if (displayBoard[r, c - 1] == LIVE) neighbors++;
                if (displayBoard[r, c + 1] == LIVE) neighbors++;
                if (displayBoard[r + 1, c - 1] == LIVE) neighbors++;
                if (displayBoard[r + 1, c] == LIVE) neighbors++;
                if (displayBoard[r + 1, c + 1] == LIVE) neighbors++;
            }
            else if (r == MaxRows - 1)
            {
                if (displayBoard[r - 1, c - 1] == LIVE) neighbors++;
                if (displayBoard[r - 1, c] == LIVE) neighbors++;
                if (displayBoard[r - 1, c + 1] == LIVE) neighbors++;
                if (displayBoard[r, c - 1] == LIVE) neighbors++;
                if (displayBoard[r, c + 1] == LIVE) neighbors++;
                //if (displayBoard[r + 1, c - 1] == LIVE) neighbors++;
                //if (displayBoard[r + 1, c] == LIVE) neighbors++;
                //if (displayBoard[r + 1, c + 1] == LIVE) neighbors++;
            }
            else if (c == 0)
            {
                //if (displayBoard[r - 1, c - 1] == LIVE) neighbors++;
                if (displayBoard[r - 1, c] == LIVE) neighbors++;
                if (displayBoard[r - 1, c + 1] == LIVE) neighbors++;
                //if (displayBoard[r, c - 1] == LIVE) neighbors++;
                if (displayBoard[r, c + 1] == LIVE) neighbors++;
                //if (displayBoard[r + 1, c - 1] == LIVE) neighbors++;
                if (displayBoard[r + 1, c] == LIVE) neighbors++;
                if (displayBoard[r + 1, c + 1] == LIVE) neighbors++;
            }
            else if (c == MaxColumns - 1)
            {
                if (displayBoard[r - 1, c - 1] == LIVE) neighbors++;
                if (displayBoard[r - 1, c] == LIVE) neighbors++;
                //if (displayBoard[r - 1, c + 1] == LIVE) neighbors++;
                if (displayBoard[r, c - 1] == LIVE) neighbors++;
                //if (displayBoard[r, c + 1] == LIVE) neighbors++;
                if (displayBoard[r + 1, c - 1] == LIVE) neighbors++;
                if (displayBoard[r + 1, c] == LIVE) neighbors++;
                //if (displayBoard[r + 1, c + 1] == LIVE) neighbors++;
            }
            else
            {
                if (displayBoard[r - 1, c - 1] == LIVE) neighbors++;
                if (displayBoard[r - 1, c] == LIVE) neighbors++;
                if (displayBoard[r - 1, c + 1] == LIVE) neighbors++;
                if (displayBoard[r, c - 1] == LIVE) neighbors++;
                if (displayBoard[r, c + 1] == LIVE) neighbors++;
                if (displayBoard[r + 1, c - 1] == LIVE) neighbors++;
                if (displayBoard[r + 1, c] == LIVE) neighbors++;
                if (displayBoard[r + 1, c + 1] == LIVE) neighbors++;
            }
            //if im in a corner what is my edge 

            //check for four conditions 
            //corners
            return neighbors;
        }

        //display the game board to the user with a generation number label
        private void PrintGameBoard(int gen)
        { 
            Console.WriteLine($"Generation: #{gen}");
            for (int r = 0; r < MaxRows; r++)
            {
                for (int c = 0; c < MaxColumns; c++)
                {
                    //for init, just set everything to dead
                    Console.Write(displayBoard[r, c]);

                }
                Console.WriteLine();

            }
        }

            //swap the 2 game boards to prepare for the next generation
        private void SwapGameBoards()
        {
            char[,] tmp = displayBoard;
            displayBoard = resultsBoard;
            resultsBoard = tmp;
        }

        private void InitializeGameBoard(char[,] board)
        {
            for (int r = 0; r < MaxRows; r++) 
            {
                for (int c = 0; c < MaxColumns; c++)
                {
                    //for init, just set everything to dead
                    board[r, c] = DEAD;
                    
                }
            }
        }

        private void ConfiguredStartingPattern(int r, int c)
        {
            
            displayBoard[r, c+1] = LIVE;
            displayBoard[r, c+2] = LIVE;
            displayBoard[r, c+3] = LIVE;
            displayBoard[r, c+4] = LIVE;
            displayBoard[r, c+5] = LIVE;
            displayBoard[r, c+6] = LIVE;
            displayBoard[r, c + 7] = LIVE;
            displayBoard[r, c + 8] = LIVE;
            /*displayBoard[r, c + 9] = LIVE;*/
            displayBoard[r, c + 10] = LIVE;
            displayBoard[r, c + 11] = LIVE;
            displayBoard[r, c + 12] = LIVE;
            displayBoard[r, c + 13] = LIVE;
            displayBoard[r, c + 14] = LIVE;
            /*displayBoard[r, c + 15] = LIVE;
            displayBoard[r, c + 16] = LIVE;
            displayBoard[r, c + 17] = LIVE;
            displayBoard[r, c + 18] = LIVE;*/
            displayBoard[r, c + 19] = LIVE;
            displayBoard[r, c + 20] = LIVE;
            displayBoard[r, c + 21] = LIVE;
            displayBoard[r, c + 22] = LIVE;
            displayBoard[r, c + 23] = LIVE;
            displayBoard[r, c + 24] = LIVE;
            displayBoard[r, c + 25] = LIVE;
            displayBoard[r, c + 26] = LIVE;
            displayBoard[r, c + 27] = LIVE;
            displayBoard[r, c + 28] = LIVE;
            displayBoard[r, c + 29] = LIVE;
            displayBoard[r, c + 30] = LIVE;
            displayBoard[r, c + 31] = LIVE;
            displayBoard[r, c + 32] = LIVE;
            displayBoard[r,c + 33] = LIVE;
            displayBoard[r, c + 34] = LIVE;
            displayBoard[r, c + 35] = LIVE;
            displayBoard[r, c + 36] = LIVE;
            displayBoard[r, c + 37] = LIVE;
            displayBoard[r, c + 38] = LIVE;
            displayBoard[r, c + 39] = LIVE;
            displayBoard[r, c + 40] = LIVE;




        }
    }
}
