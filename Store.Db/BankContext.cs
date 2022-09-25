using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Store.Db
{
    public class BankContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
            "Host=localhost;" +
            "Port = 5432;" +
            "Database = ;" +
            "Username = postgres;" +
            "Password = tany0206");

        }
    }
}
