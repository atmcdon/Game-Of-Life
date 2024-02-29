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
            //here is our singltion object that represents the acutal "game play"
            Life game = new Life(rows, cols);
            game.Play(numGenerations);


        }

        private static void DisplayUserInterface()
        {
            Console.WriteLine(@"
DISPLAY THE USER INTERFACE HERE...
Get From wiki pedia
");
        }
    }
}
