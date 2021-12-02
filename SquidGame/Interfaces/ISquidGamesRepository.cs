using System.Linq;
using System.Threading.Tasks;

namespace SquidGame.Interfaces
{
    public interface ISquidGamesRepository
    {
        IQueryable<T> QueryAll<T>() where T : class;

        IQueryable<T> GetAll<T>() where T : class;

        void Add<T>(T entity) where T : class;

        Task<int> SaveChangesAsync();
    }
}
