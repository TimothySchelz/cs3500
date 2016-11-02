using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;


namespace GUITests
{
    /// <summary>
    /// Summary description for CodedUITest1
    /// </summary>
    [CodedUITest]
    public class CodedUITest1
    {
        public CodedUITest1()
        {
        }

        [TestMethod]
        public void Update_ContentsCheckValueLabel()
        {
            this.UIMap.OpenSpreadsheet();
            this.UIMap.EnterContents();
            this.UIMap.UpdateCheckedByValueLabel();
            this.UIMap.Xout();
        }

        [TestMethod]
        public void Update_ContentsCheckNameLabel()
        {
            this.UIMap.OpenSpreadsheet();
            this.UIMap.EnterContents();
            this.UIMap.CheckFocusA1();
            this.UIMap.Xout(); 
        }

        [TestMethod]
        public void Update_ContentsCheckContentsBox()
        {
            this.UIMap.OpenSpreadsheet();
            this.UIMap.EnterContents();
            this.UIMap.CheckContentBox();
            this.UIMap.Xout();
        }

        [TestMethod]
        public void Update_OverwriteContents()
        {
            this.UIMap.OpenSpreadsheet();
            this.UIMap.EnterContents();
            this.UIMap.OverwriteA1();
            this.UIMap.CheckOverwirteWorked();
            this.UIMap.Xout();
        }

        [TestMethod]
        public void New_CheckExistanceofSecondWindow()
        {
            this.UIMap.OpenSpreadsheet();
            this.UIMap.OpenSecondSpreadsheet();
            this.UIMap.CheckEmptyContentsOnSecondSpreadsheet();
            this.UIMap.Close2EmptySpreadsheets();
        }

        [TestMethod]
        public void New_CheckNewWindowIsEmpty()
        {
            this.UIMap.OpenSpreadsheet();
            this.UIMap.InputInSS1AndOpenSS2();
            this.UIMap.CheckContentsOfOldAndNewSS();
            this.UIMap.CloseSpreadsheets();
        }

        [TestMethod]
        public void New_CheckWindowsAreNotLinked()
        {
            this.UIMap.OpenSpreadsheet();
            this.UIMap.OpenSecondSSAndEnterValues();
            this.UIMap.CheckSS1B2AndSS2A1();
            this.UIMap.Closing();

        }

        [TestMethod]
        public void Close_CloseOriginalWindow()
        {
            this.UIMap.OpenSpreadsheet();
            this.UIMap.OpenNewCloseOriginal();
            this.UIMap.SecondWindowExists();
            this.UIMap.CloseSecond();

        }

        [TestMethod]
        public void Help_CheckHelpPopUp()
        {
            this.UIMap.OpenSpreadsheet();
            this.UIMap.OpenHelp();
            this.UIMap.CheckHelpText();
            this.UIMap.CloseHelpandWindow();

        }

        [TestMethod]
        public void ChangeSelection_MakeSureNameLabelChanges()
        {
            this.UIMap.OpenSpreadsheet();
            this.UIMap.SelectE11();
            this.UIMap.AssertMethod1();
            this.UIMap.CloseWindow();

        }

        [TestMethod]
        public void ChangeSelection_CheckValueLabelChanges()
        {
            this.UIMap.OpenSpreadsheet();
            this.UIMap.PlugInContents();
            this.UIMap.CheckC3Value2();
            this.UIMap.SelectB4();
            this.UIMap.CheckB4ValueHello();
            this.UIMap.SelectC5();
            this.UIMap.CheckC5Value1();

        }

        [TestMethod]
        public void ChangeSelection_CheckContentsBox()
        {
            this.UIMap.OpenSpreadsheet();
            this.UIMap.EnterSomeContents();
            this.UIMap.A2IsHello();
            this.UIMap.SelectCellC2();
            this.UIMap.CheckContentsOfC2();
            this.UIMap.SelectCellB4();
            this.UIMap.CheckContentsOfB4();
            this.UIMap.Closeout();
        }

        [TestMethod]
        public void Problems_CircularDependency()
        {
            this.UIMap.OpenSpreadsheet();
            this.UIMap.CreateCircularDependency();
            this.UIMap.CheckSCircDepErrorMessage();
            this.UIMap.Close();
        }

        [TestMethod]
        public void Problems_FormulaFormatProblems()
        {
            this.UIMap.OpenSpreadsheet();
            this.UIMap.CreateFormulaProblems();
            this.UIMap.CheckErrorMessageForFormulaProblems();
            this.UIMap.CloseItAgain();

        }

        [TestMethod]
        public void Tab_ChecksTabMovesSelection()
        {
            this.UIMap.OpenSpreadsheet();
            this.UIMap.TabbedSome();
            this.UIMap.CheckSelection();
            this.UIMap.CloseItout();
        }

        [TestMethod]
        public void ShiftTab_ChecksShiftTabMovesSelection()
        {
            this.UIMap.OpenSpreadsheet();
            this.UIMap.MakeSelectionAndShiftTabBack();
            this.UIMap.ShiftTabCheck();
            this.UIMap.CloseItout();
        }




        #region Additional test attributes

        // You can use the following additional attributes as you write your tests:

        ////Use TestInitialize to run code before running each test 
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{        
        //    // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        //}

        ////Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{        
        //    // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        //}

        #endregion

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        private TestContext testContextInstance;

        public UIMap UIMap
        {
            get
            {
                if ((this.map == null))
                {
                    this.map = new UIMap();
                }

                return this.map;
            }
        }

        private UIMap map;
    }
}
