using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static FormulaEvaluator.Evaluator;


namespace FETester
{
    [TestClass]
    public class FETests
    {
        public static int NullLookup(String s)
        {
            return 0;
        }
        Lookup zero = NullLookup;

        // Successfull tests with no variables
        [TestMethod]
        public void OneOperation()
        {
            Assert.AreEqual(7, Evaluate("2 + 5", zero));
        }

        public void NegativeResults()
        {

        }

        //The variable lookup delegates

    }
}