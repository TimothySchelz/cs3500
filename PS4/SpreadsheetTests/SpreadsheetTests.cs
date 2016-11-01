// Written by Timothy Schelz, u0851027, October 2016

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using SpreadsheetUtilities;
using System.Collections.Generic;

namespace SpreadsheetTests
{
#if false

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

        /// <summary>
        /// Checks to make sure the normalizer was set to identity function
        /// </summary>
        [TestMethod]
        public void Public_0argCon_CheckNormalizer()
        {
            Spreadsheet s = new Spreadsheet();

            s.SetContentsOfCell("a1", "content");
            s.SetContentsOfCell("B2", "Some more content");
            HashSet<String> expected = new HashSet<String>() { "a1", "B2" };

            
            foreach (String name in s.GetNamesOfAllNonemptyCells())
            {
                Assert.IsTrue(expected.Remove(name));
            }
            Assert.AreEqual(0, expected.Count);
        }

        /// <summary>
        /// Checks to make sure the validator is set to true.  Can't really do this.
        /// Just going to check some weird ugly variable names
        /// </summary>
        [TestMethod]
        public void Public_0argCon_CheckValidator()
        {
            Spreadsheet s = new Spreadsheet();

            s.SetContentsOfCell("a1", "content");
            s.SetContentsOfCell("B2", "Some more content");
            s.SetContentsOfCell("awkjenf113904756", "content");
            s.SetContentsOfCell("BbBbBbBbBbBbB2", "Some more content");
            HashSet<String> expected = new HashSet<String>() { "a1", "B2", "awkjenf113904756", "BbBbBbBbBbBbB2" };

            foreach (String name in s.GetNamesOfAllNonemptyCells())
            {
                Assert.IsTrue(expected.Remove(name));
            }
            Assert.AreEqual(0, expected.Count);
        }
        /// <summary>
        /// Checks to make sure the version is default
        /// </summary>
        [TestMethod]
        public void Public_0argCon_Checkversion()
        {
            Spreadsheet s = new Spreadsheet();

            Assert.AreEqual("default", s.Version);
        }


        /*
         * 3 Arg Constructor
         */
        /// <summary>
        /// Makes sure a new spreadsheet is empty.  Since IEnumerable does not have a
        /// count or size or anything we have to use the loop
        /// </summary>
        [TestMethod]
        public void Public_3argCon_MakeSureItIsEmpty()
        {
            Spreadsheet s = new Spreadsheet(t=>true, t=>t, "1.0");

            foreach (String name in s.GetNamesOfAllNonemptyCells())
            {
                Assert.Fail();
            }
            Assert.IsTrue(true);
        }

        /// <summary>
        /// Checks to make sure the normalizer was set properly
        /// </summary>
        [TestMethod]
        public void Public_3argCon_CheckNormalizer()
        {
            Spreadsheet s = new Spreadsheet(t => true, t => t.ToUpper(), "1.0");

            s.SetContentsOfCell("a1", "content");
            s.SetContentsOfCell("B2", "Some more content");
            HashSet<String> expected = new HashSet<String>() { "A1", "B2" };

            IEnumerable<String> result = s.GetNamesOfAllNonemptyCells();

            foreach (String name in result)
            {
                Assert.IsTrue(expected.Remove(name));
            }
            Assert.AreEqual(0, expected.Count);
        }

        /// <summary>
        /// Checks to make sure the validator does throw an error when variable name does
        /// not match the delegate's rule
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void Public_3argCon_CheckValidator()
        {
            Spreadsheet s = new Spreadsheet(t => t[0]=='A', t => t, "1.0");

            s.SetContentsOfCell("A1", "content");
            s.SetContentsOfCell("B2", "Some more content");
        }

        /// <summary>
        /// Checks to make sure the version is set correctly
        /// </summary>
        [TestMethod]
        public void Public_3argCon_Checkversion()
        {
            Spreadsheet s = new Spreadsheet(t => true, t => t, "1.0");

            Assert.AreEqual("1.0", s.Version);
        }

