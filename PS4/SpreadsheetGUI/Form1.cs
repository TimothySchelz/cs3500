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

            //spreadsheetPanel1.SelectionChanged += ;
            spreadsheetPanel1.SetSelection(1, 1);

        }

        private void spreadsheetPanel1_Load(object sender, EventArgs e)
        {

        }
    }
}
