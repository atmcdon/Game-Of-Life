/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Change History


using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;

namespace Game_Of_Life
{
    internal class Program
    {
        static void Main(string[] args)
        {

            
            DisplayUserInterface();

            Console.WriteLine("How many generations in this sim: ");
            int numGenerations = int.Parse(Console.ReadLine());


            Console.WriteLine("How many rows do you want in the game field: ");
            int rows = int.Parse(Console.ReadLine());


            Console.WriteLine("How many columns do you want in the game field: ");
            int cols = int.Parse(Console.ReadLine());

            Console.WriteLine("How many random generations would you want on the screen?");
            Console.WriteLine("If you want the default just press input < 0 >");
            int randomAmount = int.Parse(Console.ReadLine());
            
            //here is our singltion object that represents the acutal "game play"
            Life game = new Life(rows, cols, randomAmount);
            game.Play(numGenerations);


        }

        private static void DisplayUserInterface()
        {
            Console.WriteLine(@"
Conway's Game of Life is a zero-player game created by mathematician John Horton Conway in 1970.
It simulates cellular automaton on a grid where cells live or die based on simple rules relating
to neighboring cells. The game illustrates how complex patterns can emerge from basic rules,
showcasing phenomena like self-organization and emergence. It's not just a game but a mathematical
model used for exploring computation, physics, and theoretical biology concepts.

500X4000
");
        }
    }
}
