using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MistkurtAPI.Models
{
    [Table("Products")]
    public class Product
    {
        [Column("ProductID"), Key]
        public int ProductID { get; set; }

        [Column("Name"), Required]
        public string Name { get; set; }

        [Column("Tag")]
        public string Tag { get; set; }
        [Column("Type")]
        public string Type { get; set; }
        [Column("Cost")]
        public float Cost { get; set; }

        [Column("ExpensesID"), ForeignKey("ExpensesID"), Index("ExpensesID"), Required]
        public int ExpensesID { get; set; }

        public virtual Expenses Expenses { get; set; }

    }
}
