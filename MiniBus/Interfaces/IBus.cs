namespace MiniBus.Interfaces
{
    public interface IBus
    {
        void Send<T>(T data, string address);
        object Get(string address);
    }
}