using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using MiniBus.Interfaces;

namespace MiniBus
{
    public class MessageProcessor : IMessageProcessor
    {
        private readonly Dictionary<Type, Action<object>> callbacks;

        public MessageProcessor(Dictionary<Type, Action<Object>> callbacks)
        {
            this.callbacks = callbacks;
        }

        public void Process(TransportMessage message)
        {
            if (message == null)
            {
                return;
            }

            var matchingCallbacks = callbacks.Where(pair => pair.Key.ToString() == message.Type);

            foreach (var callback in matchingCallbacks.Select(pair => pair.Value))
            {
                callback(DeserializeByteArrayToObject(message.Data));
            }
        }

        private static object DeserializeByteArrayToObject(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                var binaryFormatter = new BinaryFormatter();
                return binaryFormatter.Deserialize(stream);
            }
        }
    }
}