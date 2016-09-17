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
        public void Con1InvalidNonsense()
        {
            Formula f = new Formula("+++");
        }

        /// <summary>
        /// Tests creating a Formula with an invalid input
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Con1InvalidInvalidCharacter()
        {
            Formula f = new Formula("4 ^ 15");
        }

        /// <summary>
        /// Tests creating a Formula with an invalid input, no valid tokens
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Con1InvalidNoTokens()
        {
            Formula f = new Formula("");
        }

        /// <summary>
        /// Tests creating a Formula with no operations between variables
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Con1InvalidVariablesNoOperations()
        {
            Formula f = new Formula("x y + y x");
        }

        /// <summary>
        /// Tests the small constructor with a formula with no variables
        /// </summary>
        [TestMethod]
        public void Con1NoVariableInput()
        {
            Formula f = new Formula("13 - 15*(1/2)");

            Assert.AreEqual("13-15*(1/2)", f.ToString());
        }

        /// <summary>
        /// Checks the itty bitty constructor with a formula with variables
        /// </summary>
        [TestMethod]
        public void Con1YesVariableInput()
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
        public void Con2NormalizerChangesFormula()
        {
            Formula f = new Formula("a1 + a1 * c3 / c3 - e5", s => s.ToUpper(), s => true);

            Assert.AreEqual("A1 + A1 * C3 / C3 - E5", f.ToString());
        }

        /// <summary>
        /// Checks the rotund constructor's use of the normalizer on things that don't need it
        /// </summary>
        [TestMethod]
        public void Con2NormalizerIsUnnecessary()
        {
            Formula f = new Formula("A1 + A1 * C3 / C3 - E5", s => s.ToUpper(), s => true);

            Assert.AreEqual("A1+A1*C3/C3-E5", f.ToString());
        }

        /// <summary>
        /// Checks that the IsValid delegate is used correcty on a valid variable
        /// </summary>
        [TestMethod]
        public void Con2IsValidCheckOnValidVariable()
        {
            Formula f = new Formula("A1 + A1 * 5", s => s.ToUpper(), s => s == "A1");

            Assert.AreEqual("A1+A1*5", f.ToString());
        }

        /// <summary>
        /// Checks that the FormulaFormatException is thrown
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Con2IsValidFormulaFormatExceptionThrowDown()
        {
            Formula f = new Formula("A2 + B1 * 5", s => s.ToUpper(), s => s == "A1");
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
        public void GetVariablesNormalCase()
        {
            Formula f = new Formula("A1 + B2 * C3 / D4 - E5");

            HashSet<String> expected = new HashSet<String> { "A1", "B2", "C3", "D4", "E5" };

            HashSet<String> result = (HashSet<String>) f.GetVariables();

            //Check size equality
            Assert.AreEqual(expected.Count, result.Count);

            //Check element equality
            foreach (String s in result)
            {
                Assert.IsTrue(expected.Contains(s));
            }
        }

        /// <summary>
        /// Checks the method when there are no variables to be gotten
        /// </summary>
        [TestMethod]
        public void GetVariablesEmptyCase()
        {
            Formula f = new Formula("5 + 4 - 3");

            HashSet<String> expected = new HashSet<String> {};

            HashSet<String> result = (HashSet<String>)f.GetVariables();

            //Check size equality
            Assert.AreEqual(expected.Count, result.Count);
        }

        /// <summary>
        /// Checks to make sure that when there are variables that are normalized to the same thing only 1 copy of it is returned
        /// </summary>
        [TestMethod]
        public void GetVariablesNormalizedRedundentVariables()
        {
            Formula f = new Formula("A1 + a1 * C3 / c3 - E5", s => s.ToUpper(), s => true);

            HashSet<String> expected = new HashSet<String> { "A1", "C3", "E5" };

            HashSet<String> result = (HashSet<String>)f.GetVariables();

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
        public void ToStringCheckForSpaces()
        {
            Formula f = new Formula("5 + B2 - 3");

            Assert.AreEqual("5+B2-3", f.ToString());
        }

        /// <summary>
        /// Checks to make the formula created by the string is the same as the original formula
        /// </summary>
        [TestMethod]
        public void ToStringResultingFormulaEquality()
        {
            Formula f = new Formula("5 + B2 - 3");
            Formula g = new Formula(f.ToString());

            Assert.IsTrue(f.Equals(g));
        }

        /*
         * Equals Tests
         */

        /// <summary>
        /// Checks that it returns true when both are null
        /// </summary>
        [TestMethod]
        public void EqualsBothNull()
        {
            Formula f = null;
            Formula g = null;

            Assert.IsTrue(f.Equals(g));
        }

        /// <summary>
        /// Checks that it returns false when the calling object is null
        /// </summary>
        [TestMethod]
        public void EqualsCallingNull()
        {
            Formula f = null;
            Formula g = new Formula("5 + 3");

            Assert.IsTrue(f.Equals(g));
        }

        /// <summary>
        /// Checks that it returns false when the parameter object is null
        /// </summary>
        [TestMethod]
        public void EqualsParameterNull()
        {
            Formula f = new Formula("5 + 3");
            Formula g = null;

            Assert.IsTrue(f.Equals(g));
        }

        /// <summary>
        /// Checks equals on a non Formula Obj
        /// </summary>
        [TestMethod]
        public void EqualsNonFormulaParameter()
        {
            Formula f = new Formula("5 + 3");

            Assert.IsFalse(f.Equals("Hello"));
        }

        /// <summary>
        /// Checks equality of formulas whose strings are not equal but their normalized versions are
        /// </summary>
        [TestMethod]
        public void EqualsNormalizedEquality()
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
        public void EqualsBoring()
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
        public void EqualsNotEqual()
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
        public void EqualsCheckMinusculeNumberDifferences()
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
        public void EQBoring()
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
        public void EQNotEqual()
        {
            Formula f = new Formula("A1 + A1 * C3 / C3 - E5", s => s.ToUpper(), s => true);
            Formula g = new Formula("B3*5", s => s.ToUpper(), s => true);

            Assert.IsFalse(f == g);
            Assert.IsFalse(g == f);
        }

        /// <summary>
        /// Checks that it returns true when both are null
        /// </summary>
        [TestMethod]
        public void EQBothNull()
        {
            Formula f = null;
            Formula g = null;

            Assert.IsTrue(f == g);
        }

        /// <summary>
        /// Checks that it returns flase when the calling object is null
        /// </summary>
        [TestMethod]
        public void EQCallingNull()
        {
            Formula f = null;
            Formula g = new Formula("5 + 3");

            Assert.IsFalse(f == g);
        }

        /// <summary>
        /// Checks that it returns false when the parameter object is null
        /// </summary>
        [TestMethod]
        public void EQParameterNull()
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
        public void NEQBoring()
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
        public void NEQEqual()
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
        public void NEQBothNull()
        {
            Formula f = null;
            Formula g = null;

            Assert.IsFalse(f != g);
        }

        /// <summary>
        /// Checks that it returns true when the calling object is null
        /// </summary>
        [TestMethod]
        public void NEQCallingNull()
        {
            Formula f = null;
            Formula g = new Formula("5 + 3");

            Assert.IsTrue(f != g);
        }

        /// <summary>
        /// Checks that it returns true when the parameter object is null
        /// </summary>
        [TestMethod]
        public void NEQParameterNull()
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
        public void GetHashCodeStressTest()
        {
            Random rando = new Random();

            // checks a bunch of different pairs of 
            for (int i = 0; i < 10000; i++)
            {
                int charint1 = rando.Next(25) + 34;
                char char1 = (char) charint1;
                Formula f = new Formula("" + char1 + rando.Next(10000));

                int charint2 = rando.Next(25) + 34;
                char char2 = (char)charint2;
                Formula g = new Formula("" + char2 + rando.Next(10000));

                Assert.IsFalse(f.GetHashCode() == g.GetHashCode());
            }
        }

        /// <summary>
        /// Checks to make sure that when two formulas equal each other their hashcodes also equal each other.
        /// </summary>
        [TestMethod]
        public void GetHashCodeSameNormalizedFormulasSameHashCode()
        {
            Formula f = new Formula("a1 + a1 * c3 / c3 - e5", s => s.ToUpper(), s => true);
            Formula g = new Formula("A1 + A1 * C3 / C3 - E5", s => s.ToUpper(), s => true);

            Assert.AreEqual(f.GetHashCode(), g.GetHashCode());
        }

        /// <summary>
        /// Checks to make sure that when two formulas equal each other exactly their hashcodes also equal each other.
        /// </summary>
        [TestMethod]
        public void GetHashCodeSameFormulasSameHashCode()
        {
            Formula f = new Formula("A1 + A1 * C3 / C3 - E5", s => s.ToUpper(), s => true);
            Formula g = new Formula("A1 + A1 * C3 / C3 - E5", s => s.ToUpper(), s => true);

            Assert.AreEqual(f.GetHashCode(), g.GetHashCode());
        }
    }
}
