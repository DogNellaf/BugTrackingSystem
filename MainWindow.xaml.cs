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

namespace BugTrackingSystem
{
    public partial class MainWindow : Window
    {
        public string FilePath = "";
        private SQLiteBase dataBase;

        public MainWindow()
        {
            InitializeComponent();
            Logger.ClearLogFile();
        }

        private void button_new_Click(object sender, RoutedEventArgs e)
        {
            //получаем название файла до папки
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "(*.sqlite3)|*.sqlite3",
            };

            string dbPath = "Error";
            if (saveFileDialog.ShowDialog() == true)
            {
                dbPath = saveFileDialog.FileName;
                Console.WriteLine(FilePath);
            }

            dataBase = new SQLiteBase(dbPath);
            dataBase.CreateNewBase();

        }

        private void button_load_Click(object sender, RoutedEventArgs e)
        {
            //получаем путь до папки
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
                Console.WriteLine(FilePath);
            }

            //если файл существует
            if (File.Exists(FilePath))
            {
                SQLiteConnection databaseConnection = 
                    new SQLiteConnection($"Data Source={FilePath};;Version=3;");
                databaseConnection.Open();
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

class SQLiteBase
{
    private SQLiteConnection databaseConnection;
    private SQLiteCommand databaseCommand;
    private string dbPath;

    public SQLiteBase(string path)
    {
        dbPath = path;
        createNewConnection();
    }

    public void CreateNewBase()
    {
        SQLiteConnection.CreateFile(dbPath);
        createNewConnection();

        databaseConnection.Open();
        createNewTable("Users", "user TEXT, role TEXT");
        createNewTable("Tasks", "project INTEGER, title TEXT, type TEXT, priority TEXT, user INTEGER, description TEXT");
        createNewTable("Project", "title TEXT, author TEXT");
        databaseConnection.Close();

        Logger.WriteRow("System",$"Создана новая база данных. {dbPath}");

    }

    public void LoadBase(string path)
    {
        dbPath = path;
        createNewConnection();
    }

    private void createNewConnection()
    {
        try
        {
            databaseConnection = new SQLiteConnection($"Data Source={dbPath};;Version=3;");
            databaseCommand = new SQLiteCommand(databaseConnection);
        }
        catch
        {

        }
    }

    private void createNewTable(string tableName, string rows)
    {
        databaseCommand.CommandText = $"CREATE TABLE IF NOT EXISTS {tableName} (id INTEGER PRIMARY KEY AUTOINCREMENT, {rows})";
        databaseCommand.ExecuteNonQuery();
    }
}

static class Logger
{
    private static string path = "log.txt";

    public static void ClearLogFile()
    {
        File.Delete(path);
    }

    public static void WriteRow(string title, string text)
    {
        StreamWriter log = new StreamWriter(path);

        // Московское время +3
        log.WriteLine($"[{DateTime.UtcNow.Hour + 3}:{DateTime.UtcNow.Minute}:{DateTime.UtcNow.Second}][{title}] - {text}");
        log.Close();
    }
}