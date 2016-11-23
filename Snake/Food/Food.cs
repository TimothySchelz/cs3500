// Created by Gray Marchese, u0884194, and Timothy Schelz, u0851027
// Last Date Updated: 11/22/16
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SnakeModel
{
    /// <summary>
    /// Represents food.  Kinda obvious.  It has an ID, and a location
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Food
    {
        /// <summary>
        /// ID for this peice of food.
        /// </summary>
        [JsonProperty]
        public int ID
        {
            get;
            private set;
        }

        /// <summary>
        /// X and Y coordinate point for the food's location. When food is eaten,
        /// the point is (-1,-1).
        /// </summary>
        [JsonProperty]
        public Point loc
        {
            get;
            private set;
        }

        /// <summary>
        /// A constructor to create a food
        /// </summary>
        /// <param name="ID">The ID the food should have</param>
        /// <param name="loc">The location of the Food as a SnakeModel.Point</param>
        public Food( int ID, Point loc)
        {
            this.ID = ID;
            this.loc = loc;
        }
       
    }
}
