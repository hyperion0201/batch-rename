using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BatchRename.Features;
using BatchRename.Utils;
using MessageBox = System.Windows.MessageBox;
using Path = System.IO.Path;
using System.Windows.Forms;
using System.IO;

namespace BatchRename {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        // defining global lists

        ObservableCollection<StringAction> methodList = null;
        ObservableCollection<FileClass> fileList = null;
        ObservableCollection<FileClass> folderList = null;
        ObservableCollection<Preset> presetList = null;
        string currentPresetFile = "";
        public MainWindow() { 
            InitializeComponent();
            
        }
        private void AddMethod(object sender, RoutedEventArgs e) {
            if (methodBox.SelectedItem == null) {
                MessageBox.Show("No method selected to add ?");
                return;
            }
            var methodSelected = methodBox.SelectedItem as StringAction;
            var instance = methodSelected.Clone();

            methodListBox.Items.Add(instance);
            
        }
        private void Window_Loaded(object sender, RoutedEventArgs e) {
          methodList = new ObservableCollection<StringAction>() {
            new ReplaceAction(),
            new RemoveAction(),
            new ExtensionAction(), 
            new CaseAction(),
            new NormalizeAction(),
            new ISBNAction(),
            new GUIDAction()
            };
            methodBox.ItemsSource = methodList;
            fileList = new ObservableCollection<FileClass>();
            folderList = new ObservableCollection<FileClass>();
            presetList = new ObservableCollection<Preset>();
        }

        private void ClearMethodList(object sender, RoutedEventArgs e) {
            // clear all UI controls
            methodListBox.ItemsSource = null;
            methodListBox.Items.Clear();
            FileTab.ItemsSource = null;
            FileTab.Items.Clear();
            // clear source collections
            
            fileList.Clear();
            folderList.Clear();
        }
        
        private void MoveMethodUp(object sender, RoutedEventArgs e) {
           
           if (methodListBox.SelectedIndex ==-1) {
                MessageBox.Show("No method selected. Try again.");
            }
           else {
                int newIndex = methodListBox.SelectedIndex - 1;
                if (newIndex < 0) return;
                object selected = methodListBox.SelectedItem;
                methodListBox.Items.Remove(selected);
                methodListBox.Items.Insert(newIndex, selected);
                methodListBox.SelectedIndex = newIndex; 
            }
        }

        private void MoveMethodDown(object sender, RoutedEventArgs e) {

            if (methodListBox.SelectedIndex ==-1) {
                MessageBox.Show("No method selected. Try again.");
            }
            else {
                int newIndex = methodListBox.SelectedIndex + 1;
                if (newIndex >= methodListBox.Items.Count) return;
                object selected = methodListBox.SelectedItem;
                methodListBox.Items.Remove(selected);
                methodListBox.Items.Insert(newIndex, selected);
                methodListBox.SelectedIndex = newIndex;
            }
        }

