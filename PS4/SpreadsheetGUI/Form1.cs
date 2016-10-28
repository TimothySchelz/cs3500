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
        public Form1()
        {
            InitializeComponent();

            spreadsheetPanel1.SelectionChanged += displaySelection;
            spreadsheetPanel1.SetSelection(1, 1);

        }

        private void spreadsheetPanel1_Load(object sender, EventArgs e)
        {

        }

        private void makeNewForm(object sender, EventArgs e)
        {
            DemoApplicationContext.getAppContext().RunForm(new Form1());
        }

        private void closeForm(object sender, EventArgs e)
        {
            Close();
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


        }

        private string valueToName(int col)
        {
            double colVal = col;

            //65 = unicode for A
            colVal += 64;

            return "" + (char)colVal ;
        }
    }
}
