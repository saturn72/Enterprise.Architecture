namespace Programmer.Test.Framework.Q
{
    internal class RabbitMqConfig
    {
        public string HostName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ExchangeName { get; set; }
        public string ExchangeType { get; set; }
        public string OutgoingQueueName { get; set; }
        public string IncomingQueueName { get; set; }
    }
}
