using NetworkController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeModel;
using System.Collections;
using System.Timers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Server
{
    public class Server
    {
        // A dictionay to store ll of the socket states that are connected to clients
        Clients clients;
        // The world the game is taking plce in
        World world = new World(0, 150, 150);
        // Timer to go off when a new frame should be sent to the clients
        System.Timers.Timer FrameTimer;

        /// <summary>
        /// Main method to run when the server is staarted
        /// </summary>
        /// <param name="args"></param>
        public static void Main(String[] args)
        {
            Server TheServer = new Server();
        }

        /// <summary>
        /// Constructor for the server.  Starts the world and listening for clients
        /// </summary>
        private Server()
        {
            // create the hashset of clients
            clients = new Clients();

            // create a World
            world = new World(0, 150, 150);

            List<Point> verts = new List<Point>();

            Point p1 = new Point();
            Point p2 = new Point();
            p1.X = 1;
            p1.Y = 1;
            p2.X = 1;
            p2.Y = 10;
            verts.Add(p1);
            verts.Add(p2);
            Snake s1 = new Snake(verts, 1, "Snek");

            Point p3 = new Point();
            p3.X = 3;
            p3.Y = 3;
            Food f1 = new Food(1, p3);

            world.updateFood(f1);
            world.updateSnake(s1);


            Console.WriteLine("Server Started up.");

            FrameTimer = new System.Timers.Timer(33);
            FrameTimer.Elapsed += UpdateFrame;
            
            //Establishes an non-blocking loop to collect clients
            Networking.ServerAwaitingClientLoop(ClientConnected);

            // Keep console window open
            Console.Read();

        }

        /// <summary>
        /// Tells everything to update for the next frame and then send the info to the clients
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateFrame(object sender, ElapsedEventArgs e)
        {
            //Update the world
            throw new NotImplementedException();


            // Compile the date to be sent
            // Create a stringbuilder to hold data to be sent and append new items on
            StringBuilder data = new StringBuilder();
            // Append all the snakes onto the StringBuilder
            foreach (Snake snake in world.GetSnakes())
            {
                data.Append(JsonConvert.SerializeObject(snake) + "\n");
            }
            // Append all the food onto the StringBuider
            foreach (Food food in world.GetFood())
            {
                data.Append(JsonConvert.SerializeObject(food) + "\n");
            }

            String message = data.ToString();

            //Send data to Clients
            foreach(SocketState client in clients.GetAllClients())
            {
                Networking.SendData(client, message);
            }
        }

        public void ClientConnected(SocketState Client)
        {
            //Updates callback
            Client.CallMe = RecieveName;

            //Begins listening for name information from client
            Networking.GetData(Client);
        }

        /// <summary>
        /// Callback used to parse initial information from the client. Sends
        /// start-up information to client, adds this client to the list of connected
        /// clients, begins listening for input.
        /// </summary>
        /// <param name="State"></param>
        private void RecieveName(SocketState State)
        {

            //Generates start-up info for the newly connected client
            string startUpInfo = "" + State.ID + '\n' + world.Width + '\n' + world.Height + '\n';
            Networking.SendData(State, startUpInfo);

            //PARSE NAME INFO & CREATE SNAKE WITH ID = State.ID
            Console.WriteLine(State.sb.ToString());
            State.sb.Clear();

            //Adds this client to list of connected clients
            clients.Add(State);

            //Changes the callback to listen for user input
            State.CallMe = GetInput;

            //Starts an event loop listening for info from client.
            Networking.GetData(State);
        }

        /// <summary>
        /// Callback used to continually deal with input from the client while the connection
        /// is still open.
        /// </summary>
        /// <param name="State"></param>
        private void GetInput(SocketState State)
        {
            Console.WriteLine(State.ID + ": " + State.sb.ToString());

            //pull info from string builder and clear it
            String message = State.sb.ToString();
            State.sb.Clear();

            // Get the actual irection the layer wishes to move
            char direction = message[message.Length-3];

            //Changes
            world.ChangeSnakeDirection(State.ID, direction);

            Networking.GetData(State);
        }

        /// <summary>
        /// A collection of socket states that is locked when accessed or changed.
        /// </summary>
        private class Clients
        {
            //Collection of socet states
            private Dictionary<int,SocketState> clients;

            /// <summary>
            /// Zero-parameter constructor that creates an empty collection of clients.
            /// </summary>
            public Clients()
            {
                clients = new Dictionary<int, SocketState>();
            }

            /// <summary>
            /// Adds given socket state to the current collection of clients
            /// </summary>
            /// <param name="newClient"></param>
            public void Add(SocketState newClient)
            {
                lock (this)
                {
                    clients.Add(newClient.ID, newClient);
                }
            }


            /// <summary>
            /// Yields all clients in our collection, gives an iterable result.
            /// </summary>
            /// <returns></returns>
            public IEnumerable<SocketState> GetAllClients()
            {
                lock (this)
                {
                    foreach (KeyValuePair<int, SocketState> pair in clients)
                    {
                        yield return pair.Value;
                    }
                }
            }

            /// <summary>
            /// Returns the socket state associated with the given ID.
            /// </summary>
            /// <param name="ID">ID of desired socket state</param>
            /// <returns>SocketState corrisponding to th given ID</returns>
            public SocketState GetClient(int ID)
            {
                lock (this)
                {
                    return clients[ID];
                }
            }


        }

    }
}
