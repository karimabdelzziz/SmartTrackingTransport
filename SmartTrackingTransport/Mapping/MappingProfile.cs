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
using Services.Services.LostItemsService.DTO;

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
			CreateMap<Buses, BusDto>()
							.ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
							.ForMember(d => d.LicensePlate, o => o.MapFrom(s => s.LicensePlate))
							.ForMember(d=>d.Capacity, o=> o.MapFrom(s=>s.Capacity))
							.ForMember(d=>d.Status,o=>o.MapFrom(s=>s.Status))
							.ForMember(d=>d.Model , o=> o.MapFrom(s=>s.Model))
							.ForMember(d => d.Origin, o => o.MapFrom(s => s.Route.RouteStops.OrderBy(rs => rs.Order).FirstOrDefault().Stop.Name))
				            .ForMember(d => d.Destination, o => o.MapFrom(s => s.Route.RouteStops.OrderByDescending(rs => rs.Order).FirstOrDefault().Stop.Name));

			CreateMap<BusDto, Buses>()
				.ForMember(d => d.Driver, o => o.Ignore())
				.ForMember(d => d.Route, o => o.Ignore())
				.ForMember(d => d.Trips, o => o.Ignore())
				.ForMember(d => d.TrackingData, o => o.Ignore());

			CreateMap<Buses, BusAbstractDto>()
				.ForMember(d => d.BusNumber, o => o.MapFrom(s => s.LicensePlate))
				.ForMember(d => d.Origin, o => o.MapFrom(s => s.Route.RouteStops.OrderBy(rs => rs.Order).FirstOrDefault().Stop.Name))
				.ForMember(d => d.Destination, o => o.MapFrom(s => s.Route.RouteStops.OrderByDescending(rs => rs.Order).FirstOrDefault().Stop.Name));
			
			
			CreateMap<Buses, BusTripDetailsDto>()
				.ForMember(d => d.BusNumber, o => o.MapFrom(s => s.LicensePlate))
				.ForMember(d => d.Origin, o => o.MapFrom(s => s.Route.RouteStops.OrderBy(rs => rs.Order).FirstOrDefault().Stop.Name))
				.ForMember(d => d.Destination, o => o.MapFrom(s => s.Route.RouteStops.OrderByDescending(rs => rs.Order).FirstOrDefault().Stop.Name))
				.ForMember(d => d.StartTime, o => o.MapFrom(s => s.Trips.OrderByDescending(t => t.StartTime).FirstOrDefault().StartTime))
				.ForMember(d => d.EndTime, o => o.MapFrom(s => s.Trips.OrderByDescending(t => t.StartTime).FirstOrDefault().EndTime))
				.ForMember(d => d.Stops, o => o.MapFrom(s => s.Route.RouteStops
					.OrderBy(rs => rs.Order)
					.Select(rs => new StopTimeDto
					{
						Stop = rs.Stop.Name,
						Time = s.Trips.OrderByDescending(t => t.StartTime).FirstOrDefault().StartTime.AddMinutes(rs.Order * 20)
					})))
				.ForMember(d => d.LifeTrack, o => o.MapFrom(s => s.TrackingData
					.OrderBy(td => td.Timestamp)
					.Select(td => new LocationPointDto
					{
						Latitude = td.Latitude,
						Longitude = td.Longitude,
						Timestamp = td.Timestamp
					})));

			CreateMap<Buses, BusTripsDto>()
				.ForMember(d => d.BusNumber, o => o.MapFrom(s => s.LicensePlate))
				.ForMember(d => d.Origin, o => o.MapFrom(s => s.Route.RouteStops.OrderBy(rs => rs.Order).FirstOrDefault().Stop.Name))
				.ForMember(d => d.Destination, o => o.MapFrom(s => s.Route.RouteStops.OrderByDescending(rs => rs.Order).FirstOrDefault().Stop.Name))
				.ForMember(d => d.TripsToday, o => o.MapFrom(s => s.Trips.Select(t => new TripTimeDto
				{
					TripId = t.Id,
					StartTime = t.StartTime
				})));

			CreateMap<Trip, TripTimeDto>()
				.ForMember(d => d.TripId, o => o.MapFrom(s => s.Id))
				.ForMember(d => d.StartTime, o => o.MapFrom(s => s.StartTime));

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

			CreateMap<LostItem, LostItemDto>()
				.ForMember(d => d.ContactName, o => o.MapFrom(s => s.Name))
				.ForMember(d => d.ContactPhone, o => o.MapFrom(s => s.Phone))
				.ForMember(d => d.PhotoUrl, o => o.MapFrom(s => s.ImagePath))
				.ForMember(d => d.BusNumber, o => o.MapFrom(s => s.BusNumber));

			CreateMap<LostItemDto, LostItem>()
				.ForMember(d => d.Name, o => o.MapFrom(s => s.ContactName))
				.ForMember(d => d.Phone, o => o.MapFrom(s => s.ContactPhone))
				.ForMember(d => d.ImagePath, o => o.MapFrom(s => s.PhotoUrl))
				.ForMember(d => d.BusNumber, o => o.MapFrom(s => s.BusNumber));

			CreateMap<LostItem, ReportLostItemDto>()
				.ForMember(d => d.ContactName, o => o.MapFrom(s => s.Name))
				.ForMember(d => d.ContactPhone, o => o.MapFrom(s => s.Phone))
				.ForMember(d => d.PhotoUrl, o => o.MapFrom(s => s.ImagePath))
				.ForMember(d => d.BusNumber, o => o.MapFrom(s => s.BusNumber));

			CreateMap<ReportLostItemDto, LostItem>()
				.ForMember(d => d.Name, o => o.MapFrom(s => s.ContactName))
				.ForMember(d => d.Phone, o => o.MapFrom(s => s.ContactPhone))
				.ForMember(d => d.ImagePath, o => o.MapFrom(s => s.PhotoUrl))
				.ForMember(d => d.BusNumber, o => o.MapFrom(s => s.BusNumber));
		}
	}
}