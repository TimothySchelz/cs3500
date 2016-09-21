using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using System.Collections.Generic;

namespace FormulaTester
{
    [TestClass]
    public class FormulaTester
    {
        /*
         * Single Parameter Constructor Tests
         */

        /// <summary>
        /// Tests creating a Formula with an invalid input
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void PublicCon1InvalidNonsense()
        {
            Formula f = new Formula("+++");
        }

        /// <summary>
        /// Tests creating a Formula with an invalid input
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void PublicCon1InvalidInvalidCharacter()
        {
            Formula f = new Formula("4 ^ 15");
        }

        /// <summary>
        /// Tests creating a Formula with an invalid input, no valid tokens
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void PublicCon1InvalidNoTokens()
        {
            Formula f = new Formula("");
        }

        /// <summary>
        /// Tests creating a Formula with no operations between variables
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void PublicCon1InvalidVariablesNoOperations()
        {
            Formula f = new Formula("x y + y x");
        }

        /// <summary>
        /// Tests the small constructor with a formula with no variables
        /// </summary>
        [TestMethod]
        public void PublicCon1NoVariableInput()
        {
            Formula f = new Formula("13 - 15*(1/2)");

            Assert.AreEqual("13-15*(1/2)", f.ToString());
        }

        /// <summary>
        /// Checks the itty bitty constructor with a formula with variables
        /// </summary>
        [TestMethod]
        public void PublicCon1YesVariableInput()
        {
            Formula f = new Formula("A1 - b2*(Df900/2)");

            Assert.AreEqual("A1-b2*(Df900/2)", f.ToString());
        }

        /*
         * Big Boy Constructor Tests
         */

        /// <summary>
        /// Checks the rotund constructor's use of the normalizer
        /// </summary>
        [TestMethod]
        public void PublicCon2NormalizerChangesFormula()
        {
            Formula f = new Formula("a1 + a1 * c3 / c3 - e5", s => s.ToUpper(), s => true);

            Assert.AreEqual("A1+A1*C3/C3-E5", f.ToString());
        }

        /// <summary>
        /// Checks the rotund constructor's use of the normalizer
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void PublicCon2EmptyStringInput()
        {
            Formula f = new Formula("", s => s.ToUpper(), s => true);
        }

        /// <summary>
        /// Checks the rotund constructor's use of the normalizer on things that don't need it
        /// </summary>
        [TestMethod]
        public void PublicCon2NormalizerIsUnnecessary()
        {
            Formula f = new Formula("A1 + A1 * C3 / C3 - E5", s => s.ToUpper(), s => true);

            Assert.AreEqual("A1+A1*C3/C3-E5", f.ToString());
        }

        /// <summary>
        /// Checks that the IsValid delegate is used correcty on a valid variable
        /// </summary>
        [TestMethod]
        public void PublicCon2IsValidCheckOnValidVariable()
        {
            Formula f = new Formula("A1 + A1 * 5", s => s.ToUpper(), s => s == "A1");

            Assert.AreEqual("A1+A1*5", f.ToString());
        }

        /// <summary>
        /// Checks that the FormulaFormatException is thrown
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void PublicCon2IsValidFormulaFormatExceptionThrowDown()
        {
            Formula f = new Formula("A2 + B1 * 5", s => s.ToUpper(), s => s == "A1");
        }

        /// <summary>
        /// Checks that it throws an exception when there are too many closing parenthesis at some point.  the extra one is at the end
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void PublicCon2TooManyClosingParenthesis1()
        {
            Formula f = new Formula("(A2 + B1 * 5))", s => s.ToUpper(), s => s == "A1");
        }

        /// <summary>
        /// Checks that it throws an exception when there are too many closing parenthesis at some point. the extra one is in the middle
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void PublicCon2TooManyClosingParenthesis2()
        {
            Formula f = new Formula("A2 + B1) * (5)", s => s.ToUpper(), s => s == "A1");
        }

        /// <summary>
        /// Checks that it throws an exception when there are too many closing parenthesis at some point. the extra one is in the middle
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void PublicCon2BreaksRule4()
        {
            Formula f = new Formula("(A2 + B1) * 5(", s => s.ToUpper(), s => s == "A1");
        }

        /// <summary>
        /// Checks that it throws an exception when the starting token is not a valid starting token
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void PublicCon2BreaksRule51()
        {
            Formula f = new Formula("- A2 + B1) * (5)", s => s.ToUpper(), s => s == "A1");
        }

        /// <summary>
        /// Checks that it throws an exception when the starting token is not a valid starting token
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void PublicCon2BreaksRule52()
        {
            Formula f = new Formula("/ A2 + B1 * (5)", s => s.ToUpper(), s => s == "A1");
        }

