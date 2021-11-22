using Microsoft.EntityFrameworkCore;

using SquidGame.Domain;
using SquidGame.Interfaces;
using SquidGame.Models;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SquidGame.Services
{
    public class GameService : IGameService
    {
        private readonly SquidGameContext _dbContext;

        public GameService(SquidGameContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ICollection<GameListItem>> GetGameList()
        {
            return await _dbContext.Games
                .Where(g => g.IsActive)
                .OrderBy(g => g.StartDateTime)
                .Select(g => new GameListItem()
                {
                    Id = g.Id
                }).ToListAsync();
        }
    }
}
