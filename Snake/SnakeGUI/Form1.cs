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
            this.BackColor = Color.LightGray;
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
            world.updateSnake(new Snake(snakeVerts, 3, "sss"));

            snakeVerts = new LinkedList<SnakeModel.Point>();
            p1 = new SnakeModel.Point();
            p2 = new SnakeModel.Point();
            p1.X = 1;
            p1.Y = 1;
            p2.X = 1;
            p2.Y = 10;
            snakeVerts.AddFirst(p1);
            snakeVerts.AddLast(p2);
            world.updateSnake(new Snake(snakeVerts, 2, "Boaty Mc Boatface"));

            snakeVerts = new LinkedList<SnakeModel.Point>();
            p1 = new SnakeModel.Point();
            p2 = new SnakeModel.Point();
            p1.X = 56;
            p1.Y = 2;
            p2.X = 5;
            p2.Y = 2;
            snakeVerts.AddFirst(p1);
            snakeVerts.AddLast(p2);
            world.updateSnake(new Snake(snakeVerts, 1, "zzzs"));


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
            // Get string out of StringBuilder
            String message = ss.sb.ToString();

            List < String >  messageLines= new List<String>();

            // Check if we have all the startup info
            if (messageLines.Count >= 3)
            {
                int height, width;
                bool IDParsed, WidthParsed, HeightParsed;

                // Parsed the infor out of the string
                IDParsed = Int32.TryParse(messageLines[0], out PlayerID);
                WidthParsed = Int32.TryParse(messageLines[1], out width);
                HeightParsed = Int32.TryParse(messageLines[2], out height);

                // check to make sure the info parsed correctly
                if (!(IDParsed && WidthParsed && HeightParsed))
                {
                    MessageBox.Show("Wonky data came from server");
                    return;
                }

                // create the world
                World world = new World(PlayerID, width, height);

                // Calculate how many characters were parsed so we delete them
                int CharsParsed = messageLines[0].Length + messageLines[1].Length + messageLines[2].Length + 3;

                // Remove parsed info from SB
                ss.sb.Remove(0, CharsParsed);

                // Any remaining info in the SB gets put in the string head
                prevStringHead = ss.sb.ToString();

                // clear SB and Buffer
                ss.sb.Clear();
                ss.messageBuffer = new byte[ss.BufferSize];

                // Set the callback to ReceiveWorld so we can receive world data
                ss.CallMe = ReceiveWorld;  
            }

            // Start Listening for more data
            Networking.GetData(ss);
        }

        /// <summary>
        /// Method is called when data from the server has been received
        /// </summary>
        /// <param name="ss">The socket over which data has been received</param>
        private void ReceiveWorld(SocketState ss)
        {
            
        }

    }
}
