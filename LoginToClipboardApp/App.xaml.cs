using LoginClipboardApp.Model;
using LoginClipboardApp.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.IO;
using System.Windows;

namespace LoginClipboardApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        public IConfiguration Configuration { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // make configuration available if other services need it
            services.AddSingleton(Configuration);

            // bind the "Profiles" section to a List<UserCredentialsProfile> and register it with the options system
            //services.AddOptions();
            //services.Configure<List<UserCredentialsProfile>>(Configuration.GetSection("Profiles"));

            services.AddSingleton<AppViewModel>();
            services.AddTransient(typeof(MainWindow));
        }
    }
}
