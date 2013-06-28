using System;

namespace MiniBus
{
    public class TransportMessage
    {
        public String Type { get; set; }
        public byte[] Data { get; set; }
        public string Address { get; set; }
    }
}