using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Api.Orders.Models;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

namespace EShop.Api.Orders.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        [HttpPost]
        [Route("Book")]
        public IActionResult Book(Order order)
        {
            var message = $"{order.CustomerId};{order.TourName};{order.Email}";

            SendMessageFanout(message);
            SendMessageDirect("EShop.Order", message);
            SendMessageTopic("EShop.Order", message);

            var headers = new Dictionary<string, object>()
            {
                { "subject" , "tour"},
                { "action" , "Booked"}
            };

            SendMessageHeaders(headers, message);
            return Ok();
        }





        private void SendMessageFanout(string message)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.ExchangeDeclare("orderappFanoutExchange", ExchangeType.Fanout, true);


            var bytes = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish("orderappFanoutExchange", "", null, bytes);

            channel.Close();
            connection.Close();
        }

        private void SendMessageDirect(string routingKey, string message)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.ExchangeDeclare("orderappDirectExchange", ExchangeType.Direct, true);


            var bytes = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish("orderappDirectExchange", routingKey, null, bytes);

            channel.Close();
            connection.Close();
        }

        private void SendMessageTopic(string routingKey, string message)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.ExchangeDeclare("orderappTopicExchange", ExchangeType.Topic, true);


            var bytes = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish("orderappTopicExchange", routingKey, null, bytes);

            channel.Close();
            connection.Close();
        }

        private void SendMessageHeaders(IDictionary<string, object> headers, string message)
        {
            var connectionFactory = new ConnectionFactory()
            {
                UserName = "guest",
                Password = "guest",
                HostName = "localhost",
                Port = 5672
            };

            var connection = connectionFactory.CreateConnection();
            var channel = connection.CreateModel();
            var properties = channel.CreateBasicProperties();
            properties.Persistent = false;           
            properties.Headers = headers;
            var bytes = Encoding.UTF8.GetBytes(message);

            channel.ExchangeDeclare("orderappHeadersExchange", ExchangeType.Headers, true); 
            channel.BasicPublish("orderappHeadersExchange", "", properties, bytes);

            channel.Close();
            connection.Close();
        }
    }
}