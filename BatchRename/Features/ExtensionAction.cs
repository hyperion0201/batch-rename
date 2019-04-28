using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using BatchRename.Utils;
namespace BatchRename.Features

{
   public class ExtensionArgs: StringArgs, INotifyPropertyChanged {
        private string _newextension;

        public string ParseArgs() {
            return NewExtension;
        }
        public ExtensionArgs() { }
        public ExtensionArgs(string details) {
            // extract args
            // 
            string[] words = details.Split('/');
            NewExtension = words[1];
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public string Details => $"Change extenstion to .{NewExtension}";
        
        private void NotifyChange(string v) {
            if (PropertyChanged!=null) {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(v));
            }
        }
        public string NewExtension { get => _newextension; set {
                _newextension = value;
                NotifyChange("New extension");
                NotifyChange("Details");
                
            }
        }
    }
    public class ExtensionAction : StringAction {
        public string Name => "Extension action";
        private string _changeExtension(string origin) {
            var extArgs = Args as ExtensionArgs;
            var newExt = extArgs.NewExtension;
            var foundPosition = origin.LastIndexOf(".");
            var beginning = origin.Substring(0, foundPosition);
            return $"{beginning}.{newExt}";
        }
        public StringProcessor Processor { get => _changeExtension; }
        public StringArgs Args { get; set; }
        public StringAction Clone() {
            return new ExtensionAction() {
                Args = new ExtensionArgs()
            };
        }
        public void ShowEditDialog() {
            var screen = new ExtensionDialog(Args as ExtensionArgs);
            if (screen.ShowDialog()==true) {
                var extensionArgs = Args as ExtensionArgs;
                extensionArgs.NewExtension = screen.newExtension;
            }
        }
    }
}
