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
        public void Subtraction()
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
            Assert.AreEqual(256, Evaluate("(2+14)*(3+13)", zero));
        }

        [TestMethod]
        public void IntegerDivison()
        {
            Assert.AreEqual(2, Evaluate("5/2", zero));
            Assert.AreEqual(2, Evaluate("(7-2)/(1+1)", zero));
        }

        // Failing tests with no variables
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DivideByZero()
        {
            Evaluate("5/0", zero);
            Evaluate("(7-2)/(1-1)", zero);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DivideByZeroComplicated()
        {
            Evaluate("(7-2)/(1-1)", zero);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TooManyOperations1()
        {
            Evaluate("5//3", zero);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TooManyOperations2()
        {
            Evaluate("(7-2)/+(1+1)", zero);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TooManyOperations3()
        {
            Evaluate("(7-*2)/(1+1)", zero);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TooManyOperations4()
        {
            Evaluate("(7-2)+(1++1)", zero);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TooFewOperations1()
        {
            Evaluate("5 3", zero);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TooFewOperations2()
        {
            Evaluate("(7 2)/+(1+1)", zero);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TooFewOperations3()
        {
            Evaluate("(7-2)/(1+1)4", zero);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TooFewOperations4()
        {
            Evaluate("3(7-2)+(1+1)", zero);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EmptyString()
        {
            Evaluate("", zero);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void OnlyWhiteSpace1()
        {
            Evaluate("   ", zero);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void OnlyWhiteSpace2()
        {
            Evaluate("\n", zero);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void OnlyWhiteSpace3()
        {
            Evaluate("               ", zero);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExtraOpeningParenthesis1()
        {
            Evaluate("2*(5-3)(", zero);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExtraOpeningParenthesis2()
        {
            Evaluate("(16/(2*2)", zero);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExtraOpeningParenthesis3()
        {
            Evaluate("((2+(14()(/(3(+(13()", zero);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExtraClosingParenthesis1()
        {
            Evaluate("2*(5-3))", zero);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExtraClosingParenthesis2()
        {
            Evaluate(")16/(2*2))", zero);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExtraClosingParenthesis3()
        {
            Evaluate("()2+)14))(/)3)+)13))", zero);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExtraEmptyParenthesis1()
        {
            Evaluate("2*(5-3)()", zero);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExtraEmptyParenthesis2()
        {
            Evaluate("16/()(2*2)", zero);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExtraEmptyParenthesis3()
        {
            Evaluate("()(2+14)()()/()(3()+()13())", zero);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidCharacters1()
        {
            Evaluate("^", zero);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidCharacters2()
        {
            Evaluate("18*2/3=2", zero);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidCharacters3()
        {
            Evaluate("[18*2]/3", zero);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidVariables1()
        {
            Evaluate("A3A", zero);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidVariables2()
        {
            Evaluate("5 + 2W5", zero);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidVariables3()
        {
            Evaluate("pppppppp * cm3po0ei9r4390klkdf983", zero);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WeirdTokens1()
        {
            Evaluate("spaghetti - 3&@sd", zero);
        }

        // Successful tests with variables
    }
}