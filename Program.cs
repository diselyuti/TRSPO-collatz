using System;
using System.Threading;
using System.Threading.Tasks;

namespace Collatz
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const int N = 1000;
            const int numThreads = 12;

            long totalSteps = 0;
            object lockObject = new object();

            // Створюємо завдання для обчислення кількості кроків для кожного числа
            Parallel.For(1, N + 1, new ParallelOptions { MaxDegreeOfParallelism = numThreads },
                () => 0, // Ініціалізуємо локальну змінну для кожного потоку
                (n, loopState, localSteps) =>
                {
                    int steps = CollatzConjectureSteps(n);
                    return localSteps + steps; // Додаємо кроки для поточного числа до локального підрахунку
                },
                (localSteps) =>
                {
                    // Додаємо локальний підрахунок до загальної кількості кроків захищеною атомарною операцією
                    Interlocked.Add(ref totalSteps, localSteps);
                });

            // Обчислюємо середню кількість кроків
            double averageSteps = (double)totalSteps / N;

            Console.WriteLine($"Average count of steps: {averageSteps}");

            Console.ReadLine();
        }


        static int CollatzConjectureSteps(int n)
        {
            int steps = 0;
            while (n != 1)
            {
                if (n % 2 == 0)
                    n /= 2;
                else
                    n = 3 * n + 1;
                steps++;
            }
            return steps;
        }
    }
}
