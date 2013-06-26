namespace MiniBus.Interfaces
{
    public interface ITransport
    {
        void Send(TransportMessage message);
        TransportMessage Receive(string address);
    }
}