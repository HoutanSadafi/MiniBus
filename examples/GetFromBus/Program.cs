using System;
using Messages;

namespace GetFromBus
{
    class Program
    {
        static void Main(string[] args)
        {
            const string address = ".\\Private$\\minibustest";

            var bus = MiniBus.MiniBus.Msmq();

            Console.WriteLine("Hit 'Enter' to retrieve a message. To exit, Ctrl + C");

            while (Console.ReadLine() != null)
            {

                var msg = bus.Get(address) as Message;
                if (msg != null)
                {
                    Console.WriteLine("Message receieved: {0}, {1}, {2}", msg.Id, msg.Created, msg.Data);
                } else
                {
                    Console.WriteLine("No message");
                }
            }
        }
    }
}
