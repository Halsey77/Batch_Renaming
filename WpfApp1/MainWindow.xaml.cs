using System;
using Fluent;
using Microsoft.WindowsAPICodePack.Dialogs;
using RenameRules;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using Application = System.Windows.Application;
using DataFormats = System.Windows.DataFormats;
using DragEventArgs = System.Windows.DragEventArgs;
using MessageBox = System.Windows.MessageBox;
using Path = System.IO.Path;

namespace WpfApp1
{
    public partial class MainWindow : RibbonWindow
    {
        private List<IRenameRule> rules = new();
        private List<IRenameRule> ruleSource = new();
        private RenameFactory.RenameFactory factory = RenameFactory.RenameFactory.getInstance();
        BindingList<FileInformation> info = new();

        public MainWindow()
        {
            InitializeComponent();
            var lines = File.ReadLines("rules.txt");
            foreach (var line in lines)
            {
                try
                {
                    rules.Add(factory.Create(line));
                }
                catch (Exception exception)
                {
                    //do nothing.
                }
            }
            RuleComboBox.ItemsSource = rules;

            //get presets in app.config(if exists)
            string presets = ConfigurationManager.AppSettings["presets"];
            if (!String.IsNullOrEmpty(presets))
            {
                string[] presetArr = presets.Split(';');
                foreach (string s in presetArr)
                {
                    ruleSource.Add(factory.Create(s));
                }
            }
            RuleListBox.ItemsSource = ruleSource;

            listView.ItemsSource = info;

            //set window previous size
            string size = ConfigurationManager.AppSettings["windowSize"];
            if (!String.IsNullOrEmpty(size))
            {
                string[] dimen = size.Split(';');
                Application.Current.MainWindow.Width = Double.Parse(dimen[0]);
                Application.Current.MainWindow.Height = Double.Parse(dimen[1]);
            }

            //set window previous position
            string pos = ConfigurationManager.AppSettings["windowPos"];
            if (!String.IsNullOrEmpty(pos))
            {
                string[] position = pos.Split(';');
                Application.Current.MainWindow.Top = Double.Parse(position[0]);
                Application.Current.MainWindow.Left = Double.Parse(position[1]);
            }
        }

        private void ListView_Drop(object sender, DragEventArgs e)
        {
            bool exist = false;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                var dirs = (string[])e.Data.GetData(DataFormats.FileDrop);
                for (int i = 0; i < dirs.Length; i++)
                {
                    FileInformation info1 = new FileInformation(Path.GetFileName(dirs[i]), Path.GetFileName(dirs[i]), "",
                        Path.GetDirectoryName(dirs[i]), Path.GetExtension(dirs[i]));
                    exist = false;
                    // kiem tra file da ton tai chua 
                    foreach (FileInformation info2 in info)
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
                foreach (FileInformation info2 in info)
                {
                    listView.Items.Add(info2);
                }
            }
        }

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
                var fileNames = dialog.FileNames;

                var dirs = dialog.FileNames.ToArray();
                for (int i = 0; i < dirs.Length; i++)
                {
                    FileInformation info1 = new FileInformation(Path.GetFileName(dirs[i]),
                        Path.GetFileName(dirs[i]), "", Path.GetDirectoryName(dirs[i]), Path.GetExtension(dirs[i]));
                    exist = false;
                    // kiem tra file da ton tai chua 
                    foreach (FileInformation info2 in info)
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

                listView.Items.Refresh();
                getPreviewFiles();
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
                    FileInformation info1 = new FileInformation(Path.GetFileName(dirs[i]), Path.GetFileName(dirs[i]), "",
                        Path.GetDirectoryName(dirs[i]), Path.GetExtension(dirs[i]));
                    exist = false;
                    // kiem tra file da ton tai chua 
                    foreach (FileInformation info2 in info)
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

                listView.Items.Refresh();
                getPreviewFiles();
            }
        }
        private void ClickBrowseFilesInFolder(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "D:\\";
            dialog.Multiselect = true;
            dialog.IsFolderPicker = true;
            bool exist = false;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                var fileNames = dialog.FileNames;

                var dirs = dialog.FileNames.ToArray();
                for (int i = 0; i < dirs.Length; i++)
                {
                    foreach (var dir in Directory.GetFiles(dirs[i]))
                    {
                        FileInformation info1 = new FileInformation(Path.GetFileName(dir), Path.GetFileName(dir), "",
                           Path.GetDirectoryName(dir), Path.GetExtension(dir));
                        exist = false;
                        // kiem tra file da ton tai chua 
                        foreach (FileInformation info2 in info)
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
                }
                
                listView.Items.Refresh();
                getPreviewFiles();
            }
        }
        private void ClickRefreshButton(object sender, RoutedEventArgs e)
        {
            //TODO: Sửa lỗi refresh button. Xóa đi nếu ko thực sự cần thiết.
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
                MessageBox.Show("There are no files to refresh!", "Attention", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }

        private void AddRuleButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedRule = RuleComboBox.SelectedItem as IRenameRule;
            var instance = selectedRule.Clone();
            ruleSource.Add(instance);
            RuleListBox.Items.Refresh();
            getPreviewFiles();
        }

        private void AddRule_Click(object sender, RoutedEventArgs e)
        {
            if (RuleComboBox.IsDropDownOpen == false)
            {
                RuleComboBox.IsDropDownOpen = true;
            }
        }