        private void OpenFileBrowser(object sender, RoutedEventArgs e) {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                string[] files = Directory.GetFiles(dialog.SelectedPath);
                foreach (var file in files) {
                    FileTab.Items.Add(new FileClass() {
                        FileName = Path.GetFileName(file),
                        Path = file
                    });

                }
            }
        }
        private void MoveMethodTop(object sender, RoutedEventArgs e) {
            if (methodListBox.SelectedIndex == -1) {
                MessageBox.Show("No method selected. Try again.");
            }
            else {
                // if selected item is on top? 
                if (methodListBox.SelectedIndex == 0) return;
                // else, insert on top
                object selected = methodListBox.SelectedItem;
                methodListBox.Items.Remove(selected);
                methodListBox.Items.Insert(0, selected);
                methodListBox.SelectedIndex = 0;
            }
        }

        private void MoveMethodBottom(object sender, RoutedEventArgs e) {
            if (methodListBox.SelectedIndex == -1) {
                MessageBox.Show("No method selected. Try again.");
            }
            else {
                // if item is at the bottom
                if (methodListBox.SelectedIndex == methodListBox.Items.Count) return;
                // else, insert item at the bottom
                object selected = methodListBox.SelectedItem;
                methodListBox.Items.Remove(selected);
                methodListBox.Items.Insert(methodListBox.Items.Count, selected);
                methodListBox.SelectedIndex = methodListBox.Items.Count - 1;
            }
        }

        private void ShowHelpDialog(object sender, RoutedEventArgs e) {
            var helpscreen = new HelpDialog();
            helpscreen.ShowDialog();
        }

        private void OpenFolderBrowser(object sender, RoutedEventArgs e) {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog()==System.Windows.Forms.DialogResult.OK) {
                
                string[] dirs = Directory.GetDirectories(dialog.SelectedPath);
                foreach (var dir in dirs) {
                    FolderTab.Items.Add(new FileClass() {
                        FileName = new DirectoryInfo(dir).Name,
                        Path = dir
                    });
                }
              
            }
        }

        private void ConfigureMethod(object sender, RoutedEventArgs e) {
            var method = methodListBox.SelectedItem as StringAction;
            method.ShowEditDialog();
        }

        private void StartBatch(object sender, RoutedEventArgs e) {
            // clear fileList and folderList
            fileList = new ObservableCollection<FileClass>();
            folderList = new ObservableCollection<FileClass>();
            // create files list and folders list for executing methods
            foreach (FileClass f in FileTab.Items) fileList.Add(f);
            foreach (FileClass folder in FolderTab.Items) folderList.Add(folder);
            

            //  no method selected?
            if (methodListBox.Items.Count==0) {
                MessageBox.Show("Method box is empty ?");
                return;
            }

            // no file & folder selected?
            if (fileList.Count==0 && folderList.Count==0) {
                MessageBox.Show("No file/folder selected!");
                return;
            } 

            //begin batching files

            for (int i = 0; i < fileList.Count; i++) {
                string result = fileList[i].FileName;
                string path = Path.GetDirectoryName(fileList[i].Path);
                foreach (StringAction action in methodListBox.Items) {                  
                    result = action.Processor.Invoke(result);
                    ObservableCollection<FileClass> temp = new ObservableCollection<FileClass>(fileList);
                    temp.Remove(temp[i]);
                    foreach(FileClass f in temp) {
                        if (result == f.FileName) {
                            fileList[i].Error = "Duplicate detected.";
                            fileList[i].NewFileName = result;
                         
                        }
                    }
                    if (result == " ") {
                        // a space mean there was an error when executed this method, defined in classes of BatchRename.Features
                        // stop performing other methods
                        fileList[i].Error = "Error.";
                        break;
                    };
                }
               // all done without error ?
               if (fileList[i].Error!="Duplicate detected.") {
                    fileList[i].NewFileName = result;
                    fileList[i].Error = "Ok.";
                }
                    
                
                // check if status column has a value or not, if not, mark Done to it.
                  
            }
            // begin batching folders
                for (int i =0;i<folderList.Count;i++) {
                    string result = folderList[i].FileName;
                    string path = Path.GetDirectoryName(folderList[i].Path);
                        
                    foreach(StringAction action in methodListBox.Items) {
                    // ignore extension method
                    if (action.Name == "Change extension") continue;
                        result = action.Processor.Invoke(result);
                    ObservableCollection<FileClass> temp = new ObservableCollection<FileClass>(folderList);
                    temp.Remove(temp[i]);
                    foreach (FileClass f in temp) {
                        if (result == f.FileName) {
                            folderList[i].Error = "Duplicate detected.";
                            folderList[i].NewFileName = result;
                           
                        }
                    }
                    if (result == " ") {
                        // a space mean there was an error when executed this method 
                        folderList[i].Error = "Error.";
                        break;

                    }
                }
                // all done without error?
                if (fileList[i].Error != "Duplicate detected.") {
                    fileList[i].NewFileName = result;
                    fileList[i].Error = "Ok.";
                }

            }

            var previewScreen = new PreviewDialog(fileList, folderList);
            if (previewScreen.ShowDialog() == true) {
                string dupAction = previewScreen.DuplicateAction;
                if (dupAction=="keep") {
                    foreach (FileClass f in fileList) {
                        if (f.Error == "Duplicate detected.") continue;
                        var fInfo = new FileInfo(f.Path);
                        fInfo.MoveTo($"{Path.GetDirectoryName(f.Path)}\\{f.NewFileName}");
                    }
                    foreach (FileClass folder in folderList) {
                        if (folder.Error == "Duplicate detected.") continue;
                        var f = new FileInfo(folder.Path);
                        f.MoveTo($"{Path.GetDirectoryName(folder.Path)}\\{folder.NewFileName}");
                    }
                    FileTab.Items.Refresh();
                    FolderTab.Items.Refresh();
                    MessageBox.Show("Done.");
                }

                else if (dupAction=="addnumber") {

                    foreach (FileClass f in fileList) {
                        if (f.Error == "Duplicate detected.") {
                            var fInfo1 = new FileInfo(f.Path);
                            fInfo1.MoveTo($"{Path.GetDirectoryName(f.Path)}\\{f.NewFileName}01");
                        }
                        else {
                            var fInfo = new FileInfo(f.Path);
                            fInfo.MoveTo($"{Path.GetDirectoryName(f.Path)}\\{f.NewFileName}");
                        }
                       
                    }
                    foreach (FileClass folder in folderList) {
                        if (folder.Error == "Duplicate detected.") {
                            var f1 = new FileInfo(folder.Path);
                            f1.MoveTo($"{Path.GetDirectoryName(folder.Path)}\\{folder.NewFileName}01");
                        }
                        else {
                            var f = new FileInfo(folder.Path);
                            f.MoveTo($"{Path.GetDirectoryName(folder.Path)}\\{folder.NewFileName}");
                        }
                        
                    }
                    FileTab.Items.Refresh();
                    FolderTab.Items.Refresh();
                    MessageBox.Show("Done.");
                }
               
            }
            else {
                MessageBox.Show("Cancelled by user.");
            }

        }

        private void MoveFileTop(object sender, RoutedEventArgs e) {
             if (FileTab.SelectedIndex == -1) {
                MessageBox.Show("No file selected. Try again.");
            } else {
                // if selected item is on top? 
                if (FileTab.SelectedIndex == 0) return;
                // else, insert on top
                object selected = FileTab.SelectedItem;
                FileTab.Items.Remove(selected);
                FileTab.Items.Insert(0, selected);
                FileTab.SelectedIndex = 0;
            }
        }

        private void MoveFileBottom(object sender, RoutedEventArgs e) {
            if (FileTab.SelectedIndex == -1) {
                MessageBox.Show("No file selected. Try again.");
            } else {
                // if item is at the bottom
                if (FileTab.SelectedIndex == FileTab.Items.Count) return;
                // else, insert item at the bottom
                object selected = FileTab.SelectedItem;
                FileTab.Items.Remove(selected);
                FileTab.Items.Insert(FileTab.Items.Count, selected);
                FileTab.SelectedIndex = FileTab.Items.Count-1;
            }

        }
        private void MoveFileUp(object sender, RoutedEventArgs e) {
            
            if (FileTab.SelectedIndex == -1) {
                MessageBox.Show("No file selected. Try again.");
            } else {
                int newIndex = FileTab.SelectedIndex - 1;
                if (newIndex < 0) return;
                object selected = FileTab.SelectedItem;
                FileTab.Items.Remove(selected);
                FileTab.Items.Insert(newIndex, selected);
                FileTab.SelectedIndex = newIndex;
            }

        }
        private void MoveFileDown(object sender, RoutedEventArgs e) {
            if (FileTab.SelectedIndex == -1) {
                MessageBox.Show("No file selected. Try again.");
            } else {
                int newIndex = FileTab.SelectedIndex + 1;
                if (newIndex >= FileTab.Items.Count) return;
                object selected = FileTab.SelectedItem;
                FileTab.Items.Remove(selected);
                FileTab.Items.Insert(newIndex, selected);
                FileTab.SelectedIndex = newIndex;
            }
        }

        private void OnRefresh(object sender, RoutedEventArgs e) {
            FileTab.Items.Refresh();
            methodListBox.Items.Refresh();
   
        }

        private void RemoveMethod(object sender, RoutedEventArgs e) {
            var selectedItem = methodListBox.SelectedItem;
            methodListBox.Items.Remove(selectedItem);
        }

        private void MoveFolderTop(object sender, RoutedEventArgs e) {
            if (FolderTab.SelectedIndex == -1) {
                MessageBox.Show("No method selected. Try again.");
            } else {
                // if selected item is on top? 
                if (FolderTab.SelectedIndex == 0) return;
                // else, insert on top
                object selected = FolderTab.SelectedItem;
                FolderTab.Items.Remove(selected);
                FolderTab.Items.Insert(0, selected);
                FolderTab.SelectedIndex = 0;
            }
        }
        private void MoveFolderUp(object sender, RoutedEventArgs e) {
            if (FolderTab.SelectedIndex == -1) {
                MessageBox.Show("No file selected. Try again.");
            } else {
                int newIndex = FolderTab.SelectedIndex - 1;
                if (newIndex < 0) return;
                object selected = FileTab.SelectedItem;
                FolderTab.Items.Remove(selected);
                FolderTab.Items.Insert(newIndex, selected);
                FolderTab.SelectedIndex = newIndex;
            }
        }
        private void MoveFolderDown(object sender, RoutedEventArgs e) {
            if (FolderTab.SelectedIndex == -1) {
                MessageBox.Show("No file selected. Try again.");
            } else {
                int newIndex = FolderTab.SelectedIndex + 1;
                if (newIndex >= FolderTab.Items.Count) return;
                object selected = FolderTab.SelectedItem;
                FolderTab.Items.Remove(selected);
                FolderTab.Items.Insert(newIndex, selected);
                FolderTab.SelectedIndex = newIndex;
            }
        }
        private void MoveFolderBottom(object sender, RoutedEventArgs e) {
            if (FolderTab.SelectedIndex == -1) {
                MessageBox.Show("No file selected. Try again.");
            } else {
                // if item is at the bottom
                if (FolderTab.SelectedIndex == FolderTab.Items.Count) return;
                // else, insert item at the bottom
                object selected = FolderTab.SelectedItem;
                FolderTab.Items.Remove(selected);
                FolderTab.Items.Insert(FolderTab.Items.Count, selected);
                FolderTab.SelectedIndex = FolderTab.Items.Count - 1;
            }
        }

        private void OnSavePreset(object sender, RoutedEventArgs e) {
            // check methodListBox is empty?
            if (methodListBox.Items.Count == 0) {
                MessageBox.Show("Nothing to save. ");
                return;
            }
            // there are something to save

            var presetScreen = new PresetDialog();
            if (presetScreen.ShowDialog() == true) {
                string presetName = presetScreen.presetName;


                // any preset file opened before ?
                if (currentPresetFile != "") {
                    using (StreamWriter sw = File.AppendText(currentPresetFile)) {
                        sw.WriteLine(presetName);

                        foreach (StringAction action in methodListBox.Items) {
                            string methodTemplate = $"{action.Name}/{action.Args.ParseArgs()}";
                            sw.WriteLine(methodTemplate);
                        }

                        sw.WriteLine("**");

                    }
                    MessageBox.Show($"Saved. Please check preset file in {currentPresetFile}");
                } else {
                    // default preset path is : C:/BatchRename/preset.txt
                    string presetFolderPath = @"C:\BatchRename";
                    string presetFilePath = @"C:\BatchRename\preset.txt";

                    if (!Directory.Exists(presetFolderPath)) Directory.CreateDirectory(presetFolderPath);
                    // if folder exist 
                    if (!File.Exists(presetFilePath)) {
                        using (StreamWriter sw = File.CreateText(presetFilePath)) {
                            sw.WriteLine(presetName);

                            foreach (StringAction action in methodListBox.Items) {
                                string methodTemplate = $"{action.Name}/{action.Args.ParseArgs()}";
                                sw.WriteLine(methodTemplate);
                            }
                            sw.WriteLine("**");
                        }
                        MessageBox.Show($"Saved. Please check preset file in {presetFilePath}");
                    } else
                          // append file
                          {
                        using (StreamWriter sw = File.AppendText(presetFilePath)) {
                            sw.WriteLine(presetName);

                            foreach (StringAction action in methodListBox.Items) {
                                string methodTemplate = $"{action.Name}/{action.Args.ParseArgs()}";
                                sw.WriteLine(methodTemplate);
                            }

                            sw.WriteLine("**");

                        }
                        MessageBox.Show($"Saved. Please check preset file in {presetFilePath}");
                    }
                }
            }
        }

        private void OnLoadPreset(object sender, RoutedEventArgs e) {
            var loadDialog = new OpenFileDialog();
            if (loadDialog.ShowDialog()==System.Windows.Forms.DialogResult.OK) {
                // remove all items of methodPreset
                presetList = new ObservableCollection<Preset>();
                methodListBox.Items.Clear();
                string presetFilePath = loadDialog.FileName;

                // set global preset file path 
                currentPresetFile = presetFilePath;

                // begin reading preset

                using (StreamReader sr = new StreamReader(presetFilePath)) {
                    string currentLine;

                   while ((currentLine = sr.ReadLine())!=null) { // not endpoint?
                        string presetname = currentLine; // set name of preset

                        //create a list of action to store actions
                        ObservableCollection<StringAction> temp = new ObservableCollection<StringAction>();

                        // determine type of method to add to temp
                       while((currentLine = sr.ReadLine())!="**") {
                            if (currentLine.Contains("Replace")) temp.Add(new ReplaceAction() {
                                Args = new ReplaceArgs(currentLine)
                            });
                            else if (currentLine.Contains("Remove")) temp.Add(new RemoveAction() {
                                Args = new RemoveArgs(currentLine)
                            });
                            else if (currentLine.Contains("Extension")) temp.Add(new ExtensionAction() {
                                Args = new ExtensionArgs(currentLine)
                            });
                            else if (currentLine.Contains("Case")) temp.Add(new CaseAction() {
                                Args = new CaseArgs(currentLine)
                            });
                            else if (currentLine.Contains("Normalize")) temp.Add(new NormalizeAction() {
                                Args = new NormalizeArgs()
                            });
                            else if (currentLine.Contains("ISBN")) temp.Add(new ISBNAction() {
                                Args = new ISBNArgs()
                            });
                            else if (currentLine.Contains("GUID")) temp.Add(new GUIDAction() {
                                Args = new GUIDArgs()
                            });
                        }
                        presetList.Add(new Preset() {
                            Name = presetname,
                            presetItems = temp
                        });
                    }
                }
            }

            methodPreset.ItemsSource = presetList;
        }

        private void OnChangePreset(object sender, EventArgs e) {
            if (methodPreset.SelectedIndex == -1) return;
            // clear methodListBox
            methodListBox.Items.Clear();
            methodListBox.ItemsSource = null;
            object selected = methodPreset.SelectedItem;
            foreach(StringAction act in (selected as Preset).presetItems) {
                methodListBox.Items.Add(act);
            }
        }
    }
}
