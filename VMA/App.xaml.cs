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
using VMA.MVVM.Views;

namespace VMA
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;
        private VendorManagementDbContext _vendorManagementDbContext;
        private IUserRepository _userRepository;
        private IUserBusinessLogic _userBusinessLogic;
        public App()
        {
            var serviceCollection = new ServiceCollection();
            _vendorManagementDbContext = new VendorManagementDbContext();
            _userRepository = new UserRepository(_vendorManagementDbContext);
            _userBusinessLogic = new UserBusinessLogic(_userRepository);

            ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }
        
        private void ConfigureServices(IServiceCollection services)
        {
            string cs = System.Configuration.ConfigurationManager.ConnectionStrings["VMA"].ConnectionString;
           // services.AddDbContext<VendorManagementDbContext>(options => options.UseSqlServer(cs));

            //services.AddSingleton<IUserBusinessLogic, UserBusinessLogic>();
          //  services.AddSingleton<IUserRepository, UserRepository>();
            // Register services and view models
             
           
            services.AddSingleton(x=>new LoginView(_userBusinessLogic));
            services.AddSingleton(x => new MainView(_userBusinessLogic));

        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var mainWindow = _serviceProvider.GetService<LoginView>();
            mainWindow!.Show();
            var loginView = new LoginView(_userBusinessLogic);
            loginView.Show();
            loginView.IsVisibleChanged += (s, ev) =>
            {
                if (loginView.IsVisible == false && loginView.IsLoaded)
                {
                    var mainView = new MainView(_userBusinessLogic);
                    mainView.Show();
                    loginView.Close();

                }
            };
        }
    }

}
