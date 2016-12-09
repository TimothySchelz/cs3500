// Created by Gray Marchese, u0884194, and Timothy Schelz, u0851027
// Last Date Updated: 11/22/16
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
            List<Point> joints = new List<Point>();
            Point p1 = new Point();
            Point p2 = new Point();
            p1.X = 5;
            p1.Y = 5;
            p2.X = 15;
            p2.Y = 5;
            joints.Add(p1);
            joints.Add(p2);

            Snake testSnake = new Snake(joints, 1, "john");

            Assert.AreEqual(10, testSnake.GetLength());
        }

        [TestMethod]
        public void Snake_Length_ShortSegment()
        {
            List<Point> joints = new List<Point>();
            Point p1 = new Point();
            Point p2 = new Point();
            p1.X = 5;
            p1.Y = 5;
            p2.X = 6;
            p2.Y = 5;
            joints.Add(p1);
            joints.Add(p2);

            Snake testSnake = new Snake(joints, 1, "john");

            Assert.AreEqual(1, testSnake.GetLength());
        }

        [TestMethod]
        public void Snake_Length_3Segments()
        {
            List<Point> joints = new List<Point>();
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
            joints.Add(p1);
            joints.Add(p2);
            joints.Add(p3);
            joints.Add(p4);

            Snake testSnake = new Snake(joints, 1, "john");

            Assert.AreEqual(6, testSnake.GetLength());
        }

        [TestMethod]
        public void Snake_GetVerticies_CheckCount()
        {
            List<Point> joints = new List<Point>();
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
            joints.Add(p1);
            joints.Add(p2);
            joints.Add(p3);
            joints.Add(p4);

            Snake testSnake = new Snake(joints, 1, "john");

            Assert.AreEqual(4, testSnake.GetVerticies().Count);
        }

        [TestMethod]
        public void Snake_GetVerticies_Short()
        {
            List<Point> joints = new List<Point>();
            Point p1 = new Point();
            Point p2 = new Point();
            p1.X = 5;
            p1.Y = 5;
            p2.X = 6;
            p2.Y = 5;
            joints.Add(p1);
            joints.Add(p2);

            Snake testSnake = new Snake(joints, 1, "john");

            Assert.AreEqual(joints.Count, testSnake.GetVerticies().Count);
        }

        [TestMethod]
        public void Snake_GetSnakePoints_CheckCount()
        {
            List<Point> joints = new List<Point>();
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
            joints.Add(p1);
            joints.Add(p2);
            joints.Add(p3);
            joints.Add(p4);

            Snake testSnake = new Snake(joints, 1, "john");

            Assert.AreEqual(7, testSnake.GetSnakePoints().Count);
        }

        /*
         * World Tests
         */
        [TestMethod]
        public void World_Constructor_TestWidth()
        {
            World testWorld = new World(1, 10, 100);

            Assert.AreEqual(10, testWorld.Width);
        }

        [TestMethod]
        public void World_Constructor_TestHeight()
        {
            World testWorld = new World(1, 10, 100);

            Assert.AreEqual(10, testWorld.Height);
        }

        [TestMethod]
        public void World_GetSnakes_Empty()
        {
            World testWorld = new World(1, 10, 100);

            Assert.AreEqual(0, testWorld.GetSnakes().Count);
        }

        [TestMethod]
        public void World_GetSnakes_AFew()
        {
            World testWorld = new World(1, 100, 100);

            testWorld.updateSnake(RandomSingleSegment(100));
            testWorld.updateSnake(RandomSingleSegment(100));
            testWorld.updateSnake(RandomSingleSegment(100));
            testWorld.updateSnake(RandomSingleSegment(100));

            Assert.AreEqual(4, testWorld.GetSnakes().Count);
        }

        [TestMethod]
        public void World_GetSnakes_SnakeName()
        {
            World testWorld = new World(1, 100, 100);

            testWorld.updateSnake(RandomSingleSegment(100));
            testWorld.updateSnake(RandomSingleSegment(100));
            testWorld.updateSnake(RandomSingleSegment(100));
            testWorld.updateSnake(RandomSingleSegment(100));

            foreach(Snake snake in testWorld.GetSnakes())
            {
                Assert.AreEqual("Seymour", snake.name);
            }      
        }

        [TestMethod]
        public void World_GetFood_Empty()
        {
            World testWorld = new World(1, 100, 100);

            Assert.AreEqual(0, testWorld.GetFood().Count);
        }

        [TestMethod]
        public void World_GetFood_AFew()
        {
            World testWorld = new World(1, 100, 100);

            testWorld.updateFood(RandomFood(100));
            testWorld.updateFood(RandomFood(100));
            testWorld.updateFood(RandomFood(100));
            testWorld.updateFood(RandomFood(100));


            Assert.AreEqual(4, testWorld.GetFood().Count);
        }

        [TestMethod]
        public void World_GenerateFood_AFew()
        {
            World testWorld = new World(1, 100, 100);

            testWorld.generateFood();
            testWorld.generateFood();
            testWorld.generateFood();
            testWorld.generateFood();

            Assert.AreEqual(4, testWorld.GetFood().Count);
        }

        [TestMethod]
        public void World_GenerateFood_DefinedSpots()
        {
            World testWorld = new World(1, 100, 100);

            testWorld.generateFood(5, 5);
            testWorld.generateFood(5, 6);
            testWorld.generateFood(6, 5);
            testWorld.generateFood(6, 6);

            Assert.AreEqual(4, testWorld.GetFood().Count);
            foreach (Food food in testWorld.GetFood())
            {
                Assert.IsTrue((food.loc.X == 5 || food.loc.X == 6) && (food.loc.X == 6 || food.loc.X == 6));
            }
        }

        [TestMethod]
        public void World_GetSnakeColor_CheckColorConsistency()
        {
            World testWorld = new World(1, 100, 100);

            testWorld.updateSnake(RandomSingleSegment(100));

            int ID = 0;
            foreach(Snake snake in testWorld.GetSnakes())
            {
                ID = snake.ID;
            }

            System.Drawing.Color snakeColor = testWorld.GetSnakeColor(ID);

            testWorld.updateSnake(RandomSingleSegment(100));
            testWorld.updateSnake(RandomSingleSegment(100));
            testWorld.updateSnake(RandomSingleSegment(100));
            testWorld.updateSnake(RandomSingleSegment(100));

            Assert.AreEqual(snakeColor, testWorld.GetSnakeColor(ID));
        }

        [TestMethod]
        public void World_GetSnakeColor_NoSnakes()
        {
            World testWorld = new World(1, 100, 100);

            Assert.AreEqual(System.Drawing.Color.Black, testWorld.GetSnakeColor(1729));
        }

        [TestMethod]
        public void World_createSnake_SingleCreate()
        {
            World testWorld = new World(1, 100, 100);

            testWorld.createSnake(5, "Simon");


            Assert.AreEqual(1, testWorld.GetSnakes().Count);
            foreach (Snake snake in testWorld.GetSnakes())
            {
                Assert.AreEqual("Simon", snake.name);
            }

        }

        [TestMethod]
        public void World_createSnake_AFew()
        {
            World testWorld = new World(1, 100, 100);

            testWorld.createSnake(5, "Simon");
            testWorld.createSnake(10, "Salazar");
            testWorld.createSnake(10, "Satan");
            testWorld.createSnake(10, "Sean");

            Assert.AreEqual(4, testWorld.GetSnakes().Count);
        }

        [TestMethod]
        public void Snake_KillMe()
        {
            List<Point> joints = new List<Point>();
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
            joints.Add(p1);
            joints.Add(p2);
            joints.Add(p3);
            joints.Add(p4);

            Snake doomedSnake = new Snake(joints, 1, "john");

            doomedSnake.KillMe();

            LinkedList<Point> body = doomedSnake.GetVerticies();

            if (body.Count != 2)
                Assert.Fail("Dead snake doesn't have 1 vertices");

            foreach(Point vertex in body)
            {
                if (vertex.X != -1 || vertex.Y != -1)
                    Assert.Fail("Body point was not set to (-1,-1)");
            }
        }

        //Method called in World, but its guts are really in snake.
        [TestMethod]
        public void Snake_ChageDirection()
        {
            World testWorld = new World(1, 100, 100);

            testWorld.createSnake(5, "Simon");

            Snake Simon = findSnakeInWorld(testWorld, 5);

            Simon.Direction = 1;
            Simon.PrevDirection = 1;

            if (Simon.Direction != 1)
                Assert.Fail("Snake direction not set to 1");

            Simon.Direction = 2;

            if (Simon.Direction != 2)
                Assert.Fail("Snake direction not changed to 2");

            Simon.PrevDirection = 2;
            Simon.Direction = 4;

            if (Simon.Direction == 4)
                Assert.Fail("Direction should not have been changed from right to left.");

        }

        [TestMethod]
        public void World_Snake_MovesHeadForward()
        {

            World testWorld = new World(100,100,5,10,10,0.3);

            List<Point> joints = new List<Point>();
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
            joints.Add(p1);
            joints.Add(p2);
            joints.Add(p3);
            joints.Add(p4);

            Snake john = new Snake(joints, 1, "john");

            john.Direction = 4;
            john.PrevDirection = 4;

            testWorld.updateSnake(john);

            testWorld.UpdateWorld();

            // (7,7) -> (6,7)
            Assert.AreEqual(6, john.GetHead().X);


            john.Direction = 3;
            john.PrevDirection = 4;
            testWorld.UpdateWorld();
            
            // (6,7) -> (6,8)
            Assert.AreEqual(8, john.GetHead().Y);

            john.Direction = 2;
            john.PrevDirection = 3;
            testWorld.UpdateWorld();

            // (6,8) -> (7,8)
            Assert.AreEqual(7, john.GetHead().X);


        }

        [TestMethod]
        public void World_Snake_MovesTailForward()
        {

            World testWorld = new World(100, 100, 5, 10, 10, 0.3);

            List<Point> joints = new List<Point>();
            Point p1 = new Point();
            Point p2 = new Point();
            Point p3 = new Point();
            Point p4 = new Point();
            p1.X = 7;
            p1.Y = 5;

            p2.X = 8;
            p2.Y = 5;

            p3.X = 8;
            p3.Y = 7;

            p4.X = 7;
            p4.Y = 7;
            joints.Add(p1);
            joints.Add(p2);
            joints.Add(p3);
            joints.Add(p4);

            Snake john = new Snake(joints, 1, "john");

            john.Direction = 4;
            john.PrevDirection = 4;

            testWorld.updateSnake(john);

            testWorld.UpdateWorld();

            Point tail = john.GetVerticies().First.Value;

            // (7,5) -> (8,5)
            Assert.AreEqual(8, tail.X);


            john.Direction = 3;
            john.PrevDirection = 4;
            testWorld.UpdateWorld();

            // (8,5) -> (8,6)
            Assert.AreEqual(6, tail.Y);

            john.Direction = 2;
            john.PrevDirection = 3;
            testWorld.UpdateWorld();

            // (8,6) -> (8,7)
            Assert.AreEqual(7, tail.Y);
        }


        [TestMethod]
        public void World_DefaultSnakeLength()
        {

            World testWorld = new World(100, 100, 5, 10, 12, 0.3);


            testWorld.createSnake(1, "Sarah");

            int Length = findSnakeInWorld(testWorld, 1).GetLength();

            Assert.AreEqual(12, Length);
        }

        [TestMethod]
        public void World_Snake_Eats_Food()
        {
            World testWorld = new World(100, 100, 5, 10, 10, 0.3);

            List<Point> joints = new List<Point>();
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

            joints.Add(p1);
            joints.Add(p2);
            joints.Add(p3);
            joints.Add(p4);

            Snake john = new Snake(joints, 1, "john");

            john.Direction = 4;
            john.PrevDirection = 4;

            int originalLength = john.GetVerticies().Count;

            testWorld.updateSnake(john);

            // Set a peice of food at 6,7
            testWorld.generateFood(6, 7);

            testWorld.UpdateWorld();

            Assert.AreEqual(originalLength+1, john.GetVerticies().Count);
        }




        /*
         * Helper methods to make testing easier... It might be better for some of these to be in classes
         */

        /// <summary>
        /// Returns a piece of food at a random location
        /// </summary>
        /// <param name="max">The lower of height and width</param>
        /// <returns></returns>
        public Food RandomFood(int max)
        {
            Point p1 = new Point();
            Random rando = new Random(123456);

            p1.X = rando.Next(max);
            p1.Y = rando.Next(max);

            int ID = p1.X.GetHashCode()*37 + p1.Y.GetHashCode()*157;
            return new Food(ID, p1);
        }

        /// <summary>
        /// Searches the world for a snake with the given ID. Returns the snake if it is found.
        /// returns null otherwise.
        /// </summary>
        /// <param name="testWorld">World to be searched for the snake</param>
        /// <param name="v">ID of the desired snake.</param>
        /// <returns></returns>
        private Snake findSnakeInWorld(World testWorld, int ID)
        {
            foreach (Snake snake in testWorld.GetSnakes())
            {
                if (snake.ID == ID)
                    return snake;
            }

            return null;
        }



        /// <summary>
        /// returns a snake with the given head and tail x,y values.  It is named Sebastion
        /// </summary>
        /// <param name="HX">Head X value</param>
        /// <param name="HY">Head Y value</param>
        /// <param name="TX">Tail X value</param>
        /// <param name="TY">Tail Y value</param>
        /// <returns></returns>
        public Snake SingleSegmentSnake(int HX, int HY, int TX, int TY)
        {

            Point p1 = new Point();
            Point p2 = new Point();

            p1.X = HX;
            p1.Y = HY;
            p2.X = TX;
            p2.Y = TY;

            int ID = (HX.GetHashCode() * 37 + HY.GetHashCode() * 67 + TX.GetHashCode() * 79 + TY.GetHashCode() * 19);
            return new Snake(new List<Point>() { p2, p1 }, ID, "Sebastion");
        }

        /// <summary>
        /// Returns a random horizontal snake. It is named Seymour
        /// </summary>
        /// <param name="max">the maximum x and y value to use</param>
        /// <returns>The random snake</returns>
        public Snake RandomSingleSegment(int max)
        {
            Point p1 = new Point();
            Point p2 = new Point();

            Random rando = new Random(1729);

            int HX = rando.Next(max);
            int HY = rando.Next(max);
            int TX = HX;
            int TY = rando.Next(max);

            p1.X = HX;
            p1.Y = HY;
            p2.X = TX;
            p2.Y = TY;

            int ID = (HX.GetHashCode() * 37 + HY.GetHashCode() * 67 + TX.GetHashCode() * 79 + TY.GetHashCode() * 19);

            return new Snake(new List<Point>() { p2, p1 }, ID, "Seymour");
        }
    }
}
