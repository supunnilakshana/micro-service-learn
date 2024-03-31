using Platform_Service.Models;

namespace Platform_Service.Data
{
    public class PlatformRepo : IPlatformRepo
    {
        public readonly AppDbContext _context;
        public PlatformRepo(AppDbContext context)
        {
            _context = context;
        }
        public void CreatePlatform(Platform plat)
        {
            if (plat == null)
            {
                throw new System.ArgumentNullException(nameof(plat));
            }
            _context.Platforms.Add(plat);
            
        }

        public void DeletePlatform(Platform plat)
        {
            

        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platforms.ToList();
        }

        public Platform? GetPlatformById(int id)
        {

            return _context.Platforms.FirstOrDefault(p => p.Id == id);
        }

        public bool SaveChanges()
        {
           return (_context.SaveChanges() >= 0);

        }

        public void UpdatePlatform(Platform plat)
        {
              _context.Platforms.Update(plat);
           
        }
    }
}
