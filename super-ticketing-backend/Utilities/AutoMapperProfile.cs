
using AutoMapper;
using super_ticketing_backend.Dto_s;
using super_ticketing_backend.Models;

namespace super_ticketing_backend.Utilities;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Tickets, TicketDto>()
            .ForMember(dest => dest.UserEmail, opt => opt.Ignore())
            .ForMember(dest => dest.ItGuyEmail, opt => opt.Ignore());

        CreateMap<TicketCreateDto, Tickets>();
        CreateMap<TicketUpdateDto, Tickets>();
        

        CreateMap<Country, CountryDto>();
        CreateMap<CountryCreateDto, Country>();

        CreateMap<ITGuys, ItGuyDto>()
            .ForMember(dest => dest.CountryCode, opt => opt.Ignore());
        CreateMap<ItGuyCreateDto, ITGuys>()
            .ForMember(dest => dest.CountryId, opt => opt.Ignore());

        CreateMap<Users, UserDto>()
            .ForMember(dest => dest.CountryCode, opt => opt.Ignore());
        CreateMap<UserCreateDto, Users>()
            .ForMember(dest => dest.CountryId, opt => opt.Ignore());

        CreateMap<TicketStatus, TicketStatusDto>();
        CreateMap<TicketStatusCreateDto, TicketStatus>();

    }
    
}