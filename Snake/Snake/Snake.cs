using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World;

namespace Snake
{
    public class Snake
    {
        //Head, turning points, and tail of the snake.
        private LinkedList<Point> Verticies;

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

        public Snake (LinkedList<Point> Verticies, int ID, string Name)
        {
            this.Verticies = Verticies;
            this.ID = ID;
            this.Name = Name;
        }

    }
}
