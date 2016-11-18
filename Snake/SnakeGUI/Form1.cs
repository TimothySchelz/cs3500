using SnakeModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetworkController;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace SnakeGUI
{
    public partial class Form1 : Form
    {
        // Stores the game world
        private World world;
        //The ID of the Player
        private int PlayerID;
        //The scoket state with the cnnection to the server.  It is initialized when the 'Go' button is clicked
        private SocketState theServer;
        //Possibly incomplete heads of incomming data
        private string prevStringHead;

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Method to be called everytime we want to display another frame.
        /// Hopefully at least 60 per second
        /// </summary>
        private void UdpateFrame()
        {
            gamePanel1.Invalidate();
            scoreBoardPanel1.Invalidate();
        }

        /// <summary>
        /// Method to be called everytime we want to display another frame.  This is a wrapper for UpdateFrame so that
        /// we can use it as an event handler.
        /// Hopefully at least 60 per second
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UdpateFrame(object sender, EventArgs e)
        {
            UdpateFrame();
        }

        /// <summary>
        /// Method to be called when the program originally starts up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnStartUp(object sender, EventArgs e)
        {
            // Creates a filler world to be displayed
            world = new World(1, 150, 150);

            
            // Creates snakes and food to be displayed
            LinkedList<SnakeModel.Point> snakeVerts = new LinkedList<SnakeModel.Point>();
            SnakeModel.Point p1, p2;
            
            p1 = new SnakeModel.Point();
            p2 = new SnakeModel.Point();
            p1.X = 30;
            p1.Y = 25;
            p2.X = 100;
            p2.Y = 25;
            snakeVerts.AddFirst(p1);
            snakeVerts.AddLast(p2);
            world.updateSnake(new Snake(snakeVerts, 1, "sss"));

            snakeVerts = new LinkedList<SnakeModel.Point>();
            p1 = new SnakeModel.Point();
            p2 = new SnakeModel.Point();
            p1.X = 23;
            p1.Y = 64;
            p2.X = 23;
            p2.Y = 101;
            snakeVerts.AddFirst(p1);
            snakeVerts.AddLast(p2);
            world.updateSnake(new Snake(snakeVerts, 6514, "Boaty Mc Boatface"));

            snakeVerts = new LinkedList<SnakeModel.Point>();
            p1 = new SnakeModel.Point();
            p2 = new SnakeModel.Point();
            p1.X = 56;
            p1.Y = 2;
            p2.X = 5;
            p2.Y = 2;
            snakeVerts.AddFirst(p1);
            snakeVerts.AddLast(p2);
            world.updateSnake(new Snake(snakeVerts, 21545, "zzzs"));

            snakeVerts = new LinkedList<SnakeModel.Point>();
            p1 = new SnakeModel.Point();
            p2 = new SnakeModel.Point();
            p1.X = 30;
            p1.Y = 25;
            p2.X = 100;
            p2.Y = 25;
            snakeVerts.AddFirst(p1);
            snakeVerts.AddLast(p2);
            world.updateSnake(new Snake(snakeVerts, 1, "sss"));


            p1 = new SnakeModel.Point();
            p2 = new SnakeModel.Point();
            p1.X = 3;
            p1.Y = 4;
            p2.X = 4;
            p2.Y = 3;

            // Adds the snakes and food to the world
            
            world.updateFood(new Food(1, p1));
            world.updateFood(new Food(2, p2));
            
            // Sets the world to be the world for both panels
            gamePanel1.SetWorld(world);
            scoreBoardPanel1.SetWorld(world);

            UdpateFrame();
        }

        /// <summary>
        /// An Event Handler for when the player hits the 'Go' button trying to connect to a server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connect(object sender, EventArgs e)
        {
            // Check to make sure we have valid name
            if (NameBox.Text == "")
            {
                // Display Error
                MessageBox.Show("Please enter a nonempty name");
            }

            // Check to make sure we have valid Address
            if (ServerBox.Text == "")
            {
                // Display Error
                MessageBox.Show("Please enter a nonempty address");
            }

            // Sets previous incomplete message received to the empty string
            prevStringHead = "";

            // Gets the address the player wishes to connect to
            string Address = ServerBox.Text;

            try {
                //Establishes a socket with the server, instructing it to get initial data.
                theServer = Networking.ConnectToServer(FirstContact, Address);
            }
            catch (Exception error)
            {
                // Display Error
                MessageBox.Show("We could not connect to the server.  Please check yo self before you, inadvertantly, wreck yo self");
            }
        }

        /// <summary>
        /// Method to be called when the server makes first contact with this client
        /// It isn't as good as the movie...
        /// </summary>
        /// <param name="ss">The socket stat connected to the server sending the firstcontact</param>
        private void FirstContact(SocketState ss)
        {
            ss.CallMe = RecieveStartup;
            string name = NameBox.Text;
            Networking.SendData(ss, name+'\n');
        }

        /// <summary>
        /// his method is called when the start up information is sent from the server
        /// </summary>
        /// <param name="ss">The socket over which startup data is received</param>
        private void RecieveStartup(SocketState ss)
        {
            // Pulled a string out of the Stringbuilder
            string message = ss.sb.ToString();

            //Split it up into different strings delineated by \n
            string[] argVector = message.Split('\n');

            if (argVector.Length >= 3) {
                //Tried parsing the first one into the player's ID
                Int32.TryParse(argVector[0], out PlayerID);

                // Tried parseing the next two into the width and height of the world
                int Height, Width;
                Int32.TryParse(argVector[1], out Width);
                Int32.TryParse(argVector[2], out Height); // TODO: add a check to make sure they parsed correctly

                //Counts Parsed Characters and removes them from stringbuilder
                int parsedChars = argVector[0].Length + argVector[1].Length + argVector[2].Length + 3;
                ss.sb.Remove(0, parsedChars);

                // Creating the world
                world = new World(PlayerID, Width, Height);

                // Change the SS's Callback to start receiving the world info
                ss.CallMe = ReceiveWorld;

                // Sets the new world to be the world for both panels
                gamePanel1.SetWorld(world);
                scoreBoardPanel1.SetWorld(world);
            }

            //Clears the buffer, as we've processed all data in it.
            ss.messageBuffer = new byte[ss.messageBuffer.Length];

            // Set the rest of the junk in the sb to the previousStringHead so that we can take care of it once we start
            // receiving world data.
            prevStringHead = ss.sb.ToString();

            // Clear out the string builder
            ss.sb.Clear();

            // Tell the Network controller to continue listening for data
            Networking.GetData(ss);
        }

        /// <summary>
        /// Method is called when data from the server has been received
        /// </summary>
        /// <param name="ss">The socket over which data has been received</param>
        private void ReceiveWorld(SocketState ss)
        {

            // Places previous incomplete message at the beginning of the stringbuilder
            ss.sb.Insert(0, prevStringHead);

            // pull Json Objects out of the SB
            String message = ss.sb.ToString();

            // Doesn't try to parse data with nothing there, resets event loop, gets moar data.
            if (ss.sb.Length == 0)
            {
                Networking.GetData(ss);
                return;
            }

            // Tests if final character is '\n' and all objects are complete
            bool isComplete = (message[message.Length - 1] == '\n');

            //Split it up into different strings delineated by \n
            string[] argVector = message.Split('\n');

            // Turn Json Objects into normal objects
            for(int index = 0; index < argVector.Length-1; index++) { 

                JObject obj = JObject.Parse(argVector[index]);
                JToken snakeProp = obj["vertices"];
                JToken foodProp = obj["loc"];

                // Update world with these Objects

                // Object is a snake
                if (snakeProp != null)
                {
                        Snake newSnake = JsonConvert.DeserializeObject<Snake>(argVector[index]);
                        world.updateSnake(newSnake);
                }
                // Object is a food
                if (foodProp != null)
                {
                        Food newFood = JsonConvert.DeserializeObject<Food>(argVector[index]);
                        world.updateFood(newFood);
                }
            }

            //Adds the newline character if all JSON ojbects were complete
            if (isComplete)
                prevStringHead = argVector[argVector.Length - 1]+'\n';
            else
                prevStringHead = argVector[argVector.Length - 1];

            //Clears the stringbuilder
            ss.sb.Clear();

            //Clears the buffer, as we've processed all data in it.
            ss.messageBuffer = new byte[ss.messageBuffer.Length];

            //Resets event loop
            Networking.GetData(ss);

            // Redraw Everything
            UdpateFrame();
        }

    }
}
