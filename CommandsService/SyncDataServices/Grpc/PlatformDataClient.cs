using AutoMapper;
using CommandService;
using CommandsService.Models;
using Grpc.Net.Client;



namespace CommandsService.SyncDataServices.Grpc
{
    public class PlatformDataClient : IPlatformDataClient
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public PlatformDataClient(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }


        public IEnumerable<Platform> ReturnAllPlatforms()
        {
            Console.WriteLine($"--> Calling gRPC Service to get platforms {_configuration["GrpcPlatformHost"]}");

            var channel = GrpcChannel.ForAddress(_configuration["GrpcPlatformHost"]);
            var client = new GrpcPlatform.GrpcPlatformClient(channel);

            var request = new GetAllRequest();

            try
            {
                var reply = client.GetAllPlatforms(request);

                return _mapper.Map<IEnumerable<Platform>>(reply.Platform);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not call the gRPC Server. {ex.Message}");
                return null;
            }
        }
    }

}