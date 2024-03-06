//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Date            Developer           Info:
/// 2/22/2024       atmcdon             Start Game Of Life
/// 2/22/2024       atmcdon             Game rules, ConfiguredStartingPattern, wapGameBoards(), and print board
/// 2/27/2024       atmcdon             developed play, apply game rules, and countNeighbor cells.
/// 2/29/2024       atmcdon             Developed CountNeighborCellsCHATBOT to run program faster.
/// 2/1 /2024       atmcdon             added randomly placed starting pattern.
/// 2/2 /2024       atmcdon             added comments.
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

        private const char DEAD = ' ';
        private const char LIVE = '@';
        //This will allow this ti be a const but just not need to 
        // have a value when inits
        private readonly int MaxRows;
        private readonly int MaxColumns;

        //Show char for console app
        // storage for the 2 game boards.
        private char[,] displayBoard;
        private char[,] resultsBoard;

        private int randomAmount = 0;

        Stopwatch watch;

        //This sets up the display board from the given rows and cols given
        //If given a value greater then 0 will setup ConfiguredStartingPatternRandom
        public Life(int rows, int cols, int randomAmount)
        {
            watch = new Stopwatch();
            MaxRows = rows;
            MaxColumns = cols;

            //allocate storage for the 2 game boards
            displayBoard = new char[MaxRows, MaxColumns];
            resultsBoard = new char[MaxRows, MaxColumns];

            //init game board
            InitializeGameBoard(displayBoard);
            InitializeGameBoard(resultsBoard);

            // choose a start state for an interesting "game"
            // places starting configuration

            if (randomAmount > 0)
            {
                ConfiguredStartingPatternRandom(MaxRows, MaxColumns, randomAmount);
                ConfiguredStartingPatternSuperRandom(MaxRows, MaxColumns, randomAmount);
            }
            else
            {
                ConfiguredStartingPattern(MaxRows / 2, MaxColumns / 3);
            }
        }

        //Play will process and display the game board.
        // runs in a loop of how many generations are wanted to be preformed.
        // 1 print the game board
        // 2 process the rules of the game
        // 2.1 store results in results board
        // 3 swap the boards to get ready for another generation.
        public void Play(int numGenerations)
        {
            watch.Start();
            for (int i = 1; i <= numGenerations; i++)
            {
                int count = 0;

                //Console.Clear();
                // 1 - print game board (display to the user)
                /*if (count == 100 || count==200 || count == numGenerations) 
                {
                    PrintGameBoard(i);
                }*/
                PrintGameBoard(i);
                count++;

                // 2 - apply the rules of the game
                // process the (current) game board and store the results in the resultsBoard
                ProcessMainGameBoard();

                // 3 - swap the two boards to prepare to start over for the next generation.
                SwapGameBoards();
            }
            //PrintGameBoard(numGenerations);
            watch.Stop();
            Console.WriteLine($"Elapsed Time{watch.Elapsed}");
        }

        // Iterates through each row and col to each cell and determines if it should be LIVE or DEAD.
        // Out puts this result to the results board.
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
        //Applies the the Game of life Rules to a given cell to determine if it is LIVE or DEAD.
        //This is dependent on the amount of neighbors that cell is returned.
        private char ApplyGameRules(int r, int c)
        {
            //Try to clarify the game rules
            /*Any live cell with fewer than two live neighbors dies, as if by underpopulation.
             *              If live and < 2 lives DEAD
             * 
              Any live cell with two or three live neighbors lives on to the next generation.
                            if live cell == 2 || 3 LIVE
              Any live cell with more than three live neighbors dies, as if by overpopulation.
                            LIVE > three neighbors live neighbors died
              Any dead cell with exactly three live neighbors becomes a live cell, as if by reproduction.*/
            int neighborCount = CountNeighborCellsCHATBOT(r, c);
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

        // This is a "faster" way to determine neighbor count says the chat bot.
        private int CountNeighborCellsCHATBOT(int r, int c)
        {
            int neighbors = 0;
            for (int i = Math.Max(0, r - 1); i <= Math.Min(r + 1, MaxRows - 1); i++)
            {
                for (int j = Math.Max(0, c - 1); j <= Math.Min(c + 1, MaxColumns - 1); j++)
                {
                    if (i == r && j == c) continue; // Skip the cell itself
                    if (displayBoard[i, j] == LIVE) neighbors++;
                }
            }
            return neighbors;
        }


        // This is a logical brute force approach to finding a cell's neighbor amount.
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

        //This displays the displayBoard through two for loop iterations.
        //displays generation number before starting each new Board.
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

        //swaps the 2 display boards to prepare for the next generation.
        private void SwapGameBoards()
        {
            char[,] tmp = displayBoard;
            displayBoard = resultsBoard;
            resultsBoard = tmp;
        }

        //Inits the board as dead.
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

        // This will Randomly place LIVE cells throughout the board in the amount
        // of live cells wanted.
        private void ConfiguredStartingPatternRandom(int r, int c, int amount)
        {
            Random random = new Random();
            for (int i = 0; i < amount; i++)
            {
                displayBoard[random.Next(0, r), random.Next(0, c)] = LIVE;
            }
        }

        //Supposed to be even more random the ConfiguredStartingPatternRandom but
        // is not really.
        private void ConfiguredStartingPatternSuperRandom(int r, int c, int amount)
        {
            Random random = new Random();
            for (int i = 0; i < amount; i++)
            {
                int firstNum = random.Next(0, r);
                int secNum = random.Next(0, r);

                int firstNumc = random.Next(0, c);
                int secNumc = random.Next(0, c);
                int randnum = random.Next(0, r);
                if (randnum < r / 2 || randnum < c / 2)
                {
                    displayBoard[firstNum, firstNumc] = LIVE;
                }
                if (randnum > r / 2 || randnum > c / 2)
                {
                    displayBoard[secNum, secNumc] = LIVE;
                }
            }
        }

        //This is the default Configuration for the display board.
        private void ConfiguredStartingPattern(int r, int c)
        {

            displayBoard[r, c + 1] = LIVE;
            displayBoard[r, c + 2] = LIVE;
            displayBoard[r, c + 3] = LIVE;
            displayBoard[r, c + 4] = LIVE;
            displayBoard[r, c + 5] = LIVE;
            displayBoard[r, c + 6] = LIVE;
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
            displayBoard[r, c + 17] = LIVE;*/
            displayBoard[r, c + 18] = LIVE;
            displayBoard[r, c + 19] = LIVE;
            displayBoard[r, c + 20] = LIVE;
            /*displayBoard[r, c + 21] = LIVE;
            displayBoard[r, c + 22] = LIVE;
            displayBoard[r, c + 23] = LIVE;
            displayBoard[r, c + 24] = LIVE;
            displayBoard[r, c + 25] = LIVE;
            displayBoard[r, c + 26] = LIVE;*/
            displayBoard[r, c + 27] = LIVE;
            displayBoard[r, c + 28] = LIVE;
            displayBoard[r, c + 29] = LIVE;
            displayBoard[r, c + 30] = LIVE;
            displayBoard[r, c + 31] = LIVE;
            displayBoard[r, c + 32] = LIVE;
            displayBoard[r, c + 33] = LIVE;
            /*displayBoard[r, c + 34] = LIVE;*/
            displayBoard[r, c + 35] = LIVE;
            displayBoard[r, c + 36] = LIVE;
            displayBoard[r, c + 37] = LIVE;
            displayBoard[r, c + 38] = LIVE;
            displayBoard[r, c + 39] = LIVE;
            /* displayBoard[r, c + 40] = LIVE;*/
            /*displayBoard[r, c + 41] = LIVE;*/
        }
    }
}
