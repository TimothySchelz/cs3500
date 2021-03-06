﻿// Written by Timothy Schelz, u0851027, October 2016

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
using System.Xml;

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
        /// 4 argument constructor.  Throws a SpreadsheetReadWriteException if there is any 
        /// problem opening or reading from the file.
        /// </summary>
        /// <param name="filename">A saved Spreadsheet to load in</param>
        /// <param name="isValid">A Variable validator</param>
        /// <param name="normalize">The normalizer for variables</param>
        /// <param name="version">The version of spreadsheet to be used</param>
        public Spreadsheet(string filename, Func<string, bool> isValid, Func<string, string> normalize, string version) : this(isValid, normalize, version)
        {
            //Chech versions
            if (GetSavedVersion(filename) != Version)
            {
                throw new SpreadsheetReadWriteException("The version of the file and this spreadsheet do not match");
            }

            //All the work.  Make sure to catch any exception
            try
            {
                // create the reader from the file and make sure it is disposed of
                using (XmlReader reader = XmlReader.Create(filename))
                {
                    String cellName = "";
                    String Content = "";
                    //cycle through the file
                    while (reader.Read())
                    {
                        //Check what we have
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case "spreadsheet":
                                    //The start of the spreadsheet
                                    break;
                                case "cell": 
                                    //Start of a cell.  We don't do anything here
                                    break;
                                case "name": 
                                    //Go to the content of the name
                                    reader.Read();
                                    //Store the name
                                    cellName = reader.Value.Trim();
                                    break;

                                case "contents":
                                    //Go to the content of the content
                                    reader.Read();
                                    //Store the content
                                    Content = reader.Value.Trim();
                                    //create the cell
                                    SetContentsOfCell(cellName, Content);
                                    break;
                                default:
                                    // makes sure to throw an exception if we find any incorrect opening tags
                                    throw new SpreadsheetReadWriteException("There were unexpected starting elements in the file.");
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //Catches whatever happened in the file and then throws a new exception with the same error
                throw new SpreadsheetReadWriteException(e.Message);
            }


            // Make sure Changed is set to false since the spreadsheet hasn't changed yet
            Changed = false;
        }

        /// <summary>
        /// True if this spreadsheet has been modified since it was created or saved                  
        /// (whichever happened most recently); false otherwise.
        /// </summary>
        public override bool Changed
        {
            get;
            protected set;
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
            //Normalize and validate
            NameValidator(name);
            name = Normalize(name);

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
            //Normalize and validate
            NameValidator(name);
            name = Normalize(name);

            //Check if it is a cell
            if (cells.ContainsKey(name))
            {
                return cells[name].getValue();
            }
            else
            {
                return "";
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

        /// <summary>
        /// Returns the version information of the spreadsheet saved in the named file.
        /// If there are any problems opening, reading, or closing the file, the method
        /// should throw a SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        /// <param name="filename">the filelocation of the spreadsheet</param>
        /// <returns>The version of the saved file</returns>
        public override string GetSavedVersion(string filename)
        {
            //The version that wil be returned
            String fileVersion = null;
            try
            {
                using (XmlReader reader = XmlReader.Create(filename))
                {
                    //Goes through and finds the spreadsheet tag
                    while (reader.Read())
                    {
                        if (reader.Name == "spreadsheet")
                        {
                            //When it is found set the value of the fileVersion and break out.  We are done
                            fileVersion = reader.GetAttribute("version");
                            break;
                        }
                    }

                    //Make sure the fileVersion is not null
                    if(fileVersion == null)
                    {
                        //If it is the file whas not properly formated and an exception needs to be thrown
                        throw new SpreadsheetReadWriteException("Did not find a version for this spreadsheet");
                    }
                }
            } catch(Exception e)
            {
                //Catches whatever happened in the file and then throws a new exception with the same error
                throw new SpreadsheetReadWriteException(e.Message);
            }

            //If nothing went wrong then just return the fileVersion
            return fileVersion;
        }

        /// <summary>
        /// Writes the contents of this spreadsheet to the named file using an XML format.
        /// 
        /// If there are any problems opening, writing, or closing the file, the method should throw a
        /// SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        /// <param name="filename">The name of the file being saved to</param>
        public override void Save(string filename)
        {
            //Try so that we can catch and throw the proper exception
            try
            {
                using (XmlWriter writer = XmlWriter.Create(filename))
                {
                    //The thing that we need to start writing to an .XML
                    writer.WriteStartDocument();

                    //Header of the Document
                    writer.WriteStartElement("spreadsheet");
                    writer.WriteAttributeString("version", Version);

                    //Write the cells
                    foreach (KeyValuePair<String, Cell> pair in cells)
                    {
                        writer.WriteStartElement("cell");
                        writer.WriteElementString("name", pair.Key);
                        writer.WriteElementString("contents", pair.Value.ToString());
                        writer.WriteEndElement();
                    }

                    //Ending of the document
                    writer.WriteEndElement();
                }
            } catch (Exception e)
            {
                throw new SpreadsheetReadWriteException(e.Message);
            }

            //Reset The Changed property
            Changed = false;
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
        protected override ISet<string> SetCellContents(string name, Formula formula)
        {
            //Normalize and validate
            NameValidator(name);
            name = Normalize(name);

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

                //remove old thing from the hashmap
                cells.Remove(name);
            }
            else
            {
                old = "";
            }

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

            //Change Changed to true
            Changed = true;

            foreach(String current in cellsToChange)
            {
                cells[current].recalculate(name, lookup);
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
        protected override ISet<string> SetCellContents(string name, string text)
        {
            //Normalize and validate
            NameValidator(name);
            name = Normalize(name);

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
            if (text.Equals(""))
            {
                return new HashSet<String>() { name };
            }

            //create a new cell and add it to cells
            cells.Add(name, new Cell(name, text));

            //Change Changed to True
            Changed = true;

            //Go through and recalculate everything!
            IEnumerable<String> cellsToChange = GetCellsToRecalculate(name);

            foreach (String current in cellsToChange)
            {
                cells[current].recalculate(name, lookup);
            }

            //return a set
            return new HashSet<String>(cellsToChange);
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
        protected override ISet<string> SetCellContents(string name, double number)
        {
            //normalize and then validate
            NameValidator(name);
            name = Normalize(name);

            //get rid of any dependees of the old cell
            depGraph.ReplaceDependees(name, new List<String>());

            //remove old cell from the hashmap
            cells.Remove(name);

            //create a new cell and add it to cells
            cells.Add(name, new Cell(name, number));

            //Change Changed to True
            Changed = true;

            //Go through and recalculate everything!
            IEnumerable<String> cellsToChange = GetCellsToRecalculate(name);

            foreach (String current in cellsToChange)
            {
                cells[current].recalculate(name, lookup);
            }

            //return a set
            return new HashSet<String>(cellsToChange);
        }

        /// <summary>
        /// If content is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if content parses as a double, the contents of the named
        /// cell becomes that double.
        /// 
        /// Otherwise, if content begins with the character '=', an attempt is made
        /// to parse the remainder of content into a Formula f using the Formula
        /// constructor.  There are then three possibilities:
        /// 
        ///   (1) If the remainder of content cannot be parsed into a Formula, a 
        ///       SpreadsheetUtilities.FormulaFormatException is thrown.
        ///       
        ///   (2) Otherwise, if changing the contents of the named cell to be f
        ///       would cause a circular dependency, a CircularException is thrown.
        ///       
        ///   (3) Otherwise, the contents of the named cell becomes f.
        /// 
        /// Otherwise, the contents of the named cell becomes content.
        /// 
        /// If an exception is not thrown, the method returns a set consisting of
        /// name plus the names of all other cells whose value depends, directly
        /// or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        /// <param name="name">The name of the Cell to be set</param>
        /// <param name="content">the content the cell will be set too</param>
        /// <returns>A set of cells that were recalculated</returns>
        public override ISet<string> SetContentsOfCell(string name, string content)
        {
            //Checks if content is null
            if (content == null)
            {
                throw new ArgumentNullException();
            }
            //For they TryParse
            double doubleContent;
            //Checks the different cases
            //Starts with looking for the '=' then trys parsing, then it puts it into a string
            if (content.Length > 0 && content[0].Equals('='))
            {
                //set the contents to a formula
                return SetCellContents(name, new Formula(content.Substring(1), Normalize, IsValid));
            } else if (Double.TryParse(content, out doubleContent))
            {
                //set the contents to a double
                SetCellContents(name, doubleContent);
                return SetCellContents(name, doubleContent);
            } else
            {
                //set the contents to a string
                return SetCellContents(name, content);
            }
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
            //Normalize and validate
            // check if it is null for this one weird case for this one weird method.
            if (name == null)
            {
                //This is actually unreachable so the code coverage will flag this.
                //I am going to keep it encase something else needs to call it later on.
                throw new ArgumentNullException();
            }
            //chech if the name is valid
            NameValidator(name);
            name = Normalize(name);

            // return dependents.
            return depGraph.GetDependents(name);
        }

        /// <summary>
        /// Just checks if the string is of the format of a variable.  If it is the wrong format it will throw an InvalidNameException.
        /// </summary>
        /// <param name="s">The string to be checked</param>
        /// <returns>returns true if it is a valid variable.  Returns false if it is an empty string or white space</returns>
        private void NameValidator(string name)
        {
            //First we check the condition the general conditions for variables

            // check for a null name
            if (name == null)
            {
                throw new InvalidNameException();
            }
            // Simplifies what needs to be checked.
            String s = name.ToUpper();
            char[] variable = s.ToCharArray();

            // Keeps track of when we switch from letters to numbers.
            bool switchedToNumbers = false;

            // Checks if the first char is a letter and that the last char is an integer.
            if (!(variable[0] >= 'A' && variable[0] <= 'Z') || !(variable[variable.Length - 1] >= '0' && variable[variable.Length - 1] <= '9'))
            {
                throw new InvalidNameException();
            }

            // Go through one char at a time seeing if it is letters then numbers
            for (int i = 0; i < variable.Length; i++)
            {
                // while still just a series of letters
                if (!switchedToNumbers)
                {
                    // If we are still on a letter move to the next char
                    if ((variable[i] >= 'A' && variable[i] <= 'Z'))
                    {
                        continue;
                    }
                    // If we have a number switch to the number part of the variable's name
                    else if ((variable[i] >= '0' && variable[i] <= '9'))
                    {
                        switchedToNumbers = true;
                        continue;
                    }
                    // If it is anything else then it does not fit the scheme
                    else
                    {
                        throw new InvalidNameException();
                    }
                    // If we are on the number section of the variable
                }
                else
                {
                    // If we don't have a number then we have a problem
                    if (!(variable[i] >= '0' && variable[i] <= '9'))
                    {
                        throw new InvalidNameException();
                    }
                }
            }

            //Now that we know it fits the general format of a variable we can now check 
            //to make sure The normalized version passes the variable validator
            if (!IsValid(Normalize(name)))
            {
                throw new InvalidNameException();
            }
        }

        /// <summary>
        /// A method to be used as the lookup delegate for Formula's Evaluate method.  It will throw
        /// some exceptions but they Evaluate method should catch and deal with them.
        /// </summary>
        /// <param name="name">The name of the variable being looked up</param>
        /// <returns>the douvle in the variable.  It might not actually be a double.</returns>
        internal double lookup(String name)
        {
            if (cells.ContainsKey(name) && cells[name].getValue() is double)
            {
                return (double) cells[name].getValue();
            }
            throw new FormulaFormatException("A variable that is being referenced does not contain a valid entry.  i.e. a number");
        }
    }

    /// <summary>
    /// A class to act as one cell in a spreadsheet.  It can hold either a String, a double or a Formula.
    /// Each cell is sort of immutable.  The contents of a cell never change but the value will.
    /// </summary>
    internal class Cell
    {
        // This cell holds either a String, a double, or a Formula
        private String StringContent;
        private Double DoubleContent;
        private Formula FormContent;

        //Can be a String Double or a FormulaError
        private Object Value;



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

        /// <summary>
        /// Constructor to build a Cell from a String
        /// </summary>
        /// <param name="s">The string to go in the cell</param>
        internal Cell(String name, String s)
        {
            //sets the type to string and then the contents to the given value
            Type = 1;
            StringContent = s;
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
        }

        /// <summary>
        /// Returns the value of the contents of the cell.  It can return a string double or FormulError.
        /// </summary>
        /// <returns>The value of the cell. Either a string, double or FormulaError</returns>
        internal object getValue()
        {
            return Value;
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

        internal void recalculate(String name, Func<String,double> lookup)
        {
            //Only need to Evaluate if it is a formula
            if (Type == 3)
            {
                //If the delegate throws an exception due to the casting as a double it should
                // be caught in the evaluate function and turn it into an FormulaError.
                Value = FormContent.Evaluate(lookup);
            } else if (Type == 1)
            {
                Value = StringContent;
            } else if (Type == 2)
            {
                Value = DoubleContent;
            }
        }

        /// <summary>
        /// A ToString method.  returns the content of the cell.  If it is a formula it also puts a = in front.
        /// </summary>
        /// <returns>The string representing the cell</returns>
        public override String ToString()
        {
            if (Type == 1)
            {
                return StringContent;
            }
            else if (Type == 2)
            {
                return DoubleContent.ToString();
            }
            else
            {
                // We have to append the '=' on the beginning
                return "=" + FormContent.ToString();
            }
        }
    }
}
