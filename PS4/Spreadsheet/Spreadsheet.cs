// Written by Timothy Schelz, u0851027, October 2016

// Branched from PS4
//           Made the three SetCellContents methods protected
//           Added a new method SetContentsOfCell.
//           Added a new method GetCellValue.
//           Added a new property Changed.
//           Added a new method Save.
//           Added a new method GetSavedVersion.
//           Added a new class SpreadsheetReadWriteException.
//           Added IsValid, Normalize, and Version properties
//           Added a constructor for AbstractSpreadsheet

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetUtilities;

namespace SS
{
    /// <summary>
    /// A class to do all the back end of a spreadsheet such as storing values evaluating the 
    /// contents and recalculating when necessary
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        // A dictionary to hold each of the cells with their name as a key
        private Dictionary<String, Cell> cells;

        // A DependencyGraph that will hold dependency relationship between the cells.
        private DependencyGraph depGraph;

        /// <summary>
        /// A public constructor to create a new Spreadsheet object.  It just creates an empty 
        /// spreadsheet.  It will not have any filled cells.  Nothing special
        /// </summary>
        public Spreadsheet() : this(s => true, s => s, "default")
        {
        }

        /// <summary>
        /// A public 3 argument constructor to create a new Spreadsheet object.  It just 
        /// creates an empty spreadsheet.  It will not have any filled cells.  Sets the 
        /// validator, normalizer, and the version.
        /// </summary>
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
            cells = new Dictionary<String, Cell>();
            depGraph = new DependencyGraph();
        }

        /// <summary>
        /// True if this spreadsheet has been modified since it was created or saved                  
        /// (whichever happened most recently); false otherwise.
        /// </summary>
        public override bool Changed
        {
            get
            {
                return Changed;
            }

            protected set
            {
                Changed = value;
            }
        }

        /// <summary>
        /// Gets the contents of the specified cell.  
        /// 
        /// If name is null or invalid, throws an InvalidNameException.
        ///  
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
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
        /// value should be either a string, a double, or a SpreadsheetUtilities.FormulaError.
        /// </summary>
        /// <param name="name">The name of the cell</param>
        /// <returns>the value of the cell</returns>
        public override object GetCellValue(string name)
        {
            // Make sure the name is valid
            NameValidator(name);

            lookup variableLookup = ;

            //Check if it is an empty cell
            if (!cells.ContainsKey(name))
            {
                return "";
            } else if(cells[name].Type == 3)
            {
                Formula result = (Formula) cells[name].getContents();
                return result.Evaluate(new lookup(findVariable));
            } else
            {
                return cells[name];
            }
        }

        //The type of lookup delegate for the evaluate function in Formula
        private delegate double lookup(String s);

        //The Lookup function used ofr Evaluate.
        private double findVariable(String name)
        {
            if (cells.ContainsKey(name) && cells[name].Type == 3)
            {
                Formula contents = (Formula) cells[name].getContents();
                object value = contents.Evaluate(findVariable);
                if (value is FormulaError)
                {
                    throw new 
                }
                return ;
            }
        }

        /// <summary>
        /// Returns a IEnumeralble with a list of every nonempty cell
        /// </summary>
        /// <returns>An IEnumerable with the names of every non empty cell</returns>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            // Makes a copy of the keys.  I am not sure is cells.Keys would return a copy of the keys
            // or just the keys themselves.  Better safe than sorry
            List<String> copy = new List<string>();
            foreach (String s in cells.Keys)
            {
                copy.Add(s);
            }

            return copy;
        }

        public override string GetSavedVersion(string filename)
        {
            throw new NotImplementedException();
        }

        public override void Save(string filename)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the content of a specific cell
        /// 
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// </summary>
        /// <param name="name">The cell to be set</param>
        /// <param name="formula">The contents to be put in the cell</param>
        /// <returns></returns>
        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            NameValidator(name);

            //Check if formula is null
            if (formula == null)
            {
                throw new ArgumentNullException();
            }

            // Stores the old cell's contents so that we can put it back in if necessary
            object old;
            if (cells.ContainsKey(name))
            {
                old = cells[name].getContents();
            }
            else
            {
                old = "";
            }

            //remove old thing from the hashmap
            cells.Remove(name);

            //get rid of any dependees of the old cell and replace them with the new ones
            depGraph.ReplaceDependees(name, formula.GetVariables());

            //create a new cell and add it to cells
            cells.Add(name, new Cell(name, formula));

            // Check for a circular dependency
            IEnumerable<string> cellsToChange = new List<String>();
            try
            {
                cellsToChange = GetCellsToRecalculate(name);
            }
            catch (CircularException)
            {
                //We got a circular dependency 
                // Put the old stuff back in
                if (old is Formula)
                {
                    SetCellContents(name, (Formula)old);
                }
                else if (old is String)
                {
                    SetCellContents(name, (String)old);
                }
                else
                {
                    SetCellContents(name, (Double)old);
                }

                //rethrow the exception
                throw new CircularException();
            }

            // Return the cells to be recalculated as a set
            return new HashSet<String>(cellsToChange);
        }

        /// <summary>
        /// Sets the content of a specific cell
        /// 
        /// If text is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// </summary>
        /// <param name="name">The cell to be set</param>
        /// <param name="text">The content to be put in the cell</param>
        /// <returns>A set of variable that might be effected by the change</returns>
        public override ISet<string> SetCellContents(string name, string text)
        {
            NameValidator(name);

            //Check if text is null
            if (text == null)
            {
                throw new ArgumentNullException();
            }

            //get rid of any dependees of the old cell
            depGraph.ReplaceDependees(name, new List<String>());

            //remove old cell from the hashmap
            cells.Remove(name);

            // If the text is empty then we don't add it.  There is nothing in it.
            if (!text.Equals(""))
            {
                //create a new cell and add it to cells
                cells.Add(name, new Cell(name, text));
            }

            return new HashSet<String>(GetCellsToRecalculate(name));
        }

        /// <summary>
        /// Sets the content of a specific cell
        /// 
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// </summary>
        /// <param name="name">The cell to be set</param>
        /// <param name="number">The content to be put in the cell</param>
        /// <returns> A set of cells to be recalculated</returns>
        public override ISet<string> SetCellContents(string name, double number)
        {
            NameValidator(name);

            //get rid of any dependees of the old cell
            depGraph.ReplaceDependees(name, new List<String>());

            //remove old cell from the hashmap
            cells.Remove(name);

            //create a new cell and add it to cells
            cells.Add(name, new Cell(name, number));

            return new HashSet<String>(GetCellsToRecalculate(name));
        }

        public override ISet<string> SetContentsOfCell(string name, string content)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a list of the direct dependents on the given cell
        /// 
        /// If name is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name isn't a valid cell name, throws an InvalidNameException.
        /// 
        /// </summary>
        /// <param name="name">The cell</param>
        /// <returns>a list of direct dependents of name</returns>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            // check if it is null for this one weird case for this one weird method.
            if (name == null)
            {
                //This is actually unreachable so the code coverage will flag this.
                //I am going to keep it encase something else needs to call it later on.
                throw new ArgumentNullException();
            }
            //chech if the name is valid
            NameValidator(name);
            // return dependents.
            return depGraph.GetDependents(name);
        }

        /// <summary>
        /// Checks to make sure the name is not null or invalid.  If it is null or invalid
        /// it throws an InvalidNameException.
        /// </summary>
        /// <param name="name">the name to be validated</param>
        private void NameValidator(String name)
        {
            //Throw exception if it is not valid
            if (name == null || !((Char.ToUpper(name[0]) <= 90 && Char.ToUpper(name[0]) >= 65) || name[0] == '_') || !IsValid(name))
            {
                throw new InvalidNameException();
            }
        }
    }

    /// <summary>
    /// A class to act as one cell in a spreadsheet.  It can hold either a String, a double or a Formula.
    /// Each cell is immutable.
    /// </summary>
    internal class Cell
    {
        // This cell holds either a String, a double, or a Formula
        private String StringContent;
        private Double DoubleContent;
        private Formula FormContent;

        /// <summary>
        /// A delegate of type lookup to be passed to a cell when we want to look it up
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal delegate double LookUp(String name);

        /// <summary>
        /// The type of cell it is.  getContents returns this type.
        /// 
        /// 1 for String
        /// 2 for Double
        /// 3 for Formula
        /// </summary>
        internal int Type
        {
            get;

            private set;
        }

        internal String Name
        {
            get;

            private set;
        }

        /// <summary>
        /// Constructor to build a Cell from a String
        /// </summary>
        /// <param name="s">The string to go in the cell</param>
        internal Cell(String name, String s)
        {
            //sets the type to string and then the contents to the given value
            Type = 1;
            StringContent = s;
            Name = name;
        }

        /// <summary>
        /// Constructor to build a Cell from a Formula
        /// </summary>
        /// <param name="f"></param>
        /// <param name="l">The delegate used to look up varaibles.  Takes a string and
        /// returns a double</param>
        internal Cell(String name, Formula f)
        {
            //sets the type to formula and then the contents to the given value
            Type = 3;
            FormContent = f;
            Name = name;
        }

        /// <summary>
        /// Constructor to build a Cell from a Double
        /// </summary>
        /// <param name="d">The double to be put into a cell</param>
        internal Cell(String name, double d)
        {
            //sets the type to double and then the contents to the given value
            Type = 2;
            DoubleContent = d;
            Name = name;
        }

        /// <summary>
        /// Get the content of the Cell
        /// </summary>
        /// <returns>The Content of the cell</returns>
        internal object getContents()
        {
            if (Type == 1)
            {
                return StringContent;
            }
            else if (Type == 2)
            {
                return DoubleContent;
            }
            else
            {
                //Formulas are immutable so we can just return it and not worry about 
                //data protection.
                return FormContent;
            }
        }
    }
}
