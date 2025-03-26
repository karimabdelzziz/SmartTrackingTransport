using Services.Services.BusService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.BusService
{
	public interface IBusService
	{
		Task<IEnumerable<BusDto>> GetAvailableBusesAsync(string origin, string destination);
		Task<bool> AddBusAsync(BusDto busDto);
		Task<bool> UpdateBusAsync(int id, BusDto busDto);
		Task<bool> RemoveBusAsync(int id);
		Task<BusDto> GetBusByIdAsync(int id);
		Task<bool> UpdateBusStatusAsync(int id , string status);

	}
}
