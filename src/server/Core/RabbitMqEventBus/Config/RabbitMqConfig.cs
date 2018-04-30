namespace RabbitMqEventBus.Config
{
    public class RabbitMqConfig
    {
        public string BrokerName { get; internal set; }
        public string ExchangeType { get; internal set; }
        public string QueueName { get; internal set; }
        public int MaxRetries { get; internal set; }
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
