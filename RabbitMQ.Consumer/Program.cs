using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Extensions;
using RabbitMQ.Extensions.Models;
using RabbitMQ.Producer;
using System;
using System.Text;

namespace RabbitMQ.Consumer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            while (true)
            {
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "datawrite", durable: false, exclusive: false, autoDelete: false, arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += Consumer_Received;
                    channel.BasicConsume(queue: "datawrite", autoAck: true, consumer: consumer);
                    Console.ReadLine();
                }
            }
        }

        private static void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var data = Encoding.UTF8.GetString(e.Body.Span);
            DataModel dataModel = JsonConvert.DeserializeObject<DataModel>(data);
            new ExtensionsFileIO().WriteFile(dataModel);
            Console.WriteLine($"Data kayıt edildi, dosya yolu : {new ExtensionsFileIO().Path}");
        }
    }
}
