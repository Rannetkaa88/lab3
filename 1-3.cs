using System;
using System.Text;

class Matrix
{
    private int[,] data;

    // Конструкторы
    public Matrix(int rows, int cols)
    {
        data = new int[rows, cols];
    }

    // Конструктор для ввода с клавиатуры (первая матрица)
    public Matrix(int rows, int cols, bool isManualInput)
    {
        data = new int[rows, cols];
        if (isManualInput)
        {
            Console.WriteLine("\nЗаполнение массива по столбцам:");
            for (int j = 0; j < cols; j++)
            {
                for (int i = 0; i < rows; i++)
                {
                    Console.Write($"Введите элемент [{i},{j}]: ");
                    data[i, j] = int.Parse(Console.ReadLine());
                }
            }
        }
    }

    // Конструктор для случайной генерации (вторая матрица)
    public Matrix(int n)
    {
        data = new int[n, n];
        Random rand = new Random();
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (i + j < n - 1)
                    data[i, j] = rand.Next(-12, 4566);
                else
                    data[i, j] = rand.Next(-1024, 1025);
            }
        }
    }

    // Конструктор для константной матрицы (третья матрица)
    public Matrix()
    {
        data = new int[5, 5]
        {
            { 0,  0,  0,  0, 15},
            { 0,  0,  0, 14, 10},
            { 0,  0, 13,  9,  6},
            { 0, 12,  8,  5,  3},
            {11,  7,  4,  2,  1}
        };
    }

    // Перегрузка операторов
    public static Matrix operator *(int scalar, Matrix matrix)
    {
        Matrix result = new Matrix(matrix.data.GetLength(0), matrix.data.GetLength(1));
        for (int i = 0; i < matrix.data.GetLength(0); i++)
            for (int j = 0; j < matrix.data.GetLength(1); j++)
                result.data[i, j] = scalar * matrix.data[i, j];
        return result;
    }

    public static Matrix operator +(Matrix a, Matrix b)
    {
        if (a.data.GetLength(0) != b.data.GetLength(0) ||
            a.data.GetLength(1) != b.data.GetLength(1))
            throw new ArgumentException("Несовместимые размеры матриц");

        Matrix result = new Matrix(a.data.GetLength(0), a.data.GetLength(1));
        for (int i = 0; i < a.data.GetLength(0); i++)
            for (int j = 0; j < a.data.GetLength(1); j++)
                result.data[i, j] = a.data[i, j] + b.data[i, j];
        return result;
    }

    public static Matrix operator -(Matrix a, Matrix b)
    {
        if (a.data.GetLength(0) != b.data.GetLength(0) ||
            a.data.GetLength(1) != b.data.GetLength(1))
            throw new ArgumentException("Несовместимые размеры матриц");

        Matrix result = new Matrix(a.data.GetLength(0), a.data.GetLength(1));
        for (int i = 0; i < a.data.GetLength(0); i++)
            for (int j = 0; j < a.data.GetLength(1); j++)
                result.data[i, j] = a.data[i, j] - b.data[i, j];
        return result;
    }

    // Транспонирование
    public Matrix Transpose()
    {
        Matrix result = new Matrix(data.GetLength(1), data.GetLength(0));
        for (int i = 0; i < data.GetLength(0); i++)
            for (int j = 0; j < data.GetLength(1); j++)
                result.data[j, i] = data[i, j];
        return result;
    }

    // Расчет сумм столбцов
    public void CalculateColumnSums()
    {
        int rows = data.GetLength(0);
        int cols = data.GetLength(1);
        int maxSum = int.MinValue;
        int maxSumColumn = -1;

        for (int j = 0; j < cols - 1; j++)
        {
            int skipElements = j + 1;
            int elementsToSum = rows - skipElements;

            if (elementsToSum <= 0)
                continue;

            int sum = 0;
            for (int i = 0; i < elementsToSum; i++)
                sum += data[i, j];

            Console.WriteLine($"Сумма {j}-го столбца (пропущено {skipElements} элем.): {sum}");

            if (sum > maxSum)
            {
                maxSum = sum;
                maxSumColumn = j;
            }
        }

        if (maxSumColumn != -1)
            Console.WriteLine($"\nМаксимальная сумма {maxSum} находится в столбце {maxSumColumn}");
        else
            Console.WriteLine("\nНет столбцов для обработки");
    }

    // Перегрузка ToString
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < data.GetLength(0); i++)
        {
            for (int j = 0; j < data.GetLength(1); j++)
            {
                sb.Append($"{data[i, j],6} ");
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }

    // Основной метод
    static void Main()
    {
        while (true)
        {
            try
            {
                Console.WriteLine("\nВведите количество строк (n):");
                int n = int.Parse(Console.ReadLine());
                Console.WriteLine("Введите количество столбцов (m):");
                int m = int.Parse(Console.ReadLine());

                // Создание матриц
                Matrix A = new Matrix(n, m, true);
                Matrix B = new Matrix(n);
                Matrix C = new Matrix();

                // Вывод матриц и расчет сумм
                Console.WriteLine("\nПервая матрица (A):");
                Console.WriteLine(A);
                Console.WriteLine("Расчет сумм для первой матрицы:");
                A.CalculateColumnSums();

                Console.WriteLine("\nВторая матрица (B):");
                Console.WriteLine(B);
                Console.WriteLine("Расчет сумм для второй матрицы:");
                B.CalculateColumnSums();

                Console.WriteLine("\nТретья матрица (C):");
                Console.WriteLine(C);
                Console.WriteLine("Расчет сумм для третьей матрицы:");
                C.CalculateColumnSums();

                // Выполнение операции A + 4*B - CT
                Matrix result = A + 4 * B - C.Transpose();
                Console.WriteLine("\nРезультат операции (A + 4*B - CT):");
                Console.WriteLine(result);

                Console.WriteLine("\nХотите продолжить? (да/нет)");
                string answer = Console.ReadLine().ToLower();
                if (answer != "да")
                {
                    break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\nПроизошла ошибка: {e.Message}");
                Console.WriteLine("Попробуйте снова.");
            }
        }
    }
}
