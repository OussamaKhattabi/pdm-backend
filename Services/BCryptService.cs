using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Policy;

namespace PremiumDeluxeMotorSports_v1.Services
{
    public class BCryptVerifyService
    {
        public bool VerifyPassword(string HashedPassword, string InputPassword)
        {
            return BCrypt.Net.BCrypt.Verify(InputPassword, HashedPassword);
        }
    }
}
