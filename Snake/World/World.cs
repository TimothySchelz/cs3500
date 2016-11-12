using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeModel
{
    public class World
    {

        // A 2D array to model the worldspace.
        Int32[,] Map; //first entry is X and the second entry is Y!!!!!
        // All the snakes in the world
        HashSet<Snake> Snakes;
        // All the Food in the world
        HashSet<Food> Foods;

        public World(int NumberOfPlayers, int Length, int Width)
        {
            Map = new Int32[Length, Width];

            Snakes = new HashSet<Snake>();
            Foods = new HashSet<Food>();
        }

        /// <summary>
        /// Takes in a new set of snakes and a set of food to replace the old ones
        /// </summary>
        /// <param name="NewSnakes">The Snakes </param>
        /// /// <param name="NewFood"></param>
        public void UpdateWorld(HashSet<Snake> NewSnakes, HashSet<Food> NewFood)
        {
            // Loop through the old snakes to change their cells to -1
            foreach(Snake currentSnake in Snakes)
            {
                // Loop through each point in the snakes
                foreach(Point currentPoint in currentSnake.GetSnakePoints())
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


    }
}


