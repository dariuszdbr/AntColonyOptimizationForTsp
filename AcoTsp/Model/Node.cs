using System.Collections.Generic;
using System.Linq;

namespace AcoTsp.Model
{
    public class Node
    {
        public IDictionary<Node,Route> Routes { get; /*private*/ set; }
        private Point Point { get; }
        public string Name { get; set; }

        public Node(string name, double x, double y) : this(name, new Point(x, y)) { }
        public Node(string name,Point point)
        {
            this.Name = name;
            this.Point = point;
            this.Routes = new Dictionary<Node, Route>();          
        }

        public void GetDistanceTable(IEnumerable<Node> nodes)
        {
            Routes = nodes
                .Select(n => new { Key = n, Value = Route.GetDistance(this.Point, n.Point)})
                .ToDictionary(arg => arg.Key , arg => arg.Value);
        }

        public Route GetRouteTo(Node node)
        {
            return Routes[node];
        }

        public override string ToString() => $"({this.Name}: {this.Point.X}, {this.Point.Y})";
    }
}
