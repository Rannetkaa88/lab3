using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;

[Serializable]
public class Toy
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int MinAge { get; set; }
    public int MaxAge { get; set; }

    public Toy() { }

    public Toy(string name, decimal price, int minAge, int maxAge)
    {
        Name = name;
        Price = price;
        MinAge = minAge;
        MaxAge = maxAge;
    }

    public override string ToString()
    {
        return $"Название: {Name}, Цена: {Price:C}, Возраст: {MinAge}-{MaxAge} лет";
    }
}

class Program
{
    static void Main()
    {
        try
        {
            // Работа с текстовым файлом
            string textFilePath = "numbers.txt";
            Console.WriteLine("=== Работа с текстовым файлом ===");
            FillFileWithRandomNumbers(textFilePath, 10, 5);  // Заполняем 10 строк по 5 чисел в каждой
            Console.WriteLine("Содержимое текстового файла:");
            DisplayFileContents(textFilePath);
            int evenCount = CountEvenNumbers(textFilePath);
            Console.WriteLine($"Количество чётных чисел в файле: {evenCount}");

            int sumOfMatchingIndex = CalculateSumOfMatchingIndex(textFilePath);
            Console.WriteLine($"Сумма элементов, равных своим индексам: {sumOfMatchingIndex}");

            // Работа с бинарным файлом
            Console.WriteLine("\n=== Работа с бинарным файлом ===");
            string inputBinaryPath = "input.bin";
            string outputBinaryPath = "output.bin";

            Console.Write("Введите число k (для фильтрации чисел): ");
            int k;
            while (!int.TryParse(Console.ReadLine(), out k) || k == 0)
            {
                Console.WriteLine("Введите корректное ненулевое целое число.");
            }

            FillBinaryFile(inputBinaryPath, 10, 1, 10);
            ReadAndPrintBinaryFile(inputBinaryPath);
            FilterBinaryFile(inputBinaryPath, outputBinaryPath, k);
            ReadAndPrintBinaryFile(outputBinaryPath);

            // Работа с XML файлом игрушек
            Console.WriteLine("\n=== Работа с файлом игрушек ===");
            string toysFilePath = "toys.xml";
            FillToysFile(toysFilePath);
            DisplayAllToys(toysFilePath);
            FindSuitableToys(toysFilePath, 3);

            // Работа с текстовым файлом для извлечения первых символов
            Console.WriteLine("\n=== Работа с файлом для извлечения первых символов ===");
            string inputTextFilePath = "input.txt";
            string outputTextFilePath = "output.txt";
            CreateInputFile(inputTextFilePath);
            Console.WriteLine("Содержимое исходного файла:");
            DisplayFileContents(inputTextFilePath);
            ExtractFirstCharacters(inputTextFilePath, outputTextFilePath);
            Console.WriteLine("\nСодержимое нового файла:");
            DisplayFileContents(outputTextFilePath);
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Ошибка ввода-вывода: {ex.Message}");
        }
        catch (FormatException ex)
        {
            Console.WriteLine($"Ошибка формата: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }

        Console.WriteLine("\nНажмите любую клавишу для завершения программы...");
        Console.ReadKey();
    }

    // Методы для работы с текстовым файлом
    static void FillFileWithRandomNumbers(string path, int lines, int numbersPerLine)
    {
        Random random = new Random();
        using (StreamWriter writer = new StreamWriter(path))
        {
            for (int i = 0; i < lines; i++)
            {
                for (int j = 0; j < numbersPerLine; j++)
                {
                    writer.Write(random.Next(0, 100));
                    if (j < numbersPerLine - 1)
                    {
                        writer.Write(" ");
                    }
                }
                writer.WriteLine();
            }
        }
    }

    static void DisplayFileContents(string path)
    {
        using (StreamReader reader = new StreamReader(path))
        {
            string line;
            int lineNumber = 1;
            while ((line = reader.ReadLine()) != null)
            {
                Console.WriteLine($"Строка {lineNumber}: {line}");
                lineNumber++;
            }
        }
    }

    static int CountEvenNumbers(string path)
    {
        int evenCount = 0;

        using (StreamReader reader = new StreamReader(path))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] numbers = line.Split(' ');

                foreach (string number in numbers)
                {
                    if (int.TryParse(number, out int num) && num % 2 == 0)
                    {
                        evenCount++;
                    }
                }
            }
        }
        return evenCount;
    }

    static int CalculateSumOfMatchingIndex(string path)
    {
        int sum = 0;
        int index = 0;

        using (StreamReader reader = new StreamReader(path))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] numbers = line.Split(' ');
                foreach (string number in numbers)
                {
                    if (int.TryParse(number, out int num) && num == index)
                    {
                        sum += num;
                    }
                    index++;
                }
            }
        }
        return sum;
    }

    // Методы для работы с бинарным файлом
    static void FillBinaryFile(string filePath, int count, int minValue, int maxValue)
    {
        Random random = new Random();
        using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
        {
            for (int i = 0; i < count; i++)
            {
                writer.Write(random.Next(minValue, maxValue));
            }
        }
        Console.WriteLine($"Файл \"{filePath}\" успешно заполнен случайными числами.");
    }

    static void FilterBinaryFile(string inputFilePath, string outputFilePath, int k)
    {
        using (BinaryReader reader = new BinaryReader(File.Open(inputFilePath, FileMode.Open)))
        using (BinaryWriter writer = new BinaryWriter(File.Open(outputFilePath, FileMode.Create)))
        {
            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                int number = reader.ReadInt32();
                if (number % k != 0)
                {
                    writer.Write(number);
                }
            }
        }
    }

    static void ReadAndPrintBinaryFile(string filePath)
    {
        using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
        {
            Console.WriteLine($"Содержимое файла \"{filePath}\":");
            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                Console.Write(reader.ReadInt32() + " ");
            }
            Console.WriteLine();
        }
    }

    // Методы для работы с XML файлом игрушек
    static void FillToysFile(string fileName)
    {
        List<Toy> toys = new List<Toy>
        {
            new Toy("Мяч", 500, 2, 10),
            new Toy("Кукла", 1500, 3, 7),
            new Toy("Конструктор", 2000, 4, 8),
            new Toy("Машинка", 800, 2, 5),
            new Toy("Пазл", 600, 3, 6)
        };

        XmlSerializer serializer = new XmlSerializer(typeof(List<Toy>));
        using (FileStream fs = new FileStream(fileName, FileMode.Create))
        {
            serializer.Serialize(fs, toys);
        }
    }

    static List<Toy> DeserializeToysFile(string fileName)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<Toy>));
        using (FileStream fs = new FileStream(fileName, FileMode.Open))
        {
            return (List<Toy>)serializer.Deserialize(fs);
        }
    }

    static void DisplayAllToys(string fileName)
    {
        List<Toy> toys = DeserializeToysFile(fileName);
        Console.WriteLine("\nВсе игрушки в файле:");
        foreach (var toy in toys)
        {
            Console.WriteLine(toy);
        }
    }

    static void FindSuitableToys(string fileName, int childAge)
    {
        List<Toy> toys = DeserializeToysFile(fileName);
        Console.WriteLine($"\nПоиск игрушек для ребенка {childAge} лет (кроме мяча):");
        bool found = false;

        foreach (var toy in toys)
        {
            if (toy.Name.ToLower() != "мяч" && childAge >= toy.MinAge && childAge <= toy.MaxAge)
                {
                Console.WriteLine(toy);
                found = true;
            }
        }

        if (!found)
        {
            Console.WriteLine("Подходящих игрушек не найдено.");
        }
    }

    // Методы для работы с текстовыми файлами для извлечения первых символов
    static void CreateInputFile(string path)
    {
        string[] lines = {
            "Hello, World!",
            "C# Programming",
            "Lab 3",
            "Text File Processing",
            "Extracting First Characters"
        };

        try
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                foreach (string line in lines)
                {
                    writer.WriteLine(line);
                }
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Ошибка при записи в файл: {ex.Message}");
            throw; // Пробрасываем исключение дальше
        }
    }

    static void ExtractFirstCharacters(string inputPath, string outputPath)
    {
        try
        {
            using (StreamReader reader = new StreamReader(inputPath))
            using (StreamWriter writer = new StreamWriter(outputPath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Проверяем, что строка не пустая
                    if (line.Length > 0)
                    {
                        // Записываем первый символ в новый файл
                        writer.WriteLine(line[0]);
                    }
                }
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Ошибка при обработке файлов: {ex.Message}");
            throw; // Пробрасываем исключение дальше
        }
    }
}
