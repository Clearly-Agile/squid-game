using ClearlyAgile.Testing.Core;

using Microsoft.EntityFrameworkCore;

using NSubstitute;

using Shouldly;

using SquidGame.Domain;
using SquidGame.Exceptions;
using SquidGame.Interfaces;
using SquidGame.Models;
using SquidGame.Services;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Xunit;

namespace SquidGame.Tests.Services
{
    public class GameServiceTests
    {
        private GameServiceStub _sut;
        private ISquidGamesRepository _repoMock;

        private DbContextOptions<SquidGameContext> _dbOptions = new DbContextOptionsBuilder<SquidGameContext>()
            .UseInMemoryDatabase(databaseName: "SquidGames")
            .Options;

        public GameServiceTests()
        {
            _repoMock = Substitute.For<ISquidGamesRepository>();
            _sut = new GameServiceStub(new SquidGameContext(_dbOptions), _repoMock);
            using var context = new SquidGameContext(_dbOptions);
            context.RemoveRange(context.Games.ToList());
            context.SaveChanges();
        }

        [Fact(Skip = "DB Context Test")]
        public async Task GetGameList_ShouldReturnAllGamesSortedByStartDate_DbContext()
        {
            var expectedFirstGame = new Game()
            {
                Id = 3333,
                IsActive = true,
                StartDateTime = new DateTimeOffset(new DateTime(2021, 1, 11))
            };

            var expectedSecondGame = new Game()
            {
                Id = 222,
                IsActive = true,
                StartDateTime = new DateTimeOffset(new DateTime(2021, 2, 11))
            };

            var expectedThirdGame = new Game()
            {
                Id = 5555,
                IsActive = true,
                StartDateTime = new DateTimeOffset(new DateTime(2021, 3, 11))
            };

            var expectedGames = new Collection<Game>()
            {
                expectedSecondGame,
                expectedThirdGame,
                expectedFirstGame
            };

            await using var context = new SquidGameContext(_dbOptions);
            context.Games.AddRange(expectedGames);
            await context.SaveChangesAsync();

            var games = await _sut.GetGameList();

            games.Count.ShouldBe(expectedGames.Count);

            games.ElementAt(0).Id.ShouldBe(expectedFirstGame.Id);
            games.ElementAt(1).Id.ShouldBe(expectedSecondGame.Id);
            games.ElementAt(2).Id.ShouldBe(expectedThirdGame.Id);

        }

        [Fact]
        public async Task GetGameList_ShouldReturnAllGamesSortedByStartDate()
        {
            var expectedFirstGame = new Game()
            {
                Id = 3333,
                IsActive = true,
                StartDateTime = new DateTimeOffset(new DateTime(2021, 1, 11))
            };

            var expectedSecondGame = new Game()
            {
                Id = 222,
                IsActive = true,
                StartDateTime = new DateTimeOffset(new DateTime(2021, 2, 11))
            };

            var expectedThirdGame = new Game()
            {
                Id = 5555,
                IsActive = true,
                StartDateTime = new DateTimeOffset(new DateTime(2021, 3, 11))
            };

            var expectedGames = new Collection<Game>()
            {
                expectedSecondGame,
                expectedThirdGame,
                expectedFirstGame
            };

            _repoMock.QueryAll<Game>().Returns(new TestAsyncEnumerable<Game>(new Collection<Game>()
            {
                expectedFirstGame,
                expectedThirdGame,
                expectedSecondGame
            }));

            var games = await _sut.GetGameList();

            games.Count.ShouldBe(expectedGames.Count);

            games.ElementAt(0).Id.ShouldBe(expectedFirstGame.Id);
            games.ElementAt(1).Id.ShouldBe(expectedSecondGame.Id);
            games.ElementAt(2).Id.ShouldBe(expectedThirdGame.Id);

        }

        [Fact(Skip = "DB Context Test")]
        public async Task GetGameList_ShouldOnlyReturnActiveGames_DbContext()
        {
            var expectedGame1 = new Game()
            {
                Id = 333,
                IsActive = true
            };

            var expectedGame2 = new Game()
            {
                Id = 444,
                IsActive = true
            };

            var notActiveGame = new Game()
            {
                Id = 555,
                IsActive = false
            };

            var expectedGameCount = 2;

            await using var context = new SquidGameContext(_dbOptions);
            context.Games.Add(expectedGame1);
            context.Games.Add(expectedGame2);
            context.Games.Add(notActiveGame);
            await context.SaveChangesAsync();

            var games = await _sut.GetGameList();

            games.Count.ShouldBe(expectedGameCount);

        }

