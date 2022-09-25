using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsDb
{
    [Table("account")]
    public class AccountDb
    {
        [Column("id")]
        public Guid Id { get; set; }
        [Column("currency")]
        public string Currency { get; set; }
        
        [Column("amount")]
        public decimal Amount { get; set; }
      
        [Column("client_id")]
        public Guid clientId { get; set; }

        public ClientDb Client { get; set; }

    }
}
