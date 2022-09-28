using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsDb
{
    [Table("currencies")]
    public class CurrencyDb
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("code")]
        public int Code { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("account_id")]

        public ICollection<AccountDb> Accounts { get; set; }

    }
}
