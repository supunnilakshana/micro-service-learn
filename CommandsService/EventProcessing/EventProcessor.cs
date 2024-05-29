using System.Text.Json;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dto;
using CommandsService.Dtos;
using CommandsService.EventProcessing;
using CommandsService.Models;

namespace NamespaceName.EventProcessing
{

    public class EventProcessor : IEventProcessor
    {
        private readonly ICommandRepo _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<EventProcessor> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public EventProcessor(ICommandRepo repository, IMapper mapper, ILogger<EventProcessor> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }
        public void ProcessEvent(string message)
        {
            _logger.LogInformation($"ProcessEvent Method Started");
            var eventType = DetermineEvent(message);
            switch (eventType)
            {
                case EventType.PlatformPublishedEvent:
                    AddPlatform(message);
                    break;
                default:
                    _logger.LogInformation($"ProcessEvent Method Undetermined Event Type");
                    break;
            }
        }
        private EventType DetermineEvent(string notificationMessage)
        {
            _logger.LogInformation($"DetermineEvent Method Started");
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);
            _logger.LogInformation($"DetermineEvent Method Ended");
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
                    if (!_repository.ExternalPlatformExists(platform.ExternalId))
                    {
                        _repository.CreatePlatform(platform);
                        _repository.SaveChanges();
                        _logger.LogInformation($"Platform added");
                    }
                    else
                    {
                        _logger.LogInformation($"Platform already exists");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Platform added failed: {ex.Message}");
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
