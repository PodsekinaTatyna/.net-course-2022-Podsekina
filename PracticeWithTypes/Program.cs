using Models;
using System.Data;

class Program
{
    static void Main()
    {
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

        Put50Rubles(currency);
        Console.WriteLine($"\nDeposited : {currency.Code} {currency.Name}");

        currency.Code = Put50Rubles(currency.Code);
        Console.WriteLine($"\nDeposited : {currency.Code} {currency.Name}");

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

    public static void Put50Rubles(Curreny curreny)
    {
        curreny.Code += 50;
    }

    public static decimal Put50Rubles(decimal code)
    {
        return code + 50;
    }
}