        /// <summary>
        /// Checks that it throws an exception when the last token is not a valid ending token
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void PublicCon2BreaksRule61()
        {
            Formula f = new Formula("(A2 + B1) * (5)*", s => s.ToUpper(), s => s == "A1");
        }

        /// <summary>
        /// Checks that it throws an exception when the last token is not a valid ending token
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void PublicCon2BreaksRule62()
        {
            Formula f = new Formula("(A2 + B1) * (5) +", s => s.ToUpper(), s => s == "A1");
        }


        /*
         * Evaluate Tests
         * 
         * 
         * Ugh I guess I will start by just stealing some tests from PS1 and then checking doubles
         * 
         * 
         */


        /*
         * GetVariables Tests
         */

        /// <summary>
        /// Checks the method in a typical case with a few variables to be returned
        /// </summary>
        [TestMethod]
        public void PublicGetVariablesNormalCase()
        {
            Formula f = new Formula("A1 + B2 * C3 / D4 - E5");

            HashSet<String> expected = new HashSet<String> { "A1", "B2", "C3", "D4", "E5" };

            List<String> result = (List<String>) f.GetVariables();

            //Check size equality
            Assert.AreEqual(expected.Count, result.Count);

            //Check element equality
            foreach (String s in result)
            {
                Assert.IsTrue(expected.Contains(s));
            }
        }

        /// <summary>
        /// Checks the method when there are no variables to be gotten.  just checks that the sizes are equal.  The should be 0.
        /// </summary>
        [TestMethod]
        public void PublicGetVariablesEmptyCase()
        {
            Formula f = new Formula("5 + 4 - 3");

            HashSet<String> expected = new HashSet<String> {};

            List<String> result = (List<String>)f.GetVariables();

            //Check size equality
            Assert.AreEqual(expected.Count, result.Count);
        }

        /// <summary>
        /// Checks to make sure that when there are variables that are normalized to the same thing only 1 copy of it is returned
        /// </summary>
        [TestMethod]
        public void PublicGetVariablesNormalizedRedundentVariables()
        {
            Formula f = new Formula("A1 + a1 * C3 / c3 - E5", s => s.ToUpper(), s => true);

            HashSet<String> expected = new HashSet<String> { "A1", "C3", "E5" };

            List<String> result = (List<String>)f.GetVariables();

            //Check size equality
            Assert.AreEqual(expected.Count, result.Count);

            //Check element equality
            foreach (String s in result)
            {
                Assert.IsTrue(expected.Contains(s));
            }
        }

        /*
         * ToString Tests
         */

        /// <summary>
        /// Checks to make sure there is no whitespace in the returned string
        /// </summary>
        [TestMethod]
        public void PublicToStringCheckForSpaces()
        {
            Formula f = new Formula("5 + B2 - 3");

            Assert.AreEqual("5+B2-3", f.ToString());
        }

        /// <summary>
        /// Checks to make the formula created by the string is the same as the original formula
        /// </summary>
        [TestMethod]
        public void PublicToStringResultingFormulaEquality()
        {
            Formula f = new Formula("5 + B2 - 3");
            Formula g = new Formula(f.ToString());

            Assert.IsTrue(f.Equals(g));
        }

        /*
         * Equals Tests
         */

        /// <summary>
        /// Checks that it returns false when the parameter object is null
        /// </summary>
        [TestMethod]
        public void PublicEqualsParameterNull()
        {
            Formula f = new Formula("5 + 3");
            Formula g = null;

            Assert.IsFalse(f.Equals(g));
        }

        /// <summary>
        /// Checks equals on a non Formula Obj
        /// </summary>
        [TestMethod]
        public void PublicEqualsNonFormulaParameter()
        {
            Formula f = new Formula("5 + 3");

            Assert.IsFalse(f.Equals("Hello"));
        }

        /// <summary>
        /// Checks equality of formulas whose strings are not equal but their normalized versions are
        /// </summary>
        [TestMethod]
        public void PublicEqualsNormalizedEquality()
        {
            Formula f = new Formula("A1 + A1 * C3 / C3 - e5", s => s.ToUpper(), s => true);
            Formula g = new Formula("a1 + a1 * c3 / c3 - E5", s => s.ToUpper(), s => true);

            Assert.IsTrue(f.Equals(g));
            Assert.IsTrue(g.Equals(f));
        }

