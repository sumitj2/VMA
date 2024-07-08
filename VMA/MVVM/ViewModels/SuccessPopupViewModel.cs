using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using VMA.Enums;
using VMA.MVVM.Views;

namespace VMA.MVVM.ViewModels
{
    public class SuccessPopupViewModel: ViewModelBase
    {
        private static SuccessPopupViewModel _instance;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static SuccessPopupViewModel Instance 
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SuccessPopupViewModel();
                }

                return _instance;
            }
        }

        private string _message;

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        private string _header;

        public string Header
        {
            get => _header;
            set
            {
                _header = value;
                OnPropertyChanged(nameof(Header));
            }
        }

        private NotificationType _notificationType;

        public NotificationType TypeOfNotification
        {
            get => _notificationType;
            set
            {
                _notificationType = value;
                OnPropertyChanged(nameof(TypeOfNotification));
            }
        }

        public SuccessPopupViewModel()
        {
            
        }

        public void ShowPopup(NotificationType notificationType, string message)
        {
            Message = message;
            TypeOfNotification = notificationType;
            Header = notificationType.ToString();

            Window window = (Window)Activator.CreateInstance(typeof(SuccessPopup))!;
            window?.Show();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5); // Set the interval to 2 seconds
            timer.Tick += (s, args) =>
            {
                window?.Close(); // Close the popup
                timer.Stop(); // Stop the timer
            };

            timer.Start();
        }
    }
}
