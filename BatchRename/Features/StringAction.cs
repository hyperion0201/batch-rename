using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchRename.Features {
    public delegate string StringProcessor(string origin);

    public interface StringArgs {
        string Details { get; }
        string ParseArgs();
    }
    public interface StringAction {
        string Name { get; }
        StringProcessor Processor { get; }
        StringArgs Args { get; set; }
       
        StringAction Clone();
        void ShowEditDialog();
    }
}