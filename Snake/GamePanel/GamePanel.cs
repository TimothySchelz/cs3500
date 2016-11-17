using System;
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
        private const int PIXELSPERCELL = 5;


        /// <summary>
        /// Constructor for creating a Game panel.
        /// </summary>
        public GamePanel()
        {
            this.DoubleBuffered = true;
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
            base.OnPaint(e);

            // check if the world exists.  If it doesn't we are done
            if (world == null)
                return;

            // turn on antialiasing
           // e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Paint all the items in the world
            PaintWalls(e);
            PaintSnakes(e);
            PaintFood(e);
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

                // go through all he food
                foreach( Food food in Foods)
                {
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
