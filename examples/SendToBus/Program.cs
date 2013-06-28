using System;
using Messages;

namespace SendToBus
{
    class Program
    {
        static void Main(string[] args)
        {
            const string address = ".\\Private$\\minibustest";

            var bus = MiniBus.MiniBus.Msmq();

            Console.WriteLine("Hit 'Enter' to send a message. To exit, Ctrl + C");

            while (Console.ReadLine() != null)
            {
                var guid = Guid.NewGuid();
                var person = new Message {Id = guid, Created = DateTime.Now, Data = "My data"};
                bus.Send(person, address);
                Console.WriteLine("Message {0} sent to {1}", guid, address);
            }
        }
    }
}
