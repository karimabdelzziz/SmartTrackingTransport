using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Entities
{
	public class Route : BaseEntity
	{
		public string Origin { get; set; }
		public string Destination { get; set; }
		public string Stops { get; set; } // JSON formatted stops
		public ICollection<Trip> Trips { get; set; }
	}
}
