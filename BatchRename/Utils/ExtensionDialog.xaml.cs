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
using System.Text.RegularExpressions;
namespace BatchRename.Utils {
    /// <summary>
    /// Interaction logic for ExtensionDialog.xaml
    /// </summary>
    public partial class ExtensionDialog : Window {
        public string newExtension;
        public ExtensionDialog(ExtensionArgs args) {
            InitializeComponent();
            newExtension = args.NewExtension;
        }

       private void SubmitExtension(object sender, RoutedEventArgs e) {
            string specChars = @"<>/\\?:|""";
            foreach (char ch in specChars) {
                if (extensionTextBox.Text.Contains(ch)) {
                    MessageBox.Show("Invalid extension. Please try again. Auto set extension to 'default'.");
                   
                    extensionTextBox.Text = "default";
                }
            }
            if (string.IsNullOrWhiteSpace(extensionTextBox.Text) || string.IsNullOrEmpty(extensionTextBox.Text))
              {
                MessageBox.Show("Extension can't be empty or white spaces.");
               
                extensionTextBox.Text = "default";

            }
            newExtension = extensionTextBox.Text;
            
            this.DialogResult = true;
        }

        private void Cancel(object sender, RoutedEventArgs e) {
            Close();
        }

    
    }
}
