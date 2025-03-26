using AutoMapper;
using Infrastructure.Interfaces;
using Infrastucture.Entities;
using Services.Services.BusService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.BusService
{
	public class BusService : IBusService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public BusService(IUnitOfWork unitOfWork,IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		public async Task<bool> AddBusAsync(BusDto busDto)
		{
			var bus = _mapper.Map<Buses>(busDto);
			await _unitOfWork.Repository<Buses>().Add(bus);
			return await _unitOfWork.Complete() > 0;
		}

		// Still not implemented
		public async Task<IEnumerable<BusDto>> GetAvailableBusesAsync(string origin, string destination)
		{
			var busRepo = _unitOfWork.Repository<Buses>() as IBusRepository;
			if (busRepo == null)
				throw new InvalidOperationException("Bus repository not found in UnitOfWork");

			var buses = await busRepo.GetAvailableBusesAsync(origin, destination);
			return _mapper.Map<IEnumerable<BusDto>>(buses);
		}

		public async Task<BusDto> GetBusByIdAsync(int id)
		{
			var bus = await _unitOfWork.Repository<Buses>().GetByIdAsync(id);
			return _mapper.Map<BusDto>(bus);
		}

		public async Task<bool> RemoveBusAsync(int id)
		{
			var bus = await _unitOfWork.Repository<Buses>().GetByIdAsync(id);
			if (bus == null)
				return false;
			_unitOfWork.Repository<Buses>().Delete(bus);
			return await _unitOfWork.Complete() > 0;
		}

		public async Task<bool> UpdateBusAsync(int id, BusDto busDto)
		{
			var bus = await _unitOfWork.Repository<Buses>().GetByIdAsync(id);
			if (bus == null) return false;

			_mapper.Map(busDto, bus); // AutoMapper updates the entity
			_unitOfWork.Repository<Buses>().Update(bus);
			return await _unitOfWork.Complete() > 0;
		}

		public async Task<bool> UpdateBusStatusAsync(int id, string status)
		{
			var bus = await _unitOfWork.Repository<Buses>().GetByIdAsync(id);
			if (bus == null) return false;

			bus.Status = status;
			_unitOfWork.Repository<Buses>().Update(bus);
			return await _unitOfWork.Complete() > 0;
		}
	}
}
