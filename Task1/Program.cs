using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Введите путь до папки как аргумент командной строки");
            return;
        }

        string folderPath = args[0];

        CleanFolder(folderPath);

    }

    static void CleanFolder(string folderPath)
    {
        if (!Directory.Exists(folderPath))
        {
            Console.WriteLine("Указанная папка не существует");
            return;
        }

        try
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
            DateTime currentDateTime = DateTime.Now;
            TimeSpan timeSpan = TimeSpan.FromMinutes(30);

            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                if (currentDateTime - file.LastAccessTime > timeSpan)
                {
                    file.Delete();
                    Console.WriteLine($"Файл {file.Name} удален");
                }
            }

            foreach (DirectoryInfo subDirectory in directoryInfo.GetDirectories())
            {
                if (currentDateTime - subDirectory.LastAccessTime > timeSpan)
                {
                    subDirectory.Delete(true);
                    Console.WriteLine($"Папка {subDirectory.Name} удалена");
                }
                else
                {
                    CleanFolder(subDirectory.FullName);
                }
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"Ошибка доступа к папке: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }
}