using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public string Token { get; set; }
    }
}
