using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphWindow
{
    public partial class Chart : Form
    {

        List<double> XValues;
        List<double> YValues;


        public Chart(List<double> XVals, List<double> YVals)
        {
            InitializeComponent();

            XValues = XVals;
            YValues = YVals;

            FillChart();
        }

        private void FillChart()
        {

            for( int i = 0; i < XValues.Count; i++)
            {
                chart1.Series[0].Points[i].XValue = XValues[i];
                chart1.Series[0].Points[i].YValues[0] = YValues[i];
            }

        }
    }
}
