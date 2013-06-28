using System;
using System.Collections.Generic;
using Messages;
using MiniBus;

namespace PollingBus
{
    class Program
    {
        static void Main(string[] args)
        {
            const string address = ".\\Private$\\minibustest";

            var callbacks = new Dictionary<Type, Action<Object>> { { typeof(Message), o => Handle((Message)o) } };
            var poller = MiniBus.MiniBus.Msmq().Listener(address, callbacks);
            poller.Start();
            Console.WriteLine("Begin Listening for messages at {0}", address);

            Console.ReadLine();
            poller.Stop();
        }

        static void Handle(Message msg)
        {
            Console.WriteLine("Message receieved: {0}, {1}, {2}", msg.Id, msg.Created, msg.Data);
        }
    }
}
