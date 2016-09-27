// Written by Timothy Schelz, u0851027, September 2016

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using SpreadsheetUtilities;
using System.Collections.Generic;

namespace SpreadsheetTests
{
        /*
         * Constructor Tests
         * 
         * Not much to test here.  Just going to make sure it is empty
         */
        /// <summary>
        /// Makes sure a new spreadsheet is empty.  Since IEnumerable does not have a
        /// count or size or anything we have to use the loop
        /// </summary>
        [TestMethod]
        public void Public_Con_MakeSureItIsEmpty()
        {
            Spreadsheet s = new Spreadsheet();

            foreach (String name in s.GetNamesOfAllNonemptyCells())
            {
                Assert.Fail();
            }
            Assert.IsTrue(true);
        }

        /*
         * GetNamesOfAllNonemptyCells Tests
         */
         /// <summary>
         /// Tests the method on an empty Spreadsheet
         /// </summary>
        [TestMethod]
        public void Public_GetNamesOfAllNonemptyCells_EmptySS()
        {
            Spreadsheet s = new Spreadsheet();

            foreach (String name in s.GetNamesOfAllNonemptyCells())
            {
                Assert.Fail();
            }
            Assert.IsTrue(true);
        }

        /// <summary>
        /// Tests a standard case wheree a SS has some filled cells
        /// </summary>
        [TestMethod]
        public void Public_GetNamesOfAllNonemptyCells_StandardCase()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", "String 1");
            s.SetCellContents("B2", new Formula("2+2"));
            s.SetCellContents("C3", new Formula("B2+D4"));
            s.SetCellContents("D4", 1.0);

            HashSet<String> expected = new HashSet<string>();
            expected.Add("A1");
            expected.Add("B2");
            expected.Add("C3");
            expected.Add("D4");

