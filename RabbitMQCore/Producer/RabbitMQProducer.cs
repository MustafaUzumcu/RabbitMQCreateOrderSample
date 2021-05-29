using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQCore.Base;
using System;
using System.Text;

namespace RabbitMQCore.Producer
{
    public class RabbitMQProducer<T>
    {
        #region Singleton

        private static readonly Lazy<RabbitMQProducer<T>> _instance =
            new Lazy<RabbitMQProducer<T>>(() => new RabbitMQProducer<T>());

        public static RabbitMQProducer<T> Instance => _instance.Value;

        #endregion Singleton


        #region Methods

        public void Publish(string queueName, T message, bool durable = true)
        {
            using IModel channel = RabbitMQConnection.Instance.GetChannel(queueName, durable);
            var messageArray = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            IBasicProperties basicProperties = null;
            if (durable)
            {
                basicProperties = channel.CreateBasicProperties();
                basicProperties.Persistent = true;
            }
            channel.BasicPublish(exchange: string.Empty, routingKey: queueName, basicProperties: basicProperties,
                body: messageArray);
        }

        #endregion Methods
    }
}