using SnakeModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGUI
{
    public partial class Form1 : Form
    {
        private World world;

        public Form1()
        {
            InitializeComponent();

            Timer FrameTimer = new Timer();
            FrameTimer.Interval = 33;
            FrameTimer.Tick += UdpateFrame;
            FrameTimer.Start();

        }

        private void UdpateFrame(object sender, EventArgs e)
        {
            gamePanel1.Invalidate();
            scoreBoardPanel1.Invalidate();
        }

        private void OnStartUp(object sender, EventArgs e)
        {
            world = new World(1, 150, 150);

            HashSet<Snake> snakes = new HashSet<Snake>();
            LinkedList<SnakeModel.Point> snakeVerts = new LinkedList<SnakeModel.Point>();
            SnakeModel.Point p1, p2;
            p1.X = 2;
            p1.Y = 2;
            p2.X = 2;
            p2.Y = 5;
            snakeVerts.AddFirst(p1);
            snakeVerts.AddLast(p2);
            snakes.Add(new Snake(snakeVerts, 1, "TestSnake"));

            HashSet<Food> foods = new HashSet<Food>();
            p1.X = 3;
            p1.Y = 4;
            p2.X = 4;
            p2.Y = 3;
            foods.Add(new Food(1, p1));
            foods.Add(new Food(2, p2));

            world.UpdateWorld(snakes, foods);

            gamePanel1.SetWorld(world);
            scoreBoardPanel1.SetWorld(world);
        }
    }
}
