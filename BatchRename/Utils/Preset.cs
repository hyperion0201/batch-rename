using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BatchRename.Features;

namespace BatchRename.Utils {
    public class Preset {
        public string Name { get; set; }
        public ObservableCollection<StringAction> presetItems = null;
    }
}
