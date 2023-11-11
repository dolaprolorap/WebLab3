using backend.Models.DB;

namespace backend.DataAccess.Repository
{
    public interface IUnitOfWork
    {
        IRepository<User> UserRepo { get; }
        IRepository<Plot> PlotRepo { get; }
        void Save();
    }
}
