using Infrastructure.Interfaces;
using Infrastucture.DbContexts;
using Infrastucture.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repos
{
	public class BusRepository : IBusRepository
	{
		protected readonly TransportContext _transportContext;


		public BusRepository(TransportContext transportContext)
		{
			_transportContext = transportContext;
		}
		public Task Add(Buses entity)
		{
			throw new NotImplementedException();
		}

		public void Delete(Buses entity)
		{
			throw new NotImplementedException();
		}

		public Task<IReadOnlyList<Buses>> FindAllAsync(Expression<Func<Buses, bool>> predicate)
		{
			throw new NotImplementedException();
		}

		public Task<IReadOnlyList<Buses>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

		// still not implemented
		public async Task<IEnumerable<Buses>> GetAvailableBusesAsync(string startLocation, string destination)
		{
			return await _transportContext.Buses
		                 .Include(b => b.Route)
		                 .ThenInclude(r => r.RouteStops)
		                 .ThenInclude(rs => rs.Stop)
		                 .Where(b => b.Route.RouteStops.Any(rs => rs.Stop.Name == startLocation) &&
					            b.Route.RouteStops.Any(rs => rs.Stop.Name == destination))
		                 .ToListAsync();

		}

		public Task<Buses> GetByIdAsync(int? id)
		{
			throw new NotImplementedException();
		}

		public void Update(Buses entity)
		{
			throw new NotImplementedException();
		}
	}
}
