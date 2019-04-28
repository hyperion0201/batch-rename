using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using BatchRename.Utils;

/*
 * replace act
 * remove act
 * ext act
 * case act
 * normal act
 * isbn act
 * guid act
 */
namespace BatchRename.Features
{
    public class RemoveArgs: StringArgs, INotifyPropertyChanged {
        private int _startIndex;
        private int _count;
        public event PropertyChangedEventHandler PropertyChanged;
        public string Details => $"Remove start at pos {StartIndex} and remove {Count} chars";

        public string ParseArgs() { return $"{StartIndex}/{Count}"; }
        public RemoveArgs() { }
        public RemoveArgs(string details) {
            // extract args : Remove action /3/5
            string[] word = details.Split('/');
            StartIndex = Int32.Parse(word[1]);
            Count = Int32.Parse(word[2]);
        }
        private void NotifyChange(string v) {
            if (PropertyChanged!=null) {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(v));
            }
        }
           public int StartIndex { get => _startIndex; set {
                _startIndex = value;
                NotifyChange("Start index");
                NotifyChange("Detail");
            } }
           public int Count { get => _count; set {
                _count = value;
                NotifyChange("Count");
                NotifyChange("Details");
            } } 
    }
 
    public class RemoveAction:StringAction {
        public string Name => "Remove action";
        public StringArgs Args { get; set; }
        
        private string _remove(string origin) {
            var removeArgs = Args as RemoveArgs;
            var startIndex = removeArgs.StartIndex;
            var count = removeArgs.Count;
            if (startIndex >= origin.Length || count > origin.Length) return " "; 
            return origin.Remove(startIndex, count);
        }
        public StringProcessor Processor => _remove;
        
        public StringAction Clone() {
            return new RemoveAction() {
                Args = new RemoveArgs()
            };
        }

        public void ShowEditDialog() {
            var screen = new RemoveDialog(Args as RemoveArgs);
           
            if (screen.ShowDialog()==true)
            {
                var removeArgs = Args as RemoveArgs;
                removeArgs.StartIndex = screen.startIndex;
                removeArgs.Count = screen.count;
                
            }
        }
    }

}
