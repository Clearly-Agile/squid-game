using Microsoft.EntityFrameworkCore;

using SquidGame.Domain;
using SquidGame.Exceptions;
using SquidGame.Interfaces;
using SquidGame.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SquidGame.Services
{
    public class GameService : IGameService
    {
        private readonly SquidGameContext _dbContext;
        private readonly ISquidGamesRepository _repo;

        public GameService(
            SquidGameContext dbContext,
            ISquidGamesRepository repo)
        {
            _dbContext = dbContext;
            _repo = repo;
        }

        public async Task<ICollection<GameListItem>> GetGameList()
        {
            return await _repo.QueryAll<Game>()
                .Where(g => g.IsActive)
                .OrderBy(g => g.StartDateTime)
                .Select(g => new GameListItem()
                {
                    Id = g.Id,
                    NumberOfPlayers = g.Players.Count
                }).ToListAsync();
        }

        public async Task<int> CreateGame(CreateGameDto newGame)
        {
            ValidateCommon(newGame);

            var createdGame = new Game()
            {
                Name = newGame.Name,
                Description = newGame.Description
            };

            _repo.Add(createdGame);

            await _repo.SaveChangesAsync();

            return createdGame.Id;
        }

        protected virtual void ValidateCommon(CreateGameDto newGame)
        {
            if (String.IsNullOrWhiteSpace(newGame.Name))
            {
                throw new ValidationException("Name is required");
            }
        }
    }
}
