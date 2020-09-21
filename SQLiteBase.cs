using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BugTrackingSystem
{
    //класс взаимодействия с базой данных
    public class SQLiteBase
    {
        //соединение с базой данных и команда
        private SQLiteConnection databaseConnection;
        private SQLiteCommand databaseCommand;
        
        
        //конструктор для начальной инциализации
        public SQLiteBase() { }

        public string dbPath;

        //конструктор для подключения к новой базе данных
        public SQLiteBase(string path)
        {
            dbPath = path;
            createNewConnection();
        }

        //создание новой базы 
        public void CreateNewBase()
        {
            //создаем пустую базу и подключаемся к ней
            SQLiteConnection.CreateFile(dbPath);
            createNewConnection();

            //добавляем в базу стандартные таблицы
            databaseConnection.Open();
            createNewTable("Users", "user TEXT, role TEXT");
            createNewTable("Tasks", "project INTEGER, title TEXT, type TEXT, priority TEXT, user INTEGER, description TEXT");
            createNewTable("Project", "title TEXT, author TEXT");
            databaseConnection.Close();

            Logger.WriteRow("System", $"Создана новая база данных {dbPath};");

        }

        //загрузка базы
        public void LoadBase()
        {
            //если удалось установить соединение
            if (createNewConnection())
            {
                Logger.WriteRow("System", $"Установлено соединение с базой данных {dbPath};");
            }
            else
            {
                Logger.WriteRow("Error", $"Не удалось установить соединение с базой данных {dbPath};");
            }

        }

        //функция чтения данных из таблицы
        public void ReadTable(string table, TextBox tb)
        {
            DataTable dTable = new DataTable();
            try
            {
                string query = $"SELECT * FROM {table}";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, databaseConnection);
                adapter.Fill(dTable);

                Logger.WriteRow("System", $"Считана таблица {table};");

                if (dTable.Rows.Count > 0)
                {
                    tb.Text = "";

                    for (int i = 0; i < dTable.Rows.Count; i++)
                    {
                        object[] temp = dTable.Rows[i].ItemArray;
                        for (int j = 0; j < temp.Length; j++)
                        {
                            tb.Text += $"{dTable.Rows[i].ItemArray[j]}; ";
                        }
                        tb.Text += "\n";
                    }
                        
                }
            }
            catch (SQLiteException ex)
            {
                Logger.WriteRow("Error", $"Вызвано исключение: {ex.Message}");
            }
        }

        //создаем новое подключение к базе данных
        private bool createNewConnection()
        {
            //пробуем подключиться
            try
            {
                databaseConnection = new SQLiteConnection($"Data Source={dbPath};;Version=3;");
                databaseCommand = new SQLiteCommand(databaseConnection);
            }
            catch (SQLiteException ex) // если исключение, то логируем результат
            {
                Logger.WriteRow("Error", $"Вызвано исключение: {ex.Message}");
                return false;
            }
            return true;
        }

        //создание новой таблицы
        private void createNewTable(string tableName, string rows)
        {
            databaseCommand.CommandText = $"CREATE TABLE IF NOT EXISTS {tableName} (id INTEGER PRIMARY KEY AUTOINCREMENT, {rows})";
            databaseCommand.ExecuteNonQuery();
            Logger.WriteRow("System", $"Создана таблица {tableName};");
        }

        
    }
}
