using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfApp1
{
    public class FileInfo: INotifyPropertyChanged
    {
        public string _name;
        public string _newName;
        public string _status;
        public string _directory;
        public string _extension;

        public FileInfo(string name, string newName, string status, string directory, string extension)
        {
            _name = name;
            _newName = newName;
            _status = status;
            _directory = directory;
            _extension = extension;
        }

        public string Name
        {
            get => _name; set
            {
                _name = value;
                RaiseEvent();
            }
        }

        public string Extension
        {
            get => _extension; set
            {
                _extension = value;
                RaiseEvent();
            }
        }

        public string Directory
        {
            get => _directory; set
            {
                _directory = value;
                RaiseEvent();
            }
        }

        public string NewName
        {
            get => _newName; set
            {
                _newName = value;
                RaiseEvent();
            }
        }

        public string Status
        {
            get => _status; set
            {
                _status = value;
                RaiseEvent();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void RaiseEvent([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


   
}