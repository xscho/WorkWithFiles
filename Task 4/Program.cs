using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

class Program
{
    class Student
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public DateTime DateOfBirth { get; set; }
        public decimal AverageGrade { get; set; }
    }

    static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("Введите путь к бинарному файлу: ");
            string filePath = Console.ReadLine();

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Файл по пути {filePath} не существует.");
                return;
            }

            List<Student> students = ReadStudentsFromBinaryFile(filePath);

            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string directoryPath = Path.Combine(desktop, "Students");
            Directory.CreateDirectory(directoryPath);

            Dictionary<string, List<Student>> groups = new Dictionary<string, List<Student>>();

            foreach (var student in students)
            {
                if (!groups.ContainsKey(student.Group))
                {
                    groups[student.Group] = new List<Student>();
                }
                groups[student.Group].Add(student);
            }

            foreach (var group in groups)
            {
                string groupFilePath = Path.Combine(directoryPath, $"{group.Key}.txt");
                using (StreamWriter writer = new StreamWriter(groupFilePath, false, Encoding.UTF8))
                {
                    foreach (var student in group.Value)
                    {
                        writer.WriteLine($"{student.Name}, {student.DateOfBirth.ToShortDateString()}, {student.AverageGrade}");
                    }
                }
            }

            Console.WriteLine($"Данные успешно загружены и распределены по группам в папке {directoryPath}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }

    static List<Student> ReadStudentsFromBinaryFile(string filePath)
    {
        List<Student> students = new List<Student>();

        using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        using (BinaryReader reader = new BinaryReader(fs))
        {
            try
            {
                while (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    string name = ReadString(reader);
                    string group = ReadString(reader);
                    DateTime dateOfBirth = new DateTime(reader.ReadInt64());
                    decimal averageGrade = reader.ReadDecimal();

                    students.Add(new Student
                    {
                        Name = name,
                        Group = group,
                        DateOfBirth = dateOfBirth,
                        AverageGrade = averageGrade
                    });
                }
            }
            catch (EndOfStreamException ex)
            {
                Console.WriteLine("Произошла ошибка при чтении файла: конец файла достигнут неожиданно.");
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при чтении файла: " + ex.Message);
            }
        }

        return students;
    }

    static string ReadString(BinaryReader reader)
    {
        try
        {
            int length = reader.ReadInt32();
            if (length < 0)
                throw new IOException("Некорректная длина строки.");

            byte[] bytes = reader.ReadBytes(length);
            if (bytes.Length != length)
                throw new EndOfStreamException("Не удалось прочитать полную строку.");

            return Encoding.UTF8.GetString(bytes);
        }
        catch (EndOfStreamException ex)
        {
            throw new EndOfStreamException("Не удалось прочитать строку: конец потока достигнут неожиданно.", ex);
        }
    }
}