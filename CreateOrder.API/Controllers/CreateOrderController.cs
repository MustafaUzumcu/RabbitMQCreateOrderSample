using Microsoft.AspNetCore.Mvc;
using OrderDB.Models;
using RabbitMQCore.Producer;
using System;
using System.Collections.Generic;
using System.Threading;

namespace CreateOrder.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateOrderController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
                var randomProductId = new Random();
                int rNumber = randomProductId.Next(1, 10);
                var order = new Order
                {
                    Id = i,
                    OrderStatus = OrderStatus.Created,
                    OrderDetails = new List<OrderDetail>
                    {
                        new OrderDetail
                        {
                            Id = i,
                            ProductId = rNumber,
                            Quantity = rNumber
                        }
                    }
                };

               RabbitMQProducer<Order>.Instance.Publish(OrderDB.Constants.RabbitMQQueueNames.ApproveOrder.ToString(), order);
            }

            return "Order Created.";
        }
    }
}