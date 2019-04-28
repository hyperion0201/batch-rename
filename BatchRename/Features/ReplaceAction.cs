using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using BatchRename.Utils;

namespace BatchRename.Features
{
  public class ReplaceArgs : StringArgs, INotifyPropertyChanged {
        private string _needle;
        private string _hammer;

        public ReplaceArgs() { }
        
        public string ParseArgs() {
            return $"{Needle}/{Hammer}";
        }
       public ReplaceArgs(string details) {
            // extract args
            string[] word = details.Split('/');
            Needle = word[1];
            Hammer = word[2];
        }
        public string Needle { get => _needle; set {
                _needle = value;
                NotifyChange("Needle");
                NotifyChange("Details");
            } }

        public string Hammer { get => _hammer; set {
                _hammer = value;
                NotifyChange("Hammer");
                NotifyChange("Details");
            } }

        public string Details {
            get => $"Replace {Needle} with {Hammer}";
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyChange(string v) {
           if (PropertyChanged!=null) {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(v));
            }
        }
        
    }

  public class ReplaceAction: StringAction {

        public string Name => "Replace action";
        private string _replace (string origin) {

            var replaceArgs = Args as ReplaceArgs;
            var needle = replaceArgs.Needle;
            var hammer = replaceArgs.Hammer;
            if (needle == null || hammer == null) return " ";
            string result = origin.Replace(needle, hammer);

            return result;
        }
        public StringProcessor Processor => _replace;
        public StringArgs Args { get; set; }

        public StringAction Clone() {
            return new ReplaceAction() {
                Args = new ReplaceArgs()
            };
        }
        public void ShowEditDialog() {
            var screen = new ReplaceDialog(Args as ReplaceArgs);

            if(screen.ShowDialog()==true) {
                var replaceArgs = Args as ReplaceArgs;
                replaceArgs.Needle = screen.Needle;
                replaceArgs.Hammer = screen.Hammer;
                
            }
        }
       
    }
}
