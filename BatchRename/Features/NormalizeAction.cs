using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BatchRename.Utils;
using System.ComponentModel;
using System.Text.RegularExpressions;
namespace BatchRename.Features {
    public class NormalizeArgs : StringArgs {
        public string Details => $"Normalize name";
        
        public string ParseArgs() { return ""; }
    }
    public class NormalizeAction :StringAction {
        public string Name => "Normalize action";
        public StringArgs Args { get; set; }
        public StringProcessor Processor => _normalize;
        private string _normalize(string origin) {

            // trimming and using regex to get remove unnecessary spaces
            string trimmed = origin.Trim();
            trimmed = Regex.Replace(trimmed, @"\s+", " ");
            string res = "";
            string[] words = trimmed.Split(' ');
            foreach (var word in words) {
                res = res + char.ToUpper(word[0]) + word.Substring(1).ToLower() + " ";
            }
            return res;

        }
        public StringAction Clone() {
            return new NormalizeAction() {
                Args = new NormalizeArgs()
            };
        }
        public void ShowEditDialog() {
            return;
            // nothing to do with this action
        }
    }
}
