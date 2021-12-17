
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;
using RenameRules;
using System.Diagnostics;
using Fluent;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>



    public partial class MainWindow : RibbonWindow
    {
        public List<IRenameRule> Rules;
        Dictionary<string, IRenameRule> _nameRules = new Dictionary<string, IRenameRule>();
        public MainWindow()
        {
            InitializeComponent();
            var exeFolder = AppDomain.CurrentDomain.BaseDirectory;
            var dlls = new DirectoryInfo(exeFolder).GetFiles("*.dll");

            foreach (var dll in dlls)
            {
                var assembly = Assembly.LoadFile(dll.FullName);
                var types = assembly.GetTypes();

                foreach (var type in types)
                {
                    Debug.WriteLine(type);
                    if (type.IsClass)
                    {
                        if (typeof(IRenameRule).IsAssignableFrom(type))
                        {
                            var shape = Activator.CreateInstance(type) as IRenameRule;
                            if (shape != null)
                            {
                                _nameRules.Add(shape.Name, shape);
                            }
                        }
                    }
                }
            }

            Debug.WriteLine("Hello");
        }
        public class Information : INotifyPropertyChanged
        {
            public string _name;
            public string _newName;
            public string _status;
            public string _directory;
            public string _extension;
            public Information(string name, string newName, string status, string directory, string extension)
            {
                this._name = name;
                this._newName = newName;
                this._status = status;
                this._directory = directory;
                this._extension = extension;
            }
            public string Name
            {
                get
                {
                    return _name;
                }
                set
                {
                    _name = value;
                    RaisePropertyChange("Name");
                }
            }

            public string Extension
            {
                get
                {
                    return _extension;
                }
                set
                {
                    _extension = value;
                    RaisePropertyChange("Extension");
                }
            }

            public string Directory
            {
                get
                {
                    return _directory;
                }
                set
                {
                    _directory = value;
                    RaisePropertyChange("Directory");
                }
            }

            public string NewName
            {
                get
                {
                    return _newName;
                }
                set
                {
                    _newName = value;
                    RaisePropertyChange("NewName");
                }
            }

            public string Status
            {
                get
                {
                    return _status;
                }
                set
                {
                    _status = value;
                    RaisePropertyChange("Status");
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            void RaisePropertyChange([CallerMemberName] string propertyName = "")
            {
                if(PropertyChanged!=null)
                     PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

     
     

        // chu cai dau viet hoa
       



       

        BindingList<Information> info = new BindingList<Information>();
        
        private void ClickBrowseFolders(object sender, RoutedEventArgs e)
        {
            // Add folders
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "D:\\";
            dialog.Multiselect = true;
            dialog.IsFolderPicker = true;
            // tao dialog co muon xoa ko 
            if (info.Count>0)
            {
                info.Clear();
            }
            if(listView.Items.Count>0)
            {
                listView.Items.Clear();
            }
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                var fileNames=dialog.FileNames;

                var dirs = dialog.FileNames.ToArray();
                for(int i=0;i<dirs.Length;i++)
                {
                    Information info1 = new Information(Path.GetFileName(dirs[i]), "", "", Path.GetDirectoryName(dirs[i]), Path.GetExtension(dirs[i]));
                    info.Add(info1);
                }
                foreach(Information info2 in info)
                {
                    listView.Items.Add(info2);

                }
                DirectoryofFile.Text = Path.GetDirectoryName(dirs[0]);
            }
        }

        private void ClickBrowseFiles(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "D:\\";
            dialog.Multiselect = true;
            dialog.IsFolderPicker = false;
            // tao dialog co muon xoa ko 
            if (info.Count > 0)
            {
                info.Clear();
            }
            if (listView.Items.Count > 0)
            {
                listView.Items.Clear();
            }
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                var fileNames = dialog.FileNames;

                var dirs = dialog.FileNames.ToArray();
                for (int i = 0; i < dirs.Length; i++)
                {
                    Information info1 = new Information(Path.GetFileName(dirs[i]), "", "", Path.GetDirectoryName(dirs[i]), Path.GetExtension(dirs[i]));
                    info.Add(info1);
                }
                foreach (Information info2 in info) 
                {
                    listView.Items.Add(info2);

                }
                DirectoryofFile.Text = Path.GetDirectoryName(dirs[0]);
            }
        }

        private void ClickRefreshButton(object sender, RoutedEventArgs e)
        {
            if (info.Count > 0)
            {
                listView.Items.Clear();
                foreach (var info1 in info)
                {
                    info1.Status = "";
                    info1.NewName = "";
                    listView.Items.Add(info1);
                }
            }
            else
            {
                MessageBox.Show("Khong co file hoac folder de refresh");
            }
        }

        private void ContextMenuClearClicked(object sender, RoutedEventArgs e)
        {

        }
    }
}