        /// <summary>
        /// Checks the equals method on a boring typical example
        /// </summary>
        [TestMethod]
        public void PublicEqualsBoring()
        {
            Formula f = new Formula("A1 + A1 * C3 / C3 - E5", s => s.ToUpper(), s => true);
            Formula g = new Formula("A1 + A1 * C3 / C3 - E5", s => s.ToUpper(), s => true);

            Assert.IsTrue(f.Equals(g));
            Assert.IsTrue(g.Equals(f));
        }

        /// <summary>
        /// Checks equals in a case where they are not equal
        /// </summary>
        [TestMethod]
        public void PublicEqualsNotEqual()
        {
            Formula f = new Formula("A1 + A1 * C3 / C3 - E5", s => s.ToUpper(), s => true);
            Formula g = new Formula("B3*5", s => s.ToUpper(), s => true);

            Assert.IsFalse(f.Equals(g));
            Assert.IsFalse(g.Equals(f));
        }

        /// <summary>
        /// Checks equals in a case where one of the doubles is so slightly larger than a double that they will be equal
        /// </summary>
        [TestMethod]
        public void PublicEqualsCheckMinusculeNumberDifferences()
        {
            Formula f = new Formula("A1 * 2.0000000000000000001", s => s.ToUpper(), s => true);
            Formula g = new Formula("A1 * 2.0", s => s.ToUpper(), s => true);

            Assert.IsTrue(f.Equals(g));
            Assert.IsTrue(g.Equals(f));
        }

        /*
         * == Tests
         */

        /// <summary>
        /// Checks == in a boring typical case
        /// </summary>
        [TestMethod]
        public void PublicEQBoring()
        {
            Formula f = new Formula("A1 + A1 * C3 / C3 - E5", s => s.ToUpper(), s => true);
            Formula g = new Formula("A1 + A1 * C3 / C3 - E5", s => s.ToUpper(), s => true);

            Assert.IsTrue(f == g);
            Assert.IsTrue(g == f);
        }
        
        /// <summary>
        /// Checks == in a boring typical case
        /// </summary>
        [TestMethod]
        public void PublicEQNotEqual()
        {
            Formula f = new Formula("A1 + A1 * C3 / C3 - E5", s => s.ToUpper(), s => true);
            Formula g = new Formula("B3*5", s => s.ToUpper(), s => true);

            bool result = f == g;

            Assert.IsFalse(result);
            Assert.IsFalse(g == f);
        }

        /// <summary>
        /// Checks that it returns true when both are null
        /// </summary>
        [TestMethod]
        public void PublicEQBothNull()
        {
            Formula f = null;
            Formula g = null;

            Assert.IsTrue(f == g);
        }

        /// <summary>
        /// Checks that it returns false when the calling object is null
        /// </summary>
        [TestMethod]
        public void PublicEQCallingNull()
        {
            Formula f = null;
            Formula g = new Formula("5 + 3");

            Assert.IsFalse(f == g);
        }

        /// <summary>
        /// Checks that it returns false when the parameter object is null
        /// </summary>
        [TestMethod]
        public void PublicEQParameterNull()
        {
            Formula f = null;
            Formula g = new Formula("5 + 3");

            Assert.IsFalse(g == f);
        }

        /*
         * != Tests
         */

        /// <summary>
        /// Checks != in a boring typical case
        /// </summary>
        [TestMethod]
        public void PublicNEQBoring()
        {
            Formula f = new Formula("A1 + A1 * C3 / C3 - E5", s => s.ToUpper(), s => true);
            Formula g = new Formula("A3 - E5", s => s.ToUpper(), s => true);

            Assert.IsTrue(f != g);
            Assert.IsTrue(g != f);
        }

        /// <summary>
        /// Checks != in a boring typical case
        /// </summary>
        [TestMethod]
        public void PublicNEQEqual()
        {
            Formula f = new Formula("A1 + A1 * C3 / C3 - E5", s => s.ToUpper(), s => true);
            Formula g = new Formula("A1 + A1 * C3 / C3 - E5", s => s.ToUpper(), s => true);

            Assert.IsFalse(f != g);
            Assert.IsFalse(g != f);
        }

        /// <summary>
        /// Checks that it returns false when both are null
        /// </summary>
        [TestMethod]
        public void PublicNEQBothNull()
        {
            Formula f = null;
            Formula g = null;

            Assert.IsFalse(f != g);
        }

        /// <summary>
        /// Checks that it returns true when the calling object is null
        /// </summary>
        [TestMethod]
        public void PublicNEQCallingNull()
        {
            Formula f = null;
            Formula g = new Formula("5 + 3");

            Assert.IsTrue(f != g);
        }

        /// <summary>
        /// Checks that it returns true when the parameter object is null
        /// </summary>
        [TestMethod]
        public void PublicNEQParameterNull()
        {
            Formula f = null;
            Formula g = new Formula("5 + 3");

            Assert.IsTrue(g != f);
        }

