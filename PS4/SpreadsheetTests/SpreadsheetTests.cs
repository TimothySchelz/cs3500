// Written by Timothy Schelz, u0851027, October 2016

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using SpreadsheetUtilities;
using System.Collections.Generic;

namespace SpreadsheetTests
{
    /// <summary>
    /// A Unit test class for testing the Spreadsheet class from PS5
    /// </summary>
    [TestClass]
    public class SpreadsheetTests
    {
        /*
         * 0 arg Constructor Tests
         */
        /// <summary>
        /// Makes sure a new spreadsheet is empty.  Since IEnumerable does not have a
        /// count or size or anything we have to use the loop
        /// </summary>
        [TestMethod]
        public void Public_0argCon_MakeSureItIsEmpty()
        {
            Spreadsheet s = new Spreadsheet();

            foreach (String name in s.GetNamesOfAllNonemptyCells())
            {
                Assert.Fail();
            }
            Assert.IsTrue(true);
        }
        
        /*
         * 3 Arg Constructor
         */

        /*
         * 4 arg Constructor
         */

        /*
         * GetSavedVersion Tests
         */

        /*
         * Save Tests
         */

        /*
         * GetCellValue Tests
         */

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
            s.SetContentsOfCell("A1", "String 1");
            s.SetContentsOfCell("B2", "2+2");
            s.SetContentsOfCell("C3", "B2+D4");
            s.SetContentsOfCell("D4", "1.0");

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
            s.SetContentsOfCell("A1", "String 1");
            s.SetContentsOfCell("B2", "2+2");
            s.SetContentsOfCell("C3", "B2+D4");
            s.SetContentsOfCell("D4", "1.0");
            s.SetContentsOfCell("D4", "2.0");
            s.SetContentsOfCell("D4", "3.0");

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
            s.SetContentsOfCell("A1", "String 1");
            s.SetContentsOfCell("B2", "2+2");
            s.SetContentsOfCell("C3", "B2+D4");
            s.SetContentsOfCell("D4", "1.0");

            Assert.IsTrue("String 1".Equals(s.GetCellContents("A1")));
        }

