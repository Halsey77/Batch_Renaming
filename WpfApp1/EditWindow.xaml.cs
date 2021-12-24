using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        public EditWindow()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if(finderTextBox.Text== ""|| replacerTextBox.Text=="")
            {
                MessageBox.Show("Please enter all input required");
            }    
            else
            {
                string lines = "Replacer " + finderTextBox.Text + " " + replacerTextBox.Text;
                List<string> quotelist = File.ReadAllLines("rules.txt").ToList();
                foreach(string line in quotelist)
                {

                    if(line.Contains("Replacer"))
                    {
                        quotelist.Remove(line);
                        break;
                    }
                }
                quotelist.Add(lines);
                File.WriteAllLines("rules.txt", quotelist.ToArray());

            }
        }
    }
}
