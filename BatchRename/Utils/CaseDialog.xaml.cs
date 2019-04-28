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
    /// Interaction logic for CaseDialog.xaml
    /// </summary>
    public partial class CaseDialog : Window {
        public string OldCase;
        public string CaseType;

        public CaseDialog(CaseArgs args) {
            InitializeComponent();
           
            CaseType = args.CaseType;
            if (CaseType == "uppercase") upperRadio.IsChecked = true;
            else if (CaseType == "lowercase") lowerRadio.IsChecked = true;
            else firstUpperRadio.IsChecked = true;
        }

        private void SubmitCasing(object sender, RoutedEventArgs e) {
            if (upperRadio.IsChecked == true) {
                CaseType = "uppercase";
                this.DialogResult = true;
            } else if (lowerRadio.IsChecked == true) {
                CaseType = "lowercase";
                this.DialogResult = true;
            } else if (firstUpperRadio.IsChecked == true) {
                CaseType = "firstupper";
                this.DialogResult = true;
            } else {
                MessageBox.Show("No case option selected!");
            }
            
        }
        private void Cancel(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
