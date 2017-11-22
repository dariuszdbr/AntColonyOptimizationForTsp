using System;

namespace AcoTsp.Model
{
    public class Route
    {
        private Route() { Pheromone = new Pheromone(); }
        public Pheromone Pheromone { get; private set; }

        public double Distance { get; private set; }

        public static Route GetDistance(Point from, Point to) => new Route
        {
            Distance = 
                Math.Sqrt(
                Math.Pow(from.X - to.X, 2) + 
                Math.Pow(from.Y - to.Y, 2))
        };

    }
}
