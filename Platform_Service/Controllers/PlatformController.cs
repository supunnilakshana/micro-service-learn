using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Platform_Service.AysncDataServices;
using Platform_Service.Data;
using Platform_Service.Dto;
using Platform_Service.Models;
using Platform_Service.SyncDataService.Http;

namespace Platform_Service.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformController : ControllerBase
    {
        private readonly IPlatformRepo _platformRepo;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public PlatformController(IPlatformRepo platformRepo, IMapper mapper, ICommandDataClient commandDataClient, IMessageBusClient messageBusClient)
        {
            _platformRepo = platformRepo;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
            _messageBusClient = messageBusClient;



        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPatforms()
        {
            Console.WriteLine("--> Getting Platforms");
            var platformItems = _platformRepo.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            var platformItem = _platformRepo.GetPlatformById(id);
            if (platformItem != null)
            {
                return Ok(_mapper.Map<PlatformReadDto>(platformItem));
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            var platformModel = _mapper.Map<Platform>(platformCreateDto);
            _platformRepo.CreatePlatform(platformModel);
            _platformRepo.SaveChanges();
            var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);

            // Send Sync Message
            try
            {
                await _commandDataClient.SendPlatformToCommand(platformReadDto);
            }
            catch (Exception e)
            {
                Console.WriteLine($"--> Could not send synchronously: {e.Message}");
            }

            // Send Async Message
            try
            {
                var platformPublishDto = _mapper.Map<PlatformPublishDto>(platformReadDto);
                platformPublishDto.Event = "Platform_Published";
                _messageBusClient.PublishNewPlatform(platformPublishDto);
            }
            catch (Exception e)
            {
                Console.WriteLine($"--> Could not send asynchronously: {e.Message}");
            }

            return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDto.Id }, platformReadDto);

        }



    }
}
