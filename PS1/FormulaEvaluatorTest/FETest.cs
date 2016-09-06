using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FormulaEvaluatorTest
{
    class FETest
    {

        public static String test = "( 2 + 35 ) * A7";

        static void Main(string[] args)
        {
            Stack<char> operation = new Stack<char>();

            Console.WriteLine(operation.Peek());

            Console.ReadLine();
        }
    }
}
