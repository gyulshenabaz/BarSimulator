using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BarSimulator
{
    class Bar
    {
        List<Student> students;
        List<Drink> drinks;
        Semaphore semaphore;
        private bool isClosed;

        public Bar(List<Drink> drinks)
        {
            this.students = new List<Student>();
            this.drinks = drinks;
            this.semaphore = new Semaphore(10, 10);
            this.isClosed = false;
        }
        
        public void Enter(Student student)
        {
            semaphore.WaitOne();
            lock (students)
            {
                if (this.isClosed)
                {
                    throw new InvalidOperationException($"Bar is closed.");
                }
                if (student.Age < 18)
                {
                    throw new InvalidOperationException($"Student {student.Name} is not old enough to enter the bar.");
                }
                students.Add(student);
            }
        }

        public void Leave(Student student)
        {
            lock (students)
            {
                students.Remove(student);
            }
            semaphore.Release();
        }

        public void Close()
        {
            Console.WriteLine("Bar is closing.");
            
            lock (students) 
            { 
                this.isClosed = true;
                foreach (var student in this.students)
                {
                    this.Leave(student);
                    Console.WriteLine($"Student {student.Name} is kicked reason:bar is closing");
                }
            }
        }
        
        public void Drink(Student student)
        {
            lock (drinks)
            {
                Drink randomDrink = this.GetRandomDrink();
            
                if(randomDrink.Quantity <= 0)
                {
                    Console.WriteLine($"The bar is out of {randomDrink}.");
                    return;
                }
                if(student.Budget < randomDrink.Price)
                {
                    Console.WriteLine($"{student} doesn't have enough money to get a {randomDrink}.");
                    return;
                }

                Console.WriteLine($"{student} is drinking {randomDrink}.");
            
                randomDrink.Quantity--;
                randomDrink.SoldDrinksQuantity++;
                student.Budget -= randomDrink.Price;

                Console.WriteLine($"{student} drinks a {randomDrink}.");
            }
        }

        public String GetSalesReport()
        {
            StringBuilder sb = new StringBuilder();

            double totalRevenue = 0;
            
            foreach (var drink in drinks)
            {
                totalRevenue += drink.Price * drink.SoldDrinksQuantity;
                sb.AppendLine($"{drink.Name}, Sold Quantity: {drink.SoldDrinksQuantity}, Left Quantity: {drink.Quantity}, Revenue: {drink.SoldDrinksQuantity*drink.Price} $.");
            }

            sb.AppendLine($"Total revenue: {totalRevenue} $");
            
            return sb.ToString();
        }
        
        private Drink GetRandomDrink()
        {
            Random r = new();
            int n = r.Next(0, drinks.Count);
            return drinks[n];
        }
    }
}