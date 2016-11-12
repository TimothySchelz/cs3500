using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeModel
{
    /// <summary>
    /// A struct to hold the X and Y values of whatever
    /// </summary>
    public struct Point
    {
        public int X;
        public int Y;
    }

    public class Snake
    {
        //Head, turning points, and tail of the snake.
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
        public int ID
        {
            get;
            private set;
        }

        //Name of snake as decided by associated player.
        private string Name;

        public Snake (LinkedList<Point> Verticies, int Length, int ID, string Name)
        {
            this.Verticies = Verticies;
            this.Length = Length;
            this.ID = ID;
            this.Name = Name;
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
                // Check if this is the first Virtice
                if (joint.Equals(Verticies.First))
                {
                    // just set it as the previous point and we are done on this go around.
                    PreviousPoint = joint;
                } else
                {
                    //Initilize the start and end vlaues to 0
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
