namespace LZ1.Core.Services
{
    internal class CounterState : ICounterState
    {
        public int Count { get; private set; }

        public void Increment()
        {
            Count += 1;
        }

        public void Decrement()
        {
            if (Count > 0)
            {
                Count -= 1;
            }
        }
    }
}
