using Platform_Service.Models;

namespace Platform_Service.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());

            }



           
        }

        private static void SeedData(AppDbContext context)
        {
            if (!context.Platforms.Any())
            {
                System.Console.WriteLine("--> Seeding Data...");
                context.Platforms.AddRange(
                     new Platform() { Name = "Dot Net", Publisher = "Microsoft", Cost = "Free" },
                     new Platform() { Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free" },
                     new Platform() { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
                );
                context.SaveChanges();
            } 
            {

                System.Console.WriteLine("--> We already have data");
            }
                    
            
        } 
        

    }
}
