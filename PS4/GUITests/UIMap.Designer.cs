﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by coded UI test builder.
//      Version: 14.0.0.0
//
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------

namespace GUITests
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text.RegularExpressions;
    using System.Windows.Input;
    using Microsoft.VisualStudio.TestTools.UITest.Extension;
    using Microsoft.VisualStudio.TestTools.UITesting;
    using Microsoft.VisualStudio.TestTools.UITesting.WinControls;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;
    using Mouse = Microsoft.VisualStudio.TestTools.UITesting.Mouse;
    using MouseButtons = System.Windows.Forms.MouseButtons;
    
    
    [GeneratedCode("Coded UITest Builder", "14.0.23107.0")]
    public partial class UIMap
    {
        
        /// <summary>
        /// Opens the Spreadsheet
        /// </summary>
        public void OpenSpreadsheet()
        {

            // Launch '%USERPROFILE%\Source\Repos\CS3500Assignments\PS4\SpreadsheetGUI\bin\Debug\SpreadsheetGUI.exe'
            ApplicationUnderTest spreadsheetGUIApplication = ApplicationUnderTest.Launch(this.OpenSpreadsheetParams.ExePath, this.OpenSpreadsheetParams.AlternateExePath);
        }
        
        /// <summary>
        /// Enters the word "Hello" into cell A1.
        /// </summary>
        public void EnterContents()
        {
            #region Variable Declarations
            WinEdit uIContentsBoxEdit = this.UINewSpreadsheetsprdWindow.UIContentsBoxWindow.UIContentsBoxEdit;
            WinButton uIUpdateButton = this.UINewSpreadsheetsprdWindow.UIUpdateWindow.UIUpdateButton;
            #endregion

            // Type 'Hello' in 'ContentsBox' text box
            uIContentsBoxEdit.Text = this.EnterContentsParams.UIContentsBoxEditText;

            // Click 'Update' button
            Mouse.Click(uIUpdateButton, new Point(15, 3));
        }
        
        /// <summary>
        /// Checks to make sure that entries are put into the spreadsheet by checking the valueLabel and making sure it is the correct value.
        /// </summary>
        public void UpdateCheckedByValueLabel()
        {
            #region Variable Declarations
            WinText uIHelloText = this.UINewSpreadsheetsprdWindow.UIHelloWindow.UIHelloText;
            #endregion

            // Verify that the 'DisplayText' property of 'Hello' label equals 'Hello'
            Assert.AreEqual(this.UpdateCheckedByValueLabelExpectedValues.UIHelloTextDisplayText, uIHelloText.DisplayText, "Entered contentis not displayed in valueLabel.");
        }
        
        /// <summary>
        /// Uses the X button to exit the spreadsheet.
        /// </summary>
        public void Xout()
        {
            #region Variable Declarations
            WinButton uICloseButton = this.UINewSpreadsheetsprdWindow.UINewSpreadsheetsprdTitleBar.UICloseButton;
            WinButton uINOButton = this.UIThereareunsavedchangWindow.UINOWindow.UINOButton;
            #endregion

            // Click 'Close' button
            Mouse.Click(uICloseButton, new Point(27, 11));

            // Click '&No' button
            Mouse.Click(uINOButton, new Point(43, 12));
        }
        
        /// <summary>
        /// Checks to make sure the focus is on A1
        /// </summary>
        public void CheckFocusA1()
        {
            #region Variable Declarations
            WinText uIA1Text = this.UINewSpreadsheetsprdWindow.UIA1Window.UIA1Text;
            #endregion

            // Verify that the 'DisplayText' property of 'A1' label equals 'A1'
            Assert.AreEqual(this.CheckFocusA1ExpectedValues.UIA1TextDisplayText, uIA1Text.DisplayText, "The focused cell is incorrect");
        }
        
        #region Properties
        public virtual OpenSpreadsheetParams OpenSpreadsheetParams
        {
            get
            {
                if ((this.mOpenSpreadsheetParams == null))
                {
                    this.mOpenSpreadsheetParams = new OpenSpreadsheetParams();
                }
                return this.mOpenSpreadsheetParams;
            }
        }
        
        public virtual EnterContentsParams EnterContentsParams
        {
            get
            {
                if ((this.mEnterContentsParams == null))
                {
                    this.mEnterContentsParams = new EnterContentsParams();
                }
                return this.mEnterContentsParams;
            }
        }
        
        public virtual UpdateCheckedByValueLabelExpectedValues UpdateCheckedByValueLabelExpectedValues
        {
            get
            {
                if ((this.mUpdateCheckedByValueLabelExpectedValues == null))
                {
                    this.mUpdateCheckedByValueLabelExpectedValues = new UpdateCheckedByValueLabelExpectedValues();
                }
                return this.mUpdateCheckedByValueLabelExpectedValues;
            }
        }
        
        public virtual CheckFocusA1ExpectedValues CheckFocusA1ExpectedValues
        {
            get
            {
                if ((this.mCheckFocusA1ExpectedValues == null))
                {
                    this.mCheckFocusA1ExpectedValues = new CheckFocusA1ExpectedValues();
                }
                return this.mCheckFocusA1ExpectedValues;
            }
        }
        
        public UINewSpreadsheetsprdWindow UINewSpreadsheetsprdWindow
        {
            get
            {
                if ((this.mUINewSpreadsheetsprdWindow == null))
                {
                    this.mUINewSpreadsheetsprdWindow = new UINewSpreadsheetsprdWindow();
                }
                return this.mUINewSpreadsheetsprdWindow;
            }
        }
        
        public UIThereareunsavedchangWindow UIThereareunsavedchangWindow
        {
            get
            {
                if ((this.mUIThereareunsavedchangWindow == null))
                {
                    this.mUIThereareunsavedchangWindow = new UIThereareunsavedchangWindow();
                }
                return this.mUIThereareunsavedchangWindow;
            }
        }
        #endregion
        
        #region Fields
        private OpenSpreadsheetParams mOpenSpreadsheetParams;
        
        private EnterContentsParams mEnterContentsParams;
        
        private UpdateCheckedByValueLabelExpectedValues mUpdateCheckedByValueLabelExpectedValues;
        
        private CheckFocusA1ExpectedValues mCheckFocusA1ExpectedValues;
        
        private UINewSpreadsheetsprdWindow mUINewSpreadsheetsprdWindow;
        
        private UIThereareunsavedchangWindow mUIThereareunsavedchangWindow;
        #endregion
    }
    
    /// <summary>
    /// Parameters to be passed into 'OpenSpreadsheet'
    /// </summary>
    [GeneratedCode("Coded UITest Builder", "14.0.23107.0")]
    public class OpenSpreadsheetParams
    {
        
        #region Fields
        /// <summary>
        /// Launch '%USERPROFILE%\Source\Repos\CS3500Assignments\PS4\SpreadsheetGUI\bin\Debug\SpreadsheetGUI.exe'
        /// </summary>
        public string ExePath = "C:\\Users\\TimothySchelz\\Source\\Repos\\CS3500Assignments\\PS4\\SpreadsheetGUI\\bin\\Debu" +
            "g\\SpreadsheetGUI.exe";
        
        /// <summary>
        /// Launch '%USERPROFILE%\Source\Repos\CS3500Assignments\PS4\SpreadsheetGUI\bin\Debug\SpreadsheetGUI.exe'
        /// </summary>
        public string AlternateExePath = "%USERPROFILE%\\Source\\Repos\\CS3500Assignments\\PS4\\SpreadsheetGUI\\bin\\Debug\\Spreads" +
            "heetGUI.exe";
        #endregion
    }
    
    /// <summary>
    /// Parameters to be passed into 'EnterContents'
    /// </summary>
    [GeneratedCode("Coded UITest Builder", "14.0.23107.0")]
    public class EnterContentsParams
    {
        
        #region Fields
        /// <summary>
        /// Type 'Hello' in 'ContentsBox' text box
        /// </summary>
        public string UIContentsBoxEditText = "Hello";
        #endregion
    }
    
    /// <summary>
    /// Parameters to be passed into 'UpdateCheckedByValueLabel'
    /// </summary>
    [GeneratedCode("Coded UITest Builder", "14.0.23107.0")]
    public class UpdateCheckedByValueLabelExpectedValues
    {
        
        #region Fields
        /// <summary>
        /// Verify that the 'DisplayText' property of 'Hello' label equals 'Hello'
        /// </summary>
        public string UIHelloTextDisplayText = "Hello";
        #endregion
    }
    
    /// <summary>
    /// Parameters to be passed into 'CheckFocusA1'
    /// </summary>
    [GeneratedCode("Coded UITest Builder", "14.0.23107.0")]
    public class CheckFocusA1ExpectedValues
    {
        
        #region Fields
        /// <summary>
        /// Verify that the 'DisplayText' property of 'A1' label equals 'A1'
        /// </summary>
        public string UIA1TextDisplayText = "A1";
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "14.0.23107.0")]
    public class UINewSpreadsheetsprdWindow : WinWindow
    {
        
        public UINewSpreadsheetsprdWindow()
        {
            #region Search Criteria
            this.SearchProperties[WinWindow.PropertyNames.Name] = "NewSpreadsheet.sprd";
            this.SearchProperties.Add(new PropertyExpression(WinWindow.PropertyNames.ClassName, "WindowsForms10.Window", PropertyExpressionOperator.Contains));
            this.WindowTitles.Add("NewSpreadsheet.sprd");
            #endregion
        }
        
        #region Properties
        public UIContentsBoxWindow UIContentsBoxWindow
        {
            get
            {
                if ((this.mUIContentsBoxWindow == null))
                {
                    this.mUIContentsBoxWindow = new UIContentsBoxWindow(this);
                }
                return this.mUIContentsBoxWindow;
            }
        }
        
        public UIUpdateWindow UIUpdateWindow
        {
            get
            {
                if ((this.mUIUpdateWindow == null))
                {
                    this.mUIUpdateWindow = new UIUpdateWindow(this);
                }
                return this.mUIUpdateWindow;
            }
        }
        
        public UIHelloWindow UIHelloWindow
        {
            get
            {
                if ((this.mUIHelloWindow == null))
                {
                    this.mUIHelloWindow = new UIHelloWindow(this);
                }
                return this.mUIHelloWindow;
            }
        }
        
        public UINewSpreadsheetsprdTitleBar UINewSpreadsheetsprdTitleBar
        {
            get
            {
                if ((this.mUINewSpreadsheetsprdTitleBar == null))
                {
                    this.mUINewSpreadsheetsprdTitleBar = new UINewSpreadsheetsprdTitleBar(this);
                }
                return this.mUINewSpreadsheetsprdTitleBar;
            }
        }
        
        public UIA1Window UIA1Window
        {
            get
            {
                if ((this.mUIA1Window == null))
                {
                    this.mUIA1Window = new UIA1Window(this);
                }
                return this.mUIA1Window;
            }
        }
        #endregion
        
        #region Fields
        private UIContentsBoxWindow mUIContentsBoxWindow;
        
        private UIUpdateWindow mUIUpdateWindow;
        
        private UIHelloWindow mUIHelloWindow;
        
        private UINewSpreadsheetsprdTitleBar mUINewSpreadsheetsprdTitleBar;
        
        private UIA1Window mUIA1Window;
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "14.0.23107.0")]
    public class UIContentsBoxWindow : WinWindow
    {
        
        public UIContentsBoxWindow(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Search Criteria
            this.SearchProperties[WinWindow.PropertyNames.ControlName] = "ContentsBox";
            this.WindowTitles.Add("NewSpreadsheet.sprd");
            #endregion
        }
        
        #region Properties
        public WinEdit UIContentsBoxEdit
        {
            get
            {
                if ((this.mUIContentsBoxEdit == null))
                {
                    this.mUIContentsBoxEdit = new WinEdit(this);
                    #region Search Criteria
                    this.mUIContentsBoxEdit.WindowTitles.Add("NewSpreadsheet.sprd");
                    #endregion
                }
                return this.mUIContentsBoxEdit;
            }
        }
        #endregion
        
        #region Fields
        private WinEdit mUIContentsBoxEdit;
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "14.0.23107.0")]
    public class UIUpdateWindow : WinWindow
    {
        
        public UIUpdateWindow(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Search Criteria
            this.SearchProperties[WinWindow.PropertyNames.ControlName] = "Enter";
            this.WindowTitles.Add("NewSpreadsheet.sprd");
            #endregion
        }
        
        #region Properties
        public WinButton UIUpdateButton
        {
            get
            {
                if ((this.mUIUpdateButton == null))
                {
                    this.mUIUpdateButton = new WinButton(this);
                    #region Search Criteria
                    this.mUIUpdateButton.SearchProperties[WinButton.PropertyNames.Name] = "Update";
                    this.mUIUpdateButton.WindowTitles.Add("NewSpreadsheet.sprd");
                    #endregion
                }
                return this.mUIUpdateButton;
            }
        }
        #endregion
        
        #region Fields
        private WinButton mUIUpdateButton;
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "14.0.23107.0")]
    public class UIHelloWindow : WinWindow
    {
        
        public UIHelloWindow(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Search Criteria
            this.SearchProperties[WinWindow.PropertyNames.ControlName] = "ValueLabel";
            this.WindowTitles.Add("NewSpreadsheet.sprd");
            #endregion
        }
        
        #region Properties
        public WinText UIHelloText
        {
            get
            {
                if ((this.mUIHelloText == null))
                {
                    this.mUIHelloText = new WinText(this);
                    #region Search Criteria
                    this.mUIHelloText.SearchProperties[WinText.PropertyNames.Name] = "Hello";
                    this.mUIHelloText.WindowTitles.Add("NewSpreadsheet.sprd");
                    #endregion
                }
                return this.mUIHelloText;
            }
        }
        #endregion
        
        #region Fields
        private WinText mUIHelloText;
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "14.0.23107.0")]
    public class UINewSpreadsheetsprdTitleBar : WinTitleBar
    {
        
        public UINewSpreadsheetsprdTitleBar(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Search Criteria
            this.WindowTitles.Add("NewSpreadsheet.sprd");
            #endregion
        }
        
        #region Properties
        public WinButton UICloseButton
        {
            get
            {
                if ((this.mUICloseButton == null))
                {
                    this.mUICloseButton = new WinButton(this);
                    #region Search Criteria
                    this.mUICloseButton.SearchProperties[WinButton.PropertyNames.Name] = "Close";
                    this.mUICloseButton.WindowTitles.Add("NewSpreadsheet.sprd");
                    #endregion
                }
                return this.mUICloseButton;
            }
        }
        #endregion
        
        #region Fields
        private WinButton mUICloseButton;
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "14.0.23107.0")]
    public class UIA1Window : WinWindow
    {
        
        public UIA1Window(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Search Criteria
            this.SearchProperties[WinWindow.PropertyNames.ControlName] = "SelectionLabel";
            this.WindowTitles.Add("NewSpreadsheet.sprd");
            #endregion
        }
        
        #region Properties
        public WinText UIA1Text
        {
            get
            {
                if ((this.mUIA1Text == null))
                {
                    this.mUIA1Text = new WinText(this);
                    #region Search Criteria
                    this.mUIA1Text.SearchProperties[WinText.PropertyNames.Name] = "A1";
                    this.mUIA1Text.WindowTitles.Add("NewSpreadsheet.sprd");
                    #endregion
                }
                return this.mUIA1Text;
            }
        }
        #endregion
        
        #region Fields
        private WinText mUIA1Text;
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "14.0.23107.0")]
    public class UIThereareunsavedchangWindow : WinWindow
    {
        
        public UIThereareunsavedchangWindow()
        {
            #region Search Criteria
            this.SearchProperties[WinWindow.PropertyNames.Name] = "There are unsaved changes.";
            this.SearchProperties[WinWindow.PropertyNames.ClassName] = "#32770";
            this.WindowTitles.Add("There are unsaved changes.");
            #endregion
        }
        
        #region Properties
        public UINOWindow UINOWindow
        {
            get
            {
                if ((this.mUINOWindow == null))
                {
                    this.mUINOWindow = new UINOWindow(this);
                }
                return this.mUINOWindow;
            }
        }
        #endregion
        
        #region Fields
        private UINOWindow mUINOWindow;
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "14.0.23107.0")]
    public class UINOWindow : WinWindow
    {
        
        public UINOWindow(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Search Criteria
            this.SearchProperties[WinWindow.PropertyNames.ControlId] = "7";
            this.WindowTitles.Add("There are unsaved changes.");
            #endregion
        }
        
        #region Properties
        public WinButton UINOButton
        {
            get
            {
                if ((this.mUINOButton == null))
                {
                    this.mUINOButton = new WinButton(this);
                    #region Search Criteria
                    this.mUINOButton.SearchProperties[WinButton.PropertyNames.Name] = "No";
                    this.mUINOButton.WindowTitles.Add("There are unsaved changes.");
                    #endregion
                }
                return this.mUINOButton;
            }
        }
        #endregion
        
        #region Fields
        private WinButton mUINOButton;
        #endregion
    }
}
