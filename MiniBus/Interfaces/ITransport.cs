namespace MiniBus.Interfaces
{
    public interface ITransport
    {
        void Send(TransportMessage message);
        TransportMessage Get(string address);
    }
}