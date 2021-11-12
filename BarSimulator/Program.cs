using System;
using System.Collections.Generic;
using System.Threading;

namespace BarSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();
            
            List<Drink> drinks = new()
            {
                new Drink("Beer", 8, 100),
                new Drink("Whiskey", 15, 200),
                new Drink("Vodka", 15, 200)

            };
            
            Bar bar = new Bar(drinks);
            
            List<Thread> studentThreads = new List<Thread>();
            for (int i = 1; i < 100; i++)
            {
                var student = new Student(i.ToString(), random.Next(16, 80), random.Next(0, 200), bar);
                var thread = new Thread(student.PaintTheTownRed);
                thread.Start();
                studentThreads.Add(thread);
            }
            
            foreach (var t in studentThreads) t.Join();
            Console.WriteLine();
            Console.WriteLine("The party is over.");
            Console.WriteLine(bar.GetSalesReport());
            Console.ReadLine();
        }
    }
}
