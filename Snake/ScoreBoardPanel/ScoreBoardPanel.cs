using SnakeModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGUI
{
    public class ScoreBoardPanel : Panel
    {
        private const int TOPMARGIN = 10;
        private const int NAMEALIGN = 10;
        private const int SCOREALIGN = 200;
        private const int LINEHEIGHT = 24;
        private const int LINESPACE = 5;

        private World world;

        public ScoreBoardPanel()
        {
            this.DoubleBuffered = true;
            this.BackColor = Color.LightGray;
        }

        public void SetWorld(World newWorld)
        {
            world = newWorld;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            
            //base.OnPaint(e);
            int currentLine = TOPMARGIN;

            if (world == null)
            {
                return;
            }

            foreach(Snake currentSnake in world.GetSnakes())
            {

                int fontSize = Math.Min(LINEHEIGHT, 10 * LINEHEIGHT / currentSnake.name.Length);

                using(Font font = new Font("Arial", fontSize, FontStyle.Bold, GraphicsUnit.Pixel))
                {
                    // Draw the names
                    System.Drawing.Point NameStart = new System.Drawing.Point(NAMEALIGN, currentLine);
                    TextRenderer.DrawText(e.Graphics, currentSnake.name, font, NameStart, world.GetSnakeColor(currentSnake.ID));

                    // Draw the score
                    System.Drawing.Point ScoreStart = new System.Drawing.Point(SCOREALIGN, currentLine);
                    TextRenderer.DrawText(e.Graphics, ""+currentSnake.GetLength(), font, ScoreStart, world.GetSnakeColor(currentSnake.ID));
                }

                currentLine += fontSize + LINESPACE;
            }
        }
    }
}