        /*
         * 4 arg Constructor
         */
        /// <summary>
        /// Checks to make sure an exception is thrown the versions don't match. ... Did I already test this?
        /// 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void Public_4argCon_VersionMismatch()
        {
            Spreadsheet s = new Spreadsheet(@"../../TestFiles/Test_CircularDependency.xml", t => t[0] == 'A', t => t, "Something Completely Different");
        }

        /// <summary>
        /// Checks to make sure an exception is thrown when a circular dependency exists in the 
        /// file.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void Public_4argCon_CircularDependency()
        {
            Spreadsheet s = new Spreadsheet(@"../../TestFiles/Test_CircularDependency.xml", t => t[0] == 'A', t => t, "1");
        }

        /// <summary>
        /// Checks to make sure an exception is thrown when when there is a typo in the starting 
        /// spreadsheet element
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void Public_4argCon_SpreadSheetTypo()
        {
            Spreadsheet s = new Spreadsheet(@"../../TestFiles/Test_SpreadSheetTypo.xml", t => t[0] == 'A', t => t, "1");
        }

        /// <summary>
        /// Checks to make sure an exception is thrown when the file has a typo in the starting cell
        /// element
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void Public_4argCon_CellTypo()
        {
            Spreadsheet s = new Spreadsheet(@"../../TestFiles/Test_CellTypo.xml", t => t[0] == 'A', t => t, "1");
        }

        /// <summary>
        /// Checks to make sure an exception is thrown when there is a typo in the starting name 
        /// element
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void Public_4argCon_NameTypo()
        {
            Spreadsheet s = new Spreadsheet(@"../../TestFiles/Test_NameTypo.xml", t => t[0] == 'A', t => t, "1");
        }

        /// <summary>
        /// Checks to make sure an exception is thrown when a there is no version attribute
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void Public_4argCon_NoVersion()
        {
            Spreadsheet s = new Spreadsheet(@"../../TestFiles/Test_NoVersion.xml", t => t[0] == 'A', t => t, "1");
        }

        /// <summary>
        /// Checks to make sure an exception is thrown when there is a typo in the version attribute
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void Public_4argCon_VersionTypo()
        {
            Spreadsheet s = new Spreadsheet(@"../../TestFiles/Test_VersionTypo.xml", t => t[0] == 'A', t => t, "1");
        }

        /// <summary>
        /// Checks to make sure an exception is thrown when there is a typo in the starting content 
        /// element
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void Public_4argCon_ContentTypo()
        {
            Spreadsheet s = new Spreadsheet(@"../../TestFiles/Test_ContentTypo.xml", t => t[0] == 'A', t => t, "1");
        }

        /// <summary>
        /// Checks to make sure an exception is thrown when a cell has an invalid name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void Public_4argCon_InvalidName()
        {
            Spreadsheet s = new Spreadsheet(@"../../TestFiles/Test_InvalidName.xml", t => t[0] == 'A', t => t, "1");
        }

        /// <summary>
        /// Checks to make sure an exception is thrown when a cell has an invalid fomrula
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void Public_4argCon_InvalidFormula()
        {
            Spreadsheet s = new Spreadsheet(@"../../TestFiles/Test_InvalidFormula.xml", t => t[0] == 'A', t => t, "1");
        }

        /// <summary>
        /// This test makes sure that the spreadsheet can properly save a String cell
        /// </summary>
        [TestMethod]
        public void Public_4argCon_StringCell()
        {
            Spreadsheet t = new Spreadsheet(@"../../TestFiles/Test_Save_StringCell.xml", u => true, u => u, "default");

            IEnumerable<String> actual = t.GetNamesOfAllNonemptyCells();
            List<String> expected = new List<string> { "A1" };

            foreach (String name in actual)
            {
                Assert.IsTrue(expected.Remove(name));
            }
            Assert.IsTrue(expected.Count == 0);
            Assert.AreEqual("Content", t.GetCellContents("A1"));
        }

