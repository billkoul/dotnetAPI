using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Application.Queries;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace API.Config
{
    public class FileUpdateSender : IFileUpdateSender
    {
        private readonly string _hostname;
        private readonly string _password;
        private readonly string _queueName;
        private readonly string _username;
        private IConnection _connection;

        public FileUpdateSender(IOptions<RabbitMqConfiguration> rabbitMqOptions)
        {
            _queueName = rabbitMqOptions.Value.QueueName;
            _hostname = rabbitMqOptions.Value.Hostname;
            _username = rabbitMqOptions.Value.UserName;
            _password = rabbitMqOptions.Value.Password;

            CreateConnection();
        }

        public async Task SendFile(CreateUpload file)
        {
            if (ConnectionExists())
            {
                using var channel = _connection.CreateModel();
                await Task.FromResult(channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null));

                var json = JsonConvert.SerializeObject(file);
                var body = Encoding.UTF8.GetBytes(json);

                channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);

                _connection.Close();
            }
        }

        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostname,
                    UserName = _username,
                    Password = _password
                };
                _connection = factory.CreateConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not create connection: {ex.Message}");
            }
        }

        private bool ConnectionExists()
        {
            if (_connection != null)
            {
                return true;
            }

            CreateConnection();

            return _connection != null;
        }
    }
}
