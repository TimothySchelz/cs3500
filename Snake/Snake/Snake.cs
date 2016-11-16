using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SnakeModel
{
    /// <summary>
    /// A struct to hold the X and Y values of whatever
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Point
    {
        [JsonProperty]
        public int X;

        [JsonProperty]
        public int Y;
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class Snake
    {

        //Head, turning points, and tail of the snake.
        [JsonProperty]
        private LinkedList<Point> Verticies;

        //Length of snake
        public int Length
        {
            get;
            private set;
        }

        /// <summary>
        /// ID of the snake.
        /// </summary>
        [JsonProperty]
        public int ID
        {
            get;
            private set;
        }

        /// <summary>
        /// The name of the snake choosen by the player
        /// </summary>
        [JsonProperty]
        public string name
        {
            get;
            private set;
        }

        /// <summary>
        /// Constructor for the snake that takes a list of verticies, the id of the snake, and the name
        /// </summary>
        /// <param name="Verticies"></param>
        /// <param name="ID"></param>
        /// <param name="Name"></param>
        public Snake (LinkedList<Point> Verticies, int ID, string Name)
        {
            this.Verticies = Verticies;
            this.Length = this.GetSnakePoints().Count-1;
            this.ID = ID;
            this.name = Name;
        }

        /// <summary>
        /// Returns a list of points specifying the head, turning points, and tail of the snake
        /// </summary>
        /// <returns></returns>
        public LinkedList<Point> GetVerticies()
        {
            LinkedList<Point> result = new LinkedList<Point>();

            foreach (Point vertex in Verticies)
            {
                result.AddLast(vertex);
            }
            return result;
        }

        /// <summary>
        /// Returns a HashSet of all the points in this snake
        /// </summary>
        /// <returns>All the points in this snake</returns>
        public HashSet<Point> GetSnakePoints()
        {
            HashSet<Point> result = new HashSet<Point>();
            Point PreviousPoint = new Point();
            int initial;
            int end;
            
            // Go through each vertice in this snake
            foreach(Point joint in Verticies)
            {
                // Check if this is the first Vertice
                if (joint.Equals(Verticies.First.Value))
                {
                    // just set it as the previous point and we are done on this go around.
                    PreviousPoint = joint;
                } else
                {
                    //Initilize the start and end values to 0
                    initial = 0;
                    end = 0;

                    // Check if the current vertice and the last one have the same X value
                    if (PreviousPoint.X == joint.X)
                    {
                        // Figure out which one has the smaller Y value and let that be our starting point
                        if (PreviousPoint.Y < joint.Y)
                        {
                            initial = PreviousPoint.Y;
                            end = joint.Y;
                        } else
                        {
                            initial = joint.Y;
                            end = PreviousPoint.Y;
                        }

                        // loop through and add all the points between the vertices
                        for(int i = initial; i <= end; i++)
                        {
                            Point current = new Point();
                            current.X = joint.X;
                            current.Y = i;
                            result.Add(current);
                        }
                    // Check if the current vertice and the last one have the same Y value
                    }
                    else if (PreviousPoint.Y == joint.Y)
                    {
                        // Figure out which one has the smaller Y value and let that be our starting point
                        if (PreviousPoint.X < joint.X)
                        {
                            initial = PreviousPoint.X;
                            end = joint.X;
                        }
                        else
                        {
                            initial = joint.X;
                            end = PreviousPoint.X;
                        }

                        // loop through and add all the points between the vertices
                        for (int i = initial; i <= end; i++)
                        {
                            Point current = new Point();
                            current.Y = joint.Y;
                            current.X = i;
                            result.Add(current);
                        }
                    }
                }
            }
            return result;
        }

    }
}
