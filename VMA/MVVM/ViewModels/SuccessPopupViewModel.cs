using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
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

        private bool _isOKbtnVisible;

        public bool IsOKbtnVisible
        {
            get => _isOKbtnVisible;
            set
            {
                _isOKbtnVisible = value;
                OnPropertyChanged(nameof(IsOKbtnVisible));
            }
        }

        /// <summary>
        /// New Document commmnad
        /// </summary>
        private ViewModelCommand closePopupView;

        /// <summary>
        /// Enroll Command
        /// </summary>
        public ICommand ClosePopupView
        {
            get
            {
                if (this.closePopupView == null)
                {
                    this.closePopupView = new ViewModelCommand(c => this.ClosePopup());
                }

                return this.closePopupView;
            }
        }

        public SuccessPopupViewModel()
        {
            Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Into the constructor", this.GetType().Name, MethodBase.GetCurrentMethod().Name));
        }

        Window Window;

        public void ShowPopup(NotificationType notificationType, string message, bool isAutomaticClosed,bool isOkBtnVisible = false)
        {
            Message = message;
            TypeOfNotification = notificationType;
            Header = notificationType.ToString();
            IsOKbtnVisible = isOkBtnVisible;

            Window = (Window)Activator.CreateInstance(typeof(SuccessPopup))!;

            Window?.Show();

            if (isAutomaticClosed)
            {
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(5); // Set the interval to 2 seconds
                
                timer.Start();

                timer.Tick += (s, args) =>
                {
                    Window?.Close(); // Close the popup
                    timer.Stop(); // Stop the timer
                };

            }
        }

        public void ClosePopup()
        {
            Window?.Close();
        }
    }
}
