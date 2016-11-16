using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SnakeModel;

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
            Food testFood = new Food(24, new Point());

            Assert.AreEqual(null, testFood.loc);
        }

        /*
         * Snake Tests
         */
        [TestMethod]
        public void Snake_()
        {
            throw new NotImplementedException();
        }

        /*
         * World Tests
         */

    }
}
