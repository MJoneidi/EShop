using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EShop.BackOffice
{
    class Program
    {
        static void Main(string[] args)
        {

            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare("backOfficeQueue", true, false, false);
            channel.QueueBind("backOfficeQueue", "orderappTopicExchange", "EShop.*");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, eventArgs) =>
            {
                var msg = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
                Console.WriteLine($"Topic Exchange - {eventArgs.RoutingKey}: {msg}");
            };

            channel.BasicConsume("backOfficeQueue", true, consumer);

            Console.ReadLine();

            channel.Close();
            connection.Close();
            
        }
    }
}
