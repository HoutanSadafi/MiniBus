using System;
using System.Collections.Generic;
using MiniBus.Interfaces;

namespace MiniBus
{
    public static class MiniBus
    {
        public static IBus Msmq()
        {
            return new Bus(new MsmqTransport());
        }

        public static BusPoller Listener(this IBus bus, string address, Dictionary<Type, Action<object>> callbacks)
        {
            return new BusPoller(new MsmqTransport(), new MessageProcessor(callbacks), 1, address);
        }
    }
}
