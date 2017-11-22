namespace AcoTsp.Model
{
	public class Pheromone
	{
		private double _value;
		public double Value
		{
			get => _value;
			set
			{
				if (value > 0)
					_value = value;
			}
		}

		public Pheromone()
		{
			Value = 1;
		}

		public void Update(double Rho)
		{
			this.Value = (1 - Rho) * this.Value;
		}


	}
}
