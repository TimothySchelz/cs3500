// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpreadsheetUtilities
{

    /// <summary>
    /// (s1,t1) is an ordered pair of strings
    /// t1 depends on s1; s1 must be evaluated before t1
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
    /// set, and the element is already in the set, the set remains unchanged.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
    ///        (The set of things that depend on s)    
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
    ///        (The set of things that s depends on) 
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph
    {
        //This holds all those tasty little nodes
        Dictionary<String, Node> map;

        private int size;

        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
            map = new Dictionary<string, Node>();
            size = 0;
        }

        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get { return size; }
        }


        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        public int this[string s]
        {
            get
            {
                if(!map.ContainsKey(s))
                {
                    return 0;
                }
                return map[s].DeesSize;
            }
        }


        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)
        {
            if (map.ContainsKey(s))
            {
                return map[s].DentsSize != 0;
            }

            return false;
        }


        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s)
        {
            if (map.ContainsKey(s))
            {
                return map[s].DeesSize != 0;
            }

            return false;
        }


        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if (!map.ContainsKey(s))
            {
                return new HashSet<String>();
            }
            return map[s].Dents;
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (!map.ContainsKey(s))
            {
                return new HashSet<String>();
            }
            return map[s].Dees;
        }


        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        /// 
        /// <para>This should be thought of as:</para>   
        /// 
        ///   t depends on s
        ///
        /// </summary>
        /// <param name="s"> s must be evaluated first. T depends on S</param>
        /// <param name="t"> t cannot be evaluated until s is</param>        /// 
        public void AddDependency(string s, string t)
        {
            // Keeps track of if we are actually adding a dependency or if it alrteady existed
            bool addedDependency = false;

            //Checks if s already exists.  If it does simply add the new dependent, if it doesn't then create it and add the dependent.
            if (map.ContainsKey(s))
            {
                addedDependency = map[s].AddDent(t);
            }
            else
            {
                Node newbie = new Node(s);
                addedDependency = newbie.AddDent(t);
                map.Add(s, newbie);
            }

            //Checks if t already exists.  If it does simply add the new dependee, if it doesn't then create it and add the dependee.
            if (map.ContainsKey(t))
            {
                map[t].AddDee(s);
            }
            else
            {
                Node newbie = new Node(t);
                newbie.AddDee(s);
                map.Add(t, newbie);
            }

            // If a dependency has been added increment size.
            if (addedDependency)
            {
                size++;
            }
        }


        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public void RemoveDependency(string s, string t)
        {
            // Keeps track of if we are actually adding a dependency or if it alrteady existed
            bool removedDependency = false;

            //Checks if s already exists.  If it does simply remove the dependent, if it doesn't then you are done.
            if (map.ContainsKey(s))
            {
                removedDependency = map[s].RemoveDent(t);
            }
            else
            {
                return;
            }

            //Checks if t already exists.  If it does simply remove the dependee, if it doesn't then you are done.
            if (map.ContainsKey(t))
            {
                map[t].RemoveDee(s);
            }
            else
            {
                return;
            }

            // If a dependency has been removed decrement size.
            if (removedDependency)
            {
                size--;
            }
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            // check if s exists or if we need to create it
            if (map.ContainsKey(s))
            {
                //pulls the s node out so we don't have to repeatedly look it up and gets it's dependents
                Node S = map[s];
                HashSet<String> dependents = S.Dents;
                
                // remove old dependencies

                // remove S's collection of dependents
                S.EraseDents();
                // go to each of S's dependents and remove s from their dependees
                foreach (String i in dependents)
                {
                    // If something is actually removed decrement size
                    if (map[i].RemoveDee(s))
                    {
                        size--;
                    }
                }

                // add new dependencies
                foreach (String i in newDependents)
                {
                    // Add the current new dependent
                    S.AddDent(i);

                    //increment size
                    size++;

                    //check if the new dependent exists and then add s to it's list of dependees
                    if (map.ContainsKey(i))
                    {
                        map[i].AddDee(s);
                    }
                    else
                    {
                        Node newbie = new Node(i);
                        newbie.AddDee(s);
                        map.Add(i, newbie);
                    }
                }
            }
            else
            {
                // create s
                Node S = new Node(s);

                foreach (String i in newDependents)
                {
                    // add the dependents to s's list of dependents
                    S.AddDent(i);

                    // increment size
                    size++;

                    // if the current dependent exists add s to it's list of dependees otherwise create it
                    if (map.ContainsKey(i))
                    {
                        map[i].AddDee(s);
                    }
                    else
                    {
                        Node newbie = new Node(i);
                        newbie.AddDee(s);
                        map.Add(i, newbie);
                    }
                }

                // Add s to the map of nodes
                map.Add(s, S);
            }
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            // check if s exists or if we need to create it
            if (map.ContainsKey(s))
            {
                //pulls the s node out so we don't have to repeatedly look it up and gets it's dependees
                Node S = map[s];
                HashSet<String> dependees = S.Dees;

                // remove old dependencies

                // remove S's collection of dependees
                S.EraseDees();
                // go to each of S's dependents and remove s from their dependents
                foreach (String i in dependees)
                {
                    // If something is actually removed decrement size
                    if (map[i].RemoveDent(s))
                    {
                        size--;
                    }
                }

                // add new dependencies
                foreach (String i in newDependees)
                {
                    // Add the current new dependee
                    S.AddDee(i);

                    //increment size
                    size++;

                    //check if the new dependee exists and then add s to it's list of dependents
                    if (map.ContainsKey(i))
                    {
                        map[i].AddDent(s);
                    }
                    else
                    {
                        Node newbie = new Node(i);
                        newbie.AddDent(s);
                        map.Add(i, newbie);
                    }
                }
            }
            else
            {
                // create s
                Node S = new Node(s);

                foreach (String i in newDependees)
                {
                    // add the dependees to s's list of dependees
                    S.AddDee(i);

                    // increment size
                    size++;

                    // if the current dependee exists add s to it's list of dependents otherwise create it
                    if (map.ContainsKey(i))
                    {
                        map[i].AddDent(s);
                    }
                    else
                    {
                        Node newbie = new Node(i);
                        newbie.AddDent(s);
                        map.Add(i, newbie);
                    }
                }

                // Add s to the map of nodes
                map.Add(s, S);
            }
        }

    }

    /// <summary>
    /// A Node to represent the individual cells and remember what it depends on and what depends on it.  Basically the nodes of a directed graph.
    /// </summary>
    internal class Node
    {
        /// <summary>
        /// A constructor.  The node needs a name but that is it.
        /// </summary>
        /// <param name="name">The name of the node</param>
        internal Node(String name)
        {
            this.name = name;
            Dents = new HashSet<string>();
            Dees = new HashSet<string>();
        }

        /// <summary>
        /// The name of the node.
        /// </summary>
        internal String name
        {
            get;

            private set;
        }

        /// <summary>
        /// The size of the dependents of this node
        /// </summary>
        internal int DentsSize
        {
            get;

            private set;
        }

        /// <summary>
        /// The size of the dependees of this node
        /// </summary>
        internal int DeesSize
        {
            get;

            private set;
        }

        /// <summary>
        /// A Hashset with the dependents of this node.
        /// </summary>
        internal HashSet<String> Dents
        {
            get;

            private set;
        }

        /// <summary>
        /// A Hashset with the dependees of this node
        /// </summary>
        internal HashSet<String> Dees
        {
            get;

            private set;
        }

        /// <summary>
        /// Adds the string to the dependents
        /// </summary>
        /// <param name="s">The string to be added to the dependents</param>
        internal bool AddDent(String s)
        {
            bool success = Dents.Add(s);
            DentsSize = Dents.Count;
            return success;
        }

        /// <summary>
        /// Adds the string to the dependees
        /// </summary>
        /// <param name="s">The string to be added to the dependees</param>
        internal bool AddDee(String s)
        {
            bool success = Dees.Add(s);
            DeesSize = Dees.Count;
            return success;
        }

        /// <summary>
        /// Removes the string s from the dependents.  If s is not in dependents it does nothing.
        /// </summary>
        /// <param name="s">The string to be removed</param>
        internal bool RemoveDent(String s)
        {
            bool success = Dents.Remove(s);
            DentsSize = Dents.Count;
            return success;
        }

        /// <summary>
        /// Removes the string s from the dependees.  If s is not in dependees it does nothing.
        /// </summary>
        /// <param name="s">The string to be removed</param>
        internal bool RemoveDee(String s)
        {
            bool success = Dees.Remove(s);
            DeesSize = Dees.Count;
            return success;
        }

        /// <summary>
        /// Removes all the dependents of this node
        /// </summary>
        internal void EraseDents()
        {
            Dents = new HashSet<string>();
            DentsSize = 0;
        }

        /// <summary>
        /// Removes all the dependees of this node
        /// </summary>
        internal void EraseDees()
        {
            Dees = new HashSet<string>();
            DeesSize = 0;
        }
    }
}