        /// <summary>
        /// This test makes sure that the spreadsheet can properly save a Double cell
        /// </summary>
        [TestMethod]
        public void Public_4argCon_DoubleCell()
        {
            Spreadsheet t = new Spreadsheet(@"../../TestFiles/Test_Save_DoubleCell.xml", u => true, u => u, "default");

            IEnumerable<String> actual = t.GetNamesOfAllNonemptyCells();
            List<String> expected = new List<string> { "A1" };

            foreach (String name in actual)
            {
                Assert.IsTrue(expected.Remove(name));
            }
            Assert.IsTrue(expected.Count == 0);
            Assert.AreEqual(4.2, t.GetCellContents("A1"));
        }

        /// <summary>
        /// This test makes sure that the spreadsheet can properly save a Formula cell
        /// </summary>
        [TestMethod]
        public void Public_4argCon_FormulaCell()
        {
            Spreadsheet t = new Spreadsheet(@"../../TestFiles/Test_Save_FormulaCell.xml", u => true, u => u, "default");

            IEnumerable<String> actual = t.GetNamesOfAllNonemptyCells();
            List<String> expected = new List<string> { "A1" };

            foreach (String name in actual)
            {
                Assert.IsTrue(expected.Remove(name));
            }
            Assert.IsTrue(expected.Count == 0);
            Assert.AreEqual(new Formula("4.2"), t.GetCellContents("A1"));
        }

        /// <summary>
        /// This test makes sure that the spreadsheet can properly save multiple cells
        /// </summary>
        [TestMethod]
        public void Public_4argCon_MultipleCells()
        {
            Spreadsheet t = new Spreadsheet(@"../../TestFiles/Test_Save_MultipleCells.xml", u => true, u => u, "default");

            IEnumerable<String> actual = t.GetNamesOfAllNonemptyCells();
            List<String> expected = new List<string> { "A1", "B2", "C3", "D4", "E5", "F6" };

            foreach (String name in actual)
            {
                Assert.IsTrue(expected.Remove(name));
            }
            Assert.IsTrue(expected.Count == 0);
        }

        /// <summary>
        /// This test makes sure that the spreadsheet can properly save multiple dependencies and wont poop the bed
        /// </summary>
        [TestMethod]
        public void Public_4argCon_MultipleDependencies()
        {
            Spreadsheet t = new Spreadsheet(@"../../TestFiles/Test_Save_MultipleDependencies.xml", u => true, u => u, "default");

            IEnumerable<String> actual = t.GetNamesOfAllNonemptyCells();
            List<String> expected = new List<string> { "A1", "B2", "C3", "D4", "E5", "F6" };

            foreach (String name in actual)
            {
                Assert.IsTrue(expected.Remove(name));
            }
            Assert.IsTrue(expected.Count == 0);
        }

        /// <summary>
        /// Makes sure it throws when the file doesn't exist
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void Public_4argCon_FileDNE()
        {
            Spreadsheet t = new Spreadsheet(@"../../TestFiles/ThisFileDoesn'tExist.xml", u => true, u => u, "default");
        }

        /*
         * GetSavedVersion Tests
         */
        [TestMethod]
        public void Public_GetSavedVersion_CheckBasicFile()
        {
            Spreadsheet s = new Spreadsheet();
            Assert.AreEqual("version information goes here", s.GetSavedVersion(@"../../TestFiles/Test_Empty.xml"));
        }

