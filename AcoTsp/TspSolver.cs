using AcoTsp.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AcoTsp
{
    public class TspSolver : ITspSolver ,IStartPositionHolder,  IAlphaHolder, IBetaHolder, IAntHolder, ICitiesHolder, ISelectorHolder, IQHolder, IRhoHolder,  IIterationNubmerHolder
    {
        private int _alpha = 1;     // influence of pheromone on direction		
        private int _beta = 2;      // influence of adjacent node distance		
        private double _rho = 0.01; // pheromone decrease factor		
        private double _q = 2.0;    // pheromone increase factor
        private int _times;         // max number of iterations 
        private List<Node> Nodes { get; set; }
        private List<Ant> Ants { get; set; }
        private Ant BestAnt => GetBestAnt;
        private double BestLength { get; set; } = Double.MaxValue;
        private IEnumerable<Node> BestTrail => GetBestTrail;
        private int _startFrom;
        private TspSolver() { }
        private TspSolver(IEnumerable<Node> nodes)
        {
            this.Nodes = new List<Node>(nodes);
            CalculateDistance();
        }
        public ICitiesHolder Default() => this;
        public IAlphaHolder Custom() => this;
        public static ISelectorHolder AntColonyOptimization() => new TspSolver();
        public IStartPositionHolder WithCities(string filePath) => new TspSolver(GetNodesFromFile(filePath));
        public IStartPositionHolder WithCities(IEnumerable<Node> nodes) => new TspSolver(nodes);

        public IAntHolder StartFrom(int indexOfCity)
        {
            if (indexOfCity < 0 || indexOfCity > this.Nodes.Count) throw new ArgumentOutOfRangeException(nameof(indexOfCity));
            _startFrom = indexOfCity;
            return this;
        }

        public IAntHolder StartFromRandomPosition() => this;

        public IIterationNubmerHolder WithAnts(int numberOfAnts)
        {
            if (numberOfAnts < 0) throw new ArgumentOutOfRangeException(nameof(numberOfAnts));

            return new TspSolver
            {
                Nodes = this.Nodes,
                Ants = new List<Ant>(Ant.CreateAnts(numberOfAnts)),
                _startFrom = this._startFrom
            };
        }


        public ITspSolver WithMaximumTime(int maxTime)
        {
            if (maxTime < 0) throw new ArgumentOutOfRangeException(nameof(maxTime));

            return new TspSolver
            {
                Nodes = this.Nodes,
                Ants = this.Ants,
                _startFrom = this._startFrom,
                _times = maxTime
            };
        }

        private List<Node> GetNodesFromFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(filePath));

            return Regex.Replace(File.ReadAllText(filePath), @"\s", "")
                        .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(row => row.Split(','))
                        .Select(ParseToNode)
                        .ToList();
        }

        private static Node ParseToNode(string[] n)
        {
            return new Node(n[0].Trim(), double.Parse(n[1].Trim(), CultureInfo.InvariantCulture), double.Parse(n[2].Trim(),
                CultureInfo.InvariantCulture));
        }

        public IBetaHolder WithInfluenceOfPheromoneOnDrection(int alpha) => new TspSolver
        {
            _alpha = alpha
        };

        public IRhoHolder WithInfluenceOfAdjacentNodeDistance(int beta) => new TspSolver
        {
            _alpha = this._alpha,
            _beta = beta
        };

        public IQHolder WithPheromoneDecreaseFactor(double rho) => new TspSolver
        {
            _alpha = this._alpha,
            _beta = this._beta,
            _rho = rho
        };

        public ICitiesHolder WithPheromoneIncreaseFactor(double q) => new TspSolver
        {
            _alpha = this._alpha,
            _beta = this._beta,
            _rho = this._rho,
            _q = q
        };


        public void Solve()
        {
            for (int i = 0; i < _times; i++)
            {
                UpdateAnts(i);
                IsNewBestLength(i);
                UpdatePheromones();
            }
            Console.WriteLine("End of Loop");
        }

        private void IsNewBestLength(int when)
        {
            if (!(GetMinLength < BestLength)) return;
            BestLength = GetMinLength;
            Console.WriteLine(
                $"New Best Length = {BestLength:F2} At {when} iteration"); // ->  with Trail: [ {BestTrailLabel()} ]
                                                        // Console.WriteLine($"Pheromons on that iteration: \n{PheromoneLabel()}");
        }

        private string PheromoneLabel()
        {
            var sb = new StringBuilder();

            foreach (Node node in Nodes)
            {
                sb.Append("[ ");
                foreach (var nodeRoute in node.Routes)
                {
                    sb.Append($"{node.Name}->{nodeRoute.Key.Name}:{nodeRoute.Value.Pheromone.Value:F2}");
                    sb.Append(" ");
                }
                sb.Append("]");
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

        private string BestTrailLabel()
        {
            StringBuilder TrailLabel = new StringBuilder();

            foreach (Node node in BestTrail)
            {
                TrailLabel.Append($"{node} ");
            }
            return TrailLabel.ToString();
        }

        private void CalculateDistance()
        {
            for (var index = 0; index < this.Nodes.Count; index++)
            {
                var node = this.Nodes[index];
                node.GetDistanceTable(Nodes);
            }
        }

        private Ant GetBestAnt => Ants.OrderBy(ant => ant.TrailLength).First();

        private double GetMinLength => BestAnt.TrailLength;

        private IEnumerable<Node> GetBestTrail => BestAnt.Trail;

        private void UpdateAnts(int iteration)
        {
            //Console.WriteLine($"Started {iteration} iteration to build trails for {Ants.Count} Ants and for {Nodes.Count} number of Cities....");
            Ants.ForEach(n => n.BuildTrail(_startFrom, Nodes, _alpha, _beta));
        }

        private void UpdatePheromones()
        {
            this.Nodes
                .ForEach(node =>
                    node.Routes.ToList()
                        .ForEach(route => route.Value.Pheromone.Update(_rho)));

            this.Ants
                .ForEach(ant =>
                {
                    for (int index = 0; index < ant.Trail.Count - 1; index++)
                    {
                        Route(ant, index).Pheromone.Value += _q / ant.TrailLength;
                    }
                });
        }

        private static Route Route(Ant ant, int index)
        {
            return ant.Trail[index].GetRouteTo(ant.Trail[index + 1]);
        }
    }
}