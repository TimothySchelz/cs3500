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
            string[] substrings = Regex.Split(test, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            for(int i = 0; i < substrings.Length; i++)
            {
                if(substrings[i].Equals(" "))
                {
                    Console.WriteLine("!");
                } else
                {
                    Console.WriteLine(substrings[i]);
                }
            }

            Console.Read();
        }
    }
}
