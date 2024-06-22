using Microsoft.Maui.Controls;

namespace MLZ
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }
    }
}
