using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace BatchRename.Features {
   public class FileClass: INotifyPropertyChanged {
        private string _filename;
        private string _newfilename;
        private string _path;
        private string _error;

        public string FileName { get => _filename; set {
                _filename = value;
                NotifyChange("FileName");
            } }
        public string NewFileName { get => _newfilename; set {
                _newfilename = value;
                //NotifyChange("NewFileName");
            } }
        public string Path { get => _path; set {
                _path = value;
                NotifyChange("Path");
            } }
        public string Error { get => _error; set {
                _error = value;
                
            } }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyChange(string v) {
            if (PropertyChanged != null) {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(v));
            }
        }
        public FileClass Clone() {
            return new FileClass() {
                FileName = this._filename,
                NewFileName = this._newfilename,
                Path = this._path,
                Error = this._error
            };
        }
    }
}
