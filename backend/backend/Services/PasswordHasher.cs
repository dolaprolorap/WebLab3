using System.Security.Cryptography;
using System.Text;

namespace backend.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password, string salt, string pepper)
        {
            const int keySize = 64;
            const int iterations = 350000;
            HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                Encoding.ASCII.GetBytes(salt + pepper),
                iterations,
                hashAlgorithm,
                keySize);
            return Convert.ToHexString(hash);
        }
    }
}
