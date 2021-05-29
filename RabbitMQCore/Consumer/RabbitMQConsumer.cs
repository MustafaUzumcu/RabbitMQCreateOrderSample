using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQCore.Base;
using System;
using System.Text;

namespace RabbitMQCore.Consumer
{
    public class RabbitMQConsumer<T>
    {
        #region Members

        private IModel _channel;

        #endregion Members


        #region Events

        public Action<T, BasicDeliverEventArgs> DataReceivedAction { get; set; }

        #endregion Events


        #region Methods

        public void Consume(string queueName, bool autoAck = false, bool durable = true, ushort prefetchCount = 1)
        {
            _channel = RabbitMQConnection.Instance.GetChannel(queueName, durable);
            if (prefetchCount != 0)
                _channel.BasicQos(prefetchSize: 0, prefetchCount: prefetchCount, global: false);

            var eventingBasicConsumer = new EventingBasicConsumer(_channel);
            eventingBasicConsumer.Received += DocumentConsumerOnReceived;
            _channel.BasicConsume(queue: queueName, autoAck: autoAck, consumer: eventingBasicConsumer);

        }

        private void DocumentConsumerOnReceived(object sender, BasicDeliverEventArgs e)
        {
            var jsonData = Encoding.UTF8.GetString(e.Body.ToArray());
            DataReceivedAction(JsonConvert.DeserializeObject<T>(jsonData), e);
        }

        public void BasicAck(ulong deliveryTag, bool multiple = false)
        {
            _channel.BasicAck(deliveryTag, multiple);
        }

        public void BasicNack(ulong deliveryTag, bool multiple = false, bool requeue = true)
        {
            _channel.BasicNack(deliveryTag, multiple, requeue);
        }

        #endregion Methods
    }
}