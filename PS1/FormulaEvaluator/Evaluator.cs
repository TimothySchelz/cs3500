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
            // TODO... The sixth option and what to do when all parsed

            //Turn string into string tokens in an array
            string[] substrings = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            //Remove whitespace
            for(int i = 0; i < substrings.Length; i++)
            {
                substrings[i].Trim();
            }

            //Create the stacks
            Stack<int> values = new Stack<int>();
            Stack<char> operations = new Stack<char>();

            int currentNumber;
            char currentOperation;

            for (int i = 0; i < substrings.Length; i++)
            {

                switch (CategorizeToken(substrings[i]))
                {
                    // Integer
                    case 1:
                        //Turn it from a string into a char

                        Int32.TryParse(substrings[i], out currentNumber);

                        // Checking for an empty value stack
                        if (values.Count == 0)
                        {
                            throw new ArgumentException("The value stack is empty already"); 
                        }
                        // Check if the operation stack is empty before peeking
                        if (operations.Count == 0)
                        {
                            // Push the current integer onto the stack
                            values.Push(currentNumber);
                        }

                        Multiplication(values, operations, currentNumber);
                        break;

                    // Variable
                    case 2:
                        currentNumber = variableEvaluator(substrings[i]);
                        Multiplication(values, operations, currentNumber);
                        break;

                    // Addition
                    case 3:
                        Addition(values, operations);

                        // Push the current operation onto the operations stack
                        Char.TryParse(substrings[i], out currentOperation);
                        operations.Push(currentOperation);
                        break;

                    // Multiplication
                    case 4:
                        // Turn the token into a char
                        Char.TryParse(substrings[i], out currentOperation);

                        // Push it onto the stack
                        operations.Push(currentOperation);
                        break;

                    // Opening Parenthesis
                    case 5:
                        Char.TryParse(substrings[i], out currentOperation);

                        // Push it onto the stack
                        operations.Push(currentOperation);
                        break;

                    // Closing Parenthesis
                    case 6:
                        Addition(values, operations);

                        if (operations.Count < 0 || operations.Pop() != '(')
                        {
                            throw new ArgumentException("Opening parenthesis not in the correct position.");
                        }

                        operations.Pop();


                        Multiplication(values, operations, );

                        break;

                    default:
                        Console.WriteLine("Default case");
                        break;
                }
            }

        }

        private static void Addition(Stack<int> values, Stack<char> operations)
        {
            // Check if the previous stuff was also addition/subtraction
            if (operations.Peek().Equals('+') || operations.Peek() == '-')
            {
                // Check if there are enough values to do the previous operation
                if (values.Count < 2)
                {
                    throw new ArgumentException("Not enough values to perform operation");
                }

                // Pop everything out
                int num1 = values.Pop();
                int num2 = values.Pop();
                int oper = operations.Pop();

                // Perform the previous addition or subtraction
                if (oper == '+')
                {
                    values.Push(num2 + num1);
                }
                else
                {
                    values.Push(num2 - num1);
                }

            }
        }

        private static void Multiplication(Stack<int> values, Stack<char> operations, int currentNumber)
        {

            // Check if multiplication is at the top
            if (operations.Peek().Equals('*') || operations.Peek() == '/')
            {
                int lastNumber = values.Pop();
                char operation = operations.Pop();

                // Chacks for division by zero
                if (currentNumber == 0 && operation == '/') {
                    throw new ArgumentException("Division by zero");
                }
                // Multiply or divide and push
                if (operation == '*') {
                    values.Push(lastNumber*currentNumber);
                } else {
                    values.Push(lastNumber/currentNumber);
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
