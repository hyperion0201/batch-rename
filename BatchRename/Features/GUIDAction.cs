using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BatchRename.Utils;
namespace BatchRename.Features {
    public class GUIDArgs : StringArgs {
        public string Details => "Generate GUID";

        public string ParseArgs() {
            return "";
        }
    }
    public class GUIDAction :StringAction {
        public string Name => "GUID action";
        public StringArgs Args { get; set; }

        public StringProcessor Processor => _generating;
        private string _generating(string origin) {
            var extPos = origin.LastIndexOf('.');
            var g = Guid.NewGuid();
            if (extPos == -1) return g.ToString();
            return $"{g.ToString()}.{origin.Substring(extPos+1)}";
        }
        public StringAction Clone() {
            return new GUIDAction() {
                Args = new GUIDArgs()
            };
        }
        public void ShowEditDialog() {
            return; 
            // nothing to do with this.
        }
    }
}
