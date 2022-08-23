using Models;
using System.Data;

class Program
{
    static void Main()
    {
        Employee employee = new Employee("Bill", "Smit", 123456, new DateTime(2000, 12, 13));

        //RenewalContract(employee); 
        employee.Contract = RenewalContract(employee.ID, employee.FirstName, employee.LastName, employee.DateOfBirth);
        Console.WriteLine(employee.Contract);


        Curreny currency = new Curreny(123456, 200, "RUP");

        Put_50_Rubles(currency);// ничего не изменилось
        currency.Amount = Put_50_Rubles(currency.Amount);//изменилось значение
        Console.WriteLine($"\nOn account {currency.ID} - {currency.Amount} {currency.Name}");

        Console.ReadKey();
    }

    // Непривильный подход
    public static void RenewalContract(Employee employee)
    {
        employee.Contract = "ID: " + employee.ID + "\nFull Name: " + employee.FirstName + " " + employee.LastName + "\nDate of Birth: " + employee.DateOfBirth.ToString("D");
    }

    //Правильный подход
    public static string RenewalContract(int id, string firstName, string lastName, DateTime date)
    {
        string Contract = "ID: " + id + "\nFull Name: " + firstName + " " + lastName + "\nDate of Birth: " + date.ToString("D");
        return Contract;
    }

    //передача структуры в качестве параметра
    public static void Put_50_Rubles(Curreny curreny)
    {
        curreny.Amount += 50;
    }

    //передача поля структры
    public static decimal Put_50_Rubles(decimal amount)
    {
        return amount + 50;
    }
}
