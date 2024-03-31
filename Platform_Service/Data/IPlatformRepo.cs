using Platform_Service.Models;

namespace Platform_Service.Data
{
    public interface IPlatformRepo
    {
        bool SaveChanges();

        IEnumerable<Platform> GetAllPlatforms();
        Platform GetPlatformById(int id);
        void CreatePlatform(Platform plat);
        void UpdatePlatform(Platform plat);
        void DeletePlatform(Platform plat);
    }
}
