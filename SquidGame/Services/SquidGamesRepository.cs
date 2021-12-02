using Microsoft.EntityFrameworkCore;

using SquidGame.Domain;
using SquidGame.Interfaces;

using System.Linq;
using System.Threading.Tasks;

namespace SquidGame.Services
{
    public class SquidGamesRepository : ISquidGamesRepository
    {
        private readonly SquidGameContext _dbContext;

        public SquidGamesRepository(SquidGameContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<T> QueryAll<T>() where T : class
        {
            return _dbContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> GetAll<T>() where T : class
        {
            return _dbContext.Set<T>();
        }

        public void Add<T>(T entity) where T : class
        {
            _dbContext.Add(entity);
        }

        public Task<int> SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }
    }
}
