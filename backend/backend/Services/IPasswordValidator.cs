namespace backend.Services
{
    public interface IPasswordValidator
    {
        public bool Validate(string password, string hash, string salt, string pepper);
    }
}
