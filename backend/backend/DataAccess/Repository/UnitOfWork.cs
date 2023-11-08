using backend.Models;

namespace backend.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;
        public IRepository<User> UserRepo { get; private set; }
        public UnitOfWork(AppDbContext db)
        {
            _db = db;
            UserRepo = new Repository<User>(db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
