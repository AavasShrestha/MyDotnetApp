using Microsoft.EntityFrameworkCore;
using Sample.Data.KamanaDB;
using Sample.Data.KamanaDB.Entities;
using System.Linq.Expressions;

namespace Sample.Repository.Kamana
{
    public class AppMenuRepository : IRepository<TblAppMenu>
    {
        private readonly KamanaDbContext _context;
        private readonly DbSet<TblAppMenu> _dbSet;

        public AppMenuRepository(KamanaDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = context.Set<TblAppMenu>();
        }

        public IQueryable<TblAppMenu> GetQuerable() => _dbSet.AsQueryable();

        public IQueryable<TblAppMenu> GetQuerable(Expression<Func<TblAppMenu, bool>> criteria)
            => _dbSet.Where(criteria).AsQueryable();

        public async Task<IEnumerable<TblAppMenu>> GetAllAsync()
        {
            return await _dbSet
                .Select(t => new TblAppMenu
                {
                    ID = t.ID,
                    MenuId = t.MenuId != Guid.Empty ? t.MenuId : Guid.NewGuid(), 
                    MenuName = t.MenuName ?? string.Empty,
                    NepaliName = t.NepaliName ?? string.Empty,
                    MenuDescription = t.MenuDescription ?? string.Empty,
                    IsActive = t.IsActive ?? false,
                    MenuGroup = t.MenuGroup ?? string.Empty,
                    MenuGroupID = t.MenuGroupID,
                    BackDateEntryAllowed = t.BackDateEntryAllowed
                })
                .ToListAsync();
        }


        public async Task<TblAppMenu> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        public void Add(TblAppMenu entity) => _dbSet.Add(entity);

        public void Add(IEnumerable<TblAppMenu> entities) => _dbSet.AddRange(entities);

        public void Update(TblAppMenu entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(TblAppMenu entity) => _dbSet.Remove(entity);

        public void Delete(IEnumerable<TblAppMenu> entities) => _dbSet.RemoveRange(entities);

        public async Task ExecuteStoredProcedureAsync(string storedProcedure, params object[] parameters)
            => await _context.Database.ExecuteSqlRawAsync(storedProcedure, parameters);

        public async Task<IEnumerable<TblAppMenu>> ExecuteStoredProcedureQueryAsync(string storedProcedure, params object[] parameters)
            => await _dbSet.FromSqlRaw(storedProcedure, parameters).ToListAsync();
    }
}
