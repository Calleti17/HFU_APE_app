using Core.Services;
using Microsoft.Extensions.DependencyInjection;
using MLZ.Core.Models;
using MLZ.Core.Services;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Tests
{
    [TestFixture]
    public class LocalStorageServiceTests : TestsBase
    {
        [Test]
        public async Task TestSaveAndLoad()
        {
            var localStorage = await GetLocalStorage();
            var fisch = CreateFisch();

            Assert.That(fisch.Id, Is.Null);

            var saved = await localStorage.Save(fisch);
            Assert.Multiple(() =>
            {
                Assert.That(saved, "Object was not saved");
                Assert.That(fisch.Id, Is.Not.Zero);
            });

            var loadedFisch = await localStorage.Load(fisch.Id.Value);

            Assert.That(loadedFisch.Id, Is.EqualTo(fisch.Id));
        }

        [Test]
        public async Task TestSaveAndTryLoad()
        {
            var localStorage = await GetLocalStorage();
            var fisch = CreateFisch();

            Assert.That(fisch.Id, Is.Null);

            var saved = await localStorage.Save(fisch);
            Assert.Multiple(() =>
            {
                Assert.That(saved, "Object was not saved");
                Assert.That(fisch.Id, Is.Not.Zero);
            });

            var loadedFisch = await localStorage.TryLoad(fisch.Id.Value);

            Assert.Multiple(() =>
            {
                Assert.That(loadedFisch, Is.Not.Null, $"Could not load item with Id = [{fisch.Id}]");
                Assert.That(loadedFisch!.Id, Is.EqualTo(fisch.Id));
            });
        }

        [Test]
        public async Task TestSaveLoadAndDelete()
        {
            var localStorage = await GetLocalStorage();
            var fisch = CreateFisch();

            Assert.That(fisch.Id, Is.Null);

            var saved = await localStorage.Save(fisch);
            Assert.Multiple(() =>
            {
                Assert.That(saved, "Object was not saved");
                Assert.That(fisch.Id, Is.Not.Zero);
            });

            var loadedFisch = await localStorage.Load(fisch.Id.Value);

            Assert.That(loadedFisch.Id, Is.EqualTo(fisch.Id));

            var deleted = await localStorage.Delete(loadedFisch);

            Assert.That(deleted, "Object was not deleted.");

            var fischWithId = await localStorage.TryLoad(fisch.Id.Value);

            Assert.That(fischWithId, Is.Null, "Object should no longer exist.");
        }

        [Test]
        public async Task TestDeleteAndLoadAll()
        {
            var localStorage = await GetLocalStorage();
            var deleted = await localStorage.DeleteAll();

            Assert.That(deleted, "Could not delete all objects.");

            var fische = await localStorage.LoadAll();

            Assert.That(fische.Count, Is.EqualTo(0));

            for (var i = 0; i < 5; i++)
            {
                var saved = await localStorage.Save(CreateFisch());

                Assert.That(saved, "Object was not saved.");
            }

            fische = await localStorage.LoadAll();

            Assert.That(fische.Count, Is.EqualTo(5));
        }

        protected override IServiceCollection AddServices(IServiceCollection serviceCollection)
        {
            return base.AddServices(serviceCollection)
                .AddSingleton(new LocalStorageSettings { DatabaseFilename = "Maui2024Tests.db3" });
        }

        private async Task<ILocalStorage> GetLocalStorage()
        {
            var serviceProvider = CreateServiceProvider();
            var localStorage = serviceProvider.GetRequiredService<ILocalStorage>();

            await localStorage.Initialize();
            return localStorage;
        }

        private static Fisch CreateFisch()
        {
            return new Fisch
            {
                Name = "TestFisch",
                Lake = "TestLake",
                Method = "TestMethod",
                Date = DateTime.Now
            };
        }
    }
}
