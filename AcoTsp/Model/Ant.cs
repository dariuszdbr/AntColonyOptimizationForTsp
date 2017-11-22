using System;
using System.Collections.Generic;
using System.Linq;

namespace AcoTsp.Model
{
	public class Ant
	{
		private readonly Random _random = new Random();
		internal IList<Node> Trail { get; set; } = new List<Node>();
		internal IList<double> CumulativeProbs { get; set; }
		internal IList<double> MoveProbs { get; set; } = new List<double>();
		internal IList<double> Taueta { get; set; } = new List<double>();

		public double TrailLength => GetLength();

		private double GetLength()
		{
			var sum = 0.0;

			for (int index = 0; index < this.Trail.Count - 2; index++)
			{
				sum += this.Trail[index].GetRouteTo(this.Trail[index + 1]).Distance;
			}
			sum += this.Trail.Last().GetRouteTo(this.Trail.First()).Distance;

			return sum;
		}

		internal void BuildTrail(int maybeStartFrom, IList<Node> nodes, int alpha, int beta)
		{
			var startFrom = StartFrom(maybeStartFrom, nodes);

			this.Trail = new List<Node>();
			Trail.Add(startFrom);

			for (int i = 0; i < nodes.Count - 1; i++)
				Trail.Add(MoveNextCity(nodes, alpha, beta));

			Trail.Add(startFrom);
		}

		private Node StartFrom(int maybeStartFrom, IList<Node> nodes)
		{
			return maybeStartFrom == -1 ? GetRandomCity(nodes) : nodes[maybeStartFrom];
		}


		private Node MoveNextCity(IList<Node> nodes, int alpha, int beta)
		{
			this.Taueta = CreateTauetaTable(nodes, alpha, beta);
			this.MoveProbs = CreateProbTable();
			this.CumulativeProbs = CreateCumulativesProbs();
			var node = SelectNode(nodes);
			return node;
		}

		private Node SelectNode(IList<Node> nodes)
		{
			double P = _random.NextDouble();
			for (int i = 0; i < this.CumulativeProbs.Count - 1; i++)
				if (P >= this.CumulativeProbs[i] && P < this.CumulativeProbs[i + 1])
					return nodes[i];

			throw new ArgumentOutOfRangeException(nameof(SelectNode), "Failure to return valid city for MoveNextCity");
		}

		private IList<double> CreateCumulativesProbs()
		{
			var cumulatives = new List<double>(MoveProbs.Count + 1);

			double sum = 0.0d;

			foreach (double probability in this.MoveProbs)
			{
				cumulatives.Add(sum);
				sum += probability;
			}
			cumulatives.Add(1.0d);

			return cumulatives;
		}

		private IList<double> CreateProbTable()
		{
			double sum = this.Taueta.Sum();
			return this.Taueta.Select(t => t / sum).ToList();
		}

		private IList<double> CreateTauetaTable(IList<Node> nodes, int alpha, int beta)
		{
			var taueta = new List<double>(nodes.Count);
			taueta
				.AddRange(nodes
					.Select(node => CalculateTaueta(node, alpha, beta, nodes.Count)));

			return taueta;
		}

		private double CalculateTaueta(Node node, int alpha, int beta, int numberOfCities)
		{
			if (this.Trail.Contains(node)) // moving to self = prob 0.0, the same if the node was already visited
				return 0.0;

			var routeBetweenCities = this.Trail.Last().GetRouteTo(node);
			double taueta = Math.Pow(routeBetweenCities.Pheromone.Value, alpha) * Math.Pow((1.0 / routeBetweenCities.Distance), beta);
			return ValidTaueta(taueta, numberOfCities);
		}

		private static double ValidTaueta(double taueta, int numberOfCities)
		{
			if (taueta < 0.0001)
				return 0.0001;

			if (taueta > (double.MaxValue / (numberOfCities * 100)))
				return double.MaxValue / (numberOfCities * 100);

			return taueta;
		}


		private Node GetRandomCity(IList<Node> nodes)
		{
			return nodes[_random.Next(1, nodes.Count)];
		}



		internal static IEnumerable<Ant> CreateAnts(int numberOfAnts)
		{
			var ants = new List<Ant>();
			for (int i = 0; i < numberOfAnts; i++)
				ants.Add(new Ant());

			return ants;
		}
	}
}
