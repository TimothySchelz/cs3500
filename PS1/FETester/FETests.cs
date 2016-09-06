using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static FormulaEvaluator.Evaluator;


namespace FETester
{
    [TestClass]
    public class FETests
    {
        public static int ZeroLookup(String s)
        {
            return 0;
        }

        Lookup zero = ZeroLookup;

        // Successfull tests with no variables
        [TestMethod]
        public void OneOperation()
        {
            Assert.AreEqual(7, Evaluate("2 + 5", zero));
            Assert.AreEqual(7, Evaluate("5 + 2", zero));
        }

        [TestMethod]
        public void NoOperations()
        {
            Assert.AreEqual(2, Evaluate("2", zero));
        }

        [TestMethod]
        public void subtraction()
        {
            Assert.AreEqual(3, Evaluate("5-2", zero));
        }

        [TestMethod]
        public void SubtractionNegativeResult()
        {
            Assert.AreEqual(-3, Evaluate("2 - 5", zero));
        }

        [TestMethod]
        public void MultipleAdds()
        {
            Assert.AreEqual(6, Evaluate("2 - 5 + 15-64+45+13", zero));
        }

        [TestMethod]
        public void Division()
        {
            Assert.AreEqual(2, Evaluate("4/2", zero));
            Assert.AreEqual(2, Evaluate("4 / 2", zero));
            Assert.AreEqual(2, Evaluate("4 /2", zero));
            Assert.AreEqual(2, Evaluate("4/ 2", zero));
        }

        [TestMethod]
        public void Multiplication()
        {
            Assert.AreEqual(8, Evaluate("4*2", zero));
            Assert.AreEqual(8, Evaluate("4 * 2", zero));
            Assert.AreEqual(8, Evaluate("4 *2", zero));
            Assert.AreEqual(8, Evaluate("4* 2", zero));
        }

        [TestMethod]
        public void MultipleMultiplications()
        {
            Assert.AreEqual(4, Evaluate("2*4/2", zero));
            Assert.AreEqual(12, Evaluate("16/4*3", zero));
            Assert.AreEqual(8, Evaluate("4 *2*1*1*1", zero));
            Assert.AreEqual(1, Evaluate("4/2/2", zero));
        }

        [TestMethod]
        public void SimpleParenthesis()
        {
            Assert.AreEqual(5, Evaluate("(5)", zero));
        }

        [TestMethod]
        public void OperationinParenthesis()
        {
            Assert.AreEqual(5, Evaluate("(3+2)", zero));
            Assert.AreEqual(6, Evaluate("(3*2)", zero));
        }

        [TestMethod]
        public void ThrowInSomeParenthesis()
        {
            Assert.AreEqual(4, Evaluate("2*(5-3)", zero));
            Assert.AreEqual(4, Evaluate("16/(2*2)", zero));
            Assert.AreEqual(2, Evaluate("(4+6)/5", zero));
            Assert.AreEqual(1, Evaluate("(2+14)/(3+13)", zero));
            Assert.AreEqual(1, Evaluate("(2+14)*(3+13)", zero));
        }

        [TestMethod]
        public void IntegerDivison()
        {
            Assert.AreEqual(2, Evaluate("5/2", zero));
            Assert.AreEqual(2, Evaluate("(7-2)/(1+1)", zero));
        }
    }
}