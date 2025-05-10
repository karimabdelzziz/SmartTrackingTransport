using AutoMapper;
using Infrastucture.Entities;
using Microsoft.AspNetCore.Mvc;
using Services.Services.BusService;
using Services.Services.BusService.DTO;

namespace SmartTrackingTransport.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class BusController : ControllerBase
	{
		private readonly IBusService _busService;

		public BusController(IBusService busService)
		{
			_busService = busService;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<BusDto>>> GetBuses([FromQuery] string origin, [FromQuery] string destination)
		{
			var buses = await _busService.GetAvailableBusesAsync(origin, destination);
			return Ok(buses);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<BusDto>> GetBus(int id)
		{
			var bus = await _busService.GetBusByIdAsync(id);
			if (bus == null)
				return NotFound();

			return Ok(bus);
		}

	}
}