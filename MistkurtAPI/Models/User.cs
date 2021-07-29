using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MistkurtAPI.Models
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
        public Guid UserID { get; set; }
        [Column("Email"), Required, Index(IsUnique = true)]
        public string Email { get; set; }
        [Column("Role"), Required]
        public Role Role { get; set; }
        [Column("Token"), Index("TokenIndex")]
        public string Token { get; set; }

    }
}
