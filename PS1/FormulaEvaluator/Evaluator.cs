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

        /// <summary>
        /// Evaluate takes a mathmatical expressions and evaluates it.  It can really only deal with +-*/ and parenthesis.
        /// If the input string is not a valid expression it will throw an ArgumentException.
        /// </summary>
        /// <param name="exp">The mathematical expression to be evaluated</param>
        /// <param name="variableEvaluator"> A function to look up variables.  It should take a string as a variable and return an integer</param>
        /// <returns>Returns the integer the expression evaluated to.</returns>
        public static int Evaluate(String exp, Lookup variableEvaluator)
        {
            // TODO... The sixth option and what to do when all parsed

            //Turn string into string tokens in an array
            string[] substrings = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            //Remove whitespace
            for (int i = 0; i < substrings.Length; i++)
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
                        //Turn it from a string into a int
                        Int32.TryParse(substrings[i], out currentNumber);

                        // Check if the operation stack is empty before peeking
                        if (operations.Count == 0)
                        {
                            // Push the current integer onto the stack
                            values.Push(currentNumber);
                            break;
                        }

                        // Check if multiplication is at the top
                        if (operations.Peek().Equals('*') || operations.Peek() == '/')
                        {
                            Multiplication(values, operations, currentNumber);
                        } else
                        {
                            // Push the current integer onto the stack
                            values.Push(currentNumber);
                        }
                        break;

                    // Variable
                    case 2:
                        currentNumber = variableEvaluator(substrings[i]);

                        // Check if the operation stack is empty before peeking
                        if (operations.Count == 0)
                        {
                            // Push the current integer onto the stack
                            values.Push(currentNumber);
                        }

                        // Check if multiplication is at the top
                        if (operations.Peek().Equals('*') || operations.Peek() == '/')
                        {
                            Multiplication(values, operations, currentNumber);
                        }
                        else
                        {
                            // Push the current integer onto the stack
                            values.Push(currentNumber);
                        }
                        break;

                    // Addition or subtraction
                    case 3:
                        Addition(values, operations);

                        // Push the current operation onto the operations stack
                        Char.TryParse(substrings[i], out currentOperation);
                        operations.Push(currentOperation);
                        break;

                    // Multiplication or division
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
                        // Does the first step(addition)
                        Addition(values, operations);

                        // checks to make sure we have a matching opening parenthesis
                        if (operations.Count < 0 || operations.Peek() != '(')
                        {
                            throw new ArgumentException("Opening parenthesis not in the correct position.");
                        }
                        // Pops the opening parenthesis
                        operations.Pop();

                        // Makes sure we are doing muliplication/division
                        if (operations.Count > 0 && (operations.Peek() == '*' || operations.Peek() == '/'))
                        {
                            // Checks to make sure there are enough values to do the operation
                            if (values.Count < 2)
                            {
                                throw new ArgumentException("Not enough values to perform operation");
                            }

                            // Doing the multiplication and pushes the result
                            currentNumber = values.Pop();

                            Multiplication(values, operations, currentNumber);
                        }
                        break;

                    case 0:
                        //Empty string.  Disregard.
                        break;

                    //Something went wrong!
                    default:
                        throw new ArgumentException("CategorizeToken is returning stupid things");

                }
            }

            // Check if the operations stack is empty or not
            if (operations.Count == 0)
            {
                if (values.Count != 1)
                {
                    throw new ArgumentException("There are too many values on the stack");
                }

                return values.Pop();
            }
            else
            {
                // Check to make sure we have everything to do addition or subtraction
                if (operations.Count != 1 || values.Count != 2 || (operations.Peek() != '+' && operations.Peek() != '-'))
                {
                    throw new ArgumentException("There isn't exaclty 1 operator and 2 values left");
                }

                if (operations.Pop() == '+')
                {
                    currentNumber = values.Pop();
                    int previousNumber = values.Pop();

                    return previousNumber + currentNumber;
                }
                else
                {
                    currentNumber = values.Pop();
                    int previousNumber = values.Pop();

                    return previousNumber - currentNumber;
                }
            }

        }

        /// <summary>
        /// Performs the section of the algorithm that does addition or subtraction.  It pops 2 entries from values and one entry from operations and then performs the operation.
        /// If there are not enough entries in either stack it will throw an ArgumentException.
        /// </summary>
        /// <param name="values">The values stack</param>
        /// <param name="operations">The operations stack</param>
        private static void Addition(Stack<int> values, Stack<char> operations)
        {
            //Make sure there is something on the operations stack
            if (operations.Count < 1)
            {
                return;
            }

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
                char oper = operations.Pop();

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

        /// <summary>
        /// Performs the part of the operation where you multiply or divide values.  It will pop one entry off of values and one off of operations.
        /// It will throw an ArgumentException if there is not any entries in values of operations.
        /// </summary>
        /// <param name="values">The values stack</param>
        /// <param name="operations">the operations stack.  Seriously why don't I just change the permissions.</param>
        /// <param name="currentNumber"> The second number in the operation. i.e. the number you would divide by.</param>
        private static void Multiplication(Stack<int> values, Stack<char> operations, int currentNumber)
        {
            // Checking for an empty value stack
            if (values.Count == 0)
            {
                throw new ArgumentException("The value stack is empty already");
            }

            // Checking for an empty operations stack
            if (operations.Count == 0)
            {
                throw new ArgumentException("The operations stack is empty already");
            }

            int lastNumber = values.Pop();
            char operation = operations.Pop();

            // Chacks for division by zero
            if (currentNumber == 0 && operation == '/')
            {
                throw new ArgumentException("Division by zero");
            }
            // Multiply or divide and push
            if (operation == '*')
            {
                values.Push(lastNumber * currentNumber);
            }
            else
            {
                values.Push(lastNumber / currentNumber);
            }
        }



        /// <summary>
        /// This method looks at a token, caegorizes it, and returns a corresponding int.
        /// 0 = empty buffer string.  Disregards
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
            if (Int32.TryParse(s, out number))
            {
                return 1;
            }
            else if (s.Equals("+") || s.Equals("-"))
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
            else if (IsVariableFormat(s))
            {
                return 2;
            } else if (s.Equals(""))
            {
                return 0;
            }

            throw new ArgumentException("AHHHHH!! Run for your life! It isn't one of the accepted tokens");
        }

        /// <summary>
        /// Just checks if the string is of the format of a variable.  If it is the wrong format it will throw an ArgumentException.
        /// </summary>
        /// <param name="s">The string to be checked</param>
        /// <returns>returns true if it is a valid variable.  Returns false if it is an empty string</returns>
        private static bool IsVariableFormat(string s)
        {
            // Simplifies what needs to be checked.
            s.ToUpper();
            char[] variable = s.ToCharArray();

            // Keeps track of when we switch from letters to numbers.
            bool switchedToNumbers = false;

            // Check if it is actually an empty token.  Disregard if it is empty
            if (variable.Length == 0) {
                return false;
            }

            // Checks if the first char is a letter and that the last char is an integer.
            if (!(variable[0] >= 'A' && variable[0] <= 'Z') || !(variable[variable.Length - 1] >= 'A' && variable[variable.Length - 1] <= 'Z'))
            {
                throw new ArgumentException("Variable name is of the wrong format." + s + " is an invalid format");
            }

            // Go through one char at a time seeing if it is letters then numbers
            for (int i = 0; i < variable.Length; i++)
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
                }
                else
                {
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
