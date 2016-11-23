using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SnakeModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SnakeGUI
{
    /// <summary>
    /// A panel to be used to display the snake game
    /// </summary>
    public class GamePanel : Panel
    {

        // the world that this panel should draw
        private World world;

        float SnakeScaling;
        float WorldScaling;

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
        public void SetWorld(World newWorld)
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
            if (world == null || world.PlayerSnake == null)
                return;

            if (world.PlayerSnake != null)
            {
                // The Length of the snake
                SnakeSize = (float)world.PlayerSnake.GetLength() + 1;
            }

            // turn on antialiasing
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Game is scaled and focused if the snake is relatively small and alive.

            if ((SnakeSize < Math.Min(world.Width, world.Height) / 2) && world.PlayerSnake.GetHead().X != -1)
            {

                // What to scale the width by
                SnakeScaling = (float)this.Height / ((float)2 * (SnakeSize));

                // Shift to snakehead
                float xOff = (((float)this.Width / (2F)) - world.PlayerSnake.GetHead().X * SnakeScaling);
                float yOff = (((float)this.Height / (2F)) - world.PlayerSnake.GetHead().Y * SnakeScaling);

                e.Graphics.TranslateTransform(xOff, yOff);

                // Scale
                e.Graphics.ScaleTransform(SnakeScaling, SnakeScaling);

            } else
            {
                // Do the world scaling
                WorldScaling = (float)this.Height / (float)Math.Max(world.Height, world.Width);

                e.Graphics.ScaleTransform(WorldScaling, WorldScaling);

                float xOffset = this.Width / 2F - (world.Width * WorldScaling) / 2F;
                float yOffset = this.Height / 2F - (world.Height * WorldScaling) / 2F;

                e.Graphics.TranslateTransform(xOffset / WorldScaling, yOffset / WorldScaling);
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
                foreach (Food food in Foods)
                {

                    //don't draw eaten phood
                    if (food.loc.X == -1)
                        continue;

                    //draw the food
                    Rectangle dropFood = new Rectangle(food.loc.X, food.loc.Y, 1, 1);
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

                        //don't draw dead sneaks
                        if (point.X == -1)
                            continue;

                        // change the color
                        drawBrush.Color = world.GetSnakeColor(snake.ID);

                        // Draw this point
                        Rectangle drawPoint = new Rectangle(point.X, point.Y, 1, 1);
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
            using (SolidBrush drawBrush = new SolidBrush(Color.Black))
            {
                Rectangle TopWall = new Rectangle(0, 0, world.Width, 1);
                e.Graphics.FillRectangle(drawBrush, TopWall);

                Rectangle BottomWall = new Rectangle(0, (world.Height - 1), world.Width, 1);
                e.Graphics.FillRectangle(drawBrush, BottomWall);

                Rectangle LeftWall = new Rectangle(0, 0, 1, world.Height);
                e.Graphics.FillRectangle(drawBrush, LeftWall);

                Rectangle RightWall = new Rectangle((world.Width - 1), 0, 1, world.Height);
                e.Graphics.FillRectangle(drawBrush, RightWall);
            }
        }
    }
}
