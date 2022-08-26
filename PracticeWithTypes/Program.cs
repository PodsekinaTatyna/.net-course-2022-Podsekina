using Services;
using Models;
using System.Data;

class Program
{
    static void Main()
    {
        //Ссылочные типы и типы значений
        Employee employee = new Employee
        {
            FirstName = "Bill",
            LastName = "Smit",
            PassportID = 123456,
            DateOfBirth = new DateTime(2000, 12, 13),
        };

        RenewalContract(employee);
        Console.WriteLine(employee.Contract);

        employee.Contract = RenewalContract(employee.PassportID, employee.FirstName, employee.LastName, employee.DateOfBirth);
        Console.WriteLine(employee.Contract);


        Curreny currency = new Curreny
        {
            Code = 200,
            Name = "RUP",
        };

        IncorrectСurrencyСhange(currency);
        Console.WriteLine($"\nCurrency: {currency.Code} {currency.Name}");

        currency = СorrectСurrencyСhange(currency);
        Console.WriteLine($"\nCurrency : {currency.Code} {currency.Name}");

        // Приведение и преобразование типов
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
        BankService bankService = new BankService();
        foreach(Employee ouner in ouners)
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


    public static void RenewalContract(Employee employee)
    {
        employee.Contract = "ID: " + employee.PassportID + "\nFull Name: " + employee.FirstName + " " + employee.LastName + "\nDate of Birth: " + employee.DateOfBirth.ToString("D");
    }

    public static string RenewalContract(int passportID, string firstName, string lastName, DateTime date)
    {
        string Contract = "ID: " + passportID + "\nFull Name: " + firstName + " " + lastName + "\nDate of Birth: " + date.ToString("D");
        return Contract;
    }

    public static void IncorrectСurrencyСhange(Curreny curreny)
    {
        curreny.Code = 504;
        curreny.Name = "USD";
    }

    public static Curreny СorrectСurrencyСhange(Curreny curreny)
    {
        curreny.Code = 504;
        curreny.Name = "USD";
        return curreny;
    }
}
