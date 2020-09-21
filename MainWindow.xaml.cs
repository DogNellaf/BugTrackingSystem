using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BugTrackingSystem;

namespace BugTrackingSystem
{
    //взаимодействие с пользовательским интерфейсом
    public partial class MainWindow : Window
    {
        //путь до базы данных и сама база данных
        public string FilePath = "";

        private SQLiteBase dataBase = new SQLiteBase();

        public MainWindow()
        {
            InitializeComponent();
            Logger.ClearLogFile();
        }

        //нажатие на кнопку создания нового файла
        private void button_new_Click(object sender, RoutedEventArgs e)
        {
            //получаем название файла до папки
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "(*.sqlite3)|*.sqlite3",
            };

            //если пользователь сохранил файл, то создаем базу данных, иначе логируем отказ
            if (saveFileDialog.ShowDialog() == true)
            {
                FilePath = saveFileDialog.FileName;
                dataBase = new SQLiteBase(FilePath);
                dataBase.CreateNewBase();
            }
            else
            {
                Logger.WriteRow("System", $"Пользователь передумал создавать новую базу данных;");
            }
        }

        //нажатие на кнопку загрузки базы данных
        private void button_load_Click(object sender, RoutedEventArgs e)
        {
            
            OpenFileDialog openFileDialog = new OpenFileDialog();

            //если пользователь загрузил файл, то создаем базу данных, иначе логируем отказ
            if (openFileDialog.ShowDialog() == true)
            {

                //подгружаем базу
                dataBase.dbPath = openFileDialog.FileName;
                dataBase.LoadBase();

                textbox_ListOfUsers.Text = "";
                textbox_ListOfProjects.Text = "";

                //загружаем все таблицы
                dataBase.ReadData("SELECT * FROM Users", "Таблица пустая", textbox_ListOfUsers);
                dataBase.ReadData("SELECT * FROM Project", "Таблица пустая", textbox_ListOfProjects);
            }
            else
            {
                Logger.WriteRow("System", $"Пользователь передумал загружать базу данных из файла;");
            }
        }

        private void button_usereditor_Click(object sender, RoutedEventArgs e)
        {
            UserEditor editor = new UserEditor(this, dataBase);
            editor.Show();
            Visibility = Visibility.Hidden;
        }

        private void button_taskeditor_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button_projecteditor_Click(object sender, RoutedEventArgs e)
        {

        }

        private void text_NameOfProject_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void button_loadByProject_Click(object sender, RoutedEventArgs e)
        {
            textbox_ListOfTasksInProject.Text = "";
            if (!(textbox_NameOfProject.Text == ""))
            {
                if (int.TryParse(textbox_NameOfProject.Text, out int number))
                {
                    dataBase.ReadData($"SELECT * FROM Tasks T WHERE T.project = {number}", 
                        "Проект не найден или в проекте нет задач;", textbox_ListOfTasksInProject);
                }
                else
                {
                    Logger.WriteRow("Error", $"Не удалось считать номер проекта;");
                }
            }
            else
            {
                Logger.WriteRow("Warning", $"Пользователь попыталя произвести выборку задач по проекту без номера проекта;");
            }
        }

        private void button_loadByUser_Click(object sender, RoutedEventArgs e)
        {
            if (!(textbox_NameOfUser.Text == ""))
            {
                dataBase.ReadData($"SELECT * FROM Tasks WHERE user = '{textbox_NameOfUser.Text}'",
                    "Пользователь не найден или на пользователе нет задач;", textbox_ListOfTasksInUser);
            }
            else
            {
                Logger.WriteRow("Warning", $"Пользователь попыталя произвести выборку задач по исполнителю без имени пользователя;");
            }
        }
    }
}