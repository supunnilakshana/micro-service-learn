using AutoMapper;
using Platform_Service.Dto;
using Platform_Service.Models;

namespace Platform_Service.Profiles
{
    public class PlatformProfile : Profile
    {
        public PlatformProfile()
        {
            // Source -> Target

            CreateMap<Platform, PlatformReadDto>();
            CreateMap<PlatformCreateDto, Platform>();
            CreateMap<PlatformReadDto, PlatformPublishDto>();
            CreateMap<Platform, GrpcPlatformModel>()
                .ForMember(dest => dest.Platformid, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Publisher, opt => opt.MapFrom(src => src.Publisher));






        }
    }
}
