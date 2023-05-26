using System;
using System.IO;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;

        if (args.Length < 2)
        {
            PrintHelp();
            return;
        }
        // Виводимо довідку при недостатній кількості параметрів
        static void PrintHelp()
        {
            Console.WriteLine("Використання: Program.exe <шлях_до_каталогу> <шаблон_файлу>");
            Console.WriteLine("Параметри:");
            Console.WriteLine("  <шлях_до_каталогу>  - Шлях до каталогу, в якому треба підрахувати кількість файлів");
            Console.WriteLine("  <шаблон_файлу>      - Шаблон для вибору файлів (наприклад, *.txt)");
            Console.WriteLine();
            Console.WriteLine("Приклад виклику: Program.exe C:\\MyFolder *.txt");
        }

        string directoryPath = args[0];
        string filePattern = args[1];


        // Перевіряємо, чи існує вказаний каталог
        if (!Directory.Exists(directoryPath))
        {
            Console.WriteLine("Вказаний каталог не існує.");
            return;
        }

        try
        {
            int fileCount = CountFilesInSubdirectories(directoryPath, filePattern);
            Console.WriteLine("Загальна кількість файлів: " + fileCount);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Сталася помилка: " + ex.Message);
        }
    }

    static int CountFilesInSubdirectories(string directoryPath, string filePattern)
    {
        int fileCount = 0;

        foreach (string file in Directory.GetFiles(directoryPath, filePattern, SearchOption.AllDirectories))
        {
            FileAttributes attributes = File.GetAttributes(file);

            // Враховуємо атрибути файлу
            if ((attributes & FileAttributes.Hidden) != 0 ||
                (attributes & FileAttributes.ReadOnly) != 0 ||
                (attributes & FileAttributes.Archive) != 0)
            {
                continue; // Пропускаємо файли з відповідними атрибутами
            }

            fileCount++;
        }

        return fileCount;
    }
}