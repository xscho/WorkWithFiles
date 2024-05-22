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
            long initialSize = CalculateFolderSize(folderPath);
            Console.WriteLine($"Исходный размер папки '{folderPath}' составляет {initialSize} байт.");

            long freedSize = ClearFolder(folderPath);
            long currentSize = CalculateFolderSize(folderPath);

            Console.WriteLine($"Освобождено: {freedSize} байт.");
            Console.WriteLine($"Текущий размер папки '{folderPath}' составляет {currentSize} байт.");
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
            //Файлы в папке
            string[] files = Directory.GetFiles(folderPath);
            foreach (string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                folderSize += fileInfo.Length;
            }

            //папки
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

    static long ClearFolder(string folderPath)
    {
        long freedSize = 0;

        try
        {
            //файлы
            string[] files = Directory.GetFiles(folderPath);
            foreach (string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                freedSize += fileInfo.Length;
                fileInfo.Delete();
            }

            //подкаталоги
            string[] subFolders = Directory.GetDirectories(folderPath);
            foreach (string subFolder in subFolders)
            {
                freedSize += ClearFolder(subFolder);
                Directory.Delete(subFolder);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ошибка при очистке папки '{folderPath}': {e.Message}");
        }

        return freedSize;
    }
}
