using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SnakeModel
{
    public class World
    {

        // A 2D array to model the worldspace.
        private Int32[,] Map;                               //first entry is X and the second entry is Y!!!!!
        // All the snakes in the world
        private HashSet<Snake> Snakes;
        // All the Food in the world
        private HashSet<Food> Foods;
        // Saves each snake's assigned color;
        private Dictionary<int, Color> SnakeColors;

        /// <summary>
        /// Length of the game board.
        /// </summary>
        public int Height
        {
            get;
            private set;
        }

        /// <summary>
        /// Width of the game board.
        /// </summary>
        public int Width
        {
            get;
            private set;
        }

        public World(int NumberOfPlayers, int Length, int Height)
        {
            this.Height = Height;
            this.Width = Width;
            Map = new Int32[Length+2, Width+2];
            Snakes = new HashSet<Snake>();
            Foods = new HashSet<Food>();
            SnakeColors = new Dictionary<int, Color>();
        }

        /// <summary>
        /// Gets all the snakes in the world
        /// </summary>
        /// <returns>Hashset of snakes in the world</returns>
        public HashSet<Snake> GetSnakes()
        {
            // The copy of the snakes to be returned
            HashSet<Snake> result = new HashSet<Snake>();

            // cycle through all the snakes
            foreach (Snake Voldemort in Snakes)
            {
                // add the current snake to set of things to be returned
                result.Add(Voldemort);
            }

            // Return the list of snakes
            return result;
        }

        /// <summary>
        /// Returns all the Food in the world
        /// </summary>
        /// <returns>A hashset of food in the world</returns>
        public HashSet<Food> GetFood()
        {
            // The copy of the Food to be returned
            HashSet<Food> result = new HashSet<Food>();

            // cycle through all the Food
            foreach (Food ComboMeal in Foods)
            {
                // add the current Food to set of things to be returned
                result.Add(ComboMeal);
            }

            // Return the list of Food
            return result;
        }

        /// <summary>
        /// Takes in a new set of snakes and a set of food to replace the old ones
        /// </summary>
        /// <param name="NewSnakes">The new snakes to store</param>
        /// /// <param name="NewFood">The new food to store</param>
        public void UpdateWorld(HashSet<Snake> NewSnakes, HashSet<Food> NewFood)
        {

            // Loop through the old snakes to change their cells to -1
            foreach(Snake currentSnake in Snakes)
            {
                if (!SnakeColors.ContainsKey(currentSnake.ID))
                {
                    SnakeColors[currentSnake.ID] = Color.FromArgb(currentSnake.ID * 4567);
                }

                // Loop through each point in the snakes
                foreach (Point currentPoint in currentSnake.GetSnakePoints())
                {
                    Map[currentPoint.X, currentPoint.Y] = -1;
                }
            }

            // Loop through all the old food and change their cells to -1
            foreach (Food currentFood in Foods)
            {
                Map[currentFood.loc.X, currentFood.loc.Y] = -1;
            }

            // Updating what snakes and food we have
            Snakes = NewSnakes;
            Foods = NewFood;

            // Loop through the new snakes to change their cells to -1
            foreach (Snake currentSnake in Snakes)
            {
                // Loop through each point in the snakes
                foreach (Point currentPoint in currentSnake.GetSnakePoints())
                {
                    Map[currentPoint.X, currentPoint.Y] = -1;
                }
            }

            // Loop through all the new food and change their cells to -1
            foreach (Food currentFood in Foods)
            {
                Map[currentFood.loc.X, currentFood.loc.Y] = -1;
            }
        }

        /// <summary>
        /// Returns the color of the snake with the given ID
        /// </summary>
        /// <param name="ID">The ID of the snake</param>
        /// <returns>The color of the snake</returns>
        public Color GetSnakeColor(int ID)
        {
            if (!SnakeColors.ContainsKey(ID))
            {
                return Color.Black;
            }

            return SnakeColors[ID];
        }
    }
}


