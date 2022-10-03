using Models;
using ModelsDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Storages
{
    public class EmployeeStorage : IStorage<Employee>
    {
        private List<Employee> data = new List<Employee>();

        public List<Employee> Data => data;

        public void Add(Employee employee)
        {
            Data.Add(employee);
        }

        public void Delete(Employee employee)
        {
            Data.Remove(employee);
        }

        public void Update(Employee employee)
        {
            var oldemployee = Data.First(p => p.PassportID == employee.PassportID);

            oldemployee.FirstName = employee.FirstName;
            oldemployee.LastName = employee.LastName;
            oldemployee.PassportID = employee.PassportID;
            oldemployee.DateOfBirth = employee.DateOfBirth;

        }
    }
}
