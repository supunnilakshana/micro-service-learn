using AutoMapper;
using Platform_Service.Dto;
using Platform_Service.Models;

namespace Platform_Service.Profiles
{
    public class PlatformProfile: Profile
    {
        public PlatformProfile()
        {
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<PlatformCreateDto, Platform>();
             



        }
    }
}
