using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.VMA;
using Database.Abstraction.VMA.Contract;
using Database.VMA;
using Database.VMA.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;
using VMA.MVVM.ViewModels;
using VMA.MVVM.ViewModels.Login;
using VMA.MVVM.Views;

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
            string cs = ConfigurationManager.ConnectionStrings["VMA"].ConnectionString;
            services.AddDbContext<VendorManagementDbContext>(options => options.UseSqlServer(cs));

            services.AddSingleton<IUserBusinessLogic, UserBusinessLogic>();
            services.AddSingleton<IUserRepository, UserRepository>();

            services.AddSingleton<IVendorBusinessLogic, VendorBusinessLogic>();
            services.AddSingleton<IVendorRepository, VendorRepository>();

            //Register services and view models
            services.AddSingleton(x => new LoginViewModel(x.GetRequiredService<IUserBusinessLogic>()));
            services.AddSingleton(x => new LoginView(x.GetRequiredService<LoginViewModel>()));

            services.AddSingleton(x => new MainViewModel(x.GetRequiredService<IUserBusinessLogic>(),
                                                         x.GetRequiredService<IVendorBusinessLogic>()));
            services.AddSingleton(x => new MainView(x.GetRequiredService<MainViewModel>()));
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var loginView = _serviceProvider.GetService<LoginView>();
            loginView!.Show();

            loginView.IsVisibleChanged += (s, ev) =>
            {
                if (loginView.IsVisible == false && loginView.IsLoaded)
                {
                    var mainView = _serviceProvider.GetService<MainView>();
                    mainView!.Show();
                }
            };
        }
    }

}
