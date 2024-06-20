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
using System.Runtime;

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
            DataContext = new MainViewModel(_userBusinessLogic);
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            InitializeComponent();            
        }
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
        private void PnlControlBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            SendMessage(helper.Handle, 161, 2, 0);
        }
        private void PnlControlBar_MouseEnter(object sender, MouseEventArgs e)
        {
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth= SystemParameters.MaximizedPrimaryScreenWidth;
        }
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void BtnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else WindowState = WindowState.Normal;
        }


    }
}
