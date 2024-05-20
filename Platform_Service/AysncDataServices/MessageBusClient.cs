using Platform_Service.Dto;
using RabbitMQ.Client;

namespace Platform_Service.AysncDataServices
{

    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"])
            };
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
                Console.WriteLine("--> Connected to MessageBus");
                ListenForMessage();
            }
            catch (System.Exception)
            {

                throw;
            }



        }


        public void PublishNewPlatform(PlatformPublishDto platformPublishDto)
        {
            throw new NotImplementedException();
        }


        private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");

        }
        private void ListenForMessage()
        {
            throw new NotImplementedException();
        }



    }



}
