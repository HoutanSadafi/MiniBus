using System;
using System.Messaging;
using MiniBus.Interfaces;

namespace MiniBus
{
    public class MsmqTransport : ITransport
    {
        private readonly uint timeout;
        
        public MsmqTransport() : this(1)
        {}

        public MsmqTransport(uint timeout)
        {
            this.timeout = timeout;
        }

        public void Send(TransportMessage message)
        {
            using (var queue = new MessageQueue(message.Address, false, true, QueueAccessMode.Send))
            {
                queue.Send(message, MessageQueueTransactionType.Automatic);
            }
        }

        public TransportMessage Get(string address)
        {
            try
            {
                using (var queue = new MessageQueue(address, QueueAccessMode.Receive))
                {
                    queue.Formatter = new XmlMessageFormatter(new[] { typeof(TransportMessage) });
                    using (var message = queue.Receive(TimeSpan.FromSeconds(timeout)))
                    {
                        if (message == null)
                        {
                            return null;
                        }

                        return message.Body as TransportMessage;
                    }
                }
            }
            catch (MessageQueueException exception)
            {
                if (exception.MessageQueueErrorCode == MessageQueueErrorCode.IOTimeout)
                {
                    return null;
                }

                throw;
            }
            
        }
    }
}