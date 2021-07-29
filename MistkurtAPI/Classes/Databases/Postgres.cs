using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MistkurtAPI.Classes.Databases
{
    public class Postgres
    {
        public static bool UserExistsByEmail(string email, MistKurtContext context)
        {
            return context.Users.Any(e => e.Email == email);
        }
    }
}