        [Fact]
        public async Task GetGameList_ShouldOnlyReturnActiveGames()
        {
            var expectedGame1 = new Game()
            {
                Id = 333,
                IsActive = true
            };

            var expectedGame2 = new Game()
            {
                Id = 444,
                IsActive = true
            };

            var notActiveGame = new Game()
            {
                Id = 555,
                IsActive = false
            };

            var expectedGameCount = 2;

            _repoMock.QueryAll<Game>().Returns(new TestAsyncEnumerable<Game>(new Collection<Game>()
            {
                expectedGame1,
                expectedGame2,
                notActiveGame
            }));

            var games = await _sut.GetGameList();

            games.Count.ShouldBe(expectedGameCount);
            games.Any(g => g.Id == notActiveGame.Id).ShouldBeFalse();
        }

        [Fact]
        public async Task GetGameList_ShouldReturnCountOfPlayersInEachGame()
        {
            var expectedGame1 = new Game()
            {
                Id = 11,
                IsActive = true,
                Players = new Collection<Player>()
                {
                    new Player(),
                    new Player()
                }
            };

            var expectedGame2 = new Game()
            {
                Id = 222,
                IsActive = true,
                Players = new Collection<Player>()
                {
                    new Player()
                }
            };

            var expectedGame3 = new Game()
            {
                Id = 333,
                IsActive = true
            };

            _repoMock.QueryAll<Game>().Returns(new TestAsyncEnumerable<Game>(
                new Collection<Game>()
            {
                expectedGame1,
                expectedGame2,
                expectedGame3,
            }));

            var games = await _sut.GetGameList();

            games.Where(g => g.Id == expectedGame1.Id).First().NumberOfPlayers.ShouldBe(expectedGame1.Players.Count);
            games.Where(g => g.Id == expectedGame2.Id).First().NumberOfPlayers.ShouldBe(expectedGame2.Players.Count);
            games.Where(g => g.Id == expectedGame3.Id).First().NumberOfPlayers.ShouldBe(expectedGame3.Players.Count);
        }

        [Fact]
        public async Task CreateGame_ShouldCallValidateCommonWithCreateGameDto()
        {
            var newGame = new CreateGameDto()
            {
                Name = "Glass Bridge",
                Description = "Number 16 really has an advantage on this one"
            };

            await _sut.CreateGame(newGame);

            _sut.WasValidateCommonCalled.ShouldBeTrue();
            _sut.ValidateCommonCalledWith.ShouldBe(newGame);
        }

        [Fact]
        public async Task CreateGame_ShouldAddGameToTheContext()
        {
            var newGame = new CreateGameDto()
            {
                Name = "Glass Bridge",
                Description = "Number 16 really has an advantage on this one"
            };

            await _sut.CreateGame(newGame);

            _repoMock.Received(1).Add(Arg.Is<Game>(g => 
                g.Name == newGame.Name
                && g.Description == newGame.Description));

            await _repoMock.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task CreateGame_ShouldReturnTheIdOfTheNewGame()
        {
            var expectedNewId = 555;

            var newGame = new CreateGameDto()
            {
            };

            Game createdGame = null;

            _repoMock.When(r => r.Add(Arg.Any<Game>())).Do(args =>
            {
                createdGame = ((Game)args[0]);
            });

            _repoMock.When(r => r.SaveChangesAsync()).Do(args =>
            {
                createdGame.Id = expectedNewId;
            });

            var newId = await _sut.CreateGame(newGame);

            newId.ShouldBe(expectedNewId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("      ")]
        public void ValidateCommon_WhenNameIsEmpty_ShouldThrowValidationException(string value)
        {
            var newGame = new CreateGameDto()
            {
                Name = value
            };

            Should.Throw<ValidationException>(() => _sut.ValidateCommonStub(newGame));
        }

        private class GameServiceStub : GameService
        {
            public GameServiceStub(SquidGameContext context, ISquidGamesRepository repo) : base(context, repo)
            {

            }

            public bool WasValidateCommonCalled { get; set; }
            public CreateGameDto ValidateCommonCalledWith { get; set; }

            protected override void ValidateCommon(CreateGameDto game)
            {
                this.WasValidateCommonCalled = true;
                this.ValidateCommonCalledWith = game;
            }

            public void ValidateCommonStub(CreateGameDto game)
            {
                base.ValidateCommon(game);
            }
        }
    }
}
