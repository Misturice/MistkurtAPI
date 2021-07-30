using Microsoft.AspNetCore.Authorization;
using MistkurtAPI;
using MistkurtAPI.Models;
using MistkurtAPI.Classes.Databases;
internal class ProperTokenForUser : AuthorizeAttribute
{
    const string POLICY_PREFIX = "ProperTokenForUser";

    public ProperTokenForUser(MistKurtContext context, string email, string token)
    {
        User user = context.Users.Find(email);
        user.Token.Equals(token);
    }
}