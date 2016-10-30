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
        
        private Spreadsheet guts;
        private Func<string, bool> validator;
        private string filename;

        public Form1()
        {
            validator = delegate (string s)
            {
                if ( !Char.IsLetter(s[0]))
                {
                    return false;
                }

                int row;
                Int32.TryParse(s.Substring(1), out row);

                if(row < 1 || row > 99)
                {
                    return false;
                }

                return true;
            };

            guts = new Spreadsheet(validator, s => s.ToUpper(), "ps6");

            InitializeComponent();

            panelSetUp("NewSpreadsheet.sprd");

            //Set the minimum size of the window.
            this.MinimumSize = new Size(200, 200);
        }

        private void panelSetUp(String filename)
        {
            spreadsheetPanel1.SelectionChanged += displaySelection;
            ContentsBox.Focus();
            ValueLabel.Text = "";
            this.filename = filename;
            Text = filename;
            spreadsheetPanel1.SetSelection(0, 0);
        }

        private void spreadsheetPanel1_Load(object sender, EventArgs e)
        {

        }

        private void makeNewForm(object sender, EventArgs e)
        {
            DemoApplicationContext.getAppContext().RunForm(new Form1());
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            closeForm(this , e);
        }

        private void closeForm(object sender, EventArgs e)
        {
            if (guts.Changed)
            {
                string messageBoxText = "Do you want to save changes?";
                string caption = "There are unsaved changes.";
                MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
                MessageBoxIcon icon = MessageBoxIcon.Warning;

                DialogResult result = MessageBox.Show(messageBoxText, caption, buttons, icon);

                switch (result)
                {
                    case DialogResult.Yes:
                        save(sender, e);
                        Close();
                        break;

                    case DialogResult.No:
                        break;

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

        private void displaySelection(SpreadsheetPanel ss)
        {

            int row, col;
            string value, contents;

            ss.GetSelection(out col, out row);
            ss.GetValue(col, row, out value);

            string colName = valueToName(col+1);

            SelectionLabel.Text = colName + (row+1);
            ValueLabel.Text = value;

            ContentsBox.Focus();

            
            string name = valueToName(col+1) + (row+1);
            contents = ContentsToString(guts.GetCellContents(name));
            ContentsBox.Text = contents;
            
        }

        /// <summary>
        /// Takes the contents of a cell and turns it into a string to be displayed
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

        private string valueToName(int col)
        {
            double colVal = col;

            //65 = unicode for A
            colVal += 64;

            return "" + (char)(colVal);
        }

        private void nameToCoordinate(string name, out int row, out int col)
        {

            col = name[0]-65;  
            Int32.TryParse(name.Substring(1), out row);

            row--;
        }

        private void updateCells(object sender, EventArgs e)
        {
            string contents = ContentsBox.Text;
            string name = SelectionLabel.Text;

            string originalContents = ContentsToString(guts.GetCellContents(name));

            ISet<string> changedCells = new HashSet<string>();
            try
            {
                changedCells = guts.SetContentsOfCell(name, contents);
            }
            catch(FormulaFormatException error)
            {
                MessageBox.Show(error.Message);
                return;
            }

            object value = guts.GetCellValue(name);

            if (value is FormulaError)
            {
                guts.SetContentsOfCell(name, originalContents);

                FormulaError error = (FormulaError)value;
                MessageBox.Show(error.Reason);
            }
            else {

                ValueLabel.Text = "" + value;
                int row, col;

                foreach (string cell in changedCells)
                {
                    nameToCoordinate(cell, out row, out col);
                    spreadsheetPanel1.SetValue(col, row, "" + guts.GetCellValue(cell));
                }
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

        private void keyPressed(object sender, KeyPressEventArgs e)
        {
           

            switch (e.KeyChar)
            {

                case (char)Keys.Enter:
                    updateCells(sender, e);
                    break;

                

            }

        }

        private void save(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Sreadsheet Files|*.sprd|All Files|*.*";
            saveDialog.Title = "Save";
            saveDialog.FileName = filename;
            saveDialog.ShowDialog();

            if(saveDialog.FileName != "")
            {
                guts.Save(saveDialog.FileName);
                filename = saveDialog.FileName;
                Text = filename;
            }
            else
            {
                MessageBox.Show("Use non-empty file name.");
            }

        }

        private void LoadSpreadsheet(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Sreadsheet Files|*.sprd|All Files|*.*";
            openDialog.Title = "Open a Spreadsheet";
            String file;

            if(openDialog.ShowDialog() == DialogResult.OK)
            {
                file = openDialog.FileName;

                Spreadsheet intermediateGuts;
                try
                {
                    intermediateGuts = new Spreadsheet(file, validator, s => s.ToUpper(), "ps6");
                } catch (SpreadsheetReadWriteException error)
                {
                    MessageBox.Show(error.Message);
                    return;
                }

                HashSet<String> cellsToUpdate = new HashSet<string>();
                foreach(String cell in guts.GetNamesOfAllNonemptyCells())
                {
                    cellsToUpdate.Add(cell);
                }

                guts = intermediateGuts;

                foreach (String cell in guts.GetNamesOfAllNonemptyCells())
                {
                    cellsToUpdate.Add(cell);
                }

                this.filename = file;
                this.Text = filename;

                updateSpreadsheetCells(cellsToUpdate);
            }
            
        }

        private void AskForHelp(object sender, EventArgs e)
        {
            MessageBox.Show("Click on any cell with your mouse to select it.  At the top the cell name and the value are displayed.  Next to them is an editable textbox with the current contents of the cells.  You can change the contents in this textbox and then hit \"Update\" or type ENTER to update the contents of the cell.");
        }
    }
}
