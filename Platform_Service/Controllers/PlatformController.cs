using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Platform_Service.Data;
using Platform_Service.Dto;
using Platform_Service.Models;

namespace Platform_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformController : ControllerBase
    {
        private readonly IPlatformRepo _platformRepo;
        private readonly IMapper _mapper;
        public PlatformController(IPlatformRepo platformRepo,IMapper mapper)
        {
            _platformRepo = platformRepo;
            _mapper = mapper;

            
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPatforms()
        {
            Console.WriteLine("--> Getting Platforms");
            var platformItems = _platformRepo.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
        }

        [HttpGet("{id}",Name = "GetPlatformById")]
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
        public ActionResult<PlatformReadDto> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            var platformModel = _mapper.Map<Platform>(platformCreateDto );
            _platformRepo.CreatePlatform(platformModel);
            _platformRepo.SaveChanges();
            var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);
            return CreatedAtRoute(nameof(GetPlatformById), new {Id = platformReadDto.Id}, platformReadDto);
        }  
        


    }
}
