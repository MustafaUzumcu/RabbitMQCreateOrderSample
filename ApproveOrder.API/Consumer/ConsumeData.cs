using OrderDB.Constants;
using OrderDB.Context;
using OrderDB.Models;
using RabbitMQ.Client.Events;
using RabbitMQCore.Consumer;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ApproveOrder.API.Consumer
{
    public class ConsumeData
    {
        private RabbitMQConsumer<Order> _orderConsumer;
        private RabbitMQConsumer<Order> _orderConsumer1;

        public void StartConsume()
        {
            // Channel 1
            _orderConsumer = new RabbitMQConsumer<Order>
            {
                DataReceivedAction = OrderConsumerReceived
            };
            _orderConsumer.Consume(RabbitMQQueueNames.ApproveOrder.ToString());

            // Channel 2
            _orderConsumer1 = new RabbitMQConsumer<Order>
            {
                DataReceivedAction = OrderConsumerReceived1
            };
            _orderConsumer1.Consume(RabbitMQQueueNames.ApproveOrder.ToString());
        }

        public async void OrderConsumerReceived(Order order, BasicDeliverEventArgs e)
        {
            try
            {
                var result = await ApproveOrder(order);
                System.Diagnostics.Debug.WriteLine("Channel 1 " + order.Id);
            }
            catch (Exception ex)
            {
                // TODO: Exception Log.
            }
            finally
            {
                _orderConsumer.BasicAck(e.DeliveryTag);
            }
        }

        public void OrderConsumerReceived1(Order order, BasicDeliverEventArgs e)
        {
            //_orderConsumer1.BasicAck(e.DeliveryTag);
            System.Diagnostics.Debug.WriteLine("Channel 2 " + order.Id);
        }

        private Task<bool> ApproveOrder(Order order)
        {
            var task = Task.Run(() =>
            {
                // Ex1: Check ProductId
                var orderProductIdList = order.OrderDetails.Select(od => od.ProductId).ToList();
                var productIdList = OrderContext.Products.Select(o => o.Id).ToList();
                var checkProductIdResult = orderProductIdList.Except(productIdList).ToList();
                if (checkProductIdResult.Any())
                {
                    order.OrderStatus = OrderStatus.Failed;
                    order.OrderStatusText = "ProductId Not Found.";
                    throw new Exception("ProductId Not Found.");
                }

                // Ex1: Check AvailableStock
                foreach (var orderDetail in order.OrderDetails)
                {
                    if (OrderContext.Products.FirstOrDefault(p => p.Id == orderDetail.ProductId).AvailableStock < orderDetail.Quantity)
                    {
                        order.OrderStatus = OrderStatus.Failed;
                        order.OrderStatusText = "ProductId Not Found.";
                        throw new Exception("Available Stock Not Found.");
                    }
                }

                order.OrderStatus = OrderStatus.Approved;
                order.OrderStatusText = "Product Approved.";

                return true;
            });
            return task;
        }
    }
}