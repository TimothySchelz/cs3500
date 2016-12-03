// Created by Gray Marchese, u0884194, and Timothy Schelz, u0851027
// Last Date Updated: 11/22/16
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SnakeModel
{
    /// <summary>
    /// This class represents the world!  It contains all snakes and food. 
    /// As a reminder, first entry is X and second entry is Y!!!
    /// </summary>
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
        // RNG for random world values
        Random rando = new Random();
        // Probability that a snake body point will turn to food
        private double snakeRecycleRate;
        // Food made available per snake.
        private int foodDensity;

        // A variable used for assigning unique Food ID numbers
        int NextFoodID;

        // Locks for The Food and the Snakes so that we can only be adding or getting from them by one thread at a time
        Object SnakeLock = new object();
        Object FoodLock = new object();


        /// <summary>
        /// Snake being controlled by the player.
        /// </summary>
        public Snake PlayerSnake
        {
            get;
            private set;
        }

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

        /// <summary>
        /// Creates A whole new world, 
        /// A new fantastic point of view
        /// No one to tell us no
        /// Or where to go
        /// Or say we're only dreaming
        /// (Only for client use)
        /// </summary>
        /// <param name="PlayerID">The ID of the player snake</param>
        /// <param name="Width">The width of the world</param>
        /// <param name="Height">The height of the world</param>
        public World(int PlayerID, int Width, int Height)
        {
            // initializes and sets everything
            this.Height = Height;
            this.Width = Width;
            Map = new Int32[Width, Height];

            //Iterate through every  cell in our map
            for(int x = 0; x < Width; x++)
            {
                for(int y = 0; y < Height; y++)
                {
                    //Leave the edges of our map to be 0
                    if (x == 0 || x == Width - 1 || y == 0 || y == Height - 1)
                        continue;

                    //Change everything else to -1
                    Map[x, y] = -1;
                }
            }




            Snakes = new Dictionary<int, Snake>();
            Foods = new Dictionary<int, Food>();
            SnakeColors = new Dictionary<int, Color>();
            this.PlayerID = PlayerID;
            NextFoodID = 0;
        }

        /// <summary>
        ///  Constructor used only by the server, where food density and snake recycle rate can be defined.
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="FoodDensity"></param>
        /// <param name="snakeRecycleRate"></param>
        public World(int Width, int Height, int foodDensity, double snakeRecycleRate) : this(0, Width, Height)
        {
            this.snakeRecycleRate = snakeRecycleRate;
            this.foodDensity = foodDensity;
        }

        /// <summary>
        /// Advances the world to the next frame of the game. Responsible for snake motion and food generation.
        /// </summary>
        public void UpdateWorld()
        {

            // Move snakes and checks for collisions and interactions
            MoveSnakes();

            // Generate food
            PopulateWithFood(foodDensity);
        }

       /// <summary>
       /// Given a specified snake and a new location for its head,
       /// detects collisions of the snakes head and kills or grows
       /// the snake if needed.
       /// </summary>
       /// <param name="newHead">New possition of the snake head</param>
       /// <param name="snake">Snake to be updated</param>
        private void Interactions(Point newHead, Snake snake)
        {
           
            switch(Map[newHead.X, newHead.Y])
            {

                //Empty Space
                case -1:
                    Map[newHead.X, newHead.Y] = 2;
                    Point prevTail = snake.moveTailForward();
                    Map[prevTail.X, prevTail.Y] = -1;
                    break;
                
                //Wall
                case 0:
                    KillSnake(snake);
                    break;

                //Food
                case 1:
                    Map[newHead.X, newHead.Y] = 2;
                    break;
                
                //Snake
                case 2:
                    KillSnake(snake);
                    break;

            }

        }

        /// <summary>
        /// Decides what to do with each body point upon death
        /// </summary>
        /// <param name="snake"></param>
        private void KillSnake(Snake snake)
        {
            //Iterates over all points in snake body
            foreach(Point point in snake.GetSnakePoints())
            {
                //With specified probability, turns the snake body to food or empty space
                if (rando.NextDouble() < snakeRecycleRate)
                {
                    //Updates world map with the new food.
                    Map[point.X, point.Y] = 1;

                    //Creates a new peice of food in our list.
                    updateFood(new Food(NextFoodID, point));
                }
                else
                {
                    //Updates world map with empty space.
                    Map[point.X, point.Y] = -1;
                }
            }

            //this kills the snake
            snake.KillMe();
        }

        /// <summary>
        /// Populares the world with food until we have enough food to feed every snake.
        /// </summary>
        /// <param name="FoodDensity">The amount of food per snake</param>
        private void PopulateWithFood(int FoodDensity)
        {
            int FoodDisparity;

            // Determine how much food is missing from the world based on density 
            // Formula split into seperate locks to avoid nested locks and possible deadlocks.
            lock (SnakeLock)  // Deal with snake portion first.  Lock it so we don't hve any problems
            {
                FoodDisparity = (Snakes.Count * FoodDensity);
            }

            lock(FoodLock) // then deal with food portion.  Lock it so e don't have any problems
            {
                FoodDisparity = FoodDisparity - Foods.Count;
            }

            // Create new food
            if (FoodDisparity > 0)
            {
                for (int i = 0; i < FoodDisparity; i++)
                {
                    generateFood();
                }
            }
        }

        /// <summary>
        /// Moves the snakes in the direction they want to go by 1
        /// </summary>
        private void MoveSnakes()
        {
            lock(SnakeLock)
            {
                // List of snake ID's to be removed
                List<int> MarkedForRemoval = new List<int>();

                foreach (KeyValuePair<int, Snake> snakePair in Snakes)
                {
                    // Remove the snake from the list of snakes if the snake is dead.
                    if (snakePair.Value.GetHead().X == -1)
                    {
                        // Create a list of IDs of snakes to be removed... Can't actually remove 
                        // them here since we are iterating through the snakes
                        MarkedForRemoval.Add(snakePair.Key);
                        continue;
                    }

                    // Move head Forward
                    Point newHead = snakePair.Value.moveHeadForward();

                    // Checks for interactions of the snake with other objects.
                    // Updates world map & snake body accordingly.
                    Interactions(newHead, snakePair.Value);
                }

                // Actually remove the dead snakes
                foreach(int SID in MarkedForRemoval)
                {
                    Snakes.Remove(SID);
                }
                
            }
        }

        /// <summary>
        /// Gets all the snakes in the world
        /// </summary>
        /// <returns>Hashset of snakes in the world</returns>
        public HashSet<Snake> GetSnakes()
        {
            // No one messes with the Tunnel Snakes!
            // Tunnel Snakes Rule!
            // These snakes on the other hand can be messed with 1 thread at a time.
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
            // Only 1 thread messes with food at a time.
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
            // Only 1 thread messes with food at a time.
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
                        // if it hasn't, add it to our dictionary of food & our map
                        Foods.Add(newFood.ID, newFood);
                        Map[newFood.loc.X, newFood.loc.Y] = 1;

                        // otherwise don't do anything.  It is gone
                    }
                }
            }
        }

        /// <summary>
        /// Generates food in a random location in our world.
        /// </summary>
        public void generateFood() { 
            Point pt = new Point();

            pt.X = rando.Next() % Width;
            pt.Y = rando.Next() % Height;

            NextFoodID++;
            updateFood(new Food(NextFoodID - 1, pt));

        }

        /// <summary>
        /// Places a peice of food at the specified (X,Y) coordinate.
        /// </summary>
        /// <param name="X">Horizontal coordinate for food placement</param>
        /// <param name="Y">Vertical coordinate for food placement</param>
        public void generateFood(int X, int Y)
        {
            Point pt = new Point();

            pt.X = X;
            pt.Y = Y;

            NextFoodID++;
            updateFood(new Food(NextFoodID - 1, pt));

        }

        /// <summary>
        /// Updates the given snake
        /// </summary>
        /// <param name="newSnake">Snake that needs to be updated</param>
        public void updateSnake(Snake newSnake)
        {
            // No one messes with the Tunnel Snakes!
            // Tunnel Snakes Rule!
            // These snakes on the other hand can be messed with 1 thread at a time.
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
            // No one messes with the Tunnel Snakes!
            // Tunnel Snakes Rule!
            // These snakes on the other hand can be messed with 1 thread at a time.
            lock (SnakeLock)
            {
                if (!SnakeColors.ContainsKey(ID))
                {
                    return Color.Black;
                }

                return SnakeColors[ID];
            }
        }

        /// <summary>
        /// Creates a snake with the given ID number. This is the first appearance of this snake
        /// in the world.
        /// </summary>
        /// <param name="ID">Unique ID number for this new snake</param>
        public void createSnake(int ID, string Name)
        {

            Point Head = new Point();
            Point Tail = new Point();

            //TODO: FIGURE OUT BETTER PLACEMENT FOR NEW SNAKE
            

            Head.X = (Width / 2) - 1;
            Head.Y = Height / 2;

            Tail.X = (Width / 2) + 40;
            Tail.Y = Head.Y;


            List<Point> verts = new List<Point>() { Head, Tail };
            Snake snake = new Snake(verts, ID, Name);


            //Set appropriate direction
            snake.Direction = 2;
            snake.PrevDirection = 2;

            // Puts the new snake in the world
            this.updateSnake(snake);


        }

        /// <summary>
        /// Changes the direction of the given snake to a valid direction based on the input and the prevDirection
        /// 1: up
        /// 2: right
        /// 3: down
        /// 4: left
        /// 
        /// </summary>
        /// <param name="ID">The ID os the snake to change direction</param>
        /// <param name="direction">The direction the snake should be changed too</param>
        public void ChangeSnakeDirection(int ID, int direction)
        {
            Snakes[ID].Direction = direction;
        }

        /// <summary>
        /// This should only be called when the world gets updated and the snakes move forward.
        /// Changes the direction of the given snake to a valid direction based on the input and the prevDirection
        /// 1: up
        /// 2: right
        /// 3: down
        /// 4: left
        /// 
        /// </summary>
        /// <param name="ID">The ID os the snake to change direction</param>
        /// <param name="direction">The direction the snake should be changed too</param>
        public void ChangeSnakePrevDirection(int ID, int direction)
        {
            Snakes[ID].PrevDirection = direction;
        }
    }
}