using CommandsService.Models;

namespace CommandsService.Data
{

    public interface ICommandRepo
    {
        bool SaveChanges();
        // Platforms
        IEnumerable<Platform> GetAllPlatforms();
        void CreatePlatform(Platform platform);
        bool PlatformExists(int platformId);
        bool ExternalPlatformExists(int externalPlatformId);
        IEnumerable<Command> GetCommandsForPlatform(int platformId);
        // Commands
        Command GetCommand(int platformId, int commandId);
        void CreateCommand(int platformId, Command command);
    }


}