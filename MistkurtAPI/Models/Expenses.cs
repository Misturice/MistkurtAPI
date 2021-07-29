using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MistkurtAPI.Models
{
    [Table("Expenses")]
    public class Expenses
    {
        [Column("ExpensesID"), Key]
        public int ID { get; set; }

        [Column("Date"), Index("DateIndex"), Index(IsUnique = true), Required]
        public int Date { get; set; }

        [Column("Total"), Required]
        public int Total { get; set; }

        [Column("UserID"), ForeignKey("UserID"), Required, Index("UserIDIndex")]
        public Guid UserID { get; set; }
        
        public virtual User User { get; set; }

    }
}
