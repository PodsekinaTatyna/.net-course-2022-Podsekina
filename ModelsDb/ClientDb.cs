using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsDb
{
    [Table("clients")]
    public class ClientDb
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

        [Column("phone_number")]
        public string PhoneNumber { get; set; }

        public ICollection<AccountDb> Accounts { get; set; }
    }
    
}
