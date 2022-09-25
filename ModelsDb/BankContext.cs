using Microsoft.EntityFrameworkCore;

namespace ModelsDb
{
    public class BankContext : DbContext
    {

        public DbSet<ClientDb> Clients { get; set; }

        public DbSet<EmployeeDb> Employees { get; set; }

        public DbSet<AccountDb> Accounts { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                                    "Host=localhost;" +
                                    "Port = 5432;" +
                                    "Database = banking_database;" +
                                    "Username = postgres;" +
                                    "Password = tany0206"
                                    );

        }
    }
}
