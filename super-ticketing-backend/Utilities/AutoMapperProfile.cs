
using AutoMapper;
using super_ticketing_backend.Dto_s;
using super_ticketing_backend.Models;

namespace super_ticketing_backend.Utilities;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Tickets, TicketDto>();
        CreateMap<TicketCreateDto, Tickets>();

        CreateMap<Country, CountryDto>();
        CreateMap<CountryCreateDto, Country>();

        CreateMap<ITGuys, ItGuyDto>();
        CreateMap<ItGuyCreateDto, ITGuys>();

        CreateMap<Users, UserDto>();
        CreateMap<UserCreateDto, Users>();
    }
    
}