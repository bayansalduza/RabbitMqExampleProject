using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Extensions;
using RabbitMQ.Extensions.Models;
using System;
using System.Text;

namespace RabbitMQ.Producer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string path = Console.ReadLine();
            if (!string.IsNullOrEmpty(path))
            {
                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "datawrite", durable: false, exclusive: false, autoDelete: false, arguments: null);
                    DataModel datamodel = new ExtensionsFileIO().ReadFile(@path);
                    string data = JsonConvert.SerializeObject(datamodel);

                    var body = Encoding.UTF8.GetBytes(data);

                    channel.BasicPublish(exchange: "", routingKey: "datawrite", basicProperties: null, body: body);

                    Console.WriteLine("Data kuyruğa alındı");
                }
                Console.ReadLine();
            }
        }
    }
}
