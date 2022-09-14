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
            var oldemployee = _employeeList.First(p => p.PassportID == employee.PassportID);

            oldemployee.FirstName = employee.FirstName;
            oldemployee.LastName = employee.LastName;
            oldemployee.PassportID = employee.PassportID;
            oldemployee.DateOfBirth = employee.DateOfBirth;
            oldemployee.Salary = employee.Salary;
            oldemployee.Contract = employee.Contract;
           
        }
    }
}
