// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

// Skeleton fleshed out by Timothy Schelz, u0851027, 9/22/2016

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax; variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {
        private LinkedList<String> formula; // Holds each token of the formula.  Each token should be 
                                            // normalized, in a valid order, and each variable should be valid.
        private HashSet<String> variables; // A HashSet of all the variables.  They should all be 
                                           //normalized to prevent duplicates and a big ol mess.

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) :
            this(formula, s => s, s => true)
        {
            // It just has to basically do the other constructor so this doesn't need to do anything
        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically 
        /// incorrect.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            //Some variables that will be used in each loop
            String normed; // The current token after it has been normalized
            int tokenType; // The current token's type
            int parens = 0; // The number of parenthesis that have been opened but not closed
            int previousType = -1; // The type of the previous token.  The -1 means it hasn't been 
                                   // used yet

            this.formula = new LinkedList<String>();
            variables = new HashSet<string>();

            // check to make sure it is breaking any rules.  All my tokens play by the rules.  I don't 
            // tolerate any loose cannons.
            // also puts the token into a linked list to be stored.  Goes through and does this for 
            // each token that GetTokens returns.
            foreach (String current in GetTokens(formula))
            {
                normed = normalize(current);
                tokenType = CategorizeToken(normed, isValid);

                // If we are dealing with a number we cast it as a double
                if (tokenType == 1)
                {
                    normed = "" + Double.Parse(normed); //already parsed inside of CategorizeTokens() so we know it is a double
                }

                //Check if the previous token was a parenthesis or an operator
                //Syntax rule 7
                if ((previousType == 5 || previousType == 3 || previousType == 4)
                    && !(tokenType == 1 || tokenType == 2 || tokenType == 5))
                {
                    throw new FormulaFormatException("The Parenthesis Following Rule was broken." + 
                        "  An invalid token followed an opening parenthesis or operation");
                }

                //Check if the previous token was a number variable or closing parens
                //Syntax rule 8
                if ((previousType == 1 || previousType == 2 || previousType == 6)
                    && !(tokenType == 3 || tokenType == 4 || tokenType == 6))
                {
                    throw new FormulaFormatException("The Extra Following Rule was broken.  " + 
                        "An invalid token followed a variable, closing parenthesis or a number.");
                }

                // Add values to the variables HashSet if we come across any variables
                if (tokenType == 2)
                {
                    variables.Add(normed);
                }

                //increments or decrements the number of opened parenthesis
                // Syntax rule 3
                if (tokenType == 5)
                {
                    parens++;
                }
                else if (tokenType == 6)
                {
                    parens--;
                }
                //Check at each step that the number of closing parenthesis doesn't outpace the number 
                //of opening parenthesis.
                if (parens < 0)
                {
                    throw new FormulaFormatException("The number of closing parenthesis was larger " + 
                        "than the number of opening parenthesis.  Check the number of opening and " + 
                        "closing parenthesis.");
                }

                // Add this sucker into the permanent formula and put it's type in the previous pile
                this.formula.AddLast(normed);
                previousType = tokenType;
            }

            //Checks if the resulting formula is empty
            //Syntax rule 2
            if (this.formula.Count() == 0)
            {
                throw new FormulaFormatException("There were no tokens in the given formula.  " + 
                    "Please enter a formula");
            }

            //Checks if there were no extra parenthesis
            // Syntax Rule 4
            if (parens != 0)
            {
                throw new FormulaFormatException("There was an extra opening parenthesis that was not closed.");
            }

            //Checks the first entry of the formula to make sure it is a valid token
            // Syntax rule 5
            int firstToken = CategorizeToken(this.formula.First(), isValid);
            if (!(firstToken == 1 || firstToken == 2 || firstToken == 5))
            {
                throw new FormulaFormatException("The first token is not a valid starting token");
            }

            //Checks the last entry of the formula to make sure it is a valid token
            // Syntax rule 5
            int lastToken = CategorizeToken(this.formula.Last(), isValid);
            if (!(lastToken == 1 || lastToken == 2 || lastToken == 6))
            {
                throw new FormulaFormatException("The ending token is not a valid ending token");
            }
        }

        /// <summary>
        /// This method looks at a token, caegorizes it, and returns a corresponding int.
        /// 0 = empty buffer string.  Disregards
        /// 1 = double
        /// 2 = variable
        /// 3 = + or -
        /// 4 = * or /
        /// 5 = (
        /// 6 = )
        /// 
        /// throws an FormulaFormatException if it is not one of the listed types
        /// 
        /// </summary>
        /// <param name="s"> The String token to be categorized</param>
        /// <param name="isValid"> A delegate to check to make sure a variable has the proper format
        ///     </param>
        /// <returns> An int corresponding with one of the cases above</returns>
        private static int CategorizeToken(String s, Func<string, bool> isValid)
        {
            // Checks to see which case the current token falls into
            double number;
            if (Double.TryParse(s, out number))
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
            else if ((s[0].Equals('_') || 
                (s.ToUpper()[0] >= 65 && s.ToUpper()[0] <= 90) && isValid(s)))
            {
                return 2;
            }

            //Syntax rule 1
            throw new FormulaFormatException(s + 
                " is not a valid type of token.  Please turn it into a double," + 
                " variable, (, ), +, -, *, or /.  Remember Variables must start with " + 
                "a letter or underscore");
        }

        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            //Create the stacks
            Stack<double> values = new Stack<double>();
            Stack<char> operations = new Stack<char>();

            double currentNumber;
            char currentOperation;

            foreach (String current in formula)
            {
                //It is ok to use a true validator here since they have all already been validated
                switch (CategorizeToken(current, s => true))
                {
                    // Double
                    case 1:
                        //Turn it from a string into a int
                        currentNumber = Double.Parse(current);//already parsed in constructor so we know it is a double

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
                            //Checks for division by zero
                            if (operations.Count() > 0 && operations.Peek() == 'u')
                            {
                                return new FormulaError("You divided by zero. Shame on you!");
                            }
                        }
                        else
                        {
                            // Push the current integer onto the stack
                            values.Push(currentNumber);
                        }
                        break;

                    // Variable
                    case 2:
                        try
                        {
                            currentNumber = lookup(current);
                        }
                        catch
                        {
                            return new FormulaError("The variable " + current + " does not exist.");
                        }

                        // Check if the operation stack is empty before peeking
                        if (operations.Count == 0)
                        {
                            // Push the current integer onto the stack
                            values.Push(currentNumber);
                            break;
                        }

                        // Check if multiplication is at the top
                        if ((operations.Peek().Equals('*') || operations.Peek() == '/'))
                        {
                            Multiplication(values, operations, currentNumber);
                            //Checks for division by zero
                            if (operations.Count() > 0 && operations.Peek() == 'u')
                            {
                                return new FormulaError("You divided by zero. Shame on you!");
                            }
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
                        currentOperation = Char.Parse(current); //already parsed inside of CategorizeTokens() so we know
                                                                //it is an addition or subtraction
                        operations.Push(currentOperation);
                        break;

                    // Multiplication or division
                    case 4:
                        // Turn the token into a char
                        currentOperation = Char.Parse(current);//already parsed inside of CategorizeTokens() so we know 
                                                               //it is a * or /

                        // Push it onto the stack
                        operations.Push(currentOperation);
                        break;

                    // Opening Parenthesis
                    case 5:
                        currentOperation = Char.Parse(current); //already parsed inside of CategorizeTokens() so we know
                                                                //it is an (

                        // Push it onto the stack
                        operations.Push(currentOperation);
                        break;

                    // Closing Parenthesis
                    case 6:
                        // Does the first step(addition)
                        Addition(values, operations);

                        // Pops the opening parenthesis
                        operations.Pop();

                        // Makes sure we are doing muliplication/division
                        if (operations.Count > 0 && (operations.Peek() == '*' 
                            || operations.Peek() == '/'))
                        {
                            // Doing the multiplication and pushes the result
                            currentNumber = values.Pop();

                            Multiplication(values, operations, currentNumber);
                            //Checks for division by zero
                            if (operations.Count() > 0 && operations.Peek() == 'u')
                            {
                                return new FormulaError("You divided by zero. Shame on you!");
                            }
                        }
                        break;
                }
            }


            // Now that we are done parsing we do the finishing touches

            // Check if the operations stack is empty or not
            if (operations.Count == 0)
            {
                return values.Pop();
            }
            else
            {
                if (operations.Pop() == '+')
                {
                    currentNumber = values.Pop();
                    double previousNumber = values.Pop();

                    return previousNumber + currentNumber;
                }
                else
                {
                    currentNumber = values.Pop();
                    double previousNumber = values.Pop();

                    return previousNumber - currentNumber;
                }
            }
        }

        /// <summary>
        /// Performs the section of the algorithm that does addition or subtraction.  It pops 2 entries 
        /// from values and one entry from operations and then performs the operation.
        /// 
        /// If there are not enough entries in either stack it will throw an ArgumentException.
        /// </summary>
        /// <param name="values">The values stack</param>
        /// <param name="operations">The operations stack</param>
        private static void Addition(Stack<double> values, Stack<char> operations)
        {
            // Check if the previous stuff was also addition/subtraction
            if ((operations.Count() > 0) && (operations.Peek().Equals('+') || operations.Peek() == '-'))
            {
                // Pop everything out
                double num1 = values.Pop();
                double num2 = values.Pop();
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
        /// Performs the part of the operation where you multiply or divide values.  It will pop one 
        /// entry off of values and one off of operations.
        /// 
        /// It will push a 'u' on the operations stack if there is a division by zero. Make sure to 
        /// deal with it!
        /// 
        /// </summary>
        /// <param name="values">The values stack</param>
        /// <param name="operations">the operations stack.  Seriously why don't I just change the 
        ///     permissions.</param>
        /// <param name="currentNumber"> The second number in the operation. i.e. the number you would 
        ///     divide by.</param>
        private static void Multiplication(Stack<double> values, Stack<char> operations, double currentNumber)
        {
            double lastNumber = values.Pop();
            char operation = operations.Pop();

            // Checks for division by zero
            if (currentNumber == 0 && operation == '/')
            {
                operations.Push('u');
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
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            return variables.ToList<String>();
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            String output = "";
            foreach (String current in formula)
            {
                output = output + current;
            }

            return output;
        }

        /// <summary>
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens, which are compared as doubles, and variable tokens,
        /// whose normalized forms are compared as strings.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object obj)
        {
            // Returns false if obj is null or not a formula
            if (obj == null || !(obj is Formula))
            {
                return false;
            }

            // Checks to see if their to strings are the same.  If they are then they should be equal
            if (this.ToString() == obj.ToString())
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return true.  If one is
        /// null and one is not, this method should return false.
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            // Check the weird null cases
            if (object.Equals(f1, f2))
            {
                return true;
            }

            // Check to see if f1 is null
            if (object.Equals(f1, null))
            {
                return false;
            }

            // return if they are equal
            return f1.Equals(f2);
        }

        /// <summary>
        /// Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return false.  If one is
        /// null and one is not, this method should return true.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            return !(f1 == f2);
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            // Just returns the defining string's hashcode
            return this.ToString().GetHashCode();
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }

        }
    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
    }
}
