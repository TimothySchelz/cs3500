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
        public void UpdateContentsCheckValueLabel()
        {
            this.UIMap.OpenSpreadsheet();
            this.UIMap.EnterContents();
            this.UIMap.UpdateCheckedByValueLabel();
            this.UIMap.Xout();
        }

        [TestMethod]
        public void UpdateContentsCheckNameLabel()
        {
            this.UIMap.OpenSpreadsheet();
            this.UIMap.EnterContents();
            this.UIMap.CheckFocusA1();
            this.UIMap.Xout(); 
        }

        [TestMethod]
        public void UpdateContentsCheckContentsBox()
        {
            this.UIMap.OpenSpreadsheet();
            this.UIMap.EnterContents();
            this.UIMap.CheckContentBox();
            this.UIMap.Xout();
        }

        [TestMethod]
        public void UpdateOverwriteContents()
        {
            this.UIMap.OpenSpreadsheet();
            this.UIMap.EnterContents();
            this.UIMap.OverwriteA1();
            this.UIMap.CheckOverwirteWorked();
            this.UIMap.Xout();
        }

        [TestMethod]
        public void NewCheckExistanceofSecondWindow()
        {
            this.UIMap.OpenSpreadsheet();
            this.UIMap.OpenSecondSpreadsheet();
            this.UIMap.CheckEmptyContentsOnSecondSpreadsheet();
            this.UIMap.Close2EmptySpreadsheets();
        }

        [TestMethod]
        public void NewCheckNewWindowIsEmpty()
        {
            this.UIMap.OpenSpreadsheet();
            this.UIMap.InputInSS1AndOpenSS2();
            this.UIMap.CheckContentsOfOldAndNewSS();
            this.UIMap.CloseSpreadsheets();
        }

        [TestMethod]
        public void NewCheckWindowsAreNotLinked()
        {
            this.UIMap.OpenSpreadsheet();
            this.UIMap.OpenSecondSSAndEnterValues();
            this.UIMap.CheckSS1B2AndSS2A1();
            this.UIMap.Closing();

        }

        [TestMethod]
        public void CloseCloseOriginalWindow()
        {
            this.UIMap.OpenSpreadsheet();
            this.UIMap.OpenNewCloseOriginal();
            this.UIMap.SecondWindowExists();
            this.UIMap.CloseSecond();

        }

        [TestMethod]
        public void HelpCheckHelpPopUp()
        {
            this.UIMap.OpenSpreadsheet();
            this.UIMap.OpenHelp();
            this.UIMap.CheckHelpText();
            this.UIMap.CloseHelpandWindow();

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
