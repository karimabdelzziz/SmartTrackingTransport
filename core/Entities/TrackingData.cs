﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Entities
{
	public class TrackingData : BaseEntity
	{
		public int VehicleId { get; set; }
		public decimal Latitude { get; set; }
		public decimal Longitude { get; set; }
		public DateTime Timestamp { get; set; } = DateTime.UtcNow;
		public Buses Bus { get; set; }
	}
}
