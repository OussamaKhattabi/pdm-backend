using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Policy;

namespace pdm.Services
{
    public class BCryptVerifyService
    {
        public bool VerifyPassword(string HashedPassword, string InputPassword)
        {
            return BCrypt.Net.BCrypt.Verify(InputPassword, HashedPassword);
        }
    }
}
