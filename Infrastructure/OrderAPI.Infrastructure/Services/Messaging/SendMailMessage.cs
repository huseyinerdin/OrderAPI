namespace OrderAPI.Infrastructure.Services.Messaging
{
    public class SendMailMessage
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
