using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("Expenses")]
    public class Expenses
    {
        [Column("ExpensesID"), Key]
        public Guid Id { get; set; }

        [Column("Date"), Index("DateIndex"), Required]
        public long Date { get; set; }

        [Column("Total"), Required]
        public float Total { get; set; }

        [Column("UserID"), ForeignKey(nameof(UserId)), Required, Index("UserIDIndex")]
        public Guid UserId { get; set; }
        
        public User User { get; set; }

        public ICollection<Product> Products { get; set; }

    }
}
