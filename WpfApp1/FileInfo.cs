using System.ComponentModel;

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

        public event PropertyChangedEventHandler PropertyChanged;
    }
}