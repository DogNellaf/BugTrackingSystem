using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BugTrackingSystem
{
    public partial class MainWindow : Window
    {
        public string FilePath = "";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_load_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
                Console.WriteLine(FilePath);
            }

        }

        private void button_usereditor_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button_taskeditor_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button_projecteditor_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
