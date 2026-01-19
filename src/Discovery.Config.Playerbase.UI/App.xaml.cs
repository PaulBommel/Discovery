using Discovery.Config.Playerbase.UI.ViewModels;
using Discovery.Config.Playerbase.UI.Views;
using Discovery.Darkstat;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.Data;
using System.Net;
using System.Net.Mime;
using System.Windows;

namespace Discovery.Config.Playerbase.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            var services = new ServiceCollection();
            services.AddHttpClient();
            services.AddHttpClient<BaseItemRecipeClient>(client =>
            {
                client.BaseAddress = new("https://discoverygc.com/gameconfigpublic/");
                client.DefaultRequestHeaders.Add(HttpRequestHeader.Accept.ToString(), MediaTypeNames.Text.Plain);
            });
            services.AddHttpClient<BaseModuleRecipeClient>("", client =>
            {
                client.BaseAddress = new("https://discoverygc.com/gameconfigpublic/");
                client.DefaultRequestHeaders.Add(HttpRequestHeader.Accept.ToString(), MediaTypeNames.Text.Plain);
            });
            services.AddHttpClient<DarkstatClient>(client =>
            {
                client.BaseAddress = new("https://darkstat.dd84ai.com");
                client.DefaultRequestHeaders.Add(HttpRequestHeader.Accept.ToString(), MediaTypeNames.Application.Json);
            });

            services.AddTransient<BaseModuleRecipeClient>();
            services.AddTransient<BaseItemRecipeClient>();
            services.AddTransient<DarkstatClient>();

            services.AddTransient<MainViewModel>();
            services.AddTransient<MainView>();

            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var view = _serviceProvider.GetRequiredService<MainView>();
            view.Show();
        }
    }

}
