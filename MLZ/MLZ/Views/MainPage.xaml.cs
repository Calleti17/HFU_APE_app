using Microsoft.Maui.Controls;
using MLZ.ViewModels;

namespace MLZ.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            var viewModel = new FischViewModel();
            BindingContext = viewModel;
        }
    }
}
