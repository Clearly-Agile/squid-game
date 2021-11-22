using Microsoft.EntityFrameworkCore;

using Shouldly;

using SquidGame.Domain;
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
        private GameService _sut;

        private DbContextOptions<SquidGameContext> _dbOptions = new DbContextOptionsBuilder<SquidGameContext>()
            .UseInMemoryDatabase(databaseName: "SquidGames")
            .Options;

        public GameServiceTests()
        {
            _sut = new GameService(new SquidGameContext(_dbOptions));
            using var context = new SquidGameContext(_dbOptions);
            context.RemoveRange(context.Games.ToList());
            context.SaveChanges();
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

            await using var context = new SquidGameContext(_dbOptions);
            context.Games.Add(expectedGame1);
            context.Games.Add(expectedGame2);
            context.Games.Add(notActiveGame);
            await context.SaveChangesAsync();

            var games = await _sut.GetGameList();

            games.Count.ShouldBe(expectedGameCount);

        }
    }
}
