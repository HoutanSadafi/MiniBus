using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using MiniBus.Interfaces;

namespace MiniBus
{
    public class Bus : IBus
    {
        private readonly ITransport transport;

        public Bus(ITransport transport)
        {
            this.transport = transport;
        }

        public void Send<T>(T data, string address)
        {
            var message = CreateTransportMessage(data);
            message.Address = address;

            transport.Send(message);
        }

        public object Get(string address)
        {
            var message = transport.Get(address);
            if (message == null)
            {
                return null;
            }

            return DeserializeByteArrayToObject(message.Data);
        }

        private TransportMessage CreateTransportMessage<T>(T data)
        {
            var memory = SerializeToByteArray(data);


            var message = new TransportMessage
                              {
                                  Type = typeof(T).ToString(),
                                  Data = memory
                              };

            return message;
        }

        private static byte[] SerializeToByteArray<T>(T data)
        {
            byte[] memory;

            using (var stream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(stream, data);
                memory = stream.ToArray();
            }

            return memory;
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