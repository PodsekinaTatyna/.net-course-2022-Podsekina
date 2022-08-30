using Models;
using Services;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        TestDataGenerator testDataGenerator = new TestDataGenerator();
        var stopwatch = new Stopwatch();

        var clientList = testDataGenerator.GetClientsList();
        var clientDictionary = testDataGenerator.GetClientsDictionary();
        var employeesList = testDataGenerator.GetEmployeesList();

        Console.WriteLine("Замер времени поиска клиента по его номеру телефона среди элементов коллекции:");
        for (int i = 0; i < 3; i++)
        {
            stopwatch.Start();

            var lastСlientOnList = clientList.FirstOrDefault(p => p.PhoneNumber == clientList.Last().PhoneNumber);

            stopwatch.Stop();
            Console.WriteLine($"Тест {i + 1} : {stopwatch.ElapsedTicks}");
            stopwatch.Restart();
        }

        Console.WriteLine("Замер времени выполнения поиска клиента по его номеру телефона, среди элементов словаря:");
        for (int i = 0; i < 3; i++)
        {
            stopwatch.Start();

            var lastСlientOnDictionary = clientDictionary[clientDictionary.Keys.Last()];

            stopwatch.Stop();
            Console.WriteLine($"Тест {i + 1} : {stopwatch.ElapsedTicks}");
            stopwatch.Restart();
        }

        Console.WriteLine("Клиенты младше 18:");
        var clientsUnder18 = clientList.Where(p => DateTime.Now.Year-p.DateOfBirth.Year<18).ToList();
        foreach(Client client in clientsUnder18)
        {
            Console.WriteLine($"Полное имя : {client.FirstName} {client.LastName}\n" +
                              $"ID паспорта : {client.PassportID}\n" +
                              $"Дата рождения : {client.DateOfBirth.ToString("D")}");
        }

        var employeeWithMinSalary = employeesList.FirstOrDefault(p=>p.Salary == employeesList.Min(s=>s.Salary));
        Console.WriteLine($"\nСотрудник с минимально заработной платой:\n" +
                          $"Полное имя : {employeeWithMinSalary.FirstName} {employeeWithMinSalary.LastName}\n" +
                          $"ID паспорта : {employeeWithMinSalary.PassportID}\n" +
                          $"Зарплата : {employeeWithMinSalary.Salary}");

        Console.WriteLine("\nCкорость поиска по словарю двумя методами:");
        for (int i = 0; i < 3; i++)
        {
            stopwatch.Start();

            var lastСlientByExtensionMethod = clientDictionary.FirstOrDefault(p => p.Key == clientDictionary.Keys.Last());
            
            stopwatch.Stop();
            Console.Write($"FirstOrDefault: {i + 1} : {stopwatch.ElapsedTicks}\t\t");
            stopwatch.Restart();

            stopwatch.Start();

            var lastСlientByKey = clientDictionary[clientDictionary.Keys.Last()];

            stopwatch.Stop();
            Console.Write($"By key: {i + 1} : {stopwatch.ElapsedTicks}\n");
            stopwatch.Restart();
        }


        Console.ReadKey();

    }
}
