using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SnakeModel
{
    public class Food
    {
        /// <summary>
        /// ID for this peice of food.
        /// </summary>
        public int ID
        {
            get;
            private set;
        }

        /// <summary>
        /// X and Y coordinate point for the food's location. When food is eaten,
        /// the point is (-1,-1).
        /// </summary>
        public Point loc
        {
            get;
            private set;
        }

        public Food( int ID, Point loc)
        {
            this.ID = ID;
            this.loc = loc;
        }
       
    }
}
