using SpreadsheetUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace PS2GradingTests
{
    /// <summary>
    ///  This is a test class for DependencyGraphTest
    /// 
    ///  These tests should help guide you on your implementation.  Warning: you can not "test" yourself
    ///  into correctness.  Tests only show incorrectness.  That being said, a large test suite will go a long
    ///  way toward ensuring correctness.
    /// 
    ///  You are strongly encouraged to write additional tests as you think about the required
    ///  functionality of yoru library.
    /// 
    ///</summary>
    [TestClass()]
    public class DependencyGraphTest
    {
        // ************************** TESTS ON EMPTY DGs ************************* //

        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void EmptyTest1()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.AreEqual(0, t.Size);
        }

        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void EmptyTest2()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.IsFalse(t.HasDependees("a"));
        }

        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void EmptyTest3()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.IsFalse(t.HasDependents("a"));
        }

        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void EmptyTest4()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.IsFalse(t.GetDependees("a").GetEnumerator().MoveNext());
        }

        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void EmptyTest5()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.IsFalse(t.GetDependents("a").GetEnumerator().MoveNext());
        }

        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void EmptyTest6()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.AreEqual(0, t["a"]);
        }

        /// <summary>
        ///Removing from an empty DG shouldn't fail
        ///</summary>
        [TestMethod()]
        public void EmptyTest7()
        {
            DependencyGraph t = new DependencyGraph();
            t.RemoveDependency("a", "b");
            Assert.AreEqual(0, t.Size);
        }

        /// <summary>
        ///Adding an empty DG shouldn't fail
        ///</summary>
        [TestMethod()]
        public void EmptyTest8()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
        }

        /// <summary>
        ///Replace on an empty DG shouldn't fail
        ///</summary>
        [TestMethod()]
        public void EmptyTest9()
        {
            DependencyGraph t = new DependencyGraph();
            t.ReplaceDependents("a", new HashSet<string>());
            Assert.AreEqual(0, t.Size);
        }

        /// <summary>
        ///Replace on an empty DG shouldn't fail
        ///</summary>
        [TestMethod()]
        public void EmptyTest10()
        {
            DependencyGraph t = new DependencyGraph();
            t.ReplaceDependees("a", new HashSet<string>());
            Assert.AreEqual(0, t.Size);
        }


        /**************************** SIMPLE NON-EMPTY TESTS ****************************/

        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void NonEmptyTest1()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            Assert.AreEqual(2, t.Size);
        }

        /// <summary>
        ///Slight variant
        ///</summary>
        [TestMethod()]
        public void NonEmptyTest2()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "b");
            Assert.AreEqual(1, t.Size);
        }

        /// <summary>
        ///Nonempty graph should contain something
        ///</summary>
        [TestMethod()]
        public void NonEmptyTest3()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("d", "c");
            Assert.IsFalse(t.HasDependees("a"));
            Assert.IsTrue(t.HasDependees("b"));
            Assert.IsTrue(t.HasDependents("a"));
            Assert.IsTrue(t.HasDependees("c"));
        }

        /// <summary>
        ///Nonempty graph should contain something
        ///</summary>
        [TestMethod()]
        public void NonEmptyTest4()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("d", "c");
            HashSet<String> aDents = new HashSet<String>(t.GetDependents("a"));
            HashSet<String> bDents = new HashSet<String>(t.GetDependents("b"));
            HashSet<String> cDents = new HashSet<String>(t.GetDependents("c"));
            HashSet<String> dDents = new HashSet<String>(t.GetDependents("d"));
            HashSet<String> eDents = new HashSet<String>(t.GetDependents("e"));
            HashSet<String> aDees = new HashSet<String>(t.GetDependees("a"));
            HashSet<String> bDees = new HashSet<String>(t.GetDependees("b"));
            HashSet<String> cDees = new HashSet<String>(t.GetDependees("c"));
            HashSet<String> dDees = new HashSet<String>(t.GetDependees("d"));
            HashSet<String> eDees = new HashSet<String>(t.GetDependees("e"));
            Assert.IsTrue(aDents.Count == 2 && aDents.Contains("b") && aDents.Contains("c"));
            Assert.IsTrue(bDents.Count == 0);
            Assert.IsTrue(cDents.Count == 0);
            Assert.IsTrue(dDents.Count == 1 && dDents.Contains("c"));
            Assert.IsTrue(eDents.Count == 0);
            Assert.IsTrue(aDees.Count == 0);
            Assert.IsTrue(bDees.Count == 1 && bDees.Contains("a"));
            Assert.IsTrue(cDees.Count == 2 && cDees.Contains("a") && cDees.Contains("d"));
            Assert.IsTrue(dDees.Count == 0);
            Assert.IsTrue(dDees.Count == 0);
        }

        /// <summary>
        ///Nonempty graph should contain something
        ///</summary>
        [TestMethod()]
        public void NonEmptyTest5()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("d", "c");
            Assert.AreEqual(0, t["a"]);
            Assert.AreEqual(1, t["b"]);
            Assert.AreEqual(2, t["c"]);
            Assert.AreEqual(0, t["d"]);
            Assert.AreEqual(0, t["e"]);
        }

        /// <summary>
        ///Removing from a DG 
        ///</summary>
        [TestMethod()]
        public void NonEmptyTest6()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("d", "c");
            t.RemoveDependency("a", "b");
            Assert.AreEqual(2, t.Size);
        }

        /// <summary>
        ///Replace on a DG
        ///</summary>
        [TestMethod()]
        public void NonEmptyTest7()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("d", "c");
            t.ReplaceDependents("a", new HashSet<string>() { "x", "y", "z" });
            HashSet<String> aPends = new HashSet<string>(t.GetDependents("a"));
            Assert.IsTrue(aPends.SetEquals(new HashSet<string>() { "x", "y", "z" }));
        }

        /// <summary>
        ///Replace on a DG
        ///</summary>
        [TestMethod()]
        public void NonEmptyTest8()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("d", "c");
            t.ReplaceDependees("c", new HashSet<string>() { "x", "y", "z" });
            HashSet<String> cDees = new HashSet<string>(t.GetDependees("c"));
            Assert.IsTrue(cDees.SetEquals(new HashSet<string>() { "x", "y", "z" }));
        }

        // ************************** STRESS TESTS ******************************** //
        /// <summary>
        ///Using lots of data
        ///</summary>
        [TestMethod()]
        public void StressTest1()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();

            // A bunch of strings to use
            const int SIZE = 100;
            string[] letters = new string[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                letters[i] = ("" + (char)('a' + i));
            }

            // The correct answers
            HashSet<string>[] dents = new HashSet<string>[SIZE];
            HashSet<string>[] dees = new HashSet<string>[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                dents[i] = new HashSet<string>();
                dees[i] = new HashSet<string>();
            }

            // Add a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j++)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }

            // Remove a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 2; j < SIZE; j += 2)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }

            // Make sure everything is right
            for (int i = 0; i < SIZE; i++)
            {
                Assert.IsTrue(dents[i].SetEquals(new HashSet<string>(t.GetDependents(letters[i]))));
                Assert.IsTrue(dees[i].SetEquals(new HashSet<string>(t.GetDependees(letters[i]))));
            }
        }



        // ********************************** ANOTHER STESS TEST ******************** //
        /// <summary>
        ///Using lots of data with replacement
        ///</summary>
        [TestMethod()]
        public void StressTest8()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();

            // A bunch of strings to use
            const int SIZE = 100;
            string[] letters = new string[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                letters[i] = ("" + (char)('a' + i));
            }

            // The correct answers
            HashSet<string>[] dents = new HashSet<string>[SIZE];
            HashSet<string>[] dees = new HashSet<string>[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                dents[i] = new HashSet<string>();
                dees[i] = new HashSet<string>();
            }

            // Add a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j++)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }

            // Remove a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 2; j < SIZE; j += 2)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }

            // Replace a bunch of dependents
            for (int i = 0; i < SIZE; i += 4)
            {
                HashSet<string> newDents = new HashSet<String>();
                for (int j = 0; j < SIZE; j += 7)
                {
                    newDents.Add(letters[j]);
                }
                t.ReplaceDependents(letters[i], newDents);

                foreach (string s in dents[i])
                {
                    dees[s[0] - 'a'].Remove(letters[i]);
                }

                foreach (string s in newDents)
                {
                    dees[s[0] - 'a'].Add(letters[i]);
                }

                dents[i] = newDents;
            }

            // Make sure everything is right
            for (int i = 0; i < SIZE; i++)
            {
                Assert.IsTrue(dents[i].SetEquals(new HashSet<string>(t.GetDependents(letters[i]))));
                Assert.IsTrue(dees[i].SetEquals(new HashSet<string>(t.GetDependees(letters[i]))));
            }
        }

        // ********************************** A THIRD STESS TEST ******************** //
        /// <summary>
        ///Using lots of data with replacement
        ///</summary>
        [TestMethod()]
        public void StressTest15()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();

            // A bunch of strings to use
            const int SIZE = 100;
            string[] letters = new string[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                letters[i] = ("" + (char)('a' + i));
            }

            // The correct answers
            HashSet<string>[] dents = new HashSet<string>[SIZE];
            HashSet<string>[] dees = new HashSet<string>[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                dents[i] = new HashSet<string>();
                dees[i] = new HashSet<string>();
            }

            // Add a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j++)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }

            // Remove a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 2; j < SIZE; j += 2)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }

            // Replace a bunch of dependees
            for (int i = 0; i < SIZE; i += 4)
            {
                HashSet<string> newDees = new HashSet<String>();
                for (int j = 0; j < SIZE; j += 7)
                {
                    newDees.Add(letters[j]);
                }
                t.ReplaceDependees(letters[i], newDees);

                foreach (string s in dees[i])
                {
                    dents[s[0] - 'a'].Remove(letters[i]);
                }

                foreach (string s in newDees)
                {
                    dents[s[0] - 'a'].Add(letters[i]);
                }

                dees[i] = newDees;
            }

            // Make sure everything is right
            for (int i = 0; i < SIZE; i++)
            {
                Assert.IsTrue(dents[i].SetEquals(new HashSet<string>(t.GetDependents(letters[i]))));
                Assert.IsTrue(dees[i].SetEquals(new HashSet<string>(t.GetDependees(letters[i]))));
            }
        }

        /*
         * *************************************************************** *
         *                                                                 *
         * *************************** My Tests ************************** *
         *                                                                 *
         * *************************************************************** */

        /*
         * Size Tests
         */
        [TestMethod()]
        public void SizeEmptyGraph()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.AreEqual(0, t.Size);
        }

        [TestMethod()]
        public void SizeOneItem()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("A", "B");
            Assert.AreEqual(1, t.Size);
        }

        [TestMethod()]
        public void SizeSeveralItems()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("A", "B");
            t.AddDependency("B", "C");
            t.AddDependency("D", "E");
            t.AddDependency("E", "D");
            t.AddDependency("A", "A");
            Assert.AreEqual(5, t.Size);
        }

        [TestMethod()]
        public void SizeAddingAndRemoving()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("A", "B");
            t.AddDependency("B", "C");
            t.AddDependency("D", "E");
            t.AddDependency("E", "D");
            t.AddDependency("A", "A");
            t.RemoveDependency("D", "E");
            Assert.AreEqual(4, t.Size);
        }

        /*
         * Indexer Tests i.e. the t["a"] thing
         */
        [TestMethod()]
        public void IndexerNormalTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("A", "B");
            t.AddDependency("B", "A");
            t.AddDependency("D", "A");
            t.AddDependency("E", "D");
            t.AddDependency("A", "A");
            Assert.AreEqual(3, t["A"]);
        }

        [TestMethod()]
        public void IndexerNoDependees()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("A", "B");
            t.AddDependency("B", "A");
            t.AddDependency("D", "A");
            t.AddDependency("E", "D");
            t.AddDependency("C", "A");
            Assert.AreEqual(0, t["C"]);
        }

        [TestMethod()]
        public void IndexerNoNode()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("A", "B");
            t.AddDependency("B", "A");
            t.AddDependency("D", "A");
            t.AddDependency("E", "D");
            t.AddDependency("C", "A");
            Assert.AreEqual(0, t["Slagathor!"]);
        }

        /*
         * HasDependents Tests
         */
        [TestMethod()]
        public void HasDependentsNormalCase()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("A", "B");
            t.AddDependency("B", "C");
            t.AddDependency("A", "A");
            Assert.IsTrue(t.HasDependents("A"));
        }

        [TestMethod()]
        public void HasDependentsNoDependents()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("A", "B");
            t.AddDependency("B", "C");
            t.AddDependency("A", "A");
            Assert.IsFalse(t.HasDependents("C"));
        }

        [TestMethod()]
        public void HasDependentsNoNode()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("A", "B");
            t.AddDependency("B", "C");
            t.AddDependency("A", "A");
            Assert.IsFalse(t.HasDependents("D"));
        }

        [TestMethod()]
        public void HasDependentsEmptyGraph()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.IsFalse(t.HasDependents("D"));
        }


        /*
         * HasDependees Tests
         */
        [TestMethod()]
        public void HasDependeesNormalCase()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("A", "B");
            t.AddDependency("B", "C");
            t.AddDependency("A", "A");
            Assert.IsTrue(t.HasDependees("B"));
        }

        [TestMethod()]
        public void HasDependeesNoDependees()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("A", "B");
            t.AddDependency("B", "C");
            Assert.IsTrue(t.HasDependees("A"));
        }

        [TestMethod()]
        public void HasDependeesNoNode()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("A", "B");
            t.AddDependency("B", "C");
            Assert.IsTrue(t.HasDependees("D"));
        }

        [TestMethod()]
        public void HasDependeesEmptyGraph()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.IsTrue(t.HasDependees("D"));
        }

        /*
         * GetDependents Tests
         */
        [TestMethod()]
        public void GetDependentsNormalCase()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("A", "B");
            t.AddDependency("B", "C");
            t.AddDependency("C", "B");

            MyDict expected = new MyDict();
            expected.Add("C", "C");

            Assert.IsTrue(expected.Equals(t.GetDependents("B")));
        }

        [TestMethod()]
        public void GetDependentsNoDependents()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("A", "B");
            t.AddDependency("B", "C");
            t.AddDependency("B", "B");

            MyDict expected = new MyDict();

            Assert.IsTrue(expected.Equals(t.GetDependents("C")));
        }

        [TestMethod()]
        public void GetDependentsNoNode()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("A", "B");
            t.AddDependency("B", "C");
            t.AddDependency("B", "B");

            MyDict expected = new MyDict();

            Assert.IsTrue(expected.Equals(t.GetDependents("w25")));
        }

        [TestMethod()]
        public void GetDependentsReferenceToItself()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("A", "B");
            t.AddDependency("B", "C");
            t.AddDependency("B", "B");

            MyDict expected = new MyDict();
            expected.Add("C", "C");
            expected.Add("B", "B");

            Assert.IsTrue(expected.Equals(t.GetDependents("B")));
        }

        /*
         * GetDependees Test
         */
        [TestMethod()]
        public void GetDependeesNormalCase()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("A", "B");
            t.AddDependency("B", "C");
            t.AddDependency("C", "B");

            MyDict expected = new MyDict();
            expected.Add("C", "C");

            Assert.IsTrue(expected.Equals(t.GetDependees("B")));
        }

        [TestMethod()]
        public void GetDependeesNoDependees()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("A", "B");
            t.AddDependency("B", "C");
            t.AddDependency("B", "B");

            MyDict expected = new MyDict();

            Assert.IsTrue(expected.Equals(t.GetDependees("A")));
        }

        [TestMethod()]
        public void GetDependeesNoNode()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("A", "B");
            t.AddDependency("B", "C");
            t.AddDependency("B", "B");

            MyDict expected = new MyDict();

            Assert.IsTrue(expected.Equals(t.GetDependees("S")));
        }

        [TestMethod()]
        public void GetDependentsReferenceToItself()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("A", "B");
            t.AddDependency("B", "C");
            t.AddDependency("B", "B");

            MyDict expected = new MyDict();
            expected.Add("A", "A");
            expected.Add("B", "B");

            Assert.IsTrue(expected.Equals(t.GetDependents("B")));
        }

        /*
         * AddDependency Tests
         */
        [TestMethod()]
        public void AddDependencyEmptyCheckSize()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("A", "B");

            Assert.AreEqual(1, t.Size);
        }

        [TestMethod()]
        public void AddDependencyEmptyCheckDependents()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("A", "B");

            Assert.IsTrue(t.HasDependents("A"));
        }

        [TestMethod()]
        public void AddDependencyEmptyCheckDependees()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("A", "B");

            Assert.IsTrue(t.HasDependees("B"));
        }

        [TestMethod()]
        public void AddDependencyRedundantDependency()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("A", "B");
            t.AddDependency("A", "B");

            Assert.AreEqual(1, t.Size);
        }

        [TestMethod()]
        public void AddDependencyMultipleTimes()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("A", "B");
            t.AddDependency("B", "C");
            t.AddDependency("A", "C");
            t.AddDependency("D", "Q");
            t.AddDependency("F", "B");
            t.AddDependency("B", "C");

            Assert.AreEqual(6, t.Size);
        }

        [TestMethod()]
        public void AddDependencyDifferentStrings()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("123456789", "Base");
            t.AddDependency("/*-+~", "C");

            Assert.AreEqual(2, t.Size);
        }

        /*
         * RemoveDependency
         */
        [TestMethod()]
        public void RemoveDependencyNormalCheckDependees()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("A", "B");
            t.AddDependency("B", "A");
            t.AddDependency("D", "A");
            t.AddDependency("E", "D");
            t.AddDependency("C", "A");
            t.RemoveDependency("C","A");
            Assert.AreEqual(2, t["A"]);
        }

        [TestMethod()]
        public void RemoveDependencyNormalCheckSize()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("A", "B");
            t.AddDependency("B", "A");
            t.AddDependency("D", "A");
            t.AddDependency("E", "D");
            t.AddDependency("C", "A");
            t.RemoveDependency("C", "A");
            Assert.AreEqual(4, t.Size);
        }

        [TestMethod()]
        public void RemoveDependencyDNECheckSize()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("A", "B");
            t.AddDependency("B", "A");
            t.AddDependency("D", "A");
            t.AddDependency("E", "D");
            t.AddDependency("C", "A");
            t.RemoveDependency("df", "Franklin");
            Assert.AreEqual(5, t.Size);
        }

        [TestMethod()]
        public void RemoveDependencyDNECheckDependees()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("A", "B");
            t.AddDependency("B", "A");
            t.AddDependency("D", "A");
            t.AddDependency("E", "D");
            t.AddDependency("C", "A");
            t.RemoveDependency("C", "A");
            Assert.AreEqual(2, t["Escobar"]);
        }

        /*
         * ReplaceDependents
         */
        [TestMethod()]
        public void ReplaceDependentsNormalCheckSize()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("A", "B");
            t.AddDependency("A", "C");
            t.AddDependency("A", "A");

            List<String> newDep = new List<String>();
            newDep.Add("X");
            newDep.Add("Y");

            t.ReplaceDependents("A", newDep);

            Assert.AreEqual(2, t.Size);
        }

        [TestMethod()]
        public void ReplaceDependentsNormalCheckdependees()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("A", "B");
            t.AddDependency("A", "C");
            t.AddDependency("A", "A");

            List<String> newDep = new List<String>();
            newDep.Add("X");
            newDep.Add("Y");

            t.ReplaceDependents("A", newDep);

            Assert.AreEqual(1, t["X"]);
        }

        [TestMethod()]
        public void ReplaceDependentsNoOrigDepsCheckdependees()
        {
            DependencyGraph t = new DependencyGraph();

            List<String> newDep = new List<String>();
            newDep.Add("X");
            newDep.Add("Y");

            t.ReplaceDependents("A", newDep);

            Assert.AreEqual(1, t["X"]);
        }

        [TestMethod()]
        public void ReplaceDependentsNoOrigDepsCheckSize()
        {
            DependencyGraph t = new DependencyGraph();

            List<String> newDep = new List<String>();
            newDep.Add("X");
            newDep.Add("Y");

            t.ReplaceDependents("A", newDep);

            Assert.AreEqual(2, t.Size);
        }

        [TestMethod()]
        public void ReplaceDependentsNoNewDepsCheckSize()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("A", "B");
            t.AddDependency("A", "C");
            t.AddDependency("A", "A");

            List<String> newDep = new List<String>();

            t.ReplaceDependents("A", newDep);

            Assert.AreEqual(0, t.Size);
        }

        [TestMethod()]
        public void ReplaceDependentsNoNewDepsCheckdependees()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("A", "B");
            t.AddDependency("A", "C");
            t.AddDependency("A", "A");

            List<String> newDep = new List<String>();

            t.ReplaceDependents("A", newDep);

            Assert.AreEqual(0, t["X"]);
        }

        [TestMethod()]
        public void ReplaceDependentsNoNodeCheckSize()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("A", "B");
            t.AddDependency("A", "C");
            t.AddDependency("A", "A");

            List<String> newDep = new List<String>();
            newDep.Add("X");
            newDep.Add("Y");

            t.ReplaceDependents("Thirteen", newDep);

            Assert.AreEqual(5, t.Size);
        }

        [TestMethod()]
        public void ReplaceDependentsNoNodeCheckDependees()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("A", "B");
            t.AddDependency("A", "C");
            t.AddDependency("A", "A");

            List<String> newDep = new List<String>();
            newDep.Add("X");
            newDep.Add("Y");

            t.ReplaceDependents("Thirteen", newDep);

            Assert.AreEqual(1, t["Y"]);
        }

        /*
         * ReplaceDependees
         */
        [TestMethod()]
        public void ReplaceDependeesNormalCheckSize()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("A", "B");
            t.AddDependency("A", "C");
            t.AddDependency("A", "A");

            List<String> newDep = new List<String>();
            newDep.Add("X");
            newDep.Add("Y");

            t.ReplaceDependees("A", newDep);

            Assert.AreEqual(4, t.Size);
        }

        [TestMethod()]
        public void ReplaceDependeesNormalCheckdependees()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("A", "B");
            t.AddDependency("A", "C");
            t.AddDependency("A", "A");

            List<String> newDep = new List<String>();
            newDep.Add("X");
            newDep.Add("Y");

            t.ReplaceDependees("A", newDep);

            Assert.AreEqual(2, t["A"]);
        }

        [TestMethod()]
        public void ReplaceDependeesNoOrigDepsCheckdependees()
        {
            DependencyGraph t = new DependencyGraph();

            List<String> newDep = new List<String>();
            newDep.Add("X");
            newDep.Add("Y");

            t.ReplaceDependees("A", newDep);

            Assert.AreEqual(2, t["A"]);
        }

        [TestMethod()]
        public void ReplaceDependeesNoOrigDepsCheckSize()
        {
            DependencyGraph t = new DependencyGraph();

            List<String> newDep = new List<String>();
            newDep.Add("X");
            newDep.Add("Y");

            t.ReplaceDependees("A", newDep);

            Assert.AreEqual(2, t.Size);
        }

        [TestMethod()]
        public void ReplaceDependeesNoNewDepsCheckSize()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("A", "B");
            t.AddDependency("A", "C");
            t.AddDependency("A", "A");

            List<String> newDep = new List<String>();

            t.ReplaceDependees("A", newDep);

            Assert.AreEqual(2, t.Size);
        }

        [TestMethod()]
        public void ReplaceDependeesNoNewDepsCheckdependees()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("A", "B");
            t.AddDependency("A", "C");
            t.AddDependency("A", "A");

            List<String> newDep = new List<String>();

            t.ReplaceDependees("A", newDep);

            Assert.AreEqual(0, t["A"]);
        }

        [TestMethod()]
        public void ReplaceDependeesNoNodeCheckSize()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("A", "B");
            t.AddDependency("A", "C");
            t.AddDependency("A", "A");

            List<String> newDep = new List<String>();
            newDep.Add("X");
            newDep.Add("Y");

            t.ReplaceDependees("Thirteen", newDep);

            Assert.AreEqual(5, t.Size);
        }

        [TestMethod()]
        public void ReplaceDependeesNoNodeCheckDependees()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("A", "B");
            t.AddDependency("A", "C");
            t.AddDependency("A", "A");

            List<String> newDep = new List<String>();
            newDep.Add("X");
            newDep.Add("Y");

            t.ReplaceDependees("Thirteen", newDep);

            Assert.AreEqual(2, t["Thirteen"]);
        }
    }

    /// <summary>
    /// A class that extends Dictionary so that it is easy to check if the output dictionary from DependencyGraph is valid.
    /// I just added an Equals method so check if the values of the dictionaries are equal.
    /// </summary>
    public class MyDict : Dictionary<String, String> {

        /// <summary>
        /// A Method to check if this dictionary has the same elements as the given dictionary.
        /// </summary>
        /// <param name="d">a Dictionary object to be checked if it is the same as this one</param>
        /// <returns>true if they contains the same elements</returns>
        public bool Equals(Dictionary<String, String> d) 
        {
            // Make sure they are the same size
            if (this.Count != d.Count)
            {
                return false;
            }

            // Go through checking to make sure each element in d is in this
            foreach (KeyValuePair<String, String>  current in d)
            {
                if (!this.ContainsKey(current.Key))
                {
                    return false;
                }
            }

            // Check to make sure every element in this is in d
            foreach (KeyValuePair<String, String>  current in this)
            {
                if (!d.ContainsKey(current.Key))
                {
                    return false;
                }
            }

            return true;
        }
    }
}