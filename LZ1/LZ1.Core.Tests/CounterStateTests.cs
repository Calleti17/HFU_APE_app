using LZ1.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace LZ1.Core.Tests
{
    [TestFixture]
    public class CounterStateTests : TestsBase
    {
        [Test]
        public void TestIncrement()
        {
            var provider = CreateProvider();
            var counterState = provider.GetRequiredService<ICounterState>();

            Assert.That(counterState.Count, Is.EqualTo(0));

            counterState.Increment();

            Assert.That(counterState.Count, Is.EqualTo(1));
        }

        [Test]
        public void TestDecrement()
        {
            var provider = CreateProvider();
            var counterState = provider.GetRequiredService<ICounterState>();

            counterState.Increment();
            Assert.That(counterState.Count, Is.EqualTo(1));

            counterState.Decrement();
            Assert.That(counterState.Count, Is.EqualTo(0));

           
            counterState.Decrement();
            Assert.That(counterState.Count, Is.EqualTo(0));
        }
    }
}
