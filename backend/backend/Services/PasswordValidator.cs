using System.Security.Cryptography;

namespace backend.Services
{
    public class PasswordValidator : IPasswordValidator
    {
        IPasswordHasher hasher;

        public PasswordValidator(IPasswordHasher passwordHasher) 
        {
            this.hasher = passwordHasher;
        }

        public bool Validate(string password, string hash, string salt, string pepper)
        {
            return hasher.Hash(password, salt, pepper) == hash;
        } 
    }
}
