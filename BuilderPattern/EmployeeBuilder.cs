using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuilderPattern
{
    public class EmployeeBuilder
    {
        private Employee employee = new Employee();

        public EmployeeBuilder SetName(string name)
        {
            employee.Name = name; return this;
        }

        public EmployeeBuilder SetDepartment(string department)
        {
            employee.Department = department; return this;
        }

        public EmployeeBuilder SetSalary(decimal salary)
        {
            employee.Salary = salary; return this;
        } 
        public Employee Build() { return employee; }
    }
}
