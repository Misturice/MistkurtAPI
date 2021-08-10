using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Entities
{
    [Table("Products")]
    public class Product
    {
        [Column("ProductID"), Key]
        public Guid Id { get; set; }

        [Column("Name"), Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Column("Tag"), Required(ErrorMessage = "Tag is required")]
        public string Tag { get; set; }
        [Column("Type")]
        public string Type { get; set; }
        [Column("Cost"), Required(ErrorMessage = "Cost is required")]
        public float Cost { get; set; }

        [Column("ExpensesID"), ForeignKey(nameof(ExpensesId)), Index("ExpensesID"), Required]
        public Guid ExpensesId { get; set; }

    }
}
