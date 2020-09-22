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
            bool isHaveEmpty = false;
            //проверяем поля
            for (int i = 0; i < table.Rows.Count; i++)
            {
                string temp = gen.generatedTextBoxed[1, i].Text; //проект
                string temp2 = gen.generatedTextBoxed[5, i].Text; //исполнитель
                if (temp == "" || temp == "NULL" || temp == "Null" || temp == "null")
                {
                    isHaveEmpty = true;
                    MessageBox.Show("Проект не может быть пустым!");
                    Logger.WriteRow("Error", "Пользователь попытался сохранить данные задач с пустым полем под проект!");
                    break;
                }
                else if (temp2 == "" || temp2 == "NULL" || temp2 == "Null" || temp2 == "null")
                {
                    isHaveEmpty = true;
                    MessageBox.Show("Исполнитель не может быть пустым!");
                    Logger.WriteRow("Error", "Пользователь попытался сохранить данные задач с пустым полем под исполнителя!");
                    break;
                }
            }

            DataTable users = dataBase.QueryToBase("SELECT * FROM Users", "Таблица с пользователями пустая", null);
            DataTable project = dataBase.QueryToBase("SELECT * FROM Project", "Таблица с проектами пустая", null);

            if (!isHaveEmpty)
            {
                if (project.Rows.Count == 0 || users.Rows.Count == 0)
                {
                    dataBase.ReWriteTable(ref gen, "Tasks");
                    window.ReloadBase();
                    window.Visibility = Visibility.Visible;
                    Close();
                }
                else
                {
                    bool usersIsCorrect = false;
                    bool projectsIsCorrect = false;

                    //проверка по проектам
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        for (int j = 0; j < project.Rows.Count; j++)
                        {
                            if (gen.generatedTextBoxed[1, i].Text == project.Rows[j].ItemArray[0].ToString())
                            {
                                projectsIsCorrect = true;
                                break;
                            }
                            else
                            {
                                projectsIsCorrect = false;
                            }
                        }
                        if (!projectsIsCorrect)
                            break;
                    }

                    //проверка по пользователям
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        for (int j = 0; j < users.Rows.Count; j++)
                        {
                            if (gen.generatedTextBoxed[5, i].Text == users.Rows[j].ItemArray[0].ToString())
                            {
                                usersIsCorrect = true;
                                break;
                            }
                            else
                            {
                                usersIsCorrect = false;
                            }
                        }
                        if (!usersIsCorrect)
                            break;
                    }

                    if (usersIsCorrect && projectsIsCorrect)
                    {
                        dataBase.ReWriteTable(ref gen, "Tasks");
                        window.ReloadBase();
                        window.Visibility = Visibility.Visible;
                        Close();
                    }
                    else
                    {
                        MessageBox.Show(@"Введите реальные проект\пользователя!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        Logger.WriteRow("Warning", "Пользователь ввел несуществующие данные!");
                    }
                }
                

            }
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
