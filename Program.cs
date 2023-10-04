using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collatz
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int N = 1000;
            int numThreads = 12;

            ConcurrentBag<int> results = new ConcurrentBag<int>();

            // Створюємо завдання для обчислення кількості кроків для кожного числа
            Parallel.For(1, N + 1, new ParallelOptions { MaxDegreeOfParallelism = numThreads },
                (n) =>
                {
                    int steps = CollatzConjectureSteps(n);
                    results.Add(steps);
                    Console.WriteLine($"Number {n}: {steps} steps");
                });

            // Обчислюємо середню кількість кроків
            double averageSteps = results.Average();

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
