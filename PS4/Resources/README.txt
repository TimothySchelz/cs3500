Read Me for PS4
Last Updated 9/25/16
Timothy Schelz, u0851027

The Spreadsheet class is implemented using a HashMap of each cell and a DependencyGraph with the name of each cell in the
HashMap.  This should give us O(1) access to each cell as well as provide efficient functionality of a graph.  

There will be a helper class called Cell that has a name, contents, and a value.  The name will be the name of the variable.
The contents will be the String or the Formula that the cell contains.  The value will be either the String in the contents
or it will be whatever the Formula evaluates to.  Each cell will contain either a string or a Formula object.  For constants 
such as "5" the formula will just be a formula with that value in it. This way we don't have to distinguish between Formulas 
and doubles when doing things with the cells.

The naming scheme of the tests is Public_NameofMethodBeingTested_SpecificCase().  So if I wanted to test GetCellContents on a 
cell that is empty it would be called Public_GetCellContents_EmptyCell().  Numbers can also follow the specific case if I have
a few methods testing the same thing in slightly different ways for some reason.