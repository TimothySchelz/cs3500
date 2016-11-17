using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SnakeModel;
using System.Collections.Generic;

namespace SnakeUnitTest
{
    /// <summary>
    /// A project for testing pieces of the snake game
    /// </summary>
    [TestClass]
    public class SnakeUnitTests
    {
        /*
         * Food Tests
         */

        /// <summary>
        /// Check if the ID is storing the correct value
        /// </summary>
        [TestMethod]
        public void Food_Con_CheckID()
        {
            Food testFood = new Food(24, new Point());

            Assert.AreEqual(24, testFood.ID);
        }

        /// <summary>
        /// 
        /// Check if the Loc is storing the correct value
        /// </summary>
        [TestMethod]
        public void Food_Con_CheckLoc()
        {
            Point point1 = new Point();
            point1.X = 5;
            point1.Y = 3;
            Food testFood = new Food(24, point1);

            Assert.AreEqual(5, testFood.loc.X);
            Assert.AreEqual(3, testFood.loc.Y);
        }

        /// <summary>
        /// Check that it is storing null locs
        /// </summary>
        [TestMethod]
        public void Food_Con_NullLoc()
        {
            Point point1 = new Point();

            point1 = null;
            Food testFood = new Food(24, point1);

            Assert.AreEqual(null, testFood.loc);
        }

        /*
         * Snake Tests
         */
        [TestMethod]
        public void Snake_Length_1Segment()
        {
            LinkedList<Point> joints = new LinkedList<Point>();
            Point p1 = new Point();
            Point p2 = new Point();
            p1.X = 5;
            p1.Y = 5;
            p2.X = 15;
            p2.Y = 5;
            joints.AddFirst(p1);
            joints.AddFirst(p2);

            Snake testSnake = new Snake(joints, 1, "john");

            Assert.AreEqual(10, testSnake.Length);
        }

        [TestMethod]
        public void Snake_Length_ShortSegment()
        {
            LinkedList<Point> joints = new LinkedList<Point>();
            Point p1 = new Point();
            Point p2 = new Point();
            p1.X = 5;
            p1.Y = 5;
            p2.X = 6;
            p2.Y = 5;
            joints.AddFirst(p1);
            joints.AddFirst(p2);

            Snake testSnake = new Snake(joints, 1, "john");

            Assert.AreEqual(1, testSnake.Length);
        }

        [TestMethod]
        public void Snake_Length_3Segments()
        {
            LinkedList<Point> joints = new LinkedList<Point>();
            Point p1 = new Point();
            Point p2 = new Point();
            Point p3 = new Point();
            Point p4 = new Point();
            p1.X = 5;
            p1.Y = 5;

            p2.X = 8;
            p2.Y = 5;

            p3.X = 8;
            p3.Y = 7;

            p4.X = 7;
            p4.Y = 7;
            joints.AddFirst(p1);
            joints.AddFirst(p2);
            joints.AddFirst(p3);
            joints.AddFirst(p4);

            Snake testSnake = new Snake(joints, 1, "john");

            Assert.AreEqual(6, testSnake.Length);
        }

        [TestMethod]
        public void Snake_GetVerticies_CheckCount()
        {
            LinkedList<Point> joints = new LinkedList<Point>();
            Point p1 = new Point();
            Point p2 = new Point();
            Point p3 = new Point();
            Point p4 = new Point();
            p1.X = 5;
            p1.Y = 5;

            p2.X = 8;
            p2.Y = 5;

            p3.X = 8;
            p3.Y = 7;

            p4.X = 7;
            p4.Y = 7;
            joints.AddFirst(p1);
            joints.AddFirst(p2);
            joints.AddFirst(p3);
            joints.AddFirst(p4);

            Snake testSnake = new Snake(joints, 1, "john");

            Assert.AreEqual(4, testSnake.GetVerticies().Count);
        }

        [TestMethod]
        public void Snake_GetVerticies_Short()
        {
            LinkedList<Point> joints = new LinkedList<Point>();
            Point p1 = new Point();
            Point p2 = new Point();
            p1.X = 5;
            p1.Y = 5;
            p2.X = 6;
            p2.Y = 5;
            joints.AddFirst(p1);
            joints.AddFirst(p2);

            Snake testSnake = new Snake(joints, 1, "john");

            Assert.AreEqual(joints.Count, testSnake.GetVerticies().Count);
        }

        [TestMethod]
        public void Snake_GetSnakePoints_CheckCount()
        {
            LinkedList<Point> joints = new LinkedList<Point>();
            Point p1 = new Point();
            Point p2 = new Point();
            Point p3 = new Point();
            Point p4 = new Point();
            p1.X = 5;
            p1.Y = 5;

            p2.X = 8;
            p2.Y = 5;

            p3.X = 8;
            p3.Y = 7;

            p4.X = 7;
            p4.Y = 7;
            joints.AddFirst(p1);
            joints.AddFirst(p2);
            joints.AddFirst(p3);
            joints.AddFirst(p4);

            Snake testSnake = new Snake(joints, 1, "john");

            Assert.AreEqual(7, testSnake.GetSnakePoints().Count);
        }

        /*
         * World Tests
         */

    }
}
