using RabbitMQ.Client;
using System;

namespace RabbitMQCore.Base
{
    public class RabbitMQConnection
    {
        #region Singleton

        private static readonly Lazy<RabbitMQConnection> _instance =
            new Lazy<RabbitMQConnection>(() => new RabbitMQConnection());

        public static RabbitMQConnection Instance => _instance.Value;

        #endregion Singleton


        #region Members

        private static readonly object _lockObject = new object();

        private IConnection _connection { get; set; }

        #endregion Members


        #region Constructor

        public RabbitMQConnection()
        {
            Configure();
        }

        #endregion Constructor


        #region Methods

        public void Configure()
        {
            lock (_lockObject)
            {
                var connectionFactory = new ConnectionFactory()
                {
                    HostName = Environment.GetEnvironmentVariable("RABBITMQ_URL"),
                    UserName = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME"),
                    Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD"),
                };
                _connection = connectionFactory.CreateConnection();
            }
        }

        public IModel GetChannel(string queueName, bool durable = true)
        {
            IModel channel = _connection.CreateModel();
            channel.QueueDeclare(queue: queueName, durable: durable, exclusive: false,
                autoDelete: false, arguments: null);
            return channel;
        }

        #endregion Methods
    }
}