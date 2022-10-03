using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsDb
{
    [Table("accounts")]
    public class AccountDb
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("currency")]
        public string CurrencyName { get; set; }
        
        [Column("amount")]
        public decimal Amount { get; set; }
      
        [Column("client_id")]
        public Guid ClientId { get; set; }

        public ClientDb Client { get; set; }

        public CurrencyDb Currency { get; set; }
    }
}
