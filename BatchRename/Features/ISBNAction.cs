using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BatchRename.Utils;
using System.ComponentModel;
namespace BatchRename.Features {
    public class ISBNArgs : StringArgs, INotifyPropertyChanged {
        private string _direction;  // before/after
        
        public string Direction { get => _direction; set {
                _direction = value;
                NotifyChange("Direction");
                NotifyChange("Details");
            } }
        public ISBNArgs() { }
        public ISBNArgs(string details) {
            string[] word = details.Split('/');
            Direction = word[1];
        }
        public string Details => $"Move ISBN {Direction} name" ;

        public string ParseArgs() {
            return Direction;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyChange(string v) {
            if (PropertyChanged!=null) {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(v));
            }
        }
    }
    public class ISBNAction :StringAction {
        public string Name => "ISBN action";
        public StringProcessor Processor => _directing;
        public StringArgs Args { get; set; }
        private string _directing(string origin) {
            
            var isbnArgs = Args as ISBNArgs;
            if (isbnArgs.Direction == "before") {
                string isbn = origin.Substring(0, 13);
                string name = origin.Substring(15);
                return $"{isbn} {name}";
            } else if (isbnArgs.Direction == "after") {
                string isbn = origin.Substring(0, 13);
                string name = origin.Substring(15);
                return $"{name} {isbn}";
            }

            return "error";
        }
        public StringAction Clone() {
            return new ISBNAction() {
                Args = new ISBNArgs()
            };
        }

        public void ShowEditDialog() {
            var screen = new ISBNDialog(Args as ISBNArgs);
            if (screen.ShowDialog()==true) {
                var args = Args as ISBNArgs;
                args.Direction = screen.Direction;
            }
        }
    }
}
