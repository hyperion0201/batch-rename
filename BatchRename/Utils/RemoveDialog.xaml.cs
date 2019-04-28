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
    
    public partial class RemoveDialog : Window {
        public int startIndex;
        public int count;
        public RemoveDialog(RemoveArgs args) {
            InitializeComponent();
            startIndexTextBox.Text = args.StartIndex.ToString();
            countTextBox.Text = args.Count.ToString();
        }

        private void SubmitRemove(object sender, RoutedEventArgs e)
        {
            
            if (int.TryParse(startIndexTextBox.Text, out startIndex) == false ||
           int.TryParse(countTextBox.Text, out count) == false) {
                MessageBox.Show("We need a number here.");
            }
            else {
                this.DialogResult = true;
            }
            
        }

        private void Cancel(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}
