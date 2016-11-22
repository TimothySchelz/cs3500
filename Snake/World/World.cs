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
        private Int32[,] Map;                     //first entry is X and the second entry is Y!!!!!
        // All the snakes in the world
        private Dictionary<int, Snake> Snakes;
        // All the Food in the world
        private Dictionary<int, Food> Foods;
        // Saves each snake's assigned color;
        private Dictionary<int, Color> SnakeColors;
        // The player
        int PlayerID;

        /// <summary>
        /// Snake being controlled by the player.
        /// </summary>
        public Snake PlayerSnake
        {
            get;
            private set;
        }

        // RNG to get  colors for the snakes
        Random rando = new Random();

        // Locks for The Food and the Snakes so that we can only be adding or getting from them by one thread at a time
        Object SnakeLock = new object();
        Object FoodLock = new object();

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

        public World(int PlayerID, int Width, int Height)
        {
            this.Height = Height;
            this.Width = Width;
            Map = new Int32[Width, Height];
            Snakes = new Dictionary<int, Snake>();
            Foods = new Dictionary<int, Food>();
            SnakeColors = new Dictionary<int, Color>();
            this.PlayerID = PlayerID;
        }

        /// <summary>
        /// Gets all the snakes in the world
        /// </summary>
        /// <returns>Hashset of snakes in the world</returns>
        public HashSet<Snake> GetSnakes()
        {
            lock(SnakeLock)
            {
                // The copy of the snakes to be returned
                HashSet<Snake> result = new HashSet<Snake>();

                // cycle through all the snakes
                foreach (KeyValuePair<int, Snake> Voldemort in Snakes)
                {
                    // add the current snake to set of things to be returned
                    result.Add(Voldemort.Value);
                }

                // Return the list of snakes
                return result;
            }
        }

        /// <summary>
        /// Returns all the Food in the world
        /// </summary>
        /// <returns>A hashset of food in the world</returns>
        public HashSet<Food> GetFood()
        {
            lock (FoodLock)
            {
                // The copy of the Food to be returned
                HashSet<Food> result = new HashSet<Food>();

                // cycle through all the Food
                foreach (KeyValuePair<int, Food> ComboMeal in Foods)
                {
                    // add the current Food to set of things to be returned
                    result.Add(ComboMeal.Value);
                }

                // Return the list of Food
                return result;
            }
        }

        /// <summary>
        /// Updates the given food item
        /// </summary>
        /// <param name="newFood"></param>
        public void updateFood(Food newFood)
        {
            lock (FoodLock)
            {
                // check if the food is already known about
                if (Foods.ContainsKey(newFood.ID))
                {
                    // If the food hs been eaten we remove it
                    if (newFood.loc.X == -1)
                    {
                        Foods.Remove(newFood.ID);
                    }

                    // Reset the food incase they implement moving food or something
                    Foods[newFood.ID] = newFood;
                }
                else
                {
                    // Check if the food has been eaten 
                    if (newFood.loc.X != -1)
                    {
                        // if it hasn't, add it to our dictionary of food
                        Foods.Add(newFood.ID, newFood);

                        // otherwise don't do anything.  It is gone
                    }
                }
            }
        }

        /// <summary>
        /// Updates the given snake
        /// </summary>
        /// <param name="newSnake">Snake that needs to be updated</param>
        public void updateSnake(Snake newSnake)
        {
            lock (SnakeLock)
            {
                //Makes sure we can reference the player's snake easily.
                if(newSnake.ID == PlayerID)
                {
                    PlayerSnake = newSnake;
                }

                // Check if the new snake already existed
                if (Snakes.ContainsKey(newSnake.ID))
                {
                    foreach (Point point in Snakes[newSnake.ID].GetSnakePoints())
                    {
                        Map[point.X, point.Y] = -1;
                    }

                    // Check if the snake is dead
                    if (newSnake.GetVerticies().First.Value.X == -1)
                    {
                        Snakes.Remove(newSnake.ID);
                        return;
                    }

                    // Loop through each point in the snake set the value to 2
                    foreach (Point currentPoint in newSnake.GetSnakePoints())
                    {
                        Map[currentPoint.X, currentPoint.Y] = 2;
                    }

                    Snakes[newSnake.ID] = newSnake;
                }
                else
                {
                    
                    SnakeColors[newSnake.ID] = Color.FromArgb(rando.Next(255), rando.Next(255), rando.Next(255));

                    // Check if the snake is dead
                    if (newSnake.GetVerticies().First.Value.X == -1)
                    {
                        Snakes.Remove(newSnake.ID);
                        return;
                    }

                    // Loop through each point in the snake set the value to 2
                    foreach (Point currentPoint in newSnake.GetSnakePoints())
                    {
                        Map[currentPoint.X, currentPoint.Y] = 2;
                    }

                    Snakes.Add(newSnake.ID, newSnake);
                }
            }
        }

        /// <summary>
        /// Returns the color of the snake with the given ID
        /// </summary>
        /// <param name="ID">The ID of the snake</param>
        /// <returns>The color of the snake</returns>
        public Color GetSnakeColor(int ID)
        {
            lock (SnakeLock)
            {
                if (!SnakeColors.ContainsKey(ID))
                {
                    return Color.Black;
                }

                return SnakeColors[ID];
            }
        }
    }
}


