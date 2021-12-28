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
            if (finderTextBox.Text == "" || replacerTextBox.Text == "" || prefixTextBox.Text == ""||suffixTextBox.Text==""||startValue.Text==""||step.Text==""||NumberOfDigits.Text=="")
            {
                MessageBox.Show("Please enter all input required");
            }    
            else
            {
                string line1 = "Replacer " + finderTextBox.Text + " " + replacerTextBox.Text;
                string line2 = "SuffixCase " + suffixTextBox.Text;
                string line3="PrefixCase " +prefixTextBox.Text;
                string line4 = "EndCounterCase " + startValue.Text + " " + step.Text + " " + NumberOfDigits.Text;
                List<string> quotelist = File.ReadAllLines("rules.txt").ToList();
                for(int i=0;i<quotelist.Count;i++)
                {

                    if(quotelist[i].Contains("Replacer")||quotelist[i].Contains("SuffixCase")||quotelist[i].Contains("PrefixCase")||quotelist[i].Contains("EndCounterCase"))
                    {
                        quotelist.RemoveAt(i);
                        i--;
                        
                    }
                }
                quotelist.Add(line1);
                quotelist.Add(line2);
                quotelist.Add(line3);
                quotelist.Add(line4);
                File.WriteAllLines("rules.txt", quotelist.ToArray());
                DialogResult = true;
                

            }
        }
    }
}