        private void EditRule_Click(object sender, RoutedEventArgs e)
        {
            Window editWindow = new EditWindow();
            editWindow.ShowDialog();
            var lines = File.ReadLines("rules.txt");
            rules.Clear();
            foreach (var line in lines)
            {
                var renameRule = factory.Create(line);
                rules.Add(renameRule);
                
                //update existing rules in ruleSource (if found)
                int index = ruleSource.FindIndex(r => r.Name == renameRule.Name);
                if (index != -1)
                {
                    ruleSource[index] = renameRule;
                }
            }
            RuleComboBox.Items.Refresh();
            getPreviewFiles();
        }

        private void RemoveRule_Click(object sender, RoutedEventArgs e)
        {
            if (RuleListBox.SelectedItem == null)
            {
                MessageBox.Show("Please choose an item in rules list to delete");
            }
            else
            {
                IRenameRule selected = RuleListBox.SelectedItem as IRenameRule;
                ruleSource.Remove(selected);
                RuleListBox.Items.Refresh();
                getPreviewFiles();
            }
        }

        public string getPreview(string origin)
        {
            string result = origin;
            foreach (IRenameRule rule in ruleSource)
            {
                result = rule.Process(result);
            }

            return result;
        }

        public void getPreviewFiles()
        {
            clearNewNameForAllFiles();
            foreach (FileInformation info1 in listView.Items)
            {
                info1.NewName = getPreview(info1.NewName);
            }

            listView.Items.Refresh();
        }

        public void clearNewNameForAllFiles()
        {
            foreach (FileInformation info1 in listView.Items)
            {
                info1.NewName = info1.Name;
            }
        }

        private void BatchButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (FileInformation file in listView.Items)
            {
                int countRule = 0;
                foreach (IRenameRule rule in ruleSource)
                {
                    countRule++;
                }

                if (countRule == 0)
                {
                    MessageBox.Show("Please use at least one rule to batch", "Error");
                    return;
                }

                try
                {
                    var newFile = new FileInfo(file.Directory + file.Name);
                    var targetPlace = file.Directory + file.NewName;
                    newFile.MoveTo(targetPlace);
                    file.NewName = Path.GetFileName(targetPlace);
                    file.Status = "Success";
                    listView.Items.Refresh();
                }

                catch (FileNotFoundException)
                {
                    file.Status = "Cannot find file. Check your directory again";
                    listView.Items.Refresh();
                }
            }
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            //prepare preset string to save
            string presets = String.Empty;
            if (ruleSource.Count > 0)
            {
                int i = 0;
                for (; i < ruleSource.Count - 1; i++)
                {
                    presets += ruleSource[i].ToString() + ";";
                }
                presets += ruleSource[i].ToString();
            }

            //get window size
            string windowWidth = Application.Current.MainWindow.Width.ToString();
            string windowHeight = Application.Current.MainWindow.Height.ToString();
            string size = windowWidth + ";" + windowHeight;

            //get window position
            string windowTop = Application.Current.MainWindow.Top.ToString();
            string windowLeft = Application.Current.MainWindow.Left.ToString();
            string pos = windowTop + ";" + windowLeft;

            //Save all to app.config
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove("presets");
            config.AppSettings.Settings.Add("presets", presets);
            config.AppSettings.Settings.Remove("windowPos");
            config.AppSettings.Settings.Add("windowPos", pos);
            config.AppSettings.Settings.Remove("windowSize");
            config.AppSettings.Settings.Add("windowSize", size);
            config.Save(ConfigurationSaveMode.Minimal);
            ConfigurationManager.RefreshSection("appSettings");
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = listView.SelectedItem as FileInformation;
            if(selectedItem ==null)
            {
                MessageBox.Show("Please choose file to delete");
            }    
            else
            {
                info.Remove(selectedItem);
                listView.Items.Refresh();
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            info.Clear();
            listView.Items.Refresh();
        }

        private void ShowAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Created by Pham Duy Minh & Tran Nhat Huy","About box", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ShowHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Home tab is used for files and folders, Rules tab is used for rules which is applied for object", "Help Box", MessageBoxButton.OK,MessageBoxImage.Information);
        }

        private void DeleteAllButton_Click(object sender, RoutedEventArgs e)
        {
            info.Clear();
            listView.Items.Refresh();
        }

        private void LoadPreset_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string presetFile = dialog.FileName;
                var lines = File.ReadLines(presetFile);
                ruleSource.Clear();
                foreach (string s in lines)
                {
                    try
                    {
                        ruleSource.Add(factory.Create(s));
                    }
                    catch (Exception exception)
                    {
                        //do nothing. Continue the loop
                    }
                }
            }

            RuleListBox.Items.Refresh();
        }

        private void SavePreset_Click(object sender, RoutedEventArgs e)
        {
            if (ruleSource.Count == 0)
            {
                MessageBox.Show("No rules to save into preset!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = "preset";
                saveFileDialog.DefaultExt = ".txt";
                saveFileDialog.Filter = "Text documents (.txt)|*.txt";

                if (saveFileDialog.ShowDialog() == true)
                {
                    using (var streamWriter = new StreamWriter(saveFileDialog.FileName))
                    {
                        foreach (IRenameRule rule in ruleSource)
                        {
                            streamWriter.WriteLine(rule.ToString());
                        }
                    }
                }
            }
        }

        private void RuleMenuItemDelete_OnClick(object sender, RoutedEventArgs e)
        {
            IRenameRule selected = RuleListBox.SelectedItem as IRenameRule;
            ruleSource.Remove(selected);
            getPreviewFiles();
            RuleListBox.Items.Refresh();
        }

        private void RemoveAllRule_Click(object sender, RoutedEventArgs e)
        {
            ruleSource.Clear();
            getPreviewFiles();
            RuleListBox.Items.Refresh();
        }
    }
}