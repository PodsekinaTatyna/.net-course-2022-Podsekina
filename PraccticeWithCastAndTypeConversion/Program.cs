using Services;
using Models;
using System.Data;

class Program
{
    static void Main()
    {
        BankService bankService = new BankService();

        List<Employee> ouners = new List<Employee>
        {
            new Employee
            {
                FirstName = "Bill",
                LastName = "Smit",
                PassportID = 123456,
                DateOfBirth = new DateTime(2000, 12, 13),
            },
            new Employee
            {
                FirstName = "Bill",
                LastName = "Smit",
                PassportID = 123456,
                DateOfBirth = new DateTime(2000, 12, 13),
            }
        };

        foreach (Employee ouner in ouners)
        {
            ouner.Salary = bankService.BankOwnersSalaries(123456, 10000, ouners.Count);
        }
        Console.WriteLine("\nSalary of each owner : {0}", ouners[0].Salary);

        Client client = new Client
        {
            FirstName = "Tom",
            LastName = "Fert",
            PassportID = 654321,
            DateOfBirth = new DateTime(1998, 07, 27),
        };
        Employee clientToEmployee = bankService.ClientConverToEmployee(client);
        clientToEmployee.Contract = RenewalContract(clientToEmployee.PassportID, clientToEmployee.FirstName, clientToEmployee.LastName, clientToEmployee.DateOfBirth);
        Console.WriteLine("\n{0}", clientToEmployee.Contract);

        Console.ReadKey();
    }
}
