﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.TrackingService.DTO
{
	public class LocationUpdateDto
	{
		public int BusId { get; set; }
		public decimal Latitude { get; set; }
		public decimal Longitude { get; set; }
	}
}
