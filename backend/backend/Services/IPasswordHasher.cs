namespace backend.Services
{
    public interface IPasswordHasher
    {
        string Hash(string password, string salt, string pepper);
    }
}
