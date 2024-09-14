using System.Text;
using System.Security.Cryptography;

namespace Bruteforce
{
    public class Hasher
    {
        private const string StaticSalt = "TEST_STATIC_SALT";
        public string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                string saltedPassword = password + StaticSalt;
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
                return Convert.ToHexString(hashBytes);
            }
        }
    }
}
