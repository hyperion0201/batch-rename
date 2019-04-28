using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BatchRename.Utils
{
    public class AdditionalFunction {

        public static void MoveUp(ListBox lv) {
            MoveItem(lv, -1);
        }

        public static void MoveDown(ListBox lv) {
            MoveItem(lv,1);
        }

        public static void MoveItem(ListBox lv, int direction) {
            
        }

    }
}
