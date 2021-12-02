using SquidGame.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace SquidGame.Interfaces
{
    public interface IGameService
    {
        Task<ICollection<GameListItem>> GetGameList();

        Task<int> CreateGame(CreateGameDto newGame);
    }
}
