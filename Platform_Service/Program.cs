using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Platform_Service.Data;
using Platform_Service.SyncDataService.Http;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var environment = builder.Environment;
var configuration = builder.Configuration;

if (environment.IsProduction())
{
    // Use SQL Server in production
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("PlatformsConn")));
    Console.WriteLine("--> Using Production settings");
    Console.WriteLine("--> Using MsSQL DB");
}
else
{
    // Use in-memory database in development
    builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("InMem"));
    Console.WriteLine("--> Using Development settings");
    Console.WriteLine("--> Using InMem DB");
}



var app = builder.Build();
System.Console.WriteLine($"Command Service End point {app.Configuration["CommandService"]}");



//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
PrepDb.PrepPopulation(app,environment.IsProduction());

app.Run();
