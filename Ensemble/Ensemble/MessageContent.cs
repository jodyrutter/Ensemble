using System;

namespace Ensemble
{
    public class MessageContent
    {
        public string Email { get; set; }
        public string Message { get; set; }
        public string Time { get; set; }

        public MessageContent()
        {

        }

        public MessageContent(string email, string message)
        {
            this.Email = email;
            this.Message = message;
            Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public MessageContent(string email, string message, string time)
        {
            this.Email = email;
            this.Message = message;
            this.Time = time;
        }
    }
}