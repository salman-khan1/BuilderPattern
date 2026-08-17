using BuilderPattern;

class Program
{
    static void Main()
    {
        Employee employee = new EmployeeBuilder()
            .SetName("Salman")
            .SetDepartment("IT")
            .SetSalary(100000)
            .Build();
        employee.Display();
    }
}