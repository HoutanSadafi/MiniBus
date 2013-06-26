using System;
using System.Messaging;
using MiniBus.Interfaces;

namespace MiniBus
{
    public class MsmqTransport : ITransport
    {
        public void Send(TransportMessage message)
        {
            using (var queue = new MessageQueue(message.Address, false, true, QueueAccessMode.Send))
            {
                queue.Send(message, MessageQueueTransactionType.Automatic);
            }
        }

        public TransportMessage Receive(string address)
        {
            try
            {
                using (var queue = new MessageQueue(address, QueueAccessMode.Receive))
                {
                    queue.Formatter = new XmlMessageFormatter(new[] { typeof(TransportMessage) });
                    using (var message = queue.Receive(TimeSpan.FromSeconds(1)))
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