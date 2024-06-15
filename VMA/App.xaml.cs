using Database.VMA;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace VMA
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;
        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }
        private void ConfigureServices(IServiceCollection services)
        {
            string cs = System.Configuration.ConfigurationManager.ConnectionStrings["VMA"].ConnectionString;
            // Register services and view models
            services.AddSingleton<MainWindow>();
            services.AddDbContext<VendorManagementDbContext>(options => options.UseSqlServer(cs));
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow!.Show();
        }
    }

}
