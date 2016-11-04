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

namespace GetRangeWindw
{
    public partial class Form1 : Form
    {
        private Spreadsheet guts;

        public Form1(Spreadsheet guts)
        {
            InitializeComponent();
            this.guts = guts;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //Get Starting and Ending Cells
            String StartX = XStartBox.Text;
            String StartY = YStartBox.Text;
            String EndX = XEndBox.Text;
            String EndY = YEndBox.Text;

            if (!checkEntries(StartX, StartY, EndX, EndY))
            {
                MessageBox.Show("Entered Start and end cell entries are invalid.");
                return;
            }

            int XStartRow, YStartRow, XEndRow, YEndRow;

            Int32.TryParse(StartX.Substring(1), out XStartRow);
            Int32.TryParse(StartY.Substring(1), out YStartRow);
            Int32.TryParse(EndX.Substring(1), out XEndRow);
            Int32.TryParse(EndY.Substring(1), out YEndRow);

            if ((XStartRow > XEndRow) || (YStartRow > YEndRow))
            {
                MessageBox.Show("One of the start cells is larger than its end cell.  Please reverse them");
                return;
            }

            if (!(XStartRow - XEndRow == YStartRow - YEndRow))
            {
                MessageBox.Show("Not the same number of X and Y values selected.");
                return;
            }

            // Make a list for X nd Y values
            List<Double> YVals = new List<double>();
            List<Double> XVals = new List<double>();

            double X, Y;
            bool gotX, gotY;

            for (int i = 0; i <= XEndRow - XStartRow; i++)
            {
                gotX = tryGetData(""+StartX[0] + (XStartRow+i), out X);
                gotY = tryGetData("" + StartY[0] + (YStartRow+i), out Y);

                if(gotX && gotY)
                {
                    XVals.Add(X);
                    YVals.Add(Y);
                }
            }

            GraphWindow.Chart chart = new GraphWindow.Chart(XVals, YVals);
            chart.Show();
        }

        private bool checkEntries(string startX, string startY, string endX, string endY)
        {
            return startX.Length > 1 && startX.Length <= 3 &&
                startY.Length > 1 && startY.Length <= 3 &&
                endX.Length > 1 && endX.Length <= 3 &&
                endY.Length > 1 && endY.Length <= 3;
        }

        /// <summary>
        /// Trys to gt out a double value from the spreadsheet.  Returns true if succeeds false otherwise.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool tryGetData(String name, out double value)
        {

            Object result = guts.GetCellValue(name);

            if (result is double)
            {
                value = (double)result;
                return true;
            }

            value = 0;
            return false;

        }
    }
}
