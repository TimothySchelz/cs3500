﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SnakeModel;
using System.Drawing;

namespace SnakeGUI
{
    /// <summary>
    /// A panel to be used to display the snake game
    /// </summary>
    public class GamePanel : Panel
    {

        // the world that this panel should draw
        private World world;

        // The length and width of each cell
        private int PIXELSPERCELL = 1;

        float ScalingFactor;

        // Current length of the player
        private float SnakeSize;


        /// <summary>
        /// Constructor for creating a Game panel.
        /// </summary>
        public GamePanel()
        {
            this.DoubleBuffered = true;
            this.BackColor = Color.White;
        }
        
        /// <summary>
        /// Sets the world
        /// </summary>
        /// <param name="newWorld"></param>
        public void SetWorld( World newWorld)
        {
            world = newWorld;
        }

        /// <summary>
        /// What to do when this panel needs to be painted
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // check if the world exists.  If it doesn't we are done
            if (world == null)
                return;

            if (world.PlayerSnake != null)
            {
                // The Length of the snake
                SnakeSize = (float)world.PlayerSnake.GetLength() + 1;
            }

            // turn on antialiasing
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Game is scaled and focused if the snake is relatively small and alive.
            if ((SnakeSize < world.Width / 2) && world.PlayerSnake.GetHead().X != -1)
            {
                PIXELSPERCELL = 1;

                // What to scale the world by
                ScalingFactor = 2.5F * world.Height * PIXELSPERCELL / (SnakeSize);


                // Scale
                e.Graphics.ScaleTransform(ScalingFactor, ScalingFactor);



                // Shift to snakehead
                float xOffset = -(world.PlayerSnake.GetHead().X - world.PlayerSnake.GetLength())*PIXELSPERCELL;
                float yOffset = -(world.PlayerSnake.GetHead().Y - world.PlayerSnake.GetLength())* PIXELSPERCELL;

                e.Graphics.TranslateTransform(xOffset, yOffset);
            }
            else
            {
                PIXELSPERCELL = 5;
            }



            // Paint all the items in the world
            PaintSnakes(e);
            PaintFood(e);
            PaintWalls(e);
        }

        /// <summary>
        /// Paints the food in the world
        /// </summary>
        /// <param name="e"></param>
        private void PaintFood(PaintEventArgs e)
        {
            // Use a brush
            using (SolidBrush drawBrush = new SolidBrush(Color.Brown))
            {
                HashSet<Food> Foods = world.GetFood();

                // go through all the food
                foreach( Food food in Foods)
                {
                    /*If this food is within a snake length of the player's head, it is drawn.
                    //It is not drawn if it has been eaten.
                    if (Math.Abs(food.loc.X - world.PlayerSnake.GetHead().X) > world.PlayerSnake.GetLength() + 1
                        || Math.Abs(food.loc.Y - world.PlayerSnake.GetHead().Y) > world.PlayerSnake.GetLength() + 1
                        || food.loc.X == -1)
                        continue;
                        */
                        

                    //draw the food
                    Rectangle dropFood = new Rectangle(food.loc.X * PIXELSPERCELL, food.loc.Y * PIXELSPERCELL, PIXELSPERCELL, PIXELSPERCELL);
                    e.Graphics.FillEllipse(drawBrush, dropFood);
                }
            }
        }

        /// <summary>
        /// Paints the snakes from the world
        /// </summary>
        /// <param name="e"></param>
        private void PaintSnakes(PaintEventArgs e)
        {


            // Use the brush
            using (SolidBrush drawBrush = new SolidBrush(Color.Black))
            {
                // Go through each snake
                foreach (Snake snake in world.GetSnakes())
                {
                    // Go through each point in this snake
                    HashSet<SnakeModel.Point> snakePoints = snake.GetSnakePoints();
                    foreach (SnakeModel.Point point in snakePoints)
                    {
                        /*If this point is within a snake length of the player's head, it is drawn.
                        //It is not drawn if it is part of a dead snake.
                        if (Math.Abs(point.X - world.PlayerSnake.GetHead().X) > world.PlayerSnake.GetLength() + 1
                        || Math.Abs(point.Y - world.PlayerSnake.GetHead().Y) > world.PlayerSnake.GetLength() + 1
                        || point.X == -1)
                            continue;
                            */

                        // change the color
                        drawBrush.Color = world.GetSnakeColor(snake.ID);

                        // Draw this point
                        Rectangle drawPoint = new Rectangle(point.X * PIXELSPERCELL, point.Y * PIXELSPERCELL, PIXELSPERCELL, PIXELSPERCELL);
                        e.Graphics.FillRectangle(drawBrush, drawPoint);
                    }
                }
            }
        }

        /// <summary>
        /// Paints the walls on the panel
        /// </summary>
        /// <param name="e"></param>
        private void PaintWalls(PaintEventArgs e)
        {
            using(SolidBrush drawBrush = new SolidBrush(Color.Black))
            {
                Rectangle TopWall = new Rectangle(0, 0, world.Width * PIXELSPERCELL, PIXELSPERCELL);
                e.Graphics.FillRectangle(drawBrush, TopWall);

                Rectangle BottomWall = new Rectangle(0, (world.Height - 1) * PIXELSPERCELL, world.Width * PIXELSPERCELL, PIXELSPERCELL);
                e.Graphics.FillRectangle(drawBrush, BottomWall);

                Rectangle LeftWall = new Rectangle(0, 0, PIXELSPERCELL, world.Height * PIXELSPERCELL);
                e.Graphics.FillRectangle(drawBrush, LeftWall);

                Rectangle RightWall = new Rectangle((world.Width - 1) * PIXELSPERCELL, 0, PIXELSPERCELL, world.Height * PIXELSPERCELL);
                e.Graphics.FillRectangle(drawBrush, RightWall);
            }
        }
    }
}
