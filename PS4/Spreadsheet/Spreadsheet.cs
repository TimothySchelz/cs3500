// Written by Timothy Schelz, u0851027, September 2016

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetUtilities;

namespace SS
{
    public class Spreadsheet : AbstractSpreadsheet
    {
        private Dictionary<String, Cell> cells = new Dictionary<String, Cell>();
        /// <summary>
        /// A public constructor to create a new Spreadsheet object.  It just creates an empty 
        /// spreadsheet.  It should not have any filled cells.  Nothing special
        /// </summary>
        public Spreadsheet()
        {
            throw new NotImplementedException();
        }

        public override object GetCellContents(string name)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            throw new NotImplementedException();
        }

        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            throw new NotImplementedException();
        }

        public override ISet<string> SetCellContents(string name, string text)
        {
            throw new NotImplementedException();
        }

        public override ISet<string> SetCellContents(string name, double number)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// A class to act as one cell in a spreadsheet.  It can hold either a String or a Formula.
    /// Let's try to make it immutable.  This way when it gets replaced by something it just 
    /// makes a new one.
    /// </summary>
    internal class Cell
    {
        // This cell holds either a String or a Formula
        private String StringContent;
        private Formula FormContent;

        // The lookup delegate to be used
        Func<string, Double> variableFinder;
        
        /// <summary>
        /// The type of cell it is.  It will be "String" if there is a string in the cell
        /// It will be "Formula" if there is a Formula in the cell.
        /// </summary>
        internal String Type
        {
            get;

            private set;
        }

        /// <summary>
        /// Constructor to build a Cell from a String
        /// </summary>
        /// <param name="s">The string to go in the cell</param>
        internal Cell(String s)
        {
            Type = "String";
            StringContent = s;
        }

        /// <summary>
        /// Constructor to build a Cell from a Formula
        /// </summary>
        /// <param name="f"></param>
        /// <param name="l">The delegate used to look up varaibles.  Takes a string and
        /// returns a double</param>
        internal Cell(Formula f, Func<string, Double> l)
        {
            Type = "Formula";
            FormContent = f;
            variableFinder = l;
        }

        /// <summary>
        /// Constructor to build a Cell from a Double
        /// </summary>
        /// <param name="d">The double to be put into a cell</param>
        internal Cell(double d)
        {
            Type = "Formula";
            FormContent = new Formula("" + d);
        }

        /// <summary>
        /// A no argument Constructor
        /// </summary>
        internal Cell()
        {
            Type = "String";
            StringContent = "";
        }

        /// <summary>
        /// Get the content of the Cell
        /// </summary>
        /// <returns>The Content of the cell</returns>
        internal object getContents()
        {
            if (Type == "String")
            {
                return StringContent;
            } else
            {
                return FormContent;
            }
        }

        /// <summary>
        /// Get the value of the Cell
        /// </summary>
        /// <returns>The value of the cell</returns>
        internal object getValue()
        {
            if (Type == "String")
            {
                return StringContent;
            }
            else
            {
                return FormContent.Evaluate(variableFinder);
            }
        }
    }
}
