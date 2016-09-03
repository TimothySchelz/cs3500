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
                    case 3:
                        Console.WriteLine("Case 3");
                        break;
                    case 4:
                        Console.WriteLine("Case 4");
                        break;
                    case 5:
                        Console.WriteLine("Case 5");
                        break;
                    case 6:
                        Console.WriteLine("Case 6");
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
            // Checks to see which case the current token falls into
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

            // It was not one of the listed types.  Something is wrong
            throw new ArgumentException();
        }

        /// <summary>
        /// Just checks if the string is of the format of a variable.  If it is the wrong format it will throw an ArgumentException.
        /// </summary>
        /// <param name="s">The string to be checked</param>
        /// <returns>returns true as long as an exception is not thrown</returns>
        private static bool IsVariableFormat(string s)
        {
            // Simplifies what needs to be checked.
            s.ToUpper();
            char[] variable = s.ToCharArray();

            // Keeps track of when we switch from letters to numbers.
            bool switchedToNumbers = false;

            // Checks if the first char is a letter and that the last char is an integer.
            if (!(variable[0] >= 'A' && variable[0] <= 'Z') || !(variable[variable.Length - 1] >= 'A' && variable[variable.Length - 1] <= 'Z'))
            {
                throw new ArgumentException("Variable name is of the wrong format." + s + " is an invalid format");
            }

            // Go through one char at a time seeing if it is letters then numbers
            for(int i = 0; i < variable.Length; i++)
            {
                // while still just a series of letters
                if (!switchedToNumbers)
                {
                    // If we are still on a letter move to the next char
                    if ((variable[i] >= 'A' && variable[i] <= 'Z'))
                    {
                        continue;
                    }
                        // If we have a number switch to the number part of the variable's name
                    else if ((variable[i] >= '0' && variable[i] <= '9'))
                    {
                        switchedToNumbers = true;
                    }
                        // If it is anything else then it does not fit the scheme
                    else
                    {
                        throw new ArgumentException("Variable name is of the wrong format." + s + " is an invalid format");
                    }
                // If we are on the number section of the variable
                } else {
                    // If we don't have a number then we have a problem
                    if (!(variable[i] >= '0' && variable[i] <= '9'))
                    {
                        throw new ArgumentException("Variable name is of the wrong format." + s + " is an invalid format");
                    }
                }
            }

            // No exceptions have been thrown so it should be of the correct format.
            return true;
        }
    }
}
