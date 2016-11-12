Read Me for PS6
Last Updated 11/03/16
Timothy Schelz, u0851027
Gray Marchese, u0884194

This is the working version of our PS6 Spreadsheet.  There was a problem with all the coded UI tests and since we were told in class that they wont be graded we commented them out.  We ran into a bunch of really weird bugs near the end and had to revert to an older version.  This is the most functional one we have.  Extra Features are listed below.

Extra Feaures:
1. Keyboard Navigation: We added the ability to navigate the spreadsheet using keyboard commands.  Tab and Shift+Tab move the selection left and right, Enter and Shift+Enter move it up and down.  Also Arrowkeys move the selection. Updated cells and relocated cursor for convenience whenever possible.
2. Graphing Points:  We dded the ability to graph 2d points.  Just enter some X and Y values and hit the Chart menu.  It will ask you what columns you want to use for your Xs and Ys.  There is a problem that it has some default points it graphs but other than that it works.


The Spreadsheet class is implemented using a HashMap of each cell and a DependencyGraph with relationships between the cells in
the hashmap.  There is an internal Cell class that will be the contents of the Hashmap. Each cell contains one of the valid entries.
When the cell only contain "" it will not even be stored.

I am using the versions of PS2 that I turned in but I had to update them to the .NET Framework 4.5.2
I changed it over and committed it on 9/27/2016.  For PS3 I am using an updated and debugged version from 10/03/16.

The naming scheme of the tests is Public_NameofMethodBeingTested_SpecificCase().  So if I wanted to test GetCellContents on a 
cell that is empty it would be called Public_GetCellContents_EmptyCell().  Numbers can also follow the specific case if I have
a few methods testing the same thing in slightly different ways.

10/03/16
Created a branch of the solution in order to do PS5.  This is using a different AbstractSpreadsheet class and has more functionality.

11/1/2016
The spreadsheet now has a gui.  The gui can be used by selecting a cell with the mouse and editing the contents with the textbox.  Click 
update or hit ENTER to plug the new contents into the spreadsheet.  At the top there is a file menu.  Inside this menue there is New, 
Open, Save, and Close. New opens a new spreadsheet in a new window.  Open opens an openDialogBox for the user to select a saved file to open.
Once a file is selected the saved file will be loaded into the current spreadsheet.  Save will open a dialog so that the user can save their
spreadsheet.  Close will close the window.  If changes have been made to the spreadsheet a save warning will pop up.  There is also a Help menu.
When the help menu is selected a pop up will appear describing basic use of the spreadsheet.

11/3/2016
The spreadsheet can be navigated with the tab and enter keys.  TAB moves the selection to the right, SHIFT+TAB moves it to the left.  ENTER 
moves it down, and SHIFT+ENTER moves it up.  Whenever one of these are typed it also enters whatever is in the contentsbox into the cell.