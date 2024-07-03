using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Hosting;
using MLZ.Services;
using MLZ.ViewModels;
using System.IO;
using System;

namespace MLZ
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            // Registriere Services und ViewModels
            var services = builder.Services;
            services.AddSingleton<FischDatabase>(sp =>
            {
                var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "fisch.db3");
                return new FischDatabase(dbPath);
            });
            services.AddSingleton<FischService>();
            services.AddTransient<FischViewModel>();

            return builder.Build();
        }
    }
}
