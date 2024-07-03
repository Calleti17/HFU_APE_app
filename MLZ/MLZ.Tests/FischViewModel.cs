using MLZ.Core.Models;
using Core.Tests;
using MLZ.ViewModels;

namespace MLZ.Tests
{
    [TestFixture]
    public class FischViewModelTests : TestsBase
    {
        [Test]
        public async Task TestAddFisch()
        {
            var viewModel = await GetFischViewModel();
            viewModel.SelectedFisch = new Fisch { Name = "TestFisch", Lake = "TestLake", Method = "TestMethod", Date = DateTime.Now };
            await viewModel.AddFischCommand.ExecuteAsync(null);

            
            Assert.That(viewModel.Fische.Count, Is.GreaterThan(3));
            object value = viewModel.Fische.FirstOrDefault(f => f.Name == "TestFisch");
            Assert.That(value, Is.Not.Null);
        }

        [Test]
        public async Task TestEditFisch()
        {
            var viewModel = await GetFischViewModel();
            viewModel.SelectedFisch = viewModel.Fische.FirstOrDefault();
            if (viewModel.SelectedFisch != null)
            {
                viewModel.SelectedFisch.Name = "EditedFisch";
                await viewModel.EditFischCommand.ExecuteAsync(null);

               
                Assert.That(viewModel.Fische.FirstOrDefault(f => f.Id == viewModel.SelectedFisch.Id)?.Name, Is.EqualTo("EditedFisch"));
            }
        }

        [Test]
        public async Task TestDeleteFisch()
        {
            var viewModel = await GetFischViewModel();
            var fischToDelete = viewModel.Fische.FirstOrDefault();
            viewModel.SelectedFisch = fischToDelete;
            await viewModel.DeleteFischCommand.ExecuteAsync(null);

            
            Assert.That(viewModel.Fische.Contains(fischToDelete), Is.False);
        }
        
        private async Task<FischViewModel> GetFischViewModel()
        {
            var serviceProvider = CreateServiceProvider();
            return await GetFischViewModel(serviceProvider);
        }

        private static async Task<FischViewModel> GetFischViewModel(IServiceProvider serviceProvider)
        {
            var result = serviceProvider.GetRequiredService<FischViewModel>();
            await Task.CompletedTask;  
            return result;
        }
    }
}
