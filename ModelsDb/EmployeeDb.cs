using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsDb
{
    [Table("employees")]
    public class EmployeeDb
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("last_name")]
        public string LastName { get; set; }

        [Column("passport_id")]
        public int PassportID { get; set; }

        [Column("date_of_birth", TypeName = "date")]
        public DateTime DateOfBirth { get; set; }

        [Column("bonus")]
        public int Bonus { get; set; }

        [Column("contract")]
        public string Contract { get; set; }

        [Column("salary")]
        public int Salary { get; set; }
    }
}
