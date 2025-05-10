using AutoMapper;
using Core.Entities;
using Infrastucture.Entities;
using Infrastucture.IdentityEntities;
using Services.Services.UserService.Dto;
using Services.Services.BusService.DTO;
using Services.Services.StopsService.DTO;
using Services.Services.RouteService.Dto;
using Services.Services.TripService.DTO;
using System.Linq;
using Route = Infrastucture.Entities.Route;

namespace SmartTrackingTransport.Mappings
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			// User mappings
			CreateMap<AppUser, UserDto>();


			CreateMap<RegisterDto, AppUser>()
				.ForMember(d => d.UserName, o => o.MapFrom(s => s.Email));

			// Bus mappings
			CreateMap<Buses, BusDto>();


			CreateMap<BusDto, Buses>()
				.ForMember(d => d.Driver, o => o.Ignore())
				.ForMember(d => d.Route, o => o.Ignore())
				.ForMember(d => d.Trips, o => o.Ignore())
				.ForMember(d => d.TrackingData, o => o.Ignore());

			// Stop mappings
			CreateMap<Stops, StopsDto>();


			CreateMap<StopsDto, Stops>()
				.ForMember(d => d.RouteStops, o => o.Ignore());

			//Route mappings
			CreateMap<Route, RouteDto>()
				.ForMember(d => d.RouteId, o => o.MapFrom(s => s.Id))
				.ForMember(d => d.Origin, o => o.MapFrom(s => s.Origin))
				.ForMember(d => d.Destination, o => o.MapFrom(s => s.Destination))
				.ForMember(d => d.Stops, o => o.MapFrom(s => s.RouteStops
					.OrderBy(rs => rs.Order)
					.Select(rs => rs.Stop.Name)
					.ToList()));

			CreateMap<RouteDto, Route>()
				.ForMember(d => d.Id, o => o.MapFrom(s => s.RouteId))
				.ForMember(d => d.Origin, o => o.MapFrom(s => s.Origin))
				.ForMember(d => d.Destination, o => o.MapFrom(s => s.Destination))
				.ForMember(d => d.RouteStops, o => o.Ignore())
				.ForMember(d => d.Trips, o => o.Ignore())
				.ForMember(d => d.Buses, o => o.Ignore());

			// Trip mappings
			CreateMap<Trip, TripDto>();


			CreateMap<TripDto, Trip>()
				.ForMember(d => d.Bus, o => o.Ignore())
				.ForMember(d => d.Route, o => o.Ignore());
		}
	}
}