namespace LZ1.Core.Services
{
    public interface ICounterState
    {
        int Count { get; }

        void Increment();
        void Decrement(); 
    }
}
