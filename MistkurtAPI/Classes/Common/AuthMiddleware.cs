using Contracts;
using Entities;
using Microsoft.AspNetCore.Authorization;
internal class ProperTokenForUser : AuthorizeAttribute
{
    const string POLICY_PREFIX = "ProperTokenForUser";

    public ProperTokenForUser(IRepositoryWrapper repository, string email, string token)
    {
        User user = repository.User.GetUserByEmail(email);
        user.Token.Equals(token);
    }
}