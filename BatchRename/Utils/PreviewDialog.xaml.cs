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
using System.Collections.ObjectModel;
namespace BatchRename.Utils {
    /// <summary>
    /// Interaction logic for PreviewDialog.xaml
    /// </summary>
    public partial class PreviewDialog : Window {
        ObservableCollection<FileClass> fileList = null;
        ObservableCollection<FileClass> folderList = null;
        public string DuplicateAction ="";
        public PreviewDialog(ObservableCollection<FileClass> file, ObservableCollection<FileClass> folder) {
            InitializeComponent();
            fileList = new ObservableCollection<FileClass>();
            folderList = new ObservableCollection<FileClass>();
            fileList = file;
            folderList = folder;
            previewTab.Items.Clear();
            foreach (FileClass f in folderList) fileList.Add(f);
            foreach(FileClass f in fileList) {
                previewTab.Items.Add(f);
            }
            previewTab.Items.Refresh();
        }

        private void SubmitBatch(object sender, RoutedEventArgs e) {
            if (keepOldName.IsChecked == true) DuplicateAction = "keep";
            else if (addNumberAfter.IsChecked == true) DuplicateAction = "addnumber";
            this.DialogResult = true;
        }

        private void Cancel(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
