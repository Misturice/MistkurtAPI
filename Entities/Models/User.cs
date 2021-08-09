using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{

    public enum Role
    {
        Admin,
        User
    }

    [Table("Users")]
    public class User
    {
        [Key]
        [Column("UserID"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Column("Email"), Required(ErrorMessage = "Email is required"), Index(IsUnique = true)]
        public string Email { get; set; }
        [Column("Role"), Required(ErrorMessage = "Role is required")]
        public Role Role { get; set; }
        [Column("Token"), Index("TokenIndex")]
        public string Token { get; set; }

        public ICollection<Expenses> Expenses { get; set; }

    }
}
