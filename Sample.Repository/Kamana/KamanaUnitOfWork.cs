using Sample.Data.KamanaDB.Entities;
using Sample.Data.KamanaDB;
using Sample.Repository.Kamana;
using Sample.Repository;

public class KamanaUnitOfWork : IKamanaUnitOfWork
{
    private readonly KamanaDbContext _dbContext;
    private AppMenuRepository _appMenuRepository;

    public KamanaDbContext DbContext => _dbContext;

    public KamanaUnitOfWork(KamanaDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public IRepository<TblAppMenu> AppMenuRepository =>
        _appMenuRepository ??= new AppMenuRepository(_dbContext);

    public bool Commit()
    {
        using var transaction = _dbContext.Database.BeginTransaction();
        try
        {
            _dbContext.SaveChanges();
            transaction.Commit();
            return true;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}
