using AutoMapper;
using CommandsService.Dto;
using CommandsService.Dtos;
using CommandsService.Models;

namespace CommandsService.Profiles
{
    public class CommandProfile : Profile
    {
        public CommandProfile()
        {
            // Source -> Target
            CreateMap<Command, CommandReadDto>();
            CreateMap<CommandCreateDto, Command>();
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<PlatformPublishDto, Platform>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));




        }
    }

}