            foreach (String name in s.GetNamesOfAllNonemptyCells())
            {
                Assert.IsTrue(expected.Contains(name));
            }
        }

        /// <summary>
        /// Used set contents on an already existent cell.  Makes sure there are no duplicates
        /// </summary>
        [TestMethod]
        public void Public_GetNamesOfAllNonemptyCells_ResetVariable()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", "String 1");
            s.SetCellContents("B2", new Formula("2+2"));
            s.SetCellContents("C3", new Formula("B2+D4"));
            s.SetCellContents("D4", 1.0);
            s.SetCellContents("D4", 2.0);
            s.SetCellContents("D4", 3.0);

            HashSet<String> expected = new HashSet<string>();
            expected.Add("A1");
            expected.Add("B2");
            expected.Add("C3");
            expected.Add("D4");

            foreach (String name in s.GetNamesOfAllNonemptyCells())
            {
                Assert.IsTrue(expected.Contains(name));
                // Removes ones that have already been checked.  So if there is a duplicate it wont be in
                // expected the second time
                expected.Remove(name);
            }
        }

        /*
         * GetCellContents Tests
         */

        /// <summary>
        /// Tests a standard case where the cell has a string in it
        /// </summary>
        [TestMethod]
        public void Public_GetCellContents_StringCell()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", "String 1");
            s.SetCellContents("B2", new Formula("2+2"));
            s.SetCellContents("C3", new Formula("B2+D4"));
            s.SetCellContents("D4", 1.0);

            Assert.IsTrue("String 1".Equals(s.GetCellContents("A1")));
        }

        /// <summary>
        /// Tests a standard case where the cell has a double in it
        /// </summary>
        [TestMethod]
        public void Public_GetCellContents_DoubleCell()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", "String 1");
            s.SetCellContents("B2", new Formula("2+2"));
            s.SetCellContents("C3", new Formula("B2+D4"));
            s.SetCellContents("D4", 1.0);

            Assert.IsTrue(s.GetCellContents("D4").Equals(1.0));
        }

        /// <summary>
        /// Tests a standard case where the cell has a Formula in it
        /// </summary>
        [TestMethod]
        public void Public_GetCellContents_FormulaCell()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", "String 1");
            s.SetCellContents("B2", new Formula("2+2"));
            s.SetCellContents("C3", new Formula("B2+D4"));
            s.SetCellContents("D4", 1.0);

            Assert.IsTrue(s.GetCellContents("C3").Equals(new Formula("B2+D4")));
        }

        /// <summary>
        /// Tests a null name.  It should throw an ArgumentNullException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Public_GetCellContents_NullName()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", "String 1");
            s.SetCellContents("B2", new Formula("2+2"));
            s.SetCellContents("C3", new Formula("B2+D4"));
            s.SetCellContents("D4", 1.0);

            s.GetCellContents(null);
        }

        /// <summary>
        /// Tests an invalid name.  It should throw an InvalidNameException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void Public_GetCellContents_InvalidName1()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", "String 1");
            s.SetCellContents("B2", new Formula("2+2"));
            s.SetCellContents("C3", new Formula("B2+D4"));
            s.SetCellContents("D4", 1.0);

            s.GetCellContents("12_sdf");
        }

        /// <summary>
        /// Tests an invalid name.  It should throw an InvalidNameException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void Public_GetCellContents_InvalidName2()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", "String 1");
            s.SetCellContents("B2", new Formula("2+2"));
            s.SetCellContents("C3", new Formula("B2+D4"));
            s.SetCellContents("D4", 1.0);

            s.GetCellContents("&&&");
        }

        /// <summary>
        /// Tests an it on a cell that has not been used yet
        /// </summary>
        [TestMethod]
        public void Public_GetCellContents_EmptyCell()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", "String 1");
            s.SetCellContents("B2", new Formula("2+2"));
            s.SetCellContents("C3", new Formula("B2+D4"));
            s.SetCellContents("D4", 1.0);

            Assert.IsTrue(s.GetCellContents("E5").Equals(""));
        }

        /*
         * SetCellContents to double Tests
         */

        /// <summary>
        /// Tests a null name.  It should throw an ArgumentNullException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Public_SetCellContents_DoubleNullName()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", "String 1");
            s.SetCellContents("B2", new Formula("2+2"));
            s.SetCellContents("C3", new Formula("B2+D4"));
            s.SetCellContents("D4", 1.0);

            s.SetCellContents(null, 2.52);
        }

        /// <summary>
        /// Tests an invalid name.  It should throw an InvalidNameException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void Public_SetCellContents_DoubleInvalidName1()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", "String 1");
            s.SetCellContents("B2", new Formula("2+2"));
            s.SetCellContents("C3", new Formula("B2+D4"));
            s.SetCellContents("D4", 1.0);

            s.SetCellContents("12_sdf", 2.52);
        }

        /// <summary>
        /// Tests an invalid name.  It should throw an InvalidNameException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void Public_SetCellContents_DoubleInvalidName2()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", "String 1");
            s.SetCellContents("B2", new Formula("2+2"));
            s.SetCellContents("C3", new Formula("B2+D4"));
            s.SetCellContents("D4", 1.0);

            s.SetCellContents("&&&", 2.52);
        }

        /// <summary>
        /// Tests to make sure that the method can create new cells with the correct contents
        /// </summary>
        [TestMethod]
        public void Public_SetCellContents_DoubleCreatesNewCells()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", "String 1");
            s.SetCellContents("B2", new Formula("2+2"));
            s.SetCellContents("C3", new Formula("B2+D4"));
            s.SetCellContents("D4", 1.0);

            Assert.IsTrue(s.GetCellContents("D4").Equals(1.0));
        }

        /// <summary>
        /// Tests to make sure that the method can overwrite cells
        /// </summary>
        [TestMethod]
        public void Public_SetCellContents_DoubleOverwriteCells()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", "String 1");
            s.SetCellContents("B2", new Formula("2+2"));
            s.SetCellContents("C3", new Formula("B2+D4"));
            s.SetCellContents("D4", 1.0);
            s.SetCellContents("D4", 15.2);

            Assert.IsTrue(s.GetCellContents("D4").Equals(15.2));
        }

        /// <summary>
        /// Makes sure it returns a set with just this cell's name if it has no dependents
        /// </summary>
        [TestMethod]
        public void Public_SetCellContents_DoubleNoDependents()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", "String 1");
            s.SetCellContents("B2", new Formula("2+2"));
            s.SetCellContents("C3", new Formula("B2+D4"));

            ISet<String> result = s.SetCellContents("D4", 1.0);

            ISet<String> expected = new HashSet<String>();
            expected.Add("D4");

            // Makes sure they are the same size and then that each element of one is in the other
            Assert.AreEqual(result.Count, expected.Count);
            foreach (String name in result)
            {
                Assert.IsTrue(expected.Contains(name));
            }
        }

        /*
         * SetCellContents to String Tests
         */

        /// <summary>
        /// Tests a null name.  It should throw an ArgumentNullException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Public_SetCellContents_StringNullName()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", "String 1");
            s.SetCellContents("B2", new Formula("2+2"));
            s.SetCellContents("C3", new Formula("B2+D4"));
            s.SetCellContents("D4", 1.0);

            s.SetCellContents(null, "Hello");
        }

        /// <summary>
        /// Tests an invalid name.  It should throw an InvalidNameException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void Public_SetCellContents_StringInvalidName1()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", "String 1");
            s.SetCellContents("B2", new Formula("2+2"));
            s.SetCellContents("C3", new Formula("B2+D4"));
            s.SetCellContents("D4", 1.0);

            s.SetCellContents("12_sdf", "Hello");
        }

        /// <summary>
        /// Tests an invalid name.  It should throw an InvalidNameException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void Public_SetCellContents_StringInvalidName2()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", "String 1");
            s.SetCellContents("B2", new Formula("2+2"));
            s.SetCellContents("C3", new Formula("B2+D4"));
            s.SetCellContents("D4", 1.0);

            s.SetCellContents("&&&", "Hello");
        }

        /// <summary>
        /// Tests to make sure that the method can create new cells with the correct contents
        /// </summary>
        [TestMethod]
        public void Public_SetCellContents_StringCreatesNewCells()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("B2", new Formula("2+2"));
            s.SetCellContents("C3", new Formula("B2+D4"));
            s.SetCellContents("D4", 1.0);

            Assert.IsTrue(s.GetCellContents("A1").Equals("String 1"));
        }

        /// <summary>
        /// Tests to make sure that the method can overwrite cells
        /// </summary>
        [TestMethod]
        public void Public_SetCellContents_StringOverwriteCells()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", "String 1");
            s.SetCellContents("B2", new Formula("2+2"));
            s.SetCellContents("C3", new Formula("B2+D4"));
            s.SetCellContents("A1", "String 2");


            Assert.IsTrue(s.GetCellContents("A1").Equals("String 2"));
        }

        /// <summary>
        /// Makes sure it returns a set with just this cell's name if it has no dependents
        /// </summary>
        [TestMethod]
        public void Public_SetCellContents_StringNoDependents()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("B2", new Formula("2+2"));
            s.SetCellContents("C3", new Formula("B2+D4"));

            ISet<String> result = s.SetCellContents("A1", "String 1");

            ISet<String> expected = new HashSet<String>();
            expected.Add("A1");

            // Makes sure they are the same size and then that each element of one is in the other
            Assert.AreEqual(result.Count, expected.Count);
            foreach (String name in result)
            {
                Assert.IsTrue(expected.Contains(name));
            }
        }

        /*
         * SetCellContents to Formula Tests
         */

        /// <summary>
        /// Tests a null name.  It should throw an ArgumentNullException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Public_SetCellContents_FormulaNullName()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", "String 1");
            s.SetCellContents("B2", new Formula("2+2"));
            s.SetCellContents("C3", new Formula("B2+D4"));
            s.SetCellContents("D4", 1.0);

            s.SetCellContents(null, new Formula("5+A1"));
        }

        /// <summary>
        /// Tests an invalid name.  It should throw an InvalidNameException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void Public_SetCellContents_FormulaInvalidName1()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", "String 1");
            s.SetCellContents("B2", new Formula("2+2"));
            s.SetCellContents("C3", new Formula("B2+D4"));
            s.SetCellContents("D4", 1.0);

            s.SetCellContents("12_sdf", new Formula("5+A1"));
        }

        /// <summary>
        /// Tests an invalid name.  It should throw an InvalidNameException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void Public_SetCellContents_FormulaInvalidName2()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", "String 1");
            s.SetCellContents("B2", new Formula("2+2"));
            s.SetCellContents("C3", new Formula("B2+D4"));
            s.SetCellContents("D4", 1.0);

            s.SetCellContents("&&&", new Formula("5+A1"));
        }

        /// <summary>
        /// Makes sure it returns a set with just this cell's name if it has no dependents
        /// </summary>
        [TestMethod]
        public void Public_SetCellContents_FormulaNoDependents()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", "String 1");
            s.SetCellContents("B2", new Formula("2+2"));
            s.SetCellContents("C3", new Formula("B2+D4"));

            ISet<String> result = s.SetCellContents("D4", 1.0);

            ISet<String> expected = new HashSet<String>();
            expected.Add("D4");

            // Makes sure they are the same size and then that each element of one is in the other
            Assert.AreEqual(result.Count, expected.Count);
            foreach (String name in result)
            {
                Assert.IsTrue(expected.Contains(name));
            }
        }

        /// <summary>
        /// Makes sure it returns a set with itself, and direct dependents
        /// </summary>
        [TestMethod]
        public void Public_SetCellContents_FormulaDirectDependents()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", "String 1");
            s.SetCellContents("B2", new Formula("2+2"));
            s.SetCellContents("D4", 1.0);

            ISet<String> result = s.SetCellContents("C3", new Formula("B2+D4"));

            ISet<String> expected = new HashSet<String>();
            expected.Add("D4");
            expected.Add("B2");
            expected.Add("C3");

            // Makes sure they are the same size and then that each element of one is in the other
            Assert.AreEqual(result.Count, expected.Count);
            foreach (String name in result)
            {
                Assert.IsTrue(expected.Contains(name));
            }
        }

        /// <summary>
        /// Makes sure it returns a set with itself, and direct dependents, and indirect dependents
        /// </summary>
        [TestMethod]
        public void Public_SetCellContents_FormulaIndirectDependents()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", 5);
            s.SetCellContents("B2", new Formula("A1"));
            s.SetCellContents("D4", 1.0);

            ISet<String> result = s.SetCellContents("C3", new Formula("B2+D4"));

            ISet<String> expected = new HashSet<String>();
            expected.Add("D4");
            expected.Add("B2");
            expected.Add("C3");
            expected.Add("A1");

            // Makes sure they are the same size and then that each element of one is in the other
            Assert.AreEqual(result.Count, expected.Count);
            foreach (String name in result)
            {
                Assert.IsTrue(expected.Contains(name));
            }
        }

        /// <summary>
        /// Checks to make sure an exception is thrown when there is a circular dependency
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void Public_SetCellContents_FormulaCircularDependency()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", "D4");
            s.SetCellContents("B2", new Formula("A1"));
            s.SetCellContents("C3", new Formula("B2+D4"));
            s.SetCellContents("D4", "B2");

            s.SetCellContents("&&&", new Formula("5+A1"));
        }

        /// <summary>
        /// Checks to make sure an exception is thrown when there is a circular dependency
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void Public_SetCellContents_FormulaDirectCircularDependency()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", "D4");
            s.SetCellContents("D4", "A1");
        }
    }
}
