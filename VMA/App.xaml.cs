using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.VMA;
using Database.Abstraction.VMA.Contract;
using Database.VMA;
using Database.VMA.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Configuration;
using System.Data;
using System.Diagnostics;
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

        /// <summary>
        /// how to apply validations on multiple field with minimun coding and using standard practices in wpf
        /// </summary>
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

            services.AddSingleton<IVendorServiceBusinessLogic, VendorServiceBusinessLogic>();
            services.AddSingleton<IVendorServiceRepository, VendorServiceRepository>();

            services.AddSingleton<IVendorDetailsBusinessLogic, VendorDetailsBusinessLogic>();
            services.AddSingleton<IVendorDetailsRepository, VendorDetailsRepository>();

            services.AddSingleton<IVendorPaymentBusinessLogic, VendorPaymentBusinessLogic>();
            services.AddSingleton<IVendorPaymentRepository, VendorPaymentRepository>();

            services.AddSingleton<IVenderPaymentNotesBusinessLogic, VenderPaymentNotesBusinesslogic>();
            services.AddSingleton<IVenderPaymentNotesRepository, VenderPaymentNotesRepository>();

            services.AddSingleton<IInvoiceDetailsBusinessLogic, InvoiceDetailsBusinessLogic>();
            services.AddSingleton<IInvoiceDetailsRepository, InvoiceDetailsRepository>();

            services.AddSingleton<IGstcalculationMasterBusinessLogic, GstcalculationMasterBusinessLogic>();
            services.AddSingleton<IGstcalculationMasterRepository, GstcalculationMasterRepository>();

            services.AddSingleton<IReportExportToExcelPaymentNote, ReportExportToExcelPaymentNote>();
            //services.AddSingleton(x =>new VenderPaymentNotesBusinesslogic(x.GetRequiredService<IVenderPaymentNotesRepository>(),
            //                                                              x.GetRequiredService<IInvoiceDetailsBusinessLogic>()));
            //Register services and view models
            services.AddSingleton(x => new LoginViewModel(x.GetRequiredService<IUserBusinessLogic>()));
            services.AddSingleton(x => new LoginView(x.GetRequiredService<LoginViewModel>()));

            services.AddSingleton(x => new MainViewModel(x.GetRequiredService<IUserBusinessLogic>(),
                                                         x.GetRequiredService<IVendorBusinessLogic>(),
                                                         x.GetRequiredService<IVendorServiceBusinessLogic>(),
                                                         x.GetRequiredService<IVendorDetailsBusinessLogic>(),
                                                         x.GetRequiredService<IVendorPaymentBusinessLogic>(),
                                                         x.GetRequiredService<IVenderPaymentNotesBusinessLogic>(),
                                                         x.GetRequiredService<IGstcalculationMasterBusinessLogic>(),
                                                         x.GetRequiredService<IReportExportToExcelPaymentNote>()));
            services.AddSingleton(x => new MainView(x.GetRequiredService<MainViewModel>()));
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("d:\\llogs", rollingInterval: RollingInterval.Minute)
                .CreateLogger();

            // Handle UI thread exceptions
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;

            // Handle non-UI thread exceptions
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

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

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            // Log the exception
            LogException(e.Exception);

            // Show a message to the user
            MessageBox.Show("An unhandled exception occurred: " + e.Exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            // Prevent default unhandled exception processing
            e.Handled = true;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // Log the exception
            LogException(e.ExceptionObject as Exception);

            // Show a message to the user
            MessageBox.Show("An unhandled exception occurred: " + (e.ExceptionObject as Exception)?.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            // Log the exception
            LogException(e.Exception);

            // Show a message to the user
            MessageBox.Show("An unobserved task exception occurred: " + e.Exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            // Prevent the exception from crashing the application
            e.SetObserved();
        }

        private void LogException(Exception? ex)
        {
            // Implement logging logic here
            // For example, write to a file, send to a logging server, etc.
            Debug.WriteLine(ex.ToString());
        }
    }

}
