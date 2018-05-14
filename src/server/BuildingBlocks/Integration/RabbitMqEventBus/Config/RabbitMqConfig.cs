namespace RabbitMqEventBus.Config
{
    public class RabbitMqConfig
    {
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ExhangeName { get; internal set; }
        public string ExchangeType { get; internal set; }
        public string OutgoingQueueName { get; internal set; }
        public string IncomingQueueName { get; internal set; }
        public int MaxRetries { get; internal set; }
    }
}
