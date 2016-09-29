Read Me for PS4
Last Updated 9/25/16
Timothy Schelz, u0851027

The Spreadsheet class is implemented using a HashMap of each cell and a DependencyGraph with the name of each cell in the
HashMap.  This should give us O(1) access to each cell as well as provide efficient functionality of a graph.  

I am using the versions of PS2 and PS3 that I turned in butI had to update them to the .NET Framework 4.5.2
I changed them over and committed them on 9/27/2016.

The naming scheme of the tests is Public_NameofMethodBeingTested_SpecificCase().  So if I wanted to test GetCellContents on a 
cell that is empty it would be called Public_GetCellContents_EmptyCell().  Numbers can also follow the specific case if I have
a few methods testing the same thing in slightly different ways for some reason.