        /// <summary>
        /// Tests a standard case where the cell has a double in it
        /// </summary>
        [TestMethod]
        public void Public_GetCellContents_DoubleCell()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "String 1");
            s.SetContentsOfCell("B2", "2+2");
            s.SetContentsOfCell("C3", "B2+D4");
            s.SetContentsOfCell("D4", "1.0");

            Assert.IsTrue(s.GetCellContents("D4").Equals(1.0));
        }

        /// <summary>
        /// Tests a standard case where the cell has a Formula in it
        /// </summary>
        [TestMethod]
        public void Public_GetCellContents_FormulaCell()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "String 1");
            s.SetContentsOfCell("B2", "2+2");
            s.SetContentsOfCell("C3", "B2+D4");
            s.SetContentsOfCell("D4", "1.0");

            Assert.IsTrue(s.GetCellContents("C3").Equals(new Formula("B2+D4")));
        }

        /// <summary>
        /// Tests a null name.  It should throw an InvalidNameException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void Public_GetCellContents_NullName()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "String 1");
            s.SetContentsOfCell("B2", "2+2");
            s.SetContentsOfCell("C3", "B2+D4");
            s.SetContentsOfCell("D4", "1.0");

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
            s.SetContentsOfCell("A1", "String 1");
            s.SetContentsOfCell("B2", "2+2");
            s.SetContentsOfCell("C3", "B2+D4");
            s.SetContentsOfCell("D4", "1.0");

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
            s.SetContentsOfCell("A1", "String 1");
            s.SetContentsOfCell("B2", "2+2");
            s.SetContentsOfCell("C3", "B2+D4");
            s.SetContentsOfCell("D4", "1.0");

            s.GetCellContents("&&&");
        }

        /// <summary>
        /// Tests an it on a cell that has not been used yet
        /// </summary>
        [TestMethod]
        public void Public_GetCellContents_EmptyCell()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "String 1");
            s.SetContentsOfCell("B2", "2+2");
            s.SetContentsOfCell("C3", "B2+D4");
            s.SetContentsOfCell("D4", "1.0");

            Assert.IsTrue(s.GetCellContents("E5").Equals(""));
        }

        /*
         * SetContentsOfCell to double Tests
         */

        /// <summary>
        /// Tests a null name.  It should throw an ArgumentNullException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void Public_SetContentsOfCell_DoubleNullName()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "String 1");
            s.SetContentsOfCell("B2", "2+2");
            s.SetContentsOfCell("C3", "B2+D4");
            s.SetContentsOfCell("D4", "1.0");

            s.SetContentsOfCell(null, "2.52");
        }

        /// <summary>
        /// Tests an invalid name.  It should throw an InvalidNameException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void Public_SetContentsOfCell_DoubleInvalidName1()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "String 1");
            s.SetContentsOfCell("B2", "2+2");
            s.SetContentsOfCell("C3", "B2+D4");
            s.SetContentsOfCell("D4", "1.0");

            s.SetContentsOfCell("12_sdf", "2.52");
        }

        /// <summary>
        /// Tests an invalid name.  It should throw an InvalidNameException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void Public_SetContentsOfCell_DoubleInvalidName2()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "String 1");
            s.SetContentsOfCell("B2", "2+2");
            s.SetContentsOfCell("C3", "B2+D4");
            s.SetContentsOfCell("D4", "1.0");

            s.SetContentsOfCell("&&&", "2.52");
        }

        /// <summary>
        /// Tests to make sure that the method can create new cells with the correct contents
        /// </summary>
        [TestMethod]
        public void Public_SetContentsOfCell_DoubleCreatesNewCells()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "String 1");
            s.SetContentsOfCell("B2", "2+2");
            s.SetContentsOfCell("C3", "B2+D4");
            s.SetContentsOfCell("D4", "1.0");

            Assert.IsTrue(s.GetCellContents("D4").Equals(1.0));
        }

        /// <summary>
        /// Tests to make sure that the method can overwrite cells
        /// </summary>
        [TestMethod]
        public void Public_SetContentsOfCell_DoubleOverwriteCells()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "String 1");
            s.SetContentsOfCell("B2", "2+2");
            s.SetContentsOfCell("C3", "B2+D4");
            s.SetContentsOfCell("D4", "1.0");
            s.SetContentsOfCell("D4", "15.2");

            Assert.IsTrue(s.GetCellContents("D4").Equals(15.2));
        }

        /// <summary>
        /// Makes sure it returns a set with just this cell's name if it has no dependents
        /// </summary>
        [TestMethod]
        public void Public_SetContentsOfCell_DoubleNoDependents()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "String 1");
            s.SetContentsOfCell("B2", "2+2");
            s.SetContentsOfCell("C3", "B2");

            ISet<String> result = s.SetContentsOfCell("D4", "1.0");

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
        /// checks to make sure any old dependencies are removed and replaced
        /// </summary>
        [TestMethod]
        public void Public_SetContentsOfCell_DoubleDependentsReplaced()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "3");
            s.SetContentsOfCell("B2", "2+2");
            s.SetContentsOfCell("C3", "B2 + A1");
            s.SetContentsOfCell("C3", "1.0");

            ISet<String> result = s.SetContentsOfCell("A1", "5");

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
         * SetContentsOfCell to String Tests
         */
        /// <summary>
        /// Tests when a null string is put in for the text
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Public_SetContentsOfCell_StringNullText()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "String 1");
            s.SetContentsOfCell("B2", "2+2");
            s.SetContentsOfCell("C3", "B2+D4");
            s.SetContentsOfCell("D4", "1.0");

            String input ="";
            input = null;
            s.SetContentsOfCell("E5", input);
        }


        /// <summary>
        /// Tests a null name.  It should throw an ArgumentNullException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void Public_SetContentsOfCell_StringNullName()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "String 1");
            s.SetContentsOfCell("B2", "2+2");
            s.SetContentsOfCell("C3", "B2+D4");
            s.SetContentsOfCell("D4", "1.0");

            s.SetContentsOfCell(null, "Hello");
        }

        /// <summary>
        /// Tests an invalid name.  It should throw an InvalidNameException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void Public_SetContentsOfCell_StringInvalidName1()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "String 1");
            s.SetContentsOfCell("B2", "2+2");
            s.SetContentsOfCell("C3", "B2+D4");
            s.SetContentsOfCell("D4", "1.0");

            s.SetContentsOfCell("12_sdf", "Hello");
        }

        /// <summary>
        /// Tests an invalid name.  It should throw an InvalidNameException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void Public_SetContentsOfCell_StringInvalidName2()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "String 1");
            s.SetContentsOfCell("B2", "2+2");
            s.SetContentsOfCell("C3", "B2+D4");
            s.SetContentsOfCell("D4", "1.0");

            s.SetContentsOfCell("&&&", "Hello");
        }

        /// <summary>
        /// Tests to make sure that the method can create new cells with the correct contents
        /// </summary>
        [TestMethod]
        public void Public_SetContentsOfCell_StringCreatesNewCells()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "String 1");
            s.SetContentsOfCell("B2", "2+2");
            s.SetContentsOfCell("C3", "B2+D4");
            s.SetContentsOfCell("D4", "1.0");

            Assert.IsTrue(s.GetCellContents("A1").Equals("String 1"));
        }

        /// <summary>
        /// Tests to make sure that the method can overwrite cells
        /// </summary>
        [TestMethod]
        public void Public_SetContentsOfCell_StringOverwriteCells()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "String 1");
            s.SetContentsOfCell("B2", "2+2");
            s.SetContentsOfCell("C3", "B2+D4");
            s.SetContentsOfCell("A1", "String 2");


            Assert.IsTrue(s.GetCellContents("A1").Equals("String 2"));
        }

        /// <summary>
        /// Makes sure it returns a set with just this cell's name if it has no dependents
        /// </summary>
        [TestMethod]
        public void Public_SetContentsOfCell_StringNoDependents()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B2", "2+2");
            s.SetContentsOfCell("C3", "B2+D4");

            ISet<String> result = s.SetContentsOfCell("A1", "String 1");

            ISet<String> expected = new HashSet<String>();
            expected.Add("A1");

            // Makes sure they are the same size and then that each element of one is in the other
            Assert.AreEqual(result.Count, expected.Count);
            foreach (String name in result)
            {
                Assert.IsTrue(expected.Contains(name));
            }
        }

        /// <summary>
        /// Makes that the dependencies are replaced when we write over C3
        /// </summary>
        [TestMethod]
        public void Public_SetContentsOfCell_StringReplaceDependents()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "3");
            s.SetContentsOfCell("B2", "2+2");
            s.SetContentsOfCell("C3", "A1+D4");
            s.SetContentsOfCell("C3", "Hello");

            ISet<String> result = s.SetContentsOfCell("A1", "5");

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
         * SetContentsOfCell to Formula Tests
         */
        /// <summary>
        /// Tests to make sure adding something with a circular dependency does not change the SS
        /// </summary>
        [TestMethod]
        public void Public_SetContentsOfCell_FormulaCircularDoesntChangeSS1()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "B2");
            s.SetContentsOfCell("B2", "C3");
            s.SetContentsOfCell("C3", "D4");
            s.SetContentsOfCell("D4", "5");
            try
            {
                s.SetContentsOfCell("D4", "A1");
            } catch (CircularException)
            {
                //Dont do anything
            }
            Assert.AreEqual(5.0, s.GetCellContents("D4"));
        }

        /// <summary>
        /// Tests to make sure adding something with a circular dependency does not change the SS
        /// </summary>
        [TestMethod]
        public void Public_SetContentsOfCell_FormulaCircularDoesntChangeSS2()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "B2");
            s.SetContentsOfCell("B2", "C3");
            s.SetContentsOfCell("C3", "D4");
            s.SetContentsOfCell("D4", "Hello");
            try
            {
                s.SetContentsOfCell("D4", "A1");
            }
            catch (CircularException)
            {
                //Dont do anything
            }
            Assert.AreEqual("Hello", s.GetCellContents("D4"));
        }

        /// <summary>
        /// Tests to make sure adding something with a circular dependency does not change the SS
        /// </summary>
        [TestMethod]
        public void Public_SetContentsOfCell_FormulaCircularDoesntChangeSS3()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "B2");
            s.SetContentsOfCell("B2", "C3");
            s.SetContentsOfCell("C3", "D4");
            s.SetContentsOfCell("D4", "5 + 4");
            try
            {
                s.SetContentsOfCell("D4", "A1");
            }
            catch (CircularException)
            {
                //Dont do anything
            }
            Assert.AreEqual(new Formula("5 + 4"), s.GetCellContents("D4"));
        }

        /// <summary>
        /// Tests to make sure adding something with a circular dependency does not change the SS
        /// </summary>
        [TestMethod]
        public void Public_SetContentsOfCell_FormulaCircularDoesntChangeSS4()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "B2");
            s.SetContentsOfCell("B2", "C3");
            s.SetContentsOfCell("C3", "D4");
            try
            {
                s.SetContentsOfCell("D4", "A1");
            }
            catch (CircularException)
            {
                //Dont do anything
            }
            Assert.AreEqual("", s.GetCellContents("D4"));
        }

        /// <summary>
        /// Tests if a null formula is put in.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Public_SetContentsOfCell_FormulaNullFormula()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "String 1");
            s.SetContentsOfCell("B2", "2+2");
            s.SetContentsOfCell("C3", "B2+D4");
            s.SetContentsOfCell("D4", "1.0");

            String input = new Formula("3").ToString();
            input = null;

            s.SetContentsOfCell("E5", input);
        }

        /// <summary>
        /// Tests a null name.  It should throw an ArgumentNullException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void Public_SetContentsOfCell_FormulaNullName()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "String 1");
            s.SetContentsOfCell("B2", "2+2");
            s.SetContentsOfCell("C3", "B2+D4");
            s.SetContentsOfCell("D4", "1.0");

            s.SetContentsOfCell(null, "5+A1");
        }

        /// <summary>
        /// Tests an invalid name.  It should throw an InvalidNameException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void Public_SetContentsOfCell_FormulaInvalidName1()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "String 1");
            s.SetContentsOfCell("B2", "2+2");
            s.SetContentsOfCell("C3", "B2+D4");
            s.SetContentsOfCell("D4", "1.0");

            s.SetContentsOfCell("12_sdf", "5+A1");
        }

        /// <summary>
        /// Tests an invalid name.  It should throw an InvalidNameException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void Public_SetContentsOfCell_FormulaInvalidName2()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "String 1");
            s.SetContentsOfCell("B2", "2+2");
            s.SetContentsOfCell("C3", "B2+D4");
            s.SetContentsOfCell("D4", "1.0");

            s.SetContentsOfCell("&&&", "5+A1");
        }

        /// <summary>
        /// Makes sure it returns a set with just this cell's name if it has no dependents
        /// </summary>
        [TestMethod]
        public void Public_SetContentsOfCell_FormulaNoDependents()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "String 1");
            s.SetContentsOfCell("B2", "2+2");
            s.SetContentsOfCell("C3", "B2");

            ISet<String> result = s.SetContentsOfCell("D4", "1.0");

            ISet<String> expected = new HashSet<String>();
            expected.Add("D4");

            // Makes sure they are the same size and then that each element of one is in the other
            Assert.AreEqual(expected.Count, result.Count);
            foreach (String name in result)
            {
                Assert.IsTrue(expected.Contains(name));
            }
        }

        /// <summary>
        /// Makes sure it returns a set with itself, and direct dependents
        /// </summary>
        [TestMethod]
        public void Public_SetContentsOfCell_FormulaDirectDependents()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "String 1");
            s.SetContentsOfCell("B2", "C3");
            s.SetContentsOfCell("D4", "C3+3");

            ISet<String> result = s.SetContentsOfCell("C3", "3");

            ISet<String> expected = new HashSet<String>();
            expected.Add("D4");
            expected.Add("B2");
            expected.Add("C3");

            // Makes sure they are the same size and then that each element of one is in the other
            Assert.AreEqual(expected.Count, result.Count);
            foreach (String name in result)
            {
                Assert.IsTrue(expected.Contains(name));
            }
        }

        /// <summary>
        /// Makes sure it returns a set with itself, and direct dependents, and indirect dependents
        /// </summary>
        [TestMethod]
        public void Public_SetContentsOfCell_FormulaIndirectDependents()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "C3");
            s.SetContentsOfCell("B2", "A1");
            s.SetContentsOfCell("D4", "C3");

            ISet<String> result = s.SetContentsOfCell("C3", "5");

            ISet<String> expected = new HashSet<String>();
            expected.Add("D4");
            expected.Add("B2");
            expected.Add("C3");
            expected.Add("A1");

            // Makes sure they are the same size and then that each element of one is in the other
            Assert.AreEqual(expected.Count, result.Count);
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
        public void Public_SetContentsOfCell_FormulaCircularDependency()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "D4");
            s.SetContentsOfCell("B2", "A1");
            s.SetContentsOfCell("C3", "B2+D4");
            s.SetContentsOfCell("D4", "B2");
        }

        /// <summary>
        /// Checks to make sure an exception is thrown when there is a circular dependency
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void Public_SetContentsOfCell_FormulaDirectCircularDependency()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "D4");
            s.SetContentsOfCell("D4", "A1");
        }

        /// <summary>
        /// Checks to make sure an exception is thrown when there is a circular dependency
        /// </summary>
        [TestMethod]
        public void TestToMakeSureEmptyStringCellsAreNotCreated()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "");
            s.SetContentsOfCell("D4", "");

            HashSet<String> expected = new HashSet<string>();

            foreach (String name in s.GetNamesOfAllNonemptyCells())
            {
                Assert.IsTrue(expected.Contains(name));
            }
        }

        /// <summary>
        /// Makes that the dependencies are replaced when we write over C3
        /// </summary>
        [TestMethod]
        public void Public_SetContentsOfCell_FormulaReplaceDependents()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "3");
            s.SetContentsOfCell("B2", "2+2");
            s.SetContentsOfCell("C3", "A1+D4");
            s.SetContentsOfCell("C3", "5");

            ISet<String> result = s.SetContentsOfCell("A1", "5");

            ISet<String> expected = new HashSet<String>();
            expected.Add("A1");

            // Makes sure they are the same size and then that each element of one is in the other
            Assert.AreEqual(result.Count, expected.Count);
            foreach (String name in result)
            {
                Assert.IsTrue(expected.Contains(name));
            }
        }

        /// <summary>
        /// This test just makes sure that if someone gets accces to this method it will throw the correct error.
        /// I wouldn't usually test this but since it it protected I probably should.  Someone might come by and 
        /// inherit from my class and muck everything up.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Protected_GetDirectDependents_MakeSureItWillThrowError()
        {
            SpecialSS s = new SpecialSS();

            s.PassNullToGetDirectDependents(null);
        }

        /// <summary>
        /// A class just to check the protected members
        /// </summary>
        public class SpecialSS : Spreadsheet
        {
            public void PassNullToGetDirectDependents(String name)
            {
                this.GetDirectDependents(name);
            }
        }

    }
}
