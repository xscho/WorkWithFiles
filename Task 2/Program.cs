using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Введите путь к папке: ");
        string folderPath = Console.ReadLine();

        // Проверка, что папка существует
        if (!Directory.Exists(folderPath))
        {
            Console.WriteLine("Указанная папка не существует.");
            return;
        }

        try
        {
            long folderSize = CalculateFolderSize(folderPath);
            Console.WriteLine($"Размер папки '{folderPath}' составляет {folderSize} байт.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ошибка: {e.Message}");
        }

        Console.ReadKey();

    }

    static long CalculateFolderSize(string folderPath)
    {
        long folderSize = 0;

        try
        {
            // Файлы в папке
            string[] files = Directory.GetFiles(folderPath);
            foreach (string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                folderSize += fileInfo.Length;
            }

            // папки
            string[] subFolders = Directory.GetDirectories(folderPath);
            foreach (string subFolder in subFolders)
            {
                folderSize += CalculateFolderSize(subFolder);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ошибка при обработке папки '{folderPath}': {e.Message}");
        }

        return folderSize;
    }
}