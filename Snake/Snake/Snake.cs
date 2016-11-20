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
        private int length;
        //Head, turning points, and tail of the snake.
        [JsonProperty]
        private LinkedList<Point> verticies;

        /// <summary>
        /// Returns the length of the snake.  The first time being called it may take a moment.  After that it should run in constant time.
        /// </summary>
        /// <returns>The length of the snake</returns>
        public int GetLength()
        {
            if (length == 0)
            {
                length = this.GetSnakePoints().Count - 1;
            }

            return length;
        }

        /// <summary>
        /// Returns the head of the snake as a point
        /// </summary>
        /// <returns>A point representing the head of the snake</returns>
        public Point GetHead()
        {
            return verticies.Last.Value;
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
        [JsonConstructor]
        public Snake(LinkedList<Point> Verticies, int ID, string Name)
        {
            this.verticies = Verticies;
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

            foreach (Point vertex in verticies)
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

            // Go through each vertice in this snake
            foreach (Point nextPoint in verticies)
            {
                // If it isn't the first verticie do this junk
                if (!nextPoint.Equals(verticies.First.Value))
                {
                    // Look at the case where they have the same  but ifferent y values
                    if (PreviousPoint.X == nextPoint.X)
                    {
                        // if previous points Y is the larger one we decrement down to nextpoint y
                        if (PreviousPoint.Y > nextPoint.Y)
                        {
                            // loop through and add all the points between the vertices
                            for (int i = PreviousPoint.Y; i > nextPoint.Y; i--)
                            {
                                // go ahead and add the current point to the result
                                Point current = new Point();
                                current.X = nextPoint.X;
                                current.Y = i;
                                if (nextPoint.Y != current.Y)
                                    result.Add(current);
                            }
                            
                        }
                        // If previouspoint Y is smaller than the next point Y we increment up to it
                        else if (PreviousPoint.Y < nextPoint.Y)
                        {
                            // loop through and add all the points between the vertices
                            for (int i = PreviousPoint.Y; i < nextPoint.Y; i++)
                            {
                                // go ahead and add the current point to the result
                                Point current = new Point();
                                current.X = nextPoint.X;
                                current.Y = i;
                                if (nextPoint.Y != current.Y)
                                    result.Add(current);
                            }
                        }
                    }
                    else if (PreviousPoint.Y == nextPoint.Y)
                    {
                        // if previous points X is the larger one we decrement down to nextpoint x
                        if (PreviousPoint.X > nextPoint.X)
                        {
                            // loop through and add all the points between the vertices
                            for (int i = PreviousPoint.X; i > nextPoint.X; i--)
                            {
                                // go ahead and add the current point to the result
                                Point current = new Point();
                                current.Y = nextPoint.Y;
                                current.X = i;
                                if (nextPoint.X != current.X)
                                    result.Add(current);
                            }

                        }
                        // If previouspoint X is smaller than the next point X we increment up to it
                        else if (PreviousPoint.X < nextPoint.X)
                        {
                            // loop through and add all the points between the vertices
                            for (int i = PreviousPoint.X; i < nextPoint.X; i++)
                            {
                                // go ahead and add the current point to the result
                                Point current = new Point();
                                current.Y = nextPoint.Y;
                                current.X = i;
                                if (nextPoint.X != current.X)
                                    result.Add(current);
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("The verticies of the snake do not make sense");
                    }
                }

                PreviousPoint = nextPoint;
            }
            // Add the last verticie into the reslut.  It would not be add in the above algorithm
            result.Add(verticies.Last.Value);
            return result;
        }

    }
}
