using BusinessLogic.Abstraction.VMA.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VMA.MVVM.ViewModels;
using VMA.MVVM.ViewModels.Login;


namespace VMA.MVVM.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        private IUserBusinessLogic _userBusinessLogic;
        public MainView(IUserBusinessLogic userBusinessLogic)
        {
            _userBusinessLogic = userBusinessLogic;
            this.DataContext = new MainViewModel(_userBusinessLogic);
            InitializeComponent();            
        }
        //[DllImport("user32.dll")]
        //public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
        //private void pnlControlBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    WindowInteropHelper helper = new WindowInteropHelper(this);
        //    SendMessage(helper.Handle, 161, 2, 0);​
        //}
        //private void pnlControlBar_MouseEnter(object sender, MouseEventArgs e)
        //{
        //  //  Este.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        //}
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
           /// Este.WindowState = WindowState.Minimized;
        }
        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal) { }
            /// Este.WindowState = WindowState.Maximized;
            else this.WindowState = WindowState.Normal;
        }
    }
}
