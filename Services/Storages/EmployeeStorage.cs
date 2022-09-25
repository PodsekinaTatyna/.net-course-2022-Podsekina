using Models;
using ModelsDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Storages
{
    public class EmployeeStorage : IStorage<EmployeeDb>
    {
        private BankContext data = new BankContext();

        public BankContext Data => data;

        public void Add(EmployeeDb employee)
        {
            Data.Employees.Add(employee);
            Data.SaveChanges();
        }

        public void Delete(EmployeeDb employee)
        {
            Data.Employees.Remove(employee);
            Data.SaveChanges();
        }

        public void Update(EmployeeDb employee)
        {
            Data.Employees.Update(employee);
            Data.SaveChanges();
        }
    }
}
