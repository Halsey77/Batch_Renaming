
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
 



    public partial class MainWindow : RibbonWindow
    {
        private List<IRenameRule> rules = new List<IRenameRule>();
        private List<IRenameRule> ruleSource= new List<IRenameRule>();
       
        private RenameFactory.RenameFactory factory = RenameFactory.RenameFactory.getInstance();

        public MainWindow()
        {
            InitializeComponent();
            var lines = File.ReadLines("rules.txt");
            foreach (var line in lines)
            {
                rules.Add(factory.Create(line));
                
            }
            
            RuleComboBox.ItemsSource= rules;
            RuleListBox.ItemsSource = ruleSource;
            
        }
        


        BindingList<FileInfo> info = new BindingList<FileInfo>();

        private void ClickBrowseFolders(object sender, RoutedEventArgs e)
        {
            // Add folders
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "D:\\";
            dialog.Multiselect = true;
            dialog.IsFolderPicker = true;
            bool exist = false;
       
          
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                var fileNames=dialog.FileNames;

                var dirs = dialog.FileNames.ToArray();
                for(int i=0;i<dirs.Length;i++)
                {
                    
                    FileInfo info1 = new FileInfo(Path.GetFileName(dirs[i]), "", "", Path.GetDirectoryName(dirs[i]), Path.GetExtension(dirs[i]));
                    exist = false;
                    // kiem tra file da ton tai chua 
                    foreach (FileInfo info2 in info)
                    {
                        if(info2._name==info1._name&&info2._extension==info1._extension)
                        {
                            exist = true;
                            break;
                        }
                    }
                    if (exist == false)
                    {
                        info.Add(info1);
                    }
                }
                foreach(FileInfo info2 in info)
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
            bool exist = false;
            // tao dialog co muon xoa ko 
           
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                var fileNames = dialog.FileNames;

                var dirs = dialog.FileNames.ToArray();
                for (int i = 0; i < dirs.Length; i++)
                {
                    FileInfo info1 = new FileInfo(Path.GetFileName(dirs[i]), "", "", Path.GetDirectoryName(dirs[i]), Path.GetExtension(dirs[i]));
                    exist = false;
                    // kiem tra file da ton tai chua 
                    foreach (FileInfo info2 in info)
                    {
                        if (info2._name == info1._name && info2._extension == info1._extension)
                        {
                            exist = true;
                            break;
                        }
                    }
                    if (exist == false)
                    {
                        info.Add(info1);
                    }
                }
                listView.Items.Clear();
                foreach (FileInfo info2 in info) 
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
                    info1._status = "";
                    info1._newName = "";
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

        private void AddRuleButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedRule = RuleComboBox.SelectedItem as IRenameRule;
            var instance = selectedRule.Clone();
            ruleSource.Add(instance);
            RuleListBox.Items.Refresh();



        }

        private void IsActivate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddRule_Click(object sender, RoutedEventArgs e)
        {
            if(RuleComboBox.IsDropDownOpen==false)
            {
                RuleComboBox.IsDropDownOpen = true;
            }    
        }

        private void EditRule_Click(object sender, RoutedEventArgs e)
        {
            Window editWindow=new EditWindow();
            editWindow.ShowDialog();

        }

        private void RemoveRule_Click(object sender, RoutedEventArgs e)
        {
            if(RuleListBox.SelectedItem==null)
            {
                MessageBox.Show("Please choose an item in rules list to delete");
            }
            else
            {
                IRenameRule selected=RuleListBox.SelectedItem as IRenameRule;
                ruleSource.Remove(selected);
                RuleListBox.Items.Refresh();

            }
        }
    }
}
