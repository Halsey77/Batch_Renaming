using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>


    
    public partial class MainWindow : Window
    {
      
        public MainWindow()
        {
            InitializeComponent();
            string dllFile = @"D:\lap trinh windows\minh\RenamingRules\RenamingRules\bin\Debug\RenamingRules.dll";
            var assembly=Assembly.LoadFile(dllFile);
            var type = assembly.GetType("RenamingRules.Rules");
            var method = type.GetMethod("UpperCase");
            var obj = Activator.CreateInstance(type);
            if (method != null)
            {
                 var result=method.Invoke(obj, null);
                Console.WriteLine(result);
            }


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

        public interface RenameRule
        {

            string Process(string origin);
        }
        public class Replacer : RenameRule
        {
            public string _needle { get; set; }
            public string _hammer { get; set; }
            public string Process(string origin)
            {
                var needle = _needle;
                var hammer = _hammer;
                string res = "";
                // neu chuoi needle khong chua hoac khong ton tai thi khong phai thay the
                if(!origin.Contains(needle)||String.IsNullOrEmpty(needle))
                {
                    res = origin;
                }
                else
                {
                    res = origin.Replace(needle, hammer);
                }
                return res;
            }
        }
        // viet in hoa
        public class UpperCase : RenameRule
        {
            public string Process(string origin)
            {
                string fileName = Path.GetFileNameWithoutExtension(origin);
                string res = fileName.ToUpper() + Path.GetExtension(origin);
                return res;
            }
        }
        public class LowerCase : RenameRule
        {
            public string Process(string origin)
            {
                string fileName = Path.GetFileNameWithoutExtension(origin);
                string res = fileName.ToLower() + Path.GetExtension(origin);
                return res;
            }
        }

        // chu cai dau viet hoa
        public class SpecialCase : RenameRule
        {
            public string Process(string origin)
            {
                string fileName = Path.GetFileNameWithoutExtension(origin).ToLower();
                string res = fileName.First().ToString().ToUpper() + fileName.Substring(1) + Path.GetExtension(origin);
                return res;
            }
        }
        //ki tu dau khong the la khoang trang va so, ki tu cuoi khong the la khong trang 
        // bo cac khoang trong >1 giua cac tu 
        // viet hoa sau " "neu la chu cai
        public class PerfectCase : RenameRule
        {
            public string Process(string origin)
            {
                string res = "";
                string fileName = Path.GetFileNameWithoutExtension(origin);
                // loai bo khoang trong o dau dong va so 
                while(!Char.IsLetter(fileName,0))
                {
                    fileName = fileName.Remove(0, 1);
                }    
                while(!Char.IsWhiteSpace(fileName[fileName.Length-1]))
                {   
                    fileName = fileName.Remove(fileName.Length - 1, 1);
                }    
                for(int i=0;i<fileName.Length;i++)
                {
                    if(i==0)
                    {
                        res=res+Char.ToUpper(fileName[i]);
                    }    
                    else if(Char.IsWhiteSpace(fileName[i-1])&&Char.IsLetter(fileName[i]))
                    {
                        res = res + " " + Char.ToUpper(fileName[i]);
                    }    
                    else if(Char.IsWhiteSpace(fileName[i-1])&&!Char.IsWhiteSpace(fileName[i]))
                    {
                        res = res + " " + fileName[i];
                    }    
                    else if(!Char.IsWhiteSpace(fileName[i]))
                    {
                        res = res + fileName[i];
                    }    
                }
                res = res + Path.GetExtension(origin);
                return res;

            }
             
        }

        // tao mot ten ngau nhien
        public class UniqueName : RenameRule
        {
            public string Process(string origin)
            {
                string fileName = Path.GetFileNameWithoutExtension(origin);
                var originalGuild = new Guid();
                string res = fileName + originalGuild.ToString();
                return res;
            }
        }



       


        private void ClickSavePresetButton(object sender, RoutedEventArgs e)
        {

        }

        private void ClickRemovePresetButton(object sender, RoutedEventArgs e)
        {

        }
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
