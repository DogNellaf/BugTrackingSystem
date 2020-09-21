using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTrackingSystem
{
    public static class Logger
    {
        private static string path = "log.txt";

        //очистка лог файла путем создания нового пустого
        public static void ClearLogFile()
        {
            File.Delete(path);
            FileStream temp = File.Create(path);
            temp.Close();
        }


        //запись новой строки
        public static void WriteRow(string title, string text)
        {
            //считываем файл
            StreamReader logReader = new StreamReader(path);
            string logText = logReader.ReadToEnd();
            logReader.Close();

            //добавляем новую строку и записываем все в файл
            StreamWriter logWriter = new StreamWriter(path);
            logWriter.WriteAsync(logText + $"[{DateTime.UtcNow.Hour + 3}:{DateTime.UtcNow.Minute}:{DateTime.UtcNow.Second}][{title}] - {text}\n"); // Московское время +3
            logWriter.Close();
        }
    }
}
