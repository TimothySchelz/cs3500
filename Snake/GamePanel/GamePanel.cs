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
    public class GamePanel : Panel
    {

        private World world;

        private const int PIXELSPERCELL = 5;


        public GamePanel()
        {
            this.DoubleBuffered = true;
        }

        public void SetWorld( World newWorld)
        {
            world = newWorld;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (world == null)
                return;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            PaintWalls(e);
            PaintSnakes(e);
            PaintFood(e);

        }

        private void PaintFood(PaintEventArgs e)
        {
            using (SolidBrush drawBrush = new SolidBrush(Color.Brown))
            {
                HashSet<Food> Foods = world.GetFood();

                foreach( Food food in Foods)
                {
                    Rectangle dropFood = new Rectangle(food.loc.X * PIXELSPERCELL, food.loc.Y * PIXELSPERCELL, PIXELSPERCELL, PIXELSPERCELL);
                    e.Graphics.FillEllipse(drawBrush, dropFood);
                }
            }
        }

        private void PaintSnakes(PaintEventArgs e)
        {
            
            foreach(Snake snake in world.GetSnakes())
            {
                using (SolidBrush drawBrush = new SolidBrush(world.GetSnakeColor(snake.ID)))
                {
                    foreach (SnakeModel.Point point in snake.GetSnakePoints())
                    {
                        Rectangle drawPoint = new Rectangle(point.X * PIXELSPERCELL, point.Y * PIXELSPERCELL, PIXELSPERCELL, PIXELSPERCELL);
                        e.Graphics.FillRectangle(drawBrush, drawPoint);
                    }
                }
            }

        }

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
