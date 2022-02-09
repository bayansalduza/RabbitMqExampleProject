using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitMqExampleProject
{
    internal class Program
    {
        /// <summary>
        /// Publish
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var email = new EmailTemplate
            {
                Title = "RabbitMQ",
                Message = "RabbitMQ Deneme",
                Email = "rabbitmq@deneme.com"
            };

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "mail", durable: false, exclusive: false, autoDelete: false, arguments: null);

                //Modeli json string formatına dönüştürüyoruz
                string message = JsonConvert.SerializeObject(email);

                //Kuyruğa gönderilecek değeri byte'a çeviriyoruz
                var body = Encoding.UTF8.GetBytes(message);

                //Mesajı RabbitMQ'ya ekliyoruz
                channel.BasicPublish(exchange: "", routingKey: "mail", basicProperties: null, body: body);

                Console.WriteLine("Gönderilen mail içeriği:", message);
            }
            Console.WriteLine(" Mailiniz başarı ile kuyruğa alındı.");
            Console.ReadLine();
        }
    }
    public class EmailTemplate
    {
        public string Email { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
