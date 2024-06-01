using CommandsService.Models;
using CommandsService.SyncDataServices.Grpc;

namespace CommandsService.Data

{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var grpcClent = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();
                var platforms = grpcClent.ReturnAllPlatforms();
                if (platforms != null)
                {

                    var commandRepo = serviceScope.ServiceProvider.GetService<ICommandRepo>()!;
                    SeedData(commandRepo, platforms);

                }
                else
                {
                    System.Console.WriteLine("No Platforms");
                }

            }



        }

        private static void SeedData(ICommandRepo commandRepo, IEnumerable<Platform> platforms)
        {
            System.Console.WriteLine("--> Seeding New Platforms...");
            foreach (var plat in platforms)
            {
                if (!commandRepo.ExternalPlatformExists(plat.ExternalId))
                {
                    commandRepo.CreatePlatform(plat);
                }
                commandRepo.SaveChanges();
            }
        }

    }

}