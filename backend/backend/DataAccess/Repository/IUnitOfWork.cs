using backend.Models;

namespace backend.DataAccess.Repository
{
    public interface IUnitOfWork
    {
        IRepository<User> UserRepo { get; }
        void Save();
    }
}