        /*
         * GetHashCode Tests
         */

        /// <summary>
        /// Checks that two different formulas have different Hashcodes.  This might fail in rare instances but shouldn't fail dependably
        /// </summary>
        [TestMethod]
        public void PublicGetHashCodeStressTest()
        {
            Random rando = new Random();

            // checks a bunch of different pairs of 
            for (int i = 0; i < 10000; i++)
            {
                int charint1 = rando.Next(25) + 65;
                char char1 = (char) charint1;
                Formula f = new Formula("" + char1 + rando.Next(10000));

                int charint2 = rando.Next(25) + 65;
                char char2 = (char)charint2;
                Formula g = new Formula("" + char2 + rando.Next(10000));

                Assert.IsFalse(f.GetHashCode() == g.GetHashCode());
            }
        }

        /// <summary>
        /// Checks to make sure that when two formulas equal each other their hashcodes also equal each other.
        /// </summary>
        [TestMethod]
        public void PublicGetHashCodeSameNormalizedFormulasSameHashCode()
        {
            Formula f = new Formula("a1 + a1 * c3 / c3 - e5", s => s.ToUpper(), s => true);
            Formula g = new Formula("A1 + A1 * C3 / C3 - E5", s => s.ToUpper(), s => true);

            Assert.AreEqual(f.GetHashCode(), g.GetHashCode());
        }

        /// <summary>
        /// Checks to make sure that when two formulas equal each other exactly their hashcodes also equal each other.
        /// </summary>
        [TestMethod]
        public void PublicGetHashCodeSameFormulasSameHashCode()
        {
            Formula f = new Formula("A1 + A1 * C3 / C3 - E5", s => s.ToUpper(), s => true);
            Formula g = new Formula("A1 + A1 * C3 / C3 - E5", s => s.ToUpper(), s => true);

            Assert.AreEqual(f.GetHashCode(), g.GetHashCode());
        }


        /*
         * Here are my copied tests.  I will have to throw out / alter most
         */

        /// <summary>
        /// Tests Evaluate with only one operation
        /// </summary>
        [TestMethod]
        public void PublicEvaluateOneOperation()
        {

            Formula f = new Formula("2 + 5", s => s.ToUpper(), s => true);

            Assert.AreEqual(7, f.Evaluate(s=>0));
        }

        /// <summary>
        /// Makes sure it works without doing any operations
        /// </summary>
        [TestMethod]
        public void PublicEvaluateNoOperations()
        {
            Formula f = new Formula("2", s => s.ToUpper(), s => true);
            Assert.AreEqual(2, f.Evaluate(s=>0));
        }

        /// <summary>
        /// Checks to make sure subtraction works
        /// </summary>
        [TestMethod]
        public void PublicEvaluateSubtraction()
        {
            Formula f = new Formula("4-1", s => s.ToUpper(), s => true);

            Assert.AreEqual(3, f.Evaluate(s=>0));
        }

        /// <summary>
        /// Makes sure we can get a negative result
        /// </summary>
        [TestMethod]
        public void PublicEvaluateSubtractionNegativeResult()
        {
            Formula f = new Formula("2-5", s => s.ToUpper(), s => true);

            Assert.AreEqual(-3, f.Evaluate(s => 0));
        }

        /// <summary>
        /// Makes sure it works with multiple additions
        /// </summary>
        [TestMethod]
        public void PublicEvaluateMultipleAdds()
        {
            Formula f = new Formula("2 - 5 + 15-64+45+13", s => s.ToUpper(), s => true);

            Assert.AreEqual(6, f.Evaluate(s => 0));
        }

        [TestMethod]
        public void PublicEvaluateDivision()
        {
            Formula f = new Formula("4 /2", s => s.ToUpper(), s => true);

            Assert.AreEqual(2, f.Evaluate(s => 0));
        }

        [TestMethod]
        public void PublicEvaluateMultiplication()
        {
            Formula f = new Formula("4* 2", s => s.ToUpper(), s => true);

            Assert.AreEqual(8, f.Evaluate(s => 0));
        }

        [TestMethod]
        public void PublicEvaluateMultipleMultiplications()
        {
            Formula f = new Formula("4 /2*5", s => s.ToUpper(), s => true);

            Assert.AreEqual(10, f.Evaluate(s => 0));
        }

        [TestMethod]
        public void PublicEvaluateSimpleParenthesis()
        {
            Formula f = new Formula("(5)", s => s.ToUpper(), s => true);

            Assert.AreEqual(5, f.Evaluate(s => 0));
        }

