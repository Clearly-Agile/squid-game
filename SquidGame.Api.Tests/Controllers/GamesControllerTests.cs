using Microsoft.AspNetCore.Mvc;

using NSubstitute;

using Shouldly;

using SquidGame.Api.Controllers;
using SquidGame.Interfaces;
using SquidGame.Models;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Xunit;

namespace SquidGame.Api.Tests.Controllers
{
    public class GamesControllerTests
    {
        private GamesController _sut;
        private IGameService _gameServiceMock;

        public GamesControllerTests()
        {
            _gameServiceMock = Substitute.For<IGameService>();
            _sut = new GamesController(_gameServiceMock);
        }

        [Fact]
        public async Task GetGameList_ShouldCallGameService()
        {
            await _sut.GetGamesList();

            await _gameServiceMock.Received(1).GetGameList();
        }

        [Fact]
        public async Task GetGameList_ShouldReturnGamesFromGameService()
        {
            var existingGames = new Collection<GameListItem>()
            {
                new GameListItem()
                {
                    Id = 333
                },
                new GameListItem()
                {
                    Id = 444
                }
            };

            _gameServiceMock.GetGameList().Returns(existingGames);

            var actionResult = await _sut.GetGamesList();

            var result = actionResult.Result as OkObjectResult;
            //Assert.NotNull(result);
            result.ShouldNotBeNull();

            var resultGames = result.Value as ICollection<GameListItem>;
            //Assert.NotNull(resultGames);
            resultGames.ShouldNotBeNull("No result games!");

            //Assert.Equal(existingGames.Count, resultGames.Count);
            resultGames.Count.ShouldBe(existingGames.Count);

            //Assert.True(resultGames.All(g => existingGames.Select(game => game.Id).Contains(g.Id)));
            resultGames.All(g => existingGames.Select(game => game.Id).Contains(g.Id)).ShouldBeTrue();
        }
    }
}
