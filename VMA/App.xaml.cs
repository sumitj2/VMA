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

        #region repositories

        private IUserRepository _userRepository;
        private IVendorRepository _vendorRepository;
        private IVendorDetailsRepository _vendorDetailsRepository;
        private IVendorPaymentRepository _vendorPaymentRepository;
        private IVendorServiceRepository _vendorServiceRepository;
        private IGstcalculationMasterRepository _gstcalculationMasterRepository;
        private IInvoiceDetailsRepository _invoiceDetailsRepository;
        private IVenderPaymentNotesRepository _vendorPaymentNotesRepository;

        #endregion

        #region BusinessLayer

        private IUserBusinessLogic _userBusinessLogic;
        private IVendorBusinessLogic _vendorBusinessLogic;
        private IVendorDetailsBusinessLogic _vendorDetailsBusinessLogic;
        private IVendorPaymentBusinessLogic _vendorPaymentBusinessLogic;
        private IVendorServiceBusinessLogic _vendorServiceBusinessLogic;
        private IGstcalculationMasterBusinessLogic _gstcalculationMasterBusinessLogic;
        private IInvoiceDetailsBusinessLogic _invoiceDetailsBusinessLogic;
        private IVenderPaymentNotesBusinessLogic _vendorPaymentNotesBusinessLogic;
        #endregion

        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            _vendorManagementDbContext = new VendorManagementDbContext();
            InitializeRepositories();
            InitializeBusinessLogic();

            //string cs = System.Configuration.ConfigurationManager.ConnectionStrings["VMA"].ConnectionString;
            //services.AddDbContext<VendorManagementDbContext>(options => options.UseSqlServer(cs));

            //services.AddSingleton<IUserBusinessLogic, UserBusinessLogic>();
            //services.AddSingleton<IUserRepository, UserRepository>();
            //Register services and view models            

            services.AddSingleton(x => new LoginView(_userBusinessLogic));
            //services.AddSingleton(x => new VendorsView(_vendorBusinessLogic));
            services.AddSingleton(x => new MainView(_userBusinessLogic));

        }

        private void InitializeRepositories()
        {
            _userRepository = new UserRepository(_vendorManagementDbContext);
            _vendorDetailsRepository = new VendorDetailsRepository(_vendorManagementDbContext);
            _vendorRepository = new VendorRepository(_vendorManagementDbContext);
            _vendorPaymentRepository = new VendorPaymentRepository(_vendorManagementDbContext);
            _vendorServiceRepository = new VendorServiceRepository(_vendorManagementDbContext);
            _gstcalculationMasterRepository = new GstcalculationMasterRepository(_vendorManagementDbContext);
            _invoiceDetailsRepository = new InvoiceDetailsRepository(_vendorManagementDbContext);
            _vendorPaymentNotesRepository = new VenderPaymentNotesRepository(_vendorManagementDbContext);
        }

        private void InitializeBusinessLogic()
        {
            _userBusinessLogic = new UserBusinessLogic(_userRepository);
            _vendorBusinessLogic = new VendorBusinessLogic(_vendorRepository);
            _gstcalculationMasterBusinessLogic = new GstcalculationMasterBusinessLogic(_gstcalculationMasterRepository);
            _invoiceDetailsBusinessLogic = new InvoiceDetailsBusinessLogic(_invoiceDetailsRepository);
            _vendorDetailsBusinessLogic = new VendorDetailsBusinessLogic(_vendorDetailsRepository);
            _vendorPaymentNotesBusinessLogic = new VenderPaymentNotesBusinesslogic(_vendorPaymentNotesRepository);
            _vendorServiceBusinessLogic = new VendorServiceBusinessLogic(_vendorServiceRepository);
            _vendorPaymentBusinessLogic=new VendorPaymentBusinessLogic(_vendorPaymentRepository);
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
