using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using BatchRename.Utils;
namespace BatchRename.Features {
    public class CaseArgs : StringArgs, INotifyPropertyChanged {
      
        private string _casetype ; // uppercase, lowercase, first upper

        public string ParseArgs() {
            return CaseType;
        }
        public CaseArgs() { }
        public CaseArgs(string details) {
            string[] word = details.Split('/');
            CaseType = word[1];
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public string Details => $"Casing filename to {CaseType} ";
        public int GetCaseType() {
            if (_casetype == "uppercase") return 1;
            else if (_casetype == "lowercase") return 2;
            return 3;
        }
        public void NotifyChange(string eventStr) {
            if (PropertyChanged != null) {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(eventStr));
            }
        }

        public string CaseType {
            get => _casetype; set {
                _casetype = value;
                NotifyChange("CaseType");
                NotifyChange("Details");
            }
        }

    }

    public class CaseAction : StringAction {
        public string Name => "Case action";
        public StringArgs Args { get; set; }
        private string _changecase(string origin) {
            var caseArgs = Args as CaseArgs;
           
            int casetype = caseArgs.GetCaseType();
            if (casetype == 1) {
                return origin.ToUpper();
            } else if (casetype == 2) {
                return origin.ToLower();

            } else {
                return char.ToUpper(origin[0]) + origin.Substring(1).ToLower();

            }
        }
        public StringProcessor Processor => _changecase;

        public StringAction Clone() {
            return new CaseAction() {
                Args = new CaseArgs()
            };
        }

        public void ShowEditDialog() {
            var screen = new CaseDialog(Args as CaseArgs);
            if (screen.ShowDialog() == true) {
                var caseArgs = Args as CaseArgs;
                
                caseArgs.CaseType = screen.CaseType;
            }
        }
    }
}


