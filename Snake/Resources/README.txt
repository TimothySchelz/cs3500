README File for Snake Game Client Project

Authors: Gray Marchese, u0884194
		 Timothy Schekz, u0851027

Last Updated: 11/11/16

Design Decisions:
Using a MVC design pattern.  The Model portion will consist of the projects World, Snake, and Food.  It will be in the SnakeModel Namespace.  The Control and View portions will be contained in the SnakeView Namespace.

The world will be represented as a 2D array with entries corresponding with what is in each cell.  The values are listed below.
-1 empty
0 Wall
1 Food
2 Snake

To keep tack of the cells that each snake or food occupies there is a Point Struct defined in Snake that simply keeps track of the X and Y coordinates of the object.  We also store each snake as a LinkedList and store an array of food.  

Everything is designed to just be a client at the moment.  The model and NetworkController will be expanded later to serve as a server as well later on, possibly in the next assignment.

Extra Features:
Space Filling Snake AI

Unimplemented/Broken features:
Fix Gui Display
Implement Zooming
Networking (send data)
Extra Features
Gamepanel constantly loads even when connection is not made.
Receiving unexpected IndexOutOfBounds Exc in ReceiveWorld after already clearing stringbuilder.
Also ending up with messages that begin in the middle of JSON strings.

Time Spent Working on Project:
11/11/16 2:20PM - 7:10PM
11/13/16 2:00PM - 6:30PM
11/14/16 5:00PM - 7:30PM