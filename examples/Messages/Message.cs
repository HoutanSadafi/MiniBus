using System;

namespace Messages
{
    [Serializable]
    public class Message
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public string Data { get; set; }
    }
}
