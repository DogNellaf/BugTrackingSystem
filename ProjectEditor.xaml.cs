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
    /// <summary>
    /// Логика взаимодействия для ProjectEditor.xaml
    /// </summary>
    public partial class ProjectEditor : Window
    {
        private MainWindow window;
        private SQLiteBase dataBase;
        private Generator gen;
        private DataTable table;

        public ProjectEditor(MainWindow win, SQLiteBase db)
        {
            window = win;
            dataBase = db;

            InitializeComponent();

            table = dataBase.QueryToBase("SELECT * FROM Project", "Таблица с пользователями пустая", null);
            gen = new Generator(ref mainGrid, new string[] { "id", "title", "author" }, 3, table.Rows.Count);
            gen.GenerateElements();
            dataBase.LoadTableInBoxes(ref gen, "Project");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            dataBase.ReWriteTable(ref gen, "Project");
            window.ReloadBase();
            window.Visibility = Visibility.Visible;
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DataTable dt = dataBase.QueryToBase("SELECT * FROM Project",
                "Таблица пустая", null);
            dataBase.QueryToBase($"INSERT INTO Project (id, title, author) values ({dt.Rows.Count}, NULL, NULL)", "Не удалось добавить новое значение", null);
            table = dataBase.QueryToBase("SELECT * FROM Project", "Таблица с пользователями пустая", null);
            gen.RemoveElements();
            gen.GenerateElements(table.Rows.Count);
            dataBase.LoadTableInBoxes(ref gen, "Project");
        }

        private void button_confirm_Click(object sender, RoutedEventArgs e)
        {
            int rowCount;
            RowWindow row = new RowWindow();
            if (row.ShowDialog() == true)
            {
                rowCount = int.Parse(row.textbox_input.Text);
                table = dataBase.QueryToBase($"DELETE FROM Project WHERE id = {rowCount}; ", $"Не удалось удалить строку {rowCount}", null);
                gen.RemoveElements();

                table = dataBase.QueryToBase("SELECT * FROM Project", "Таблица с пользователями пустая", null);
                gen.GenerateElements(table.Rows.Count);
                dataBase.LoadTableInBoxes(ref gen, "Project");
            }

        }
    }
}