        [TestMethod]
        public void PublicEvaluateOperationinParenthesis()
        {
            Formula f = new Formula("(4 /2)", s => s.ToUpper(), s => true);

            Assert.AreEqual(2, f.Evaluate(s => 0));
        }

        [TestMethod]
        public void PublicEvaluateThrowInSomeParenthesis()
        {
            Formula f = new Formula("(4 /2) - (10*3)/15", s => s.ToUpper(), s => true);

            Assert.AreEqual(0, f.Evaluate(s => 0));
        }

        [TestMethod]
        public void PublicEvaluateDivideByZero()
        {
            Formula f = new Formula("4/ 0", s => s.ToUpper(), s => true);

            Assert.IsTrue(f.Evaluate(s => 0) is FormulaError);
        }


        /*
         * Tests with variables
         */
        [TestMethod]
        public void PublicEvaluateWeirdTokens1()
        {
            Formula f = new Formula("spaghetti - 3&@sd", s => s.ToUpper(), s => true);

            Assert.AreEqual(0, f.Evaluate(s => 0));
        }

        // Just using the zero lookup
        [TestMethod]
        public void PublicEvaluateSimpleUsingZero()
        {
            Formula f = new Formula("5*A1 + 2", s => s.ToUpper(), s => true);

            Assert.AreEqual(2, f.Evaluate(s => 0));
        }

        // Using the limited lookup
        [TestMethod]
        public void PublicEvaluateJustLookUp1()
        {
            Formula f = new Formula("A1", s => s.ToUpper(), s => true);

            Assert.AreEqual(2, f.Evaluate(LimitedLookup));
        }

        // Using the limited lookup
        [TestMethod]
        public void PublicEvaluateJustLookUp2()
        {
            Formula f = new Formula("C3", s => s.ToUpper(), s => true);

            Assert.AreEqual(6, f.Evaluate(LimitedLookup));
        }

        // Using the limited lookup
        [TestMethod]
        public void PublicEvaluateJustLookUp3()
        {
            Formula f = new Formula("XyZ321", s => s.ToUpper(), s => true);

            Assert.AreEqual(85, f.Evaluate(LimitedLookup));
        }

        [TestMethod]
        public void PublicEvaluateOneVariableLimited()
        {
            Formula f = new Formula("(D4+4)/3", s => s.ToUpper(), s => true);

            Assert.AreEqual(4, f.Evaluate(LimitedLookup));
        }

        [TestMethod]
        public void PublicEvaluateMultipleVariablesLimited()
        {
            Formula f = new Formula("(C3+B2)/A1", s => s.ToUpper(), s => true);

            Assert.AreEqual(5, f.Evaluate(LimitedLookup));
        }

        [TestMethod]
        public void PublicEvaluateMultipleOperationsInParenthesisLimited()
        {
            Formula f = new Formula("(C3+B2*B2+C3)/A1", s => s.ToUpper(), s => true);

            Assert.AreEqual(14, f.Evaluate(LimitedLookup));
        }

        [TestMethod]
        public void PublicEvaluateDivideByZeroUsingVariable()
        {

            Formula f = new Formula("2 / A1", s => s.ToUpper(), s => true);

            Assert.IsTrue(f.Evaluate(LimitedLookup) is FormulaError);
        }

        [TestMethod]
        public void PublicEvaluateLongExpression()
        {

            Formula f = new Formula("5+13*(A1+16)/9+D4*3-84+B2+(13*0)", s => s.ToUpper(), s => true);

            Assert.AreEqual(-25, f.Evaluate(LimitedLookup));
        }

        [TestMethod]
        public void PublicEvaluateEmptyVariable1()
        {

            Formula f = new Formula("A2", s => s.ToUpper(), s => true);

            Assert.IsTrue(f.Evaluate(LimitedLookup) is FormulaError);
        }

        [TestMethod]
        public void PublicEvaluateEmptyVariable2()
        {

            Formula f = new Formula("A2 + 3*(B1/15)", s => s.ToUpper(), s => true);

            Assert.IsTrue(f.Evaluate(LimitedLookup) is FormulaError);
        }

        /// <summary>
        /// A simple lookup method to test some junk out
        /// </summary>
        /// <param name="s">Variable's name</param>
        /// <returns> the variable's name</returns>
        public static double LimitedLookup(String s)
        {
            if (s == "A1") { return 2; }
            if (s == "B2") { return 4; }
            if (s == "C3") { return 6; }
            if (s == "D4") { return 8; }
            if (s == "XyZ321") { return 85; }
            throw new ArgumentException("Variable DNE");
        }
    }
}
