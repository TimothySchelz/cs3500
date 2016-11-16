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

        private World world;

        public ScoreBoardPanel()
        {
            this.DoubleBuffered = true;
        }

        public void SetWorld(World newWorld)
        {
            world = newWorld;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            int currentLine = 0;

            if (world == null)
            {
                return;
            }

            foreach(Snake currentSnake in world.GetSnakes())
            {
                using(Font font = new Font("Arial", LINEHEIGHT, FontStyle.Bold, GraphicsUnit.Pixel))
                {
                    System.Drawing.Point NameStart = new System.Drawing.Point(NAMEALIGN, LINEHEIGHT*currentLine+TOPMARGIN);
                    TextRenderer.DrawText(e.Graphics, currentSnake.name, font, NameStart, world.GetSnakeColor(currentSnake.ID));

                    System.Drawing.Point ScoreStart = new System.Drawing.Point(SCOREALIGN, LINEHEIGHT * currentLine + TOPMARGIN);
                    TextRenderer.DrawText(e.Graphics, ""+currentSnake.Length, font, ScoreStart, world.GetSnakeColor(currentSnake.ID));
                }

                currentLine++;
            }
        }
    }
}
