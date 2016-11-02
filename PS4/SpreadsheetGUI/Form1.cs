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
    /// <summary>
    /// A windowed GUI for a single26x99 spreadsheet.
    /// </summary>
    public partial class Form1 : Form
    {
        
        //Back end utility for handling cell values
        private Spreadsheet guts;

        //Restrictions on the backend cells (names only range from A1 - Z99)
        private Func<string, bool> validator;

        //Name of current spreadsheet
        private string filename;

        /// <summary>
        /// Creates a new windowed spreadsheet with all cells empty and "NewSpreadsheet.sprd"
        /// as the file name.
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

            //Initializes selection and filename
            panelSetUp("NewSpreadsheet.sprd");

            //Set the minimum size of the window.
            this.MinimumSize = new Size(200, 200);
        }

        /// <summary>
        /// Initializes selection information and filename for a spreadsheet.
        /// </summary>
        /// <param name="filename"></param>
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
        /// Runs a new windowed spreadsheet in the current program context.
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
        /// Updates the display for cell value, contents, and address. Also redirects GUI focus to
        /// the contents box for quickly updating many cells.
        /// </summary>
        /// <param name="ss"></param>
        private void displaySelection(SpreadsheetPanel ss)
        {
            int row, col;
            

            // Gets the selection
            ss.GetSelection(out col, out row);
            
            //Changes the Name, Value labels and the contents box.
            UpdateLabels(col, row);
        }

        /// <summary>
        /// Updates all the labels and the text box at the top of the form.  Also sets the focus.
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        private void UpdateLabels(int col, int row)
        {

            String contents;
            string value;

            // Gets the value in the cell
            spreadsheetPanel1.GetValue(col, row, out value);

            // Changes the number associated with the col to a letter
            string colName = valueToName(col + 1);

            //Updates address label and value label.
            SelectionLabel.Text = colName + (row + 1);
            // Sets the value of the value label to the value in the cell
            ValueLabel.Text = value;

            // Puts the focus on the Contents box so that they can immediately start 
            // typing
            ContentsBox.Focus();

            //Updates contents box
            string name = valueToName(col + 1) + (row + 1);
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
        /// Transforms the unicode value of a column index to its corrisponding alphabetical letter.
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        private string valueToName(int col)
        {
            double colVal = col;

            //65 = unicode for A
            colVal += 64;

            return "" + (char)(colVal);
        }

        /// <summary>
        /// Transforms a cell name to coodrinates for the spreadsheet panel. The indicies "row" and "col" are
        /// updated as out variables.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        private void nameToCoordinate(string name, out int row, out int col)
        {
            col = name[0]-65;  
            Int32.TryParse(name.Substring(1), out row);

            row--;
        }

        /// <summary>
        /// Updates a cell's contents and the contents of all dependent cells
        /// after the user changes the value in the contents box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updateCells(object sender, EventArgs e)
        {
            // Gets the entered contents and the name of the cell
            string contents = ContentsBox.Text;
            string name = SelectionLabel.Text;

            //Store original contents in case an error is made.
            string originalContents = ContentsToString(guts.GetCellContents(name));

            //Sets the new contents of all affected cells.
            ISet<string> changedCells = new HashSet<string>();

            // Tries to set the contents of a cell.
            try
            {
                changedCells = guts.SetContentsOfCell(name, contents);
            }
            catch(CircularException error)
            {
                MessageBox.Show("The entered formula creates a circular dependency.  Make sure your formula"+
                    " does not depend on the cell it is in.");
                return;
            }
            catch(FormulaFormatException error)
            {
                MessageBox.Show(error.Message);
                // If there is a problem we don't want to do anything else.  This 
                // way anything after this will only execute if the info is set without 
                // a hitch.
                return;
            }

            //Attempts to evaluate inital updated cell
            object value = guts.GetCellValue(name);

            //If a formula error occurs in evalutation, the error discription is displayed.
            //Otherwise, updates the value labels in the spreadsheet.
            if (value is FormulaError)
            {
                guts.SetContentsOfCell(name, originalContents);

                FormulaError error = (FormulaError)value;
                MessageBox.Show(error.Reason);
            }
            // Otherwise we go through and change everythign that needs to be changed
            else {

                //Updates value label
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
        /// Listens for a key to be pressed and controls the spreadsheet accordingly.
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

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Tab)
            {
                TabSelection(1);
                return true;
            }
            if (keyData == (Keys.Tab | Keys.Shift))
            {
                TabSelection(-1);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void TabSelection(int shift)
        {
            int col, row;
            spreadsheetPanel1.GetSelection(out col, out row);
            
            //Change the selection to the cell to the right of the current one.
            spreadsheetPanel1.SetSelection(col + shift, row);
            //Changes all the labels and junk to the new selection
            UpdateLabels(col + shift, row);            
        }

        /// <summary>
        /// Saves the current state of the spreadsheet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void save(object sender, EventArgs e)
        {
            //Prompts the user to save
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Sreadsheet Files|*.sprd|All Files|*.*";
            saveDialog.Title = "Save";
            saveDialog.FileName = filename;
            saveDialog.ShowDialog();

            //If the User names the file an empty string, reports an error.
            //Otherwise, saves the spreadsheet with entered name.
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
        /// Runs an file open dialog for the user, allowing them to browse and open
        /// any .sprd file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadSpreadsheet(object sender, EventArgs e)
        {
            //Opens dialog for the user
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Sreadsheet Files|*.sprd|All Files|*.*";
            openDialog.Title = "Open a Spreadsheet";
            String file;

            //Executes if the user has made a selection.
            if(openDialog.ShowDialog() == DialogResult.OK)
            {
                // Gets the filename
                file = openDialog.FileName;

                // Creates some intermediate guts to load in before discarding the old guts
                Spreadsheet intermediateGuts;

                try
                {
                    //Attempts to construct a new internal spreadsheet representation
                    intermediateGuts = new Spreadsheet(file, validator, s => s.ToUpper(), "ps6");
                } catch (SpreadsheetReadWriteException error)
                {
                    //If file loading is not sucessful, displays a message and exits dialog.
                    MessageBox.Show(error.Message);
                    return;
                }

                //Gets all cells in the old spreadsheet that were not empty
                //They must be updated in the display.
                HashSet<String> cellsToUpdate = new HashSet<string>();

                // Loads all the cells that currently have entries into the list of cells to be 
                // updated.  They may need to be replaced or erased for the new spreadsheeet.
                foreach(String cell in guts.GetNamesOfAllNonemptyCells())
                {
                    cellsToUpdate.Add(cell);
                }

                //Our current internal representation is updated.
                guts = intermediateGuts;

                //Gets all cells in the new spreadsheet that are not empty.
                foreach (String cell in guts.GetNamesOfAllNonemptyCells())
                {
                    cellsToUpdate.Add(cell);
                }

                // Sets the name of this spreadsheet to the name of the loaded file and displays 
                // it at the top of the window.
                this.filename = file;
                this.Text = filename;

                //Updates all changed cells in the display.
                updateSpreadsheetCells(cellsToUpdate);
            }
            
        }

        /// <summary>
        /// Breif instruction is provided to the user if they select "Help".
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

        /// <summary>
        /// Handler for when the user tries to close the form.  Checks to see if they want to save any unsaved changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tryToClose(object sender, FormClosingEventArgs e)
        {

            if (guts.Changed)
            {
                //Creates the warning, prompts user to save
                string messageBoxTxt = "Do you want to save changes?";
                string caption = "There are unsaved changes.";
                MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
                MessageBoxIcon icon = MessageBoxIcon.Warning;

                DialogResult result = MessageBox.Show(messageBoxTxt, caption, buttons, icon);

                switch (result)
                {

                    //Closes after saving
                    case DialogResult.Yes:
                        save(sender, e);
                        break;

                    //Closes without saving
                    case DialogResult.No:
                        break;

                    //Does not close
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }
        }

        private void closeFromMenu(object sender, EventArgs e)
        {
            Close();
        }
    }
}
