using Microsoft.AspNetCore.Mvc;

using SquidGame.Interfaces;
using SquidGame.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace SquidGame.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GamesController : ControllerBase
    {
        private IGameService _gameService;

        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<GameListItem>>> GetGamesList()
        {
            var games = await _gameService.GetGameList();

            return Ok(games);
        }
    }
}
