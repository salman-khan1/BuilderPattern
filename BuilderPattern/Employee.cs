using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuilderPattern
{
    public class Employee
    { 
        public string Name { get; set; } 
        public string Department { get; set; } 
        public decimal Salary { get; set; }

        public void Display()
        {
            Console.WriteLine($"Name : {Name}"); 
            Console.WriteLine($"Department : {Department}");
            Console.WriteLine($"Salary : {Salary}");
        }
    }
}
