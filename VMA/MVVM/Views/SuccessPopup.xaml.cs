using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using VMA.MVVM.ViewModels;

namespace VMA.MVVM.Views
{
    /// <summary>
    /// Interaction logic for SuccessPopup.xaml
    /// </summary>
    public partial class SuccessPopup : Window
    {
        public SuccessPopup()
        {
            InitializeComponent();
            this.DataContext = SuccessPopupViewModel.Instance;

            Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
            {
                var workingArea = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
                var transform = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
                var corner = transform.Transform(new Point(workingArea.Right, workingArea.Bottom));

                //this.Left = corner.X - this.ActualWidth - 100;
                //this.Top = corner.Y - this.ActualHeight;
            }));
        }
    }
}
