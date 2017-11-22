using AcoTsp;
using AcoTsp.Model;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

// Demo of Ant Colony Optimization (ACO) solving a Traveling Salesman Problem (TSP).
// There are many variations of ACO; this is just one approach.
// The problem to solve has a program defined number of cities. We assume that every
// city is connected to every other city.
// Free parameters are alpha, beta, rho, and Q. 


namespace AntColonyOptimization
{
    internal class AntColonyProgram
    {
        public static void Main(string[] args)
        {

            #region MyRegion
            /*            new Node("f", 0, 9),
                            new Node("g", 1, 4),
                            new Node("h", 1, 6),
                            new Node("i", 3, 4),
                            new Node("j", 8, 4),
                            new Node("k", 9, 4),
                            new Node("l", 4, 4)

                                 */

            #endregion

            var xxxx = new List<Node>
            {
                new Node("A", 0, 0),
                new Node("B", 1, 1),
                new Node("C", 2, 2),
                new Node("D", 2, 9),
                new Node("E", 4, 5),
                new Node("F", 8, 1),

            };

            #region mockList                 

            //var A = new Node("A", 1, 1);
            //var B = new Node("B", 1, 1);
            //var C = new Node("C", 1, 1);
            //var D = new Node("D", 1, 1);
            //var E = new Node("E", 1, 1);

            //A.Routes = new Dictionary<Node, Route>()
            //{
            //    {A, new Route(0)},
            //    {B, new Route(10)},
            //    {C, new Route(12)},
            //    {D, new Route(11)},
            //    {E, new Route(14)},
            //};

            //B.Routes = new Dictionary<Node, Route>()
            //{
            //    {A, new Route(10)},
            //    {B, new Route(0)},
            //    {C, new Route(13)},
            //    {D, new Route(15)},
            //    {E, new Route(18)},
            //};

            //C.Routes = new Dictionary<Node, Route>()
            //{
            //    {A, new Route(12)},
            //    {B, new Route(13)},
            //    {C, new Route(0)},
            //    {D, new Route(9)},
            //    {E, new Route(14)},
            //};

            //D.Routes = new Dictionary<Node, Route>()
            //{
            //    {A, new Route(11)},
            //    {B, new Route(15)},
            //    {C, new Route(9)},
            //    {D, new Route(0)},
            //    {E, new Route(16)},
            //};

            //E.Routes = new Dictionary<Node, Route>()
            //{
            //    {A, new Route(14)},
            //    {B, new Route(8)},
            //    {C, new Route(14)},
            //    {D, new Route(16)},
            //    {E, new Route(0)},
            //};

            //var mockNodes = new List<Node> {A, B, C, D, E};

            #endregion

            //TspSolver
            //    .AntColonyOptimization()
            //    .Default()
            //    .WithCities(xxxx)
            //    .StartFrom(1)
            //    .WithAnts(6)
            //    .WithMaximumTime(1000)
            //    .Solve();

            TspSolver
                .AntColonyOptimization()
                .Custom()
                .WithInfluenceOfPheromoneOnDrection(2)
                .WithInfluenceOfAdjacentNodeDistance(3)
                .WithPheromoneDecreaseFactor(0.1)
                .WithPheromoneIncreaseFactor(3)
                .WithCities(xxxx/*@"C:\Users\dariusz.dabrowski\Desktop\Ceneo\Nodes.txt"*/)
                .StartFromRandomPosition()
                .WithAnts(2)
                .WithMaximumTime(10000)
                .Solve();



            Console.ReadKey();
        }
    }
}
// ns
