using System;
using System.Collections.Generic;
using System.Data;
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

namespace BugTrackingSystem
{
    public partial class UserEditor : Window
    {
        private MainWindow window;
        private SQLiteBase dataBase;
        public UserEditor(MainWindow win, SQLiteBase db)
        {
            window = win;
            dataBase = db;

            DataTable table = dataBase.ReadData("SELECT * FROM Users", "Таблица с пользователями пустая", null);

            Generator.CreateElements(table);

            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            window.Visibility = Visibility.Visible;
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
