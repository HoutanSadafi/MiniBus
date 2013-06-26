namespace MiniBus.Interfaces
{
    public interface IMessageProcessor
    {
        void Process(TransportMessage message);
    }
}