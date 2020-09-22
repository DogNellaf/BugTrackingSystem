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

    public partial class TaskEditor : Window
    {
        private MainWindow window;
        private SQLiteBase dataBase;
        private Generator gen;
        private DataTable table;

        public TaskEditor(MainWindow win, SQLiteBase db)
        {
            window = win;
            dataBase = db;

            InitializeComponent();

            table = dataBase.QueryToBase("SELECT * FROM Tasks", "Таблица с пользователями пустая", null);
            gen = new Generator(ref mainGrid, new string[] { "id", "project", "title", "type", "priority", "user", "description" }, 7, table.Rows.Count);
            gen.GenerateElements();
            dataBase.LoadTableInBoxes(ref gen, "Tasks");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            dataBase.ReWriteTable(ref gen, "Tasks");
            window.ReloadBase();
            window.Visibility = Visibility.Visible;
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DataTable dt = dataBase.QueryToBase("SELECT * FROM Tasks",
                "Таблица пустая", null);
            dataBase.QueryToBase($"INSERT INTO Tasks (id, project, title, type, priority, user, description) values ({dt.Rows.Count}, NULL, NULL, NULL, NULL, NULL, NULL)", "Не удалось добавить новое значение", null);
            table = dataBase.QueryToBase("SELECT * FROM Tasks", "Таблица с пользователями пустая", null);
            gen.RemoveElements();
            gen.GenerateElements(table.Rows.Count);
            dataBase.LoadTableInBoxes(ref gen, "Tasks");
        }

        private void button_confirm_Click(object sender, RoutedEventArgs e)
        {
            int rowCount;
            RowWindow row = new RowWindow();
            if (row.ShowDialog() == true)
            {
                rowCount = int.Parse(row.textbox_input.Text);
                table = dataBase.QueryToBase($"DELETE FROM Tasks WHERE id = {rowCount}; ", $"Не удалось удалить строку {rowCount}", null);
                gen.RemoveElements();

                table = dataBase.QueryToBase("SELECT * FROM Tasks", "Таблица с пользователями пустая", null);
                gen.GenerateElements(table.Rows.Count);
                dataBase.LoadTableInBoxes(ref gen, "Tasks");
            }

        }
    }
}
