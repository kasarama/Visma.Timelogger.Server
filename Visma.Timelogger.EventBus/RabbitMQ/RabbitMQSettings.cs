namespace Visma.Timelogger.EventBus.RabbitMQ
{
    public class RabbitMQSettings
    {
        public static string Position = "RabbitMQSettings";
        public string HostName { get; set; } = string.Empty;
        public int HostPort { get; set; }
        public string VirtualHost { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
