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
        // A dictionary to hold each of the cells with their name as a key
        private Dictionary<String, Cell> cells;

        // A DependencyGraph that will hold dependency relationship between the cells.
        private DependencyGraph depGraph;

        /// <summary>
        /// A public constructor to create a new Spreadsheet object.  It just creates an empty 
        /// spreadsheet.  It should not have any filled cells.  Nothing special
        /// </summary>
        public Spreadsheet()
        {
            cells = new Dictionary<String, Cell>();
            depGraph = new DependencyGraph();
        }

        /// <summary>
        /// Gets the contents of the specified cell
        /// </summary>
        /// <param name="name">The cell</param>
        /// <returns>the contents in name</returns>
        public override object GetCellContents(string name)
        {
            // confirms that the name is valid
            NameValidator(name);
            //Checks if the name has been used and has a cell in it
            if (cells.ContainsKey(name))
            {
                //if it does it return the contents of the cell
                return cells[name].getContents();
            }
            else
            {
                //if it doesn't it returns an empty string
                return "";
            }
        }

        /// <summary>
        /// Returns a IEnumeralble with a list of every nonempty cell
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            // Makes a copy of the keys.  I am not sure is cells.Keys would return a copy of the keys
            // or just the keys themselves.  Better safe than sorry
            List<String> copy = new List<string>();
            foreach(String s in cells.Keys)
            {
                copy.Add(s);
            }

            return copy;
        }

        /// <summary>
        /// Sets the content of a specific cell
        /// </summary>
        /// <param name="name">The cell to be set</param>
        /// <param name="formula">The contents to be put in the cell</param>
        /// <returns></returns>
        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the content of a specific cell
        /// </summary>
        /// <param name="name">The cell to be set</param>
        /// <param name="text">The content to be put in the cell</param>
        /// <returns>A set of variable that might be effected by the change</returns>
        public override ISet<string> SetCellContents(string name, string text)
        {
            NameValidator(name);

            //check if there is something already there
            if(cells.ContainsKey(name))
            {
                //get rid of any dependees of the old cell
                depGraph.ReplaceDependees(name, new List<String>());
                //remove from the hashmap
                cells.Remove(name);
            }

            //create a new cell and add it to cells
            cells.Add(name, new Cell(text));

            return (ISet<string>)GetCellsToRecalculate(name);
        }

        /// <summary>
        /// Sets the content of a specific cell
        /// </summary>
        /// <param name="name">The cell to be set</param>
        /// <param name="number">The content to be put in the cell</param>
        /// <returns></returns>
        public override ISet<string> SetCellContents(string name, double number)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a list of the direct dependents on the given cell
        /// </summary>
        /// <param name="name">The cell</param>
        /// <returns>a list of direct dependents of name</returns>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            // check if it is null for this one weird case for this one weird method.
            if (name == null)
            {
                throw new ArgumentNullException();
            }
            //chech if the name is valid
            NameValidator(name);
            // return dependents
            return depGraph.GetDependents(name);
        }

        /// <summary>
        /// Checks to make sure the name is not null or invalid.  If it is null or invalid
        /// it throws an InvalidNameException.
        /// </summary>
        /// <param name="name"></param>
        private void NameValidator(String name)
        {
            //Throw exception if it is not valid
            if (name == null || !((Char.ToUpper(name[0]) <= 90 && Char.ToUpper(name[0]) >= 65) || name[0] == '_'))
            {
                throw new InvalidNameException();
            }
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
            }
            else
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

        /// <summary>
        /// Get the varialbes that this cell depends on.
        /// </summary>
        /// <returns>A list of variables this cell depends on</returns>
        internal IEnumerable<String> getDependees()
        {
            if (Type == "String")
            {
                return new List<String>();
            }
            else
            {
                return FormContent.GetVariables();
            }
        }
    }
}
