using Platform_Service.Dto;

namespace Platform_Service.AysncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewPlatform(PlatformPublishDto platformPublishDto);
    }
}