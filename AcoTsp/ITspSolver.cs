using System.Collections.Generic;
using AcoTsp.Model;

namespace AcoTsp
{
	public interface ISelectorHolder
	{
		IAlphaHolder Custom();
		ICitiesHolder Default();
	}

	public interface IAlphaHolder
	{
		IBetaHolder WithInfluenceOfPheromoneOnDrection(int alpha);
	}

	public interface IBetaHolder
	{
		IRhoHolder WithInfluenceOfAdjacentNodeDistance(int beta);
	}

	public interface IRhoHolder
	{
		IQHolder WithPheromoneDecreaseFactor(double rho);
	}

	public interface IQHolder
	{
		ICitiesHolder WithPheromoneIncreaseFactor(double q);
	}

	public interface ICitiesHolder
	{
		IStartPositionHolder WithCities(IEnumerable<Node> nodes);
		IStartPositionHolder WithCities(string filePath);
	}

	public interface IStartPositionHolder
	{
		IAntHolder StartFrom(int indexOfCity);
		IAntHolder StartFromRandomPosition();
	}

	public interface IAntHolder
	{

		IIterationNubmerHolder WithAnts(int numberOfAnts);
	}

	public interface IIterationNubmerHolder
	{
		ITspSolver WithMaximumTime(int maxTime);
	}
	public interface ITspSolver
	{
		void Solve();
	}
}