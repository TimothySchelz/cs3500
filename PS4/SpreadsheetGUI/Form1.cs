/*
 * Authored by Gray Marchese, u, and Timothy Schelz, u0851027 
 * November, 2016 
 */
using SpreadsheetUtilities;
using SS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetGUI
{

    public partial class Form1 : Form
    {
        // The backend of the spreadsheet.
        private Spreadsheet guts;
        // The validator for the spreadsheet to use
        private Func<string, bool> validator;
        // The name of the file that this spreadsheet will be saved to or was loaded from
        private string filename;

        /// <summary>
        /// A public constructor that creates the form.
        /// </summary>
        public Form1()
        {
            // The validator for the guts of the spreadsheet.  It makes sure that any 
            // name is of the form C35 where the letters go from A to Z and the numbrs go 
            // from 1 to 99.
            validator = delegate (string s)
            {
                // Checks the leading letter
                if ( !Char.IsLetter(s[0]))
                {
                    return false;
                }

                // Gets the number out of the string.  If it fails row stays as 0
                int row = 0;
                Int32.TryParse(s.Substring(1), out row);

                // Checks to make sure the number is in the correct range.  If it 
                // failed to parse row will not be in the range
                if(row < 1 || row > 99)
                {
                    return false;
                }

                //If it does not fail then it passes
                return true;
            };

            // Creates the back end of the spreadsheet and passes the validator.  The 
            // normalizer just makes sure that it capitalizes the names.  This makes it 
            // case insensitive.
            guts = new Spreadsheet(validator, s => s.ToUpper(), "ps6");

            // Some necessary component junk
            InitializeComponent();

            // Sets up the panel.
            panelSetUp("NewSpreadsheet.sprd");

            //Set the minimum size of the window.
            this.MinimumSize = new Size(200, 200);
        }

        /// <summary>
        /// This just does a bit of the set up for a panel.
        /// </summary>
        /// <param name="filename">the name of the file that was loaded or the default name</param>
        private void panelSetUp(String filename)
        {
            spreadsheetPanel1.SelectionChanged += displaySelection;
            ContentsBox.Focus();
            ValueLabel.Text = "";
            this.filename = filename;
            Text = filename;
            spreadsheetPanel1.SetSelection(0, 0);
        }

        /// <summary>
        /// The handler for when the panel is first loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void spreadsheetPanel1_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Makes a new form.  Called when the new menustrip item is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void makeNewForm(object sender, EventArgs e)
        {
            // Just creates a new spreadsheet.  The DemoApplicationContext takes care
            // of making it seperate from the previous window.
            DemoApplicationContext.getAppContext().RunForm(new Form1());
        }

        /// <summary>
        /// The method called when the X button is clicked.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Do the overridden method
            base.OnFormClosing(e);
            // Performs the usual closing stuff
            closeForm(this , e);
        }

        /// <summary>
        /// The handler for when the user closes the window from either the X button or the 
        /// close option in File
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeForm(object sender, EventArgs e)
        {
            // Checks to see if any of the info in the spreadsheet has changed
            if (guts.Changed)
            {
                // Creates the warning message box
                string messageBoxText = "Do you want to save changes?";
                string caption = "There are unsaved changes.";
                MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
                MessageBoxIcon icon = MessageBoxIcon.Warning;

                // Displays and gets the result of the warning message box
                DialogResult result = MessageBox.Show(messageBoxText, caption, buttons, icon);

                // Checks what the result was
                switch (result)
                {
                    // If the user does want to save performs the usual saving behavior and 
                    // then closes
                    case DialogResult.Yes:
                        save(sender, e);
                        Close();
                        break;

                    // If they do not want to save then it just closes.
                    case DialogResult.No:
                        break;

                    // If they cancel It stops the closing actions
                    case DialogResult.Cancel:
                        if(e is FormClosingEventArgs)
                        {
                            FormClosingEventArgs closingEvent = (FormClosingEventArgs)e;
                            closingEvent.Cancel = true;
                        }
                        break;
                }
            }

        }

        /// <summary>
        /// Private handler for when the user changed the selection in the SpreadsheetPanel
        /// </summary>
        /// <param name="ss"></param>
        private void displaySelection(SpreadsheetPanel ss)
        {
            int row, col;
            string value, contents;

            // Gets the selection
            ss.GetSelection(out col, out row);
            // Gets the value in the cell
            ss.GetValue(col, row, out value);

            // Changes the number associated with the col to a letter
            string colName = valueToName(col+1);

            // Displays the selection in the selection label
            SelectionLabel.Text = colName + (row+1);
            // Sets the value of the value label to the value in the cell
            ValueLabel.Text = value;

            // Puts the focus on the Contents box so that they can immediately start 
            // typing
            ContentsBox.Focus();

            // Gets the full name of the cell
            string name = valueToName(col+1) + (row+1);
            // Takes the content of the cell and converts it to a string
            contents = ContentsToString(guts.GetCellContents(name));
            // Puts the contents in the contentsBox
            ContentsBox.Text = contents;
            
        }

        /// <summary>
        /// Takes the contents of a cell and turns it into a string to be displayed... Basically 
        /// just appends a "=" to the beginning of fomrula or just does toString
        /// </summary>
        /// <param name="contents">The contents of a string from the guts of the Spreadsheet</param>
        /// <returns>the contents in string from</returns>
        private String ContentsToString(Object contents)
        {
            if (contents is Formula)
            {
                Formula cont = (Formula)contents;
                return "=" + cont.ToString();
            } else
            {
                return contents.ToString();
            }
        }

        /// <summary>
        /// Changes the numeric value of col and converts it to a letter
        /// </summary>
        /// <param name="col">the number of the column</param>
        /// <returns>the letter of the column</returns>
        private string valueToName(int col)
        {
            double colVal = col;

            //65 = unicode for A
            colVal += 64;

            return "" + (char)(colVal);
        }

        /// <summary>
        /// Converts a name of a cell to numeric values
        /// </summary>
        /// <param name="name"> The name of the cell </param>
        /// <param name="row">The numeric value of the row</param>
        /// <param name="col">The numeric value of the col</param>
        private void nameToCoordinate(string name, out int row, out int col)
        {
            col = name[0]-65;  
            Int32.TryParse(name.Substring(1), out row);

            row--;
        }

        /// <summary>
        /// Updates the cells and other parts of the SS when the update button is 
        /// clicked or enter is hit.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updateCells(object sender, EventArgs e)
        {
            // Gets the entered contents and the name of the cell
            string contents = ContentsBox.Text;
            string name = SelectionLabel.Text;

            // Stores the original contents if we must go back and replace them
            string originalContents = ContentsToString(guts.GetCellContents(name));

            // A list of cells that might need to be changed
            ISet<string> changedCells = new HashSet<string>();

            // Tries to set the contents of a cell.
            try
            {
                changedCells = guts.SetContentsOfCell(name, contents);
            }
            // If there is a problem we create a warning box telling the user to 
            // get it together.
            catch(FormulaFormatException error)
            {
                MessageBox.Show(error.Message);
                // If there is a problem we don't want to do anything else.  This 
                // way anything after this will only execute if the info is set without 
                // a hitch.
                return;
            }

            // Gets the value of the changed cell
            object value = guts.GetCellValue(name);

            // If the value is a formula error we show a warning message and reset the contents
            // to what they used to be.
            if (value is FormulaError)
            {
                guts.SetContentsOfCell(name, originalContents);

                FormulaError error = (FormulaError)value;
                MessageBox.Show(error.Reason);
            }
            // Otherwise we go through and change everythign that needs to be changed
            else {
                // Sets the valuelabel to the current value as a string
                ValueLabel.Text = "" + value;

                // For every cell that could have been changed we set the value to the correct value
                updateSpreadsheetCells(changedCells);
            }
        }


        /// <summary>
        /// Takes a list of cell names and updates the list of cells in the spreadsheet gui
        /// </summary>
        /// <param name="cells">cells to be updated</param>
        private void updateSpreadsheetCells(IEnumerable<String> cells)
        {
            int row, col;
            foreach (string cell in cells)
            {
                nameToCoordinate(cell, out row, out col);
                spreadsheetPanel1.SetValue(col, row, "" + guts.GetCellValue(cell));
            }
        }

        /// <summary>
        /// A handler for when enter is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void keyPressed(object sender, KeyPressEventArgs e)
        {
            // Chacks if the pressed button was the ENTER key
            switch (e.KeyChar)
            {
                //If it was entered we update all cells as if the Update button was clicked
                case (char)Keys.Enter:
                    updateCells(sender, e);
                    break;
            }
        }

        /// <summary>
        /// A Handler for when the save button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void save(object sender, EventArgs e)
        {
            // Opens and setsup the save dialog
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Sreadsheet Files|*.sprd|All Files|*.*";
            saveDialog.Title = "Save";
            saveDialog.FileName = filename;
            saveDialog.ShowDialog();

            // Check if they entered an empty name for the savefile
            if(saveDialog.FileName != "")
            {
                // Saves the spreadsheet and sets the name of this spreadsheet to the
                // name of the spreadsheet
                guts.Save(saveDialog.FileName);
                filename = saveDialog.FileName;
                Text = filename;
            }
            else
            {
                // warn the user not to enter an empty filename
                MessageBox.Show("Use non-empty file name.");
            }

        }

        /// <summary>
        /// handler for loading a new spreadsheet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadSpreadsheet(object sender, EventArgs e)
        {
            // Sets up the load dialog box
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Sreadsheet Files|*.sprd|All Files|*.*";
            openDialog.Title = "Open a Spreadsheet";
            String file;

            // Display the dialogbox and check if they clicked ok
            if(openDialog.ShowDialog() == DialogResult.OK)
            {
                // Gets the filename
                file = openDialog.FileName;

                // Creates some intermediate guts to load in before discarding the old guts
                Spreadsheet intermediateGuts;
                try
                {
                    // Loads the new spreadsheet
                    intermediateGuts = new Spreadsheet(file, validator, s => s.ToUpper(), "ps6");
                } catch (SpreadsheetReadWriteException error)
                {
                    // If there is a problem an error box is displayed
                    MessageBox.Show(error.Message);
                    return;
                }

                // A set of cells that may need to be updated.
                HashSet<String> cellsToUpdate = new HashSet<string>();

                // Loads all the cells that currently have entries into the list of cells to be 
                // updated.  They may need to be replaced or erased for the new spreadsheeet.
                foreach(String cell in guts.GetNamesOfAllNonemptyCells())
                {
                    cellsToUpdate.Add(cell);
                }

                // Sets the guts to the newly loaded spreadsheet
                guts = intermediateGuts;

                // Loads all the cells that have entries in the new spreadsheet.  They will need 
                // to be drawn on the spreadsheet
                foreach (String cell in guts.GetNamesOfAllNonemptyCells())
                {
                    cellsToUpdate.Add(cell);
                }

                // Sets the name of this spreadsheet to the name of the loaded file and displays 
                // it at the top of the window.
                this.filename = file;
                this.Text = filename;

                // Updates all the cells that may need to change.
                updateSpreadsheetCells(cellsToUpdate);
            }
            
        }

        /// <summary>
        /// Handler for when the user clicks on the Help option on the menu strip.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AskForHelp(object sender, EventArgs e)
        {
            MessageBox.Show("Click on any cell with your mouse to select it.  At the top the cell name and the value " + 
                "are displayed.  Next to them is an editable textbox with the current contents of the cells.  You can " + 
                "change the contents in this textbox and then hit \"Update\" or type ENTER to update the contents of the" + 
                " cell.");
        }
    }
}
