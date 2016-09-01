using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FormulaEvaluator
{
    public static class Evaluator
    {
        public delegate int Lookup(String v);

        public static int Evaluate(String exp, Lookup variableEvaluator)
        {
            // TODO... Everything

            //Turn string into string tokens in an array
            string[] substrings = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            //Remove whitespace
            for(int i = 0; i < substrings.Length; i++)
            {
                substrings[i].Trim();
            }

            //Create the stacks
            Stack<int> values = new Stack<int>();
            Stack<char> operation = new Stack<char>();

            for (int i = 0; i < substrings.Length; i++)
            {
                switch (CategorizeToken(substrings[i]))
                {
                    case 1:
                        Console.WriteLine("Case 1");
                        break;
                    case 2:
                        Console.WriteLine("Case 2");
                        break;
                    default:
                        Console.WriteLine("Default case");
                        break;
                }
            }
        }

        /// <summary>
        /// This method looks at a token, caegorizes it, and returns a corresponding int.
        /// 1 = integer
        /// 2 = variable
        /// 3 = + or -
        /// 4 = * or /
        /// 5 = (
        /// 6 = )
        /// 
        /// throws an ArgumentException if it is not one of the listed types
        /// 
        /// </summary>
        /// <param name="s"> The String token to be categorized</param>
        /// <returns> An int corresponding with one of the cases above</returns>
        private static int CategorizeToken(String s)
        {
            int number;
            if(Int32.TryParse(s, out number) && number >= 0)
            {
                return 1;
            }
            else if (IsVariableFormat(s))
            {
                return 2;
            }
            else if(s.Equals("+")|| s.Equals("-"))
            {
                return 3;
            }
            else if (s.Equals("*") || s.Equals("/"))
            {
                return 4;
            }
            else if (s.Equals("("))
            {
                return 5;
            }
            else if (s.Equals(")"))
            {
                return 6;
            }

            throw new ArgumentException();
        }

        /// <summary>
        /// Just checks if the string is of the format of a variable
        /// </summary>
        /// <param name="s">The string to be checked</param>
        /// <returns>returns true if it is a varaible</returns>
        private static bool IsVariableFormat(string s)
        {
            // TODO: Finish writing this method, figure out how to tell if the formatting is correct
            bool formatted = true;
            bool switchedToNumbers = false;
            for(int i = 0; i < s.Length; i++)
            {
                if (!switchedToNumbers)
                {

                }
            }
        }
    }
}
