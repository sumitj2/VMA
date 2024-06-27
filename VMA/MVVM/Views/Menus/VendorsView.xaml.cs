using BusinessLogic.Abstraction.VMA.Contract;
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
using VMA.MVVM.ViewModels.Menus;

namespace VMA.MVVM.Views
{
    /// <summary>
    /// Interaction logic for VendorsView.xaml
    /// </summary>
    public partial class VendorsView : UserControl
    {
        public VendorsView()
        {
            InitializeComponent();           
        }

        private void Deselect_Unchecked(object sender, RoutedEventArgs e)
        {

        }
    }
}
