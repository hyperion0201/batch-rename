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
using System.Windows.Shapes;
using BatchRename.Features;
namespace BatchRename.Utils {
    /// <summary>
    /// Interaction logic for ReplaceDialog.xaml
    /// </summary>
    public partial class ReplaceDialog : Window {
        public string Needle;
        public string Hammer;
        public ReplaceDialog(ReplaceArgs args) {
            InitializeComponent();
            needleTextBox.Text = args.Needle;
            hammerTextBox.Text = args.Hammer;
        }

        private void SubmitReplace(object sender, RoutedEventArgs e) {
            string specChars = @"<>/\\?:|""";
           
           
            foreach(char ch in specChars) {
                if (needleTextBox.Text.Contains(ch) || hammerTextBox.Text.Contains(ch)) {
                    MessageBox.Show("Invalid name. Please try again. Auto set needle and hammer to 'default'.");
                    needleTextBox.Text = "default";
                    hammerTextBox.Text = "default";
                   
                }
            }
          
          
            if (string.IsNullOrWhiteSpace(needleTextBox.Text) || string.IsNullOrEmpty(needleTextBox.Text)
                || string.IsNullOrEmpty(hammerTextBox.Text)|| string.IsNullOrWhiteSpace(hammerTextBox.Text)) {
                MessageBox.Show("File name can't be empty or white spaces.");
                needleTextBox.Text = "default";
                hammerTextBox.Text = "default";
               
            }
            Needle = needleTextBox.Text;
            Hammer = hammerTextBox.Text;
            this.DialogResult = true;
        }

        private void Cancel(object sender, RoutedEventArgs e) {
            this.Close();   
        }
    }
}
