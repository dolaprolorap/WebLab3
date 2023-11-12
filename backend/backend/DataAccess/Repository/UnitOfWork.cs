using backend.Models.DB;

namespace backend.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;
        public IRepository<User> UserRepo { get; private set; }
        public IRepository<Plot> PlotRepo { get; private set; }
        public IRepository<PlotEntry> EntryRepo { get; private set; }
        public UnitOfWork(AppDbContext db)
        {
            _db = db;
            UserRepo = new Repository<User>(db);
            PlotRepo = new Repository<Plot>(db);
            EntryRepo = new Repository<PlotEntry>(db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
