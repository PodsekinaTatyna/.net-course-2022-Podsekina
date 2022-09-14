using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Storages
{
    public class EmployeeStorage : IStorage<Employee>
    {
        public readonly List<Employee> _employeeList = new List<Employee>();

        public void Add(Employee employee)
        {
            _employeeList.Add(employee);
        }

        public void Delete(Employee employee)
        {
            _employeeList.Remove(employee);
        }

        public void Update(Employee employee)
        {
            int employeeIndex = _employeeList.IndexOf(_employeeList.First(p => p.PassportID == employee.PassportID));

            _employeeList[employeeIndex] = new Employee()
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                PassportID = employee.PassportID,
                DateOfBirth = employee.DateOfBirth,
                Salary = employee.Salary,
                Contract = employee.Contract
            };
        }
    }
}
