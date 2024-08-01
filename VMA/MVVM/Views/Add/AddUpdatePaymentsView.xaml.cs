using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace VMA.MVVM.Views.Add
{
    /// <summary>
    /// Interaction logic for AddUpdatePaymentsView.xaml
    /// </summary>
    public partial class AddUpdatePaymentsView : UserControl
    {
        public AddUpdatePaymentsView()
        {
            InitializeComponent();
        }

        private void txtTDSAmount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if(!Regex.Match(e.Text, "^[0-9.]+$").Success)
            {
                e.Handled = true;
            }
        }

    }
}
