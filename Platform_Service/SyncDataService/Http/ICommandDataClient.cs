using Platform_Service.Dto;

namespace Platform_Service.SyncDataService.Http
{
    public interface ICommandDataClient
    {

        Task SendPlatformToCommand(PlatformReadDto platform);
    }
}
