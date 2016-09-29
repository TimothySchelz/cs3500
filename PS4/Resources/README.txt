Read Me for PS4
Last Updated 9/29/16
Timothy Schelz, u0851027

The Spreadsheet class is implemented using a HashMap of each cell and a DependencyGraph with relationships between the cells in
the hashmap.  

I am using the versions of PS2 and PS3 that I turned in but I had to update them to the .NET Framework 4.5.2
I changed them over and committed them on 9/27/2016.

The naming scheme of the tests is Public_NameofMethodBeingTested_SpecificCase().  So if I wanted to test GetCellContents on a 
cell that is empty it would be called Public_GetCellContents_EmptyCell().  Numbers can also follow the specific case if I have
a few methods testing the same thing in slightly different ways.