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

namespace BatchRename.Utils {
    /// <summary>
    /// Interaction logic for PresetDialog.xaml
    /// </summary>
    public partial class PresetDialog : Window {

        public string presetName;
        public PresetDialog() {
            InitializeComponent();
        }

        private void OnSave(object sender, RoutedEventArgs e) {
            presetName = presetTextBox.Text;
            this.DialogResult = true;
        }
        private void Cancel(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
