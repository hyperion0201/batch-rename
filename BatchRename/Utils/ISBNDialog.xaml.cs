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
using System.Windows.Shapes;
using BatchRename.Features;
namespace BatchRename.Utils {
    /// <summary>
    /// Interaction logic for ISBNDialog.xaml
    /// </summary>
    public partial class ISBNDialog : Window {

        public string Direction;
        public ISBNDialog(ISBNArgs Args) {
            InitializeComponent();
            Direction = Args.Direction;
        }

        private void SubmitDirecting(object sender, RoutedEventArgs e) {
            if (beforeRadio.IsChecked == true) {
                Direction = "before"; this.DialogResult = true;
            } else if (afterRadio.IsChecked == true) {
                Direction = "after";
                this.DialogResult = true;
            } else {
                MessageBox.Show("No direction selected!");
            }
            
        }
        private void Cancel(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}
