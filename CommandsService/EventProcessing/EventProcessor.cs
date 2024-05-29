using System.Text.Json;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dto;
using CommandsService.Dtos;
using CommandsService.EventProcessing;
using CommandsService.Models;

namespace CommandsService.EventProcessing
{

    public class EventProcessor : IEventProcessor
    {

        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public EventProcessor(IMapper mapper, IServiceScopeFactory serviceScopeFactory)
        {
            _mapper = mapper;
            _serviceScopeFactory = serviceScopeFactory;
        }
        public void ProcessEvent(string message)
        {
            System.Console.WriteLine("ProcessEvent Method Started");
            var eventType = DetermineEvent(message);
            switch (eventType)
            {
                case EventType.PlatformPublishedEvent:
                    AddPlatform(message);
                    break;
                default:
                    System.Console.WriteLine("ProcessEvent Method Ended");
                    break;
            }
        }
        private EventType DetermineEvent(string notificationMessage)
        {
            System.Console.WriteLine("DetermineEvent Method Started");
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);
            System.Console.WriteLine($"DetermineEvent Method Ended");
            return eventType?.Event switch
            {
                "Platform_Published" => EventType.PlatformPublishedEvent,
                "PlatformDeletedEvent" => EventType.PlatformDeletedEvent,
                _ => EventType.Undetermined
            };
        }
        private void AddPlatform(string platformPublishedMessage)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<ICommandRepo>();
                var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishDto>(platformPublishedMessage);
                try
                {
                    var platform = _mapper.Map<Platform>(platformPublishedDto);
                    if (!repository.ExternalPlatformExists(platform.ExternalId))
                    {
                        repository.CreatePlatform(platform);
                        repository.SaveChanges();
                        System.Console.WriteLine($"Platform added");
                    }
                    else
                    {
                        System.Console.WriteLine($"Platform already exists");
                    }
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"Platform added failed: {ex.Message}");
                }
            }

        }



    }
    enum EventType
    {
        PlatformPublishedEvent,
        PlatformDeletedEvent,
        Undetermined
    }
}
