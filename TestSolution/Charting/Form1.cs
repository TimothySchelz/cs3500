﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Charting
{
    public partial class Form1 : Form
    {
        private int[] xVals = new int[]{1,2,3,4,5,6,7,8,9,10};
        private int[] yVals = new int[] {2, 4, 6, 8, 10, 12, 14, 16, 18, 20 };

        public Form1()
        {
            InitializeComponent();
        }
    }
}
