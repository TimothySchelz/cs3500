README File for Snake Game Client Project

Authors: Gray Marchese, u0884194
		 Timothy Schekz, u0851027

Last Updated: 11/11/16

Design Decisions:
Using a MVC design pattern.  The Model portion will consist of the projects World, Snake, and Food.  It will be in the SnakeModel Namespace.  The Control and 
View portions will be contained in the SnakeGUI Namespace.  There is also completely seperate Network coding that the Gui uses to get info from the server.  
The View is the main bulk of the program since the client basically just draws things and interacts with the server.  This client can run at 60 fps and should
be because it is on PC.  

Model:
To keep tack of the cells that each snake or food occupies there is a Point class defined in Snake that simply keeps track of the X and Y coordinates of the 
object.  We store each snake as a LinkedList and store an array of food in the world.  The world will also be represented as a 2D array with entries 
corresponding with what is in each cell.  The values are listed below.
-1 empty
0 Wall
1 Food
2 Snake

This 2D array is never actually used.  It will just be useful later on when we write the server.  Everything is designed to just be a client at the moment.  
The Model will be expanded later in the next assignment.

View/Control:
The View takes care of communication with the server, dealing with inputs from the user, and drawing everything.  The SnakeGui it the main view of the client.
It consists of some buttons and boxes for connecting to a server, a ScoreboardPanel, and a GamePanel.  The ScoreboardPanel draws the actual snake game and the 
GamePanel draws the different players scores on the side.  The SnakeGUI also communicates with the server.  It gets all the JSON objects converts them to the
appropriate information and deals with them.  It also sends inputs from the player to the server.


Everything is designed to just be a client at the moment.  The model and NetworkController will be expanded later to serve as a server in the next assignment.

Extra Features:
Benny Hill (Need to Implement)
Sound Effects? (Need to Implement)
Background? (Need to Implement)


Time Spent Working on Project:
11/11/16 2:20PM - 7:10PM
11/13/16 2:00PM - 6:30PM
11/14/16 5:00PM - 7:30PM
11/15/16 3:20PM - 5:30PM
11/17/16 7:00PM	- 10:00PM
11/18/16 7:30PM - 10:30PM
11/19/16 2:10PM	- 5:30PM
11/21/16 4:30PM - 7:45PM

Total: 26:35