        /// <summary>
        /// Makes sure it throws when the file doesn't exist
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void Public_GetSavedVersion_NoFile()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetSavedVersion("HelloWorld.xml");
        }

        /*
         * Save Tests
         */

        /// <summary>
        /// This test makes sure that the spreadsheet can properly save a String cell
        /// </summary>
        [TestMethod]
        public void Public_Save_BrokenFileName()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "Content");
            s.Save(@"../../TestFiles//\/\/\/\/.xml");
        }

        /// <summary>
        /// This test makes sure that the spreadsheet can properly save a String cell
        /// </summary>
        [TestMethod]
        public void Public_Save_StringCell()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "Content");
            s.Save(@"../../TestFiles/Test_Save_StringCell.xml");

            Spreadsheet t = new Spreadsheet(@"../../TestFiles/Test_Save_StringCell.xml", u => true, u => u, "default");

            IEnumerable<String> actual = t.GetNamesOfAllNonemptyCells();
            List<String> expected = new List<string> { "A1" };

            foreach (String name in actual)
            {
                Assert.IsTrue(expected.Remove(name));
            }
            Assert.IsTrue(expected.Count == 0);
            Assert.AreEqual("Content", t.GetCellContents("A1"));
        }

        /// <summary>
        /// This test makes sure that the spreadsheet can properly save a Double cell
        /// </summary>
        [TestMethod]
        public void Public_Save_DoubleCell()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "4.2");
            s.Save(@"../../TestFiles/Test_Save_DoubleCell.xml");

            Spreadsheet t = new Spreadsheet(@"../../TestFiles/Test_Save_DoubleCell.xml", u => true, u => u, "default");

            IEnumerable<String> actual = t.GetNamesOfAllNonemptyCells();
            List<String> expected = new List<string> { "A1" };

            foreach (String name in actual)
            {
                Assert.IsTrue(expected.Remove(name));
            }
            Assert.IsTrue(expected.Count == 0);
            Assert.AreEqual(4.2, t.GetCellContents("A1"));
        }

        /// <summary>
        /// This test makes sure that the spreadsheet can properly save a Formula cell
        /// </summary>
        [TestMethod]
        public void Public_Save_FormulaCell()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=4.2");
            s.Save(@"../../TestFiles/Test_Save_FormulaCell.xml");

            Spreadsheet t = new Spreadsheet(@"../../TestFiles/Test_Save_FormulaCell.xml", u => true, u => u, "default");

            IEnumerable<String> actual = t.GetNamesOfAllNonemptyCells();
            List<String> expected = new List<string> { "A1" };

            foreach (String name in actual)
            {
                Assert.IsTrue(expected.Remove(name));
            }
            Assert.IsTrue(expected.Count == 0);
            Assert.AreEqual(new Formula("4.2"), t.GetCellContents("A1"));
        }

        /// <summary>
        /// This test makes sure that the spreadsheet can properly save multiple cells
        /// </summary>
        [TestMethod]
        public void Public_Save_MultipleCells()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "Merle casts Zone of Truth!");
            s.SetContentsOfCell("B2", "Merle cast Zone of Truth again!");
            s.SetContentsOfCell("C3", "5.8");
            s.SetContentsOfCell("D4", "4.2");
            s.SetContentsOfCell("E5", "=4.2+6.7-3"); // value == 7.9
            s.SetContentsOfCell("F6", "=D4 + C3"); // value == 10
            s.Save(@"../../TestFiles/Test_Save_MultipleCells.xml");

            Spreadsheet t = new Spreadsheet(@"../../TestFiles/Test_Save_MultipleCells.xml", u => true, u => u, "default");

            IEnumerable<String> actual = t.GetNamesOfAllNonemptyCells();
            List<String> expected = new List<string> { "A1", "B2", "C3", "D4", "E5", "F6" };

            foreach (String name in actual)
            {
                Assert.IsTrue(expected.Remove(name));
            }
            Assert.IsTrue(expected.Count == 0);
        }

        /// <summary>
        /// This test makes sure that the spreadsheet can properly save multiple dependencies and wont poop the bed
        /// </summary>
        [TestMethod]
        public void Public_Save_MultipleDependencies()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "3.8");
            s.SetContentsOfCell("B2", "=A1*2");
            s.SetContentsOfCell("C3", "=A1*A1");
            s.SetContentsOfCell("D4", "=B2+C3");
            s.SetContentsOfCell("E5", "=D4/1"); 
            s.SetContentsOfCell("F6", "=E5+C3"); 
            s.Save(@"../../TestFiles/Test_Save_MultipleDependencies.xml");

            Spreadsheet t = new Spreadsheet(@"../../TestFiles/Test_Save_MultipleDependencies.xml", u => true, u => u, "default");

            IEnumerable<String> actual = t.GetNamesOfAllNonemptyCells();
            List<String> expected = new List<string> { "A1", "B2", "C3", "D4", "E5", "F6" };

            foreach (String name in actual)
            {
                Assert.IsTrue(expected.Remove(name));
            }
            Assert.IsTrue(expected.Count == 0);
        }

        /*
         * GetCellValue Tests
         */

        /// <summary>
        /// Makes sure that the normalizer and validator are working correctly in tandem
        /// </summary>
        [TestMethod]
        public void Public_GetCellValue_NormalizerValidatorCheck1()
        {
            Spreadsheet s = new Spreadsheet(t => t[0] == 'A', t => t.ToUpper(), "1.0");
            s.SetContentsOfCell("a1", "Magnus Rolls a 20!");
            Assert.AreEqual("Magnus Rolls a 20!", s.GetCellValue("a1"));
        }

        /// <summary>
        /// Makes sure that it passes through the general restrictions before normalization
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void Public_GetCellValue_NormalizerValidatorCheck2()
        {
            Spreadsheet s = new Spreadsheet(t => true, t => t+"1", "1.0");
            s.SetContentsOfCell("a1", "Magnus Rolls a 20!");
            s.GetCellValue("a");
        }

        /// <summary>
        /// Checks that it throws an InvalidNameException when the name is null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void Public_GetCellValue_NullName()
        {
            Spreadsheet s = new Spreadsheet();

            s.GetCellValue(null);
        }

        /// <summary>
        /// Checks that it throws an InvalidNameException when the name is invalid
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void Public_GetCellValue_InvalidName1()
        {
            Spreadsheet s = new Spreadsheet();

            s.GetCellValue("Eskimo");
        }

        /// <summary>
        /// Checks that it throws an InvalidNameException when the name is invalid
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void Public_GetCellValue_InvalidName2()
        {
            Spreadsheet s = new Spreadsheet();

            s.GetCellValue("_Frankfurter");
        }

        /// <summary>
        /// Checks that it throws an InvalidNameException when the name is invalid
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void Public_GetCellValue_InvalidName3()
        {
            Spreadsheet s = new Spreadsheet();

            s.GetCellValue("ABC123DEF456");
        }

        /// <summary>
        /// Checks that it throws an InvalidNameException when the name does not pass the validator
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void Public_GetCellValue_NameDoesntPassValidator()
        {
            Spreadsheet s = new Spreadsheet(t=>false, t=>t, "0");

            s.GetCellValue("A1");
        }

        /// <summary>
        /// Gets the value of empty cell
        /// </summary>
        [TestMethod]
        
        public void Public_GetCellValue_Empty()
        {
            Spreadsheet s = new Spreadsheet(t => true, t => t, "0");
            
            Assert.AreEqual("", s.GetCellValue("A1"));
        }

        /// <summary>
        /// Gets the value of String cell
        /// </summary>
        [TestMethod]
        public void Public_GetCellValue_String()
        {
            Spreadsheet s = new Spreadsheet(t => true, t => t, "0");
            s.SetContentsOfCell("A1", "content");

            Assert.AreEqual("content", s.GetCellValue("A1"));
        }

        /// <summary>
        /// Gets the value of double cell
        /// </summary>
        [TestMethod]
        public void Public_GetCellValue_Double()
        {
            Spreadsheet s = new Spreadsheet(t => true, t => t, "0");
            s.SetContentsOfCell("A1", "3.5");

            object result = s.GetCellValue("A1");
            object expected = 3.5;

            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// Gets the value of formula cell
        /// </summary>
        [TestMethod]

        public void Public_GetCellValue_Formula()
        {
            Spreadsheet s = new Spreadsheet(t => true, t => t, "0");
            s.SetContentsOfCell("A1", "5");
            s.SetContentsOfCell("B2", "=A1+6");

            Assert.AreEqual(11.0, s.GetCellValue("B2"));
        }

        /// <summary>
        /// Gets the value of formula cell when it should be FormulaError
        /// </summary>
        [TestMethod]

        public void Public_GetCellValue_FormulaError()
        {
            Spreadsheet s = new Spreadsheet(t => true, t => t, "0");
            s.SetContentsOfCell("B2", "=A1+6");

            Assert.IsTrue(s.GetCellValue("B2") is FormulaError);
        }


        /*
         * Changed Property
         */

        /// <summary>
        /// Changed == false after saving
        /// </summary>
        [TestMethod]
        public void Public_ChangedProperty_AfterSaving()
        {
            Spreadsheet s = new Spreadsheet(@"../../TestFiles/Test_Empty.xml", t => true, t => t, "version information goes here");
            s.SetContentsOfCell("A1", "contents");
            s.Save(@"../../TestFiles/Test_ChangedProperty.xml");
            Assert.IsFalse(s.Changed);
        }

        /// <summary>
        /// Not actuall changing a cell
        /// </summary>
        [TestMethod]
        public void Public_ChangedProperty_NotARealChange()
        {
            Spreadsheet s = new Spreadsheet(@"../../TestFiles/Test_Empty.xml", t => true, t => t, "version information goes here");
            s.SetContentsOfCell("A1", "contents");
            Assert.IsTrue(s.Changed);
        }

        /// <summary>
        /// Makes sure that the changed is already set to false with a blank SS
        /// </summary>
        [TestMethod]
        public void Public_ChangedProperty_NoChange1()
        {
            Spreadsheet s = new Spreadsheet();

            Assert.IsFalse(s.Changed);
        }

        /// <summary>
        /// Makes sure that the changed is already set to false with a blank SS using 3 arg constructor
        /// </summary>
        [TestMethod]
        public void Public_ChangedProperty_NoChange2()
        {
            Spreadsheet s = new Spreadsheet(t => t[0] == 'A', t => t, "1.0");

            Assert.IsFalse(s.Changed);
        }

        /// <summary>
        /// Makes sure that the changed is already set to false with a blank SS using 4arg constructor
        /// </summary>
        [TestMethod]
        public void Public_ChangedProperty_NoChange3()
        {
            Spreadsheet s = new Spreadsheet(@"../../TestFiles/Test_Empty.xml", t => true, t => t, "version information goes here");

            Assert.IsFalse(s.Changed);
        }

        /// <summary>
        /// Makes sure that it changes when a String is added
        /// </summary>
        [TestMethod]
        public void Public_ChangedProperty_AddString()
        {
            Spreadsheet s = new Spreadsheet(t => t[0] == 'A', t => t, "1.0");
            s.SetContentsOfCell("A1", "Hello");
            Assert.IsTrue(s.Changed);
        }

        /// <summary>
        /// Makes sure that it changes when a Double is added
        /// </summary>
        [TestMethod]
        public void Public_ChangedProperty_AddDouble()
        {
            Spreadsheet s = new Spreadsheet(t => t[0] == 'A', t => t, "1.0");
            s.SetContentsOfCell("A1", "5.8");
            Assert.IsTrue(s.Changed);
        }

        /// <summary>
        /// Makes sure that it changes when a Formula is added
        /// </summary>
        [TestMethod]
        public void Public_ChangedProperty_AddFormula()
        {
            Spreadsheet s = new Spreadsheet(t => t[0] == 'A', t => t, "1.0");
            s.SetContentsOfCell("A1", "=5.8+0.2");
            Assert.IsTrue(s.Changed);
        }

        /// <summary>
        /// Makes sure that changed changes when something is removed
        /// </summary>
        [TestMethod]
        public void Public_ChangedProperty_RemoveCell()
        {
            Spreadsheet s = new Spreadsheet(@"../../TestFiles/Test_Empty.xml", t => true, t => t, "version information goes here");
            s.SetContentsOfCell("A1", "");
            Assert.IsFalse(s.Changed);
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
        /// Makes sure that the normalizer and validator are working correctly in tandem
        /// </summary>
        [TestMethod]
        public void Public_GetContentsOfCell_NormalizerValidatorCheck1()
        {
            Spreadsheet s = new Spreadsheet(t => t[0] == 'A', t => t.ToUpper(), "1.0");
            s.SetContentsOfCell("a1", "Magnus Rolls a 20!");
            Assert.AreEqual("Magnus Rolls a 20!", s.GetCellContents("a1"));
        }

        /// <summary>
        /// Makes sure that it passes through the general restrictions before normalization
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void Public_GetContentsOfCell_NormalizerValidatorCheck2()
        {
            Spreadsheet s = new Spreadsheet(t => true, t => t + "1", "1.0");
            s.SetContentsOfCell("a1", "Magnus Rolls a 20!");
            s.GetCellContents("a");
        }

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
            s.SetContentsOfCell("B2", "=2+2");
            s.SetContentsOfCell("C3", "=B2+D4");
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
        /// Makes sure that the normalizer and validator are working correctly in tandem
        /// </summary>
        [TestMethod]
        public void Public_SetContentsOfCell_NormalizerValidatorCheck1()
        {
            Spreadsheet s = new Spreadsheet(t => t[0] == 'A', t => t.ToUpper(), "1.0");
            s.SetContentsOfCell("a1", "Magnus Rolls a 20!");
        }

        /// <summary>
        /// Makes sure that it passes through the general restrictions before normalization
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void Public_SetContentsOfCell_NormalizerValidatorCheck2()
        {
            Spreadsheet s = new Spreadsheet(t => true, t => t + "1", "1.0");
            s.SetContentsOfCell("a", "Magnus Rolls a 20!");
        }

        /// <summary>
        /// Makes sure the strings with just a double end up as doubles
        /// </summary>
        [TestMethod]
        public void Public_SetContentsOfCell_CreateDouble()
        {
            Spreadsheet s = new Spreadsheet();

            s.SetContentsOfCell("A1", "2.52");

            Assert.AreEqual(2.52, s.GetCellContents("A1"));
        }

        /// <summary>
        /// Makes sure that a string that looks like a formula ends up as a string
        /// </summary>
        [TestMethod]
        public void Public_SetContentsOfCell_CreateString()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B2", "1.5");
            s.SetContentsOfCell("C3", "=B2*2");
            s.SetContentsOfCell("A1", "B2+C3");

            Assert.AreEqual("B2+C3", s.GetCellContents("A1"));
        }

        /// <summary>
        /// Makes sure that a string wath an '=' creates a formula 
        /// </summary>
        [TestMethod]
        public void Public_SetContentsOfCell_CreateFormula()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B2", "1.5");
            s.SetContentsOfCell("C3", "=B2*2");
            s.SetContentsOfCell("A1", "=B2+C3");

            Assert.AreEqual(new Formula("B2+C3"), s.GetCellContents("A1"));
        }

        /// <summary>
        /// Makes sure that a string starting with '=' throws an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Public_SetContentsOfCell_BreakFormula()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B2", "1.5");
            s.SetContentsOfCell("C3", "=Hey Taako, what is the magic word?");
        }


        /// <summary>
        /// Tests a null name.  It should throw an ArgumentNullException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void Public_SetContentsOfCell_DoubleNullName()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "String 1");
            s.SetContentsOfCell("B2", "=2+2");
            s.SetContentsOfCell("D4", "1.0");
            s.SetContentsOfCell("C3", "=B2+D4");


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
            s.SetContentsOfCell("C3", "=B2+D4");
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
            s.SetContentsOfCell("C3", "=B2+D4");
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
            s.SetContentsOfCell("C3", "=B2+D4");
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
            s.SetContentsOfCell("C3", "=B2+D4");
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
            s.SetContentsOfCell("C3", "=B2");

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
            s.SetContentsOfCell("C3", "=B2 + A1");
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
            s.SetContentsOfCell("C3", "=B2+D4");
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
            s.SetContentsOfCell("C3", "=B2+D4");
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
            s.SetContentsOfCell("C3", "=B2+D4");
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
            s.SetContentsOfCell("C3", "=B2+D4");
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
            s.SetContentsOfCell("C3", "=B2+D4");
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
            s.SetContentsOfCell("C3", "=B2+D4");
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
            s.SetContentsOfCell("C3", "=B2+D4");

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
            s.SetContentsOfCell("C3", "=A1+D4");
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
            s.SetContentsOfCell("A1", "=B2");
            s.SetContentsOfCell("B2", "=C3");
            s.SetContentsOfCell("C3", "=D4");
            s.SetContentsOfCell("D4", "5");
            try
            {
                s.SetContentsOfCell("D4", "=A1");
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
            s.SetContentsOfCell("A1", "=B2");
            s.SetContentsOfCell("B2", "=C3");
            s.SetContentsOfCell("C3", "=D4");
            s.SetContentsOfCell("D4", "Hello");
            try
            {
                s.SetContentsOfCell("D4", "=A1");
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
            s.SetContentsOfCell("A1", "=B2");
            s.SetContentsOfCell("B2", "=C3");
            s.SetContentsOfCell("C3", "=D4");
            s.SetContentsOfCell("D4", "=5 + 4");
            try
            {
                s.SetContentsOfCell("D4", "=A1");
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
            s.SetContentsOfCell("A1", "=B2");
            s.SetContentsOfCell("B2", "=C3");
            s.SetContentsOfCell("C3", "=D4");
            try
            {
                s.SetContentsOfCell("D4", "=A1");
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
            s.SetContentsOfCell("C3", "=B2+D4");
            s.SetContentsOfCell("D4", "1.0");

            String input = "=" + new Formula("3").ToString();
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
            s.SetContentsOfCell("C3", "=B2+D4");
            s.SetContentsOfCell("D4", "1.0");

            s.SetContentsOfCell(null, "=5+A1");
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
            s.SetContentsOfCell("C3", "=B2+D4");
            s.SetContentsOfCell("D4", "1.0");

            s.SetContentsOfCell("12_sdf", "=5+A1");
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
            s.SetContentsOfCell("C3", "=B2+D4");
            s.SetContentsOfCell("D4", "1.0");

            s.SetContentsOfCell("&&&", "=5+A1");
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
            s.SetContentsOfCell("C3", "=B2");

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
            s.SetContentsOfCell("B2", "=C3");
            s.SetContentsOfCell("D4", "=C3+3");

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
            s.SetContentsOfCell("A1", "=C3");
            s.SetContentsOfCell("B2", "=A1");
            s.SetContentsOfCell("D4", "=C3");

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
            s.SetContentsOfCell("A1", "=D4");
            s.SetContentsOfCell("B2", "=A1");
            s.SetContentsOfCell("C3", "=B2+D4");
            s.SetContentsOfCell("D4", "=B2");
        }

        /// <summary>
        /// Checks to make sure an exception is thrown when there is a circular dependency
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void Public_SetContentsOfCell_FormulaDirectCircularDependency()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=D4");
            s.SetContentsOfCell("D4", "=A1");
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
            s.SetContentsOfCell("C3", "=A1+D4");
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

        /*
         * Various other tests
         */
        /// <summary>
        /// Makes that the dependencies are replaced when we write over C3
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void Private_NameValidator_WeirdCharacters()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("AB$D1234", "3");
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
#endif
}
