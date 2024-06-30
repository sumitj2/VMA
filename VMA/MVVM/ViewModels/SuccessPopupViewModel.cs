using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace VMA.MVVM.ViewModels
{
    public class SuccessPopupViewModel: ViewModelBase
    {
        private bool _isVisible;
        private string _message;
        private BitmapImage _gifSource;

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                OnPropertyChanged(nameof(IsVisible));
            }
        }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        public BitmapImage GifSource
        {
            get => _gifSource;
            set
            {
                _gifSource = value;
                OnPropertyChanged(nameof(GifSource));
            }
        }

        public SuccessPopupViewModel()
        {
            // Load GIF image
            GifSource = new BitmapImage();
            GifSource.BeginInit();
            GifSource.UriSource = new Uri("pack://application:,,,/YourAssemblyName;component/Resources/success.gif");
            GifSource.EndInit();
        }

        public void ShowPopup(string message, int durationInSeconds)
        {
            Message = message;
            IsVisible = true;
            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(durationInSeconds)
            };
            timer.Tick += (sender, args) =>
            {
                IsVisible = false;
                timer.Stop();
            };
            timer.Start();
        }
    }
}
